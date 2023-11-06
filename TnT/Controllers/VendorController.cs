using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TnT.Models;
using TnT.Models.Vendor;
using TnT.DataLayer;
using TnT.Models.Code;
using TnT.Models.Job;
using TnT.Models.Vendor.ViewModels;
using TnT.DataLayer.EmailService;
using TnT.Models.Product;
using TnT.DataLayer.Trailings;
using TnT.DataLayer.Security;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class VendorController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();

        [HttpGet]
        public ActionResult VCreate()
        {

            return PartialView();
        }


        // GET: Vendor
        #region Notify
        public ActionResult Notify()
        {
            BindDDL();
            return View();
        }
        private void BindDDL()
        {
            try
            {
                ViewBag.Jobs = db.Job.Where(m => m.VerifiedBy != null);
                ViewBag.Vendors = db.M_Vendor.Where(m => m.IsActive == true);
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        [HttpPost]
        public ActionResult getVendorData(int Id)
        {
            try
            {
                var vendor = db.M_Vendor.Find(Id);
                //object[] obj = { data, availablelevels, dataPackagingAssoDetails };
                return Json(vendor);
            }
            catch (Exception ex)
            {
                throw;                

            }       
        }


        [HttpPost]
        public ActionResult getJobData(decimal JID)
        {
            try
            {
                Job job = db.Job.Find(JID);
                var product = db.PackagingAsso.Find(job.PAID);
                object[] obj = { job, product };
                return Json(obj);
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NotifyVendor([Bind(Include = "Id,JID")] NotifyViewModel to)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Job job = db.Job.Find(to.JID);
                    PackagingAsso product = db.PackagingAsso.Find(job.PAID);
                    M_Vendor vendor = db.M_Vendor.Find(to.Id);

                    Emailer email = new Emailer();
                    string subject = "Propix Track N Trace : New Assignment Created !";
                    string content = email.Composer(product, vendor, job);
                    email.sendMail(content, subject, vendor.ContactPerson, vendor.Email, true,"","");

                    trail.AddTrail(vendor.CompanyName + " "+TnT.LangResource.GlobalRes.TempDataVendorNotifiedviaemail, Convert.ToInt32(User.ID), vendor.CompanyName + " " + TnT.LangResource.GlobalRes.TempDataVendorNotifiedviaemail,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataVendoeSuccessmailsent;
                    return RedirectToAction("Notify");
                }
                TempData["Error"] = TnT.LangResource.GlobalRes.TempDataVendorSomethingwentworng;
                return View("Notify", to);
            }
            catch (Exception ex)
            {
                BindDDL();
                TempData["Error"] =  ex.Message;
                return View("Notify", to);
            }
           
        }

        #endregion

        public ActionResult Index()
        {
            try
            {
                return View(db.M_Vendor.Where(v => v.IsActive == true).ToList());
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }
        }

        // GET: Vendor/Details/5
        public ActionResult Details(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                M_Vendor vendor = db.M_Vendor.Find(id);
                if (vendor == null)
                {
                    return HttpNotFound();
                }
                return View(vendor);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }
        }

        // GET: Vendor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vendor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyName,ContactPerson,ContactNo,Email,Address,IsActive,ServiceKey")] M_Vendor vendor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.M_Vendor.Add(vendor);
                    db.SaveChanges();
                    int id = vendor.Id;

                    string keyy = id.ToString();
                    keyy += vendor.ServiceKey;
                    vendor.ServiceKey = keyy;

                    UpdateModel(vendor);
                    db.SaveChanges();

                    trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TempDataVendorcreatedvendornamed+" " + vendor.CompanyName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataVendorcreatedvendornamed + " " + vendor.CompanyName,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                    TempData["Success"] = "'" + vendor.CompanyName + "' "+TnT.LangResource.GlobalRes.TempDataVendorcreatedsuccessfully;
                    return RedirectToAction("Index");
                }
                return View(vendor);
            }
            catch (Exception ex)
            {
                BindDDL();
                return Content(ex.Message);

            }
        }

        // GET: Vendor/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                M_Vendor vendorRegistrationViewModel = db.M_Vendor.Find(id);
                if (vendorRegistrationViewModel == null)
                {
                    return HttpNotFound();
                }
                return View(vendorRegistrationViewModel);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }
        }

        // POST: Vendor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyName,ContactPerson,ContactNo,Email,Address,IsActive,ServiceKey")] M_Vendor vendorRegistrationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(vendorRegistrationViewModel).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Success"] = "'" + vendorRegistrationViewModel.CompanyName + "' "+TnT.LangResource.GlobalRes.TempDataVendorupdatedsuccessfully;
                    return RedirectToAction("Index");
                }
                return View(vendorRegistrationViewModel);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }
        }

        // GET: Vendor/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                M_Vendor vendor = db.M_Vendor.Find(id);

                if (vendor == null)
                {
                    return HttpNotFound();
                }
                return View(vendor);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }
        }

        // POST: Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                M_Vendor vendor = db.M_Vendor.Find(id);
                vendor.IsActive = false;
                UpdateModel(vendor);

                db.SaveChanges();
                TempData["Success"] = "'" + vendor.CompanyName + "' "+TnT.LangResource.GlobalRes.TempDataVendordeletedsuccessfully;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);

            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
