using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public partial class PermissionsDAO
	{
		public PermissionsDAO()
		{
			DbProviderHelper.GetConnection();
		}
        public List<Permissions> GetPermissionss(int Flag)
		{
			try
			{
				List<Permissions> lstPermissionss = new List<Permissions>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPermissions]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, DBNull.Value));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					Permissions oPermissions = new Permissions();
					oPermissions.ID = Convert.ToInt32(oDbDataReader["ID"]);

					if(oDbDataReader["Permission"] != DBNull.Value)
						oPermissions.Permission = Convert.ToString(oDbDataReader["Permission"]);

					if(oDbDataReader["Remarks"] != DBNull.Value)
						oPermissions.Remarks = Convert.ToString(oDbDataReader["Remarks"]);
					lstPermissionss.Add(oPermissions);
				}
				oDbDataReader.Close();
				return lstPermissionss;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public Permissions GetPermission(int Flag, string Param)
		{
			try
			{
				Permissions oPermissions = new Permissions();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPermissions]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Param.ToString()));			
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					oPermissions.ID = Convert.ToInt32(oDbDataReader["ID"]);

					if(oDbDataReader["Permission"] != DBNull.Value)
						oPermissions.Permission = Convert.ToString(oDbDataReader["Permission"]);

					if(oDbDataReader["Remarks"] != DBNull.Value)
						oPermissions.Remarks = Convert.ToString(oDbDataReader["Remarks"]);
				}
				oDbDataReader.Close();
				return oPermissions;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public int InsertOrUpdatePermissions(int Flag, Permissions oPermissions)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdatePermissions]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PermissionId", DbType.Int64, oPermissions.ID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("Permission", DbType.String, oPermissions.Permission));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oPermissions.Remarks));

                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		public int RemovePermissions(decimal ID)
		{

			try
			{
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_DeletePermissions]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, ID));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
