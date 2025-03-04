using NTDLS.WinFormsHelpers;
using SecureChat.Library;
using SecureChat.Library.Models;
using SecureChat.Library.ReliableMessages;

namespace SecureChat.Client.Forms
{
    public partial class FormAccountSearch : Form
    {
        private readonly Button _cancelButton;

        public FormAccountSearch()
        {
            InitializeComponent();

            _cancelButton = new Button();
            _cancelButton.Click += CancelButton_Click;

            AcceptButton = buttonSearch;
            CancelButton = _cancelButton;

            dataGridViewAccounts.CellContentClick += DataGridViewAccounts_CellContentClick;
        }

        private void DataGridViewAccounts_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (LocalSession.Current == null || !LocalSession.Current.Client.IsConnected)
            {
                MessageBox.Show("Connection to the server was lost.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.InvokeClose(DialogResult.Cancel);
                return;
            }

            if (e.RowIndex >= 0 && dataGridViewAccounts.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                if (dataGridViewAccounts.Rows[e.RowIndex].Tag is AccountSearchModel account)
                {
                    if (dataGridViewAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() == "Invite")
                    {
                        dataGridViewAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Sending";

                        LocalSession.Current.Client.Query(new InviteContactQuery(account.Id)).ContinueWith(o =>
                        {
                            if (!o.IsFaulted && o.Result.IsSuccess)
                            {
                                Invoke(() =>
                                {
                                    dataGridViewAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Remove";
                                });

                                LocalSession.Current.FormHome.Repopulate();
                            }
                        });
                    }
                    else if (dataGridViewAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() == "Remove")
                    {
                        dataGridViewAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Removing";

                        LocalSession.Current.Client.Query(new RemoveContactQuery(account.Id)).ContinueWith(o =>
                        {
                            if (!o.IsFaulted && o.Result.IsSuccess)
                            {
                                Invoke(() =>
                                {
                                    dataGridViewAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Invite";
                                });
                                LocalSession.Current.FormHome.Repopulate();
                            }
                        });

                    }
                }
            }
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (LocalSession.Current == null || !LocalSession.Current.Client.IsConnected)
            {
                MessageBox.Show("Connection to the server was lost.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.InvokeClose(DialogResult.Cancel);
                return;
            }

            var displayName = textBoxDisplayName.Text;

            dataGridViewAccounts.Rows.Clear();

            if (string.IsNullOrEmpty(displayName))
            {
                return;
            }

            LocalSession.Current.Client.Query(new AccountSearchQuery(displayName)).ContinueWith(o =>
            {
                if (!o.IsFaulted && o.Result.IsSuccess)
                {
                    Invoke(() =>
                    {
                        foreach (var account in o.Result.Accounts)
                        {
                            var button = new DataGridViewButtonCell();

                            if (account.Id == LocalSession.Current.AccountId)
                            {
                                button.Value = "You";
                            }
                            else if (account.IsExitingContact)
                            {
                                button.Value = "Remove";
                            }
                            else
                            {
                                button.Value = "Invite";
                            }

                            var row = new DataGridViewRow();
                            row.Cells.AddRange(
                                new DataGridViewTextBoxCell { Value = account.DisplayName },
                                new DataGridViewTextBoxCell { Value = account.State },
                                button
                            ); row.Tag = account;

                            dataGridViewAccounts.Rows.Add(row);
                        }
                    });
                }
            });
        }
    }
}
