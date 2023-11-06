
namespace REDTR.UTILS
{
    public static class RoleName
    {
        public const string Admin = "Administrator";
        public const string Supervisor = "Supervisor";
        public const string Operator = "Operator";
        public const string QA = "QA";
        public const string SUPERUSER = "SUPERUSER";

    }

    // By Tushar
    public static class USerTrailWHO
    {
        public const string Admin = "Administrator";
        public const string Supervisor = "Supervisor";
        public const string Operator = "Operator";
        public const string QA = "QA";
        public const string SUPERUSER = "SUPERUSER";
    }

    public static class USerTrailWHAT
    {
        // REPORT SIDE OPERATION [06.10.2016]
        public const string JOB_REPORT = "GENERAL(JOB/PRODUCT) REPORT ACCESS";
        public const string AUDITTRAIL_REPPORT = "AUDIT TRAIL REPORT ACCESS";
        public const string UIDVALIDATION_REPORT = "UID VALIDATION REPORT ACCESS";
        public const string PRODUCTDETAILS_REPORT = "PRODUCT DETAILS REPORT ACESS";

        public const string JOB_REPORTEXIT = "EXIT FROM GENERAL(JOB/PRODUCT) REPORT";
        public const string UIDVALIDATION_REPORTEXIT = "EXIT FROM UID VALIDATION REPORT";

        public const string SERVER_ACCESS = "SERVER SIDE ACCESS ";
        public const string LINE_ACCESS = "LINE SIDE ACCESS ";
        public const string FrmWelcomeExit = "EXITING WELCOME SCREEN";

        public const string USERTRAILSWSETTING = "CHECKING SOFTWARE SETTING ";
        public const string USERTRAILHWSETTING = "CHECKING HARDWARE SETTING ";
        public const string USERTRAILSWANDHWSETTING = "CHECKING SOFTWARE AND HARDWARE SETTING ";

        public const string LOGIN_FAILED = "LOGIN FAILED ";
        public const string ADMIN_LOGIN = "ADMIN LOGIN ";
        public const string QA_LOGIN = "QA LOGIN ";
        public const string UserLOGIN = "LOGIN ";
        public const string UserLogOut = "EXITING ";
        public const string UserAutoLogOut = "AUTO EXIT";
        public const string UserAutoLogIn = "AUTO LOGIN ";
        public const string DavaFileRegeneration = "FILE GENERATION FOR DAVA ";
        // Added by Anshuman
        public const string START_SYSTEM = "START SYSTEM ";
        public const string EXIT_SYSTEM = "EXIT SYSTEM ";
        public const string STOP_SYSTEM = "STOP SYSTEM ";
        public const string STOP_CHALLENGE_SYSTEM = "EXIT SYSTEM FROM CHALLENGE TEST MODE";
        public const string STOP_QA_SYSTEM = "EXIT SYSTEM FROM ONLINE QA SAMPLING MODE";
        public const string EXIT_MANAGER = "EXIT MANAGER ";
        public const string ACCEPTED_GRADE = "ACCEPTED GRADE ";

        public const string ONLINE_INSPECTION_MODE = "SYSTEM IS IN ONLINE INSPECTION MODE";

        public const string OPERATIONS_TIMEOUT = "OPERATIONS TIMEOUT ";

        public const string JOB_SETTINGS = "JOB SETTINGS ";
        public const string FUNCTION = "FUNCTIONING ";
        public const string PRODUCT_PACK_ASSOCIATION = "PRODUCT PACK ASSOCIATION ";
        public const string SETCAM_IMAGE = "SET CAMERA IMAGE ";
        public const string TUNE_FONT = "TUNE FONT ";
        public const string PRINT_FONT = "PRINT FONT TRAIN DATA ";
        public const string SET_ROI = "ROI SET DONE ";
        public const string SET_INSPONOFF = "OCR/GRADE INSPECTION ON/OFF ";

        public const string ADD_NEW_JOB = "ADDED NEW JOB: ";  // By Ansuman
        public const string MODIFY_JOB = "JOB MODIFIED: ";    // By Ansuman
        public const string VERIFY_JOB = "JOB VERIFIED: ";    // By Ansuman

