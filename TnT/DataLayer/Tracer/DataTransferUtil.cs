using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TnT.DataLayer.Tracer
{
    public class DataTransferUtil
    {
        DataHelper hlp = new DataHelper();


        public bool TransferBatch(decimal Jid)
        {
            DataHelper hlp = new DataHelper();
            var ds = hlp.getBatchData(Jid);

            //ds.Tables[""].Columns[""].Expression = "1";
            //ds.Tables[""].Columns[""].Expression = "1";
            //ds.Tables[""].Columns[""].Expression = "1";
            //ds.Tables[""].Columns[""].Expression = "1";
            ds.Tables["job"].Columns["CreatedBy"].Expression = "1";
            ds.Tables["job"].Columns["VerifiedBy"].Expression = "1";

            BatchTransfer trnsfr = new BatchTransfer();
            return trnsfr.transfer(ds);
        }
    }

    public class BatchTransfer
    {
        private string destinationCon = string.Empty;
        private SqlConnection connection = null;
        private DataSet DSmstr;
        private SqlTransaction transaction = null;

        public BatchTransfer()
        {
            destinationCon = "Data Source=tnttraceability.cgc4wrtd1ilz.us-east-1.rds.amazonaws.com,1433;Initial Catalog= trackntrace;Integrated Security=false; user id =propix_tnt; password=ashWIN5001; MultipleActiveResultSets=true;";
        }

        public bool transfer(DataSet Ds)
        {
            DSmstr = Ds;
            using (connection = new SqlConnection(destinationCon))
            {
              
                connection.Open();
                try
                {
                    transaction = connection.BeginTransaction();

                    if (insertPackagingAsso())
                    {
                        if (insertPackagingAssoDetails())
                        {
                            //if (insertPackageLblMstr())
                            //{
                            if (insertJob())
                            {
                                //if (insertJobDetails())
                                //{
                                    if (insertPackagingDetails())
                                    {
                                        transaction.Commit();
                                        return true;
                                    }
                                //}
                            }
                            //}
                        }
                    }
                    transaction.Rollback();
                    return false;

                    //var stsPA = insertPackagingAsso(connection, Ds.Tables["packagingAsso"], transaction);
                    //var stsPAD = insertPackagingAssoDetails(connection, Ds.Tables["packagingAssoDetails"], transaction);
                    //var stsPLM = insertPackageLblMstr(connection, Ds.Tables["packageLabelMaster"], transaction);
                    //var stsJob = insertJob(connection, Ds.Tables["job"], transaction);
                    //var stsJD = insertJobDetails(connection, Ds.Tables["jobDetails"], transaction);
                    //var stsPD = insertPackagingDetails(connection, Ds.Tables["packagingDetails"], transaction);


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }

            }
        }


        private bool insertPackagingAsso()
        {
            try
            {
                using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                {
                    sqlBulkCopy.DestinationTableName = "PackagingAsso";
                    sqlBulkCopy.ColumnMappings.Add("PAID", "PAID");
                    sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                    sqlBulkCopy.ColumnMappings.Add("ProductCode", "ProductCode");
                    sqlBulkCopy.ColumnMappings.Add("Description", "Description");
                    sqlBulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                    sqlBulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                    sqlBulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                    sqlBulkCopy.ColumnMappings.Add("LastUpdatedDate", "LastUpdatedDate");
                    sqlBulkCopy.WriteToServer(DSmstr.Tables["packagingAsso"]);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool insertPackagingAssoDetails()
        {
            try
            {
                using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                {
                    sqlBulkCopy.DestinationTableName = "PackagingAssoDetails";
                    sqlBulkCopy.ColumnMappings.Add("Id", "ID");
                    sqlBulkCopy.ColumnMappings.Add("PAID", "PAID");
                    sqlBulkCopy.ColumnMappings.Add("PackageTypeCode", "PackageTypeCode");
                    sqlBulkCopy.ColumnMappings.Add("Size", "Size");
                    sqlBulkCopy.ColumnMappings.Add("GTIN", "GTIN");
                    sqlBulkCopy.ColumnMappings.Add("MRP", "MRP");
                    sqlBulkCopy.ColumnMappings.Add("TerCaseIndex", "TerCaseIndex");
                    sqlBulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                    sqlBulkCopy.ColumnMappings.Add("LastUpdatedDate", "LastUpdatedDate");
                    sqlBulkCopy.WriteToServer(DSmstr.Tables["packagingAssoDetails"]);
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        private bool insertPackageLblMstr()
        {
            try
            {
                using (connection)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        private bool insertJob()
        {
            try
            {
                using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                {
                    sqlBulkCopy.DestinationTableName = "Job";
                    sqlBulkCopy.ColumnMappings.Add("JID", "JID");
                    sqlBulkCopy.ColumnMappings.Add("JobName", "JobName");
                    sqlBulkCopy.ColumnMappings.Add("PAID", "PAID");
                    sqlBulkCopy.ColumnMappings.Add("BatchNo", "BatchNo");
                    sqlBulkCopy.ColumnMappings.Add("MfgDate", "MfgDate");
                    sqlBulkCopy.ColumnMappings.Add("ExpDate", "ExpDate");
                    sqlBulkCopy.ColumnMappings.Add("Quantity", "Quantity");
                    sqlBulkCopy.ColumnMappings.Add("SurPlusQty", "SurPlusQty");
                    sqlBulkCopy.ColumnMappings.Add("JobStatus", "JobStatus");
                    sqlBulkCopy.ColumnMappings.Add("DetailInfo", "DetailInfo");
                    sqlBulkCopy.ColumnMappings.Add("JobStartTime", "JobStartTime");
                    sqlBulkCopy.ColumnMappings.Add("JobEndTime", "JobEndTime");
                    sqlBulkCopy.ColumnMappings.Add("LabelStartIndex", "LabelStartIndex");
                    sqlBulkCopy.ColumnMappings.Add("AutomaticBatchCloser", "AutomaticBatchCloser");
                    sqlBulkCopy.ColumnMappings.Add("TID", "TID");
                    sqlBulkCopy.ColumnMappings.Add("MLNO", "MLNO");
                    sqlBulkCopy.ColumnMappings.Add("TenderText", "TenderText");
                    sqlBulkCopy.ColumnMappings.Add("JobWithUID", "JobWithUID");
                    sqlBulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                    sqlBulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                    sqlBulkCopy.ColumnMappings.Add("VerifiedBy", "VerifiedBy");
                    sqlBulkCopy.ColumnMappings.Add("VerifiedDate", "VerifiedDate");
                    sqlBulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                    sqlBulkCopy.ColumnMappings.Add("LastUpdatedDate", "LastUpdatedDate");
                    sqlBulkCopy.ColumnMappings.Add("AppId", "AppId");
                    sqlBulkCopy.WriteToServer(DSmstr.Tables["Job"]);
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        private bool insertJobDetails()
        {
            try
            {
                using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                {
                    sqlBulkCopy.DestinationTableName = "JobDetails";
                    sqlBulkCopy.ColumnMappings.Add("Id", "ID");
                    sqlBulkCopy.ColumnMappings.Add("JD_JobID", "JD_JobID");
                    sqlBulkCopy.ColumnMappings.Add("JD_ProdName", "JD_ProdName");
                    sqlBulkCopy.ColumnMappings.Add("JD_ProdCode", "JD_ProdCode");
                    sqlBulkCopy.ColumnMappings.Add("JD_Deckcode", "JD_Deckcode");
                    sqlBulkCopy.ColumnMappings.Add("JD_GTIN", "JD_GTIN");
                    sqlBulkCopy.ColumnMappings.Add("JD_DeckSize", "JD_DeckSize");
                    sqlBulkCopy.ColumnMappings.Add("JD_MRP", "JD_MRP");
                    sqlBulkCopy.ColumnMappings.Add("JD_Description", "JD_Description");
                    sqlBulkCopy.ColumnMappings.Add("LastUpdatedDate", "LastUpdatedDate");
                    sqlBulkCopy.WriteToServer(DSmstr.Tables["jobDetails"]);
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        private bool insertPackagingDetails()
        {
            try
            {

                using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                {
                    sqlBulkCopy.DestinationTableName = "PackagingDetails";
                    sqlBulkCopy.ColumnMappings.Add("Code", "Code");
                    sqlBulkCopy.ColumnMappings.Add("PAID", "PAID");
                    sqlBulkCopy.ColumnMappings.Add("JobID", "JobID");
                    sqlBulkCopy.ColumnMappings.Add("PackageTypeCode", "PackageTypeCode");
                    sqlBulkCopy.ColumnMappings.Add("MfgPackDate", "MfgPackDate");
                    sqlBulkCopy.ColumnMappings.Add("ExpPackDate", "ExpPackDate");
                    sqlBulkCopy.ColumnMappings.Add("NextLevelCode", "NextLevelCode");
                    sqlBulkCopy.ColumnMappings.Add("IsRejected", "IsRejected");         
                    sqlBulkCopy.ColumnMappings.Add("Reason", "Reason");
                    sqlBulkCopy.ColumnMappings.Add("BadImage", "BadImage");
                    sqlBulkCopy.ColumnMappings.Add("SSCC", "SSCC");
                    sqlBulkCopy.ColumnMappings.Add("SSCCVarificationStatus", "SSCCVarificationStatus");
                    sqlBulkCopy.ColumnMappings.Add("IsManualUpdated", "IsManualUpdated");//ManualUpdateDesc
                    sqlBulkCopy.ColumnMappings.Add("ManualUpdateDesc", "ManualUpdateDesc");
                    sqlBulkCopy.ColumnMappings.Add("CaseSeqNum", "CaseSeqNum");
                    sqlBulkCopy.ColumnMappings.Add("OperatorId", "OperatorId");
                    sqlBulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                    sqlBulkCopy.ColumnMappings.Add("IsDecomission", "IsDecomission");
                    sqlBulkCopy.ColumnMappings.Add("CreatedDate", "CreatedDate");
                    sqlBulkCopy.ColumnMappings.Add("LastUpdatedDate", "LastUpdatedDate");
                    sqlBulkCopy.WriteToServer(DSmstr.Tables["packagingDetails"]);
                    return true;
                }


            }
            catch (Exception ex)
            {
                return false;
            }
        }




    }




}
