using EPCIS_XMLs_Generation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer;
using TnT.DataLayer.RFXCELService;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.RFXLImport;
using TnT.Models.TraceLinkImporter;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class RFXLImportUIDController : BaseController
    {
        private Trails trail = new Trails();
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: RFXLImportUID
        public ActionResult Index()
        {
            BindDDL();
            return View();
        }

        private void BindDDL()
        {
            try
            {
                //Provider 2 = Tracelink
                ViewBag.Customer = db.M_Customer.Where(x => x.IsDeleted == false && x.ProviderId == 10);
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
            return Json(data);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RFXLRequest(RFXLImportViewModel vm)
        {
            var customer = db.M_Customer.Find(vm.CustomerId);
            var provider = db.M_Providers.Where(x => x.Id == customer.ProviderId).FirstOrDefault();
            double sid = Convert.ToDouble(customer.SenderId);
            customer.SenderId = Convert.ToString(sid + 1);
            if (vm.Quantity > 0)
            {
                RfxcelServices r1 = new RfxcelServices();
                EPCISConfig gln = new EPCISConfig();
              
                var data = r1.getSerialNumbersFromRfxcel(customer.APIUrl, customer.SenderId, customer.ReceiverId, vm.Quantity, vm.GTIN, gln.GetEPCGLN(customer.Id));
                
                var errs = r1.getErrors();
                
                if (errs.Count() > 0)
                {
                    string err = "";
                    foreach (var item in errs)
                    {
                        err += item;
                    }
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataRFXLServerError + err;
                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataRFXLImportError + " " + err, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterservererrors + " " + err, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                    return RedirectToAction("Index");
                }
                if (data.Count > 0)
                {

                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    M_TracelinkRequest req = new M_TracelinkRequest();
                    req.CustomerId = vm.CustomerId;
                    req.GTIN = vm.GTIN;
                    req.Quatity = vm.Quantity;
                    req.RequestedOn = DateTime.Now;
                    req.IsDeleted = false;
                    req.ProviderId = provider.Id;
                    db.M_TracelinkRequest.Add(req);
                    db.SaveChanges();


                    foreach (var item in data)
                    {
                        item.TLRequestId = req.Id;
                    }
                    var convertedData = DataLayer.GeneralDataHelper.convertToDataTable(data);
                    BulkDataHelper dataHlpr = new BulkDataHelper();
                    if (dataHlpr.InsertTracelinkUIDIdenties(convertedData))
                    {
                        TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterDataimported + " " + convertedData.Rows.Count + TnT.LangResource.GlobalRes.TempDataExporterSerialNoImpo;
                        trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImporterDataimported + " " + convertedData.Rows.Count + TnT.LangResource.GlobalRes.TempDataExporterSerialNoImpoGTIN + vm.GTIN, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterDataimported + " " + convertedData.Rows.Count + TnT.LangResource.GlobalRes.TempDataExporterSerialNoImpoGTIN + vm.GTIN, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                    }
                    else
                    {
                        TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImporterUnablestore;
                        trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImporterUnablestore, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImporterUnablestore, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                    }

                }
            }
            return RedirectToAction("Index");
        }


        public ActionResult UploadRFXLFile()
        {
            ViewBag.Customer = db.M_Customer.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadRFXLFile(FormCollection frm, RFXLImportViewModel vm)
        {
            RFXManager r1 = new RFXManager();
            var customer = db.M_Customer.Find(vm.CustomerId);
            var httpRequest = System.Web.HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    string filePath = "";
                    var postedFile = httpRequest.Files[file];
                    FileInfo fi = new FileInfo(postedFile.FileName);
                    string fNm = postedFile.FileName;
                    string tempPath = Server.MapPath("~/Content/ImportedProducts/");
                    if (!System.IO.Directory.Exists(tempPath))
                    {
                        System.IO.Directory.CreateDirectory(tempPath);
                    }
                    filePath = Server.MapPath("~/Content/ImportedProducts/" + fNm);
                    postedFile.SaveAs(filePath);
                    var data = r1.sendData(filePath, customer.APIUrl, customer.BizLocGLN, vm.CustomerId);
                    TempData["Success"] = data;
                }
            }

            ViewBag.Customer = db.M_Customer.ToList();
            return View();
        }

    }
}