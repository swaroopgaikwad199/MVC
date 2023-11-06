using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using REDTR.UTILS.SystemIntegrity;

namespace REDTR.CODEMGR
{
    public static class SampleData
    {
        public const string TestGTIN = "12345678901231";
        public const string TestCTIGTIN = "12345678901231";
        public const string TestUID = "SAMPLES4TESTING";
        public const string TestLOT = "L12345";
    }
    public static class TestCompanyDetails // 28.04.2015
    {
        public const string CompanyName = "Propix Technologies Private Limited";
        public const string CompanyAddress = "Nanded Phata,Pune";
        public const string CompanyCode = "8900001";
    }
    public static class FilterData
    {
        public const string fldSep = "|";
        public const string fldDataSep = "-";

        public const string fldGS12D1 = "GS12D1";
        public const string fldGS12D2 = "GS12D2";
        public const string fldGS1BC1 = "GS1BC1";
        public const string fldGS1BC2 = "GS1BC2";
        public const string fldGS1BC3 = "GS1BC3";

        public const string fldIDGTIN = "IDGTIN";
        public const string fldIDLOT = "IDLOT";
        public const string fldIDEXP = "IDEXP";
        public const string fldIDMFG = "IDMFG";
        public const string fldIDUID = "IDUID";
        public const string fldIDSSCC = "IDSSCC";

        public const string fldTxtGTIN = "GTIN";
        public const string fldTxtLOT = "LOT";
        public const string fldTxtEXP = "EXP";
        public const string fldTxtMFG = "MFG";
        public const string fldTxtUID = "UID";
        public const string fldTxtCSNO = "CSNO";

        public const string fldTxtPRODNAME = "PRODNAME";

        public const string fldTxtADDDATA1 = "ADDDATA1";
        public const string fldTxtADDDATA2 = "ADDDATA2";
        public const string fldTxtADDDATA3 = "ADDDATA3";
        public const string fldTxtADDDATA4 = "ADDDATA4";

        /// <summary>
        /// "GS12D1-01-17-10-21|GTIN|LOT|EXP|UID"
        /// "GS12D1-01-17-10|GTIN|LOT|EXP"
        /// "GS12D1-01-10-17-21|GTIN|LOT|EXP|UID|ADDDATA1|ADDDATA2"
        /// "GS12D1-01-10-17-21|GTIN|ADDDATA1|ADDDATA2|LOT|EXP|UID"
        /// "GS12D1-01-10-17-21|GS12D2-01-10-11-17-21|GTIN|LOT|EXP|UID"
        /// </summary>
        /// <param name="giveAllFlds"></param>
        /// <returns></returns>
        public static string GetSampleFilter(bool giveAllFlds)
        {
            string Filter = string.Empty;

            if (giveAllFlds == false)
            {
                Filter += fldGS12D1 + fldDataSep + "01" + fldDataSep + "10" + fldDataSep + "17" + fldDataSep + "21" + fldSep;

                Filter += fldTxtGTIN + fldSep;
                Filter += fldTxtLOT + fldSep;
                Filter += fldTxtEXP + fldSep;
                Filter += fldTxtUID + fldSep;
            }
            else
            {
                Filter += fldGS12D1 + fldDataSep + "01" + fldDataSep + "10" + fldDataSep + "11" + fldDataSep + "17" + fldDataSep + "21" + fldSep;

                Filter += fldTxtGTIN + fldSep;
                Filter += fldTxtLOT + fldSep;
                Filter += fldTxtMFG + fldSep;
                Filter += fldTxtEXP + fldSep;
                Filter += fldTxtUID + fldSep;

                Filter += fldTxtADDDATA1 + fldSep;
                Filter += fldTxtADDDATA2 + fldSep;
                Filter += fldTxtADDDATA3 + fldSep;
                Filter += fldTxtADDDATA4 + fldSep;
            }
            if (Filter.EndsWith(fldSep) == true)
            {
                Filter = Filter.Remove(Filter.Length - fldSep.Length);
            }
            return Filter;
        }

    }

    public class CodeData
    {
        private string m_GTIN;
        private string m_LOT;
        private DateTime m_MfgDate;
        private DateTime m_ExpDate;
        private string m_UID;
        private bool m_IsGS1;

