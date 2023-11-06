using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.Models;
using TnT.Models.Compliance;
using TnT.Models.Product;

namespace TnT.Controllers
{
    public class ComplianceController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Compliance
        public ActionResult Index()
        {
            return View(db.JOBTypes);
        }


        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            ComplianceViewModel cvm = new ComplianceViewModel();
            var jtype = db.JOBTypes.Find(id);
            cvm.ID = id;
            cvm.ComplianceName = jtype.Job_Type;
            cvm.Remarks = cvm.Remarks;
            List<S_JobFields> totalFields = new List<S_JobFields>();
            totalFields = db.S_JobFields.Where(x => x.IsActive == true).OrderBy(x => x.FieldName).ToList();
            List<S_JobFields> ActiveFields = new List<S_JobFields>();
            var jobFields = db.X_JobTypeFields.Where(p => p.JobTypeId == jtype.TID);
            //get active permissions
            foreach (var item in jobFields)
            {
                S_JobFields pm = new S_JobFields();
                pm = totalFields.Where(p => p.Id == item.JobFieldId).OrderBy(k => k.FieldName).FirstOrDefault();
                totalFields.Remove(pm);
                pm.IsChecked = true;
                totalFields.Insert(0, pm);

            }

            cvm.fields = totalFields;
            return View(cvm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ComplianceViewModel rvm)
        {
            var complianceFields = rvm.fields.Where(p => p.IsChecked == true);
            var jobType = db.JOBTypes.Find(rvm.ID);
            if (complianceFields.Count() == 0)
            {
                ModelState.AddModelError("NoPermission", TnT.LangResource.GlobalRes.ModelStateComplianceProvideField);
                return View(rvm);
            }

            if (ModelState.IsValid)
            {
                //Roles rle = new Roles();
                //rle.ID = Convert.ToInt32(rvm.ID);
                //rle.Roles_Name = rvm.Roles_Name;
                //rle.Remarks = "OK";
                //rle.IsActive = role.IsActive;
                //var local = db.Entry(rle).State;
                //if (local == null)
                //{
                //    db.Entry(rle).State = EntityState.Modified;
                //}
                //db.SaveChanges();

                var deleteOldRP = db.X_JobTypeFields.Where(p => p.JobTypeId == rvm.ID);
                //delete the existing permission
                db.X_JobTypeFields.RemoveRange(deleteOldRP);


                //add the new permissions
                foreach (var item in complianceFields)
                {
                    X_JobTypeFields rp = new X_JobTypeFields();
                    rp.JobTypeId = Convert.ToDecimal(rvm.ID);
                    rp.JobFieldId = item.Id;                    
                    db.X_JobTypeFields.Add(rp);
                    db.SaveChanges();

                }
                TempData["Success"] = "'" + rvm.ComplianceName+ "' " +TnT.LangResource.GlobalRes.TempDataEPCISReceiverUpdated;
                return RedirectToAction("Index");
            }
            return View(rvm);
        }


    }
}