using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer.Russia;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.Russia;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class RussiaController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        // GET: Russia
        public ActionResult Index()
        {
            ViewBag.job = db.Job.Where(x => x.JobStatus == 3);
            return View();
        }

        public ActionResult GenerateFile(RussiaViewModel vm)
        {
            string xml = RussiaFile(vm);
            var job = db.Job.Where(x => x.JID == vm.JID).Select(x => x.JobName).FirstOrDefault();
            if (xml != "")
            {
                byte[] fileinbyte = Encoding.ASCII.GetBytes(xml);
                string FileName = vm.FileType + "_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "_" + job+".xml";
                trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.DavaGenerated + FileName+TnT.LangResource.GlobalRes.TrailRussiaofType + vm.FileType+" for Batch "+job, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.DavaGenerated + FileName + TnT.LangResource.GlobalRes.TrailRussiaofType + vm.FileType + " for Batch " + job, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return File(fileinbyte, "application/xml", FileName);
            }
            else
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataRussiaNodatafound + vm.FileType + " File";
            }

            ViewBag.job = db.Job.Where(x => x.JobStatus == 3);
            return View("Index");
        }


        public string RussiaFile(RussiaViewModel vm)
        {
            string xml = string.Empty;
            RussiaFileGeneration rs = new RussiaFileGeneration();
            switch (vm.FileType)
            {
                case "321":
                    xml = rs.generate321(vm.JID,vm);
                    break;
                case "331":
                    xml = rs.generate331(vm.JID,vm);
                    break;
                case "332":
                    xml = rs.generate332(vm.JID,vm);
                    break;
                case "911":
                    xml = rs.generate911(vm.JID,vm.SSCC,vm);
                    break;
                case "914":
                    xml = rs.generate914(vm.JID,vm.SSCC,vm);
                    break;
                case "915":
                    xml = rs.generate915(vm.JID,vm);
                    break;

                case "213":
                    xml = rs.generate213(vm.JID,vm);
                    break;


            }

            return xml;
        }

        public ActionResult GetSSCC(int JID)
        {
            var SSCC = db.PackagingDetails.Where(x => x.JobID == JID && x.IsUsed == true && x.IsRejected == false && x.IsDecomission == false && x.SSCC != null).Select(x=>x.SSCC);
            return Json(SSCC);
        }


    }
}