﻿namespace Talkster.Client.Forms
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            pictureBox1 = new PictureBox();
            buttonOk = new Button();
            listViewVersions = new ListView();
            columnHeaderApplication = new ColumnHeader();
            columnHeaderVersion = new ColumnHeader();
            linkWebsite = new LinkLabel();
            labelVersion = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.Center;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(35, 35);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(103, 108);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // buttonOk
            // 
            buttonOk.Location = new Point(419, 171);
            buttonOk.Margin = new Padding(4, 3, 4, 3);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(88, 27);
            buttonOk.TabIndex = 7;
            buttonOk.Text = "Ok";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += ButtonOk_Click;
            // 
            // listViewVersions
            // 
            listViewVersions.Columns.AddRange(new ColumnHeader[] { columnHeaderApplication, columnHeaderVersion });
            listViewVersions.FullRowSelect = true;
            listViewVersions.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listViewVersions.Location = new Point(174, 14);
            listViewVersions.Margin = new Padding(4, 3, 4, 3);
            listViewVersions.MultiSelect = false;
            listViewVersions.Name = "listViewVersions";
            listViewVersions.Size = new Size(332, 149);
            listViewVersions.Sorting = SortOrder.Ascending;
            listViewVersions.TabIndex = 8;
            listViewVersions.UseCompatibleStateImageBehavior = false;
            listViewVersions.View = View.Details;
            // 
            // columnHeaderApplication
            // 
            columnHeaderApplication.Text = "Application";
            columnHeaderApplication.Width = 100;
            // 
            // columnHeaderVersion
            // 
            columnHeaderVersion.Text = "Version";
            columnHeaderVersion.Width = 75;
            // 
            // linkWebsite
            // 
            linkWebsite.AutoSize = true;
            linkWebsite.Font = new Font("Microsoft Sans Serif", 11F);
            linkWebsite.Location = new Point(170, 171);
            linkWebsite.Margin = new Padding(4, 0, 4, 0);
            linkWebsite.Name = "linkWebsite";
            linkWebsite.Size = new Size(164, 18);
            linkWebsite.TabIndex = 9;
            linkWebsite.TabStop = true;
            linkWebsite.Text = "www.NetworkDLS.com";
            linkWebsite.LinkClicked += LinkWebsite_LinkClicked;
            // 
            // labelVersion
            // 
            labelVersion.Location = new Point(12, 178);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(42, 20);
            labelVersion.TabIndex = 10;
            labelVersion.Values.Text = "v0.0.0";
            // 
            // FormAbout
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(545, 204);
            Controls.Add(labelVersion);
            Controls.Add(linkWebsite);
            Controls.Add(listViewVersions);
            Controls.Add(buttonOk);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAbout";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Talkster";
            Load += FormAbout_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button buttonOk;
        private ListView listViewVersions;
        private ColumnHeader columnHeaderApplication;
        private ColumnHeader columnHeaderVersion;
        private LinkLabel linkWebsite;
        private Krypton.Toolkit.KryptonLabel labelVersion;
    }
}