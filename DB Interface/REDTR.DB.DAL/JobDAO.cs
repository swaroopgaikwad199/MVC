using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public partial class JobDAO
	{
		public JobDAO()
		{
			DbProviderHelper.GetConnection();
		}
        public List<Job> GetJobs(int Flag,int AppCode, string Value, string Value1)
		{
            try
            {
                List<Job> lstJobs = new List<Job>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETJob]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AppID", DbType.Int16, AppCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, Value1));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    Job oJob = new Job();
                    oJob.JID = Convert.ToDecimal(oDbDataReader["JID"]);
                    oJob.JobName = Convert.ToString(oDbDataReader["JobName"]);
                    oJob.PAID = Convert.ToDecimal(oDbDataReader["PAID"]);
                    oJob.BatchNo = Convert.ToString(oDbDataReader["BatchNo"]);

                    if (oDbDataReader["MfgDate"] != DBNull.Value)
                        oJob.MfgDate = Convert.ToDateTime(oDbDataReader["MfgDate"]);

                    if (oDbDataReader["ExpDate"] != DBNull.Value)
                        oJob.ExpDate = Convert.ToDateTime(oDbDataReader["ExpDate"]);
                    oJob.Quantity = Convert.ToInt32(oDbDataReader["Quantity"]);
                    oJob.SurPlusQty = Convert.ToInt32(oDbDataReader["SurPlusQty"]);
                    oJob.JobStatus = Convert.ToSByte(oDbDataReader["JobStatus"]);

                    if (oDbDataReader["DetailInfo"] != DBNull.Value)
                        oJob.DetailInfo = Convert.ToString(oDbDataReader["DetailInfo"]);
                    oJob.JobStartTime = Convert.ToDateTime(oDbDataReader["JobStartTime"]);

                    if (oDbDataReader["JobEndTime"] != DBNull.Value)
                        oJob.JobEndTime = Convert.ToDateTime(oDbDataReader["JobEndTime"]);

                    if (oDbDataReader["LabelStartIndex"] != DBNull.Value)
                        oJob.LabelStartIndex = Convert.ToDecimal(oDbDataReader["LabelStartIndex"]);

                    if (oDbDataReader["AutomaticBatchCloser"] != DBNull.Value)
                        oJob.AutomaticBatchCloser = Convert.ToBoolean(oDbDataReader["AutomaticBatchCloser"]);
                    if (oDbDataReader["DAVAPortalUpload"] != DBNull.Value)
                        oJob.DAVAPortalUpload = Convert.ToBoolean(oDbDataReader["DAVAPortalUpload"]);
                    if (oDbDataReader["TID"] != DBNull.Value)
                        oJob.TID = Convert.ToDecimal(oDbDataReader["TID"]);

                    if (oDbDataReader["MLNO"] != DBNull.Value)
                        oJob.MLNO = Convert.ToString(oDbDataReader["MLNO"]);

                    if (oDbDataReader["TenderText"] != DBNull.Value)
                        oJob.TenderText = Convert.ToString(oDbDataReader["TenderText"]);

                    if (oDbDataReader["JobWithUID"] != DBNull.Value)
                        oJob.JobWithUID = Convert.ToBoolean(oDbDataReader["JobWithUID"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oJob.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["CreatedBy"] != DBNull.Value)
                        oJob.CreatedBy = Convert.ToDecimal(oDbDataReader["CreatedBy"]);

                    if (oDbDataReader["VerifiedBy"] != DBNull.Value)
                        oJob.VerifiedBy = Convert.ToDecimal(oDbDataReader["VerifiedBy"]);

                    if (oDbDataReader["VerifiedDate"] != DBNull.Value)
                        oJob.VerifiedDate = Convert.ToDateTime(oDbDataReader["VerifiedDate"]);

                    if (oDbDataReader["PrimaryPCMapCount"] != DBNull.Value)
                        oJob.PrimaryPCMapCount = Convert.ToInt32(oDbDataReader["PrimaryPCMapCount"]);

                    if (oDbDataReader["ForExport"] != DBNull.Value)
                        oJob.ForExport = Convert.ToBoolean(oDbDataReader["ForExport"]);
                   
                    oJob.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);
                    oJob.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);

                    if (oDbDataReader["LineCode"] != DBNull.Value)
                        oJob.LineCode = Convert.ToString(oDbDataReader["LineCode"]);

                    if (oDbDataReader["NoReadCount"] != DBNull.Value)
                        oJob.NoReadCount = Convert.ToDecimal(oDbDataReader["NoReadCount"]);

                    if (oDbDataReader["UseExpDay"] != DBNull.Value)
                        oJob.UseExpDay = Convert.ToBoolean(oDbDataReader["UseExpDay"]);

                    if (oDbDataReader["ExpDateFormat"] != DBNull.Value)
                        oJob.ExpDateFormat = Convert.ToString(oDbDataReader["ExpDateFormat"]);

                    if (oDbDataReader["PackagingLvlId"] != DBNull.Value)
                        oJob.PackagingLvlId = Convert.ToInt32(oDbDataReader["PackagingLvlId"]);

                    if (oDbDataReader["CustomerId"] != DBNull.Value)
                        oJob.CustomerId = Convert.ToInt32(oDbDataReader["CustomerId"]);

                    if (oDbDataReader["ProviderId"] != DBNull.Value)
                        oJob.ProviderId= Convert.ToInt32(oDbDataReader["ProviderId"]);

                    if (oDbDataReader["PPNCountryCode"] != DBNull.Value)
                        oJob.PPNCountryCode =Convert.ToString(oDbDataReader["PPNCountryCode"]);

                    if (oDbDataReader["PPNPostalCode"] != DBNull.Value)
                        oJob.PPNPostalCode = Convert.ToString(oDbDataReader["PPNPostalCode"]);

                    if (oDbDataReader["CompType"] != DBNull.Value)
                        oJob.CompType = Convert.ToString(oDbDataReader["CompType"]);
                    oJob.AppId = Convert.ToInt16(oDbDataReader["AppId"]);
                    lstJobs.Add(oJob);
                }
                oDbDataReader.Close();
                return lstJobs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}
        public Job GetJob(int Flag,int AppCode, string Value, string Value1)
		{
            try
            {
                Job oJob = new Job();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GETJob", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AppID", DbType.Int16, AppCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, Value1));

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    oJob.JID = Convert.ToDecimal(oDbDataReader["JID"]);
                    oJob.JobName = Convert.ToString(oDbDataReader["JobName"]);
                    oJob.PAID = Convert.ToDecimal(oDbDataReader["PAID"]);
                    oJob.BatchNo = Convert.ToString(oDbDataReader["BatchNo"]);
                    oJob.MfgDate = Convert.ToDateTime(oDbDataReader["MfgDate"]);
                    oJob.ExpDate = Convert.ToDateTime(oDbDataReader["ExpDate"]);
                    oJob.Quantity = Convert.ToInt32(oDbDataReader["Quantity"]);
                    oJob.SurPlusQty = Convert.ToInt32(oDbDataReader["SurPlusQty"]);
                    oJob.JobStatus = Convert.ToSByte(oDbDataReader["JobStatus"]);
                    if (oDbDataReader["PrimaryPCMapCount"] != DBNull.Value)
                        oJob.PrimaryPCMapCount = Convert.ToInt32(oDbDataReader["PrimaryPCMapCount"]);
                    if (oDbDataReader["DetailInfo"] != DBNull.Value)
                        oJob.DetailInfo = Convert.ToString(oDbDataReader["DetailInfo"]);
                    oJob.JobStartTime = Convert.ToDateTime(oDbDataReader["JobStartTime"]);
                    if (oDbDataReader["DAVAPortalUpload"] != DBNull.Value)
                        oJob.DAVAPortalUpload = Convert.ToBoolean(oDbDataReader["DAVAPortalUpload"]);
                    if (oDbDataReader["JobEndTime"] != DBNull.Value)
                        oJob.JobEndTime = Convert.ToDateTime(oDbDataReader["JobEndTime"]);
                    //oJob.OperatorID = Convert.ToDecimal(oDbDataReader["OperatorID"]);

                    if (oDbDataReader["LabelStartIndex"] != DBNull.Value)
                        oJob.LabelStartIndex = Convert.ToDecimal(oDbDataReader["LabelStartIndex"]);

                    if (oDbDataReader["AutomaticBatchCloser"] != DBNull.Value)
                        oJob.AutomaticBatchCloser = Convert.ToBoolean(oDbDataReader["AutomaticBatchCloser"]);
                    if (oDbDataReader["ForExport"] != DBNull.Value)
                        oJob.ForExport = Convert.ToBoolean(oDbDataReader["ForExport"]);
                    if (oDbDataReader["TID"] != DBNull.Value)
                        oJob.TID = Convert.ToDecimal(oDbDataReader["TID"]);

                    if (oDbDataReader["MLNO"] != DBNull.Value)
                        oJob.MLNO = Convert.ToString(oDbDataReader["MLNO"]);

                    if (oDbDataReader["TenderText"] != DBNull.Value)
                        oJob.TenderText = Convert.ToString(oDbDataReader["TenderText"]);

                    if (oDbDataReader["JobWithUID"] != DBNull.Value)
                        oJob.JobWithUID = Convert.ToBoolean(oDbDataReader["JobWithUID"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oJob.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["CreatedBy"] != DBNull.Value)
                        oJob.CreatedBy = Convert.ToDecimal(oDbDataReader["CreatedBy"]);

                    if (oDbDataReader["VerifiedBy"] != DBNull.Value)
                        oJob.VerifiedBy = Convert.ToDecimal(oDbDataReader["VerifiedBy"]);

                    if (oDbDataReader["VerifiedDate"] != DBNull.Value)
                        oJob.VerifiedDate = Convert.ToDateTime(oDbDataReader["VerifiedDate"]);

                    oJob.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);
                    oJob.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);

                    oJob.AppId = Convert.ToInt16(oDbDataReader["AppId"]);

                    if (oDbDataReader["LineCode"] != DBNull.Value)
                        oJob.LineCode = Convert.ToString(oDbDataReader["LineCode"]);

                    if (oDbDataReader["NoReadCount"] != DBNull.Value)
                        oJob.NoReadCount = Convert.ToDecimal(oDbDataReader["NoReadCount"]);

                    if (oDbDataReader["UseExpDay"] != DBNull.Value)
                        oJob.UseExpDay = Convert.ToBoolean(oDbDataReader["UseExpDay"]);

                    if (oDbDataReader["ExpDateFormat"] != DBNull.Value)
                        oJob.ExpDateFormat = Convert.ToString(oDbDataReader["ExpDateFormat"]);

                    if (oDbDataReader["PackagingLvlId"] != DBNull.Value)
                        oJob.PackagingLvlId = Convert.ToInt32(oDbDataReader["PackagingLvlId"]);

                    if (oDbDataReader["CustomerId"] != DBNull.Value)
                        oJob.CustomerId = Convert.ToInt32(oDbDataReader["CustomerId"]);

                    if (oDbDataReader["ProviderId"] != DBNull.Value)
                        oJob.ProviderId = Convert.ToInt32(oDbDataReader["ProviderId"]);

                    if (oDbDataReader["PPNCountryCode"] != DBNull.Value)
                        oJob.PPNCountryCode = Convert.ToString(oDbDataReader["PPNCountryCode"]);

                    if (oDbDataReader["PPNPostalCode"] != DBNull.Value)
                        oJob.PPNPostalCode = Convert.ToString(oDbDataReader["PPNPostalCode"]);

                    if (oDbDataReader["CompType"] != DBNull.Value)
                        oJob.CompType = Convert.ToString(oDbDataReader["CompType"]);


                }
                oDbDataReader.Close();
                return oJob;
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}
        public int InsertOrUpdateJob(int Flag, Job oJob)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateJob]", CommandType.StoredProcedure);

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UseExpDay", DbType.Boolean, oJob.UseExpDay));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExpDateFormat", DbType.String, oJob.ExpDateFormat));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JID", DbType.Decimal, oJob.JID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobName", DbType.String, oJob.JobName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Decimal, oJob.PAID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchNo", DbType.String, oJob.BatchNo));
                
                if (oJob.MfgDate != System.DateTime.MinValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MfgDate", DbType.DateTime, oJob.MfgDate.Date));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MfgDate", DbType.DateTime, DBNull.Value));

                if (oJob.ExpDate != System.DateTime.MinValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExpDate", DbType.DateTime, oJob.ExpDate.Date));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExpDate", DbType.DateTime, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Quantity", DbType.Int32, oJob.Quantity));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PrimaryPCMapCount", DbType.Int32, oJob.PrimaryPCMapCount));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SurPlusQty", DbType.Int32, oJob.SurPlusQty));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobStatus", DbType.Byte, oJob.JobStatus));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DetailInfo", DbType.String, oJob.DetailInfo));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DAVAPortalUpload", DbType.Boolean, oJob.DAVAPortalUpload));
                if (oJob.JobStartTime != System.DateTime.MinValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobStartTime", DbType.DateTime, oJob.JobStartTime));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobStartTime", DbType.DateTime, DBNull.Value));

                if (oJob.JobEndTime.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobEndTime", DbType.DateTime, oJob.JobEndTime));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobEndTime", DbType.DateTime, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LabelStartIndex", DbType.Decimal, oJob.LabelStartIndex));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AutomaticBatchCloser", DbType.Boolean, oJob.AutomaticBatchCloser));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ForExport", DbType.Boolean, oJob.ForExport));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TID", DbType.Decimal, oJob.TID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MLNO", DbType.String, oJob.MLNO));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TenderText", DbType.String, oJob.TenderText));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobWithUID", DbType.Boolean, oJob.JobWithUID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oJob.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedBy", DbType.Decimal, oJob.CreatedBy));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@VerifiedBy", DbType.Decimal, oJob.VerifiedBy));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate", DbType.DateTime, DateTime.Now));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AppID", DbType.Int16, oJob.AppId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, oJob.LineCode)); 
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@NoReadCount", DbType.Decimal, oJob.NoReadCount)); 
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackagingLevelId", DbType.Int32, oJob.PackagingLvlId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int64, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CustomerId", DbType.Int32, oJob.CustomerId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProviderId", DbType.Int32, oJob.ProviderId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PPNCountryCode", DbType.String, oJob.PPNCountryCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PPNPostalCode", DbType.String, oJob.PPNPostalCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CompType", DbType.String, oJob.CompType));
                DbProviderHelper.ExecuteNonQuery(oDbCommand);

                if (oDbCommand.Parameters["@ID"].Value != DBNull.Value)
                return Convert.ToInt32(oDbCommand.Parameters["@ID"].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }	
		public int RemoveJob(int Flag,Nullable<Decimal> JID)
		{
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_DeleteJob]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Jid", DbType.Decimal, JID));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}

        public int UpdateNoRead(int JID,Int32 Count)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("update Job Set NoReadCount = " + Count + " Where JID=" + JID, CommandType.Text);
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

 // For contoller count by tushar

        public int InsertOrUpdateJobCount(int Flag, int jobid, int BCnt, int LCnt, int WCnt, int SCnt, int MTCnt, int MVCnt, int MLPCnt, int MCBCnt)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateJobCount]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Int32,jobid));
                
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BADCOUNT", DbType.Int32, BCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BACKtoBACK", DbType.Int32, LCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@OUTofWIN", DbType.Int32, WCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SAMPLECOUNT", DbType.Int32, SCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MCTECH", DbType.Int32, MTCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MCVERIF", DbType.Int32, MVCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MCLOWP", DbType.Int32, MLPCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MCCBAD", DbType.Int32, MCBCnt));
                DbProviderHelper.ExecuteNonQuery(oDbCommand);
               
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<int> GetJobCount(int Flag, int jobid, int BCnt, int LCnt, int WCnt, int SCnt, int MTCnt, int MVCnt, int MLPCnt, int MCBCnt)
        {
            try
            {
                List<int> Cnt = new List<int>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateJobCount]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Int32, jobid));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BADCOUNT", DbType.Int32, BCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BACKtoBACK", DbType.Int32, LCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@OUTofWIN", DbType.Int32, WCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SAMPLECOUNT", DbType.Int32, SCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MCTECH", DbType.Int32, MTCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MCVERIF", DbType.Int32, MVCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MCLOWP", DbType.Int32, MLPCnt));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MCCBAD", DbType.Int32, MCBCnt));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    Cnt.Add(Convert.ToInt32(oDbDataReader["BADCOUNT"]));
                    Cnt.Add(Convert.ToInt32(oDbDataReader["BACKtoBACK"]));
                    Cnt.Add(Convert.ToInt32(oDbDataReader["OUTofWIN"]));
                    Cnt.Add(Convert.ToInt32(oDbDataReader["SAMPLECOUNT"]));
                    Cnt.Add(Convert.ToInt32(oDbDataReader["MCTECH"]));
                    Cnt.Add(Convert.ToInt32(oDbDataReader["MCVERIF"]));
                    Cnt.Add(Convert.ToInt32(oDbDataReader["MCLOWP"]));
                    Cnt.Add(Convert.ToInt32(oDbDataReader["MCCBAD"]));
                }
                oDbDataReader.Close();
                return Cnt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	    
	}
}
