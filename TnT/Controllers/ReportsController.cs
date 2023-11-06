using TnT.DataLayer;
using TnT.DataLayer.Reports;
using TnT.Models.Reports.Jobs;
using TnT.Models.Reports.Products;
using REDTR.DB.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.Models;
using TnT.Models.Code;
using TnT.Models.Job;
using TnT.Models.Product;
using TnT.Models.Reports;
using Westwind.Web.Mvc;
using TnT.DataLayer.Security;
using REDTR.UTILS.SystemIntegrity;
using REDTR.UTILS;
using TnT.DataLayer.Trailings;
using Rotativa;
using System.Net.Http;
using System.Text;
//using iTextSharp.text.pdf;
//using iTextSharp.text;
using System.IO;
using System.Web.Script.Serialization;
using System.Configuration;
using Rotativa.Options;
using TnT.Models.TraceLinkImporter;
using REDTR.HELPER;
//using iTextSharp.text.html.simpleparser;

namespace TnT.Controllers
{

    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class ReportsController : BaseController
    {
        string DataRPT;
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        //   string ReportPfdPassword = Utilities.getAppSettings("ReportPdfPassword");
        string ReportPfdPassword = DateTime.Now.ToString("hhmmss");

        #region Jobs Reports

        // GET: Job Reports
        public ActionResult Jobs()
        {
            bindData();
            return View();
        }
        public ActionResult Trails()
        {
            Bind();
            return View();
        }

        public ActionResult UserStatus()
        {
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataReportViewUserReport, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataReportViewUserReport, TnT.LangResource.GlobalRes.TrailReportActivity);
            return View();
        }

