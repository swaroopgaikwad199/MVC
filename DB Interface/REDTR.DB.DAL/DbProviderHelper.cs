using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


namespace REDTR.DB.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DbProviderHelper
    {
        private static DbProviderFactory dbProviderFactory;
        private static DbConnection dbConnection;
        private static DbConnection dbConnection1;
        //private static OracleConnection dbConnectionOra;
        //private static DbProviderFactory OracleProviderFactory;
       

        #region dbConnection
        public static DbConnection GetConnection()
        {
            if (dbConnection == null)
            {
                dbProviderFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
                dbConnection = dbProviderFactory.CreateConnection();

                REDTR.DB.Connection.CreateDbConnection dbConn = new REDTR.DB.Connection.CreateDbConnection();
                dbConnection.ConnectionString = dbConn.GetConnectionString();

                //ConnectionStringsSection connectionStringsSection = GetConnectionStringsSection();
                //dbProviderFactory = DbProviderFactories.GetFactory(connectionStringsSection.ConnectionStrings[1].ProviderName);
                //dbConnection = dbProviderFactory.CreateConnection();
                //dbConnection.ConnectionString = connectionStringsSection.ConnectionStrings[1].ConnectionString;
            }
            return dbConnection;
        }
        public static void InitialiseDBConnection()
        {
            dbConnection = null;
        }

        /// <summary>
        ///  ADDED By SUNIL FOR CONNECTION WITH LINE [Sunil 25.09.2015]
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbConnection GetConnection(string connectionString)
        {
            dbConnection1 = null;
            if (dbConnection1 == null)
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                dbConnection1 = con;
            }
            return dbConnection1;
        }
