using Microsoft.Extensions.Configuration;
using NTDLS.SqliteDapperWrapper;
using SecureChat.Server.Models;
using static SecureChat.Library.ScConstants;

namespace SecureChat.Server
{
    internal class DatabaseRepository
    {
        private readonly ManagedDataStorageFactory _dbFactory;

        public DatabaseRepository(IConfiguration configuration)
        {
            var sqliteConnection = configuration.GetValue<string>("SQLiteConnection");
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
            instance.Execute(@"SqlQueries\UpdateAccountState.sql", new
            {
                AccountId = accountId,
                State = state.ToString()
            });
        }

        public Guid? GetAccountIdByUserName(string username)
        {
            return _dbFactory.QuerySingleOrDefault<Guid?>(@"SqlQueries\GetAccountIdByUserName.sql",
                new
                {
                    Username = username
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

        public void UpdateAccountStatus(Guid accountId, ScOnlineState state, string? status)
        {
            _dbFactory.Execute(@"SqlQueries\UpdateAccountStatus.sql",
                new
                {
                    AccountId = accountId,
                    State = state.ToString(),
                    Status = status ?? string.Empty,
                    LastSeen = DateTime.UtcNow
                });
        }

        public LoginModel? Login(string username, string passwordHash, bool explicitAway)
        {
            return _dbFactory.Ephemeral<LoginModel?>(o =>
            {
                var login = _dbFactory.QueryFirst<LoginModel>(@"SqlQueries\Login.sql",
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

        public List<AcquaintanceModel> GetAcquaintances(Guid accountId)
        {
            return _dbFactory.Ephemeral(o =>
            {
                var acquaintances = o.Query<AcquaintanceModel>(@"SqlQueries\GetAcquaintances.sql",
                    new
                    {
                        AccountId = accountId,
                    }).ToList();

                UpdateAccountLastSeen(o, accountId);

                return acquaintances;
            });
        }
    }
}
