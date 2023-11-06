using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
    public partial class SupplierDAO
    {
        public SupplierDAO()
        {
            DbProviderHelper.GetConnection();
        }

        public int insertOrUpdateSupplierUIDs(int flag, Supplier oSupplier)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateSupplierUIDs]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag",DbType.Decimal,flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID",DbType.String,oSupplier.ID1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String, oSupplier.UIDs1));
      
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SDate", DbType.String, oSupplier.SDate1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SFlag", DbType.Boolean, oSupplier.SFlag1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchNo", DbType.String, oSupplier.BatchNo1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransNo", DbType.Int32, oSupplier.TransNo1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SupplierName", DbType.String, oSupplier.SupplierName1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductName", DbType.String, oSupplier.ProductName1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FirstId", DbType.Int32, oSupplier.FirstId1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountToCompare", DbType.Int32, oSupplier.CountToCompare1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.String, oSupplier.Status1));

                return DbProviderHelper.ExecuteNonQuery(oDbCommand);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int insertOrUpdateSupplierUIDsInStore(int flag, Supplier oSupplier,int PAID)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateInStore]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, oSupplier.ID1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String, oSupplier.UIDs1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.Boolean, oSupplier.SFlag1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SDate", DbType.DateTime,null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackageTypeCode", DbType.String, oSupplier.PackageTypeCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Int32, PAID));

                return DbProviderHelper.ExecuteNonQuery(oDbCommand);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int getUIDCount(int flag, bool SFlag)
        {
            try
            {
                Supplier sp = new Supplier();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GETUIDCount", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SFlag", DbType.Boolean, SFlag));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
               
                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["CNT"] != DBNull.Value)
                        sp.RecordCount1 = Convert.ToInt32(oDbDataReader["CNT"]);

                }
                return sp.RecordCount1;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int getTopID(int flag, bool SFlag)
        {
            try
            {
                Supplier sp = new Supplier();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GETUIDCount", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SFlag", DbType.Boolean, SFlag));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);

                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["First_ID"] != DBNull.Value)
                        sp.FirstID1 = Convert.ToInt32(oDbDataReader["First_ID"]);

                }
                return sp.FirstID1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateSupplierUIDs(int flag, Supplier oSupplier)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateSupplierUIDs]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, oSupplier.ID1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SDate", DbType.String, oSupplier.SDate1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SFlag", DbType.Boolean, oSupplier.SFlag1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchNo", DbType.String, oSupplier.BatchNo1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransNo", DbType.Int32, oSupplier.TransNo1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SupplierName", DbType.String, oSupplier.SupplierName1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductName", DbType.String, oSupplier.ProductName1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FirstId", DbType.Int32, oSupplier.FirstId1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountToCompare", DbType.Int32, oSupplier.CountToCompare1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.String, oSupplier.Status1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String, null));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        //public static int UpdateSupplierUIDs(int p, bool p_2)
        //{
        //    throw new NotImplementedException();
        //}

        public List<Supplier> GetCheckedUIDs(int flag, string paramValue)
        {
            try
            {
                List<Supplier> lstSupplier = new List<Supplier>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateSupplierUIDs]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32,0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SDate", DbType.String,null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SFlag", DbType.Boolean,false));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchNo", DbType.String,paramValue));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransNo", DbType.String,null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SupplierName", DbType.String,null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductName", DbType.String,null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FirstId", DbType.Int32,0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountToCompare", DbType.Int32,0));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, paramVal));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.String,null));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    Supplier oSupplier = new Supplier();

                    if (oDbDataReader["UIDs"] != DBNull.Value)
                        oSupplier.UIDs1 = Convert.ToString(oDbDataReader["UIDs"]);
                    lstSupplier.Add(oSupplier);
                }
                oDbDataReader.Close();
                return lstSupplier;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetmaxTransNo(int flag)
        {
            try
            {
                Supplier sp = new Supplier();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_InsertOrUpdateSupplierUIDs", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SDate", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SFlag", DbType.Boolean, false));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchNo", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransNo", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SupplierName", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductName", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FirstId", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountToCompare", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String,null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.String,null));

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["Max_TransNo"] != DBNull.Value)
                        sp.TransNo1 = Convert.ToString(oDbDataReader["Max_TransNo"]);

                }
                return sp.TransNo1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Supplier> GetCheckedUIDsWithTransNo(int flag,Supplier Supplier1)
        {
            try
            {
                List<Supplier> lstSupplierUIDs = new List<Supplier>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateSupplierUIDs]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SDate", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SFlag", DbType.Boolean,false));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchNo", DbType.String,Supplier1.BatchNo1));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransNo", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SupplierName", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductName", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FirstId", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountToCompare", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransNo", DbType.String,null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.String,Supplier1.Status1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String, null));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    Supplier oSupplier = new Supplier();

                    if (oDbDataReader["UIDs"] != DBNull.Value)
                        oSupplier.UIDs1 = Convert.ToString(oDbDataReader["UIDs"]);
                    lstSupplierUIDs.Add(oSupplier);
                }
                oDbDataReader.Close();
                return lstSupplierUIDs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateUIDStatus(int flag, Supplier oSupplier)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateSupplierUIDs]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.String, oSupplier.Status1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String, oSupplier.UIDs1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SDate", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SFlag", DbType.Boolean, false));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchNo", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransNo", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SupplierName", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductName", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FirstId", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountToCompare", DbType.Int32, 0));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.Boolean, oSupplier.Status1));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Supplier> GetAllData(int flag)
        {
            try
            {
                List<Supplier> lstBatchName = new List<Supplier>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateSupplierUIDs]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SDate", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SFlag", DbType.Boolean,1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchNo", DbType.String,0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransNo", DbType.String, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SupplierName", DbType.String, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductName", DbType.String, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FirstId", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountToCompare", DbType.Int32, 0));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, paramVal));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.String, 0));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    Supplier oSupplier1 = new Supplier();

                    if (oDbDataReader["Batch_No"] != DBNull.Value)
                        oSupplier1.BatchNo1 = Convert.ToString(oDbDataReader["Batch_No"]);
                    lstBatchName.Add(oSupplier1);
                }
                oDbDataReader.Close();
                return lstBatchName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetUIDResult(int flag,Supplier Supplier1)
        {
            try
            {
                Supplier sp = new Supplier();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_InsertOrUpdateSupplierUIDs", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SDate", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SFlag", DbType.Boolean, false));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchNo", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TransNo", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SupplierName", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductName", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FirstId", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountToCompare", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String,Supplier1.UIDs1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.String, null));

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetUIDResultFromStore(int Flag,string ParamValue)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateInStore]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal,Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32,0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String,ParamValue));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.Boolean,false));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SDate", DbType.DateTime, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackageTypeCode", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Int32, 0));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }


        public List<string> GetAllUIDsFromStore(int Flag)
        {
            try
            {
                List<string> lstUIDs = new List<string>();
                
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateInStore]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@UID", DbType.String,null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Status", DbType.Boolean, false));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SDate", DbType.DateTime, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackageTypeCode", DbType.String, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Int32, 0));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);

                while (oDbDataReader.Read())
                {
                    //Supplier oSupplier1 = new Supplier();
                    string UID = "";

                    if (oDbDataReader["UID"] != DBNull.Value)
                        UID = Convert.ToString(oDbDataReader["UID"]);
                    lstUIDs.Add(UID);
                }
                oDbDataReader.Close();
                return lstUIDs;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //...28/06/2014 Sunil Jadhav

        public int InsertOrUpdateSupplierMaster(int Flag, Supplier oSupplier)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateSupplierMaster]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchId", DbType.Int32,oSupplier.BatchId1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchName", DbType.String, oSupplier.BatchNo1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SupplierId", DbType.Int32, oSupplier.SupplierId1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductId", DbType.Int32,oSupplier.ProdId1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchQuantity", DbType.Int32, oSupplier.BatchQuantity1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SurPlusQuantity", DbType.Int32, oSupplier.SurPlusQuantity1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedDate", DbType.DateTime, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, ParameterDirection.Output, "", DataRowVersion.Current, true, DBNull.Value));
                DbProviderHelper.ExecuteNonQuery(oDbCommand);

                if (oDbCommand.Parameters["@ID"].Value != null)
                    return Convert.ToInt32(oDbCommand.Parameters["@ID"].Value);
                else
                    return 0;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        public List<string> GetAllBatchNamesFromSupplierMAster(int Flag)
        {
            try
            {
                List<string> lstBatchNames = new List<string>();

                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateSupplierMaster]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchId", DbType.Int32,0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchName", DbType.String,null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SupplierId", DbType.Int32,0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductId", DbType.Int32,0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchQuantity", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SurPlusQuantity", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedDate", DbType.DateTime, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, 0));


                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);

                while (oDbDataReader.Read())
                {
                    string BatchName = "";

                    if (oDbDataReader["BatchName"] != DBNull.Value)
                        BatchName = Convert.ToString(oDbDataReader["BatchName"]);
                    lstBatchNames.Add(BatchName);
                }
                oDbDataReader.Close();
                return lstBatchNames;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int GetBatchIdFromBatchName(int Flag, string paramValue)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateSupplierMaster]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchId", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchName", DbType.String, paramValue));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SupplierId", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ProductId", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchQuantity", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@SurPlusQuantity", DbType.Int32, 0));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedDate", DbType.DateTime, null));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, 0));


                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                Int32 BatchId = 0;
                while (oDbDataReader.Read())
                {


                    if (oDbDataReader["BatchId"] != DBNull.Value)
                        BatchId = Convert.ToInt32(oDbDataReader["BatchId"]);
                    //return BatchId;
                }
                oDbDataReader.Close();
                return BatchId;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
