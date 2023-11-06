using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPCIS_XMLs_Generation;
using System.Data;
using REDTR.HELPER;
using TnT.Models;

namespace EPCIS_XMLs_Generation
{
    public class EPCISConfig
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private DbHelper dbhelper = new DbHelper();
        public string GetEPCSGTIN(string GTIN, string SRNO)
        {
            string EPCISSGTIN = "urn:epc:id:sgtin:";
            string IndicatorDigit, GS1ComPrepix, ItemRef, CheckDigit;
            IndicatorDigit = GTIN.Substring(0, 1);  //1
            GS1ComPrepix = GTIN.Substring(1, 7);    //2345678
            ItemRef = GTIN.Substring(8, 5);         //90123
            CheckDigit = GTIN.Substring(12, 1);     //1
            EPCISSGTIN += GS1ComPrepix + "." + IndicatorDigit + ItemRef + "." + SRNO;
            return EPCISSGTIN.Trim();
        }

        public string GetEPCSGTIN(string GTIN, string SRNO, int CompCodeLen)
        {
            string EPCISSGTIN = "urn:epc:id:sgtin:";
            string IndicatorDigit, GS1ComPrepix, ItemRef, CheckDigit;
            IndicatorDigit = GTIN.Substring(0, 1);  //1
            GS1ComPrepix = GTIN.Substring(1, CompCodeLen);    //2345678
            ItemRef = GTIN.Substring(CompCodeLen + 1, 5 - (CompCodeLen - 7));         //90123
            CheckDigit = GTIN.Substring(12, 1);     //1
            EPCISSGTIN += GS1ComPrepix + "." + IndicatorDigit + ItemRef + "." + SRNO;
            return EPCISSGTIN.Trim();
        }

        public string GetEPCClassSGTIN(string GTIN, int CompanyCodeLength)
        {
            string EPCISSGTIN = "urn:epc:idpat:sgtin:";
            string IndicatorDigit, GS1ComPrepix, ItemRef, CheckDigit;
            IndicatorDigit = GTIN.Substring(0, 1);  //1
            GS1ComPrepix = GTIN.Substring(1, CompanyCodeLength);    //2345678
            ItemRef = GTIN.Substring(CompanyCodeLength + 1, (12 - CompanyCodeLength));         //90123
            CheckDigit = GTIN.Substring(12, 1);     //1
            EPCISSGTIN += GS1ComPrepix + "." + IndicatorDigit + ItemRef + ".*";
            return EPCISSGTIN.Trim();
        }

        public string GetEPCSSCC(string SSCC)
        {
            string EPCISSSCC = "urn:epc:id:sscc:";
            string ExtensionDigit, GS1ComPrepix, SerialRef, CheckDigit;
            ExtensionDigit = SSCC.Substring(0, 1);
            GS1ComPrepix = SSCC.Substring(1, 7);
            SerialRef = SSCC.Substring(8, 9);
            CheckDigit = SSCC.Substring(17, 1);
            EPCISSSCC += GS1ComPrepix + "." + ExtensionDigit + SerialRef;
            return EPCISSSCC.Trim();
        }

        public string GetEPCGLN(string GLN, string Ext, int compCode)
        {
            string EPCISSGLN = "urn:epc:id:sgln:";
            string GS1ComPrepix, LocationRef, Extension;
            int locReflength = GLN.Length - compCode;
            GS1ComPrepix = GLN.Substring(0, compCode); //1234567
            LocationRef = GLN.Substring(compCode, locReflength - 1); //89012
            Extension = GLN.Substring(GLN.Length - 1, 1); //3
            EPCISSGLN += GS1ComPrepix + "." + LocationRef + "." + Ext;
            return EPCISSGLN.Trim();
        }

        public string GetEPCGLN(int custid)
        {
            var cust = db.M_Customer.Where(x => x.Id == custid).FirstOrDefault();
            string GLN = cust.BizLocGLN;
            string Ext = cust.BizLocGLN_Ext;
            int compCode = cust.CompanyCode.Length;
            int locReflength = GLN.Length - compCode;
            string EPCISSGLN = "urn:epc:id:sgln:";
            string GS1ComPrepix, LocationRef, Extension;
            GS1ComPrepix = GLN.Substring(0, compCode); //1234567
            LocationRef = GLN.Substring(compCode, locReflength-1); //89012
            Extension = GLN.Substring(GLN.Length-1, 1); //3
            EPCISSGLN += GS1ComPrepix + "." + LocationRef + "." + Ext;

            return EPCISSGLN.Trim();
        }

        public string GetTimeZoneOffset()
        {
            int day = TimeZoneInfo.Local.BaseUtcOffset.Days;
            int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
            int minutes = TimeZoneInfo.Local.BaseUtcOffset.Minutes;
            string localZone = "";
            if (hours > 0)
                localZone = string.Format("+{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
            else if (hours < 0)
                localZone = string.Format("-{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
            return localZone;
        }

        public string GetEPCGLN(string GLN, string Ext)
        {
            //string EPCISSGLN = "urn:epc:id:sgln:";
            //string GS1ComPrepix, LocationRef, Extension;
            //GS1ComPrepix = GLN.Substring(0, 7); //1234567
            //LocationRef = GLN.Substring(7, 5); //89012
            //Extension = GLN.Substring(11, 1); //3
            //EPCISSGLN += GS1ComPrepix + "." + LocationRef + "." + Ext;

            //return EPCISSGLN.Trim();
            ApplicationDbContext db = new ApplicationDbContext();
            var companyData = db.Settings.FirstOrDefault();
            int compCode = companyData.CompanyCode.Length;
            int locReflength = GLN.Length - compCode;
            string EPCISSGLN = "urn:epc:id:sgln:";
            string GS1ComPrepix, LocationRef, Extension;
            GS1ComPrepix = GLN.Substring(0, compCode); //1234567
            LocationRef = GLN.Substring(compCode, locReflength - 1); //89012
            Extension = GLN.Substring(GLN.Length - 1, 1); //3
            EPCISSGLN += GS1ComPrepix + "." + LocationRef + "." + Ext;

            return EPCISSGLN.Trim();
        }
    }
}
