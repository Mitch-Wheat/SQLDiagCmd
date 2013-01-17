using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SQLDiagRunner
{
    public partial class FrmSqlDiag : Form
    {
        public FrmSqlDiag()
        {
            InitializeComponent();

            SetTrustedConnectionState();
            SetExecuteButtonEnabledState();
        }

        private void ChkTrustedConnectionCheckedChanged(object sender, EventArgs e)
        {
            SetTrustedConnectionState();
        }

        private void SetTrustedConnectionState()
        {
            bool userEnabled = !chkTrustedConnection.Checked;

            txtUsername.Enabled = userEnabled;
            txtPassword.Enabled = userEnabled;
        }

        private void BtBrowseScriptLocationClick(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();

            ofd.Title = "Select SQL Script File";
            ofd.InitialDirectory = @".";
            ofd.Filter = "Text files (*.txt)|*.txt|SQL files (*.sql)|*.sql";
            ofd.FilterIndex = 2;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtScriptLocation.Text = ofd.FileName;
            }
        }

        private void BtBrowseOutputFolderClick(object sender, EventArgs e)
        {
            var ofd = new FolderBrowserDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtOutputFolder.Text = ofd.SelectedPath;
            }
        }

        private void TxtScriptLocationTextChanged(object sender, EventArgs e)
        {
            SetExecuteButtonEnabledState();
        }

        private void SetExecuteButtonEnabledState()
        {
            btExecute.Enabled = File.Exists(txtScriptLocation.Text) &&
                                Directory.Exists(txtOutputFolder.Text) &&
                                IsSecurityInfoPresent();
        }

        private bool IsSecurityInfoPresent()
        {
            return (!string.IsNullOrEmpty(txtServer.Text) ||
                    (!chkTrustedConnection.Checked && (!string.IsNullOrEmpty(txtUsername.Text)
                                                      && !string.IsNullOrEmpty(txtPassword.Text))));
        }

        private void TxtOutputFolderTextChanged(object sender, EventArgs e)
        {
            SetExecuteButtonEnabledState();
        }

        private void BtExecuteClick(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                var databases = new List<string>(txtDBs.Text.Trim().Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries));

                var runner = new Runner();
                runner.ExecuteQueries(txtServer.Text, txtUsername.Text, txtPassword.Text,
                                      txtScriptLocation.Text, txtOutputFolder.Text, databases,
                                      chkTrustedConnection.Checked, chkAutoFitExcelColumns.Checked, 360);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }        
        }

    }
}
