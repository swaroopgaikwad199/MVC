using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;
using System.Data.Common;

namespace REDTR.DB.BLL
{
    public partial class PerformSyncBLL
    {
        private PerformDBSync _PerformDBSync;
        public PerformDBSync PerformDBSync
        {
            get { return _PerformDBSync; }
            set { _PerformDBSync = value; }
        }

        public enum Op
        {
            GetPackagingDetailsJobWise=4
        }

        public PerformSyncBLL()
        {
            PerformDBSync = new PerformDBSync();
        }
        public void OpenDBConnection(string ConnectionString)
        {
            try
            {
                PerformDBSync.ConnectionString = ConnectionString;
                PerformDBSync.OpenConnection();  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool IsOpen()
        {
            return PerformDBSync.IsOpen(); 
        }
        public int InsertUpdatePackagingDetailsForDataSync(int flag, PackagingDetails Pck,decimal JID,string ProdName,string UserName)
        {
            try
            {
                return PerformDBSync.InsertOrUpdatePackagingDetailsForSync(flag, Pck,JID,ProdName,UserName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateCustomerForDataSync(int flag,M_Customer cust)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateCustomerForDataSync(flag, cust);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdateCustomerForDataSyncPUSHTOGLOBAL(int flag, M_Customer cust)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateCustomerForDataSyncPUSHTOGLOBAL(flag, cust);
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
                return PerformDBSync.InsertOrUpdateProductApplicatorSetting(pro);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ProductGlueSetting GetProductGlueSetting(decimal Flag)
        {
            try
            {
                return PerformDBSync.GETProductGlueSetting(Flag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateProductGlueSetting(ProductGlueSetting pro)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateProductGlueSetting(pro);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdatePackagingAssoForSync(PackagingAsso Pck)
        {
            try
            {
                return PerformDBSync.InsertOrUpdatePackagingAssoForSync(1, Pck); //....0 is replaced by 1. 09 Oct 2014
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdatePackagingAssoForSyncPushGlobal(PackagingAsso Pck)
        {
            try
            {
                return PerformDBSync.InsertOrUpdatePackagingAssoForSyncPushToGlobal(1, Pck); //....0 is replaced by 1. 09 Oct 2014
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdatePckAssoDetailsForSync(PackagingAssoDetails Pck,decimal PAID)
        {
            try
            {
                return PerformDBSync.InsertOrUpdatePckAssoDetailsForSync(1, Pck,PAID); //....0 is replaced by 1. 09 Oct 2014
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Job> GetJobs(int Flag)
        {
            try
            {
                return PerformDBSync.GetJobs(Flag);
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
                return PerformDBSync.GetProductAppicatorSetting(Flag);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdatePckLabelForSync(PackageLabelAsso Pck,decimal PAID)
        {
            try
            {
                return PerformDBSync.InsertOrUpdatePckLabelForSync(1, Pck, PAID); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateJobForSync(Job Pck, string ProductName, string CreatedByName, string VeryfiedByName,int flag)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateJobForSync(flag, Pck, ProductName, CreatedByName, VeryfiedByName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public int InsertOrUpdateJobDetailsForSync(int flag, JobDetails Pck,decimal JID)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateJobDetailsForSync(flag, Pck, JID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateSupplierUIDsInStoreForSync(Supplier Pck)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateSupplierUIDsInStoreForSync(0, Pck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertUpdateChinaUIDForSync(ChinaUID Pck, int flag,decimal JID)
        {
            try
            {
                //return PerformDBSync.InsertUpdateChinaUIDForSync(Pck, flag, JID);
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateSSCCLineHolder(SSCCLineHolder Pck)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateSSCCLineHolder(Pck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateUSerTrail(USerTrail Pck, string LineCode)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateUSerTrail(Pck, LineCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdateUserDetailsForSync(Users Pck, int falg)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateUserDetails(Pck, falg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdateUserDetailsForSyncGlobalServer(Users Pck, int falg)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateUserDetailsForGlobalServer(Pck, falg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PackagingDetails> GetPackagingDetailss(int Flag, string ParamVal, string ParamVal1)
        {
            try
            {
                return PerformDBSync.GetPackagingDetailssFromServer(Flag,ParamVal,ParamVal1,null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // 30.12.2014 by sunil to retrive al batches from for respective line from client computer.

        public DataSet GetFinishedJobDetailDataSetFromClient(int Flag,string LineCode)
        {
            try
            {
                return PerformDBSync.GetFinishedJobDetailView(Flag,LineCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Users> GetUsersFromServer(int Flag)
        {
            try
            {
                return PerformDBSync.GetUsersFromServer(Flag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Users GetUsersFromGlobalServer(int Flag,Users user)
        {
            try
            {
                return PerformDBSync.GetUserFromGlobalServer(Flag, user);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        // End Here.

        public int InsertOrUpdateJobTypeForSync(JOBType JbType)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateJobTypeForSync(JbType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateREC_Count(int Flag, int jobid, int BCnt, int LCnt, int WCnt, int SCnt, int MTCnt, int MVCnt, int MLPCnt, int MCBCnt)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateREC_CountForSync(Flag, jobid, BCnt, LCnt, WCnt, SCnt, MTCnt, MVCnt, MLPCnt, MCBCnt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateLineLocationForDataSync(LineLocation oLineLocation)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateLineLocationForDataSync(oLineLocation);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateJobForSyncToGlobal(Job Pck, string ProductName, string CreatedByName, string VeryfiedByName, int flag)
        {
            try
            {
                return PerformDBSync.InsertOrUpdateJobForSyncToGlobal(flag, Pck, ProductName, CreatedByName, VeryfiedByName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveOLD_PckRecords(string Qry)
        {
            try
            {
                return PerformDBSync.ExecuteQuery(Qry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteQuery(string Qry)
        {
            try
            {
                return PerformDBSync.ExecuteQuery(Qry);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
