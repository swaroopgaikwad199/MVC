using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
    public class PackagingAssoDetailsDAO
    {
        public PackagingAssoDetailsDAO()
        {
            DbProviderHelper.GetConnection();
        }

        public int InsertOrUpdatePckAssoDtls(int Flag, PackagingAssoDetails oPackagingAssoDetails)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdatePackagingAssoDetails]", CommandType.StoredProcedure);

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Decimal, oPackagingAssoDetails.PAID));
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
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PackagingAssoDetails> GetPckAssoDtlssOfPckAsso(int Flag, string param)
        {
            try
            {
                List<PackagingAssoDetails> lstPackagingAssoDetailss = new List<PackagingAssoDetails>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingAssoDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, param));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PckType", DbType.String, DBNull.Value));

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    PackagingAssoDetails oPackagingAssoDetails = new PackagingAssoDetails();
                    oPackagingAssoDetails.PAID = Convert.ToDecimal(oDbDataReader["PAID"]);

                    if (oDbDataReader["PackageTypeCode"] != DBNull.Value)
                        oPackagingAssoDetails.PackageTypeCode = Convert.ToString(oDbDataReader["PackageTypeCode"]);

                    if (oDbDataReader["Size"] != DBNull.Value)
                        oPackagingAssoDetails.Size = Convert.ToInt32(oDbDataReader["Size"]);

                    if (oDbDataReader["BundleQty"] != DBNull.Value)
                        oPackagingAssoDetails.BundleQty = Convert.ToInt32(oDbDataReader["BundleQty"]);

                    if (oDbDataReader["PPN"] != DBNull.Value)
                        oPackagingAssoDetails.PPN = Convert.ToString(oDbDataReader["PPN"]);

                    if (oDbDataReader["GTIN"] != DBNull.Value)
                        oPackagingAssoDetails.GTIN = Convert.ToString(oDbDataReader["GTIN"]);
                    if (oDbDataReader["NTIN"] != DBNull.Value)
                        oPackagingAssoDetails.NTIN = Convert.ToString(oDbDataReader["NTIN"]);

                    if (oDbDataReader["GTINCTI"] != DBNull.Value)
                        oPackagingAssoDetails.GTINCTI = Convert.ToString(oDbDataReader["GTINCTI"]); 
                    
                    if (oDbDataReader["MRP"] != DBNull.Value)
                        oPackagingAssoDetails.MRP = Convert.ToDecimal(oDbDataReader["MRP"]);


                    if (oDbDataReader["TerCaseIndex"] != DBNull.Value)
                        oPackagingAssoDetails.TerCaseIndex = Convert.ToInt32(oDbDataReader["TerCaseIndex"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oPackagingAssoDetails.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["LastUpdatedDate"] != DBNull.Value)
                        oPackagingAssoDetails.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);


                    lstPackagingAssoDetailss.Add(oPackagingAssoDetails);
                }
                oDbDataReader.Close();
                return lstPackagingAssoDetailss;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public PackagingAssoDetails GetPckAssoDtls(int Flag, string param, string PackTypeCode)
        {
            try
            {
                PackagingAssoDetails oPackagingAssoDetails = new PackagingAssoDetails();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackagingAssoDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, param));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PckType", DbType.String, PackTypeCode));

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    oPackagingAssoDetails.PAID = Convert.ToDecimal(oDbDataReader["PAID"]);

                    if (oDbDataReader["PackageTypeCode"] != DBNull.Value)
                        oPackagingAssoDetails.PackageTypeCode = Convert.ToString(oDbDataReader["PackageTypeCode"]);

                    if (oDbDataReader["Size"] != DBNull.Value)
                        oPackagingAssoDetails.Size = Convert.ToInt32(oDbDataReader["Size"]);

                    if (oDbDataReader["BundleQty"] != DBNull.Value)
                        oPackagingAssoDetails.BundleQty = Convert.ToInt32(oDbDataReader["BundleQty"]);

                    if (oDbDataReader["PPN"] != DBNull.Value)
                        oPackagingAssoDetails.PPN = Convert.ToString(oDbDataReader["PPN"]);

                    if (oDbDataReader["GTIN"] != DBNull.Value)
                        oPackagingAssoDetails.GTIN = Convert.ToString(oDbDataReader["GTIN"]); 

                    if (oDbDataReader["GTINCTI"] != DBNull.Value)
                        oPackagingAssoDetails.GTINCTI = Convert.ToString(oDbDataReader["GTINCTI"]);

                    if (oDbDataReader["MRP"] != DBNull.Value)
                        oPackagingAssoDetails.MRP = Convert.ToDecimal(oDbDataReader["MRP"]);

                    if (oDbDataReader["TerCaseIndex"] != DBNull.Value)
                        oPackagingAssoDetails.TerCaseIndex = Convert.ToInt32(oDbDataReader["TerCaseIndex"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oPackagingAssoDetails.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["LastUpdatedDate"] != DBNull.Value)
                        oPackagingAssoDetails.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);

                }
                oDbDataReader.Close();
                return oPackagingAssoDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
