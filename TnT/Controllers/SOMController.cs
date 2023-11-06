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
using TnT.Models.TraceLinkImporter;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class SOMController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        Trails trail = new Trails();

        // GET: SOM
        public ActionResult Index()
        {
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailSoMReqtoView, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailSoMReqtoView, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
            return View(db.M_SOM.Where(x => x.IsDeleted == false).ToList());
        }

        // GET: SOM/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_SOM m_SOM = db.M_SOM.Find(id);
            if (m_SOM == null)
            {
                return HttpNotFound();
            }
            trail.AddTrail(User.FirstName+TnT.LangResource.GlobalRes.TrailSOMReqviewDetails + m_SOM.BusinessName,Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailSOMReqviewDetails + m_SOM.BusinessName, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
            return View(m_SOM);
        }

        // GET: SOM/Create
        public ActionResult Create()
        {
            ViewBag.Country = db.Country;
            return View();
        }

        // POST: SOM/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CreatedOn,IsDeleted,BusinessId,BusinessName,Street1,City,StateOrRegion,PostalCode,Country,FacilityId_GLN,FacilityId_SGLN,SFLI_BusinessName,SFLI_Street1,SFLI_City,SFLI_StateOrRegion,SFLI_PostalCode,SFLI_Country,DeliveryNumber,DeliveryCompleteFlag,TransactionIdentifier,TransactionDate,SalesDistributionType,IsSerialized,FromBusinessPartyLookupId,ShipFromLocationLookupId,ToBusinessPartyLookupId,ShipToLocationLookupId")] M_SOM m_SOM)
        {
            if (ModelState.IsValid)
            {
                m_SOM.IsDeleted = false;
                m_SOM.CreatedOn = DateTime.Now;

                db.M_SOM.Add(m_SOM);
                db.SaveChanges();
                TempData["Success"] = TnT.LangResource.GlobalRes.TrailSomSuccessfullyadded;
                trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailSOMCreatedBusinessName + m_SOM.BusinessName, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailSOMCreatedBusinessName + m_SOM.BusinessName, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                return RedirectToAction("Index");
            }

            return View(m_SOM);
        }

        // GET: SOM/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_SOM m_SOM = db.M_SOM.Find(id);
            if (m_SOM == null)
            {
                return HttpNotFound();
            }
            ViewBag.Country_Con = new SelectList(db.Country, "CountryName", "CountryName", m_SOM.Country);
            ViewBag.Country_SFLI_Country = new SelectList(db.Country, "CountryName", "CountryName", m_SOM.SFLI_Country);
            return View(m_SOM);
        }

        // POST: SOM/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreatedOn,IsDeleted,BusinessId,BusinessName,Street1,City,StateOrRegion,PostalCode,Country,FacilityId_GLN,FacilityId_SGLN,SFLI_BusinessName,SFLI_Street1,SFLI_City,SFLI_StateOrRegion,SFLI_PostalCode,SFLI_Country,DeliveryNumber,DeliveryCompleteFlag,TransactionIdentifier,TransactionDate,SalesDistributionType,IsSerialized,FromBusinessPartyLookupId,ShipFromLocationLookupId,ToBusinessPartyLookupId,ShipToLocationLookupId")] M_SOM m_SOM)
        {
            if (ModelState.IsValid)
            {
                //var ord = db.M_SOM.Find(m_SOM.Id);
              
                db.Entry(m_SOM).State = EntityState.Modified;
                db.SaveChanges();
                trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailSOMUpdatedBusinessName + m_SOM.BusinessName, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailSOMUpdatedBusinessName + m_SOM.BusinessName, TnT.LangResource.GlobalRes.TrailActionTraceLinkModule);
                TempData["Success"] = TnT.LangResource.GlobalRes.TrailSomSuccessfullyupdated;
                return RedirectToAction("Index");
            }
            return View(m_SOM);
        }

        // GET: SOM/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_SOM m_SOM = db.M_SOM.Find(id);
            if (m_SOM == null)
            {
                return HttpNotFound();
            }
            return View(m_SOM);
        }

        // POST: SOM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            M_SOM m_SOM = db.M_SOM.Find(id);
            db.Entry(m_SOM).State = EntityState.Modified;
            db.SaveChanges();

            TempData["Success"] = TnT.LangResource.GlobalRes.TrailSomSuccessfullydeleted;
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
