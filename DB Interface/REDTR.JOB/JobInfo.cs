using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using REDTR.UTILS.SystemIntegrity;

namespace REDTR.JOB
{
    [Serializable()]
    public enum JobState
    {
        Created,
        Running,
        Paused,
        Closed,
        Decommissioned,
        LineClearance,
        CompleteTransfer,
        ForcefullyBatchClose
    };
    public enum Cust_GTIN_JbTypes
    {
        CIP,
        CHINACODE,
        DGFT
    }
    public class JobInfo
    {
        //private const string _DefDateFormat = "yyMMdd";

        //public static string DefDateFormat
        //{
        //    get { return _DefDateFormat; }
        //} 

        //public string DateFormat = _DefDateFormat;

        private List<PackInfo> m_jPackInfoLst;
        public List<PackInfo> jPackInfoLst
        {
            get { return m_jPackInfoLst; }
            set
            {
                m_jPackInfoLst = value;
                ExMethodsDeckProp.setPackInfo(m_jPackInfoLst);
            }
        }

        private string m_ProductCode;
        public string ProductCode
        {
            get { return m_ProductCode; }
            set { m_ProductCode = value; }
        }

        private string m_BatchNo;
        public string BatchNo
        {
            get { return m_BatchNo; }
            set { m_BatchNo = value; }
        }

        private DateTime m_Mfg;
        public DateTime Mfg
        {
            get { return m_Mfg; }
            set { m_Mfg = value; }
        }

        public string MfgGS1
        {
            get { return REDTR.CODEMGR.GS1Mgr.GetGS1Date(m_Mfg, Globals.AppSettings.IsUseExpiryDay); }
        }
        public string MfgTXT
        {
            get
            {
                if (Globals.AppSettings.HasAllCapsText == true)
                    return m_Mfg.ToString(Globals.AppSettings.ExpiryDayTextFormat).ToUpper();
                else
                    return m_Mfg.ToString(Globals.AppSettings.ExpiryDayTextFormat);
            }
        }

        private DateTime m_Exp;
        public DateTime Exp
        {
            get { return m_Exp; }
            set { m_Exp = value; }
        }

        public string ExpGS1
        {
            get { return REDTR.CODEMGR.GS1Mgr.GetGS1Date(m_Exp, Globals.AppSettings.IsUseExpiryDay); }
        }
        public string ExpTXT
        {
            get
            {
                if (Globals.AppSettings.HasAllCapsText == true)
                    return m_Exp.ToString(Globals.AppSettings.ExpiryDayTextFormat).ToUpper();
                else
                    return m_Exp.ToString(Globals.AppSettings.ExpiryDayTextFormat);
            }
        }
        private Decimal _NoReadCount;
        public Decimal NoReadCount
        {
            get { return _NoReadCount; }
            set { _NoReadCount = value; }
        }

        private decimal m_PAID;
        public decimal PAID
        {
            get { return m_PAID; }
            set { m_PAID = value; }
        }

        private string m_Product;
        public string Product
        {
            get { return m_Product; }
            set { m_Product = value; }
        }

        private string _FGCode; // Newly Added Murtaza
        public string FGCode
        {
            get { return _FGCode; }
            set { _FGCode = value; }
        }

        private string m_JobName;
        public string JobName
        {
            get { return m_JobName; }
            set { m_JobName = value; }
        }

        private bool _ForExport;

        public bool ForExport
        {
            get { return _ForExport; }
            set { _ForExport = value; }
        }
        private bool m_AutoBatchCloser;
        public bool AutoBatchCloser
        {
            get { return m_AutoBatchCloser; }
            set { m_AutoBatchCloser = value; }
        }

        private int m_PrimaryPCMapCount;
        public int PrimaryPCMapCount
        {
            get { return m_PrimaryPCMapCount; }
            set { m_PrimaryPCMapCount = value; }
        }

        private int m_BatchQty;
        public int BatchQty
        {
            get { return m_BatchQty; }
            set { m_BatchQty = value; }
        }

        private int m_SurplusQty;
        public int SurplusQty
        {
            get { return m_SurplusQty; }
            set { m_SurplusQty = value; }
        }

        private int m_QntToProduce;
        public int QntToProduce
        {
            get { return m_QntToProduce; }
            set { m_QntToProduce = value; }
        }

        private decimal m_JobID;
        public decimal JobID
        {
            get { return m_JobID; }
            set { m_JobID = value; }
        }
        private decimal m_TypeID;
        public decimal TypeID
        {
            get { return m_TypeID; }
            set { m_TypeID = value; }
        }

        private string m_TypeName;
        public string TypeName
        {
            get { return m_TypeName; }
            set { m_TypeName = value; }
        }

        private string m_MLNO;
        public string MLNO
        {
            get { return m_MLNO; }
            set { m_MLNO = value; }
        }

        private string m_TenderText;
        public string TenderText
        {
            get { return m_TenderText; }
            set { m_TenderText = value; }
        }

