using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.Models;

namespace TnT.DataLayer.EPCISHelper
{
    public class EPCIS_Helper
    {
    }

    public class EPCISConfig
    {
        
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

        public string GetEPCGLN(string GLN, string Ext)
        {
            string EPCISSGLN = "urn:epc:id:sgln:";
            string GS1ComPrepix, LocationRef, Extension;
            if (GLN != null)
            {
                GS1ComPrepix = GLN.Substring(0, 7); //1234567
                LocationRef = GLN.Substring(7, 5); //89012
                Extension = GLN.Substring(11, 1); //3
                EPCISSGLN += GS1ComPrepix + "." + LocationRef + "." + Ext;
            }
            return EPCISSGLN.Trim();
        }

        public string GetEPCGLN(int custid)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var cust = db.M_Customer.Where(x => x.Id == custid).FirstOrDefault();
            string GLN = cust.BizLocGLN;
            string Ext = cust.BizLocGLN_Ext;
            int compCode = cust.CompanyCode.Length;
            int locReflength = GLN.Length - compCode;
            string EPCISSGLN = "urn:epc:id:sgln:";
            string GS1ComPrepix, LocationRef, Extension;
            GS1ComPrepix = GLN.Substring(0, compCode); //1234567
            LocationRef = GLN.Substring(compCode, locReflength - 1); //89012
            Extension = GLN.Substring(GLN.Length - 1, 1); //3
            EPCISSGLN += GS1ComPrepix + "." + LocationRef + "." + Ext;

            return EPCISSGLN.Trim();
        }

        public string GetEPCGLN(string GLN, string Ext, int companyLength)
        {
            int locReflength = GLN.Length - companyLength;
            string EPCISSGLN = "urn:epc:id:sgln:";
            string GS1ComPrepix, LocationRef, Extension;
            if (GLN != null)
            {
                GS1ComPrepix = GLN.Substring(0, companyLength); //1234567
                LocationRef = GLN.Substring(companyLength, locReflength - 1); //89012
                Extension = GLN.Substring(GLN.Length - 1, 1); //3
                EPCISSGLN += GS1ComPrepix + "." + LocationRef + "." + Ext;
            }
            return EPCISSGLN.Trim();
        }

    }


}