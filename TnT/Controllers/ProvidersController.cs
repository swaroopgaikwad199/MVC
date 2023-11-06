using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.Providers;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class ProvidersController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        // GET: Providers
        public ActionResult Index()
        {
            return View(db.M_Providers.Where(x => x.IsDeleted == false).ToList());
        }

        // GET: Providers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_Providers m_Providers = db.M_Providers.Find(id);
            if (m_Providers == null)
            {
                return HttpNotFound();
            }
          
            trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailProvidersView+" " + m_Providers.Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProvidersView + " " + m_Providers.Name,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);

            return View(m_Providers);
        }

        // GET: Providers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CreatedOn,IsActive,IsDeleted")] M_Providers m_Providers)
        {
            if (ModelState.IsValid)
            {
                m_Providers.CreatedOn = DateTime.Now;
                m_Providers.IsActive = true;
                m_Providers.IsDeleted = false;                
                db.M_Providers.Add(m_Providers);
                db.SaveChanges();

                TempData["Success"] = "'" + m_Providers.Name + "' "+TnT.LangResource.GlobalRes.TempDataProviderSuccessfullycreated;
                trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailProvidercreatedprovider+" " + m_Providers.Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProvidercreatedprovider + " " + m_Providers.Name,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);

                return RedirectToAction("Index");
            }

            return View(m_Providers);
        }

        // GET: Providers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_Providers m_Providers = db.M_Providers.Find(id);
            if (m_Providers == null)
            {
                return HttpNotFound();
            }
            return View(m_Providers);
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CreatedOn,IsActive,IsDeleted")] M_Providers m_Providers)
        {
            if (ModelState.IsValid)
            {
                var prov = db.M_Providers.Find(m_Providers.Id);
                prov.Name = m_Providers.Name;
                prov.IsActive = m_Providers.IsActive;

                db.Entry(prov).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "'" + m_Providers.Name + "' "+TnT.LangResource.GlobalRes.TempDataProvidersuccessfullyupdated;
                trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailProviderupdatedprovider+" " + m_Providers.Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProviderupdatedprovider + " " + m_Providers.Name,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return RedirectToAction("Index");
            }
            return View(m_Providers);
        }

        // GET: Providers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_Providers m_Providers = db.M_Providers.Find(id);
            if (m_Providers == null)
            {
                return HttpNotFound();
            }
            return View(m_Providers);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var prov = db.M_Providers.Find(id);
            prov.IsDeleted = true;
            db.Entry(prov).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Success"] = "'" + prov.Name + "' "+TnT.LangResource.GlobalRes.TempDataProviderDeletedsuccessfully;
            trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailProviderdeleted+" " + prov.Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailProviderdeleted + " " + prov.Name,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            return RedirectToAction("Index");
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
