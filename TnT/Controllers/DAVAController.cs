using REDTR.DB.BLL;
using REDTR.DB.BusinessObjects;
using REDTR.HELPER;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer;
using TnT.DataLayer.DAVAService;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.DAVAPortal;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class DAVAController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        string msg = "";
        // GET: DAVA
        public ActionResult Index()
        {

            BindeFields();
            return View();
        }


        public void BindeFields()
        {
            DbHelper m_dbhelper = new DbHelper();
            //List<PackagingDetails> LstBatchData = new List<PackagingDetails>();
            //string TertiaryDeck = "";
            DService util = new DService();
            //var batch= util.getDAVABatches().ToList();

            //for (int i=0;i<batch.Count();i++)
            //{

            //    decimal jid = Convert.ToDecimal(batch[i].Value);
            //    if (jid != 0)
            //    {
            //        var lstJob = db.Job.Where(x => x.JID == jid && x.DavaPortalUpload == false).FirstOrDefault();
            //        if (lstJob == null)
            //        {
            //            var LstJobDetails = db.JobDetails.Where(x => x.JD_JobID == jid).ToList();
            //            var sortedLvls = ProductPackageHelper.sorttheLevels(LstJobDetails.Select(x => x.JD_Deckcode).ToList());
            //            TertiaryDeck = sortedLvls[sortedLvls.Count - 1];
            //            var pkg = db.PackagingDetails.Where(x => (x.DavaPortalUpload == false || x.DavaPortalUpload == null) && x.JobID == jid && (x.IsDecomission == false || x.IsDecomission == null) && x.IsRejected != null).ToList();
            //            var lstdata = pkg.FindAll(item => item.PackageTypeCode == TertiaryDeck).ToList();
            //            if (lstdata.Count > 0)
            //            {

            //                lst.Add(new SelectListItem { Text = batch[i].Text, Value = batch[i].Value });

            //            }
            //        }
            //        else
            //        {
            //            lst.Add(new SelectListItem { Text = lstJob.BatchNo, Value = lstJob.JID.ToString() });
            //        }
            //    }
            //}
            //var b = lst;
            //      ViewBag.Batches = util.getDAVABatches();
            List<SelectListItem> lst = new List<SelectListItem>();
            string Query = "Select BatchNo,JID from Vw_GetBatchForDAVA"
+ " where JID in (Select distinct JobID from PackagingDetails where PackagingDetails.SSCC is not null and(PackagingDetails.DavaPortalUpload = 0 or PackagingDetails.DavaPortalUpload is null) and(PackagingDetails.IsDecomission = 0 or PackagingDetails.IsDecomission is null) and PackagingDetails.IsRejected is not null and NextLevelCode is null) ";
            //+"union select batchno,jid from Job where JobStatus = 3 and DavaPortalUpload = 0 or DavaPortalUpload is null";
            DataSet ds = m_dbhelper.GetDataSet(Query);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                lst.Add(new SelectListItem { Text = ds.Tables[0].Rows[i][0].ToString(), Value = ds.Tables[0].Rows[i][1].ToString() });
            }
            //var lst = ds.Tables[0].AsEnumerable().Select(dataRow => new BatchNoForDavaViewModel {  BatchNo= dataRow.Field<string>("BatchNo"), JID = dataRow.Field<int>("JID")}).ToList();
            ViewBag.Batches = lst;
            ViewBag.Countries = util.getCountries();
            ViewBag.ExempCodes = util.getExemptionCodes();
        }


        [HttpPost]
        public ActionResult getDAVASSCCs(string JobId, string BatchNo)
        {
            if (!string.IsNullOrEmpty(JobId) && !string.IsNullOrEmpty(BatchNo))
            {
                var jid = Convert.ToInt32(JobId);
                DService util = new DService();
                var data = util.getSSCCGrid(null, jid, BatchNo);
                // data.ActualBatchQty = db.Job.Where(d => d.JID == jid).FirstOrDefault().Quantity;
                data.ActualBatchQty = (from j in db.PackagingDetails where j.SSCC != null && j.JobID == jid && j.IsRejected == false && j.IsUsed == true && (j.IsDecomission == false || j.IsDecomission != null) select new { j.SSCC }).Count();
                data.ExportBatchQty = db.PackagingDetails.Where(ep => ep.IsRejected == false && ep.JobID == jid && ep.DavaPortalUpload == true && ep.IsUsed == true).Count();
                data.RemainingBatchQty = Convert.ToInt32(data.ActualBatchQty) - Convert.ToInt32(data.ExportBatchQty);
                return Json(data);
            }
            else
            {
                return null;
            }
        }
        [HttpPost]
        public ActionResult getJobType(string JobId)
        {
            if (!string.IsNullOrEmpty(JobId))
            {
                var jobIntID = Convert.ToInt32(JobId);
                var jobTID = db.Job.Where(k => k.JID == jobIntID).FirstOrDefault().TID;
                var data = db.JOBTypes.Where(x => x.TID == jobTID).FirstOrDefault().Job_Type;
                return Json(data);
            }
            else
            {
                return null;
            }
        }
        private string DAVAStausMessages(DAVAStatus sts)
        {
            string msg = "";

            if (sts == DAVAStatus.Success)
            {
                msg = TnT.LangResource.GlobalRes.DAVAStatusSuccess;
            }
            if (sts == DAVAStatus.NoProductsAvailable)
            {
                msg = TnT.LangResource.GlobalRes.DAVAStatusNoProductsAvailable;
            }
            if (sts == DAVAStatus.AlreadyBatchExported)
            {
                msg = TnT.LangResource.GlobalRes.DAVAStatusAlreadyBatchExported;
            }
            if (sts == DAVAStatus.Failure)
            {
                msg = TnT.LangResource.GlobalRes.DAVAStatusFailure;
            }
            if (sts == DAVAStatus.NoBatchesAvailable)
            {
                msg = TnT.LangResource.GlobalRes.DAVAStatusNoBatchesAvailable;
            }
            if (sts == DAVAStatus.NoProductionDataAvailable)
            {
                msg = TnT.LangResource.GlobalRes.DAVAStatusNoProductionDataAvailable;
            }
            if (sts == DAVAStatus.NoSSCCsAvailable)
            {
                msg = TnT.LangResource.GlobalRes.DAVAStatusNoSSCCsAvailable;
            }

            return msg;
        }

        //public ActionResult Create([Bind(Include = "IsProductSelected,IsBatchSelected,IsAllBatchSelected,IsSingleBatchSelected,BatchNo,BatchType,IsExemptedBarcode,ExemptedCountryCode,IsProductionSelected,IsTertiaryPackExempted")]GenerateDAVAViewModel vm)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IsProductSelected,IsBatchSelected,IsAllBatchSelected,IsSingleBatchSelected,BtcNo,BatchNo,BatchType,IsExemptedBarcode,ExempCode,ExemptedCountryCode,IsProductionSelected,IsTertiaryPackExempted,EXEMPTION_NOTIFICATION_AND_DATE")]GenerateDAVAViewModel vm)
        {
            DbHelper m_dbhelper = new DbHelper();
            vm.IsWholeBatch = Convert.ToBoolean(Request["IsWholeBatch"]);
            bool IsProduction= Convert.ToBoolean(Utilities.getAppSettings("DavaProduction"));
            DAVAGenerator gen = new DAVAGenerator();
            //try
            //{
            if (vm.IsProductSelected)
            {
                var status = gen.generateDAVAProduct(); // CreateProductDAVA();

                msg = DAVAStausMessages(status);
                if (Convert.ToString(status) == "Success")
                {
                    msg = TnT.LangResource.GlobalRes.DAVAIndexSelectOptionProduct +" " + msg;
                }

                TempData["Success"] = msg;
                trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);

            }
            else
            {
                if (vm.IsAllBatchSelected)
                {
                  
                        var sts = gen.generateDAVABatch(true, null, vm.IsExemptedBarcode, null, vm.ExemptedCountryCode,vm.EXEMPTION_NOTIFICATION_AND_DATE.ToString("dd/MM/yyyy"));

                        msg = DAVAStausMessages(sts);

                        if (Convert.ToString(sts) == "Success")
                        {
                            msg = TnT.LangResource.GlobalRes.DAVAIndexSelectOptionBatch + msg;
                        }
                  
                    TempData["Success"] = msg;

                    trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                }
                else
                {
                    if (!string.IsNullOrEmpty(vm.BtcNo))
                    {
                        var sts = gen.generateDAVABatch(false, vm.BtcNo, vm.IsExemptedBarcode, null, vm.ExemptedCountryCode,vm.EXEMPTION_NOTIFICATION_AND_DATE.ToString("dd/MM/yyyy"));

                        if (sts == DAVAStatus.Success || sts == DAVAStatus.NoBatchesAvailable)
                        {
                            if (vm.IsProductionSelected)
                            {
                                if (vm.IsWholeBatch)
                                {
                                    decimal jid = Convert.ToDecimal(vm.BatchNo);
                                    if (IsProduction)
                                    {
                                        var prodSts = gen.generateDAVAProduction(jid, vm.BtcNo, true, null);
                                        if (Convert.ToString(prodSts) == "Success")
                                        {

                                            msg = DAVAStausMessages(prodSts);
                                            msg = TnT.LangResource.GlobalRes.DAVAIndexSelectOptionBatch + " " + msg;
                                            TempData["Success"] = msg;
                                            trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                                        }
                                        else
                                        {
                                            msg = TnT.LangResource.GlobalRes.TempDataDavaNoData;
                                            TempData["Error"] = msg;
                                            trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                                        }
                                    }
                                    else
                                    {
                                        var tnsSts = gen.generateDAVAProductionTNS(jid, vm.BtcNo, true, null);
                                        if (Convert.ToString(tnsSts) == "Success")
                                        {

                                            msg = DAVAStausMessages(tnsSts);
                                            msg = TnT.LangResource.GlobalRes.DAVAIndexSelectOptionBatch + " " + msg;
                                            TempData["Success"] = msg;
                                            trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                                        }
                                        else
                                        {
                                            msg = TnT.LangResource.GlobalRes.TempDataDavaNoData;
                                            TempData["Error"] = msg;
                                            trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                                        }
                                    }
                                }
                                else
                                {

                                    var SSccstoGen = Request["GENSSCC[]"].ToString();
                                    var codes = SSccstoGen.Split(',').ToList();
                                    decimal jid = Convert.ToDecimal(vm.BatchNo);
                                    if (IsProduction)
                                    {
                                        var prodSts = gen.generateDAVAProduction(jid, vm.BtcNo, false, codes);
                                        msg = DAVAStausMessages(prodSts);
                                        if (Convert.ToString(prodSts) == "Success")
                                        {
                                            msg = TnT.LangResource.GlobalRes.DAVAIndexSelectOptionBatch +" " + msg;
                                        }
                                    }
                                    else
                                    {
                                        var tnsSts = gen.generateDAVAProductionTNS(jid, vm.BtcNo, false, codes);
                                        msg = DAVAStausMessages(tnsSts);
                                        if (Convert.ToString(tnsSts) == "Success")
                                        {
                                            msg = TnT.LangResource.GlobalRes.DAVAIndexSelectOptionBatch +" " + msg;
                                        }
                                    }
                                    TempData["Success"] = msg;
                                    trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(vm.ExemptedCountryCode))
                                {
                                    if (vm.IsWholeBatch)
                                    {
                                        decimal jid = Convert.ToDecimal(vm.BatchNo);
                                        var prodSts = gen.generateDAVATertPackExemp(jid, vm.BtcNo, true, vm.IsExemptedBarcode, vm.ExempCode, vm.ExemptedCountryCode, null);
                                        msg = DAVAStausMessages(prodSts);
                                        TempData["Success"] = msg;
                                        if (prodSts.ToString() == "Success")
                                        {
                                            msg = TnT.LangResource.GlobalRes.DAVAIndexSelectOptionBatch+ " " + msg;
                                        }
                                        trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                                    }
                                    else
                                    {
                                        var SSccstoGen = Request["GENSSCC[]"].ToString();
                                        var codes = SSccstoGen.Split(',').ToList();
                                        decimal jid = Convert.ToDecimal(vm.BatchNo);
                                        var prodSts = gen.generateDAVATertPackExemp(jid, vm.BtcNo, false, vm.IsExemptedBarcode, vm.ExempCode, vm.ExemptedCountryCode, codes);
                                        msg = DAVAStausMessages(prodSts);
                                        if (prodSts.ToString() == "Success")
                                        {
                                            msg = TnT.LangResource.GlobalRes.DAVAIndexSelectOptionBatch + " " + msg;
                                        }
                                        TempData["Success"] = msg;

                                        trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                                    }
                                }
                                else
                                {
                                    msg = TnT.LangResource.GlobalRes.TempDataDavaInvalidcountry;
                                    TempData["Success"] = msg;
                                    trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                                }
                            }
                        }
                        else
                        {
                            msg = DAVAStausMessages(sts);
                            TempData["Success"] = msg;
                            trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                        }
                    }
                    else
                    {
                        msg = TnT.LangResource.GlobalRes.TempDataDavaNobatch;
                        TempData["Success"] = msg;
                        trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                        ModelState.AddModelError("", TnT.LangResource.GlobalRes.TempDataDavaNobatch);
                    }
                }
            }
            //}
            //catch(Exception ex)
            //{
            //    TempData["Success"] = "No Data Found";
            //}
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult CheckSSCC(string SSCC, string JobId, string BatchNo)
        {
            if (SSCC.StartsWith("00") == true)
            {
                SSCC = SSCC.Substring(2);
            }
            if (string.IsNullOrEmpty(SSCC))
                return Json(TnT.LangResource.GlobalRes.TempDataDavaInvalid);
            if (SSCC.Length == 18 || SSCC.Length == 20 || SSCC.Length == 13)
            {
                DService util = new DService();
                if (util.IsSSCCValid(SSCC))
                {
                    if (!string.IsNullOrEmpty(JobId) && !string.IsNullOrEmpty(BatchNo))
                    {
                        var jid = Convert.ToInt32(JobId);
                        var data = util.getSSCCGrid(null, jid, BatchNo);
                        data.ActualBatchQty = db.Job.Where(d => d.JID == jid).FirstOrDefault().Quantity;
                        data.ExportBatchQty = db.PackagingDetails.Where(ep => ep.IsRejected == false && ep.JobID == jid).Count();
                        return Json(data);
                    }
                    else
                    {
                        return Json(TnT.LangResource.GlobalRes.TempDataDavaInvalidinput);
                    }
                }
                else
                {
                    return Json(TnT.LangResource.GlobalRes.TempDataDavaNotexist);
                }
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataDavaInvalid);
            }

        }



        #region Regenerate

        public ActionResult Regenerate()
        {
            getRegenerateFilesBatches();
            return View();
        }

        public void getRegenerateFilesBatches()
        {
            DService util = new DService();
            ViewBag.Batches = util.getDAVABtcForRegeneratn();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Regenerate([Bind(Include = "ProductionId,FileName")]RegenerateViewModel vm)
        {
            if (vm.ProductionId != 0 && (!string.IsNullOrEmpty(vm.FileName)))
            {
                string sFilePath = DAVAUtility.getFileLocationToStore() + "\\REGENERATED FILES";
                if (!Directory.Exists(sFilePath))
                {
                    Directory.CreateDirectory(sFilePath);
                }
                DAVAExportFileTagsInfo lstDAVAExportFileTagsInfos = getFileContentFromDB(vm.ProductionId);
                string content = lstDAVAExportFileTagsInfos.FileData;
                StreamWriter Sw = new StreamWriter(sFilePath + "\\" + lstDAVAExportFileTagsInfos.FILENAME + ".xml");
                Sw.WriteLine(content);
                Sw.Close();
                msg = TnT.LangResource.GlobalRes.TempDataDavaRegeneratedsuccessfully;
                TempData["Success"] = msg;
                trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                return RedirectToAction("Regenerate");
            }
            else
            {
                msg = TnT.LangResource.GlobalRes.TempDataDavaselectXML;
                TempData["Error"] = msg;
                trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailInfoDavaActivity);
                getRegenerateFilesBatches();
                return View(vm);
            }

        }

        private DAVAExportFileTagsInfo getFileContentFromDB(int ProductionId)
        {
            DbHelper m_dbhelper = new DbHelper();
            List<DAVAExportFileTagsInfo> lstDAVAExportFileTagsInfos = new List<DAVAExportFileTagsInfo>();
            lstDAVAExportFileTagsInfos = m_dbhelper.DBManager.DAVAExportFileTagsInfoBLL.DAVAExportFileTagsInfoDAO.GetDAVAExportFileTagsInfos(2, ProductionId.ToString());
            //return lstDAVAExportFileTagsInfos[0].FileData.ToString();
            return lstDAVAExportFileTagsInfos[0];
        }


        [HttpPost]
        public ActionResult getFileContent(string ProductionId)
        {
            int Prod = Convert.ToInt32(ProductionId);
            if (!string.IsNullOrEmpty(ProductionId))
            {
                string data = getFileContentFromDB(Prod).FileData;
                JsonResult result = Json(data);
                result.MaxJsonLength = Int32.MaxValue;
                return result;
            }
            else
            {
                return Json( TnT.LangResource.GlobalRes.TempDataDavaInvalid + "!");
            }

        }


        #endregion

    }
}