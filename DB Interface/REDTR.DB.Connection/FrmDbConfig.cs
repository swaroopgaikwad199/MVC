using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using REDTR.UTILS;
using REDTR.UTILS.SystemIntegrity;
namespace REDTR.DB.Connection
{
    public partial class FrmDbConfig : Form
    {
        string AppStartUpPath;
        public FrmDbConfig(string DirName)
        {
            InitializeComponent();
            AppStartUpPath = DirName;
            ReadSettings(DirName);
        }
        private string DataBaseName = "TRACKnTRACE";
        private void ReadSettings(string DirName)
        {
            if (string.IsNullOrEmpty(DbConnectionConfig.mDbConfig.Database))
            {
                DbConnectionConfig.LoadConection(DirName);
            }
            if (string.IsNullOrEmpty(DbConnectionConfig.mDbConfig.Database))
            {
                TXT_DbName.Text = DataBaseName;
                return;
            }

            TXT_DbServer.Text = DbConnectionConfig.mDbConfig.DataSourcePath;
            TXT_DbName.Text = DbConnectionConfig.mDbConfig.Database;
            TXT_UserName.Text = DbConnectionConfig.mDbConfig.UserName;
            TXT_PWD.Text = Globals.SimpleEncrypt.Decrypt(DbConnectionConfig.mDbConfig.Password);
        }
        private void BTN_SaveHWSysConfig_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TXT_DbServer.Text))
            {
                MessageBox.Show("PLEASE Enter Database Server");
                return;
            }
            else if (string.IsNullOrEmpty(TXT_DbName.Text))
            {
                MessageBox.Show("PLEASE Enter Database Name");
                return;
            }
            else if (string.IsNullOrEmpty(TXT_UserName.Text))
            {
                MessageBox.Show("PLEASE Enter User Name");
                return;
            }
            else if (string.IsNullOrEmpty(TXT_PWD.Text))
            {
                MessageBox.Show("PLEASE Enter Password");
                return;
            }
            try
            {
                List<DbConnectionConfig> lst = new List<DbConnectionConfig>();

                DbConnectionConfig db = new DbConnectionConfig();
                db.DataSourcePath = TXT_DbServer.Text;
                db.Database = TXT_DbName.Text;
                db.UserName = TXT_UserName.Text;
                string strEncrypt = Globals.SimpleEncrypt.Encrypt(TXT_PWD.Text);
                db.Password = strEncrypt;
                lst.Add(db);
                //MessageBox.Show(AppStartUpPath);
                DbConnectionConfig.saveConnections(AppStartUpPath, lst);
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);

            }
            //this.DialogResult = DialogResult.OK;
            //this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
            //Application.Exit();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            TXT_DbName.Enabled = true;
        }
    }
}
