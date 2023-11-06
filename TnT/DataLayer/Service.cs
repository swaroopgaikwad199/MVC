using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using TnT.Models;

namespace TnT.DataLayer
{
    public class Service
    {

        public bool IsKeyValid(string Key)
        {
            try
            {
                string qry = @"SELECT  ServiceKey FROM dbo.M_Vendor 
                                WHERE(ServiceKey = N'" + Key + "') AND (IsActive = 'True')";
              
                return BoolExecuteQuery(qry);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsJobIdExisting(int jobId)
        {
            try
            {
                string qry = @"SELECT        JID FROM            dbo.Job  WHERE        (JID = " + jobId + ")";
                return BoolExecuteQuery(qry);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool IsJobIdExistinginLog(int jobId)
        {
            try
            {
                string qry = @"SELECT  JobId FROM dbo.M_RequestLog  WHERE        (JobId = " + jobId + ")";
                return BoolExecuteQuery(qry);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsBatchExisting(string BatchNo)
        {
            try
            {
                string qry = @"SELECT BatchNo FROM dbo.Job WHERE (JID = '" + BatchNo + "')";
                return BoolExecuteQuery(qry);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsRequestValid(string ServiceKey ,decimal JobId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                if (!db.M_RequestLog.Any(m => m.ServiceKey == ServiceKey && m.JobId == JobId))
                {
                    return true;
                }
                else
                {
                    return false;
                }              

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsProductValid(int jobId, string BatchNo, DateTime Mfg, DateTime Expiry)
        {
            try
            {
                string qry = @"SELECT JobName FROM dbo.Job WHERE   
                                (JID = " + jobId + ") AND (MfgDate = '" + Mfg + "') AND (ExpDate = '" + Expiry + "') AND (BatchNo = '" + BatchNo + "')";
                return BoolExecuteQuery(qry);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsProductValid(int jobId, string BatchNo)
        {
            try
            {
                string qry = @"SELECT JobName FROM dbo.Job WHERE   
                                (JID = " + jobId + ") AND (BatchNo = '" + BatchNo + "')";
                return BoolExecuteQuery(qry);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsProductExisting(int PAID)
        {
            try
            {
                string qry = @"select * from PackagingAsso where PAID="+PAID+"";
                return BoolExecuteQuery(qry);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool BoolExecuteQuery(string query)
        {
            try
            {
                string connectionString;
                connectionString = Utilities.getConnectionString("DefaultConnection");
                DataSet ds = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, query);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}