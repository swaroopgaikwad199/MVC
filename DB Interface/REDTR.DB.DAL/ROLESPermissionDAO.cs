using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public class ROLESPermissionDAO
	{
		public ROLESPermissionDAO()
		{
			DbProviderHelper.GetConnection();
		}
        public List<ROLESPermission> GetROLESPermissions(int Flag, Nullable<Decimal> ID)
		{
			try
			{
				List<ROLESPermission> lstROLESPermissions = new List<ROLESPermission>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETRolesPermissions]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                   oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, ID));
              DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					ROLESPermission oROLESPermission = new ROLESPermission();
					oROLESPermission.ID = Convert.ToDecimal(oDbDataReader["ID"]);

					if(oDbDataReader["Roles_Id"] != DBNull.Value)
						oROLESPermission.Roles_Id = Convert.ToInt32(oDbDataReader["Roles_Id"]);

					if(oDbDataReader["Permission_Id"] != DBNull.Value)
						oROLESPermission.Permission_Id = Convert.ToDecimal(oDbDataReader["Permission_Id"]);

					if(oDbDataReader["Remarks"] != DBNull.Value)
						oROLESPermission.Remarks = Convert.ToString(oDbDataReader["Remarks"]);
					lstROLESPermissions.Add(oROLESPermission);
				}
				oDbDataReader.Close();
				return lstROLESPermissions;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public ROLESPermission GetROLESPermission(int Flag,Decimal ID)
		{
			try
			{
				ROLESPermission oROLESPermission = new ROLESPermission();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETRolesPermissions]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID",DbType.Decimal,ID));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					oROLESPermission.ID = Convert.ToDecimal(oDbDataReader["ID"]);

					if(oDbDataReader["Roles_Id"] != DBNull.Value)
						oROLESPermission.Roles_Id = Convert.ToInt32(oDbDataReader["Roles_Id"]);

					if(oDbDataReader["Permission_Id"] != DBNull.Value)
						oROLESPermission.Permission_Id = Convert.ToDecimal(oDbDataReader["Permission_Id"]);

					if(oDbDataReader["Remarks"] != DBNull.Value)
						oROLESPermission.Remarks = Convert.ToString(oDbDataReader["Remarks"]);
				}
				oDbDataReader.Close();
				return oROLESPermission;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int AddROLESPermission(ROLESPermission oROLESPermission)
		{
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateRolePermissions]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RoleId", DbType.Int32, oROLESPermission.Roles_Id));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PermissionsId", DbType.Decimal, oROLESPermission.Permission_Id));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oROLESPermission.Remarks));
                return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}
        public int AddROLESPermission(int Flag, ROLESPermission oROLESPermission)//Added by Arvind on 04.05.2015
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateRolePermissions]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RoleId", DbType.Int32, oROLESPermission.Roles_Id));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PermissionsId", DbType.Decimal, oROLESPermission.Permission_Id));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oROLESPermission.Remarks));
                // oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Permission", DbType.String, oROLESPermission.Permission));
                return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		public int RemoveROLESPermission(int Flag,Decimal RoleID, int PermissionID)
		{
			try
			{
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_DeleteRolePermissions]", CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag",DbType.Decimal,Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PermissionID", DbType.Decimal, PermissionID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RoleID", DbType.Decimal, RoleID));

				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}	
	}
}
