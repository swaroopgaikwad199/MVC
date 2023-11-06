using REDTR.DB.BLL;
using REDTR.DB.BusinessObjects;
using REDTR.HELPER;
using REDTR.UTILS.SystemIntegrity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using TnT.Models;

namespace TnT.DataLayer.DAVAService
{
    class DAVAUtility
    {
        public DAVAStatus CreateProductDAVA()
        {
            DataTable dtPA = new DataTable("PackageAsso");
            DbHelper m_dbhelper = new DbHelper();
            dtPA = m_dbhelper.DBManager.PackagingAssoBLL.PackagingAssoDAO.GetProductdataTableforXML((int)PackagingAssoBLL.PackagingAssoOp.GetProductforXML, null, null);
            if (dtPA.Rows.Count > 0)
            {
                DAVAProductHelper davaHelper = new DAVAProductHelper();
                return davaHelper.CreateProduct(dtPA);
            }
            else
            {
                return DAVAStatus.NoProductsAvailable;
            }
        }
        

        public DAVAStatus CreateBatchDAVA(bool IsAllBatches, string BatchNo, bool IsExemptBarcode, string Country, string CountryCode,string EXEMPTION_NOTIFICATION_AND_DATE)
        {
            DataTable dt = new DataTable("Batch");
            DbHelper m_dbhelper = new DbHelper();

            if (IsAllBatches)
            {
                dt = m_dbhelper.DBManager.JobBLL.JobDAO.GetBatchDataSetforXML((int)JobBLL.JobOp.GetAllBatchForDavaPortal, null, null, null);
               
            }
            else
            {
                dt = m_dbhelper.DBManager.JobBLL.JobDAO.GetBatchDataSetforXML((int)JobBLL.JobOp.GetSingleBatchForDavaPortal, BatchNo, null, null);
            }

            if (dt.Rows.Count == 0)
            {
                return DAVAStatus.NoBatchesAvailable;
            }
            else
            {
                DAVABatchHelper davaHelper = new DAVABatchHelper();
                return davaHelper.createBatchDAVA(dt, IsAllBatches, IsExemptBarcode, Country, CountryCode, EXEMPTION_NOTIFICATION_AND_DATE);
            }

        }
        
        int ChildCount = 0;
        public DAVAStatus CreateProductionDAVA(decimal JObId, string BatchNo, bool IsWholeBatch, List<string> cods)
        {

            DbHelper m_dbhelper = new DbHelper();
            List<Job> LstJb = new List<Job>();
            LstJb = m_dbhelper.DBManager.JobBLL.GetJobs(JobBLL.JobOp.GetJobOfBatch, -1, BatchNo, null);

            if (LstJb.Count > 0)
            {
                DAVAProductionHelper davaHelper = new DAVAProductionHelper();
                if (IsWholeBatch)
                {
                    DAVAStatus sts;
                    var sscc = getSSCCCodes(JObId);
                    var pkgDtls = getPDSSCCforDAVA(sscc, JObId);
                    if (sscc.Count == 0 && pkgDtls == null) {
                        sts = DAVAStatus.NoProductsAvailable;
                    } else { 
                        sts = davaHelper.CreateProductionDAVA(BatchNo, LstJb, pkgDtls, true, false, null);
                    }
                    return sts;
                }
                else
                {
                    var pkgDtls = getPDSSCCforDAVA(cods, JObId);
                    return davaHelper.CreateProductionDAVA(BatchNo, LstJb, pkgDtls, true, false, null);
                }
            }
            else
            {
                return DAVAStatus.NoProductionDataAvailable;
            }
        }

        public DAVAStatus CreateProductionDAVATNS(decimal JObId, string BatchNo, bool IsWholeBatch, List<string> cods)
        {

            DbHelper m_dbhelper = new DbHelper();
            List<Job> LstJb = new List<Job>();
            LstJb = m_dbhelper.DBManager.JobBLL.GetJobs(JobBLL.JobOp.GetJobOfBatch, -1, BatchNo, null);

            if (LstJb.Count > 0)
            {
                DAVAProductionHelper davaHelper = new DAVAProductionHelper();
                if (IsWholeBatch)
                {
                    DAVAStatus sts;
                    var sscc = getSSCCCodes(JObId);
                    var pkgDtls = getPDSSCCforDAVA(sscc, JObId);
                    if (sscc.Count == 0 && pkgDtls == null)
                    {
                        sts = DAVAStatus.NoProductsAvailable;
                    }
                    else
                    {
                        sts = davaHelper.CreateProductionDAVATNS(BatchNo, LstJb, pkgDtls, true, false, null);
                    }
                    return sts;
                }
                else
                {
                    var pkgDtls = getPDSSCCforDAVA(cods, JObId);
                    return davaHelper.CreateProductionDAVATNS(BatchNo, LstJb, pkgDtls, true, false, null);
                }
            }
            else
            {
                return DAVAStatus.NoProductionDataAvailable;
            }
        }

 

