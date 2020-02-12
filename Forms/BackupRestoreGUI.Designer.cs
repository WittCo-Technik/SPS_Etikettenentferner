namespace SPSCtrl
{
    partial class BackupRestoreGUI
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
            this.accept = new System.Windows.Forms.Button();
            this.abort = new System.Windows.Forms.Button();
            this.backuplist = new System.Windows.Forms.ListView();
            this.backupcolumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // accept
            // 
            this.accept.Enabled = false;
            this.accept.Location = new System.Drawing.Point(41, 254);
            this.accept.Name = "accept";
            this.accept.Size = new System.Drawing.Size(86, 45);
            this.accept.TabIndex = 0;
            this.accept.Text = "BACKUP EINSPIELEN";
            this.accept.UseVisualStyleBackColor = true;
            this.accept.Click += new System.EventHandler(this.Accept_Click);
            // 
            // abort
            // 
            this.abort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.abort.Location = new System.Drawing.Point(347, 254);
            this.abort.Name = "abort";
            this.abort.Size = new System.Drawing.Size(75, 45);
            this.abort.TabIndex = 1;
            this.abort.Text = "Abbrechen";
            this.abort.UseVisualStyleBackColor = true;
            this.abort.Click += new System.EventHandler(this.abort_Click);
            // 
            // backuplist
            // 
            this.backuplist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.backupcolumn,
            this.date});
            this.backuplist.HideSelection = false;
            this.backuplist.Location = new System.Drawing.Point(12, 12);
            this.backuplist.MultiSelect = false;
            this.backuplist.Name = "backuplist";
            this.backuplist.Size = new System.Drawing.Size(439, 231);
            this.backuplist.TabIndex = 2;
            this.backuplist.UseCompatibleStateImageBehavior = false;
            this.backuplist.View = System.Windows.Forms.View.Details;
            this.backuplist.SelectedIndexChanged += new System.EventHandler(this.ListView1_SelectedIndexChanged);
            // 
            // backupcolumn
            // 
            this.backupcolumn.Text = "Backup-Datei";
            this.backupcolumn.Width = 251;
            // 
            // date
            // 
            this.date.Text = "Erstelldatum";
            this.date.Width = 185;
            // 
            // BackupRestoreGUI
            // 
            this.AcceptButton = this.accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.abort;
            this.ClientSize = new System.Drawing.Size(463, 311);
            this.ControlBox = false;
            this.Controls.Add(this.backuplist);
            this.Controls.Add(this.abort);
            this.Controls.Add(this.accept);
            this.Name = "BackupRestoreGUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Backup einspielen";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.Button abort;
        private System.Windows.Forms.ListView backuplist;
        private System.Windows.Forms.ColumnHeader backupcolumn;
        private System.Windows.Forms.ColumnHeader date;
    }
}