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
using TnT.Models.SettingsNUtility;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class RolesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();

        // GET: Roles
        public ActionResult Index()
        {
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailRolesViewRoles, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailRolesViewRoles, TnT.LangResource.GlobalRes.TrailActionRoleActivity);
            return View(db.Roles.Where(x => x.ID != 0).ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roles roles = db.Roles.Find(id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailRolesViewDetail + " " + roles.Roles_Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailRolesViewDetail + " " + roles.Roles_Name, TnT.LangResource.GlobalRes.TrailActionRoleActivity);
            return View(roles);
        }

        // GET: Roles/CreateVM
        public ActionResult CreateVM()
        {
            RolesViewModel rvm = new RolesViewModel();
            rvm.Permissions = db.Permissions.Where(x => x.IsActive == true).OrderBy(x => x.Permission).ToList();
            return View(rvm);
        }

        // GET: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVM([Bind(Include = "Roles_Name,Remarks,Permissions")] RolesViewModel roles)
        {
            var RolePermissions = roles.Permissions.Where(p => p.IsChecked == true);
            if (RolePermissions.Count() == 0)
            {
                ModelState.AddModelError("NoPermission", TnT.LangResource.GlobalRes.AddModuleErrorRolesProvidepermission);
                return View(roles);
            }

            if (ModelState.IsValid)
            {



                Roles rle = new Roles();
                rle.Roles_Name = roles.Roles_Name;
                rle.Remarks = "OK";
                rle.IsActive = true;
                db.Roles.Add(rle);
                db.SaveChanges();

                //Permision Selected
                foreach (var item in RolePermissions)
                {
                    ROLESPermission rp = new ROLESPermission();
                    rp.Roles_Id = rle.ID;
                    rp.Permission_Id = item.ID;
                    rp.Remarks = null;

                    //save
                    db.ROLESPermission.Add(rp);
                    db.SaveChanges();

                }
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataRoleCreatedRole + " " + roles.Roles_Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataRoleCreatedRole + " " + roles.Roles_Name, TnT.LangResource.GlobalRes.TrailActionRoleActivity);
                TempData["Success"] = "'" + roles.Roles_Name + "' " + TnT.LangResource.GlobalRes.TrailRoleCreatesuccessfully;
                return RedirectToAction("Index");

            }
            return View(roles);
        }


        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolesViewModel rvm = new RolesViewModel();
            Roles roles = db.Roles.Find(id);
            rvm.ID = id;
            rvm.Roles_Name = roles.Roles_Name;
            rvm.Remarks = roles.Remarks;

            List<Permissions> totalPermissions = new List<Permissions>();
            totalPermissions = db.Permissions.Where(x => x.IsActive == true).OrderBy(x => x.Permission).ToList();

            List<Permissions> Activeprm = new List<Permissions>();

            var rolepermission = db.ROLESPermission.Where(p => p.Roles_Id == roles.ID);

            //get active permissions
            foreach (var item in rolepermission)
            {
                Permissions pm = new Permissions();
                pm = totalPermissions.Where(p => p.ID == item.Permission_Id).OrderBy(k => k.Permission).FirstOrDefault();

                if (pm != null)
                {
                    totalPermissions.Remove(pm);
                    pm.IsChecked = true;

                    totalPermissions.Insert(0, pm);
                }
                //Activeprm.Add(pm);
            }


            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailRoleupdatepermissionrole + " " + roles.Roles_Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailRoleupdatepermissionrole + " " + roles.Roles_Name, TnT.LangResource.GlobalRes.TrailActionRoleActivity);



            rvm.Permissions = totalPermissions.Where(x => x.IsActive == true).OrderBy(x => x.Permission).ToList();




            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(rvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Roles_Name,Remarks,Permissions")] RolesViewModel rvm)
        {
            var RolePermissions = rvm.Permissions.Where(p => p.IsChecked == true);
            var role = db.Roles.Where(y => y.ID == rvm.ID).FirstOrDefault();
            if (RolePermissions.Count() == 0)
            {
                ModelState.AddModelError("NoPermission", TnT.LangResource.GlobalRes.AddModuleErrorRolesProvidepermission);
                return View(rvm);
            }

            if (ModelState.IsValid)
            {
                Roles rle = new Roles();
                rle.ID = Convert.ToInt32(rvm.ID);
                rle.Roles_Name = rvm.Roles_Name;
                rle.Remarks = "OK";
                rle.IsActive = role.IsActive;
                var local = db.Entry(rle).State;
                if (local == null)
                {
                    db.Entry(rle).State = EntityState.Modified;
                }
                db.SaveChanges();

                var deleteOldRP = db.ROLESPermission.Where(p => p.Roles_Id == rvm.ID);
                //delete the existing permission
                db.ROLESPermission.RemoveRange(deleteOldRP);


                //add the new permissions
                foreach (var item in RolePermissions)
                {
                    ROLESPermission rp = new ROLESPermission();
                    rp.Roles_Id = rle.ID;
                    rp.Permission_Id = item.ID;
                    rp.Remarks = null;

                    //save
                    db.ROLESPermission.Add(rp);
                    db.SaveChanges();

                }
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailRoleupdatedRole + " " + rvm.Roles_Name, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailRoleupdatedRole + " " + rvm.Roles_Name, TnT.LangResource.GlobalRes.TrailActionRoleActivity);
                TempData["Success"] = "'" + rvm.Roles_Name + "' " + TnT.LangResource.GlobalRes.TempDataRoleupdatedsuccessfully;
                return RedirectToAction("Index");
            }
            return View(rvm);
        }


        public ActionResult Alarms(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AlarmsViewModel rvm = new AlarmsViewModel();
            Roles roles = db.Roles.Find(id);
            rvm.ID = id;
            rvm.Roles_Name = roles.Roles_Name;

            List<Alarms> lstAlrm = new List<Alarms>();
            lstAlrm = db.Alarms.Where(x => x.IsActive == true).OrderBy(x => x.Aname).ToList();


            var roleAlarm = db.RoleAlarms.Where(x => x.Role_ID == roles.ID);
            foreach (var item in roleAlarm)
            {
                Alarms pm = new Alarms();

                pm = lstAlrm.Where(p => p.ID == item.Alarm_ID).OrderBy(k => k.Aname).FirstOrDefault();
                lstAlrm.Remove(pm);
                pm.IsChecked = true;

                lstAlrm.Insert(0, pm);

                //Activeprm.Add(pm);
            }

            rvm.Alarms = lstAlrm.Where(x => x.IsActive == true).OrderBy(x => x.Aname).ToList();

            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(rvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Alarms([Bind(Include = "ID,Roles_Name,Alarms")] AlarmsViewModel rvm)
        {
            var RoleAlarms = rvm.Alarms.Where(p => p.IsChecked == true);
            var role = db.Roles.Where(y => y.ID == rvm.ID).FirstOrDefault();
            if (RoleAlarms.Count() == 0)
            {
                ModelState.AddModelError("NoPermission", TnT.LangResource.GlobalRes.AddModuleErrorRolesAlarms);
                return View(rvm);
            }

            if (ModelState.IsValid)
            {
                Roles rle = new Roles();
                rle.ID = Convert.ToInt32(rvm.ID);
                rle.Roles_Name = rvm.Roles_Name;
                rle.Remarks = "OK";
                rle.IsActive = role.IsActive;
                var local = db.Entry(rle).State;
                if (local == null)
                {
                    db.Entry(rle).State = EntityState.Modified;
                }
                db.SaveChanges();

                var deleteOldRP = db.RoleAlarms.Where(p => p.Role_ID == rvm.ID);
                //delete the existing permission
                db.RoleAlarms.RemoveRange(deleteOldRP);


                //add the new permissions
                foreach (var item in RoleAlarms)
                {
                    RoleAlarms rp = new RoleAlarms();
                    rp.Role_ID = rle.ID;
                    rp.Alarm_ID = item.ID;


                    //save
                    db.RoleAlarms.Add(rp);
                    db.SaveChanges();

                }

                TempData["Success"] = "'" + rvm.Roles_Name + "' " + TnT.LangResource.GlobalRes.TempDataRoleupdatedsuccessfully;
                return RedirectToAction("Index");
            }
            return View(rvm);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roles roles = db.Roles.Find(id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(roles);
        }



        public ActionResult Activation(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roles rls = db.Roles.Find(id);
            if (rls == null)
            {
                return HttpNotFound();
            }
            return View(rls);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activation([Bind(Include = "ID,IsActive")] Roles role)
        {
            decimal id = role.ID;
            bool IsActive = role.IsActive;
            string msg = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roles rle = db.Roles.Find(id);
            rle.IsActive = role.IsActive;

            db.Entry(rle).State = EntityState.Modified;
            db.SaveChanges();

            if (IsActive)
            {
                msg = User.FirstName + " " + TnT.LangResource.GlobalRes.TrailRoleActivatedrole + " " + rle.Roles_Name;
                trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailActionRoleActivity);
                TempData["Success"] = rle.Roles_Name + " " + TnT.LangResource.GlobalRes.TempDataRoleactivatedsuccessfully;
            }
            else
            {
                msg = User.FirstName + " " + TnT.LangResource.GlobalRes.TrailRoleDeactivated + " " + rle.Roles_Name;
                trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailActionRoleActivity);
                TempData["Success"] = rle.Roles_Name + " " + TnT.LangResource.GlobalRes.TempDataRoledeactivatedsuccessfully;
            }


            return RedirectToAction("Index");
        }

        public ActionResult IsRoleExisting(string RoleName)
        {
            try
            {
                var data = db.Roles.Where(x => x.Roles_Name == RoleName).FirstOrDefault();
                if (data != null)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: Roles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Roles roles = db.Roles.Find(id);
        //    db.Roles.Remove(roles);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
