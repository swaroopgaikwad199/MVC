using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer.Trailings;

namespace TnT.Controllers
{
    public class ErrorController : BaseController
    {
        private Trails trail = new Trails();
        // GET: Error
        

        public ActionResult HError()
        {
            trail.AddTrail(TnT.LangResource.GlobalRes.TrailErrorOccured, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TrailErrorOccured,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            return View();
        }
    }
}