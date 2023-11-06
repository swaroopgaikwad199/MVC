using TnT.DataLayer;
using TnT.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System.Web.Security;

using TnT.DataLayer.LicenseChecker;
using TnT.DataLayer.Trailings;
using TnT.DataLayer.Security;
using System.Configuration;
using TnT.Models;
using PTPLCRYPTORENGINE;
using REDTR.HELPER;
using System.Reflection;
using System.Collections;
using System.Web.SessionState;

namespace TnT.Controllers
{

    public class AccountController : BaseController
    {
        private Trails trail = new Trails();

        private Users tuser;
        private SecurityUtils utils = new SecurityUtils();
        private Account ac = new Account();
        private int CheckLicense()
        {
            Validator obj = new Validator();
            int val = obj.ReadLicense();
            return val;
        }
        private void PwdData()
        {
            ViewData["maxpwd"] = Utilities.getAppSettings("MaxPassword");
            ViewData["minpwd"] = Utilities.getAppSettings("MinPassword");
        }
        //[CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
        public ActionResult SetPassword(int uid)
        {
            PwdData();
            ViewData["UID"] = uid;
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reset(ResetPswViewModel vm)
        {
            PwdData();
            ApplicationDbContext db = new ApplicationDbContext();
            var u = db.Users.Find(vm.Uid);
            if (u == null)
            {
                return RedirectToAction("Login");
            }

            SecurityUtils util = new SecurityUtils();
            if (util.IsPasswordExistng(vm.Uid, vm.Password))
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataAccountdifferentpassword;
                return View("SetPassword",vm);
            }

            if (ModelState.IsValid)
            {
                REDTR.DB.BusinessObjects.Users usr = new REDTR.DB.BusinessObjects.Users();
                usr.ID = u.ID;
                usr.UserName = u.UserName;
                usr.UserName1 = u.UserName1;
                string cypher = AESCryptor.Encrypt(vm.Password.Trim(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                usr.Password = cypher;
                usr.RoleID = u.RoleID;
                usr.LastUpdatedDate = DateTime.Now;
                usr.Active = true;
                usr.IsFirstLogin = false;
                usr.UserType = u.UserType;

                DbHelper m_dbhelper = new DbHelper();
                m_dbhelper.DBManager.UsersBLL.InsertOrUpdateUsers(REDTR.DB.BLL.UsersBLL.UsersOp.UpdateUser, usr);
                //m_dbhelper.AddUserTrail(Users.ID, null, USerTrailWHERE.TnT1, "User" + User.UserName + " Updated", null, null);


                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataAccountPwdReset;
                util.updatePasswords(usr.ID, vm.Password);
                //db.Entry(users).State = EntityState.Modified;
                //db.SaveChanges();
                trail.AddTrail(u.UserName + " " + TnT.LangResource.GlobalRes.TrailAccountResetpassword + u.UserName, Convert.ToInt32(u.ID), u.UserName + " " + TnT.LangResource.GlobalRes.TrailAccountResetpassword + u.UserName, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);

                return RedirectToAction("Login", "Account", null);
            }
            return View("SetPassword", vm);
        }


        [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
        public ActionResult _Login()
        {
            return PartialView();
        }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                //int Status = CheckLicense();

                //if (Status == 1)
                //{
                //    return View();
                //}
                //else if (Status == 2)
                //{
                //    return Content("<div style='width:100%; text-align:center; font-size:18px; font-weight:bold;'>License  expired.<br/>Please contact Propix Technologies, Pune.</div>");
                //}
                //else if (Status == 3)
                //{
                //    return Content("<div style='width:100%; text-align:center; font-size:18px; font-weight:bold;'>License suspended. <br/>Please contact Propix Technologies, Pune.</div>");
                //}
                //else if (Status == 4)
                //{
                //    return Content("<div style='width:100%; text-align:center; font-size:18px; font-weight:bold;'>Invalid request. <br/>Please contact Propix Technologies, Pune.</div>");
                //}
                //else
                //{
                //    return Content("<div style='width:100%; text-align:center; font-size:18px; font-weight:bold;'>Invalid request. <br/>Please contact Propix Technologies, Pune.</div>");
                //}

                ViewBag.ReturnUrl = returnUrl;
                return View();

            }
            catch (Exception ex)
            {
                return View("HError", ex);
            }
        }



        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.Select(x => x.Errors);
                foreach (var itemErr in errors)
                {
                    var ErrMsgs = itemErr.Select(x => x.ErrorMessage);
                    foreach (var msg in ErrMsgs)
                    {
                        trail.AddTrail(msg, 0, msg, TnT.LangResource.GlobalRes.TrailLoginActivity);
                    }
                }
                return View(model);
            }

