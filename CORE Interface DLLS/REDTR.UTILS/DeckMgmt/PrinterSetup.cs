using System.Collections.Generic;
using System.IO;
using REDTR.UTILS.SystemIntegrity;
using RedXML;
using REDTR.DB.BusinessObjects;

namespace REDTR.UTILS
{
    public class PrinterSetup
    {
        #region PROPERTIES
        //Will be fix as per TnT_PackBoxesSetup.bin
        private DECKs _DECK;
        public DECKs DECK
        {
            get { return _DECK; }
            set { _DECK = value; }
        }

        // USV: From printer Interface list / dropdown
        private string _PrinterName;
        public string PrinterName
        {
            get { return _PrinterName; }
            set { _PrinterName = value; }
        }

        // USV: From printer Interface list
        private PrinterType _Type;
        public PrinterType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        // USV: From printer Interface list. NULL if Type == Desk
        private LinePrinterInfo _LinePrinter;
        public LinePrinterInfo LinePrinter
        {
            get { return _LinePrinter; }
            set { _LinePrinter = value; }
        }

        // USV: From printer Interface list. NULL if Type == Line
        private DeskPrinterInfo _DeskPrinter;
        public DeskPrinterInfo DeskPrinter
        {
            get { return _DeskPrinter; }
            set { _DeskPrinter = value; }
        }

        // USV: From Printer Label list/ grid
        List<JobPrintLabelAsso> _PrintLabels;
        public List<JobPrintLabelAsso> PrintLabels
        {
            get { return _PrintLabels; }
            set { _PrintLabels = value; }
        }

        #endregion PROPERTIES

