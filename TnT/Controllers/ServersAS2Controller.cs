using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer.AS2;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.AS2;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class ServersAS2Controller : BaseController
    {
        private Trails trail = new Trails();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ServersAS2
        public ActionResult Index()
        {
            var trlmsg = User.FirstName + " " + TnT.LangResource.GlobalRes.TrailMsgServerAs2ReqtoAs2;
            trail.AddTrail(trlmsg, Convert.ToInt32(User.ID), trlmsg, TnT.LangResource.GlobalRes.TrailActionAS2ServertActivity);
            return View(db.M_ServersAS2.ToList());
        }

        // GET: ServersAS2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_ServersAS2 m_ServersAS2 = db.M_ServersAS2.Find(id);
            var trlmsg = User.FirstName + " " + TnT.LangResource.GlobalRes.TrailMsgServerAs2ReqtoAs2 + " : "+ m_ServersAS2.Name;
            trail.AddTrail(trlmsg, Convert.ToInt32(User.ID), trlmsg, TnT.LangResource.GlobalRes.TrailActionAS2ServertActivity);
            if (m_ServersAS2 == null)
            {
                return HttpNotFound();
            }
            return View(m_ServersAS2);
        }

        // GET: ServersAS2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServersAS2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,HostAddress,HostPort,FromName,ToName,HostPublicKeyFile,SelfPublicKeyFile,SelfPrivateKeyFile,SelfPrivateKeyPassword,IsActive")] M_ServersAS2 m_ServersAS2)
        {
            try
            {
                var httpRequest = System.Web.HttpContext.Current.Request;
                if (httpRequest.Files.Count != 3)
                {
                    ModelState.AddModelError("", TnT.LangResource.GlobalRes.ModelStateServerAs2FileNtInclude);
                    return View(m_ServersAS2);
                }

                var SelfPublicKeyFile = httpRequest.Files["SelfPublicKeyFile"];
                var SelfPrivateKeyFile = httpRequest.Files["SelfPrivateKeyFile"];
                var HostPublicKeyFile = httpRequest.Files["HostPublicKeyFile"];
                string serverReqFiles = Server.MapPath("~/Content/As2/" + m_ServersAS2.Name + "/");
                if (!System.IO.Directory.Exists(serverReqFiles))
                {
                    System.IO.Directory.CreateDirectory(serverReqFiles);
                }


                m_ServersAS2.SelfPublicKeyPath = serverReqFiles + SelfPublicKeyFile.FileName;
                m_ServersAS2.SelfPrivateKeyPath = serverReqFiles + SelfPrivateKeyFile.FileName;
                m_ServersAS2.HostPublicKeyPath = serverReqFiles + HostPublicKeyFile.FileName;
                ModelState.Remove("SelfPublicKeyPath");
                ModelState.Remove("SelfPrivateKeyPath");
                ModelState.Remove("HostPublicKeyPath");
                if (ModelState.IsValid)
                {

                    SelfPublicKeyFile.SaveAs(m_ServersAS2.SelfPublicKeyPath);
                    SelfPrivateKeyFile.SaveAs(m_ServersAS2.SelfPrivateKeyPath);
                    HostPublicKeyFile.SaveAs(m_ServersAS2.HostPublicKeyPath);

                    db.M_ServersAS2.Add(m_ServersAS2);
                    db.SaveChanges();
                    var trlmsg = User.FirstName + " " + TnT.LangResource.GlobalRes.TrailMsgServerAs2Addeddetails + m_ServersAS2.Name;
                    trail.AddTrail(trlmsg, Convert.ToInt32(User.ID), trlmsg, TnT.LangResource.GlobalRes.TrailActionAS2ServertActivity);
                    TempData["Success"] = " '" + m_ServersAS2.Name + "' " + TnT.LangResource.GlobalRes.TempDataAS2createdsuccessfully;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(m_ServersAS2);
            }

            return View(m_ServersAS2);
        }

        // GET: ServersAS2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            M_ServersAS2 m_ServersAS2 = db.M_ServersAS2.Find(id);
            var trlmsg = User.FirstName + " " + TnT.LangResource.GlobalRes.TrailMsgServerAs2Reqtoeditservr + m_ServersAS2.Name;
            trail.AddTrail(trlmsg, Convert.ToInt32(User.ID), trlmsg, TnT.LangResource.GlobalRes.TrailActionAS2ServertActivity);


            if (m_ServersAS2 == null)
            {
                return HttpNotFound();
            }
            return View(m_ServersAS2);
        }

        // POST: ServersAS2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,HostAddress,HostPort,FromName,ToName,HostPublicKeyPath,SelfPublicKeyPath,SelfPrivateKeyPath,SelfPrivateKeyPassword,IsActive")] M_ServersAS2 m_ServersAS2)
        {
            try
            {
                var httpRequest = System.Web.HttpContext.Current.Request;
                if (httpRequest.Files.Count != 3)
                {
                    ModelState.AddModelError("", TnT.LangResource.GlobalRes.ModelStateServerAs2FileNtInclude);
                    return View(m_ServersAS2);
                }

              
                var SelfPublicKeyFile = httpRequest.Files["SelfPublicKeyFile"];
                var SelfPrivateKeyFile = httpRequest.Files["SelfPrivateKeyFile"];
                var HostPublicKeyFile = httpRequest.Files["HostPublicKeyFile"];
                string serverReqFiles = Server.MapPath("~/Content/As2/" + m_ServersAS2.Name + "/");

                var pevServs = db.M_ServersAS2.Find(m_ServersAS2.Id);
                if (!System.IO.Directory.Exists(serverReqFiles))
                {
                    System.IO.Directory.CreateDirectory(serverReqFiles);
                }


                m_ServersAS2.SelfPublicKeyPath = serverReqFiles + SelfPublicKeyFile.FileName;
                m_ServersAS2.SelfPrivateKeyPath = serverReqFiles + SelfPrivateKeyFile.FileName;
                m_ServersAS2.HostPublicKeyPath = serverReqFiles + HostPublicKeyFile.FileName;
                //ModelState.Remove("SelfPublicKeyPath");
                //ModelState.Remove("SelfPrivateKeyPath");
                //ModelState.Remove("HostPublicKeyPath");
                if (ModelState.IsValid)
                {

                    SelfPublicKeyFile.SaveAs(m_ServersAS2.SelfPublicKeyPath);
                    SelfPrivateKeyFile.SaveAs(m_ServersAS2.SelfPrivateKeyPath);
                    HostPublicKeyFile.SaveAs(m_ServersAS2.HostPublicKeyPath);

                    db.Entry(m_ServersAS2).State = EntityState.Modified;
                    db.SaveChanges();
                    var trlMsg = User.FirstName + " " + TnT.LangResource.GlobalRes.TrailMsgUpdatedserver + pevServs.Name + " .";
                    trail.AddTrail(trlMsg, Convert.ToInt32(User.ID),trlMsg, TnT.LangResource.GlobalRes.TrailActionAS2ServertActivity);
                    TempData["Success"] = " '" + m_ServersAS2.Name + "' " + TnT.LangResource.GlobalRes.TempDataAS2updatedsuccessfully;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(m_ServersAS2);
            }

            return View(m_ServersAS2);
            //if (ModelState.IsValid)
            //{
            //    db.Entry(m_ServersAS2).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(m_ServersAS2);
        }

        //// GET: ServersAS2/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    M_ServersAS2 m_ServersAS2 = db.M_ServersAS2.Find(id);
        //    if (m_ServersAS2 == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(m_ServersAS2);
        //}

        //// POST: ServersAS2/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    M_ServersAS2 m_ServersAS2 = db.M_ServersAS2.Find(id);
        //    db.M_ServersAS2.Remove(m_ServersAS2);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}





        public ActionResult Send()
        {
            BindData();
            var trlmsg = User.FirstName + TnT.LangResource.GlobalRes.TrailMsgReqtoViewUpdatePlatform;
            trail.AddTrail(trlmsg, Convert.ToInt32(User.ID), trlmsg, TnT.LangResource.GlobalRes.TrailActionAS2ServertActivity);

            return View();
        }
        private void BindData()
        {
            ViewBag.As2Servers = db.M_ServersAS2.Where(x => x.IsActive == true);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                string as2FileUploadPath = "~/Content/As2/";

                try
                {
                    var serverDtls = db.M_ServersAS2.Find(model.ServerId);
                    if (serverDtls == null)
                    {
                        ModelState.AddModelError("", TnT.LangResource.GlobalRes.TrailMsgserverNotAvailable);
                        BindData();
                        return View(model);
                    }
                    var httpRequest = System.Web.HttpContext.Current.Request;
                    int serverId = model.ServerId;
                    var epcisFile = httpRequest.Files["epcisfile"];
                    if (epcisFile == null)
                    {
                        ModelState.AddModelError("", TnT.LangResource.GlobalRes.ModelStateServerAs2EpcisFileNtProvide);
                        BindData();
                        return View(model);
                    }

                    var docfiles = new List<string>();


                    string filePath = "";

                    FileInfo fi = new FileInfo(epcisFile.FileName);                    
                    string tempPath = as2FileUploadPath + serverDtls.Name + "//EpcisUploadedFiles//";
                    filePath = Server.MapPath(tempPath + epcisFile.FileName);
                    if (!System.IO.Directory.Exists(filePath)) { System.IO.Directory.CreateDirectory(Path.GetDirectoryName(filePath)); }
                    epcisFile.SaveAs(filePath);

                    AS2Sender as2Con = new AS2Sender();
                    var stats = as2Con.uploadDataToAS2Server(filePath, serverId);
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataServerAs2UploadStatus + stats;

                    var trlmsg = User.FirstName + TnT.LangResource.GlobalRes.TempDataServerAs2UploadedFile + serverDtls.Name + ". " + TnT.LangResource.GlobalRes.TempDataServerAs2UploadedFile + stats + " .";
                    trail.AddTrail(trlmsg, Convert.ToInt32(User.ID), trlmsg, TnT.LangResource.GlobalRes.TrailActionAS2ServertActivity);
                    
                    return RedirectToAction("Index");


                }
                catch (Exception ex)
                {
                    BindData();
                    ModelState.AddModelError("", ex.Message);
                    var trlmsg = User.FirstName + TnT.LangResource.GlobalRes.TempDataServerAs2AccrossError + ex.Message;
                    trail.AddTrail(trlmsg, Convert.ToInt32(User.ID), trlmsg, TnT.LangResource.GlobalRes.TrailActionAS2ServertActivity);
                    return View(model);
                }

            }
            BindData();
            return View(model);
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
