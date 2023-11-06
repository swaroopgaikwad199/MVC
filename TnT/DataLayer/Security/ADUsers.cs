using REDTR.HELPER;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web.Security;
using TnT.Models;

namespace TnT.DataLayer.Security
{
    public enum AppUserType
    {
        ADUser = 0,
        AppUser = 1
    }
    public class ADUsers
    {

        #region Listing All Users
        /// <summary>
        /// Lists all the users from current domain
        /// </summary>
        public List<ADData> getADUsers(List<TnT.Models.SettingsNUtility.Roles> LstRoles)
        {
            //List<string> lstusers = new List<string>();
            List<ADData> newUsrList = new List<ADData>();
            try
            {
                string OU = "";
                string Path = Utilities.getAppSettings("ADGroupPath");
                string[] ADGroupPath = Path.Split('/');
                for (int i = 0; i < ADGroupPath.Length; i++)
                {
                    OU = "OU=" + ADGroupPath[i] + "," + OU;
                }
                OU = OU.TrimEnd(',');
                string[] domain = Utilities.getAppSettings("DomainName").Split('.');
                for (int j = 0; j < domain.Length; j++)
                {
                    OU = OU + ",dc=" + domain[j];
                }


                // using (var context = new PrincipalContext(ContextType.Domain,"192.168.10.183", OU))
                using (var context = new PrincipalContext(ContextType.Domain, Utilities.getAppSettings("DomainName")))
                {
                    foreach (var item in LstRoles)
                    {
                        //  using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                        using (var searcher = GroupPrincipal.FindByIdentity(context, item.ADRole.Trim()))
                        {
                            var users = searcher.GetMembers(true);
                            // foreach (var result in searcher.FindAll())
                            foreach (UserPrincipal result in users)
                            {
                                DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                                ADData actData = new ADData();
                                actData.Name = de.Properties["samAccountName"].Value.ToString();
                                actData.RoleID = item.ID;
                                var status = result.Enabled;
                                if (((System.DirectoryServices.AccountManagement.UserPrincipal)result).EmailAddress != null)
                                {
                                    actData.EmailId = ((System.DirectoryServices.AccountManagement.UserPrincipal)result).EmailAddress.ToString();
                                }
                                if (status == false)
                                {
                                    actData.isActive = false;
                                }
                                else
                                {
                                    actData.isActive = true;
                                }
                                newUsrList.Add(actData);
                            }
                        }
                    }
                }


                //DirectoryEntry directoryEntry1 = new DirectoryEntry("LDAP://DC=propixtech,DC=co");
                //var allU = directoryEntry1.Children;
                //DirectoryEntry directoryEntry = new DirectoryEntry("WinNT://" + Environment.UserDomainName);
                //string userNames = "";
                //string authenticationType = "";
                //foreach (DirectoryEntry child in directoryEntry1.Children)
                //{
                //    if (child.SchemaClassName == "User")
                //    {
                //        userNames += child.Name + Environment.NewLine; //Iterates and binds all user using a newline
                //        authenticationType += child.Username + Environment.NewLine;
                //        lstusers.Add(child.Name);
                //    }
                //}
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

            return newUsrList;
            //return lstusers;
        }
        #endregion


        public static bool ValidateUser(string contextOption, string serverName, string DomainName, string userName, string password)
        {
            bool valid = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(contextOption))
                {
                    ContextOptions co = (ContextOptions)Enum.Parse(typeof(ContextOptions), contextOption);

                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain, serverName))
                    {
                        valid = context.ValidateCredentials(System.IO.Path.GetFileNameWithoutExtension(DomainName) + "\\" + userName, password, co);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valid;
        }


        public int SyncUsers(List<ADData> users)
        {
            int instCnt = 0;
            ApplicationDbContext db = new ApplicationDbContext();
            List<Models.Account.Users> PropixUsers = db.Users.Where(u => u.UserType == 0).ToList();
            foreach (var item in users)
            {
                var cnt = db.Users.Where(u => u.UserName == item.Name).Count();
                var userRecord = db.Users.Where(u => u.UserName == item.Name).FirstOrDefault();
                PropixUsers.Remove(userRecord);
                if (cnt == 0)
                {
                    REDTR.DB.BusinessObjects.Users usr = new REDTR.DB.BusinessObjects.Users();
                    usr.UserName = item.Name;
                    usr.UserName1 = item.Name;
                    usr.Password = "";
                    usr.RoleID = item.RoleID;
                    usr.LastUpdatedDate = DateTime.Now;
                    usr.Active = true;
                    usr.IsFirstLogin = true;
                    usr.CreatedDate = DateTime.Now;
                    usr.UserType = Convert.ToInt32(AppUserType.ADUser);
                    usr.EmailId = item.EmailId;
                    DbHelper m_dbhelper = new DbHelper();
                    var id = m_dbhelper.DBManager.UsersBLL.InsertOrUpdateUsers(REDTR.DB.BLL.UsersBLL.UsersOp.AddUser, usr);
                    instCnt++;
                }
                else
                {
                    if (userRecord.RoleID != item.RoleID || userRecord.Active != item.isActive)
                    {
                        REDTR.DB.BusinessObjects.Users usr = new REDTR.DB.BusinessObjects.Users();
                        usr.ID = userRecord.ID;
                        usr.UserName = item.Name;
                        usr.UserName1 = item.Name;
                        usr.Password = "";
                        usr.RoleID = item.RoleID;
                        usr.LastUpdatedDate = DateTime.Now;
                        usr.Active = item.isActive;
                        usr.IsFirstLogin = true;
                        usr.CreatedDate = DateTime.Now;
                        usr.UserType = Convert.ToInt32(AppUserType.ADUser);
                        usr.EmailId = item.EmailId;
                        DbHelper m_dbhelper = new DbHelper();
                        var id = m_dbhelper.DBManager.UsersBLL.InsertOrUpdateUsers(REDTR.DB.BLL.UsersBLL.UsersOp.UpdateUser, usr);
                        instCnt++;
                    }
                }

            }

            foreach (var user in PropixUsers)
            {
                if (user.Active != false)
                {
                    REDTR.DB.BusinessObjects.Users usr = new REDTR.DB.BusinessObjects.Users();
                    usr.ID = user.ID;
                    usr.UserName = user.UserName;
                    usr.UserName1 = user.UserName1;
                    usr.Password = "";
                    usr.RoleID = user.RoleID;
                    usr.LastUpdatedDate = DateTime.Now;
                    usr.Active = false;
                    usr.IsFirstLogin = true;
                    usr.CreatedDate = DateTime.Now;
                    usr.UserType = Convert.ToInt32(AppUserType.ADUser);
                    usr.EmailId = user.EmailId;
                    DbHelper m_dbhelper = new DbHelper();
                    var id = m_dbhelper.DBManager.UsersBLL.InsertOrUpdateUsers(REDTR.DB.BLL.UsersBLL.UsersOp.UpdateUser, usr);
                    instCnt++;
                }
            }
            return instCnt;
        }

    }


    public class ADData
    {
        public string Name { get; set; }
        public string EmailId { get; set; }
        public int RoleID { get; set; }
        public bool isActive { get; set; }
    }

}

