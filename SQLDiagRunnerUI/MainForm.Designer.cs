namespace SQLDiagUI
{
    partial class FrmSqlDiag
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSqlDiag));
            this.label1 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkTrustedConnection = new System.Windows.Forms.CheckBox();
            this.txtScriptLocation = new System.Windows.Forms.TextBox();
            this.btBrowseScriptLocation = new System.Windows.Forms.Button();
            this.btBrowseOutputFolder = new System.Windows.Forms.Button();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.btExecute = new System.Windows.Forms.Button();
            this.txtDBs = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkAutoFitExcelColumns = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.gbConnection = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.gbScript = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtExcludeQueryNumbers = new System.Windows.Forms.TextBox();
            this.gbConnection.SuspendLayout();
            this.gbScript.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "SQL Server:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(74, 19);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(175, 20);
            this.txtServer.TabIndex = 0;
            this.toolTip.SetToolTip(this.txtServer, "Server or Instance name (semicolon separated list).");
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(332, 45);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(156, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(276, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(332, 19);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(156, 20);
            this.txtUsername.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(274, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Username:";
            // 
            // chkTrustedConnection
            // 
            this.chkTrustedConnection.AutoSize = true;
            this.chkTrustedConnection.Checked = true;
            this.chkTrustedConnection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTrustedConnection.Location = new System.Drawing.Point(75, 45);
            this.chkTrustedConnection.Name = "chkTrustedConnection";
            this.chkTrustedConnection.Size = new System.Drawing.Size(141, 17);
            this.chkTrustedConnection.TabIndex = 1;
            this.chkTrustedConnection.Text = "Use Trusted Connection";
            this.toolTip.SetToolTip(this.chkTrustedConnection, "Connect using your logged on Windows credentials.");
            this.chkTrustedConnection.UseVisualStyleBackColor = true;
            this.chkTrustedConnection.CheckedChanged += new System.EventHandler(this.ChkTrustedConnectionCheckedChanged);
            // 
            // txtScriptLocation
            // 
            this.txtScriptLocation.Location = new System.Drawing.Point(18, 19);
            this.txtScriptLocation.Name = "txtScriptLocation";
            this.txtScriptLocation.Size = new System.Drawing.Size(413, 20);
            this.txtScriptLocation.TabIndex = 0;
            this.toolTip.SetToolTip(this.txtScriptLocation, "Location of Glenn Berry\'s SQL diagnostic script that targets your version of SQL " +
        "Server.");
            this.txtScriptLocation.TextChanged += new System.EventHandler(this.TxtScriptLocationTextChanged);
            // 
            // btBrowseScriptLocation
            // 
            this.btBrowseScriptLocation.Location = new System.Drawing.Point(438, 17);
            this.btBrowseScriptLocation.Name = "btBrowseScriptLocation";
            this.btBrowseScriptLocation.Size = new System.Drawing.Size(59, 23);
            this.btBrowseScriptLocation.TabIndex = 1;
            this.btBrowseScriptLocation.Text = "Browse...";
            this.btBrowseScriptLocation.UseVisualStyleBackColor = true;
            this.btBrowseScriptLocation.Click += new System.EventHandler(this.BtBrowseScriptLocationClick);
            // 
            // btBrowseOutputFolder
            // 
            this.btBrowseOutputFolder.Location = new System.Drawing.Point(438, 16);
            this.btBrowseOutputFolder.Name = "btBrowseOutputFolder";
            this.btBrowseOutputFolder.Size = new System.Drawing.Size(59, 23);
            this.btBrowseOutputFolder.TabIndex = 1;
            this.btBrowseOutputFolder.Text = "Browse...";
            this.btBrowseOutputFolder.UseVisualStyleBackColor = true;
            this.btBrowseOutputFolder.Click += new System.EventHandler(this.BtBrowseOutputFolderClick);
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(18, 19);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(413, 20);
            this.txtOutputFolder.TabIndex = 0;
            this.toolTip.SetToolTip(this.txtOutputFolder, "Folder to Save Excel Results file to.  If a folder is specified, the filename wil" +
        "l be generated automatically, consisting of date, time and server name.");
            this.txtOutputFolder.TextChanged += new System.EventHandler(this.TxtOutputFolderTextChanged);
            // 
            // btExecute
            // 
            this.btExecute.Location = new System.Drawing.Point(453, 378);
            this.btExecute.Name = "btExecute";
            this.btExecute.Size = new System.Drawing.Size(61, 23);
            this.btExecute.TabIndex = 0;
            this.btExecute.Text = "Execute...";
            this.btExecute.UseVisualStyleBackColor = true;
            this.btExecute.Click += new System.EventHandler(this.BtExecuteClick);
            // 
            // txtDBs
            // 
            this.txtDBs.Location = new System.Drawing.Point(80, 19);
            this.txtDBs.Name = "txtDBs";
            this.txtDBs.Size = new System.Drawing.Size(417, 20);
            this.txtDBs.TabIndex = 0;
            this.toolTip.SetToolTip(this.txtDBs, "Semi-colon separated list of databases to run DB specific queries against.");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Specific DBs:";
            // 
            // chkAutoFitExcelColumns
            // 
            this.chkAutoFitExcelColumns.AutoSize = true;
            this.chkAutoFitExcelColumns.Checked = true;
            this.chkAutoFitExcelColumns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoFitExcelColumns.Location = new System.Drawing.Point(18, 45);
            this.chkAutoFitExcelColumns.Name = "chkAutoFitExcelColumns";
            this.chkAutoFitExcelColumns.Size = new System.Drawing.Size(128, 17);
            this.chkAutoFitExcelColumns.TabIndex = 2;
            this.chkAutoFitExcelColumns.Text = "Autofit Excel Columns";
            this.toolTip.SetToolTip(this.chkAutoFitExcelColumns, "Should Excel worksheet columns be auto-sized to fit contents.");
            this.chkAutoFitExcelColumns.UseVisualStyleBackColor = true;
            // 
            // txtTimeout
            // 
            this.txtTimeout.Location = new System.Drawing.Point(155, 42);
            this.txtTimeout.MaxLength = 5;
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(41, 20);
            this.txtTimeout.TabIndex = 1;
            this.txtTimeout.Text = "360";
            this.txtTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.txtTimeout, "Individual Query execution timeout in seconds.");
            this.txtTimeout.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTimeout_KeyPress);
            // 
            // gbConnection
            // 
            this.gbConnection.Controls.Add(this.txtServer);
            this.gbConnection.Controls.Add(this.label1);
            this.gbConnection.Controls.Add(this.label2);
            this.gbConnection.Controls.Add(this.txtPassword);
            this.gbConnection.Controls.Add(this.label3);
            this.gbConnection.Controls.Add(this.txtUsername);
            this.gbConnection.Controls.Add(this.chkTrustedConnection);
            this.gbConnection.Location = new System.Drawing.Point(15, 25);
            this.gbConnection.Name = "gbConnection";
            this.gbConnection.Size = new System.Drawing.Size(506, 84);
            this.gbConnection.TabIndex = 1;
            this.gbConnection.TabStop = false;
            this.gbConnection.Text = "Connection Details:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(198, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "(s)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(8, 45);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(149, 13);
            this.lblTimeout.TabIndex = 7;
            this.lblTimeout.Text = "Long Running Query Timeout:";
            // 
            // gbScript
            // 
            this.gbScript.Controls.Add(this.txtScriptLocation);
            this.gbScript.Controls.Add(this.btBrowseScriptLocation);
            this.gbScript.Location = new System.Drawing.Point(15, 114);
            this.gbScript.Name = "gbScript";
            this.gbScript.Size = new System.Drawing.Size(506, 50);
            this.gbScript.TabIndex = 2;
            this.gbScript.TabStop = false;
            this.gbScript.Text = "SQL Script to Run:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtOutputFolder);
            this.groupBox1.Controls.Add(this.btBrowseOutputFolder);
            this.groupBox1.Controls.Add(this.chkAutoFitExcelColumns);
            this.groupBox1.Location = new System.Drawing.Point(15, 303);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(506, 69);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output Results To:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtDBs);
            this.groupBox2.Controls.Add(this.txtTimeout);
            this.groupBox2.Controls.Add(this.lblTimeout);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(15, 171);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(506, 70);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User Databases:";
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.Right;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(468, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(67, 412);
            this.menuStrip.TabIndex = 21;
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(54, 19);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtExcludeQueryNumbers);
            this.groupBox3.Location = new System.Drawing.Point(15, 247);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(506, 50);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Query Numbers to Exclude:";
            // 
            // txtExcludeQueryNumbers
            // 
            this.txtExcludeQueryNumbers.Location = new System.Drawing.Point(18, 19);
            this.txtExcludeQueryNumbers.Name = "txtExcludeQueryNumbers";
            this.txtExcludeQueryNumbers.Size = new System.Drawing.Size(479, 20);
            this.txtExcludeQueryNumbers.TabIndex = 0;
            this.toolTip.SetToolTip(this.txtExcludeQueryNumbers, "Location of Glenn Berry\'s SQL diagnostic script that targets your version of SQL " +
        "Server.");
            // 
            // FrmSqlDiag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 412);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbScript);
            this.Controls.Add(this.gbConnection);
            this.Controls.Add(this.btExecute);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSqlDiag";
            this.Text = "SQL Server Diagnostic Script Runner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSqlDiag_FormClosing);
            this.Load += new System.EventHandler(this.FrmSqlDiag_Load);
            this.gbConnection.ResumeLayout(false);
            this.gbConnection.PerformLayout();
            this.gbScript.ResumeLayout(false);
            this.gbScript.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkTrustedConnection;
        private System.Windows.Forms.TextBox txtScriptLocation;
        private System.Windows.Forms.Button btBrowseScriptLocation;
        private System.Windows.Forms.Button btBrowseOutputFolder;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Button btExecute;
        private System.Windows.Forms.TextBox txtDBs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkAutoFitExcelColumns;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.GroupBox gbConnection;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.GroupBox gbScript;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtExcludeQueryNumbers;
    }
}

