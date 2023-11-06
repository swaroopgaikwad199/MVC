using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;


namespace REDTR.DB.DAL
{
    public class JobDetailsDAO
    {
        public JobDetailsDAO()
        {
            DbProviderHelper.GetConnection();
        }
        public List<JobDetails> GetJobDetailss(int Flag, string  param, int AppID)
        {
            try
            {
                List<JobDetails> lstJobDetailss = new List<JobDetails>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETJobDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, param));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_Deckcode", DbType.String, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@APPID", DbType.Int16, AppID));
                oDbCommand.CommandTimeout = 0;
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    JobDetails oJobDetails = new JobDetails();
                    oJobDetails.JD_JobID = Convert.ToDecimal(oDbDataReader["JD_JobID"]);
                    oJobDetails.JD_ProdName = Convert.ToString(oDbDataReader["JD_ProdName"]);
                    oJobDetails.JD_ProdCode = Convert.ToString(oDbDataReader["JD_ProdCode"]);
                    oJobDetails.JD_FGCode = Convert.ToString(oDbDataReader["JD_FGCode"]);
                    oJobDetails.JD_Deckcode = Convert.ToString(oDbDataReader["JD_Deckcode"]);
                    oJobDetails.JD_PPN = Convert.ToString(oDbDataReader["JD_PPN"]);
                    oJobDetails.JD_GTIN = Convert.ToString(oDbDataReader["JD_GTIN"]);
                    if (oDbDataReader["JD_NTIN"] != null)
                        oJobDetails.JD_NTIN = Convert.ToString(oDbDataReader["JD_NTIN"]);
                    if (oDbDataReader["GTINCTI"] != null)
                        oJobDetails.JD_GTINCTI = Convert.ToString(oDbDataReader["GTINCTI"]);

                    oJobDetails.JD_DeckSize = Convert.ToInt32(oDbDataReader["JD_DeckSize"]);
                    if (oDbDataReader["BundleQty"] != null)
                        oJobDetails.BundleQty = Convert.ToInt32(oDbDataReader["BundleQty"]);
                    oJobDetails.JD_MRP = Convert.ToDecimal(oDbDataReader["JD_MRP"]);
                    oJobDetails.JD_Description = Convert.ToString(oDbDataReader["JD_Description"]);
                    oJobDetails.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);
                    if (oDbDataReader["LabelName"] != null)
                        oJobDetails.LabelName= Convert.ToString(oDbDataReader["LabelName"]);
                    if (oDbDataReader["Filter"] != null)
                        oJobDetails.Filter = Convert.ToString(oDbDataReader["Filter"]);
                    lstJobDetailss.Add(oJobDetails);
                }
                oDbDataReader.Close();
                return lstJobDetailss;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JobDetails GetJobDetails(int Flag, string  param, string deckCode)
        {
            try
            {
                JobDetails oJobDetails = new JobDetails();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETJobDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String , param));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_Deckcode", DbType.String, deckCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@APPID", DbType.Int16, DBNull.Value));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    oJobDetails.JD_JobID = Convert.ToDecimal(oDbDataReader["JD_JobID"]);
                    oJobDetails.JD_ProdCode = Convert.ToString(oDbDataReader["JD_ProdCode"]);
                    oJobDetails.JD_FGCode = Convert.ToString(oDbDataReader["JD_FGCode"]);
                    oJobDetails.JD_ProdName = Convert.ToString(oDbDataReader["JD_ProdName"]);
                    oJobDetails.JD_Deckcode = Convert.ToString(oDbDataReader["JD_Deckcode"]);
                    oJobDetails.JD_PPN = Convert.ToString(oDbDataReader["JD_PPN"]);
                    oJobDetails.JD_GTIN = Convert.ToString(oDbDataReader["JD_GTIN"]);
                    if (oDbDataReader["GTINCTI"] != null)
                        oJobDetails.JD_GTINCTI = Convert.ToString(oDbDataReader["GTINCTI"]);
                    oJobDetails.JD_DeckSize = Convert.ToInt32(oDbDataReader["JD_DeckSize"]);
                    if (oDbDataReader["BundleQty"] != null)
                        oJobDetails.BundleQty = Convert.ToInt32(oDbDataReader["BundleQty"]);
                    oJobDetails.JD_MRP = Convert.ToDecimal(oDbDataReader["JD_MRP"]);
                    oJobDetails.JD_Description = Convert.ToString(oDbDataReader["JD_Description"]);
                    oJobDetails.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);                    
                }
                oDbDataReader.Close();
                return oJobDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddJobDetails(int flag, JobDetails oJobDetails)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_InsertOrUpdateJobDetails", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_JobID", DbType.Decimal, oJobDetails.JD_JobID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_ProdName", DbType.String, oJobDetails.JD_ProdName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_ProdCode", DbType.String, oJobDetails.JD_ProdCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_FGCode", DbType.String, oJobDetails.JD_FGCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_Deckcode", DbType.String, oJobDetails.JD_Deckcode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_PPN", DbType.String, oJobDetails.JD_PPN));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_GTIN", DbType.String, oJobDetails.JD_GTIN));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_NTIN", DbType.String, oJobDetails.JD_NTIN));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@GTINCTI", DbType.String, oJobDetails.JD_GTINCTI));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BundleQty", DbType.String, oJobDetails.BundleQty));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_DeckSize", DbType.Int32, oJobDetails.JD_DeckSize));
                if (oJobDetails.JD_MRP.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_MRP", DbType.Decimal, oJobDetails.JD_MRP));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_MRP", DbType.Decimal, DBNull.Value));

                if (oJobDetails.JD_Description != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_Description", DbType.String, oJobDetails.JD_Description));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_Description", DbType.String, DBNull.Value));

                if (oJobDetails.LabelName!="")
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LabelName", DbType.String, oJobDetails.LabelName));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LabelName", DbType.String, DBNull.Value));

                if (oJobDetails.Filter!="")
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Filter", DbType.String, oJobDetails.Filter));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Filter", DbType.String, DBNull.Value));

                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveJobDetails(int Flag, decimal jobID)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_DeleteJobDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_JobID", DbType.Decimal, jobID));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
  

        private static string GetConnectionString()
        {
            REDTR.UTILS.DbConnectionConfig.LoadConectionOracle();

            //string Server1 = "XE";
            //string UserId1 = "test";
            //string Pass1 = "test";

            string Server1 = REDTR.UTILS.DbConnectionConfig.mOraDbConfig.DataSourcePath.ToString().Trim();
            string UserId1 = REDTR.UTILS.DbConnectionConfig.mOraDbConfig.UserName.ToString().Trim();
            string Pass1 = REDTR.UTILS.DbConnectionConfig.mOraDbConfig.Password.ToString().Trim(); 

            //string connString = "Data Source=" + REDTR.UTILS.DbConnectionConfig.mOraDbConfig.DataSourcePath.ToString() + ";User id=" + REDTR.UTILS.DbConnectionConfig.mOraDbConfig.UserName.ToString() + ";Password=" + REDTR.UTILS.DbConnectionConfig.mOraDbConfig.Password.ToString() + ";Persist Security Info=False";

            string connString = "Data Source=" + Server1 + ";User id=" + UserId1 + ";Password=" + Pass1 + ";Persist Security Info=False";
                        
            //String connString = "host= serverName;database=myDatabase;uid=userName;pwd=passWord";
            return connString;
        }

    }
}
