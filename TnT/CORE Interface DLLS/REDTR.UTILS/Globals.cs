using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using RedUtils;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Globalization;

namespace REDTR.UTILS.SystemIntegrity
{
    /// <summary>
    /// These are the DECKs used for various packaging levels.
    /// </summary>
    public enum DECKs
    {
        PPB = 1,    // PPB PrimaryPACK
        MOC,        //  MonoCARTON
        OBX,        //  OuterBOX
        ISH,        //  InnerSHIPPER
        OSH,        //  OuterSHIPPER
        PAL,        //  PALLET
        None
    }
    /// <summary>
    /// This enum will be used to store the reason of failure during Inspection.
    /// The reasons will be stored with Seperator as #.
    /// EX. 
    /// If 2D & GTIN Text failed than : 1#3
    /// If BC-PI & BC-SSCC failed than : 7#8
    /// </summary>
    /// 

    public static class InspectionFailed
    {
        public const float DM2DCode = 1;
        public const float DM2DGrade = 2;
        public const float Text = 3;

        public const float Text_GTIN = 3.1f;
        public const float Text_LOT = 3.2f;
        public const float Text_EXPIRY = 3.3f;

        public const float Text_UID = 3.4f;
        public const float Text_PRODCODE = 3.5f;

        public const float Text_MFG = 3.6f;
        public const float Text_XtraLine1 = 3.7f;
        public const float Text_XtraLine2 = 3.8f;
        public const float Text_XtraLine3 = 3.9f;
        public const float Text_XtraLine4 = 3.11f;
        public const float Text_XtraLine5 = 3.12f;

        public const float Text_XtraLine6 = 3.13f;
        public const float Text_XtraLine7 = 3.14f;
        public const float Text_XtraLine8 = 3.15f;
        public const float Text_XtraLine9 = 3.16f;
        public const float Text_XtraLine10 = 3.17f;

        public const float BC_PI = 4f;
        public const float BC_SSCC = 5f;
        public const float BC_UID = 6f;
        public const float XtraID1 = 7.1f;
        public const float XtraID2 = 7.2f;

        public const float NONE = -1f;

        public static string RetTypeDesc(float Type)
        {
            string reason = string.Empty;

            if (InspectionFailed.DM2DCode == Type)
                reason = "2D";

            else if (InspectionFailed.DM2DGrade == Type)
                reason = "2D Grade";

            else if (InspectionFailed.Text == Type)
                reason = "Text";

            else if (InspectionFailed.Text_EXPIRY == Type)
                reason = "EXPIRY";

            else if (InspectionFailed.Text_GTIN == Type)
                reason = "GTIN";

            else if (InspectionFailed.Text_LOT == Type)
                reason = "LOT";

            else if (InspectionFailed.Text_MFG == Type)
                reason = "MFG";

            else if (InspectionFailed.Text_UID == Type)
                reason = "UID";

            else if (InspectionFailed.Text_PRODCODE == Type)
                reason = "PRODUCT CODE";
            else if (InspectionFailed.BC_PI == Type)
                reason = "PRODUCT BARCODE";

            else if (InspectionFailed.BC_SSCC == Type)
                reason = "SSCC BARCODE";

            else if (InspectionFailed.BC_UID == Type)
                reason = "UID BARCODE";

            else if (InspectionFailed.Text_XtraLine1 == Type)
                reason = "Extra Line 1";

            else if (InspectionFailed.Text_XtraLine2 == Type)
                reason = "Extra Line 2";

            else if (InspectionFailed.Text_XtraLine3 == Type)
                reason = "Extra Line 3";

            else if (InspectionFailed.Text_XtraLine4 == Type)
                reason = "Extra Line 4";

            else if (InspectionFailed.Text_XtraLine5 == Type)
                reason = "Extra Line 5";

            else if (InspectionFailed.Text_XtraLine6 == Type)
                reason = "Extra Line 6";
            else if (InspectionFailed.Text_XtraLine7 == Type)
                reason = "Extra Line 7";

            else if (InspectionFailed.Text_XtraLine8 == Type)
                reason = "Extra Line 8";

            else if (InspectionFailed.Text_XtraLine9 == Type)
                reason = "Extra Line 9";
            else if (InspectionFailed.Text_XtraLine10 == Type)
                reason = "Extra Line 10";

            else if (InspectionFailed.XtraID1 == Type)
                reason = "Extra ID Code 1";

            else if (InspectionFailed.XtraID2 == Type)
                reason = "Extra ID Code 2";

            return reason;
        }
    }

    /// <summary>
    /// GS1 Separator setting
    /// </summary>
    public enum FNC1Char
    {
        Default = 0,
        Gs1,
        Nothing
    }

    public enum UIDSource
    {
        PTPLGEN,
        UIDStore,
        XLX,
        CSV,
        XML
    }

    public static class ChallenegTest
    {
        public static string SubstringBefore(this string source, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            int index = compareInfo.IndexOf(source, value, CompareOptions.Ordinal);
            if (index < 0)
            {
                //No such substring
                return string.Empty;
            }
            return source.Substring(0, index);
        }
    }

    public class Globals
    {
        // by tushar for Cam delay
        public static Int32 Cam_Delay;
        public static Int32 TempMcStopDelay;

        public static bool IsBatchForcefullyClosed = false;

        public static decimal SerUserId = 0; // 03.12.2015 
        public static decimal SerVerifyUserId = 0; // 03.12.2015 
        public static decimal CurrentFunUId = 0;
        public static decimal SerUserRoleId;
        public static bool IsOnlineMode;

        public static bool IsFunctionWork = false;
        public static bool IsAprovalForInspection = false;
        public static int flageMS = 0;
        public static string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static string Cipherkey = "PTPLCRYPTOSYS";
        public static string DAVAFilePath = "";
        public static int TotalPrintQuantity = 0;
        public static int GoodQty = 0;
        public static int BadQty = 0;
        public static int SurplusQty = 0;
        public static int BatchQty = 0;
        public static string JobToVerify = string.Empty;
        public static int JobLength = 0;
        public static int UserID;
        public static string JobType;
        public static List<string> Lst_Inspected = new List<string>();
        public static int Jid;
        public static int TransId;
        public static int TCount;
        public static decimal PAID;
        public static bool Has2CodeRead = false;
        public static string TertiaryPrintername = string.Empty;
        public static string language = "English";
        public static bool VerifyFlag = false;   //Add By Aparna for dgv null values
        public static bool HasLineConnect = false;
        public static bool IsPrintingRunning = false;
        public static bool IsSSCCLastReprint = false;
        public static bool HasPrinterReConnect = false;
        public static int NoReadCountFromJob = 0;
        ////public static int NewNoRead= 0;
        public static bool LastReadUIDIsValid = true;
        public static int CurrentTestCNT = 0;
        public static string ChallenegeTestResult = string.Empty; // Added by sunil for challenge Test Result [Pass/Fail]
        public static int UIDLength = 12;
        public static string LineCode = string.Empty;


