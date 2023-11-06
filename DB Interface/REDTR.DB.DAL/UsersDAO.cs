using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
    public partial class UsersDAO
    {
        public UsersDAO()
        {
            DbProviderHelper.GetConnection();
        }
        public List<Users> GetUserss(int Flag, string ParamVal)
        {
            try
            {
                List<Users> lstUserss = new List<Users>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETUsers]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, ParamVal));

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    Users oUsers = new Users();
                    oUsers.ID = Convert.ToDecimal(oDbDataReader["ID"]);

                    if (oDbDataReader["UserName"] != DBNull.Value)
                        oUsers.UserName = Convert.ToString(oDbDataReader["UserName"]);

                    if (oDbDataReader["UserName1"] != DBNull.Value)
                        oUsers.UserName1 = Convert.ToString(oDbDataReader["UserName1"]);

                    if (oDbDataReader["Password"] != DBNull.Value)
                        oUsers.Password = Convert.ToString(oDbDataReader["Password"]);

                    if (oDbDataReader["RoleID"] != DBNull.Value)
                        oUsers.RoleID = Convert.ToInt32(oDbDataReader["RoleID"]);

                    if (oDbDataReader["UserType"] != DBNull.Value)
                        oUsers.RoleID = Convert.ToInt32(oDbDataReader["UserType"]);

                    if (oDbDataReader["Active"] != DBNull.Value)
                        oUsers.Active = Convert.ToBoolean(oDbDataReader["Active"]);

                    if (oDbDataReader["IsFirstLogin"] != DBNull.Value)
                        oUsers.IsFirstLogin = Convert.ToBoolean(oDbDataReader["IsFirstLogin"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oUsers.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["EmailId"] != DBNull.Value)
                        oUsers.EmailId = Convert.ToString(oDbDataReader["EmailId"]);

                    if (oDbDataReader["CreatedDate"] != DBNull.Value)
                        oUsers.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);

                    if (oDbDataReader["LastUpdatedDate"] != DBNull.Value)
                        oUsers.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);
                    lstUserss.Add(oUsers);
                }
                oDbDataReader.Close();
                return lstUserss;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Users GetUsers(int Flag, string ParamVal)
        {
            try
            {
                Users oUsers = new Users();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETUsers]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, ParamVal));

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    oUsers.ID = Convert.ToDecimal(oDbDataReader["ID"]);

                    if (oDbDataReader["UserName"] != DBNull.Value)
                        oUsers.UserName = Convert.ToString(oDbDataReader["UserName"]);

                    if (oDbDataReader["UserName1"] != DBNull.Value)
                        oUsers.UserName1 = Convert.ToString(oDbDataReader["UserName1"]);

                    if (oDbDataReader["Password"] != DBNull.Value)
                        oUsers.Password = Convert.ToString(oDbDataReader["Password"]);

                    if (oDbDataReader["RoleID"] != DBNull.Value)
                        oUsers.RoleID = Convert.ToInt32(oDbDataReader["RoleID"]);

                    if (oDbDataReader["UserType"] != DBNull.Value)
                        oUsers.RoleID = Convert.ToInt32(oDbDataReader["UserType"]);

                    if (oDbDataReader["Active"] != DBNull.Value)
                        oUsers.Active = Convert.ToBoolean(oDbDataReader["Active"]);

                    if (oDbDataReader["IsFirstLogin"] != DBNull.Value)
                        oUsers.IsFirstLogin = Convert.ToBoolean(oDbDataReader["IsFirstLogin"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oUsers.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["CreatedDate"] != DBNull.Value)
                        oUsers.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);

                    if (oDbDataReader["EmailId"] != DBNull.Value)
                        oUsers.EmailId = Convert.ToString(oDbDataReader["EmailId"]);

                    if (oDbDataReader["LastUpdatedDate"] != DBNull.Value)
                        oUsers.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);
                }
                oDbDataReader.Close();
                return oUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDSUSer(int Flag, string ParamVal)
        {
            try
            {
                DataSet ds;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETUsers]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, ParamVal));
                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                ds = DbProviderHelper.FillDataSet(DbAdpt);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdateUsers(int Flag, Users oUsers)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateUsers]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, oUsers.ID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserName", DbType.String, oUsers.UserName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@EmailId", DbType.String, oUsers.EmailId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserName1", DbType.String, oUsers.UserName1));
                
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Password", DbType.String, oUsers.Password));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RoleID", DbType.Int32, oUsers.RoleID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Active", DbType.Boolean, oUsers.Active));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsFirstLoginAtempt", DbType.Boolean, oUsers.IsFirstLogin));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oUsers.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.Int64, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserType", DbType.Int32, oUsers.UserType));
                DbProviderHelper.ExecuteNonQuery(oDbCommand);

                if (oDbCommand.Parameters["@UID"].Value != DBNull.Value)
                    return Convert.ToInt32(oDbCommand.Parameters["@UID"].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdateUsersForSync(int Flag, Users oUsers)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateUsersForDataSync]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, oUsers.ID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserName", DbType.String, oUsers.UserName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserName1", DbType.String, oUsers.UserName1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Password", DbType.String, oUsers.Password));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RoleID", DbType.Int32, oUsers.RoleID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Active", DbType.Boolean, oUsers.Active));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsFirstLoginAtempt", DbType.Boolean, oUsers.IsFirstLogin));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oUsers.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserType", DbType.Int32, oUsers.UserType));
                return Convert.ToInt32(DbProviderHelper.ExecuteNonQuery(oDbCommand));
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
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_DeleteUsers]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserId", DbType.Decimal, ID));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
