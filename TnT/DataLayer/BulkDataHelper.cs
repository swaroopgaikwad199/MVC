using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TnT.DataLayer
{
    public class BulkDataHelper
    {

        public bool InsertPackagingDetails(DataTable dataTable, string connectionStr)
        {
            using (var connection = new SqlConnection(connectionStr))
            {
                SqlTransaction transaction = null;
                connection.Open();
                try
                {
                    transaction = connection.BeginTransaction();
                    using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                    {
                        sqlBulkCopy.BulkCopyTimeout = 0;
                        sqlBulkCopy.DestinationTableName = "PackagingDetails";
                        sqlBulkCopy.ColumnMappings.Add("Code", "Code");
                        sqlBulkCopy.ColumnMappings.Add("JobID", "JobID");
                        sqlBulkCopy.ColumnMappings.Add("PackageTypeCode", "PackageTypeCode");
                        sqlBulkCopy.ColumnMappings.Add("MfgPackDate", "MfgPackDate");
                        sqlBulkCopy.ColumnMappings.Add("ExpPackDate", "ExpPackDate");
                        sqlBulkCopy.ColumnMappings.Add("NextLevelCode", "NextLevelCode");
                        sqlBulkCopy.ColumnMappings.Add("IsRejected", "IsRejected");
                        sqlBulkCopy.ColumnMappings.Add("Reason", "Reason");
                        sqlBulkCopy.ColumnMappings.Add("SSCC", "SSCC");
                        sqlBulkCopy.ColumnMappings.Add("SSCCVarificationStatus", "SSCCVarificationStatus");
                        sqlBulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                        sqlBulkCopy.ColumnMappings.Add("LastUpdatedDate", "LastUpdatedDate");
                        sqlBulkCopy.ColumnMappings.Add("LineCode", "LineCode");

                        sqlBulkCopy.WriteToServer(dataTable);
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }

            }
        }
        public bool InsertPackagingDetailsForInitial(DataTable dataTable, string connectionStr)
        {
            using (var connection = new SqlConnection(connectionStr))
            {
                SqlTransaction transaction = null;
                connection.Open();
                try
                {
                    transaction = connection.BeginTransaction();
                    using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                    {
                        sqlBulkCopy.BulkCopyTimeout = 0;
                        sqlBulkCopy.DestinationTableName = "PackagingDetails";
                        sqlBulkCopy.ColumnMappings.Add("PAID", "PAID");
                        sqlBulkCopy.ColumnMappings.Add("Code", "Code");
                        sqlBulkCopy.ColumnMappings.Add("JobID", "JobID");
                        sqlBulkCopy.ColumnMappings.Add("PackageTypeCode", "PackageTypeCode");
                        sqlBulkCopy.ColumnMappings.Add("MfgPackDate", "MfgPackDate");
                        sqlBulkCopy.ColumnMappings.Add("ExpPackDate", "ExpPackDate");
                        sqlBulkCopy.ColumnMappings.Add("NextLevelCode", "NextLevelCode");
                        sqlBulkCopy.ColumnMappings.Add("IsRejected", "IsRejected");
                        sqlBulkCopy.ColumnMappings.Add("Reason", "Reason");
                        sqlBulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                        sqlBulkCopy.ColumnMappings.Add("SSCC", "SSCC");
                        sqlBulkCopy.ColumnMappings.Add("SSCCVarificationStatus", "SSCCVarificationStatus");
                        sqlBulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                        sqlBulkCopy.ColumnMappings.Add("LastUpdatedDate", "LastUpdatedDate");
                        sqlBulkCopy.ColumnMappings.Add("LineCode", "LineCode");
                        sqlBulkCopy.ColumnMappings.Add("IsLoose", "IsLoose");
                        sqlBulkCopy.ColumnMappings.Add("CryptoCode", "CryptoCode");
                        sqlBulkCopy.ColumnMappings.Add("PublicKey", "PublicKey");
                        sqlBulkCopy.WriteToServer(dataTable);
                    }
                    transaction.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    ExceptionHandler.ExceptionLogger.logException(ex);
                    transaction.Rollback();
                    return false;
                }

            }
        }

        public bool InsertUIDIdenties(DataTable dataTable)
        {
            string connectionString;
            connectionString = Utilities.getConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                connection.Open();
                try
                {
                    transaction = connection.BeginTransaction();
                    using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                    {
                        sqlBulkCopy.BulkCopyTimeout = 0;
                        sqlBulkCopy.DestinationTableName = "X_Identities";
                        sqlBulkCopy.ColumnMappings.Add("MasterId", "MasterId");
                        sqlBulkCopy.ColumnMappings.Add("SerialNo", "SerialNo");
                        sqlBulkCopy.ColumnMappings.Add("CodeType", "CodeType");
                        sqlBulkCopy.ColumnMappings.Add("PackTypeCode", "PackTypeCode");
                        sqlBulkCopy.ColumnMappings.Add("IsTransfered", "IsTransfered");
                        sqlBulkCopy.ColumnMappings.Add("CryptoCode", "CryptoCode");
                        sqlBulkCopy.ColumnMappings.Add("PublicKey", "PublicKey");

                        sqlBulkCopy.WriteToServer(dataTable);
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);
                    transaction.Rollback();
                    return false;
                }

            }
        }

        public bool InsertTracelinkUIDIdenties(DataTable dataTable)
        {
            string connectionString;
            connectionString = Utilities.getConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                connection.Open();
                try
                {
                    transaction = connection.BeginTransaction();
                    using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                    {
                        sqlBulkCopy.BulkCopyTimeout = 0;
                        sqlBulkCopy.DestinationTableName = "X_TracelinkUIDStore";
                        sqlBulkCopy.ColumnMappings.Add("TLRequestId", "TLRequestId");
                        sqlBulkCopy.ColumnMappings.Add("SerialNo", "SerialNo");
                        sqlBulkCopy.ColumnMappings.Add("IsUsed", "IsUsed");
                        sqlBulkCopy.ColumnMappings.Add("GTIN", "GTIN");
                        sqlBulkCopy.ColumnMappings.Add("Type", "Type");
                        sqlBulkCopy.ColumnMappings.Add("CryptoCode", "CryptoCode");
                        //sqlBulkCopy.ColumnMappings.Add("CryptoString", "CryptoString");
                        sqlBulkCopy.ColumnMappings.Add("PublicKey", "PublicKey");
                        sqlBulkCopy.WriteToServer(dataTable);
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);
                    transaction.Rollback();
                    return false;
                }

            }
        }

        public int setFlagToTransferd(int MasterId)
        {
            try
            {
                string connectionString; int rowsAffected = 0;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateUIDTransferStatus", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@MasterId", SqlDbType.Int).Value = MasterId ;
                        connection.Open();
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public int setFlagToTransferdUID(int MasterId)
        {
            try
            {
                string connectionString; int rowsAffected = 0;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateTracelinkUIDTransferStatus", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@MasterId", SqlDbType.Int).Value = MasterId ;
                        connection.Open();
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);
                throw;
            }

        }
    }
}