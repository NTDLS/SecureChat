using Krypton.Toolkit;

namespace SecureChat.Client.Forms
{
    partial class FormLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLog));
            buttonClear = new KryptonButton();
            dataGridViewLog = new KryptonDataGridView();
            splitContainer1 = new SplitContainer();
            ColumnTimestamp = new KryptonDataGridViewTextBoxColumn();
            ColumnSeverity = new DataGridViewTextBoxColumn();
            ColumnMessage = new KryptonDataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridViewLog).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonClear
            // 
            buttonClear.Location = new Point(12, 12);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(58, 23);
            buttonClear.TabIndex = 2;
            buttonClear.Values.DropDownArrowColor = Color.Empty;
            buttonClear.Values.Text = "Clear";
            buttonClear.Click += ButtonClear_Click;
            // 
            // dataGridViewAccounts
            // 
            dataGridViewLog.AllowUserToAddRows = false;
            dataGridViewLog.AllowUserToDeleteRows = false;
            dataGridViewLog.AllowUserToResizeRows = false;
            dataGridViewLog.BorderStyle = BorderStyle.None;
            dataGridViewLog.CausesValidation = false;
            dataGridViewLog.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            dataGridViewLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewLog.Columns.AddRange(new DataGridViewColumn[] { ColumnTimestamp, ColumnSeverity, ColumnMessage });
            dataGridViewLog.Dock = DockStyle.Fill;
            dataGridViewLog.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewLog.Location = new Point(0, 0);
            dataGridViewLog.MultiSelect = false;
            dataGridViewLog.Name = "dataGridViewAccounts";
            dataGridViewLog.ReadOnly = true;
            dataGridViewLog.RowHeadersVisible = false;
            dataGridViewLog.ShowCellErrors = false;
            dataGridViewLog.ShowCellToolTips = false;
            dataGridViewLog.ShowEditingIcon = false;
            dataGridViewLog.ShowRowErrors = false;
            dataGridViewLog.Size = new Size(726, 361);
            dataGridViewLog.TabIndex = 3;
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = Color.Transparent;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(buttonClear);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridViewLog);
            splitContainer1.Size = new Size(726, 405);
            splitContainer1.SplitterDistance = 40;
            splitContainer1.TabIndex = 4;
            // 
            // ColumnTimestamp
            // 
            ColumnTimestamp.FillWeight = 20F;
            ColumnTimestamp.HeaderText = "Timestamp";
            ColumnTimestamp.Name = "ColumnTimestamp";
            ColumnTimestamp.ReadOnly = true;
            // 
            // ColumnSeverity
            // 
            ColumnSeverity.FillWeight = 20F;
            ColumnSeverity.HeaderText = "Severity";
            ColumnSeverity.Name = "ColumnSeverity";
            ColumnSeverity.ReadOnly = true;
            // 
            // ColumnMessage
            // 
            ColumnMessage.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColumnMessage.HeaderText = "Message";
            ColumnMessage.Name = "ColumnMessage";
            ColumnMessage.ReadOnly = true;
            // 
            // FormLog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(726, 405);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimumSize = new Size(525, 450);
            Name = "FormLog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            ((System.ComponentModel.ISupportInitialize)dataGridViewLog).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private KryptonButton buttonClear;
        private KryptonDataGridView dataGridViewLog;
        private SplitContainer splitContainer1;
        private KryptonDataGridViewTextBoxColumn ColumnTimestamp;
        private DataGridViewTextBoxColumn ColumnSeverity;
        private KryptonDataGridViewTextBoxColumn ColumnMessage;
    }
}