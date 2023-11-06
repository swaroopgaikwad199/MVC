using REDTR.HELPER;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using TnT.DataLayer;
using TnT.DataLayer.EPCISHelper;
using TnT.DataLayer.RFXCELService;
using TnT.DataLayer.Security;
using TnT.DataLayer.TraceKey;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.Code;
using TnT.Models.EPCIS;
using TnT.Models.TraceKey;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class TraceKeyController : BaseController
    {
        EPCISConfig config = new EPCISConfig();
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        // GET: TraceKey
        public ActionResult Index()
        {
            Bind();
            return View();
        }

        public void Bind()
        {
            ViewBag.Jobs = db.Job.Where(x => x.JobStatus == 3);
            ViewBag.Customer = db.M_Customer.Where(x => x.IsActive == true);
        }



        //public ActionResult GenerateTraceKeyOld(TraceKeyViewModel vm)
        //{
        //    int JID = Convert.ToInt32(vm.JID);
        //    List<EpcisEventDetails> EPICSEvents = new List<EpcisEventDetails>();
        //    var job = db.Job.Where(x => x.JID == JID).FirstOrDefault();
        //    var jobdetail = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == "MOC").FirstOrDefault();
        //    var sendr = db.Settings.FirstOrDefault();
        //    var customer = db.M_Customer.Where(x => x.Id == vm.Cid).FirstOrDefault();
        //    var lineLocationData = db.LineLocation.Where(l => l.ID == job.LineCode).FirstOrDefault();
        //    EPICSEvents = db.EpcisEventDetails.Where(x => x.JobId == JID ).ToList();
        //    string sernderId = config.GetEPCGLN(lineLocationData.ReadGLN, lineLocationData.GLNExtension, sendr.CompanyCode.Length);
        //    string receiverId = config.GetEPCGLN(customer.BizLocGLN, customer.BizLocGLN_Ext, customer.CompanyCode.Length);
        //    XmlGeneration xml = new XmlGeneration();
        //    string Header = "";
        //    Header = xml.GenerateEPCISHeader(job, sernderId, receiverId, vm.TypeVersion, vm.ProductionDocument, jobdetail.JD_GTIN);
        //    string EndDocument = "";
        //    EndDocument = xml.EndDocument();
        //    string Events = "";
        //    foreach (var EpcisEve in EPICSEvents)
        //    {
        //        Events += xml.convertToEvent(EpcisEve, sernderId);
        //    }

        //    string MyPML = "";
        //    MyPML = Header + Events + EndDocument;
        //    var byts = Encoding.ASCII.GetBytes(MyPML);
        //    string Filename = job.BatchNo + "_v" + vm.TypeVersion + "_" + DateTime.Now + ".xml";

        //    return File(byts, ".xml", Filename);
        //}

        public ActionResult TraceKeyList()
        {
            var openRequest = db.M_TkeySerialRequest.Where(s => s.Status != (int)TKEY_SERIAL_REQUEST_STATUS.REQUEST_DOWNLOADED);
            return View(openRequest);
        }

        public ActionResult TraceKeyGetStatus(int id)
        {
            M_TkeySerialRequest obj = db.M_TkeySerialRequest.FirstOrDefault(s => s.ID == id);

            TKEY_REQUEST_PROCESS_URL = Convert.ToString(Utilities.getAppSettings("TKEY_REQUEST_PROCESS_URL"));
            TKEY_USERNAME = Convert.ToString(Utilities.getAppSettings("TKEY_USERNAME"));
            TKEY_PASSWORD = Convert.ToString(Utilities.getAppSettings("TKEY_PASSWORD"));

            #region PROCESS REQUEST
            try
            {
                var process_request_url = new Uri(TKEY_REQUEST_PROCESS_URL.Replace("{ticketId}", obj.TicketId));

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(TKEY_USERNAME, TKEY_PASSWORD);

                    var response = client.DownloadData(process_request_url);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream(response);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(ms);

                    obj = new M_TkeySerialRequest();

                    obj.TicketId = doc.GetElementsByTagName("TicketId").Item(0).InnerText;
                    obj.Message = doc.GetElementsByTagName("Message").Item(0).InnerText;
                    obj.ResponseStatus = doc.GetElementsByTagName("Status").Item(0).InnerText;
                    obj.Status = (int)TKEY_SERIAL_REQUEST_STATUS.REQUEST_PROCESS;

                    string connectionString = Utilities.getConnectionString("DefaultConnection");

                    string qry = @" UPDATE M_TkeySerialRequest SET ResponseStatus= '" + obj.ResponseStatus + "', " + " Message ='" + obj.Message + "'" +
                                  ", Status = " + obj.Status + ", LastUpdatedDate =GETDATE() WHERE TicketId ='" + obj.TicketId + "'";

                    int rowAffected = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, qry);
                }
            }
            catch (Exception ex)
            {
                DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);

                TempData["Success"] = TnT.LangResource.GlobalRes.TRacekeyListTempstatusreq;
            }

            #endregion

            //var openRequest = db.M_TkeySerialRequest.Where(s => s.Status != (int)TKEY_SERIAL_REQUEST_STATUS.REQUEST_DOWNLOADED);

            return RedirectToAction("TraceKeyList");//, openRequest);
        }

        public class WebClientWithTimeout : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest wr = base.GetWebRequest(address);
                wr.Timeout = 20 * 60 * 10000;// = 20 mins timeout in milliseconds (ms) //System.Threading.Timeout.Infinite;//
                return wr;
            }
        }

        public ActionResult TraceKeyGetDownloadNo(int id)
        {
            M_TkeySerialRequest obj = db.M_TkeySerialRequest.FirstOrDefault(s => s.ID == id);

            TKEY_REQUEST_DOWNLOAD_URL = Convert.ToString(Utilities.getAppSettings("TKEY_REQUEST_DOWNLOAD_URL"));
            TKEY_USERNAME = Convert.ToString(Utilities.getAppSettings("TKEY_USERNAME"));
            TKEY_PASSWORD = Convert.ToString(Utilities.getAppSettings("TKEY_PASSWORD"));

            #region DOWNLOAD REQUEST
            try
            {
                var download_request_url = new Uri(TKEY_REQUEST_DOWNLOAD_URL.Replace("{ticketId}", obj.TicketId));

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                using (var client = new WebClientWithTimeout())
                {
                    client.Credentials = new NetworkCredential(TKEY_USERNAME, TKEY_PASSWORD);

                    var response = client.DownloadData(download_request_url);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream(response);
                    XmlDocument doc = new XmlDocument();

                    doc.Load(ms);
                    //doc.Load(@"D:\Exception\TKEY_RES_04150568728383_1004122019_09_07_22.xml");

                    if (System.IO.Directory.Exists(@"D:\Exception\"))
                    {
                        doc.Save(@"D:\Exception\TKEY_RES_" + obj.GTIN + "_" + obj.Quantity + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + ".xml");
                    }

                    #region Add M_TracelinkRequest
                    Models.TraceLinkImporter.M_TracelinkRequest m_Tracelink = new Models.TraceLinkImporter.M_TracelinkRequest();

                    m_Tracelink.Quatity = obj.Quantity;
                    m_Tracelink.GTIN = obj.GTIN;
                    m_Tracelink.RequestedOn = DateTime.Now;
                    m_Tracelink.IsDeleted = false;
                    m_Tracelink.Threshold = 0;
                    m_Tracelink.CustomerId = obj.CustomerId;
                    m_Tracelink.ProviderId = obj.ProviderId;
                    m_Tracelink.SrnoType = "GTIN";

                    db.M_TracelinkRequest.Add(m_Tracelink);
                    db.SaveChanges();

                    #endregion

                    var TicketId = doc.GetElementsByTagName("TicketId").Item(0).InnerText;

                    //var serialResponse = doc.GetElementsByTagName("SerialNumberList")[0].ChildNodes;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("TLRequestId", typeof(int));
                    dt.Columns.Add("SerialNo", typeof(string));
                    dt.Columns.Add("IsUsed", typeof(bool));
                    dt.Columns.Add("GTIN", typeof(string));
                    dt.Columns.Add("Type", typeof(string));
                    dt.Columns.Add("CryptoCode", typeof(string));
                    dt.Columns.Add("PublicKey", typeof(string));

                    DataSet ds = new DataSet();
                    var xmlReader = new XmlNodeReader(doc);
                    ds.ReadXml(xmlReader);
                    string PublicKey = null;
                    for (int i = 0; i < ds.Tables["SN"].Rows.Count; i++)
                    {
                        string serialNo = Convert.ToString(ds.Tables["SN"].Rows[i]["SN_Text"]);
                        serialNo = serialNo.Substring(serialNo.LastIndexOf('.') + 1);

                        string CryptoCode = null, type = "GTIN";

                        if (ds.Tables["SN"].Columns.Contains("russianCryptoNumber"))
                        {
                            type = "CRPTO";

                            CryptoCode = Convert.ToString(ds.Tables["SN"].Rows[i]["russianCryptoNumber"]);
                            PublicKey = Convert.ToString(ds.Tables["SN"].Rows[i]["russianCryptoKey"]);
                        }

                        dt.Rows.Add(new object[] { m_Tracelink.Id, serialNo, false, obj.GTIN, type, CryptoCode, PublicKey });
                    }

                    //foreach (XmlNode item in serialResponse)
                    //{
                    //    string serialNo = item.InnerText.Substring(item.InnerText.LastIndexOf('.') + 1);
                    //    var cryptoCode = item.OuterXml;
                    //    dt.Rows.Add(new object[] { m_Tracelink.Id, serialNo, false, obj.GTIN, "GTIN" });
                    //}

                    BulkDataHelper bulkDataHelper = new BulkDataHelper();
                    bool IsUpdated = bulkDataHelper.InsertTracelinkUIDIdenties(dt);
                    

                    if (IsUpdated)
                    {
                        //obj = new M_TkeySerialRequest();

                        obj.Message = doc.GetElementsByTagName("Message").Item(0).InnerText;
                        obj.ResponseStatus = doc.GetElementsByTagName("Status").Item(0).InnerText;
                        obj.Status = (int)TKEY_SERIAL_REQUEST_STATUS.REQUEST_DOWNLOADED;

                        string connectionString = Utilities.getConnectionString("DefaultConnection");

                        string qry = @" UPDATE M_TkeySerialRequest SET ResponseStatus= '" + obj.ResponseStatus + "', " + " Message ='" + obj.Message + "'" +
                                      ", Status = " + obj.Status + ", LastUpdatedDate =GETDATE(), TLRequestId= " + m_Tracelink.Id + " WHERE TicketId ='" + TicketId + "'";

                        int rowAffected = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, qry);

                        if (!string.IsNullOrEmpty(PublicKey))
                        {
                            qry = @" UPDATE PackagingAsso SET PublicKey='" + PublicKey + "' WHERE PAID IN (SELECT PAID FROM " +
                                   " PackagingAssoDetails  WHERE  GTIN ='" + obj.GTIN + "')";

                            Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, qry);
                        }
                        int count = db.X_TracelinkUIDStore.Where(x => x.TLRequestId == m_Tracelink.Id).Count();
                        int duplicate = obj.Quantity - count;
                       //TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterDataimported + " " + count + " " + obj.GTIN + " imported.!" + duplicate + " Number of Duplicate " + obj.GTIN + " Found.";
                        TempData["Success"] = count + " " + TnT.LangResource.GlobalRes.TempDataTrackeySerailNoImportedSucc + duplicate + " SERIAL NUMBER FOUND ";
                        trail.AddTrail(User.FirstName + " " + count + " " + TnT.LangResource.GlobalRes.TempDataTrackeySerailNoImportedSucc + duplicate + " SERIAL NUMBER FOUND ", Convert.ToInt32(User.ID), User.FirstName + " " + count + " " + TnT.LangResource.GlobalRes.TempDataTrackeySerailNoImportedSucc + duplicate + " SERIAL NUMBER FOUND ", TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                    }
                    else
                    {
                        TempData["Success"] = "UNABLE TO SAVE RECORDS";
                    }

                    

                }

            }
            catch (Exception ex)
            {
                DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);

                TempData["Success"] = TnT.LangResource.GlobalRes.TRacekeyListTempstatusreq;
            }
            #endregion

            //var openRequest = db.M_TkeySerialRequest.Where(s => s.Status != (int)TKEY_SERIAL_REQUEST_STATUS.REQUEST_DOWNLOADED);

            return RedirectToAction("TraceKeyList");//, openRequest);
        }

        //public ActionResult GenerateTraceKeyOLD(TraceKeyViewModel vm)
        //{
        //    int JID = Convert.ToInt32(vm.JID);
        //    List<EpcisEventDetails> EPICSEvents = new List<EpcisEventDetails>();
        //    EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
        //    var job = db.Job.Where(x => x.JID == JID).FirstOrDefault();
        //    var jobdetail = db.JobDetails.Where(x => x.JD_JobID == JID);
        //    var sendr = db.Settings.FirstOrDefault();
        //    var customer = db.M_Customer.Where(x => x.Id == vm.Cid).FirstOrDefault();
        //    var lineLocationData = db.LineLocation.Where(l => l.ID == job.LineCode).FirstOrDefault();
        //    EPICSEvents = db.EpcisEventDetails.Where(x => x.JobId == JID).ToList();
        //    string sernderId = config.GetEPCGLN(lineLocationData.ReadGLN, lineLocationData.GLNExtension, sendr.CompanyCode.Length);
        //    string receiverId = config.GetEPCGLN(customer.BizLocGLN, customer.BizLocGLN_Ext, customer.CompanyCode.Length);
        //    XmlGeneration xml = new XmlGeneration();
        //    string Header = "";
        //    Header = xml.GenerateEPCISHeader(job, sernderId, receiverId, vm.TypeVersion, vm.ProductionDocument, jobdetail.FirstOrDefault(s => s.JD_Deckcode == "MOC").JD_GTIN);
        //    string EndDocument = "";
        //    EndDocument = xml.EndDocument();
        //    DbHelper m_dbhelper = new DbHelper();
        //    DataTable dtPacks = new DataTable();
        //    //string qryPacks = "Select Code,LastUpdatedDate from PackagingDetails where JobID=" + JID + " and IsRejected=0 and (IsDecomission=0 or IsDecomission is null) and (IsManualUpdated=0 or IsManualUpdated is null) and IsUsed=1 and PackageTypeCode='MOC'";// and NextLevelCode!='FFFFF'
        //    string qryPacks = "Select Code,LastUpdatedDate from PackagingDetails where JobID=" + JID + " and IsRejected=0 and (IsDecomission=0 or IsDecomission is null) and IsUsed=1 and PackageTypeCode='MOC'";// and NextLevelCode!='FFFFF'
        //    DataSet ds = m_dbhelper.GetDataSet(qryPacks);
        //    dtPacks = ds.Tables[0];
        //    DateTime evtTime = Convert.ToDateTime(dtPacks.Rows[0]["LastUpdatedDate"]);
        //    string formatedCode = epcisConf.GetEPCSGTIN(jobdetail.FirstOrDefault(s => s.JD_Deckcode == "MOC").JD_GTIN, string.Empty, customer.CompanyCode.Length);
        //    var packByDeck_Code = dtPacks.AsEnumerable().ToList();
        //    string Commission = "";
        //    //foreach (var EpcisEve in EPICSEvents)
        //    //{
        //    //    Events += xml.convertToEvent(EpcisEve, sernderId);
        //    //}
        //    List<string> lstFormatedCodes = new List<string>();
        //    if (packByDeck_Code != null && packByDeck_Code.Count > 0)
        //    {
        //        lstFormatedCodes = packByDeck_Code.Select(x => formatedCode + x.Field<string>("Code")).ToList();
        //    }

        //    Commission = xml.CommissionEvent(job, lstFormatedCodes, evtTime);

        //    var lvls = ProductPackageHelper.getAllDeck(job.JID.ToString());
        //    List<string> lstPackType = ProductPackageHelper.sorttheLevels(lvls);

        //    #region DeCommission 

        //    string mocDeck = lstPackType[0];
        //    string GTINmoc = jobdetail.Where(x => x.JD_Deckcode == mocDeck).Select(x => x.JD_GTIN).FirstOrDefault();
        //    string mocPrefix = epcisConf.GetEPCSGTIN(GTINmoc, string.Empty, customer.CompanyCode.Length);

        //    var lstBad = db.PackagingDetails.Where(s => s.JobID == vm.JID && s.PackageTypeCode == mocDeck && (s.IsRejected == true || s.IsUsed == false || s.IsDecomission == true)).ToList();
        //    string DeCommision = string.Empty;
        //    if (lstBad != null && lstBad.Count > 0)
        //    {
        //        DeCommision = xml.DecommissioneEvent(job, lstBad.Select(s => mocPrefix + s.Code.ToString()).ToList(), lstBad.ToList()[0].LastUpdatedDate);
        //    }

        //    #endregion

        //    string aggregationEvent = string.Empty;
        //    string SSCCCommision = string.Empty;
        //    if (lstPackType.Count > 1)
        //    {
        //        #region Commission SSCC
        //        string firstDeck = lstPackType[0];

        //        foreach (string PackType in lstPackType)
        //        {
        //            if (PackType != firstDeck)
        //            {
        //                var lstSSCC = db.PackagingDetails.Where(s => s.JobID == vm.JID &&
        //                                                     s.PackageTypeCode == PackType &&
        //                                                     s.IsRejected == false && s.IsUsed == true && s.IsDecomission == false).OrderBy(x => x.LastUpdatedDate).ToList();

        //                var ssccs = lstSSCC.Select(s => string.IsNullOrEmpty(s.SSCC) ? s.Code : s.SSCC).ToList();

        //                SSCCCommision += xml.SSCCCommsionEvent(ssccs, lstSSCC.ToList()[0].LastUpdatedDate);
        //            }
        //        }



        //        #endregion

        //        #region Aggregation
        //        //string qryAgg = " SELECT PackageTypeCode, Code, NextLevelCode, SSCC FROM PackagingDetails where JobID = " + vm.JID + " AND NextLevelCode IS NOT NULL AND " +
        //        //                " NextLevelCode<>'FFFFF' ORDER BY NextLevelCode, Code";

        //        //DataSet dsAgg = m_dbhelper.GetDataSet(qryAgg);


        //        List<PackagingDetails> lstPack = new List<PackagingDetails>(); //db.PackagingDetails.Where(s => s.JobID == vm.JID && s.NextLevelCode != "FFFFF").ToList();
        //                                                                       //if (lstPack != null && lstPack.Count() > 0)
        //        {
        //            for (int i = 1; i < lstPackType.Count; i++)
        //            {
        //                string childlvl = lstPackType[i - 1];
        //                string parentlvl = lstPackType[i];
        //                string GTINChild = jobdetail.Where(x => x.JD_Deckcode == childlvl).Select(x => x.JD_GTIN).FirstOrDefault();
        //                string GTINParent = jobdetail.Where(x => x.JD_Deckcode == parentlvl).Select(x => x.JD_GTIN).FirstOrDefault();

        //                lstPack = db.PackagingDetails.Where(s => s.JobID == vm.JID).ToList();

        //                string codePrefix = epcisConf.GetEPCSGTIN(GTINChild, string.Empty, customer.CompanyCode.Length);
        //                string parentPrefix = epcisConf.GetEPCSGTIN(GTINParent, string.Empty, customer.CompanyCode.Length);

        //                var thisDeckAllCode = lstPack.Where(x => x.PackageTypeCode == childlvl).ToList();
        //                var thisDeckParent = thisDeckAllCode.Where(s => s.NextLevelCode != "FFFFF").Select(s => s.NextLevelCode).Distinct().ToList();

        //                for (int prnt = 0; prnt < thisDeckParent.Count; prnt++)
        //                {
        //                    string parentcode = Convert.ToString(thisDeckParent[prnt]).Trim();

        //                    evtTime = lstPack.FirstOrDefault(s => s.Code == parentcode.ToString()).LastUpdatedDate;

        //                    var lstChild = lstPack.Where(x => x.NextLevelCode == parentcode.Trim())
        //                                          //.Select(s => codePrefix + s.Code).ToList();
        //                                          .Select(s => (s.Code.Length == 18) ? s.Code.ToString() : codePrefix + s.Code).ToList();

        //                    parentcode = (parentcode.Length == 18) ? epcisConf.GetEPCSSCC(parentcode) : parentPrefix + parentcode;

        //                    aggregationEvent += xml.AggregationEvent(parentcode, lstChild, evtTime);
        //                }
        //            }
        //        }


        //        //if (dsAgg != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        //{
        //        //    var AllPacks = dsAgg.Tables[0].AsEnumerable();

        //        //    for (int i = 1; i < lstPackType.Count; i++)
        //        //    {
        //        //        string childlvl = lstPackType[i - 1];
        //        //        string parentlvl = lstPackType[i];
        //        //        string GTINChild = jobdetail.Where(x => x.JD_Deckcode == childlvl).Select(x => x.JD_GTIN).FirstOrDefault();
        //        //        string GTINParent = jobdetail.Where(x => x.JD_Deckcode == parentlvl).Select(x => x.JD_GTIN).FirstOrDefault();

        //        //        var thisDeckAllCode = AllPacks.Where(x => x.Field<string>("PackageTypeCode") == lstPackType[i - 1]).ToList();
        //        //        var thisDeckParent = thisDeckAllCode.Select(s => new { parent = s.Field<string>("NextLevelCode") }).Distinct().ToList();

        //        //        for (int prnt = 0; prnt < thisDeckParent.Count; prnt++)
        //        //        {
        //        //            string parentcode = Convert.ToString(thisDeckParent[prnt]).Trim();

        //        //            var lstChild = dsAgg.Tables[0].AsEnumerable().Where(x => x.Field<string>("NextLevelCode").Trim() == parentcode.Trim()).ToList();

        //        //            //var lstChildCd = AllPacks.AsEnumerable().ToList()[0];

        //        //            aggregationEvent += xml.AggregationEvent(parentcode, lstChild, evtTime);
        //        //        }
        //        //    }



        //        //    var parents = ds.Tables[0].AsEnumerable().Select(s => new { parent = s.Field<string>("NextLevelCode"), }).Distinct().ToList();


        //        //    for (int i = 1; i < lstPackType.Count; i++)
        //        //    {
        //        //        //var childs = from row in ds.Tables[0].AsEnumerable() where row.Field<string>("NextLevelCode") == item.ToString() select row;

        //        //        //foreach (DataRow pack in AllPacks.Where(x => x.Field<string>("PackageTypeCode") == lstPackType[i]).ToList())
        //        //        //{
        //        //        //    code = pack["Code"].ToString();
        //        //        //    sscc = pack["SSCC"].ToString();
        //        //        //    List<string> allChild = AllPacks.Where(x => x.Field<string>("NextLevelCode") == code).Select(x => (x.Field<string>("Code").Length == 18 ? epcisConf.GetEPCSSCC(x.Field<string>("Code")) : epcisConf.GetEPCSGTIN(GTINChild, x.Field<string>("Code"), customer.CompanyCode.Length))).ToList();
        //        //        //    if (!string.IsNullOrEmpty(sscc))
        //        //        //    {
        //        //        //        code = sscc;
        //        //        //    }

        //        //        //    events += Xeg.GenerateAggregationEventNew("AggregationEvent", (code.Length == 18 ? epcisConf.GetEPCSSCC(code) : epcisConf.GetEPCSGTIN(GTINParent, code, customer.CompanyCode.Length)), allChild, pack.Field<DateTime>("LastUpdatedDate"), SenderGLN);
        //        //        //}

        //        //    }

        //        //    //foreach (var item in parents)
        //        //    //{
        //        //    //    string codeWithEPC = epcisConf.GetEPCSGTIN(.JD_GTIN, string.Empty, customer.CompanyCode.Length);

        //        //    //    var childs = from row in ds.Tables[0].AsEnumerable()
        //        //    //                 where row.Field<string>("NextLevelCode") == item.ToString()
        //        //    //                 select row;

        //        //    //    //string childGTIN = jobdetail.Where(s => s.JD_GTIN == childs.ToList()[0].ToString())



        //        //    //    aggregationEvent += xml.AggregationEvent(item.parent, childs, evtTime);
        //        //    //}
        //        //}


        //        #endregion
        //    }

        //    string MyPML = "";
        //    MyPML = Header + Commission + DeCommision + SSCCCommision + aggregationEvent + EndDocument;
        //    var byts = Encoding.ASCII.GetBytes(MyPML);
        //    string Filename = job.BatchNo + "_v" + vm.TypeVersion + "_" + DateTime.Now + ".xml";

        //    return File(byts, ".xml", Filename);
        //}


        public ActionResult GenerateTraceKey(TraceKeyViewModel vm)
        {
            int JID = Convert.ToInt32(vm.JID);
            List<EpcisEventDetails> EPICSEvents = new List<EpcisEventDetails>();
            EPCIS_XMLs_Generation.EPCISConfig epcisConf = new EPCIS_XMLs_Generation.EPCISConfig();
            var job = db.Job.Where(x => x.JID == JID).FirstOrDefault();
            var jobdetail = db.JobDetails.Where(x => x.JD_JobID == JID);
            var sendr = db.Settings.FirstOrDefault();
            var customer = db.M_Customer.Where(x => x.Id == vm.Cid).FirstOrDefault();
            var lineLocationData = db.LineLocation.Where(l => l.ID == job.LineCode).FirstOrDefault();
            EPICSEvents = db.EpcisEventDetails.Where(x => x.JobId == JID).ToList();
            string sernderId = config.GetEPCGLN(lineLocationData.ReadGLN, lineLocationData.GLNExtension, sendr.CompanyCode.Length);
            string receiverId = config.GetEPCGLN(customer.BizLocGLN, customer.BizLocGLN_Ext, customer.CompanyCode.Length);
            XmlGeneration xml = new XmlGeneration();
            string Header = "";
            Header = xml.GenerateEPCISHeader(job, sernderId, receiverId, vm.TypeVersion, vm.ProductionDocument, jobdetail.FirstOrDefault(s => s.JD_Deckcode == "MOC").JD_GTIN);
            string EndDocument = "";
            EndDocument = xml.EndDocument();
            DbHelper m_dbhelper = new DbHelper();
            DataTable dtPacks = new DataTable();
            //string qryPacks = "Select Code,LastUpdatedDate from PackagingDetails where JobID=" + JID + " and IsRejected=0 and (IsDecomission=0 or IsDecomission is null) and (IsManualUpdated=0 or IsManualUpdated is null) and IsUsed=1 and PackageTypeCode='MOC'";// and NextLevelCode!='FFFFF'
            string qryPacks = "Select Code,LastUpdatedDate from PackagingDetails where JobID=" + JID + " and IsRejected=0 and (IsDecomission=0 or IsDecomission is null) and IsUsed=1 and PackageTypeCode='MOC'";// and NextLevelCode!='FFFFF'
            DataSet ds = m_dbhelper.GetDataSet(qryPacks);
            dtPacks = ds.Tables[0];
            DateTime evtTime = Convert.ToDateTime(dtPacks.Rows[0]["LastUpdatedDate"]);
            evtTime = Convert.ToDateTime(job.VerifiedDate).AddMinutes(20).AddSeconds(20).AddMilliseconds(20);
            string formatedCode = epcisConf.GetEPCSGTIN(jobdetail.FirstOrDefault(s => s.JD_Deckcode == "MOC").JD_GTIN, string.Empty, customer.CompanyCode.Length);
            var packByDeck_Code = dtPacks.AsEnumerable().ToList();
            //string Commission = "";
            //foreach (var EpcisEve in EPICSEvents)
            //{
            //    Events += xml.convertToEvent(EpcisEve, sernderId);
            //}
            string Filename = @"D:\Exception\" + job.BatchNo + "_v" + vm.TypeVersion + "_" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + ".xml";
            DataLayer.ExceptionHandler.ExceptionLogger.LogError(Filename);

            using (StreamWriter sw = new StreamWriter(Filename, true))
            {
                sw.WriteLine(Header);
            }
            List<string> lstFormatedCodes = new List<string>();
            if (packByDeck_Code != null && packByDeck_Code.Count > 0)
            {
                lstFormatedCodes = packByDeck_Code.Select(x => formatedCode + x.Field<string>("Code")).ToList();
            }

            xml.CommissionEvent(Filename, job, lstFormatedCodes, evtTime);

            var lvls = ProductPackageHelper.getAllDeck(job.JID.ToString());
            List<string> lstPackType = ProductPackageHelper.sorttheLevels(lvls);

            #region DeCommission 

            string mocDeck = lstPackType[0];
            string GTINmoc = jobdetail.Where(x => x.JD_Deckcode == mocDeck).Select(x => x.JD_GTIN).FirstOrDefault();
            string mocPrefix = epcisConf.GetEPCSGTIN(GTINmoc, string.Empty, customer.CompanyCode.Length);

            var lstBad = db.PackagingDetails.Where(s => s.JobID == vm.JID && s.PackageTypeCode == mocDeck && (s.IsRejected == true || s.IsUsed == false || s.IsDecomission == true)).ToList();
            evtTime = evtTime.AddMinutes(20).AddSeconds(20).AddMilliseconds(20);
            string DeCommision = string.Empty;
            if (lstBad != null && lstBad.Count > 0)
            {
                //DeCommision = xml.DecommissioneEvent(Filename, job, lstBad.Select(s => mocPrefix + s.Code.ToString()).ToList(), lstBad.ToList()[0].LastUpdatedDate);
                DeCommision = xml.DecommissioneEvent(Filename, job, lstBad.Select(s => mocPrefix + s.Code.ToString()).ToList(), evtTime);
            }

            #endregion

            string aggregationEvent = string.Empty;
            string SSCCCommision = string.Empty;
            if (lstPackType.Count > 1)
            {
                #region Commission SSCC
                string firstDeck = lstPackType[0];
                
                foreach (string PackType in lstPackType)
                {
                    if (PackType != firstDeck)
                    {
                        var lstSSCC = db.PackagingDetails.Where(s => s.JobID == vm.JID &&
                                                             s.PackageTypeCode == PackType &&
                                                             s.IsRejected == false && s.IsUsed == true && s.IsDecomission == false).OrderBy(x => x.LastUpdatedDate).ToList();

                        var ssccs = lstSSCC.Select(s => string.IsNullOrEmpty(s.SSCC) ? s.Code : s.SSCC).ToList();
                        
                        evtTime = evtTime.AddMinutes(2).AddSeconds(2).AddMilliseconds(2);

                        //SSCCCommision += 
                        // xml.SSCCCommsionEvent(Filename, ssccs, lstSSCC.ToList()[0].LastUpdatedDate);
                        xml.SSCCCommsionEvent(Filename, ssccs, evtTime);
                    }
                }



                #endregion

                #region Aggregation
                //string qryAgg = " SELECT PackageTypeCode, Code, NextLevelCode, SSCC FROM PackagingDetails where JobID = " + vm.JID + " AND NextLevelCode IS NOT NULL AND " +
                //                " NextLevelCode<>'FFFFF' ORDER BY NextLevelCode, Code";

                //DataSet dsAgg = m_dbhelper.GetDataSet(qryAgg);


                List<PackagingDetails> lstPack = new List<PackagingDetails>(); //db.PackagingDetails.Where(s => s.JobID == vm.JID && s.NextLevelCode != "FFFFF").ToList();
                                                                               //if (lstPack != null && lstPack.Count() > 0)
                {
                    for (int i = 1; i < lstPackType.Count; i++)
                    {
                        string childlvl = lstPackType[i - 1];
                        string parentlvl = lstPackType[i];
                        string GTINChild = jobdetail.Where(x => x.JD_Deckcode == childlvl).Select(x => x.JD_GTIN).FirstOrDefault();
                        string GTINParent = jobdetail.Where(x => x.JD_Deckcode == parentlvl).Select(x => x.JD_GTIN).FirstOrDefault();

                        lstPack = db.PackagingDetails.Where(s => s.JobID == vm.JID).ToList();

                        string codePrefix = epcisConf.GetEPCSGTIN(GTINChild, string.Empty, customer.CompanyCode.Length);
                        string parentPrefix = epcisConf.GetEPCSGTIN(GTINParent, string.Empty, customer.CompanyCode.Length);

                        var thisDeckAllCode = lstPack.Where(x => x.PackageTypeCode == childlvl).ToList();
                        var thisDeckParent = thisDeckAllCode.Where(s => s.NextLevelCode != "FFFFF").Select(s => s.NextLevelCode).Distinct().ToList();

                        for (int prnt = 0; prnt < thisDeckParent.Count; prnt++)
                        {
                            string parentcode = Convert.ToString(thisDeckParent[prnt]).Trim();

                            //evtTime = lstPack.FirstOrDefault(s => s.Code == parentcode.ToString()).LastUpdatedDate;
                            evtTime = evtTime.AddMinutes(2).AddSeconds(2).AddMilliseconds(2);

                            var lstChild = lstPack.Where(x => x.NextLevelCode == parentcode.Trim())
                                                  //.Select(s => codePrefix + s.Code).ToList();
                                                  .Select(s => (s.Code.Length == 18) ? s.Code.ToString() : codePrefix + s.Code).ToList();

                            parentcode = (parentcode.Length == 18) ? epcisConf.GetEPCSSCC(parentcode) : parentPrefix + parentcode;

                            //aggregationEvent += 
                            xml.AggregationEvent(Filename, parentcode, lstChild, evtTime);
                        }
                    }
                }


                //if (dsAgg != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    var AllPacks = dsAgg.Tables[0].AsEnumerable();

                //    for (int i = 1; i < lstPackType.Count; i++)
                //    {
                //        string childlvl = lstPackType[i - 1];
                //        string parentlvl = lstPackType[i];
                //        string GTINChild = jobdetail.Where(x => x.JD_Deckcode == childlvl).Select(x => x.JD_GTIN).FirstOrDefault();
                //        string GTINParent = jobdetail.Where(x => x.JD_Deckcode == parentlvl).Select(x => x.JD_GTIN).FirstOrDefault();

                //        var thisDeckAllCode = AllPacks.Where(x => x.Field<string>("PackageTypeCode") == lstPackType[i - 1]).ToList();
                //        var thisDeckParent = thisDeckAllCode.Select(s => new { parent = s.Field<string>("NextLevelCode") }).Distinct().ToList();

                //        for (int prnt = 0; prnt < thisDeckParent.Count; prnt++)
                //        {
                //            string parentcode = Convert.ToString(thisDeckParent[prnt]).Trim();

                //            var lstChild = dsAgg.Tables[0].AsEnumerable().Where(x => x.Field<string>("NextLevelCode").Trim() == parentcode.Trim()).ToList();

                //            //var lstChildCd = AllPacks.AsEnumerable().ToList()[0];

                //            aggregationEvent += xml.AggregationEvent(parentcode, lstChild, evtTime);
                //        }
                //    }



                //    var parents = ds.Tables[0].AsEnumerable().Select(s => new { parent = s.Field<string>("NextLevelCode"), }).Distinct().ToList();


                //    for (int i = 1; i < lstPackType.Count; i++)
                //    {
                //        //var childs = from row in ds.Tables[0].AsEnumerable() where row.Field<string>("NextLevelCode") == item.ToString() select row;

                //        //foreach (DataRow pack in AllPacks.Where(x => x.Field<string>("PackageTypeCode") == lstPackType[i]).ToList())
                //        //{
                //        //    code = pack["Code"].ToString();
                //        //    sscc = pack["SSCC"].ToString();
                //        //    List<string> allChild = AllPacks.Where(x => x.Field<string>("NextLevelCode") == code).Select(x => (x.Field<string>("Code").Length == 18 ? epcisConf.GetEPCSSCC(x.Field<string>("Code")) : epcisConf.GetEPCSGTIN(GTINChild, x.Field<string>("Code"), customer.CompanyCode.Length))).ToList();
                //        //    if (!string.IsNullOrEmpty(sscc))
                //        //    {
                //        //        code = sscc;
                //        //    }

                //        //    events += Xeg.GenerateAggregationEventNew("AggregationEvent", (code.Length == 18 ? epcisConf.GetEPCSSCC(code) : epcisConf.GetEPCSGTIN(GTINParent, code, customer.CompanyCode.Length)), allChild, pack.Field<DateTime>("LastUpdatedDate"), SenderGLN);
                //        //}

                //    }

                //    //foreach (var item in parents)
                //    //{
                //    //    string codeWithEPC = epcisConf.GetEPCSGTIN(.JD_GTIN, string.Empty, customer.CompanyCode.Length);

                //    //    var childs = from row in ds.Tables[0].AsEnumerable()
                //    //                 where row.Field<string>("NextLevelCode") == item.ToString()
                //    //                 select row;

                //    //    //string childGTIN = jobdetail.Where(s => s.JD_GTIN == childs.ToList()[0].ToString())



                //    //    aggregationEvent += xml.AggregationEvent(item.parent, childs, evtTime);
                //    //}
                //}


                #endregion
            }
            using (StreamWriter sw = new StreamWriter(Filename, true))
            {
                sw.WriteLine(EndDocument);
            }
            //string MyPML = "";
            //MyPML = Header + Commission + DeCommision + SSCCCommision + aggregationEvent + EndDocument;
            //var byts = Encoding.ASCII.GetBytes(MyPML);
            //return File(byts, ".xml", Filename);

            return File(Filename, ".xml", job.BatchNo + "_v" + vm.TypeVersion + "_" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + ".xml");
        }


        #region IMPORT SERIAL NUMBERS

        public string TKEY_USERNAME = string.Empty; //"vikrant.k@propixtech.com-1";
        public string TKEY_PASSWORD = string.Empty; //"Propix@2016";
        public string TKEY_REQUEST_URL = string.Empty; //https://pharmasandbox.tracekey.com/mytk/download/pool/request?gtin=04150077428897&quantity=2;
        public string TKEY_REQUEST_PROCESS_URL = string.Empty; //https://pharmasandbox.tracekey.com/mytk/download/pool/process?ticketId=81e84935-d680-4538-9e26-ec271de60466
        public string TKEY_REQUEST_DOWNLOAD_URL = string.Empty; //https://pharmasandbox.tracekey.com/mytk/download/pool/download?ticketId=c1bb3cb6-e2f4-4378-9416-c571709257ca

        public void BindImportSerialDropdown()
        {
            var provider = db.M_Providers.FirstOrDefault(c => c.Code == UIDCustomType.TKEY.ToString());

            if (provider != null)
            {
                ViewBag.Products = db.PackagingAsso.Where(p => p.ProviderId == provider.Id && p.IsActive && p.VerifyProd).OrderBy(o => o.Name).ToList();
            }
            else
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTrackeyAdentProviderNtConfigred;
            }

            var gtin = db.PackagingAssoDetails.Take(0).Select(s => new { PackageTypeCode = "Select", GTIN = "" }).ToList();
            ViewBag.GTIN = gtin;
            ViewBag.Customer = db.M_Customer.Where(p => p.ProviderId == provider.Id).ToList();

            TKEY_REQUEST_URL = Convert.ToString(Utilities.getAppSettings("TKEY_REQUEST_URL"));
            TKEY_REQUEST_PROCESS_URL = Convert.ToString(Utilities.getAppSettings("TKEY_REQUEST_PROCESS_URL"));
            TKEY_REQUEST_DOWNLOAD_URL = Convert.ToString(Utilities.getAppSettings("TKEY_REQUEST_DOWNLOAD_URL"));

            TKEY_USERNAME = Convert.ToString(Utilities.getAppSettings("TKEY_USERNAME"));
            TKEY_PASSWORD = Convert.ToString(Utilities.getAppSettings("TKEY_PASSWORD"));
        }

        [HttpPost]
        public ActionResult getProductData(decimal PAID, bool IsPPN)
        {
            try
            {
                var data = db.PackagingAsso.Find(PAID);
                var packagingAssoDetails = db.PackagingAssoDetails.Where(m => m.PAID == PAID).OrderBy(x => x.Id).ToList();
                int tertiaryDeck = 0;
                if (packagingAssoDetails.Count() > 1)
                {
                    tertiaryDeck = packagingAssoDetails.Max(x => x.Id);
                }

                var dataPackagingAssoDetails = packagingAssoDetails.Where(x => x.Id != tertiaryDeck);

                object[] obj = { data, dataPackagingAssoDetails };
                return Json(obj);
            }
            catch (Exception ex)
            {
                DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);

                return View("HError", ex);
            }
        }

        public List<ExecutionStatus> lstExecutionStatus = new List<ExecutionStatus>();
        public class ExecutionStatus
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
        }

        public ActionResult ImportSerialNumber()
        {
            BindImportSerialDropdown();
            return View();
        }

        [HttpPost]
        public ActionResult ImportSerialNumber(TraceKeyImportUIDViewModel model)
        {
            BindImportSerialDropdown();

            if (model.PAID <= 0)
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.ImporterTacelinkSelectProName;
            }
            else if (string.IsNullOrEmpty(model.GTIN))
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TRacekeyImportselectGTIN;
            }
            else if (model.Quantity <= 0)
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTrackeyEnterValidQty;
            }
            else
            {
                if (model.Customer <= 0)
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.ImporterCustomerUID;
                }
                var provider = db.M_Providers.FirstOrDefault(s => s.Code == UIDCustomType.TKEY.ToString());
                if (provider == null)
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTrackeyProviderNtForAdents;
                }
                // model.GTIN = "04150077428897";

                REQUEST_SERIAL_NUMBERS(model.GTIN, model.Quantity, provider, model.Customer);
            }
            // model = new TraceKeyImportUIDViewModel();
            ModelState.Clear();
            return RedirectToAction("TraceKeyList");//, openRequest);
            //return View();
        }


        public bool REQUEST_SERIAL_NUMBERS(string gtin, int Quantity, Models.Providers.M_Providers provider, int customer)
        {
            lstExecutionStatus.Clear();

            try
            {
                M_TkeySerialRequest obj = null;//db.M_TkeySerialRequest.FirstOrDefault(x => x.GTIN == gtin && x.Status == (int)TKEY_SERIAL_REQUEST_STATUS.REQUEST_ADDED);

                #region SEND REQUEST
                var requst_url = new Uri(TKEY_REQUEST_URL.Replace("{gtin}", gtin).Replace("{quantity}", Quantity.ToString()));
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    using (var client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential(TKEY_USERNAME, TKEY_PASSWORD);
                        var response = client.DownloadData(requst_url);

                        obj = new M_TkeySerialRequest();

                        System.IO.MemoryStream ms = new System.IO.MemoryStream(response);
                        XmlDocument doc = new XmlDocument();
                        doc.Load(ms);
                        if (System.IO.Directory.Exists(@"D:\Exception\"))
                        {
                            doc.Save(@"D:\Exception\TKEY_REQ_" + obj.GTIN + "_" + obj.Quantity + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + ".xml");
                        }

                        obj.GTIN = gtin;
                        obj.Quantity = Quantity;

                        obj.docVersion = doc.GetElementsByTagName("docVersion").Item(0).InnerText;
                        obj.docType = doc.GetElementsByTagName("docType").Item(0).InnerText;
                        obj.CreatedOn = Convert.ToDateTime(doc.GetElementsByTagName("creationDateAndTime").Item(0).InnerText);
                        obj.TicketId = doc.GetElementsByTagName("TicketId").Item(0).InnerText;
                        obj.ResponseStatus = doc.GetElementsByTagName("Status").Item(0).InnerText;
                        obj.Message = doc.GetElementsByTagName("Message").Item(0).InnerText;
                        obj.LastUpdatedDate = DateTime.Now;
                        obj.docIdentifier = doc.GetElementsByTagName("docIdentifier").Item(0).InnerText;
                        obj.Status = (int)TKEY_SERIAL_REQUEST_STATUS.REQUEST_ADDED;
                        obj.CustomerId = customer;
                        obj.ProviderId = provider.Id;
                        obj.LastUpdatedDate = DateTime.Now;

                        db.M_TkeySerialRequest.Add(obj);
                        db.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);

                    TempData["Success"] = TnT.LangResource.GlobalRes.TRacekeyListTempstatuschkstock;
                }

                #endregion

                //System.Threading.Thread.Sleep(5000);

                #region PROCESS REQUEST

                //var process_request_url = new Uri(TKEY_REQUEST_PROCESS_URL.Replace("{ticketId}", obj.TicketId));

                //using (var client = new WebClient())
                //{
                //    client.Credentials = new NetworkCredential(TKEY_USERNAME, TKEY_PASSWORD);

                //    var response = client.DownloadData(process_request_url);

                //    System.IO.MemoryStream ms = new System.IO.MemoryStream(response);
                //    XmlDocument doc = new XmlDocument();
                //    doc.Load(ms);


                //    obj = new M_TkeySerialRequest();

                //    obj.TicketId = doc.GetElementsByTagName("TicketId").Item(0).InnerText;
                //    obj.Message = doc.GetElementsByTagName("Message").Item(0).InnerText;
                //    obj.ResponseStatus = doc.GetElementsByTagName("Status").Item(0).InnerText;
                //    obj.Status = (int)TKEY_SERIAL_REQUEST_STATUS.REQUEST_PROCESS;

                //    string connectionString = Utilities.getConnectionString("DefaultConnection");

                //    string qry = @" UPDATE M_TkeySerialRequest SET ResponseStatus= '" + obj.ResponseStatus + "', " + " Message ='" + obj.Message + "'" +
                //                  ", Status = " + obj.Status + ", LastUpdatedDate =GETDATE() WHERE TicketId ='" + obj.TicketId + "'";

                //    int rowAffected = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, qry);
                //}

                #endregion

                //System.Threading.Thread.Sleep(5000);

                #region DOWNLOAD REQUEST

                //var download_request_url = new Uri(TKEY_REQUEST_DOWNLOAD_URL.Replace("{ticketId}", obj.TicketId));

                //using (var client = new WebClient())
                //{
                //    client.Credentials = new NetworkCredential(TKEY_USERNAME, TKEY_PASSWORD);

                //    var response = client.DownloadData(download_request_url);

                //    System.IO.MemoryStream ms = new System.IO.MemoryStream(response);
                //    XmlDocument doc = new XmlDocument();
                //    doc.Load(ms);

                //    #region Add M_TracelinkRequest
                //    Models.TraceLinkImporter.M_TracelinkRequest m_Tracelink = new Models.TraceLinkImporter.M_TracelinkRequest();

                //    m_Tracelink.CustomerId = customer.Id;
                //    m_Tracelink.Quatity = Quantity;
                //    m_Tracelink.GTIN = gtin;
                //    m_Tracelink.RequestedOn = DateTime.Now;
                //    m_Tracelink.IsDeleted = false;
                //    m_Tracelink.Threshold = 0;
                //    m_Tracelink.ProviderId = provider.Id;
                //    m_Tracelink.SrnoType = "GTIN";

                //    db.M_TracelinkRequest.Add(m_Tracelink);
                //    db.SaveChanges();

                //    #endregion

                //    var TicketId = doc.GetElementsByTagName("TicketId").Item(0).InnerText;

                //    var serialResponse = doc.GetElementsByTagName("SerialNumberList")[0].ChildNodes;

                //    DataTable dt = new DataTable();
                //    dt.Columns.Add("TLRequestId", typeof(int));
                //    dt.Columns.Add("SerialNo", typeof(string));
                //    dt.Columns.Add("IsUsed", typeof(bool));
                //    dt.Columns.Add("GTIN", typeof(string));
                //    dt.Columns.Add("Type", typeof(string));

                //    foreach (XmlNode item in serialResponse)
                //    {
                //        string serialNo = item.InnerText.Substring(item.InnerText.LastIndexOf('.') + 1);

                //        dt.Rows.Add(new object[] { m_Tracelink.Id, serialNo, false, gtin, "GTIN" });
                //    }
                //    BulkDataHelper bulkDataHelper = new BulkDataHelper();
                //    bool IsUpdated = bulkDataHelper.InsertTracelinkUIDIdenties(dt);

                //    if (IsUpdated)
                //    {
                //        obj = new M_TkeySerialRequest();

                //        obj.Message = doc.GetElementsByTagName("Message").Item(0).InnerText;
                //        obj.ResponseStatus = doc.GetElementsByTagName("Status").Item(0).InnerText;
                //        obj.Status = (int)TKEY_SERIAL_REQUEST_STATUS.REQUEST_DOWNLOADED;

                //        string connectionString = Utilities.getConnectionString("DefaultConnection");

                //        string qry = @" UPDATE M_TkeySerialRequest SET ResponseStatus= '" + obj.ResponseStatus + "', " + " Message ='" + obj.Message + "'" +
                //                      ", Status = " + obj.Status + ", LastUpdatedDate =GETDATE(), TLRequestId= " + m_Tracelink.Id + " WHERE TicketId ='" + TicketId + "'";

                //        int rowAffected = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, qry);
                //    }

                //    TempData["Success"] = dt.Rows.Count + " " + TnT.LangResource.GlobalRes.TempDataTrackeySerailNoImportedSucc;

                //    trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataTrackeySerailNoImportedSucc + " ", Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataTrackeySerailNoImportedSucc + " ", TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);

                //}

                #endregion

                return true;
            }
            catch (System.Exception ex)
            {
                DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);

                lstExecutionStatus.Add(new ExecutionStatus
                {
                    IsSuccess = false,
                    Message = "ERROR GET_PRODUCT()" + ex.Message
                });

                TempData["Success"] = "ERROR : " + ex.Message;

                return false;
            }
        }

        #endregion
    }

    //public class Serializer
    //{
    //    public T Deserialize<T>(string input) where T : class
    //    {
    //        System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

    //        using (StringReader sr = new StringReader(input))
    //        {
    //            return (T)ser.Deserialize(sr);
    //        }
    //    }

    //    public string Serialize<T>(T ObjectToSerialize)
    //    {
    //        XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

    //        using (StringWriter textWriter = new StringWriter())
    //        {
    //            xmlSerializer.Serialize(textWriter, ObjectToSerialize);
    //            return textWriter.ToString();
    //        }
    //    }
    //}
}
//CustomerId , ProviderId ADDED IN M_TkeySerialRequest TABLE