        #region SETTINGS FILE AVAILABILTIY CHECK
        public static void CreateDeafaultFiles(bool FactoryReset)
        {
            if (Directory.Exists(SettingsPath.SettingDir) == false)
            {
                Directory.CreateDirectory(SettingsPath.SettingDir);
            }
            if (FactoryReset || File.Exists(SettingsPath.App) == false)
                AppSettings.WriteSettings();
            if (FactoryReset || File.Exists(SettingsPath.Vendor) == false)
                VendorSettings.WriteSettings();
            if (FactoryReset || File.Exists(SettingsPath.PwdSetting) == false)
                PasswordSettings.WriteSettings();
            if (FactoryReset || File.Exists(SettingsPath.PrintRptProjectSetting) == false)
                ReportPrintProjectSetting.WriteSetting();
            if (FactoryReset || File.Exists(SettingsPath.UIDSourceConfig) == false)
                UIDSourceConfig.WriteSetting();

            if (File.Exists(AppSettings.LogsPath + "logs.csv") == true)
            {
                try
                {
                    FileInfo f = new FileInfo(AppSettings.LogsPath + "logs.csv");
                    long s1 = f.Length;
                    if (s1 > 1024 * 1024)
                    {
                        string uniqTS = DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
                        System.IO.File.Move(AppSettings.LogsPath + "logs.csv", AppSettings.LogsPath + "logs_" + uniqTS + ".csv");
                    }
                }
                catch { }
            }
        }
        public static bool CheckFiles()
        {
            bool retval = true;
            string[] VarFiles = { 
                                    SettingsPath.Commnads,
                                    SettingsPath.LineConfig,
                                    SettingsPath.CRBasePrinterConfig,
                                    SettingsPath.DBConnection,
                                    SettingsPath.DBConnectionServer,
                                    SettingsPath.InspectionDevice,
                                    SettingsPath.PackBoxesSetup,
                                    SettingsPath.PrinterSetup,
                                    SettingsPath.PrintRptProjectSetting,
                                    SettingsPath.Vendor,
                                    //SettingsPath.VendorAppAsso
                                    ///,SettingsPath.Ejector,
                                    //SettingsPath.InspectionDevice
                                };

            foreach (string str in VarFiles)
            {
                if (File.Exists(str) == false)
                {
                    Trace.TraceWarning("{0},{1}", DateTime.Now.ToString(), "FILE MISSING : " + str + "\r\nPLEASE CONTACT VENDOR.");
                    if (retval == true)
                        retval = false;
                }
            }
            return retval;
        }
        #endregion SETTINGS FILE AVAILABILTIY CHECK

        #region OS VARIFICATION
        public enum OSVer
        {
            WINNT3,
            WINNT4,
            WIN2000,
            WINXP,
            WIN7,
            WINOLD
        };
        public static OSVer GetVersion()
        {
            System.OperatingSystem osInfo = System.Environment.OSVersion;
            switch (osInfo.Platform)
            {
                case System.PlatformID.Win32Windows:
                    {
                        // Code to determine specific version of Windows 95, 
                        // Windows 98, Windows 98 Second Edition, or Windows Me.
                    }
                    break;
                case System.PlatformID.Win32NT:
                    {
                        switch (osInfo.Version.Major)
                        {
                            case 3:
                                return OSVer.WINNT3;
                            case 4:
                                return OSVer.WINNT4;
                            case 5:
                                if (osInfo.Version.Minor == 0)
                                    return OSVer.WINNT4;
                                else
                                    return OSVer.WINXP;
                            case 6:
                                return OSVer.WIN7;
                        } break;

                    }

            }
            return OSVer.WINOLD;
        }
        #endregion OS VARIFICATION

        #region FACTORY SETTINGS / USER SETTTINGS / JOB CONFIGURATION SPECIFICS

        public class AppSettings
        {
            //System Specific

            public static bool Zimba = false;
            public static int HWRecheckDuration = 1;
            public static bool CameraEjectionON = true;
            public static bool IsSaveAsCamJob = false;
            public static string DataDumpPath = "D:\\TnTDATA\\";
            //public static bool IsSaveAsCamJob = false;
            //public static bool IsSaveAsCamJob = false;
            public static bool IsShowPrintReports = false;
            public static bool IsShowPrintedCount = false;
            public static bool IsShowUIDListReport = false;
            public static int LastLoggedInUserId = 0;
            internal static string LogsPath = "D:\\TnTLOGS\\";
            public static int RetriveJobFileCount = 10;
            public static int McStopDelay = 0;

            //Users Configuration
            public static int WrongImgCount = 250;      // -1:NOIMAGE, 0:EACHIMAGE, n:nIMAGES
            public static int AnalysisImgCount = -1;    // -1:NOIMAGE, 0:EACHIMAGE, n:nIMAGES
            public static bool IsUnlockApprovalNeeded = false;
            public static string DateDisplayFormat = "dd/MMM/yyyy";
            public static bool IsTertiaryPrintingWithZPL = false;
            public static bool IsTertiaryPrintingWithImaje5800 = true;//Added by Arvind on 25.07.2015
            public static bool IsMcStopOnPCMapCount = false;
            public static string PCStationNo = "1";
            public static string PackLevel = "0";
            public static int AutoLogOutTime = 5;       // Added by Ansuman on 20/09/2015
            public static bool IPCAHANDLESCANNER = false; // By tushar for Ipca Handle scanner
            public static bool IsSSCCUsed = false;
            public static int LooseShiperCnt = 0;
            /// <summary>
            /// JOB config Information.
            /// </summary>
            /// <param name="ProdUID"></param>
            /// <returns></returns>

            //Printing & GS1 2D Code Management
            public static bool IsUseExpiryDay = false;
            public static string DefaultGs1LabelFilter = "GS12D1-01-17-10-21|GTIN|EXP|LOT|UID"; // For Product default filter by Sunil [31.10.2015]
            public static string GS1FilterWithUID = "GS12D1-01-11-17-10-21|GTIN|MFG|EXP|LOT|UID";
            public static string GS1FilterWithOutUID = "GS12D1-01-11-17-10|GTIN|MFG|EXP|LOT";
            public static string ExpiryDayTextFormat = "yyMMdd";
            public static bool HasAllCapsText = false;
            public static string SoftwareVersion = "1.0.0.0"; // SOFTWARE VERSION [SUNIL 31.10.2015]
            //public static int JobTypeSize_DATA1 = 60;
            //public static int JobTypeSize_DATA2 = 60;


            //NEW JOB CREATION: Default setting parameters
            public static bool PrintWithUID = true;
            public static bool AutoBatchClosure = true;
            public static int LotBatchSize_NoOfChars = 0;   // Total Number of Chars for LotBatch. 0 for no size restrictions
            public static bool AutoJobName = true;
            public static string JobNameFormat = "ProdCode#BatchNo";
            public static int MinBatchExpiryGAP = 12;       //in month
            public static int extra_uid_percentage = 10;
            public static int MaxJobLimit = 20;
            public static int PreviousPrintCount = 0; // [10.01.2015 by sunil]
            public static int PreviousSurPlusCount = 0; // [10.01.2015 by sunil]
            public static bool UseOracleSCMView = false;
            public static bool LineLevelReport = false; // by tushar for line level report 
            public static bool IsUseLabelGeneratorAtLine = false; // Label Generator at line level
            public static int PrinterConnectionCheckInterval = 30;
            public static int ProductSplit = 26;
            public static bool AllowChallengeTest = false; // FOR CHALLENGE TEST [SUNIL 22.12.2015]
            public static bool AllowOuterLoosePrint = false; // FOR LOOSE OUTER PRINTING [SUNIL 29.09.2016]
            public static bool AllowDatabaseBackup = false;
            public static bool AllowNotVerifiedUID = false; // By Ansuman 22/09/2015
            public static bool AllowInspectionOnOffForOperator = true; // FOR OPERATOR WHETHER TO ALLOW INSPECTION ON/OFF [Sunil 07.10.2016]
            public static bool HasGetTCPDataFromIC = false; // Added By Sagar for Getting Camera Data from TCP port
            public static bool HasMachineStartWithTimer = false; // Added By Sagar for Stop conveyor after PC Mapping

