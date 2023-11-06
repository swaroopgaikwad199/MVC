using System.Collections.Generic;
using System.IO;
using RedXML;

namespace REDTR.UTILS
{
    public class DbConnectionConfig
    {
        private string _DataSourcePath;

        public string DataSourcePath
        {
            get { return _DataSourcePath; }
            set { _DataSourcePath = value; }
        }


        private string _Database;

        public string Database
        {
            get { return _Database; }
            set { _Database = value; }
        }

        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private static List<DbConnectionConfig> LstDbConn = new List<DbConnectionConfig>();
        private static List<DbConnectionConfig> LstDbConnServer = new List<DbConnectionConfig>();
        public static DbConnectionConfig mDbConfig = new DbConnectionConfig();
        //By Tushar
        private static List<DbConnectionConfig> LstOraDbConn = new List<DbConnectionConfig>();
        public static DbConnectionConfig mOraDbConfig = new DbConnectionConfig();

        public static string GetFilePath(string AppStartUpPath)
        {
            FileInfo fi = new FileInfo(UTILS.SettingsPath.DBConnection);

            string DirectoryName = AppStartUpPath + UTILS.SettingsPath.InnerDir; //+ "\\" + fi.Name;
            if (Directory.Exists(DirectoryName) == false)
                Directory.CreateDirectory(DirectoryName);
            string FileName = DirectoryName + "\\" + fi.Name;
            return FileName;
        }

        public static List<DbConnectionConfig> LoadConection(string StartUpPath) //For set up installation
        {
            //string FileName = GetFilePath(StartUpPath);
            if (File.Exists(StartUpPath))
            {
                LstDbConn = GenericXmlSerializer<List<DbConnectionConfig>>.Deserialize(StartUpPath); 
                mDbConfig = LstDbConn[0];
            }
            return LstDbConn;
        }
        public static List<DbConnectionConfig> LoadConection() //For Current Activated File
        {
            string FileName = UTILS.SettingsPath.DBConnection;
            if (File.Exists(FileName))
            {
                LstDbConn = GenericXmlSerializer<List<DbConnectionConfig>>.Deserialize(FileName);
                mDbConfig = LstDbConn[0];
            }
            return LstDbConn;
        }
        public static List<DbConnectionConfig> LoadServerConection() //For Current Activated File
        {
            string FileName = UTILS.SettingsPath.DBConnectionServer;
            if (File.Exists(FileName))
            {
                LstDbConnServer = GenericXmlSerializer<List<DbConnectionConfig>>.Deserialize(FileName);
                //mDbConfig = LstDbConn[0];
            }
            return LstDbConnServer;
        }
        // By tushar
        // For access the Oracle server details from file
        public static List<DbConnectionConfig> LoadConectionOracle()
        {
            string FileName = UTILS.SettingsPath.ORADBConnection;
            if (File.Exists(FileName))
            {
                LstOraDbConn = GenericXmlSerializer<List<DbConnectionConfig>>.Deserialize(FileName);
                mOraDbConfig = LstOraDbConn[0];
            }
            return LstOraDbConn;
        }
        public static void saveConnections(string AppStartUpPath, List<DbConnectionConfig> Lst)
        {
            string FileName = GetFilePath(AppStartUpPath);
            LstDbConn = Lst;
            GenericXmlSerializer<List<DbConnectionConfig>>.Serialize(LstDbConn, FileName);
            mDbConfig = LstDbConn[0];
        }
    }
}
