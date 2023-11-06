using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
    public class JobAssoDeckDAO
    {
        public JobAssoDeckDAO()
        {
            DbProviderHelper.GetConnection();
        }
        public List<JobAssoDeck> GetJobAssoDecks(int Flag,string Value)
        {
            try
            {
                List<JobAssoDeck> lstJobAssoDecks = new List<JobAssoDeck>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETJobAssoDeckdetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Value));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    JobAssoDeck oJobAssoDeck = new JobAssoDeck();
                    oJobAssoDeck.ID = Convert.ToDecimal(oDbDataReader["ID"]);
                    oJobAssoDeck.JobID = Convert.ToDecimal(oDbDataReader["JobID"]);
                    oJobAssoDeck.DeckCode = Convert.ToString(oDbDataReader["DeckCode"]);
                    oJobAssoDeck.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);                    
                    lstJobAssoDecks.Add(oJobAssoDeck);
                }
                oDbDataReader.Close();
                return lstJobAssoDecks;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JobAssoDeck GetJobAssoDeck(int Flag, string Value)
        {
            try
            {
                JobAssoDeck oJobAssoDeck = new JobAssoDeck();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETJobAssoDeckdetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Value));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    oJobAssoDeck.ID = Convert.ToDecimal(oDbDataReader["ID"]);
                    oJobAssoDeck.JobID = Convert.ToDecimal(oDbDataReader["JobID"]);
                    oJobAssoDeck.DeckCode = Convert.ToString(oDbDataReader["DeckCode"]);
                    oJobAssoDeck.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);
                }
                oDbDataReader.Close();
                return oJobAssoDeck;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddJobAssoDeck(int Flag,JobAssoDeck oJobAssoDeck)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateJobAssoDeck]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int16, oJobAssoDeck.ID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobID", DbType.Decimal, oJobAssoDeck.JobID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DeckCode", DbType.String, oJobAssoDeck.DeckCode));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveJobAssoDeck(int Flag, string Param1,string Param2)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_DeleteJobAssoDeck]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Param1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, Param2));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
    }
}