        public const string LABLE_LAYOUT_DESIGNER = "LABLE LAYOUT DESIGNER ";
        public const string APPLICATION_SETTINGS = "APPLICATION SETTINGS ";
        public const string LOOSE_SHIPPER = "LOOSE SHIPPER";
        // END

        //public const string PRODUCT_PACK_ASSOCIATION = "PRODUCT PACK ASSOCIATION";

        public const string NEW_BATCH = "NEW BATCH ";
        public const string LOAD_BATCH = "LOAD BATCH ";
        public const string CLOSE_BATCH = "CLOSE BATCH ";
        public const string FINISH_BATCH = "FINISH BATCH ";
        public const string UID_END = "UID QUANTITY EXHAUSTED OF BATCH ";
        public const string FORCEFULLYFINISH_BATCH = "FORCEFULLY BATCH CLOSE ";
        public const string DECOMMISSION_BATCH = "DECOMMISSION BATCH ";
        public const string TERMINATE_BATCH = "TERMINATE BATCH ";
        public const string VERIFY_BATCH = "VERIFY BATCH ";

        public const string DELETE_JOB = "DELETE JOB ";

        public const string BACKUPnRESTORE = "BACKUP AND RESTORE ";
        public const string REJECTION_VERIFICATION = "REJECTION VERIFICATION ";
        public const string ONLINEREJECTION_VERIFICATION = "ONLINE REJECTION VERIFICATION "; // Sunil 18.10.2015
        public const string QA_SAMPLING = "QA SAMPLING ";
        public const string DECOMISSIONING_UID = "DECOMMISSIONING UID ";
        public const string REPLACE_ITEM = "REPLACE ITEM ";
        public const string LABLE_PRINT = "LABEL PRINT ";
        public const string LABLE_REPRINT = "LABEL REPRINT ";

        public const string CONVEYOR_VALIDATION = "CONVEYOR VALIDATION ";
        public const string ACCEPTANCE_VERIFICATION = "ACCEPT TO REJECT ";
        public const string EJECTION_VERIFICATIONFAILED  = "EJECTION VERIFICATION FAILED ";

        public const string CMPNY_SETTINGS = "COMPANY SETTINGS ";

        public const string DATA_SYNC = "DATA SYNC ";    // 08Sept2015 Sunil
        public const string NEW_LINE = "NEW LINE CREATED ";
        public const string UPDATE_LINE = "UPDATE LINE ";
        public const string DELETE_LINE = "DELETE LINE ";
        public const string CLEAR_LINE = "PERFORM LINE CLEARANCE ";
        public const string ALLOCATE_BATCH_TO_LINE = "ALLOCATE BATCH TO LINE "; //END

        public const string INSPECTION_ONOFF = "INSPECTION ON/OFF"; //END
        public const string INSPECTION_SETTING = "INSPECTION SETTING"; //END

        public const string ROLE_PERMISSION = "ROLES PERMISSION"; //END

        public const string CHLNG_TEST = "CHALLENGE TEST";

        public const string MACHINE_START = "MACHINE START"; // [28.09.2016 FOR AUDIT TRAIL ENTRY]


    }

    public static class USerTrailWHERE
    {
        public const string LogIn = "TRACK N TRACE LOGIN"; // Added BY Ansuman
        public const string LogOut = "TRACK N TRACE LOGOUT";
        public const string TnT = "TRACK N TRACE SYSTEM";
        public const string TnT1 = "TRACK N TRACE MANAGER";
        public const string TnTWelcome = "WELCOME SCREEN";
        public const string LABLE_GENERATOR = "LABEL GENERATOR SYSTEM";
    }

    public class USerTrailWHY
    {        
        public static string UNAUTHORIZED_ACCESS = "UNAUTHORIZED ACCESS";
        public static string FailedHwSwFailed = "CHECKING SOFTWARE AND HARDWARE SETTING FAILED";

