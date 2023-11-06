using TnT.Models;
using REDTR.DB.BLL;
using REDTR.DB.BusinessObjects;
using REDTR.HELPER;
using REDTR.JOB;
using REDTR.UTILS;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TnT.Models;
using System.Configuration;
using TnT.Controllers;

namespace TnT.DataLayer
{

    public class TransferJobHelper 
    {
        public  string JobName;
        public  int OverallProgress;

        private decimal JobId;
        private string LineId;
        string Msg;
        private ApplicationDbContext db = new ApplicationDbContext();
        DbHelper dbhelper = new DbHelper();



        public int? percent = null;
        int PBMinimum = 0;
        int PBMaximum = 100;
        int PB = 0;
        int PBCounter = 0;
        string graphicString;


        #region Data Members of Data Sync for Line Computer

        List<LineImplementation> Lst_LineDetails = new List<LineImplementation>();
        List<Job> oClientLst_Job = new List<Job>();
        List<JobDetails> oClientLst_JobDetails = new List<JobDetails>();
        List<ChinaUID> oClientLst_ChinaUID = new List<ChinaUID>();
        List<PackagingDetails> oClientLst_PackagingDetails = new List<PackagingDetails>();
        PerformSyncBLL Obj_perfSync = new PerformSyncBLL();
        List<Users> oClientLst_Users = new List<Users>();
        List<PackagingAsso> oClientLst_PackagingAsso = new List<PackagingAsso>();
        List<PackagingAssoDetails> oClientLst_PackagingAssoDetails = new List<PackagingAssoDetails>();
        List<JOBType> oClientLst_JobTypeDetails = new List<JOBType>();
        List<PackageLabelAsso> oClientLst_PckLabels = new List<PackageLabelAsso>();

        string ConnectionStr = string.Empty;
        decimal? Client_Jid = null;
        int? Client_JtypeId = null;
        string server = string.Empty;
        #endregion Data Members of Data Sync for Line Computer


        public bool TransferJob(decimal JobId, string LineId)
        {
            
            this.JobId = JobId;
            this.LineId = LineId;
            var Line = db.LineLocation.Find(LineId);
             server = "Line";
            ConnectionStr = @"Data Source=" + Line.LineIP + ";" + "Initial Catalog=" + Line.DBName + ";Persist Security Info=True;User ID=" + Line.SQLUsername + ";Password=" + Line.SQLPassword + ";MultipleActiveResultSets=True";
            return processJobTransfer(server);
        }

        //public bool TransferJobToGlobal(decimal JobId, string connection)
        //{

        //    this.JobId = JobId;
        //    ConnectionStr = connection;
        //     server = "Global";
        //        return processJobTransferToGlobalServer(server);
        //}

        public bool TransferJobToGlobalServer(decimal JobId, string connection,int LoginId)
        {

            this.JobId = JobId;
            ConnectionStr = connection;
            server = "Global";

            return processJobTransferToGlobalServer(server,LoginId);
        }

        private bool processJobTransfer(string server)
        {
            bool IsTemperEvidence = Convert.ToBoolean(Utilities.getAppSettings("TemperEvidence"));//; Utilities.getAppSettings("TemperEvidence"]
            bool isGlobal = Convert.ToBoolean(Utilities.getAppSettings("IsGlobalServer")); //Utilities.getAppSettings("IsGlobalServer"]
            int Custid = 0;
            Job jb = new Job();
            Obj_perfSync.OpenDBConnection(ConnectionStr);
            if (Obj_perfSync.IsOpen())
            {
                PBCounter++;
                percent = (int)(((double)PB / (double)PBMaximum) * 100);
               
                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.ShwMsgCurrentProcess  + " : " + JobName, OverallProgress, (int)percent);
                jb.JID = JobId;
                jb.LineCode = LineId;

                jb.JobStatus = (sbyte)JobState.Running;

                PBCounter++;
                percent = (int)(((double)PB / (double)PBMaximum) * 100);
               
                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.ShwMsgCurrentProcess + ": " + JobName, OverallProgress, (int)percent);
                if (server == "Line")
                {
                    dbhelper.DBManager.JobBLL.UpdateJobDetails(JobBLL.JobOp.UpdateJobStatus, jb);
                }
                else
                {
                    var lineid = db.Job.Where(x => x.JID == JobId).FirstOrDefault();
                    string lid = lineid.LineCode;
                    LineLocation linedata = dbhelper.DBManager.LineLocationBLL.GetLineLocationss(lid);
                    Obj_perfSync.InsertOrUpdateLineLocationForDataSync(linedata);
                    jb.LineCode = lid;
                    LineId = lid;
                }


