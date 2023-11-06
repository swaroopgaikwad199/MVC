using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.DataLayer.RFXCELService
{
    public enum UIDCustomType
    {
        TLINK,
        PROP,
        RFXL,
        XYZ,
        DAWA,
        RSA,
        SAP,
        TKEY,
        NA

        //TLINK,
        //RFXCEL,
        //PROPIX,
        //XYZ,
        //DAWA,
        //NA,
        //TKEY
    }
    public class UIDCustomTypeHelper
    {
        public static UIDCustomType convertToUIDCustomType(string compliance)
        {
            UIDCustomType ct;
            if (compliance == "TLINK")
            {
                ct = UIDCustomType.TLINK;
            }
            else if (compliance == "TKEY")
            {
                ct = UIDCustomType.TKEY;
            }
            else if (compliance == "RFXL")
            {
                ct = UIDCustomType.RFXL;
            }
            else if (compliance == "PROP")
            {
                ct = UIDCustomType.PROP;
            }
            else if (compliance == "XYZ")
            {
                ct = UIDCustomType.XYZ;
            }
            else if (compliance == "DAWA")
            {
                ct = UIDCustomType.DAWA;
            }
            else if (compliance == "RSA")
            {
                ct = UIDCustomType.RSA;
            }
            else if (compliance == "SAP")
            {
                ct = UIDCustomType.SAP;
            }
            else
            {
                ct = UIDCustomType.NA;
            }
            return ct;
        }
    }
}