        #region Rpt Dava Status
        public ActionResult DavaStatus()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RptDavaStatus(string rpttype, string status)
        {
            if (rpttype != "")
            {
                ReportHelper rpthlr = new ReportHelper();
                var data = rpthlr.getDavaStatus(rpttype, status, User.FirstName);
                trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptReqtoViewDta, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptReqtoViewDta, TnT.LangResource.GlobalRes.TrailReportActivity);
                return Json(data);
            }
            else
            {

                return Json(TnT.LangResource.GlobalRes.ShwMsgRptDavaStatusSelectReportType);
            }
        }

        public FileResult genePDFDavaStatus(string rpttype, string status)
        {

            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.getDavaStatusPdf(rpttype, status, User.FirstName);
            bindCss();
            string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.RptJobsDavaReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptDavaDetails", data)
            {
                CustomSwitches = footer,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedDavaRpt, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedDavaRpt, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "DavaDetailsReport_" + ReportPfdPassword + ".pdf");
        }
        #endregion

        private void Bind()
        {
            try
            {
                ViewBag.LineLocations = db.LineLocation.Where(x => x.IsActive == true);
                ViewBag.UserName = db.Users.Where(x => x.Active == true);
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region retrive Job : chart Data

        [HttpPost]
        public ActionResult getJobChartDetails(string JID)
        {
            try
            {

                decimal j = Convert.ToDecimal(JID);
                var jb = db.Job.Find(j);

                REDTR.HELPER.DbHelper ObjHelper = new REDTR.HELPER.DbHelper();
                var FirstDeck = ProductPackageHelper.getTopDeck(JID);
                DataSet DS = ObjHelper.DBManager.JobBLL.GetReportDataSet(JobBLL.ReportOp.ForJobdeckProcess, JID, FirstDeck, null, null, null, false, -1);

                if (DS.Tables[0].Rows.Count > 0)
                {
                    var badCount = Convert.ToInt32(DS.Tables[0].Rows[0][3]) + jb.NoReadCount;
                    var goodCount = DS.Tables[0].Rows[0][2].ToString();
                    var decommisionedCount = DS.Tables[0].Rows[0][4].ToString();
                    var notVerified = DS.Tables[0].Rows[0][5].ToString();

                    //object[] jbdt = { badCount, goodCount, decommisionedCount, notVerified };

                    object[] jbdt = { goodCount, badCount, decommisionedCount };
                    var product = db.PackagingAsso.Find(jb.PAID);
                    var gtin = ProductPackageHelper.getGTIN(jb.PAID, FirstDeck);
                    var jobType = db.JOBTypes.Where(x => x.TID == jb.TID).FirstOrDefault();
                    object[] response = { jbdt, gtin, product, jb, jobType.Job_Type };
                    //trail.AddTrail(User.FirstName + " requested to view job Details of " + jb.JobName, Convert.ToInt32(User.ID));
                    return Json(response);
                }
                else
                {
                    return Json(TnT.LangResource.GlobalRes.toastrLblLytDsgNoData);

                }
            }
            catch (Exception ex)
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptErrorInval);
            }
        }

        #endregion

        #region retrive Job : Report Popup Data
        #region User Report
        [HttpPost]
        public ActionResult RptUserDetails(string Status)
        {
            ReportHelper helper = new ReportHelper();
            var data = helper.getUserDetail(Status, User.FirstName);
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailDataReportrequestedtoview + " " + Status + " " + TnT.LangResource.GlobalRes.TrailReportUserReport, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailDataReportrequestedtoview + " " + Status + " " + TnT.LangResource.GlobalRes.TrailReportUserReport, TnT.LangResource.GlobalRes.TrailReportActivity);
            return Json(data);
        }
        public FileResult genePDFUserDetails(string Status)
        {

            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.getUserDetailPdf(Status, User.FirstName);
            bindCss();
            string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.TempDataRptUsrDetailReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptUserDetails", data)
            {
                CustomSwitches = footer,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedUserDeatilRpt, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedUserDeatilRpt, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "UserDetailsReport_" + ReportPfdPassword + ".pdf");
        }
        #endregion

        #region Available SeialCount

        public ActionResult SerailNoCount()
        {
            ReportHelper helper = new ReportHelper();
            var data = helper.getSerailCount(User.FirstName);
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewTracelinkSerailNumber, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewTracelinkSerailNumber, TnT.LangResource.GlobalRes.TrailReportActivity);
            return Json(data);
        }

        public FileResult genePDFSerailNoCount()
        {

            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.getSerailCountPdf(User.FirstName);
            bindCss();
            string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.TempDataRptAvalblTracSerialNorpt + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptAvailableSerialNo", data)
            {
                CustomSwitches = footer,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedTracelinkSerialNoRpt, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedTracelinkSerialNoRpt, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "AvailableSerialNoReport_" + ReportPfdPassword + ".pdf");
        }
        #endregion

        #region Tracelink Request

        public ActionResult TlinkRequest()
        {
            ReportHelper helper = new ReportHelper();
            var data = helper.GetTlinkRequest(User.FirstName);
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewTracelinkRequest, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewTracelinkRequest, TnT.LangResource.GlobalRes.TrailReportActivity);
            return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
        }

        public FileResult genePDFTlinkRequest()
        {

            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.GetTlinkRequestPdf(User.FirstName);
            bindCss();
            string footer = "--footer-left \" " + TnT.LangResource.GlobalRes.cmnMenuItemTracelinkServRequestReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptTlinkRequest", data)
            {
                CustomSwitches = footer,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedTracelinkReqRpt, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedTracelinkReqRpt, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "TlinkRequestReport_" + ReportPfdPassword + ".pdf");
        }
        #endregion
        #region Unused Serail Number

        public ActionResult UnusedSrNo()
        {
            ReportHelper helper = new ReportHelper();
            var data = helper.GetUnusedSrno(User.FirstName);
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            var result = new ContentResult();
            result.Content = serializer.Serialize(data);
            result.ContentType = "application/json";
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewTracelinkUnusedSerialNumber, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewTracelinkUnusedSerialNumber, TnT.LangResource.GlobalRes.TrailReportActivity);
            return result;


        }

        public FileResult genePDFUnusedSrNo()
        {

            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.GetUnusedSrnoPdf(User.FirstName);
            bindCss();
            string footer = "--footer-left \" " + TnT.LangResource.GlobalRes.cmnMenuItemTracelinkServUnusedSerialNumReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptUnusedSrNo", data)
            {
                CustomSwitches = footer,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedTracelinkUnusedserialNoReqRpt, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedTracelinkUnusedserialNoReqRpt, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "UnusedSrNoReport_" + ReportPfdPassword + ".pdf");
        }

        public ActionResult UnusedUidCount()
        {
            List<TKeyUnusedSrNoCount> vm = new List<TKeyUnusedSrNoCount>();

            DbHelper m_dbhelper = new DbHelper();
            string Query = "SELECT GTIN, COUNT(GTIN) CNT FROM [X_TracelinkUIDStore] WHERE ISUSED = 0 GROUP BY GTIN";
            DataSet ds = m_dbhelper.GetDataSet(Query);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string gtin = Convert.ToString(ds.Tables[0].Rows[i]["GTIN"]);
                    string productname = string.Empty;

                    var pad = db.PackagingAssoDetails.FirstOrDefault(s => s.GTIN == gtin);
                    if (pad != null && pad.PAID > 0)
                    {
                        var pa = db.PackagingAsso.FirstOrDefault(s => s.PAID == pad.PAID);

                        if (pa != null && !string.IsNullOrEmpty(pa.Name))
                        {
                            productname = pa.Name;
                        }
                    }
                    vm.Add(new TKeyUnusedSrNoCount { ProductName = productname, GTIN = Convert.ToString(ds.Tables[0].Rows[i]["GTIN"]), Count = Convert.ToInt32(ds.Tables[0].Rows[i]["CNT"]) });

                }
            }
            return View(vm);


            //List<string> lstGTINs = db.M_TkeySerialRequest.Select(x => x.GTIN).Distinct().ToList();
            //decimal PAID = 0;

            //foreach (string GTIN in lstGTINs)
            //{
            //    var prod = db.PackagingAssoDetails.FirstOrDefault(x => x.GTIN == GTIN);
            //    if (prod != null && prod.PAID > 0)
            //    {
            //        TKeyUnusedSrNoCount SrNoCnt = new TKeyUnusedSrNoCount();
            //        SrNoCnt.GTIN = GTIN;
            //        SrNoCnt.ProductName = db.PackagingAsso.FirstOrDefault(x => x.PAID == PAID).Name;
            //        SrNoCnt.Count = db.X_TracelinkUIDStore.Count(x => x.GTIN == GTIN && x.IsUsed == false);
            //        vm.Add(SrNoCnt);
            //    }
            //}

            return View(vm);
        }

        public ActionResult TKeyUnusedSrNoReport(string GTIN)
        {
            string RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;

            RptUnusedSrNo vm = new RptUnusedSrNo();
            vm.CompanyName = db.Settings.Select(s => s.CompanyName).FirstOrDefault();
            vm.Address = db.Settings.Select(s => s.Address).FirstOrDefault();
            vm.UserName = User.FirstName;
            DateTime rqDateTime;
            List<X_TracelinkUIDStore> lstSrNos = db.X_TracelinkUIDStore.Where(x => x.GTIN == GTIN && x.IsUsed == false).ToList();
            vm.UnSrNo = new List<UnusedSrno>();
            int validity = Convert.ToInt32(Utilities.getAppSettings("RussiaUIDValidity"));
            foreach (X_TracelinkUIDStore SrNo in lstSrNos)
            {
                UnusedSrno rp = new UnusedSrno();
                rp.GTIN = GTIN;
                rp.SrNo = SrNo.SerialNo;
                if (SrNo.Type == "CRPTO")
                {
                    rqDateTime = db.M_TracelinkRequest.Where(x => x.Id == SrNo.TLRequestId).First().RequestedOn;
                    rp.ValidFor = validity - (DateTime.Now - rqDateTime).Days;
                    rp.ValidFor = (rp.ValidFor > 0 ? rp.ValidFor : -1);
                    vm.ShowValidity = true;
                }
                vm.UnSrNo.Add(rp);
            }

            if (vm.UnSrNo.Count() > 0)
            {
                RptData = ViewRenderer.RenderPartialView("~/Views/Reports/RptUnusedSrNoTKey.cshtml", vm);
            }
            else
            {
                RptData = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            var result = new ContentResult();
            result.Content = serializer.Serialize(RptData);
            result.ContentType = "application/json";
            //trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewTracelinkUnusedSerialNumber, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewTracelinkUnusedSerialNumber, TnT.LangResource.GlobalRes.TrailReportActivity);
            return result;
        }

        public FileResult genePDFUnusedSrNoTKey(string GTIN)
        {

            byte[] bPDF = null;

            RptUnusedSrNo vm = new RptUnusedSrNo();
            vm.CompanyName = db.Settings.Select(s => s.CompanyName).FirstOrDefault();
            vm.Address = db.Settings.Select(s => s.Address).FirstOrDefault();
            vm.UserName = User.FirstName;
            DateTime rqDateTime;
            List<X_TracelinkUIDStore> lstSrNos = db.X_TracelinkUIDStore.Where(x => x.GTIN == GTIN && x.IsUsed == false).ToList();
            vm.UnSrNo = new List<UnusedSrno>();
            int validity = Convert.ToInt32(Utilities.getAppSettings("RussiaUIDValidity"));
            foreach (X_TracelinkUIDStore SrNo in lstSrNos)
            {
                UnusedSrno rp = new UnusedSrno();
                rp.GTIN = GTIN;
                rp.SrNo = SrNo.SerialNo;
                if (SrNo.Type == "CRPTO")
                {
                    rqDateTime = db.M_TracelinkRequest.Where(x => x.Id == SrNo.TLRequestId).First().RequestedOn;
                    rp.ValidFor = validity - (DateTime.Now - rqDateTime).Days;
                    rp.ValidFor = (rp.ValidFor > 0 ? rp.ValidFor : -1);
                    vm.ShowValidity = true;
                }
                vm.UnSrNo.Add(rp);
            }
            bindCss();
            string footer = "--footer-left \" " + TnT.LangResource.GlobalRes.cmnMenuItemTracelinkServUnusedSerialNumReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptUnusedSrNoTKey", vm)
            {
                CustomSwitches = footer,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedTracelinkUnusedserialNoReqRpt, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedTracelinkUnusedserialNoReqRpt, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "UnusedSrNoReport_" + ReportPfdPassword + ".pdf");
        }
        #endregion

        #region Product Details Report

        [HttpPost]
        public ActionResult ProductDetails(string k)
        {
            var vm = getProductDetailsRpt(User.FirstName);
            var data = ViewRenderer.RenderPartialView("~/Views/Reports/RptProductsDetails.cshtml", vm);
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportTracelinkviewProductDetails, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportTracelinkviewProductDetails, TnT.LangResource.GlobalRes.TrailReportActivity);
            return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
        }


        private ProductDetailsViewModel getProductDetailsRpt(string UserName)
        {
            ProductDetailsViewModel pv = new ProductDetailsViewModel();

            pv.CompanyName = db.Settings.Select(s => s.CompanyName).FirstOrDefault();
            pv.Address = db.Settings.Select(s => s.Address).FirstOrDefault();
            pv.UserName = UserName;
            pv.Products = db.PackagingAsso.ToList();
            pv.ProductDetails = db.PackagingAssoDetails.OrderBy(x => x.Id).ToList(); //.Where(p => p.Size != -1).ToList();

            return pv;
        }

        public FileResult genePDFProductDetails()
        {

            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = getProductDetailsRpt(User.FirstName);
            bindCss();
            string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.TempDataRpProductDetailRpt + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptProductsDetails", data)
            {
                CustomSwitches = footer,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedTracelinkProductRpt, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadedTracelinkProductRpt, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "ProductsDetailsReport_" + ReportPfdPassword + ".pdf");
        }
        #endregion

        #region Rpt Product Wise
        [HttpPost]
        public ActionResult getProductWiseReport(string FromDate, string ToDate)
        {
            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)))
            {
                DateTime frmDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);
                toDate = toDate.AddDays(1).AddTicks(-1);
                ReportHelper helper = new ReportHelper();
                var data = helper.getProductWise(frmDate, toDate, "NA", User.FirstName);
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportViewProductWise + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy"), Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportViewProductWise + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy"), TnT.LangResource.GlobalRes.TrailReportActivity);
                return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidjb);
            }
        }

        [HttpPost]
        public ActionResult getProductWiseReportAccLvl(string FromDate, string ToDate, string Level)
        {
            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)) && (!string.IsNullOrEmpty(Level)))
            {
                DateTime frmDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);
                toDate = toDate.AddDays(1).AddTicks(-1);
                ReportHelper helper = new ReportHelper();
                var data = helper.getProductWise(frmDate, toDate, Level, User.FirstName);
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewProductDetails + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy") + " For " + Level, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewProductDetails + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy") + " For " + Level, TnT.LangResource.GlobalRes.TrailReportActivity);
                return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidjb);
            }
        }

        public FileResult genePDFProductWise(string FromDate, string ToDate, string Level)
        {
            DateTime frmDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
            DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);
            toDate = toDate.AddDays(1).AddTicks(-1);
            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.getProductWisePDF(frmDate, toDate, Level, User.FirstName);
            bindCss();
            string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.RptProductWise + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptProductWise", data)
            {
                CustomSwitches = footer,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TempDataRptDownloadProdwisereport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TempDataRptDownloadProdwisereport, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "RptProductWise_" + ReportPfdPassword + ".pdf");
        }

        #endregion

        #region Rpt Summary

        [HttpPost]
        public ActionResult getSummaryReport(string FromDate, string ToDate)
        {
            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)))
            {
                DateTime frmDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);
                toDate = toDate.AddDays(1).AddTicks(-1);
                ReportHelper helper = new ReportHelper();
                var data = helper.getSummary(frmDate, toDate, "NA", User.FirstName);
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewSummary + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy"), Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewSummary + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy"), TnT.LangResource.GlobalRes.TrailReportActivity);
                return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidDates);
            }
        }

        [HttpPost]
        public ActionResult getSummaryReportAccLvl(string FromDate, string ToDate, string Level)
        {
            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)) && (!string.IsNullOrEmpty(Level)))
            {
                DateTime frmDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);
                toDate = toDate.AddDays(1).AddTicks(-1);
                ReportHelper helper = new ReportHelper();

                var data = helper.getSummary(frmDate, toDate, Level, User.FirstName);
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewSummary + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy") + ". For " + Level, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewSummary + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy") + ". For " + Level, TnT.LangResource.GlobalRes.TrailReportActivity);
                return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidjb);
            }
        }

        public FileResult genePDFSummary(string FromDate, string ToDate, string Level)
        {
            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)) && (!string.IsNullOrEmpty(Level)))
            {

                DateTime frmDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);
                toDate = toDate.AddDays(1).AddTicks(-1);
                byte[] bPDF = null;
                ReportHelper helper = new ReportHelper();
                var data = helper.getSummaryPDF(frmDate, toDate, Level, User.FirstName);
                bindCss();
                string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.TempDataRptSummReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
                var p = new ViewAsPdf("RptSummary", data)
                {
                    CustomSwitches = footer,
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = new Margins(3, 3, 20, 3)
                };
                bPDF = p.BuildPdf(ControllerContext);

                PDFEncryptor encryptor = new PDFEncryptor();
                var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
                trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TempDataRptDownloadSummaryReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TempDataRptDownloadSummaryReport, TnT.LangResource.GlobalRes.TrailReportActivity);
                return File(encPdf, "application/pdf", "SummaryReport_" + ReportPfdPassword + ".pdf");
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Rpt Detailed Information

        [HttpPost]
        public ActionResult getDetailedReport(string FromDate, string ToDate)
        {
            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)))
            {
                DateTime frmDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);
                toDate = toDate.AddDays(1).AddTicks(-1);
                ReportHelper helper = new ReportHelper();
                var data = helper.getDetails(frmDate, toDate, User.FirstName);
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportViewDetailed + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy"), Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportViewDetailed + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy"), TnT.LangResource.GlobalRes.TrailReportActivity);
                return Json(data);
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidDates);
            }
        }

        public FileResult genePDFDetailed(string FromDate, string ToDate)
        {

            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)))
            {
                DateTime frmDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);
                toDate = toDate.AddDays(1).AddTicks(-1);
                byte[] bPDF = null;
                ReportHelper helper = new ReportHelper();
                var data = helper.getDetailsPdf(frmDate, toDate, User.FirstName);
                bindCss();
                string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.TempDataRptDetailBatchReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
                ViewAsPdf p = new ViewAsPdf("RptDetails", data)
                {
                    CustomSwitches = footer,
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = new Margins(3, 3, 20, 3)
                };
                bPDF = p.BuildPdf(ControllerContext);

                PDFEncryptor encryptor = new PDFEncryptor();
                var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
                trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadDetailReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadDetailReport, TnT.LangResource.GlobalRes.TrailReportActivity);
                return File(encPdf, "application/pdf", "DetailedReport_" + ReportPfdPassword + ".pdf");
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Rpt Details with Operator

        [HttpPost]
        public ActionResult getDetailsWithOprtrRpt(string FromDate, string ToDate, string Level)
        {
            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)))
            {
                DateTime frmDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);
                toDate = toDate.AddDays(1).AddTicks(-1);
                ReportHelper helper = new ReportHelper();
                string data;
                if (Level == "NA")
                {
                    data = helper.getDetailsWithOprtr(frmDate, toDate, "NA", User.FirstName);
                    trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewDetailwithOperator + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy"), Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewDetailwithOperator + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy"), TnT.LangResource.GlobalRes.TrailReportActivity);
                }
                else
                {
                    data = helper.getDetailsWithOprtr(frmDate, toDate, Level, User.FirstName);
                    trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewDetailwithOperator + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy") + " for " + Level, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewDetailwithOperator + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy") + " for " + Level, TnT.LangResource.GlobalRes.TrailReportActivity);
                }

                return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidDates);
            }
        }

        public FileResult genePDFDetailsWithOprtr(string FromDate, string ToDate, string Level)
        {
            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)))
            {
                DateTime frmDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);
                toDate = toDate.AddDays(1).AddTicks(-1);
                byte[] bPDF = null;
                ReportHelper helper = new ReportHelper();
                var data = helper.getDetailsWithOprtrpdf(frmDate, toDate, Level, User.FirstName);
                bindCss();
                string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.TempDataRptDetailwthOpratorReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
                ViewAsPdf p = new ViewAsPdf("RptDtlWithOptr", data)
                {
                    CustomSwitches = footer,
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = new Margins(3, 3, 20, 3)
                };
                bPDF = p.BuildPdf(ControllerContext);

                PDFEncryptor encryptor = new PDFEncryptor();
                var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
                trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadDetailWthOpratorReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadDetailWthOpratorReport, TnT.LangResource.GlobalRes.TrailReportActivity);
                return File(encPdf, "application/pdf", "DetailsWithOprtrReport_" + ReportPfdPassword + ".pdf");
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Rpt  Operator Statistcis

        [HttpPost]
        public ActionResult getOprtrStatsRpt(string FromDate, string ToDate, string Level)
        {
            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)))
            {
                DateTime frmDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);
                toDate = toDate.AddDays(1).AddTicks(-1);
                ReportHelper helper = new ReportHelper();
                string data;
                if (Level == "NA")
                {
                    data = helper.getOprtrStats(frmDate, toDate, "NA", User.FirstName);
                    trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewOperatorStatistics + " " + frmDate.Date.ToString("dd/MM/yyyy") + " till " + toDate.Date.ToString("dd/MM/yyyy"), Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewOperatorStatistics + " " + frmDate.Date.ToString("dd/MM/yyyy") + " till " + toDate.Date.ToString("dd/MM/yyyy"), TnT.LangResource.GlobalRes.TrailReportActivity);
                }
                else
                {
                    data = helper.getOprtrStats(frmDate, toDate, Level, User.FirstName);
                    trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewOperatorStatistics + " " + frmDate.Date.ToString("dd/MM/yyyy") + " till " + toDate.Date.ToString("dd/MM/yyyy") + "  For  " + Level, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewOperatorStatistics + " " + frmDate.Date.ToString("dd/MM/yyyy") + " till " + toDate.Date.ToString("dd/MM/yyyy") + "  For  " + Level, TnT.LangResource.GlobalRes.TrailReportActivity);
                }

                return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidDates);
            }
        }

        public FileResult genePDFOprtrStatsRpt(string FromDate, string ToDate, string Level)
        {
            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)))
            {
                DateTime frmDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);
                toDate = toDate.AddDays(1).AddTicks(-1);
                byte[] bPDF = null;
                ReportHelper helper = new ReportHelper();
                var data = helper.getOprtrStatsPdf(frmDate, toDate, Level, User.FirstName);
                string footer = "--footer-left \" " + TnT.LangResource.GlobalRes.TempDataRptOpratorStatReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
                bindCss();
                ViewAsPdf p = new ViewAsPdf("RptOperatorStats", data)
                {
                    CustomSwitches = footer,
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = new Margins(3, 3, 20, 3)
                };
                bPDF = p.BuildPdf(ControllerContext);

                PDFEncryptor encryptor = new PDFEncryptor();
                var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
                trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadOpratorstatReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadOpratorstatReport, TnT.LangResource.GlobalRes.TrailReportActivity);
                return File(encPdf, "application/pdf", "OperatorStatsReport_" + ReportPfdPassword + ".pdf");
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Rpt parent Child Relationships

        [HttpPost]
        public ActionResult getPCRelationshipReport(string JID)
        {
            if (!string.IsNullOrEmpty(JID))
            {
                decimal jid = Convert.ToDecimal(JID);
                ReportHelper helper = new ReportHelper();
                var jobData = db.Job.Find(jid);

                var data = helper.getParentChildReport(jid, User.FirstName);

                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportPCReport + " " + jobData.JobName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportPCReport + " " + jobData.JobName, TnT.LangResource.GlobalRes.TrailReportActivity);
                //return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
                return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidjb);
            }
        }



        public FileResult getPCRelationshipReportPDF(string JID, string Level)
        {

            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.getParentChildReportPdf(Convert.ToDecimal(JID), User.FirstName);
            bindCss();
            string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.TempDataRptParentChldRelationReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptPCTable", data)
            {
                CustomSwitches = footer, //"--page-offset 0 --footer-center [page] --footer-font-size 8"
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadParentChildrelReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadParentChildrelReport, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "ParentchildReport_" + ReportPfdPassword + ".pdf");
        }

        #endregion

        #region Rpt Job wise SSCC

        [HttpPost]
        public ActionResult getJobwiseSSCCReport(string JID)
        {
            if (!string.IsNullOrEmpty(JID))
            {
                decimal jid = Convert.ToDecimal(JID);
                ReportHelper helper = new ReportHelper();
                var data = helper.getJobWiseSSCCs(jid, User.FirstName);
                var name = db.Job.Find(jid).JobName;
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportJobWiseSSCC + " " + name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportJobWiseSSCC + " " + name, TnT.LangResource.GlobalRes.TrailReportActivity);
                return Json(data);
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidjb);
            }
        }

        public FileResult genePDJobwiseSSCC(string JID)
        {

            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.getJobWiseSSCCsPDF(Convert.ToDecimal(JID), User.FirstName);
            bindCss();
            ViewAsPdf p = new ViewAsPdf("RptJobWiseSSCC", data)
            {
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadBatchWisSScclReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadBatchWisSScclReport, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "JobWiseSSCCReport_" + ReportPfdPassword + ".pdf");
        }
        #endregion

        #region Rpt UID List

        /// <summary>
        /// retrive  UID List at initial
        /// </summary>
        /// <param name="JID"></param>
        /// <returns></returns> 
        [HttpPost]
        public ActionResult getUIDList(string JID)
        {
            if (!string.IsNullOrEmpty(JID))
            {
                decimal jid = Convert.ToDecimal(JID);
                string lvl = ProductPackageHelper.getTopDeck(JID);
                ReportHelper helper = new ReportHelper();
                var data = helper.getUIDs(jid, lvl, User.FirstName);
                var name = db.Job.Find(jid).JobName;
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewUID + " " + name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewUID + " " + name, TnT.LangResource.GlobalRes.TrailReportActivity);
                return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidjb);
            }
        }


        [HttpPost]
        public ActionResult getUIDListDataLevelWise(string JID, string Level)
        {
            if (!(string.IsNullOrEmpty(JID)) && !(string.IsNullOrEmpty(Level)))
            {
                decimal jid = Convert.ToDecimal(JID);
                ReportHelper helper = new ReportHelper();
                var lst = helper.getUIDs(jid, Level, User.FirstName);
                var name = db.Job.Find(jid).JobName;
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewUID + " " + name + " " + TnT.LangResource.GlobalRes.TrailReportAndfor + " " + Level, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewUID + " " + name + " " + TnT.LangResource.GlobalRes.TrailReportAndfor + " " + Level, TnT.LangResource.GlobalRes.TrailReportActivity);
                return new JsonResult { Data = lst, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataDavaInvalidinput);
            }

        }


        public FileResult genePDFUidList(string JID, string Level)
        {

            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.getUIDsPDF(Convert.ToDecimal(JID), Level, User.FirstName);
            bindCss();
            string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.TempDataRptUidListReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptUidList", data)
            {
                CustomSwitches = footer,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(13, 13, 50, 13)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadUidLiatReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadUidLiatReport, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "UIDListReport_" + ReportPfdPassword + ".pdf");
        }
        #endregion

        #region Rpt Bad UID List

        /// <summary>
        /// retrive  UID List at initial
        /// </summary>
        /// <param name="JID"></param>
        /// <returns></returns> 
        [HttpPost]
        public ActionResult getBadUIDList(string JID, string Level)
        {
            if (!string.IsNullOrEmpty(JID))
            {
                decimal jid = Convert.ToDecimal(JID);
                // string lvl = ProductPackageHelper.getTopDeck(JID);
                if (Level == null)
                {
                    var pkg = db.PackagingDetails.Where(x => x.JobID == jid && x.IsRejected == true && x.IsManualUpdated == false).Select(x => x.PackageTypeCode).Distinct().ToList();
                    if (pkg.Count > 0)
                    {
                        if (pkg.Contains("MOC"))
                        {
                            Level = "MOC";
                        }
                        else
                        {
                            Level = pkg[0];
                        }
                    }
                    else
                    {
                        Level = "MOC";
                    }

                }
                ReportHelper helper = new ReportHelper();
                var data = helper.getBadUIDs(jid, Level, User.FirstName);
                var name = db.Job.Find(jid).JobName;
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReporViewBadUIDList + " " + name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReporViewBadUIDList + " " + name, TnT.LangResource.GlobalRes.TrailReportActivity);
                return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidjb);
            }
        }

        public FileResult genePDFBadUidList(string JID, string Level)
        {

            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.getBadUIDsPdf(Convert.ToDecimal(JID), Level, User.FirstName);
            bindCss();
            string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.TempDataRptBadUidListReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptBadUIdList", data)
            {
                CustomSwitches = footer, //"--page-offset 0 --footer-center [page] --footer-font-size 8"
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadBadUidListReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadBadUidListReport, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "BadUIDListReport_" + ReportPfdPassword + ".pdf");
        }

        #endregion

        #region Rpt Detailed Job Information

        [HttpPost]
        public ActionResult getDetailedJobInfo(string JID)
        {
            if (!string.IsNullOrEmpty(JID))
            {
                decimal jid = Convert.ToDecimal(JID);
                ReportHelper helper = new ReportHelper();

                var data = helper.getDetailedJobInformation(jid, DateTime.Now, DateTime.Now, User.FirstName);
                var name = db.Job.Find(jid).JobName;
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewDetailedJobInfo + " " + name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewDetailedJobInfo + " " + name, TnT.LangResource.GlobalRes.TrailReportActivity);
                return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidjb);
            }
        }

        public FileResult getDetailedJobInfoPdf(string JID)
        {
            decimal jid = Convert.ToDecimal(JID);
            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.getDetailedJobInformationPdf(jid, DateTime.Now, DateTime.Now, User.FirstName);
            bindCss();
            string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.TempDataRptDetailedBatchInfoReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptDetailedJobInfo", data)
            {
                CustomSwitches = footer,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };
            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadDetailBatchInfoReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadDetailBatchInfoReport, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "DetailedBatchInfoReport_" + ReportPfdPassword + ".pdf");
        }

        #endregion

        #region Rpt Decommisioned UIDs

        /// <summary>
        /// retrive  UID List at initial
        /// </summary>
        /// <param name="JID"></param>
        /// <returns></returns> 
        [HttpPost]
        public ActionResult getDecommUIDList(string JID)
        {
            if (!string.IsNullOrEmpty(JID))
            {
                decimal jid = Convert.ToDecimal(JID);
                string lvl = ProductPackageHelper.getTopDeck(JID);
                ReportHelper helper = new ReportHelper();
                var data = helper.getDecommisionedUIDs(jid, lvl, User.FirstName);
                var name = db.Job.Find(jid).JobName;
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewDecommissionedUID + " " + name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewDecommissionedUID + " " + name, TnT.LangResource.GlobalRes.TrailReportActivity);
                return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidjb);
            }
        }

        [HttpPost]
        public ActionResult getDecommUIDListDataLevelWise(string JID, string Level)
        {
            if (!(string.IsNullOrEmpty(JID)) && !(string.IsNullOrEmpty(Level)))
            {
                decimal jid = Convert.ToDecimal(JID);
                ReportHelper helper = new ReportHelper();
                var lst = helper.getDecommisionedUIDs(jid, Level, User.FirstName);
                var name = db.Job.Find(jid).JobName;
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewDecommissionedUID + " " + name + TnT.LangResource.GlobalRes.TrailReportAndfor + Level, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewDecommissionedUID + " " + name + TnT.LangResource.GlobalRes.TrailReportAndfor + Level, TnT.LangResource.GlobalRes.TrailReportActivity);
                return Json(lst);
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataDavaInvalidinput);
            }
        }

        public FileResult genePDFDecommUIDList(string JID, string Level)
        {

            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.getDecommisionedUIDsPDF(Convert.ToDecimal(JID), Level, User.FirstName);
            bindCss();
            string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.TempDataRptDecommUidListReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptDecomUIdList", data)
            {
                CustomSwitches = footer,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadDecommisionUidListReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadDecommisionUidListReport, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "DecommUIDListReport_" + ReportPfdPassword + ".pdf");
        }
        #endregion

        #endregion

        //  getting Partial View of Jobs
        #region job filter date wise
        [HttpPost]
        public ActionResult getMfgWiseJobs(string FrmDt, string ToDt)
        {
            try
            {
                DateTime frmdt = DateTime.ParseExact(FrmDt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime todt = DateTime.ParseExact(ToDt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                todt = todt.AddDays(1).AddTicks(-1);
                var jbs = db.Job.Where(j => j.MfgDate >= frmdt && j.MfgDate <= todt);
                string postsHtml = ViewRenderer.RenderPartialView("~/Views/Reports/JobsPartialView.cshtml", jbs);

                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewBatchMfg + " " + frmdt.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReportand + " " + todt.Date.ToString("dd/MM/yyyy"), Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewBatchMfg + " " + frmdt.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReportand + " " + todt.Date.ToString("dd/MM/yyyy"), TnT.LangResource.GlobalRes.TrailReportActivity);
                return Json(postsHtml);
            }
            catch (Exception ex)
            {
                return Json(TnT.LangResource.GlobalRes.TempDataDavaInvalid + "!");
            }
        }

        [HttpPost]
        public ActionResult getExpgWiseJobs(string FrmDt, string ToDt)
        {
            try
            {
                DateTime frmdt = DateTime.ParseExact(FrmDt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime todt = DateTime.ParseExact(ToDt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                todt = todt.AddDays(1).AddTicks(-1);
                var jbs = db.Job.Where(j => j.ExpDate >= frmdt && j.ExpDate <= todt);
                string postsHtml = ViewRenderer.RenderPartialView("~/Views/Reports/JobsPartialView.cshtml", jbs);
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewbatchExp + " " + frmdt.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReportand + " " + todt.Date.ToString("dd/MM/yyyy"), Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewbatchExp + " " + frmdt.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReportand + " " + todt.Date.ToString("dd/MM/yyyy"), TnT.LangResource.GlobalRes.TrailReportActivity);
                return Json(postsHtml);
            }
            catch (Exception ex)
            {
                return Json(TnT.LangResource.GlobalRes.TempDataDavaInvalid + "!");
            }
        }

        [HttpPost]
        public ActionResult getCreatedDtWiseJobs(string FrmDt, string ToDt)
        {
            try
            {
                string postsHtml = string.Empty;
                DateTime frmdt = DateTime.ParseExact(FrmDt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime todt = DateTime.ParseExact(ToDt, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                todt = todt.AddDays(1).AddTicks(-1);
                var jbs = db.Job.Where(j => j.CreatedDate >= frmdt && j.CreatedDate <= todt).OrderBy(j => j.JobName).OrderBy(x => x.CreatedDate);
                if (jbs.Count() <= 0)
                {
                    //postsHtml = TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
                    postsHtml = "No Data";
                }
                else
                {
                    postsHtml = ViewRenderer.RenderPartialView("~/Views/Reports/JobsPartialView.cshtml", jbs);
                }
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewbatchcreatedDate + frmdt.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReportand + " " + todt.Date.ToString("dd/MM/yyyy"), Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewbatchcreatedDate + frmdt.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReportand + " " + todt.Date.ToString("dd/MM/yyyy"), TnT.LangResource.GlobalRes.TrailReportActivity);
                return Json(postsHtml);
            }
            catch (Exception ex)
            {
                return Json(TnT.LangResource.GlobalRes.TempDataDavaInvalid + "!");
            }
        }
        #endregion

        #region retriveJobs

        private string getJobsForLineAssigned(IQueryable<LineLocation> Line)
        {
            try
            {
                List<Job> jobs = new List<Job>();
                foreach (var line in Line)
                {
                    var data = db.Job.Where(j => j.LineCode == line.ID).ToList();
                    foreach (var jb in data)
                    {
                        jobs.Add(jb);
                    }
                }
                string postsHtml = ViewRenderer.RenderPartialView("~/Views/Reports/JobsPartialView.cshtml", jobs);

                return postsHtml;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private string getRenderedViewJobs(string LocationCode, string DivisionCode, string PlantCode, string LineCode)
        {
            try
            {
                if ((!string.IsNullOrEmpty(LocationCode)) && (!string.IsNullOrEmpty(DivisionCode)) && (!string.IsNullOrEmpty(PlantCode)) && (!string.IsNullOrEmpty(LineCode)))
                {
                    var Line = db.LineLocation.Where(l => l.LocationCode == LocationCode && l.DivisionCode == DivisionCode && l.PlantCode == PlantCode && l.LineCode == LineCode);
                    return getJobsForLineAssigned(Line);
                }
                else if ((!string.IsNullOrEmpty(LocationCode)) && (!string.IsNullOrEmpty(DivisionCode)) && (!string.IsNullOrEmpty(PlantCode)))
                {
                    var Line = db.LineLocation.Where(l => l.LocationCode == LocationCode && l.DivisionCode == DivisionCode && l.PlantCode == PlantCode);
                    return getJobsForLineAssigned(Line);
                }
                else if ((!string.IsNullOrEmpty(LocationCode)) && (!string.IsNullOrEmpty(DivisionCode)))
                {
                    var Line = db.LineLocation.Where(l => l.LocationCode == LocationCode && l.DivisionCode == DivisionCode);
                    return getJobsForLineAssigned(Line);
                }
                else if ((!string.IsNullOrEmpty(LocationCode)))
                {
                    var Line = db.LineLocation.Where(l => l.LocationCode == LocationCode);
                    return getJobsForLineAssigned(Line);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private List<JobStatus> getJobState()
        {
            //JobStatus
            List<JobStatus> lstJobStatus = new List<JobStatus>();
            JobStatus obj0 = new JobStatus(); //JobStatus { Status = "All" };
            obj0.Status = "All";
            lstJobStatus.Add(obj0);
            JobStatus obj = new JobStatus();
            obj.Status = "Created";
            lstJobStatus.Add(obj);
            JobStatus obj1 = new JobStatus();
            obj1.Status = "Running";
            lstJobStatus.Add(obj1);
            JobStatus obj2 = new JobStatus();
            obj2.Status = "Created";
            lstJobStatus.Add(obj2);
            JobStatus obj3 = new JobStatus();
            obj3.Status = "Paused";
            lstJobStatus.Add(obj3);
            JobStatus obj4 = new JobStatus();
            obj4.Status = "Closed";
            lstJobStatus.Add(obj4);
            JobStatus obj5 = new JobStatus();
            obj5.Status = "Decommissioned";
            lstJobStatus.Add(obj5);

            return lstJobStatus;

        }

        public enum JobState
        {
            Created,
            Running,
            Paused,
            Closed,
            Decommissioned,
            LineClearance,
            CompleteTransfer,
            ForcefullyBatchClose
        };

        #endregion

        #region bind dynamic drop downs
        [HttpPost]
        public ActionResult getDivisionCodes(string LocationCode)
        {
            try
            {
                // var divsionCodes = db.LineLocation.Where(l => l.LocationCode == LocationCode).Distinct();
                var divsionCodes = (from u in db.LineLocation.Where(l => l.LocationCode == LocationCode) select new { DivisionCode = u.DivisionCode }).Distinct();
                string jView = getRenderedViewJobs(LocationCode, null, null, null);
                object[] response = { divsionCodes, jView };
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReporviewDivisionCode + " " + LocationCode, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReporviewDivisionCode + " " + LocationCode, TnT.LangResource.GlobalRes.TrailReportActivity);
                return Json(response);

            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        [HttpPost]
        public ActionResult getPlantCodes(string LocationCode, string DivisionCode)
        {
            try
            {
                //var lns = db.LineLocation.Where(l => l.LocationCode == LocationCode && l.DivisionCode == DivisionCode).Distinct();
                var lns = (from u in db.LineLocation.Where(l => l.LocationCode == LocationCode && l.DivisionCode == DivisionCode) select new { PlantCode = u.PlantCode }).Distinct();
                string jView = getRenderedViewJobs(LocationCode, DivisionCode, null, null);
                object[] response = { lns, jView };
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewPlantCode + " " + LocationCode + " " + TnT.LangResource.GlobalRes.TrailReportandDivisionCode + " " + DivisionCode, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewPlantCode + " " + LocationCode + " " + TnT.LangResource.GlobalRes.TrailReportandDivisionCode + " " + DivisionCode, TnT.LangResource.GlobalRes.TrailReportActivity);
                return Json(response);
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        [HttpPost]
        public ActionResult getLineCodes(string LocationCode, string DivisionCode, string PlantCode)
        {
            try
            {
                //    var lns = db.LineLocation.Where(l => l.LocationCode == LocationCode && l.DivisionCode == DivisionCode && l.PlantCode == PlantCode).Distinct();
                var lns = (from u in db.LineLocation.Where(l => l.LocationCode == LocationCode && l.DivisionCode == DivisionCode && l.PlantCode == PlantCode) select new { LineCode = u.LineCode }).Distinct();
                string jView = getRenderedViewJobs(LocationCode, DivisionCode, PlantCode, null);
                object[] response = { lns, jView };
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportLineCode + " " + LocationCode + " " + TnT.LangResource.GlobalRes.TrailReportandDivisionCode + " " + DivisionCode + " " + TnT.LangResource.GlobalRes.TrailReportandPlantCode + " " + PlantCode, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportLineCode + " " + LocationCode + " " + TnT.LangResource.GlobalRes.TrailReportandDivisionCode + " " + DivisionCode + " " + TnT.LangResource.GlobalRes.TrailReportandPlantCode + " " + PlantCode, TnT.LangResource.GlobalRes.TrailReportActivity);
                return Json(response);
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        [HttpPost]
        public ActionResult getProductsStatusBatchesJobNames(string LocationCode, string DivisionCode, string PlantCode, string LineCode)
        {
            try
            {
                var ln = db.LineLocation.Where(l => l.LocationCode == LocationCode && l.DivisionCode == DivisionCode && l.PlantCode == PlantCode && l.LineCode == LineCode).FirstOrDefault();
                var Jobs = db.Job.Where(j => j.LineCode == ln.ID);

                List<decimal> PAIDs = new List<decimal>();
                foreach (var job in Jobs)
                {
                    decimal paid = job.PAID;
                    PAIDs.Add(paid);
                }
                List<PackagingAsso> products1 = new List<PackagingAsso>();
                foreach (var paid in PAIDs)
                {
                    var p = db.PackagingAsso.Find(paid);
                    products1.Add(p);
                }
                var status = (from u in getJobState() select new { Status = u.Status }).Distinct();
                var products = (from u in products1 select new { PAID = u.PAID.ToString(), Name = u.Name }).Distinct();

                string jView = getRenderedViewJobs(LocationCode, DivisionCode, PlantCode, LineCode);
                object[] response = { products, Jobs, jView, status };
                return Json(response);
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        [HttpPost]
        public ActionResult getProductWiseJobs(string PAID, string LineCode)
        {
            try
            {
                decimal paid = Convert.ToDecimal(PAID);
                var product = db.PackagingAsso.Find(paid);
                var jobs = db.Job.Where(j => j.PAID == paid);
                string postsHtml = ViewRenderer.RenderPartialView("~/Views/Reports/JobsPartialView.cshtml", jobs);
                var linecode = db.LineLocation.Where(l => l.LineCode == LineCode).FirstOrDefault();
                var jb = db.Job.Where(j => j.PAID == paid && j.LineCode == linecode.ID).ToList();
                //return postsHtml;
                object[] response = { jb, postsHtml };
                return Json(response);

            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        [HttpPost]
        public ActionResult getBatchWiseJobs(string BatchNo)
        {
            try
            {
                decimal jid = Convert.ToDecimal(BatchNo);
                var jobs = db.Job.Where(j => j.JID == jid);
                string postsHtml = ViewRenderer.RenderPartialView("~/Views/Reports/JobsPartialView.cshtml", jobs);
                var jb = db.Job.Where(j => j.JID == jid).ToList();
                object[] response = { jobs, postsHtml };
                return Json(response);
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        [HttpPost]
        public ActionResult getJobNameWiseJobs(string JobName, string batchno)
        {
            try
            {
                decimal jid = Convert.ToDecimal(JobName);
                decimal bno = Convert.ToDecimal(batchno);
                var jobs = db.Job.Where(j => j.JID == jid);
                var jb = db.Job.Where(j => j.JID == bno);
                string postsHtml = ViewRenderer.RenderPartialView("~/Views/Reports/JobsPartialView.cshtml", jobs);
                object[] response = { jb, postsHtml };
                return Json(response);
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }

        [HttpPost]
        public ActionResult getStatusWiseJobs(string LocationCode, string DivisionCode, string PlantCode, string LineCode, string Status)
        {
            try
            {
                var ln = db.LineLocation.Where(l => l.LocationCode == LocationCode && l.DivisionCode == DivisionCode && l.PlantCode == PlantCode && l.LineCode == LineCode).FirstOrDefault();
                IQueryable<Job> jbs;
                if (Enum.IsDefined(typeof(JobState), Status))
                {
                    JobState JState = (JobState)Enum.Parse(typeof(JobState), Status, true);
                    int jstat = Convert.ToInt32(JState);
                    jbs = db.Job.Where(j => j.LineCode == ln.ID && j.JobStatus == jstat);
                }
                else if (Status == "All")
                {
                    jbs = db.Job.Where(j => j.LineCode == ln.ID);
                }
                else
                {
                    jbs = null;
                }

                string postsHtml = ViewRenderer.RenderPartialView("~/Views/Reports/JobsPartialView.cshtml", jbs);
                var jb = jbs.ToList();
                object[] responce = { jb, postsHtml };
                return Json(responce);
            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }
        #endregion

        private void bindData()
        {
            // Line Location
            //   var lines = db.LineLocation.Distinct();
            var lines = (from u in db.LineLocation select new { LocationCode = u.LocationCode }).Distinct().ToList();
            var locationCodes = new SelectList(lines, "LocationCode", "LocationCode").Distinct().ToList();
            locationCodes.Insert(0, new SelectListItem() { Value = "0", Text = TnT.LangResource.GlobalRes.RptJobFilterSelectLocation });
            ViewData["LocationCode"] = locationCodes;
        }

        private void bindCss()
        {
            ViewBag.scrollprint = "scrollprint";
            ViewBag.tbFont = "tbFont";
            ViewBag.rptPdfBox = "rptPdfBox";
            ViewBag.rptBadUIDTbl = "rptBadUIDTbl";
        }

        enum JobRetrival
        {
            MfgDateWise,
            ExpDateWise,
            CreatedDateWise
        }

        #endregion



        // GET: Job Reports
        public ActionResult IdChecker()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search()
        {
            string Type, UID, SSCC, Batch;
            Type = Request["IdType"];
            UID = Request["txtUID"];
            SSCC = Request["txtSSCC"];
            Batch = Request["txtBatch"];
            return View();
        }




        #region Audit Trail Report
        [HttpPost]
        public ActionResult getAuditTrailReport(string FromDate, string ToDate, string Type, string lineLocation, string UserId, string Activity)
        {
            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)))
            {

                string fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"); // Convert.ToDateTime(FromDate); 
                string tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");  // Convert.ToDateTime(ToDate);
                DateTime toDate = Convert.ToDateTime(tDate);
                DateTime frmDate = Convert.ToDateTime(fDate);

                string fsDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"); // Convert.ToDateTime(FromDate); 
                string tsDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                DateTime frmSDate = Convert.ToDateTime(fsDate);//DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toSDate = Convert.ToDateTime(tsDate); //DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);

                toDate = toDate.AddDays(1).AddTicks(-1);
                toSDate = toSDate.AddDays(1).AddTicks(-1);

                string lineCode = lineLocation;
                ReportHelper helper = new ReportHelper();
                var data = helper.getAuditTrailRpt(frmDate, toDate, frmSDate, toSDate, Type, lineCode, User.FirstName, UserId, Activity);

                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewAuditTrail + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy"), Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailReportviewAuditTrail + " " + frmDate.Date.ToString("dd/MM/yyyy") + " " + TnT.LangResource.GlobalRes.TrailReporttill + " " + toDate.Date.ToString("dd/MM/yyyy"), TnT.LangResource.GlobalRes.TrailReportActivity);
                //return Json(result);
                return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataRptInvalidDates);
            }
        }

        public FileResult genePDAuditTrail(string FromDate, string ToDate, string Type, string lineLocation, string UserId, string Activity)
        {
            if ((!string.IsNullOrEmpty(FromDate)) && (!string.IsNullOrEmpty(ToDate)))
            {
                DateTime from = DateTime.Parse(FromDate);
                string fDate = from.ToString("yyyy-MM-dd");

                DateTime to = DateTime.Parse(ToDate);
                string tDate = to.ToString("yyyy-MM-dd");
                // string fDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"); // Convert.ToDateTime(FromDate); 
                //  string tDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");  // Convert.ToDateTime(ToDate);
                DateTime toDate = Convert.ToDateTime(tDate);
                DateTime frmDate = Convert.ToDateTime(fDate);

                //  string fsDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd"); // Convert.ToDateTime(FromDate); 
                //string tsDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                DateTime frmSDate = Convert.ToDateTime(fDate);//DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                DateTime toSDate = Convert.ToDateTime(tDate); //DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);  //Convert.ToDateTime(ToDate);

                toDate = toDate.AddDays(1).AddTicks(-1);
                toSDate = toSDate.AddDays(1).AddTicks(-1);
                //if(Type=="Line")
                //{
                //    var sactivity = db.S_Activity.Where(x => x.Activity == Activity).FirstOrDefault();
                //    Activity =Convert.ToString(sactivity.Id);
                //}
                string lineCode = lineLocation;
                byte[] bPDF = null;
                ReportHelper helper = new ReportHelper();
                var data = helper.getAuditTrailRptPdf(frmDate, toDate, frmSDate, toSDate, Type, lineCode, User.FirstName, UserId, Activity);
                bindCss();
                string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.RptAuditTrail + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
                ViewAsPdf p = new ViewAsPdf("RptAuditTrails", data)
                {
                    CustomSwitches = footer,
                    PageSize = Rotativa.Options.Size.A4,
                    PageMargins = new Margins(3, 3, 20, 3)
                };

                bPDF = p.BuildPdf(ControllerContext);

                PDFEncryptor encryptor = new PDFEncryptor();
                var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
                trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadAuditTrailReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadAuditTrailReport, TnT.LangResource.GlobalRes.TrailReportActivity);
                return File(encPdf, "application/pdf", "AuditTrailReport_" + ReportPfdPassword + ".pdf");
            }
            else
            {
                return null;
            }
        }


        [HttpPost]
        public ActionResult getActivity(string Type)
        {
            var data = db.S_Activity.Where(x => x.Type == Type).ToList();
            return Json(data);
        }
        #endregion

        public ActionResult PrintView(HttpContent DataRPT)
        {
            var respo = DataRPT;


            ViewBag.DataRPT = respo;
            return View();
        }

        //[ValidateInput(false)]

        //public ActionResult PrintReport(string reportData)
        //{
        //    //try
        //    //{
        //    //    StringReader sr = new StringReader(reportData.ToString());

        //    //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //    //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //    //    using (MemoryStream memoryStream = new MemoryStream())
        //    //    {
        //    //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
        //    //        pdfDoc.Open();

        //    //        htmlparser.Parse(sr);
        //    //        pdfDoc.Close();

        //    //        byte[] bytes = memoryStream.ToArray();
        //    //        memoryStream.Close();

        //    //        //var fileBytes = Encoding.ASCII.GetBytes(reportData);
        //    //        string FileName = "Report.pdf";
        //    //        return File(bytes, ".pdf", FileName);
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //    throw;
        //    //}

        //}



        #region EPCIS REport

        public ActionResult EPCISSearch()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EPCISReport()
        {
            string uid = Request["txtEPCISuid"];
            if (uid == null)
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataReportInvalidUID;
                return RedirectToAction("EPCISSearch");
            }
            var data2 = db.EpcisEventDetails.SqlQuery(string.Format("select * from EpcisEventDetails where EpcList like '%{0}%' or ChildEPC like '%{0}%'", uid));
            if (data2.Count() == 0)
            {
                var data3 = db.EpcisEventDetails.SqlQuery(string.Format("select * from EpcisEventDetails where ParentID like '%{0}%'", uid));
                if (data3.Count() == 0)
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataReportUIDnotfound;
                    return RedirectToAction("EPCISSearch");
                }
                else
                {
                    return View(data2);
                }
            }
            else
            {
                return View(data2);
            }

        }


        #endregion

        #region UID Verification

        public ActionResult UIdDetails()
        {
            return View();
        }
        #endregion
        #region UID Validation
        public ActionResult RptUidDetail(string detail, string type)
        {
            ReportHelper helper = new ReportHelper();
            var data = helper.getUIDDetail(detail, type, User.FirstName);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptReqViewUidVallReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptReqViewUidVallReport, TnT.LangResource.GlobalRes.TrailReportActivity);
            return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
        }

        public FileResult genePDUidDetail(string detail, string type)
        {

            byte[] bPDF = null;
            ReportHelper helper = new ReportHelper();
            var data = helper.getUIDDetailPdf(detail, type, User.FirstName);
            bindCss();
            string footer = "--footer-left \"" + TnT.LangResource.GlobalRes.TempDataRptUidValReport + "\" --footer-right \"Page [page] of [toPage]\"" + " --footer-font-size \"9\" --footer-spacing 6 --footer-font-name \"calibri light\"";
            ViewAsPdf p = new ViewAsPdf("RptUidValidation", data)
            {
                CustomSwitches = footer,
                PageSize = Rotativa.Options.Size.A4,
                PageMargins = new Margins(3, 3, 20, 3)
            };

            bPDF = p.BuildPdf(ControllerContext);

            PDFEncryptor encryptor = new PDFEncryptor();
            var encPdf = encryptor.encrypt(bPDF, "user", ReportPfdPassword);
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadUidValReport, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailRptDownloadUidValReport, TnT.LangResource.GlobalRes.TrailReportActivity);
            return File(encPdf, "application/pdf", "UidValidationReport_" + ReportPfdPassword + ".pdf");
        }
        #endregion


    }

}



