using REDTR.DB.BLL;
using REDTR.DB.BusinessObjects;
using REDTR.HELPER;
using REDTR.UTILS.SystemIntegrity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.Models.DAVAPortal;

namespace TnT.DataLayer.DAVAService
{
    public class DService
    {
        int ChildCount = 0;

        public IEnumerable<SelectListItem> getExemptionCodes()
        {
            List<SelectListItem> ecodes = new List<SelectListItem>();
           // ecodes.Add(new SelectListItem() { Value = "E21", Text = "E21" });
            //ecodes.Add(new SelectListItem() { Value = "EME", Text = "EME" });
            ecodes.Add(new SelectListItem() { Value = "ELL", Text = "ELL" });

            ecodes.Insert(0, new SelectListItem() { Value = "0", Text = TnT.LangResource.GlobalRes.JobCreateDrpCode });
            return ecodes;
        }


        public IEnumerable<SelectListItem> getCountries()
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            DbHelper m_dbhelper = new DbHelper();
            DataTable dt = new DataTable();
            var cntrys = m_dbhelper.DBManager.CountryBLL.CountryDAO.GetCountrys();

            foreach (var cntry in cntrys)
            {
                countries.Add(new SelectListItem() { Value = cntry.TwoLetterAbbreviation, Text = cntry.CountryName });
            }            
            countries.Insert(0, new SelectListItem() { Value = "0", Text = TnT.LangResource.GlobalRes.ProductDrpSelectCountry });
            return countries;
        }

        public IEnumerable<SelectListItem> getDAVABatches()
        {
            List<SelectListItem> btcs = new List<SelectListItem>();
            DbHelper m_dbhelper = new DbHelper();
            DataTable dt = new DataTable();
            dt = m_dbhelper.DBManager.JobBLL.JobDAO.GetBatch_ProductCode((int)JobBLL.JobOp.GetClosedBatches, null, null, 1);
            foreach (DataRow item in dt.Rows)
            {
                btcs.Add(new SelectListItem() { Value = item["JID"].ToString(), Text = item["BatchNo"].ToString() });
            }
            btcs.Insert(0, new SelectListItem() { Value = "0", Text = TnT.LangResource.GlobalRes.RptJobFilterSelectBatch });
            return btcs;
        }
        
