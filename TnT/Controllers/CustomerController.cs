using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;

using System.Web.Mvc;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.Customer;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class CustomerController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        // GET: Customer
        public ActionResult Index()
        {
            return View(db.M_Customer.Where(x => x.IsDeleted == false).ToList());
        }
        [HttpPost]
        public ActionResult getStateOrRegion(int Conid)
        {

            var data = (from st in db.S_State join con in db.Country on st.CountryID equals con.Id where st.CountryID == Conid select new { st.ID, st.StateName }).Distinct().ToList();

            return Json(data);
        }

        private void Bind()
        {
            ViewBag.Provider = db.M_Providers.Where(x => x.IsDeleted == false);
            ViewBag.Country = db.Country;
            ViewBag.StateOrRegion =new SelectList(db.S_State, "ID","StateName");
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_Customer m_Customer = db.M_Customer.Find(id);
            if (m_Customer == null)
            {
                return HttpNotFound();
            }
            return View(m_Customer);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            Bind();
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyName,ContactPerson,ContactNo,Email,IsActive,Address,Country,APIUrl,APIKey,SenderId,ProviderId,ReceiverId,IsSSCC,stateOrRegion,city,street1,street2,CompanyCode,BizLocGLN,BizLocGLN_Ext,License,LicenseAgency,LicenseState,postalCode,Host,HostPswd,HostPort,HostUser,ToCorpID,SSCCExt,LoosExt,FilterValue")] M_Customer m_Customer)
        {
            if (ModelState.IsValid)
            {

                m_Customer.IsActive = true;
                m_Customer.CreatedBy = Convert.ToInt32(User.ID); //getUserId()
                m_Customer.CreatedOn = DateTime.Now;
                m_Customer.LastModified = DateTime.Now;
                m_Customer.ModifiedBy = Convert.ToInt32(User.ID);
                m_Customer.IsDeleted = false;
                db.M_Customer.Add(m_Customer);
                db.SaveChanges();
               
                TempData["Success"] = "'" + m_Customer.CompanyName + "' "+TnT.LangResource.GlobalRes.TempDataCustomerCreate;
                trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailCustomerCreated+" " + m_Customer.CompanyName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailCustomerCreated + " " + m_Customer.CompanyName,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return RedirectToAction("Index");
            }
            Bind();
            return View(m_Customer);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_Customer m_Customer = db.M_Customer.Find(id);
            if (m_Customer == null)
            {
                return HttpNotFound();
            }
            Bind();
            ViewBag.Country = new SelectList(db.Country, "ID", "CountryName", m_Customer.Country);
            var data = db.S_State.Where(x => x.CountryID == m_Customer.Country);
            ViewBag.StateOrRegion = new SelectList(data, "ID", "StateName", m_Customer.stateOrRegion);
            
            return View(m_Customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyName,ContactPerson,ContactNo,Email,IsActive,Address,Country,APIUrl,APIKey,SenderId,ProviderId,ReceiverId,IsSSCC,stateOrRegion,city,street1,street2,CompanyCode,BizLocGLN,BizLocGLN_Ext,License,LicenseAgency,LicenseState,postalCode,Host,HostPswd,HostPort,HostUser,ToCorpID,LoosExt,SSCCExt,FilterValue")] M_Customer m_Customer)
        {
            if (ModelState.IsValid)
            {
                List<string> compare = new List<string>();
                compare.Add("Proivder");
                compare.Add("M_Country");
                compare.Add("M_State");
                compare.Add("CreatedOn");
                compare.Add("CreatedBy");
                compare.Add("ModifiedBy");
                compare.Add("IsDeleted");
                compare.Add("LastModified");
                int custid = Convert.ToInt32(m_Customer.Id);
                var oldcust = db.M_Customer.Where(x => x.Id == custid).FirstOrDefault();
                System.Reflection.PropertyInfo[] properties = oldcust.GetType().GetProperties();
                string msg = "";
                foreach (var oProperty in properties)
                {
                   
                        if (!compare.Contains(oProperty.Name))
                        {
                            var oOldValue = oProperty.GetValue(oldcust, null);
                            var oNewValue = oProperty.GetValue(m_Customer, null);
                            // this will handle the scenario where either value is null

                            if (!object.Equals(oOldValue, oNewValue))
                            {
                                // Handle the display values when the underlying value is null
                                var sOldValue = oOldValue == null ? "null" : oOldValue.ToString();
                                var sNewValue = oNewValue == null ? "null" : oNewValue.ToString();
                            if (oProperty.Name == "Country")
                            {
                                int oldcon = Convert.ToInt32(oOldValue);
                                if (oldcon != 0)
                                {
                                    sOldValue = db.Country.Where(x => x.Id == oldcon).Select(x => x.CountryName).FirstOrDefault().ToString();
                                }

                                int newcon = Convert.ToInt32(sNewValue);
                                if (newcon != 0)
                                {
                                    sNewValue = db.Country.Where(x => x.Id == newcon).Select(x => x.CountryName).FirstOrDefault().ToString();
                                }
                            }

                            if (oProperty.Name == "stateOrRegion")
                            {
                                int oldstat = Convert.ToInt32(oOldValue);
                                if (oldstat != 0)
                                {
                                    sOldValue = db.S_State.Where(x => x.ID == oldstat).Select(x => x.StateName).FirstOrDefault().ToString();
                                }

                                int newtstat = Convert.ToInt32(sNewValue);
                                if (newtstat != 0)
                                {
                                    sNewValue = db.S_State.Where(x => x.ID == newtstat).Select(x => x.StateName).FirstOrDefault().ToString();
                                }
                            }

                            if (oProperty.Name == "ProviderId")
                            {
                                int oldprov = Convert.ToInt32(oOldValue);
                                if (oldprov != 0)
                                {
                                    sOldValue = db.M_Providers.Where(x => x.Id == oldprov).Select(x => x.Name).FirstOrDefault().ToString();
                                }

                                int newprov = Convert.ToInt32(sNewValue);
                                if (newprov != 0)
                                {
                                    sNewValue = db.M_Providers.Where(x => x.Id == newprov).Select(x => x.Name).FirstOrDefault().ToString();
                                }
                            }

                            //msg += oProperty.Name + " was: " + sOldValue + "; is changed to: " + sNewValue + " ,";
                            msg += oProperty.Name + " "+ TnT.LangResource.GlobalRes.RptAuditTrailWas + " : " + sOldValue + ";" +" "+TnT.LangResource.GlobalRes.RptAuditTrailIsChngeTo + " : " + sNewValue + " ,";
                        }
                        }
                  
                }

                msg = msg.TrimEnd(',');
                if(msg=="")
                {
                    msg = User.FirstName + " " + TnT.LangResource.GlobalRes.TrailCustomerUpdated;
                }
                else
                {
                    msg += TnT.LangResource.GlobalRes.RptAuditTrailUsersFor + " :";
                }
                var prevDtls = db.M_Customer.Find(m_Customer.Id);
                prevDtls.CompanyName = m_Customer.CompanyName;
                prevDtls.ContactPerson = m_Customer.ContactPerson;
                prevDtls.ContactNo = m_Customer.ContactNo;
                prevDtls.Email = m_Customer.Email;
                prevDtls.IsActive = m_Customer.IsActive;
                prevDtls.Address = m_Customer.Address;
                prevDtls.Country = m_Customer.Country;
                prevDtls.APIKey = m_Customer.APIKey;
                prevDtls.SenderId = m_Customer.SenderId;
                prevDtls.ReceiverId = m_Customer.ReceiverId;
                prevDtls.APIUrl = m_Customer.APIUrl;
                prevDtls.LastModified = DateTime.Now;
                prevDtls.ProviderId = m_Customer.ProviderId;
                prevDtls.ModifiedBy = Convert.ToInt32(User.ID);
                prevDtls.IsDeleted = false;
                prevDtls.IsSSCC = m_Customer.IsSSCC;
                prevDtls.stateOrRegion = m_Customer.stateOrRegion;
                prevDtls.city = m_Customer.city;
                prevDtls.CompanyCode = m_Customer.CompanyCode;
                prevDtls.postalCode = m_Customer.postalCode;
                prevDtls.street1 = m_Customer.street1;
                prevDtls.street2 = m_Customer.street2;
                prevDtls.BizLocGLN = m_Customer.BizLocGLN;
                prevDtls.BizLocGLN_Ext = m_Customer.BizLocGLN_Ext;
                prevDtls.License = m_Customer.License;
                prevDtls.LicenseAgency = m_Customer.LicenseAgency;
                prevDtls.LicenseState = m_Customer.LicenseState;
                prevDtls.Host = m_Customer.Host;
                prevDtls.HostUser = m_Customer.HostUser;
                prevDtls.HostPswd= m_Customer.HostPswd;
                prevDtls.HostPort = m_Customer.HostPort;
                prevDtls.ToCorpID = m_Customer.ToCorpID;
                prevDtls.SSCCExt = m_Customer.SSCCExt;
                prevDtls.LoosExt = m_Customer.LoosExt;
                prevDtls.FilterValue = m_Customer.FilterValue;
                db.Entry(prevDtls).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Success"] = "'" + m_Customer.CompanyName + "' "+TnT.LangResource.GlobalRes.TempDataCustomerUpdated;
                trail.AddTrail(msg+" " + m_Customer.CompanyName, Convert.ToInt32(User.ID), msg + " " + m_Customer.CompanyName,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return RedirectToAction("Index");
            }
            Bind();
            ViewBag.Country = new SelectList(db.Country, "ID", "CountryName", m_Customer.Country);
            var data = db.S_State.Where(x => x.CountryID == m_Customer.Country);
            ViewBag.StateOrRegion = new SelectList(data, "ID", "StateName", m_Customer.stateOrRegion);
            return View(m_Customer);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_Customer m_Customer = db.M_Customer.Find(id);
            if (m_Customer == null)
            {
                return HttpNotFound();
            }
            return View(m_Customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            M_Customer m_Customer = db.M_Customer.Find(id);
            m_Customer.IsDeleted = true;
            db.Entry(m_Customer).State = EntityState.Modified;
            db.SaveChanges();

            TempData["Success"] = "'" + m_Customer.CompanyName + "' "+TnT.LangResource.GlobalRes.TempDataCustomerDeleted;
            trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailCustomerDeleted+" " + m_Customer.CompanyName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailCustomerDeleted + " " + m_Customer.CompanyName,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
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
