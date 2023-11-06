using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public partial class DAVAExportDetailsDAO
	{
		public DAVAExportDetailsDAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<DAVAExportDetails> GetDAVAExportDetailss()
		{
			try
			{
				List<DAVAExportDetails> lstDAVAExportDetailss = new List<DAVAExportDetails>();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("SELECTDAVAExportDetailss",CommandType.StoredProcedure);
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					DAVAExportDetails oDAVAExportDetails = new DAVAExportDetails();
					oDAVAExportDetails.Id = Convert.ToInt32(oDbDataReader["Id"]);
					oDAVAExportDetails.JobId = Convert.ToInt32(oDbDataReader["JobId"]);

					if(oDbDataReader["BatchName"] != DBNull.Value)
						oDAVAExportDetails.BatchName = Convert.ToString(oDbDataReader["BatchName"]);

					if(oDbDataReader["ProductCode"] != DBNull.Value)
						oDAVAExportDetails.ProductCode = Convert.ToString(oDbDataReader["ProductCode"]);

					if(oDbDataReader["BatchQuantity"] != DBNull.Value)
						oDAVAExportDetails.BatchQuantity = Convert.ToInt32(oDbDataReader["BatchQuantity"]);

					if(oDbDataReader["ExemptedFromBarcoding"] != DBNull.Value)
						oDAVAExportDetails.ExemptedFromBarcoding = Convert.ToBoolean(oDbDataReader["ExemptedFromBarcoding"]);

					if(oDbDataReader["ExemptionDate"] != DBNull.Value)
						oDAVAExportDetails.ExemptionDate = Convert.ToDateTime(oDbDataReader["ExemptionDate"]);

					if(oDbDataReader["ExemptedCountryCode"] != DBNull.Value)
						oDAVAExportDetails.ExemptedCountryCode = Convert.ToString(oDbDataReader["ExemptedCountryCode"]);

					if(oDbDataReader["BatchStatus"] != DBNull.Value)
						oDAVAExportDetails.BatchStatus = Convert.ToString(oDbDataReader["BatchStatus"]);

					if(oDbDataReader["LastUpdatedDate"] != DBNull.Value)
						oDAVAExportDetails.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);

					if(oDbDataReader["PrimaryPackPCMap"] != DBNull.Value)
						oDAVAExportDetails.PrimaryPackPCMap = Convert.ToInt32(oDbDataReader["PrimaryPackPCMap"]);

                    if (oDbDataReader["ProductionInfo_Id"] != DBNull.Value)
                        oDAVAExportDetails.ProductionInfo_Id = Convert.ToInt32(oDbDataReader["ProductionInfo_Id"]);
					lstDAVAExportDetailss.Add(oDAVAExportDetails);
				}
				oDbDataReader.Close();
				return lstDAVAExportDetailss;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public DAVAExportDetails GetDAVAExportDetails(int Id)
		{
			try
			{
				DAVAExportDetails oDAVAExportDetails = new DAVAExportDetails();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("SELECTDAVAExportDetails",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Id",DbType.Int32,Id));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					oDAVAExportDetails.Id = Convert.ToInt32(oDbDataReader["Id"]);
					oDAVAExportDetails.JobId = Convert.ToInt32(oDbDataReader["JobId"]);

					if(oDbDataReader["BatchName"] != DBNull.Value)
						oDAVAExportDetails.BatchName = Convert.ToString(oDbDataReader["BatchName"]);

					if(oDbDataReader["ProductCode"] != DBNull.Value)
						oDAVAExportDetails.ProductCode = Convert.ToString(oDbDataReader["ProductCode"]);

					if(oDbDataReader["BatchQuantity"] != DBNull.Value)
						oDAVAExportDetails.BatchQuantity = Convert.ToInt32(oDbDataReader["BatchQuantity"]);

					if(oDbDataReader["ExemptedFromBarcoding"] != DBNull.Value)
						oDAVAExportDetails.ExemptedFromBarcoding = Convert.ToBoolean(oDbDataReader["ExemptedFromBarcoding"]);

					if(oDbDataReader["ExemptionDate"] != DBNull.Value)
						oDAVAExportDetails.ExemptionDate = Convert.ToDateTime(oDbDataReader["ExemptionDate"]);

					if(oDbDataReader["ExemptedCountryCode"] != DBNull.Value)
						oDAVAExportDetails.ExemptedCountryCode = Convert.ToString(oDbDataReader["ExemptedCountryCode"]);

					if(oDbDataReader["BatchStatus"] != DBNull.Value)
						oDAVAExportDetails.BatchStatus = Convert.ToString(oDbDataReader["BatchStatus"]);

					if(oDbDataReader["LastUpdatedDate"] != DBNull.Value)
						oDAVAExportDetails.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);

					if(oDbDataReader["PrimaryPackPCMap"] != DBNull.Value)
						oDAVAExportDetails.PrimaryPackPCMap = Convert.ToInt32(oDbDataReader["PrimaryPackPCMap"]);
                   if (oDbDataReader["ProductionInfo_Id"] != DBNull.Value)
                        oDAVAExportDetails.ProductionInfo_Id = Convert.ToInt32(oDbDataReader["ProductionInfo_Id"]);
				}
				oDbDataReader.Close();
				return oDAVAExportDetails;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int AddDAVAExportDetails(DAVAExportDetails oDAVAExportDetails)
		{
			try
			{
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_InsertOrUpdateDAVAExportDetails", CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId",DbType.Int32,oDAVAExportDetails.JobId));
				if (oDAVAExportDetails.BatchName!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchName",DbType.String,oDAVAExportDetails.BatchName));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchName",DbType.String,DBNull.Value));
				if (oDAVAExportDetails.ProductCode!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductCode",DbType.String,oDAVAExportDetails.ProductCode));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductCode",DbType.String,DBNull.Value));
				if (oDAVAExportDetails.BatchQuantity.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchQuantity",DbType.Int32,oDAVAExportDetails.BatchQuantity));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchQuantity",DbType.Int32,DBNull.Value));
				if (oDAVAExportDetails.ExemptedFromBarcoding.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExemptedFromBarcoding",DbType.Boolean,oDAVAExportDetails.ExemptedFromBarcoding));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExemptedFromBarcoding",DbType.Boolean,DBNull.Value));
				if (oDAVAExportDetails.ExemptionDate.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExemptionDate",DbType.DateTime,oDAVAExportDetails.ExemptionDate));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExemptionDate",DbType.DateTime,DBNull.Value));
				if (oDAVAExportDetails.ExemptedCountryCode!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExemptedCountryCode",DbType.String,oDAVAExportDetails.ExemptedCountryCode));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExemptedCountryCode",DbType.String,DBNull.Value));
				if (oDAVAExportDetails.BatchStatus!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchStatus",DbType.String,oDAVAExportDetails.BatchStatus));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchStatus",DbType.String,DBNull.Value));
				if (oDAVAExportDetails.LastUpdatedDate.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate",DbType.DateTime,oDAVAExportDetails.LastUpdatedDate));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate",DbType.DateTime,DBNull.Value));
				if (oDAVAExportDetails.PrimaryPackPCMap.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PrimaryPackPCMap",DbType.Int32,oDAVAExportDetails.PrimaryPackPCMap));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PrimaryPackPCMap",DbType.Int32,DBNull.Value));
                if (oDAVAExportDetails.ProductionInfo_Id.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductionInfo_Id", DbType.Int32, oDAVAExportDetails.ProductionInfo_Id));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductionInfo_Id", DbType.Int32, DBNull.Value));

				return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int UpdateDAVAExportDetails(DAVAExportDetails oDAVAExportDetails)
		{

			try
			{
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_InsertOrUpdateDAVAExportDetails", CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId",DbType.Int32,oDAVAExportDetails.JobId));
				if (oDAVAExportDetails.BatchName!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchName",DbType.String,oDAVAExportDetails.BatchName));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchName",DbType.String,DBNull.Value));
				if (oDAVAExportDetails.ProductCode!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductCode",DbType.String,oDAVAExportDetails.ProductCode));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductCode",DbType.String,DBNull.Value));
				if (oDAVAExportDetails.BatchQuantity.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchQuantity",DbType.Int32,oDAVAExportDetails.BatchQuantity));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchQuantity",DbType.Int32,DBNull.Value));
				if (oDAVAExportDetails.ExemptedFromBarcoding.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExemptedFromBarcoding",DbType.Boolean,oDAVAExportDetails.ExemptedFromBarcoding));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExemptedFromBarcoding",DbType.Boolean,DBNull.Value));
				if (oDAVAExportDetails.ExemptionDate.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExemptionDate",DbType.DateTime,oDAVAExportDetails.ExemptionDate));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExemptionDate",DbType.DateTime,DBNull.Value));
				if (oDAVAExportDetails.ExemptedCountryCode!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExemptedCountryCode",DbType.String,oDAVAExportDetails.ExemptedCountryCode));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExemptedCountryCode",DbType.String,DBNull.Value));
				if (oDAVAExportDetails.BatchStatus!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchStatus",DbType.String,oDAVAExportDetails.BatchStatus));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchStatus",DbType.String,DBNull.Value));
				if (oDAVAExportDetails.LastUpdatedDate.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate",DbType.DateTime,oDAVAExportDetails.LastUpdatedDate));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate",DbType.DateTime,DBNull.Value));
				if (oDAVAExportDetails.PrimaryPackPCMap.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PrimaryPackPCMap",DbType.Int32,oDAVAExportDetails.PrimaryPackPCMap));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PrimaryPackPCMap",DbType.Int32,DBNull.Value));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Id",DbType.Int32,oDAVAExportDetails.Id));

                if (oDAVAExportDetails.ProductionInfo_Id.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductionInfo_Id", DbType.Int32, oDAVAExportDetails.ProductionInfo_Id));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductionInfo_Id", DbType.Int32, DBNull.Value));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemoveDAVAExportDetails(DAVAExportDetails oDAVAExportDetails)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("DELETEDAVAExportDetails",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Id",DbType.Int32,oDAVAExportDetails.Id));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemoveDAVAExportDetails(int Id)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("DELETEDAVAExportDetails",CommandType.StoredProcedure);
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
