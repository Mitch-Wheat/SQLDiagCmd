namespace SQLDiagRunner
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkTrustedConnection = new System.Windows.Forms.CheckBox();
            this.txtScriptLocation = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btBrowseScriptLocation = new System.Windows.Forms.Button();
            this.btBrowseOutputFolder = new System.Windows.Forms.Button();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btExecute = new System.Windows.Forms.Button();
            this.txtDBs = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkAutoFitExcelColumns = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(74, 20);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(144, 20);
            this.txtServer.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(300, 46);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(144, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(244, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(300, 20);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(144, 20);
            this.txtUsername.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(242, 23);
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
            this.chkTrustedConnection.Location = new System.Drawing.Point(74, 49);
            this.chkTrustedConnection.Name = "chkTrustedConnection";
            this.chkTrustedConnection.Size = new System.Drawing.Size(119, 17);
            this.chkTrustedConnection.TabIndex = 6;
            this.chkTrustedConnection.Text = "Trusted Connection";
            this.chkTrustedConnection.UseVisualStyleBackColor = true;
            this.chkTrustedConnection.CheckedChanged += new System.EventHandler(this.ChkTrustedConnectionCheckedChanged);
            // 
            // txtScriptLocation
            // 
            this.txtScriptLocation.Location = new System.Drawing.Point(74, 84);
            this.txtScriptLocation.Name = "txtScriptLocation";
            this.txtScriptLocation.Size = new System.Drawing.Size(370, 20);
            this.txtScriptLocation.TabIndex = 8;
            this.txtScriptLocation.TextChanged += new System.EventHandler(this.TxtScriptLocationTextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Script:";
            // 
            // btBrowseScriptLocation
            // 
            this.btBrowseScriptLocation.Location = new System.Drawing.Point(451, 82);
            this.btBrowseScriptLocation.Name = "btBrowseScriptLocation";
            this.btBrowseScriptLocation.Size = new System.Drawing.Size(61, 23);
            this.btBrowseScriptLocation.TabIndex = 9;
            this.btBrowseScriptLocation.Text = "Browse...";
            this.btBrowseScriptLocation.UseVisualStyleBackColor = true;
            this.btBrowseScriptLocation.Click += new System.EventHandler(this.BtBrowseScriptLocationClick);
            // 
            // btBrowseOutputFolder
            // 
            this.btBrowseOutputFolder.Location = new System.Drawing.Point(451, 114);
            this.btBrowseOutputFolder.Name = "btBrowseOutputFolder";
            this.btBrowseOutputFolder.Size = new System.Drawing.Size(61, 23);
            this.btBrowseOutputFolder.TabIndex = 12;
            this.btBrowseOutputFolder.Text = "Browse...";
            this.btBrowseOutputFolder.UseVisualStyleBackColor = true;
            this.btBrowseOutputFolder.Click += new System.EventHandler(this.BtBrowseOutputFolderClick);
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(74, 116);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(370, 20);
            this.txtOutputFolder.TabIndex = 11;
            this.txtOutputFolder.TextChanged += new System.EventHandler(this.TxtOutputFolderTextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Output To:";
            // 
            // btExecute
            // 
            this.btExecute.Location = new System.Drawing.Point(451, 181);
            this.btExecute.Name = "btExecute";
            this.btExecute.Size = new System.Drawing.Size(61, 23);
            this.btExecute.TabIndex = 13;
            this.btExecute.Text = "Execute...";
            this.btExecute.UseVisualStyleBackColor = true;
            this.btExecute.Click += new System.EventHandler(this.BtExecuteClick);
            // 
            // txtDBs
            // 
            this.txtDBs.Location = new System.Drawing.Point(74, 152);
            this.txtDBs.Name = "txtDBs";
            this.txtDBs.Size = new System.Drawing.Size(370, 20);
            this.txtDBs.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1, 155);
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
            this.chkAutoFitExcelColumns.Location = new System.Drawing.Point(74, 185);
            this.chkAutoFitExcelColumns.Name = "chkAutoFitExcelColumns";
            this.chkAutoFitExcelColumns.Size = new System.Drawing.Size(128, 17);
            this.chkAutoFitExcelColumns.TabIndex = 16;
            this.chkAutoFitExcelColumns.Text = "Autofit Excel Columns";
            this.chkAutoFitExcelColumns.UseVisualStyleBackColor = true;
            // 
            // FrmSqlDiag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 231);
            this.Controls.Add(this.chkAutoFitExcelColumns);
            this.Controls.Add(this.txtDBs);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btExecute);
            this.Controls.Add(this.btBrowseOutputFolder);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btBrowseScriptLocation);
            this.Controls.Add(this.txtScriptLocation);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkTrustedConnection);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.label1);
            this.Name = "FrmSqlDiag";
            this.Text = "SQL Diagnostic Runner";
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btBrowseScriptLocation;
        private System.Windows.Forms.Button btBrowseOutputFolder;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btExecute;
        private System.Windows.Forms.TextBox txtDBs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkAutoFitExcelColumns;
    }
}

