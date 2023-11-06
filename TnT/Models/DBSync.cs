using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TnT.Models.Job;

namespace TnT.Models
{
    //public class DBBulk
    //{
    //    public string connectionString { get; set; }

    //    public DBBulk(string token = null)
    //    {
    //        if (string.IsNullOrEmpty(token))
    //        {
    //            //connectionString = Utilities.getConnectionString(Token.UserDBCon.ToString());
    //            connectionString = Utilities.getConnectionString("DefaultConnection");
    //        }
    //        else
    //        {
    //            connectionString = Utilities.getConnectionString(token);
    //        }
    //    }

    //    public object ServerExecuteQuery(string query)
    //    {
    //        //string connectionString = Utilities.getConnectionString(Token.UserDBCon.ToString());
    //        using (var connection = new SqlConnection(connectionString))
    //        {
    //            SqlTransaction transaction = null;
    //            try
    //            {
    //                connection.Open();

    //                transaction = connection.BeginTransaction();

    //                SqlCommand cmd = new SqlCommand(query, connection, transaction);

    //                cmd.CommandTimeout = 0;

    //                var output = cmd.ExecuteNonQuery();

    //                transaction.Commit();

    //                return output;
    //            }
    //            catch (Exception ex)
    //            {
    //                transaction.Rollback();
    //                //Models.Logger.LogError(ex);
    //                return 0;
    //            }
    //        }
    //    }

    //    public object ServerBulkUpdate(string Query, Dictionary<string, object> parameter, List<ExecutionStatus> lstExecutionStatus)
    //    {
    //        //string connectionString = Utilities.getConnectionString(Token.UserDBCon.ToString());

    //        using (var connection = new SqlConnection(connectionString))
    //        {
    //            SqlTransaction transaction = null;
    //            try
    //            {
    //                connection.Open();

    //                transaction = connection.BeginTransaction();

    //                SqlCommand cmd = new SqlCommand(Query, connection, transaction);

    //                cmd.CommandType = CommandType.StoredProcedure;
    //                foreach (var par in parameter)
    //                {
    //                    cmd.Parameters.AddWithValue(par.Key, par.Value);
    //                }
    //                cmd.CommandTimeout = 0;

    //                var output = cmd.ExecuteNonQuery();

    //                transaction.Commit();

    //                return output;
    //            }
    //            catch (Exception ex)
    //            {
    //                lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = false, Message = "ERRO :" + Query + "," + ex.Message });
    //                transaction.Rollback();
    //               // Models.Logger.LogError(ex);
    //                return 0;
    //            }
    //        }
    //    }

    //    public DataSet ServerGetDataSet(string Query, Dictionary<string, object> parameter = null)
    //    {
    //        //string connectionString = Utilities.getConnectionString(Token.UserDBCon.ToString());
    //        SqlCommand cmd = new SqlCommand();
    //        SqlDataAdapter da = new SqlDataAdapter();
    //        DataSet ds = new DataSet();

    //        using (var connection = new SqlConnection(connectionString))
    //        {
    //            try
    //            {
    //                cmd = new SqlCommand(Query, connection);
    //                if (parameter != null && parameter.Count > 0)
    //                {
    //                    foreach (var item in parameter)
    //                    {
    //                        cmd.Parameters.Add(new SqlParameter(item.Key, item.Value));
    //                    }
    //                    cmd.CommandType = CommandType.StoredProcedure;
    //                }
    //                else
    //                {
    //                    cmd.CommandType = CommandType.Text;
    //                }

    //                cmd.CommandTimeout = 0;
    //                da.SelectCommand = cmd;
    //                da.Fill(ds);
    //            }
    //            catch (Exception ex)
    //            {
    //                //Models.Logger.LogError(ex);
    //            }
    //            finally
    //            {
    //                cmd.Dispose();
    //            }
    //        }
    //        return ds;
    //    }

    //    public bool ServerBulkInsert(DataSet dataSet, List<ExecutionStatus> lstExecutionStatus)
    //    {
    //        //string connectionString = Utilities.getConnectionString(Token.UserDBCon.ToString());
    //        using (var connection = new SqlConnection(connectionString))
    //        {
    //            SqlTransaction transaction = null;
    //            connection.Open();
    //            try
    //            {
    //                transaction = connection.BeginTransaction();
    //                using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
    //                {
    //                    foreach (DataTable dataTable in dataSet.Tables)
    //                    {
    //                        sqlBulkCopy.ColumnMappings.Clear();

    //                        sqlBulkCopy.BulkCopyTimeout = 0;
    //                        sqlBulkCopy.DestinationTableName = dataTable.TableName;

    //                        foreach (var item in dataTable.Columns)
    //                        {
    //                            sqlBulkCopy.ColumnMappings.Add(item.ToString(), item.ToString());
    //                        }

    //                        sqlBulkCopy.WriteToServer(dataTable);
    //                    }

    //                    transaction.Commit();
    //                    //lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = true, Message = "SERVER BULK INSERT : " + sqlBulkCopy.DestinationTableName + " SUCCESS" });
    //                    //Logger.LogInfo("SERVER BULK INSERT : " + sqlBulkCopy.DestinationTableName + " SUCCESS");
    //                }
    //                return true;
    //            }
    //            catch (SqlException sql)
    //            {
    //                lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = false, Message = "BULK COPY ERROR : " + sql.Message });
    //                transaction.Rollback();
    //                //Logger.LogError(sql);
    //                return false;
    //            }
    //            catch (Exception ex)
    //            {
    //                lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = false, Message = "BULK COPY ERROR : " + ex.Message });
    //                transaction.Rollback();
    //                //Logger.LogError(ex);
    //                return false;
    //            }
    //        }
    //    }
    //    //string DB_BACKUP_PATH = @"D:\TNTWEB\DB\";
    //    //string DB_LOG_PATH = @"D:\DATA\";
    //    //string DB_BACKUP_FILE_NAME = @"LINE_WISE_DB.bak";
    //    public object Generate_Line_Wise_DB(string databaseName)
    //    {
    //        databaseName = databaseName.Replace("-", "_");

