using Krypton.Toolkit;
using System.ComponentModel;
using Talkster.Client.Helpers;

namespace Talkster.Client.Forms
{
    /// <summary>
    /// Progress form used for multi-threaded progress reporting.
    /// </summary>
    internal partial class FormThemedProgress : KryptonForm
    {
        #region Events

        /// <summary>
        /// Event to fire if the clicks user cancels.
        /// </summary>
        public event ThemedProgressForm.EventOnCancel? OnCancel;

        #endregion

        /// <summary>
        /// Indicates whether the form has been shown or not.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasBeenShown { get; private set; } = false;

        /// <summary>
        /// Indicates whether a cancel operation has been started.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCancelPending { get; private set; } = false;

        /// <summary>
        /// Creates a new instance of the FormProgress which is used for multi-threaded progress reporting.
        /// </summary>
        public FormThemedProgress()
        {
            InitializeComponent();

            BackColor = KryptonManager.CurrentGlobalPalette.GetBackColor1(PaletteBackStyle.PanelClient, PaletteState.Normal);

            labelBody.Text = "";
            buttonCancel.Enabled = false;
            pbProgress.Minimum = 0;
            pbProgress.Maximum = 100;

            DialogResult = DialogResult.OK;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            IsCancelPending = true;
            if (OnCancel != null)
            {
                var onCancelInfo = new ThemedProgressForm.OnCancelInfo();
                OnCancel(this, onCancelInfo);
                if (onCancelInfo.Cancel)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Closes the form in a thread safe manner.
        /// </summary>
        public new void Close()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Close));
                return;
            }

            base.Close();
        }

        /// <summary>
        /// Closes the form in a thread safe manner, with the given dialog result.
        /// </summary>
        /// <param name="result"></param>
        public void Close(DialogResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<DialogResult>(Close), result);
                return;
            }

            DialogResult = result;

            base.Close();
        }

        /// <summary>
        /// Sets the header label text in a thread safe manner (this is not the title).
        /// </summary>
        /// <param name="text"></param>
        public void SetHeaderText(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(SetHeaderText), text);
                return;
            }

            labelHeader.Text = text;
        }

        /// <summary>
        /// Sets the body label text in a thread safe manner (this is not the title).
        /// </summary>
        /// <param name="text"></param>
        public void SetBodyText(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(SetBodyText), text);
                return;
            }

            labelBody.Text = text;
        }

        /// <summary>
        /// Sets the form title text in a thread safe manner.
        /// </summary>
        /// <param name="text"></param>
        public void SetTitleText(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(SetTitleText), text);
                return;
            }

            Text = text;
        }

        /// <summary>
        /// Sets the progress bar minimum value in a thread safe manner.
        /// </summary>
        /// <param name="value"></param>
        public void SetProgressMinimum(int value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(SetProgressMinimum), value);
                return;
            }

            if (pbProgress.Style == ProgressBarStyle.Marquee)
            {
                pbProgress.Style = ProgressBarStyle.Continuous;
            }

            pbProgress.Minimum = value;
        }

        /// <summary>
        /// Sets the progress bar maximum value in a thread safe manner.
        /// </summary>
        /// <param name="value"></param>
        public void SetProgressMaximum(int value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(SetProgressMaximum), value);
                return;
            }

            if (pbProgress.Style == ProgressBarStyle.Marquee)
            {
                pbProgress.Style = ProgressBarStyle.Continuous;
            }

            pbProgress.Maximum = value;
        }

        /// <summary>
        /// Increments the progress bar value in a thread safe manner.
        /// </summary>
        public void IncrementProgressValue()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(IncrementProgressValue));
                return;
            }

            if (pbProgress.Style == ProgressBarStyle.Marquee)
            {
                pbProgress.Style = ProgressBarStyle.Continuous;
            }

            pbProgress.Value++;
        }

        /// <summary>
        /// Sets the progress bar value in a thread safe manner.
        /// </summary>
        /// <param name="value"></param>
        public void SetProgressValue(int value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(SetProgressValue), value);
                return;
            }

            if (pbProgress.Style == ProgressBarStyle.Marquee)
            {
                pbProgress.Style = ProgressBarStyle.Continuous;
            }

            pbProgress.Value = value;
        }

        /// <summary>
        /// Sets the progress bar style in a thread safe manner.
        /// </summary>
        /// <param name="value"></param>
        public void SeProgressStyle(ProgressBarStyle value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<ProgressBarStyle>(SeProgressStyle), value);
                return;
            }

            pbProgress.Style = value;
        }

        /// <summary>
        /// Enables or disabled cancelation support in a thread safe manner.
        /// </summary>
        /// <param name="value"></param>
        public void SetCanCancel(bool value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<bool>(SetCanCancel), value);
                return;
            }

            buttonCancel.Enabled = value;
        }

        private void FormProgress_Shown(object sender, EventArgs e)
        {
            HasBeenShown = true;
            IsCancelPending = false;
        }
    }
}
