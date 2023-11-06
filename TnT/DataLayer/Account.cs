using TnT.Models;
using TnT.Models.Account;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TnT.DataLayer.Security;

namespace TnT.DataLayer
{
    public class Account
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public int getUserType(string UserName)
        {
            var ur = db.Users.Where(u => u.UserName == UserName).FirstOrDefault();
            if (ur != null)
            {
                if (ur.UserType == null)
                {
                    return 1;
                }
                else
                {
                    return Convert.ToInt32(ur.UserType);
                }

            }
            return -1;
        }

        public void Deactivateuser(string userName)
        {

            Users usr = db.Users.Where(x => x.UserName == userName || x.UserName1 == userName).FirstOrDefault();

            if (usr != null)
            {
                usr.Active = false;
                usr.LastUpdatedDate = DateTime.Now;
                db.Entry(usr).State = System.Data.Entity.EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

            }
        }

        public Users getUser(string UserName, string Password)
        {
            try
            {
                string pass = encrypt(Password);

                return db.Users.Where(u => u.UserName == UserName && u.Password == pass).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool validateUser(string UserName, string Password)
        {

            string pass = encrypt(Password);
            var uuu = db.Users.Where(u => u.UserName == UserName && u.Password == pass).FirstOrDefault();
            if (uuu != null)
            {
                return true;
            }

            return false;
        }

        public Users getUser(string UserName)
        {
            try
            {
                return db.Users.Where(u => u.UserName == UserName).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string getRoleName(int RoleId)
        {
            try
            {
                string RoleName = "";
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                using (var cn = new SqlConnection(connectionString))
                {
                    string _sql = @"SELECT [Roles_Name]    FROM [dbo].[Roles]  where  ID = @RID";
                    var cmd = new SqlCommand(_sql, cn);
                    cmd.Parameters
                        .Add(new SqlParameter("@RID", SqlDbType.NVarChar))
                        .Value = RoleId;


                    cn.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            RoleName = reader.GetValue(0).ToString();
                        }

                        reader.Dispose();
                        cmd.Dispose();
                        return RoleName;
                    }
                    else
                    {
                        RoleName = "";
                        reader.Dispose();
                        cmd.Dispose();
                        return RoleName;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        public bool IsValid(string _username, string _password)
        {
            try
            {
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                using (var cn = new SqlConnection(connectionString))
                {
                    string _sql = @"SELECT [Username] FROM [dbo].[Users] " +
                           @"WHERE [Username] = @u AND [Password] = @p";
                    var cmd = new SqlCommand(_sql, cn);
                    cmd.Parameters
                        .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                        .Value = _username;

                    string encryPass = encrypt(_password);
                    cmd.Parameters
                        .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                        .Value = encryPass;
                    cn.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Dispose();
                        cmd.Dispose();
                        return true;
                    }
                    else
                    {
                        reader.Dispose();
                        cmd.Dispose();
                        return false;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void registerUserLogin(decimal userId, string SessionId)
        {
           
            var result = db.Logins.Where(b => b.UserId == userId).FirstOrDefault();
            if (result != null)
            {
                result.LoggedIn = true;
                result.LastUpdated = DateTime.Now;
                result.SessionId = "Web Server";
                db.SaveChanges();
            }
            else
            {
                Logins logins = new Logins();
                logins.UserId = userId;
                logins.SessionId = "Web Server";
                logins.LoggedIn = true;
                logins.LastUpdated = DateTime.Now;
                db.Logins.Add(logins);
                db.SaveChanges();
            }


        }
        public void updateUserLogin(decimal userId)
        {
            var result = db.Logins.Where(b => b.UserId == userId).FirstOrDefault();
            if (result != null)
            {
                result.LastUpdated = DateTime.Now;
                db.SaveChanges();
            }

        }

        public bool IsUserLoggedIn(decimal UserId)
        {
            int checkTimeOfExpnInSec = Convert.ToInt32(Utilities.getAppSettings("ExecutionIdleTimeSeconds"));
            var date = db.Logins.Where(x => x.UserId == UserId).FirstOrDefault();
            
            if (date !=null)
            {
                if (date.LoggedIn)
                {
                    var differnceTime = (DateTime.Now - date.LastUpdated).TotalSeconds;
                    if (differnceTime <= checkTimeOfExpnInSec)
                    {
                        return true;
                    }
                    if (differnceTime > checkTimeOfExpnInSec)
                    {
                        removeLogginSession(UserId);
                        return false;
                    }
                }                
            }
            return false;
        }
        public void removeLogginSession(decimal userId)
        {
            var log = db.Logins.Where(x => x.UserId == userId).FirstOrDefault();
            if (log != null) {
                log.LoggedIn = false;
                db.SaveChanges();
            }            
        }
       
        private string encrypt(string password)
        {
            try
            {
                string encrypPass = PTPLCRYPTORENGINE.AESCryptor.Encrypt(password, "PTPLCRYPTOSYS");
                return encrypPass;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string decrypt(string password)
        {
            try
            {
                string decrypPass = PTPLCRYPTORENGINE.AESCryptor.Decrypt(password, "PTPLCRYPTOSYS");
                return decrypPass;
            }
            catch (Exception)
            {
                throw;
            }

        }

    }

   
}
