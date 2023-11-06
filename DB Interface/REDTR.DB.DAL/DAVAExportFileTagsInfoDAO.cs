using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public partial class DAVAExportFileTagsInfoDAO
	{
		public DAVAExportFileTagsInfoDAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<DAVAExportFileTagsInfo> GetDAVAExportFileTagsInfos(int Flag,string Value)
		{
			try
			{
				List<DAVAExportFileTagsInfo> lstDAVAExportFileTagsInfos = new List<DAVAExportFileTagsInfo>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[GET_DAVAExportFileTagsInfo ]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Value));

				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					DAVAExportFileTagsInfo oDAVAExportFileTagsInfo = new DAVAExportFileTagsInfo();
					oDAVAExportFileTagsInfo.ProductionInfo_Id = Convert.ToInt32(oDbDataReader["ProductionInfo_Id"]);

					if(oDbDataReader["FILENAME"] != DBNull.Value)
						oDAVAExportFileTagsInfo.FILENAME = Convert.ToString(oDbDataReader["FILENAME"]);
                    if (oDbDataReader["FileData"] != DBNull.Value)
                    oDAVAExportFileTagsInfo.FileData = Convert.ToString(oDbDataReader["FileData"]);   

					if(oDbDataReader["CreatedDate"] != DBNull.Value)
						oDAVAExportFileTagsInfo.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);

					if(oDbDataReader["TypeofUpload"] != DBNull.Value)
						oDAVAExportFileTagsInfo.TypeofUpload = Convert.ToString(oDbDataReader["TypeofUpload"]);
					lstDAVAExportFileTagsInfos.Add(oDAVAExportFileTagsInfo);
				}
				oDbDataReader.Close();
				return lstDAVAExportFileTagsInfos;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        //public DAVAExportFileTagsInfo GetDAVAExportFileTagsInfom(int Flag, string Value)
        //{
        //    try
        //    {
        //        DAVAExportFileTagsInfo oDAVAExportFileTagsInfo = new DAVAExportFileTagsInfo();
        //        DbCommand oDbCommand = DbProviderHelper.CreateCommand("[GET_DAVAExportFileTagsInfo ]", CommandType.StoredProcedure);
        //        oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
        //        oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Value));
        //        DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
        //        while (oDbDataReader.Read())
        //        {
        //            //oDAVAExportFileTagsInfo.ProductionInfo_Id = Convert.ToInt32(oDbDataReader["ProductionInfo_Id"]);

        //            //if(oDbDataReader["ENVELOPE_Id"] != DBNull.Value)
        //            //    oDAVAExportFileTagsInfo.ENVELOPE_Id = Convert.ToInt32(oDbDataReader["ENVELOPE_Id"]);

        //            if (oDbDataReader["FILENAME"] != DBNull.Value)
        //                oDAVAExportFileTagsInfo.FILENAME = Convert.ToString(oDbDataReader["FILENAME"]);

        //            if (oDbDataReader["CreatedDate"] != DBNull.Value)
        //                oDAVAExportFileTagsInfo.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);

        //            if (oDbDataReader["TypeofUpload"] != DBNull.Value)
        //                oDAVAExportFileTagsInfo.TypeofUpload = Convert.ToString(oDbDataReader["TypeofUpload"]);

        //            if (oDbDataReader["FileData"] != DBNull.Value)
        //                oDAVAExportFileTagsInfo.FileData = Convert.ToString(oDbDataReader["FileData"]);
        //        }
        //        oDbDataReader.Close();
        //        return oDAVAExportFileTagsInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

		public DAVAExportFileTagsInfo GetDAVAExportFileTagsInfo(int ProductionInfo_Id)
		{
			try
			{
				DAVAExportFileTagsInfo oDAVAExportFileTagsInfo = new DAVAExportFileTagsInfo();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[GET_DAVAExportFileTagsInfo ]", CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductionInfo_Id",DbType.Int32,ProductionInfo_Id));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					oDAVAExportFileTagsInfo.ProductionInfo_Id = Convert.ToInt32(oDbDataReader["ProductionInfo_Id"]);

                    //if(oDbDataReader["ENVELOPE_Id"] != DBNull.Value)
                    //    oDAVAExportFileTagsInfo.ENVELOPE_Id = Convert.ToInt32(oDbDataReader["ENVELOPE_Id"]);

					if(oDbDataReader["FILENAME"] != DBNull.Value)
						oDAVAExportFileTagsInfo.FILENAME = Convert.ToString(oDbDataReader["FILENAME"]);

					if(oDbDataReader["CreatedDate"] != DBNull.Value)
						oDAVAExportFileTagsInfo.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);

					if(oDbDataReader["TypeofUpload"] != DBNull.Value)
						oDAVAExportFileTagsInfo.TypeofUpload = Convert.ToString(oDbDataReader["TypeofUpload"]);

                    if (oDbDataReader["FileData"] != DBNull.Value)
                        oDAVAExportFileTagsInfo.FileData = Convert.ToString(oDbDataReader["FileData"]);   
				}
				oDbDataReader.Close();
				return oDAVAExportFileTagsInfo;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int AddDAVAExportFileTagsInfo(DAVAExportFileTagsInfo oDAVAExportFileTagsInfo)
		{
			try
			{
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_InsertOrUpdateDAVAExportFileTagsInfo", CommandType.StoredProcedure);
                //if (oDAVAExportFileTagsInfo.ENVELOPE_Id.HasValue)
                //    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ENVELOPE_Id",DbType.Int32,oDAVAExportFileTagsInfo.ENVELOPE_Id));
                //else
                //    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ENVELOPE_Id",DbType.Int32,DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Production_Id", DbType.Int64, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
                if (oDAVAExportFileTagsInfo.FILENAME!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FILENAME",DbType.String,oDAVAExportFileTagsInfo.FILENAME));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FILENAME",DbType.String,DBNull.Value));
				if (oDAVAExportFileTagsInfo.CreatedDate.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedDate",DbType.DateTime,oDAVAExportFileTagsInfo.CreatedDate));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedDate",DbType.DateTime,DBNull.Value));
				if (oDAVAExportFileTagsInfo.TypeofUpload!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TypeofUpload",DbType.String,oDAVAExportFileTagsInfo.TypeofUpload));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TypeofUpload",DbType.String,DBNull.Value));

                if (oDAVAExportFileTagsInfo.FileData != null)

                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FileData", DbType.String, oDAVAExportFileTagsInfo.FileData));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FileData", DbType.String, DBNull.Value));

                //int Production_Id = Convert.ToInt32(DbProviderHelper.ExecuteNonQuery(oDbCommand));
                //return Production_Id;

                DbProviderHelper.ExecuteNonQuery(oDbCommand);
                return Convert.ToInt32(oDbCommand.Parameters["@Production_Id"].Value);


			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int UpdateDAVAExportFileTagsInfo(DAVAExportFileTagsInfo oDAVAExportFileTagsInfo)
		{

			try
			{
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_InsertOrUpdateDAVAExportFileTagsInfo", CommandType.StoredProcedure);
                //if (oDAVAExportFileTagsInfo.ENVELOPE_Id.HasValue)
                //    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ENVELOPE_Id",DbType.Int32,oDAVAExportFileTagsInfo.ENVELOPE_Id));
                //else
                //    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ENVELOPE_Id",DbType.Int32,DBNull.Value));
				if (oDAVAExportFileTagsInfo.FILENAME!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FILENAME",DbType.String,oDAVAExportFileTagsInfo.FILENAME));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FILENAME",DbType.String,DBNull.Value));
				if (oDAVAExportFileTagsInfo.CreatedDate.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedDate",DbType.DateTime,oDAVAExportFileTagsInfo.CreatedDate));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedDate",DbType.DateTime,DBNull.Value));
                if (oDAVAExportFileTagsInfo.FileData != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FileData", DbType.String, oDAVAExportFileTagsInfo.FileData));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FileData", DbType.String, DBNull.Value));
				if (oDAVAExportFileTagsInfo.TypeofUpload!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TypeofUpload",DbType.String,oDAVAExportFileTagsInfo.TypeofUpload));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TypeofUpload",DbType.String,DBNull.Value));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductionInfo_Id",DbType.Int32,oDAVAExportFileTagsInfo.ProductionInfo_Id));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemoveDAVAExportFileTagsInfo(DAVAExportFileTagsInfo oDAVAExportFileTagsInfo)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("DELETEDAVAExportFileTagsInfo",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductionInfo_Id",DbType.Int32,oDAVAExportFileTagsInfo.ProductionInfo_Id));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemoveDAVAExportFileTagsInfo(int ProductionInfo_Id)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("DELETEDAVAExportFileTagsInfo",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductionInfo_Id",DbType.Int32,ProductionInfo_Id));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
