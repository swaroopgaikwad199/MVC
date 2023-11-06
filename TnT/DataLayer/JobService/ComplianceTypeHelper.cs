using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.DataLayer.JobService
{
    public enum BatchComplianceType
    {
        India_DGFT,
        Germany_PPN,
        USA_DSCSA,
        France_CIP,
        Non_GS1,
        RUSSIA,
        SOUTHKOREA,
        CHINACODE,
        SAUDIARABIA,
        TURKEY,
        EU,
        NA
    }
    public class BatchComplianceTypeHelper
    {
        private List<string> LstToSave = new List<string>();
        private int lengthOfUID = 12;
        private Dictionary<string, string> masterList = new Dictionary<string, string>();




        #region Compliances

        public Dictionary<string, string> commonComplianceSet(string pkgTypeCode, int QtyToGenerate,string selectedJobType)
        {
            UIDVerifier verifier = new UIDVerifier();
            int i = 1;

            while (i <= QtyToGenerate)
            {
                string Uid = generateUIDs(1, lengthOfUID, selectedJobType).FirstOrDefault();
                if (verifier.AddCodeGen(Uid))
                {
                    i++;
                }
            }
            LstToSave = verifier.getUniqueIds();

            Dictionary<string, string> ids = new Dictionary<string, string>();

            foreach (var item in LstToSave)
            {
                ids.Add(item, pkgTypeCode);
            }
            return masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);
        }

        public Dictionary<string, string> complianceCIP(string pkgTypeCode, int QtyToGenerate)
        {
            UIDVerifier verifier = new UIDVerifier();
            int i = 1;

            while (i <= QtyToGenerate)
            {
                string Uid = GEtDummyUID(i, pkgTypeCode);
                if (verifier.AddCodeGen(Uid))
                {
                    i++;
                }
            }
            LstToSave = verifier.getUniqueIds();

            Dictionary<string, string> ids = new Dictionary<string, string>();

            foreach (var item in LstToSave)
            {
                ids.Add(item, pkgTypeCode);
            }
            return masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);

        }

        //public Dictionary<string, string> complianceNonGS1(string pkgTypeCode, int qtyToGenerate, BuildingLocation bldgLoc, string lineCode, string lastSerialNo, string shiftCode, string sizeCode)
        //{


        //    if (!(pkgTypeCode == "MOC" || pkgTypeCode == "ISH" || pkgTypeCode == "PAL"))
        //    {
        //        return null;
        //    }
        //    string democode = string.Empty;
        //    if (string.IsNullOrEmpty(lastSerialNo))
        //    {
        //        if (pkgTypeCode == "MOC")
        //        {
        //            lastSerialNo = "INMFLA1B000000000000001";
        //        }
        //        else if (pkgTypeCode == "ISH")
        //        {
        //            lastSerialNo = "INMFLA1B000000000001";
        //        }
        //        else if (pkgTypeCode == "PAL")
        //        {
        //            lastSerialNo = "INMFL010117A1111BBBA1000001";
        //        }
        //    }
        //    int initialSerialNumber = 0;
        //    int totalQty = 0;
        //    UIDVerifier verifier = new UIDVerifier();
        //    if (pkgTypeCode == "MOC")
        //    {
        //        initialSerialNumber = Convert.ToInt32(lastSerialNo.Remove(0, 8));
        //        totalQty = qtyToGenerate + initialSerialNumber;
        //        while (initialSerialNumber <= totalQty)
        //        {
        //            var SeqSerialNumber = initialSerialNumber.ToString().PadLeft(15, '0');
        //            string Uid = bldgLoc.CountryCode + bldgLoc.MFGCode + bldgLoc.BuildingCode + lineCode + SeqSerialNumber;
        //            if (verifier.AddCodeGen(Uid))
        //            {
        //                initialSerialNumber++;
        //            }
        //        }
        //    }
        //    if (pkgTypeCode == "ISH")
        //    {
        //        initialSerialNumber = Convert.ToInt32(lastSerialNo.Remove(0, 8));
        //        totalQty = qtyToGenerate + initialSerialNumber;
        //        while (initialSerialNumber <= totalQty)
        //        {
        //            var SeqSerialNumber = initialSerialNumber.ToString().PadLeft(12, '0');
        //            string Uid = bldgLoc.CountryCode + bldgLoc.MFGCode + bldgLoc.BuildingCode + lineCode + SeqSerialNumber;
        //            if (verifier.AddCodeGen(Uid))
        //            {
        //                initialSerialNumber++;
        //            }
        //        }
        //    }
        //    if (pkgTypeCode == "PAL")
        //    {
        //        initialSerialNumber = Convert.ToInt32(lastSerialNo.Remove(0, 21));
        //        totalQty = qtyToGenerate + initialSerialNumber;
        //        while (initialSerialNumber <= totalQty)
        //        {
        //            var SeqSerialNumber = initialSerialNumber.ToString().PadLeft(6, '0');
        //            //string uuu = "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}";
        //            string Uid = bldgLoc.CountryCode + bldgLoc.MFGCode + DateTime.Now.ToString("ddmmyy") + shiftCode + bldgLoc.BrandID + sizeCode + bldgLoc.BuildingCode + SeqSerialNumber;
        //            if (verifier.AddCodeGen(Uid))
        //            {
        //                initialSerialNumber++;
        //            }
        //        }
        //    }




        //    LstToSave = verifier.getUniqueIds();
        //    Dictionary<string, string> ids = LstToSave.ToDictionary(pair => pair, pair => pkgTypeCode);
        //    return ids;
        //}


        #endregion


        #region utils

        private List<string> generateUIDs(int Quantity, int LengthOfUid,string selectedJobType)
        {
            try
            {
                IDGenrationFactory obj = new IDGenrationFactory();
                return obj.generateIDs(Quantity, LengthOfUid, selectedJobType);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GEtDummyUID(int i, string PackageTypeCode)
        {
            if (PackageTypeCode == "OBX")
            {
                PackageTypeCode = "0BX";
            }

            if (PackageTypeCode == "OSH")
            {
                PackageTypeCode = "ASH";
            }

            string UId = PackageTypeCode;

            UId = UId + i.ToString().PadLeft(9, '0');
            return UId;
        }


        public static BatchComplianceType convertToComplianceType(string compliance)
        {
            BatchComplianceType ct;
            if (compliance == "DGFT")
            {
                ct = BatchComplianceType.India_DGFT;
            }
            else if (compliance == "PPN")
            {
                ct = BatchComplianceType.Germany_PPN;
            }
            else if (compliance == "DSCSA")
            {
                ct = BatchComplianceType.USA_DSCSA;
            }
            else if (compliance == "CIP")
            {
                ct = BatchComplianceType.France_CIP;
            }
            else if(compliance== "RUSSIA")
            {
                ct = BatchComplianceType.RUSSIA;
            }
            else if (compliance == "SOUTH_KOREA")
            {
                ct = BatchComplianceType.SOUTHKOREA;
            }
            else if (compliance == "CHINACODE")
            {
                ct = BatchComplianceType.CHINACODE;
            }
            else if (compliance == "SAUDI_ARABIA")
            {
                ct = BatchComplianceType.SAUDIARABIA;
            }
            else if (compliance == "TURKEY")
            {
                ct = BatchComplianceType.TURKEY;
            }
            else if (compliance == "EU")
            {
                ct = BatchComplianceType.EU;
            }
            //else if (compliance == "Non-GS1")
            //{
            //    ct = BatchComplianceType.Non_GS1;
            //}
            else
            {
                ct = BatchComplianceType.NA;
            }
            return ct;
        }
        #endregion
    }
}