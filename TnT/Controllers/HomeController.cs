using TnT.DataLayer.Line;
using TnT.DataLayer.Security;
using TnT.Models;
using TnT.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer.Trailings;
using System.Globalization;
using System.Threading;

namespace TnT.Controllers
{

    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class HomeController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Demo()
        {
            return View();
        }
        public ActionResult pos()
        {
            var prod = db.PackagingAsso.Take(5);
            return View(prod);
        }
        public ActionResult Live()
        {
            trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailHomeRequestedLiveLines, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailHomeRequestedLiveLines,TnT.LangResource.GlobalRes.TrailInfoLineActivity);
            List<LiveLinesViewModel> llm;
            LineDetails ld = new LineDetails();
            llm = ld.getLiveLine();

            return View(llm);
        }


        public ActionResult LiveExecution()
         {
            //trail.AddTrail(User.FirstName + " Requested to view Live Lines ", Convert.ToInt32(User.ID));        

            LineExecutionHelper leh = new LineExecutionHelper();
            var llm = leh.getLiveLineExecution();

            return View(llm);
        }

        public ActionResult LiveLineExecution(string LineLocId)
        {
            //trail.AddTrail(User.FirstName + " Requested to view Live Lines ", Convert.ToInt32(User.ID));     
            LineExecutionHelper leh = new LineExecutionHelper();
            var llm = leh.getLiveLineExecution(LineLocId);
            return PartialView("_LineDetails", llm); // View(llm);
        }

        public ActionResult getLineChartData(string LineLocId)
        {          
            LineExecutionHelper leh = new LineExecutionHelper();
            var llm = leh.getLineChartExecution(LineLocId);
            return PartialView("_LineChart", llm); // View(llm);
        }


        public ActionResult Change(string LanguageAbbrevation)
        {
            if (LanguageAbbrevation != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(LanguageAbbrevation);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(LanguageAbbrevation);
            }
            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LanguageAbbrevation;
            //Response.Cookies.Add(cookie);
            HttpContext.Response.Cookies.Remove("Language");

            HttpContext.Response.SetCookie(cookie);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult getDashboardDetailsPichart(string month,string year)
        {

            object[] response = DataLayer.DashBoard.getDashboardData(month,year);


            return Json(response);
        }


      
    }
}