                oClientLst_Job = dbhelper.DBManager.JobBLL.GetJobs(JobBLL.JobOp.GetAllJobs, 1, "", "");
                oClientLst_Users = dbhelper.DBManager.UsersBLL.GetUserss(UsersBLL.UsersOp.GetUserss, "");
                oClientLst_PackagingAsso = dbhelper.DBManager.PackagingAssoBLL.GetPackagingAssos();
                oClientLst_JobTypeDetails = dbhelper.DBManager.JOBTypeBLL.GetJOBTypes();
                oClientLst_PckLabels = dbhelper.DBManager.PackageLabelBLL.GetPackagingLabelAssos(Convert.ToInt32(PackageLabelAssoBLL.PackageLabelOp.GetAllPackageLabelDetails), null, null);
                M_Customer oClientLst_Customer = new M_Customer();
            


                Job ObjJob = new Job();
                ObjJob = oClientLst_Job.Find(item => item.JID == jb.JID);
                oClientLst_Customer = dbhelper.DBManager.M_CustomerBLL.GetCustomer(1, Convert.ToString(ObjJob.CustomerId));
                PackagingAsso ObjPackagingAsso = new PackagingAsso();
                ObjPackagingAsso = oClientLst_PackagingAsso.Find(item => item.PAID == ObjJob.PAID);

                JOBType objJobTypes = new JOBType();
                objJobTypes = oClientLst_JobTypeDetails.Find(item => item.TID == ObjJob.TID);

                Users ObjUsers = new Users();
                ObjUsers = oClientLst_Users.Find(item => item.ID == ObjJob.CreatedBy);

                Users ObjUsers1 = new Users();
                ObjUsers1 = oClientLst_Users.Find(item => item.ID == ObjJob.VerifiedBy);

                string CreatedBy = string.Empty;
                string VerifiedBy = string.Empty;
                string User = string.Empty;
                string QA = string.Empty;
                if (ObjUsers != null)
                {
                    CreatedBy = ObjUsers.UserName;
                    User = ObjUsers.UserName;
                }

                if (ObjUsers1 != null)
                {
                     VerifiedBy = ObjUsers1.UserName;
                    QA = ObjUsers1.UserName;
                }
                string ProductName = ObjPackagingAsso.Name;
                
               
                decimal PAID = 0;

                List<PackageLabelAsso> PackLabels = new List<PackageLabelAsso>();

                try
                {
                    Trace.TraceInformation("{0},{1},{2},{3},{4}", DateTime.Now.ToString(), ObjUsers.UserName, ObjUsers.ID, ObjUsers1.UserName, ObjUsers1.ID);
                    Obj_perfSync.InsertOrUpdateUserDetailsForSync(ObjUsers, (int)UsersBLL.UsersOp.AddUserForSync);
                    Obj_perfSync.InsertOrUpdateUserDetailsForSync(ObjUsers1, (int)UsersBLL.UsersOp.AddUserForSync);
                    Trace.TraceInformation("Transfer done for both users");
                }
                catch (Exception ex)
                {
                    ExceptionHandler.ExceptionLogger.logException(ex);
                    Trace.TraceInformation(DateTime.Now.ToString() + " " + ex.Message + " " + ex.StackTrace);
                }

                try
                {
                    //if(!isGlobal)
                  /*  Custid =*/ Obj_perfSync.InsertOrUpdateCustomerForDataSync(1, oClientLst_Customer);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.ExceptionLogger.logException(ex);
                    Trace.TraceInformation(DateTime.Now.ToString() + " " + ex.Message + " " + ex.StackTrace);
                }

                //dbhelper.AddUserTrail(ObjUsers.ID, ObjUsers1.ID, USerTrailWHERE.TnT, USerTrailWHAT.DATA_SYNC, USerTrailWHY.DATA_SYNC_TO_CLIENT, "Status " + "'" + jb.JobStatus.ToString() + "'" + " updated to plant server for job " + ObjJob.BatchNo);
                Trace.TraceInformation("Update job status at plant server=>{0},{1},{2},{3}", "Status updation at plant server", DateTime.Now.ToString(), jb.JobStatus.ToString(), ObjJob.BatchNo);

                //.....Here job table is added from server to client side.
                ObjJob.LineCode = LineId;
                //ObjJob.LineName = JobShift.LineName;
                ObjJob.JobStatus = 1; // 08Sept2015 sunil temporary

                Client_JtypeId = Obj_perfSync.InsertOrUpdateJobTypeForSync(objJobTypes);

                //dbhelper.AddUserTrail(ObjUsers.ID, ObjUsers1.ID, USerTrailWHERE.TnT, USerTrailWHAT.DATA_SYNC, USerTrailWHY.DATA_SYNC_TO_CLIENT, "Job Type " + "'" + Client_JtypeId.ToString() + "'" + " updated to line server for jobtype named " + objJobTypes.Job_Type);
                Trace.TraceInformation("Update jobtype at line server=>{0},{1},{2},{3}", "jobtype updation at line server", DateTime.Now.ToString(), Client_JtypeId.ToString(), objJobTypes.Job_Type);