        public string GTIN
        {
            get { return m_GTIN; }
            set { m_GTIN = value; }
        }
        public string BatchNo
        {
            get { return m_LOT; }
            set { m_LOT = value; }
        }
        public DateTime MfgDate
        {
            get { return m_MfgDate; }
            set { m_MfgDate = value; }
        }
        public DateTime ExpiryDate
        {
            get { return m_ExpDate; }
            set { m_ExpDate = value; }
        }
        public string SerialNo
        {
            get { return m_UID; }
            set { m_UID = value; }
        }
        public bool IsGS1
        {
            get { return m_IsGS1; }
            set { m_IsGS1 = value; }
        }
        public CodeData()
        {

        }
        public CodeData(string gtin, string lot, DateTime mfg, DateTime exp, string serial)
        {
            m_GTIN = gtin;
            m_LOT = lot;
            m_MfgDate = mfg;
            m_ExpDate = exp;
            m_UID = serial;
        }
        public bool Compare(CodeData data)
        {
            //if (this.SerialNo != data.SerialNo)
            if (String.Compare(this.SerialNo, 0, data.SerialNo, 0, this.SerialNo.Length) != 0)
                return false;
            if (this.BatchNo != data.BatchNo)
                return false;
            if (this.GTIN != data.GTIN)
                return false;
            if (GS1Mgr.IsValidGS1Date(this.ExpiryDate, data.ExpiryDate) == false)
                return false;
            return true;
        }
        public bool CompareAvailable(CodeData data)
        {
            //if (this.SerialNo != data.SerialNo)
            if (data.SerialNo != null && String.Compare(this.SerialNo, 0, data.SerialNo, 0, this.SerialNo.Length) != 0)
                return false;
            if (data.BatchNo != null && this.BatchNo != data.BatchNo)
                return false;
            if (data.GTIN != null && this.GTIN != data.GTIN)
                return false;
            if (GS1Mgr.IsValidGS1Date(this.ExpiryDate, data.ExpiryDate) == false)
                return false;
            return true;
        }
        public bool Compare(string oUID, string oLOT, string oGTIN, DateTime oEXP)
        {
            if (oUID != null && String.Compare(this.SerialNo, 0, oUID, 0, this.SerialNo.Length) != 0)
                return false;
            if (oLOT != null && this.BatchNo != oLOT)
                return false;
            if (oGTIN != null && this.GTIN != oGTIN)
                return false;
            if (GS1Mgr.IsValidGS1Date(this.ExpiryDate, oEXP) == false)
                return false;

            return true;
        }
        public override string ToString()
        {
            return "GTIN: " + m_GTIN + ",LOT:" + m_LOT + ",MFG:" + m_MfgDate + ",EXP:" + m_ExpDate + ",UID:" + m_UID;
        }
    }

    public class GS1Mgr
    {
        public enum GS1AI
        {
            SSCC = 0,
            GTIN = 1,
            LOT = 10,
            MFG = 11,
            EXP = 17,
            UID = 21,
            None = -1
        }
        private const int SSCCLen = 18;
        private const int GTINLen = 14;
        private const int LOTLen = 20;
        private const int MFGLen = 6;
        private const int EXPLen = 6;
        private const int UIDLen = 20;

        public const string GS1DateFormat = "yyMMdd";

        public const string GS1GroupSeparator = "\x1D"; 

        public static bool IsValidGS1DataSize(string GS1Data, GS1AI GS1AI)
        {
            int dataLen = GS1Data.Length;
            if (GS1AI == GS1AI.SSCC && dataLen == SSCCLen)
                return true;
            else if (GS1AI == GS1AI.GTIN && dataLen == GTINLen)
                return true;
            else if (GS1AI == GS1AI.LOT && dataLen <= LOTLen)
                return true;
            else if (GS1AI == GS1AI.EXP && dataLen == EXPLen)
                return true;
            else if (GS1AI == GS1AI.UID && dataLen <= UIDLen)
                return true;
            else
                return false;
        }

        public static string GetGS1AI(GS1AI AI)
        {
            return AI.ToString().PadLeft(2, '0');
        }

        public static List<GS1Mgr.GS1AI> GetGS1AISeq(string GS1AISeqFilter)
        {
            List<GS1Mgr.GS1AI> lstGS1AISeq = new List<GS1AI>();
            string[] GS1AISeq = GS1AISeqFilter.Split(new string[] { FilterData.fldDataSep }, StringSplitOptions.None);

            foreach (string AI in GS1AISeq)
            {
                if (AI.StartsWith("01"))      // GTIN
                {
                    lstGS1AISeq.Add(GS1AI.GTIN);
                }
                else if (AI.StartsWith("10")) // LOT or BATCH NO
                {
                    lstGS1AISeq.Add(GS1AI.LOT);
                }
                else if (AI.StartsWith("11")) // MFG
                {
                    lstGS1AISeq.Add(GS1AI.MFG);
                }
                else if (AI.StartsWith("17")) // EXPIRY
                {
                    lstGS1AISeq.Add(GS1AI.EXP);
                }
                else if (AI.StartsWith("21")) // UID or SRNO
                {
                    lstGS1AISeq.Add(GS1AI.UID);
                }
            }
            return lstGS1AISeq;
        }
        public static List<GS1Mgr.GS1AI> GetGS1AISeqDefault()
        {
            List<GS1Mgr.GS1AI> lstGS1AISeq = new List<GS1AI>();
            lstGS1AISeq.Add(GS1AI.GTIN);
            lstGS1AISeq.Add(GS1AI.LOT);
            lstGS1AISeq.Add(GS1AI.EXP);
            lstGS1AISeq.Add(GS1AI.UID);
            return lstGS1AISeq;
        }