        private bool m_JobWithUID;
        public bool JobWithUID
        {
            get { return m_JobWithUID; }
            set { m_JobWithUID = value; }
        }
        private int m_UserID;

        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }
        private Nullable<Decimal> _LabelStartIndex;
        public Nullable<Decimal> LabelStartIndex
        {
            get { return _LabelStartIndex; }
            set { _LabelStartIndex = value; }
        }
    }

    public class PackInfo
    {
        private Decimal _PAID;
        public Decimal PAID
        {
            get { return _PAID; }
            set { _PAID = value; }
        }

        private DECKs _Deck;
        public DECKs Deck
        {
            get { return _Deck; }
            set { _Deck = value; }
        }

        private string _PPN;
        public string PPN
        {
            get { return _PPN; }
            set { _PPN = value; }
        }

        private string _GTIN;
        public string GTIN
        {
            get { return _GTIN; }
            set { _GTIN = value; }
        }
        private string _GTINCTI;
        public string GTINCTI
        {
            get { return _GTINCTI; }
            set { _GTINCTI = value; }
        }

        private Nullable<decimal> _MRP;
        public Nullable<decimal> MRP
        {
            get { return _MRP; }
            set { _MRP = value; }
        }
        private int _Size;
        public int Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        private int _BundleQty;
        public int BundleQty
        {
            get { return _BundleQty; }
            set { _BundleQty = value; }
        }

        private int _BatchQty;  ////means actual batch qty
        public int BatchQty
        {
            get { return _BatchQty; }
            set { _BatchQty = value; }
        }

        private int _TotalQty;  ////means actual batch qty and surplus qty 
        public int TotalQty
        {
            get { return _TotalQty; }
            set { _TotalQty = value; }
        }

        private int _UIDsToPRint;
        public int UIDsToPRint
        {
            get { return _UIDsToPRint; }
            set { _UIDsToPRint = value; }
        }

        private int _RemainigUIDsToPrint;

        public int RemainigUIDsToPrint
        {
            get { return _RemainigUIDsToPrint; }
            set { _RemainigUIDsToPrint = value; }
        }

        private string _FGCode;

        public string FGCode
        {
            get { return _FGCode; }
            set { _FGCode = value; }
        }

    }

    public static class ExMethodsDeckProp
    {
        private static List<PackInfo> _m_Lst;
        private static List<PackInfo> m_Lst
        {
            get { return _m_Lst; }
        }
        internal static void setPackInfo(List<PackInfo> m_PackInformationLst)
        {
            _m_Lst = m_PackInformationLst;
        }
        public static string GTIN(this DECKs mDeck)
        {
            if (_m_Lst == null)
                return string.Empty;
            PackInfo pckInfo = _m_Lst.Find(item => item.Deck == mDeck);
            if (pckInfo != null)
                return pckInfo.GTIN;
            else
                return string.Empty;
        }
        public static string MRP(this DECKs mDeck)
        {
            if (_m_Lst == null)
                return null;
            PackInfo pckInfo = _m_Lst.Find(item => item.Deck == mDeck);
            if (pckInfo != null)
                return pckInfo.MRP.ToString();
            else
                return null;
        }
        public static int BatchQty(this DECKs mDeck)
        {
            if (_m_Lst == null)
                return 0;
            PackInfo pckInfo = _m_Lst.Find(item => item.Deck == mDeck);
            if (pckInfo != null)
                return pckInfo.BatchQty;
            else
                return 0;
        }
        public static int TotalQty(this DECKs mDeck)
        {
            if (_m_Lst == null)
                return 0;
            PackInfo pckInfo = _m_Lst.Find(item => item.Deck == mDeck);
            if (pckInfo != null)
                return pckInfo.TotalQty;
            else
                return 0;
        }
        public static int UIDToPrint(this DECKs mDeck)
        {
            if (_m_Lst == null)
                return 0;
            PackInfo pckInfo = _m_Lst.Find(item => item.Deck == mDeck);
            if (pckInfo != null)
                return pckInfo.UIDsToPRint;
            //return pckInfo.RemainigUIDsToPrint; // 09.01.2015 by sunil
            else
                return 0;
        }
        public static int Size(this DECKs mDeck)
        {
            if (_m_Lst == null)
                return 0;
            PackInfo pckInfo = _m_Lst.Find(item => item.Deck == mDeck);
            if (pckInfo != null)
                return pckInfo.Size;
            else
                return 0;
        }

        public static int BunddleQty(this DECKs mDeck)
        {
            if (_m_Lst == null)
                return 0;
            PackInfo pckInfo = _m_Lst.Find(item => item.Deck == mDeck);
            if (pckInfo != null)
                return pckInfo.BundleQty;
            else
                return 0;
        }

        public static string GTINHR(this DECKs deck, string JobtypeName)
        {
            string gtin = deck.GTIN();
            if (JobtypeName == Cust_GTIN_JbTypes.CIP.ToString())
            {
                if (gtin.Length > 0)
                {
                    gtin = gtin.Substring(1);
                }
            }
            return gtin;
        }
    }
}