            tuser = ac.getUser(model.username);
            if (tuser == null)
            {
                ModelState.AddModelError("", TnT.LangResource.GlobalRes.TempDataAccountContInvalidUser);
                trail.AddTrail(model.username + " " + TnT.LangResource.GlobalRes.TempDataAccountContInvalidUser,0, model.username + " " + TnT.LangResource.GlobalRes.TempDataAccountContInvalidUser, TnT.LangResource.GlobalRes.TrailLoginActivity);
                return View(model);
            }
            if (IsUserLoggedIn())
            {
                ModelState.AddModelError("", TnT.LangResource.GlobalRes.ModelStateAccountUsrLoggedIn);
                trail.AddTrail(model.username + " "+TnT.LangResource.GlobalRes.ModelStateAccountUsrLoggedIn, 0, model.username + "  "+ TnT.LangResource.GlobalRes.ModelStateAccountUsrLoggedIn, TnT.LangResource.GlobalRes.TrailLoginActivity);
                return View(model);
            }

            if (model.prevUser != model.username)
            {
                model.Attempts = 0;
                model.prevUser = "";
            }
            if (model.Attempts == null)
            {
                model.Attempts = 0;
            }
            if (model.prevUser == "")
            {
                model.prevUser = model.username;
            }

            if (model.prevUser == model.username)
            {
                ModelState.Remove("Attempts");
                ModelState.Remove("prevUser");
                model.Attempts = model.Attempts + 1;
                model.prevUser = model.prevUser;
            }
            else
            {
                model.Attempts = 0;
                model.prevUser = "";
                model.prevUser = model.username;
            }
            if (model.Attempts >= Convert.ToInt32(Utilities.getAppSettings("Atmts")))
            {
                //tuser = ac.getUser(model.username);
                ac.Deactivateuser(model.username);
                trail.AddTrail(model.username + " " + TnT.LangResource.GlobalRes.TrailAccountdeactivated, Convert.ToInt32(tuser.ID), model.username + " " + TnT.LangResource.GlobalRes.TrailAccountdeactivated, TnT.LangResource.GlobalRes.TrailLoginActivity);
                ModelState.AddModelError("", TnT.LangResource.GlobalRes.AddModelErrorAccountdeactive);
                return View(model);
            }