            // For Usage Help - NOT TO BE SAVED
            public static string JobNameFormat_STD = "ProdCode#BatchNo";
            public static string JobNameFormat_ALL = "ProdCode#BatchNo#DeckCode#Date:dd-MMM-yyyy#Time:hh-mm tt#DateTime:dd-MMM-yyyy hh-mm tt#ProdName#BatchQty";

            public static string DateFrmtInfo = " PLEASE USE UPPERCASE LETTER 'M' TO SPECIFY MONTH FORMAT. e.g: MMM or MM";
            public static bool ReadSettings(string AppStartUpPath, string ProdUID)
            {
                RedSysIntegrity.AppStartUpPath = AppStartUpPath;
                return ReadSettings(ProdUID);
            }
            public static bool ReadSettings(string ProdUID)
            {
                if (RedSysIntegrity.CheckIntegrity(ProdUID) == false)
                {
                    System.Windows.Forms.MessageBox.Show("FAILED TO INITIALIZE THE SYSTEM.", "SETUP FAILURE");
                    Trace.Assert(false, "FAILED TO INITIALIZE THE SYSTEM.", "SETUP FAILURE");
                    return false;
                }
                IniFile ini = new IniFile(SettingsPath.App);
                try
                {
                    AppSettings.Zimba = bool.Parse(ini.ReadValue("FACTORY_SETTINGS", "Zimba", AppSettings.Zimba.ToString()));
                    AppSettings.HWRecheckDuration = ini.ReadValue("FACTORY_SETTINGS", "HWRecheckDuration", AppSettings.HWRecheckDuration);
                    AppSettings.CameraEjectionON = bool.Parse(ini.ReadValue("FACTORY_SETTINGS", "CameraEjectionON", AppSettings.CameraEjectionON.ToString()));
                    AppSettings.IsSaveAsCamJob = bool.Parse(ini.ReadValue("FACTORY_SETTINGS", "IsSaveAsCamJob", AppSettings.IsSaveAsCamJob.ToString()));
                    AppSettings.DataDumpPath = ini.ReadValue("FACTORY_SETTINGS", "DataDumpPath", AppSettings.DataDumpPath);
                    if (Directory.Exists(AppSettings.DataDumpPath) == false)
                        Directory.CreateDirectory(AppSettings.DataDumpPath);
                    if (Directory.Exists(AppSettings.LogsPath) == false)
                        Directory.CreateDirectory(AppSettings.LogsPath);

                    AppSettings.IsShowPrintReports = bool.Parse(ini.ReadValue("FACTORY_SETTINGS", "IsShowPrintReports", AppSettings.IsShowPrintReports.ToString()));
                    AppSettings.IsShowPrintedCount = bool.Parse(ini.ReadValue("FACTORY_SETTINGS", "IsShowPrintedCount", AppSettings.IsShowPrintedCount.ToString()));
                    AppSettings.IsShowUIDListReport = bool.Parse(ini.ReadValue("FACTORY_SETTINGS", "IshowUIDListReport", AppSettings.IsShowUIDListReport.ToString()));
                    AppSettings.LastLoggedInUserId = ini.ReadValue("FACTORY_SETTINGS", "LastLoggedInUserId", AppSettings.LastLoggedInUserId);

                    AppSettings.WrongImgCount = ini.ReadValue("USER_SETTINGS", "WrongImagesCount", AppSettings.WrongImgCount);
                    AppSettings.AnalysisImgCount = ini.ReadValue("USER_SETTINGS", "AnalysisImageCount", AppSettings.AnalysisImgCount);
                    AppSettings.IsUnlockApprovalNeeded = bool.Parse(ini.ReadValue("USER_SETTINGS", "IsUnlockApprovalNeeded", AppSettings.IsUnlockApprovalNeeded.ToString()));
                    AppSettings.DateDisplayFormat = ini.ReadValue("USER_SETTINGS", "DateDisplayFormat", AppSettings.DateDisplayFormat);
                    AppSettings.IsTertiaryPrintingWithZPL = bool.Parse(ini.ReadValue("USER_SETTINGS", "IsTertiaryPrintingWithZPL", AppSettings.IsTertiaryPrintingWithZPL.ToString()));
                    AppSettings.IsTertiaryPrintingWithImaje5800 = bool.Parse(ini.ReadValue("USER_SETTINGS", "IsTertiaryPrintingWithImaje5800", AppSettings.IsTertiaryPrintingWithImaje5800.ToString()));//Added by Arvind on 25.07.2015
                    AppSettings.IsMcStopOnPCMapCount = bool.Parse(ini.ReadValue("USER_SETTINGS", "IsMcStopOnPCMapCount", AppSettings.IsMcStopOnPCMapCount.ToString()));//Added by Arvind on 19.10.2015
                    AppSettings.HasGetTCPDataFromIC = bool.Parse(ini.ReadValue("USER_SETTINGS", "HasGetTCPDataFromIC", AppSettings.HasGetTCPDataFromIC.ToString()));//Added by Sagar on 27/05/2016
                    AppSettings.HasMachineStartWithTimer = bool.Parse(ini.ReadValue("USER_SETTINGS", "HasMachineStartWithTimer", AppSettings.HasMachineStartWithTimer.ToString()));//Added by Sagar on 09/07/2016
                    AppSettings.PrinterConnectionCheckInterval = ini.ReadValue("USER_SETTINGS", "PrinterConnectionCheckInterval", AppSettings.PrinterConnectionCheckInterval);

                    AppSettings.PCStationNo = ini.ReadValue("USER_SETTINGS", "PCStationNo", AppSettings.PCStationNo);
                    AppSettings.PackLevel = ini.ReadValue("USER_SETTINGS", "PackLevel", AppSettings.PackLevel);
                    AppSettings.AutoLogOutTime = ini.ReadValue("USER_SETTINGS", "AutoLogOutTime", AppSettings.AutoLogOutTime);  //Added by Ansuman on 20/09/2015
                    AppSettings.LooseShiperCnt = ini.ReadValue("USER_SETTINGS", "LooseShiperCnt", AppSettings.LooseShiperCnt);  //Added by Arvind 14.01.2016

                    AppSettings.RetriveJobFileCount = int.Parse(ini.ReadValue("USER_SETTINGS", "RetriveJobFileCount", AppSettings.RetriveJobFileCount.ToString()));
                    AppSettings.McStopDelay = int.Parse(ini.ReadValue("USER_SETTINGS", "McStopDelay", AppSettings.McStopDelay.ToString()));
                    AppSettings.IsUseLabelGeneratorAtLine = bool.Parse(ini.ReadValue("USER_SETTINGS", "IsUseLabelGeneratorAtLine", AppSettings.IsUseLabelGeneratorAtLine.ToString()));
                    AppSettings.AllowChallengeTest = bool.Parse(ini.ReadValue("USER_SETTINGS", "AllowChallengeTest", AppSettings.AllowChallengeTest.ToString())); // FOR CHALLENGE TEST [SUNIL 22.12.2015]
                    AppSettings.AllowOuterLoosePrint = bool.Parse(ini.ReadValue("USER_SETTINGS", "AllowOuterLoosePrint", AppSettings.AllowOuterLoosePrint.ToString()));  //Sunil 29.09.2016
                    AppSettings.AllowInspectionOnOffForOperator = bool.Parse(ini.ReadValue("USER_SETTINGS", "AllowInspectionOnOffForOperator", AppSettings.AllowInspectionOnOffForOperator.ToString()));


                    AppSettings.IsUseExpiryDay = bool.Parse(ini.ReadValue("APP_CONFIGURATION", "IsUseExpiryDay", AppSettings.IsUseExpiryDay.ToString()));
                    AppSettings.GS1FilterWithUID = ini.ReadValue("APP_CONFIGURATION", "GS1FilterWithUID", AppSettings.GS1FilterWithUID);
                    AppSettings.GS1FilterWithOutUID = ini.ReadValue("APP_CONFIGURATION", "GS1FilterWithOutUID", AppSettings.GS1FilterWithOutUID);
                    AppSettings.ExpiryDayTextFormat = ini.ReadValue("APP_CONFIGURATION", "ExpiryDayTextFormat", AppSettings.ExpiryDayTextFormat);
                    AppSettings.HasAllCapsText = bool.Parse(ini.ReadValue("APP_CONFIGURATION", "HasAllCapsText", AppSettings.HasAllCapsText.ToString()));
                    AppSettings.SoftwareVersion = ini.ReadValue("APP_CONFIGURATION", "SoftwareVersion", AppSettings.SoftwareVersion); // For Challenge Test [Sunil 22.12.2015]

                    AppSettings.PrintWithUID = bool.Parse(ini.ReadValue("JOB_CONFIGURATION", "PrintWithUID", AppSettings.PrintWithUID.ToString()));
                    AppSettings.AutoBatchClosure = bool.Parse(ini.ReadValue("JOB_CONFIGURATION", "AutoBatchClosure", AppSettings.AutoBatchClosure.ToString()));
                    AppSettings.LotBatchSize_NoOfChars = ini.ReadValue("JOB_CONFIGURATION", "LotBatchSize_NoOfChars", AppSettings.LotBatchSize_NoOfChars);
                    AppSettings.AutoJobName = bool.Parse(ini.ReadValue("JOB_CONFIGURATION", "AutoJobName", AppSettings.AutoJobName.ToString()));
                    AppSettings.UseOracleSCMView = bool.Parse(ini.ReadValue("JOB_CONFIGURATION", "UseOracleSCMView", AppSettings.UseOracleSCMView.ToString()));
                    AppSettings.JobNameFormat = ini.ReadValue("JOB_CONFIGURATION", "JobNameFormat", AppSettings.JobNameFormat);
                    AppSettings.MinBatchExpiryGAP = ini.ReadValue("JOB_CONFIGURATION", "MinBatchExpiryGAP", AppSettings.MinBatchExpiryGAP);
                    AppSettings.extra_uid_percentage = ini.ReadValue("JOB_CONFIGURATION", "extra_uid_percentage", AppSettings.extra_uid_percentage);
                    AppSettings.MaxJobLimit = ini.ReadValue("JOB_CONFIGURATION", "MaxJobLimit", AppSettings.MaxJobLimit);
                    AppSettings.PreviousPrintCount = ini.ReadValue("JOB_CONFIGURATION", "PreviousPrintCount", AppSettings.PreviousPrintCount); // [10.01.2015 by sunil]
                    AppSettings.PreviousSurPlusCount = ini.ReadValue("JOB_CONFIGURATION", "PreviousSurPlusCount", AppSettings.PreviousSurPlusCount); // [13.01.2015 by sunil]
                    AppSettings.AllowNotVerifiedUID = bool.Parse(ini.ReadValue("JOB_CONFIGURATION", "AllowNotVerifiedUID", AppSettings.AllowNotVerifiedUID.ToString())); // [22.09.2015 by Ansuman]
                    AppSettings.LineLevelReport = bool.Parse(ini.ReadValue("JOB_CONFIGURATION", "LineLevelReport", AppSettings.LineLevelReport.ToString()));// by tushar for ipca handle scanner
                    AppSettings.IPCAHANDLESCANNER = bool.Parse(ini.ReadValue("JOB_CONFIGURATION", "IPCAHANDLESCANNER", AppSettings.IPCAHANDLESCANNER.ToString()));// by tushar for ipca handle scanner

                    return true;
                }
                catch
                {
                    //MessageBoxEx.Show("Error While Reading system files, Please contact vendor to fix problem..... Closing System",0);
                }
                return false;
            }
            public static void WriteSettings()
            {
                IniFile ini = new IniFile(SettingsPath.App);

                ini.WriteValue("FACTORY_SETTINGS", "Zimba", AppSettings.Zimba.ToString());
                ini.WriteValue("FACTORY_SETTINGS", "HWRecheckDuration", AppSettings.HWRecheckDuration);
                ini.WriteValue("FACTORY_SETTINGS", "CameraEjectionON", AppSettings.CameraEjectionON.ToString());
                ini.WriteValue("FACTORY_SETTINGS", "IsSaveAsCamJob", AppSettings.IsSaveAsCamJob.ToString());
                ini.WriteValue("FACTORY_SETTINGS", "DataDumpPath", AppSettings.DataDumpPath);

                ini.WriteValue("FACTORY_SETTINGS", "IsShowPrintReports", AppSettings.IsShowPrintReports.ToString());
                ini.WriteValue("FACTORY_SETTINGS", "IsShowPrintedCount", AppSettings.IsShowPrintedCount.ToString());
                ini.WriteValue("FACTORY_SETTINGS", "IshowUIDListReport", AppSettings.IsShowUIDListReport.ToString());
                ini.WriteValue("FACTORY_SETTINGS", "LastLoggedInUserId", AppSettings.LastLoggedInUserId);

                ini.WriteValue("USER_SETTINGS", "WrongImagesCount", AppSettings.WrongImgCount);
                ini.WriteValue("USER_SETTINGS", "AnalysisImageCount", AppSettings.AnalysisImgCount);
                ini.WriteValue("USER_SETTINGS", "IsUnlockApprovalNeeded", AppSettings.IsUnlockApprovalNeeded.ToString());
                ini.WriteValue("USER_SETTINGS", "DateDisplayFormat", AppSettings.DateDisplayFormat);
                ini.WriteValue("USER_SETTINGS", "IsTertiaryPrintingWithZPL", AppSettings.IsTertiaryPrintingWithZPL.ToString());
                ini.WriteValue("USER_SETTINGS", "IsTertiaryPrintingWithImaje5800", AppSettings.IsTertiaryPrintingWithImaje5800.ToString());//Added by Arvind on 25.07.2015
                ini.WriteValue("USER_SETTINGS", "IsMcStopOnPCMapCount", AppSettings.IsMcStopOnPCMapCount.ToString());//Added by Arvind on 25.07.2015
                ini.WriteValue("USER_SETTINGS", "HasGetTCPDataFromIC", AppSettings.HasGetTCPDataFromIC.ToString());
                ini.WriteValue("USER_SETTINGS", "HasMachineStartWithTimer", AppSettings.HasMachineStartWithTimer.ToString());
                ini.WriteValue("USER_SETTINGS", "PrinterConnectionCheckInterval", AppSettings.PrinterConnectionCheckInterval.ToString());

                ini.WriteValue("USER_SETTINGS", "PCStationNo", AppSettings.PCStationNo);
                ini.WriteValue("USER_SETTINGS", "PackLevel", AppSettings.PackLevel);
                ini.WriteValue("USER_SETTINGS", "AutoLogOutTime", AppSettings.AutoLogOutTime);  // Added By Ansuman
                ini.WriteValue("USER_SETTINGS", "LooseShiperCnt", AppSettings.LooseShiperCnt);  // Added By Arvind

                ini.WriteValue("USER_SETTINGS", "RetriveJobFileCount", AppSettings.RetriveJobFileCount.ToString());
                ini.WriteValue("USER_SETTINGS", "McStopDelay", AppSettings.McStopDelay.ToString());
                ini.WriteValue("USER_SETTINGS", "IsUseLabelGeneratorAtLine", AppSettings.IsUseLabelGeneratorAtLine.ToString());
                ini.WriteValue("USER_SETTINGS", "AllowChallengeTest", AppSettings.AllowChallengeTest.ToString()); // For Challenge Test [Sunil 22.12.2015]AllowDatabaseBackup
                ini.WriteValue("USER_SETTINGS", "AllowDatabaseBackup", AppSettings.AllowDatabaseBackup.ToString()); // Arvind 19.01.2016
                ini.WriteValue("USER_SETTINGS", "AllowOuterLoosePrint", AppSettings.AllowOuterLoosePrint.ToString()); // FOR LOOSE OUTER [SUNIL 29.09.2016]
                ini.WriteValue("USER_SETTINGS", "AllowInspectionOnOffForOperator", AppSettings.AllowInspectionOnOffForOperator.ToString());


                ini.WriteValue("APP_CONFIGURATION", "IsUseExpiryDay", AppSettings.IsUseExpiryDay.ToString());
                ini.WriteValue("APP_CONFIGURATION", "GS1FilterWithUID", AppSettings.GS1FilterWithUID);
                ini.WriteValue("APP_CONFIGURATION", "GS1FilterWithOutUID", AppSettings.GS1FilterWithOutUID);
                ini.WriteValue("APP_CONFIGURATION", "ExpiryDayTextFormat", AppSettings.ExpiryDayTextFormat);
                ini.WriteValue("APP_CONFIGURATION", "HasAllCapsText", AppSettings.HasAllCapsText.ToString());
                ini.WriteValue("APP_CONFIGURATION", "SoftwareVersion", AppSettings.SoftwareVersion); // FOR CHALLENGE TEST [SUNIL 30.12.2015]

                ini.WriteValue("JOB_CONFIGURATION", "PrintWithUID", AppSettings.PrintWithUID.ToString());
                ini.WriteValue("JOB_CONFIGURATION", "AutoBatchClosure", AppSettings.AutoBatchClosure.ToString());
                ini.WriteValue("JOB_CONFIGURATION", "LotBatchSize_NoOfChars", AppSettings.LotBatchSize_NoOfChars);
                ini.WriteValue("JOB_CONFIGURATION", "AutoJobName", AppSettings.AutoJobName.ToString());
                ini.WriteValue("JOB_CONFIGURATION", "UseOracleSCMView", AppSettings.UseOracleSCMView.ToString());
                ini.WriteValue("JOB_CONFIGURATION", "JobNameFormat", AppSettings.JobNameFormat);
                ini.WriteValue("JOB_CONFIGURATION", "MinBatchExpiryGAP", AppSettings.MinBatchExpiryGAP);
                ini.WriteValue("JOB_CONFIGURATION", "extra_uid_percentage", AppSettings.extra_uid_percentage);
                ini.WriteValue("JOB_CONFIGURATION", "MaxJobLimit", AppSettings.MaxJobLimit);
                ini.WriteValue("JOB_CONFIGURATION", "PreviousPrintCount", AppSettings.PreviousPrintCount); //[10.01.2015 by sunil]
                ini.WriteValue("JOB_CONFIGURATION", "AllowNotVerifiedUID", AppSettings.AllowNotVerifiedUID.ToString()); //[10.01.2015 by sunil]
                ini.WriteValue("JOB_CONFIGURATION", "LineLevelReport", AppSettings.LineLevelReport.ToString()); // by tushar for ipca handle scanner
                ini.WriteValue("JOB_CONFIGURATION", "IPCAHANDLESCANNER", AppSettings.IPCAHANDLESCANNER.ToString()); // by tushar for ipca handle scanner

                ini.WriteValue("HELP", "JobNameFormatALL_PARAMETERS", AppSettings.JobNameFormat_ALL);
                ini.WriteValue("HELP", "JobNameFormatMANUAL_PARAMETERS", "#STD_PARAMETERS#|DATA(MANUAL_PARAMETERS)#");
                ini.WriteValue("HELP", "JobNameFormatMANUAL_PARAMETERS correct method EXAMPLE:", "#ProdCode#|VendorName#");
                ini.WriteValue("HELP", "JobNameFormatMANUAL_PARAMETERS incorrect method EXAMPLE:", "#Prod|VendorNameCode#");
                ini.WriteValue("HELP", "EXPIRY DAY TEXT FORMAT ", AppSettings.DateFrmtInfo);
            }
        }

