using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace TnT.DataLayer
{
    public class Response
    {

        public static DataTable getData(int RequestId)
        {
            try
            {
                string QRY2 = @"   SELECT DISTINCT dbo.X_Code.Code FROM            dbo.X_Code INNER JOIN
                         dbo.PackageTypeCode ON dbo.X_Code.CodeType = dbo.PackageTypeCode.CodeSeq INNER JOIN
                         dbo.M_RequestLog INNER JOIN
                         dbo.Job ON dbo.M_RequestLog.JobId = dbo.Job.JID INNER JOIN
                         dbo.PackagingAssoDetails ON dbo.Job.PAID = dbo.PackagingAssoDetails.PAID ON dbo.X_Code.RequestId = dbo.M_RequestLog.id
                         WHERE(dbo.X_Code.RequestId = " + RequestId + ")";

                string qry = @"      SELECT DISTINCT dbo.X_Code.Code, dbo.PackageTypeCode.Code AS Type, dbo.M_RequestLog.MfgDate, dbo.M_RequestLog.ExpDate, dbo.M_RequestLog.BatchNo
                            FROM            dbo.X_Code INNER JOIN
                         dbo.PackageTypeCode ON dbo.X_Code.CodeType = dbo.PackageTypeCode.CodeSeq INNER JOIN
                         dbo.M_RequestLog INNER JOIN
                         dbo.Job ON dbo.M_RequestLog.JobId = dbo.Job.JID INNER JOIN
                         dbo.PackagingAssoDetails ON dbo.Job.PAID = dbo.PackagingAssoDetails.PAID ON dbo.X_Code.RequestId = dbo.M_RequestLog.id
                        WHERE        (dbo.X_Code.RequestId = " + RequestId + ")";
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, QRY2);
                return ds.Tables[0];
            }
            catch (Exception)
            {

                throw;
            }
        


        }

        public DataTable getSSCCCode(int RequestId)
        {
            try
            {
                string QRY2 = @"SELECT DISTINCT dbo.X_Code.Code
                                FROM dbo.X_Code INNER JOIN
                                dbo.PackageTypeCode ON dbo.X_Code.CodeType = dbo.PackageTypeCode.CodeSeq INNER JOIN
                                dbo.M_RequestLog INNER JOIN
                                dbo.Job ON dbo.M_RequestLog.JobId = dbo.Job.JID INNER JOIN
                                dbo.PackagingAssoDetails ON dbo.Job.PAID = dbo.PackagingAssoDetails.PAID ON dbo.X_Code.RequestId = dbo.M_RequestLog.id
                                WHERE(dbo.X_Code.RequestId = " + RequestId + ") AND(dbo.PackageTypeCode.Code = 'SSC')";
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, QRY2);

                var strData = convetDatatableToString(ds.Tables[0]);
                var dt = convertStringToDataTable(strData);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable getGTINCode(int RequestId)
        {
            try
            {
                string QRY2 = @"SELECT DISTINCT TOP (6) dbo.PackagingAssoDetails.PackageTypeCode, dbo.PackagingAssoDetails.GTIN, dbo.PackagingAssoDetails.LastUpdatedDate
                                FROM            dbo.PackagingAssoDetails INNER JOIN
                                 dbo.Job ON dbo.PackagingAssoDetails.PAID = dbo.Job.PAID INNER JOIN
                                dbo.M_RequestLog ON dbo.Job.JID = dbo.M_RequestLog.JobId
                                WHERE (dbo.M_RequestLog.id = "+RequestId+")";
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, QRY2);
                var strData =  convetGTINDatatableToString(ds.Tables[0]);
                var dt = convertStringToDataTable(strData);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region for UID Validation
        public string getGTINCodeforVldtn(int RequestId)
        {
            try
            {
                string QRY2 = @"SELECT DISTINCT TOP (6) dbo.PackagingAssoDetails.PackageTypeCode, dbo.PackagingAssoDetails.GTIN, dbo.PackagingAssoDetails.LastUpdatedDate
                                FROM            dbo.PackagingAssoDetails INNER JOIN
                                 dbo.Job ON dbo.PackagingAssoDetails.PAID = dbo.Job.PAID INNER JOIN
                                dbo.M_RequestLog ON dbo.Job.JID = dbo.M_RequestLog.JobId
                                WHERE (dbo.M_RequestLog.id = " + RequestId + ")";
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, QRY2);
                return convetGTINDatatableToString(ds.Tables[0]);
             
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string getSSCCCodeforVldtn(int RequestId)
        {
            try
            {
                string QRY2 = @"SELECT DISTINCT dbo.X_Code.Code
                                FROM dbo.X_Code INNER JOIN
                                dbo.PackageTypeCode ON dbo.X_Code.CodeType = dbo.PackageTypeCode.CodeSeq INNER JOIN
                                dbo.M_RequestLog INNER JOIN
                                dbo.Job ON dbo.M_RequestLog.JobId = dbo.Job.JID INNER JOIN
                                dbo.PackagingAssoDetails ON dbo.Job.PAID = dbo.PackagingAssoDetails.PAID ON dbo.X_Code.RequestId = dbo.M_RequestLog.id
                                WHERE(dbo.X_Code.RequestId = " + RequestId + ") AND(dbo.PackageTypeCode.Code = 'SSC')";
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, QRY2);
                 return convetDatatableToString(ds.Tables[0]);
                //return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string getUIDsforVldtn(int RequestId)
        {
            try
            {
                string QRY2 = @"SELECT DISTINCT dbo.X_Code.Code
                                FROM            dbo.X_Code INNER JOIN
                         dbo.PackageTypeCode ON dbo.X_Code.CodeType = dbo.PackageTypeCode.CodeSeq INNER JOIN
                         dbo.M_RequestLog INNER JOIN
                         dbo.Job ON dbo.M_RequestLog.JobId = dbo.Job.JID INNER JOIN
                         dbo.PackagingAssoDetails ON dbo.Job.PAID = dbo.PackagingAssoDetails.PAID ON dbo.X_Code.RequestId = dbo.M_RequestLog.id
                                WHERE        (dbo.X_Code.RequestId = " + RequestId + ") AND (dbo.PackageTypeCode.Code <> 'SSC')";
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, QRY2);
                return convetDatatableToString( ds.Tables[0]);
                //return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        public DataTable getUIDs(int RequestId)
        {
            try
            {
                string QRY2 = @"SELECT DISTINCT dbo.X_Code.Code
                                FROM            dbo.X_Code INNER JOIN
                         dbo.PackageTypeCode ON dbo.X_Code.CodeType = dbo.PackageTypeCode.CodeSeq INNER JOIN
                         dbo.M_RequestLog INNER JOIN
                         dbo.Job ON dbo.M_RequestLog.JobId = dbo.Job.JID INNER JOIN
                         dbo.PackagingAssoDetails ON dbo.Job.PAID = dbo.PackagingAssoDetails.PAID ON dbo.X_Code.RequestId = dbo.M_RequestLog.id
                                WHERE        (dbo.X_Code.RequestId = "+RequestId+") AND (dbo.PackageTypeCode.Code <> 'SSC')";
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, QRY2);
                var strData = convetDatatableToString(ds.Tables[0]);
                var dt = convertStringToDataTable(strData);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private string convetGTINDatatableToString(DataTable dt)
        {
           
            try
            {
                string output = "";
                if (dt.Rows.Count > 0 )
                {
                   
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        output = output + dt.Rows[i][0].ToString() + " : ";
                        output += dt.Rows[i][1].ToString();

                        output += (i < dt.Rows.Count) ? "," : string.Empty;
                    }
                    return output;
                }
                else
                {
                    return output;
                }
             
            }
            catch (Exception)
            {

                throw;
            }           
        }
        private string convetDatatableToString(DataTable dt)
        {

            try
            {
                string output = "";
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        output = output + dt.Rows[i]["Code"].ToString();
                        output += (i < dt.Rows.Count) ? "," : string.Empty;
                    }
                    return output;
                }
                else
                {
                    return output;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable convertStringToDataTable(string data)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Code",typeof(string));
            var Ids = data.Split(',');

            foreach (var item in Ids)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = item;
                    dataTable.Rows.Add(dataRow);
                }
            }           
            return dataTable;
        }


    }
}