using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using TnT.Models;

namespace TnT.DataLayer.Security
{

    public class CustomAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }

        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        public void getPermissions(int RoleId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var permissions = db.ROLESPermission.Where(x => x.Roles_Id == RoleId).Select(x => x.Permissions.Permission).ToList();
            //TempData["Permissions"] =  permissions;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
               
                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    //var authorizedUsers = ConfigurationManager.AppSettings[UsersConfigKey];
                    //var authorizedRoles = ConfigurationManager.AppSettings[RolesConfigKey];

                    //string[] authRoles = authorizedRoles.Split(',');
                    //Roles = CurrentUser.Roles_Name;
                    //// Users = String.IsNullOrEmpty(CurrentUser.FirstName) ? authorizedUsers : CurrentUser.FirstName;

                    ////Users = String.IsNullOrEmpty(Users) ? authorizedUsers : Users;
                    //for (int i = 0; i < authRoles.Count(); i++)
                    //{
                    //    Roles = String.IsNullOrEmpty(Roles) ? authRoles[i] : Roles;
                    //}

                    //if (!String.IsNullOrEmpty(Roles))
                    //{
                    //    //if (!CurrentUser.IsInRole(Roles))
                    //    //{
                    //        filterContext.Result = new RedirectToRouteResult(new
                    //        RouteValueDictionary(new { controller = "Account", action = "Login" }));

                    //        // base.OnAuthorization(filterContext); //returns to login url
                    //    //}
                    //}


                    if (string.IsNullOrEmpty(CurrentUser.FirstName))
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Account", action = "Login" }));
                    }

                    //int rid = ((TnT.DataLayer.Security.CustomPrincipal)((System.Web.HttpContextWrapper)filterContext.HttpContext).User).roles;
                    //getPermissions(rid);
                    //if (!String.IsNullOrEmpty(Users))
                    //{  
                    //    if (!Users.Contains(CurrentUser.FirstName))
                    //    {
                    //        filterContext.Result = new RedirectToRouteResult(new
                    //          RouteValueDictionary(new { controller = "Account", action = "Login" }));

                    //        // base.OnAuthorization(filterContext); //returns to login url
                    //    }
                    //}

                }
                else
                {
                    string url = HttpContext.Current.Request.Url.AbsoluteUri;
                    filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = url }));
                }
            }
            catch (Exception)
            {

                throw;
            }


        }


    }
}