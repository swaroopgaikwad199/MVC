using PTPLCRYPTORENGINE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TnT.Models;
using TnT.Models.Account;
using TnT.Models.SettingsNUtility;

namespace TnT.DataLayer.Security
{
    public class SecurityUtils
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<Permissions> getPermissions(int RoleId)
        {
            return db.ROLESPermission.Where(x => x.Roles_Id == RoleId).Select(x => x.Permissions).Distinct().ToList();
        }

        public List<Permissions> getPermissionsList()
        {
            return db.Permissions.Where(x => x.IsActive == true).ToList();
            //return db.ROLESPermission.Where(x => x.Roles_Id == RoleId).Select(x => x.Permissions ).Distinct().ToList();
        }

        public string getRoleName(int RoleId)
        {
            return db.ROLESPermission.Where(x => x.Roles_Id == RoleId).Select(x => x.Roles.Roles_Name).FirstOrDefault();

        }
        public bool IsPasswordExpired(DateTime DtLastupdated)
        {
            string days = db.AppSettings.Where(x => x.Key == "PasswordExpiryDays").Select(x => x.Value).FirstOrDefault();
            int Expdays = 0;
            if (string.IsNullOrEmpty(days))
            {
                Expdays = 1;
            }
            else
            {
                Expdays = Convert.ToInt32(days);
            }

            var ExpiryDt = DtLastupdated + TimeSpan.FromDays(Expdays);

            if (DateTime.Now < ExpiryDt)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public int PasswordExpiryDays(DateTime DtLastupdated)
        {

            int passwordExpired = 0;

            int passwordExp = Convert.ToInt32(Utilities.getAppSettings("PasswordExpiryDaysLeft"));
            //var user = db.Users.Where(x => x.ID == userid).FirstOrDefault();
            //DateTime DtLastupdated = user.LastUpdatedDate;
            string days = db.AppSettings.Where(x => x.Key == "PasswordExpiryDays").Select(x => x.Value).FirstOrDefault();
            int Expdays = 0;
            if (string.IsNullOrEmpty(days))
            {
                Expdays = 1;
            }
            else
            {
                Expdays = Convert.ToInt32(days);
            }
            int expdaysleft = Expdays - passwordExp;
            var ExpiryDt = DtLastupdated + TimeSpan.FromDays(expdaysleft);

            if (DateTime.Now > ExpiryDt)
            {

                var expdate = DateTime.Now.Date - DtLastupdated.Date;
                passwordExpired = Expdays - expdate.Days;

            }

            return passwordExpired;
        }

        public bool IsPasswordExistng(decimal UserId, string AtemptPassword)
        {
            var apass = AESCryptor.Encrypt(AtemptPassword, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
            int cnt = Convert.ToInt32(Utilities.getAppSettings("PrevPswdCheck"));
            var passList = db.UserLoginPswds.Where(x => x.UserId == UserId).OrderByDescending(x=>x.LastUpdated).Select(x=>x.Password);
            var plist = passList.Take(cnt);
            if (passList == null) return false;
              return plist.Contains(apass);

            //var PrevPasswords = db.M_UserPasswords.Where(x => x.UserId == UserId).FirstOrDefault();
            //if (PrevPasswords == null)
            //{
            //    return false;
            //}

            //string OldPass = AESCryptor.Decrypt(PrevPasswords.PasswordOld, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
            //string OlderPass = AESCryptor.Decrypt(PrevPasswords.PasswordOlder, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
            //string OldestPass = AESCryptor.Decrypt(PrevPasswords.PasswordOldest, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);

            //if (OldPass == AtemptPassword)
            //{
            //    return true;
            //}
            //else if (OlderPass == AtemptPassword)
            //{
            //    return true;
            //}
            //else if (OldestPass == AtemptPassword)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }


        public void updatePasswords(decimal UserId, string NewPassword)
        {
            UserLoginPswds userPswd = new UserLoginPswds();
            userPswd.UserId = UserId;
            userPswd.Password = AESCryptor.Encrypt(NewPassword.Trim(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
            userPswd.LastUpdated = DateTime.Now;
            db.UserLoginPswds.Add(userPswd);
            db.SaveChanges();

            //string existinigPass = db.Users.Find(UserId).Password;
            //var ExistingPasswords = db.M_UserPasswords.Where(x => x.UserId == UserId).FirstOrDefault();
            ////  string EncNewPassword = AESCryptor.Encrypt(NewPassword, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
            //string EncNewPassword = AESCryptor.Encrypt(NewPassword.Trim(), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
            //if (ExistingPasswords == null)
            //{
            //    M_UserPasswords usr = new M_UserPasswords();
            //    usr.UserId = UserId;
            //    usr.PasswordOld = EncNewPassword;
            //    usr.PasswordOlder = existinigPass;
            //    usr.PasswordOldest = existinigPass;
            //    db.M_UserPasswords.Add(usr);
            //    db.SaveChanges();

            //}
            //else
            //{
            //    var older = ExistingPasswords.PasswordOld;
            //    var oldest = ExistingPasswords.PasswordOlder;

            //    ExistingPasswords.PasswordOld = EncNewPassword;

            //    ExistingPasswords.PasswordOlder = older;
            //    ExistingPasswords.PasswordOldest = oldest;

            //    db.Entry(ExistingPasswords).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //}


        }

    }
}