        public SSCCsViewModel getSSCCGrid(string SSCC, int JobId, string BatchNo)
        {
            SSCCsViewModel vm = new SSCCsViewModel();
            List<SSCCDetails> ListSSCCs = new List<SSCCDetails>();
            List<PackagingDetails> LstBatchData = new List<PackagingDetails>();
            string TertiaryDeck = "";
            string PrimaryDeck = "";
            List<Job> LstJb = new List<Job>();
            try
            {

                DbHelper m_dbhelper = new DbHelper();
                DataTable dt = new DataTable();
                List<JobDetails> LstJobDetails = m_dbhelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, JobId, 1);
                List<PackagingDetails> LstTertioryPackagingDetails = new List<PackagingDetails>();
                List<PackagingDetails> LstSecondaryPackagingDetails = new List<PackagingDetails>();
                LstJb = m_dbhelper.DBManager.JobBLL.GetJobs(JobBLL.JobOp.GetJobOfBatch, -1, BatchNo, null);
                Job oJob = LstJb.Find(item => item.JID == JobId);
                if (LstJobDetails != null)
                {
                    LstBatchData = new List<PackagingDetails>();
                    TertiaryDeck= ProductPackageHelper.getTertiarryDeck(oJob.PAID,JobId);
                    string Query = "SELECT * FROM PACKAGINGDETAILS WHERE (ISDECOMISSION IS NULL OR ISDECOMISSION =0) AND ISREJECTED IS NOT NULL " +
                                    " AND (DAVAPORTALUPLOAD IS NULL OR DAVAPORTALUPLOAD =0)  AND JOBID =" + JobId+ " ";
                    LstBatchData = m_dbhelper.DBManager.PackagingDetailsBLL.GetPackagingDetailss(Query);

                    //var sortedLvls = ProductPackageHelper.sorttheLevels(LstJobDetails.Select(x => x.JD_Deckcode).ToList());
                    //TertiaryDeck = sortedLvls[sortedLvls.Count - 1];
                    //PrimaryDeck = sortedLvls.First();
                    //LstTertioryPackagingDetails = LstBatchData.FindAll(item => item.PackageTypeCode == TertiaryDeck);

                    LstTertioryPackagingDetails = LstBatchData.FindAll(item => item.SSCC != null && item.PackageTypeCode==TertiaryDeck);

                }
                string strQuery = "Select Count(NextLevelCode) As QuantityInCase,NextLevelCode from PackagingDetails " +
                    " where JobID=" + JobId + " and (DavaPortalUpload is null or DavaPortalUpload=0) group by NextLevelCode  ";


                string sscccode = string.Empty;
                PackagingDetails pack = new PackagingDetails();
              
                
                int qty = 1;
                if (oJob != null)
                    qty = oJob.PrimaryPCMapCount;

                foreach (PackagingDetails Teriory in LstTertioryPackagingDetails)
                {
                    ChildCount = 0;                   
                    GetChildCount(Teriory.Code, LstBatchData, false,PrimaryDeck);
                    if (Teriory != null)
                        ListSSCCs.Add(new SSCCDetails() { SSCC = Teriory.SSCC, PrimaryPackQty = (ChildCount * qty), Active = true });
                }
                
                vm.SSCCs = ListSSCCs;
                vm.BatchQuatityScanned = vm.SSCCs.Sum(x => x.PrimaryPackQty);
                return vm;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool IsSSCCValid(string SSCC)
        {
            DbHelper m_dbhelper = new DbHelper();
            return m_dbhelper.DBManager.PackagingDetailsBLL.PackagingDetailsDAO.GetSSCCValid(SSCC);
        }

        private void GetChildCount(string NextLevelCode, List<PackagingDetails> LstDetails, bool AddTertoryCount,string PrimaryDeck)
        {
            try
            {
                List<PackagingDetails> LstPckDetails = LstDetails.FindAll(item => item.NextLevelCode == NextLevelCode);
                if (LstPckDetails.Count > 0 && AddTertoryCount == false)
                {
                    var currentDeck = LstPckDetails.FirstOrDefault().PackageTypeCode;
                    if (currentDeck == "MOC")
                    {
                        ChildCount = ChildCount + LstPckDetails.Count;
                    }

                }
                if (LstPckDetails.Count == 0)
                    return;
                if (LstPckDetails[0].PackageTypeCode == DECKs.MOC.ToString())
                    return;
                foreach (PackagingDetails Pck in LstPckDetails)
                {
                    GetChildCount(Pck.Code, LstDetails, false,PrimaryDeck);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(TnT.LangResource.GlobalRes.TrailErrorDsreviceGetChildcnt + ex.Message);
              
            }
        }



        #region Regenerate

        public IEnumerable<SelectListItem> getDAVABtcForRegeneratn()
        {
            List<DAVAExportFileTagsInfo> lstDAVAExportFileTagsInfos = new List<DAVAExportFileTagsInfo>();
            List<SelectListItem> btcs = new List<SelectListItem>();
            DbHelper m_dbhelper = new DbHelper();
            lstDAVAExportFileTagsInfos = m_dbhelper.DBManager.DAVAExportFileTagsInfoBLL.DAVAExportFileTagsInfoDAO.GetDAVAExportFileTagsInfos(1, null);

            foreach (var item in lstDAVAExportFileTagsInfos)
            {
                btcs.Add(new SelectListItem() { Value =  item.ProductionInfo_Id.ToString(), Text = item.FILENAME.ToString() });
            }

            btcs.Insert(0, new SelectListItem() { Value = "0", Text = TnT.LangResource.GlobalRes.LblLytSelect });

            return btcs;
        }

        #endregion
    }
}