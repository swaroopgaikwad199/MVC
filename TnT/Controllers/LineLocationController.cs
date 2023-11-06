using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.Job;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class LineLocationController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();

        // GET: LineLocation
        public ActionResult Index()
        {
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailLineLocationViewLine, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailLineLocationViewLine, TnT.LangResource.GlobalRes.TrailInfoLineActivity);
            return View(db.LineLocation.Where(x => x.IsActive == true).ToList());
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LineLocation m_LineLocation = db.LineLocation.Find(id);
            if (m_LineLocation == null)
            {
                return HttpNotFound();
            }
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailLineLocationViewDetail + m_LineLocation.ID, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailLineLocationViewDetail + m_LineLocation.ID, TnT.LangResource.GlobalRes.TrailInfoLineActivity);
            return View(m_LineLocation);
        }

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LocationCode,DivisionCode,PlantCode,LineCode,LineIP,DBName,ServerName,SQLUsername,SQLPassword,ReadGLN,LineName")]LineLocation m_LineLocation)
        {
            if (ModelState.IsValid)
            {

                LineLocation m_LineLocation1 = db.LineLocation.Find(m_LineLocation.ID);
                if (m_LineLocation1 != null)
                {
                    TempData["Error"] = TnT.LangResource.GlobalRes.TempDataLineLocationalreadyexist;
                }
                else
                {
                    m_LineLocation.IsActive = true;
                    m_LineLocation.GLNExtension = "0";
                    db.LineLocation.Add(m_LineLocation);
                    db.SaveChanges();
                    TempData["Success"] = "'" + m_LineLocation.ID + "' " + TnT.LangResource.GlobalRes.TrailLineLocationsuccessfullycreated;
                    trail.AddTrail(m_LineLocation.ID + "' " + TnT.LangResource.GlobalRes.TrailLineLocationsuccessfullycreated, Convert.ToInt32(User.ID), m_LineLocation.ID + "' " + TnT.LangResource.GlobalRes.TrailLineLocationsuccessfullycreated, TnT.LangResource.GlobalRes.TrailInfoLineActivity);
                }
                return RedirectToAction("Index");

            }
            return View(m_LineLocation);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineLocation m_LineLocation = db.LineLocation.Find(id);
            if (m_LineLocation == null)
            {
                return HttpNotFound();
            }

            return View(m_LineLocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LocationCode,DivisionCode,PlantCode,LineCode,LineIP,DBName,ServerName,SQLUsername,SQLPassword,ReadGLN,LineName")]LineLocation m_LineLocation)
        {
            if (ModelState.IsValid)
            {
                var prevDtls = db.LineLocation.Find(m_LineLocation.ID);
                prevDtls.LocationCode = m_LineLocation.LocationCode;
                prevDtls.DivisionCode = m_LineLocation.DivisionCode;
                prevDtls.PlantCode = m_LineLocation.PlantCode;
                prevDtls.LineCode = m_LineLocation.LineCode;

                prevDtls.LineIP = m_LineLocation.LineIP;
                prevDtls.DBName = m_LineLocation.DBName;
                prevDtls.ServerName = m_LineLocation.ServerName;
                prevDtls.SQLUsername = m_LineLocation.SQLUsername;
                prevDtls.SQLPassword = m_LineLocation.SQLPassword;
                prevDtls.ReadGLN = m_LineLocation.ReadGLN;
                prevDtls.LineName = m_LineLocation.LineName;
                db.Entry(prevDtls).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "'" + m_LineLocation.ID + "' " + TnT.LangResource.GlobalRes.TrailLineLocationsuccessfullyupdated;
                trail.AddTrail("'" + m_LineLocation.ID + "' " + TnT.LangResource.GlobalRes.TrailLineLocationsuccessfullyupdated, Convert.ToInt32(User.ID), "'" + m_LineLocation.ID + "' " + TnT.LangResource.GlobalRes.TrailLineLocationsuccessfullyupdated, TnT.LangResource.GlobalRes.TrailInfoLineActivity);
                return RedirectToAction("Index");
            }

            return View(m_LineLocation);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineLocation m_LineLocation = db.LineLocation.Find(id);
            if (m_LineLocation == null)
            {
                return HttpNotFound();
            }
            return View(m_LineLocation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LineLocation m_LineLocation = db.LineLocation.Find(id);
            m_LineLocation.IsActive = false;
            db.Entry(m_LineLocation).State = EntityState.Modified;
            db.SaveChanges();

            TempData["Success"] = "'" + m_LineLocation.ID + "' " + TnT.LangResource.GlobalRes.TrailLineLocationsuccessfullydeleted;
            trail.AddTrail("'" + m_LineLocation.ID + "' " + TnT.LangResource.GlobalRes.TrailLineLocationsuccessfullydeleted, Convert.ToInt32(User.ID), "'" + m_LineLocation.ID + "' " + TnT.LangResource.GlobalRes.TrailLineLocationsuccessfullydeleted, TnT.LangResource.GlobalRes.TrailInfoLineActivity);
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