using REDTR.HELPER;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web.Mvc;
using TnT.Models;
using TnT.Models.Account;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using System.Collections.Generic;


namespace TnT.Controllers
{
    public class ADSyncController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        static string baseDir = AppDomain.CurrentDomain.BaseDirectory + "ResourceSet";
        //ResourceManager rm = ResourceManager.CreateFileBasedResourceManager("18_Common", baseDir + "\\ResourceSet", null);
        CultureInfo cul = CultureInfo.CreateSpecificCulture("de");
        // GET: ADSync

        public ActionResult Index()
        {
            return RedirectToAction("Index");
        }
        public String Sync()
        {
            ADUsers hlpr = new ADUsers();
            List<TnT.Models.SettingsNUtility.Roles> LstRoles = db.Roles.Where(x => x.IsActive == true && !string.IsNullOrEmpty(x.ADRole)).ToList();
            var lstUsrs = hlpr.getADUsers(LstRoles);
            if (lstUsrs.Count > 0)
            {
                int cnt = hlpr.SyncUsers(lstUsrs);
                TempData["Success"] = cnt + " " + TnT.LangResource.GlobalRes.TempDataUsersyncedsuccessfully + " " + lstUsrs.Count;
                return TnT.LangResource.GlobalRes.TrailADSyncUsersync;
                
                //trail.AddTrail(cnt + " " + TnT.LangResource.GlobalRes.TempDataUsersyncedsuccessfully + " " + lstUsrs.Count, Convert.ToInt32(User.ID), cnt + " " + TnT.LangResource.GlobalRes.TempDataUsersyncedsuccessfully + " " + lstUsrs.Count, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            }
            else
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataUserNoUseravailabletosync;
                return TnT.LangResource.GlobalRes.TrailNoADSyncUsersync;
                //trail.AddTrail(TnT.LangResource.GlobalRes.TempDataUserNoUseravailabletosync, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataUserNoUseravailabletosync, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            }

            //return RedirectToAction("Index");
        }

        
        }
}