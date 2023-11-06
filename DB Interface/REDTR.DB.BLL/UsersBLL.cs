using System;
using System.Collections.Generic;
using System.Data;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public partial class UsersBLL
    {
        public enum UsersOp
        {
            GetUserss = 1, //Param[@Flag]      
            GetUsers = 2,//Param[@Flag,@Paramval as UserId]      
            GetUserssOfRoles = 3,//Param[@Flag,@Paramval as RoleId]     
            GetUserssOfUserName = 4,//Param[@Flag,@Paramval as UserName]     
            GetActiveUserDetails = 5,//Param[@Flag]  
            GetUserDtlsRpt = 6,
            GetUsersofName = 7,
            GetAllUserDetails = 9, ////Param[@Flag]  Sunil For Active Directory.


            GetUsersGrThanLUD=8,
            //added to Sync Users from server............
            AddUserForSync = 1,
            UpdateUserForSync = 2,
            AddUser = 1,
            UpdateUser = 2,
            UpdateActiveUser = 3,
        }
        private UsersDAO _UsersDAO;

        public UsersDAO UsersDAO
        {
            get { return _UsersDAO; }
            set { _UsersDAO = value; }
        }

        public UsersBLL()
        {
            UsersDAO = new UsersDAO();
        }
        public List<Users> GetUserss(UsersOp Op, string Param)
        {
            try
            {
                return UsersDAO.GetUserss((int)Op, Param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Users> GetUserss(Nullable<DateTime> lastUpdatedDate, string format)
        {
            try
            {
                string dt = null;
                if (string.IsNullOrEmpty(format))
                    format = "yyyy-MM-dd HH:mm:ss.fff";
                if (lastUpdatedDate != null)
                    dt = ((DateTime)lastUpdatedDate).ToString(format);
                return UsersDAO.GetUserss((int)UsersOp.GetUsersGrThanLUD, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Users GetUsers(UsersOp Op, string Param)
        {
            try
            {
                return UsersDAO.GetUsers((int)Op, Param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDataset(UsersOp Op, string Param)
        {
            try
            {
                return UsersDAO.GetDSUSer((int)Op, Param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdateUsers(UsersOp Op, Users oUsers)
        {
            try
            {
                return UsersDAO.InsertOrUpdateUsers((int)Op, oUsers);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdateUsersForSync(UsersOp Op, Users oUsers)
        {
            try
            {
                return UsersDAO.InsertOrUpdateUsersForSync((int)Op, oUsers);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveUsers(Decimal ID)
        {
            try
            {
                return UsersDAO.RemoveUsers(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Users> DeserializeUserss(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<Users>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializeUserss(string Path, List<Users> Userss)
        {
            try
            {
                GenericXmlSerializer<List<Users>>.Serialize(Userss, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