// By tushar
      
        public static DbConnection GetConnection(string providerName, string connectionString)
        {
            if (dbConnection == null)
            {
                dbProviderFactory = DbProviderFactories.GetFactory(providerName);
                dbConnection = dbProviderFactory.CreateConnection();
                dbConnection.ConnectionString = connectionString;
            }
            return dbConnection;
        }
        public static ConnectionStringsSection GetConnectionStringsSection()
        {
            return ConfigurationManager.GetSection("connectionStrings") as ConnectionStringsSection;
        }

        #endregion dbConnexion

        #region  dbCommand

        public static DbCommand CreateCommand(String commandText, CommandType commandType)
        {
            DbCommand dbCommand = dbProviderFactory.CreateCommand();
            dbCommand.Connection = dbConnection;
            dbCommand.CommandType = commandType;
            dbCommand.CommandText = commandText;
            dbCommand.CommandTimeout = 0;

            return dbCommand;
        }
        public static DbCommand CreateCommand(String commandText, CommandType commandType, DbTransaction dbTransaction)
        {
            DbCommand dbCommand = dbProviderFactory.CreateCommand();
            dbCommand.Connection = dbConnection;
            dbCommand.CommandType = commandType;
            dbCommand.CommandText = commandText;
            dbCommand.Transaction = dbTransaction;
            dbCommand.CommandTimeout = 0;
            return dbCommand;
        }
        /// <summary>
        /// FOR LINE WISE DATA TRANSFER [Sunil 26.09.2015]
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="dbCon"></param>
        /// <returns></returns>
        public static DbCommand CreateCommand(String commandText, CommandType commandType, DbConnection dbCon)
        {
            DbCommand dbCommand = dbProviderFactory.CreateCommand();
            dbCommand.Connection = dbCon;
            dbCommand.CommandType = commandType;
            dbCommand.CommandText = commandText;
            dbCommand.CommandTimeout = 0;
            return dbCommand;
        }

        #endregion  dbCommand

        #region mapping

        public static DataColumnMapping CreateDataColumnMapping(string sourceColumn, string dataSetColumn)
        {
            return new DataColumnMapping(sourceColumn, dataSetColumn);
        }
        public static DataTableMapping CreateDataTableMapping(string sourceTable, string dataSetTable, DataColumnMappingCollection dataColumnMappings)
        {
            DataTableMapping dataTableMapping = new DataTableMapping(sourceTable, dataSetTable);
            foreach (DataColumnMapping dataColumnMapping in dataColumnMappings)
                dataTableMapping.ColumnMappings.Add(dataColumnMapping);
            return dataTableMapping;
        }

        #endregion mapping

        #region dbDataAdapter

        public static DbDataAdapter CreateDataAdapter(DbCommand selectCommand)
        {
            DbDataAdapter dbDataAdapter = dbProviderFactory.CreateDataAdapter();
            dbDataAdapter.SelectCommand = selectCommand;
            return dbDataAdapter;
        }
        public static DbDataAdapter CreateDataAdapter(DbCommand selectCommand, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand)
        {
            return CreateDataAdapter(selectCommand, insertCommand, updateCommand, deleteCommand, false);
        }
        public static DbDataAdapter CreateDataAdapter(DbCommand selectCommand, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, bool continueUpdateOnError)
        {
            DbDataAdapter dbDataAdapter = dbProviderFactory.CreateDataAdapter();
            dbDataAdapter.ContinueUpdateOnError = continueUpdateOnError;
            dbDataAdapter.SelectCommand = selectCommand;
            dbDataAdapter.InsertCommand = insertCommand;
            dbDataAdapter.UpdateCommand = updateCommand;
            dbDataAdapter.DeleteCommand = deleteCommand;

            return dbDataAdapter;
        }
        #endregion dbDataAdapter

        #region dbParameter

        public static DbParameter CreateParameter(string parameterName, DbType dbType, object value)
        {
            DbParameter oDbParameter = dbProviderFactory.CreateParameter();
            oDbParameter.ParameterName = parameterName;
            oDbParameter.DbType = dbType;
            if (value != null)
                oDbParameter.Value = value;
            else
                oDbParameter.Value = DBNull.Value;

            return oDbParameter;
        }
        public static DbParameter CreateParameter(string parameterName, DbType dbType, ParameterDirection parameterDirection, string sourceColumn, DataRowVersion dataRowVersion, bool sourceColumnNullMapping, object value)
        {
            DbParameter oDbParameter = dbProviderFactory.CreateParameter();
            oDbParameter.ParameterName = parameterName;
            oDbParameter.DbType = dbType;
            oDbParameter.Direction = parameterDirection;
            oDbParameter.SourceColumn = sourceColumn;
            oDbParameter.SourceVersion = dataRowVersion;
            oDbParameter.SourceColumnNullMapping = sourceColumnNullMapping;
            if (value != null)
                oDbParameter.Value = value;
            else
                oDbParameter.Value = DBNull.Value;
            return oDbParameter;
        }

        #endregion dbParameter

        #region ManageConnection
        private static bool m_KeepConnAlive = true;

        public static bool KeepConnAlive
        {
            get { return m_KeepConnAlive; }
            set { m_KeepConnAlive = value; }
        }
        public static bool CloseConn()
        {
            if (dbConnection.State == ConnectionState.Open)
                dbConnection.Close();
            return true;
        }

        #endregion ManageConnection

        #region Operations

        public static DbDataReader ExecuteReader(DbCommand dbCommand)
        {
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            return dbCommand.ExecuteReader();
        }
        public static DbDataReader ExecuteReader(DbCommand dbCommand, CommandBehavior commandBehavior)
        {
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();
          
            return dbCommand.ExecuteReader(commandBehavior);
        }
        public static int ExecuteNonQuery(DbCommand dbCommand)
        {
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
                return dbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (KeepConnAlive == false)
                    dbConnection.Close();
            }
        }
        /// <summary>
        /// FOR LINE LEVEL DATABASE OPERATION [Sunil 26.09.2015]
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbCommand dbCommand, int x)
        {
            try
            {
                if (dbConnection1.State != ConnectionState.Open)
                    dbConnection1.Open();
                return dbCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (KeepConnAlive == false)
                    dbConnection1.Close();
            }
        }
        public static object ExecuteScalar(DbCommand dbCommand)
        {
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
                return dbCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (KeepConnAlive == false)
                    dbConnection.Close();
            }
        }
        public static DataSet FillDataSet(DbDataAdapter dbDataAdapter)
        {
            try
            {
                DataSet dataSet = new DataSet();
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
                dbDataAdapter.Fill(dataSet);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (KeepConnAlive == false)
                    dbConnection.Close();
            }
        }

        public static DataSet FillXMLDataSet(DbDataAdapter dbDataAdapter,string Nodename)
        {
            try
            {
                DataSet dataSet = new DataSet();
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
                dbDataAdapter.Fill(dataSet, Nodename);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (KeepConnAlive == false)
                    dbConnection.Close();
            }
        }
        public static DataTable FillDataTable(DbDataAdapter dbDataAdapter)
        {
            try
            {
                DataTable dataTable = new DataTable();
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                dbDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (KeepConnAlive == false)
                    dbConnection.Close();
            }
        }
        public static int UpdateDataSet(DbDataAdapter dbDataAdapter, DataSet dataSet)
        {
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
                return dbDataAdapter.Update(dataSet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (KeepConnAlive == false)
                    dbConnection.Close();
            }
        }
        public static int UpdateDataTable(DbDataAdapter dbDataAdapter, DataTable dataTable)
        {
            try
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();
                return dbDataAdapter.Update(dataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (KeepConnAlive == false)
                    dbConnection.Close();
            }
        }

        #endregion Operations

        #region dbTransaction

        public static DbTransaction BeginTransaction()
        {
            return dbConnection.BeginTransaction();
        }
        public static void CommitTransaction(DbTransaction dbTransaction)
        {
            dbTransaction.Commit();
        }
        public static void RollbackTransaction(DbTransaction dbTransaction)
        {
            dbTransaction.Rollback();
        }

        #endregion dbTransaction
    }
}