                //ObjJob.TID = Client_JtypeId;
                //Client_Jid = Obj_perfSync.InsertOrUpdateJobForSync(ObjJob, ProductName, CreatedBy, VerifiedBy, 1);

                //dbhelper.AddUserTrail(ObjUsers.ID, ObjUsers1.ID, USerTrailWHERE.TnT, USerTrailWHAT.DATA_SYNC, USerTrailWHY.DATA_SYNC_TO_CLIENT, "Status and job " + "'" + ObjJob.JobStatus.ToString() + "'" + " updated to line server for job " + ObjJob.BatchNo);
                Trace.TraceInformation("update job and status at line server=>{0},{1},{2},{3}", "Status and job updation at line server", DateTime.Now.ToString(), ObjJob.JobStatus.ToString(), ObjJob.BatchNo);
                
                //foreach (PackagingAsso prod in oClientLst_PackagingAsso)
                if (ObjPackagingAsso!=null && ObjPackagingAsso.PAID >0)
                {
                    
                        PAID = Obj_perfSync.InsertOrUpdatePackagingAssoForSync(ObjPackagingAsso);
                    if (IsTemperEvidence == true)
                    {
                        REDTR.DB.BusinessObjects.ProductApplicatorSetting OClientLst_ProductAppicatorSetting = new REDTR.DB.BusinessObjects.ProductApplicatorSetting();
                        OClientLst_ProductAppicatorSetting = dbhelper.DBManager.ProductApplicatorSettingBLL.GetProductApplicationSettind(ObjJob.PAID,1);
                        if (OClientLst_ProductAppicatorSetting != null)
                        {
                            OClientLst_ProductAppicatorSetting.ServerPAID = ObjPackagingAsso.PAID;
                            if (OClientLst_ProductAppicatorSetting.S1 == null)
                            {
                                OClientLst_ProductAppicatorSetting.S1 = 0;
                            }
                            if (OClientLst_ProductAppicatorSetting.S2 == null)
                            {
                                OClientLst_ProductAppicatorSetting.S2 = 0;
                            }
                            if (OClientLst_ProductAppicatorSetting.S3 == null)
                            {
                                OClientLst_ProductAppicatorSetting.S3 = 0;
                            }
                            if (OClientLst_ProductAppicatorSetting.S4 == null)
                            {
                                OClientLst_ProductAppicatorSetting.S4 = 0;
                            }
                            if (OClientLst_ProductAppicatorSetting.S5 == null)
                            {
                                OClientLst_ProductAppicatorSetting.S5 = 0;
                            }
                            if (OClientLst_ProductAppicatorSetting.FrontLabelOffset == null)
                            {
                                OClientLst_ProductAppicatorSetting.FrontLabelOffset = 0;
                            }
                            if (OClientLst_ProductAppicatorSetting.BackLabelOffset == null)
                            {
                                OClientLst_ProductAppicatorSetting.BackLabelOffset = 0;
                            }
                            if (OClientLst_ProductAppicatorSetting.CartonLength == null)
                            {
                                OClientLst_ProductAppicatorSetting.CartonLength = 0;
                            }
                            int i = Obj_perfSync.InsertOrUpdateProductApplicatorSetting(OClientLst_ProductAppicatorSetting);
                        }
                    }
                    oClientLst_PackagingAssoDetails = dbhelper.DBManager.PackagingAssoDetailsBLL.GetPckAssoDtlss(ObjPackagingAsso.PAID);
                    PackLabels = oClientLst_PckLabels.FindAll(item => item.PAID == ObjPackagingAsso.PAID);
                    foreach (PackagingAssoDetails PackAssDtls in oClientLst_PackagingAssoDetails)
                    {
                        //UpdateProgressBar(PBCounter);
                        Obj_perfSync.InsertOrUpdatePckAssoDetailsForSync(PackAssDtls, PAID);
                        PBCounter++;
                        percent = (int)(((double)PB / (double)PBMaximum) * 100);
                        ProgressHub.sendMessage(TnT.LangResource.GlobalRes.ShwMsgCurrentProcess  +" : " + JobName, OverallProgress, (int)percent);
                        //dbhelper.AddUserTrail(ObjUsers.ID, ObjUsers1.ID, USerTrailWHERE.TnT, USerTrailWHAT.DATA_SYNC, USerTrailWHY.DATA_SYNC_TO_CLIENT, "product" + "'" + prod.Name + "'" + " updated to line server");
                        Trace.TraceInformation("load packagingAsso and PackagingAssoDetails at line server=>{0},{1},{2}", "Product details updation at line server", DateTime.Now.ToString(), ObjPackagingAsso.Name);
                    }
                    foreach (PackageLabelAsso PckLblAssos in PackLabels)
                    {
                        Obj_perfSync.InsertOrUpdatePckLabelForSync(PckLblAssos, PAID);
                        //dbhelper.AddUserTrail(ObjUsers.ID, ObjUsers1.ID, USerTrailWHERE.TnT, USerTrailWHAT.DATA_SYNC, USerTrailWHY.DATA_SYNC_TO_CLIENT, "Product Label" + "'" + PckLblAssos.Code + "'" + " updated to line ");
                        Trace.TraceInformation("Updated packaging label line =>{0},{1},{2}", "Product Label Details updation at line", DateTime.Now.ToString(), ObjPackagingAsso.Name);
                    }
                }