    //        object status = null;
    //        if (string.IsNullOrEmpty(databaseName))
    //        {
    //            status = "INVALID DATABASE NAME";
    //            return status;
    //        }

    //        string sp_query = "EXEC sp_Create_Line_Wise_DB '" + databaseName + "'";

    //        string connectionString = Utilities.getConnectionString("");

    //        using (var connection = new SqlConnection(connectionString))
    //        {
    //            try
    //            {
    //                connection.Open();
    //                SqlCommand cmd = new SqlCommand(sp_query, connection);
    //                cmd.CommandTimeout = 0;
    //                var output = cmd.ExecuteScalar();
    //                return output;
    //            }
    //            catch (Exception ex)
    //            {
    //                //Models.Logger.LogError(ex);
    //                return 0;
    //            }
    //        }
    //        //return status;
    //    }
    //}

    public class DBSync
    {
        #region Database Operations
        public SqlConnection dbConnection { get; set; }
        public DbProviderFactory dbProviderFactory;
        string dbConStr = string.Empty;
        string UserDBCon = "DefaultConnection";

        public DBSync(LineLocation lineLocation = null)
        {
            if (lineLocation == null)
            {
                dbConStr = ConfigurationManager.ConnectionStrings[UserDBCon].ToString();
            }
            else
            {
                DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                builder.ConnectionString = ConfigurationManager.ConnectionStrings[UserDBCon].ToString();

                builder["Data Source"] = lineLocation.LineIP.Trim();
                builder["Initial Catalog"] = lineLocation.DBName.Trim();
                builder["user id"] = lineLocation.SQLUsername.Trim();
                builder["password"] = lineLocation.SQLPassword.Trim();

                dbConStr = builder.ConnectionString;
            }
            dbConnection = new SqlConnection(dbConStr);
            dbProviderFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
        }

        public bool OpenConnection()
        {
            try
            {
                if (dbConnection.State == ConnectionState.Open)
                {
                    return true;
                }
                else
                {
                    if (string.IsNullOrEmpty(dbConnection.ConnectionString))
                    {
                        dbConnection.ConnectionString = dbConStr;
                    }
                    dbConnection.Open();
                }
                return (dbConnection.State == ConnectionState.Open);
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex);
                return false;
            }
        }


