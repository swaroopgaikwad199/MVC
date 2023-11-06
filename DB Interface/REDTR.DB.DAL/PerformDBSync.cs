using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;
using System.Data.SqlClient;

namespace REDTR.DB.DAL
{
    public partial class PerformDBSync
    {
        DbConnection sqldbCon;
        public PerformDBSync()
        {
            //sqldbCon = DbProviderHelper.GetConnection("System.Data.SqlClient", ConnectionString);
        }
        public void OpenConnection()
        {
            sqldbCon = DbProviderHelper.GetConnection(ConnectionString);
        }
        public bool IsOpen()
        {
            if (sqldbCon.State == ConnectionState.Open)
                return true;
            else if (sqldbCon.State == ConnectionState.Closed)
                return false;
            else
                return false;
        }

        public void CloseDbConnection()
        {
        }

        private string _ConnectionString;
        public string ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }
        public List<Job> GetJobs(int Flag)
        {
            try
            {
                List<Job> lstJobs = new List<Job>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETJob]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.CommandTimeout = 0;
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AppID", DbType.Int16, 1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, null));
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


        public List<ProductApplicatorSetting> GetProductAppicatorSetting(int Flag)
        {
            try
            {
                List<ProductApplicatorSetting> lstProd = new List<ProductApplicatorSetting>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GetProductApplicatorSetting", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    ProductApplicatorSetting pro = new ProductApplicatorSetting();
                    pro.ServerPAID = Convert.ToDecimal(oDbDataReader["ServerPAID"]);
                    if (oDbDataReader["S1"] != DBNull.Value)
                        pro.S1 = Convert.ToSingle(oDbDataReader["S1"]);
                    if (oDbDataReader["S2"] != DBNull.Value)
                        pro.S2 = Convert.ToSingle(oDbDataReader["S2"]);
                    if (oDbDataReader["S3"] != DBNull.Value)
                        pro.S3 = Convert.ToSingle(oDbDataReader["S3"]);
                    if (oDbDataReader["S4"] != DBNull.Value)
                        pro.S4 = Convert.ToSingle(oDbDataReader["S4"]);
                    if (oDbDataReader["S5"] != DBNull.Value)
                        pro.S5 = Convert.ToSingle(oDbDataReader["S5"]);
                    if (oDbDataReader["FrontLabelOffset"] != DBNull.Value)
                        pro.FrontLabelOffset = Convert.ToSingle(oDbDataReader["FrontLabelOffset"]);
                    if (oDbDataReader["BackLabelOffset"] != DBNull.Value)
                        pro.BackLabelOffset = Convert.ToSingle(oDbDataReader["BackLabelOffset"]);
                    if (oDbDataReader["CartonLength"] != DBNull.Value)
                        pro.CartonLength = Convert.ToSingle(oDbDataReader["CartonLength"]);

                    lstProd.Add(pro);
                }
                oDbDataReader.Close();
                return lstProd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ProductGlueSetting GETProductGlueSetting(decimal Flag)
        {
            ProductGlueSetting pro = new ProductGlueSetting();
            DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GETProductGlueSetting", CommandType.StoredProcedure, sqldbCon);
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag ", DbType.Decimal, Flag));

            DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
            while (oDbDataReader.Read())
            {
                pro.HotGlueDotSize = Convert.ToSingle(oDbDataReader["HotGlueDotSize"]);
                pro.HotGlueGapDistance = Convert.ToSingle(oDbDataReader["HotGlueGapDistance"]);
                pro.HotGlueStartDistance = Convert.ToSingle(oDbDataReader["HotGlueStartDistance"]);
                pro.ColdGlueDotSize = Convert.ToSingle(oDbDataReader["ColdGlueDotSize"]);
                pro.ColdGlueGapDistance = Convert.ToSingle(oDbDataReader["ColdGlueGapDistance"]);
                pro.ColdGlueStartDistance = Convert.ToSingle(oDbDataReader["ColdGlueStartDistance"]);
            }
            oDbDataReader.Close();
            return pro;
        }

        public int InsertOrUpdateProductGlueSetting(ProductGlueSetting pro)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_insertorupdateProductGlueSettingForDataSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ServerPAID", DbType.Decimal, pro.ServerPAID));

                if (pro.HotGlueDotSize != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HotGlueDotSize", DbType.Decimal, pro.HotGlueDotSize));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HotGlueDotSize", DbType.Decimal, DBNull.Value));
                }

