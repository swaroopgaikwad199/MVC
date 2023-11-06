using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TnT.Models;
using TnT.Models.Job;
using TnT.Models.Product;
using TnT.DataLayer.Trailings;
using REDTR.UTILS.SystemIntegrity;
using REDTR.HELPER;
using System.Globalization;
using System.Resources;
using System.Diagnostics;
using REDTR.JOB;
using REDTR.UTILS;
using REDTR.DB.BLL;
using PTPLCRYPTORENGINE;
using TnT.DataLayer.Line;
using TnT.DataLayer;
using TnT.DataLayer.Security;
using System.Data.Entity;
using TnT.DataLayer.JobService;
using TnT.Models.Code;
using TnT.DataLayer.TracelinkService;
using System.Configuration;
using TnT.DataLayer.RFXCELService;
using System.Data.Common;
using TnT.DataLayer.LabelDesigner;
using TnT.Models.AdditionBatchQty;
using System.Reflection;
using TnT.Models.TraceLinkImporter;
using System.Web.Services.Description;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class JobsController : BaseController
    {
        BatchNotificationHelper batchNotificationHlpr = new BatchNotificationHelper();
        private Trails trail = new Trails();
        private ApplicationDbContext db = new ApplicationDbContext();
        private DbHelper dbhelper = new DbHelper();
        CultureInfo cul = CultureInfo.CreateSpecificCulture("de");
        static string baseDir = AppDomain.CurrentDomain.BaseDirectory + "ResourceSet";
        ResourceManager rm = ResourceManager.CreateFileBasedResourceManager("18_Common", baseDir, null);
        public JobInfo jobinfo = new JobInfo();
        string numfrom = string.Empty;
        string numto = string.Empty;
        List<PackInfo> LstpackInfo;
        List<REDTR.DB.BusinessObjects.PackageLabelAsso> LstPackLabelInfo;
        bool duallabel = Convert.ToBoolean(Utilities.getAppSettings("DualLabel"));
        decimal? Client_Jid = null;
        int? Client_JtypeId = null;
        string ConnectionStr = string.Empty;
        List<LineImplementation> LstLineInfo = new List<LineImplementation>();
        List<REDTR.DB.BusinessObjects.Job> oClientLst_Job = new List<REDTR.DB.BusinessObjects.Job>();
        List<REDTR.DB.BusinessObjects.JobDetails> oClientLst_JobDetails = new List<REDTR.DB.BusinessObjects.JobDetails>();
        List<REDTR.DB.BusinessObjects.ChinaUID> oClientLst_ChinaUID = new List<REDTR.DB.BusinessObjects.ChinaUID>();
        List<REDTR.DB.BusinessObjects.PackagingDetails> oClientLst_PackagingDetails = new List<REDTR.DB.BusinessObjects.PackagingDetails>();
        PerformSyncBLL Obj_perfSync = new PerformSyncBLL();
        List<REDTR.DB.BusinessObjects.Users> oClientLst_Users = new List<REDTR.DB.BusinessObjects.Users>();
        List<REDTR.DB.BusinessObjects.Users> oServer_Users = new List<REDTR.DB.BusinessObjects.Users>();
        List<REDTR.DB.BusinessObjects.PackagingAsso> oClientLst_PackagingAsso = new List<REDTR.DB.BusinessObjects.PackagingAsso>();
        List<REDTR.DB.BusinessObjects.PackagingAssoDetails> oClientLst_PackagingAssoDetails = new List<REDTR.DB.BusinessObjects.PackagingAssoDetails>();
        List<REDTR.DB.BusinessObjects.PackageLabelAsso> oClientLst_PackageLabels = new List<REDTR.DB.BusinessObjects.PackageLabelAsso>();
        List<REDTR.DB.BusinessObjects.JOBType> oClientLst_JobTypeDetails = new List<REDTR.DB.BusinessObjects.JOBType>();

        //for saving job
        List<REDTR.DB.BusinessObjects.JobDetails> LstJobDtls = new List<REDTR.DB.BusinessObjects.JobDetails>();

        #region Line Details

        public ActionResult Line()
        {
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobrequestview, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobrequestview, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
            ViewBag.Lines = db.LineLocation;
            return View();
        }

        [HttpPost]
        public ActionResult getBatchesofLines(string Id)
        {
            try
            {
                var LineData = db.LineLocation.Find(Id);
                var Jobdata = db.Job.Where(j => j.LineCode == LineData.ID).ToList();
                int totalCount = Jobdata.Count();
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobrequestviewBatchdata, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobrequestviewBatchdata, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                //object[] obj = { data, availablelevels, dataPackagingAssoDetails };
                return Json(totalCount);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region CSVFILEREPORT
        public ActionResult CSVFile()
        {
            ViewBag.BatchNo = db.Job.Where(j=>j.JobStatus==5 || j.JobStatus==3 && j.TID==58).Select(s =>new {s.JID ,s.BatchNo });
            ViewBag.JobName = db.Job.Where(j => j.JobStatus == 5 || j.JobStatus == 3 && j.TID == 58).Select(s =>new { s.JID, s.JobName});
            //var job = db.Job.ToList().Select(s => new { s.BatchNo, s.JobName });
            return View();
        }
        [HttpPost]
        public ActionResult CSVFile(FormCollection form)
        {
             var JID=  form["JID"];
            Dictionary<string, object> dicparameter = new Dictionary<string, object>();
            dicparameter.Add("JobID",JID);

            DBSync dbServer = new DBSync();
            DataSet ds = dbServer.GetDataSet("SP_TableDependency", dicparameter);
            return View();
        }

        [HttpPost]
        public ActionResult getPoNumber(int JID,string JobName)
        {
            try
            {
                var BatchNo = db.Job.Where(s => s.JID == JID && s.JobName == JobName).Select(s => s.BatchNo);
                
                return Json(BatchNo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        // GET: Jobs
        public ActionResult Index()
        {
            try
            {

                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobrequestviewbatch, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobrequestviewbatch, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                return View(db.Job.Where(x => x.JobStatus != 4).ToList());

            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        // GET: Job/Details/5
        public ActionResult Details(decimal id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Job job = db.Job.Find(id);

                decimal j = Convert.ToDecimal(id);
                var jb = job;
                string jID = id.ToString();
                REDTR.HELPER.DbHelper ObjHelper = new REDTR.HELPER.DbHelper();
                var FirstDeck = ProductPackageHelper.getTopDeck(jb.PAID);
                DataSet DS = ObjHelper.DBManager.JobBLL.GetReportDataSet(JobBLL.ReportOp.ForJobdeckProcess, jID, FirstDeck, null, null, null, false, -1);

                if (DS.Tables[0].Rows.Count > 0)
                {
                    ViewBag.badCount = Convert.ToInt32(DS.Tables[0].Rows[0][3]) + jb.NoReadCount;
                    ViewBag.goodCount = DS.Tables[0].Rows[0][2].ToString();
                    ViewBag.decommisionedCount = DS.Tables[0].Rows[0][4].ToString();
                    ViewBag.notVerified = DS.Tables[0].Rows[0][5].ToString();

                }
                else
                {
                    ViewBag.badCount = 0;
                    ViewBag.goodCount = 0;
                    ViewBag.decommisionedCount = 0;
                    ViewBag.notVerified = 0;

                }

                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobrequestdetailbatch + " " + job.JobName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobrequestdetailbatch + " " + job.JobName, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                if (job == null)
                {
                    return HttpNotFound();
                }

                var selectedJobType = db.JOBTypes.Where(x => x.TID == job.TID).FirstOrDefault();
                ViewBag.SelectedJobType = selectedJobType.Job_Type;

                var jobDetails = db.JobDetails.Where(m => m.JD_JobID == id);
                ViewBag.JobDetails = jobDetails;

                BindDDL();
                ViewBag.Istemper = Convert.ToBoolean(Utilities.getAppSettings("TemperEvidence"));
                ViewBag.IsGlueSetting = Convert.ToBoolean(Utilities.getAppSettings("IsGlueSetting"));
                if (Convert.ToBoolean(Utilities.getAppSettings("TemperEvidence")) == true)
                {
                    ViewBag.temper = db.ProductApplicatorSetting.Where(x => x.ServerPAID == job.PAID).FirstOrDefault();
                }
                if (Convert.ToBoolean(Utilities.getAppSettings("IsGlueSetting")) == true)
                {
                    ViewBag.GlueSetting = db.ProductGlueSetting.Where(x => x.ServerPAID == job.PAID).FirstOrDefault();
                }

                return View(job);
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        public ActionResult Verification()
        {
            try
            {
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobVerifyBatch, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobVerifyBatch, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                var Jobs = db.Job.Where(m => m.VerifiedBy == null && m.JobStatus != 4);
                return View(Jobs);
                //return View(db.PackagingAsso.Where(m => m.VerifyProd == false).ToList());
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        public ActionResult Verify(decimal? JobId)
        {
            try
            {
                if (JobId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Job job = db.Job.Find(JobId);
                var jobDetails = db.JobDetails.Where(m => m.JD_JobID == JobId);
                var selectedJobType = db.JOBTypes.Where(x => x.TID == job.TID).FirstOrDefault();
                ViewBag.SelectedJobType = selectedJobType.Job_Type;
                ViewBag.JobDetails = jobDetails;
                ViewBag.DateFormats = db.S_DateFormats;
                BindDDL();
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobprocessingbatch + " " + job.JobName + " " + TnT.LangResource.GlobalRes.TrialJobtoverify, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobprocessingbatch + " " + job.JobName + " " + TnT.LangResource.GlobalRes.TrialJobtoverify, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                return View("VerifyJob", job);
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyJob([Bind(Include = "JID,Remarks")] Job jb)
        {
            try
            {
                //bool AllowMultipleBatchesOnLine = Convert.ToBoolean(Utilities.getAppSettings("AllowMultipleBatchesOnLine"));
                bool IsTemperEvidence = Convert.ToBoolean(Utilities.getAppSettings("TemperEvidence"));
                bool IsGlueSetting = Convert.ToBoolean(Utilities.getAppSettings("IsGlueSetting"));

                List<Models.ExecutionStat> lstES = new List<Models.ExecutionStat>();
                Job selectedJob = db.Job.FirstOrDefault(s => s.JID == jb.JID);

                #region VALIDATION
                if (string.IsNullOrEmpty(jb.Remarks))
                {
                    TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobtakelineclearance;
                    trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobprovidedremark + " " + selectedJob.JobName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobprovidedremark + " " + selectedJob.JobName, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                    BindDDL();
                    return RedirectToAction("Verification");
                }

                var selectedLine = db.LineLocation.FirstOrDefault(x => x.ID == selectedJob.LineCode);
                bool AllowMultipleBatchesOnLine = selectedLine.AllowMultipleBatches;
                if (selectedLine == null)
                {
                    TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobLinedetailsnotprovided;
                    return RedirectToAction("Verification");
                }

                DBSync dbServer = new DBSync();
                if (!dbServer.OpenConnection())
                {
                    TempData["Error"] = "Unable to connect. Try again later.";
                    return RedirectToAction("Verification");
                }

                DBSync dbLine = new DBSync(selectedLine);
                if (!dbLine.OpenConnection())
                {
                    TempData["Error"] = "Unable to connect Packaging Line " + selectedLine.ID + ". Try again later";
                    return RedirectToAction("Verification");
                }

                DataSet dstemp = null; string query = String.Empty;
                decimal productID = 0;
                int? customerID = 0;
                
                if (!AllowMultipleBatchesOnLine) //Not Allow
                {
                    query = "SELECT * FROM JOB";
                    dstemp = dbLine.GetDataSet(query);
                    if (dstemp != null && dstemp.Tables.Count > 0 && dstemp.Tables[0].Rows.Count >= 1)
                    {
                        TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobtakelineclearance;
                        return RedirectToAction("Verification");
                    }
                }
                //else //Allow
                {
                    query = "SELECT * FROM PackagingAsso WHERE PAID = " + selectedJob.PAID;
                    dstemp = dbLine.GetDataSet(query);
                    if (dstemp.Tables.Count <= 0 || dstemp.Tables[0].Rows.Count == 0)
                    {
                        productID = selectedJob.PAID;
                    }
                    query = "SELECT * FROM M_Customer WHERE Id = " + selectedJob.CustomerId;
                    dstemp = dbLine.GetDataSet(query);
                    if (dstemp.Tables.Count <= 0 || dstemp.Tables[0].Rows.Count == 0)
                    {
                        customerID = selectedJob.CustomerId;
                    }
                }

                query = "select * from Linelocation";
                dstemp = dbLine.GetDataSet(query);
                if (dstemp != null && dstemp.Tables.Count > 0 && dstemp.Tables[0].Rows.Count == 0)
                {
                    TempData["Error"] = "No Packaging Line configuration found";
                    return RedirectToAction("Verification");
                }
                if (dstemp != null && dstemp.Tables.Count > 0 && dstemp.Tables[0].Rows.Count > 1)
                {
                    TempData["Error"] = "Multiple line configurations found";
                    return RedirectToAction("Verification");
                }
                if (dstemp != null && dstemp.Tables.Count > 0 && dstemp.Tables[0].Rows.Count == 1)
                {
                    if (selectedLine.ID.ToLower() != dstemp.Tables[0].Rows[0]["ID"].ToString().ToLower())
                    {
                        TempData["Error"] = "Multiple line configurations found " + selectedLine.ID;
                        return RedirectToAction("Verification");
                    }
                }
                #endregion

                #region CENTAUR CUSTOME REQUIRENMENT
                var customer = db.M_Customer.Find(selectedJob.CustomerId);
                var jobType = db.JOBTypes.Find(selectedJob.TID);
                var provider = db.M_Providers.Find(customer.ProviderId); //Where(x => x.ProviderId == customer.ProviderId).Select(x => x.Proivder.Name);

                // CALL ON 17.08.2021 WITH PRADIP THIS EXTRA CODE NOT REQUIRED 
                //  JUST USE CUSTOMER'S SSCC OR PROPIX SSCC AS PER ISSCC FLAG
                //List<X_TracelinkUIDStore> remarksscc = null;
                //// IF DSCSA THEN ADD CUSTOMER SSCC IN REMARK COLUMN
                //if (jobType.Job_Type == "DSCSA" && provider.Code != UIDCustomType.PROP.ToString())
                //{
                //    var lastDeck = db.JobDetails.Where(j => j.JD_JobID == jb.JID).OrderByDescending(s => s.Id).FirstOrDefault();

                //    var m_id = db.M_Identities.FirstOrDefault(s => s.JID == jb.JID && s.GTIN == lastDeck.JD_GTIN);
                //    var x_id = db.X_Identities.Count(s => s.MasterId == m_id.Id);

                //    remarksscc = db.X_TracelinkUIDStore.Where(s => s.GTIN == lastDeck.JD_GTIN && s.IsUsed == false).Take(x_id).ToList();

                //    if (remarksscc.Count() != x_id)
                //    {
                //        TempData["Error"] = "REMARK SSCC FOUND " + remarksscc.Count() + " REQUIRED " + x_id;
                //        return RedirectToAction("Verification");
                //    }
                //}
                #endregion

                Dictionary<string, object> dicparameter = new Dictionary<string, object>();
                dicparameter.Add("JobID", selectedJob.JID);
                dicparameter.Add("PAID", productID);
                dicparameter.Add("IsManageJob", false);
                dicparameter.Add("IsTemperEvidence", IsTemperEvidence);
                dicparameter.Add("IsGlueSetting", IsGlueSetting);
                dicparameter.Add("CustomerID", customerID);

                DataSet ds = dbServer.GetDataSet("SP_VERIFYJOB", dicparameter);
                int tblPackDtlsno = ds.Tables.Count - 1;

                if (ds.Tables[tblPackDtlsno].Rows.Count < (selectedJob.Quantity + selectedJob.SurPlusQty))
                {
                    TempData["Error"] = "Pack data not found. Kindly contact Administrator";
                    return RedirectToAction("Verification");
                }

                #region Create PackagingDetails

                DataTable dtPackDetails = ConvertToPackagingDetailsV2(ds.Tables[tblPackDtlsno], selectedJob);
                ds.Tables.Remove(ds.Tables[tblPackDtlsno]);
                ds.Tables.Add(dtPackDetails);

                if (productID > 0)
                {
                    ds.Tables[3].Rows[0]["VerifiedBy"] = User.ID;
                    ds.Tables[3].Rows[0]["VerifiedDate"] = DateTime.Now;
                }
                else
                {
                    ds.Tables[1].Rows[0]["VerifiedBy"] = User.ID;
                    ds.Tables[1].Rows[0]["VerifiedDate"] = DateTime.Now;
                }
                #endregion

                var isverifyatLine = dbLine.SyncVerifyJob(ds, lstES);

                if (isverifyatLine && lstES.Count(s => !s.IsSuccess) == 0)
                {
                    #region INSERT PACK DETAILS ON SERVER
                    for (int i = 0; i < tblPackDtlsno; i++)
                    {
                        if (i > 0 && i < tblPackDtlsno)
                        {
                            ds.Tables.Remove("Table" + i);
                            ds.Tables[0].Columns.Remove("Column" + i);
                        }
                    }

                    var isverifyatServer = dbServer.SyncVerifyJob(ds, lstES);
                    if (isverifyatServer && lstES.Count(s => !s.IsSuccess) == 0)
                    {
                        TempData["Success"] = "'" + selectedJob.JobName + "' " + TnT.LangResource.GlobalRes.TempDataJobSuccessfullyverified + " " + selectedJob.LineCode;
                        selectedJob.JobStatus = 1;
                        selectedJob.VerifiedBy = User.ID;
                        selectedJob.VerifiedDate = DateTime.Now;

                        db.Entry(selectedJob).State = EntityState.Modified;
                        db.SaveChanges();

                        //dbhelper.DBManager.JobBLL.UpdateJobDetails(REDTR.DB.BLL.JobBLL.JobOp.UpdateVerifiedJob, selectedJob);
                        trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataJobverifiedbatch + " " + selectedJob.JobName + " " +
                            TnT.LangResource.GlobalRes.TempDataJobforline + " " + selectedJob.LineCode, Convert.ToInt32(User.ID), User.FirstName + " " +
                            TnT.LangResource.GlobalRes.TempDataJobverifiedbatch + " " + selectedJob.JobName + " " + TnT.LangResource.GlobalRes.TempDataJobforline + " " +
                            selectedJob.LineCode, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                    }
                    else
                    {
                        TempData["Success"] = "'" + selectedJob.JobName + "'" + " Batch verify. Server not update";
                    }

                    #endregion

                    return RedirectToAction("Verification");
                }
                else
                {
                    TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJonNoconnectionrepectiveline;
                }
                return RedirectToAction("Verification");
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionLogger.logException(ex);
                string exmsg = ex.Message;

                TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJonNoconnectionrepectiveline;
                return RedirectToAction("Verification");
            }


        }
        public DataTable ConvertToPackagingDetailsV2_Old(DataTable dtXUIDs, Job selectedJob)
        {
            //List<PackagingDetails> lstPackDetails = new List<PackagingDetails>();
            DataTable dtPackDetails = new DataTable();
            dtPackDetails.Columns.Add("Code", typeof(string));
            dtPackDetails.Columns.Add("PAID", typeof(int));
            dtPackDetails.Columns.Add("JobID", typeof(int));
            dtPackDetails.Columns.Add("PackageTypeCode", typeof(string));
            dtPackDetails.Columns.Add("MfgPackDate", typeof(DateTime));
            dtPackDetails.Columns.Add("ExpPackDate", typeof(DateTime));
            dtPackDetails.Columns.Add("NextLevelCode", typeof(string));
            dtPackDetails.Columns.Add("SSCC", typeof(string));
            dtPackDetails.Columns.Add("IsLoose", typeof(bool));
            dtPackDetails.Columns.Add("CreatedDate", typeof(DateTime));
            dtPackDetails.Columns.Add("LastUpdatedDate", typeof(DateTime));
            dtPackDetails.Columns.Add("CryptoCode", typeof(string));
            dtPackDetails.Columns.Add("PublicKey", typeof(string));
            dtPackDetails.Columns.Add("Remarks", typeof(string));

            try
            {
                var providerOfJob = db.M_Providers.FirstOrDefault(p => p.Id == selectedJob.Customer.ProviderId);
                var jobDetails = db.JobDetails.Where(p => p.JD_JobID == selectedJob.JID).OrderBy(s => s.Id).ToList();
                var selectedJobType = db.JOBTypes.FirstOrDefault(s => s.TID == selectedJob.TID);

                foreach (var jdeck in jobDetails)
                {
                    #region AddCode

                    //OTHER THAN LAST LEVEL
                    if (jdeck.JD_Deckcode != jobDetails.OrderByDescending(s => s.Id).FirstOrDefault().JD_Deckcode || jdeck.JD_Deckcode == jobDetails.OrderByDescending(s => s.Id).FirstOrDefault().JD_Deckcode)
                    {
                        DataRow[] codeOfDeck = new DataRow[0];

                        //codeOfDeck = LstOfIdsToSave.Where(s => s.PackTypeCode == jdeck.JD_Deckcode).ToList();
                        codeOfDeck = dtXUIDs.Select("PackTypeCode = '" + jdeck.JD_Deckcode.ToString() + "'");
                        foreach (DataRow dr in codeOfDeck)
                        {
                            ////RUSSIA OTHER THAN MOC == SSCC || LAST DEKC ALWAYS SSCC
                            if ((jobDetails.First().JD_Deckcode != jdeck.JD_Deckcode && selectedJobType.Job_Type == "RUSSIA"))
                            {
                                if (dr["SerialNo"].ToString().Length == 18)
                                {
                                    dtPackDetails.Rows.Add(new object[]{
                                        dr["SerialNo"], selectedJob.PAID, selectedJob.JID,
                                        dr["PackageTypeCode"], selectedJob.MfgDate,selectedJob.ExpDate,
                                        (jdeck.JD_Deckcode == jobDetails.Last().JD_Deckcode)?null:"FFFFF",
                                        (dr["SerialNo"].ToString().Length == 18)? dr["SerialNo"].ToString():null,
                                        false,DateTime.Now,DateTime.Now,dr["CryptoCode"],dr["PublicKey"], null
                                    });
                                }
                            }
                            else
                            {
                                dtPackDetails.Rows.Add(new object[]{
                                    dr["SerialNo"], selectedJob.PAID, selectedJob.JID,
                                    dr["PackageTypeCode"], selectedJob.MfgDate,selectedJob.ExpDate,
                                    (jdeck.JD_Deckcode == jobDetails.Last().JD_Deckcode)?null:"FFFFF",
                                    null,false,DateTime.Now,DateTime.Now,dr["CryptoCode"],dr["PublicKey"],null
                                });
                            }
                        }
                    }
                    #endregion

                    #region AddSSCC
                    if (jdeck.JD_Deckcode == jobDetails.Last().JD_Deckcode)
                    {
                        DataRow[] ssccOfDeck = new DataRow[0];
                        ssccOfDeck = dtXUIDs.Select("PackTypeCode = 'SSC'");

                        int rowNo = 0;
                        foreach (DataRow drSSCC in ssccOfDeck)
                        {
                            dtPackDetails.Rows.Add(new object[]{
                                drSSCC["SerialNo"], selectedJob.PAID, selectedJob.JID,
                                drSSCC["PackageTypeCode"], selectedJob.MfgDate,selectedJob.ExpDate,
                                (jdeck.JD_Deckcode == jobDetails.Last().JD_Deckcode)?null:"FFFFF",
                                (drSSCC["SerialNo"].ToString().Length == 18)? drSSCC["SerialNo"].ToString():null,
                                false,DateTime.Now,DateTime.Now,drSSCC["CryptoCode"],drSSCC["PublicKey"], null
                                });
                        }
                    }
                    #endregion

                    #region Add Loos SSCC
                    if ((jdeck.JD_Deckcode == jobDetails.Last().JD_Deckcode) ||
                        (jdeck.JD_Deckcode != jobDetails.First().JD_Deckcode && selectedJobType.Job_Type == "DSCSA"))
                    {
                        DataRow[] loosessccOfDeck = new DataRow[0];
                        loosessccOfDeck = dtXUIDs.Select("PackTypeCode ='" + jdeck.JD_Deckcode.ToString() + "Loos'");

                        foreach (DataRow drLooseSSC in loosessccOfDeck)
                        {
                            dtPackDetails.Rows.Add(new object[]{
                            drLooseSSC["SerialNo"], selectedJob.PAID, selectedJob.JID,
                            drLooseSSC["PackageTypeCode"], selectedJob.MfgDate,selectedJob.ExpDate,
                            (jdeck.JD_Deckcode == jobDetails.Last().JD_Deckcode)?null:"FFFFF",
                            (drLooseSSC["SerialNo"].ToString().Length == 18)? drLooseSSC["SerialNo"].ToString():null,
                            true,DateTime.Now,DateTime.Now,drLooseSSC["CryptoCode"],drLooseSSC["PublicKey"], null
                            });
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionLogger.logException(ex);
                throw;
            }

            return dtPackDetails; //lstPackDetails;
        }
        public DataTable ConvertToPackagingDetailsV2(DataTable dtXUIDs, Job selectedJob)
        {
            //List<PackagingDetails> lstPackDetails = new List<PackagingDetails>();
            DataTable dtPackDetails = new DataTable();
            dtPackDetails.Columns.Add("Code", typeof(string));
            dtPackDetails.Columns.Add("PAID", typeof(int));
            dtPackDetails.Columns.Add("JobID", typeof(int));
            dtPackDetails.Columns.Add("PackageTypeCode", typeof(string));
            dtPackDetails.Columns.Add("MfgPackDate", typeof(DateTime));
            dtPackDetails.Columns.Add("ExpPackDate", typeof(DateTime));
            dtPackDetails.Columns.Add("NextLevelCode", typeof(string));
            dtPackDetails.Columns.Add("SSCC", typeof(string));
            dtPackDetails.Columns.Add("IsLoose", typeof(bool));
            dtPackDetails.Columns.Add("CreatedDate", typeof(DateTime));
            dtPackDetails.Columns.Add("LastUpdatedDate", typeof(DateTime));
            dtPackDetails.Columns.Add("CryptoCode", typeof(string));
            dtPackDetails.Columns.Add("PublicKey", typeof(string));
            dtPackDetails.Columns.Add("Remarks", typeof(string));

            try
            {
                var providerOfJob = db.M_Providers.FirstOrDefault(p => p.Id == selectedJob.Customer.ProviderId);
                var jobDetails = db.JobDetails.Where(p => p.JD_JobID == selectedJob.JID).OrderBy(s => s.Id).ToList();
                var selectedJobType = db.JOBTypes.FirstOrDefault(s => s.TID == selectedJob.TID);

                foreach (var jdeck in jobDetails)
                {
                    #region AddCode

                    //OTHER THAN LAST LEVEL
                    if (jobDetails.Count == 1 || jdeck.JD_Deckcode != jobDetails.OrderByDescending(s => s.Id).FirstOrDefault().JD_Deckcode)
                    {
                        DataRow[] codeOfDeck = new DataRow[0];

                        //codeOfDeck = LstOfIdsToSave.Where(s => s.PackTypeCode == jdeck.JD_Deckcode).ToList();
                        codeOfDeck = dtXUIDs.Select("PackTypeCode = '" + jdeck.JD_Deckcode.ToString() + "'");
                        foreach (DataRow dr in codeOfDeck)
                        {
                            ////RUSSIA OTHER THAN MOC == SSCC || LAST DEKC ALWAYS SSCC
                            if ((jobDetails.First().JD_Deckcode != jdeck.JD_Deckcode && selectedJobType.Job_Type == "RUSSIA"))
                            {
                                if (dr["SerialNo"].ToString().Length == 18)
                                {
                                    dtPackDetails.Rows.Add(new object[]{
                                        dr["SerialNo"], selectedJob.PAID, selectedJob.JID,
                                        dr["PackageTypeCode"], selectedJob.MfgDate,selectedJob.ExpDate,
                                        (jdeck.JD_Deckcode == (jobDetails.Last().JD_Deckcode))?null:"FFFFF",
                                        (dr["SerialNo"].ToString().Length == 18)? dr["SerialNo"].ToString():null,
                                        false,DateTime.Now,DateTime.Now,dr["CryptoCode"],dr["PublicKey"], null
                                    });
                                }
                            }
                            else
                            {
                                dtPackDetails.Rows.Add(new object[]{
                                    dr["SerialNo"], selectedJob.PAID, selectedJob.JID,
                                    dr["PackageTypeCode"], selectedJob.MfgDate,selectedJob.ExpDate,
                                    (jdeck.JD_Deckcode == (jobDetails.Last().JD_Deckcode))?null:"FFFFF",
                                    null,false,DateTime.Now,DateTime.Now,dr["CryptoCode"],dr["PublicKey"],null
                                });
                            }
                        }
                    }
                    #endregion
                    if (jobDetails.Count > 1)
                    {
                        #region AddSSCC
                        if (jdeck.JD_Deckcode == jobDetails.Last().JD_Deckcode)
                        {
                            DataRow[] ssccOfDeck = new DataRow[0];
                            ssccOfDeck = dtXUIDs.Select("PackTypeCode = 'SSC'");

                            int rowNo = 0;
                            foreach (DataRow drSSCC in ssccOfDeck)
                            {
                                dtPackDetails.Rows.Add(new object[]{
                                drSSCC["SerialNo"], selectedJob.PAID, selectedJob.JID,
                                drSSCC["PackageTypeCode"], selectedJob.MfgDate,selectedJob.ExpDate,
                                (jdeck.JD_Deckcode == jobDetails.Last().JD_Deckcode)?null:"FFFFF",
                                (drSSCC["SerialNo"].ToString().Length == 18)? drSSCC["SerialNo"].ToString():null,
                                false,DateTime.Now,DateTime.Now,drSSCC["CryptoCode"],drSSCC["PublicKey"], null
                                });
                            }
                        }
                        #endregion

                        #region Add Loos SSCC
                        if ((jdeck.JD_Deckcode == jobDetails.Last().JD_Deckcode) ||
                            (jdeck.JD_Deckcode != jobDetails.First().JD_Deckcode && selectedJobType.Job_Type == "DSCSA"))
                        {
                            DataRow[] loosessccOfDeck = new DataRow[0];
                            loosessccOfDeck = dtXUIDs.Select("PackTypeCode ='" + jdeck.JD_Deckcode.ToString() + "Loos'");

                            foreach (DataRow drLooseSSC in loosessccOfDeck)
                            {
                                dtPackDetails.Rows.Add(new object[]{
                            drLooseSSC["SerialNo"], selectedJob.PAID, selectedJob.JID,
                            drLooseSSC["PackageTypeCode"], selectedJob.MfgDate,selectedJob.ExpDate,
                            (jdeck.JD_Deckcode == jobDetails.Last().JD_Deckcode)?null:"FFFFF",
                            (drLooseSSC["SerialNo"].ToString().Length == 18)? drLooseSSC["SerialNo"].ToString():null,
                            true,DateTime.Now,DateTime.Now,drLooseSSC["CryptoCode"],drLooseSSC["PublicKey"], null
                            });
                            }
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionLogger.logException(ex);
                throw;
            }

            return dtPackDetails; //lstPackDetails;
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult VerifyJob([Bind(Include = "JID,Remarks")] Job jb)
        //{
        //    //string AllowMultipleBatches = Utilities.getAppSettings("AllowMultipleBatchesOnLine");
        //    bool IsTemperEvidence = Convert.ToBoolean(Utilities.getAppSettings("TemperEvidence"));
        //    bool IsGlueSetting = Convert.ToBoolean(Utilities.getAppSettings("IsGlueSetting"));
        //    Job mjob;
        //    if (string.IsNullOrEmpty(jb.Remarks))
        //    {
        //        ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModelErrorJobkindlyprovide);
        //        mjob = db.Job.Find(jb.JID);
        //        var jobDetails = db.JobDetails.Where(m => m.JD_JobID == jb.JID);
        //        ViewBag.JobDetails = jobDetails;
        //        BindDDL();
        //        trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobprovidedremark + " " + mjob.JobName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobprovidedremark + " " + mjob.JobName, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
        //        return View(mjob);
        //    }
        //    mjob = db.Job.Find(jb.JID);

        //    try
        //    {
        //        #region Track N Trace 
        //        int Custid = 0;
        //        int TotalSSCC = 0;
        //        REDTR.DB.BusinessObjects.Job job = new REDTR.DB.BusinessObjects.Job();
        //        job = dbhelper.DBManager.JobBLL.GetJob(JobBLL.JobOp.GetJob, 1, jb.JID.ToString(), null);
        //        job.VerifiedBy = User.ID;
        //        job.Remarks = jb.Remarks;
        //        jobinfo = FillJObINFOSM(job, "Verify");

        //        PackInfo item1 = jobinfo.jPackInfoLst[jobinfo.jPackInfoLst.Count - 1];
        //        if (item1 != null)
        //        {
        //            TotalSSCC = item1.UIDsToPRint;
        //        }
        //        /////////////////////////////////////////////////////////////

        //        string Type = db.JOBTypes.Where(j => j.TID == job.TID).Select(j => j.Job_Type).FirstOrDefault();

        //        var LineServer = db.LineLocation.Where(x => x.ID == job.LineCode).FirstOrDefault();
        //        REDTR.DB.BusinessObjects.M_Customer oClientLst_Customer = new REDTR.DB.BusinessObjects.M_Customer();

        //        oClientLst_Job = dbhelper.DBManager.JobBLL.GetJobs(JobBLL.JobOp.GetAllJobs, 1, "", "");
        //        oClientLst_Users = dbhelper.DBManager.UsersBLL.GetUserss(UsersBLL.UsersOp.GetUserss, "");
        //        oClientLst_PackagingAsso = dbhelper.DBManager.PackagingAssoBLL.GetPackagingAssos();
        //        oClientLst_JobTypeDetails = dbhelper.DBManager.JOBTypeBLL.GetJOBTypes();
        //        oClientLst_PackageLabels = dbhelper.DBManager.PackageLabelBLL.GetPackagingLabelAssos(Convert.ToInt32(PackageLabelAssoBLL.PackageLabelOp.GetAllPackageLabelDetails), null, null);
        //        oClientLst_Customer = dbhelper.DBManager.M_CustomerBLL.GetCustomer(1, Convert.ToString(job.CustomerId));



        //        if (LineServer != null)
        //        {
        //            ConnectionStr = @"Data Source=" + LineServer.LineIP + ";" + "Initial Catalog=" + LineServer.DBName + ";Persist Security Info=True;User ID=" + LineServer.SQLUsername + ";Password=" + LineServer.SQLPassword + ";MultipleActiveResultSets=True";
        //        }
        //        else
        //        {
        //            TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobLinedetailsnotprovided;
        //            return RedirectToAction("Verification");
        //        }

        //        if (string.IsNullOrEmpty(ConnectionStr))
        //        {
        //            TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobNoconnectionstringavailableforline;
        //            return RedirectToAction("Verification");
        //            //var Jobs = db.Job.Where(m => m.VerifiedBy == null);
        //            //return View("Verification", Jobs);
        //        }
        //        else
        //        {
        //            Obj_perfSync.OpenDBConnection(ConnectionStr);

        //            if (Obj_perfSync.IsOpen())
        //            {
        //                List<REDTR.DB.BusinessObjects.Job> jb1 = Obj_perfSync.GetJobs(1);
        //                //if (AllowMultipleBatches == "false")
        //                //{
        //                //    if (jb1.Count >= 1)
        //                //    {
        //                //        // MessageBoxEx.Show("LINE CLEARANCE NOT COMPLETED. PLEASE MAKE LINE CLEARANCE", APPNAME, MessageBoxEx.MessageBoxButtonsEx.OK, 0);
        //                //        TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobtakelineclearance;
        //                //        return RedirectToAction("Verification");
        //                //    }
        //                //}
        //                if (!LineServer.AllowMultipleBatches && jb1.Count > 0)
        //                {
        //                    TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobtakelineclearance;
        //                    return RedirectToAction("Verification");
        //                }

        //                if (IsTemperEvidence == true)
        //                {
        //                    List<REDTR.DB.BusinessObjects.ProductApplicatorSetting> lstPro = Obj_perfSync.GetProductAppicatorSetting(1);
        //                    if (lstPro.Count >= 1)
        //                    {
        //                        TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobProductApplicator;
        //                        return RedirectToAction("Verification");
        //                    }
        //                }
        //                if (IsGlueSetting == true)
        //                {
        //                    REDTR.DB.BusinessObjects.ProductGlueSetting pro = Obj_perfSync.GetProductGlueSetting(1);
        //                    if (pro == null)
        //                    {
        //                        TempData["Success"] = TnT.LangResource.GlobalRes.TempDataJobProductGlueSetting;
        //                        return RedirectToAction("Verification");
        //                    }
        //                }

        //                REDTR.DB.BusinessObjects.PackagingAsso ObjPackagingAsso = new REDTR.DB.BusinessObjects.PackagingAsso();
        //                ObjPackagingAsso = oClientLst_PackagingAsso.Find(item => item.PAID == job.PAID);

        //                REDTR.DB.BusinessObjects.JOBType objJobTypes = new REDTR.DB.BusinessObjects.JOBType();
        //                objJobTypes = oClientLst_JobTypeDetails.Find(item => item.TID == job.TID);

        //                REDTR.DB.BusinessObjects.Users ObjUsers = new REDTR.DB.BusinessObjects.Users();
        //                ObjUsers = oClientLst_Users.Find(item => item.ID == job.CreatedBy);

        //                REDTR.DB.BusinessObjects.Users ObjUsers1 = new REDTR.DB.BusinessObjects.Users();
        //                ObjUsers1 = oClientLst_Users.Find(item => item.ID == job.VerifiedBy);

        //                List<REDTR.DB.BusinessObjects.PackageLabelAsso> PackLabels = new List<REDTR.DB.BusinessObjects.PackageLabelAsso>();
        //                if (ObjUsers == null)
        //                    ObjUsers = ObjUsers1;
        //                string CreatedBy = ObjUsers.UserName;
        //                string VerifiedBy = ObjUsers1.UserName;
        //                string ProductName = ObjPackagingAsso.Name;
        //                decimal PAID = 0;

        //                try
        //                {
        //                    Trace.TraceInformation("{0},{1},{2},{3},{4}", DateTime.Now.ToString(), ObjUsers.UserName, ObjUsers.ID, ObjUsers1.UserName, ObjUsers1.ID);
        //                    Obj_perfSync.InsertOrUpdateUserDetailsForSync(ObjUsers, (int)UsersBLL.UsersOp.AddUserForSync);
        //                    Obj_perfSync.InsertOrUpdateUserDetailsForSync(ObjUsers1, (int)UsersBLL.UsersOp.AddUserForSync);
        //                    Trace.TraceInformation(TnT.LangResource.GlobalRes.TempDataJobTransferbothusers);
        //                }
        //                catch (Exception ex)
        //                {
        //                    ExceptionHandler.ExceptionLogger.logException(ex);

        //                    Trace.TraceInformation(DateTime.Now.ToString() + " " + ex.Message + " " + ex.StackTrace);
        //                }

        //                try
        //                {
        //                    /*Custid =*/
        //                    Obj_perfSync.InsertOrUpdateCustomerForDataSync(1, oClientLst_Customer);
        //                }
        //                catch (Exception ex)
        //                {
        //                    ExceptionHandler.ExceptionLogger.logException(ex);

        //                    Trace.TraceInformation(DateTime.Now.ToString() + " " + ex.Message + " " + ex.StackTrace);
        //                }

        //                //foreach (PackagingAsso prod in oClientLst_PackagingAsso)
        //                if (ObjPackagingAsso != null && ObjPackagingAsso.PAID > 0)
        //                {
        //                    PAID = Obj_perfSync.InsertOrUpdatePackagingAssoForSync(ObjPackagingAsso);
        //                    if (IsTemperEvidence == true)
        //                    {


        //                        REDTR.DB.BusinessObjects.ProductApplicatorSetting OClientLst_ProductAppicatorSetting = new REDTR.DB.BusinessObjects.ProductApplicatorSetting();
        //                        OClientLst_ProductAppicatorSetting = dbhelper.DBManager.ProductApplicatorSettingBLL.GetProductApplicationSettind(job.PAID, 1);
        //                        if (OClientLst_ProductAppicatorSetting != null)
        //                        {
        //                            OClientLst_ProductAppicatorSetting.ServerPAID = ObjPackagingAsso.PAID;
        //                            if (OClientLst_ProductAppicatorSetting.S1 == null)
        //                            {
        //                                OClientLst_ProductAppicatorSetting.S1 = 0;
        //                            }
        //                            if (OClientLst_ProductAppicatorSetting.S2 == null)
        //                            {
        //                                OClientLst_ProductAppicatorSetting.S2 = 0;
        //                            }
        //                            if (OClientLst_ProductAppicatorSetting.S3 == null)
        //                            {
        //                                OClientLst_ProductAppicatorSetting.S3 = 0;
        //                            }
        //                            if (OClientLst_ProductAppicatorSetting.S4 == null)
        //                            {
        //                                OClientLst_ProductAppicatorSetting.S4 = 0;
        //                            }
        //                            if (OClientLst_ProductAppicatorSetting.S5 == null)
        //                            {
        //                                OClientLst_ProductAppicatorSetting.S5 = 0;
        //                            }
        //                            if (OClientLst_ProductAppicatorSetting.FrontLabelOffset == null)
        //                            {
        //                                OClientLst_ProductAppicatorSetting.FrontLabelOffset = 0;
        //                            }
        //                            if (OClientLst_ProductAppicatorSetting.BackLabelOffset == null)
        //                            {
        //                                OClientLst_ProductAppicatorSetting.BackLabelOffset = 0;
        //                            }
        //                            if (OClientLst_ProductAppicatorSetting.CartonLength == null)
        //                            {
        //                                OClientLst_ProductAppicatorSetting.CartonLength = 0;
        //                            }
        //                            int i = Obj_perfSync.InsertOrUpdateProductApplicatorSetting(OClientLst_ProductAppicatorSetting);
        //                        }
        //                    }

        //                    if (IsGlueSetting == true)
        //                    {


        //                        REDTR.DB.BusinessObjects.ProductGlueSetting OClientLst_ProductGlueSetting = new REDTR.DB.BusinessObjects.ProductGlueSetting();
        //                        OClientLst_ProductGlueSetting = dbhelper.DBManager.ProductGlueSettingBLL.GETProductGlueSetting(2, job.PAID);
        //                        if (OClientLst_ProductGlueSetting != null)
        //                        {
        //                            OClientLst_ProductGlueSetting.ServerPAID = job.PAID;
        //                            if (OClientLst_ProductGlueSetting.HotGlueDotSize == null)
        //                            {
        //                                OClientLst_ProductGlueSetting.HotGlueDotSize = 0;
        //                            }
        //                            if (OClientLst_ProductGlueSetting.HotGlueGapDistance == null)
        //                            {
        //                                OClientLst_ProductGlueSetting.HotGlueGapDistance = 0;
        //                            }
        //                            if (OClientLst_ProductGlueSetting.HotGlueStartDistance == null)
        //                            {
        //                                OClientLst_ProductGlueSetting.HotGlueStartDistance = 0;
        //                            }
        //                            if (OClientLst_ProductGlueSetting.ColdGlueDotSize == null)
        //                            {
        //                                OClientLst_ProductGlueSetting.ColdGlueDotSize = 0;
        //                            }
        //                            if (OClientLst_ProductGlueSetting.ColdGlueGapDistance == null)
        //                            {
        //                                OClientLst_ProductGlueSetting.ColdGlueGapDistance = 0;
        //                            }
        //                            if (OClientLst_ProductGlueSetting.ColdGlueStartDistance == null)
        //                            {
        //                                OClientLst_ProductGlueSetting.ColdGlueStartDistance = 0;
        //                            }
        //                            int i = Obj_perfSync.InsertOrUpdateProductGlueSetting(OClientLst_ProductGlueSetting);
        //                        }
        //                    }

        //                    oClientLst_PackagingAssoDetails = dbhelper.DBManager.PackagingAssoDetailsBLL.GetPckAssoDtlss(ObjPackagingAsso.PAID);
        //                    PackLabels = oClientLst_PackageLabels.FindAll(item => item.PAID == ObjPackagingAsso.PAID);
        //                    foreach (REDTR.DB.BusinessObjects.PackagingAssoDetails PackAssDtls in oClientLst_PackagingAssoDetails)
        //                    {
        //                        Obj_perfSync.InsertOrUpdatePckAssoDetailsForSync(PackAssDtls, PAID);


        //                        Trace.TraceInformation("Updated packagingAsso and PackagingAssoDetails at line Server=>{0},{1},{2}", TnT.LangResource.GlobalRes.TraceInfoJobProductDetailsUpdation, DateTime.Now.ToString(), ObjPackagingAsso.Name);
        //                    }

        //                    foreach (REDTR.DB.BusinessObjects.PackageLabelAsso PckLblAssos in PackLabels)
        //                    {
        //                        Obj_perfSync.InsertOrUpdatePckLabelForSync(PckLblAssos, PAID);
        //                        Trace.TraceInformation("Updated packaging label line =>{0},{1},{2}", TnT.LangResource.GlobalRes.TraceInfoJobProductLeblDetailsUpdation, DateTime.Now.ToString(), ObjPackagingAsso.Name);
        //                    }
        //                }

        //                Client_JtypeId = Obj_perfSync.InsertOrUpdateJobTypeForSync(objJobTypes);
        //                Trace.TraceInformation("Updated Jobtype at line server=>{0},{1},{2},{3}", TnT.LangResource.GlobalRes.TraceInfoJobUpdationjobtype, DateTime.Now.ToString(), Client_JtypeId.ToString(), objJobTypes.Job_Type);

        //                job.TID = Client_JtypeId;
        //                job.PAID = PAID;
        //                var cust = db.M_Customer.Where(x => x.Id == job.CustomerId).FirstOrDefault();
        //                //if (Custid != 0)
        //                //{
        //                //    job.CustomerId = Custid;
        //                //}
        //                Client_Jid = Obj_perfSync.InsertOrUpdateJobForSync(job, ProductName, CreatedBy, VerifiedBy, 1);
        //                Trace.TraceInformation("Updated job and TID at line server=>{0},{1},{2},{3}", TnT.LangResource.GlobalRes.TraceInfoJobUpdationjobnTID, DateTime.Now.ToString(), job.JobName, job.TID.ToString());

        //                dbhelper.DBManager.JobBLL.AddJobcount(Convert.ToInt32(job.JID), 0, 0, 0, 0, 0, 0, 0, 0);
        //                Trace.TraceInformation(DateTime.Now.ToString() + TnT.LangResource.GlobalRes.TraceInfoJobJObIdServer + job.JID.ToString());
        //                Obj_perfSync.InsertOrUpdateREC_Count(1, Convert.ToInt32(Client_Jid), 0, 0, 0, 0, 0, 0, 0, 0);
        //                Trace.TraceInformation(DateTime.Now.ToString() + TnT.LangResource.GlobalRes.TraceInfoJobJObIdLine + Client_Jid.ToString());

        //                oClientLst_JobDetails = dbhelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, job.JID, 1);
        //                foreach (REDTR.DB.BusinessObjects.JobDetails jDetails in oClientLst_JobDetails)
        //                {
        //                    Obj_perfSync.InsertOrUpdateJobDetailsForSync(1, jDetails, (decimal)Client_Jid);
        //                    Trace.TraceInformation("Update Batch at Line side from Server=>{0},{1},{2},{3},{4},{5},{6},{7},{8}", job.JobName, jDetails.JD_ProdName, job.BatchNo, job.Quantity, job.SurPlusQty, job.AutomaticBatchCloser, job.TID, job.JobWithUID, job.LineCode);
        //                }

        //                if (objJobTypes.TID != 26 && objJobTypes.TID != 10054)
        //                {

        //                    var jobSSCC = db.SSCCLineHolder.Where(x => x.JobID == job.JID && (x.Type == "SSCC" || x.Type == null || x.Type == "TSSCC")).FirstOrDefault();
        //                    if (jobSSCC != null)
        //                    {
        //                        int latest = Convert.ToInt32(jobSSCC.FirstSSCC);
        //                        int first = Convert.ToInt32(jobSSCC.LastSSCC);
        //                        dbhelper.SaveSSCCForFreshBatch(Convert.ToInt32(job.JID), first, latest, job.LineCode);
        //                        REDTR.DB.BusinessObjects.SSCCLineHolder objSSCCLineHolder = dbhelper.DBManager.SSCCLineHolderBLL.GetSSCCLineHolder(SSCCLineHolderBLL.SSCCLineHolderOp.GETSSCCLineHolder, Convert.ToInt32(job.JID));

        //                        objSSCCLineHolder.JobID = Client_Jid;
        //                        Obj_perfSync.InsertOrUpdateSSCCLineHolder(objSSCCLineHolder);
        //                    }

        //                }



        //                // AddJobDetails();
        //                string ConnectionServer = Utilities.getConnectionString("DefaultConnection");
        //                //Transfer UID and SSCCs
        //                TransferJobData tjd = new TransferJobData();
        //                if (tjd.Transfer(mjob, ConnectionStr, PAID, (decimal)Client_Jid, ConnectionServer, false))
        //                {
        //                    ConnectionStr = string.Empty;
        //                    //string StrMsg = "USER HAS VERIFIED BATCH: " + jobinfo.JobName.ToString() +" for line:"+job.LineCode;
        //                    //dbhelper.AddUserTrail(User.ID, null, USerTrailWHERE.TnT1, "BATCH MANAGER  ", StrMsg, string.Empty);
        //                    TempData["Success"] = "'" + job.JobName + "' " + TnT.LangResource.GlobalRes.TempDataJobSuccessfullyverified + " " + job.LineCode;
        //                    job.JobStatus = 1;
        //                    dbhelper.DBManager.JobBLL.UpdateJobDetails(REDTR.DB.BLL.JobBLL.JobOp.UpdateVerifiedJob, job);
        //                    trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataJobverifiedbatch + " " + job.JobName + " " + TnT.LangResource.GlobalRes.TempDataJobforline + " " + job.LineCode, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataJobverifiedbatch + " " + job.JobName + " " + TnT.LangResource.GlobalRes.TempDataJobforline + " " + job.LineCode, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
        //                    //  batchNotificationHlpr.notifyPerimissionHolders(ref job, BatchEventType.batchVerificationEvent);
        //                }
        //                else
        //                {
        //                    oClientLst_PackagingDetails = Obj_perfSync.GetPackagingDetailss(4, Convert.ToString(Client_Jid), "");
        //                    if (oClientLst_PackagingDetails.Count > 0)
        //                    {
        //                        string Query = "delete from job where jid=" + Client_Jid;
        //                        int i = Obj_perfSync.RemoveOLD_PckRecords(Query);

        //                        var m_id = db.M_Identities.Where(x => x.JID == mjob.JID).ToList();

        //                        foreach (var item in m_id)
        //                        {
        //                            string Query2 = "update M_Identities set IsTransfered=0 where JID=" + mjob.JID;
        //                            dbhelper.ExecuteQuery(Query2);

        //                            string Query3 = "update X_Identities set IsTransfered=0 where MasterId=" + item.Id;
        //                            dbhelper.ExecureQuery(Query3);
        //                        }
        //                    }
        //                    var err = tjd.getErrorDetails();
        //                    TempData["Success"] = "'" + job.JobName + "'" + TnT.LangResource.GlobalRes.TempDataJobBatchVerificationFailed;
        //                    // TempData["Success"] = err;
        //                }


        //            }
        //            else
        //            {
        //                TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJonNoconnectionrepectiveline;
        //            }
        //            return RedirectToAction("Verification");
        //        }

        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHandler.ExceptionLogger.logException(ex);
        //        string exmsg = ex.Message;

        //        TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJonNoconnectionrepectiveline;
        //        return RedirectToAction("Verification");
        //    }

        //}


        public ActionResult NotVerifyJob(string JID, string Remark)
        {
            //var job = db.Job.Find(jb.JID);
            REDTR.DB.BusinessObjects.Job job = new REDTR.DB.BusinessObjects.Job();
            job = dbhelper.DBManager.JobBLL.GetJob(JobBLL.JobOp.GetJob, 1, JID.ToString(), null);
            var userData = User;
            batchNotificationHlpr.notifyPerimissionHolders(ref job, BatchEventType.batchNotVerifyEvent, userData);
            TempData["Success"] = job.JobName + TnT.LangResource.GlobalRes.TempDataJobBatchNotVerification;
            trail.AddTrail(job.JobName + TnT.LangResource.GlobalRes.TempDataJobBatchNotVerification, Convert.ToInt32(User.ID), job.JobName + TnT.LangResource.GlobalRes.TempDataJobBatchNotVerification, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
            return RedirectToAction("Verification");
        }
        // GET: Job/Create
        public ActionResult Create()
        {
            try
            {
                BindDDL();
                return View();
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }

        private void BindDDL()
        {
            try
            {
                ViewBag.Customer = db.M_Customer.Where(x => x.IsDeleted == false && x.IsActive == true);
                //ViewBag.Provider = db.M_Providers.Where(x => x.IsDeleted == false);
                ViewBag.Types = db.JOBTypes;
                ViewBag.FPCodes = db.PackagingAsso.Where(m => m.VerifyProd == true && m.IsActive == true);
                ViewBag.LineCode = db.LineLocation.Where(x => x.IsActive == true);
                ViewBag.DateFormats = db.S_DateFormats;
                ViewBag.PackLvls = db.PackagingLevels.Where(x => x.IsActive == true);
            }
            catch (Exception)
            {
                throw;
            }

        }
        private void BindEditDDL(decimal jobID)
        {
            try
            {
                ViewBag.Customer = db.M_Customer.Where(x => x.IsDeleted == false && x.IsActive == true);
                //ViewBag.Provider = db.M_Providers.Where(x => x.IsDeleted == false);
                ViewBag.Types = db.JOBTypes;
                ViewBag.FPCodes = db.PackagingAsso.Where(m => m.VerifyProd == true && m.IsActive == true);
                ViewBag.LineCode = db.LineLocation.Where(x => x.IsActive == true);
                ViewBag.DateFormats = db.S_DateFormats;
                ViewBag.PackLvls = db.PackagingLevels.Where(x => x.IsActive == true);
            }
            catch (Exception)
            {
                throw;
            }

        }
        private static List<string> sorttheLevels(List<string> levels)
        {
            try
            {
                // string code;
                List<string> output = new List<string>(6);

                for (int i = 0; i < 6; i++)
                {
                    output.Add("");
                    //= "";
                }

                foreach (string code in levels)
                {
                    if (code == "PPB")
                    {
                        output[0] = code;
                    }

                    if (code == "MOC")
                    {
                        output[1] = code;
                    }

                    if (code == "OBX")
                    {
                        output[2] = code;
                    }
                    if (code == "ISH")
                    {
                        output[3] = code;
                    }
                    if (code == "OSH")
                    {
                        output[4] = code;
                    }
                    if (code == "PAL")
                    {
                        output[5] = code;
                    }
                }


                output.RemoveAll(x => x.Equals(""));
                return output;
            }
            catch (Exception)
            {

                throw;
            }
        }




        private string checkIsComplianceProductIsValid(PackagingAsso pa, List<PackagingAssoDetails> pad, decimal jobTypeId, bool IsNHNR)
        {
            ComplianceManager mgr = new ComplianceManager();

            var complianceData = mgr.getComplianceData(jobTypeId, IsNHNR).Where(x => x.FieldCode != "N").Select(x => x.FieldCode);

            var nullpaList = Utilities.getNullFieldNames(pa);
            List<string> nullPadsList = new List<string>();
            foreach (var item in pad)
            {
                var da = Utilities.getNullFieldNames(item);
                nullPadsList.AddRange(da);
            }
            var cmn = nullPadsList.Union(nullpaList).Distinct().ToList();
            var data = cmn.Intersect(complianceData).ToList();
            var str = string.Join(",", data.ToArray());

            return str;
        }


        [HttpPost]
        public ActionResult getProductData(decimal PAID, decimal jobTypeId, bool ISNHNR)
        {
            try
            {
                var data = db.PackagingAsso.Find(PAID);
                var dataPackagingAssoDetails = db.PackagingAssoDetails.Where(m => m.PAID == PAID).OrderBy(x => x.Id).ToList();

                var complianceData = checkIsComplianceProductIsValid(data, dataPackagingAssoDetails, jobTypeId, ISNHNR);
                if (complianceData != "")
                {
                    var cvStatus = "error";
                    return Json(complianceData);
                }
                else
                {

                    List<string> availablelevels = new List<string>();

                    foreach (var item in dataPackagingAssoDetails)
                    {
                        string code = item.PackageTypeCode;
                        if (code == "PPB")
                        {
                            availablelevels.Add("PPB");
                        }

                        if (code == "MOC")
                        {
                            availablelevels.Add("MOC");
                        }

                        if (code == "OBX")
                        {
                            availablelevels.Add("OBX");
                        }
                        if (code == "ISH")
                        {
                            availablelevels.Add("ISH");
                        }
                        if (code == "OSH")
                        {
                            availablelevels.Add("OSH");
                        }
                        if (code == "PAL")
                        {
                            availablelevels.Add("PAL");
                        }
                    }
                    availablelevels = sorttheLevels(availablelevels);
                    object[] obj = { data, availablelevels, dataPackagingAssoDetails };
                    var cvStatus = "success";
                    return Json(obj);

                }


            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }

        [HttpPost]
        public ActionResult getProductInfo(decimal PAID)
        {
            try
            {
                var data = db.PackagingAsso.Find(PAID);

                var dataPackagingAssoDetails = db.PackagingAssoDetails.Where(m => m.PAID == PAID).OrderBy(x => x.Id).ToList();

                List<string> availablelevels = new List<string>();

                foreach (var item in dataPackagingAssoDetails)
                {
                    string code = item.PackageTypeCode;
                    if (code == "PPB")
                    {
                        availablelevels.Add("PPB");
                    }

                    if (code == "MOC")
                    {
                        availablelevels.Add("MOC");
                    }

                    if (code == "OBX")
                    {
                        availablelevels.Add("OBX");
                    }
                    if (code == "ISH")
                    {
                        availablelevels.Add("ISH");
                    }
                    if (code == "OSH")
                    {
                        availablelevels.Add("OSH");
                    }
                    if (code == "PAL")
                    {
                        availablelevels.Add("PAL");
                    }
                }
                availablelevels = sorttheLevels(availablelevels);
                object[] obj = { data, availablelevels, dataPackagingAssoDetails };
                return Json(obj);
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }

        private void AddJobDetails()
        {
            try
            {
                //To add/Update jobdetails............
                REDTR.DB.BusinessObjects.PackagingAsso Ps = dbhelper.DBManager.PackagingAssoBLL.GetPackagingAsso(REDTR.DB.BLL.PackagingAssoBLL.PackagingAssoOp.GetProduct, jobinfo.PAID.ToString());
                List<REDTR.DB.BusinessObjects.PackageTypeCode> lstPackageTypeCode = dbhelper.DBManager.PackageTypeCodeBLL.GetPackageTypeCodes();
                List<REDTR.DB.BusinessObjects.PackageTypeCode> templstpcktypecode = new List<REDTR.DB.BusinessObjects.PackageTypeCode>();
                //foreach (Jobdeck jdec in Jdecks) // Jobdeck.jDecks
                foreach (REDTR.DB.BusinessObjects.PackageTypeCode PckCode in lstPackageTypeCode)
                {
                    // UPDATED FOR NON PROPER ENTRIS IN JOBDETAILS TABLE [07.10.2016]
                    PackInfo item = jobinfo.jPackInfoLst.Find(item1 => item1.Deck == PckCode.Code.ParseToDeck());

                    if (item != null)
                    {
                        string deck = item.Deck.ToString();
                        var jbdetail = db.JobDetails.Where(x => x.JD_Deckcode == deck && x.JD_JobID == jobinfo.JobID).FirstOrDefault();
                        REDTR.DB.BusinessObjects.JobDetails Jd = new REDTR.DB.BusinessObjects.JobDetails();
                        Jd.JD_JobID = jobinfo.JobID;
                        Jd.JD_ProdName = jobinfo.Product;
                        Jd.JD_ProdCode = jobinfo.ProductCode;
                        Jd.JD_Description = Ps.Description;
                        Jd.JD_Deckcode = item.Deck.ToString();
                        Jd.JD_GTIN = item.GTIN;
                        Jd.JD_PPN = item.PPN;
                        Jd.JD_GTINCTI = item.GTINCTI;
                        Jd.BundleQty = item.BundleQty;
                        Jd.JD_DeckSize = item.Size;
                        Jd.JD_MRP = item.MRP;
                        Jd.JD_FGCode = Ps.FGCode;
                        Jd.LabelName = jbdetail.LabelName;
                        Jd.Filter = jbdetail.Filter;
                        dbhelper.DBManager.JobDetailsBLL.AddJobDetails(Jd);
                        templstpcktypecode.Add(PckCode);
                        Trace.TraceInformation("Job details are added at AddJobDetails function : {0},Deck In PackageTypeCode : {1},Added deck : {2}", DateTime.Now.ToString(), PckCode.Code, item.Deck.ToString());
                    }
                }

                List<REDTR.DB.BusinessObjects.JobDetails> JBDetails = dbhelper.DBManager.JobDetailsBLL.GetJobDetailss(REDTR.DB.BLL.JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, jobinfo.JobID, 1);
                foreach (REDTR.DB.BusinessObjects.JobDetails jbdtls in JBDetails)
                {
                    if (jbdtls.JD_Deckcode == templstpcktypecode[0].Code)
                    {
                        Trace.TraceInformation("After adding job details info, cross checking the details In Case of Proper : {0},Deck In PackageTypeCode : {1},Added deck after retriving : {2}", DateTime.Now.ToString(), templstpcktypecode[0].Code, jbdtls.JD_Deckcode.ToString());
                        templstpcktypecode.RemoveAt(0);

                    }
                    else
                    {
                        dbhelper.DBManager.JobDetailsBLL.RemoveJobDetails(jobinfo.JobID);
                        Trace.TraceInformation("After adding job details info, At the time of cross checking If entry from jobdetails does not match with packagetype code entries then jobdetails entries are deleted and again called AddJobDetails function : {0},For Job : {1},PackageTypeCode deck : {2},jobdetails deck :{3}", DateTime.Now.ToString(), jobinfo.BatchNo, templstpcktypecode[0].Code, jbdtls.JD_Deckcode.ToString());
                        AddJobDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Exception At AddJobDetails : {0},{1},{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
        }


        private void addToJobDetails(string[] values, decimal JobId, string PackageTypeCode, PackagingAsso pa)
        {
            try
            {
                JobDetails jd = new JobDetails();
                try
                {
                    if (Convert.ToInt32(values[3]) >= 0)
                    {

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModelErroJobInvalidPCmap);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModelErroJobInvalidPCmap);
                }


                try
                {
                    if (Convert.ToInt32(values[4]) >= 0)
                    {

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModuleErrorJobInvalidBundelQty);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModuleErrorJobInvalidBundelQty);
                }

                try
                {
                    if (values[7] != "")
                    {

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.TempDataJobInvalidLblName);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.TempDataJobInvalidLblName);
                }

                try
                {
                    if (values[8] != "")
                    {

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.TempDataJobInvalidFilter);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.TempDataJobInvalidFilter);
                }

                if (ModelState.IsValid)
                {
                    jd.JD_JobID = JobId;
                    jd.JD_ProdName = pa.Name;
                    jd.JD_ProdCode = pa.ProductCode;
                    jd.JD_FGCode = pa.FGCode;
                    jd.JD_Deckcode = PackageTypeCode;
                    jd.LastUpdatedDate = DateTime.Now;
                    jd.LineCode = pa.LineCode;
                    jd.SYNC = false;

                    //table
                    jd.JD_PPN = values[0];
                    jd.JD_GTIN = values[1];
                    jd.JD_NTIN = values[2];
                    jd.JD_DeckSize = Convert.ToInt32(values[4]);
                    jd.JD_MRP = Convert.ToDecimal(values[3]);
                    jd.BundleQty = Convert.ToInt32(values[5]);
                    jd.Filter = values[9];
                    jd.LabelName = values[8];

                    REDTR.DB.BusinessObjects.JobDetails Jd = new REDTR.DB.BusinessObjects.JobDetails();
                    Jd.JD_JobID = jd.JD_JobID;
                    Jd.JD_ProdName = jd.JD_ProdName;
                    Jd.JD_ProdCode = jd.JD_ProdCode;
                    Jd.JD_Description = jd.JD_Description;
                    Jd.JD_Deckcode = PackageTypeCode;
                    Jd.JD_GTIN = jd.JD_GTIN;
                    Jd.JD_PPN = values[0];
                    //Jd.JD_GTINCTI = item.GTINCTI;
                    Jd.BundleQty = jd.BundleQty;
                    Jd.JD_DeckSize = jd.JD_DeckSize;
                    Jd.JD_MRP = jd.JD_MRP;
                    Jd.JD_FGCode = jd.JD_FGCode;
                    Jd.LabelName = jd.LabelName;
                    Jd.Filter = jd.Filter;
                    Jd.JD_NTIN = jd.JD_NTIN;
                    //dbhelper.DBManager.JobDetailsBLL.AddJobDetails(Jd);
                    LstJobDtls.Add(Jd);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JID,JobName,PAID,BatchNo,MfgDate,ExpDate,Quantity,SurPlusQty,JobStatus,DetailInfo,JobStartTime,JobEndTime,LabelStartIndex,AutomaticBatchCloser,TID,MLNO,TenderText,JobWithUID,Remarks,CreatedBy,VerifiedBy,VerifiedDate,CreatedDate,LastUpdatedDate,AppId,LineCode,SYNC,ForExport,PrimaryPCMapCount,DavaPortalUpload,PlantCode,NoReadCount,ExpDateFormat,UseExpDay,ProviderId,CustomerId,PackagingLvlId,PPNCountryCode,PPNPostalCode, CompType")] Job job)
        {
            try
            {
                #region Validations
                var selectedCustomer = db.M_Customer.FirstOrDefault(s => s.Id == job.CustomerId);
                if (selectedCustomer == null)
                {
                    TempData["Success"] = "CUSTOMER INFO NOT FOUND TRY AGAIN LATER";
                    return RedirectToAction("Create");
                }
                if (selectedCustomer.IsSSCC == null || string.IsNullOrEmpty(selectedCustomer.IsSSCC.ToString()))
                {
                    TempData["Success"] = "SSCC CONFIG NOT FOUND CONTACT ADMINISTRATOR";
                    return RedirectToAction("Create");
                }
                if (selectedCustomer.IsProvideCodeForMiddleDeck == null || string.IsNullOrEmpty(selectedCustomer.IsProvideCodeForMiddleDeck.ToString()))
                {
                    TempData["Success"] = "MIDDLE DECK CONFIG NOT FOUND CONTACT ADMINISTRATOR";
                    return RedirectToAction("Create");
                }
                if (string.IsNullOrEmpty(selectedCustomer.SSCCExt) || string.IsNullOrEmpty(selectedCustomer.LoosExt))
                {
                    TempData["Success"] = "SSCC EXTENTION CONFIG NOT FOUND CONTACT ADMINISTRATOR";
                    return RedirectToAction("Create");
                }
                if (selectedCustomer.SSCCExt.Length > 1 || selectedCustomer.LoosExt.Length > 1)
                {
                    TempData["Success"] = "INVALID SSCC EXTENTION FOUND CONTACT ADMINISTRATOR";
                    return RedirectToAction("Create");
                }

                #endregion

                //jobinfo = FillJObINFO(job, "NewJob");
                string PPBValues, MOCValues, OBXValues, ISHValues, OSHValues, PALValues, MOCPartial, OBXPartial, ISHPartial, OSHPartial, PALPartial;

                string[] values;
                var lvls = "";
                PPBValues = Request["PPB[]"];
                values = PPBValues.Split(',');
                if (PPBValues != ",,,,,,,,,")
                {
                    lvls += "PPB-";
                }


                MOCValues = Request["MOC[]"];
                values = MOCValues.Split(',');
                if (duallabel)
                {
                    MOCPartial = Request["PartialMOC[]"];
                    values[8] = values[8] + "," + MOCPartial;
                }
                if (MOCValues != ",,,,,,,,,")
                {
                    lvls += "MOC-";
                }

                OBXValues = Request["OBX[]"];
                values = OBXValues.Split(',');
                if (duallabel)
                {
                    OBXPartial = Request["PartialOBX[]"];
                    values[8] = values[8] + "," + OBXPartial;
                }
                if (OBXValues != ",,,,,,,,,")
                {
                    lvls += "OBX-";
                }

                ISHValues = Request["ISH[]"];
                values = ISHValues.Split(',');
                if (duallabel)
                {
                    ISHPartial = Request["PartialISH[]"];
                    values[8] = values[8] + "," + ISHPartial;
                }
                if (ISHValues != ",,,,,,,,,")
                {
                    lvls += "ISH-";
                }

                OSHValues = Request["OSH[]"];
                values = OSHValues.Split(',');
                if (duallabel)
                {
                    OSHPartial = Request["PartialOSH[]"];
                    values[8] = values[8] + "," + OSHPartial;
                }
                if (OSHValues != ",,,,,,,,,")
                {
                    lvls += "OSH-";
                }

                PALValues = Request["PAL[]"];
                values = PALValues.Split(',');
                if (duallabel)
                {
                    PALPartial = Request["PartialPAL[]"];
                    values[8] = values[8] + "," + PALPartial;
                }
                if (PALValues != ",,,,,,,,,")
                {
                    lvls += "PAL-";
                }
                lvls = lvls.TrimEnd('-');
                string[] pacLvl = lvls.Split('-');

                var firstLvl = pacLvl[0];
                var lastLvl = pacLvl[pacLvl.Count() - 1];

                job.LabelStartIndex = 1;
                job.JobStatus = (int)JobState.Created;
                job.CreatedDate = DateTime.Now;
                job.LastUpdatedDate = DateTime.Now;
                job.JobStartTime = DateTime.Now;
                job.SYNC = false;
                job.DavaPortalUpload = false;
                //string mfg= DateTime.ParseExact(job.MfgDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                //DateTime MDate = Convert.ToDateTime(mfg);
                //string exp = DateTime.ParseExact(job.ExpDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                //DateTime EDate = Convert.ToDateTime(exp);
                var cust = db.M_Customer.Find(job.CustomerId);
                job.ProviderId = cust.ProviderId;
                //job.CreatedBy = HttpContext.Current.User.Identity.GetUserId();

                REDTR.DB.BusinessObjects.Job jb = new REDTR.DB.BusinessObjects.Job();
                //jb.JID = job.;
                jb.UseExpDay = job.UseExpDay;
                jb.ExpDateFormat = job.ExpDateFormat;
                jb.JobName = job.JobName;
                jb.PAID = job.PAID;
                jb.BatchNo = job.BatchNo;
                jb.MfgDate = job.MfgDate;
                jb.ExpDate = job.ExpDate;
                jb.CreatedBy = User.ID;//m_UserInfo.ID;
                jb.JobStartTime = job.JobStartTime;
                jb.Quantity = job.Quantity;
                jb.SurPlusQty = job.SurPlusQty;
                jb.JobStatus = Convert.ToSByte(job.JobStatus);
                jb.AutomaticBatchCloser = job.AutomaticBatchCloser;
                jb.TID = job.TID;
                jb.MLNO = job.MLNO;
                jb.TenderText = job.TenderText;
                jb.JobWithUID = job.JobWithUID;
                jb.ForExport = job.ForExport;
                jb.PrimaryPCMapCount = Convert.ToInt32(job.PrimaryPCMapCount);
                jb.AppId = 1;
                jb.DAVAPortalUpload = false;
                jb.NoReadCount = 0;
                jb.LabelStartIndex = job.LabelStartIndex;
                jb.PackagingLvlId = job.PackagingLvlId;
                jb.CustomerId = job.CustomerId;
                jb.ProviderId = job.ProviderId;
                jb.PPNCountryCode = job.PPNCountryCode;
                jb.PPNPostalCode = job.PPNPostalCode;
                jb.CompType = job.CompType;
                // Job Details
                var mode = db.PackagingAsso.Find(job.PAID);
                var lblmaster = db.PackageLabelMaster.Find(job.PAID);
                if (PPBValues != ",,,,,,,,,")
                {
                    values = PPBValues.Split(',');
                    addToJobDetails(values, job.JID, "PPB", mode);
                }
                if (MOCValues != ",,,,,,,,,")
                {
                    values = MOCValues.Split(',');
                    if (duallabel)
                    {
                        MOCPartial = Request["PartialMOC[]"];
                        values[8] = values[8] + "," + MOCPartial;
                    }
                    addToJobDetails(values, job.JID, "MOC", mode);
                }
                if (OBXValues != ",,,,,,,,,")
                {
                    values = OBXValues.Split(',');
                    if (duallabel)
                    {
                        OBXPartial = Request["PartialOBX[]"];
                        values[8] = values[8] + "," + OBXPartial;
                    }
                    addToJobDetails(values, job.JID, "OBX", mode);
                }
                if (ISHValues != ",,,,,,,,,")
                {
                    values = ISHValues.Split(',');
                    if (duallabel)
                    {
                        ISHPartial = Request["PartialISH[]"];
                        values[8] = values[8] + "," + ISHPartial;
                    }
                    addToJobDetails(values, job.JID, "ISH", mode);
                }
                if (OSHValues != ",,,,,,,,,")
                {
                    values = OSHValues.Split(',');
                    if (duallabel)
                    {
                        OSHPartial = Request["PartialOSH[]"];
                        values[8] = values[8] + "," + OSHPartial;
                    }
                    addToJobDetails(values, job.JID, "OSH", mode);
                }
                if (PALValues != ",,,,,,,,,")
                {
                    values = PALValues.Split(',');
                    if (duallabel)
                    {
                        PALPartial = Request["PartialPAL[]"];
                        values[8] = values[8] + "," + PALPartial;
                    }
                    addToJobDetails(values, job.JID, "PAL", mode);
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    jb.LineCode = job.LineCode;
                    jb.JID = job.JID = dbhelper.DBManager.JobBLL.AddJob(jb);
                    dbhelper.UpdateLabelStartIndexofJob(job.JID, Convert.ToDecimal(job.LabelStartIndex), 1);

                    foreach (var item in LstJobDtls)
                    {
                        item.JD_JobID = jb.JID;

                        dbhelper.DBManager.JobDetailsBLL.AddJobDetails(item);

                    }

                    trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobcreatedbatch + " " + job.BatchNo, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobcreatedbatch + " " + job.JobName, TnT.LangResource.GlobalRes.TrailActionBatchActivity);

                    var customer = db.M_Customer.Find(jb.CustomerId);
                    var provider = db.M_Providers.Find(customer.ProviderId); //Where(x => x.ProviderId == customer.ProviderId).Select(x => x.Proivder.Name);
                    var IAC_CIN = db.Settings.FirstOrDefault();

                    var JobTYpe = db.JOBTypes.Where(x => x.TID == job.TID).FirstOrDefault();
                    var selectedJobType = JobTYpe.Job_Type;
                    //var mailingUsers = User.
                    //If tracelink do not generate Ids
                    if (provider.Code== UIDCustomType.PROP.ToString())
                    {
                        IDDataGeneratorUtility util = new IDDataGeneratorUtility();

                        if (util.GenerateCodePropix(job, firstLvl, lastLvl, selectedJobType, IAC_CIN.IAC_CIN,false)) //(GenerateIds(job, firstLvl, lastLvl, selectedJobType, IAC_CIN.IAC_CIN))
                        {
                            TempData["Success"] = "'" + job.BatchNo + "' " + TnT.LangResource.GlobalRes.TempDataJobcreatedsuccessfully;
                            jb.CreatedDate = job.CreatedDate;
                            var userData = User;
                            batchNotificationHlpr.notifyPerimissionHolders(ref jb, BatchEventType.batchCreationEvent, userData);
                        }
                        else
                        {
                            TempData["Success"] = "'" + job.BatchNo + "' " + TnT.LangResource.GlobalRes.TempDataJobproblemcreatingpackagingdetails;
                        }
                    }
                    else
                    {
                        IDDataGeneratorUtility util = new IDDataGeneratorUtility();
                        if (util.GenerateCodeNonPropix(job, firstLvl, lastLvl, selectedJobType, IAC_CIN.IAC_CIN,false)) 
                        //if (GenerateDataforTracelink(job, firstLvl, lastLvl, selectedJobType, IAC_CIN.IAC_CIN))
                        {
                            TempData["Success"] = "'" + job.BatchNo + "' " + TnT.LangResource.GlobalRes.TempDataJobcreatedsuccessfully;
                            jb.CreatedDate = job.CreatedDate;
                            var userData = User;
                            batchNotificationHlpr.notifyPerimissionHolders(ref jb, BatchEventType.batchCreationEvent, userData);
                        }
                        else
                        {
                            TempData["Success"] = "'" + job.BatchNo + "' " + TnT.LangResource.GlobalRes.TempDataJobproblemcreatingpackagingdetails;
                        }
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    BindDDL();
                    return View(job);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionLogger.logException(ex);
                return View("HError", ex);
            }
        }


        public ActionResult getTypeWiseProducts(int JobId)
        {

            var data = (from prod in db.PackagingAsso join lbl in db.PackageLabelMaster on prod.PAID equals lbl.PAID where prod.VerifyProd == true && prod.IsActive == true select new { prod.PAID, prod.FGCode }).Distinct().ToList();
            return Json(data);
        }

        private bool GenerateSSCCs(Job job, string IAC_CIN)
        {
            var JobTYpe = db.JOBTypes.Where(x => x.TID == job.TID).FirstOrDefault();
            var selectedJobType = JobTYpe.Job_Type;

            DemoSSCC demo = new DemoSSCC();
            int cnt = demo.getSSCCToPrint(job);
            var tertLvl = demo.getTertDeck();

            var pacAssoDet = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID && x.PackageTypeCode == tertLvl).FirstOrDefault();


            M_Identities mids = new M_Identities();
            mids.CreatedOn = DateTime.Now;
            mids.CustomerId = (int)job.CustomerId;
            mids.GTIN = pacAssoDet.GTIN;
            mids.JID = job.JID;
            mids.PackageTypeCode = pacAssoDet.PackageTypeCode;
            mids.IsExtra = false;
            mids.IsTransfered = false;
            db.M_Identities.Add(mids);
            db.SaveChanges();


            IDGenrationFactory obj = new IDGenrationFactory();
            var UIDsSSCC = obj.generateSSCC((int)job.JID, cnt, selectedJobType, IAC_CIN);
            Dictionary<string, string> masterList = new Dictionary<string, string>();
            if (UIDsSSCC.Count > 0)
            {
                Dictionary<string, string> ids = new Dictionary<string, string>();
                foreach (var item in UIDsSSCC)
                {
                    ids.Add(item, "SSC");
                }
                masterList = masterList.Union(ids).ToDictionary(k => k.Key, v => v.Value);


                IDDataGeneratorUtility util = new IDDataGeneratorUtility();
                var ConvertedData = util.converUIDData(masterList, mids.Id, tertLvl);

                DataTable DtconvertedData = GeneralDataHelper.convertToDataTable(ConvertedData);

                BulkDataHelper bulkHlpr = new BulkDataHelper();
                return bulkHlpr.InsertUIDIdenties(DtconvertedData);
            }
            return false;

        }
        
        private bool GenerateIdsAdditionbatchqty(Job job, string firstLvl, string lastLvl, string selectedJobType, string IAC_CIN)
        {
            IDDataGeneratorUtility util = new IDDataGeneratorUtility();
            return util.GenerateDataAdditionalBatchQty(job, firstLvl, lastLvl, selectedJobType, IAC_CIN);
        }
        private bool GenerateDataforTracelink(Job job, string firstLvl, string lastLvl, string selectedJobType, string IAC_CIN)
        {
            IDDataGeneratorUtility util = new IDDataGeneratorUtility();
            return util.GenerateTracelinkData(job, firstLvl, lastLvl, selectedJobType, IAC_CIN);
        }

        private bool GenerateDataforTracelinkAdditionalBatchQty(Job job, string firstLvl, string lastLvl, string selectedJobType, string IAC_CIN)
        {
            IDDataGeneratorUtility util = new IDDataGeneratorUtility();
            return util.GenerateTracelinkDataAdditionalBatchQty(job, firstLvl, lastLvl, selectedJobType, IAC_CIN);
        }
        private bool GenerateDataforTraceKeyAdditionalBatchQty(Job job, string firstLvl, string lastLvl, string selectedJobType, string IAC_CIN)
        {
            IDDataGeneratorUtility util = new IDDataGeneratorUtility();
            return util.GenerateTraceKeyCodeAdditionalBatchQty(job, firstLvl, lastLvl, selectedJobType, IAC_CIN);
        }
        // GET: Job/Edit/5
        public ActionResult Edit(decimal id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Job job = db.Job.Find(id);
                var jobDetails = db.JobDetails.Where(m => m.JD_JobID == id);
                var selectedJobType = db.JOBTypes.Where(x => x.TID == job.TID).FirstOrDefault();
                ViewBag.SelectedJobType = selectedJobType.Job_Type;
                ViewBag.JobDetails = jobDetails;
                ViewBag.DateFormats = db.S_DateFormats;
                BindEditDDL(id);

                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobREQUESTEDEDITBATCH + " " + job.JobName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobREQUESTEDEDITBATCH + " " + job.JobName, TnT.LangResource.GlobalRes.TrailActionBatchActivity);

                return View(job);
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JID,JobName,PAID,BatchNo,MfgDate,ExpDate,Quantity,SurPlusQty,JobStatus,DetailInfo,JobStartTime,JobEndTime,LabelStartIndex,AutomaticBatchCloser,TID,MLNO,TenderText,JobWithUID,Remarks,CreatedBy,VerifiedBy,VerifiedDate,CreatedDate,LastUpdatedDate,AppId,LineCode,SYNC,ForExport,PrimaryPCMapCount,DavaPortalUpload,PlantCode,NoReadCount,ExpDateFormat,UseExpDay,ProviderId,CustomerId,PackagingLvlId,PPNCountryCode,PPNPostalCode, CompType")] Job job)
        {
            try
            {

                var oOldRecord = db.Job.Where(x => x.JID == job.JID).FirstOrDefault();
                var oNewRecord = job;


                List<string> compare = new List<string>();
                compare.Add("JobName");
                compare.Add("BatchNo");
                compare.Add("MfgDate");
                compare.Add("ExpDate");
                compare.Add("LineCode");
                compare.Add("PrimaryPCMapCount");
                compare.Add("ExpDateFormat");
                compare.Add("UseExpDay");
                compare.Add("ForExport");
                compare.Add("JobWithUID");
                compare.Add("AutomaticBatchCloser");

                System.Reflection.PropertyInfo[] properties = oNewRecord.GetType().GetProperties();
                string msg = "";
                foreach (var oProperty in properties)
                {
                    var oOldValue = oProperty.GetValue(oOldRecord, null);
                    var oNewValue = oProperty.GetValue(oNewRecord, null);
                    // this will handle the scenario where either value is null
                    if (compare.Contains(oProperty.Name))
                        if (!object.Equals(oOldValue, oNewValue))
                        {
                            // Handle the display values when the underlying value is null
                            var sOldValue = oOldValue == null ? "null" : oOldValue.ToString();
                            var sNewValue = oNewValue == null ? "null" : oNewValue.ToString();

                            //msg += oProperty.Name + " was: " + sOldValue + "; is changed to: " + sNewValue + " ,";
                            msg += oProperty.Name + " " + TnT.LangResource.GlobalRes.RptAuditTrailWas + ":" + sOldValue + ";" + " " + TnT.LangResource.GlobalRes.RptAuditTrailIsChngeTo + " : " + sNewValue + " ,";

                        }
                }



                //jobinfo = FillJObINFO(job,"");
                string PPBValues, MOCValues, OBXValues, ISHValues, OSHValues, PALValues, MOCPartial, OBXPartial, ISHPartial, OSHPartial, PALPartial;
                string[] values;
                PPBValues = Request["PPB[]"];
                values = PPBValues.Split(',');
                MOCValues = Request["MOC[]"];
                values = MOCValues.Split(',');
                OBXValues = Request["OBX[]"];
                values = MOCValues.Split(',');
                ISHValues = Request["ISH[]"];
                values = MOCValues.Split(',');
                OSHValues = Request["OSH[]"];
                values = MOCValues.Split(',');
                PALValues = Request["PAL[]"];
                values = MOCValues.Split(',');

                job.LabelStartIndex = 1;
                job.JobStatus = (int)JobState.Created;
                job.CreatedDate = DateTime.Now;
                job.LastUpdatedDate = DateTime.Now;
                job.JobStartTime = DateTime.Now;
                job.SYNC = false;
                job.DavaPortalUpload = false;
                var cust = db.M_Customer.Find(job.CustomerId);
                job.ProviderId = cust.ProviderId;

                REDTR.DB.BusinessObjects.Job jb = new REDTR.DB.BusinessObjects.Job();
                jb.JID = job.JID;
                jb.JobName = job.JobName;
                jb.PAID = job.PAID;
                jb.BatchNo = job.BatchNo;
                jb.MfgDate = job.MfgDate;
                jb.ExpDate = job.ExpDate;
                jb.CreatedBy = User.ID;//m_UserInfo.ID;
                jb.JobStartTime = job.JobStartTime;
                jb.Quantity = job.Quantity;
                jb.SurPlusQty = job.SurPlusQty;
                jb.JobStatus = Convert.ToSByte(job.JobStatus);
                jb.AutomaticBatchCloser = job.AutomaticBatchCloser;
                jb.TID = job.TID;
                jb.MLNO = job.MLNO;
                jb.TenderText = job.TenderText;
                jb.JobWithUID = job.JobWithUID;
                jb.ForExport = job.ForExport;
                jb.PrimaryPCMapCount = Convert.ToInt32(job.PrimaryPCMapCount);
                jb.AppId = 1;
                jb.DAVAPortalUpload = false;
                jb.NoReadCount = 0;
                jb.UseExpDay = job.UseExpDay;
                jb.ExpDateFormat = job.ExpDateFormat;
                jb.PackagingLvlId = job.PackagingLvlId;

                jb.CustomerId = job.CustomerId;
                jb.ProviderId = job.ProviderId;
                jb.PPNCountryCode = job.PPNCountryCode;
                jb.PPNPostalCode = job.PPNPostalCode;
                jb.CompType = job.CompType;
                //AddJobDetails();
                var mode = db.PackagingAsso.Find(job.PAID);

                if (PPBValues != ",,,,,,,,,")
                {
                    values = PPBValues.Split(',');

                    addToJobDetails(values, job.JID, "PPB", mode);
                }
                if (MOCValues != ",,,,,,,,,")
                {
                    values = MOCValues.Split(',');
                    if (duallabel)
                    {
                        MOCPartial = Request["PartialMOC[]"];
                        values[8] = values[8] + "," + MOCPartial;
                    }
                    addToJobDetails(values, job.JID, "MOC", mode);
                }
                if (OBXValues != ",,,,,,,,,")
                {
                    values = OBXValues.Split(',');
                    if (duallabel)
                    {
                        OBXPartial = Request["PartialOBX[]"];
                        values[8] = values[8] + "," + OBXPartial;
                    }
                    addToJobDetails(values, job.JID, "OBX", mode);
                }
                if (ISHValues != ",,,,,,,,,")
                {
                    values = ISHValues.Split(',');
                    if (duallabel)
                    {
                        ISHPartial = Request["PartialISH[]"];
                        values[8] = values[8] + "," + ISHPartial;
                    }
                    addToJobDetails(values, job.JID, "ISH", mode);
                }
                if (OSHValues != ",,,,,,,,,")
                {
                    values = OSHValues.Split(',');
                    if (duallabel)
                    {
                        OSHPartial = Request["PartialOSH[]"];
                        values[8] = values[8] + "," + OSHPartial;
                    }
                    addToJobDetails(values, job.JID, "OSH", mode);
                }
                if (PALValues != ",,,,,,,,,")
                {
                    values = PALValues.Split(',');
                    if (duallabel)
                    {
                        PALPartial = Request["PartialPAL[]"];
                        values[8] = values[8] + "," + PALPartial;
                    }
                    addToJobDetails(values, job.JID, "PAL", mode);
                }



                if (ModelState.IsValid)
                {
                    jb.LineCode = job.LineCode;
                    dbhelper.DBManager.JobBLL.UpdateJobDetails(REDTR.DB.BLL.JobBLL.JobOp.UpdateJob, jb);
                    dbhelper.UpdateLabelStartIndexofJob(job.JID, (decimal)job.LabelStartIndex, 1);

                    foreach (var item in LstJobDtls)
                    {
                        var jobdetail = db.JobDetails.Where(x => x.JD_JobID == job.JID && x.JD_Deckcode == item.JD_Deckcode).FirstOrDefault();
                        if (jobdetail.LabelName != item.LabelName)
                        {
                            //msg += " LabelName was:" + jobdetail.LabelName + " ;is changed to :" + item.LabelName + ",";
                            msg += TnT.LangResource.GlobalRes.ProductLabelName + " " + TnT.LangResource.GlobalRes.RptAuditTrailWas + ":" + jobdetail.LabelName + ";" + " " + TnT.LangResource.GlobalRes.RptAuditTrailIsChngeTo + " : " + item.LabelName + ",";

                        }
                        if (jobdetail.JD_MRP != item.JD_MRP)
                        {
                            // msg += " MRP was:" + jobdetail.JD_MRP + " ;is changed to :" + item.JD_MRP + ",";
                            msg += TnT.LangResource.GlobalRes.ProductMRP + " " + TnT.LangResource.GlobalRes.RptAuditTrailWas + ":" + jobdetail.JD_MRP + ";" + " " + TnT.LangResource.GlobalRes.RptAuditTrailIsChngeTo + " : " + item.JD_MRP + ",";

                        }
                        item.JD_JobID = jb.JID;
                        dbhelper.DBManager.JobDetailsBLL.AddJobDetails(item);
                    }
                    msg = msg.TrimEnd(',');
                    if (msg == "")
                    {
                        msg = User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataJobupdatedsuccessfully;
                    }
                    else
                    {
                        msg += TnT.LangResource.GlobalRes.RptAuditTrailUsersFor + " :";
                    }

                    trail.AddTrail(msg + "" + job.JobName, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                    TempData["Success"] = "'" + job.JobName + "' " + TnT.LangResource.GlobalRes.TempDataJobupdatedsuccessfully;
                    return RedirectToAction("Index");
                }
                else
                {
                    BindDDL();
                    return View(job);
                }


            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }


        public JobInfo FillJObINFOSM(REDTR.DB.BusinessObjects.Job jobb, string Type)
        {
            JobInfo jobinfo = new JobInfo();
            try
            {
                //JOBType JT = dbhelper.DBManager.JOBTypeBLL.GetJOBTypeByID(m_TypeID);
                jobinfo.JobID = jobb.JID;
                jobinfo.PAID = jobb.PAID;


                REDTR.DB.BusinessObjects.PackagingAsso ps = dbhelper.DBManager.PackagingAssoBLL.GetPackagingAsso(PackagingAssoBLL.PackagingAssoOp.GetProduct, jobinfo.PAID.ToString());
                jobinfo.Product = ps.Name;
                jobinfo.ProductCode = ps.ProductCode;
                jobinfo.FGCode = ps.FGCode; // Murtaza.
                                            //}
                jobinfo.BatchNo = jobb.BatchNo;
                jobinfo.Mfg = jobb.MfgDate;
                jobinfo.Exp = jobb.ExpDate;
                jobinfo.ForExport = jobb.ForExport;
                jobinfo.PrimaryPCMapCount = (int)jobb.PrimaryPCMapCount;
                jobinfo.JobName = jobb.JobName;
                jobinfo.BatchQty = jobb.Quantity;

                jobinfo.SurplusQty = (int)jobb.SurPlusQty;
                jobinfo.AutoBatchCloser = jobb.AutomaticBatchCloser;

                REDTR.DB.BusinessObjects.JOBType Jt = dbhelper.DBManager.JOBTypeBLL.GetJOBTypeByID(Convert.ToDecimal(jobb.TID));
                Globals.JobType = Jt.Job_Type;
                jobinfo.TypeID = Jt.TID;
                jobinfo.TypeName = Jt.Job_Type;


                jobinfo.TenderText = jobb.TenderText;
                jobinfo.JobWithUID = (bool)jobb.JobWithUID;
                jobinfo.LabelStartIndex = jobb.LabelStartIndex;


                ProductPackInfoHelper pphObj = new ProductPackInfoHelper(jobinfo.JobID, jobinfo.PAID, jobinfo.BatchQty, jobinfo.SurplusQty);
                jobinfo.jPackInfoLst = pphObj.GetPackInfoLst();

            }
            catch (Exception ex)
            {
                Trace.TraceError("{0}, FillJObInfo=> {1}, {2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);

            }
            return jobinfo;
        }

        public JobInfo FillJObINFO(REDTR.DB.BusinessObjects.Job job, string Type)
        {
            JobInfo jobinfo = new JobInfo();
            try
            {
                //JOBType JT = dbhelper.DBManager.JOBTypeBLL.GetJOBTypeByID(m_TypeID);
                jobinfo.JobID = job.JID;
                jobinfo.PAID = job.PAID;

                if (Type == "NewJob")
                {
                    REDTR.DB.BusinessObjects.JobDetails jobDetails = dbhelper.DBManager.JobDetailsBLL.GetJobDetails(REDTR.DB.BLL.JobDetailsBLL.JobDetailsOp.GetDetailsWithJIDDeck, job.JID, REDTR.UTILS.PackBoxes.GET_FirstDeck().ToString());
                    if (jobDetails.JD_JobID > 0)
                    {
                        jobinfo.Product = jobDetails.JD_ProdName;
                        jobinfo.ProductCode = jobDetails.JD_ProdCode;
                        jobinfo.FGCode = jobDetails.JD_FGCode;
                    }
                    else
                    {
                        REDTR.DB.BusinessObjects.PackagingAsso ps = dbhelper.DBManager.PackagingAssoBLL.GetPackagingAsso(REDTR.DB.BLL.PackagingAssoBLL.PackagingAssoOp.GetProduct, jobinfo.PAID.ToString());
                        jobinfo.Product = ps.Name;
                        jobinfo.ProductCode = ps.ProductCode;
                        jobinfo.FGCode = ps.FGCode;
                    }
                }
                else
                {
                    REDTR.DB.BusinessObjects.PackagingAsso ps = dbhelper.DBManager.PackagingAssoBLL.GetPackagingAsso(REDTR.DB.BLL.PackagingAssoBLL.PackagingAssoOp.GetProduct, jobinfo.PAID.ToString());
                    jobinfo.Product = ps.Name;
                    jobinfo.ProductCode = ps.ProductCode;
                    jobinfo.FGCode = ps.FGCode;
                }

                jobinfo.BatchNo = job.BatchNo;
                jobinfo.Mfg = job.MfgDate;
                jobinfo.Exp = job.ExpDate;
                jobinfo.ForExport = job.ForExport;
                jobinfo.PrimaryPCMapCount = (int)job.PrimaryPCMapCount;
                jobinfo.JobName = job.JobName;
                jobinfo.BatchQty = job.Quantity;

                jobinfo.SurplusQty = (int)job.SurPlusQty;
                jobinfo.AutoBatchCloser = job.AutomaticBatchCloser;

                REDTR.DB.BusinessObjects.JOBType Jt = dbhelper.DBManager.JOBTypeBLL.GetJOBTypeByID(Convert.ToDecimal(job.TID));
                Globals.JobType = Jt.Job_Type;
                jobinfo.TypeID = Jt.TID;
                jobinfo.TypeName = Jt.Job_Type;


                jobinfo.TenderText = job.TenderText;
                jobinfo.JobWithUID = (bool)job.JobWithUID;
                jobinfo.LabelStartIndex = job.LabelStartIndex;
                jobinfo.jPackInfoLst = GetPackInfoLst(job);


            }
            catch (Exception ex)
            {
                Trace.TraceError("{0}, FillJObInfo=> {1}, {2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
            return jobinfo;
        }


        public List<PackInfo> GetPackInfoLst(REDTR.DB.BusinessObjects.Job job)
        {
            LstpackInfo = new List<PackInfo>();
            List<REDTR.DB.BusinessObjects.JobDetails> Jd = dbhelper.DBManager.JobDetailsBLL.GetJobDetailss(REDTR.DB.BLL.JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, job.JID, -1);
            if (Jd.Count > 0)
            {
                foreach (var item in Jd)
                {
                    PackInfo PckInfo = new PackInfo();
                    PckInfo.PPN = item.JD_PPN;
                    PckInfo.GTIN = item.JD_GTIN;
                    PckInfo.GTINCTI = item.JD_GTINCTI;
                    PckInfo.Deck = item.JD_Deckcode.ParseToDeck();
                    PckInfo.Size = item.JD_DeckSize;
                    PckInfo.BundleQty = item.BundleQty;
                    PckInfo.MRP = item.JD_MRP;
                    PckInfo.FGCode = item.JD_FGCode;
                    LstpackInfo.Add(PckInfo);

                }
            }
            else
            {
                LstpackInfo = new List<PackInfo>();
                List<REDTR.DB.BusinessObjects.PackagingAssoDetails> PDLst = dbhelper.DBManager.PackagingAssoDetailsBLL.GetPckAssoDtlss(job.PAID);
                foreach (REDTR.DB.BusinessObjects.PackagingAssoDetails item in PDLst)
                {
                    if (item.PackageTypeCode.ParseToDeck().IsExists() == false)
                        continue;
                    if (item.Size == -1)
                        continue;
                    PackInfo PckInfo = new PackInfo();
                    PckInfo.PPN = item.PPN;
                    PckInfo.GTIN = item.GTIN;
                    PckInfo.GTINCTI = item.GTINCTI;
                    PckInfo.Deck = item.PackageTypeCode.ParseToDeck();
                    PckInfo.Size = (int)item.Size;
                    PckInfo.BundleQty = (int)item.BundleQty;
                    PckInfo.MRP = item.MRP;
                    PckInfo.UIDsToPRint = 0;
                    LstpackInfo.Add(PckInfo);
                }
            }



            return LstpackInfo;
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(decimal id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Job job = db.Job.Find(id);
                if (job == null)
                {
                    return HttpNotFound();
                }
                var jobDetails = db.JobDetails.Where(m => m.JD_JobID == id);
                ViewBag.JobDetails = jobDetails;

                BindDDL();
                return View(job);


            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }

        // POST: Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            try
            {
                Job job = db.Job.Find(id);
                if (job.VerifiedBy == null)
                {
                    dbhelper.DBManager.JobBLL.RemoveJob(REDTR.DB.BLL.JobBLL.JobOp.DeleteJob, id);
                    dbhelper.DBManager.JobDetailsBLL.RemoveJobDetails(id);
                    string msg = TnT.LangResource.GlobalRes.TempDataJobUserDeletedBatch + job.JobName;  //rm.GetString("FrmJobManager.Msg_SuccDelete", cul);
                    dbhelper.AddUserTrail(User.ID, null, USerTrailWHERE.TnT1, "JOB MANAGER", msg, string.Empty);
                    TempData["Success"] = rm.GetString("FrmJobManager.Msg_JBSuccDelete", cul);
                    return RedirectToAction("Index");
                }
                else
                {
                    var jobDetails = db.JobDetails.Where(m => m.JD_JobID == id);
                    ViewBag.JobDetails = jobDetails;
                    BindDDL();
                    ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.TempDataJobCannotDeletVerifiedBatch);
                    return View("Delete", job);
                }

                //db.Job.Remove(job);
                //db.SaveChanges();

            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }


        #region Close Job

        private List<Job> getJobsForClosing()
        {
            var Jobs = db.Job.Where(m => m.VerifiedBy != null && m.JobStatus == 5).ToList();
            int cnt = Jobs.Count();
            List<Job> jb = new List<Job>();
            foreach (var item in Jobs)
            {
                var lvls = ProductPackageHelper.getAllDeck(item.JID.ToString());
                if ((lvls.Count > 0))
                {
                    jb.Add(item);
                }
            }
            return jb;
        }

        public ActionResult Closing()
        {
            try
            {
                var jobs = getJobsForClosing();
                return View(jobs);
                //return View(db.PackagingAsso.Where(m => m.VerifyProd == false).ToList());
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        public ActionResult CloseJob(decimal? JobId)
        {
            try
            {
                if (JobId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Job job = db.Job.Find(JobId);
                if (job == null)
                {
                    return HttpNotFound();
                }

                var IncompleteDecks = getIncompleteDecks((decimal)JobId);

                if (IncompleteDecks.Count > 0)
                {
                    ViewBag.IncompleteDecks = IncompleteDecks;
                }

                decimal j = Convert.ToDecimal(JobId);
                var jb = job;
                string jID = JobId.ToString();
                REDTR.HELPER.DbHelper ObjHelper = new REDTR.HELPER.DbHelper();
                var FirstDeck = ProductPackageHelper.getTopDeck(jb.PAID);
                DataSet DS = ObjHelper.DBManager.JobBLL.GetReportDataSet(JobBLL.ReportOp.ForJobdeckProcess, jID, FirstDeck, null, null, null, false, -1);

                if (DS.Tables[0].Rows.Count > 0)
                {
                    ViewBag.badCount = Convert.ToInt32(DS.Tables[0].Rows[0][3]) + jb.NoReadCount;
                    ViewBag.goodCount = DS.Tables[0].Rows[0][2].ToString();
                    ViewBag.decommisionedCount = DS.Tables[0].Rows[0][4].ToString();
                    ViewBag.notVerified = DS.Tables[0].Rows[0][5].ToString();

                }
                else
                {
                    ViewBag.badCount = 0;
                    ViewBag.goodCount = 0;
                    ViewBag.decommisionedCount = 0;
                    ViewBag.notVerified = 0;

                }

                var jobDetails = db.JobDetails.Where(m => m.JD_JobID == JobId);
                ViewBag.JobDetails = jobDetails;
                BindDDL();
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobRequestedclosing + " " + job.JobName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobRequestedclosing + " " + job.JobName, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                return View(job);
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        private List<JobDetails> getIncompleteDecks(decimal JID)
        {
            var jobDetails = db.JobDetails.Where(m => m.JD_JobID == JID);
            List<JobDetails> incompleteJobs = new List<JobDetails>();
            string defaultUid = "fffff";
            //defaultUid = AESCryptor.Encrypt(defaultUid, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
            foreach (var item in jobDetails)
            {
                if (dbhelper.IsIncompleteDeck(item.JD_Deckcode, JID, defaultUid))
                {
                    incompleteJobs.Add(item);
                }
            }
            return incompleteJobs;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CloseJob([Bind(Include = "JID,JobName")] Job job)
        {
            try
            {
                //bool isGlobal = Convert.ToBoolean(Utilities.getAppSettings("IsGlobalServer"));


                //if (isGlobal)
                //{
                //    dbhelper.UpdateStatusEndTimeJob(job.JID, DateTime.Now, (sbyte)JobState.Closed);
                //    string msg = TnT.LangResource.GlobalRes.TrailJobclosedsuccessfully;
                //    GlobalServerDataTransferHelper hlpr = new GlobalServerDataTransferHelper();
                //    if (hlpr.TranferToGLobal(job.JID, job.JobName))
                //    {
                //        TempData["Success"] = "'" + job.JobName + "' " + msg + " " + TnT.LangResource.GlobalRes.TempDataJobtransferedglobal;
                //        trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobClosed + " " + job.JobName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobClosed + " " + job.JobName, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                //    }
                //    else
                //    {
                //        TempData["Success"] = "'" + job.JobName + "' " + msg + " " + TnT.LangResource.GlobalRes.TempDataJobtransferedglobalfailed;
                //        trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobClosed + " " + job.JobName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobClosed + " " + job.JobName, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                //    }

                //}
                //else
                //{
                dbhelper.UpdateStatusEndTimeJob(job.JID, DateTime.Now, (sbyte)JobState.Closed);
                bool IsSMS = Convert.ToBoolean(Utilities.getAppSettings("IsSMS"));
                if (IsSMS == true)
                {
                    M_SMSSync sys = new M_SMSSync();
                    sys.JID = Convert.ToInt32(job.JID);
                    sys.IsSync = false;
                    sys.LastUpdatedDate = DateTime.Now;
                    db.M_SMSSync.Add(sys);
                    db.SaveChanges();
                }


                string msg = TnT.LangResource.GlobalRes.TrailJobclosedsuccessfully;
                TempData["Success"] = "'" + job.JobName + "' " + msg;
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobClosed + " " + job.JobName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobClosed + " " + job.JobName, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                //}
                return RedirectToAction("Closing");
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }



        [HttpPost]
        public ActionResult checkIsJobComplete(decimal JID)
        {

            var jb = db.Job.Find(JID);
            var jobDetails = db.JobDetails.Where(m => m.JD_JobID == JID);

            //get top deck
            List<string> lvls = new List<string>();
            foreach (var item in jobDetails)
            {
                lvls.Add(item.JD_Deckcode);
            }
            string topDeck = ProductPackageHelper.getTopDeck(jb.PAID);

            int GoodCnt = dbhelper.DBManager.PackagingDetailsBLL.GetGoodCountOfJob(jb.JID, topDeck);

            bool IsComplete = false;
            string msgBuffer = "";
            if (GoodCnt < jb.Quantity)
            {
                IsComplete = false;
                msgBuffer = TnT.LangResource.GlobalRes.MsgbufferJobIncompletebatch;
            }
            else
            {
                IsComplete = true;
                CloseJob(jb);

            }
            object[] obj = { IsComplete, msgBuffer };
            return Json(obj);
        }

        #endregion

        #region Decommisioned

        public ActionResult Decommisioned(decimal? JobId)
        {
            try
            {
                if (JobId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Job job = db.Job.Find(JobId);
                if (job == null)
                {
                    return HttpNotFound();
                }
                var jobDetails = db.JobDetails.Where(m => m.JD_JobID == JobId);
                ViewBag.JobDetails = jobDetails;

                decimal j = Convert.ToDecimal(JobId);
                var jb = job;
                string jID = JobId.ToString();
                REDTR.HELPER.DbHelper ObjHelper = new REDTR.HELPER.DbHelper();
                var FirstDeck = ProductPackageHelper.getTopDeck(jb.PAID);
                DataSet DS = ObjHelper.DBManager.JobBLL.GetReportDataSet(JobBLL.ReportOp.ForJobdeckProcess, jID, FirstDeck, null, null, null, false, -1);

                var selectedJobType = db.JOBTypes.Where(x => x.TID == job.TID).FirstOrDefault();
                ViewBag.SelectedJobType = selectedJobType.Job_Type;

                if (DS.Tables[0].Rows.Count > 0)
                {
                    ViewBag.badCount = Convert.ToInt32(DS.Tables[0].Rows[0][3]) + jb.NoReadCount;
                    ViewBag.goodCount = DS.Tables[0].Rows[0][2].ToString();
                    ViewBag.decommisionedCount = DS.Tables[0].Rows[0][4].ToString();
                    ViewBag.notVerified = DS.Tables[0].Rows[0][5].ToString();

                }
                else
                {
                    ViewBag.badCount = 0;
                    ViewBag.goodCount = 0;
                    ViewBag.decommisionedCount = 0;
                    ViewBag.notVerified = 0;

                }

                BindDDL();
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobviewdecommissionedBatch + " " + job.JobName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobviewdecommissionedBatch + " " + job.JobName, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                return View(job);
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }


        public ActionResult DecommisionedConfirmed([Bind(Include = "JID,MLNO")] Job jb)
        {
            try
            {
                Job job = db.Job.Find(jb.JID);
                //if (job.VerifiedBy == null)
                //{
                job.JobStatus = 4;
                job.MLNO = jb.MLNO;
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                string msg = TnT.LangResource.GlobalRes.TrailJobDECOMMISSIONEDBATCH + " " + job.JobName;
                trail.AddTrail(User.FirstName + " " + msg, Convert.ToInt32(User.ID), User.FirstName + " " + msg, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                TempData["Success"] = "'" + job.JobName + "' " + " " + TnT.LangResource.GlobalRes.TempDatadecommissionedsuccessfully;
                return RedirectToAction("DecommisionedLst");
                //}
                //else
                //{
                //    TempData["Success"] = "Cannot Decommision !";
                //    return RedirectToAction("Index");
                //}


            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }
        }

        public ActionResult DecommisionedLst()
        {
            try
            {
                var dj = db.Job.Where(j => j.JobStatus == 4).ToList();
                return View(dj);
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        #endregion

        #region JobTransfer

        private List<JobTransferViewModel> getJobsToTranfer()
        {
            var jbs = db.Job.Where(j => j.JobStatus == 5).ToList();
            List<JobTransferViewModel> jvm = new List<JobTransferViewModel>();
            foreach (var item in jbs)
            {
                JobTransferViewModel obj = new JobTransferViewModel();
                obj.JID = Convert.ToInt32(item.JID);
                obj.JobName = item.JobName;
                obj.ProductName = db.PackagingAsso.Where(p => p.PAID == item.PAID).Select(p => p.Name).FirstOrDefault();
                obj.BatchNo = item.BatchNo;
                jvm.Add(obj);
            }
            return jvm;
        }

        [HttpPost]
        public ActionResult CheckLine(string LineId)
        {
            if (!string.IsNullOrEmpty(LineId))
            {
                //var Line = db.LineLocation.Find(LineId);
                LineDetails ld = new LineDetails();
                //var IsLineAvailable = ld.checkLine(LineId);
                var IsLineAvailable = ld.IsLineAvailableForTrnfr(LineId);
                var msg = ld.ErrorMessage;
                object[] obj = { IsLineAvailable, msg };
                //trail.AddTrail(User.FirstName + " Requested to check live line : ", Convert.ToInt32(User.ID));
                return Json(obj);

            }
            return Json("Invalid Operation");
        }

        [HttpGet]
        public ActionResult BatchManager()
        {
            try
            {
                ViewBag.LineCode = db.LineLocation.Where(x => x.IsActive == true);
                var jvm = getJobsToTranfer().OrderBy(x => x.JobName);
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobBatchTransfers, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobBatchTransfers, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                return View(jvm);
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BatchManager1(FormCollection collection)
        {
            bool IsTemperEvidence = Convert.ToBoolean(Utilities.getAppSettings("TemperEvidence"));
            string AllowMultipleBatches = Utilities.getAppSettings("AllowMultipleBatchesOnLine");
            bool transferStatus = false;
            string JobsToTransfer = Request["item.JID"];
            string LineLocation = Request["LineLocation"];
            try
            {
                int percent = 0;
                string JobName = "";
                int JobPacLvl = 0;
                int count = 0;

                // Chceck Line Exist
                var Line = db.LineLocation.Find(LineLocation);
                var con = @"Data Source=" + Line.LineIP + ";" + "Initial Catalog=" + Line.DBName + ";Persist Security Info=True;User ID=" + Line.SQLUsername + ";Password=" + Line.SQLPassword + ";MultipleActiveResultSets=True";

                Obj_perfSync.OpenDBConnection(con);

                if (Obj_perfSync.IsOpen())
                {
                    List<REDTR.DB.BusinessObjects.Job> jb1 = Obj_perfSync.GetJobs(1);
                    if (!Line.AllowMultipleBatches && jb1.Count > 0)
                    {
                        BindLinCods();
                        TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobPreviousbatchexists;
                        return View("BatchManager", getJobsToTranfer());
                    }
                    //if (AllowMultipleBatches == "false")
                    //{
                    //    if (jb1.Count >= 1)
                    //    {
                    //        BindLinCods();
                    //        TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobPreviousbatchexists;
                    //        return View("BatchManager", getJobsToTranfer());
                    //    }
                    //}
                }

                if (IsTemperEvidence == true)
                {
                    List<REDTR.DB.BusinessObjects.ProductApplicatorSetting> lstPro = Obj_perfSync.GetProductAppicatorSetting(1);
                    if (lstPro.Count >= 1)
                    {
                        TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobProductApplicator;
                        return RedirectToAction("BatchManager");
                    }
                }

                if (string.IsNullOrEmpty(JobsToTransfer) || string.IsNullOrEmpty(LineLocation))
                {
                    BindLinCods();
                    TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobNobatch;
                    return View("BatchManager", getJobsToTranfer());
                }

                string[] jobIds = JobsToTransfer.Split(',');

                foreach (var item in jobIds)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        count++;
                        decimal JID = Convert.ToDecimal(item);
                        JobName = db.Job.Where(j => j.JID == JID).Select(j => j.JobName).FirstOrDefault();
                        JobPacLvl = db.Job.Where(j => j.JID == JID).Select(j => j.PackagingLvlId).FirstOrDefault();
                        ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgJobCurrentlyProcessBtach + JobName, percent, 0);

                        REDTR.DB.BusinessObjects.Job jb = new REDTR.DB.BusinessObjects.Job();
                        jb.JID = Convert.ToDecimal(item);
                        jb.LineCode = LineLocation;
                        jb.PackagingLvlId = JobPacLvl;
                        jb.LastUpdatedDate = Convert.ToDateTime(DateTime.Now.ToString());

                        decimal jid = Convert.ToDecimal(item);

                        TransferJobHelper tjh = new TransferJobHelper();
                        //show progress when completing.
                        tjh.JobName = JobName;
                        tjh.OverallProgress = percent;
                        transferStatus = tjh.TransferJob(jid, LineLocation);
                        if (transferStatus == true)
                        {
                            dbhelper.DBManager.JobBLL.UpdateJobDetails(JobBLL.JobOp.UpdateJobByLine, jb);
                        }
                        percent = Convert.ToInt32((count * 100) / jobIds.Count());
                        trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobTranferedbatch + " " + tjh.JobName + TnT.LangResource.GlobalRes.TempDataJobSucessfully, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobTranferedbatch + " " + tjh.JobName + TnT.LangResource.GlobalRes.TempDataJobSucessfully, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                    }
                }


                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgJobBatchProcessComplete + JobName, 100, 100);
                if (transferStatus == true)
                {

                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataJobBatch + " " + JobName + ", " + TnT.LangResource.GlobalRes.TempDataJobsuccessfullyallocated + " " + LineLocation + "";

                }
                else
                {


                    foreach (var item in jobIds)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            REDTR.DB.BusinessObjects.Job jb = new REDTR.DB.BusinessObjects.Job();
                            jb.JID = Convert.ToDecimal(item);
                            jb.JobStatus = 5;
                            dbhelper.DBManager.JobBLL.UpdateJobDetails(JobBLL.JobOp.UpdateJobStatus, jb);
                        }
                    }
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataJobBatchVerificationFailed;
                }
                BindLinCods();
                return View(getJobsToTranfer());
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionLogger.logException(ex);
                BindLinCods();
                string[] jobIds = JobsToTransfer.Split(',');

                foreach (var item in jobIds)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        REDTR.DB.BusinessObjects.Job jb = new REDTR.DB.BusinessObjects.Job();
                        jb.JID = Convert.ToDecimal(item);
                        jb.JobStatus = 5;
                        dbhelper.DBManager.JobBLL.UpdateJobDetails(JobBLL.JobOp.UpdateJobStatus, jb);
                    }
                }
                TempData["Error"] = (ex.Message);
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobfailedtotransferLine + " " + LineLocation, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobfailedtotransferLine + " " + LineLocation, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                //dbhelper.AddUserTrail(User.ID, null, USerTrailWHERE.TnT1, USerTrailWHAT.ALLOCATE_BATCH_TO_LINE + " FAILED.", null, null);
                return View("BatchManager", getJobsToTranfer());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BatchManager(FormCollection collection)
        {
            bool IsTemperEvidence = Convert.ToBoolean(Utilities.getAppSettings("TemperEvidence"));
            //bool AllowMultipleBatchesOnLine = Convert.ToBoolean(Utilities.getAppSettings("AllowMultipleBatchesOnLine"));
            bool IsGlueSetting = Convert.ToBoolean(Utilities.getAppSettings("IsGlueSetting"));
            string LineLocation = Request["LineLocation"];
            string JobsToTransfer = Request["item.JID"];
            List<Models.ExecutionStat> lstES = new List<Models.ExecutionStat>();

            #region VALIDATION
            var selectedLine = db.LineLocation.Find(LineLocation);
            if (selectedLine == null)
            {
                TempData["Error"] = "Selected Line not found. try again later";
                return RedirectToAction("BatchManager");
            }

            DBSync dbServer = new DBSync();
            if (!dbServer.OpenConnection())
            {
                TempData["Error"] = "Unable to connect. Try again later.";
                return RedirectToAction("BatchManager");
            }

            DBSync dbLine = new DBSync(selectedLine);
            if (!dbLine.OpenConnection())
            {
                TempData["Error"] = "Unable to connect Packaging Line " + selectedLine.ID + ". Try again later";
                return RedirectToAction("BatchManager");
            }
           
            bool AllowMultipleBatchesOnLine = selectedLine.AllowMultipleBatches;
            #endregion

            try
            {

                string[] jobIds = JobsToTransfer.Split(',');
                int percent = 0;
                int count = 0;
                foreach (var jid in jobIds)
                {
                    if (!string.IsNullOrEmpty(jid))
                    {
                        count++;
                        Job selectedJob = db.Job.FirstOrDefault(s => s.JID.ToString() == jid.ToString());

                        decimal productID = 0;
                        int? customerID = 0;
                        if (!AllowMultipleBatchesOnLine)
                        {
                            string query = "SELECT * FROM JOB";
                            DataSet dsJob = dbLine.GetDataSet(query);
                            if (dsJob != null && dsJob.Tables.Count > 0 && dsJob.Tables[0].Rows.Count >= 1)
                            {
                                TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobtakelineclearance;
                                return RedirectToAction("BatchManager");
                            }
                        }
                        //else
                        {
                            string query = "SELECT * FROM PackagingAsso WHERE PAID = " + selectedJob.PAID;
                            DataSet dstemp = dbLine.GetDataSet(query);
                            if (dstemp.Tables.Count <= 0 || dstemp.Tables[0].Rows.Count == 0)
                            {
                                productID = selectedJob.PAID;
                            }

                            query = "SELECT * FROM M_Customer WHERE Id = " + selectedJob.CustomerId;
                            dstemp = dbLine.GetDataSet(query);
                            if (dstemp.Tables.Count <= 0 || dstemp.Tables[0].Rows.Count == 0)
                            {
                                customerID = selectedJob.CustomerId;
                            }
                        }

                        ProgressHub.sendMessage("Currently processing batch :" + selectedJob.JobName, percent, 0);

                        Dictionary<string, object> dicparameter = new Dictionary<string, object>();
                        dicparameter.Add("JobID", selectedJob.JID);
                        dicparameter.Add("PAID", productID);
                        dicparameter.Add("IsManageJob", true);
                        dicparameter.Add("IsTemperEvidence", IsTemperEvidence);
                        dicparameter.Add("IsGlueSetting", IsGlueSetting);
                        dicparameter.Add("CustomerID", customerID);
                        DataSet ds = dbServer.GetDataSet("SP_VERIFYJOB", dicparameter);
                        int tblno = ds.Tables.Count - 1;

                        if (ds.Tables[tblno].Rows.Count <= (selectedJob.Quantity + selectedJob.SurPlusQty) - 1)
                        {
                            TempData["Error"] = "Pack data not found. Kindly contact Administrator";
                            return RedirectToAction("Verification");
                        }

                        var isverify = dbLine.SyncVerifyJob(ds, lstES);

                        if (isverify && lstES.Count(s => !s.IsSuccess) == 0)
                        {
                            selectedJob.JobStatus = 1;
                            selectedJob.LineCode = selectedLine.ID;
                            selectedJob.LastUpdatedDate = DateTime.Now;
                            db.Entry(selectedJob).State = EntityState.Modified;
                            db.SaveChanges();

                            TempData["Success"] = selectedJob.JobName + " batch transferred successfully.";

                            ProgressHub.sendMessage("Batch processing completed: " + selectedJob.JobName, 100, 100);
                            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobTranferedbatch + " " + selectedJob.JobName + " successfully", Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobTranferedbatch + " " + selectedJob.JobName + " successfully", TnT.LangResource.GlobalRes.TrailActionBatchActivity);

                        }
                    }
                }

                BindLinCods();
                return View(getJobsToTranfer());
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionLogger.logException(ex);
                BindLinCods();
                string[] jobIds = JobsToTransfer.Split(',');

                foreach (var item in jobIds)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        REDTR.DB.BusinessObjects.Job jb = new REDTR.DB.BusinessObjects.Job();
                        jb.JID = Convert.ToDecimal(item);
                        jb.JobStatus = 5;
                        dbhelper.DBManager.JobBLL.UpdateJobDetails(JobBLL.JobOp.UpdateJobStatus, jb);
                    }
                }
                TempData["Error"] = (ex.Message);
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobfailedtotransferLine + " " + LineLocation, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailJobfailedtotransferLine + " " + LineLocation, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                //dbhelper.AddUserTrail(User.ID, null, USerTrailWHERE.TnT1, USerTrailWHAT.ALLOCATE_BATCH_TO_LINE + " FAILED.", null, null);
                return View("BatchManager", getJobsToTranfer());
            }
        }
        private void BindLinCods()
        {
            ViewBag.LineCode = db.LineLocation.Where(x => x.IsActive == true);
        }


        [HttpPost]
        public ActionResult IsJobNameExisting(string JobName)
        {
            try
            {
                // var data = db.Job.Where(x => x.JobName == JobName && x.JobStatus != 4).Count(); //Decommissioned Batch should not be created again with same Batch Name.
                var data = db.Job.Where(x => x.JobName == JobName).Count();
                if (data > 0)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Utils
        [HttpPost]
        public ActionResult IsTracelinkUIDsAvailable(string dataString, decimal PAID, int ProviderId, int Customerid, string Jobtype)
        {

            string msg = string.Empty;
            msg = IsUIDAvaialble(dataString, PAID, ProviderId, Customerid, Jobtype);
            return Json(msg);
        }


        public string IsUIDAvaialble(string dataString, decimal PAID, int ProviderId, int Customerid, string Jobtype)
        {
            var provider = db.M_Providers.Where(x => x.Id == ProviderId).FirstOrDefault();
            UIDCustomTypeHelper ctHelpr = new UIDCustomTypeHelper();
            UIDCustomType ctype = UIDCustomTypeHelper.convertToUIDCustomType(provider.Code);
            return CheckUIDStock(dataString, PAID, Customerid, Jobtype);



            //var provider = db.M_Providers.Where(x => x.Id == ProviderId).FirstOrDefault();
            //var jobType = db.JOBTypes.Where(x => x.TID == TID).FirstOrDefault();

            //UIDCustomTypeHelper ctHelpr = new UIDCustomTypeHelper();
            //UIDCustomType ctype = UIDCustomTypeHelper.convertToUIDCustomType(provider.Code);
            //string msg = string.Empty;
            //switch (ctype)
            //{
            //    case UIDCustomType.TLINK:
            //        return ImportTraceLinkUID(dataString, PAID, Customerid);
            //    case UIDCustomType.TKEY:
            //        return ImportTraceKeyUID(dataString, PAID, Customerid, provider, jobType);
            //    case UIDCustomType.RFXL:
            //        return ImportRFXCELUID(dataString, PAID);
            //    case UIDCustomType.XYZ:
            //        return ImportXYZUID(dataString, PAID);
            //    case UIDCustomType.DAWA:
            //        return ImportDAWAUID(dataString, PAID);

            //}
            //return msg;
        }
        public string CheckUIDStock(string dataString, decimal PAID, int Customerid, string Jobtype)
        {

            try
            {
                string msg = "NETWORK ERROR TRY AGAIN LATER";
                if (!string.IsNullOrEmpty(dataString))
                {
                    var selectedCustomer = db.M_Customer.FirstOrDefault(x => x.Id == Customerid);
                    //var SelectedJobType = db.JOBTypes.FirstOrDefault(x => x.TID == TID); 
                    var selectedProduct = db.PackagingAssoDetails.Where(x => x.PAID == PAID).OrderBy(o => o.Id);
                    var firstDek = selectedProduct.ToList().First();
                    var lastDek = selectedProduct.ToList().Last();
                    TraceLinkUtils util = new TraceLinkUtils();
                    var lstDataString = util.convertStringToGTINList(dataString);
                    //var JobType = db.JOBTypes.FirstOrDefault(x=>x.TID==Jobtype);
                    foreach (var deck in selectedProduct)
                    {
                        string SelectedDeck_uidType = string.IsNullOrEmpty(deck.GTIN) ? deck.NTIN : deck.GTIN;
                        string FirstDeck_uidType = firstDek.GTIN;
                        string LastDeck_uidType = lastDek.GTIN;


                        #region commented
                        //if (SelectedDeck_uidType == FirstDeck_uidType)
                        //{
                        //    string qery = " SELECT GTIN, COUNT(SerialNo) CNT FROM X_TracelinkUIDStore WHERE CryptoCode IS NOT NULL AND IsUsed = 0 AND " +
                        //                  " GTIN ='" + SelectedDeck_uidType + "' GROUP BY GTIN";

                        //    if (selectedJobType.Job_Type == "RUSSIA")
                        //    {
                        //        qery = "SELECT GTIN, COUNT(SerialNo) CNT FROM X_TracelinkUIDStore WHERE CryptoCode IS NOT NULL AND IsUsed = 0 AND GTIN ='"
                        //                + SelectedDeck_uidType+ "' GROUP BY GTIN";
                        //    }

                        //    DataSet ds = dbhelper.GetDataSet(qery);

                        //    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                        //    {
                        //        { return "NO SERIAL NUMBER FOUND FOR " + item.GTIN; }
                        //    }

                        //    int availableSerialNo = 0, requiredQty = 0;
                        //    int.TryParse(ds.Tables[0].Rows[0]["CNT"].ToString(), out availableSerialNo);
                        //    requiredQty = lst.FirstOrDefault(g => g.GTIN == item.GTIN).Qty;
                        //    if (requiredQty > availableSerialNo)
                        //    {
                        //        { return (requiredQty - availableSerialNo) + " SERIAL NUMBER REQUIRED FOR " + item.GTIN; }
                        //    }
                        //}

                        #endregion


                        int UnusedUidInStore = 0;
                        var uidReqest = lstDataString.FirstOrDefault(s => s.GTIN == SelectedDeck_uidType);

                        if (SelectedDeck_uidType == FirstDeck_uidType)
                        {
                            UnusedUidInStore = db.X_TracelinkUIDStore.Count(x => (x.GTIN == SelectedDeck_uidType && x.IsUsed == false) &&
                            (
                            (x.Type == "CRPTO" && string.IsNullOrEmpty(x.CryptoCode) == false) || (x.Type != "CRPTO" && string.IsNullOrEmpty(x.CryptoCode) == true))
                            );

                            if (UnusedUidInStore < uidReqest.Qty)
                            {
                                return "PLEASE IMPORT " + (uidReqest.Qty - UnusedUidInStore) + " UIDs FOR GTIN :" + deck.GTIN;
                            }
                        }
                        else if (SelectedDeck_uidType != FirstDeck_uidType &&
                                 SelectedDeck_uidType != LastDeck_uidType &&
                                 Convert.ToBoolean(selectedCustomer.IsProvideCodeForMiddleDeck))
                        {
                            if(Jobtype == "RUSSIA")
                            {
                                UnusedUidInStore = db.X_TracelinkUIDStore.Count(x => (x.GTIN == SelectedDeck_uidType && x.IsUsed == false) &&
                                                            (
                                                              (x.Type == "SSCC" && string.IsNullOrEmpty(x.CryptoCode) == true))
                                                            );
                                if (UnusedUidInStore < uidReqest.Qty)
                                {
                                    return "PLEASE IMPORT " + (uidReqest.Qty - UnusedUidInStore) + " UIDs FOR GTIN :" + deck.GTIN;
                                }
                            }
                            else
                            {
                                UnusedUidInStore = db.X_TracelinkUIDStore.Count(x => (x.GTIN == SelectedDeck_uidType && x.IsUsed == false) &&
                                                            (
                                                              (x.Type == "GTIN" && string.IsNullOrEmpty(x.CryptoCode) == true))
                                                            );
                                if (UnusedUidInStore < uidReqest.Qty)
                                {
                                    return "PLEASE IMPORT " + (uidReqest.Qty - UnusedUidInStore) + " UIDs FOR GTIN :" + deck.GTIN;
                                }
                            }
                            
                        }
                        else if (SelectedDeck_uidType == LastDeck_uidType && selectedCustomer.IsSSCC)
                        {
                            var loosQuantity = Utilities.getAppSettings("LoosShipper");
                            int loosQty = 0;
                            int.TryParse(loosQuantity, out loosQty);
                            uidReqest.Qty += loosQty;
                            
                          
                                UnusedUidInStore = db.X_TracelinkUIDStore.Count(x => (x.GTIN == SelectedDeck_uidType && x.IsUsed == false) &&
                            (
                             (x.Type == "SSCC" && string.IsNullOrEmpty(x.CryptoCode) == true))
                            );

                                if (UnusedUidInStore < uidReqest.Qty)
                                {
                                    return "PLEASE IMPORT " + (uidReqest.Qty - UnusedUidInStore) + " UIDs FOR GTIN :" + deck.GTIN;
                                }
                           
                            
                        }
                    }
                    return "Required number of UID already exists";
                }
                else
                {
                    return "INVALID DATA FOUND. PLEASE REFRESH PAGE";
                }

                return msg;
            }
            catch (Exception)
            {
                return "ERROR OCCURED CONTACT TO ADMINISTRATOR";
            }
        }

        public string ImportTraceKeyUID(string dataString, decimal PAID, int Customerid, TnT.Models.Providers.M_Providers provider, TnT.Models.Product.JOBType jobType)
        {
            string msg = string.Empty;
            List<TLGTINQty> lst;
            TraceLinkUtils util = new TraceLinkUtils();

            if (!string.IsNullOrEmpty(dataString))
            {
                var selectedCustomer = db.M_Customer.FirstOrDefault(x => x.Id == Customerid);

                var packDetials = db.PackagingAssoDetails.Where(p => p.PAID == PAID).OrderBy(p => p.Id).ToList();
                string[] packLevel = { "PPB", "MOC", "OBX", "ISH", "OSH", "PAL" };
                string lastLevelGTIN = string.Empty;
                foreach (var item in packLevel)
                {
                    var lstLvl = packDetials.FirstOrDefault(p => p.PackageTypeCode == item);
                    if (lstLvl != null)
                    {
                        lastLevelGTIN = lstLvl.GTIN;
                    }
                }
                if (packDetials.Count == 1) { lastLevelGTIN = string.Empty; }
                string gtins = string.Empty;

                lst = util.convertStringToGTINList(dataString);

                List<TLGTINQty> lsttemp = new List<TLGTINQty>();
                if (jobType.Job_Type == "RUSSIA" && !Convert.ToBoolean(selectedCustomer.IsProvideCodeForMiddleDeck))
                {
                    gtins = string.Join("','", packDetials[0].GTIN);
                    foreach (var item in lst)
                    {
                        if (gtins.Contains(item.GTIN))
                        {
                            lsttemp.Add(item);
                        }
                    }
                }
                else
                {
                    gtins = string.Join("','", packDetials.Where(s => s.GTIN != lastLevelGTIN).Select(s => s.GTIN));
                    foreach (var item in lst)
                    {
                        if (gtins.Contains(item.GTIN))
                        {
                            lsttemp.Add(item);
                        }
                    }
                }

                lst = lsttemp; //lst.Where(s => gtins.Split(',').Contains(s.GTIN.Trim('\''))).ToList();

                string Query = string.Empty;//"SELECT GTIN, COUNT(SerialNo) CNT FROM X_TracelinkUIDStore WHERE IsUsed =0 AND GTIN IN ('" + gtins + "') GROUP BY GTIN";
                Query = " SELECT TS.GTIN, COUNT(SerialNo) CNT FROM X_TracelinkUIDStore TS JOIN M_TracelinkRequest TR ON TS.TLRequestId = TR.Id " +
                        " WHERE TS.IsUsed = 0 AND TR.ProviderId = " + provider.Id + " AND TS.GTIN IN('" + gtins + "') GROUP BY TS.GTIN";

                DataSet ds = dbhelper.GetDataSet(Query);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (var dek in lst)
                    {
                        var availableQty = ds.Tables[0].AsEnumerable().Where(q => q.Field<string>("GTIN") == dek.GTIN).FirstOrDefault();
                        int foundSrNo = 0;

                        if (availableQty != null)
                        {
                            int.TryParse(availableQty["CNT"].ToString(), out foundSrNo);
                        }
                        if ((foundSrNo >= dek.Qty) == false)
                        {
                            msg += "REQUIRED " + (dek.Qty - foundSrNo) + " SERIAL NUMBERS FOR " + dek.GTIN + ". ";
                        }
                    }
                }
                else
                {
                    msg = "NO SERIAL NUMBERS FOUND. IMPORT SERIAL NUMBERS FIRST.";
                }
            }
            else
            {
                msg = "INVALID DATA STRING.";
            }
            if (string.IsNullOrEmpty(msg))
            {
                msg = TnT.LangResource.GlobalRes.TempDataImporterDataimported;
            }
            return msg;
        }

        public string ImportTraceLinkUID(string dataString, decimal PAID, int Customerid)
        {
            string TertiaryGTIN = "";
            int LSSCCQty = getLSSCCQty(PAID);
            var pkgasso = db.PackagingAsso.Where(x => x.PAID == PAID).FirstOrDefault();

            PackagingAssoDetails GTINLevel = new PackagingAssoDetails();
            var TertiaryDec = ProductPackageHelper.getBottomDeck(PAID);
            var cust = db.M_Customer.Where(x => x.Id == Customerid).FirstOrDefault();
            bool isGlobal = Convert.ToBoolean(Utilities.getAppSettings("IsGlobalServer"));
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(dataString))
            {
                string UDStatus = "";
                List<TLGTINQty> lst;
                List<TLGTINQty> lstGTIN_NA = new List<TLGTINQty>();
                TraceLinkUtils util = new TraceLinkUtils();
                lst = util.convertStringToGTINList(dataString);
                if (lst == null) { return "Invalid."; }
                if (isGlobal)
                {
                    foreach (var item in lst)
                    {
                        if (item.compType == "NTIN")
                        {
                            GTINLevel = db.PackagingAssoDetails.Where(x => x.NTIN == item.GTIN && x.PAID == PAID).FirstOrDefault();
                        }
                        else
                        {
                            GTINLevel = db.PackagingAssoDetails.Where(x => x.GTIN == item.GTIN && x.PAID == PAID).FirstOrDefault();
                        }

                        var res = util.checkTUIDExists(item);
                        if (res)
                        {
                            UDStatus += item.GTIN + " : UID available.";
                        }
                        else
                        {

                            if (GTINLevel.PackageTypeCode != TertiaryDec || TertiaryDec == "MOC")
                            {
                                lstGTIN_NA.Add(item);
                            }
                            else
                            {
                                if (cust.IsSSCC)
                                {
                                    lstGTIN_NA.Add(item);
                                    TertiaryGTIN = item.GTIN;
                                }
                            }
                        }
                    }
                    // }                    
                }
                else
                {
                    foreach (var item in lst)
                    {
                        if (item.compType == "NTIN")
                        {
                            GTINLevel = db.PackagingAssoDetails.Where(x => x.NTIN == item.GTIN && x.PAID == PAID).FirstOrDefault();
                        }
                        else
                        {
                            GTINLevel = db.PackagingAssoDetails.Where(x => x.GTIN == item.GTIN && x.PAID == PAID).FirstOrDefault();
                        }

                        if (GTINLevel.PackageTypeCode != TertiaryDec || TertiaryDec == "MOC")
                        {
                            lstGTIN_NA.Add(item);
                        }
                        else
                        {
                            if (cust.IsSSCC)
                            {
                                lstGTIN_NA.Add(item);
                                TertiaryGTIN = item.GTIN;
                            }
                        }

                    }
                }

                if (lstGTIN_NA != null)
                {
                    if (lstGTIN_NA.Count > 0)
                    {
                        string SrType = string.Empty;
                        foreach (var item in lstGTIN_NA)
                        {


                            var plantcode = db.Settings.FirstOrDefault();
                            if (item.GTIN != TertiaryGTIN)
                            {
                                SrType = item.compType;
                                msg = util.requestExtraUId(item, SrType, plantcode.PlantCode);


                                if (msg != TnT.LangResource.GlobalRes.TempDataImporterDataimported)
                                {
                                    if (msg != TnT.LangResource.GlobalRes.TempDatarequiredNoofUidalredyExist)
                                    {
                                        return msg;
                                    }
                                }
                            }
                            if (item.GTIN == TertiaryGTIN)
                            {

                                item.Qty = LSSCCQty;
                                SrType = "LSSCC";
                                msg = util.requestExtraUId(item, SrType, plantcode.PlantCode);
                                if (msg != TnT.LangResource.GlobalRes.TempDataImporterDataimported)
                                {
                                    if (msg != TnT.LangResource.GlobalRes.TempDatarequiredNoofUidalredyExist)
                                    {
                                        return msg;
                                    }
                                }
                            }

                            if (item.GTIN == TertiaryGTIN)
                            {

                                item.Qty = LSSCCQty;
                                SrType = "SSCC";
                                msg = util.requestExtraUId(item, SrType, plantcode.PlantCode);
                                if (msg != TnT.LangResource.GlobalRes.TempDataImporterDataimported)
                                {
                                    if (msg != TnT.LangResource.GlobalRes.TempDatarequiredNoofUidalredyExist)
                                    {
                                        return msg;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        msg = TnT.LangResource.GlobalRes.TempDatarequiredNoofUidalredyExist;
                    }
                }
                else
                {
                    return TnT.LangResource.GlobalRes.TempDatarequiredNoofUidalredyExist;
                }
            }
            return msg;
        }

        public string ImportRFXCELUID(string dataString, decimal PAID)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(dataString))
            {
                var TertiaryDec = ProductPackageHelper.getBottomDeck(PAID);
                var pkgasso = db.PackagingAsso.Where(x => x.PAID == PAID).FirstOrDefault();
                string UDStatus = "";
                List<TLGTINQty> lst;
                List<TLGTINQty> lstGTIN_NA = new List<TLGTINQty>();
                PackagingAssoDetails GTINLevel = new PackagingAssoDetails();
                TraceLinkUtils util = new TraceLinkUtils();
                lst = util.convertStringToGTINList(dataString);
                if (lst == null) { return "Invalid."; }

                foreach (var item in lst)
                {
                    if (item.compType == "NTIN")
                    {
                        GTINLevel = db.PackagingAssoDetails.Where(x => x.NTIN == item.GTIN && x.PAID == PAID).FirstOrDefault();
                    }
                    else
                    {
                        GTINLevel = db.PackagingAssoDetails.Where(x => x.GTIN == item.GTIN && x.PAID == PAID).FirstOrDefault();
                    }
                    if (GTINLevel.PackageTypeCode != TertiaryDec || TertiaryDec == "MOC")
                    {

                        lstGTIN_NA.Add(item);
                    }

                }

                if (lstGTIN_NA != null)
                {
                    if (lstGTIN_NA.Count > 0)
                    {
                        foreach (var item in lstGTIN_NA)
                        {

                            msg = util.requestRFXCELExtraUID(item, item.compType);
                            if (msg != TnT.LangResource.GlobalRes.TempDataImporterDataimported)
                            {
                                if (msg != TnT.LangResource.GlobalRes.TempDatarequiredNoofUidalredyExist)
                                {
                                    return msg;
                                }
                            }
                        }
                    }
                    else
                    {
                        msg = TnT.LangResource.GlobalRes.TempDatarequiredNoofUidalredyExist;
                    }
                }
                else
                {
                    return "Available.";
                }
            }
            return msg;
        }

        public string ImportXYZUID(string dataString, decimal PAID)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(dataString))
            {
                List<TLGTINQty> lst;
                List<TLGTINQty> lstGTIN_NA = new List<TLGTINQty>();
                TraceLinkUtils util = new TraceLinkUtils();
                lst = util.convertStringToGTINList(dataString);
                string lastlvl = ProductPackageHelper.getBottomDeck(PAID);
                if (lst == null) { return "Invalid."; }
                foreach (var item in lst)
                {
                    var GTINLevel = db.PackagingAssoDetails.Where(x => x.GTIN == item.GTIN && x.PAID == PAID).FirstOrDefault();
                    if (GTINLevel.PackageTypeCode != lastlvl || lastlvl == "MOC")
                    {
                        lstGTIN_NA.Add(item);
                    }

                }
                if (lstGTIN_NA != null)
                {
                    if (lstGTIN_NA.Count > 0)
                    {
                        foreach (var item in lstGTIN_NA)
                        {
                            var xTlUid = db.X_TracelinkUIDStore.Where(x => x.GTIN == item.GTIN && x.IsUsed == false).ToList();
                            if (xTlUid.Count >= item.Qty)
                            {
                                msg = TnT.LangResource.GlobalRes.TempDatarequiredNoofUidalredyExist;
                            }
                            else
                            {
                                msg = TnT.LangResource.GlobalRes.TempDataJobUidNotAvailble;
                            }
                        }
                    }
                }
            }
            return msg;
        }

        public string ImportDAWAUID(string dataString, decimal PAID)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(dataString))
            {
                List<TLGTINQty> lst;
                List<TLGTINQty> lstGTIN_NA = new List<TLGTINQty>();
                TraceLinkUtils util = new TraceLinkUtils();
                lst = util.convertStringToGTINList(dataString);
                string lastlvl = ProductPackageHelper.getBottomDeck(PAID);
                if (lst == null) { return "Invalid."; }
                if (lst.Count() == 1 && lastlvl != "MOC")
                {
                    return TnT.LangResource.GlobalRes.TempDatarequiredNoofUidalredyExist;
                }
                foreach (var item in lst)
                {
                    var GTINLevel = db.PackagingAssoDetails.Where(x => x.GTIN == item.GTIN && x.PAID == PAID).FirstOrDefault();
                    if (GTINLevel.PackageTypeCode != lastlvl || lastlvl == "MOC")
                    {
                        lstGTIN_NA.Add(item);
                    }

                }
                if (lstGTIN_NA != null)
                {
                    if (lstGTIN_NA.Count > 0)
                    {
                        foreach (var item in lstGTIN_NA)
                        {
                            var xTlUid = db.X_TracelinkUIDStore.Where(x => x.GTIN == item.GTIN && x.IsUsed == false).ToList();
                            if (xTlUid.Count >= item.Qty)
                            {
                                msg = TnT.LangResource.GlobalRes.TempDatarequiredNoofUidalredyExist;
                            }
                            else
                            {
                                msg = TnT.LangResource.GlobalRes.TempDataJobUidNotAvailble;
                            }
                        }
                    }
                }
            }
            return msg;
        }


        public int getLSSCCQty(decimal PAID)
        {
            int LSSCCQty = 0;
            var pkgAssoDtls = db.PackagingAssoDetails.Where(x => x.PAID == PAID).OrderBy(x => x.Id).ToList();
            if (pkgAssoDtls.Count == 2)
            {
                LSSCCQty = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
            }
            else if (pkgAssoDtls.Count == 3 || pkgAssoDtls.Count == 4 || pkgAssoDtls.Count == 5)
            {
                LSSCCQty = Convert.ToInt32(Utilities.getAppSettings("LoosShipper")) * 2;
            }



            return LSSCCQty;
        }

        [HttpPost]
        public ActionResult IsChinaUIDsAvailable(string dataString, decimal PAID)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(dataString))
            {
                int loosqty = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
                List<TLGTINQty> lst;
                List<TLGTINQty> lstGTIN_NA = new List<TLGTINQty>();
                TraceLinkUtils util = new TraceLinkUtils();
                lst = util.convertStringToGTINList(dataString);
                if (lst == null) { return Json("Invalid."); }
                bool flag = false;
                int qtyToPrint = 0;
                foreach (var item in lst)
                {
                    var uidsAvailable = db.X_ChinaUIDs.Count(x => x.PackageTypeCode == item.GTIN && x.PAID == PAID && x.IsUsed == false);
                    if (flag)
                    {
                        qtyToPrint = item.Qty + loosqty;
                    }
                    if (uidsAvailable > qtyToPrint)
                    {
                        msg = TnT.LangResource.GlobalRes.TempDataJobUidAvailble;
                    }
                    else
                    {
                        msg = TnT.LangResource.GlobalRes.TempDataJobUidNotAvailble;
                        break;
                    }
                    flag = true;
                }
            }
            return Json(msg);
        }
        #endregion

        [HttpPost]
        public ActionResult getClientData(int Id)
        {
            try
            {
                var ProviderID = db.M_Customer.Where(x => x.Id == Id).FirstOrDefault().ProviderId;
                var Providerdata = db.M_Providers.Where(j => j.Id == ProviderID).FirstOrDefault();

                return Json(Providerdata);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult getKoreaGTINQty(string prod)
        {
            long qty = 0;
            var product = db.PackagingAsso.Where(x => x.FGCode == prod).FirstOrDefault();
            if (product != null)
            {
                var gtin = db.PackagingAssoDetails.Where(x => x.PAID == product.PAID).FirstOrDefault();
                if (gtin != null)
                {
                    var data = db.M_SKMaster.Where(x => x.IsUsed == false).ToList();
                    if (data.Count > 0)
                    {
                        for (int i = 0; i < data.Count; i++)

                        {
                            string[] num = data[i].NumberFrom.Split(')');
                            string[] gt = num[1].Split('(');
                            if (gt[0] == gtin.GTIN)
                            {
                                string[] nto = data[i].NumberTo.Split(')');
                                numfrom = num[2];
                                qty = Convert.ToInt64(nto[2]) - Convert.ToInt64(num[2]);
                                //var UIDexist = db.M_Identities.Where(x => x.GTIN == gtin.GTIN && x.PackageTypeCode == "MOC" && x.IsTransfered == false).OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                                //if (UIDexist != null)
                                //{
                                //    int id = Convert.ToInt32(UIDexist.Id);
                                //    var UIDqty = db.X_Identities.Where(x => x.MasterId == id && x.IsTransfered == false).ToList();
                                //    if (UIDqty.Count() != 0)
                                //    {
                                //        int uqty = UIDqty.Count;
                                //        if (uqty < qty)
                                //        {
                                //            if (uqty != 0)
                                //            {
                                //                qty = qty - uqty;
                                //            }
                                //        }
                                //        else
                                //        {
                                //            qty = uqty - qty;
                                //        }
                                //    }
                                //}
                                return Json(qty + 1);
                            }
                        }

                    }

                }
            }
            return Json(qty);
        }


        public ActionResult CreateAdditionBatchQty()
        {
            var job = db.Job.Where(x => x.JobStatus == 1).ToList();
            ViewBag.job = job;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAdditionBatchQty([Bind(Include = "JID,CurrentBatchQty,RequiredBatchQty")]AdditionBatchQty ad)
        {
            var job = db.Job.Where(x => x.JID == ad.JID).FirstOrDefault();
            var additionbth = db.AdditionBatchQty.Where(x => x.JID == ad.JID).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

            AdditionBatchQty a = new AdditionBatchQty();

            a.JID = ad.JID;
            a.CurrentBatchQty = job.Quantity;
            a.RequiredBatchQty = ad.RequiredBatchQty;
            a.CreatedBy = Convert.ToInt32(User.ID);
            a.CreatedDate = DateTime.Now;
            a.LineCode = job.LineCode;
            if (additionbth != null)
            {
                if (additionbth.VerifiedBy != null)
                {

                    db.AdditionBatchQty.Add(a);

                }
                else
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.toastrJobCreateAdditionBatchQtyforBatchName + job.JobName + "";
                    return RedirectToAction("CreateAdditionBatchQty");
                }
            }
            else
            {
                db.AdditionBatchQty.Add(a);
            }
            db.SaveChanges();
            TempData["Success"] = TnT.LangResource.GlobalRes.TempDataJobAddBatchQtyReq;

            REDTR.DB.BusinessObjects.Job jb = new REDTR.DB.BusinessObjects.Job();
            //jb.JID = job.;
            jb.UseExpDay = job.UseExpDay;
            jb.ExpDateFormat = job.ExpDateFormat;
            jb.JobName = job.JobName;
            jb.PAID = job.PAID;
            jb.BatchNo = job.BatchNo;
            jb.MfgDate = job.MfgDate;
            jb.ExpDate = job.ExpDate;
            jb.CreatedBy = User.ID;//m_UserInfo.ID;
            jb.JobStartTime = job.JobStartTime;
            jb.Quantity = job.Quantity;
            jb.SurPlusQty = job.SurPlusQty;
            jb.JobStatus = Convert.ToSByte(job.JobStatus);
            jb.AutomaticBatchCloser = job.AutomaticBatchCloser;
            jb.TID = job.TID;
            jb.MLNO = job.MLNO;
            jb.TenderText = job.TenderText;
            jb.JobWithUID = job.JobWithUID;
            jb.ForExport = job.ForExport;
            jb.PrimaryPCMapCount = Convert.ToInt32(job.PrimaryPCMapCount);
            jb.AppId = 1;
            jb.DAVAPortalUpload = false;
            jb.NoReadCount = 0;
            jb.LabelStartIndex = job.LabelStartIndex;
            jb.PackagingLvlId = job.PackagingLvlId;
            jb.CustomerId = job.CustomerId;
            jb.ProviderId = job.ProviderId;
            jb.PPNCountryCode = job.PPNCountryCode;
            jb.PPNPostalCode = job.PPNPostalCode;
            jb.CompType = job.CompType;
            var userData = User;
            batchNotificationHlpr.notifyPerimissionHolders(ref jb, BatchEventType.batchQtyRequested, userData);
            var jb1 = db.Job.Where(x => x.JobStatus == 1).ToList();

            ViewBag.job = jb1;
            return RedirectToAction("CreateAdditionBatchQty");
        }


        public ActionResult VerifyAdditionBatchQty()
        {
            return View(db.AdditionBatchQty.Where(x => x.jb.JobStatus == 1 && (x.VerifiedBy == null) && (x.VerifiedDate == null)).Distinct().ToList());
        }


        //[HttpPost]
        //public ActionResult ExtraUidRequest(int jid)
        //{
        //    bool uidgenerated = false;
        //    var job = db.Job.FirstOrDefault(x => x.JID == jid);
        //    if (job == null)
        //    {
        //        return Json("Batch Does not exist");
        //    }
        //    var ad = db.AdditionBatchQty.Where(x => x.JID == jid).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        //    var LineServer = db.LineLocation.Where(x => x.ID == ad.LineCode).FirstOrDefault();

        //    if (LineServer != null)
        //    {
        //        ConnectionStr = @"Data Source=" + LineServer.LineIP + ";" + "Initial Catalog=" + LineServer.DBName + ";Persist Security Info=True;User ID=" + LineServer.SQLUsername + ";Password=" + LineServer.SQLPassword + ";MultipleActiveResultSets=True";
        //    }
        //    else
        //    {
        //        TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobLinedetailsnotprovided;

        //        return Json(TnT.LangResource.GlobalRes.TempDataJobLinedetailsnotprovided);
        //    }
        //    try
        //    {
        //        Obj_perfSync.OpenDBConnection(ConnectionStr);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json("Line Could not be connected");
        //    }

        //    if (Obj_perfSync.IsOpen())
        //    {
        //        List<REDTR.DB.BusinessObjects.Job> jb1 = Obj_perfSync.GetJobs(1);
        //        var line_job = jb1.Find(x => x.JobName == job.JobName);
        //        //Transfer UID and SSCCs
        //        TransferJobData tjd = new TransferJobData();
        //        if (line_job == null)
        //        {
        //            TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobBatchNotExit;
        //            trail.AddTrail(TnT.LangResource.GlobalRes.TempDataJobBatchNotExit, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataJobBatchNotExit, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
        //            return Json(job.JobName + TnT.LangResource.GlobalRes.TempDataJobBatchNotExit);

        //        }
        //    }
        //    int jbqty = job.Quantity + ad.RequiredBatchQty;
        //    job.Quantity = ad.RequiredBatchQty;


        //    string firstlvl = ProductPackageHelper.getTopDeck(job.PAID);
        //    string lastlvl = ProductPackageHelper.getTertiarryDeck(job.PAID, job.JID);
        //    var IAC_CIN = db.Settings.FirstOrDefault();

        //    var JobTYpe = db.JOBTypes.Where(x => x.TID == job.TID).FirstOrDefault();
        //    var selectedJobType = JobTYpe.Job_Type;
        //    if (job.Proivder.Id == 4)
        //    {
        //        if (GenerateIdsAdditionbatchqty(job, firstlvl, lastlvl, selectedJobType, IAC_CIN.IAC_CIN))
        //        {
        //            uidgenerated = true;
        //        }
        //        else
        //        {
        //            uidgenerated = false;
        //        }
        //    }
        //    else
        //    {
        //        var TertiaryDec = ProductPackageHelper.getBottomDeck((int)job.PAID);
        //        TracelinkUIDDataHelper tldHlpr = new TracelinkUIDDataHelper();
        //        tldHlpr.CalculateUIDsToGenerate(job.Quantity, (int)job.PAID);
        //        var lvl = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).ToList();
        //        string dataString = string.Empty;
        //        foreach (var item in lvl)
        //        {
        //            if (item.PackageTypeCode != TertiaryDec)
        //            {
        //                int qtyToGet = tldHlpr.getQtyToGenerate(item.PackageTypeCode);
        //                dataString += item.GTIN + ":" + qtyToGet + ":" + job.ProviderId + ", ";
        //            }
        //            else
        //            {
        //                if (TertiaryDec == "MOC")
        //                {
        //                    int qtyToGet = tldHlpr.getQtyToGenerate(item.PackageTypeCode);
        //                    dataString += item.GTIN + ":" + qtyToGet + ":" + job.ProviderId + ", ";
        //                }
        //            }

        //        }
        //        dataString = dataString.Remove(dataString.Length - 1);

        //        if (dataString != "")
        //        {
        //            var msg = IsTracelinkUIDsAvailable(dataString, job.PAID, Convert.ToInt32(job.ProviderId), Convert.ToInt32(job.CustomerId), (int)JobTYpe.TID);

        //            var provider = db.M_Providers.FirstOrDefault(x => x.Id == job.ProviderId);
        //            if (provider.Code == UIDCustomType.TKEY.ToString())
        //            {
        //                string response = ((System.Web.Mvc.JsonResult)msg).Data.ToString();
        //                string success_response = TnT.LangResource.GlobalRes.TempDataImporterDataimported;

        //                if (!string.IsNullOrEmpty(response) && !response.Contains(success_response))
        //                {
        //                    TempData["Error"] = response;
        //                    return Json(response);
        //                }
        //            }
        //        }
        //        if (GenerateDataforTraceKeyAdditionalBatchQty(job, firstlvl, lastlvl, selectedJobType, IAC_CIN.IAC_CIN))
        //        {
        //            uidgenerated = true;


        //        }
        //        else
        //        {
        //            uidgenerated = false;
        //        }
        //    }

        //    if (uidgenerated)
        //    {
        //        string ConnectionServer = Utilities.getConnectionString("DefaultConnection");

        //        if (LineServer != null)
        //        {
        //            ConnectionStr = @"Data Source=" + LineServer.LineIP + ";" + "Initial Catalog=" + LineServer.DBName + ";Persist Security Info=True;User ID=" + LineServer.SQLUsername + ";Password=" + LineServer.SQLPassword + ";MultipleActiveResultSets=True";
        //        }
        //        else
        //        {
        //            //TempData["Error"] = TnT.LangResource.GlobalRes.TempDataJobLinedetailsnotprovided;
        //            ViewBag.job = db.Job.Where(x => x.JobStatus == 1).ToList(); ;
        //            return Json(TnT.LangResource.GlobalRes.TempDataJobLinedetailsnotprovided);
        //        }

        //        Obj_perfSync.OpenDBConnection(ConnectionStr);

        //        if (Obj_perfSync.IsOpen())
        //        {
        //            List<REDTR.DB.BusinessObjects.Job> jb1 = Obj_perfSync.GetJobs(1);
        //            var line_job = jb1.Find(x => x.JobName == job.JobName);
        //            //Transfer UID and SSCCs
        //            TransferJobData tjd = new TransferJobData();
        //            if (jb1.Count >= 1)
        //            {
        //                // MessageBoxEx.Show("LINE CLEARANCE NOT COMPLETED. PLEASE MAKE LINE CLEARANCE", APPNAME, MessageBoxEx.MessageBoxButtonsEx.OK, 0);

        //                if (tjd.TransferAdditionalBatchQty(job, ConnectionStr, line_job.PAID, (decimal)line_job.JID, ConnectionServer, true))
        //                {
        //                    int qty = db.Job.Where(x => x.JID == jid).Select(x => x.Quantity).FirstOrDefault();
        //                    ConnectionStr = string.Empty;
        //                    //ad.VerifiedBy = User.ID;
        //                    //ad.VerifiedDate = DateTime.Now;

        //                    //ad.CurrentBatchQty =qty;
        //                    //db.Entry(ad).State = EntityState.Modified;
        //                    //db.SaveChanges();

        //                    //string qry = "update AdditionBatchQty set VerifiedBy=" +  User.ID + ", VerifiedDate=CONVERT(datetime,'" + DateTime.Now + "',105),CurrentBatchQty=" + qty + " where ID=" + ad.ID;
        //                    string qry = "update AdditionBatchQty set VerifiedBy=" + Convert.ToInt32(User.ID) + ", VerifiedDate=CONVERT(datetime,'" + DateTime.Now + "',105),CurrentBatchQty=" + qty + " where ID=" + ad.ID;

        //                    dbhelper.ExecureQuery(qry);

        //                    job.Quantity = jbqty;
        //                    db.Entry(job).State = EntityState.Modified;
        //                    db.SaveChanges();

        //                    string query = "update Job set Quantity=" + jbqty + " where JID=" + line_job.JID;
        //                    int i = Obj_perfSync.ExecuteQuery(query);

        //                    trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TempDataJobReqAddBatchQty + LineServer.ID, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TempDataJobReqAddBatchQty + LineServer.ID, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
        //                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataJobAddBatchQty + LineServer.ID;
        //                    //  return Json("Additional Batch Quantity Transfered To Line " + LineServer.ID);
        //                    //TempData["Success"] = "UIDs are  generated  Successfully ";
        //                }
        //            }
        //            else
        //            {
        //                //TempData["Error"] = "Batch does not exist on line";
        //                trail.AddTrail(TnT.LangResource.GlobalRes.TempDataJobBatchNotExit, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataJobBatchNotExit, TnT.LangResource.GlobalRes.TrailActionBatchActivity);

        //                return Json(TnT.LangResource.GlobalRes.TempDataJobBatchNotExit);

        //            }

        //        }
        //    }
        //    else
        //    {
        //        trail.AddTrail(TnT.LangResource.GlobalRes.TempDataJobUidNotGeneratedBatch + job.JobName, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataJobUidNotGeneratedBatch + job.JobName, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
        //        return Json(TnT.LangResource.GlobalRes.TempDataJobUidNotGeneratedBatch + job.JobName);
        //    }
        //    return Json("Success");
        //}
        [HttpPost]
        public ActionResult ExtraUidRequest(int jid)
        {
            bool uidgenerated = false;
            var job = db.Job.FirstOrDefault(x => x.JID == jid);
            if (job == null)
            {
                return Json("Batch Does not exist");
            }

            var additionBatchRequest = db.AdditionBatchQty.FirstOrDefault(x => x.JID == jid && string.IsNullOrEmpty(x.VerifiedBy.ToString()) && string.IsNullOrEmpty(x.VerifiedDate.ToString()));
            var selectedLine = db.LineLocation.FirstOrDefault(x => x.ID == job.LineCode);

            if (selectedLine == null)
            {
                return Json(TnT.LangResource.GlobalRes.TempDataJobLinedetailsnotprovided);
            }

            string LineConnectionStr = @"Data Source=" + selectedLine.LineIP + ";" + "Initial Catalog=" + selectedLine.DBName + ";Persist Security Info=True;User ID=" + selectedLine.SQLUsername + ";Password=" + selectedLine.SQLPassword + ";MultipleActiveResultSets=True";
            PerformSyncBLL lineConnction = new PerformSyncBLL();

            try
            {

                lineConnction.OpenDBConnection(LineConnectionStr);
                if (!lineConnction.IsOpen())
                {
                    return Json("UNABLE TO OPEN CONNECTION FOR " + selectedLine.ID);
                }
            }
            catch (Exception ex)
            {
                return Json("ERROR WHILE CONNECTING LINE " + selectedLine.ID);
            }
            REDTR.DB.BusinessObjects.Job jobFromLine = null;
            if (lineConnction.IsOpen())
            {
                List<REDTR.DB.BusinessObjects.Job> lstLineJobs = lineConnction.GetJobs(1);
                jobFromLine = lstLineJobs.FirstOrDefault(x => x.JobName == job.JobName);
                if (jobFromLine == null)
                {
                    TempData["Error"] = job.JobName + "Batch Does Not Exist On Line";
                    trail.AddTrail(job.JobName + " Batch does not exist on line ", Convert.ToInt32(User.ID), "Batch does not exist on line", TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                    return Json(job.JobName + " Batch does not exist on line");
                }
            }
            int newJobQuantity = job.Quantity + additionBatchRequest.RequiredBatchQty;
            job.Quantity = additionBatchRequest.RequiredBatchQty;

            //string firstlvl = ProductPackageHelper.getTopDeck(job.PAID);
            //string lastlvl = ProductPackageHelper.getTertiarryDeck(job.PAID, job.JID);

            var productOfJob = db.JobDetails.Where(s => s.JD_JobID == job.JID).OrderBy(s => s.Id).ToList();
            var settings = db.Settings.FirstOrDefault();
            var typeOfJob = db.JOBTypes.FirstOrDefault(x => x.TID == job.TID);
            var customer = db.M_Customer.FirstOrDefault(x => x.Id == job.CustomerId);

            string firstlvl = productOfJob.First().JD_Deckcode;
            string lastlvl = productOfJob.Last().JD_Deckcode;
            IDDataGeneratorUtility util = new IDDataGeneratorUtility();
            if (job.Proivder.Code == "PROP") // PROPIX SERIAL NUMBER
            {
                uidgenerated = util.GenerateCodePropix(job, firstlvl, lastlvl, typeOfJob.Job_Type, settings.IAC_CIN, true);
            }
            else
            {
                TracelinkUIDDataHelper tldHlpr = new TracelinkUIDDataHelper();
                tldHlpr.CalculateUIDsToGenerate(job.Quantity, (int)job.PAID);
                string dataString = string.Empty;
                foreach (var item in productOfJob)
                {
                    //if (typeOfJob.Job_Type == "RUSSIA")
                    //{
                    //    int qtyToGet = tldHlpr.getQtyToGenerate(item.JD_Deckcode);
                    //    dataString += item.JD_GTIN + ":" + qtyToGet + ":" + job.ProviderId + ":" + job.CompType + ",";
                    //}
                    //else
                    //{ //CHECK ONLY FOR MOC
                    //    if (item.JD_Deckcode != lastlvl || lastlvl == "MOC")
                    //    {
                    //        int qtyToGet = tldHlpr.getQtyToGenerate(item.JD_Deckcode);

                    //        dataString += item.JD_GTIN + ":" + qtyToGet + ":" + job.ProviderId + ":" + job.CompType + ",";
                    //    }
                    //}

                    int qtyToGet = tldHlpr.getQtyToGenerate(item.JD_Deckcode);
                    if (item.JD_Deckcode == firstlvl)
                    {
                        dataString += item.JD_GTIN + ":" + qtyToGet + ":" + job.ProviderId + ":" + job.CompType + ",";
                    }
                    else if (item.JD_Deckcode != firstlvl && item.JD_Deckcode != lastlvl && Convert.ToBoolean(customer.IsProvideCodeForMiddleDeck))
                    {
                        dataString += item.JD_GTIN + ":" + qtyToGet + ":" + job.ProviderId + ":" + job.CompType + ",";
                    }
                    else if (item.JD_Deckcode == lastlvl && Convert.ToBoolean(customer.IsSSCC))
                    {
                        dataString += item.JD_GTIN + ":" + qtyToGet + ":" + job.ProviderId + ":" + job.CompType + ",";
                    }

                }
                //dataString = dataString.Remove(dataString.Length - 1);
                dataString = dataString.Trim(',');

                if (!string.IsNullOrEmpty(dataString))
                {
                    //string importStatus = IsTracelinkUIDsAvailable(dataString, job.PAID, Convert.ToInt32(job.ProviderId), Convert.ToInt32(job.CustomerId), typeOfJob.Job_Type);
                    string importStatus = IsUIDAvaialble(dataString, job.PAID, Convert.ToInt32(job.ProviderId), Convert.ToInt32(job.CustomerId), typeOfJob.Job_Type);

                    if (!importStatus.Contains("already exists") && !importStatus.Contains("Imported Successfully"))
                    {
                        TempData["Success"] = importStatus;
                        return Json(importStatus);
                    }
                }
                //uidgenerated = GenerateDataforTracelinkAdditionalBatchQty(job, firstlvl, lastlvl, typeOfJob.Job_Type, settings.IAC_CIN);

                uidgenerated = util.GenerateCodeNonPropix(job, firstlvl, lastlvl, typeOfJob.Job_Type, settings.IAC_CIN, true);
            }

            if (uidgenerated)
            {
                string serverConnectionString = Utilities.getConnectionString("DefaultConnection");
                try
                {
                    lineConnction.OpenDBConnection(LineConnectionStr);
                    if (lineConnction.IsOpen())
                    {
                        TransferJobData tjd = new TransferJobData();

                        //if (tjd.TransferAdditionalBatchQty(job, LineConnectionStr, jobFromLine.PAID, (decimal)jobFromLine.JID, serverConnectionString, true))
                        if (tjd.TransferV1(job, LineConnectionStr, jobFromLine.PAID, (decimal)jobFromLine.JID, serverConnectionString, true))
                        {
                            int qty = db.Job.Where(x => x.JID == jid).Select(x => x.Quantity).FirstOrDefault();
                            ConnectionStr = string.Empty;
                            string qry = "update AdditionBatchQty set VerifiedBy=" + User.ID + ", VerifiedDate=GETDATE(),CurrentBatchQty=" + qty + " where ID=" + additionBatchRequest.ID;
                            dbhelper.ExecureQuery(qry);

                            job.Quantity = newJobQuantity;
                            db.Entry(job).State = EntityState.Modified;
                            db.SaveChanges();

                            string query = "update Job set Quantity=" + newJobQuantity + " where JID=" + jobFromLine.JID;
                            int i = lineConnction.ExecuteQuery(query);

                            trail.AddTrail(User.FirstName + " Request Additional Batch Quantity for line " + selectedLine.ID, Convert.ToInt32(User.ID), User.FirstName + " Request Additional Batch Quantity for line " + selectedLine.ID, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                            TempData["Success"] = "Additional Batch Quantity Request Transfered Successfully To Line " + selectedLine.ID;
                            return Json("Additional Batch Quantity Transfered To Line " + selectedLine.ID);
                        }
                        else
                        {
                            TempData["Success"] = "Connection loss. Serial Number Generated. Data not transfer to line";
                            return Json("Connection loss. Serial Number Generated. Data not transfer to line ");
                        }
                    }
                    else
                    {
                        return Json("UNABLE TO OPEN CONNECTION FOR " + selectedLine.ID);
                    }
                }
                catch (Exception ex)
                {
                    return Json("ERROR WHILE CONNECTING LINE " + selectedLine.ID);
                }
            }
            else
            {
                trail.AddTrail("Uid not generated for batch " + job.JobName, Convert.ToInt32(User.ID), "Uid not generated for batch " + job.JobName, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
                return Json("Uid not generated for batch " + job.JobName);
            }
            return Json("Sucess");
        }
        [HttpPost]
        public ActionResult GetbatchQty(int jid)
        {
            var jb = db.Job.Where(x => x.JID == jid).Select(x => x.Quantity).FirstOrDefault();
            var additionalbatchqty = db.AdditionBatchQty.Where(x => x.JID == jid).OrderBy(x => x.CreatedDate).FirstOrDefault();
            if (additionalbatchqty != null)
            {
                var originalQty = additionalbatchqty.CurrentBatchQty;
                if (additionalbatchqty.VerifiedBy != null)
                {
                    if (originalQty == 0)
                    {
                        originalQty = jb;
                    }
                    object[] data = { jb, originalQty };
                    return Json(data);
                }
                else
                {
                    return Json("False");
                }
            }
            else
            {

                object[] data = { jb, jb };
                return Json(data);
            }



        }

        [HttpPost]
        public ActionResult GetLabelDesignerFiles(int PAID)
        {
            try
            {
                LabelDesignHelper lblHelper = new LabelDesignHelper();
                var data = lblHelper.getLabelDesignerFiles();
                var datapackageLableMaster = db.PackageLabelMaster.Where(m => m.PAID == PAID).ToList();
                object[] response = { data, datapackageLableMaster };
                return Json(response);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetLabelDesignerFilesOfJob(int jid)
        {
            try
            {
                LabelDesignHelper lblHelper = new LabelDesignHelper();
                var data = lblHelper.getLabelDesignerFiles();
                var job = db.Job.Where(x => x.JID == jid).FirstOrDefault();
                var datapackageLableMaster = db.JobDetails.Where(x => x.JD_JobID == job.JID).ToList();

                object[] response = { data, datapackageLableMaster };
                return Json(response);

            }
            catch (Exception)
            {
                throw;
            }
        }
        #region RejectedCode
        private bool GenerateIds(Job job, string firstLvl, string lastLvl, string selectedJobType, string IAC_CIN)
        {
            IDDataGeneratorUtility util = new IDDataGeneratorUtility();
            return util.GenerateData(job, firstLvl, lastLvl, selectedJobType, IAC_CIN);
        }
        #endregion
    }
}