            try
            {
                // tuser = ac.getUser(model.username);


                var userTyp = (AppUserType)ac.getUserType(model.username);

                switch (userTyp)
                {
                    case AppUserType.ADUser:
                        if (validateADUser(model.username, model.Password))
                        {
                            if (tuser.Active)
                            {
                                //generate cookie after success
                                createAuthenticationCookie();
                                saveSingleIntance();
                            }
                            else
                            {
                                ModelState.AddModelError("", TnT.LangResource.GlobalRes.AddModelErrorAccountUseractive);
                                trail.AddTrail(model.username + " " + TnT.LangResource.GlobalRes.AddModelErrorAccountUseractive,0, model.username + " " + TnT.LangResource.GlobalRes.AddModelErrorAccountUseractive, TnT.LangResource.GlobalRes.TrailLoginActivity);
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", TnT.LangResource.GlobalRes.AddModelErrorAccountInvalidlogin);
                            trail.AddTrail(model.username + " " + TnT.LangResource.GlobalRes.AddModelErrorAccountInvalidlogin, 0, model.username + " " + TnT.LangResource.GlobalRes.AddModelErrorAccountInvalidlogin, TnT.LangResource.GlobalRes.TrailLoginActivity);
                            return View(model);
                        }
                        break;

                    case AppUserType.AppUser:

                        if (ac.validateUser(model.username, model.Password))
                        {
                            if (tuser.Active)
                            {
                                if (tuser.IsFirstLogin == true)
                                {
                                    trail.AddTrail(model.username + " " + TnT.LangResource.GlobalRes.TrailAccountFirstTime, Convert.ToInt32(tuser.ID), model.username + " " + TnT.LangResource.GlobalRes.TrailAccountFirstTime, TnT.LangResource.GlobalRes.TrailLoginActivity);
                                    return RedirectToAction("SetPassword", new { uid = tuser.ID });
                                }

                                if (utils.IsPasswordExpired(tuser.LastUpdatedDate))
                                {
                                    trail.AddTrail(model.username + " " + TnT.LangResource.GlobalRes.TrailAccountPasswordExpired, Convert.ToInt32(tuser.ID), model.username + " " + TnT.LangResource.GlobalRes.TrailAccountPasswordExpired, TnT.LangResource.GlobalRes.TrailLoginActivity);
                                    return RedirectToAction("SetPassword", new { uid = tuser.ID });
                                }

                                int i = utils.PasswordExpiryDays(tuser.LastUpdatedDate);
                                if (i > 0)
                                {
                                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataAccountPwdExpireIn +" " + i + " "+   TnT.LangResource.GlobalRes.TempDataAccountDays;
                                }
                                createAuthenticationCookie();
                                saveSingleIntance();
                            }
                            else
                            {
                                if (User == null)
                                {
                                    trail.AddTrail(model.username + " " + TnT.LangResource.GlobalRes.TrailAccounttryingtoLogIn, 0, model.username + " " + TnT.LangResource.GlobalRes.TrailAccounttryingtoLogIn, TnT.LangResource.GlobalRes.TrailLoginActivity);
                                }
                                else
                                {
                                    trail.AddTrail(model.username + " " + TnT.LangResource.GlobalRes.TrailAccounttryingtoLogIn, Convert.ToInt32(User.ID), model.username + " " + TnT.LangResource.GlobalRes.TrailAccounttryingtoLogIn, TnT.LangResource.GlobalRes.TrailLoginActivity);
                                }



                                ModelState.AddModelError("", TnT.LangResource.GlobalRes.AddModelErrorAccountdeactive);
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", TnT.LangResource.GlobalRes.AddModelErrorAccountInvalidlogin);
                            trail.AddTrail(model.username + " " + TnT.LangResource.GlobalRes.AddModelErrorAccountInvalidlogin, 0, model.username + " " + TnT.LangResource.GlobalRes.AddModelErrorAccountInvalidlogin, TnT.LangResource.GlobalRes.TrailLoginActivity);
                            return View(model);
                        }
                        break;

                    default:

                        ModelState.AddModelError("", TnT.LangResource.GlobalRes.TempDataAccountContInvalidUser);
                        trail.AddTrail(TnT.LangResource.GlobalRes.TempDataAccountContInvalidUser, 0, TnT.LangResource.GlobalRes.TempDataAccountContInvalidUser, TnT.LangResource.GlobalRes.TrailLoginActivity);
                        return View(model);

                }


                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                return View("HError", ex);

            }

        }


        [HttpPost]

        public void updateUData()
        {
            updateSingleIntance();
        }


        #region loginUtils

        public bool IsUserLoggedIn()
        {
            return ac.IsUserLoggedIn(tuser.ID);
        }


        private bool validateADUser(string usrName, string password)
        {
            string serverIP = Utilities.getAppSettings("ServerIP").ToString();
            string DomainName = Utilities.getAppSettings("DomainName").ToString();
            string contextOption = Utilities.getAppSettings("ContextOption").ToString();
            if (ADUsers.ValidateUser(contextOption, serverIP, DomainName, usrName, password))
            {
                return true;
                //user = ac.getADUser(model.username);
                //if (user.Active == true)
                //{
                //    string Password = AESCryptor.Encrypt(password, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //    if (user.Password != Password)
                //    {
                //        REDTR.DB.BusinessObjects.Users usr = new REDTR.DB.BusinessObjects.Users();
                //        usr.ID = user.ID;
                //        usr.UserName = user.UserName;
                //        usr.UserName1 = user.UserName1;
                //        string cypher = AESCryptor.Encrypt(password, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //        usr.Password = cypher;
                //        usr.RoleID = user.RoleID;
                //        usr.LastUpdatedDate = DateTime.Now;
                //        usr.Active = true;
                //        usr.IsFirstLogin = false;
                //        usr.UserType = user.UserType;

                //        DbHelper m_dbhelper = new DbHelper();
                //        m_dbhelper.DBManager.UsersBLL.InsertOrUpdateUsers(REDTR.DB.BLL.UsersBLL.UsersOp.UpdateUser, usr);
                //    }

                //    //generate cookie after success
                //    createAuthenticationCookie();

                //}
                //else
                //{
                //    ModelState.AddModelError("", TnT.LangResource.GlobalRes.AddModelErrorAccountUseractive);
                //    return View(model);
                //}


            }
            else
            {
                //ModelState.AddModelError("", TnT.LangResource.GlobalRes.AddModelErrorAccountInvalidlogin);
                //return View(model);
                return false;
            }
        }

        private void addPermissionCookie(string userName, int expTime, List<string> permissions)
        {
            string perm = JsonConvert.SerializeObject(permissions);
            string encrytedPerms = AESCryptor.Encrypt(perm, TnTGlobals.Cipherkey);
            HttpCookie faCookieperm = new HttpCookie(FormsAuthentication.FormsCookieName + "_perms", encrytedPerms);
            Response.Cookies.Add(faCookieperm);
        }

       
      

        private void createAuthenticationCookie()
        {
            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
            serializeModel.ID = tuser.ID;
            serializeModel.FirstName = tuser.UserName;
            serializeModel.Role = tuser.RoleID;

            utils = new SecurityUtils();
            serializeModel.Permissions = utils.getPermissions(tuser.RoleID).Select(x=>x.ID.ToString()).ToList();
            //addPermissionToSession(tuser.ID.ToString(), utils.getPermissions(tuser.RoleID));
            serializeModel.Roles_Name = utils.getRoleName(tuser.RoleID);
            string srlzdData = JsonConvert.SerializeObject(serializeModel);
            //var userPermissions = utils.getPermissions(tuser.RoleID);

            int ExpTime = Convert.ToInt32(Utilities.getAppSettings("ExpirationTime"));
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, tuser.UserName, DateTime.Now, DateTime.Now.AddMinutes(ExpTime), false, srlzdData);
            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
            //addPermissionCookie(tuser.UserName, ExpTime, userPermissions);
            trail.AddTrail(tuser.UserName + " " + TnT.LangResource.GlobalRes.TrailAccountLoggedIn, Convert.ToInt32(tuser.ID), tuser.UserName + " " + TnT.LangResource.GlobalRes.TrailAccountLoggedIn, TnT.LangResource.GlobalRes.TrailLoginActivity);
        }

