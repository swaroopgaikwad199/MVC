using System.IO;
using System.Windows.Forms;

namespace REDTR.UTILS
{
    public class SettingsPath   //SetupBinPaths
    {
        public static string InnerDir = "\\SetupBin";
        private static string DatabaseDir = "D:\\TnTDB";

        public static string SettingDir
        {
            get { return Application.StartupPath + InnerDir; }
        }
       
        public static void CreateSettingsDir()
        {
            if (Directory.Exists(SettingDir) == false)
                Directory.CreateDirectory(SettingDir);
        }

        public static void CreateDatabaseDir()
        {
            if (Directory.Exists(DatabaseDirectory) == false)
                Directory.CreateDirectory(DatabaseDirectory);
        }

        public static string HWSysConfig
        {
            get { return Application.StartupPath + InnerDir + "\\HWSysConfig.bin"; }
        }

        public static string App
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_AppSettings.bin"; }
        }
        public static string DBConnection
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_DBConfig.rxd"; }
        }
        public static string DBConnectionServer
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_DBConfigServer.rxd"; }
        }
        // By TUshar
        public static string ORADBConnection
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_ORADBConfig.rxd"; }
        }
        public static string Ejector
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_Ejector.bin"; }
        }
        public static string InspectionDevice
        {
            get { return Application.StartupPath + InnerDir + "\\InspectionDevice.rxd"; }
        }
        public static string Vendor
        {
            get { return Application.StartupPath + InnerDir + "\\rodnev.bin"; }
        }
        public static string VendorAppAsso
        {
            get { return Application.StartupPath + InnerDir + "\\VendorAppAsso.rxd"; }
        }
        public static string Commnads
        {
            get { return Application.StartupPath + InnerDir + "\\cmd.bin"; }
        }
        public static string CRBasePrinterConfig 
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_CRBasePrinterSetup.bin"; }
        }
        public static string PackBoxesSetup
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_PackBoxesSetup.rxd"; }
        }
        public static string PrinterSetup
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_PrinterSetup.rxd"; }
        }
        public static string DMSysOpSetting
        {
            get { return Application.StartupPath + InnerDir + "\\DMSystemOutputSetting.rxd"; }
        }
        public static string PwdSetting
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_PwdSetting.bin"; }
        }
        public static string PrintRptProjectSetting
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_PrintRptProject.bin"; }
        }
        public static string UIDSourceConfig
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_UIDSourceConfig.bin"; }
        }
        public static string LanguageSelection
        {
            get { return Application.StartupPath + InnerDir + "\\LanguageSelection.txt"; }
        }
        public static string ADConfig   // Newly For Active Directory [Sunil].
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_ADConfig.bin"; }
        }
        public static string LineConfig // Newly For Line Config [Sunil].
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_LineConfig.rxd"; }
        }
        public static string PackagingLevel // Added by Sagar
        {
            get { return Application.StartupPath + InnerDir + "\\TnT_PackagingLevels.bin"; }
        }
        public static string MainApp_Exe
        {
            get { return Application.StartupPath + "\\REDTR.APP.exe"; }
        }
        public static string MainAppName
        {
            get { return "REDTR.APP"; }
        }
        public static string TnTMgrName
        {
            get { return "REDTR.TntMgr"; }
        }
        public static string DatabaseDirectory
        {
            get { return DatabaseDir; }
        }
    }
}
