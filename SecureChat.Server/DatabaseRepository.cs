using Microsoft.Extensions.Configuration;
using NTDLS.SqliteDapperWrapper;
using SecureChat.Library;
using SecureChat.Library.Models;
using System.Text.Json;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Server
{
    internal class DatabaseRepository
    {
        private readonly ManagedDataStorageFactory _dbFactory;

        public DatabaseRepository(IConfiguration configuration)
        {
#if DEBUG
            var sqliteConnection = "..\\..\\..\\..\\data\\server.db";
#else
            var sqliteConnection = configuration.GetValue<string>("SQLiteConnection");
#endif
            _dbFactory = new ManagedDataStorageFactory($"Data Source={sqliteConnection}");
        }

        public void CreateAccount(string username, string displayName, string passwordHash)
        {
            if (GetAccountIdByUserName(username) != null)
            {
                throw new Exception("Username is already in use by another account.");
            }
            if (GetAccountIdByDisplayName(displayName) != null)
            {
                throw new Exception("Display name is already in use by another account.");
            }

            _dbFactory.Execute(@"SqlQueries\CreateAccount.sql",
                new
                {
                    Id = Guid.NewGuid(),
                    @Username = username,
                    DisplayName = displayName,
                    PasswordHash = passwordHash,
                    LastSeen = DateTime.UtcNow
                });
        }

        public void UpdateAccountProfile(Guid accountId, AccountProfileModel profile)
        {
            _dbFactory.Execute(@"SqlQueries\UpdateAccountProfile.sql",
                new
                {
                    Id = accountId,
                    ProfileJson = JsonSerializer.Serialize(profile)
                });
        }

        public void UpdateAccountDisplayName(Guid accountId, string displayName)
        {
            var existingAccountId = GetAccountIdByDisplayName(displayName);

            if (existingAccountId != null && existingAccountId != accountId)
            {
                throw new Exception("Display name is already in use by another account.");
            }

            _dbFactory.Execute(@"SqlQueries\UpdateAccountDisplayName.sql",
                new
                {
                    Id = accountId,
                    DisplayName = displayName
                });
        }

        public void UpdateAccountLastSeen(Guid accountId)
        {
            _dbFactory.Ephemeral(o => UpdateAccountLastSeen(o, accountId));
        }

        public void UpdateAccountLastSeen(ManagedDataStorageInstance instance, Guid accountId)
        {
            instance.Execute(@"SqlQueries\UpdateAccountLastSeen.sql", new
            {
                AccountId = accountId,
                LastSeen = DateTime.UtcNow
            });
        }

        public void UpdateAccountState(Guid accountId, ScOnlineState state)
        {
            _dbFactory.Ephemeral(o => UpdateAccountState(o, accountId, state));
        }

        public void UpdateAccountState(ManagedDataStorageInstance instance, Guid accountId, ScOnlineState state)
        {
            _dbFactory.Execute(@"SqlQueries\UpdateAccountState.sql",
                new
                {
                    AccountId = accountId,
                    State = state.ToString(),
                    LastSeen = DateTime.UtcNow
                });
        }

        public List<AccountSearchModel> AcceptContactInvite(Guid sourceAccountId, Guid targetAccountId)
        {
            return _dbFactory.Query<AccountSearchModel>(@"SqlQueries\AcceptContactInvite.sql",
                new
                {
                    SourceAccountId = sourceAccountId,
                    TargetAccountId = targetAccountId
                }).ToList();
        }

        public List<AccountSearchModel> RemoveContact(Guid sourceAccountId, Guid targetAccountId)
        {
            return _dbFactory.Query<AccountSearchModel>(@"SqlQueries\RemoveContact.sql",
                new
                {
                    SourceAccountId = sourceAccountId,
                    TargetAccountId = targetAccountId
                }).ToList();
        }

        public List<AccountSearchModel> AddContactInvite(Guid sourceAccountId, Guid targetAccountId)
        {
            string[] ids = [sourceAccountId.ToString(), targetAccountId.ToString()];

            return _dbFactory.Query<AccountSearchModel>(@"SqlQueries\AddContactInvite.sql",
                new
                {
                    SourceAccountId = sourceAccountId,
                    TargetAccountId = targetAccountId,
                    ContactHash = Crypto.ComputeSha256Hash(string.Join(",", ids.OrderBy(o => o)))
                }).ToList();
        }

        public List<AccountSearchModel> AccountSearch(Guid accountId, string displayName)
        {
            var accounts = _dbFactory.Query<AccountSearchModel>(@"SqlQueries\AccountSearch.sql",
                new
                {
                    AccountId = accountId,
                    DisplayName = displayName
                }).ToList();

            foreach (var account in accounts)
            {
                if (account.LastSeen == null || (DateTime.UtcNow - account.LastSeen.Value).TotalSeconds > ScConstants.OfflineLastSeenSeconds)
                {
                    account.State = ScOnlineState.Offline.ToString();
                }
            }

            return accounts;
        }

        public Guid? GetAccountIdByUserName(string username)
        {
            return _dbFactory.QuerySingleOrDefault<Guid?>(@"SqlQueries\GetAccountIdByUserName.sql",
                new
                {
                    Username = username
                });
        }

        public AccountModel GetAccountById(Guid accountId)
        {
            return _dbFactory.QuerySingle<AccountModel>(@"SqlQueries\GetAccountById.sql",
                new
                {
                    Id = accountId
                });
        }

        public Guid? GetAccountIdByDisplayName(string displayName)
        {
            return _dbFactory.QuerySingleOrDefault<Guid?>(@"SqlQueries\GetAccountIdByDisplayName.sql",
                new
                {
                    DisplayName = displayName
                });
        }

        public LoginModel? Login(string username, string passwordHash, bool explicitAway)
        {
            return _dbFactory.Ephemeral<LoginModel?>(o =>
            {
                var login = _dbFactory.QueryFirstOrDefault<LoginModel>(@"SqlQueries\Login.sql",
                    new
                    {
                        Username = username,
                        PasswordHash = passwordHash
                    });

                if (login != null)
                {
                    UpdateAccountLastSeen(o, login.Id);
                    UpdateAccountState(o, login.Id, (explicitAway ? ScOnlineState.Away : ScOnlineState.Online));
                }

                return login;
            });
        }

        public List<ContactModel> GetContacts(Guid accountId)
        {
            var accounts = _dbFactory.Ephemeral(o =>
            {
                var contacts = o.Query<ContactModel>(@"SqlQueries\GetContacts.sql",
                    new
                    {
                        AccountId = accountId,
                    }).ToList();

                UpdateAccountLastSeen(o, accountId);

                return contacts;
            });

            foreach (var account in accounts)
            {
                if (account.IsAccepted == false)
                {
                    account.ProfileJson = ""; //We do not show profile for pending contacts.
                    account.State = ScOnlineState.Pending.ToString();
                }
                else if (account.LastSeen == null || (DateTime.UtcNow - account.LastSeen.Value).TotalSeconds > ScConstants.OfflineLastSeenSeconds)
                {
                    account.State = ScOnlineState.Offline.ToString();
                }
            }

            return accounts;
        }
    }
}
