
using EPCIS_XMLs_Generation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer;
using TnT.DataLayer.EPCIS;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.EPCIS;
using TnT.DataLayer.Security;
using System.Net;
using System.Data.Entity;
using REDTR.HELPER;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using TnT.Models.Job;
using System.Data.SqlClient;
using System.IO;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class EPCISController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        List<string> lstSSCC = new List<string>();

        // get: epcis
        public ActionResult Index()
        {
            Bind();
            return View();
        }

        /// <summary>
        /// EPCIS Receiver Bind data
        /// </summary>
        private void BindEpcisData()
        {
            ViewBag.StateOrRegion = new SelectList(db.S_State, "ID", "StateName");
            ViewBag.Country = db.Country;
        }


        /// <summary>
        /// Epics Receiver Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            BindEpcisData();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyName,CountryId,StateId,City,Street1,Street2,PostalCode,GLN,site,street3,countryCode,latitude,logitude,Extension")]M_EPCISReceiver M_EPCISReceiver)
        {
            if (ModelState.IsValid)
            {
                M_EPCISReceiver.IsActive = true;
                M_EPCISReceiver.CreatedOn = DateTime.Now;
                M_EPCISReceiver.LastModified = DateTime.Now;
                M_EPCISReceiver.CreatedBy = Convert.ToInt32(User.ID);
                M_EPCISReceiver.ModifiedBy = Convert.ToInt32(User.ID);
                db.M_EPCISReceiver.Add(M_EPCISReceiver);
                db.SaveChanges();
                TempData["Success"] = M_EPCISReceiver.CompanyName + " " + TnT.LangResource.GlobalRes.TempDataEPCISReceiverCreated;
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailEPCISReceiverCreated + " " + M_EPCISReceiver.CompanyName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailEPCISReceiverCreated + " " + M_EPCISReceiver.CompanyName, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                return RedirectToAction("EPCISReceiver");
            }
            BindEpcisData();
            return View(M_EPCISReceiver);
        }

        /// <summary>
        /// EPCIS Receiver Index
        /// </summary>
        /// <returns></returns>
        public ActionResult EPCISReceiver()
        {

            return View(db.M_EPCISReceiver);
        }

        /// <summary>
        /// EPCIS Receiver Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_EPCISReceiver M_EPCISReceiver = db.M_EPCISReceiver.Find(id);
            if (M_EPCISReceiver == null)
            {
                return HttpNotFound();
            }
            return View(M_EPCISReceiver);
        }

        /// <summary>
        /// EPCIS Receiver EDIT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_EPCISReceiver M_EPCISReceiver = db.M_EPCISReceiver.Find(id);
            if (M_EPCISReceiver == null)
            {
                return HttpNotFound();
            }
            Bind();
            ViewBag.Country = new SelectList(db.Country, "ID", "CountryName", M_EPCISReceiver.CountryId);
            var data = db.S_State.Where(x => x.CountryID == M_EPCISReceiver.CountryId);
            ViewBag.StateOrRegion = new SelectList(data, "ID", "StateName", M_EPCISReceiver.StateId);

            return View(M_EPCISReceiver);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyName,CountryId,StateId,City,Street1,Street2,PostalCode,GLN,site,street3,countryCode,latitude,logitude,IsActive,Extension")]M_EPCISReceiver M_EPCISReceiver)
        {
            if (ModelState.IsValid)
            {
                var prevDtls = db.M_EPCISReceiver.Find(M_EPCISReceiver.ID);
                prevDtls.CompanyName = M_EPCISReceiver.CompanyName;
                prevDtls.CountryId = M_EPCISReceiver.CountryId;
                prevDtls.StateId = M_EPCISReceiver.StateId;
                prevDtls.City = M_EPCISReceiver.City;
                prevDtls.Street1 = M_EPCISReceiver.Street1;
                prevDtls.Street2 = M_EPCISReceiver.Street2;
                prevDtls.PostalCode = M_EPCISReceiver.PostalCode;
                prevDtls.GLN = M_EPCISReceiver.GLN;
                prevDtls.LastModified = DateTime.Now;
                prevDtls.ModifiedBy = Convert.ToInt32(User.ID);
                prevDtls.site = M_EPCISReceiver.site;
                prevDtls.street3 = M_EPCISReceiver.street3;
                prevDtls.countryCode = M_EPCISReceiver.countryCode;
                prevDtls.logitude = M_EPCISReceiver.logitude;
                prevDtls.latitude = M_EPCISReceiver.latitude;
                prevDtls.IsActive = M_EPCISReceiver.IsActive;
                prevDtls.Extension = M_EPCISReceiver.Extension;
                db.Entry(prevDtls).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "'" + M_EPCISReceiver.CompanyName + "' " + TnT.LangResource.GlobalRes.TempDataEPCISReceiverUpdated;
               trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailEpcisReceiverSuccessfullyupdated + ": " + M_EPCISReceiver.CompanyName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailEpcisReceiverSuccessfullyupdated + ": " + M_EPCISReceiver.CompanyName, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                return RedirectToAction("EPCISReceiver");
            }
            BindEpcisData();
            return View(M_EPCISReceiver);
        }

        [HttpPost]
        public ActionResult getCompanyName(string CompName)
        {
            var data = db.M_EPCISReceiver.Where(m => m.CompanyName == CompName).ToList();
            bool result;
            if (data.Count > 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return Json(result);
        }
        [HttpPost]
        public ActionResult getStateOrRegion(int Conid)
        {

            var data = (from st in db.S_State join con in db.Country on st.CountryID equals con.Id where st.CountryID == Conid select new { st.ID, st.StateName }).Distinct().ToList();

            return Json(data);
        }

        private void Bind()
        {
            try
            {
                ViewBag.Jobs = db.Job.Where(x => x.JobStatus == 3);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void Bind(int RCCount)
        {
            try
            {
                //bool status = false;
                //List<Job> jb = new List<Job>();
                //var Jobs = db.Job.Where(x => x.JobStatus == 3 ).ToList();//&& x.TID == 29 commenting this code as all batch type data should be seen

                //for (int i = 0; i < Jobs.Count(); i++)
                //{
                //    string tertiary = ProductPackageHelper.getTertiarryDeck(Jobs[i].PAID, Jobs[i].JID);
                //    status = GetJobStatus(Convert.ToInt32(Jobs[i].JID), RCCount, tertiary);
                //    if (status != false)
                //        jb.Add(Jobs[i]);
                //}
                //ViewBag.Jobs = jb;
                string qry = " DECLARE @Status int = 3, @RCResult INT = " + RCCount;
                qry += "; WITH CTE AS( SELECT J.JID, JD.JD_Deckcode, J.BatchNo, RANK() OVER(PARTITION BY JD.JD_JobID ORDER BY JD.id DESC) DeckID " +
                    "FROM Job J JOIN JobDetails JD ON J.JID = JD.JD_JobID WHERE J.JobStatus = @Status)";

                qry += " SELECT DISTINCT(C.JID), C.BatchNo FROM CTE C  JOIN PackagingDetails PD ON C.JID = PD.JobID " +
                       " WHERE DeckID = 1 AND PD.IsUsed = 1 AND IsRejected = 0 AND(PD.RCResult IS NULL OR PD.RCResult = @RCResult)" +
                       " AND PD.SSCC is not null";

                DbHelper m_dbhelper = new DbHelper();
                DataSet ds = m_dbhelper.GetDataSet(qry);

                ViewBag.Jobs = new SelectList(ds.Tables[0].AsDataView(), "JID", "BatchNo");
            }
            catch (Exception)
            {
                throw;
            }

        }


        public bool GetJobStatus(int jid, int RCcount, string tertiary)
        {
            if (!string.IsNullOrEmpty(jid.ToString()))
            {
                DbHelper m_dbhelper = new DbHelper();
                string Query = "SELECT * FROM PACKAGINGDETAILS WHERE (ISDECOMISSION IS NULL OR ISDECOMISSION =0) AND ISREJECTED IS NOT NULL and IsUsed=1 and PackageTypeCode='" + tertiary + "'"
+ "  AND SSCC is not null  AND(RCResult is null or RCResult=0 or RCResult=" + RCcount + ") and JOBID =" + jid;
                DataSet ds = m_dbhelper.GetDataSet(Query);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

      
        [HttpPost]

        public ActionResult GenerateEPCISOld([Bind(Include = "JobId,EpcisVersion")] Models.EPCIS.M_EPCIS EPCIS)
        {
            if (EPCIS.JobId != 0)
            {
                int jid = EPCIS.JobId;
                DbHelper m_dbhelper = new DbHelper();
                EventGeneration Xeg = new EventGeneration();
                EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
                var jobData = db.Job.Find(EPCIS.JobId); //db.Job.Where(x => x.JID == EPCIS.JobId).FirstOrDefault();
                var pkgAsso = db.PackagingAsso.Where(x => x.PAID == jobData.PAID).FirstOrDefault();
                DataTable dtPacks = new DataTable();
                string qryPacks = "Select * from PackagingDetails where JobID=" + jobData.JID + " and IsRejected=0 and (IsDecomission=0 or IsDecomission is null)";
                DataSet ds = m_dbhelper.GetDataSet(qryPacks);
                dtPacks = ds.Tables[0];
                var customer = db.M_Customer.Find(jobData.CustomerId);
                var settings = db.Settings.FirstOrDefault();
                string SenderGLN = string.Empty;
                SenderGLN = settings.GLN;
                SenderGLN = epcisConf.GetEPCGLN(SenderGLN, "0", settings.CompanyCode.Length);
                string ReceiverGLN = string.Empty;
                ReceiverGLN = customer.BizLocGLN;
                ReceiverGLN = epcisConf.GetEPCGLN(ReceiverGLN, "0", settings.CompanyCode.Length);
                string NDC = string.Empty;
                NDC = pkgAsso.CountryDrugCode;
                string header = string.Empty;
                string events = string.Empty;
                string end = string.Empty;
                header = Xeg.GenerateEpcisHeader(SenderGLN, ReceiverGLN, EPCIS.EpcisVersion);
                var jbdetail = db.JobDetails.Where(x => x.JD_JobID == jid);
                if (dtPacks != null)
                {
                    var AllPacks = dtPacks.AsEnumerable();
                    var lvls = ProductPackageHelper.getAllDeck(jobData.JID.ToString());
                    List<string> lstPackType = ProductPackageHelper.sorttheLevels(lvls);
                    DateTime MfgDate = AllPacks.First().Field<DateTime>("MfgPackDate");
                    DateTime ExpDate = AllPacks.First().Field<DateTime>("ExpPackDate");
                    foreach (string PackType in lstPackType)
                    {
                        string GTIN = jbdetail.Where(x => x.JD_Deckcode == PackType).Select(x => x.JD_GTIN).FirstOrDefault();
                        //List<string> allDeck = AllPacks.Where(x => x.Field<string>("PackageTypeCode") == PackType).Select(x => ((x.Field<string>("Code").Length == 18 || (!string.IsNullOrEmpty(x.Field<string>("SSCC")))) ? epcisConf.GetEPCSSCC(x.Field<string>("SSCC")) : epcisConf.GetEPCSGTIN(GTIN, x.Field<string>("Code")))).ToList();
                        List<string> allDeck = AllPacks.Where(x => x.Field<string>("PackageTypeCode") == PackType).Select(x => ((x.Field<string>("Code").Length == 18 || (!string.IsNullOrEmpty(x.Field<string>("SSCC")))) ? epcisConf.GetEPCSSCC(x.Field<string>("SSCC")) : epcisConf.GetEPCSGTIN(GTIN, x.Field<string>("Code"), customer.CompanyCode.Length))).ToList();

                        events += Xeg.GenerateObjectEventNew("ObjectEvent", allDeck, AllPacks.Where(x => x.Field<string>("PackageTypeCode") == PackType).First().Field<DateTime>("LastUpdatedDate"), MfgDate, ExpDate, jobData.BatchNo, SenderGLN, NDC);
                    }
                    string code = string.Empty;
                    string sscc = string.Empty;
                    for (int i = 1; i < lstPackType.Count; i++)
                    //foreach (string PackType in lstPackType)
                    {
                        string lstlvl = lstPackType[i - 1];
                        string fstlvl = lstPackType[i];
                        string GTINChild = jbdetail.Where(x => x.JD_Deckcode == lstlvl).Select(x => x.JD_GTIN).FirstOrDefault();
                        string GTINParent = jbdetail.Where(x => x.JD_Deckcode == fstlvl).Select(x => x.JD_GTIN).FirstOrDefault();
                        foreach (DataRow pack in AllPacks.Where(x => x.Field<string>("PackageTypeCode") == lstPackType[i]).ToList())
                        {
                            code = pack["Code"].ToString();
                            sscc = pack["SSCC"].ToString();
                            List<string> allChild = AllPacks.Where(x => x.Field<string>("NextLevelCode") == code).Select(x => (x.Field<string>("Code").Length == 18 ? epcisConf.GetEPCSSCC(x.Field<string>("Code")) : epcisConf.GetEPCSGTIN(GTINChild, x.Field<string>("Code"), customer.CompanyCode.Length))).ToList();
                            if (!string.IsNullOrEmpty(sscc))
                            {
                                code = sscc;
                            }

                            events += Xeg.GenerateAggregationEventNew("AggregationEvent", (code.Length == 18 ? epcisConf.GetEPCSSCC(code) : epcisConf.GetEPCSGTIN(GTINParent, code, customer.CompanyCode.Length)), allChild, pack.Field<DateTime>("LastUpdatedDate"), SenderGLN);
                        }
                    }

                    List<EpcisEventDetails> EPICSEvents = new List<EpcisEventDetails>();
                    List<int> lstBizStepIds = new List<int>();
                    lstBizStepIds.Add(db.BizStepMaster.Where(x => x.BizStep == "shipping").FirstOrDefault().Id);
                    var EPCISVersion = Convert.ToDouble(EPCIS.EpcisVersion);
                    if (EPCISVersion == 1.1)
                    {
                        EPICSEvents = db.EpcisEventDetails.Where(x => x.JobId == EPCIS.JobId && x.EpcisVersion == EPCISVersion && x.EventType != "AggregationEvent" && (lstBizStepIds.Contains(x.BizStepId))).ToList();
                    }
                    else
                    {
                        EPICSEvents = db.EpcisEventDetails.Where(x => x.JobId == EPCIS.JobId && x.EventType != "AggregationEvent" && (lstBizStepIds.Contains(x.BizStepId))).ToList();
                    }

                    foreach (var EpcisEve in EPICSEvents)
                    {
                        events += Xeg.convertToEvent(EpcisEve, SenderGLN, jobData.BatchNo, jobData.MfgDate, jobData.ExpDate, pkgAsso.CountryDrugCode);
                    }
                }
                end = Xeg.EndDocument();

                string MyPML = "";
                MyPML = header + events + end;
                var byts = Encoding.ASCII.GetBytes(MyPML);
                string Filename = jobData.BatchNo + "_v" + EPCIS.EpcisVersion + "_" + DateTime.Now + ".xml";
                trail.AddTrail(Filename + " "+ TnT.LangResource.GlobalRes.TempDataEPCISGeneratedSuccss, Convert.ToInt32(User.ID), Filename + " "+ TnT.LangResource.GlobalRes.TempDataEPCISGeneratedSuccss, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                return File(byts, ".xml", Filename);
            }
            else
            {
                Bind();
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataEPCISselectjob;
                trail.AddTrail(TnT.LangResource.GlobalRes.TempDataEPCISselectjob, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataEPCISselectjob, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                return View("Index");
            }
        }


        [HttpPost]

        public ActionResult GenerateEPCIS([Bind(Include = "JobId,EpcisVersion")] Models.EPCIS.M_EPCIS EPCIS)
        {
            int line_count = 10, overall_count = 5;
            if (EPCIS.JobId != 0)
            {
                line_count += 10; overall_count += 5;
                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgEPCISCollectingBatch, line_count, overall_count);

                #region FileOnLocation
                string EPCISFile = "_EPCIS_" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + ".xml";
                string TEMP_FOLDER_PATH = @"D:/TNTWEB/TEMP/";

                if (!Directory.Exists(TEMP_FOLDER_PATH)) Directory.CreateDirectory(TEMP_FOLDER_PATH);

                string[] t = Directory.GetFiles(TEMP_FOLDER_PATH, "*xml").Where(f => f.ToString().Contains("_EPCIS_")).ToArray();

                Array.ForEach(t, System.IO.File.Delete);

                EPCISFile = TEMP_FOLDER_PATH + EPCISFile;

                #endregion

                int jid = EPCIS.JobId;
                DbHelper m_dbhelper = new DbHelper();
                EventGeneration Xeg = new EventGeneration();
                EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
                var jobData = db.Job.Find(EPCIS.JobId); //db.Job.Where(x => x.JID == EPCIS.JobId).FirstOrDefault();
                var pkgAsso = db.PackagingAsso.Where(x => x.PAID == jobData.PAID).FirstOrDefault();
                DataTable dtPacks = new DataTable();
                string qryPacks = "Select Code, NextLevelCode,LastUpdatedDate, MfgPackDate, ExpPackDate, PackageTypeCode,SSCC from PackagingDetails where JobID=" + jobData.JID + " and IsRejected=0 and (IsDecomission=0 or IsDecomission is null) and IsUsed=1";
                DataSet ds = m_dbhelper.GetDataSet(qryPacks);
                dtPacks = ds.Tables[0];
                var customer = db.M_Customer.Find(jobData.CustomerId);
                var settings = db.Settings.FirstOrDefault();
                string SenderGLN = string.Empty;
                SenderGLN = settings.GLN;
                SenderGLN = epcisConf.GetEPCGLN(SenderGLN, "0", settings.CompanyCode.Length);
                string ReceiverGLN = string.Empty;
                ReceiverGLN = customer.BizLocGLN;
                ReceiverGLN = epcisConf.GetEPCGLN(ReceiverGLN, "0", settings.CompanyCode.Length);
                string NDC = string.Empty;
                NDC = string.IsNullOrEmpty(pkgAsso.CountryDrugCode) ? string.Empty : pkgAsso.CountryDrugCode;
                string header = string.Empty;
                string events = string.Empty;
                string end = string.Empty;
                line_count += 10; overall_count += 5;
                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgEPCISGeneratingHedr, line_count, overall_count);
                //header = Xeg.GenerateEpcisHeader(SenderGLN, ReceiverGLN, EPCIS.EpcisVersion);
                string HEADER = Xeg.GenerateEpcisHeader(SenderGLN, ReceiverGLN, EPCIS.EpcisVersion);
                var jbdetail = db.JobDetails.Where(x => x.JD_JobID == jid);
                if (dtPacks != null && dtPacks.Rows.Count > 0)
                {
                    var AllPacks = dtPacks.AsEnumerable();//.ToList();
                    var lvls = ProductPackageHelper.getAllDeck(jobData.JID.ToString());
                    List<string> lstPackType = ProductPackageHelper.sorttheLevels(lvls);
                    DateTime MfgDate = AllPacks.First().Field<DateTime>("MfgPackDate");
                    DateTime ExpDate = AllPacks.First().Field<DateTime>("ExpPackDate");


                    #region COMMISSION
                    using (StreamWriter sw = new StreamWriter(EPCISFile, true))
                    {
                        sw.WriteLine(HEADER);

                        string COMMISSION = string.Empty;

                        foreach (string PackType in lstPackType)
                        {
                            line_count += 10; overall_count += 5;
                            ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgEPCISCollectingDeck + " " + PackType, line_count, overall_count);

                            string GTIN = jbdetail.Where(x => x.JD_Deckcode == PackType).Select(x => x.JD_GTIN).FirstOrDefault();
                            string formatedCode = epcisConf.GetEPCSGTIN(GTIN, string.Empty, customer.CompanyCode.Length);
                            var packByDeck_ALL = AllPacks.Where(x => x.Field<string>("PackageTypeCode") == PackType).AsEnumerable().ToList();

                            if (packByDeck_ALL != null && packByDeck_ALL.Count > 0)
                            {
                                DateTime LastUpdatedDate = packByDeck_ALL.Where(x => x.Field<string>("PackageTypeCode") == PackType).First().Field<DateTime>("LastUpdatedDate");

                                var packByDeck_Code = packByDeck_ALL.Where(x => string.IsNullOrEmpty(x.Field<string>("SSCC"))).AsEnumerable().ToList();
                                var packByDeck_SSCC = packByDeck_ALL.Where(x => !string.IsNullOrEmpty(x.Field<string>("SSCC"))).AsEnumerable().ToList();

                                List<string> lstFormatedCodes = new List<string>();
                                if (packByDeck_Code != null && packByDeck_Code.Count > 0)
                                {
                                    lstFormatedCodes = packByDeck_Code.Select(x => formatedCode + x.Field<string>("Code")).ToList();
                                }
                                if (packByDeck_SSCC != null && packByDeck_SSCC.Count > 0)
                                {
                                    lstFormatedCodes.AddRange(packByDeck_SSCC.Select(x => epcisConf.GetEPCSSCC(x.Field<string>("SSCC"))).ToList());
                                }
                                line_count += 10; overall_count += 5;
                                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgEPCISGenratingObjDeck + " " + PackType, line_count, overall_count);
                                COMMISSION = Xeg.GenerateObjectEventNew("ObjectEvent", lstFormatedCodes, LastUpdatedDate, MfgDate, ExpDate, jobData.BatchNo, SenderGLN, NDC, "ADD", "urn:epcglobal:cbv:bizstep:commissioning", "urn:epcglobal:cbv:disp:active");
                            }

                            sw.WriteLine(COMMISSION);
                          
                        }
                    }
                    #endregion

                    #region DE-COMMISSION
                    //using (StreamWriter sw = new StreamWriter(EPCISFile, true))
                    //{
                    //    string DE_COMMISSION = string.Empty;

                    //    string qryPacksDecMoc = "Select Code,LastUpdatedDate from PackagingDetails where JobID=" + jobData.JID + " and PackageTypeCode='MOC' and (IsRejected=1 or IsRejected is null or IsDecomission=1 or IsUsed is null)";
                    //    DataSet dsDecMoc = m_dbhelper.GetDataSet(qryPacksDecMoc);
                    //    if (dsDecMoc.Tables[0].Rows.Count > 0)
                    //    {
                    //        string GTIN = jbdetail.Where(x => x.JD_Deckcode == "MOC").Select(x => x.JD_GTIN).FirstOrDefault();
                    //        string formatedCode = epcisConf.GetEPCSGTIN(GTIN, string.Empty, customer.CompanyCode.Length);
                    //        var pack_MOC = dsDecMoc.Tables[0].AsEnumerable();
                    //        if (pack_MOC != null && pack_MOC.Count() > 0)
                    //        {
                    //            DateTime LastUpdatedDate = pack_MOC.First().Field<DateTime>("LastUpdatedDate");

                    //            List<string> lstMOCsFormate = pack_MOC.Select(x => formatedCode + x.Field<string>("Code")).ToList();

                    //            line_count += 10; overall_count += 5;
                    //            ProgressHub.sendMessage("GENERATING DECOMMISSION EVENT.", line_count, overall_count);

                    //            DE_COMMISSION = Xeg.GenerateObjectEventNew("ObjectEvent", lstMOCsFormate, LastUpdatedDate, MfgDate, ExpDate, jobData.BatchNo, SenderGLN, NDC, "DELETE", "urn:epcglobal:cbv:bizstep:decommissioning", "urn:epcglobal:cbv:disp:inactive");
                    //        }

                    //        //var mocs = dsDecMoc.Tables[0].AsEnumerable();
                    //        //string GTIN = jbdetail.Where(x => x.JD_Deckcode == "MOC").Select(x => x.JD_GTIN).FirstOrDefault();
                    //        //List<string> allDeck = mocs.Select(x => epcisConf.GetEPCSGTIN(GTIN, x.Field<string>("Code"), customer.CompanyCode.Length)).ToList();
                    //        //events += Xeg.GenerateObjectEventNew("ObjectEvent", allDeck, mocs.First().Field<DateTime>("LastUpdatedDate"), MfgDate, ExpDate, jobData.BatchNo, SenderGLN, NDC, "DELETE", "urn:epcglobal:cbv:bizstep:decommissioning", "urn:epcglobal:cbv:disp:inactive");
                    //    }
                    //    sw.WriteLine(DE_COMMISSION);
                    //}
                    #endregion

                    #region AGGREGATION
                    using (StreamWriter sw = new StreamWriter(EPCISFile, true))
                    {
                        for (int i = 1; i < lstPackType.Count; i++)
                        //foreach (string PackType in lstPackType)
                        {
                            string AGGREGATION = string.Empty;

                            string childDeck = lstPackType[i - 1];
                            string parentDeck = lstPackType[i];

                            string GTINChild = jbdetail.Where(x => x.JD_Deckcode == childDeck).Select(x => x.JD_GTIN).FirstOrDefault();
                            string GTINParent = jbdetail.Where(x => x.JD_Deckcode == parentDeck).Select(x => x.JD_GTIN).FirstOrDefault();

                            var lstParent_ALL = AllPacks.Where(x => x.Field<string>("PackageTypeCode") == parentDeck).ToList();
                            var lstChild_ALL = AllPacks.Where(x => x.Field<string>("PackageTypeCode") == childDeck).ToList();

                            line_count += 10; overall_count += 5;
                            ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgEPCISGenratingAggrEvent, line_count, overall_count);

                            foreach (DataRow pack in lstParent_ALL)
                            {
                                string code = pack["Code"].ToString();
                                string sscc = pack["SSCC"].ToString();

                                string formatedCode = epcisConf.GetEPCSGTIN(GTINChild, string.Empty, customer.CompanyCode.Length);

                                var lstPack = lstChild_ALL.Where(x => x.Field<string>("NextLevelCode") == code).ToList();

                                // ADDED CODES
                                List<string> lstCodes = lstPack.Where(x => x.Field<string>("Code").Length < 18)
                                                               .Select(x => formatedCode + x.Field<string>("Code")).ToList();

                                // ADDED SSCC
                                lstCodes.AddRange(lstPack.Where(x => x.Field<string>("Code").Length == 18)
                                                         .Select(x => epcisConf.GetEPCSSCC(x.Field<string>("Code"))).ToList());

                                if (!string.IsNullOrEmpty(sscc))
                                {
                                    code = sscc;
                                }

                                AGGREGATION = Xeg.GenerateAggregationEventNew("AggregationEvent", (code.Length == 18 ? epcisConf.GetEPCSSCC(code) : epcisConf.GetEPCSGTIN(GTINParent, code, customer.CompanyCode.Length)), lstCodes, pack.Field<DateTime>("LastUpdatedDate"), SenderGLN);
                                sw.WriteLine(AGGREGATION);
                            }

                          
                        }
                    }
                    #endregion

                    #region SHIPPIG
                    line_count += 10; overall_count += 5;
                    ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgKopranEpcisGeneratingShippingEvent, line_count, overall_count);

                    List<EpcisEventDetails> EPICSEvents = new List<EpcisEventDetails>();
                    List<int> lstBizStepIds = new List<int>();
                    lstBizStepIds.Add(db.BizStepMaster.Where(x => x.BizStep == "shipping").FirstOrDefault().Id);
                    var EPCISVersion = Convert.ToDouble(EPCIS.EpcisVersion);

                    string strShippingQry = " SELECT E.* FROM EpcisEventDetails E " +
                                            " JOIN  BizStepMaster B ON B.Id = E.BizStepId" +
                                            " WHERE B.BizStep = 'shipping' AND E.JobID = " + jobData.JID +
                                            " AND E.EventType != 'AggregationEvent' " +
                                            "AND ('" + EPCISVersion + "' !='1.1' OR E.EpcisVersion = " + EPCISVersion + ")";

                    DataSet dsShipping = m_dbhelper.GetDataSet(strShippingQry);

                    line_count += 10; overall_count += 5;
                    ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgEPCISGenratingShipingEvent, line_count, overall_count);

                    using (StreamWriter sw = new StreamWriter(EPCISFile, true))
                    {
                        string SHIPPING = string.Empty;

                        if (dsShipping.Tables.Count > 0 && dsShipping.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow item in dsShipping.Tables[0].Rows)
                            {
                                EpcisEventDetails EpcisEve = new EpcisEventDetails();
                                EpcisEve.Id = Convert.ToInt32(item["Id"]);
                                EpcisEve.JobId = Convert.ToInt32(item["JobId"]);
                                EpcisEve.UUID = Convert.ToString(item["UUID"]);
                                EpcisEve.EventType = Convert.ToString(item["EventType"]);
                                EpcisEve.BizStepId = Convert.ToInt32(item["BizStepId"]);
                                EpcisEve.CreationDate = Convert.ToDateTime(item["CreationDate"]);
                                EpcisEve.CreatedBy = Convert.ToInt32(item["CreatedBy"]);
                                EpcisEve.EventTime = Convert.ToDateTime(item["EventTime"]);
                                EpcisEve.EventTimeZoneOffset = Convert.ToString(item["EventTimeZoneOffset"]);
                                EpcisEve.ParentID = Convert.ToString(item["ParentID"]);
                                EpcisEve.ChildEPC = Convert.ToString(item["ChildEPC"]);
                                EpcisEve.EpcList = Convert.ToString(item["EpcList"]);
                                EpcisEve.Action = Convert.ToString(item["Action"]);
                                EpcisEve.BizStep = Convert.ToString(item["BizStep"]);
                                EpcisEve.Disposition = Convert.ToString(item["Disposition"]);
                                EpcisEve.BizStep = Convert.ToString(item["BizStep"]);
                                EpcisEve.ReadPoint = Convert.ToString(item["ReadPoint"]);
                                EpcisEve.BizLocation = Convert.ToString(item["BizLocation"]);
                                EpcisEve.BizTransactionList = Convert.ToString(item["BizTransactionList"]);
                                EpcisEve.ExtensionData1 = Convert.ToString(item["ExtensionData1"]);
                                EpcisEve.ExtensionData2 = Convert.ToString(item["ExtensionData2"]);
                                EpcisEve.UserData1 = Convert.ToString(item["UserData1"]);
                                EpcisEve.UserData2 = Convert.ToString(item["UserData2"]);
                                EpcisEve.UserData3 = Convert.ToString(item["UserData3"]);
                                EpcisEve.EpcisVersion = Convert.ToDouble(item["EpcisVersion"]);

                                SHIPPING = Xeg.convertToEvent(EpcisEve, SenderGLN, jobData.BatchNo, jobData.MfgDate, jobData.ExpDate, pkgAsso.CountryDrugCode);
                                sw.WriteLine(SHIPPING);
                            }
                        }

                        string END = Xeg.EndDocument();

                        sw.WriteLine(END);
                    }

                  
                    #endregion
                }

                end = Xeg.EndDocument();

                string MyPML = "";
                MyPML = header + events + end;
                var byts = Encoding.ASCII.GetBytes(MyPML);
                string Filename = jobData.BatchNo + "_v" + EPCIS.EpcisVersion + "_" + DateTime.Now + ".xml";
                trail.AddTrail(Filename + " "+ TnT.LangResource.GlobalRes.TempDataEPCISGeneratedSuccss, Convert.ToInt32(User.ID), Filename + " "+ TnT.LangResource.GlobalRes.TempDataEPCISGeneratedSuccss, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);

                line_count = 100; overall_count = 100;
                ProgressHub.sendMessage(jobData.BatchNo + " "+ TnT.LangResource.GlobalRes.TempDataEPCISGeneratedSuccss, line_count, overall_count);
                //return File(byts, ".xml", Filename);
                return File(EPCISFile, ".xml", Filename);

            }
            else
            {
                Bind();
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataEPCISselectjob;
                trail.AddTrail(TnT.LangResource.GlobalRes.TempDataEPCISselectjob, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataEPCISselectjob, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                return View("Index");
            }
        }

        public ActionResult BatchWiseEPCISEventGeneration()
        {
            Bind();
            ViewBag.BizStep = db.BizStepMaster;
            return View();
        }

        [HttpPost]
        public ActionResult GenerateEPCISEvent([Bind(Include = "JobId,BizStepId,Disposition,BizLocation")] Models.EPCIS.EpcisEventDetails EPCIS)
        {
            if (ModelState.IsValid)
            {
                EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
                EPCIS.UUID = Guid.NewGuid().ToString().ToUpper();
                EPCIS.EventType = EpcisEnums.EPCISEventType.ObjectEvent.ToString();
                EPCIS.CreationDate = DateTime.Now;
                EPCIS.EventTime = DateTime.Now;
                EPCIS.RecordTime = DateTime.Now;
                EPCIS.CreatedBy = (int)User.ID;
                EPCIS.EventTimeZoneOffset = epcisConf.GetTimeZoneOffset();
                EPCIS.ParentID = null;
                EPCIS.ChildEPC = null;
                EPCIS.Action = EpcisEnums.action.ADD.ToString();
                EPCIS.BizStep = (from Biz in db.BizStepMaster where (Biz.Id == EPCIS.BizStepId) select Biz.BizStep).FirstOrDefault();

                string lineCode = (from jb in db.Job where jb.JID == EPCIS.JobId select jb.LineCode).Single();
                if (!string.IsNullOrEmpty(lineCode))
                {
                    string readGLN = (from line in db.LineLocation where line.ID == lineCode select line.ReadGLN).Single();
                    string readGLNExt = (from line in db.LineLocation where line.ID == lineCode select line.GLNExtension).Single();
                    var bizGLN = db.Settings.FirstOrDefault();
                    if (string.IsNullOrEmpty(readGLN) && string.IsNullOrEmpty(readGLNExt))
                    {
                        TempData["Success"] = TnT.LangResource.GlobalRes.EPCISCouldnotfindReadGLNorGLNExtension;
                        Bind();
                        ViewBag.BizStep = db.BizStepMaster;
                        return View("BatchWiseEPCISEventGeneration", EPCIS);
                    }
                    EPCIS.ReadPoint = epcisConf.GetEPCGLN(readGLN, readGLNExt, bizGLN.CompanyCode.Length);
                    EPCIS.BizLocation = epcisConf.GetEPCGLN(bizGLN.GLN, "001", bizGLN.CompanyCode.Length);
                }

                EPCIS.BizTransactionList = null;
                EPCIS.ExtensionData1 = null;
                EPCIS.ExtensionData2 = null;
                EPCIS.UserData1 = null;
                EPCIS.UserData2 = null;
                EPCIS.UserData3 = null;

                List<string> EPCList = null;
                EPCList = (from epc in db.EpcisEventDetails where (epc.JobId == EPCIS.JobId && epc.ParentID.Contains("sscc")) select epc.ParentID).ToList();
                if (EPCList != null)
                {
                    foreach (string item in EPCList)
                    {
                        EPCIS.EpcList += item + ",";
                        lstSSCC.Add(item);
                    }
                    if (EPCIS.EpcList.EndsWith(","))
                        EPCIS.EpcList = EPCIS.EpcList.Remove(EPCIS.EpcList.Length - 1);
                }


                db.EpcisEventDetails.Add(EPCIS);
                db.SaveChanges();

                TempData["Success"] = TnT.LangResource.GlobalRes.EPCISEventAddedSuccessfully;
                trail.AddTrail(TnT.LangResource.GlobalRes.EPCISEventAddedSuccessfully, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.EPCISEventAddedSuccessfully, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                return RedirectToAction("BatchWiseEPCISEventGeneration");
            }
            Bind();
            ViewBag.BizStep = db.BizStepMaster;
            return View("BatchWiseEPCISEventGeneration", EPCIS);
        }

        public ActionResult getBizStepWiseDispositions(int BizStepID)
        {
            var data = db.Dispositions.Where(x => x.BizStepId == BizStepID).ToList();
            return Json(data);
        }



        public ActionResult TransporterIndex()
        {
            return View(db.M_Transporter);
        }

        public ActionResult TransporterCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransporterCreate([Bind(Include = "ID,Name,ContactNo,EmailId,Address")]M_Transporter M_Transporter)
        {
            if (ModelState.IsValid)
            {

                db.M_Transporter.Add(M_Transporter);
                db.SaveChanges();
                TempData["Success"] = M_Transporter.Name + " " + TnT.LangResource.GlobalRes.TempDataEpcisTransporterCreatedSuccessfully;
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailEpcisTransporterCreate + " " + M_Transporter.Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailEpcisTransporterCreate + " " + M_Transporter.Name, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                return RedirectToAction("TransporterIndex");
            }
            BindEpcisData();
            return View(M_Transporter);
        }

        public ActionResult TranspoterDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_Transporter M_Transporter = db.M_Transporter.Find(id);
            if (M_Transporter == null)
            {
                return HttpNotFound();
            }
            return View(M_Transporter);
        }

        public ActionResult TransporterEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_Transporter M_Transporter = db.M_Transporter.Find(id);
            if (M_Transporter == null)
            {
                return HttpNotFound();
            }

            return View(M_Transporter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransporterEdit([Bind(Include = "ID,Name,ContactNo,EmailId,Address")]M_Transporter M_Transporter)
        {
            if (ModelState.IsValid)
            {
                var prevDtls = db.M_Transporter.Find(M_Transporter.ID);
                prevDtls.Name = M_Transporter.Name;
                prevDtls.ContactNo = M_Transporter.ContactNo;
                prevDtls.EmailId = M_Transporter.EmailId;
                prevDtls.Address = M_Transporter.Address;
                db.Entry(prevDtls).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "'" + M_Transporter.Name + "' " + TnT.LangResource.GlobalRes.TempDataEPCISReceiverUpdated;
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailTransporterSuccessfullyupdated + ": " + M_Transporter.Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailTransporterSuccessfullyupdated + ": " + M_Transporter.Name, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                return RedirectToAction("TransporterIndex");
            }
            BindEpcisData();
            return View(M_Transporter);
        }

        [HttpPost]
        public ActionResult getTranspoterName(string name)
        {
            var namelst = db.M_Transporter.Where(x => x.Name == name).ToList();
            bool result;
            if (namelst.Count > 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return Json(result);
        }

        public ActionResult Shipment()
        {
            Bind(2);
            ViewBag.BizStep = db.BizStepMaster;
            ViewBag.Receiver = db.M_EPCISReceiver.Where(x => x.IsActive == true);
            ViewBag.Transporter = db.M_Transporter;
            ViewBag.BizTransactionList = db.M_BizTransactionList;
            return View();
        }
        public ActionResult getSSCCsForShipment(int JobId, int RCCount)
        {
            if (!string.IsNullOrEmpty(JobId.ToString()))
            {
                var job = db.Job.Where(x => x.JID == JobId).FirstOrDefault();
                string tertiary = ProductPackageHelper.getTertiarryDeck(job.PAID, job.JID);
                List<string> lst = getSSCCForShipment(JobId, tertiary, RCCount);
                return Json(lst);
            }
            else
            {
                return null;
            }
        }
        public List<string> getSSCCForShipment(int JobId, string tertary, int RCCount)
        {
            if (!string.IsNullOrEmpty(JobId.ToString()))
            {
                DbHelper m_dbhelper = new DbHelper();
                string Query = "Select SSCC from PackagingDetails where PackagingDetails.JobID=" + JobId + " and PackagingDetails.SSCC is not null and IsRejected=0 and IsUsed=1 and PackageTypeCode='" + tertary + "' AND (RCResult is null or RCResult=0 or RCResult=" + RCCount + ") and (ISDECOMISSION IS NULL OR ISDECOMISSION =0)";
                DataSet ds = m_dbhelper.GetDataSet(Query);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                List<string> lst = new List<string>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lst.Add(ds.Tables[0].Rows[i][0].ToString());


                }
                return (lst);
            }
            else
            {
                return null;
            }
        }

        public bool insertPackagingDetail(int jobId, string SSCC, int RCCount)
        {
            try
            {
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                string qry = @"update [dbo].[PackagingDetails] set [RCResult]=" + RCCount + " where SSCC like '" + SSCC + "' and JobID=" + jobId;
                int rowAffected = SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, qry);

                if (rowAffected == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public ActionResult GenerateShipment([Bind(Include = "JobId,BizStepId,Disposition,ExtensionData1,ExtensionData2,UserData1,UserData2,UserData3,Action,DocumentType1,DocumentType2,DocumentType3,DocumentDetail1,DocumentDetail2,DocumentDetail3")] Models.EPCIS.ShipmentViewModel Shipment)
        {
            if (ModelState.IsValid)
            {
                EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
                EpcisEventDetails EPCIS = new EpcisEventDetails();
                EPCIS.JobId = Shipment.JobId;
                EPCIS.UUID = Guid.NewGuid().ToString().ToUpper();
                EPCIS.EventType = EpcisEnums.EPCISEventType.ObjectEvent.ToString();
                EPCIS.CreationDate = DateTime.Now;
                EPCIS.EventTime = DateTime.Now;
                EPCIS.RecordTime = DateTime.Now;
                EPCIS.CreatedBy = (int)User.ID;
                EPCIS.EventTimeZoneOffset = epcisConf.GetTimeZoneOffset();
                EPCIS.ParentID = null;
                EPCIS.ChildEPC = null;
                EPCIS.EpcisVersion = 1.1;
                EPCIS.BizStep = (from Biz in db.BizStepMaster where (Biz.Id == Shipment.BizStepId) select Biz.BizStep).FirstOrDefault();

                string lineCode = (from jb in db.Job where jb.JID == Shipment.JobId select jb.LineCode).Single();
                if (!string.IsNullOrEmpty(lineCode))
                {
                    string readGLN = (from line in db.LineLocation where line.ID == lineCode select line.ReadGLN).Single();
                    string readGLNExt = (from line in db.LineLocation where line.ID == lineCode select line.GLNExtension).Single();
                    string bizGLN = (from setting in db.Settings select setting.GLN).Single();
                    if (string.IsNullOrEmpty(readGLN) || string.IsNullOrEmpty(readGLNExt) || string.IsNullOrEmpty(bizGLN))
                    {
                        TempData["Success"] = TnT.LangResource.GlobalRes.EPCISCouldnotfindReadGLNorGLNExtension;
                        trail.AddTrail(TnT.LangResource.GlobalRes.EPCISCouldnotfindReadGLNorGLNExtension, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.EPCISCouldnotfindReadGLNorGLNExtension, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                        //Bind(2);
                        //ViewBag.BizStep = db.BizStepMaster;
                        //ViewBag.Receiver = db.M_EPCISReceiver.Where(x => x.IsActive == true);
                        //ViewBag.Transporter = db.M_Transporter;
                        return RedirectToAction("Shipment");
                    }
                    EPCIS.ReadPoint = epcisConf.GetEPCGLN(readGLN, readGLNExt);
                    EPCIS.BizLocation = epcisConf.GetEPCGLN(bizGLN, "0");
                }
                var job = db.Job.Where(x => x.JID == Shipment.JobId).FirstOrDefault();
                string tertiary = ProductPackageHelper.getTertiarryDeck(job.PAID, Shipment.JobId);
                EPCIS.BizTransactionList = null;
                List<string> SSCCs = null;
                if (Shipment.Action == "WholeBatch")
                {
                    SSCCs = getSSCCForShipment(Shipment.JobId, tertiary, 2);
                }
                else
                {
                    var SSccstoGen = Request["SSCCs[]"].ToString();
                    SSCCs = SSccstoGen.Split(',').ToList();
                }

                string epc = string.Empty;
                if (SSCCs != null)
                {
                    foreach (string item in SSCCs)
                    {
                        lstSSCC.Add(item);
                        epc = epcisConf.GetEPCSSCC(item);
                        EPCIS.EpcList += epc + ",";

                    }
                    if (EPCIS.EpcList.EndsWith(","))
                        EPCIS.EpcList = EPCIS.EpcList.Remove(EPCIS.EpcList.Length - 1);
                }

                EPCIS.Action = EpcisEnums.action.OBSERVE.ToString();

                EPCIS.BizTransactionList = Shipment.DocumentType1 + "=" + Shipment.DocumentDetail1;
                if (!string.IsNullOrEmpty(Shipment.DocumentType2))
                {
                    EPCIS.BizTransactionList += "," + Shipment.DocumentType2 + "=" + Shipment.DocumentDetail2;
                }
                if (!string.IsNullOrEmpty(Shipment.DocumentType3))
                {
                    EPCIS.BizTransactionList += "," + Shipment.DocumentType3 + "=" + Shipment.DocumentDetail3;
                }
                EPCIS.BizStepId = Shipment.BizStepId;
                EPCIS.Disposition = Shipment.Disposition;
                EPCIS.ExtensionData1 = Shipment.ExtensionData1;
                EPCIS.ExtensionData2 = Shipment.ExtensionData2;
                EPCIS.UserData1 = Shipment.UserData1;
                EPCIS.UserData2 = Shipment.UserData2;
                EPCIS.UserData3 = Shipment.UserData3;

                db.EpcisEventDetails.Add(EPCIS);
                db.SaveChanges();

                
                string connectionString = Utilities.getConnectionString("DefaultConnection");

                string SSCCsToUpdate = string.Join("','", lstSSCC.Select(s => s));

                string qry = " UPDATE dbo.PackagingDetails SET RCResult = CASE WHEN RCResult = 2 THEN 3  WHEN RCResult = 3 THEN 3 ELSE 1 END " +
                             " WHERE Jobid = " + EPCIS.JobId + " AND CONVERT(VARCHAR(20), SSCC) IN('" + SSCCsToUpdate + "')";

                int rowAffected = SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, qry);


                if (rowAffected == lstSSCC.Count)
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.EPCISEventAddedSuccessfully + ", "+ TnT.LangResource.GlobalRes.TempDataEPCISAllUpdated;
                }
                else
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.EPCISEventAddedSuccessfully + ", " + rowAffected + " "+ TnT.LangResource.GlobalRes.TempDataEPCISRecordsUpdated;
                }

               
                TempData["Success"] = TnT.LangResource.GlobalRes.EPCISEventAddedSuccessfully;
                trail.AddTrail(TnT.LangResource.GlobalRes.EPCISShipmentEventAddedSuccessfully + "  "+ TnT.LangResource.GlobalRes.TempDataEPCISForBatch + job.JobName, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.EPCISShipmentEventAddedSuccessfully + "  "+ TnT.LangResource.GlobalRes.TempDataEPCISForBatch + job.JobName, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                return RedirectToAction("Shipment");
            }
            Bind(2);
            ViewBag.BizStep = db.BizStepMaster;
            ViewBag.Receiver = db.M_EPCISReceiver.Where(x => x.IsActive == true);
            ViewBag.Transporter = db.M_Transporter;
            return View("Shipment", Shipment);
        }

        public ActionResult Transaction()
        {
            Bind(1);
            ViewBag.BizStep = db.BizStepMaster;
            ViewBag.BizTransactionList = db.M_BizTransactionList;
            return View();
        }
        [HttpPost]
        public ActionResult GenerateTransaction([Bind(Include = "JobId,BizStepId,Disposition,BizTransactionList,ExtensionData1,ExtensionData2,Action,UserData1,UserData2,UserData3")]  Models.EPCIS.EpcisEventDetails EPCIS)
        {
            if (ModelState.IsValid)
            {
                EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
                EPCIS.UUID = Guid.NewGuid().ToString().ToUpper();
                EPCIS.EventType = EpcisEnums.EPCISEventType.TransactionEvent.ToString();
                EPCIS.CreationDate = DateTime.Now;
                EPCIS.EventTime = DateTime.Now;
                EPCIS.RecordTime = DateTime.Now;
                EPCIS.CreatedBy = (int)User.ID;
                EPCIS.EventTimeZoneOffset = epcisConf.GetTimeZoneOffset();
                EPCIS.ParentID = null;
                EPCIS.ChildEPC = null;
                EPCIS.EpcisVersion = 1.1;
                EPCIS.BizStep = (from Biz in db.BizStepMaster where (Biz.Id == EPCIS.BizStepId) select Biz.BizStep).FirstOrDefault();

                string lineCode = (from jb in db.Job where jb.JID == EPCIS.JobId select jb.LineCode).Single();
                if (!string.IsNullOrEmpty(lineCode))
                {
                    string readGLN = (from line in db.LineLocation where line.ID == lineCode select line.ReadGLN).Single();
                    string readGLNExt = (from line in db.LineLocation where line.ID == lineCode select line.GLNExtension).Single();
                    var bizGLN = db.Settings.FirstOrDefault();
                    if (string.IsNullOrEmpty(readGLN) || string.IsNullOrEmpty(readGLNExt))
                    {
                        TempData["Success"] = TnT.LangResource.GlobalRes.EPCISCouldnotfindReadGLNorGLNExtension;
                        trail.AddTrail(TnT.LangResource.GlobalRes.EPCISCouldnotfindReadGLNorGLNExtension, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.EPCISCouldnotfindReadGLNorGLNExtension, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                        //Bind(1);
                        //ViewBag.BizStep = db.BizStepMaster;
                        return RedirectToAction("Transaction");
                    }
                    EPCIS.ReadPoint = epcisConf.GetEPCGLN(readGLN, readGLNExt, bizGLN.CompanyCode.Length);
                    EPCIS.BizLocation = epcisConf.GetEPCGLN(bizGLN.GLN, "001", bizGLN.CompanyCode.Length);
                }
                var job = db.Job.Where(x => x.JID == EPCIS.JobId).FirstOrDefault();
                List<string> SSCCs = null;
                if (EPCIS.Action == "WholeBatch")
                {

                    string tertiary = ProductPackageHelper.getTertiarryDeck(job.PAID, job.JID);
                    SSCCs = getSSCCForShipment(EPCIS.JobId, tertiary, 1);
                }
                else
                {
                    var SSccstoGen = Request["SSCCs[]"].ToString();
                    SSCCs = SSccstoGen.Split(',').ToList();
                }

                string epc = string.Empty;
                if (SSCCs != null)
                {
                    foreach (string item in SSCCs)
                    {
                        lstSSCC.Add(item);
                        epc = epcisConf.GetEPCSSCC(item);
                        EPCIS.EpcList += epc + ",";
                    }
                    if (EPCIS.EpcList.EndsWith(","))
                        EPCIS.EpcList = EPCIS.EpcList.Remove(EPCIS.EpcList.Length - 1);
                }

                EPCIS.Action = EpcisEnums.action.OBSERVE.ToString();
                EPCIS.BizTransactionList += "=" + EPCIS.UserData1;
                EPCIS.UserData1 = null;
                if (!string.IsNullOrEmpty(EPCIS.ExtensionData1))
                {
                    EPCIS.BizTransactionList += "," + EPCIS.ExtensionData1 + "=" + EPCIS.UserData2;
                    EPCIS.ExtensionData1 = null;
                    EPCIS.UserData2 = null;
                }
                if (!string.IsNullOrEmpty(EPCIS.ExtensionData2))
                {
                    EPCIS.BizTransactionList += "," + EPCIS.ExtensionData2 + "=" + EPCIS.UserData3;
                    EPCIS.ExtensionData2 = null;
                    EPCIS.UserData3 = null;
                }
                db.EpcisEventDetails.Add(EPCIS);
                db.SaveChanges();
                for (int i = 0; i < lstSSCC.Count(); i++)
                {
                    int jid = Convert.ToInt32(EPCIS.JobId);
                    string SSCC = lstSSCC[i].ToString();
                    var jb = db.PackagingDetails.Where(x => x.SSCC.Contains(SSCC) && x.JobID == jid).FirstOrDefault();
                    if (jb.RCResult != 1)
                    {
                        insertPackagingDetail(EPCIS.JobId, lstSSCC[i], 2);
                    }
                    else
                    {
                        insertPackagingDetail(EPCIS.JobId, lstSSCC[i], 3);
                    }
                }
                TempData["Success"] = TnT.LangResource.GlobalRes.EPCISEventAddedSuccessfully;
                trail.AddTrail(TnT.LangResource.GlobalRes.EPCISTransEventAddedSuccessfully + "  "+ TnT.LangResource.GlobalRes.TempDataEPCISForBatch + job.JobName, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.EPCISTransEventAddedSuccessfully + " "+ TnT.LangResource.GlobalRes.TempDataEPCISForBatch + job.JobName, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                return RedirectToAction("Transaction");
            }
            Bind(1);
            ViewBag.BizStep = db.BizStepMaster;
            ViewBag.BizTransactionList = db.M_BizTransactionList;
            return View(EPCIS);
        }
    }
}