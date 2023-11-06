using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using TnT.Models;
using TnT.Models.Code;
using TnT.Models.Job;

namespace TnT.DataLayer.UIDService
{
    public class UIDFactory
    {
        int PPBCode = 7;
        int MOCCode = 8;
        int OBXCode = 9;
        int ISHCode = 10;
        int OSHCode = 11;
        int PALCode = 12;
        int SSCCCode = 20;
        private int lengthOfUID = 12;
        private int PAID;
        private int UIDQuantity;

        /// <summary>
        /// size of the level of the jobs
        /// </summary>
        private int MOC, OBX, ISH, PAL, PPB, OSH;
        private int MOCtoPrint, OBXtoPrint, ISHtoPrint, OSHtoPrint, PPBtoPrint, PALtoPrint, SSCCtoPrint;
        private List<string> UIDsPPB, UIdsMOC, UIdsOBX, UIDsISH, UIDsOSH, UIDsPAL, UIDsSSCC;

        private ApplicationDbContext db = new ApplicationDbContext();

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

        private bool IsLevelExisting(int level)
        {
            if (level == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        private int GetLastLevel()
        {
            try
            {


                if (PAL != -1)
                {
                    return PALCode;
                }
                else if (OSH != -1)
                {
                    return OSHCode;
                }

                else if (ISH != -1)
                {
                    return ISHCode;
                }

                else if (OBX != -1)
                {
                    return OBXCode;
                }

                else if (MOC != -1)
                {
                    return MOCCode;
                }
                else if (PPB != -1)
                {
                    return PPBCode;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void CalculateSSCCToPrint()
        {
            try
            {


                int LastLevel;
                LastLevel = GetLastLevel();
                if (LastLevel == PALCode)
                {
                    SSCCtoPrint = PALtoPrint;
                }
                else if (LastLevel == OSHCode)
                {
                    SSCCtoPrint = OSHtoPrint;
                }
                else if (LastLevel == ISHCode)
                {
                    SSCCtoPrint = ISHtoPrint;
                }
                else if (LastLevel == OBXCode)
                {
                    SSCCtoPrint = OBXtoPrint;
                }
                else if (LastLevel == MOCCode)
                {
                    SSCCtoPrint = MOCtoPrint;
                }

                else if (LastLevel == PPBCode)
                {
                    SSCCtoPrint = PPB;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CalculateAndGenerateAndSaveIds(int RequestId,string selectedJobType)
        {
            try
            {

                if (UIDQuantity == 0) return;


                if (IsLevelExisting(PPB))
                {
                    PPBtoPrint = UIDQuantity;
                    if (PPBtoPrint != 0)
                    {
                        SaveIds(null, PPBCode, RequestId, PPBtoPrint, selectedJobType);
                    }
                }

                if (IsLevelExisting(MOC))
                {
                    if (!IsLevelExisting(PPB))
                    {
                        MOCtoPrint = UIDQuantity;
                        SaveIds(null, MOCCode, RequestId, MOCtoPrint, selectedJobType);
                    }
                    else
                    {
                        double res = (float)PPBtoPrint / (float)MOC;
                        MOCtoPrint = (int)Math.Ceiling(res);
                        SaveIds(null, MOCCode, RequestId, MOCtoPrint, selectedJobType);
                    }
                }

                if (IsLevelExisting(OBX))
                {
                    if ((!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                    {
                        OBXtoPrint = UIDQuantity;
                        //UIdsOBX = generateUIDs(OBXtoPrint, lengthOfUID);
                        SaveIds(null, OBXCode, RequestId, OBXtoPrint, selectedJobType);
                    }
                    else
                    {
                        if (!IsLevelExisting(MOC))
                        {
                            if (IsLevelExisting(PPB))
                            {
                                double res = (float)PPBtoPrint / (float)OBX;
                                OBXtoPrint = (int)Math.Ceiling(res);
                                SaveIds(null, OBXCode, RequestId, OBXtoPrint, selectedJobType);
                            }
                        }
                        else
                        {
                            double res = (float)MOCtoPrint / (float)OBX;
                            OBXtoPrint = (int)Math.Ceiling(res);
                            SaveIds(null, OBXCode, RequestId, OBXtoPrint, selectedJobType);
                        }

                    }
                }


                if (IsLevelExisting(ISH))
                {
                    if ((!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                    {
                        ISHtoPrint = UIDQuantity;
                        SaveIds(null, ISHCode, RequestId, ISHtoPrint, selectedJobType);
                    }
                    else
                    {
                        if (!IsLevelExisting(OBX))
                        {
                            if (!IsLevelExisting(MOC))
                            {
                                if (IsLevelExisting(PPB))
                                {
                                    double res = (float)PPBtoPrint / (float)ISH;
                                    ISHtoPrint = (int)Math.Ceiling(res);
                                    SaveIds(null, ISHCode, RequestId, ISHtoPrint, selectedJobType);
                                }
                            }
                            else
                            {
                                double res = (float)MOCtoPrint / (float)ISH;
                                ISHtoPrint = (int)Math.Ceiling(res);
                                SaveIds(null, ISHCode, RequestId, ISHtoPrint, selectedJobType);
                            }

                        }
                        else
                        {
                            double res = (float)OBXtoPrint / (float)ISH;
                            ISHtoPrint = (int)Math.Ceiling(res);
                            SaveIds(null, ISHCode, RequestId, ISHtoPrint, selectedJobType);
                        }
                    }
                }

                if (IsLevelExisting(OSH))
                {
                    if ((!IsLevelExisting(ISH)) && (!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                    {
                        OSHtoPrint = UIDQuantity;
                        SaveIds(null, OSHCode, RequestId, OSHtoPrint, selectedJobType);
                    }
                    else
                    {
                        if (!IsLevelExisting(ISH))
                        {
                            if (!IsLevelExisting(OBX))
                            {
                                if (!IsLevelExisting(MOC))
                                {
                                    if (IsLevelExisting(PPB))
                                    {
                                        double res = (float)PPBtoPrint / (float)OSH;
                                        OSHtoPrint = (int)Math.Ceiling(res);
                                        SaveIds(null, OSHCode, RequestId, OSHtoPrint, selectedJobType);
                                    }
                                }
                                else
                                {
                                    double res = (float)MOCtoPrint / (float)OSH;
                                    OSHtoPrint = (int)Math.Ceiling(res);
                                    SaveIds(null, OSHCode, RequestId, OSHtoPrint, selectedJobType);
                                }
                            }
                            else
                            {
                                double res = (float)OBXtoPrint / (float)OSH;
                                OSHtoPrint = (int)Math.Ceiling(res);
                                SaveIds(null, OSHCode, RequestId, OSHtoPrint, selectedJobType);
                            }
                        }
                        else
                        {
                            double res = (float)ISHtoPrint / (float)OSH;
                            OSHtoPrint = (int)Math.Ceiling(res);
                            SaveIds(null, OSHCode, RequestId, OSHtoPrint, selectedJobType);
                        }
                    }
                }

                if (IsLevelExisting(PAL))
                {
                    if ((!IsLevelExisting(OSH)) && (!IsLevelExisting(ISH)) && (!IsLevelExisting(OBX)) && (!IsLevelExisting(MOC)) && (!IsLevelExisting(PPB)))
                    {
                        PALtoPrint = UIDQuantity;
                        SaveIds(UIDsPAL, PALCode, RequestId, PALtoPrint, selectedJobType);
                    }
                    else
                    {
                        if (!IsLevelExisting(OSH))
                        {
                            if (!IsLevelExisting(ISH))
                            {
                                if (!IsLevelExisting(OBX))
                                {
                                    if (!IsLevelExisting(MOC))
                                    {
                                        if (IsLevelExisting(PPB))
                                        {
                                            double res = (float)PPBtoPrint / (float)PAL;
                                            PALtoPrint = (int)Math.Ceiling(res);
                                            SaveIds(null, PALCode, RequestId, PALtoPrint, selectedJobType);
                                        }
                                    }
                                    else
                                    {
                                        double res = (float)MOCtoPrint / (float)PAL;
                                        PALtoPrint = (int)Math.Ceiling(res);
                                        SaveIds(null, PALCode, RequestId, PALtoPrint, selectedJobType);
                                    }
                                }
                                else
                                {
                                    double res = (float)OBXtoPrint / (float)PAL;
                                    PALtoPrint = (int)Math.Ceiling(res);
                                    SaveIds(null, PALCode, RequestId, PALtoPrint, selectedJobType);
                                }
                            }
                            else
                            {
                                double res = (float)ISHtoPrint / (float)PAL;
                                PALtoPrint = (int)Math.Ceiling(res);
                                SaveIds(null, PALCode, RequestId, PALtoPrint, selectedJobType);
                            }
                        }
                        else
                        {
                            double res = (float)OSHtoPrint / (float)PAL;
                            PALtoPrint = (int)Math.Ceiling(res);
                            SaveIds(null, PALCode, RequestId, PALtoPrint, selectedJobType);
                        }

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }



        private string generateSingleUid(string selectedJobType)
        {
            try
            {
                List<string> newUid = generateUIDs(1, lengthOfUID, selectedJobType);
                return newUid.FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private List<string> checkNewlyGneretedUidsExistAndAdd(List<string> FirstLstGenerated, List<string> rplcmtGeneratedIds,string selectedJobType)
        {
            try
            {
                bool isExisting = false;

                foreach (var item in rplcmtGeneratedIds)
                {
                    isExisting = FirstLstGenerated.Contains(item);
                    if (isExisting)
                    {
                        string newUid = generateSingleUid(selectedJobType);
                        if (!FirstLstGenerated.Contains(newUid))
                        {
                            rplcmtGeneratedIds.Remove(item);
                            FirstLstGenerated.Add(newUid);
                            //generateSingleUid();
                        }

                    }
                }

                return FirstLstGenerated;
            }
            catch (Exception)
            {

                throw;
            }

        }


        private void SaveIds(List<string> LstToSave, int CodeType, int RequestId, int QtyToGenerate,string selectedJobType)
        {
            try
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
                string output = string.Join(",", LstToSave.ToArray());
                X_Code code = new X_Code();
                code.Code = output;
                code.RequestId = RequestId;
                code.CodeType = CodeType;
                db.X_Code.Add(code);
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }



        }

     
        private void GenerateAndSaveSSCC(int JobId, int RequestId, string selectedJobType, string IAC_CIN)
        {
            try
            {
                IDGenrationFactory obj = new IDGenrationFactory();
                UIDsSSCC = new List<string>();
                UIDsSSCC = obj.generateSSCC(JobId, SSCCtoPrint, selectedJobType, IAC_CIN);
                if (UIDsSSCC.Count > 0)
                {

                    //JavaScriptSerializer jss = new JavaScriptSerializer();
                    //string output = jss.Serialize(UIDsSSCC);
                    string output = string.Join(",", UIDsSSCC.ToArray());

                    X_Code code = new X_Code();
                    code.Code = output;
                    code.RequestId = RequestId;
                    code.CodeType = SSCCCode;
                    db.X_Code.Add(code);
                    db.SaveChanges();

                    //foreach (var id in UIDsSSCC)
                    //{
                    //    X_Code code = new X_Code();
                    //    code.RequestId = RequestId;
                    //    code.Code = id;
                    //    code.CodeType = SSCCCode;
                    //    db.X_Code.Add(code);
                    //    db.SaveChanges();
                    //}
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void getProductData(int PAID)
        {
            try
            {
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                string query = @"SELECT DISTINCT TOP(6) PackageTypeCode, Size, GTIN, LastUpdatedDate
                            FROM            dbo.PackagingAssoDetails
                            WHERE(PAID = " + PAID + ")  ORDER BY LastUpdatedDate DESC ";
                DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, query);

                if (ds.Tables[0].Rows.Count > 0)
                {

                    var dr = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("PackageTypeCode") == "MOC").FirstOrDefault();

                    if (dr != null)
                    {
                        MOC = Convert.ToInt32(dr[1].ToString());
                    }
                    else
                    {
                        MOC = -1;
                    }

                    var dr1 = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("PackageTypeCode") == "PPB").FirstOrDefault();
                    if (dr1 != null)
                    {
                        PPB = Convert.ToInt32(dr1[1].ToString());
                    }
                    else
                    {
                        PPB = -1;
                    }

                    var dr2 = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("PackageTypeCode") == "OBX").FirstOrDefault();
                    if (dr2 != null)
                    {
                        OBX = Convert.ToInt32(dr2[1].ToString());
                    }
                    else
                    {
                        OBX = -1;
                    }

                    var dr3 = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("PackageTypeCode") == "ISH").FirstOrDefault();
                    if (dr3 != null)
                    {
                        ISH = Convert.ToInt32(dr3[1].ToString());
                    }
                    else
                    {
                        ISH = -1;
                    }

                    var dr4 = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("PackageTypeCode") == "OSH").FirstOrDefault();
                    if (dr4 != null)
                    {
                        OSH = Convert.ToInt32(dr4[1].ToString());
                    }
                    else
                    {
                        OSH = -1;
                    }

                    var dr5 = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("PackageTypeCode") == "PAL").FirstOrDefault();
                    if (dr5 != null)
                    {
                        PAL = Convert.ToInt32(dr5[1].ToString());
                    }
                    else
                    {
                        PAL = -1;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        public int getSurplusUidQty(int jobId)
        {
            try
            {
                string qry = "SELECT SurPlusQty FROM dbo.Job WHERE(JID = " + jobId + ")";
                return getNumberUtility(qry);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int getUIDQuantity(int jobId)
        {
            try
            {

                string qry = "SELECT Quantity FROM dbo.Job WHERE(JID = " + jobId + ")";
                return getNumberUtility(qry);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int getPAID(int jobId)
        {
            try
            {
                string Query = "SELECT        PAID FROM dbo.Job WHERE(JID = " + jobId + ")";
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, Query);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void generateIds(int RequestId, int JobId, int PAID)
        {
            try
            {
                UIDQuantity = (getSurplusUidQty(JobId) + getUIDQuantity(JobId));
                this.PAID = PAID; // getPAID(JobId);
                getProductData(this.PAID);

                ///generate UIDs for product code
                if (UIDQuantity != 0)
                {
                    CalculateAndGenerateAndSaveIds(RequestId,"");
                }
                ///Generate and Save SSCC
                CalculateSSCCToPrint();
                GenerateAndSaveSSCC(JobId, RequestId, "DGFT", "-");
            }
            catch (Exception)
            {

                throw;
            }


        }


        private int getNumberUtility(string Query)
        {
            try
            {
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, Query);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }



    }
}