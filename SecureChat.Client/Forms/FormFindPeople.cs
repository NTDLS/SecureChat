using Krypton.Toolkit;
using NTDLS.WinFormsHelpers;
using SecureChat.Library;
using SecureChat.Library.Models;
using SecureChat.Library.ReliableMessages;
using Serilog;
using System.Diagnostics;

namespace SecureChat.Client.Forms
{
    public partial class FormFindPeople : KryptonForm
    {
        private readonly Button _cancelButton;

        public FormFindPeople()
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            _cancelButton = new Button();
            _cancelButton.Click += CancelButton_Click;

            AcceptButton = buttonSearch;
            CancelButton = _cancelButton;

            dataGridViewAccounts.CellContentClick += DataGridViewAccounts_CellContentClick;
        }

        private void DataGridViewAccounts_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (ServerConnection.Current == null || !ServerConnection.Current.Connection.Client.IsConnected)
                {
                    MessageBox.Show("Connection to the server was lost.", ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.InvokeClose(DialogResult.Cancel);
                    return;
                }

                if (e.RowIndex >= 0 && dataGridViewAccounts.Columns[e.ColumnIndex] is KryptonDataGridViewButtonColumn)
                {
                    if (dataGridViewAccounts.Rows[e.RowIndex].Tag is AccountSearchModel account)
                    {
                        if (dataGridViewAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() == "Invite")
                        {
                            dataGridViewAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Sending";

                            ServerConnection.Current.Connection.Client.Query(new InviteContactQuery(account.Id)).ContinueWith(o =>
                            {
                                if (!o.IsFaulted && o.Result.IsSuccess)
                                {
                                    Invoke(() =>
                                    {
                                        dataGridViewAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Remove";
                                    });

                                    ServerConnection.Current.FormHome.Repopulate();
                                }
                            });
                        }
                        else if (dataGridViewAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() == "Remove")
                        {
                            dataGridViewAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Removing";

                            ServerConnection.Current.Connection.Client.Query(new RemoveContactQuery(account.Id)).ContinueWith(o =>
                            {
                                if (!o.IsFaulted && o.Result.IsSuccess)
                                {
                                    Invoke(() =>
                                    {
                                        dataGridViewAccounts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Invite";
                                    });
                                    ServerConnection.Current.FormHome.Repopulate();
                                }
                            });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ServerConnection.Current == null || !ServerConnection.Current.Connection.Client.IsConnected)
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

                ServerConnection.Current.Connection.Client.Query(new AccountSearchQuery(displayName)).ContinueWith(o =>
                {
                    if (!o.IsFaulted && o.Result.IsSuccess)
                    {
                        Invoke(() =>
                        {
                            foreach (var account in o.Result.Accounts)
                            {
                                var button = new DataGridViewButtonCell();

                                if (account.Id == ServerConnection.Current.AccountId)
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
            catch (Exception ex)
            {
                Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
