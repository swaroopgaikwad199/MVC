using EPCIS_XMLs_Generation;
using REDTR.HELPER;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using TnT.DataLayer;
using TnT.DataLayer.Kopran_EPICS;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.EPCIS;
using TnT.Models.TraceLinkImporter;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class KopranEPCISController : BaseController
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        List<string> lstSSCC = new List<string>();
        // GET: KopranEPCIS
        public ActionResult Index()
        {
            Bind();
            return View();
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

        public ActionResult ImportUID()
        {
            ViewBag.Customer = db.M_Customer.Where(x => x.IsDeleted == false && x.ProviderId == 14);
            return View();
        }


        public ActionResult RequestXML(TracelinkViewModel vm)
        {
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                writer.Append("<?xml version=\"1.0\" ?>");
                writer.Append("<SerialNumberRequest xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                writer.Append("<TrxId>" + Guid.NewGuid() + "</TrxId>");
                writer.Append("<Gtin>" + vm.GTIN + "</Gtin>");
                writer.Append("<Quantity>" + vm.Quantity + "</Quantity>");
                writer.Append("</SerialNumberRequest>");
                string FileName = "Request_" + vm.GTIN + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xml";
                byte[] xml = Encoding.ASCII.GetBytes(writer.ToString());
                return File(xml, ".xml", FileName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult getCustomerData(int CId)
        {

            var data = db.M_Customer.Find(CId);
            int providerid = Convert.ToInt32(data.ProviderId);

            var product = db.PackagingAsso.Where(x => x.ProviderId == providerid && x.VerifyProd == true).ToList();

            object[] response = { data, product };
            return Json(response);


        }

        [HttpPost]
        public ActionResult getGTIN(int PAID)
        {
            var GTIN = db.PackagingAssoDetails.Where(x => x.PAID == PAID).OrderBy(x => x.Id).Select(x => x.GTIN).ToList();
            if (GTIN[0] == "")
            {
                var NTIN = db.PackagingAssoDetails.Where(x => x.PAID == PAID).OrderBy(x => x.Id).Select(x => x.NTIN).ToList();
                object[] response = { NTIN, "NTIN" };
                return Json(response);
            }
            else
            {
                object[] response = { GTIN, "GTIN" };
                return Json(response);

            }
        }

        public ActionResult UploadDAWAResponse()
        {
            ViewBag.Customer = db.M_Customer.Where(x => x.IsDeleted == false && x.ProviderId == 14);
            return View();
        }
        public ActionResult UploadResponseXML(HttpPostedFileBase file, TracelinkViewModel vm)
        {
            try
            {
                
                    if (file != null && file.ContentLength > 0 && file.ContentType == "text/xml")
                    {
                    if (vm.CustomerId != 0)
                    {
                        List<X_TracelinkUIDStore> listTLUIDStore = new List<X_TracelinkUIDStore>();
                        var cust = db.M_Customer.Where(x => x.Id == vm.CustomerId).FirstOrDefault();
                        var document = new XmlDocument();
                        document.Load(file.InputStream);
                        DataSet xmlDS = new DataSet();
                        XmlNodeReader reader = null;
                        reader = new XmlNodeReader(document);
                        xmlDS.ReadXml(reader);
                        if (xmlDS.Tables.Contains("SN"))
                        {
                            if (xmlDS.Tables["SN"].Rows.Count > 0)
                            {
                                M_TracelinkRequest req = new M_TracelinkRequest();
                                req.CustomerId = vm.CustomerId;
                                req.GTIN = Convert.ToString(xmlDS.Tables["SerialNumberResponse"].Rows[0]["GTIN"]);
                                req.Quatity = xmlDS.Tables["SN"].Rows.Count;
                                req.RequestedOn = DateTime.Now;
                                req.IsDeleted = false;
                                req.ProviderId =cust.ProviderId;
                                req.SrnoType = "GTIN";
                                req.FilterValue = "";
                                db.M_TracelinkRequest.Add(req);
                                db.SaveChanges();
                                for (int i = 0; i < xmlDS.Tables["SN"].Rows.Count; i++)
                                {
                                    X_TracelinkUIDStore TLUid = new X_TracelinkUIDStore();
                                    TLUid.SerialNo = Convert.ToString(xmlDS.Tables["SN"].Rows[i][0]);
                                    TLUid.TLRequestId = -1;
                                    TLUid.IsUsed = false;
                                    TLUid.GTIN = Convert.ToString(xmlDS.Tables["SerialNumberResponse"].Rows[0]["GTIN"]);
                                    TLUid.Type = vm.SrnoType;
                                    TLUid.TLRequestId = req.Id;
                                    listTLUIDStore.Add(TLUid);
                                }

                                var convertedData = DataLayer.GeneralDataHelper.convertToDataTable(listTLUIDStore);
                                BulkDataHelper dataHlpr = new BulkDataHelper();
                                if (dataHlpr.InsertTracelinkUIDIdenties(convertedData))
                                {
                                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterDataimported + " " + convertedData.Rows.Count + TnT.LangResource.GlobalRes.TempDataExporterSerialNoImpo;
                                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImporterDataimported + " " + convertedData.Rows.Count +TnT.LangResource.GlobalRes.TempDataExporterSerialNoImpoGTIN + vm.GTIN, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterDataimported + " " + convertedData.Rows.Count + TnT.LangResource.GlobalRes.TempDataExporterSerialNoImpoGTIN + vm.GTIN, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                                }
                                else
                                {
                                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterUnablestore;
                                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImporterUnablestore, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterUnablestore, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                                }
                            }
                            else
                            {
                                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataKopranEpcisDatacantnotImpo;
                                ViewBag.Customer = db.M_Customer.Where(x => x.IsDeleted == false && x.ProviderId == 14);
                                return View("UploadDAWAResponse");
                            }
                        }

                    }
                    else
                    {
                        TempData["Success"] = TnT.LangResource.GlobalRes.toastrTracelinkCustomerId;
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ViewBag.Customer = db.M_Customer.Where(x => x.IsDeleted == false && x.ProviderId == 14);
            return View("UploadDAWAResponse");
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
                KP_EPCIS_XMLs_Generation Xeg = new KP_EPCIS_XMLs_Generation();
                EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
                int jid = EPCIS.JobId;
                DbHelper m_dbhelper = new DbHelper();
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
                string header = string.Empty;
                string events = string.Empty;
                string end = string.Empty;
                line_count += 10; overall_count += 5;
                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgEPCISGeneratingHedr, line_count, overall_count);
                string HEADER = Xeg.GenerateEpcisHeader(EPCIS.EpcisVersion);
                var jbdetail = db.JobDetails.Where(x => x.JD_JobID == jid);
                if (dtPacks != null && dtPacks.Rows.Count > 0)
                {
                    var AllPacks = dtPacks.AsEnumerable();//.ToList();
                    var lvls = ProductPackageHelper.getAllDeck(jobData.JID.ToString());
                    List<string> lstPackType = ProductPackageHelper.sorttheLevels(lvls);
                    DateTime ExpDate = AllPacks.First().Field<DateTime>("ExpPackDate");
                    #region COMMISSION
                    using (StreamWriter sw = new StreamWriter(EPCISFile, true))
                    {
                        sw.WriteLine(HEADER);

                        string COMMISSION = string.Empty;

                        foreach (string PackType in lstPackType)
                        {
                            line_count += 10; overall_count += 5;
                            ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgEPCISCollectingDeck + PackType, line_count, overall_count);

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
                                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgEPCISGenratingObjDeck + PackType, line_count, overall_count);
                                COMMISSION = Xeg.GenerateObjectEvent("ObjectEvent", lstFormatedCodes, LastUpdatedDate, ExpDate, jobData.BatchNo, SenderGLN, "ADD", "urn:epcglobal:cbv:bizstep:commissioning", "urn:epcglobal:cbv:disp:active");
                            }

                            sw.WriteLine(COMMISSION);

                            if (PackType == "MOC")
                            {
                                #region DE-COMMISSION
                                //using (StreamWriter sw = new StreamWriter(EPCISFile, true))
                                //{
                                string DE_COMMISSION = string.Empty;

                                string qryPacksDecMoc = "Select Code,LastUpdatedDate from PackagingDetails where JobID=" + jobData.JID + " and PackageTypeCode='MOC' and (IsRejected=1 or IsRejected is null or IsDecomission=1 or IsUsed is null)";
                                DataSet dsDecMoc = m_dbhelper.GetDataSet(qryPacksDecMoc);
                                if (dsDecMoc.Tables[0].Rows.Count > 0)
                                {
                                    //string GTIN = jbdetail.Where(x => x.JD_Deckcode == "MOC").Select(x => x.JD_GTIN).FirstOrDefault();
                                    // string formatedCode = epcisConf.GetEPCSGTIN(GTIN, string.Empty, customer.CompanyCode.Length);
                                    var pack_MOC = dsDecMoc.Tables[0].AsEnumerable();
                                    if (pack_MOC != null && pack_MOC.Count() > 0)
                                    {
                                        DateTime LastUpdatedDate = pack_MOC.First().Field<DateTime>("LastUpdatedDate");

                                        List<string> lstMOCsFormate = pack_MOC.Select(x => formatedCode + x.Field<string>("Code")).ToList();

                                        line_count += 10; overall_count += 5;
                                        ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgKopranEpcisGeneratingDecommEvent, line_count, overall_count);

                                        DE_COMMISSION = Xeg.GenerateObjectEvent("ObjectEvent", lstMOCsFormate, LastUpdatedDate, ExpDate, jobData.BatchNo, SenderGLN, "DELETE", "urn:epcglobal:cbv:bizstep:decommissioning", "urn:epcglobal:cbv:disp:inactive");
                                    }


                                }
                                sw.WriteLine(DE_COMMISSION);
                                //}
                                #endregion
                            }

                        }
                    }

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

                                AGGREGATION = Xeg.GenerateAggregationEvent("AggregationEvent", (code.Length == 18 ? epcisConf.GetEPCSSCC(code) : epcisConf.GetEPCSGTIN(GTINParent, code, customer.CompanyCode.Length)), lstCodes, pack.Field<DateTime>("LastUpdatedDate"), SenderGLN);
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
                string Filename = jobData.BatchNo + "_v" + EPCIS.EpcisVersion + "_" + DateTime.Now + ".xml";
                trail.AddTrail(Filename + TnT.LangResource.GlobalRes.TempDataEPCISGeneratedSuccss, Convert.ToInt32(User.ID), Filename + TnT.LangResource.GlobalRes.TempDataEPCISGeneratedSuccss, TnT.LangResource.GlobalRes.TrailInfoEPCISActivity);
                line_count = 100; overall_count = 100;
                ProgressHub.sendMessage(jobData.BatchNo + TnT.LangResource.GlobalRes.TempDataEPCISGeneratedSuccss, line_count, overall_count);

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


    }
}