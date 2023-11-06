using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.Models;
using TnT.Models.Job;

namespace TnT.Controllers
{
    public class TNTReportForTestingController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: TNTReportForTesting
        //[HttpGet]
        public ActionResult CSVFile()
        {
            ViewBag.BatchNo = db.Job.ToList().Select(s => s.BatchNo);
            ViewBag.JobName = db.Job.ToList().Select(s => s.JobName);
            var job = db.Job.ToList().Select(s =>new { s.BatchNo,s.JobName });
            return View(job);
        }
        [HttpPost]
        public ActionResult CSVFile(Job job)
        {

            return View();
        }
    }
}