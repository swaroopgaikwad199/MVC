using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;
using PTPLCRYPTORENGINE;

namespace REDTR.DB.DAL
{
	public partial class PrimaryPackDummysDAO
	{
		public PrimaryPackDummysDAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<PrimaryPackDummys> GetPrimaryPackDummyss()
		{
			try
			{
				List<PrimaryPackDummys> lstPrimaryPackDummyss = new List<PrimaryPackDummys>();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("SELECTPrimaryPackDummyss",CommandType.StoredProcedure);
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					PrimaryPackDummys oPrimaryPackDummys = new PrimaryPackDummys();
					oPrimaryPackDummys.Id = Convert.ToInt32(oDbDataReader["Id"]);
					oPrimaryPackDummys.JobId = Convert.ToInt32(oDbDataReader["JobId"]);
					oPrimaryPackDummys.NextLevelCode = Convert.ToString(oDbDataReader["NextLevelCode"]);
					oPrimaryPackDummys.Code = Convert.ToString(oDbDataReader["Code"]);

					if(oDbDataReader["CreatedDate"] != DBNull.Value)
						oPrimaryPackDummys.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);
					lstPrimaryPackDummyss.Add(oPrimaryPackDummys);
				}
				oDbDataReader.Close();
				return lstPrimaryPackDummyss;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public PrimaryPackDummys GetPrimaryPackDummys(int Id)
		{
			try
			{
				PrimaryPackDummys oPrimaryPackDummys = new PrimaryPackDummys();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("SELECTPrimaryPackDummys",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Id",DbType.Int32,Id));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					oPrimaryPackDummys.Id = Convert.ToInt32(oDbDataReader["Id"]);
					oPrimaryPackDummys.JobId = Convert.ToInt32(oDbDataReader["JobId"]);
					oPrimaryPackDummys.NextLevelCode = Convert.ToString(oDbDataReader["NextLevelCode"]);
					oPrimaryPackDummys.Code = Convert.ToString(oDbDataReader["Code"]);

					if(oDbDataReader["CreatedDate"] != DBNull.Value)
						oPrimaryPackDummys.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);
				}
				oDbDataReader.Close();
				return oPrimaryPackDummys;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int AddPrimaryPackDummys(PrimaryPackDummys oPrimaryPackDummys)
		{
			try
			{
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_InsertOrUpdatePrimaryPackDummys", CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId",DbType.Int32,oPrimaryPackDummys.JobId));

                //if (!string.IsNullOrEmpty(oPrimaryPackDummys.NextLevelCode))
                //{
                //    oPrimaryPackDummys.NextLevelCode = AESCryptor.Encrypt(oPrimaryPackDummys.NextLevelCode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //}
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@NextLevelCode",DbType.String,oPrimaryPackDummys.NextLevelCode));

                //if (!string.IsNullOrEmpty(oPrimaryPackDummys.Code))
                //{
                //    oPrimaryPackDummys.Code = AESCryptor.Encrypt(oPrimaryPackDummys.Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //}

				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Code",DbType.String,oPrimaryPackDummys.Code));
				if (oPrimaryPackDummys.CreatedDate.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedDate",DbType.DateTime,oPrimaryPackDummys.CreatedDate));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedDate",DbType.DateTime,DBNull.Value));

				return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int UpdatePrimaryPackDummys(PrimaryPackDummys oPrimaryPackDummys)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("UPDATEPrimaryPackDummys",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId",DbType.Int32,oPrimaryPackDummys.JobId));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@NextLevelCode",DbType.String,oPrimaryPackDummys.NextLevelCode));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Code",DbType.String,oPrimaryPackDummys.Code));
				if (oPrimaryPackDummys.CreatedDate.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedDate",DbType.DateTime,oPrimaryPackDummys.CreatedDate));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedDate",DbType.DateTime,DBNull.Value));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Id",DbType.Int32,oPrimaryPackDummys.Id));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemovePrimaryPackDummys(PrimaryPackDummys oPrimaryPackDummys)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("DELETEPrimaryPackDummys",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Id",DbType.Int32,oPrimaryPackDummys.Id));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemovePrimaryPackDummys(int Id)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("DELETEPrimaryPackDummys",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Id",DbType.Int32,Id));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
