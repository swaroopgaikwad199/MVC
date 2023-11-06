using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public partial class JobAssoDeckBLL
    {
        public enum Op
        {
            GETJobAssoDeckss = 1,
            GETJobAssoDeckOFID = 2,

            GETJobAssoDeckOFJOBID = 3,
            GETJobAssoDeckOFDeckCode = 4,
            //GETJobAssoDeckOFRecipeID = 5,
            GETJobAssoDeckLUD = 5,

            Insert = 1,
            DeleteAll = 1,
            DeleteWithID = 2,//[@Value]       
            DeleteWithJID = 4
        }
        private JobAssoDeckDAO _JobAssoDeckDAO;

        public JobAssoDeckDAO JobAssoDeckDAO
        {
            get { return _JobAssoDeckDAO; }
            set { _JobAssoDeckDAO = value; }
        }

        public JobAssoDeckBLL()
        {
            JobAssoDeckDAO = new JobAssoDeckDAO();
        }
        public List<JobAssoDeck> GetJobAssoDecks(Op mOp, string Param)
        {
            try
            {
                return JobAssoDeckDAO.GetJobAssoDecks((int)mOp, Param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JobAssoDeck GetJobAssoDeck(Op mOp, string Param)
        {
            try
            {
                return JobAssoDeckDAO.GetJobAssoDeck((int)mOp, Param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<JobAssoDeck> GetJobAssoDecks(Nullable<DateTime> lastUpdatedDate, string format)
        {
            try
            {
                string dt = null;
                if (string.IsNullOrEmpty(format))
                    format = "yyyy-MM-dd HH:mm:ss.fff";
                if (lastUpdatedDate != null)
                    dt = ((DateTime)lastUpdatedDate).ToString(format);
                return JobAssoDeckDAO.GetJobAssoDecks((int)Op.GETJobAssoDeckLUD, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddJobAssoDeck(JobAssoDeck oJobAssoDeck)
        {
            try
            {
                return JobAssoDeckDAO.AddJobAssoDeck((int)Op.Insert, oJobAssoDeck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveJobAssoDeckOfID(decimal ID)
        {
            try
            {
                return JobAssoDeckDAO.RemoveJobAssoDeck((int)Op.DeleteWithID, ID.ToString(), null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveJobAssoDeckOfJobID(decimal JID)
        {
            try
            {
                return JobAssoDeckDAO.RemoveJobAssoDeck((int)Op.DeleteWithJID, JID.ToString(), null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveJobAssoDeckAll()
        {
            try
            {
                return JobAssoDeckDAO.RemoveJobAssoDeck((int)Op.DeleteAll, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