                ObjJob.TID = Client_JtypeId;
                ObjJob.PAID = PAID;
                //if (Custid != 0)
                //{
                //    ObjJob.CustomerId = Custid;
                //}
                Client_Jid = Obj_perfSync.InsertOrUpdateJobForSync(ObjJob, ProductName, CreatedBy, VerifiedBy, 1);

                oClientLst_JobDetails = dbhelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, jb.JID, 1);
                foreach (JobDetails jDetails in oClientLst_JobDetails)
                {
                    //UpdateProgressBar(PBCounter);
                    Obj_perfSync.InsertOrUpdateJobDetailsForSync(1, jDetails, (decimal)Client_Jid);
                    //dbhelper.AddUserTrail(ObjUsers.ID, ObjUsers1.ID, USerTrailWHERE.TnT, USerTrailWHAT.DATA_SYNC, USerTrailWHY.DATA_SYNC_TO_CLIENT, "batch details of batch" + "'" + ObjJob.BatchNo + "'" + " updated to line server");
                    Trace.TraceInformation("Load Job details at Client/Line side from Server=>{0},{1},{2},{3},{4},{5},{6},{7},{8}", ObjJob.JobName, jDetails.JD_ProdName, ObjJob.BatchNo, ObjJob.Quantity, ObjJob.SurPlusQty, ObjJob.AutomaticBatchCloser, ObjJob.TID, ObjJob.JobWithUID, "LineName");
                    PBCounter++;
                    percent = (int)(((double)PB / (double)PBMaximum) * 100);
                    ProgressHub.sendMessage(TnT.LangResource.GlobalRes.ShwMsgCurrentProcess + " : " + JobName, OverallProgress, (int)percent);
                }


