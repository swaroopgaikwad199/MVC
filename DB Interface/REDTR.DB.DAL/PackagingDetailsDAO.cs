using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;
using PTPLCRYPTORENGINE;

namespace REDTR.DB.DAL
{
    public partial  class PackagingDetailsDAO
    {
        public PackagingDetailsDAO()
        {
            DbProviderHelper.GetConnection();
        }
        public List<PackagingDetails> GetPackagingDetailssQuery(string Query)
        {
            try
            {
                List<PackagingDetails> lstPackagingDetailss = new List<PackagingDetails>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    PackagingDetails oPackagingDetails = new PackagingDetails();
                    oPackagingDetails.PackDtlsID = Convert.ToDecimal(oDbDataReader["PackDtlsID"]);
                    oPackagingDetails.Code = Convert.ToString(oDbDataReader["Code"]);
                    //if (oPackagingDetails.Code != null)
                    //{
                    //    oPackagingDetails.Code = AESCryptor.Decrypt(oPackagingDetails.Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    //}
                    oPackagingDetails.PAID = Convert.ToDecimal(oDbDataReader["PAID"]);
                    oPackagingDetails.JobID = Convert.ToDecimal(oDbDataReader["JobID"]);
                    oPackagingDetails.PackageTypeCode = Convert.ToString(oDbDataReader["PackageTypeCode"]);
                    oPackagingDetails.MfgPackDate = Convert.ToDateTime(oDbDataReader["MfgPackDate"]);
                    oPackagingDetails.ExpPackDate = Convert.ToDateTime(oDbDataReader["ExpPackDate"]);

                    if (oDbDataReader["NextLevelCode"] != DBNull.Value)
                    {
                        oPackagingDetails.NextLevelCode = Convert.ToString(oDbDataReader["NextLevelCode"]);
                        //oPackagingDetails.NextLevelCode = AESCryptor.Decrypt(oPackagingDetails.NextLevelCode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    }
                    if (oDbDataReader["IsRejected"] != DBNull.Value)
                        oPackagingDetails.IsRejected = Convert.ToBoolean(oDbDataReader["IsRejected"]);
                    if (oDbDataReader["DAVAPortalUpload"] != DBNull.Value)
                        oPackagingDetails.DAVAPortalUpload = Convert.ToBoolean(oDbDataReader["DAVAPortalUpload"]);
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
                    if (oDbDataReader["RCResult"] != DBNull.Value)
                        oPackagingDetails.RCResult = Convert.ToInt32(oDbDataReader["RCResult"]);

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

        public List<PackagingDetails> GetPackagingDetailss(int Flag, string ParamVal1, string ParamVal2, string ParamVal3)
        {
            try
            {
                List<PackagingDetails> lstPackagingDetailss = new List<PackagingDetails>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);
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
                    //if (oPackagingDetails.Code != null)
                    //{
                    //    oPackagingDetails.Code = AESCryptor.Decrypt(oPackagingDetails.Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    //}
                    oPackagingDetails.PAID = Convert.ToDecimal(oDbDataReader["PAID"]);
                    oPackagingDetails.JobID = Convert.ToDecimal(oDbDataReader["JobID"]);
                    oPackagingDetails.PackageTypeCode = Convert.ToString(oDbDataReader["PackageTypeCode"]);
                    oPackagingDetails.MfgPackDate = Convert.ToDateTime(oDbDataReader["MfgPackDate"]);
                    oPackagingDetails.ExpPackDate = Convert.ToDateTime(oDbDataReader["ExpPackDate"]);

                    if (oDbDataReader["NextLevelCode"] != DBNull.Value)
                    {
                        oPackagingDetails.NextLevelCode = Convert.ToString(oDbDataReader["NextLevelCode"]);
                        //oPackagingDetails.NextLevelCode = AESCryptor.Decrypt(oPackagingDetails.NextLevelCode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    }
                    if (oDbDataReader["IsRejected"] != DBNull.Value)
                        oPackagingDetails.IsRejected = Convert.ToBoolean(oDbDataReader["IsRejected"]);
                    if (oDbDataReader["DAVAPortalUpload"] != DBNull.Value)
                        oPackagingDetails.DAVAPortalUpload = Convert.ToBoolean(oDbDataReader["DAVAPortalUpload"]);
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
                    if (oDbDataReader["RCResult"] != DBNull.Value)
                        oPackagingDetails.RCResult = Convert.ToInt32(oDbDataReader["RCResult"]);

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

        public string GetNextLevOfJobelForLoose(int Flag, string m_jobId, string PackTypeCode, string PackTypeCode2, string FailureType)
        {
            string NextLevelCode = string.Empty;
            DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails_ForLoose]", CommandType.StoredProcedure);
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.String, Flag));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, m_jobId));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, PackTypeCode));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, PackTypeCode2));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value4", DbType.String, FailureType));
            DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
            try
            {
                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["NextLevelCode"] != DBNull.Value)
                        NextLevelCode = Convert.ToString(oDbDataReader["NextLevelCode"]);
                }
                oDbDataReader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return NextLevelCode;
        }

        public List<PackagingDetails> GetPackagingDetailssForSync(int Flag, string ParamVal1, string ParamVal2, string ParamVal3)
        {
            try
            {
                List<PackagingDetails> lstPackagingDetailss = new List<PackagingDetails>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);
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
                    //if (oPackagingDetails.Code != null)
                    //{
                    //    oPackagingDetails.Code = AESCryptor.Decrypt(oPackagingDetails.Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    //}
                    oPackagingDetails.PAID = Convert.ToDecimal(oDbDataReader["PAID"]);
                    oPackagingDetails.JobID = Convert.ToDecimal(oDbDataReader["JobID"]);
                    oPackagingDetails.PackageTypeCode = Convert.ToString(oDbDataReader["PackageTypeCode"]);
                    oPackagingDetails.MfgPackDate = Convert.ToDateTime(oDbDataReader["MfgPackDate"]);
                    oPackagingDetails.ExpPackDate = Convert.ToDateTime(oDbDataReader["ExpPackDate"]);

                    if (oDbDataReader["NextLevelCode"] != DBNull.Value)
                    {
                        oPackagingDetails.NextLevelCode = Convert.ToString(oDbDataReader["NextLevelCode"]);
                       // oPackagingDetails.NextLevelCode = AESCryptor.Decrypt(oPackagingDetails.NextLevelCode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    }
                    if (oDbDataReader["IsRejected"] != DBNull.Value)
                        oPackagingDetails.IsRejected = Convert.ToBoolean(oDbDataReader["IsRejected"]);
                    if (oDbDataReader["DAVAPortalUpload"] != DBNull.Value)
                        oPackagingDetails.DAVAPortalUpload = Convert.ToBoolean(oDbDataReader["DAVAPortalUpload"]);
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
                    if (oDbDataReader["RCResult"] != DBNull.Value)
                        oPackagingDetails.RCResult = Convert.ToInt32(oDbDataReader["RCResult"]);
                    if (oDbDataReader["LineCode"] != DBNull.Value)
                        oPackagingDetails.LineCode = Convert.ToString(oDbDataReader["LineCode"]);
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

        /// <summary>
        /// For Retriving Packaging Details count as per given condition [05.12.2016]
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="deck"></param>
        /// <param name="NextLevelCode"></param>
        /// <returns></returns>
        public int RetriveCountFromPackagingDetails(decimal? JobID, string deck, string NextLevelCode)
        {
            try
            {
                int Count = 0;
                string CmdString = string.Empty;
                
                CmdString = "SELECT COUNT(*) AS CNT FROM PackagingDetails WHERE JobID=CAST(" + JobID + " as Numeric) and (IsRejected=0) " +
                                    "and (IsDecomission =0 or IsDecomission is null) and PackageTypeCode='" + deck + "' and NextLevelCode = '" + NextLevelCode + "'";
                
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(CmdString, CommandType.Text);

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    Count = Convert.ToInt32(oDbDataReader["CNT"]);
                }
                oDbDataReader.Close();
                return Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retrive RC from first deck if rc status is there
        /// </summary>
        /// <param name="JobID"></param>
        /// <param name="deck"></param>
        /// <returns></returns>
        public int RetriveRCCountFromPackagingDetails(decimal? JobID, string deck,string CmdString)
        {
            try
            {
                int Count = 0;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(CmdString, CommandType.Text);
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    Count = Convert.ToInt32(oDbDataReader["CNT"]);
                }
                oDbDataReader.Close();
                return Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public PackagingDetails GetPackagingDetails(int Flag, string ParamVal1, string ParamVal2, string ParamVal3)
        {
            try
            {
                PackagingDetails oPackagingDetails = new PackagingDetails();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.String, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, ParamVal1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, ParamVal2));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, ParamVal3));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    oPackagingDetails.PackDtlsID = Convert.ToDecimal(oDbDataReader["PackDtlsID"]);
                    oPackagingDetails.Code = Convert.ToString(oDbDataReader["Code"]);
                    //if (oPackagingDetails.Code != null)
                    //{
                    //    oPackagingDetails.Code = AESCryptor.Decrypt(oPackagingDetails.Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    //}
                    oPackagingDetails.PAID = Convert.ToDecimal(oDbDataReader["PAID"]);
                    oPackagingDetails.JobID = Convert.ToDecimal(oDbDataReader["JobID"]);
                    oPackagingDetails.PackageTypeCode = Convert.ToString(oDbDataReader["PackageTypeCode"]);
                    oPackagingDetails.MfgPackDate = Convert.ToDateTime(oDbDataReader["MfgPackDate"]);
                    oPackagingDetails.ExpPackDate = Convert.ToDateTime(oDbDataReader["ExpPackDate"]);

                    if (oDbDataReader["NextLevelCode"] != DBNull.Value)
                    {
                        oPackagingDetails.NextLevelCode = Convert.ToString(oDbDataReader["NextLevelCode"]);
                        //oPackagingDetails.NextLevelCode = AESCryptor.Decrypt(oPackagingDetails.NextLevelCode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    }
                    if (oDbDataReader["DAVAPortalUpload"] != DBNull.Value)
                        oPackagingDetails.DAVAPortalUpload = Convert.ToBoolean(oDbDataReader["DAVAPortalUpload"]);
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

                    if (oDbDataReader["OperatorId"] != DBNull.Value)
                        oPackagingDetails.OperatorId = Convert.ToDecimal(oDbDataReader["OperatorId"]);

                    if (oDbDataReader["CaseSeqNum"] != DBNull.Value)
                        oPackagingDetails.CaseSeqNum = Convert.ToDecimal(oDbDataReader["CaseSeqNum"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oPackagingDetails.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["IsDecomission"] != DBNull.Value)
                        oPackagingDetails.IsDecomission = Convert.ToBoolean(oDbDataReader["IsDecomission"]);

                    oPackagingDetails.CreatedDate = Convert.ToDateTime(oDbDataReader["CreatedDate"]);
                    oPackagingDetails.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);

                    if (oDbDataReader["RCResult"] != DBNull.Value)
                        oPackagingDetails.RCResult = Convert.ToInt32(oDbDataReader["RCResult"]);
                    
                }
                oDbDataReader.Close();
                return oPackagingDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<string> GetnextLevelPacks(int Flag, string ParamVal1, string ParamVal2, string ParamVal3)
        {
            try
            {
                PackagingDetails oPackagingDetails = new PackagingDetails();
                List<string> GetNextLevelcodesforDummy = new List<string>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.String, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, ParamVal1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, ParamVal2));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, ParamVal3));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                   
                    oPackagingDetails.Code = Convert.ToString(oDbDataReader["Code"]);
                    //if (oPackagingDetails.Code != null)
                    //{
                    //    oPackagingDetails.Code = AESCryptor.Decrypt(oPackagingDetails.Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    //}

                    GetNextLevelcodesforDummy.Add(oPackagingDetails.Code);
                }
                oDbDataReader.Close();
                return GetNextLevelcodesforDummy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public List<PackagingDetails> GetDAVASSCCLevelPacks(int Flag, string ParamVal1, string ParamVal2, string ParamVal3)
        {
            try
            {
                PackagingDetails oPackagingDetails;
                List<PackagingDetails> GetNextLevelcodesforDummy = new List<PackagingDetails>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.String, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, ParamVal1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, ParamVal2));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, ParamVal3));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    oPackagingDetails = new PackagingDetails();
                    oPackagingDetails.SSCC = Convert.ToString(oDbDataReader["SSCC"]);
                    oPackagingDetails.PackDtlsID = int.Parse(Convert.ToString(oDbDataReader["PackDtlsID"]));
                    oPackagingDetails.JobID = int.Parse(oDbDataReader["JobID"].ToString());
                    oPackagingDetails.Code = Convert.ToString(oDbDataReader["Code"]);
                    oPackagingDetails.PackageTypeCode = Convert.ToString(oDbDataReader["PackageTypeCode"]);
                    //if (oPackagingDetails.Code != null)
                    //{
                    //    oPackagingDetails.Code = AESCryptor.Decrypt(oPackagingDetails.Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    //}

                    GetNextLevelcodesforDummy.Add(oPackagingDetails);
                }
                oDbDataReader.Close();
                return GetNextLevelcodesforDummy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<PackagingDetails> GetDAVASecondaryPacks(int Flag, string ParamVal1, string ParamVal2, string ParamVal3)
        {
            try
            {
                PackagingDetails oPackagingDetails;
                List<PackagingDetails> GetNextLevelcodesforDummy = new List<PackagingDetails>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.String, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, ParamVal1));

                //ParamVal2 = AESCryptor.Encrypt(ParamVal2, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, ParamVal2));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, ParamVal3));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    oPackagingDetails = new PackagingDetails();
                   //oPackagingDetails.JobID = int.Parse(oDbDataReader["JobID"].ToString());
                    oPackagingDetails.Code = Convert.ToString(oDbDataReader["Code"]);
                    //if (oPackagingDetails.Code != null)
                    //{
                    //    oPackagingDetails.Code = AESCryptor.Decrypt(oPackagingDetails.Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    //}

                    GetNextLevelcodesforDummy.Add(oPackagingDetails);
                }
                oDbDataReader.Close();
                return GetNextLevelcodesforDummy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<PrimaryPackDummys> GetDAVAPrimaryPacks(int Flag, string ParamVal1, string ParamVal2, string ParamVal3)
        {
            try
            {
                PrimaryPackDummys oPackagingDetails;
                List<PrimaryPackDummys> GetNextLevelcodesforDummy = new List<PrimaryPackDummys>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.String, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, ParamVal1));

               // ParamVal2 = AESCryptor.Encrypt(ParamVal2, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, ParamVal2));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, ParamVal3));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    oPackagingDetails = new PrimaryPackDummys();
                    //oPackagingDetails.JobId = Convert.ToInt32(oDbDataReader["JobId"]);
                    oPackagingDetails.Code = Convert.ToString(oDbDataReader["Code"]);
                    //if (oPackagingDetails.Code != null)
                    //{
                    //    oPackagingDetails.Code = AESCryptor.Decrypt(oPackagingDetails.Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    //}

                    GetNextLevelcodesforDummy.Add(oPackagingDetails);
                }
                oDbDataReader.Close();
                return GetNextLevelcodesforDummy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public DataSet GetDataSet(int Flag, string ParamVal1, string ParamVal2, string ParamVal3)
        {
            try
            {
                DataSet oDataSet = new DataSet();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, ParamVal1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, ParamVal2));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, ParamVal3));
                oDbCommand.CommandTimeout = 0;
                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);

                oDataSet = DbProviderHelper.FillDataSet(DbAdpt);
                return oDataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetCountDetailsOfJob(int Flag, string m_jobId, string PackTypeCode,string FailureType)
        {
            int COUNT = 0;
            DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.String, Flag));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, m_jobId));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, PackTypeCode));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, FailureType));
            DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
            try
            {
                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["CNT"] != DBNull.Value)
                        COUNT = Convert.ToInt32(oDbDataReader["CNT"]);
                }
                oDbDataReader.Close();
            }
            catch (Exception ex )
            {
              
            }
           
            return COUNT;
        }
        public int GetCountDetailsOfJobForLoose(int Flag, string m_jobId, string PackTypeCode, string PackTypeCode2, string FailureType)
        {
            int COUNT = 0;
            DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails_ForLoose]", CommandType.StoredProcedure);
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.String, Flag));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, m_jobId));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, PackTypeCode));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, PackTypeCode2));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value4", DbType.String, FailureType));
            DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
            try
            {
                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["CNT"] != DBNull.Value)
                        COUNT = Convert.ToInt32(oDbDataReader["CNT"]);
                }
                oDbDataReader.Close();
            }
            catch (Exception ex)
            {

            }

            return COUNT;
        }
        public int InsertOrUpdatePackagingDetails(int Flag, PackagingDetails oPackagingDetails)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdatePackagingDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackDtlsID", DbType.Decimal, oPackagingDetails.PackDtlsID));
                if (!string.IsNullOrEmpty(oPackagingDetails.Code))
                {
                    oPackagingDetails.Code = AESCryptor.Encrypt(oPackagingDetails.Code, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@code", DbType.String, oPackagingDetails.Code));
                }
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@code", DbType.String, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@pAID", DbType.Decimal, oPackagingDetails.PAID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@jobId", DbType.Decimal, oPackagingDetails.JobID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@pkgTypeCode", DbType.String, oPackagingDetails.PackageTypeCode));
               if (oPackagingDetails.MfgPackDate != System.DateTime.MinValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@mfgPackDate", DbType.DateTime, oPackagingDetails.MfgPackDate));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@mfgPackDate", DbType.DateTime, DBNull.Value));

                if (oPackagingDetails.ExpPackDate != System.DateTime.MinValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@expPackDate", DbType.DateTime, oPackagingDetails.ExpPackDate));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@expPackDate", DbType.DateTime, DBNull.Value));
                if (!string.IsNullOrEmpty(oPackagingDetails.NextLevelCode))
                {
                    //oPackagingDetails.NextLevelCode = AESCryptor.Encrypt(oPackagingDetails.NextLevelCode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@nextLevelCode", DbType.String, oPackagingDetails.NextLevelCode));
                }
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@nextLevelCode", DbType.String, DBNull.Value));
              
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsUsed", DbType.Boolean, oPackagingDetails.IsUsed));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@isRejected", DbType.Boolean, oPackagingDetails.IsRejected));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Reason", DbType.Xml, oPackagingDetails.Reason));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Reason", DbType.String, oPackagingDetails.Reason));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@badImage", DbType.Binary, oPackagingDetails.BadImage));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SSCC", DbType.String, oPackagingDetails.SSCC));

                if (oPackagingDetails.SSCCVarificationStatus.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SSCCVerificationStatus", DbType.Boolean, oPackagingDetails.SSCCVarificationStatus));
                else
                   oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SSCCVerificationStatus", DbType.Boolean, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@IsManualUpdated", DbType.String, oPackagingDetails.IsManualUpdated));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DAVAPortalUpload", DbType.String, oPackagingDetails.DAVAPortalUpload));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ManualUpdateDesc", DbType.Xml, oPackagingDetails.ManualUpdateDesc));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CaseSeqNum", DbType.Decimal, oPackagingDetails.CaseSeqNum));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@OperatorId", DbType.Decimal, oPackagingDetails.OperatorId));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oPackagingDetails.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@RCResult", DbType.Int32, oPackagingDetails.RCResult));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> GETUIDs(int Flag, string jobID, string PackageTypeCode, string DefaultNextLevelUID)
        {
            try
            {   string Code="";
                List<string> LstCode = new List<string>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, jobID));                
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value2", DbType.String, PackageTypeCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value3", DbType.String, DefaultNextLevelUID));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["Code"] != DBNull.Value)
                        //Code = AESCryptor.Decrypt(Convert.ToString(oDbDataReader["Code"]), REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                        Code = oDbDataReader["Code"].ToString();
                        //  LstCode.Add(Convert.ToString(oDbDataReader["Code"]));
                        LstCode.Add(Code);
                }
                oDbDataReader.Close();
                return LstCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemovePackagingDetails(int Flag, string Value)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_DeletePackagingDetails", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                //if (Value != null)
                //{
                //    Value = AESCryptor.Encrypt(Value, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //}
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@code", DbType.String, Value));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetPrintedCount(string JID,string Deck)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("Select Count(*) From PackagingDetails Where JobID=" + JID + " and PackageTypeCode='" + Deck + "'", CommandType.Text);
                return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // For PCMAP
        public List<PackagingDetails> GETPCMAP(int level, int jobID, string PackageTypeCode, string DefaultNextLevelUID)
        {
            try
            {
                List<PackagingDetails> lstPackagingDetailss = new List<PackagingDetails>();

                string query = "";

                if (level == 3)   // ADDED FOR PALLET - PRASHANT [add parameter as level]
                {
                    if (DefaultNextLevelUID == "NULL")
                        query = "Select Code, SSCC From PackagingDetails Where JobID=" + jobID + " and PackageTypeCode='" + PackageTypeCode + "' and NextLevelCode is null";
                    else
                        query = "Select Code, SSCC From PackagingDetails Where JobID=" + jobID + " and PackageTypeCode='" + PackageTypeCode + "' and NextLevelCode ='" + DefaultNextLevelUID + "' and (IsDecomission=0 or IsDecomission is null) and (IsRejected=0) "; // Condition updated to retrive non decommisioned items [Sunil 
                }
                else if (level == 4)
                {
                    if (PackageTypeCode == "PAL") 
                        query = "Select Code, SSCC From PackagingDetails Where JobID=" + jobID + " and PackageTypeCode='" + PackageTypeCode + "' and NextLevelCode is null";
                    else
                        query = "Select Code, SSCC From PackagingDetails Where JobID=" + jobID + " and PackageTypeCode='" + PackageTypeCode + "' and NextLevelCode ='" + DefaultNextLevelUID + "' and (IsDecomission=0 or IsDecomission is null) and (IsRejected=0) "; // Condition updated to retrive non decommisioned items [Sunil 09.10.2016]
                }

                List<string> LstCode = new List<string>();
                List<string> LstSSCC = new List<string>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(query, CommandType.Text);
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    PackagingDetails oPackagingDetails = new PackagingDetails();
                    if (oDbDataReader["SSCC"] != DBNull.Value)
                        oPackagingDetails.SSCC = Convert.ToString(oDbDataReader["SSCC"]);
                    if (oDbDataReader["Code"] != DBNull.Value)
                        oPackagingDetails.Code = Convert.ToString(oDbDataReader["Code"]);

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

        // For insert TempPCMAP

        public bool InsertTempPCMAP(int JID, string PALCode, string ISHCode, string OBXCode, string MOCCode, string SSCC)
        {
            try
            {
                //PALCode = AESCryptor.Decrypt(PALCode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //ISHCode = AESCryptor.Decrypt(ISHCode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //OBXCode = AESCryptor.Decrypt(OBXCode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
                //MOCCode = AESCryptor.Decrypt(MOCCode, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);

                string Query = " INSERT into [TempPCMAP] values(" + JID + ",'" + PALCode + "','" + ISHCode + "','" + OBXCode + "','" + MOCCode + "','" + SSCC + "')";
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
                DbProviderHelper.ExecuteNonQuery(oDbCommand);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;

            }
        }

        // For Delete the TempPCMAP
        public bool DeleteTempPCMAP()
        {
            try
            {
                string Query = "DELETE from TempPCMAP";
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
                DbProviderHelper.ExecuteNonQuery(oDbCommand);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;

            }
        }

    }
}