        /// <summary>
        /// This function Calculates the GS1 CheckSum/CheckDigit
        /// </summary>
        /// <param name="GS1Data">Data Passed should be without GS1 CheckSum character at the end.
        /// Example. for 14 digit GTIN pass only first 13 characters
        /// </param>
        /// <returns>GS1 CheckSum/CheckDigit Value</returns>
        public static int GetGS1CheckSum(string GS1Data)
        {
            // Code here for GTIN CheckSum
            char[] dataCA = GS1Data.ToCharArray();

            int SUM = 0;
            int Val = 0;
            int digit = 0;
            int multiplier = 3;
            for (int i = 0; i < dataCA.Length; i++)
            {
                digit = Int32.Parse(dataCA[i].ToString());
                Val = digit * multiplier;
                if (multiplier == 3)
                    multiplier = 1;
                else
                    multiplier = 3;

                SUM += Val;
            }
            decimal d = (decimal)SUM;
            d = d / 10;
            decimal x = Math.Ceiling(d);
            int ceil = (int)x * 10;

            int checkSUM = ceil - SUM;
            return checkSUM;
        }

        /// <summary>
        /// This function Checks the GS1 CheckSum/CheckDigit
        /// </summary>
        /// <param name="GS1Data">Data passed should be with CheckSum</param>
        /// <returns>True if CheckSum passes is correct; else false</returns>
        public static bool IsValidGS1CheckSum(string GS1Data)
        {
            try
            {
                string data = GS1Data.Substring(0, GS1Data.Length - 1);
                int checkSUM = GetGS1CheckSum(data);
                int OriCheckSum = -1;
                Int32.TryParse(GS1Data.Substring(GS1Data.Length - 1, 1), out OriCheckSum);
                if (checkSUM != OriCheckSum)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// This function returns the GS1 Format Date (yyMMdd).
        /// </summary>
        /// <param name="dateValue">Date which is to be formatted</param>
        /// <param name="GS1HasDayofDATE">Date Contains MFG/Expiry/Etc Day or not (00)</param>
        /// <returns>GS1 Formatted Date</returns>
        public static string GetGS1Date(DateTime dateValue, bool GS1HasDayofDATE)
        {
            string gs1Date = "";
            gs1Date += dateValue.Year.ToString().Substring(2);
            gs1Date += dateValue.Month.ToString().PadLeft(2, '0');
            if (GS1HasDayofDATE == false)
            {
                gs1Date += "00";
            }
            else
            {
                gs1Date += dateValue.Day.ToString().PadLeft(2, '0');
            }
            //gs1Date = dateValue.ToString("ddMMyy");
            return gs1Date;
        }

        /// <summary>
        /// This function Compare the GS1 Date
        /// </summary>
        /// <param name="dateBaseVal">Benchmark Date </param>
        /// /// <param name="date2Check">Inspection Date </param>
        /// <returns>True if Dates are equal as per GS1; else false</returns>
        public static bool IsValidGS1Date(DateTime dateBaseVal, DateTime date2Check)
        {
            if (REDTR.UTILS.SystemIntegrity.Globals.AppSettings.IsUseExpiryDay == false)
            {
                if (date2Check != null && date2Check.Year > 2000 && dateBaseVal.Year != date2Check.Year && dateBaseVal.Month != date2Check.Month)
                    return false;
            }
            else
            {
                if (date2Check != null && date2Check.Year > 2000 && dateBaseVal != date2Check)
                    return false;
            }
            return true;
        }

        public class AITagVal
        {
            public GS1AI AITag;
            public string AIVal;
        }
        /// <summary>
        /// Generated ID(1D/2D) Code in GS1 Format for provided AITagVal in specified sequence
        /// </summary>
        /// <param name="CodeBeginWith">Provide value if applicable or EMPTY STRING</param>
        /// <param name="GrpSep">Provide GS1 Group Seperator Applicable. "\x1D". For printer specific like "~1" etc</param>
        /// <param name="lstAITagVal"> GS1 AI and its Value in required sequence.
        /// GS1Mgr.AITagVal[] ss= new GS1Mgr.AITagVal[4];
        /// ss[0] = new GS1Mgr.AITagVal();
        /// ss[1] = new GS1Mgr.AITagVal();
        /// ss[2] = new GS1Mgr.AITagVal();
        /// ss[3] = new GS1Mgr.AITagVal();
        /// ss[0].AITag = GS1Mgr.GS1AI.GTIN;
        /// ss[0].AIVal = "12345678901231";
        /// ss[1].AITag = GS1Mgr.GS1AI.LOT;
        /// ss[1].AIVal = "LOT";
        /// ss[2].AITag = GS1Mgr.GS1AI.EXP;
        /// ss[2].AIVal = "170900";
        /// ss[3].AITag = GS1Mgr.GS1AI.UID ;
        /// ss[3].AIVal = "A1B3C5D7E9F1G3H";
        /// </param>
        /// <returns>Returns ID(1D/2D) Code string.
        /// "011234567890123110LOT<GS>1717090021A1B3C5D7E9F1G3H"
        /// </returns>
        public static string GetIDCode(string CodeBeginWith, string GrpSep, List<AITagVal> lstAITagVal)
        {
            string output = "";

            if (string.IsNullOrEmpty(CodeBeginWith) == false)
                output += CodeBeginWith;

            foreach (AITagVal item in lstAITagVal)
            {
                if (string.IsNullOrEmpty(item.AIVal) == true)
                    continue;
                switch (item.AITag)
                {
                    case GS1AI.GTIN:
                    case GS1AI.EXP:
                    case GS1AI.MFG:
                    case GS1AI.SSCC:
                        output += string.Format("{0:00}{1}", (int)item.AITag, item.AIVal); break;
                    case GS1AI.LOT:
                    case GS1AI.UID:
                        output += string.Format("{0:00}{1}{2}", (int)item.AITag, item.AIVal, GrpSep); break;
                }
            }
            if (output.EndsWith(GrpSep) && GrpSep.Length > 0)
            {
                output = output.Remove(output.Length - GrpSep.Length);
            }
            return output;
        }
        public static string GetIDCode(string oGTIN, string oLOT, string oEXP, string oUID)
        {
            string output = "";

            if (string.IsNullOrEmpty(oGTIN) == false)
                output += "01" + oGTIN;

            if (string.IsNullOrEmpty(oLOT) == false)
                output += "10" + oLOT + GS1GroupSeparator;

            if (string.IsNullOrEmpty(oEXP) == false)
                output += "17" + oEXP;

            if (string.IsNullOrEmpty(oUID) == false)
                output += "21" + oUID + GS1Mgr.GS1GroupSeparator;

            if (output.EndsWith(GS1Mgr.GS1GroupSeparator) && GS1Mgr.GS1GroupSeparator.Length > 0)
            {
                output = output.Remove(output.Length - GS1Mgr.GS1GroupSeparator.Length);
            }
            return output;
        }
        
        /// <summary>
        /// This will parse the DM2D code data and returns the value of parsed AI
        /// Parsing will be done as per GS1 Specs
        /// </summary>
        /// <param name="code">DM2D code to parse</param>
        /// <param name="Filter">Not used as of now</param>
        /// <returns>Parsed 2D code AI</returns>
        public static CodeData DecodeCode(string code, string Filter)
        {
            int codeLen = code.Length;
            string codeBuf = code;
            CodeData c = new CodeData();
            //
            if (codeBuf.Length > 3 && true == codeBuf.StartsWith("]d2", true, null))
            {
                c.IsGS1 = true;
                codeBuf = codeBuf.Substring(3);
            }
            else if (codeBuf.Length > 3 && true == codeBuf.StartsWith(GS1GroupSeparator, true, null))//"\x1D"
            {
                c.IsGS1 = true;
                if (GS1GroupSeparator.Length > 0)
                    codeBuf = codeBuf.Substring(1);
            }
            else
                c.IsGS1 = false;

            int retryCnt = 0;

            while (codeBuf.Length > 0)
            {
                if (retryCnt++ > 7)
                {
                    Trace.TraceError("{0},{1}{2}", DateTime.Now.ToString(), "MAJOR LOGICAL BLUNDER!!!", "@DecodeCode NEED REVIEW OF SYSTEM");
                    throw new ArgumentException("Invalid ID code data provided", "string code");
                    //break;
                }
                if (true == codeBuf.StartsWith("01", true, null) && codeLen > 16)
                {
                    c.GTIN = codeBuf.Substring(2, GTINLen);
                    codeBuf = codeBuf.Substring(GTINLen + 2);
                }
                if (true == codeBuf.StartsWith("10", true, null))
                {
                    int len = -1;
                    len = codeBuf.IndexOf('\x1D');
                    if (len == -1) // THis is for Cognex, as its returning seperator different...
                        len = codeBuf.IndexOf('\x25ba');

                    if (len == -1) // THis is for Cognex, as its returning seperator different...
                    {
                        len = codeBuf.IndexOf('<');
                        codeBuf = codeBuf.Replace("<GS>", "<");
                    }

                    // In case if Last String in 2DCode, No seperator available
                    if (len == -1)
                    { 
                            len = codeBuf.Length > 22 ? 22 : codeBuf.Length; // 22 (20:Max Len for VarData + 2:AI)
                            string BatchNo = codeBuf.Substring(2, len - 2);
                            c.BatchNo = BatchNo;
                            codeBuf = ""; 
                    }
                    else if (len > 2) // 2 for "10"
                    {
                        c.BatchNo = codeBuf.Substring(2, len - 2); // -2 as started from 3rd char
                        codeBuf = codeBuf.Substring(len + 1); // +1 for seperator charachter '\x1D'
                    }
                }
                if (true == codeBuf.StartsWith("11", true, null))
                {
                    string date = codeBuf.Substring(2, 6);
                    int day = int.Parse(date.Substring(4, 2));
                    int month = int.Parse(date.Substring(2, 2));
                    int year = 2000 + int.Parse(date.Substring(0, 2));
                    int lastDayofMonth = DateTime.DaysInMonth(year, month);
                    c.MfgDate = new DateTime(year, month, day != 0 ? day : lastDayofMonth);
                    codeBuf = codeBuf.Substring(8);
                }
                if (true == codeBuf.StartsWith("17", true, null))
                {
                    string date = codeBuf.Substring(2, 6);
                    int day = int.Parse(date.Substring(4, 2));
                    int month = int.Parse(date.Substring(2, 2));
                    int year = 2000 + int.Parse(date.Substring(0, 2));
                    int lastDayofMonth = DateTime.DaysInMonth(year, month);
                    c.ExpiryDate = new DateTime(year, month, day != 0 ? day : lastDayofMonth);
                    codeBuf = codeBuf.Substring(8);
                }
                if (true == codeBuf.StartsWith("21", true, null))
                {
                    ////int len = codeBuf.IndexOf('\x1D');
                    ////if (len > 2) // 2 for "21"
                    ////{
                    ////    c.SerialNo = codeBuf.Substring(2, len - 2); // -2 as started from 3rd char
                    ////    codeBuf = codeBuf.Substring(len + 1); // +1 for seperator charachter '\x1D'
                    ////}
                    //if (codeBuf.Length >= 17)
                    //{
                    //    string serial = codeBuf.Substring(2, 15);
                    //    c.SerialNo = serial;
                    //    codeBuf = codeBuf.Substring(17 + 1);
                    //}
                    //else
                    //{
                    //    string serial = codeBuf.Substring(2);
                    //    c.SerialNo = serial;
                    //    codeBuf = "";
                    //}
                    int len = -1;
                    len = codeBuf.IndexOf('\x1D');
                    if (len == -1) // THis is for Cognex, as its returning seperator different...
                        len = codeBuf.IndexOf('\x25ba');

                    // In case if Last String in 2DCode, No seperator available
                    if (len == -1)
                    {
                        len = codeBuf.Length > 22 ? 22 : codeBuf.Length; // 22 (20:Max Len for VarData + 2:AI)
                        string SerialNo = codeBuf.Substring(2, len - 2);
                        c.SerialNo = SerialNo;
                        codeBuf = "";
                    }
                    else
                        if (len > 2) // 2 for "21"
                        {
                            c.SerialNo = codeBuf.Substring(2, len - 2); // -2 as started from 3rd char
                            codeBuf = codeBuf.Substring(len + 1); // +1 for seperator charachter '\x1D'
                        }
                }
            }
            return c;
        }
        /// <summary>
        /// THis API for parsing data from Text Box, as Hendheld gives data in TB and GS Seperator is mismatched. 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public static CodeData DecodeCodeTry(string code, string Filter)
        {
            int codeLen = code.Length;
            string codeBuf = code;
            CodeData c = new CodeData();
            bool hasGTIN, hasLOT, hasEXP, hasSRNO;
            hasGTIN = hasLOT = hasEXP = hasSRNO = false;

            if (codeBuf.Length > 3 && true == codeBuf.StartsWith("]d2", true, null))
            {
                c.IsGS1 = true;
                codeBuf = codeBuf.Substring(3);
            }
            else if (codeBuf.Length > 3 && true == codeBuf.StartsWith(GS1GroupSeparator, true, null))//"\x1D"
            {
                c.IsGS1 = true;
                codeBuf = codeBuf.Substring(1);
            }
            else
                c.IsGS1 = false; ;

            int retryCnt = 0;

            while (codeBuf.Length > 0)
            {
                if (retryCnt++ > 5)
                {
                    Trace.TraceWarning("{0},{1}{2}", DateTime.Now.ToString(), "WRONG DATA "+ codeBuf +" PASSED --OR-- MAJOR LOGICAL BLUNDER!!!", "@DecodeCode NEED REVIEW OF SYSTEM");
                    break;
                }
                if (true == codeBuf.StartsWith("01", true, null) && codeLen > 16)
                {
                    c.GTIN = codeBuf.Substring(2, GTINLen);
                    codeBuf = codeBuf.Substring(GTINLen + 2);
                    hasGTIN = true;
                }
                if (true == codeBuf.StartsWith("10", true, null))
                {
                    int sepLen = 1;
                    int len = -1;
                    len = codeBuf.IndexOf('\x1D');
                    if (len == -1) // THis is for Cognex, as its returning seperator different...
                        len = codeBuf.IndexOf('\x25ba');
                    if (len == -1) // THis is for FNC1 ASCII <GS> returned by scanner as seperator...
                    {
                        sepLen = 4;
                        len = codeBuf.IndexOf("<GS>");
                    }
                    if (len == -1) // THis is for HendheldScanner, as its returning seperator different...
                    {
                        int index = 0;
                        sepLen = 1;
                        foreach (char ch in codeBuf)
                        {
                            bool isAlphNum = char.IsLetterOrDigit(ch);

                            if (isAlphNum == false)
                            {
                                len = index;
                                break;
                            }
                            index++;
                        }

                    }

                    // In case if Last String in 2DCode, No seperator available
                    if (len == -1)
                    {
                        len = codeBuf.Length > 22 ? 22 : codeBuf.Length; // 22 (20:Max Len for VarData + 2:AI)
                        string BatchNo = codeBuf.Substring(2, len - 2);
                        c.BatchNo = BatchNo;
                        codeBuf = "";
                        hasLOT = true;
                    }
                    else
                        if (len > 2) // 2 for "10"
                        {
                            c.BatchNo = codeBuf.Substring(2, len - 2); // -2 as started from 3rd char
                            codeBuf = codeBuf.Substring(len + sepLen); // +sepLen for seperator charachter '\x1D'
                            hasLOT = true;
                        }
                }
                if (true == codeBuf.StartsWith("11", true, null))
                {
                    string date = codeBuf.Substring(2, 6);
                    int day = int.Parse(date.Substring(4, 2));
                    int month = int.Parse(date.Substring(2, 2));
                    int year = 2000 + int.Parse(date.Substring(0, 2));
                    int lastDayofMonth = DateTime.DaysInMonth(year, month);
                    c.MfgDate = new DateTime(year, month, day != 0 ? day : lastDayofMonth);
                    codeBuf = codeBuf.Substring(8);
                }
                if (true == codeBuf.StartsWith("17", true, null))
                {
                    string date = codeBuf.Substring(2, 6);
                    int day = int.Parse(date.Substring(4, 2));
                    int month = int.Parse(date.Substring(2, 2));
                    int year = 2000 + int.Parse(date.Substring(0, 2));
                    int lastDayofMonth = DateTime.DaysInMonth(year, month);
                    c.ExpiryDate = new DateTime(year, month, day != 0 ? day : lastDayofMonth);
                    codeBuf = codeBuf.Substring(8);
                    hasEXP = true;
                }
                if (true == codeBuf.StartsWith("21", true, null))
                {
                    ////int len = codeBuf.IndexOf('\x1D');
                    ////if (len > 2) // 2 for "21"
                    ////{
                    ////    c.SerialNo = codeBuf.Substring(2, len - 2); // -2 as started from 3rd char
                    ////    codeBuf = codeBuf.Substring(len + 1); // +1 for seperator charachter '\x1D'
                    ////}
                    //if (codeBuf.Length >= 17)
                    //{
                    //    string serial = codeBuf.Substring(2, 15);
                    //    c.SerialNo = serial;
                    //    codeBuf = codeBuf.Substring(17 + 1);
                    //}
                    //else
                    //{
                    //    string serial = codeBuf.Substring(2);
                    //    c.SerialNo = serial;
                    //    codeBuf = "";
                    //}
                    int len = -1;
                    len = codeBuf.IndexOf('\x1D');
                    if (len == -1) // THis is for Cognex, as its returning seperator different...
                        len = codeBuf.IndexOf('\x25ba');
                    if (len == -1) // THis is for HendheldScanner, as its returning seperator different...
                    {
                        int index = 0;
                        foreach (char ch in codeBuf)
                        {
                            bool isAlphNum = char.IsLetterOrDigit(ch);
                            if (isAlphNum == false)
                            {
                                len = index;
                                break;
                            }
                            index++;
                        }

                    }

                    // In case if Last String in 2DCode, No seperator available
                    if (len == -1)
                    {
                        len = codeBuf.Length > 22 ? 22 : codeBuf.Length; // 22 (20:Max Len for VarData + 2:AI)
                        string SerialNo = codeBuf.Substring(2, len - 2);
                        c.SerialNo = SerialNo;
                        codeBuf = "";
                        hasSRNO = true;
                    }
                    else
                        if (len > 2) // 2 for "21"
                        {
                            c.SerialNo = codeBuf.Substring(2, len - 2); // -2 as started from 3rd char
                            codeBuf = codeBuf.Substring(len + 1); // +1 for seperator charachter '\x1D'
                            hasSRNO = true;
                        }
                }
            }
            if (hasGTIN == true && hasLOT == true && hasEXP == true && hasSRNO == true)
                c.IsGS1 = true;
            else
                c.IsGS1 = false;

            return c;
        }
        #region Updated DataMatrix Verification

        //int i = 1;
        static string AIvalue = "";
        static CodeData c = new CodeData();
        public static CodeData VerifyDM(string Value)
        {
            //CodeData c = new CodeData();
            try
            {
                CodeData cValues = new CodeData();
                if (Value != string.Empty)
                {
                    AIvalue = Value.Substring(0, 2);
                }
                else
                {
                    AIvalue = "";
                }

                switch (AIvalue)
                {
                    case "01":
                        string GTIN = getGTIN(Value);
                        int lenGTIN = GTIN.Length;
                        c.GTIN = GTIN;
                        string rem = Value.Substring(lenGTIN + 2);
                        VerifyDM(rem);
                        break;
                    case "10":
                        string LOT = getBatch(Value);
                        int lenBATCH = LOT.Length;
                        c.BatchNo = LOT;
                        string tm1 = "";
                        string rem1 = Value.Substring(lenBATCH + 2);
                        tm1 = remvGS(rem1);
                        VerifyDM(tm1);
                        break;
                    case "11":
                        string ProdDate = getProdDate(Value);
                        int lenProd = ProdDate.Length;
                        int day = int.Parse(ProdDate.Substring(4, 2));
                        int month = int.Parse(ProdDate.Substring(2, 2));
                        int year = 2000 + int.Parse(ProdDate.Substring(0, 2));
                        int lastDayofMonth = DateTime.DaysInMonth(year, month);
                        c.MfgDate = new DateTime(year, month, day != 0 ? day : lastDayofMonth);
                        string rem2 = Value.Substring(lenProd + 2, (Value.Length - (lenProd + 2)));
                        VerifyDM(rem2);
                        break;
                    case "15":
                        string BestBeforeDate = getBesBef(Value);
                        int lenBesBef = BestBeforeDate.Length;
                        string rem3 = Value.Substring(lenBesBef + 2, (Value.Length - (lenBesBef + 2)));
                        VerifyDM(rem3);
                        break;
                    case "17":
                        string ExpDate = getExp(Value);
                        int lenExp = ExpDate.Length;
                        int dayE = int.Parse(ExpDate.Substring(4, 2));
                        int monthE = int.Parse(ExpDate.Substring(2, 2));
                        int yearE = 2000 + int.Parse(ExpDate.Substring(0, 2));
                        int lastDayofMonthE = DateTime.DaysInMonth(yearE, monthE);
                        c.ExpiryDate = new DateTime(yearE, monthE, dayE != 0 ? dayE : lastDayofMonthE);
                        string rem4 = Value.Substring(lenExp + 2, (Value.Length - (lenExp + 2)));
                        VerifyDM(rem4);
                        break;
                    case "21":
                        string UID = getUID(Value);
                        int lenUID = UID.Length;
                        c.SerialNo = UID;
                        string tm5 = "";
                        if (Value.Length > 20)
                        {
                            string rem5 = Value.Substring(lenUID + 2, (Value.Length - (lenUID + 2)));
                            tm5 = remvGS(rem5);
                            VerifyDM(tm5);
                        }
                        break;

                    default:
                        return c;

                }
            }
            catch (Exception)
            { }
            return c;
        }

        public static string getProdDate(string str)
        {
            string finalProdDate = "";
            try
            {
                for (int i = 2, len = 8; i < len; i++)
                {
                    finalProdDate += str[i];
                }
            }
            catch (Exception)
            { }
            return finalProdDate;
        }

        public static string getExp(string str)
        {
            string finalExpDate = "";
            try
            {
                for (int i = 2, len = 8; i < len; i++)
                {
                    finalExpDate += str[i];
                }
            }
            catch (Exception)
            { }
            return finalExpDate;
        }

        public static string getGTIN(string str)
        {
            string GTINValue = str.Substring(2, 14);
            return GTINValue;
        }

        public static string getBesBef(string str)
        {
            string finalBBDate = "";
            try
            {
                for (int i = 2, len = 8; i < len; i++)
                {
                    finalBBDate += str[i];
                }
            }
            catch (Exception)
            { }
            return finalBBDate;
        }

        public static string getUID(string str) //...This function is used for retriving UID.
        {
            string finalUID = "";
            try
            {
                if (str.Length < 20)
                {
                    for (int j = 2; j < str.Length; j++)
                    {
                        if (str[j].ToString() != "<")
                        {
                            finalUID += str[j];
                        }
                        else
                        {
                            return finalUID;
                        }
                    }
                }
                else
                {
                    for (int i = 2, len = 22; i < len; i++)
                    {
                        if (str[i].ToString() != "<")
                        {
                            finalUID += str[i];
                        }
                        else
                        {
                            return finalUID;
                        }
                    }
                }
            }
            catch (Exception)
            { }
            return finalUID;
        }

        public static string getBatch(string str) //...This function is used for retriving Lot.
        {
            string finalBatchNo = "";
            try
            {
                if (str.Length < 20)
                {
                    for (int j = 2; j < str.Length + 2; j++)
                    {
                        if (str[j].ToString() != "<")
                        {
                            finalBatchNo += str[j];
                        }
                        else
                        {
                            return finalBatchNo;
                        }
                    }
                }
                else
                {
                    for (int i = 2, len = 22; i < len; i++)
                    {
                        if (str[i].ToString() != "<")
                        {
                            finalBatchNo += str[i];
                        }
                        else
                        {
                            return finalBatchNo;
                        }
                    }
                }
            }
            catch (Exception)
            { }
            return finalBatchNo;
        }

        public static string remvGS(string strGSData) //...This function is used for removing <GS> from string read by scanner.
        {
            string testString = strGSData.Substring(0, 4);
            string temp = "";
            string temp2 = "";
            try
            {
                for (int i = 0, len = 4; i < len; i++)
                {
                    temp2 = "";
                    temp2 += strGSData[i];
                    if (temp2 != "<")
                    {
                        if (testString.ToUpper() == "<GS>")
                        {
                            string cutFirst = strGSData.Substring(4);
                            c.IsGS1 = true;
                            return cutFirst;
                        }
                    }
                    else
                    {
                        temp += strGSData[i];
                    }
                }
            }
            catch (Exception)
            { }
            return strGSData;
        }


        #endregion Updated DataMatrix Verification
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        /// 
        public static string DecodeSSCC(string code)
        {
            string printedSSCC = code;

            if (printedSSCC.StartsWith("00") == true)
            {
                printedSSCC = printedSSCC.Substring(2);
            }
            return printedSSCC;
        }

         
        private static bool checkValiddataIndex(object  data, int indexGs1AI)
        {
            bool isValid = true;
            if (data is string)
            {
                string strdata = Convert.ToString(data);
                if (string.IsNullOrEmpty(strdata) == false)
                {
                    if (indexGs1AI == -1)
                        isValid = false;
                }
                else if (indexGs1AI > -1)
                    isValid = false;
            }
            else if (data is DateTime)
            {
                DateTime dt = Convert.ToDateTime(data);
                if (dt != DateTime.MinValue)
                {
                    if (indexGs1AI == -1)
                        isValid = false;
                }
                else if (indexGs1AI > -1)
                    isValid = false;
            }
            return isValid;
        }

        private static string GetGs1CodeFilter(string filter)
        {
            string[] CodeSeq = filter.Split(new string[] { FilterData.fldSep }, StringSplitOptions.None);
            if (CodeSeq.Length > 0)
                return CodeSeq[0];
            return string.Empty;
        }
        public static bool HasValidIdCodeFormat(string iDCode, string GS1Filter, bool iswithUid)
        {
            CodeData data = GS1Mgr.DecodeCode(iDCode, "");
            GS1Filter = GS1Mgr.GetGs1CodeFilter(GS1Filter);

            if (data == null || string.IsNullOrEmpty(GS1Filter) == true)
                return false;

            bool isValid = true;
            int index = -1;

            List<GS1Mgr.GS1AI> lstGS1AISeq = GS1Mgr.GetGS1AISeq(GS1Filter);

            ///to check lot            
            if (isValid == true)
            {
                index = lstGS1AISeq.FindIndex(item => item == GS1AI.LOT);
                isValid = checkValiddataIndex(data.BatchNo, index);
            }

            ///to check GTIN 

            if (isValid == true)
            {
                index = lstGS1AISeq.FindIndex(item => item == GS1AI.GTIN);
                isValid = checkValiddataIndex(data.GTIN, index);
            }

            ///to check EXP           

            if (isValid == true)
            {
                index = lstGS1AISeq.FindIndex(item => item == GS1AI.EXP);
                isValid = checkValiddataIndex(data.ExpiryDate, index);
            }

            ///to check MFG

            if (isValid == true)
            {
                index = lstGS1AISeq.FindIndex(item => item == GS1AI.MFG);
                isValid = checkValiddataIndex(data.MfgDate, index);
            }

            ///to check UID          

            if (isValid == true)
            {
                index = lstGS1AISeq.FindIndex(item => item == GS1AI.UID);
                isValid = checkValiddataIndex(data.SerialNo, index);

                if (iswithUid == true && string.IsNullOrEmpty(data.SerialNo))
                    isValid = false;
                else if (iswithUid == false && string.IsNullOrEmpty(data.SerialNo) == false)
                    isValid = false;
            }

            return isValid;
        }
    }
}
