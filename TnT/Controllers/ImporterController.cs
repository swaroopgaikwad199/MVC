using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer;
using TnT.DataLayer.Security;
using TnT.DataLayer.TracelinkService;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.Code;
using TnT.Models.TraceLinkImporter;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class ImporterController : BaseController
    {
        private Trails trail = new Trails();
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Importer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tracelink()
        {
            BindDDL();
            return View();
        }

        private void BindDDL()
        {
            try
            {
                //Provider 2 = Tracelink
                ViewBag.Customer = db.M_Customer.Where(x => x.IsDeleted == false && x.ProviderId == 2);


            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost]
        public ActionResult getCustomerData(int CId)
        {

            var data = db.M_Customer.Find(CId);
            int providerid = Convert.ToInt32(data.ProviderId);
            //var product = (from a in db.Job
            //               join b in db.PackagingAsso on a.PAID equals b.PAID
            //               where a.ProviderId == providerid
            //               select new
            //               {
            //                   Name = b.Name,
            //                   Paid = b.PAID,

            //               }).Distinct().ToList();
            var product = db.PackagingAsso.Where(x => x.ProviderId == providerid && x.VerifyProd == true).ToList();

            object[] response = { data, product };
            return Json(response);


        }

        [HttpPost]
        public ActionResult getGTIN(int PAID)
        {
            var GTIN = db.PackagingAssoDetails.Where(x => x.PAID == PAID).OrderBy(x=>x.Id).Select(x => x.GTIN).ToList();
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


        // POST: Importer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TLRequest(TracelinkViewModel vm)
        {
            try
            {

                var customer = db.M_Customer.Find(vm.CustomerId);
                var provider = db.M_Providers.Where(x => x.Id == customer.ProviderId).FirstOrDefault();
                Tracelink tl = new DataLayer.TracelinkService.Tracelink();


                if (vm.Quantity > 0)
                {
                    var data = tl.getDataFromTracelink(customer.APIUrl, customer.SenderId, customer.ReceiverId, vm.Quantity, vm.GTIN, vm.SrnoType, customer.CompanyCode, vm.filterValue);
                    var errs = tl.getErrors();
                    if (errs.Count() > 0)
                    {
                        string err = "";
                        foreach (var item in errs)
                        {
                            err += item;
                        }
                        TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterservererrors + " " + err;
                        trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImporterservererrors + " " + err, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterservererrors + " " + err, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                        return RedirectToAction("Tracelink");
                    }
                    if (data.Count > 0)
                    {
                        M_TracelinkRequest req = new M_TracelinkRequest();
                        req.CustomerId = vm.CustomerId;
                        req.GTIN = vm.GTIN;
                        req.Quatity = vm.Quantity;
                        req.RequestedOn = DateTime.Now;
                        req.IsDeleted = false;
                        req.ProviderId = provider.Id;
                        req.SrnoType = vm.SrnoType;
                        req.FilterValue = vm.filterValue;
                        db.M_TracelinkRequest.Add(req);
                        db.SaveChanges();

                        //M_Identities ids = new M_Identities();
                        //ids.CreatedOn = DateTime.Now;
                        //ids.CustomerId = vm.CustomerId;
                        //ids.GTIN = vm.GTIN;
                        //ids.PPN = null;
                        //ids.IsExtra = false;
                        //ids.IsTransfered = false;                      
                        //db.M_Identities.Add(ids);
                        //db.SaveChanges();

                        foreach (var item in data)
                        {
                            item.TLRequestId = req.Id;
                        }
                        var convertedData = DataLayer.GeneralDataHelper.convertToDataTable(data);
                        BulkDataHelper dataHlpr = new BulkDataHelper();
                        if (dataHlpr.InsertTracelinkUIDIdenties(convertedData))
                        {
                            TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterDataimported + " " + convertedData.Rows.Count + " " + TnT.LangResource.GlobalRes.TempDataExporterSerialNoImpo;
                            trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImporterDataimported + " " + convertedData.Rows.Count + TnT.LangResource.GlobalRes.TempDataExporterSerialNoImpoGTIN + "" + vm.GTIN, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterDataimported + " " + convertedData.Rows.Count + TnT.LangResource.GlobalRes.TempDataExporterSerialNoImpoGTIN + "  " + vm.GTIN, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                        }
                        else
                        {
                            TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterUnablestore;
                            trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImporterUnablestore, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterUnablestore, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                        }

                    }
                }
                return RedirectToAction("Tracelink");
            }
            catch (Exception ex)
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterError + " " + ex.Message;
                return RedirectToAction("Tracelink");
            }
        }


        public ActionResult CustomerUId()

        {
            ViewBag.Customer = db.M_Customer.ToList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerUId(HttpPostedFileBase upload, TracelinkViewModel vm)
        {
            if (vm.GTIN != null || vm.CustomerId != 0)
            {
                if (upload != null && upload.ContentLength > 0)
                {

                    if (upload.FileName.EndsWith(".csv"))
                    {
                        string filePath = "";
                        var postedFile = upload.FileName;
                        FileInfo fi = new FileInfo(postedFile);
                        string tempPath = Server.MapPath("~/Content/CustomerUID/");
                        if (!System.IO.Directory.Exists(tempPath))
                        {
                            System.IO.Directory.CreateDirectory(tempPath);
                        }
                        filePath = Server.MapPath("~/Content/CustomerUID/" + postedFile);
                        DataTable DtProds = new DataTable();
                        upload.SaveAs(filePath);
                        string csvData = System.IO.File.ReadAllText(filePath);
                        List<string> lstUid = new List<string>();
                        string[] stringSeparators = new string[] { "\r\n" };
                        var xTlinkUid = db.X_TracelinkUIDStore.Where(x => x.GTIN == vm.GTIN).Select(x => x.SerialNo).ToList();
                        foreach (string row in csvData.Split(stringSeparators, StringSplitOptions.None))
                        {
                            if (row != "")
                            {
                                string[] code = row.Split(',');
                                if (code[0] != "Code")
                                {
                                    if (vm.GTIN == code[1])
                                    {
                                        if (xTlinkUid.Contains(code[0]))
                                        {
                                            TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterDuplicateUiD;
                                            trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImporterDuplicateUiD, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterDuplicateUiD, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                                            ViewBag.Customer = db.M_Customer.ToList();
                                            return View();
                                        }
                                        else
                                        {
                                            lstUid.Add(code[0]);
                                        }
                                    }
                                }
                            }
                        }

                        var cust = db.M_Customer.Where(x => x.Id == vm.CustomerId).FirstOrDefault();

                        M_TracelinkRequest tlrequest = new M_TracelinkRequest();
                        tlrequest.GTIN = vm.GTIN;
                        tlrequest.CustomerId = vm.CustomerId;
                        tlrequest.ProviderId = cust.ProviderId;
                        tlrequest.Quatity = lstUid.Count();
                        tlrequest.RequestedOn = DateTime.Now;
                        tlrequest.IsDeleted = false;
                        tlrequest.Threshold = 0;
                        tlrequest.SrnoType = "GTIN";
                        db.M_TracelinkRequest.Add(tlrequest);
                        db.SaveChanges();

                        var mReq = db.M_TracelinkRequest.Where(x => x.GTIN == vm.GTIN).OrderByDescending(x => x.RequestedOn).FirstOrDefault();
                        List<X_TracelinkUIDStore> listTLUIDStore = new List<X_TracelinkUIDStore>();
                        for (int i = 0; i < lstUid.Count; i++)
                        {
                            X_TracelinkUIDStore TLUid = new X_TracelinkUIDStore();
                            TLUid.SerialNo = lstUid[i];
                            TLUid.TLRequestId = mReq.Id;
                            TLUid.IsUsed = false;
                            TLUid.GTIN = vm.GTIN;
                            listTLUIDStore.Add(TLUid);
                        }
                        var convertedData = DataLayer.GeneralDataHelper.convertToDataTable(listTLUIDStore);
                        BulkDataHelper dataHlpr = new BulkDataHelper();
                        if (dataHlpr.InsertTracelinkUIDIdenties(convertedData))
                        {
                            TempData["Success"] = lstUid.Count + TnT.LangResource.GlobalRes.TempDataImporterUidImpoGTIN + vm.GTIN;
                            trail.AddTrail(lstUid.Count + TnT.LangResource.GlobalRes.TempDataImporterUidImpoGTIN + vm.GTIN, Convert.ToInt32(User.ID), lstUid.Count + TnT.LangResource.GlobalRes.TempDataImporterUidImpoGTIN + vm.GTIN, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                        }
                        else
                        {
                            TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterUidCouldNotImpoGTIN + vm.GTIN;
                            trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImporterUidCouldNotImpoGTIN + vm.GTIN, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterUidCouldNotImpoGTIN + vm.GTIN, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                        }


                    }
                    else
                    {
                        TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterFileNotSupported;
                        trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImporterFileNotSupported, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterFileNotSupported, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                        ViewBag.Customer = db.M_Customer.ToList();
                        return View();
                    }
                }
                else
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterChooseCSVFile;
                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImporterChooseCSVFile, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterChooseCSVFile, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                    ViewBag.Customer = db.M_Customer.ToList();
                    return View();
                }
            }
            else
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterProvideGTINnCust;
                trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImporterProvideGTINnCust, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterProvideGTINnCust, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                ViewBag.Customer = db.M_Customer.ToList();
                return View();
            }
            ViewBag.Customer = db.M_Customer.ToList();
            return View();
        }

    }
}