        public static List<PrinterSetup> LstPrinterConfig;
        public void LoadPrinterConfiguration() //For Current Activated File
        {
            LstPrinterConfig = new List<PrinterSetup>();
            string FilePath = SettingsPath.PrinterSetup;
            if (File.Exists(FilePath) &&(!string.IsNullOrEmpty(File.ReadAllText(FilePath))))
            {
                LstPrinterConfig = GenericXmlSerializer<List<PrinterSetup>>.Deserialize(FilePath);
            } 
        }
        public static void SavePrinterSetup()
        {
            GenericXmlSerializer<List<PrinterSetup>>.Serialize(LstPrinterConfig, REDTR.UTILS.SettingsPath.PrinterSetup);
        }
        public static void CreateDefault(string FilePath, List<string> LstjOBTypes)
        {
            LstPrinterConfig = new List<PrinterSetup>();
            PrinterSetup Prnt;
            foreach (PackBoxesSetup pack in PackBoxes.LstPackBoxes)
            {   
                {
                    Prnt = new PrinterSetup();
                    Prnt.DECK = pack.Decks;
                    if (pack.IsTertiary == false)
                    {
                        Prnt.PrinterName = "ReaJetPrinter";
                        Prnt.Type = PrinterType.Line;
                        LinePrinterInfo LPInfo = new LinePrinterInfo();
                        Prnt.LinePrinter = LPInfo;
                        LPInfo.Communication = "TCP";
                        LPInfo.ConnectAddress = "192.68.1.235";
                        LPInfo.DefaultPrinterLabel = "tntocrb.job";
                        LPInfo.PrinterHasFeedback = true;
                        LPInfo.PORT = 22171;
                        LPInfo.HasPrintJobChangeDynamic = true;
                        LPInfo.JobTypeSize_DATA1 = 60;
                        LPInfo.JobTypeSize_DATA2 = 60;
                        LPInfo.FONT_TRAIN_CHARS_GTIN = "12345678901231";
                        LPInfo.FONT_TRAIN_CHARS_LOT = "ABCDEFGHIJKLMNO";
                        LPInfo.FONT_TRAIN_CHARS_SRNO = "PQRSTUVWXYZ1234";
                        Prnt.DeskPrinter = null;
                    }
                    else
                    {
                        Prnt.PrinterName = "CRBasedWinDrvPrinter";
                        Prnt.Type = PrinterType.Desk;
                        Prnt.LinePrinter = null;
                        DeskPrinterInfo DPInfo = new DeskPrinterInfo();
                        Prnt.DeskPrinter = DPInfo;
                        DPInfo.Communication = "TCP";
                        DPInfo.ConnectAddress = "192.68.1.150";
                        DPInfo.PORT = 9100;
                        DPInfo.PrintCollated = false;
                        DPInfo.PrintCopies = 1;
                        DPInfo.PrintDisable = false;
                        DPInfo.PrinterCurSchemas = "TnT_LabelDataSetup.bin";
                        DPInfo.SchemasDirectoryPath = @"\LABEL SCHEMAS\";
                    }
                    Prnt.PrintLabels = JobPrintLabelAsso.CreateDefault(LstjOBTypes);
                    LstPrinterConfig.Add(Prnt);
                }
            }
            if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(FilePath)) == false)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(FilePath));
            }
            GenericXmlSerializer<List<PrinterSetup>>.Serialize(LstPrinterConfig, FilePath);
        }

        public static PrinterSetup GetPrinterDetails(string Address)
        {
            PrinterSetup Print = null;
            if (LstPrinterConfig != null)
                Print = LstPrinterConfig.Find(item => item.LinePrinter.ConnectAddress == Address);
            return Print;
        }
        public static PrinterSetup GetPrinterDetails(DECKs deck)
        {
            PrinterSetup Print = null;
            if (LstPrinterConfig != null)
                Print = LstPrinterConfig.Find(item => item.DECK == deck);
            return Print;
        }
        public static string GetPrinterName(DECKs deck)
        {
            PrinterSetup Print = null;
            if (LstPrinterConfig != null)
                Print = LstPrinterConfig.Find(item => item.DECK == deck);
            if (Print != null)
                return Print.PrinterName;
            else
                return string.Empty;
        }
        static PrinterSetup data = new PrinterSetup();
        public static List<PrinterSetup> GetPrinterDetailsss(List<PackBoxesSetup> lstDeck)
        {
            List<PrinterSetup> LPrint = new List<PrinterSetup>();
            for (int i = 0; i < lstDeck.Count; i++)
            {
                if (LstPrinterConfig != null)
                {
                    data = LstPrinterConfig.Find(item => item.DECK == lstDeck[i].Decks);
                    LPrint.Add(data);
                }
            }
            return LPrint;
        }
        public static string GetDefaultPrinterLable(DECKs deck)
        {
            PrinterSetup Psetup = REDTR.UTILS.PrinterSetup.GetPrinterDetails(deck);

            if (Psetup == null)
                return string.Empty;
            else if (Psetup.LinePrinter != null)
                return Psetup.LinePrinter.DefaultPrinterLabel;
            else if (Psetup.DeskPrinter != null)
                return Psetup.DeskPrinter.PrinterCurSchemas;
            else return string.Empty;
        }
        // Function Parameter increased for assigning printer label as per product name [Sunil] //List<PackageLabelAsso> LstPackLabel
        public static string GetPrinterLable(DECKs deck, string JobType,List<PackageLabelAsso> LstPackLabel, bool jobwithuid)
        {
            PrinterSetup Psetup = REDTR.UTILS.PrinterSetup.GetPrinterDetails(deck);
            if (Psetup == null)
                return string.Empty;

            // This condition added by sunil [15.09.2015]
            for (int i = 0; i < Psetup.PrintLabels.Count; i++)
            {
                if (Psetup.PrintLabels[i].JobType == JobType && Psetup.PrintLabels[i].HasUID == true)
                {
                    Psetup.PrintLabels[i].GS1Filter = LstPackLabel[0].Filter;
                    Psetup.PrintLabels[i].Label2Use = LstPackLabel[0].LabelName;
                }
            }
            // ENDS HERE.

            JobPrintLabelAsso jasso = Psetup.PrintLabels.Find(item => item.JobType == JobType && item.HasUID == jobwithuid);

            if (jasso != null)
                return jasso.Label2Use;

            else return string.Empty;
        }
        public static PrinterType GetPrinterType(DECKs deck)
        {
            PrinterSetup Print = null;
            if (LstPrinterConfig != null)
                Print = LstPrinterConfig.Find(item => item.DECK == deck);
            if (Print != null)
                return Print.Type;
            else
                return PrinterType.Line;
        }
        public static List<JobPrintLabelAsso> GetPrintLabels(string printerName)
        {
            PrinterSetup Print = null;
            if (LstPrinterConfig != null)
                Print = LstPrinterConfig.Find(item => item.PrinterName.ToUpper() == printerName.ToUpper());
            if (Print != null)
                return Print.PrintLabels;
            else
                return null;
        }

        public static bool RemoveNode4deck(DECKs deck)
        {
            if (PrinterSetup.LstPrinterConfig != null && PrinterSetup.LstPrinterConfig.Count > 0)
            {
                int index = -1;
                index = PrinterSetup.LstPrinterConfig.FindIndex(item => item._DECK == deck);
                if (index == -1)
                    return false;
                PrinterSetup.LstPrinterConfig.RemoveAt(index);
                return true;
            }
            return false;
        }

        public static bool IsExistNode4Deck(DECKs deck)
        {
            if (PrinterSetup.LstPrinterConfig != null && PrinterSetup.LstPrinterConfig.Count > 0)
            {
                int index = PrinterSetup.LstPrinterConfig.FindIndex(item => item.DECK  == deck);
                if (index > -1)
                    return true;
            }
            return false;
        }

        public static bool IsDeskPrinterExists()
        {
            if (PrinterSetup.LstPrinterConfig != null && PrinterSetup.LstPrinterConfig.Count > 0)
            {
                int index = PrinterSetup.LstPrinterConfig.FindIndex(item => item.DeskPrinter != null);
                if (index > -1)
                    return true;
            }
            return false;
        }
        public static bool IsDeskPrinter(DECKs deck)
        {
            PrinterSetup prntSetup = PrinterSetup.GetPrinterDetails(deck);
            if (prntSetup != null && prntSetup.DeskPrinter != null)
                return true;
            else
                return false;
        }
        public static PrinterSetup GetAnyDeskPrinterDetails()
        {
            if (PrinterSetup.LstPrinterConfig != null && PrinterSetup.LstPrinterConfig.Count > 0)
            {
                PrinterSetup prt = PrinterSetup.LstPrinterConfig.Find(item => item.DeskPrinter != null);
                return prt;
            }
            else
                return null;
        }

        public static string GetGS1Filter(DECKs deck,string JobTypeName, bool withUid)
        {
            PrinterSetup psetup = GetPrinterDetails(deck);
            if (psetup != null)
            {
                if (psetup.PrintLabels != null)
                {
                    JobPrintLabelAsso jobprintasso = psetup.PrintLabels.Find(item => item.JobType == JobTypeName && item.HasUID == withUid);
                    if (jobprintasso != null)
                        return jobprintasso.GS1Filter;
                }
            }
            return string.Empty;
        }

        public static bool isPrinter(DECKs dECKs)
        {
            PrinterSetup prt = PrinterSetup.LstPrinterConfig.Find(item => item.DECK == dECKs);
            if (prt == null || (prt.DeskPrinter == null && prt.LinePrinter == null))
                return false;
            else
                return true;
        }
    }

    public enum PrinterType
    {
        Line,
        Desk
    }
    public class LinePrinterInfo
    {
        private string _Communication; //"COM" / "TCP";
        public string Communication
        {
            get { return _Communication; }
            set { _Communication = value; }
        }

        private string _ConnectAddress;
        public string ConnectAddress
        {
            get { return _ConnectAddress; }
            set { _ConnectAddress = value; }
        }

        private int _PORT;
        public int PORT
        {
            get { return _PORT; }
            set { _PORT = value; }
        }

        private string _DefaultPrinterLabel;
        public string DefaultPrinterLabel
        {
            get { return _DefaultPrinterLabel; }
            set { _DefaultPrinterLabel = value; }
        }

        private bool _hasPrintJobChangeDynamic;
        public bool HasPrintJobChangeDynamic
        {
            get { return _hasPrintJobChangeDynamic; }
            set { _hasPrintJobChangeDynamic = value; }
        }

        private bool _PrinterHasFeedback;
        public bool PrinterHasFeedback
        {
            get { return _PrinterHasFeedback; }
            set { _PrinterHasFeedback = value; }
        }

        private int _JobTypeSize_DATA1;
        public int JobTypeSize_DATA1
        {
            get { return _JobTypeSize_DATA1; }
            set { _JobTypeSize_DATA1 = value; }
        }

        private int _JobTypeSize_DATA2;
        public int JobTypeSize_DATA2
        {
            get { return _JobTypeSize_DATA2; }
            set { _JobTypeSize_DATA2 = value; }
        }

        #region FONT TRAIN DATA
        private string _FONT_TRAIN_CHARS_GTIN;
        public string FONT_TRAIN_CHARS_GTIN
        {
            get { return _FONT_TRAIN_CHARS_GTIN; }
            set { _FONT_TRAIN_CHARS_GTIN = value; }
        }

        private string _FONT_TRAIN_CHARS_LOT;
        public string FONT_TRAIN_CHARS_LOT
        {
            get { return _FONT_TRAIN_CHARS_LOT; }
            set { _FONT_TRAIN_CHARS_LOT = value; }
        }

        private string _FONT_TRAIN_CHARS_SRNO;
        public string FONT_TRAIN_CHARS_SRNO
        {
            get { return _FONT_TRAIN_CHARS_SRNO; }
            set { _FONT_TRAIN_CHARS_SRNO = value; }
        }
        #endregion FONT TRAIN DATA

        public LinePrinterInfo()
        {

        }
    }
    public class DeskPrinterInfo
    {
        private string _Communication; //"COM" / "TCP";
        public string Communication
        {
            get { return _Communication; }
            set { _Communication = value; }
        }

        private string _ConnectAddress;
        public string ConnectAddress
        {
            get { return _ConnectAddress; }
            set { _ConnectAddress = value; }
        }

        private int _PORT;
        public int PORT
        {
            get { return _PORT; }
            set { _PORT = value; }
        }

        private bool _IsPrintingWithZpl;

        public bool IsPrintingWithZpl
        {
            get { return _IsPrintingWithZpl; }
            set { _IsPrintingWithZpl = value; }
        }

        private string _PrinterName;
        public string PrinterName
        {
            get { return _PrinterName; }
            set { _PrinterName = value; }
        }

        private int _PrintCopies;
        public int PrintCopies
        {
            get { return _PrintCopies; }
            set { _PrintCopies = value; }
        }

        private bool _PrintCollated;
        public bool PrintCollated
        {
            get { return _PrintCollated; }
            set { _PrintCollated = value; }
        }

        private bool _PrintDisable;
        public bool PrintDisable
        {
            get { return _PrintDisable; }
            set { _PrintDisable = value; }
        }

        private string _SchemasDirectoryPath;
        public string SchemasDirectoryPath
        {
            get { return _SchemasDirectoryPath; }
            set { _SchemasDirectoryPath = value; }
        }

        private string _PrinterCurSchemas;
        public string PrinterCurSchemas
        {
            get { return _PrinterCurSchemas; }
            set { _PrinterCurSchemas = value; }
        }

        public DeskPrinterInfo()
        {
            PrinterCurSchemas = "TnT_LabelDataSetup.bin";
            SchemasDirectoryPath = @"\LABEL SCHEMAS\";
        }
    }
}
