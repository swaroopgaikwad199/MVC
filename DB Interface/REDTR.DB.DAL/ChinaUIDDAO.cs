using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
    public partial class ChinaUIDDAO
    {
        public ChinaUIDDAO()
        {
            DbProviderHelper.GetConnection();
        }
        public List<ChinaUID> GetChinaUIDs()
        {
            try
            {
                List<ChinaUID> lstChinaUIDs = new List<ChinaUID>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("SELECTChinaUIDs", CommandType.StoredProcedure);
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    ChinaUID oChinaUID = new ChinaUID();
                    oChinaUID.SendId = Convert.ToInt32(oDbDataReader["SendId"]);

                    if (oDbDataReader["JobId"] != DBNull.Value)
                        oChinaUID.JobId = Convert.ToInt32(oDbDataReader["JobId"]);

                    if (oDbDataReader["TransId"] != DBNull.Value)
                        oChinaUID.TransId = Convert.ToInt32(oDbDataReader["TransId"]);

                    if (oDbDataReader["UID"] != DBNull.Value)
                        oChinaUID.UID = Convert.ToString(oDbDataReader["UID"]);

                    if (oDbDataReader["Result"] != DBNull.Value)
                        oChinaUID.Result = Convert.ToBoolean(oDbDataReader["Result"]);
                    lstChinaUIDs.Add(oChinaUID);
                }
                oDbDataReader.Close();
                return lstChinaUIDs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ChinaUID GetChinaUID(int SendId)
        {
            try
            {
                ChinaUID oChinaUID = new ChinaUID();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("SELECTChinaUID", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SendId", DbType.Int32, SendId));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    oChinaUID.SendId = Convert.ToInt32(oDbDataReader["SendId"]);

                    if (oDbDataReader["JobId"] != DBNull.Value)
                        oChinaUID.JobId = Convert.ToInt32(oDbDataReader["JobId"]);

                    if (oDbDataReader["TransId"] != DBNull.Value)
                        oChinaUID.TransId = Convert.ToInt32(oDbDataReader["TransId"]);

                    if (oDbDataReader["UID"] != DBNull.Value)
                        oChinaUID.UID = Convert.ToString(oDbDataReader["UID"]);

                    if (oDbDataReader["Result"] != DBNull.Value)
                        oChinaUID.Result = Convert.ToBoolean(oDbDataReader["Result"]);
                }
                oDbDataReader.Close();
                return oChinaUID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetTransactionNumber(int Flag, int JobID, string Deck)
        {
            try
            {
                int TransNo=0;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[SP_GETMaxTransNo]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Int32, JobID));
                if (Deck != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Deck", DbType.String, Deck));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Deck", DbType.String, DBNull.Value));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    TransNo = Convert.ToInt32(oDbDataReader["TRANSNO"]);
                }
                oDbDataReader.Close();
                return TransNo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> GetChinaUIDs(int flag, int JobID, string Deck)
        {
            try
            {
                List<string> UIDList=new List<string>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[SP_GETChinaUID]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@flag", DbType.Int32, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Int32, JobID));
                if (Deck != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Deck", DbType.String, Deck));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Deck", DbType.String, DBNull.Value));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    UIDList.Add(Convert.ToString(oDbDataReader["UID"]));
                }
                oDbDataReader.Close();
                return UIDList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddChinaUID(ChinaUID oChinaUID)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("INSERTChinaUID", CommandType.StoredProcedure);
                if (oChinaUID.JobId.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Int32, oChinaUID.JobId));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Int32, DBNull.Value));
                if (oChinaUID.TransId.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransId", DbType.Int32, oChinaUID.TransId));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransId", DbType.Int32, DBNull.Value));
                if (oChinaUID.UID != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String, oChinaUID.UID));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String, DBNull.Value));
                if (oChinaUID.Result.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Result", DbType.Boolean, oChinaUID.Result));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Result", DbType.Boolean, DBNull.Value));

                return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateChinaUID(ChinaUID oChinaUID,int Flag)
        {

            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateChinaUID]", CommandType.StoredProcedure);
                if (oChinaUID.JobId.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Int32, oChinaUID.JobId));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Int32, DBNull.Value));
                if (oChinaUID.TransId.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransId", DbType.Int32, oChinaUID.TransId));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransId", DbType.Int32, DBNull.Value));
                if (oChinaUID.UID != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@code", DbType.String, oChinaUID.UID));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@code", DbType.String, DBNull.Value));
                if (oChinaUID.Result.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Result", DbType.Boolean, oChinaUID.Result));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Result", DbType.Boolean, DBNull.Value));

                if (oChinaUID.IsUsed.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsUsed", DbType.Boolean, oChinaUID.IsUsed));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsUsed", DbType.Boolean, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SendId", DbType.Int32, oChinaUID.SendId));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 GetReamainingUIDCount(string Deck,int PAID)
        {
            try
            {
                string Query = " SELECT COUNT(ID) FROM UIDStore WHERE [Status] =0 AND [PackageTypeCode]='" + Deck + "' AND [PAID] ="+ PAID +"";
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
                Int32 Count = (Int32)DbProviderHelper.ExecuteScalar(oDbCommand);
                return Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateAcceptedToRejected(string code, int JID)
        {
            try
            {
                string Query = " UPDATE [PackagingDetails] Set [IsRejected] = 1 Where [Code] ='" + code + "' AND [JobID] =" + JID + "";
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
                DbProviderHelper.ExecuteNonQuery(oDbCommand);
                Query = " UPDATE [ChinaUID] Set [Result] = 0  Where [UID] ='" + code + "' AND [JobId] =" + JID + "";
                oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
                DbProviderHelper.ExecuteNonQuery(oDbCommand);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;

            }
        }
        public int InsertChinaUIDFromStore(int JobID, Int32 Quantity, string Deck,int PAID)
        {
            try
            {
                    DbCommand oDbCommand = DbProviderHelper.CreateCommand("[SP_InsertUIDFromStore]", CommandType.StoredProcedure);
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Int32, JobID));
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Int32, PAID));
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Top", DbType.Int32, Quantity));
                    if (Deck != null)
                        oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Deck", DbType.String, Deck));
                    else
                        oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Deck", DbType.String, DBNull.Value));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RestoreChinaUID(int Flag, int JobID, string Deck)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[SP_RestoreUnUsedUIDs]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Int32, JobID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                if (Deck != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Deck", DbType.String, Deck));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Deck", DbType.String, DBNull.Value));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveChinaUID(ChinaUID oChinaUID)
        {

            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("DELETEChinaUID", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SendId", DbType.Int32, oChinaUID.SendId));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveChinaUID(int SendId)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("DELETEChinaUID", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SendId", DbType.Int32, SendId));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetChinaUIDsDataTable(String Query)
        {
            try
            {
                DataTable dt;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                dt = DbProviderHelper.FillDataTable(DbAdpt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