        public DAVAStatus CreateTertiaryExemption(decimal JObId, string BatchNo,bool IsWholeBatch, bool IsExempBarcode,string ExemptionCode,string CountryCode, List<string> cods)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            DAVATertiaryExemptionHelper davaHelper = new DAVATertiaryExemptionHelper();
            int lvl = db.JobDetails.Where(x => x.JD_JobID == JObId).Select(x => x.JD_JobID).Count();
            if (IsWholeBatch)
            {
                var sscc = getSSCCCodes(JObId);
                var pkgDtls = getPDSSCCforDAVA(sscc, JObId);
                return davaHelper.CreateTerExempDAVA((int)JObId, BatchNo, pkgDtls, IsExempBarcode, ExemptionCode, CountryCode,lvl);
            }
            else
            {
                var pkgDtls = getPDSSCCforDAVA(cods, JObId);
                return davaHelper.CreateTerExempDAVA((int)JObId, BatchNo, pkgDtls, IsExempBarcode, ExemptionCode, CountryCode,lvl);
            }
        }



        #region Utils

        private List<string> getSSCCCodes(decimal JobId)
        {
            List<string> codes = new List<string>();
            DbHelper m_dbhelper = new DbHelper();
            List<JobDetails> LstJobDetails = m_dbhelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, JobId, 1);
            List<PackagingDetails> LstTertioryPackagingDetails = new List<PackagingDetails>();
            List<PackagingDetails> LstBatchData = new List<PackagingDetails>();

            if (LstJobDetails != null)
            {
                LstBatchData = new List<PackagingDetails>();
                string Query = "SELECT * FROM PACKAGINGDETAILS WHERE (ISDECOMISSION IS NULL OR ISDECOMISSION =0) AND ISREJECTED IS NOT NULL " +
                                " AND (DAVAPORTALUPLOAD IS NULL OR DAVAPORTALUPLOAD =0) AND JOBID =" + JobId;
                LstBatchData = m_dbhelper.DBManager.PackagingDetailsBLL.GetPackagingDetailss(Query);

                var sortedLvls = ProductPackageHelper.sorttheLevels(LstJobDetails.Select(x => x.JD_Deckcode).ToList());
                var tertiaryDec = sortedLvls[sortedLvls.Count - 1];

                LstTertioryPackagingDetails = LstBatchData.FindAll(item => item.PackageTypeCode == tertiaryDec);

                foreach (PackagingDetails Teriory in LstTertioryPackagingDetails)
                {
                    ChildCount = 0;
                    GetChildCount(Teriory.Code, LstBatchData, true);
                    if (Teriory != null)
                        codes.Add(Teriory.SSCC);
                }
                return codes;
            }
            else
            {
                return null;
            }
        }

        private void GetChildCount(string NextLevelCode, List<PackagingDetails> LstDetails, bool AddTertoryCount)
        {
            try
            {
                List<PackagingDetails> LstPckDetails = LstDetails.FindAll(item => item.NextLevelCode == NextLevelCode);
                if (LstPckDetails.Count > 0 && AddTertoryCount == false)
                {
                    ChildCount = ChildCount + LstPckDetails.Count;
                }
                if (LstPckDetails.Count == 0)
                    return;
                if (LstPckDetails[0].PackageTypeCode == DECKs.MOC.ToString())
                    return;
                foreach (PackagingDetails Pck in LstPckDetails)
                {
                    GetChildCount(Pck.Code, LstDetails, false);
                }
            }
            catch (Exception ex)
            {
                //Trace.TraceError("Error GetChildCount :" + ex.Message);
                //return 0;
            }
        }

        private List<PackagingDetails> getPDSSCCforDAVA(List<string> SSCCs, decimal JobId)
        {
            
            if (SSCCs.Count() > 0)
            {
                DbHelper m_dbhelper = new DbHelper();
                List<PackagingDetails> SetWholeSSCC = new List<PackagingDetails>();

                List<PackagingDetails> LstBatchData = new List<PackagingDetails>();
                string Query = "SELECT * FROM PACKAGINGDETAILS WHERE (ISDECOMISSION IS NULL OR ISDECOMISSION =0) AND ISREJECTED IS NOT NULL " +
                                " AND (DAVAPORTALUPLOAD IS NULL OR DAVAPORTALUPLOAD =0) AND JOBID =" + JobId;
                LstBatchData = m_dbhelper.DBManager.PackagingDetailsBLL.GetPackagingDetailss(Query);

                foreach (var code in SSCCs)
                {
                    var TertioryRecords = LstBatchData.Find(item => item.SSCC == code);
                    SetWholeSSCC.Add(TertioryRecords);
                }
                return SetWholeSSCC;
            }
            else
            {
                return null;
            }


        }


        #endregion
        public static string getFileLocationToStore()
        {
            //AppSettingsReader MyReader = new AppSettingsReader();
            string Path = Utilities.getAppSettings("DAVAFileLctn"); //MyReader.GetValue("DAVAFileLctn", typeof(string)).ToString();
            return Path;
        }

        public static string getCompanyCode()
        {
            Settings CompanInfo = null;
            DbHelper m_dbhelper = new DbHelper();
            List<Settings> Set = m_dbhelper.DBManager.SettingsBLL.GetSettingss();
            if (Set != null && Set.Count > 0)
            {
                CompanInfo = Set[0];
                return CompanInfo.CompanyCode;
            }
            else
            {
                return string.Empty;
            }
        }


    


    }
}