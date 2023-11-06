using System;
using System.Data.SqlClient;
using System.Diagnostics;
using REDTR.UTILS;
using REDTR.UTILS.SystemIntegrity;
using System.Configuration;

namespace REDTR.DB.Connection
{
    public class CreateDbConnection
    {
        public static string AppStartUpPath = string.Empty;
        public string GetConnectionString()
        {
            try
            {
                
                //if (string.IsNullOrEmpty(DbConnectionConfig.mDbConfig.Database))
                //{
                //    if (!string.IsNullOrEmpty(AppStartUpPath)) DbConnectionConfig.LoadConection();
                //    else DbConnectionConfig.LoadConection(AppStartUpPath);
                //}

                string con  = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                SqlConnectionStringBuilder sqlconn = new SqlConnectionStringBuilder(con);
                //sqlconn.DataSource = DbConnectionConfig.mDbConfig.DataSourcePath;
                //sqlconn.UserID = DbConnectionConfig.mDbConfig.UserName;
                //sqlconn.Password = Globals.SimpleEncrypt.Decrypt(DbConnectionConfig.mDbConfig.Password);
                //sqlconn.InitialCatalog = DbConnectionConfig.mDbConfig.Database;
               
                //sqlconn.DataSource = "PROPIX27-PC";
                //sqlconn.UserID = "sa";
                //sqlconn.Password = "paul123#";
                //sqlconn.InitialCatalog = "SpdWarehouseTesting";

                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = sqlconn.ConnectionString;
                sqlconn.MultipleActiveResultSets = true;
                sqlconn.PersistSecurityInfo = true;
                return sqlconn.ConnectionString;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
                return string.Empty;
            }
        }
    }
}