                try
                {
                    // string Query = "SELECT [PackDtlsID],[Code]," + PAID + " AS [PAID]," + Client_Jid + " AS [JobID],[PackageTypeCode],[MfgPackDate],[ExpPackDate],[NextLevelCode],[IsRejected],[Reason],[BadImage],[SSCC]," +
                    //     "[SSCCVarificationStatus],[IsManualUpdated],[ManualUpdateDesc],[CaseSeqNum],[OperatorId],[Remarks],[IsDecomission],[CreatedDate],[LastUpdatedDate]," +
                    //     "[LineCode],[SYNC],[RCResult],[DavaPortalUpload],[IsUsed] FROM [PackagingDetails] Where JobID =" + JobId.ToString();

                    var pkgdetail = db.PackagingDetails.Where(x => x.JobID == JobId).FirstOrDefault();
                    if (pkgdetail != null)
                    {
                        PackagingDetails pkg = new PackagingDetails();
                        pkg.JobID = pkgdetail.JobID;
                        pkg.SYNC = false;
                      

                        dbhelper.DBManager.PackagingDetailsBLL.InsertOrUpdatePackagingDetails(17, pkg);
                    }

                    string q2 = @"SELECT [PackDtlsID],[Code]  , "+PAID+" AS [PAID]      , "+Client_Jid+ " as [JobID]     ,[PackageTypeCode]      ,[MfgPackDate]      ,[ExpPackDate]       ,[NextLevelCode]      ,[IsRejected]      ,[Reason]      ,[BadImage]      ,[SSCC]       ,[SSCCVarificationStatus]      ,[IsManualUpdated]      ,[ManualUpdateDesc]      ,[CaseSeqNum]      ,[OperatorId]      ,[Remarks]      ,[IsDecomission]      ,[CreatedDate]      ,[LastUpdatedDate]      ,[LineCode]      ,[SYNC]      ,[RCResult]      ,[DavaPortalUpload]      ,[IsUsed] ,[IsLoose],TwoDGrade,FirstDeckCount,CryptoCode     FROM [dbo].[PackagingDetails] Where JobID =" + JobId.ToString();
                    DbDataReader oDbDataReader = dbhelper.GetReader(q2);
                    if (oDbDataReader != null)
                    {

                        // open the destination data
                        using (DbConnection destinationConnection = new SqlConnection(ConnectionStr))
                        {
                            // open the connection
                            destinationConnection.Open();

                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectionStr))
                            {
                                bulkCopy.BatchSize = 500;
                                bulkCopy.NotifyAfter = 1000;
                                bulkCopy.SqlRowsCopied +=
                                    new SqlRowsCopiedEventHandler(bulkCopy_SqlRowsCopied);
                                bulkCopy.DestinationTableName = "PackagingDetails";
                                bulkCopy.WriteToServer(oDbDataReader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.ExceptionLogger.logException(ex);
                    oClientLst_PackagingDetails = Obj_perfSync.GetPackagingDetailss(4, Convert.ToString(Client_Jid), "");
                    if (oClientLst_PackagingDetails.Count == 0)
                    {
                        string Query = "delete from job where jid=" + Client_Jid;
                        int i = Obj_perfSync.RemoveOLD_PckRecords(Query);
                    }
                    //UpdateProgressBar(-1);
                    Trace.TraceError("Error : " + ex.Message + "  " + ex.StackTrace);
                    Msg = "NO CONNECTION WITH RESPECTIVE LINE.PLEASE CHECK CONNECTION SETTING.";
                    return false;
                }

                SSCCLineHolder objSSCCLineHolder = dbhelper.DBManager.SSCCLineHolderBLL.GetSSCCLineHolder(SSCCLineHolderBLL.SSCCLineHolderOp.GETSSCCLineHolder, Convert.ToInt32(jb.JID));
                objSSCCLineHolder.JobID = Client_Jid;
                Obj_perfSync.InsertOrUpdateSSCCLineHolder(objSSCCLineHolder);

                jb.JobStatus = (sbyte)JobState.Running;
                if (server == "Line")
                {
                    dbhelper.DBManager.JobBLL.UpdateJobDetails(JobBLL.JobOp.UpdateJobStatus, jb);
                }
               
                //dbhelper.AddUserTrail(ObjUsers.ID, ObjUsers1.ID, USerTrailWHERE.TnT, USerTrailWHAT.DATA_SYNC, USerTrailWHY.DATA_SYNC_TO_CLIENT, "Status " + "'" + jb.JobStatus.ToString() + "'" + " updated to plant server for job " + ObjJob.BatchNo);
                Trace.TraceInformation("Update job status at plant server=>{0},{1},{2},{3}", "Status updation at plant server", DateTime.Now.ToString(), jb.JobStatus.ToString(), ObjJob.BatchNo);

                ObjJob.JobStatus = (sbyte)JobState.CompleteTransfer;
                Obj_perfSync.InsertOrUpdateJobForSync(ObjJob, ProductName, CreatedBy, VerifiedBy, 1);
                //dbhelper.AddUserTrail(ObjUsers.ID, ObjUsers1.ID, USerTrailWHERE.TnT, USerTrailWHAT.DATA_SYNC, USerTrailWHY.DATA_SYNC_TO_CLIENT, "Status " + "'" + ObjJob.JobStatus.ToString() + "'" + " updated to line server for job " + ObjJob.BatchNo);
                Trace.TraceInformation("Update job status at plant server=>{0},{1},{2},{3}", "Status updation at plant server", DateTime.Now.ToString(), ObjJob.JobStatus.ToString(), ObjJob.BatchNo);

                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.ShwMsgCurrentProcess + " : " + JobName, OverallProgress, 100);
                ConnectionStr = string.Empty;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool processJobTransferToGlobalServer(string server,int LoginId)
        {

            int Custid = 0;
            Job jb = new Job();
            Obj_perfSync.OpenDBConnection(ConnectionStr);
            if (Obj_perfSync.IsOpen())
            {
                PBCounter++;
                percent = (int)(((double)PB / (double)PBMaximum) * 100);

                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.ShwMsgCurrentProcess + " : " + JobName, OverallProgress, (int)percent);
                jb.JID = JobId;
                jb.LineCode = LineId;

                jb.JobStatus = (sbyte)JobState.Running;

                PBCounter++;
                percent = (int)(((double)PB / (double)PBMaximum) * 100);

                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.ShwMsgCurrentProcess + " : " + JobName, OverallProgress, (int)percent);

                oClientLst_Job = dbhelper.DBManager.JobBLL.GetJobs(JobBLL.JobOp.GetAllJobs, 1, "", "");
                oClientLst_Users = dbhelper.DBManager.UsersBLL.GetUserss(UsersBLL.UsersOp.GetUserss, "");
                oClientLst_PackagingAsso = dbhelper.DBManager.PackagingAssoBLL.GetPackagingAssos();
                oClientLst_JobTypeDetails = dbhelper.DBManager.JOBTypeBLL.GetJOBTypes();
                oClientLst_PckLabels = dbhelper.DBManager.PackageLabelBLL.GetPackagingLabelAssos(Convert.ToInt32(PackageLabelAssoBLL.PackageLabelOp.GetAllPackageLabelDetails), null, null);
                M_Customer oClientLst_Customer = new M_Customer();



                Job ObjJob = new Job();
                ObjJob = oClientLst_Job.Find(item => item.JID == jb.JID);
                oClientLst_Customer = dbhelper.DBManager.M_CustomerBLL.GetCustomer(1, Convert.ToString(ObjJob.CustomerId));
                PackagingAsso ObjPackagingAsso = new PackagingAsso();
                ObjPackagingAsso = oClientLst_PackagingAsso.Find(item => item.PAID == ObjJob.PAID);

                JOBType objJobTypes = new JOBType();
                objJobTypes = oClientLst_JobTypeDetails.Find(item => item.TID == ObjJob.TID);

                Users ObjUsers = new Users();
                ObjUsers = oClientLst_Users.Find(item => item.ID == ObjJob.CreatedBy);

                Users ObjUsers1 = new Users();
                ObjUsers1 = oClientLst_Users.Find(item => item.ID == ObjJob.VerifiedBy);

                string CreatedBy = string.Empty;
                string VerifiedBy = string.Empty;
                string User = string.Empty;
                string QA = string.Empty;
                if (ObjUsers != null)
                {
                    CreatedBy = ObjUsers.UserName;
                    User = ObjUsers.UserName;
                }

                if (ObjUsers1 != null)
                {
                    VerifiedBy = ObjUsers1.UserName;
                    QA = ObjUsers1.UserName;
                }
                string ProductName = ObjPackagingAsso.Name;


                decimal PAID = 0;

                List<PackageLabelAsso> PackLabels = new List<PackageLabelAsso>();
                Users usr1 = new Users();
                Users usr2 = new Users();
                try
                {
                     usr1 = Obj_perfSync.GetUsersFromGlobalServer(4, ObjUsers);
                    if(usr1.ID==0)
                    {
                        ObjUsers.Active = false;
                        int uid = Obj_perfSync.InsertOrUpdateUserDetailsForSyncGlobalServer(ObjUsers, (int)UsersBLL.UsersOp.AddUserForSync);
                        ObjJob.CreatedBy = uid;
                    }
                    else
                    {
                        ObjJob.CreatedBy = usr1.ID;
                    }
                    Trace.TraceInformation("{0},{1},{2},{3},{4}", DateTime.Now.ToString(), ObjUsers.UserName, ObjUsers.ID, ObjUsers1.UserName, ObjUsers1.ID);
                    usr2 = Obj_perfSync.GetUsersFromGlobalServer(4, ObjUsers1);
                    if (usr2.ID == 0)
                    {
                        ObjUsers1.Active = false;
                      int uid=Obj_perfSync.InsertOrUpdateUserDetailsForSyncGlobalServer(ObjUsers1, (int)UsersBLL.UsersOp.AddUserForSync);
                        ObjJob.VerifiedBy = usr2.ID;
                    }
                    else
                    {
                        ObjJob.VerifiedBy = usr2.ID;
                    }
                    Trace.TraceInformation("Transfer done for both users");
                }
                catch (Exception ex)
                {
                    ExceptionHandler.ExceptionLogger.logException(ex);
                    Trace.TraceInformation(DateTime.Now.ToString() + " " + ex.Message + " " + ex.StackTrace);
                }

                try
                {
                    Custid = Obj_perfSync.InsertOrUpdateCustomerForDataSyncPUSHTOGLOBAL(1, oClientLst_Customer);
                }
                catch(Exception ex)
                {
                    ExceptionHandler.ExceptionLogger.logException(ex);
                    Trace.TraceInformation(DateTime.Now.ToString() + " " + ex.Message + " " + ex.StackTrace);
                }

                //dbhelper.AddUserTrail(ObjUsers.ID, ObjUsers1.ID, USerTrailWHERE.TnT, USerTrailWHAT.DATA_SYNC, USerTrailWHY.DATA_SYNC_TO_CLIENT, "Status " + "'" + jb.JobStatus.ToString() + "'" + " updated to plant server for job " + ObjJob.BatchNo);
                Trace.TraceInformation("Update job status at plant server=>{0},{1},{2},{3}", "Status updation at plant server", DateTime.Now.ToString(), jb.JobStatus.ToString(), ObjJob.BatchNo);

                //.....Here job table is added from server to client side.
             //   ObjJob.LineCode = LineId;
                //ObjJob.LineName = JobShift.LineName;
               // ObjJob.JobStatus = 1; // 08Sept2015 sunil temporary

                Client_JtypeId = Obj_perfSync.InsertOrUpdateJobTypeForSync(objJobTypes);


                Trace.TraceInformation("Update jobtype at line server=>{0},{1},{2},{3}", "jobtype updation at line server", DateTime.Now.ToString(), Client_JtypeId.ToString(), objJobTypes.Job_Type);


                Trace.TraceInformation("update job and status at line server=>{0},{1},{2},{3}", "Status and job updation at line server", DateTime.Now.ToString(), ObjJob.JobStatus.ToString(), ObjJob.BatchNo);

                //foreach (PackagingAsso prod in oClientLst_PackagingAsso)
                if (ObjPackagingAsso != null && ObjPackagingAsso.PAID > 0)
                {

                    PAID = Obj_perfSync.InsertOrUpdatePackagingAssoForSyncPushGlobal(ObjPackagingAsso);
                    oClientLst_PackagingAssoDetails = dbhelper.DBManager.PackagingAssoDetailsBLL.GetPckAssoDtlss(ObjPackagingAsso.PAID);
                    PackLabels = oClientLst_PckLabels.FindAll(item => item.PAID == ObjPackagingAsso.PAID);
                    foreach (PackagingAssoDetails PackAssDtls in oClientLst_PackagingAssoDetails)
                    {
                        //UpdateProgressBar(PBCounter);
                        Obj_perfSync.InsertOrUpdatePckAssoDetailsForSync(PackAssDtls, PAID);
                        PBCounter++;
                        percent = (int)(((double)PB / (double)PBMaximum) * 100);
                        ProgressHub.sendMessage(TnT.LangResource.GlobalRes.ShwMsgCurrentProcess  +" : " + JobName, OverallProgress, (int)percent);
                        //dbhelper.AddUserTrail(ObjUsers.ID, ObjUsers1.ID, USerTrailWHERE.TnT, USerTrailWHAT.DATA_SYNC, USerTrailWHY.DATA_SYNC_TO_CLIENT, "product" + "'" + prod.Name + "'" + " updated to line server");
                        Trace.TraceInformation("load packagingAsso and PackagingAssoDetails at line server=>{0},{1},{2}", "Product details updation at line server", DateTime.Now.ToString(), ObjPackagingAsso.Name);
                    }
                    foreach (PackageLabelAsso PckLblAssos in PackLabels)
                    {
                        Obj_perfSync.InsertOrUpdatePckLabelForSync(PckLblAssos, PAID);
                        //dbhelper.AddUserTrail(ObjUsers.ID, ObjUsers1.ID, USerTrailWHERE.TnT, USerTrailWHAT.DATA_SYNC, USerTrailWHY.DATA_SYNC_TO_CLIENT, "Product Label" + "'" + PckLblAssos.Code + "'" + " updated to line ");
                        Trace.TraceInformation("Updated packaging label line =>{0},{1},{2}", "Product Label Details updation at line", DateTime.Now.ToString(), ObjPackagingAsso.Name);
                    }
                }

                ObjJob.TID = Client_JtypeId;
                ObjJob.PAID = PAID;
                if (Custid != 0)
                {
                    ObjJob.CustomerId = Custid;
                }
                var settings = db.Settings.FirstOrDefault();
                ObjJob.PlantCode = Convert.ToString(settings.PlantCode);
                Client_Jid = Obj_perfSync.InsertOrUpdateJobForSyncToGlobal(ObjJob, ProductName, CreatedBy, VerifiedBy, 1);

                oClientLst_JobDetails = dbhelper.DBManager.JobDetailsBLL.GetJobDetailss(JobDetailsBLL.JobDetailsOp.GetDetailsWithJID, jb.JID, 1);
                foreach (JobDetails jDetails in oClientLst_JobDetails)
                {
                    //UpdateProgressBar(PBCounter);
                    Obj_perfSync.InsertOrUpdateJobDetailsForSync(1, jDetails, (decimal)Client_Jid);
                    //dbhelper.AddUserTrail(ObjUsers.ID, ObjUsers1.ID, USerTrailWHERE.TnT, USerTrailWHAT.DATA_SYNC, USerTrailWHY.DATA_SYNC_TO_CLIENT, "batch details of batch" + "'" + ObjJob.BatchNo + "'" + " updated to line server");
                    Trace.TraceInformation("Load Job details at Client/Line side from Server=>{0},{1},{2},{3},{4},{5},{6},{7},{8}", ObjJob.JobName, jDetails.JD_ProdName, ObjJob.BatchNo, ObjJob.Quantity, ObjJob.SurPlusQty, ObjJob.AutomaticBatchCloser, ObjJob.TID, ObjJob.JobWithUID, "LineName");
                    PBCounter++;
                    percent = (int)(((double)PB / (double)PBMaximum) * 100);
                    ProgressHub.sendMessage(TnT.LangResource.GlobalRes.ShwMsgCurrentProcess + " : " + JobName, OverallProgress, (int)percent);
                }


                try
                {
                    // string Query = "SELECT [PackDtlsID],[Code]," + PAID + " AS [PAID]," + Client_Jid + " AS [JobID],[PackageTypeCode],[MfgPackDate],[ExpPackDate],[NextLevelCode],[IsRejected],[Reason],[BadImage],[SSCC]," +
                    //     "[SSCCVarificationStatus],[IsManualUpdated],[ManualUpdateDesc],[CaseSeqNum],[OperatorId],[Remarks],[IsDecomission],[CreatedDate],[LastUpdatedDate]," +
                    //     "[LineCode],[SYNC],[RCResult],[DavaPortalUpload],[IsUsed] FROM [PackagingDetails] Where JobID =" + JobId.ToString();

                    var pkgdetail = db.PackagingDetails.Where(x => x.JobID == JobId).FirstOrDefault();
                    if (pkgdetail != null)
                    {
                        PackagingDetails pkg = new PackagingDetails();
                        pkg.JobID = pkgdetail.JobID;
                        pkg.SYNC = false;


                        dbhelper.DBManager.PackagingDetailsBLL.InsertOrUpdatePackagingDetails(17, pkg);
                    }

                    string q2 = @"SELECT [PackDtlsID],[Code]  , " + PAID + " AS [PAID]      , " + Client_Jid + " as [JobID]     ,[PackageTypeCode]      ,[MfgPackDate]      ,[ExpPackDate]       ,[NextLevelCode]      ,[IsRejected]      ,[Reason]      ,[BadImage]      ,[SSCC]       ,[SSCCVarificationStatus]      ,[IsManualUpdated]      ,[ManualUpdateDesc]      ,[CaseSeqNum]      ,[OperatorId]      ,[Remarks]      ,[IsDecomission]      ,[CreatedDate]      ,[LastUpdatedDate]      ,[LineCode]      ,[SYNC]      ,[RCResult]      ,[DavaPortalUpload]      ,[IsUsed]      FROM [dbo].[PackagingDetails] Where JobID =" + JobId.ToString();
                    DbDataReader oDbDataReader = dbhelper.GetReader(q2);
                    if (oDbDataReader != null)
                    {

                        // open the destination data
                        using (DbConnection destinationConnection = new SqlConnection(ConnectionStr))
                        {
                            // open the connection
                            destinationConnection.Open();

                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectionStr))
                            {
                                bulkCopy.BatchSize = 500;
                                bulkCopy.NotifyAfter = 1000;
                                bulkCopy.SqlRowsCopied +=
                                    new SqlRowsCopiedEventHandler(bulkCopy_SqlRowsCopied);
                                bulkCopy.DestinationTableName = "PackagingDetails";
                                bulkCopy.WriteToServer(oDbDataReader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.ExceptionLogger.logException(ex);
                    oClientLst_PackagingDetails = Obj_perfSync.GetPackagingDetailss(4, Convert.ToString(Client_Jid), "");
                    if (oClientLst_PackagingDetails.Count == 0)
                    {
                        string Query = "delete from job where jid=" + Client_Jid;
                        int id = Obj_perfSync.RemoveOLD_PckRecords(Query);

                    }
                    //UpdateProgressBar(-1);
                    Trace.TraceError("Error : " + ex.Message + "  " + ex.StackTrace);
                    Msg = "NO CONNECTION WITH RESPECTIVE LINE.PLEASE CHECK CONNECTION SETTING.";
                    return false;
                }

                SSCCLineHolder objSSCCLineHolder = dbhelper.DBManager.SSCCLineHolderBLL.GetSSCCLineHolder(SSCCLineHolderBLL.SSCCLineHolderOp.GETSSCCLineHolder, Convert.ToInt32(jb.JID));
                objSSCCLineHolder.JobID = Client_Jid;
                Obj_perfSync.InsertOrUpdateSSCCLineHolder(objSSCCLineHolder);
                ObjUsers = oClientLst_Users.Find(item => item.ID == LoginId);
                usr1 = Obj_perfSync.GetUsersFromGlobalServer(4, ObjUsers);

                if (usr1.ID == 0)
                {
                    ObjUsers.Active = false;
                    int uid = Obj_perfSync.InsertOrUpdateUserDetailsForSyncGlobalServer(ObjUsers, (int)UsersBLL.UsersOp.AddUserForSync);
                    usr1.ID = uid;
                }

                string query = "insert into ServerSideTrails(UserId,Message,ActitvityTime) values(" + usr1.ID + ",'" + ObjJob.JobName + " Pushed To Global Server','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                int i=Obj_perfSync.ExecuteQuery(query);

                ProgressHub.sendMessage(TnT.LangResource.GlobalRes.ShwMsgCurrentProcess  +" : " + JobName, OverallProgress, 100);
                ConnectionStr = string.Empty;
                return true;
            }
            else
            {
                return false;
            }
        }
        private void updatetails()
        {
            Job jb = new Job();
            jb.JID = JobId;
            jb.LineCode = LineId;
            //jb.LineName = JobShiftTransfer.LineName;
            jb.LastUpdatedDate = Convert.ToDateTime(DateTime.Now.ToString());
            dbhelper.DBManager.JobBLL.UpdateJobDetails(JobBLL.JobOp.UpdateJobByLine, jb);

        }

        private void bulkCopy_SqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            Console.WriteLine("-- Copied {0} rows.", e.RowsCopied);
            Msg = Convert.ToInt32(e.RowsCopied).ToString();
        }


    }
}