 using TnT.DataLayer.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class QAHomeController : BaseController
    {
        // GET: QAHome
        public ActionResult Index()
        {
            return View();
        }
    }
}