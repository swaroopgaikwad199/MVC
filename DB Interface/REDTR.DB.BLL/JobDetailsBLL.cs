using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public class JobDetailsBLL
    {
        public enum JobDetailsOp
        {
            GetDetailsWithJID = 1,
            GetDetailsWithJIDDeck = 2,
            GetDistinctProdwithAppid = 3,
            GetDetailsLUD = 4,
            
            insertjobDetails = 1,
            RemoveJobDetailsOfJob = 1,
            RemoveJobDetails = 2
        }
        private JobDetailsDAO _JobDetailsDAO;

        public JobDetailsDAO JobDetailsDAO
        {
            get { return _JobDetailsDAO; }
            set { _JobDetailsDAO = value; }
        }

        public JobDetailsBLL()
        {
            JobDetailsDAO = new JobDetailsDAO();
        }
        public List<JobDetails> GetJobDetailss(JobDetailsOp op, decimal JobID, int AppID)
        {
            try
            {
                return JobDetailsDAO.GetJobDetailss((int)op, JobID.ToString(), AppID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JobDetails GetJobDetails(JobDetailsOp op, decimal JobID, string Deck)
        {
            try
            {
                return JobDetailsDAO.GetJobDetails((int)op, JobID.ToString(), Deck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<JobDetails> GetJobDetails(Nullable<DateTime> lastUpdatedDate, string format)
        {
            try
            {
                string dt = null;
                if (string.IsNullOrEmpty(format))
                    format = "yyyy-MM-dd HH:mm:ss.fff";
                if (lastUpdatedDate != null)
                    dt = ((DateTime)lastUpdatedDate).ToString(format);
                return JobDetailsDAO.GetJobDetailss((int)JobDetailsOp.GetDetailsLUD, dt, -1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddJobDetails(JobDetails oJobDetails)
        {
            try
            {
                return JobDetailsDAO.AddJobDetails((int)JobDetailsOp.insertjobDetails, oJobDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveJobDetails(decimal JobID)
        {
            try
            {
                return JobDetailsDAO.RemoveJobDetails((int)JobDetailsOp.RemoveJobDetailsOfJob, JobID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
      


    }
}