        public bool SyncVerifyJob(DataSet ds, List<ExecutionStat> lstExecutionStatus)
        {
            string tableName = string.Empty;
            OpenConnection();

            using (dbConnection)
            {
                SqlTransaction transaction = null;
                try
                {
                    transaction = dbConnection.BeginTransaction();
                    using (var sqlBulkCopy = new SqlBulkCopy(dbConnection, SqlBulkCopyOptions.TableLock, transaction))
                    {
                        sqlBulkCopy.BulkCopyTimeout = 0;

                        int tableNo = 0;
                        DataTable dtNames = new DataTable();
                        foreach (DataTable dt in ds.Tables)
                        {
                            if (tableNo == 0)
                            {
                                dtNames = dt;
                            }
                            else
                            {
                                sqlBulkCopy.DestinationTableName = dtNames.Rows[0][tableNo - 1].ToString();
                                sqlBulkCopy.ColumnMappings.Clear();
                                tableName = sqlBulkCopy.DestinationTableName;
                                foreach (var item in dt.Columns)
                                {
                                    sqlBulkCopy.ColumnMappings.Add(item.ToString(), item.ToString());
                                }

                                //Trace.TraceInformation("SYNC " + ta   bleName);

                                sqlBulkCopy.WriteToServer(dt);
                                //lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = true, Message = "SYNC " + sqlBulkCopy.DestinationTableName + " SUCCESS ON : " });
                            }

                            tableNo++;
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transaction = null;
                    lstExecutionStatus.Add(new ExecutionStat { IsSuccess = false, Message = "Sync : " + tableName + "  ERROR : " + ex.Message });
                    return false;
                }
            }
        }

        public DataSet GetDataSet(string Query, Dictionary<string, object> parameter = null)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();

            using (dbConnection)
            {
                OpenConnection();

                try
                {
                    cmd = new SqlCommand(Query, dbConnection);
                    if (parameter != null && parameter.Count > 0)
                    {
                        foreach (var item in parameter)
                        {
                            cmd.Parameters.Add(new SqlParameter(item.Key, item.Value));
                        }
                        cmd.CommandType = CommandType.StoredProcedure;
                    }
                    else
                    {
                        cmd.CommandType = CommandType.Text;
                    }

                    cmd.CommandTimeout = 0;
                    da.SelectCommand = cmd;

                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    //Models.Logger.LogError(ex);
                }
                finally
                {
                    cmd.Dispose();
                }
            }
            return ds;
        }

        #endregion

        //public bool IsOpen()
        //{
        //    if (dbConnection.State == ConnectionState.Open)
        //        return true;
        //    else if (dbConnection.State == ConnectionState.Closed)
        //        return false;
        //    else
        //        return false;
        //}
        //public DbDataReader ExecuteReader(DbCommand dbCommand)
        //{
        //    if (dbConnection.State != ConnectionState.Open) dbConnection.Open();

        //    return dbCommand.ExecuteReader();
        //}
        //public DbDataReader ExecuteReader(DbCommand dbCommand, CommandBehavior commandBehavior)
        //{
        //    try
        //    {
        //        if (dbConnection.State != ConnectionState.Open) dbConnection.Open();

        //        return dbCommand.ExecuteReader(commandBehavior);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //if (KeepConnAlive == false)
        //        dbConnection.Close();
        //    }
        //}
        //public int ExecuteNonQuery(DbCommand dbCommand)
        //{
        //    try
        //    {
        //        if (dbConnection.State != ConnectionState.Open) dbConnection.Open();

        //        return dbCommand.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //if (KeepConnAlive == false)
        //        dbConnection.Close();
        //    }
        //}
        //public object ExecuteScalar(DbCommand dbCommand)
        //{
        //    try
        //    {
        //        if (dbConnection.State != ConnectionState.Open) dbConnection.Open();

        //        return dbCommand.ExecuteScalar();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        // if (KeepConnAlive == false)
        //        dbConnection.Close();
        //    }
        //}
        //public DataSet GetXMLDataSet(DbDataAdapter dbDataAdapter, string Nodename)
        //{
        //    try
        //    {
        //        DataSet dataSet = new DataSet();
        //        if (dbConnection.State != ConnectionState.Open) dbConnection.Open();

        //        dbDataAdapter.Fill(dataSet, Nodename);
        //        return dataSet;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //if (KeepConnAlive == false)
        //        dbConnection.Close();
        //    }
        //}
        //public DataTable FillDataTable(DbDataAdapter dbDataAdapter)
        //{
        //    try
        //    {
        //        DataTable dataTable = new DataTable();

        //        if (dbConnection.State != ConnectionState.Open) dbConnection.Open();

        //        dbDataAdapter.Fill(dataTable);
        //        return dataTable;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        // if (KeepConnAlive == false)
        //        dbConnection.Close();
        //    }
        //}

        //public DbParameter CreateParameter(string parameterName, DbType dbType, ParameterDirection parameterDirection, string sourceColumn, DataRowVersion dataRowVersion, bool sourceColumnNullMapping, object value)
        //{
        //    DbParameter oDbParameter = dbProviderFactory.CreateParameter();
        //    oDbParameter.ParameterName = parameterName;
        //    oDbParameter.DbType = dbType;
        //    oDbParameter.Direction = parameterDirection;
        //    oDbParameter.SourceColumn = sourceColumn;
        //    oDbParameter.SourceVersion = dataRowVersion;
        //    oDbParameter.SourceColumnNullMapping = sourceColumnNullMapping;
        //    if (value != null)
        //        oDbParameter.Value = value;
        //    else
        //        oDbParameter.Value = DBNull.Value;
        //    return oDbParameter;
        //}

        //public DataSet GetDataSet(DbDataAdapter dbDataAdapter)
        //{
        //    try
        //    {
        //        DataSet dataSet = new DataSet();

        //        if (dbConnection.State != ConnectionState.Open) dbConnection.Open();

        //        dbDataAdapter.Fill(dataSet);
        //        return dataSet;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //if (KeepConnAlive == false)
        //        dbConnection.Close();
        //    }
        //}
        //public DbDataAdapter CreateDataAdapter(DbCommand selectCommand)
        //{
        //    DbDataAdapter dbDataAdapter = dbProviderFactory.CreateDataAdapter();
        //    dbDataAdapter.SelectCommand = selectCommand;
        //    return dbDataAdapter;
        //}
        //public DbCommand CreateCommand(String commandText, CommandType commandType)
        //{
        //    DbCommand dbCommand = dbProviderFactory.CreateCommand();
        //    // SqlCommand dbCommand = new SqlCommand();
        //    dbCommand.Connection = dbConnection;
        //    dbCommand.CommandType = commandType;
        //    dbCommand.CommandText = commandText;
        //    dbCommand.CommandTimeout = 0;
        //    return dbCommand;
        //}
        //public DbParameter CreateParameter(string parameterName, DbType dbType, object value)
        //{
        //    DbParameter oDbParameter = dbProviderFactory.CreateParameter();
        //    oDbParameter.ParameterName = parameterName;
        //    oDbParameter.DbType = dbType;
        //    if (value != null)
        //        oDbParameter.Value = value;
        //    else
        //        oDbParameter.Value = DBNull.Value;

        //    return oDbParameter;
        //}

        //public object ServerBulkUpdate(string Query, Dictionary<string, object> parameter, List<ExecutionStatus> lstExecutionStatus)
        //{
        //    using (dbConnection)
        //    {
        //        SqlTransaction transaction = null;
        //        try
        //        {
        //            dbConnection.Open();

        //            transaction = dbConnection.BeginTransaction();

        //            SqlCommand cmd = new SqlCommand(Query, dbConnection, transaction);

        //            cmd.CommandType = CommandType.StoredProcedure;

        //            foreach (var par in parameter)
        //            {
        //                cmd.Parameters.AddWithValue(par.Key, par.Value);
        //            }
        //            cmd.CommandTimeout = 0;

        //            var output = cmd.ExecuteNonQuery();

        //            transaction.Commit();

        //            return output;
        //        }
        //        catch (Exception ex)
        //        {
        //            lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = false, Message = "ERRO :" + Query + "," + ex.Message });
        //            transaction.Rollback();
        //            // Models.Logger.LogError(ex);
        //            return 0;
        //        }
        //    }
        //}

        //public bool LineBulkInsert(DataSet ds, List<ExecutionStatus> lstExecutionStatus)
        //{
        //    string tableName = string.Empty;
        //    using (dbConnection)
        //    {
        //        SqlTransaction transaction = null;
        //        OpenConnection();
        //        try
        //        {
        //            transaction = dbConnection.BeginTransaction();
        //            using (var sqlBulkCopy = new SqlBulkCopy(dbConnection, SqlBulkCopyOptions.TableLock, transaction))
        //            {
        //                sqlBulkCopy.BulkCopyTimeout = 0;

        //                //DataTable dtNames = new DataTable();
        //                foreach (DataTable dt in ds.Tables)
        //                {
        //                    sqlBulkCopy.DestinationTableName = dt.TableName.ToString();
        //                    sqlBulkCopy.ColumnMappings.Clear();
        //                    //tableName = sqlBulkCopy.DestinationTableName;
        //                    foreach (var item in dt.Columns)
        //                    {
        //                        sqlBulkCopy.ColumnMappings.Add(item.ToString(), item.ToString());
        //                    }

        //                    //Trace.TraceInformation("SYNC " + tableName);

        //                    sqlBulkCopy.WriteToServer(dt);
        //                }
        //            }
        //            transaction.Commit();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            //Logger.LogError(ex);
        //            transaction = null;
        //            lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = false, Message = "Sync : " + tableName + "  ERROR : " + ex.Message });
        //            return false;
        //        }
        //    }
        //}

        //public List<string> GetLineStatus(bool IsTemperEvidence = false, bool IsGlueSetting = false)
        //{
        //    List<string> lstStatus = new List<string>();
        //    try
        //    {
        //        DbCommand oDbCommand = CreateCommand("[up_SyncGetLineStatus]", CommandType.StoredProcedure);
        //        oDbCommand.Parameters.Add(CreateParameter("@IsTemperEvidence", DbType.Decimal, IsTemperEvidence));
        //        oDbCommand.Parameters.Add(CreateParameter("@IsGlueSetting", DbType.Decimal, IsGlueSetting));

        //        DbDataAdapter DbAdpt = CreateDataAdapter(oDbCommand);
        //        DataSet ds = GetDataSet(DbAdpt);

        //        foreach (DataRow item in ds.Tables[0].Rows)
        //        {
        //            lstStatus.Add(item[0].ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lstStatus.Add("ERROR GETTING LINE STATUS : " + ex.Message);
        //    }
        //    return lstStatus;
        //}

        //public bool BulkInsert(DataTable dataTable)
        //{
        //    using (lineConnection)
        //    {
        //        SqlTransaction transaction = null;
        //        try
        //        {
        //            transaction = lineConnection.BeginTransaction();
        //            using (var sqlBulkCopy = new SqlBulkCopy(lineConnection, SqlBulkCopyOptions.TableLock, transaction))
        //            {

        //                sqlBulkCopy.BulkCopyTimeout = 0;
        //                sqlBulkCopy.DestinationTableName = "PackagingDetails";
        //                sqlBulkCopy.ColumnMappings.Add("Code", "Code");
        //                sqlBulkCopy.ColumnMappings.Add("JobID", "JobID");
        //                sqlBulkCopy.ColumnMappings.Add("PackageTypeID", "PackageTypeID");
        //                sqlBulkCopy.ColumnMappings.Add("NextLevelCode", "NextLevelCode");
        //                sqlBulkCopy.ColumnMappings.Add("IsRejected", "IsRejected");
        //                sqlBulkCopy.ColumnMappings.Add("SSCC", "SSCC");
        //                sqlBulkCopy.ColumnMappings.Add("SSCCVarificationStatus", "SSCCVarificationStatus");
        //                sqlBulkCopy.ColumnMappings.Add("IsManualUpdated", "IsManualUpdated");
        //                sqlBulkCopy.ColumnMappings.Add("CaseSeqNum", "CaseSeqNum");
        //                sqlBulkCopy.ColumnMappings.Add("OperatorId", "OperatorId");
        //                sqlBulkCopy.ColumnMappings.Add("ManualUpdatedDesc", "ManualUpdatedDesc");
        //                sqlBulkCopy.ColumnMappings.Add("IsDecomission", "IsDecomission");
        //                sqlBulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
        //                sqlBulkCopy.ColumnMappings.Add("LastUpdatedDate", "LastUpdatedDate");
        //                sqlBulkCopy.ColumnMappings.Add("LineID", "LineID");
        //                sqlBulkCopy.ColumnMappings.Add("PrinterId", "PrinterId");
        //                sqlBulkCopy.ColumnMappings.Add("SYNC", "SYNC");
        //                sqlBulkCopy.ColumnMappings.Add("RCResult", "RCResult");
        //                sqlBulkCopy.ColumnMappings.Add("IsUsed", "IsUsed");
        //                sqlBulkCopy.ColumnMappings.Add("IsLoose", "IsLoose");
        //                sqlBulkCopy.ColumnMappings.Add("FirstDeckCount", "FirstDeckCount");
        //                sqlBulkCopy.ColumnMappings.Add("Grade", "Grade");

        //                sqlBulkCopy.WriteToServer(dataTable);
        //            }
        //            transaction.Commit();
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            transaction.Rollback();
        //            return false;
        //        }
        //    }
        //}

        //public object GetLineJobCount()
        //{
        //    try
        //    {
        //        DbCommand oDbCommand = CreateCommand("SELECT COUNT(JobID) CNT FROM Job", CommandType.Text);

        //        //oDbCommand.Parameters.Add(CreateParameter("@Flag", DbType.Decimal, Flag));

        //        object jobCount = ExecuteScalar(oDbCommand);

        //        return jobCount;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool ResetLine()
        //{
        //    bool IsReset = false;

        //    try
        //    {
        //        DbCommand oDbCommand = CreateCommand("[up_ResetLine]", CommandType.StoredProcedure);
        //        oDbCommand.Parameters.Add(CreateParameter("@IsReset", DbType.Boolean, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));

        //        ExecuteNonQuery(oDbCommand);

        //        if (oDbCommand.Parameters["@IsReset"].Value != DBNull.Value)
        //            return Convert.ToBoolean(oDbCommand.Parameters["@IsReset"].Value);
        //        else
        //            IsReset = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        IsReset = false;
        //        throw ex;
        //    }
        //    return IsReset;
        //}

        //public int SetIDENTITY(bool identity)
        //{
        //    try
        //    {
        //        DbCommand oDbCommand = CreateCommand("[up_SetIDENTITY]", CommandType.StoredProcedure);
        //        oDbCommand.Parameters.Add(CreateParameter("@Identity", DbType.Boolean, identity));

        //        ExecuteNonQuery(oDbCommand);

        //        if (oDbCommand.Parameters["@PKID"].Value != DBNull.Value)
        //            return Convert.ToInt32(oDbCommand.Parameters["@PKID"].Value);
        //        else
        //            return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public int SyncJob(Models.Job.Job oJob)
        //{
        //    try
        //    {
        //        DbCommand oDbCommand = CreateCommand("[up_SyncJob]", CommandType.StoredProcedure);

        //        oDbCommand.Parameters.Add(CreateParameter("@JID", DbType.Decimal, oJob.JobID));
        //        oDbCommand.Parameters.Add(CreateParameter("@PONumber", DbType.String, oJob.PONumber));
        //        oDbCommand.Parameters.Add(CreateParameter("@PAID", DbType.Decimal, oJob.PAID));
        //        oDbCommand.Parameters.Add(CreateParameter("@BatchNo", DbType.String, oJob.BatchNo));
        //        oDbCommand.Parameters.Add(CreateParameter("@Quantity", DbType.Int32, oJob.Quantity));
        //        oDbCommand.Parameters.Add(CreateParameter("@SurPlusQty", DbType.Int32, oJob.SurPlusQty));
        //        oDbCommand.Parameters.Add(CreateParameter("@JobStatus", DbType.Int16, oJob.JobStatus));
        //        oDbCommand.Parameters.Add(CreateParameter("@AutomaticBatchCloser", DbType.Boolean, oJob.AutomaticBatchCloser));
        //        oDbCommand.Parameters.Add(CreateParameter("@ComplianceID", DbType.Decimal, oJob.ComplianceID));
        //        oDbCommand.Parameters.Add(CreateParameter("@Remarks", DbType.String, oJob.Remarks));
        //        oDbCommand.Parameters.Add(CreateParameter("@CreatedBy", DbType.Decimal, oJob.CreatedBy));
        //        oDbCommand.Parameters.Add(CreateParameter("@CreatedDate", DbType.DateTime, oJob.CreatedDate));
        //        oDbCommand.Parameters.Add(CreateParameter("@VerifiedBy", DbType.Decimal, oJob.VerifiedBy));
        //        oDbCommand.Parameters.Add(CreateParameter("@VerifiedDate", DbType.DateTime, oJob.VerifiedDate));
        //        oDbCommand.Parameters.Add(CreateParameter("@LastUpdatedDate", DbType.DateTime, DateTime.Now));
        //        oDbCommand.Parameters.Add(CreateParameter("@NoReadCount", DbType.Int32, oJob.NoReadCount));
        //        oDbCommand.Parameters.Add(CreateParameter("@UseDay", DbType.Boolean, oJob.UseDay));
        //        oDbCommand.Parameters.Add(CreateParameter("@DateFormat", DbType.String, oJob.DateFormat));
        //        oDbCommand.Parameters.Add(CreateParameter("@PackagingLevelId", DbType.Int32, oJob.PackagingLvlId));
        //        oDbCommand.Parameters.Add(CreateParameter("@CustomerId", DbType.Int32, oJob.CustomerId));
        //        oDbCommand.Parameters.Add(CreateParameter("@ProviderId", DbType.Int32, oJob.ProviderId));
        //        oDbCommand.Parameters.Add(CreateParameter("@LineCode", DbType.String, oJob.LineCode));

        //        if (oJob.MfgDate != System.DateTime.MinValue)
        //            oDbCommand.Parameters.Add(CreateParameter("@MfgDate", DbType.DateTime, oJob.MfgDate.Date));
        //        else
        //            oDbCommand.Parameters.Add(CreateParameter("@MfgDate", DbType.DateTime, DBNull.Value));

        //        if (oJob.ExpDate != System.DateTime.MinValue)
        //            oDbCommand.Parameters.Add(CreateParameter("@ExpDate", DbType.DateTime, oJob.ExpDate.Date));
        //        else
        //            oDbCommand.Parameters.Add(CreateParameter("@ExpDate", DbType.DateTime, DBNull.Value));

        //        if (oJob.JobStartTime != System.DateTime.MinValue)
        //            oDbCommand.Parameters.Add(CreateParameter("@JobStartTime", DbType.DateTime, oJob.JobStartTime));
        //        else
        //            oDbCommand.Parameters.Add(CreateParameter("@JobStartTime", DbType.DateTime, DBNull.Value));

        //        if (oJob.JobEndTime.HasValue)
        //            oDbCommand.Parameters.Add(CreateParameter("@JobEndTime", DbType.DateTime, oJob.JobEndTime));
        //        else
        //            oDbCommand.Parameters.Add(CreateParameter("@JobEndTime", DbType.DateTime, DBNull.Value));

        //        //oDbCommand.Parameters.Add(CreateParameter("@ProductName", DbType.String, ProductName));
        //        //oDbCommand.Parameters.Add(CreateParameter("@CreatedByName", DbType.String, CreatedByName));
        //        //oDbCommand.Parameters.Add(CreateParameter("@VerifiedByName", DbType.String, VerifiedByName));
        //        oDbCommand.Parameters.Add(CreateParameter("@ID", DbType.Decimal, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
        //        //NoReadCount
        //        //oDbCommand.Parameters.Add(CreateParameter("@DetailInfo", DbType.String, oJob.DetailInfo));
        //        //oDbCommand.Parameters.Add(CreateParameter("@PrimaryPCMapCount", DbType.Int32, oJob.PrimaryPCMapCount));
        //        //oDbCommand.Parameters.Add(CreateParameter("@LabelStartIndex", DbType.Decimal, oJob.LabelStartIndex));
        //        //oDbCommand.Parameters.Add(CreateParameter("@ForExport", DbType.Boolean, oJob.ForExport));
        //        //oDbCommand.Parameters.Add(CreateParameter("@MLNO", DbType.String, oJob.MLNO));
        //        //oDbCommand.Parameters.Add(CreateParameter("@TenderText", DbType.String, oJob.TenderText));
        //        //oDbCommand.Parameters.Add(CreateParameter("@JobWithUID", DbType.Boolean, oJob.JobWithUID));
        //        //oDbCommand.Parameters.Add(CreateParameter("@AppID", DbType.Int16, oJob.AppId));
        //        //oDbCommand.Parameters.Add(CreateParameter("@LineCode", DbType.String, oJob.LineCode));
        //        //oDbCommand.Parameters.Add(CreateParameter("@PPNCountryCode", DbType.String, oJob.PPNCountryCode));
        //        //oDbCommand.Parameters.Add(CreateParameter("@PPNPostalCode", DbType.String, oJob.PPNPostalCode));
        //        //oDbCommand.Parameters.Add(CreateParameter("@CompType", DbType.String, oJob.CompType));

        //        ExecuteNonQuery(oDbCommand);

        //        if (oDbCommand.Parameters["@ID"].Value != DBNull.Value)
        //            return Convert.ToInt32(oDbCommand.Parameters["@ID"].Value);
        //        else
        //            return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int SyncJobDetails(Models.Job.JobDetails oJobDetails)
        //{
        //    try
        //    {
        //        DbCommand oDbCommand = CreateCommand("[up_SyncJobDetails]", CommandType.StoredProcedure);

        //        oDbCommand.Parameters.Add(CreateParameter("@JobDetailsId", DbType.Int32, oJobDetails.JobDetailsId));
        //        oDbCommand.Parameters.Add(CreateParameter("@JobID", DbType.String, oJobDetails.JobID));
        //        oDbCommand.Parameters.Add(CreateParameter("@Deckcode", DbType.String, oJobDetails.Deckcode));
        //        oDbCommand.Parameters.Add(CreateParameter("@DeckSize", DbType.String, oJobDetails.DeckSize));
        //        oDbCommand.Parameters.Add(CreateParameter("@BundleQty", DbType.Byte, oJobDetails.BundleQty));
        //        oDbCommand.Parameters.Add(CreateParameter("@MRP", DbType.Decimal, oJobDetails.MRP));
        //        oDbCommand.Parameters.Add(CreateParameter("@LabelName", DbType.String, oJobDetails.LabelName));
        //        oDbCommand.Parameters.Add(CreateParameter("@Filter", DbType.String, oJobDetails.Filter));

        //        oDbCommand.Parameters.Add(CreateParameter("@PKID", DbType.Decimal, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));

        //        ExecuteNonQuery(oDbCommand);

        //        if (oDbCommand.Parameters["@PKID"].Value != DBNull.Value)
        //            return Convert.ToInt32(oDbCommand.Parameters["@PKID"].Value);
        //        else
        //            return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int SyncProviders(Models.Providers.M_Providers oProvider)
        //{
        //    try
        //    {
        //        DbCommand oDbCommand = CreateCommand("[up_SyncProviders]", CommandType.StoredProcedure);

        //        oDbCommand.Parameters.Add(CreateParameter("@Id", DbType.Decimal, oProvider.Id));

        //        oDbCommand.Parameters.Add(CreateParameter("@Name", DbType.String, oProvider.Name));
        //        oDbCommand.Parameters.Add(CreateParameter("@CreatedOn", DbType.DateTime, oProvider.CreatedOn));
        //        oDbCommand.Parameters.Add(CreateParameter("@IsActive", DbType.Boolean, oProvider.IsActive));
        //        oDbCommand.Parameters.Add(CreateParameter("@IsDeleted", DbType.Boolean, oProvider.IsDeleted));
        //        oDbCommand.Parameters.Add(CreateParameter("@Code", DbType.String, oProvider.Code));

        //        oDbCommand.Parameters.Add(CreateParameter("@PKID", DbType.Decimal, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));

        //        ExecuteNonQuery(oDbCommand);

        //        if (oDbCommand.Parameters["@PKID"].Value != DBNull.Value)
        //            return Convert.ToInt32(oDbCommand.Parameters["@PKID"].Value);
        //        else
        //            return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int SyncCustomers(Models.Customer.M_Customer oCustomer)
        //{
        //    try
        //    {
        //        DbCommand oDbCommand = CreateCommand("[up_SyncCustomers]", CommandType.StoredProcedure);

        //        oDbCommand.Parameters.Add(CreateParameter("@CustomerId", DbType.Byte, oCustomer.CustomerId));
        //        oDbCommand.Parameters.Add(CreateParameter("@CompanyName", DbType.String, oCustomer.CompanyName));
        //        oDbCommand.Parameters.Add(CreateParameter("@ContactPerson", DbType.String, oCustomer.ContactPerson));
        //        oDbCommand.Parameters.Add(CreateParameter("@ContactNumber", DbType.String, oCustomer.ContactNo));
        //        oDbCommand.Parameters.Add(CreateParameter("@Email", DbType.String, oCustomer.Email));
        //        oDbCommand.Parameters.Add(CreateParameter("@Address", DbType.String, oCustomer.Address));
        //        oDbCommand.Parameters.Add(CreateParameter("@Country", DbType.Int32, oCustomer.Country));
        //        oDbCommand.Parameters.Add(CreateParameter("@IsActive", DbType.Boolean, oCustomer.IsActive));
        //        oDbCommand.Parameters.Add(CreateParameter("@APIKey", DbType.String, oCustomer.APIKey));
        //        oDbCommand.Parameters.Add(CreateParameter("@SenderID", DbType.String, oCustomer.SenderId));
        //        oDbCommand.Parameters.Add(CreateParameter("@ReceiverID", DbType.String, oCustomer.ReceiverId));
        //        oDbCommand.Parameters.Add(CreateParameter("@CreatedDate", DbType.DateTime, oCustomer.CreatedOn));
        //        oDbCommand.Parameters.Add(CreateParameter("@LastUpdatedDate", DbType.DateTime, oCustomer.LastModified));
        //        oDbCommand.Parameters.Add(CreateParameter("@CreatedBy", DbType.Int32, oCustomer.CreatedBy));
        //        oDbCommand.Parameters.Add(CreateParameter("@ModifiedBy", DbType.Int32, oCustomer.ModifiedBy));
        //        oDbCommand.Parameters.Add(CreateParameter("@IsDeleted", DbType.Boolean, oCustomer.IsDeleted));
        //        oDbCommand.Parameters.Add(CreateParameter("@APIURL", DbType.String, oCustomer.APIURL));
        //        oDbCommand.Parameters.Add(CreateParameter("@ProviderID", DbType.Byte, oCustomer.ProviderId));
        //        oDbCommand.Parameters.Add(CreateParameter("@IsSSCC", DbType.Boolean, oCustomer.IsSSCC));
        //        oDbCommand.Parameters.Add(CreateParameter("@CompanyCode", DbType.String, oCustomer.CompanyCode));
        //        oDbCommand.Parameters.Add(CreateParameter("@BizLocGLN", DbType.String, oCustomer.BizLocGLN));
        //        oDbCommand.Parameters.Add(CreateParameter("@BizLocGLN_Ext", DbType.String, oCustomer.BizLocGLN_Ext));
        //        oDbCommand.Parameters.Add(CreateParameter("@stateOrRegion", DbType.Int16, oCustomer.stateOrRegion));
        //        oDbCommand.Parameters.Add(CreateParameter("@city", DbType.String, oCustomer.city));
        //        oDbCommand.Parameters.Add(CreateParameter("@postalCode", DbType.String, oCustomer.postalCode));
        //        oDbCommand.Parameters.Add(CreateParameter("@LooseExt", DbType.String, oCustomer.LoosExt));

        //        oDbCommand.Parameters.Add(CreateParameter("@PKID", DbType.Decimal, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));

        //        ExecuteNonQuery(oDbCommand);

        //        if (oDbCommand.Parameters["@PKID"].Value != DBNull.Value)
        //            return Convert.ToInt32(oDbCommand.Parameters["@PKID"].Value);
        //        else
        //            return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int SyncPackagingAsso(Models.Product.PackagingAsso oProduct)
        //{
        //    try
        //    {
        //        DbCommand oDbCommand = CreateCommand("[up_SyncPackagingAsso]", CommandType.StoredProcedure);

        //        oDbCommand.Parameters.Add(CreateParameter("@PAID", DbType.Int32, oProduct.PAID));
        //        oDbCommand.Parameters.Add(CreateParameter("@ProductName", DbType.String, oProduct.ProductName));
        //        oDbCommand.Parameters.Add(CreateParameter("@ProductCode", DbType.String, oProduct.ProductCode));
        //        oDbCommand.Parameters.Add(CreateParameter("@FGCode", DbType.String, oProduct.FGCode));
        //        oDbCommand.Parameters.Add(CreateParameter("@ProductInformation", DbType.String, oProduct.ProductInformation));
        //        oDbCommand.Parameters.Add(CreateParameter("@ProductStatus", DbType.String, oProduct.ProductStatus));
        //        oDbCommand.Parameters.Add(CreateParameter("@CreatedBy", DbType.Int32, oProduct.CreatedBy));
        //        oDbCommand.Parameters.Add(CreateParameter("@CreatedDate", DbType.DateTime, oProduct.CreatedDate));
        //        oDbCommand.Parameters.Add(CreateParameter("@VerifiedBy", DbType.Int32, oProduct.VerifiedBy));
        //        oDbCommand.Parameters.Add(CreateParameter("@VerifiedDate", DbType.DateTime, oProduct.VerifiedDate));
        //        oDbCommand.Parameters.Add(CreateParameter("@IsVerified", DbType.Boolean, oProduct.IsVerified));
        //        oDbCommand.Parameters.Add(CreateParameter("@UseDay", DbType.Boolean, 1));
        //        oDbCommand.Parameters.Add(CreateParameter("@CIGARETTEDIAMETER", DbType.String, oProduct.CIGARETTEDIAMETER));
        //        oDbCommand.Parameters.Add(CreateParameter("@CIGARETTEFLAVOR", DbType.String, oProduct.CIGARETTEFLAVOR));
        //        oDbCommand.Parameters.Add(CreateParameter("@IsHerbal", DbType.String, oProduct.IsHerbal));
        //        oDbCommand.Parameters.Add(CreateParameter("@CIGARETTELENGTH", DbType.String, oProduct.CIGARETTELENGTH));
        //        oDbCommand.Parameters.Add(CreateParameter("@NICOTINEDELIVERY", DbType.String, oProduct.NICOTINEDELIVERY));
        //        oDbCommand.Parameters.Add(CreateParameter("@TARDELIVERY", DbType.String, oProduct.TARDELIVERY));
        //        oDbCommand.Parameters.Add(CreateParameter("@FILTERTYPE", DbType.String, oProduct.FILTERTYPE));
        //        oDbCommand.Parameters.Add(CreateParameter("@LastUpdatedDate", DbType.DateTime, oProduct.LastUpdatedDate));

        //        oDbCommand.Parameters.Add(CreateParameter("@PKID", DbType.Decimal, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));

        //        ExecuteNonQuery(oDbCommand);

        //        if (oDbCommand.Parameters["@PKID"].Value != DBNull.Value)
        //            return Convert.ToInt32(oDbCommand.Parameters["@PKID"].Value);
        //        else
        //            return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int SyncPackagingAssoDetails(Models.Product.PackagingAssoDetails oPackagingAssoDetails)
        //{
        //    try
        //    {
        //        DbCommand oDbCommand = CreateCommand("[up_SyncPackagingAssoDetails]", CommandType.StoredProcedure);

        //        oDbCommand.Parameters.Add(CreateParameter("@Id", DbType.Int32, oPackagingAssoDetails.Id));
        //        oDbCommand.Parameters.Add(CreateParameter("@PAID", DbType.Int32, oPackagingAssoDetails.PAID));
        //        oDbCommand.Parameters.Add(CreateParameter("@GTIN", DbType.String, oPackagingAssoDetails.GTIN));
        //        oDbCommand.Parameters.Add(CreateParameter("@PackageTypeID", DbType.Byte, oPackagingAssoDetails.PackageTypeID));
        //        oDbCommand.Parameters.Add(CreateParameter("@Size", DbType.Int32, oPackagingAssoDetails.Size));
        //        oDbCommand.Parameters.Add(CreateParameter("@MRP", DbType.Decimal, oPackagingAssoDetails.MRP));
        //        oDbCommand.Parameters.Add(CreateParameter("@BundleQty", DbType.Byte, oPackagingAssoDetails.BundleQty));

        //        oDbCommand.Parameters.Add(CreateParameter("@PKID", DbType.Decimal, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));

        //        ExecuteNonQuery(oDbCommand);

        //        if (oDbCommand.Parameters["@PKID"].Value != DBNull.Value)
        //            return Convert.ToInt32(oDbCommand.Parameters["@PKID"].Value);
        //        else
        //            return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public bool SyncPackagingDetails(DataTable dt, List<ExecutionStatus> lstExecutionStatus)
        //{
        //    using (lineConnection)
        //    {
        //        SqlTransaction transaction = null;
        //        OpenConnection();
        //        try
        //        {
        //            transaction = lineConnection.BeginTransaction();
        //            using (var sqlBulkCopy = new SqlBulkCopy(lineConnection, SqlBulkCopyOptions.TableLock, transaction))
        //            {
        //                sqlBulkCopy.BulkCopyTimeout = 0;
        //                sqlBulkCopy.DestinationTableName = "PackagingDetails";

        //                foreach (var item in dt.Columns)
        //                {
        //                    sqlBulkCopy.ColumnMappings.Add(item.ToString(), item.ToString());
        //                }

        //                sqlBulkCopy.WriteToServer(dt);
        //            }
        //            transaction.Commit();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Rollback();
        //            lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = false, Message = "SyncPackagingDetails ERROR : " + ex.Message });
        //            return false;
        //        }
        //    }
        //}
        //public int SyncApplicator(Models.ProductApplicatorSetting.ProductApplicatorSetting oApplicator)
        //{
        //    try
        //    {
        //        DbCommand oDbCommand = CreateCommand("[up_SyncProductApplicator]", CommandType.StoredProcedure);

        //        oDbCommand.Parameters.Add(CreateParameter("@ServerPAID", DbType.Int16, oApplicator.ServerPAID));
        //        oDbCommand.Parameters.Add(CreateParameter("@S1", DbType.Decimal, oApplicator.S1));
        //        oDbCommand.Parameters.Add(CreateParameter("@S2", DbType.Decimal, oApplicator.S2));
        //        oDbCommand.Parameters.Add(CreateParameter("@S3", DbType.Decimal, oApplicator.S3));
        //        oDbCommand.Parameters.Add(CreateParameter("@S4", DbType.Decimal, oApplicator.S4));
        //        oDbCommand.Parameters.Add(CreateParameter("@S5", DbType.Decimal, oApplicator.S5));
        //        oDbCommand.Parameters.Add(CreateParameter("@S5", DbType.Decimal, oApplicator.S5));
        //        oDbCommand.Parameters.Add(CreateParameter("@S5", DbType.Decimal, oApplicator.S5));
        //        oDbCommand.Parameters.Add(CreateParameter("@FrontLabelOffset", DbType.Decimal, oApplicator.FrontLabelOffset));
        //        oDbCommand.Parameters.Add(CreateParameter("@BackLabelOffset", DbType.Decimal, oApplicator.BackLabelOffset));
        //        oDbCommand.Parameters.Add(CreateParameter("@CartonLength", DbType.Decimal, oApplicator.CartonLength));
        //        oDbCommand.Parameters.Add(CreateParameter("@FrontLabelOffset", DbType.Decimal, oApplicator.FrontLabelOffset));
        //        oDbCommand.Parameters.Add(CreateParameter("@FrontLabelOffset", DbType.Decimal, oApplicator.FrontLabelOffset));

        //        oDbCommand.Parameters.Add(CreateParameter("@PKID", DbType.Decimal, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));

        //        ExecuteNonQuery(oDbCommand);

        //        if (oDbCommand.Parameters["@PKID"].Value != DBNull.Value)
        //            return Convert.ToInt32(oDbCommand.Parameters["@PKID"].Value);
        //        else
        //            return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int SyncGlue(Models.ProductGlueSetting.ProductGlueSetting oGlue)
        //{
        //    try
        //    {
        //        DbCommand oDbCommand = CreateCommand("[up_SyncProductGlue]", CommandType.StoredProcedure);

        //        oDbCommand.Parameters.Add(CreateParameter("@ServerPAID", DbType.Int16, oGlue.ServerPAID));
        //        oDbCommand.Parameters.Add(CreateParameter("@HotGlueStartDistance", DbType.Decimal, oGlue.HotGlueStartDistance));
        //        oDbCommand.Parameters.Add(CreateParameter("@HotGlueGapDistance", DbType.Decimal, oGlue.HotGlueGapDistance));
        //        oDbCommand.Parameters.Add(CreateParameter("@HotGlueDotSize", DbType.Decimal, oGlue.HotGlueDotSize));
        //        oDbCommand.Parameters.Add(CreateParameter("@ColdGlueStartDistance", DbType.Decimal, oGlue.ColdGlueStartDistance));
        //        oDbCommand.Parameters.Add(CreateParameter("@ColdGlueGapDistance", DbType.Decimal, oGlue.ColdGlueGapDistance));
        //        oDbCommand.Parameters.Add(CreateParameter("@ColdGlueDotSize", DbType.Decimal, oGlue.ColdGlueDotSize));

        //        oDbCommand.Parameters.Add(CreateParameter("@PKID", DbType.Decimal, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));

        //        ExecuteNonQuery(oDbCommand);

        //        if (oDbCommand.Parameters["@PKID"].Value != DBNull.Value)
        //            return Convert.ToInt32(oDbCommand.Parameters["@PKID"].Value);
        //        else
        //            return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }

    public struct ExecutionStat
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}