                if (pro.HotGlueGapDistance != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HotGlueGapDistance", DbType.Decimal, pro.HotGlueGapDistance));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HotGlueGapDistance", DbType.Decimal, DBNull.Value));
                }

                if (pro.HotGlueStartDistance != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HotGlueStartDistance", DbType.Decimal, pro.HotGlueStartDistance));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HotGlueStartDistance", DbType.Decimal, DBNull.Value));
                }

                if (pro.ColdGlueDotSize != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ColdGlueDotSize", DbType.Decimal, pro.ColdGlueDotSize));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ColdGlueDotSize", DbType.Decimal, DBNull.Value));
                }

                if (pro.ColdGlueGapDistance != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ColdGlueGapDistance", DbType.Decimal, pro.ColdGlueGapDistance));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ColdGlueGapDistance", DbType.Decimal, DBNull.Value));
                }

                if (pro.ColdGlueStartDistance != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ColdGlueStartDistance", DbType.Decimal, pro.ColdGlueStartDistance));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ColdGlueStartDistance", DbType.Decimal, DBNull.Value));
                }

                return DbProviderHelper.ExecuteNonQuery(oDbCommand);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Insert/Update Packaging Association
        public int InsertOrUpdatePackagingAssoForSync(int Flag, PackagingAsso oPackagingAsso)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdatePackagingAssoForDataSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Name", DbType.String, oPackagingAsso.Name));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductCode", DbType.String, oPackagingAsso.ProductCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Int32, oPackagingAsso.PAID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FGCode", DbType.String, oPackagingAsso.FGCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Description", DbType.String, oPackagingAsso.Description));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oPackagingAsso.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate", DbType.DateTime, DateTime.Now));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsActive", DbType.Boolean, oPackagingAsso.IsActive));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DAVAPortalUpload", DbType.Boolean, oPackagingAsso.DAVAPortalUpload));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ScheduledDrug", DbType.Boolean, oPackagingAsso.ScheduledDrug));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DoseUsage", DbType.String, oPackagingAsso.DoseUsage));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@GenericName", DbType.String, oPackagingAsso.GenericName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Composition", DbType.String, oPackagingAsso.Composition));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductImage", DbType.String, oPackagingAsso.ProductImage));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@VerifyProd", DbType.Boolean, oPackagingAsso.VerifyProd));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@InternalMaterialCode", DbType.String, oPackagingAsso.InternalMaterialCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountryDrugCode", DbType.String, oPackagingAsso.CountryDrugCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FEACN", DbType.String, oPackagingAsso.FEACN));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@NHRN", DbType.String, oPackagingAsso.NHRN));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PublicKey", DbType.String, oPackagingAsso.PublicKey));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CompType", DbType.String, oPackagingAsso.CompType));
                if (oPackagingAsso.SaudiDrugCode != "")
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SaudiDrugCode", DbType.String, oPackagingAsso.SaudiDrugCode));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SaudiDrugCode", DbType.String, DBNull.Value));
                }
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RetID", DbType.Int64, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
                DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);
                return Convert.ToInt32(oDbCommand.Parameters["@RetID"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdatePackagingAssoForSyncPushToGlobal(int Flag, PackagingAsso oPackagingAsso)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdatePackagingAssoForDataSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Name", DbType.String, oPackagingAsso.Name));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductCode", DbType.String, oPackagingAsso.ProductCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Int32, oPackagingAsso.PAID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FGCode", DbType.String, oPackagingAsso.FGCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Description", DbType.String, oPackagingAsso.Description));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oPackagingAsso.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate", DbType.DateTime, DateTime.Now));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsActive", DbType.Boolean, oPackagingAsso.IsActive));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DAVAPortalUpload", DbType.Boolean, oPackagingAsso.DAVAPortalUpload));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ScheduledDrug", DbType.Boolean, oPackagingAsso.ScheduledDrug));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DoseUsage", DbType.String, oPackagingAsso.DoseUsage));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@GenericName", DbType.String, oPackagingAsso.GenericName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Composition", DbType.String, oPackagingAsso.Composition));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductImage", DbType.String, oPackagingAsso.ProductImage));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@VerifyProd", DbType.Boolean, oPackagingAsso.VerifyProd));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@InternalMaterialCode", DbType.String, oPackagingAsso.InternalMaterialCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountryDrugCode", DbType.String, oPackagingAsso.CountryDrugCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FEACN", DbType.String, oPackagingAsso.FEACN));
                if (oPackagingAsso.SaudiDrugCode != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SaudiDrugCode", DbType.String, oPackagingAsso.SaudiDrugCode));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SaudiDrugCode", DbType.Int32, DBNull.Value));
                }
                if (oPackagingAsso.ProviderId != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProviderId", DbType.Int32, oPackagingAsso.ProviderId));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProviderId", DbType.Int32, DBNull.Value));
                }
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RetID", DbType.Int64, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
                DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);
                return Convert.ToInt32(oDbCommand.Parameters["@RetID"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Insert/Update Packaging Association Detail
        public int InsertOrUpdatePckAssoDetailsForSync(int Flag, PackagingAssoDetails oPackagingAssoDetails, decimal PAID)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdatePackagingAssoDetailsForDataSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Decimal, PAID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackageTypeCode", DbType.String, oPackagingAssoDetails.PackageTypeCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Size", DbType.Int32, oPackagingAssoDetails.Size));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BundleQty", DbType.Int32, oPackagingAssoDetails.BundleQty));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PPN", DbType.String, oPackagingAssoDetails.PPN));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@GTIN", DbType.String, oPackagingAssoDetails.GTIN));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@GTINCTI", DbType.String, oPackagingAssoDetails.GTINCTI));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MRP", DbType.Decimal, oPackagingAssoDetails.MRP));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TerCaseIndex", DbType.Int32, oPackagingAssoDetails.TerCaseIndex));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oPackagingAssoDetails.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@NTIN", DbType.String, oPackagingAssoDetails.NTIN));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        #endregion

        #region Insert/Update PackagingLabel
        public int InsertOrUpdatePckLabelForSync(int Flag, PackageLabelAsso oPackaginglabel, decimal PAID)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdatePackageLabelForDataSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Int32, PAID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobTypeID", DbType.Int32, oPackaginglabel.JobTypeID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Code", DbType.String, oPackaginglabel.Code));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LabelName", DbType.String, oPackaginglabel.LabelName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Filter", DbType.String, oPackaginglabel.Filter));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate", DbType.DateTime, DateTime.Now));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion


        #region Insert/Update Job
        public int InsertOrUpdateJobForSync(int Flag, Job oJob, string ProductName, string CreatedByName, string VerifiedByName)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateJobForDataSync]", CommandType.StoredProcedure, sqldbCon);
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
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobStatus", DbType.Int16, oJob.JobStatus));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DetailInfo", DbType.String, oJob.DetailInfo));

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
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductName", DbType.String, ProductName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedByName", DbType.String, CreatedByName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@VerifiedByName", DbType.String, VerifiedByName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@NoReadCount", DbType.Int32, oJob.NoReadCount));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UseExpDay", DbType.Boolean, oJob.UseExpDay));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExpDateFormat", DbType.String, oJob.ExpDateFormat));
                //NoReadCount
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, oJob.LineCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Decimal, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackagingLvlId", DbType.Int32, oJob.PackagingLvlId));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CustomerId", DbType.Int32, oJob.CustomerId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProviderId", DbType.Int32, oJob.ProviderId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PPNCountryCode", DbType.String, oJob.PPNCountryCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PPNPostalCode", DbType.String, oJob.PPNPostalCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CompType", DbType.String, oJob.CompType));
                DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);

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
        #endregion

        #region Insert/Update Job Type
        public int InsertOrUpdateJobTypeForSync(JOBType oJobType)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateJOBTypeForSync]", CommandType.StoredProcedure, sqldbCon);
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int64, oJobType.TID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JOBType", DbType.String, oJobType.Job_Type));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Action", DbType.Xml, oJobType.Action));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int64, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
                DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);

                if (oDbCommand.Parameters["@ID"].Value != null)
                    return Convert.ToInt32(oDbCommand.Parameters["@ID"].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Insert/Update Job Type

        #region Insert/Update Job Detail

        public int InsertOrUpdateREC_CountForSync(int Flag, int jobid, int BCnt, int LCnt, int WCnt, int SCnt, int MTCnt, int MVCnt, int MLPCnt, int MCBCnt)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateJobCountForSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Decimal, jobid));

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

        public int InsertOrUpdateJobDetailsForSync(int flag, JobDetails oJobDetails, decimal JID)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateJobDetailsForDataSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_JobID", DbType.Decimal, JID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_ProdName", DbType.String, oJobDetails.JD_ProdName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_ProdCode", DbType.String, oJobDetails.JD_ProdCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_FGCode", DbType.String, oJobDetails.JD_FGCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_Deckcode", DbType.String, oJobDetails.JD_Deckcode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_PPN", DbType.String, oJobDetails.JD_PPN));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_GTIN", DbType.String, oJobDetails.JD_GTIN));
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
                if (oJobDetails.LabelName != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LabelName", DbType.String, oJobDetails.LabelName));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LabelName", DbType.String, DBNull.Value));
                if (oJobDetails.Filter != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Filter", DbType.String, oJobDetails.Filter));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Filter", DbType.String, DBNull.Value));

                if (oJobDetails.JD_NTIN != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_NTIN", DbType.String, oJobDetails.JD_NTIN));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JD_NTIN", DbType.String, DBNull.Value));

                return DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateLineLocationForDataSync(LineLocation oLineLocation)
        {
            DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_insertorupdateLinelocationForDataSync]", CommandType.StoredProcedure, sqldbCon);
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.String, oLineLocation.ID));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LocationCode", DbType.String, oLineLocation.LocationCode));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DivisionCode", DbType.String, oLineLocation.DivisionCode));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PlantCode", DbType.String, oLineLocation.PlantCode));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, oLineLocation.LineCode));
            if (oLineLocation.LineIP != null)
            {
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineIP", DbType.String, oLineLocation.LineIP));
            }
            else
            {
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineIP", DbType.String, DBNull.Value));
            }

            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DBName", DbType.String, oLineLocation.DBName));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsActive", DbType.Boolean, oLineLocation.IsActive));
            if (oLineLocation.ServerName != null)
            {
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ServerName", DbType.String, oLineLocation.ServerName));
            }
            else
            {
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ServerName", DbType.String, DBNull.Value));
            }
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SQLPassword", DbType.String, oLineLocation.SQLPassword));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SQLUsername", DbType.String, oLineLocation.SQLUsername));
            if (oLineLocation.LineName != null)
            {
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineName", DbType.String, oLineLocation.LineName));
            }
            else
            {
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineName", DbType.String, DBNull.Value));
            }

            if (oLineLocation.ReadGLN != null)
            {
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ReadGLN", DbType.String, oLineLocation.ReadGLN));
            }
            else
            {
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ReadGLN", DbType.String, DBNull.Value));
            }
            if (oLineLocation.GLNExtension != null)
            {
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@GLNExtension", DbType.String, oLineLocation.GLNExtension));
            }
            else
            {
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@GLNExtension", DbType.String, DBNull.Value));
            }
            return DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);
        }
        #endregion

        #region Insert/Update Packaging Details
        public int InsertOrUpdatePackagingDetailsForSync(int Flag, PackagingDetails oPackagingDetails, decimal JID, string ProdName, string UserName)
        {
            try
            {
                Nullable<bool> b = oPackagingDetails.IsRejected;
                //foreach (PackagingDetails oPackagingDetails in oLstPackagingDetails)
                //{
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdatePackagingDetailsForSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackDtlsID", DbType.Decimal, oPackagingDetails.PackDtlsID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@code", DbType.String, oPackagingDetails.Code));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@pAID", DbType.Decimal, oPackagingDetails.PAID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@jobId", DbType.Decimal, JID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@pkgTypeCode", DbType.String, oPackagingDetails.PackageTypeCode));
                if (oPackagingDetails.MfgPackDate != System.DateTime.MinValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@mfgPackDate", DbType.DateTime, oPackagingDetails.MfgPackDate));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@mfgPackDate", DbType.DateTime, DBNull.Value));

                if (oPackagingDetails.ExpPackDate != System.DateTime.MinValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@expPackDate", DbType.DateTime, oPackagingDetails.ExpPackDate));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@expPackDate", DbType.DateTime, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@nextLevelCode", DbType.String, oPackagingDetails.NextLevelCode));

                if (oPackagingDetails.IsRejected != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@isRejected", DbType.Boolean, oPackagingDetails.IsRejected)); // Modified on [27.11.2015]
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@isRejected", DbType.Boolean, b));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Reason", DbType.Xml, oPackagingDetails.Reason));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Reason", DbType.String, oPackagingDetails.Reason));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@badImage", DbType.Binary, oPackagingDetails.BadImage));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SSCC", DbType.String, oPackagingDetails.SSCC));

                if (oPackagingDetails.SSCCVarificationStatus.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SSCCVerificationStatus", DbType.Boolean, oPackagingDetails.SSCCVarificationStatus));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SSCCVerificationStatus", DbType.Boolean, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsManualUpdated", DbType.String, oPackagingDetails.IsManualUpdated));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ManualUpdateDesc", DbType.Xml, oPackagingDetails.ManualUpdateDesc));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CaseSeqNum", DbType.Decimal, oPackagingDetails.CaseSeqNum));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@OperatorId", DbType.Decimal, oPackagingDetails.OperatorId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oPackagingDetails.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsDecomission", DbType.Boolean, oPackagingDetails.IsDecomission));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RCResult", DbType.Int32, oPackagingDetails.RCResult)); // Newly Added for batch transfer.
                if (oPackagingDetails.LineCode != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, oPackagingDetails.LineCode));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DAVAPortalUpload", DbType.Boolean, oPackagingDetails.DAVAPortalUpload));

                if (ProdName != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductName", DbType.String, ProdName));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductName", DbType.String, DBNull.Value));
                if (UserName != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CurrentUser", DbType.String, UserName));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CurrentUser", DbType.String, DBNull.Value));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);
                //}
                //return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Insert/Update UID Store Table
        public int InsertOrUpdateSupplierUIDsInStoreForSync(int flag, Supplier oSupplier)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateInStore]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, oSupplier.ID1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String, oSupplier.UIDs1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.Boolean, oSupplier.SFlag1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SDate", DbType.DateTime, null));

                return DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public int InsertOrUpdateCustomerForDataSync(int Flag, M_Customer cust)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateCustomerForDataSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Id", DbType.Int16, cust.Id));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CompanyName", DbType.String, cust.CompanyName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ContactPerson", DbType.String, cust.ContactPerson));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ContactNo", DbType.String, cust.ContactNo));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Email", DbType.String, cust.Email));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Address", DbType.String, cust.Address));
                if (cust.Country.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Country", DbType.Int32, cust.Country));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Country", DbType.Int32, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsActive", DbType.Boolean, cust.IsActive));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@APIKey", DbType.String, cust.APIKey));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SenderId", DbType.String, cust.SenderId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ReceiverId", DbType.String, cust.ReceiverId));
                if (cust.CreatedOn != System.DateTime.MinValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedOn", DbType.DateTime, cust.CreatedOn));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedOn", DbType.DateTime, DBNull.Value));
                if (cust.LastModified != System.DateTime.MinValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastModified", DbType.DateTime, cust.LastModified));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastModified", DbType.DateTime, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedBy", DbType.Int32, cust.CreatedBy));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ModifiedBy", DbType.Int32, cust.ModifiedBy));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsDeleted", DbType.Boolean, cust.IsDeleted));
                if (cust.APIUrl != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@APIUrl", DbType.String, cust.APIUrl)); // Modified on [27.11.2015]
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@APIUrl", DbType.String, DBNull.Value));

                if (cust.ProviderId.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProviderId", DbType.Int32, cust.ProviderId));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProviderId", DbType.Int32, DBNull.Value));

                if (cust.IsSSCC != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsSSCC", DbType.Boolean, cust.IsSSCC));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsSSCC", DbType.Boolean, DBNull.Value));

                if (cust.CompanyCode != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CompanyCode", DbType.String, cust.CompanyCode));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CompanyCode", DbType.String, DBNull.Value));
                if (cust.BizLocGLN != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BizLocGLN", DbType.String, cust.BizLocGLN));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BizLocGLN", DbType.String, DBNull.Value));

                if (cust.BizLocGLN_Ext != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BizLocGLN_Ext", DbType.String, cust.BizLocGLN));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BizLocGLN_Ext", DbType.String, DBNull.Value));

                if (cust.stateOrRegion != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@stateOrRegion", DbType.Int32, cust.stateOrRegion));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@stateOrRegion", DbType.String, DBNull.Value));

                if (cust.city != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@city", DbType.String, cust.city));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@city", DbType.String, DBNull.Value));

                if (cust.postalCode != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@postalCode", DbType.String, cust.postalCode));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@postalCode", DbType.String, DBNull.Value));

                if (cust.License != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@License", DbType.String, cust.License));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@License", DbType.String, DBNull.Value));

                if (cust.LicenseState != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LicenseState", DbType.String, cust.LicenseState));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LicenseState", DbType.String, DBNull.Value));

                if (cust.LicenseAgency != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LicenseAgency", DbType.String, cust.LicenseAgency));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LicenseAgency", DbType.String, DBNull.Value));

                if (cust.street1 != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@street1", DbType.String, cust.street1));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@street1", DbType.String, DBNull.Value));

                if (cust.street2 != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@street2", DbType.String, cust.street2));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@street2", DbType.String, DBNull.Value));

                if (cust.Host != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Host", DbType.String, cust.Host));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Host", DbType.String, DBNull.Value));

                if (cust.HostPswd != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HostPswd", DbType.String, cust.HostPswd));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HostPswd", DbType.String, DBNull.Value));

                if (cust.HostPort != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HostPort", DbType.Int32, cust.HostPort));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HostPort", DbType.Int32, DBNull.Value));

                if (cust.HostUser != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HostUser", DbType.String, cust.HostUser));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HostUser", DbType.String, DBNull.Value));

                if (cust.Loosext != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Loosext", DbType.String, cust.Loosext));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Loosext", DbType.String, DBNull.Value));

                //  oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RetID", DbType.Int64, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);
                //   return Convert.ToInt32(oDbCommand.Parameters["@RetID"].Value);
                //}
                //return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int InsertOrUpdateCustomerForDataSyncPUSHTOGLOBAL(int Flag, M_Customer cust)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateCustomerForDataSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CompanyName", DbType.String, cust.CompanyName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ContactPerson", DbType.String, cust.ContactPerson));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ContactNo", DbType.String, cust.ContactNo));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Email", DbType.String, cust.Email));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Address", DbType.String, cust.Address));
                if (cust.Country.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Country", DbType.Int32, cust.Country));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Country", DbType.Int32, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsActive", DbType.Boolean, cust.IsActive));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@APIKey", DbType.String, cust.APIKey));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SenderId", DbType.String, cust.SenderId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ReceiverId", DbType.String, cust.ReceiverId));
                if (cust.CreatedOn != System.DateTime.MinValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedOn", DbType.DateTime, cust.CreatedOn));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedOn", DbType.DateTime, DBNull.Value));
                if (cust.LastModified != System.DateTime.MinValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastModified", DbType.DateTime, cust.LastModified));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastModified", DbType.DateTime, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedBy", DbType.Int32, cust.CreatedBy));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ModifiedBy", DbType.Int32, cust.ModifiedBy));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsDeleted", DbType.Boolean, cust.IsDeleted));
                if (cust.APIUrl != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@APIUrl", DbType.String, cust.APIUrl)); // Modified on [27.11.2015]
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@APIUrl", DbType.String, DBNull.Value));

                if (cust.ProviderId.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProviderId", DbType.Int32, cust.ProviderId));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProviderId", DbType.Int32, DBNull.Value));

                if (cust.IsSSCC != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsSSCC", DbType.Boolean, cust.IsSSCC));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsSSCC", DbType.Boolean, DBNull.Value));

                if (cust.CompanyCode != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CompanyCode", DbType.String, cust.CompanyCode));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CompanyCode", DbType.String, DBNull.Value));
                if (cust.BizLocGLN != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BizLocGLN", DbType.String, cust.BizLocGLN));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BizLocGLN", DbType.String, DBNull.Value));

                if (cust.BizLocGLN_Ext != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BizLocGLN_Ext", DbType.String, cust.BizLocGLN));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BizLocGLN_Ext", DbType.String, DBNull.Value));

                if (cust.stateOrRegion != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@stateOrRegion", DbType.Int32, cust.stateOrRegion));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@stateOrRegion", DbType.String, DBNull.Value));

                if (cust.city != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@city", DbType.String, cust.city));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@city", DbType.String, DBNull.Value));

                if (cust.postalCode != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@postalCode", DbType.String, cust.postalCode));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@postalCode", DbType.String, DBNull.Value));

                if (cust.License != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@License", DbType.String, cust.License));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@License", DbType.String, DBNull.Value));

                if (cust.LicenseState != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LicenseState", DbType.String, cust.LicenseState));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LicenseState", DbType.String, DBNull.Value));

                if (cust.LicenseAgency != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LicenseAgency", DbType.String, cust.LicenseAgency));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LicenseAgency", DbType.String, DBNull.Value));

                if (cust.street1 != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@street1", DbType.String, cust.street1));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@street1", DbType.String, DBNull.Value));

                if (cust.street2 != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@street2", DbType.String, cust.street2));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@street2", DbType.String, DBNull.Value));

                if (cust.Host != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Host", DbType.String, cust.Host));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Host", DbType.String, DBNull.Value));

                if (cust.HostPswd != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HostPswd", DbType.String, cust.HostPswd));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HostPswd", DbType.String, DBNull.Value));

                if (cust.HostPort != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HostPort", DbType.Int32, cust.HostPort));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HostPort", DbType.Int32, DBNull.Value));

                if (cust.HostUser != null)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HostUser", DbType.String, cust.HostUser));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@HostUser", DbType.String, DBNull.Value));



                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RetID", DbType.Int64, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
                DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);
                return Convert.ToInt32(oDbCommand.Parameters["@RetID"].Value);
                //}
                //return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateProductApplicatorSetting(ProductApplicatorSetting pro)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_insertorupdateProductApplicatorSettingForDataSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ServerPAID", DbType.Decimal, pro.ServerPAID));

                if (pro.S1 != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S1", DbType.Decimal, pro.S1));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S1", DbType.Decimal, DBNull.Value));
                }

                if (pro.S2 != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S2", DbType.Decimal, pro.S2));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S2", DbType.Decimal, DBNull.Value));
                }

                if (pro.S3 != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S3", DbType.Decimal, pro.S3));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S3", DbType.Decimal, DBNull.Value));
                }

                if (pro.S4 != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S4", DbType.Decimal, pro.S4));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S4", DbType.Decimal, DBNull.Value));
                }

                if (pro.S5 != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S5", DbType.Decimal, pro.S5));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S5", DbType.Decimal, DBNull.Value));
                }

                if (pro.FrontLabelOffset != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FrontLabelOffset", DbType.Decimal, pro.FrontLabelOffset));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FrontLabelOffset", DbType.Decimal, DBNull.Value));
                }

                if (pro.BackLabelOffset != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BackLabelOffset", DbType.Decimal, pro.BackLabelOffset));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BackLabelOffset", DbType.Decimal, DBNull.Value));
                }

                if (pro.CartonLength != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CartonLength", DbType.Decimal, pro.CartonLength));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CartonLength", DbType.Decimal, DBNull.Value));
                }

                return DbProviderHelper.ExecuteNonQuery(oDbCommand);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteQuery(string Query)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text, sqldbCon);
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //#region Insert/Update For China UID Table
        //public int InsertUpdateChinaUIDForSync(ChinaUID oChinaUID, int Flag, decimal JID)
        //{
        //    try
        //    {
        //        DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateChinaUIDForSync]", CommandType.StoredProcedure, sqldbCon);
        //        if (JID > 0)
        //            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Int32, JID));
        //        else
        //            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobId", DbType.Int32, DBNull.Value));
        //        if (oChinaUID.TransId.HasValue)
        //            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransId", DbType.Int32, oChinaUID.TransId));
        //        else
        //            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransId", DbType.Int32, DBNull.Value));
        //        if (oChinaUID.UID != null)
        //            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@code", DbType.String, oChinaUID.UID));
        //        else
        //            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@code", DbType.String, DBNull.Value));
        //        if (oChinaUID.Result.HasValue)
        //            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Result", DbType.Boolean, oChinaUID.Result));
        //        else
        //            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Result", DbType.Boolean, DBNull.Value));

        //        if (oChinaUID.IsUsed.HasValue)
        //            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsUsed", DbType.Boolean, oChinaUID.IsUsed));
        //        else
        //            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsUsed", DbType.Boolean, DBNull.Value));

        //        if (oChinaUID.PackagingTypeCode !=null)
        //            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackageTypeCode", DbType.String, oChinaUID.PackagingTypeCode));
        //        else
        //            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackageTypeCode", DbType.String, DBNull.Value));

        //        oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SendId", DbType.Int32, oChinaUID.SendId));

        //        oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
        //        return DbProviderHelper.ExecuteNonQuery(oDbCommand,0);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion

        #region Insert/Update For SSCC Line Holder
        public int InsertOrUpdateSSCCLineHolder(SSCCLineHolder oSSCCLineHolder)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateSSCCLineHolderForSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackageIndicator", DbType.Decimal, oSSCCLineHolder.PackageIndicator));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastSSCC", DbType.Decimal, oSSCCLineHolder.LastSSCC));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FirstSSCC", DbType.Decimal, oSSCCLineHolder.FirstSSCC));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobID", DbType.Decimal, oSSCCLineHolder.JobID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oSSCCLineHolder.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, oSSCCLineHolder.LineCode));

                return DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public int InsertOrUpdateJobForSyncToGlobal(int Flag, Job oJob, string ProductName, string CreatedByName, string VerifiedByName)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateJobForDataSync]", CommandType.StoredProcedure, sqldbCon);
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
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobStatus", DbType.Int16, oJob.JobStatus));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DetailInfo", DbType.String, oJob.DetailInfo));

                if (oJob.JobStartTime != System.DateTime.MinValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobStartTime", DbType.DateTime, oJob.JobStartTime));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobStartTime", DbType.DateTime, DBNull.Value));

                if (oJob.JobEndTime.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobEndTime", DbType.DateTime, oJob.JobEndTime));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobEndTime", DbType.DateTime, DBNull.Value));

                if (oJob.PlantCode != "")
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PlantCode", DbType.String, oJob.PlantCode));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PlantCode", DbType.String, DBNull.Value));

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
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductName", DbType.String, ProductName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedByName", DbType.String, CreatedByName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@VerifiedByName", DbType.String, VerifiedByName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@NoReadCount", DbType.Int32, oJob.NoReadCount));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UseExpDay", DbType.Boolean, oJob.UseExpDay));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ExpDateFormat", DbType.String, oJob.ExpDateFormat));
                //NoReadCount
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, oJob.LineCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Decimal, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackagingLvlId", DbType.Int32, oJob.PackagingLvlId));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CustomerId", DbType.Int32, oJob.CustomerId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProviderId", DbType.Int32, oJob.ProviderId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PPNCountryCode", DbType.String, oJob.PPNCountryCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PPNPostalCode", DbType.String, oJob.PPNPostalCode));
                DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);

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

        #region Insert/Update For UserTrail
        public int InsertOrUpdateUSerTrail(USerTrail oUSerTrail, string LineCode)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateUSerTrailForSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Decimal, oUSerTrail.ID));
                if (oUSerTrail.AccessedAt.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AccessedAt", DbType.DateTime, oUSerTrail.AccessedAt));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AccessedAt", DbType.DateTime, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Reason", DbType.Xml, oUSerTrail.Reason));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oUSerTrail.Remarks));
                // oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserName", DbType.String, oUSerTrail.UserName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, oUSerTrail.LineCode));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Insert/Update For User Details
        public int InsertOrUpdateUserDetails(Users oUsers, int flag)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateUsersForDataSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, oUsers.ID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserName", DbType.String, oUsers.UserName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserName1", DbType.String, oUsers.UserName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Password", DbType.String, oUsers.Password));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RoleID", DbType.Int32, oUsers.RoleID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Active", DbType.Boolean, oUsers.Active));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oUsers.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserType", DbType.String, oUsers.UserType));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsFirstLoginAtempt", DbType.String, oUsers.IsFirstLogin));

                return DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateUserDetailsForGlobalServer(Users oUsers, int flag)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateUsersForDataSync]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, oUsers.ID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserName", DbType.String, oUsers.UserName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserName1", DbType.String, oUsers.UserName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Password", DbType.String, oUsers.Password));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RoleID", DbType.Int32, oUsers.RoleID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Active", DbType.Boolean, oUsers.Active));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oUsers.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UserType", DbType.String, oUsers.UserType));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsFirstLoginAtempt", DbType.String, oUsers.IsFirstLogin));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RetID", DbType.Int64, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand, 0);
                //return Convert.ToInt32(oDbCommand.Parameters["@RetID"].Value);
           
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Retrive Data from Packaging Details

        public List<PackagingDetails> GetPackagingDetailssFromServer(int Flag, string ParamVal1, string ParamVal2, string ParamVal3)
        {
            try
            {
                List<PackagingDetails> lstPackagingDetailss = new List<PackagingDetails>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetailsFromServer]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.CommandTimeout = 0;
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, ParamVal1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, ParamVal2));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, ParamVal3));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    PackagingDetails oPackagingDetails = new PackagingDetails();
                    oPackagingDetails.PackDtlsID = Convert.ToDecimal(oDbDataReader["PackDtlsID"]);
                    oPackagingDetails.Code = Convert.ToString(oDbDataReader["Code"]);
                    oPackagingDetails.PAID = Convert.ToDecimal(oDbDataReader["PAID"]);
                    oPackagingDetails.JobID = Convert.ToDecimal(oDbDataReader["JobID"]);
                    oPackagingDetails.PackageTypeCode = Convert.ToString(oDbDataReader["PackageTypeCode"]);
                    oPackagingDetails.MfgPackDate = Convert.ToDateTime(oDbDataReader["MfgPackDate"]);
                    oPackagingDetails.ExpPackDate = Convert.ToDateTime(oDbDataReader["ExpPackDate"]);

                    if (oDbDataReader["NextLevelCode"] != DBNull.Value)
                        oPackagingDetails.NextLevelCode = Convert.ToString(oDbDataReader["NextLevelCode"]);
                    if (oDbDataReader["IsRejected"] != DBNull.Value)
                        oPackagingDetails.IsRejected = Convert.ToBoolean(oDbDataReader["IsRejected"]);

                    if (oDbDataReader["Reason"] != DBNull.Value)
                        oPackagingDetails.Reason = Convert.ToString(oDbDataReader["Reason"]);

                    if (oDbDataReader["BadImage"] != DBNull.Value)
                        oPackagingDetails.BadImage = (Byte[])(oDbDataReader["BadImage"]);

                    if (oDbDataReader["SSCC"] != DBNull.Value)
                        oPackagingDetails.SSCC = Convert.ToString(oDbDataReader["SSCC"]);

                    if (oDbDataReader["SSCCVarificationStatus"] != DBNull.Value)
                        oPackagingDetails.SSCCVarificationStatus = Convert.ToBoolean(oDbDataReader["SSCCVarificationStatus"]);

                    if (oDbDataReader["IsManualUpdated"] != DBNull.Value)
                        oPackagingDetails.IsManualUpdated = Convert.ToBoolean(oDbDataReader["IsManualUpdated"]);

                    if (oDbDataReader["ManualUpdateDesc"] != DBNull.Value)
                        oPackagingDetails.ManualUpdateDesc = Convert.ToString(oDbDataReader["ManualUpdateDesc"]);

                    if (oDbDataReader["CaseSeqNum"] != DBNull.Value)
                        oPackagingDetails.CaseSeqNum = Convert.ToDecimal(oDbDataReader["CaseSeqNum"]);

                    if (oDbDataReader["OperatorId"] != DBNull.Value)
                        oPackagingDetails.OperatorId = Convert.ToDecimal(oDbDataReader["OperatorId"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oPackagingDetails.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["IsDecomission"] != DBNull.Value)
                        oPackagingDetails.IsDecomission = Convert.ToBoolean(oDbDataReader["IsDecomission"]);

                    oPackagingDetails.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);
                    oPackagingDetails.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);
                    //if (oDbDataReader["RCResult"] != DBNull.Value)
                    //    oPackagingDetails.RCResult = Convert.ToInt32(oDbDataReader["RCResult"]);

                    lstPackagingDetailss.Add(oPackagingDetails);
                }
                oDbDataReader.Close();
                return lstPackagingDetailss;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Retrive Data from Packaging Details

        #region Retrive Job from perticular line

        public DataSet GetFinishedJobDetailView(int Flag, string LineCode)
        {
            try
            {
                DataSet ds;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETLineDetails]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineID", DbType.Int32, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, LineCode));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineName", DbType.String, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineDescription", DbType.String, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineOfficer", DbType.String, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchNo", DbType.String, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedBy", DbType.Decimal, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate", DbType.DateTime, DateTime.Now));
                //   DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                ds = DbProviderHelper.FillDataSet(DbAdpt);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public List<Users> GetUsersFromServer(int Flag)
        {
            try
            {
                List<Users> lstUserss = new List<Users>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETUsersFromServer]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.CommandTimeout = 0;
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, DBNull.Value));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    Users oUsers = new Users();
                    oUsers.ID = Convert.ToDecimal(oDbDataReader["ID"]);

                    if (oDbDataReader["UserName"] != DBNull.Value)
                        oUsers.UserName = Convert.ToString(oDbDataReader["UserName"]);

                    if (oDbDataReader["UserName1"] != DBNull.Value)
                        oUsers.UserName1 = Convert.ToString(oDbDataReader["UserName1"]);

                    if (oDbDataReader["Password"] != DBNull.Value)
                        oUsers.Password = Convert.ToString(oDbDataReader["Password"]);

                    if (oDbDataReader["RoleID"] != DBNull.Value)
                        oUsers.RoleID = Convert.ToInt32(oDbDataReader["RoleID"]);

                    if (oDbDataReader["Active"] != DBNull.Value)
                        oUsers.Active = Convert.ToBoolean(oDbDataReader["Active"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oUsers.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["CreatedDate"] != DBNull.Value)
                        oUsers.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);

                    if (oDbDataReader["LastUpdatedDate"] != DBNull.Value)
                        oUsers.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);

                    lstUserss.Add(oUsers);
                }

                oDbDataReader.Close();
                return lstUserss;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Users GetUserFromGlobalServer(int Flag, Users user)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETUsersFromServer]", CommandType.StoredProcedure, sqldbCon);
                oDbCommand.CommandTimeout = 0;
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, user.UserName));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                Users oUsers = new Users();
                while (oDbDataReader.Read())
                {

                    oUsers.ID = Convert.ToDecimal(oDbDataReader["ID"]);

                    if (oDbDataReader["UserName"] != DBNull.Value)
                        oUsers.UserName = Convert.ToString(oDbDataReader["UserName"]);

                    if (oDbDataReader["UserName1"] != DBNull.Value)
                        oUsers.UserName1 = Convert.ToString(oDbDataReader["UserName1"]);

                    if (oDbDataReader["Password"] != DBNull.Value)
                        oUsers.Password = Convert.ToString(oDbDataReader["Password"]);

                    if (oDbDataReader["RoleID"] != DBNull.Value)
                        oUsers.RoleID = Convert.ToInt32(oDbDataReader["RoleID"]);

                    if (oDbDataReader["Active"] != DBNull.Value)
                        oUsers.Active = Convert.ToBoolean(oDbDataReader["Active"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oUsers.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["CreatedDate"] != DBNull.Value)
                        oUsers.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);

                    if (oDbDataReader["LastUpdatedDate"] != DBNull.Value)
                        oUsers.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);


                }

                oDbDataReader.Close();
                return oUsers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