        private static bool CheckDatabaseExists(SqlConnection tmpConn, string databaseName)
        {
            string sqlCreateDBQuery;
            bool result = false;

            try
            {
                //tmpConn = new SqlConnection("server=" + tmpConn.DataSource + ";Trusted_Connection=yes");

                sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name= '{0}'", databaseName);

                using (tmpConn)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();
                        int databaseID = (int)sqlCmd.ExecuteScalar();
                        tmpConn.Close();
                        result = (databaseID > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                result = false;
            }
            return result;
        }
        public static bool CheckSQLDB()
        {
            if (string.IsNullOrEmpty(DbConnectionConfig.mDbConfig.Database) == true)
                return false;

            SqlConnectionStringBuilder sqlconn = new SqlConnectionStringBuilder();
            sqlconn.DataSource = DbConnectionConfig.mDbConfig.DataSourcePath;
            sqlconn.UserID = DbConnectionConfig.mDbConfig.UserName;
            sqlconn.Password = Globals.SimpleEncrypt.Decrypt(DbConnectionConfig.mDbConfig.Password);
            sqlconn.InitialCatalog = DbConnectionConfig.mDbConfig.Database;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = sqlconn.ConnectionString;
            return CheckDatabaseExists(con, DbConnectionConfig.mDbConfig.Database);
        }
        public class SimpleEncrypt
        {
            static int key = 25;
            public static string Encrypt(string inString)
            {
                string outString = string.Empty;

                Random rnd = new Random(DateTime.Now.Millisecond);

                foreach (char ch in inString)
                {
                    char newCh = (char)(ch + key);
                    outString += newCh;
                    int rndNum = rnd.Next(60, 125);
                    char rndCh = (char)rndNum;
                    outString += rndCh;
                }
                return outString;
            }
            public static string Decrypt(string inString)
            {
                string outString = string.Empty;
                bool isSkip = false;
                foreach (char ch in inString)
                {
                    if (isSkip == false)
                    {
                        char newCh = (char)(ch - key);
                        outString += newCh;
                        isSkip = true;
                    }
                    else
                    {
                        isSkip = false;
                    }
                }
                return outString;
            }
        }

        #endregion FACTORY SETTINGS / USER SETTTINGS / JOB CONFIGURATION SPECIFICS

        #region VENDOR SETTINGS
        public class VendorSettings
        {
            public static bool IsStrictEjectorVerify = true;            // 
            public static bool IsShowCompanySettingsOption = true;      // True will display the option of Company Settings in AdminSettings Form to change Name & Address
            public static bool IsNewJobAvailable = true;                // True will display the option of NEW JOB creation on Start Job
            public static bool IsLabelStartIndexInBatchLoad = false;    // This is to start the Case Number from Specific number. Eg. Novartis Pakistan Shipment
            public static bool IsLabelReprintOnFail = true;             //This not used as of now.
            public static bool IsDecksPreviewAvail = false;             //This is used for Showing the Primary/Secondary/Tertiary Deck Preview. If this is false, ALL IS FALSE
            public static bool IsJet2DecksPreviewAvail = false;             //This is used for Showing the JET2 Data in preview
            public static bool IsXMLExportAvail = false;                //This will display the XML Export buttons on Report Form
            public static bool LGR_DataPrintedAlwaysVerified = false;   //This is used to set Default status of UID as Verified or Not For Lable GEnerator.

            public static int ReportDispDateDiff = 30;                  //This is used to Display records of job creation date by using date diff from Cur date...

            public static string SUPERUSER = "1$831$85";
            public static bool IsMultideckLableGenerator = true;

            public static string SUP_REAPI_VER = "3.2";
            public static string JOBGRPReaJet = "Back";
            public static bool IsReplaceDot4MRP = false;
            public static bool ExtraLineInspExist = false;
            public static bool ExtraIDCodeInspExist = false;
            public static bool ShowReports = true;
            public static bool IsCondotCBPrinter = false;
            public static bool IsVideoJetPrinter = false;
            public static int CondotCBBufferSize = 1;
            public static FNC1Char fnc1CharType = FNC1Char.Default;
            public static bool IsUseReprintForChinaCode = false;
            public static string LanguageToUse = "English";
            public static void ReadSettings()
            {
                IniFile ini = new IniFile(SettingsPath.Vendor);
                VendorSettings.IsStrictEjectorVerify = bool.Parse(ini.ReadValue("VENDOR", "IsStrictEjectorVerify", VendorSettings.IsStrictEjectorVerify.ToString()));
                VendorSettings.IsShowCompanySettingsOption = bool.Parse(ini.ReadValue("VENDOR", "IsShowCompanySettingsOption", VendorSettings.IsShowCompanySettingsOption.ToString()));
                VendorSettings.IsNewJobAvailable = bool.Parse(ini.ReadValue("VENDOR", "IsNewJobAvailable", VendorSettings.IsNewJobAvailable.ToString()));
                VendorSettings.IsLabelStartIndexInBatchLoad = bool.Parse(ini.ReadValue("VENDOR", "IsLabelStartIndexInBatchLoad", VendorSettings.IsLabelStartIndexInBatchLoad.ToString()));
                VendorSettings.IsDecksPreviewAvail = bool.Parse(ini.ReadValue("VENDOR", "IsDecksPreviewAvail", VendorSettings.IsDecksPreviewAvail.ToString()));
                VendorSettings.IsJet2DecksPreviewAvail = bool.Parse(ini.ReadValue("VENDOR", "IsJet2DecksPreviewAvail", VendorSettings.IsJet2DecksPreviewAvail.ToString()));
                VendorSettings.IsXMLExportAvail = bool.Parse(ini.ReadValue("VENDOR", "IsXMLExportAvail", VendorSettings.IsXMLExportAvail.ToString()));
                VendorSettings.IsReplaceDot4MRP = bool.Parse(ini.ReadValue("VENDOR", "IsReplaceDot4MRP", VendorSettings.IsReplaceDot4MRP.ToString()));
                VendorSettings.ExtraLineInspExist = bool.Parse(ini.ReadValue("VENDOR", "ExtraLineInspExist", VendorSettings.ExtraLineInspExist.ToString()));
                VendorSettings.ExtraIDCodeInspExist = bool.Parse(ini.ReadValue("VENDOR", "ExtraIDCodeInspExist", VendorSettings.ExtraIDCodeInspExist.ToString()));
                VendorSettings.LGR_DataPrintedAlwaysVerified = bool.Parse(ini.ReadValue("VENDOR", "LGR_DataPrintedAlwaysVerified", VendorSettings.LGR_DataPrintedAlwaysVerified.ToString()));
                VendorSettings.IsCondotCBPrinter = bool.Parse(ini.ReadValue("VENDOR", "IsCondotCBPrinter", VendorSettings.IsCondotCBPrinter.ToString()));
                VendorSettings.CondotCBBufferSize = int.Parse(ini.ReadValue("VENDOR", "CondotCBBufferSize", VendorSettings.CondotCBBufferSize.ToString()));
                VendorSettings.IsUseReprintForChinaCode = bool.Parse(ini.ReadValue("VENDOR", "IsUseReprintForChinaCode", VendorSettings.IsUseReprintForChinaCode.ToString()));
                VendorSettings.ReportDispDateDiff = ini.ReadValue("VENDOR", "ReportDispDateDiff", VendorSettings.ReportDispDateDiff);
                VendorSettings.ShowReports = bool.Parse(ini.ReadValue("VENDOR", "ShowReports", VendorSettings.ShowReports.ToString()));
                Globals.language = VendorSettings.LanguageToUse = ini.ReadValue("VENDOR", "LanguageToUse", VendorSettings.LanguageToUse);

                VendorSettings.fnc1CharType = (FNC1Char)Enum.Parse(typeof(FNC1Char), ini.ReadValue("VENDOR", "fnc1CharType", VendorSettings.fnc1CharType.ToString()));

                VendorSettings.SUP_REAPI_VER = ini.ReadValue("VENDOR", "SUP_REAPI_VER", VendorSettings.SUP_REAPI_VER);
                VendorSettings.JOBGRPReaJet = ini.ReadValue("VENDOR", "JOBGRPReaJet", VendorSettings.JOBGRPReaJet);

                VendorSettings.SUPERUSER = ini.ReadValue("VENDOR", "SUPERUSER", VendorSettings.SUPERUSER);
                VendorSettings.SUPERUSER = GetStringFromTermData(VendorSettings.SUPERUSER);
            }
            public static void WriteSettings()
            {
                IniFile ini = new IniFile(SettingsPath.Vendor);
                ini.WriteValue("VENDOR", "IsStrictEjectorVerify", VendorSettings.IsStrictEjectorVerify.ToString());
                ini.WriteValue("VENDOR", "IsShowCompanySettingsOption", VendorSettings.IsShowCompanySettingsOption.ToString());
                ini.WriteValue("VENDOR", "IsNewJobAvailable", VendorSettings.IsNewJobAvailable.ToString());
                ini.WriteValue("VENDOR", "IsLabelStartIndexInBatchLoad", VendorSettings.IsLabelStartIndexInBatchLoad.ToString());
                ini.WriteValue("VENDOR", "IsDecksPreviewAvail", VendorSettings.IsDecksPreviewAvail.ToString());
                ini.WriteValue("VENDOR", "IsJet2DecksPreviewAvail", VendorSettings.IsJet2DecksPreviewAvail.ToString());
                ini.WriteValue("VENDOR", "IsXMLExportAvail", VendorSettings.IsXMLExportAvail.ToString());
                ini.WriteValue("VENDOR", "IsReplaceDot4MRP", VendorSettings.IsReplaceDot4MRP.ToString());
                ini.WriteValue("VENDOR", "ExtraLineInspExist", VendorSettings.ExtraLineInspExist.ToString());
                ini.WriteValue("VENDOR", "ExtraIDCodeInspExist", VendorSettings.ExtraIDCodeInspExist.ToString());
                ini.WriteValue("VENDOR", "LGR_DataPrintedAlwaysVerified", VendorSettings.LGR_DataPrintedAlwaysVerified.ToString());
                ini.WriteValue("VENDOR", "IsCondotCBPrinter", VendorSettings.IsCondotCBPrinter.ToString());
                ini.WriteValue("VENDOR", "CondotCBBufferSize", VendorSettings.CondotCBBufferSize.ToString());
                ini.WriteValue("VENDOR", "IsUseReprintForChinaCode", VendorSettings.IsUseReprintForChinaCode.ToString());
                ini.WriteValue("VENDOR", "ReportDispDateDiff", VendorSettings.ReportDispDateDiff.ToString());
                ini.WriteValue("VENDOR", "ShowReports", VendorSettings.ShowReports.ToString());
                ini.WriteValue("VENDOR", "LanguageToUse", VendorSettings.LanguageToUse);

                ini.WriteValue("VENDOR", "fnc1CharType", VendorSettings.fnc1CharType.ToString());

                ini.WriteValue("VENDOR", "SUPERUSER", VendorSettings.SUPERUSER);
                ini.WriteValue("VENDOR", "SUP_REAPI_VER", VendorSettings.SUP_REAPI_VER);
                ini.WriteValue("VENDOR", "JOBGRPReaJet", VendorSettings.JOBGRPReaJet);
                ini.WriteValue("fnc1CharType_HELP", "fnc1CharType_HELP", " Default(\x1D)=0 , Gs1(<GS>)=1 , Nothing=2");
            }
            private static string GetStringFromTermData(string terminalData)
            {
                string strDecr = string.Empty;

                string byteStr = string.Empty;
                for (int i = 0; i < terminalData.Length; i++)
                {
                    char ch = terminalData[i];
                    if (ch == '$')
                    {
                        byteStr = string.Empty;
                        i++; byteStr += terminalData[i];
                        i++; byteStr += terminalData[i];

                        int chVal = 0;
                        int.TryParse(byteStr, out chVal);

                        strDecr += ((char)chVal).ToString();
                    }
                    else
                    {
                        strDecr += ch.ToString();
                    }
                }
                return strDecr;
            }
        }
        #endregion VENDOR SETTINGS

        #region VendorAppAsso SETTINGS
        public class VendorAppAsso
        {
            private const int AppCode_NONE = 0;
            private const string AppName_PACKiTRACKnTRACE = "PACKi TRACKnTRACE";
            private const int AppCode_PACKiTRACKnTRACE = 1;
            private const string AppName_LABELGENERATOR = "LABELGENERATOR";
            private const int AppCode_LABELGENERATOR = 2;

            public static int AppCode = AppCode_NONE;
            public static string AppName = AppName_PACKiTRACKnTRACE;

            //public static void GetSetApp(string ProdUID)
            //{
            //    string ProdInstalled = RedUtils.RedSysIntegrity.GetIntegrity(ProdUID);
            //    if (ProdInstalled == RedUtils.RedProductUIDs.ProdUID_PACKiTRACKnTRACE)
            //    {
            //        Globals.VendorAppAsso.AppCode = AppCode_PACKiTRACKnTRACE;
            //        Globals.VendorAppAsso.AppName = Globals.VendorAppAsso.AppName_PACKiTRACKnTRACE;
            //    }
            //    else if (ProdInstalled == RedUtils.RedProductUIDs.ProdUID_PACKi)// ProdUID_LABELGENERATOR)
            //    {
            //        Globals.VendorAppAsso.AppCode = AppCode_LABELGENERATOR;
            //        Globals.VendorAppAsso.AppName = Globals.VendorAppAsso.AppName_LABELGENERATOR;
            //    }
            //}
            public static bool isPACKiTRACKnTRACE
            {
                get { return (AppCode == AppCode_PACKiTRACKnTRACE ? true : false); }
            }
            public static bool isLABELGENERATOR
            {
                get { return (AppCode == AppCode_LABELGENERATOR ? true : false); }
            }

        }
        #endregion VendorAppAsso SETTINGS

        #region PACK CARTON PALLATE SETTTINGS
        public class PackBoxesSettings
        {
            public static bool ReadSettings()
            {
                PackBoxes Pck = new PackBoxes();
                Pck.LoadPackBoxesSetup();
                return true;
            }
        }
        #endregion PACK CARTON PALLATE SETTTINGS

        #region PRINTER SETTINGS

        public class PrinterConfiguration
        {
            public static void ReadSettings()
            {
                REDTR.UTILS.PrinterSetup PrintConfig = new REDTR.UTILS.PrinterSetup();
                PrintConfig.LoadPrinterConfiguration();
            }
        }
        #endregion PRINTER SETTINGS

        #region CRBASEPRINTER SETTING [11.05.2015] BY SUNIL

        public class CRBasePrinterSetting
        {
            public static string Ip_Address = "192.168.10.11";
            public static string Port_Number = "21000";

            public static bool ReadSettings()
            {
                IniFile ini = new IniFile(SettingsPath.CRBasePrinterConfig);
                try
                {
                    CRBasePrinterSetting.Ip_Address = ini.ReadValue("COMMUNICATION", "IpAddress", CRBasePrinterSetting.Ip_Address.ToString());
                    CRBasePrinterSetting.Port_Number = ini.ReadValue("COMMUNICATION", "PortNumber", CRBasePrinterSetting.Port_Number.ToString());

                    return true;
                }
                catch (Exception)
                { }

                return false;
            }

        }

        #endregion

        #region CAMERA SETTINGS

        public class CameraConfiguration
        {
            public static bool ReadSettings()
            {
                InspectionDeviceSettings.LoadCameraSettings();
                return true;
            }
        }

        #endregion CAMERA SETTINGS

        #region PASSWORD SETTING
        public class PasswordSettings
        {
            public static int PwdLength = 8;
            public static int PWDType = 2;
            public static int RetryCount = 8;
            public static int PwdExpiryDur = 60;

            public static void WriteSettings()
            {
                IniFile ini = new IniFile(SettingsPath.PwdSetting);
                ini.WriteValue("PWD_SETTINGS", "PwdLength", PasswordSettings.PwdLength.ToString());
                ini.WriteValue("PWD_SETTINGS", "PWDType", PasswordSettings.PWDType.ToString());
                ini.WriteValue("PWD_SETTINGS", "RetryCount", PasswordSettings.RetryCount.ToString());
                ini.WriteValue("PWD_SETTINGS", "PwdExpiryDur", PasswordSettings.PwdExpiryDur.ToString());

                ini.WriteValue("PWD_SETTINGS_HELP", "Maximum password length", "10");
                ini.WriteValue("PWD_SETTINGS_HELP", "PWDTypeInfo", "0-Only Numeric,1-Only Alpha,2-AplphaNumeric");

            }
            public static bool ReadSettings()
            {
                IniFile ini = new IniFile(SettingsPath.PwdSetting);
                try
                {
                    PasswordSettings.PwdLength = Convert.ToInt16(ini.ReadValue("PWD_SETTINGS", "PwdLength", PasswordSettings.PwdLength.ToString()));
                    PasswordSettings.PWDType = Convert.ToInt16(ini.ReadValue("PWD_SETTINGS", "PWDType", PasswordSettings.PWDType.ToString()));
                    PasswordSettings.RetryCount = Convert.ToInt16(ini.ReadValue("PWD_SETTINGS", "RetryCount", PasswordSettings.RetryCount.ToString()));
                    PasswordSettings.PwdExpiryDur = Convert.ToInt16(ini.ReadValue("PWD_SETTINGS", "PwdExpiryDur", PasswordSettings.PwdExpiryDur.ToString()));
                    return true;

                }
                catch
                {
                }
                return false;
            }
        }
        #endregion

        #region REPORTPRINTPROJECT SETTING
        public class ReportPrintProjectSetting
        {
            public static int QntToPrint = 100;
            public static string QTYPostLabel = "DOSES";
            public static bool IsPassBlankLbl = false;
            public static void WriteSetting()
            {
                IniFile ini = new IniFile(SettingsPath.PrintRptProjectSetting);
                ini.WriteValue("ReportPrintProjectSetting", "QntToPrint", ReportPrintProjectSetting.QntToPrint.ToString());
                ini.WriteValue("ReportPrintProjectSetting", "QTYPostLabel", ReportPrintProjectSetting.QTYPostLabel);
                ini.WriteValue("ReportPrintProjectSetting", "IsPassBlankLbl", ReportPrintProjectSetting.IsPassBlankLbl.ToString());
            }
            public static bool ReadSetting()
            {
                IniFile ini = new IniFile(SettingsPath.PrintRptProjectSetting);
                try
                {
                    ReportPrintProjectSetting.QntToPrint = Convert.ToInt16(ini.ReadValue("ReportPrintProjectSetting", "QntToPrint", ReportPrintProjectSetting.QntToPrint.ToString()));
                    ReportPrintProjectSetting.QTYPostLabel = ini.ReadValue("ReportPrintProjectSetting", "QTYPostLabel", ReportPrintProjectSetting.QTYPostLabel.ToString());
                    ReportPrintProjectSetting.IsPassBlankLbl = bool.Parse(ini.ReadValue("ReportPrintProjectSetting", "IsPassBlankLbl", ReportPrintProjectSetting.IsPassBlankLbl.ToString()));
                    return true;
                }
                catch
                {
                }
                return false;
            }
        }
        #endregion REPORTPRINTPROJECT SETTING

        #region UIDSOURCE SETTING
        public class UIDSourceConfig
        {
            public static UIDSource UIDSourceType = UIDSource.PTPLGEN;
            // UIDSourceLoc - Folder Path for storing the various deck XMLs
            public static string UIDSourceLoc_PACK = @"D:\TnTXMLs\PACK";
            public static string UIDSourceLoc_SHIPPER = @"D:\TnTXMLs\SHIPPER";

            public static void WriteSetting()
            {
                IniFile ini = new IniFile(SettingsPath.UIDSourceConfig);
                ini.WriteValue("UIDSourceConfig", "UIDSourceType", UIDSourceConfig.UIDSourceType.ToString());
                ini.WriteValue("UIDSourceConfig", "UIDSourceLoc_PACK", UIDSourceConfig.UIDSourceLoc_PACK.ToString());
                ini.WriteValue("UIDSourceConfig", "UIDSourceLoc_SHIPPER", UIDSourceConfig.UIDSourceLoc_SHIPPER.ToString());

                ini.WriteValue("UIDSourceConfig_HELP", "UIDSource1", UIDSource.PTPLGEN.ToString());
                ini.WriteValue("UIDSourceConfig_HELP", "UIDSource2", UIDSource.UIDStore.ToString());
                ini.WriteValue("UIDSourceConfig_HELP", "UIDSource3", UIDSource.XLX.ToString());
                ini.WriteValue("UIDSourceConfig_HELP", "UIDSource4", UIDSource.CSV.ToString());
                ini.WriteValue("UIDSourceConfig_HELP", "UIDSource5", UIDSource.XML.ToString());
                ini.WriteValue("UIDSourceConfig_HELP", "UIDLength", UIDLength);
            }
            public static bool ReadSetting()
            {
                IniFile ini = new IniFile(SettingsPath.UIDSourceConfig);
                try
                {
                    try
                    {
                        UIDLength = Convert.ToInt16(ini.ReadValue("UIDSourceConfig_HELP", "UIDLength", UIDLength.ToString()));
                        string uidsrc = ini.ReadValue("UIDSourceConfig", "UIDSourceType", UIDSourceConfig.UIDSourceType.ToString());
                        UIDSourceConfig.UIDSourceType = (UIDSource)Enum.Parse(typeof(UIDSource), uidsrc, true);
                    }
                    catch { }

                    UIDSourceConfig.UIDSourceLoc_PACK = ini.ReadValue("UIDSourceConfig", "UIDSourceLoc_PACK", UIDSourceConfig.UIDSourceLoc_PACK.ToString());
                    UIDSourceConfig.UIDSourceLoc_SHIPPER = ini.ReadValue("UIDSourceConfig", "UIDSourceLoc_SHIPPER", UIDSourceConfig.UIDSourceLoc_SHIPPER.ToString());

                    return true;
                }
                catch
                {
                }
                return false;
            }
        }
        #endregion UIDSOURCE SETTING

        #region MISC INFO SETTING
        public class MiscInfo
        {
            public static string MiscStr1 = string.Empty;
            public static string MiscStr2 = string.Empty;
            public static string MiscStr3 = string.Empty;
            public static string MiscStr4 = string.Empty;
            public static string MiscStr5 = string.Empty;

            public static int MiscNum1 = 0;
            public static int MiscNum2 = 0;
            public static int MiscNum3 = 0;
            public static int MiscNum4 = 0;
            public static int MiscNum5 = 0;
        }
        #endregion MISC INFO SETTING

        #region Active Directory Settings [16.09.2015] by sunil.

        public class ADUsers
        {
            public string Email { get; set; }
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public bool isMapped { get; set; }
        }

        public class ADUserInfo
        {
            public static string ADDomainPath = "LDAP://DC=xxxx,DC=in";
            public static string User = "Admin";
            public static string Password = "@p";
            static bool IsServerReachable = false;

            public static bool ReadSetting()
            {
                IniFile ini = new IniFile(SettingsPath.ADConfig);
                try
                {
                    try
                    {
                        string ADsrc = ini.ReadValue("ADConfig", "DomainPath", ADUserInfo.ADDomainPath.ToString());
                        ADUserInfo.ADDomainPath = ADsrc;
                        ADUserInfo.User = ini.ReadValue("ADConfig", "ADUserName", ADUserInfo.ADDomainPath.ToString());
                        ADUserInfo.Password = ini.ReadValue("ADConfig", "ADPassword", ADUserInfo.ADDomainPath.ToString());
                    }
                    catch { }
                    return true;
                }
                catch
                { }
                return false;
            }

        #endregion

            //#region LANGUAGE SETTING

            //public class LanguageConfiguration
            //{
            //    public static bool LanguageSettings()
            //    {
            //        try
            //        {
            //            FileStream fr = new FileStream(SettingsPath.LanguageSelection, FileMode.Open, FileAccess.Read);
            //            StreamReader sr = new StreamReader(fr);

            //            language = sr.ReadLine();
            //            return true;

            //        }
            //        catch
            //        {
            //        }
            //        return false;
            //    }
            //}

            //#endregion LANGUAGE SETTING
        }

    }
}
