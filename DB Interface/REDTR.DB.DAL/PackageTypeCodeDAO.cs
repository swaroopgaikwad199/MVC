using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public class PackageTypeCodeDAO
	{
		public PackageTypeCodeDAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<PackageTypeCode> GetPackageTypeCodes(int Flag)
		{
			try
			{
				List<PackageTypeCode> lstPackageTypeCodes = new List<PackageTypeCode>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GETPackagingTypeCode", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, DBNull.Value));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					PackageTypeCode oPackageTypeCode = new PackageTypeCode();
					oPackageTypeCode.Code = Convert.ToString(oDbDataReader["Code"]);
					oPackageTypeCode.CodeSeq = Convert.ToSByte(oDbDataReader["CodeSeq"]);

					if(oDbDataReader["Name"] != DBNull.Value)
						oPackageTypeCode.Name = Convert.ToString(oDbDataReader["Name"]);

					if(oDbDataReader["Description"] != DBNull.Value)
						oPackageTypeCode.Description = Convert.ToString(oDbDataReader["Description"]);

					if(oDbDataReader["Remarks"] != DBNull.Value)
						oPackageTypeCode.Remarks = Convert.ToString(oDbDataReader["Remarks"]);
					lstPackageTypeCodes.Add(oPackageTypeCode);
				}
				oDbDataReader.Close();
				return lstPackageTypeCodes;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public PackageTypeCode GetPackageTypeCode(int Flag ,string Code)
		{
			try
			{
				PackageTypeCode oPackageTypeCode = new PackageTypeCode();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GETPackagingTypeCode", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.String, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Code));

				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					oPackageTypeCode.Code = Convert.ToString(oDbDataReader["Code"]);
					oPackageTypeCode.CodeSeq = Convert.ToSByte(oDbDataReader["CodeSeq"]);

					if(oDbDataReader["Name"] != DBNull.Value)
						oPackageTypeCode.Name = Convert.ToString(oDbDataReader["Name"]);

					if(oDbDataReader["Description"] != DBNull.Value)
						oPackageTypeCode.Description = Convert.ToString(oDbDataReader["Description"]);

					if(oDbDataReader["Remarks"] != DBNull.Value)
						oPackageTypeCode.Remarks = Convert.ToString(oDbDataReader["Remarks"]);
				}
				oDbDataReader.Close();
				return oPackageTypeCode;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public int AddPackageTypeCode(PackageTypeCode oPackageTypeCode)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdatePackageType]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@code", DbType.String, oPackageTypeCode.Code));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Name", DbType.String, oPackageTypeCode.Name));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Description", DbType.String, oPackageTypeCode.Description));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oPackageTypeCode.Remarks));

                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }	
		public int RemovePackageTypeCode(string Code)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("DELETEPackageTypeCode",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Code",DbType.String,Code));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