        private bool verifylogin(string password)
        {
            string userName = User.Identity.GetUserName();
            Account ac = new Account();
            var typ = ac.getUserType(userName);

            if (Convert.ToInt32(AppUserType.ADUser) == typ)
            {
                string serverIP = Utilities.getAppSettings("ServerIP").ToString();
                string DomainName = Utilities.getAppSettings("DomainName").ToString();
                string contextOption = Utilities.getAppSettings("ContextOption").ToString();
                if (ADUsers.ValidateUser(contextOption, serverIP, DomainName, userName, password))
                {
                    tuser = ac.getUser(userName);
                    if (tuser.Active)
                    {
                        return true;
                    }
                }
            }
            else if (Convert.ToInt32(AppUserType.AppUser) == typ)
            {
                tuser = ac.getUser(userName, password);
                if (tuser == null)
                {
                    return false;
                }
                if (tuser.Active)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion


        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool VerifyLogin(LoginViewModel model)
        {
            if (verifylogin(model.Password))
            {
                var Rmk = Request["Remark"].ToString();
                trail.AddTrail(model.username + " " + TnT.LangResource.GlobalRes.TrailAccountESignatureVerified +" ' " + Rmk + " '", Convert.ToInt32(User.ID), model.username + " " + TnT.LangResource.GlobalRes.TrailAccountESignatureVerifieds, TnT.LangResource.GlobalRes.TrailESignature);
                return true;
            }
            else
            {
                trail.AddTrail(model.username + " " + TnT.LangResource.GlobalRes.TrailAccountESignatureVerificationFailed, Convert.ToInt32(User.ID), model.username + " " + TnT.LangResource.GlobalRes.TrailAccountESignatureVerificationFailed, TnT.LangResource.GlobalRes.TrailESignature);
                return false;
            }
        }

        private void saveSingleIntance()
        {
            ac.registerUserLogin(tuser.ID, System.Web.HttpContext.Current.Session.SessionID);
        }
        private void updateSingleIntance()
        {
            if (User != null)
            {
                ac.updateUserLogin(User.ID);
            }
        }

        //[CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
        //public ActionResult LogOut()
        //{
        //    performLogout();
        //    return RedirectToAction("Login", "Account", null);
        //}

        [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
        public ActionResult LogOut(string returnUrl = "")
        {
            performLogout();
            return RedirectToAction("Login","Account",new { returnUrl = returnUrl });
        }

        private void performLogout()
        {
            if (User != null)
            {
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailAccountLoggedOut, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailAccountLoggedOut, TnT.LangResource.GlobalRes.TrailLoginActivity);
                ac.removeLogginSession(User.ID);
            }
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

        }
    }

}
