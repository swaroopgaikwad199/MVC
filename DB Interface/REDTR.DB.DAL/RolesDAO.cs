using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public partial class RolesDAO
	{
		public RolesDAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<Roles> GetRoless(int Flag)
		{
			try
			{
				List<Roles> lstRoless = new List<Roles>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETRoles]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String , DBNull.Value));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					Roles oRoles = new Roles();
					oRoles.ID = Convert.ToInt32(oDbDataReader["ID"]);
					oRoles.Roles_Name = Convert.ToString(oDbDataReader["Roles_Name"]);

					if(oDbDataReader["Remarks"] != DBNull.Value)
						oRoles.Remarks = Convert.ToString(oDbDataReader["Remarks"]);
					lstRoless.Add(oRoles);
				}
				oDbDataReader.Close();
				return lstRoless;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public Roles GetRoles(int Flag, string Param)
		{
			try
			{
				Roles oRoles = new Roles();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETRoles]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Param));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					oRoles.ID = Convert.ToInt32(oDbDataReader["ID"]);
					oRoles.Roles_Name = Convert.ToString(oDbDataReader["Roles_Name"]);

					if(oDbDataReader["Remarks"] != DBNull.Value)
						oRoles.Remarks = Convert.ToString(oDbDataReader["Remarks"]);
				}
				oDbDataReader.Close();
				return oRoles;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int AddRoles(Roles oRoles)
		{
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateRoles]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RoleId", DbType.Decimal, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Roles_Name", DbType.String, oRoles.Roles_Name));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oRoles.Remarks));
                return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}
        public int AddRolesForSync(Roles oRoles)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateRolesForSync]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RoleId", DbType.Decimal, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Roles_Name", DbType.String, oRoles.Roles_Name));
                if (oRoles.Remarks != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oRoles.Remarks));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, "RoleSync"));

                return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }	
		public int RemoveRoles(int ID)
		{

			try
			{
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_DeleteRoles]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Roleid", DbType.Int32, ID));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
