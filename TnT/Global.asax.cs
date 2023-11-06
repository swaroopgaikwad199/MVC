using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Http;
using TnT.Models.Account;
using Newtonsoft.Json;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using System.Globalization;
using System.Web.Helpers;
using TnT.DataLayer;
using System.Collections.Generic;
using PTPLCRYPTORENGINE;
using TnT.Models.SettingsNUtility;

namespace TnT
{
    internal class PermssionHolder
    {
        public static List<Permissions> perms { get; set; }
    }
    public class Global : HttpApplication
    {
        decimal id;
        string uName = string.Empty;
        private Trails trail = new Trails();
        string requrl = string.Empty;
        SecurityUtils secUtils = new SecurityUtils();

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            PermssionHolder.perms = secUtils.getPermissionsList();
        }

        //void Session_Start(object sender, EventArgs e)
        //{
        //    HttpContext.Current.Session.Timeout = Convert.ToInt32(Utilities.getAppSettings("ExpirationTime"));
        //}

        protected void Application_BeginRequest()
        {


            CultureInfo info = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            info.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            System.Threading.Thread.CurrentThread.CurrentCulture = info;
            HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
            if (cookie != null && cookie.Value != null)
            {
                if (cookie.Value == "en")
                {
                    System.Globalization.CultureInfo customCulture = new System.Globalization.CultureInfo("en-GB", true);
                    System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                  
                }
                else
                {
                    System.Globalization.CultureInfo customCulture = new System.Globalization.CultureInfo("en-GB", true);
                    //System.Globalization.CultureInfo customCulture = new System.Globalization.CultureInfo("de-DE", true);
                    System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cookie.Value);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cookie.Value);
                   

                }
            }
            else
            {
                System.Globalization.CultureInfo customCulture = new System.Globalization.CultureInfo("en-GB", true);
                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
        

            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
        //protected void Application_PostAuthorizeRequest()
        //{
        //    System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        //}
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                requrl = Convert.ToString(Request.UrlReferrer);
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
               
                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name);
                newUser.ID = serializeModel.ID;
                newUser.FirstName = serializeModel.FirstName;
                newUser.roles = serializeModel.Role;
                newUser.permissions = getUserPermissions(serializeModel.Permissions); //getUserPermissions(Convert.ToInt32(newUser.ID).ToString());
                newUser.Roles_Name = serializeModel.Roles_Name;
                HttpContext.Current.User = newUser;
                id = newUser.ID;
                uName = newUser.FirstName;
              
            }
        }

        private List<string> getUserPermissions(IEnumerable<string> pems)
        {
            var allPermsIds = PermssionHolder.perms.Select(x=>x.ID).ToList();
            List<decimal> userPermsIds = pems.Select(decimal.Parse).ToList();             
            var res1 = allPermsIds.Intersect(userPermsIds);
            return PermssionHolder.perms.Where(x => userPermsIds.Contains(x.ID)).Select(x => x.Permission).ToList();
            
        }
    }


}
