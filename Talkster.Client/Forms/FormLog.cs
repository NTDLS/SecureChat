using Krypton.Toolkit;
using System.Diagnostics;
using Talkster.Library;

namespace Talkster.Client.Forms
{
    public partial class FormLog : KryptonForm
    {
        private readonly Button _cancelButton;

        public FormLog()
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            _cancelButton = new Button();
            _cancelButton.Click += CancelButton_Click;

            AcceptButton = buttonClear;
            CancelButton = _cancelButton;

            dataGridViewLog.Rows.Clear();

            lock (Program.Log.Entries)
            {
                foreach (var entry in Program.Log.Entries)
                {
                    var row = new DataGridViewRow()
                    {
                        Tag = entry
                    };
                    row.Cells.AddRange(
                        new DataGridViewTextBoxCell { Value = entry.TimestampUTC },
                        new DataGridViewTextBoxCell { Value = entry.Severity.ToString() },
                        new DataGridViewTextBoxCell { Value = entry.Message }
                    );

                    dataGridViewLog.Rows.Add(row);
                }

                dataGridViewLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            try
            {
                Program.Log.Clear();
                dataGridViewLog.Rows.Clear();
            }
            catch (Exception ex)
            {
                Program.Log.Error($"Error in {new StackTrace().GetFrame(0)?.GetMethod()?.Name ?? "Unknown"}.", ex);
                MessageBox.Show(ex.Message, ScConstants.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
