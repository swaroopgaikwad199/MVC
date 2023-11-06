using PTPLCRYPTORENGINE;
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
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class UsersController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        static string baseDir = AppDomain.CurrentDomain.BaseDirectory + "ResourceSet";
        ResourceManager rm = ResourceManager.CreateFileBasedResourceManager("18_Common", baseDir + "\\ResourceSet", null);
        CultureInfo cul = CultureInfo.CreateSpecificCulture("de");

        // GET: Users
        public ActionResult Index()
        {
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailUserViewUsers, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailUserViewUsers, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            ViewBag.Roles = db.Roles.ToList();
            return View(db.Users.Where(x => x.ID != 0).ToList());
        }



        // GET: Users/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            PwdData();
            BindData();
            TnT.Models.Account.Users obj = null;
            return View(obj);
        }

        private void BindData()
        {
            ViewBag.Roles = db.Roles.Where(r => r.IsActive == true).ToList();
        }

        private void PwdData()
        {
            ViewData["maxpwd"] = Utilities.getAppSettings("MaxPassword");
            ViewData["minpwd"] = Utilities.getAppSettings("MinPassword");
        }
        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserName,Password,RoleID,Active,Remarks,CreatedDate,LastUpdatedDate,UserName1,ConfirmPassword,EmailId")] Users users)
        {
            if (ModelState.IsValid)
            {

                PwdData();
                users.LastUpdatedDate = DateTime.Now;
                users.Active = true;
                users.CreatedDate = DateTime.Now;

                bool IsUserNameExisting = db.Users.Select(u => u.UserName == users.UserName).FirstOrDefault();

                if (IsUserNameExisting)
                {
                    ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.TrailUserAlreadyexist);
                    ViewBag.Roles = db.Roles.ToList();
                    return View(users);
                }

                REDTR.DB.BusinessObjects.Users usr = new REDTR.DB.BusinessObjects.Users();
                usr.UserName = users.UserName;
                usr.UserName1 = users.UserName1;
                string cypher = AESCryptor.Encrypt(users.Password.Trim(), TnTGlobals.Cipherkey);
                usr.Password = cypher;
                usr.RoleID = users.RoleID;
                usr.LastUpdatedDate = DateTime.Now;
                usr.Active = true;
                usr.IsFirstLogin = true;
                usr.CreatedDate = DateTime.Now;
                usr.UserType = 1;
                usr.EmailId = users.EmailId;

                DbHelper m_dbhelper = new DbHelper();
                var id = m_dbhelper.DBManager.UsersBLL.InsertOrUpdateUsers(REDTR.DB.BLL.UsersBLL.UsersOp.AddUser, usr);
                //m_dbhelper.AddUserTrail(Use.ID, null, USerTrailWHERE.TnT1, "New User " + usr.UserName + " Added", null, null);

                SecurityUtils util = new SecurityUtils();
                util.updatePasswords(id, users.Password);
                ///////////////////

                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailUsercreateduser + " " + users.UserName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailUsercreateduser + " " + users.UserName, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                TempData["Success"] = " '" + users.UserName + "' " + TnT.LangResource.GlobalRes.TempDataUsercreatedsuccessfully;
                return RedirectToAction("Index");
            }
            else
            {
                BindData();
                return View(users);
            }

        }


        public ActionResult ChangeRole(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            BindData();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeRole([Bind(Include = "ID,RoleID")] Users EDusers)
        {
            var users = db.Users.Find(EDusers.ID);
            var type = db.Users.Where(x => x.ID == EDusers.ID).Select(x => x.UserType).FirstOrDefault();
            string msg = "";
            if (users.RoleID!=EDusers.RoleID)
            {
                var roles = db.Roles;
                string oldrole = roles.Where(x => x.ID == users.RoleID).Select(x => x.Roles_Name).FirstOrDefault();
                string newrole = roles.Where(x => x.ID == EDusers.RoleID).Select(x => x.Roles_Name).FirstOrDefault();
                // msg = "Role was :" + oldrole + "; is changed to:" + newrole+" for "+users.UserName;
                msg = TnT.LangResource.GlobalRes.UsersIndexRole + " " + TnT.LangResource.GlobalRes.RptAuditTrailWas + ":" + oldrole + ";" + " " + TnT.LangResource.GlobalRes.RptAuditTrailIsChngeTo + " : " + newrole + " "+TnT.LangResource.GlobalRes.RptAuditTrailUsersFor +" "+ users.UserName;


            }
            if (users != null && EDusers.RoleID > 0)
            {
                REDTR.DB.BusinessObjects.Users usr = new REDTR.DB.BusinessObjects.Users();
                usr.ID = users.ID;
                usr.UserName = users.UserName;
                usr.UserName1 = users.UserName1;
                usr.Password = users.Password;
                usr.RoleID = EDusers.RoleID;
                usr.LastUpdatedDate = DateTime.Now;
                usr.Active = true;
                usr.IsFirstLogin = false;
                usr.UserType = Convert.ToInt32(users.UserType);
                DbHelper m_dbhelper = new DbHelper();
                m_dbhelper.DBManager.UsersBLL.InsertOrUpdateUsers(REDTR.DB.BLL.UsersBLL.UsersOp.UpdateUser, usr);
                if(msg=="")
                {
                    msg = User.FirstName + " " + TnT.LangResource.GlobalRes.TrailUserchangedroleuser + " " + users.UserName;
                }
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataUserRolechanged;
                trail.AddTrail(msg, Convert.ToInt32(User.ID),msg, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, TnT.LangResource.GlobalRes.AddModuleErrorUserInvalidUser);
            BindData();
            return View(EDusers);
        }



        // GET: Users/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PwdData();
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }



        public ActionResult Activation(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Activation([Bind(Include = "ID,Active")] Users users)
        {
            decimal id = users.ID;
            bool IsActive = users.Active;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users AUsers = db.Users.Find(id);

            REDTR.DB.BusinessObjects.Users usr = new REDTR.DB.BusinessObjects.Users();
            usr.ID = AUsers.ID;
            usr.UserName = AUsers.UserName;
            usr.UserName1 = AUsers.UserName1;
            usr.IsFirstLogin = true;
            usr.Password = AUsers.Password;
            usr.RoleID = AUsers.RoleID;
            usr.LastUpdatedDate = DateTime.Now;
            usr.Active = IsActive;
            usr.UserType = Convert.ToInt32(AUsers.UserType);
            DbHelper m_dbhelper = new DbHelper();
            m_dbhelper.DBManager.UsersBLL.InsertOrUpdateUsers(REDTR.DB.BLL.UsersBLL.UsersOp.UpdateUser, usr);
            if (IsActive)
            {
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailUsersuccessfullyactivated + " " + usr.UserName1, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailUsersuccessfullyactivated + " " + usr.UserName1, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                TempData["Success"] = usr.UserName1 + " " + TnT.LangResource.GlobalRes.TempDataUseractivatedsuccessfully;
            }
            else
            {
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataUserdeactivatedsuccessfully + " " + usr.UserName1, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataUseractivatedsuccessfully + " " + usr.UserName1, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                TempData["Success"] = usr.UserName1 + " " + TnT.LangResource.GlobalRes.TempDataUserdeactivatedsuccessfully;
            }


            return RedirectToAction("Index");
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserName,UserName1,Password,ConfirmPassword,OldPassword,EmailId")] Users users)
        {
            PwdData();
            var u = db.Users.Find(users.ID);
            if (ModelState.IsValid)
            {
                List<string> compare = new List<string>();
                compare.Add("Password");
                compare.Add("EmailId");
                SecurityUtils util = new SecurityUtils();
                if (util.IsPasswordExistng(users.ID, users.Password))
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataUserusedifferentpassword;
                    return View("Edit", users);
                }



                REDTR.DB.BusinessObjects.Users usr = new REDTR.DB.BusinessObjects.Users();
                usr.ID = users.ID;
                usr.UserName = users.UserName;
                usr.UserName1 = users.UserName1;
                string cypher = AESCryptor.Encrypt(users.Password.Trim(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                usr.Password = cypher;
                usr.RoleID = u.RoleID;
                usr.LastUpdatedDate = DateTime.Now;
                usr.Active = true;
                usr.IsFirstLogin = true;
                usr.EmailId = users.EmailId;
                usr.UserType = Convert.ToInt32(u.UserType);
                users.Password = cypher;
                
                var olduser = db.Users.Where(x => x.ID == users.ID).FirstOrDefault();

                System.Reflection.PropertyInfo[] properties = olduser.GetType().GetProperties();
                string msg = "";
                foreach (var oProperty in properties)
                {
                    if (compare.Contains(oProperty.Name))
                    {
                        var oOldValue = oProperty.GetValue(olduser, null);
                        var oNewValue = oProperty.GetValue(users, null);
                     

                            if (!object.Equals(oOldValue, oNewValue))
                        {

                            // Handle the display values when the underlying value is null
                            var sOldValue = oOldValue == null ? "null" : oOldValue.ToString();
                            var sNewValue = oNewValue == null ? "null" : oNewValue.ToString();
                            if(oProperty.Name=="Password")
                            {
                                msg += TnT.LangResource.GlobalRes.RptAuditTrailUsersPwdUpdatedforUsr + " :" + users.UserName+" ";
                            }

                            if (oProperty.Name == "EmailId")
                            {
                                //msg += oProperty.Name + " was: " + sOldValue + "; is changed to: " + sNewValue + " for,";
                                msg += oProperty.Name + " " + TnT.LangResource.GlobalRes.RptAuditTrailWas + ":" + sOldValue + ";" + " " + TnT.LangResource.GlobalRes.RptAuditTrailIsChngeTo + " : " + sNewValue + ""+ TnT.LangResource.GlobalRes.RptAuditTrailUsersFor+ " ,";

                            }
                        }
                    }
                }
                msg = msg.TrimEnd(',');
                if (msg == "")
                {
                    msg = User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataUserResetpassworduser;
                }
                else
                {
                    msg += TnT.LangResource.GlobalRes.RptAuditTrailUsersFor + " :";
                }
                DbHelper m_dbhelper = new DbHelper();
                m_dbhelper.DBManager.UsersBLL.InsertOrUpdateUsers(REDTR.DB.BLL.UsersBLL.UsersOp.UpdateUser, usr);
                //m_dbhelper.AddUserTrail(Users.ID, null, USerTrailWHERE.TnT1, "User" + User.UserName + " Updated", null, null);

                util.updatePasswords(users.ID, users.Password);
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataUserPasswordreset;

                //db.Entry(users).State = EntityState.Modified;
                //db.SaveChanges();
                trail.AddTrail(msg+" " + users.UserName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataUserResetpassworduser + users.UserName, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return RedirectToAction("Index");
            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reset([Bind(Include = "ID,UserName,UserName1,Password,ConfirmPassword,EmailId")] Users users)
        {
            //string EncOldPass = AESCryptor.Encrypt(users.OldPassword.Trim(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
            //if (u.Password != EncOldPass)
            //{
            //    ModelState.AddModelError(string.Empty, "Old Password doesnt match. Kindly check it");
            //    return View(u);
            //}

            if (ModelState.IsValid)
            {

                var u = db.Users.Find(users.ID);
                SecurityUtils util = new SecurityUtils();
                if (util.IsPasswordExistng(users.ID, users.Password))
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataUserusedifferentpassword;
                    return View("Edit", users);
                }


                REDTR.DB.BusinessObjects.Users usr = new REDTR.DB.BusinessObjects.Users();
                usr.ID = users.ID;
                usr.UserName = users.UserName;
                usr.UserName1 = users.UserName1;
                string cypher = AESCryptor.Encrypt(users.Password.Trim(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                usr.Password = cypher;
                usr.RoleID = u.RoleID;
                usr.LastUpdatedDate = DateTime.Now;
                usr.Active = true;
                usr.IsFirstLogin = false;
                usr.UserType = Convert.ToInt32(u.UserType);
                DbHelper m_dbhelper = new DbHelper();
                m_dbhelper.DBManager.UsersBLL.InsertOrUpdateUsers(REDTR.DB.BLL.UsersBLL.UsersOp.UpdateUser, usr);
                //m_dbhelper.AddUserTrail(Users.ID, null, USerTrailWHERE.TnT1, "User" + User.UserName + " Updated", null, null);

                util.updatePasswords(users.ID, users.Password);
                //TempData["Success"] = "PASSWORD RESET SUCCESSFULLY";
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataUserPasswordresetsuccessfullyfor + " " + users.UserName;
                //db.Entry(users).State = EntityState.Modified;
                //db.SaveChanges();
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataUserResetpassworduser + " " + users.UserName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataUserResetpassworduser + " " + users.UserName, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);

                AccountController cntr = new AccountController();
                cntr.LogOut();
                return RedirectToAction("Login", "Account");
            }
            return View("Edit", users);
        }

        public ActionResult IsUserExisting(string UserID)
        {
            try
            {
                var data = db.Users.Where(x => x.UserName == UserID).FirstOrDefault();
                if (data != null)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        // GET: Users/Delete/5
        //public ActionResult Delete(decimal id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Users users = db.Users.Find(id);
        //    if (users == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.RoleName = db.Roles.Where(r => r.ID == users.RoleID).Select(r => r.Roles_Name).First();
        //    return View(users);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(decimal id)
        //{
        //    var usr = db.Users.Find(id);
        //    int TotalPermissions = db.Permissions.Count();
        //    int UsersTotalPermissions = db.ROLESPermission.Where(p => p.Roles_Id == usr.RoleID).Count();

        //    if (TotalPermissions == UsersTotalPermissions)
        //    {
        //        ViewBag.Msg_AdminDelete = "Cannot delete Administrator";
        //        return View();
        //    }
        //    else if (id == User.ID)
        //    {
        //        ViewBag.Msg_AdminDelete = "EXISTING LOGGED IN USER CAN NOT BE DELETED";
        //        return View();
        //    }
        //    else
        //    {
        //        DbHelper m_dbhelper = new DbHelper();
        //        REDTR.DB.BusinessObjects.Users User1 = m_dbhelper.DBManager.UsersBLL.GetUsers(UsersBLL.UsersOp.GetUsersofName, usr.UserName);
        //        m_dbhelper.DBManager.UsersBLL.RemoveUsers(User1.ID);
        //    }

        //    //Users users = db.Users.Find(id);
        //    //db.Users.Remove(users);
        //    //db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}


        #region Sync AD Users


        public ActionResult SyncADUsers()
        {
            ADUsers hlpr = new ADUsers();
            List<TnT.Models.SettingsNUtility.Roles> LstRoles = db.Roles.Where(x => x.IsActive == true && !string.IsNullOrEmpty(x.ADRole)).ToList();
            var lstUsrs = hlpr.getADUsers(LstRoles);
            if (lstUsrs.Count > 0)
            {
                int cnt = hlpr.SyncUsers(lstUsrs);
                TempData["Success"] = cnt + " " + TnT.LangResource.GlobalRes.TempDataUsersyncedsuccessfully + " " + lstUsrs.Count;
                trail.AddTrail(cnt + " " + TnT.LangResource.GlobalRes.TempDataUsersyncedsuccessfully + " " + lstUsrs.Count, Convert.ToInt32(User.ID), cnt + " " + TnT.LangResource.GlobalRes.TempDataUsersyncedsuccessfully + " " + lstUsrs.Count, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            }
            else
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataUserNoUseravailabletosync;
                trail.AddTrail(TnT.LangResource.GlobalRes.TempDataUserNoUseravailabletosync, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataUserNoUseravailabletosync, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            }

            return RedirectToAction("Index");
        }

        #endregion

    }
}
