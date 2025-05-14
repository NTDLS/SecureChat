using Krypton.Toolkit;

namespace SecureChat.Client.Forms
{
    partial class FormFindPeople
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFindPeople));
            textBoxDisplayName = new KryptonTextBox();
            labelDisplayName = new KryptonLabel();
            buttonSearch = new KryptonButton();
            dataGridViewAccounts = new KryptonDataGridView();
            ColumnName = new KryptonDataGridViewTextBoxColumn();
            ColumnState = new KryptonDataGridViewTextBoxColumn();
            Invite = new KryptonDataGridViewButtonColumn();
            splitContainer1 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAccounts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // textBoxDisplayName
            // 
            textBoxDisplayName.Location = new Point(5, 23);
            textBoxDisplayName.Name = "textBoxDisplayName";
            textBoxDisplayName.Size = new Size(438, 23);
            textBoxDisplayName.TabIndex = 0;
            // 
            // labelDisplayName
            // 
            labelDisplayName.Location = new Point(1, 3);
            labelDisplayName.Name = "labelDisplayName";
            labelDisplayName.Size = new Size(86, 20);
            labelDisplayName.TabIndex = 1;
            labelDisplayName.Values.Text = "Display Name";
            // 
            // buttonSearch
            // 
            buttonSearch.Location = new Point(449, 23);
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
            dataGridViewAccounts.Dock = DockStyle.Fill;
            dataGridViewAccounts.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridViewAccounts.Location = new Point(0, 0);
            dataGridViewAccounts.MultiSelect = false;
            dataGridViewAccounts.Name = "dataGridViewAccounts";
            dataGridViewAccounts.ReadOnly = true;
            dataGridViewAccounts.RowHeadersVisible = false;
            dataGridViewAccounts.ShowCellErrors = false;
            dataGridViewAccounts.ShowCellToolTips = false;
            dataGridViewAccounts.ShowEditingIcon = false;
            dataGridViewAccounts.ShowRowErrors = false;
            dataGridViewAccounts.Size = new Size(519, 352);
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
            splitContainer1.Panel1.Controls.Add(textBoxDisplayName);
            splitContainer1.Panel1.Controls.Add(buttonSearch);
            splitContainer1.Panel1.Controls.Add(labelDisplayName);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridViewAccounts);
            splitContainer1.Size = new Size(519, 405);
            splitContainer1.SplitterDistance = 49;
            splitContainer1.TabIndex = 4;
            // 
            // FormFindPeople
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(519, 405);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimumSize = new Size(525, 450);
            Name = "FormFindPeople";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Secure Chat";
            ((System.ComponentModel.ISupportInitialize)dataGridViewAccounts).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private KryptonTextBox textBoxDisplayName;
        private KryptonLabel labelDisplayName;
        private KryptonButton buttonSearch;
        private KryptonDataGridView dataGridViewAccounts;
        private KryptonDataGridViewTextBoxColumn ColumnName;
        private KryptonDataGridViewTextBoxColumn ColumnState;
        private KryptonDataGridViewButtonColumn Invite;
        private SplitContainer splitContainer1;
    }
}