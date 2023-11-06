using System;
using System.Collections.Generic;
using System.Text;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;
using System.Data.Common;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using PTPLCRYPTORENGINE;

namespace REDTR.DB.DAL
{
    public partial class JobDAO
    {

        public DataSet GetReportDataSet(int Flag, string Value1, string Value2,Nullable<int> AppID, Nullable<DateTime> JobFromDate, Nullable<DateTime> JobToDate,bool IsDecomission,int jobStatus)
        {
            try
            {
                DataSet ds;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[Sp_GetDataReport]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, Value1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, Value2));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobStatus", DbType.Int16, jobStatus));                
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AppID", DbType.String, AppID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobFromDate", DbType.DateTime, JobFromDate));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobToDate", DbType.DateTime, JobToDate));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsDecomissioned", DbType.Boolean, IsDecomission));

                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                ds = DbProviderHelper.FillDataSet(DbAdpt);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBatchDataSetforXML(int Flag, string Value1, string Value2,Nullable<int> AppID)
        {
            try
            {
                DataTable ds;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETJob]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Value1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, Value2));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AppID", DbType.String, AppID));
                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                ds = DbProviderHelper.FillDataTable(DbAdpt);
                return ds;
                           }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable GetBatch_ProductCode(int Flag, string Value1, string Value2, Nullable<int> AppID)
        {
            try
            {
                DataTable ds;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETJob]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Value1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, Value2));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AppID", DbType.String, AppID));
                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                ds = DbProviderHelper.FillDataTable(DbAdpt);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public int GetNoOfDecks(string SSCC)
        {
            try
            {
            int DeckSize = 0;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(" Select count(JobDetails.JD_Deckcode)from JobDetails where JobDetails.JD_JobID=(select distinct PackagingDetails.JobID from PackagingDetails where  SSCC  like  '" + SSCC + "')", CommandType.Text);
           DbProviderHelper.ExecuteScalar(oDbCommand);
         DeckSize = Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
            return DeckSize;
        }
            catch (Exception ex) { return 0; }
        }
   
    
    
    }

    public partial class UsersDAO
    {

    }

    public partial class DAVAExportDetailsDAO
    {


        public string GetProductCodeOfBatchNo(int JobId)
        {
            string GetProductCode = string.Empty;
            DbCommand oDbCommand = DbProviderHelper.CreateCommand("Select ProductCode from PackagingAsso where PAID= (Select PAID from Job where JID='" + JobId + "')", CommandType.Text);
            DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
            while (oDbDataReader.Read())
            {
                if (oDbDataReader["ProductCode"] != DBNull.Value)
                    GetProductCode = Convert.ToString(oDbDataReader["ProductCode"]);

            }
            if (!string.IsNullOrEmpty(GetProductCode))
                return GetProductCode;
            else
                return GetProductCode = "";
        }



        public int GetjobIdOfBatchNo(string BatchNo)
        {
            int GetJobID = 0;
            DbCommand oDbCommand = DbProviderHelper.CreateCommand("Select JID from Job where BatchNo='" + BatchNo + "'", CommandType.Text);
            DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
            while (oDbDataReader.Read())
            {
                if (oDbDataReader["JID"] != DBNull.Value)
                    GetJobID = Convert.ToInt32(oDbDataReader["JID"]);

            }
            if ((GetJobID != 0))
                return GetJobID;
            else
                return GetJobID = 0;
        }

    }
    public partial class RolesDAO
    {

    }
    public partial class PermissionsDAO
    {

    }
    public partial class RolesPermissionsDAO
    {
    }
    public partial class PackagingAssoDAO
    {
        public DataTable GetProductdataTableforXML(int Flag, string Value1, string Value2)
        {
            try
            {
                DataTable ds;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingAsso]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Value1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, Value2));
                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                ds = DbProviderHelper.FillDataTable(DbAdpt);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetProductMasterData(int PAID)
        {
            DataTable dt;
            DbCommand oDbCommand = DbProviderHelper.CreateCommand(" Select [Manufacturer],[Name] as [DrugName],[DosageForm],[Strength],[ContainerSize] from PackagingAsso where PAID=" + PAID, CommandType.Text);
            DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
            dt = DbProviderHelper.FillDataTable(DbAdpt);
            return dt;
        }


    }

    //public partial class PackagingAssoDetailsDAO
    //{
        

    //}

    public partial class JobDAO
    {
        public DataTable GetJobMasterData(int JOBId)
        {
            DataTable dt;
            DbCommand oDbCommand = DbProviderHelper.CreateCommand(" Select [BatchNo],[MfgDate],[ExpDate],[Quantity] from Job where JID=" + JOBId, CommandType.Text);
            DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
            dt = DbProviderHelper.FillDataTable(DbAdpt);
            return dt;
        }

    }

    //public partial class SettingsDAO
    //{
    //    //public DataTable GetSenderDetails(int Id)
    //    //{
    //    //    DataTable dt;
    //    //    DbCommand oDbCommand = DbProviderHelper.CreateCommand(" Select s.CompanyName, s.Street, st.StateName, s.city, s.postalCode, c.CountryName, s.License, s.LicenseState, s.LicenseAgency, s.GLN from dbo.Settings s, country c, S_State st where s.country = c.id, s.stateOrRegion = st.ID and s.id=" + Id, CommandType.Text);
    //    //    DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
    //    //    dt = DbProviderHelper.FillDataTable(DbAdpt);
    //    //    return dt;
    //    //}
    //}
    public partial class SSCCLineHolderDAO
    {

      
    }
    public partial class UserTrailDAO
    {

    }
    public partial class PackagingDetailsDAO
    {
        public DataTable GetPrimaryCountofScannedSSCC(int Flag, string Value1, string Value2, string Value3)
        {
            try
            {
                DataTable ds;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, Value1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, Value2));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, Value3));
                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                ds = DbProviderHelper.FillDataTable(DbAdpt);
                return ds;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public DataSet GetProductionDataSetforTSPXML(int Flag, string Value1, string Value2,string Value3)
        {
            try
            {
                DataSet ds;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, Value1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, Value2));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, Value3));
               // oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AppID", DbType.String, AppID));
                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                ds = DbProviderHelper.FillXMLDataSet(DbAdpt, "ProductionInfo");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetSSCCValid(string SSCC)
        {
            object obj;
            DbCommand oDbCommand = DbProviderHelper.CreateCommand(" select COUNT(*) from PackagingDetails where SSCC like'" + SSCC + "' and (DavaPortalUpload=0 or DavaPortalUpload is null) ", CommandType.Text);
            obj = DbProviderHelper.ExecuteScalar(oDbCommand);
            int i= int.Parse(obj.ToString());
            if (i > 0)
            { return true; }
            else
            { return false; }
        }



        public bool GEtUIDStatus(int Flag, string JOBID, string Code)
        {
            try
            {
                string Status = string.Empty;
                DataSet ds = new DataSet();

                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, (int)Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, JOBID));
                //if (Code != null)
                //{
                //    Code = AESCryptor.Encrypt(Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //}
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, Code));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, DBNull.Value));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["Status"] != DBNull.Value)
                        Status = Convert.ToString(oDbDataReader["Status"]);
                  
                }
                if (!string.IsNullOrEmpty(Status))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetUIDManualstatus(int Flag, string JOBID, string Code, string UID)
        {
            try
            {
                string Status = string.Empty;
                DataSet ds = new DataSet();

                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, (int)Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, JOBID));              
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, Code));
                //if (UID != null)
               // {
                    //UID = AESCryptor.Encrypt(UID, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    
                //}
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, UID));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["Status"] != DBNull.Value)
                        Status += Convert.ToString(oDbDataReader["Status"])+",";
                
                }
                if (Status != "")
                {
                    Status = Status.Substring(0, Status.Length - 1);
                }
                return Status;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // By Tushar
        public List<string> GEtBadUIDReason(int Flag, string JOBID, string Code, string UID)
        {
            try
            {
                string Status = string.Empty;
                List<string> FailedID = new List<string>();

                DataSet ds = new DataSet();

                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, (int)Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, JOBID));
               
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, Code));
                //if (UID != null)
                //{
                //    UID = AESCryptor.Encrypt(UID, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //}
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, UID));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["Status"] != DBNull.Value)
                        Status = Convert.ToString(oDbDataReader["Status"]);
                    if (oDbDataReader["FailedID"] != DBNull.Value)
                        FailedID.Add(Convert.ToString(oDbDataReader["FailedID"]));

                }
                //if (!string.IsNullOrEmpty(Status))
                //    return true;
                //else
                return FailedID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string GetIncompleteDeckUid(int Flag, decimal JobId, string childDeck, string ParentDeck, int parentDeckPcMapSize, out int QtyAdded, string defaultUidNotInclude)
        {
            string incompleteCode=string.Empty;
            QtyAdded = 0;
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails_ForLoose]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.String, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, JobId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, childDeck));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, ParentDeck));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value4", DbType.String, parentDeckPcMapSize));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["NextLevelCode"] != DBNull.Value)
                    {
                        string code = Convert.ToString(oDbDataReader["NextLevelCode"]);
                        //code = AESCryptor.Decrypt(code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                        if (code == defaultUidNotInclude || code == string.Empty)
                            continue;
                    }
                    incompleteCode = Convert.ToString(oDbDataReader["NextLevelCode"]);
                    //incompleteCode = AESCryptor.Decrypt(incompleteCode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    QtyAdded = Convert.ToInt32(oDbDataReader["CNT"]);
                    break;
                }
                
                oDbDataReader.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return incompleteCode;
        }
    }
}
