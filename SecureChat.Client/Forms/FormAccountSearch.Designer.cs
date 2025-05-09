using Krypton.Toolkit;

namespace SecureChat.Client.Forms
{
    partial class FormAccountSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccountSearch));
            textBoxDisplayName = new KryptonTextBox();
            labelDisplayName = new KryptonLabel();
            buttonSearch = new KryptonButton();
            dataGridViewAccounts = new KryptonDataGridView();
            ColumnName = new KryptonDataGridViewTextBoxColumn();
            ColumnState = new KryptonDataGridViewTextBoxColumn();
            Invite = new KryptonDataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAccounts).BeginInit();
            SuspendLayout();
            // 
            // textBoxDisplayName
            // 
            textBoxDisplayName.Location = new Point(12, 27);
            textBoxDisplayName.Name = "textBoxDisplayName";
            textBoxDisplayName.Size = new Size(440, 23);
            textBoxDisplayName.TabIndex = 0;
            // 
            // labelDisplayName
            // 
            labelDisplayName.Location = new Point(12, 7);
            labelDisplayName.Name = "labelDisplayName";
            labelDisplayName.Size = new Size(86, 20);
            labelDisplayName.TabIndex = 1;
            labelDisplayName.Values.Text = "Display Name";
            // 
            // buttonSearch
            // 
            buttonSearch.Location = new Point(458, 27);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new Size(58, 23);
            buttonSearch.TabIndex = 2;
            buttonSearch.Values.DropDownArrowColor = Color.Empty;
            buttonSearch.Values.Text = "Search";
            buttonSearch.Click += ButtonSearch_Click;
            // 
            // dataGridViewAccounts
            // 
            dataGridViewAccounts.AllowUserToAddRows = false;
            dataGridViewAccounts.AllowUserToDeleteRows = false;
            dataGridViewAccounts.AllowUserToResizeRows = false;
            dataGridViewAccounts.BorderStyle = BorderStyle.None;
            dataGridViewAccounts.CausesValidation = false;
            dataGridViewAccounts.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            dataGridViewAccounts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewAccounts.Columns.AddRange(new DataGridViewColumn[] { ColumnName, ColumnState, Invite });
            dataGridViewAccounts.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewAccounts.Location = new Point(12, 56);
            dataGridViewAccounts.MultiSelect = false;
            dataGridViewAccounts.Name = "dataGridViewAccounts";
            dataGridViewAccounts.ReadOnly = true;
            dataGridViewAccounts.RowHeadersVisible = false;
            dataGridViewAccounts.ShowCellErrors = false;
            dataGridViewAccounts.ShowCellToolTips = false;
            dataGridViewAccounts.ShowEditingIcon = false;
            dataGridViewAccounts.ShowRowErrors = false;
            dataGridViewAccounts.Size = new Size(504, 237);
            dataGridViewAccounts.TabIndex = 3;
            // 
            // ColumnName
            // 
            ColumnName.HeaderText = "Name";
            ColumnName.Name = "ColumnName";
            ColumnName.ReadOnly = true;
            ColumnName.Width = 300;
            // 
            // ColumnState
            // 
            ColumnState.HeaderText = "State";
            ColumnState.Name = "ColumnState";
            ColumnState.ReadOnly = true;
            // 
            // Invite
            // 
            Invite.HeaderText = "Invite";
            Invite.Name = "Invite";
            Invite.ReadOnly = true;
            Invite.Width = 80;
            // 
            // FormAccountSearch
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(534, 303);
            Controls.Add(dataGridViewAccounts);
            Controls.Add(buttonSearch);
            Controls.Add(textBoxDisplayName);
            Controls.Add(labelDisplayName);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormAccountSearch";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            ((System.ComponentModel.ISupportInitialize)dataGridViewAccounts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private KryptonTextBox textBoxDisplayName;
        private KryptonLabel labelDisplayName;
        private KryptonButton buttonSearch;
        private KryptonDataGridView dataGridViewAccounts;
        private KryptonDataGridViewTextBoxColumn ColumnName;
        private KryptonDataGridViewTextBoxColumn ColumnState;
        private KryptonDataGridViewButtonColumn Invite;
    }
}