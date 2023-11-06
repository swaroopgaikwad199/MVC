using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer.Security;
using TnT.DataLayer.Tracer;
using TnT.Models;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class TracerController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Tracer 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Transfer()
        {
            ViewBag.Jobs = db.Job.Where(x=>x.JobStatus==3);
            return View();
        }

        [HttpPost]
        public ActionResult Transfer(FormCollection collection)
        {
            string data = collection["JID"];
            decimal sdf = Convert.ToDecimal(data);


            DataTransferUtil utl = new DataTransferUtil();
            var res = utl.TransferBatch(sdf);
            if (res)
            {

                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTracerSuccessFullyTransfertoGloble;
            }
            else
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataTracerFailureOCcuredAdministrator;
            }
            return RedirectToAction("Transfer");
        }
        

    
    }
}
