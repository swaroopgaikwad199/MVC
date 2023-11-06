using PTPLCRYPTORENGINE;
using REDTR.HELPER;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer.JobService;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.Account;
using TnT.Models.SettingsNUtility;



namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class SettingsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        // GET: Settings
        BatchNotificationHelper batchNotificationHlpr = new BatchNotificationHelper();



        public ActionResult ApplicationSettings()
        {
            return View(db.Settings.ToList());
        }
        public ActionResult ApplicationSettingsEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppSettings appSettings = db.AppSettings.Find(id);
            if (appSettings == null)
            {
                return HttpNotFound();
            }
            trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailSettingsEditSettings, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailSettingsEditSettings,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            return View(appSettings);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplicationSettingsEdit([Bind(Include = "id,CompanyName,Address,Logo,CompanyCode,LineCode,Remarks,PlantCode,GLN,Country,Street,StateOrRegion,City,PostalCode,License,LicenseState,LicenseAgency,IAC_CIN")] Settings settings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(settings).State = EntityState.Modified;
                db.SaveChanges();
                trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailSettingsupdatedCompanySettings, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailSettingsupdatedCompanySettings,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return RedirectToAction("Index");
            }
            return View(settings);
        }



        public ActionResult Index()
        {
            trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailSettingsViewCompanySettings, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailSettingsViewCompanySettings,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            
            return View(db.Settings.ToList());
        }

       

        [HttpPost]
        public ActionResult getStateOrRegion(int Conid)
        {

            var data = (from st in db.S_State join con in db.Country on st.CountryID equals con.Id where st.CountryID == Conid select new { st.ID, st.StateName }).Distinct().ToList();

            return Json(data);
        }

        // GET: Settings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Settings settings = db.Settings.Find(id);
            if (settings == null)
            {
                return HttpNotFound();
            }
            ViewBag.Country = new SelectList(db.Country, "Id", "CountryName", settings.Country);
            var data = db.S_State.Where(x => x.CountryID == settings.Country);
            ViewBag.StateOrRegion = new SelectList(data, "ID", "StateName", settings.StateOrRegion);
            ViewBag.LicenseState = new SelectList(data, "ID", "StateName", settings.LicenseState);
            //BindData();
            //   getState(settings.StateOrRegion,settings.Country);
            trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailSettingsEditSettings, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailSettingsEditSettings,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            return View(settings);
        }

        // POST: Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,CompanyName,Address,Logo,CompanyCode,LineCode,Remarks,PlantCode,GLN,Country,Street,StateOrRegion,City,PostalCode,License,LicenseState,LicenseAgency,IAC_CIN,District")] Settings settings)
        {
            if (ModelState.IsValid)
            {

                
              
      
                List<string> compare = new List<string>();
                compare.Add("M_Country");
                compare.Add("M_State");
                var oldsetting = db.Settings.FirstOrDefault();
             
                System.Reflection.PropertyInfo[] properties = oldsetting.GetType().GetProperties();
                string msg = "";

                foreach (var oProperty in properties)
                {
                    if (!compare.Contains(oProperty.Name))
                    {
                        var oOldValue = oProperty.GetValue(oldsetting, null);
                        var oNewValue = oProperty.GetValue(settings, null);
                        // this will handle the scenario where either value is null

                        if (!object.Equals(oOldValue, oNewValue))
                        {
                            // Handle the display values when the underlying value is null
                            var sOldValue = oOldValue == null ? "null" : oOldValue.ToString();
                            var sNewValue = oNewValue == null ? "null" : oNewValue.ToString();
                            if (oProperty.Name == "StateOrRegion")
                            {
                                int oldstat = Convert.ToInt32(oOldValue);
                                if (oldstat != 0)
                                {
                                    sOldValue = db.S_State.Where(x => x.ID == oldstat).Select(x => x.StateName).FirstOrDefault().ToString();
                                }

                                int newtstat = Convert.ToInt32(sNewValue);
                                if (newtstat != 0)
                                {
                                    sNewValue = db.S_State.Where(x => x.ID == newtstat).Select(x => x.StateName).FirstOrDefault().ToString();
                                }
                            }

                            if (oProperty.Name == "Country")
                            {
                                int oldcon = Convert.ToInt32(oOldValue);
                                if (oldcon != 0)
                                {
                                    sOldValue = db.Country.Where(x => x.Id == oldcon).Select(x => x.CountryName).FirstOrDefault().ToString();
                                }
                                int newcon = Convert.ToInt32(oNewValue);
                                if (newcon != 0)
                                {
                                    sNewValue = db.Country.Where(x => x.Id == newcon).Select(x => x.CountryName).FirstOrDefault().ToString();
                                }
                            }

                            if (oProperty.Name == "LicenseState")
                            {
                                int oldstat = Convert.ToInt32(oOldValue);
                                if (oldstat != 0)
                                {
                                    sOldValue = db.S_State.Where(x => x.ID == oldstat).Select(x => x.StateName).FirstOrDefault().ToString();
                                }
                                int newtstat = Convert.ToInt32(sNewValue);
                                if (newtstat != 0)
                                {
                                    sNewValue = db.S_State.Where(x => x.ID == newtstat).Select(x => x.StateName).FirstOrDefault().ToString();
                                }
                            }
                            //msg += oProperty.Name + " was: " + sOldValue + "; is changed to: " + sNewValue + " ,";
                            msg += oProperty.Name + " " + TnT.LangResource.GlobalRes.RptAuditTrailWas + ":" + sOldValue + ";" + " " + TnT.LangResource.GlobalRes.RptAuditTrailIsChngeTo + " : " + sNewValue + ""+",";
                         }
                    }
                }

                msg = msg.TrimEnd(',');
                if(msg=="")
                {
                    msg = TnT.LangResource.GlobalRes.RptAuditTrailSettingUpdated;
                }
                else
                {
                    msg += " " + TnT.LangResource.GlobalRes.RptAuditTrailUsersFor + " :";
                }

                db.Entry(oldsetting).State = EntityState.Detached;
                db.Entry(settings).State = EntityState.Modified;
                db.SaveChanges();

                trail.AddTrail(msg+ " "+ settings.CompanyName , Convert.ToInt32(User.ID),msg,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return RedirectToAction("Index");
            }
            ViewBag.Country = new SelectList(db.Country, "Id", "CountryName", settings.Country);
            var data = db.S_State.Where(x => x.CountryID == settings.Country);
            ViewBag.StateOrRegion = new SelectList(data, "ID", "StateName", settings.StateOrRegion);
            ViewBag.LicenseState = new SelectList(data, "ID", "StateName", settings.LicenseState);
            return View(settings);
        }

        // GET: Settings/Delete/5
        public ActionResult getdata()
        {
            var set = db.Settings.FirstOrDefault();
            int i = Convert.ToInt32(set.LicenseState);
            var data = db.S_State.Where(x => x.ID == i);
            return Json(data);
        }

        #region AppSetting

        public ActionResult AppSettings()
        {
            return View(db.AppSettings.Where(x=>x.Key == "PasswordExpiryDays").ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAppSettings(AppSettings appSettings)
        {
            if (ModelState.IsValid)
            {
                db.AppSettings.Add(appSettings);
                db.SaveChanges();
                trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailSettingscreatedAppSettings + appSettings.Key, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailSettingscreatedAppSettings + appSettings.Key,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            }
            return View(db.AppSettings.ToList());
        }

        public ActionResult EditAppSettings(int Id)
        {
            AppSettings appS = db.AppSettings.Find(Id);

            if (appS == null)
            {
                return HttpNotFound();
            }
            trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailSettingsRequestEditAppSettings, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailSettingsRequestEditAppSettings,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            return View(appS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAppSettings(AppSettings appSettings)
        {
            if (ModelState.IsValid)
            {
                var prev = db.AppSettings.Find(appSettings.Id);
                prev.Value = appSettings.Value;

                db.Entry(prev).State = EntityState.Modified;
                db.SaveChanges();
                trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailSettingsEditAppSettings+" " + appSettings.Key, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailSettingsEditAppSettings + " " + appSettings.Key,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            }
            return RedirectToAction("AppSettings");
        }


        #endregion

        #region DataBaseBackUp

        public ActionResult DataBaseBackup()
        {
            trail.AddTrail(User.FirstName + " "+TnT.LangResource.GlobalRes.TrailSettingsviewDataBaseBackup, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailSettingsviewDataBaseBackup,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            return View();
        }


        public ActionResult GetDataBaseBackup()
        {

            string con = Utilities.getConnectionString("DefaultConnection");
            string[] db = con.Split(';');
            string[] dbname = db[1].Split('=');

            string backupDIR = Convert.ToString(Utilities.getAppSettings("DatabaseBackUpPath"));
            if (!System.IO.Directory.Exists(backupDIR))
            {
                System.IO.Directory.CreateDirectory(backupDIR);
            }
            try
            {
                SqlConnection conn = new SqlConnection(con);

                conn.Open();
                conn.FireInfoMessageEventOnUserErrors = true;
                conn.InfoMessage += OnInfoMessage;
                SqlCommand sqlcmd = new SqlCommand("backup database " + dbname[1] + " to disk='" + backupDIR + "\\" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + "_" + dbname[1] + "_.Bak' with stats = 1", conn);
                sqlcmd.CommandTimeout = 0;
                sqlcmd.ExecuteNonQuery();
                conn.Close();
                conn.InfoMessage -= OnInfoMessage;
                conn.FireInfoMessageEventOnUserErrors = false;
                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgSettingDbBackup, 100, 100);
                trail.AddTrail(TnT.LangResource.GlobalRes.TrailSettingsDatabaseBackUpSuccessfully + " " + User.FirstName, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TrailSettingsDatabaseBackUpSuccessfully + " " + User.FirstName, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return Json(true);
            }
            catch (Exception ex)
            {
                TnT.DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);
                trail.AddTrail(TnT.LangResource.GlobalRes.TrailSettingsDBConnectionLost, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TrailSettingsDBConnectionLost, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return Json(false);
            }

        }

        
   

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(HttpPostedFileBase file)
        {
            return View();
        }

        public string GetConnStr()
        {
            string database = Utilities.getConnectionString("DefaultConnection");
            return database;
        }
        private void OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            try
            {
                foreach (SqlError info in e.Errors)
                {
                    if (info.Class > 10)
                    {
                        ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgSettingDbConnectionLost, 0,0);
                    }
                    else
                    {
                        string[] per = e.Message.Split(' ');
                        string percentage = per[0];
                        if (per.Length <= 3)
                        {

                            int percent = Convert.ToInt16(percentage);
                            ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgSettingCurrentlyInProgress + percent +" %", percent, 0);

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgSettingDbConnectionLost, 0,0);
            }
           
        }



        #endregion

        private void PwdData()
        {
            ViewData["maxpwd"] = Utilities.getAppSettings("MaxPassword");
            ViewData["minpwd"] = Utilities.getAppSettings("MinPassword");
        }
        public ActionResult ChangePassword()
        {
            PwdData();
            Users users = db.Users.Find(User.ID);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View();
        }


        [HttpPost]
        public ActionResult ChangePassword([Bind(Include = "Password,ConfirmPassword,OldPassword,EmailId")] Users users)
        {
            PwdData();
            var u = db.Users.Find(User.ID);
           
                SecurityUtils util = new SecurityUtils();
                if (util.IsPasswordExistng(u.ID, users.Password))
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataUserusedifferentpassword;
                    return View("ChangePassword");
                }


                REDTR.DB.BusinessObjects.Users usr = new REDTR.DB.BusinessObjects.Users();
                usr.ID = u.ID;
                usr.UserName = u.UserName;
                usr.UserName1 = u.UserName1;
                usr.EmailId = u.EmailId;
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

                util.updatePasswords(u.ID, users.Password);
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataUserPasswordreset;

                //db.Entry(users).State = EntityState.Modified;
                //db.SaveChanges();
                trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataUserResetpassworduser + u.UserName, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TempDataUserResetpassworduser + u.UserName,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return RedirectToAction("ChangePassword");
           
           
        }

        public ActionResult GetOldPassword(string OldPassword,int Uid)
        {
            bool psd;
            var exist = db.Users.Where(x => x.ID == Uid).FirstOrDefault();
            var password = exist.Password;
            string cypher = AESCryptor.Decrypt(password.Trim(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
            if(OldPassword!=cypher)
            {
                psd = false;
            }
            else
            {
                psd = true;
            }

            return Json(psd);
        }

        #region Restore DB Back

        [HttpPost]
        public ActionResult GetDBFiles(DateTime date)
        {
            string path = Utilities.getAppSettings("DatabaseBackUpPath");
            DirectoryInfo d = new DirectoryInfo(path);//Assuming DatabaseBackup is your Folder
            FileInfo[] Files = d.GetFiles("*.Bak").OrderBy(x => x.CreationTime).ToArray(); //Getting Bak files
            string today = date.ToString("dd/MM/yyyy");
            List<FileData> lst = new List<FileData>();
            foreach (var f in Files)
            {
                string fdate = f.CreationTime.ToString("dd/MM/yyyy");
                if (today == fdate)
                {
                    var dbFile = db.RestoreDb.Where(x => x.FileName == f.Name);
                    if (dbFile.Count() == 0)
                    {

                        FileData fl = new FileData();
                        fl.Filename = f.Name;
                        fl.CreatedDate = Convert.ToString(f.CreationTime);
                        lst.Add(fl);
                    }
                }
            }

            return Json(lst);
        }

        public ActionResult RestoreDb()
        {
            trail.AddTrail(User.FirstName+ TnT.LangResource.GlobalRes.TrailSettingVisitedDbRestore, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailSettingVisitedDbRestore, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            return View();
        }

        public ActionResult SaveRestoreDbRequest(string FileName,string createddate)
        {
            REDTR.DB.BusinessObjects.Job job = new REDTR.DB.BusinessObjects.Job();
            var data = db.RestoreDb.Where(x => x.FileName == FileName).FirstOrDefault();
            RestoreDb rd = new RestoreDb();
            rd.FileName = FileName;
            rd.Createdby = User.ID;
            rd.CreatedDate = Convert.ToDateTime(createddate);
            if(data==null)
            {
                db.RestoreDb.Add(rd);
                db.SaveChanges();
                var userData = User;
                
                batchNotificationHlpr.notifyPerimissionHoldersSettings(FileName, BatchEventType.SettingRestoreDbRequest, userData);
                trail.AddTrail(rd.FileName + TnT.LangResource.GlobalRes.toastrSettingRestoreDbReqGeratedSuccesssfuly, Convert.ToInt32(User.ID), rd.FileName + TnT.LangResource.GlobalRes.toastrSettingRestoreDbReqGeratedSuccesssfuly, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return Json(TnT.LangResource.GlobalRes.toastrSettingRestoreDbReqGeratedSuccesssfuly);
            }
            else
            {
                trail.AddTrail(TnT.LangResource.GlobalRes.TrailSettingDbRewFileExist, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TrailSettingDbRewFileExist, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return Json(TnT.LangResource.GlobalRes.TrailSettingDbRewFileExist);
            }
          
        }

        public ActionResult VerifyRestoreDb()
        {
            trail.AddTrail(User.FirstName+TnT.LangResource.GlobalRes.TrailSettingVisitedDbRestoreReqVerify, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailSettingVisitedDbRestoreReqVerify, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            return View(db.RestoreDb.ToList());
        }

        [HttpPost]
        public ActionResult VerifyRestoreDB(int reqid)
        {
            string fileName = "";
            var dbdata = db.RestoreDb.Where(x => x.ReqID == reqid).FirstOrDefault();
            fileName = dbdata.FileName;
            REDTR.DB.BusinessObjects.Job job = new REDTR.DB.BusinessObjects.Job();
            string DataFile = string.Empty;
            string DataFilePath;
            string folderPath;
            string LogFile = string.Empty;
            string LogFilePath;
            string database = Utilities.getConnectionString("DefaultConnection");
            string[] db1 = database.Split(';');
            string[] dbname = db1[1].Split('=');

            try
            {
                string backupDIRBeforBakup = Utilities.getAppSettings("DBBackBeforeResore");
                if (!System.IO.Directory.Exists(backupDIRBeforBakup))
                {
                    System.IO.Directory.CreateDirectory(backupDIRBeforBakup);
                }
                string path = Convert.ToString(Utilities.getAppSettings("DatabaseBackUpPath")) + "\\" + fileName;

                SqlConnection objSaConn = new SqlConnection(GetConnStr().Replace("Database=" + dbname + ";", "Database=master;"));
                SqlCommand cmd;
                objSaConn.Open();
                SqlCommand sqlcmd = new SqlCommand("backup database " + dbname[1] + " to disk='" + backupDIRBeforBakup + "\\" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + "_" + dbname[1] + "_.Bak' with stats = 1", objSaConn);
                sqlcmd.CommandTimeout = 0;
                sqlcmd.ExecuteNonQuery();
                cmd = new SqlCommand("RESTORE FILELISTONLY FROM DISK = '" + path + "' WITH FILE = 1", objSaConn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    if (rd["Type"].ToString() == "D")
                    {
                        DataFile = rd["LogicalName"].ToString();
                        DataFilePath = rd["PhysicalName"].ToString();
                        folderPath = rd["PhysicalName"].ToString().Substring(0, rd["PhysicalName"].ToString().LastIndexOf("\\"));
                    }
                    else if (rd["Type"].ToString() == "L")
                    {
                        LogFile = rd["LogicalName"].ToString();
                        LogFilePath = rd["PhysicalName"].ToString();
                    }
                }

                rd.Close();
                cmd = new SqlCommand("ALTER DATABASE " + dbname[1] + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE ", objSaConn);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("Use Master", objSaConn);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                string backupDIR = Convert.ToString(@"D:\AUTO_DB_BACKUP");
                if (!System.IO.Directory.Exists(backupDIR))
                {
                    System.IO.Directory.CreateDirectory(backupDIR);
                }
                DirectoryInfo outputDirectory = new DirectoryInfo(@"D:\AUTO_DB_BACKUP");
                DataFilePath = @"D:\AUTO_DB_BACKUP" + "\\" + dbname[1] + ".mdf";
                LogFilePath = @"D:\AUTO_DB_BACKUP" + "\\" + dbname[1] + "_log.ldf";
                objSaConn.FireInfoMessageEventOnUserErrors = true;
                objSaConn.InfoMessage += OnInfoMessage;
                String sqlString;
                sqlString = Environment.NewLine + string.Format("EXEC('restore database [{0}] from disk = ''{1}'' with stats = 1, move ''{2}'' to ''{3}'', move ''{4}'' to ''{5}'', norewind, nounload, replace')", dbname[1], path, DataFile, DataFilePath, LogFile, LogFilePath);

                cmd = new SqlCommand(sqlString, objSaConn);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("ALTER DATABASE " + dbname[1] + " SET MULTI_USER", objSaConn);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                objSaConn.Close();

                objSaConn.InfoMessage -= OnInfoMessage;
                objSaConn.FireInfoMessageEventOnUserErrors = false;
                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.SendMsgDbRestoreProcesingComp, 100, 100);
                string fpath = Utilities.getAppSettings("RestoreDbFilePath");
                if (!System.IO.Directory.Exists(fpath))
                {
                    System.IO.Directory.CreateDirectory(fpath);
                }
                using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(fpath + "\\Db.txt", true))
                {
                    file.WriteLine("FileName:- "+fileName + "      User:- " + User.FirstName + "     " + DateTime.Now);
                }
                var userData = User;
                batchNotificationHlpr.notifyPerimissionHoldersSettings(fileName, BatchEventType.VerifyRestoreDbRequest, userData);
                trail.AddTrail(TnT.LangResource.GlobalRes.SettingDbResoreSuccessfuly + User.FirstName, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TrailSettingsDatabaseBackUpSuccessfully + " " + User.FirstName, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return Json(true);
            }
            catch (Exception ex)
            {
                trail.AddTrail(TnT.LangResource.GlobalRes.TrailSettingsDBConnectionLost, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TrailSettingsDBConnectionLost, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return Json(false);
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }


    public class FileData
    {
        public string Filename { get; set; }
        public string CreatedDate { get; set; }
    }
}
