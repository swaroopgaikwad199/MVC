using System;
using System.Collections.Generic;
using System.Data;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public class USerTrailBLL
    {
        public enum UserTrailOp
        {
            GetUSerTrails = 1,//[@Flag]
            GetuserTrialDtls = 2, //[@Flag,@FromDate,@ToDate] 
            GetTrialDtlsOfGrThanLUD = 3, //[@Flag,@FromDate as LastUpdatedDate]
            RemoveUserTrails=1
        }
        private USerTrailDAO _USerTrailDAO;

        public USerTrailDAO USerTrailDAO
        {
            get { return _USerTrailDAO; }
            set { _USerTrailDAO = value; }
        }
        public USerTrailBLL()
        {
            USerTrailDAO = new USerTrailDAO();
        }
        public List<USerTrail> GetUSerTrails(Nullable<DateTime> FromDate, Nullable<DateTime> ToDate)
        {
            try
            {
                return USerTrailDAO.GetUSerTrails((int)UserTrailOp.GetUSerTrails, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<USerTrail> GetUSerTrails(Nullable<DateTime> lastUpdatedDate)//, string format)
        {
            try
            {               
                //if (string.IsNullOrEmpty(format))
                //    format = "yyyy-MM-dd HH:mm:ss.fff";
                return USerTrailDAO.GetUSerTrails((int)UserTrailOp.GetTrialDtlsOfGrThanLUD, lastUpdatedDate, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDataset(UserTrailOp Op, Nullable<DateTime> FromDate, Nullable<DateTime> ToDate)
        {
            try
            {
                return USerTrailDAO.GetDSUSerTrails((int)Op, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddUSerTrail(USerTrail oUSerTrail)
        {
            try
            {
                return USerTrailDAO.AddUSerTrail(oUSerTrail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddUSerTrail(USerTrail oUSerTrail,int i)
        {
            try
            {
                return USerTrailDAO.AddUSerTrail(oUSerTrail,i);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RemoveUSerTrails(UserTrailOp Op)
        {
            try
            {
                return USerTrailDAO.RemoveUSerTrails((int)Op);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<USerTrail> GetUSerTrailsOfUsers(Decimal UserID)
        //{
        //    try
        //    {
        //        return USerTrailDAO.GetUSerTrailsOfUsers((int)UserTrailOp.GetUSerTrailsOfUsers,UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}		
        public List<USerTrail> DeserializeUSerTrails(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<USerTrail>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializeUSerTrails(string Path, List<USerTrail> USerTrails)
        {
            try
            {
                GenericXmlSerializer<List<USerTrail>>.Serialize(USerTrails, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