        public static string ACCESS_TnTSYSTEM = "TO ACCESS TRACKnTRACE SYSTEM";
        public static string ACCESS_SYSTEM = "To ACCESS SYSTEM";
        public static string ACCESS_PRODUCT_PACK = "TO ACCESS PRODUCT PACK ASSOCIATION";
        public const string ACCESS_ACCEPTANCE_VERIFICATION = "ACCEPTANCE VERIFICATION";

        public static string ACCESS_NEWBATCH = "TO ACCESS NEW BATCH";
        public static string ACCESS_LOADBATCH = "TO ACCESS LOAD BATCH";
        public static string ACCESS_MODIFYBATCH = "TO ACCESS MODIFY BATCH"; // BY Ansuman
        public static string ACCESS_CLOSEBATCH = "TO ACCESS CLOSE BATCH";

        public static string ToVerifyBATCH = "TO VERIFY BATCH";

        public const string ACCESS_REJECTION_VERIFICATION = "TO ACCESS REJECTION VERIFICATION";
        //public const string ACCESS_REWORKSTATION = "";

        public const string LOAD4PRINTING = "LOAD FOR PRINTING";
        public const string LOAD4FORREJECTIONVERIFICATION = "LOAD FOR REJECTION VERIFICATION";
        public const string LOAD4VERIFY = "LOAD FOR VERIFY";
        public const string LOAD4UPDATION = "LOAD FOR UPDATION";
        public const string LOAD4CLOSE = "LOAD FOR BATCH CLOSE";
        public const string LOAD4DECOMMISION = "LOAD FOR UPDATION";
        public const string LOAD4LOOSEPRINT = "LOAD FOR LOOSE PRINTING";
        public const string LOAD4LABELGEN = "LOAD FOR LABEL GENERATOR";

        public static string ACCESS_SYSTEM_LabelGenerator = "TO ACCESS LABEL GENERATOR";
        public const string ACCESS_LABLE_LAYOUT = "TO ACCESS LABLE LAYOUT";

        public static string BACKUP = "BACKUP DATABASE";
        public static string RESTORE = "RESTORE DATABASE";
        public static string BACHES_DELETED = "BATCHES DELETED";

        // Added by Anshuman
        public const string SAVE_LABLE_LAYOUT = "TO SAVE LABLE";
        public const string ACTIVATE_LAYOUT_FILE = "TO ACTIVATE LABLE";
        public const string DELETE_LAYOUT_FILE = "TO DELETE LABLE";
        public const string PRINT_LAYOUT_FILE = "TO PRINT LAYOUT RECIPE";
        public const string SAVE_APPLICATION_SETTINGS = "TO SAVE APPLICATION SETTINGS";
        // END

        public static string LableRePrint = "TO PERFORM LABEL REPRINT";
        public static string LableDecomissioning = "TO PERFORM LABEL DECOMMISSIONING";

        public static string DATA_SYNC_TO_SERVER = "TO PERFORM DATA SYNC FROM CLIENT TO SERVER";
        public static string DATA_SYNC_TO_CLIENT = "TO PERFORM DATA SYNC FROM SERVER TO CLIENT";

        public static string LINECLEARANCE = "TO PERFORM LINE CLEARANCE";

        public static string JOB_SETTING = "TO PERFORM JOB FILE SETTING ";
        public static string IMAGE_SETTING = "TO PERFORM SET CAMERA IMAGE";
        public static string TUNNING_FONT = "TO PERFORM TUNE FONT OF CURRENT IMAGE";

        public static string BTNREJECTIONCONFIRM = "TO PERFORM REJECTION VERIFICATION";

        public static string CONFIRMREJVERIFICATION = "REJECTION VERIFICATION DONE";

        public static string ONLINEREJECTION = "ONLINE RECONCILIATION";

        public static string EXITONLINEAPP = "ONLINE RECONCILIATION EXIT";

        public static string INSPECTIONONOFF = "TO PERFORM INSPECTION ON/OFF";
    }
    //All the Commented Code has been removed and the File has been renamed to "UserTrail.cs".
}
