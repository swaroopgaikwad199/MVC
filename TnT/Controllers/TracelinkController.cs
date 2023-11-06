using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer.Security;
using TnT.DataLayer.TracelinkService;
using TnT.Models;
using TnT.Models.Tracelink;
using TnT.Models.TraceLinkImporter;
using TnT.Models.Customer;
using System;
using TnT.DataLayer.Trailings;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class TracelinkController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Tracelink
        private Trails trail = new Trails();
        #region Generate Files
        public ActionResult ExportFiles()
        {
            BindFields();
            return View();
        }

        public void BindFields()
        {
          
            //customer -> provider -> traelink
            //select jobs where Tracelink is Provider
            ViewBag.SOM = db.M_SOM.Where(x => x.IsDeleted == false);
            ViewBag.Batches = db.Job.Where(x => x.JobStatus == 3 && (x.ProviderId == 2||x.ProviderId==4));
            ViewBag.Type = getFileTypes();
        }

        public List<string> getFileTypes()
        {
            List<string> ftypes = new List<string>();
          
            ftypes.Add("Disposition Assign");
            ftypes.Add("Disposition Update");
            ftypes.Add("SOM");
            return ftypes;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generate(TLFileExportsViewModel vm)
        {
            if (vm.JobId == 0)
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTracelinkBatchnotprovided;
                trail.AddTrail(TnT.LangResource.GlobalRes.TempDataTracelinkBatchnotprovided, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataTracelinkBatchnotprovided, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                return RedirectToAction("ExportFiles");
            }
            TLFileTypes ftyp = TLFileTypes.SOMFile;
            
            if (!string.IsNullOrEmpty(vm.FileType))
            {
                if (vm.FileType == "Disposition Assign")
                {
                    ftyp = TLFileTypes.DispositionFile;
                }
                else if (vm.FileType == "Disposition Update")
                {
                    ftyp = TLFileTypes.DispositionUpdateFile;
                }
                else if(vm.FileType == "SOM")
                {
                    ftyp = TLFileTypes.SOMFile;
                }
            }
            else
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTracelinkFilenotprovided;
                trail.AddTrail(TnT.LangResource.GlobalRes.TempDataTracelinkFilenotprovided, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataTracelinkBatchnotprovided, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                return RedirectToAction("ExportFiles");
            }

          
            M_SOM som = null;
            if (ftyp == TLFileTypes.SOMFile)
            {
                if (vm.SOMId == 0)
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTracelinkSOMDetailsnotprovided;
                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataTracelinkSOMDetailsnotprovided, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataTracelinkBatchnotprovided, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                    return RedirectToAction("ExportFiles");
                }
                else
                {
                    som = db.M_SOM.Find(vm.SOMId);
                }
            }

            TraceLinkUtils util = new TraceLinkUtils();
           
            var fileBytes = util.genertefileContent(ftyp,vm.JobId,som,vm.IsMoc);
            string FileName = util.getFileName();
            if (fileBytes != null)
            {
                trail.AddTrail(FileName + TnT.LangResource.GlobalRes.TempDataEPCISGeneratedSuccss, Convert.ToInt32(User.ID), FileName + TnT.LangResource.GlobalRes.TempDataEPCISGeneratedSuccss, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                return File(fileBytes, ".xml", FileName);
            }
            else
            {
                BindFields();
                return View("ExportFiles");
            }
         
        }

        decimal paid = 0;
        public ActionResult ProductXml()
        {
            ViewBag.Batches = db.Job.Where(x => x.JobStatus == 3 && (x.ProviderId == 2||x.ProviderId==4));
            ViewBag.CountryName = getCountryName();
            return View();
        }
        public ActionResult getTypeWiseProducts(decimal JobId)
        {

            var data = (from prod in db.PackagingAsso join lbl in db.Job on prod.PAID equals lbl.PAID where lbl.JID == JobId select new { prod.PAID, prod.Name }).Distinct().ToList();
           
            return Json(data);
        }
        public List<SelectListItem> getCountryName()
        {
            List<SelectListItem> Cname = new List<SelectListItem>();

            Cname.Add(new SelectListItem { Value = "IN", Text = "India" });
            Cname.Add(new SelectListItem { Value = "FR", Text = "France" });
            Cname.Add(new SelectListItem { Value = "CN", Text = "China" });
            Cname.Add(new SelectListItem { Value = "US", Text = "USA" });
            Cname.Add(new SelectListItem { Value = "DE", Text = "Germany" });

            return Cname;

         
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateProdDetails(TLProductDetailViewModel vm)
        {

           
            Models.Job.Job job = db.Job.Find(vm.JID);

            Models.SettingsNUtility.Settings setting = db.Settings.FirstOrDefault();

            Models.Product.PackagingAsso pkg = db.PackagingAsso.Find(vm.PAID);
            var customerID = db.Job.Where(jbm => jbm.JID == vm.JID).FirstOrDefault().CustomerId;
            M_Customer cm = db.M_Customer.Where(cbm => cbm.Id == customerID).FirstOrDefault();

            Models.Product.PackagingAssoDetails PkdDetails = db.PackagingAssoDetails.Where( x =>x.PackageTypeCode=="MOC" && x.PAID == vm.PAID).FirstOrDefault();


            //TraceLinkUtils util = new TraceLinkUtils();
            //if (job != null && setting != null && pkg != null && cm != null && PkdDetails != null && vm!=null)
            //{
            //    var fileBytes = util.GenerateProduct(job, setting, vm, cm, pkg,PkdDetails);
            //    string FileName = util.getFileName()+".xml";
            //    return File(fileBytes, ".xml", FileName);
            //}
            //else
            //{
            //    return null;
            //}
            if (job == null || setting == null || pkg == null || cm == null || PkdDetails == null || vm==null)
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTracelinkDetailNotProvided;
                trail.AddTrail(TnT.LangResource.GlobalRes.TempDataTracelinkDetailNotProvided, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataTracelinkBatchnotprovided, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                RedirectToAction("ProductXml");
            }
            TraceLinkUtils util = new TraceLinkUtils();
            var fileBytes = util.GenerateProduct(job, setting, vm, cm, pkg, PkdDetails);
            string date = DateTime.Now.ToString("ddMMyyyyhhmm");
            string FileName = "ProductionOrder_" + job.BatchNo + "_" + date + ".xml";
            trail.AddTrail(FileName+ TnT.LangResource.GlobalRes.TempDataEPCISGeneratedSuccss, Convert.ToInt32(User.ID), FileName + TnT.LangResource.GlobalRes.TempDataEPCISGeneratedSuccss, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
            return File(fileBytes, ".xml", FileName);


        }
        #endregion


        #region Upload Files

        public ActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        TLFileType ftype = TLFileType.None;
                        string fileType = Request["FileType"];
                        if (fileType == "SOM")
                        {
                            ftype = TLFileType.SOMFile;
                        }
                        else if (fileType == "Disposition")
                        {
                            ftype = TLFileType.Disposition;
                        }
                        else if (fileType == "DispositionUpdate")
                        {
                            ftype = TLFileType.DispositionUpdate;
                        }

                        string _fileName = Path.GetFileName(file.FileName);
                        //string _Path = Path.Combine("~/Content/Tracelink/", _fileName);
                        string _filePath = System.Web.HttpContext.Current.Server.MapPath("~/Content/Tracelink/" + _fileName);
                        file.SaveAs(_filePath);
                        TracelinkUploader upldr = new TracelinkUploader();
                        if (upldr.UploadSFTPFile(_filePath, ftype))
                        {
                            TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTracelinkSuccessfullyuploaded;
                            trail.AddTrail(TnT.LangResource.GlobalRes.TempDataTracelinkSuccessfullyuploaded, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataTracelinkBatchnotprovided, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                        }
                        else
                        {
                            var err = upldr.getError();

                            switch (err)
                            {
                                case TLUploadingErrors.NoPermmision:
                                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTracelinkNoPermmision;
                                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataTracelinkNoPermmision, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataTracelinkSelectFile, TnT.LangResource.GlobalRes.TempDataTracelinkNoPermmision);
                                    break;
                                case TLUploadingErrors.ConnectionProblem:
                                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTracelinkConnectionProblem;
                                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataTracelinkConnectionProblem, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataTracelinkSelectFile, TnT.LangResource.GlobalRes.TempDataTracelinkConnectionProblem);
                                    break;
                                case TLUploadingErrors.SSHException:
                                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTracelinkSSHException;
                                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataTracelinkSSHException, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataTracelinkSelectFile, TnT.LangResource.GlobalRes.TempDataTracelinkSSHException);
                                    break;
                                case TLUploadingErrors.Other:
                                    TempData["Success"] =TnT.LangResource.GlobalRes.TempDataTracelinkOther;
                                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataTracelinkOther, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataTracelinkSelectFile, TnT.LangResource.GlobalRes.TempDataTracelinkOther);
                                    break;

                            }
                        }

                    }
                }
                else
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTracelinkSelectFile;
                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataTracelinkSelectFile, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataTracelinkSelectFile, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                }
            }
            catch (System.Exception ex)
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTracelinkFailureoccured + " " + ex.Message;                
            }
           
            return RedirectToAction("UploadFiles");
        }
        #endregion


    }
}