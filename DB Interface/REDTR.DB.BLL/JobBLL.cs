using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;
namespace REDTR.DB.BLL
{
    public partial class JobBLL
    {
        public enum JobOp
        {
            GetAllJobs = 1, //Param[@Flag]
            GetJob = 2,//Param[@Flag,@Value As JobID]
            GetJobOfBatch = 3,//Param[@Flag,@Value As BatchNo]
            GetJobsOfProducts = 4,//Param[@Flag,@Value As ProductID]
            GetJobsOfOperator = 5,//Param[@Flag,@Value As OperatorId]
            GetJobsOfJobName = 6,//Param[@Flag,@Value AS JobName]
            GetJobsOfStatus = 7,//Param[@Flag,@Value AS JobStatus]
            GetJobsFordecommissioning = 16, //Param[@Flag,@Value AS JobStatus]

            GetUnVrfdJbs = 8,//Param[@Flag,@Value AS JobStatus,@Value1 AS JobStatus]
            GetVrfdRunigOrPusdJbs = 9,
            GetVrfdNotClosedJbs = 10,
            GetVrfdWithClosedJbs = 17,  //Flag for Label reprinting.
            GetNotClosedJbs = 11,
            GetNonDecommissionedJbs = 12,
            GetDecommissionedJbs = 13,
            GetJobsGreaterThanLUD = 14,///Param[@Value as LUd Date]
            GetJobOfJobNameNBatchNo = 15,///Param[@Value as jobName and @Value1 as Batchno]
            GetAllBatchForDavaPortal=19,
            GetSingleBatchForDavaPortal = 23,
            GetProductBatch = 18,             
            InsertJob = 1,
            UpdateJob = 6,
            UpdateEndTimeJob = 2,
            UpdateStatusEndTimeJob = 3,
            UpdateLableStartIndexJob = 4,
            UpdateVerifiedJob = 5,
            UpdateJobStatus = 7,/*Added on 22-03-2012 by Apurva A Kunkulol*/
              GetClosedBatches = 21,//For DAVA
              GetSelectedClosedBatcheDetails = 22,//For DAVA
            UpdateQTY = 8,   // By Tushar
              UpdateDavaPortalJob = 9,
              UpdateJobByLine = 10, /*Added on 26 Sept 2015 by sunil jadhav*/

            DeleteJob = 1,
            DeleteALLJob = 2,
            // For contoller count by tushar & Ansuman
            InsertJobCount = 1,
            UpdateJobCount = 2,
            GetJobCount = 3,
            UpdateSpeJobCount = 4,
        }
        private JobDAO _JobDAO;

        public JobDAO JobDAO
        {
            get { return _JobDAO; }
            set { _JobDAO = value; }
        }
        public JobBLL()
        {
            JobDAO = new JobDAO();
        }
        public List<Job> GetJobs(JobOp jobOp, int AppCode, string Value1, string Value2)
        {
            try
            {
                return JobDAO.GetJobs((int)jobOp, AppCode, Value1, Value2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       public List<Job> GetJobs(Nullable<DateTime> lastUpdatedDate,string format)
        {
            try
            {
                string dt = null;
                if (string.IsNullOrEmpty(format))
                    format = "yyyy-MM-dd HH:mm:ss.fff";
                if (lastUpdatedDate != null)
                    dt = ((DateTime)lastUpdatedDate).ToString(format);
                return JobDAO.GetJobs((int)JobOp.GetJobsGreaterThanLUD, -1, dt, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Job GetJob(JobOp jobOp, int AppCode, string Value1, string Value2)
        {
            try
            {
                return JobDAO.GetJob((int)jobOp, AppCode, Value1, Value2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddJob(Job oJob)
        {
            try
            {
                return JobDAO.InsertOrUpdateJob((int)JobOp.InsertJob, oJob);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateJobDetails(JobOp JobOp, Job oJob)
        {
            try
            {
                return JobDAO.InsertOrUpdateJob((int)JobOp, oJob);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveJob(JobOp JobOp, Nullable<Decimal> JID)
        {
            try
            {
                return JobDAO.RemoveJob((int)JobOp, JID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Job> DeserializeJobs(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<Job>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializeJobs(string Path, List<Job> Jobs)
        {
            try
            {
                GenericXmlSerializer<List<Job>>.Serialize(Jobs, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateNoRead(Int32 JobID,Int32 Count)
        {
            try
            {
                JobDAO.UpdateNoRead(JobID, Count);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }


 // for contoller count by tushar

        public void AddJobcount(int jobid, int BCnt, int LCnt, int WCnt,int SCnt, int MTCnt, int MVCnt, int MLPCnt, int MCBCnt)
        {
            try
            {
                JobDAO.InsertOrUpdateJobCount((int)JobOp.InsertJobCount, jobid, BCnt, LCnt, WCnt, SCnt, MTCnt, MVCnt, MLPCnt, MCBCnt);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }
        public void UpdateJobcount(int jobid, int BCnt, int LCnt, int WCnt,int SCnt, int MTCnt, int MVCnt, int MLPCnt, int MCBCnt)
        {
            try
            {
                JobDAO.InsertOrUpdateJobCount((int)JobOp.UpdateJobCount, jobid, BCnt, LCnt, WCnt, SCnt, MTCnt, MVCnt, MLPCnt, MCBCnt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateSpeJobcount(int jobid, int BCnt, int LCnt, int WCnt, int SCnt, int MTCnt, int MVCnt, int MLPCnt, int MCBCnt)
        {
            try
            {
                JobDAO.InsertOrUpdateJobCount((int)JobOp.UpdateSpeJobCount, jobid, BCnt, LCnt, WCnt, SCnt, MTCnt, MVCnt, MLPCnt, MCBCnt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<int> GetJobcount(int jobid,int BCnt, int LCnt, int WCnt,int SCnt, int MTCnt, int MVCnt, int MLPCnt, int MCBCnt)
        {
            try
            {
                return JobDAO.GetJobCount((int)JobOp.GetJobCount, jobid,0,0,0,0,0,0,0,0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<PackagingDetails> GetDetailJobIformation(string PackageTypeCode, decimal JobID)
        //{
        //    try
        //    {
        //        return PackagingDetailsDAO.GetPackagingDetailss((int)JobOp.ForDetailJobIformation, PackageTypeCode, JobID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
