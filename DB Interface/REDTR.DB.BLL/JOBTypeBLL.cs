using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public class JOBTypeBLL
    {
        public enum JOBTypeOp
        {
            GetJOBTypes = 1,
            GetJOBTypeByTypeID = 2,
            GetJOBTypeByName = 3,
            GetJOBTypesGreaterThanLUD= 4,
        }

        private JOBTypeDAO _JOBTypeDAO;

        public JOBTypeDAO JOBTypeDAO
        {
            get { return _JOBTypeDAO; }
            set { _JOBTypeDAO = value; }
        }

        public JOBTypeBLL()
        {
            JOBTypeDAO = new JOBTypeDAO();
        }
        public List<JOBType> GetJOBTypes()
        {
            try
            {
                return JOBTypeDAO.GetJOBTypes((int)JOBTypeOp.GetJOBTypes,null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JOBType GetJOBType(JOBTypeOp OP, string Name)
        {
            try
            {
                return JOBTypeDAO.GetJOBType((int)OP, Name);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JOBType> GetJOBTypes(Nullable<DateTime> lastUpdatedDate, string format)
        {
            try
            {
                string dt = null;
                if (string.IsNullOrEmpty(format))
                    format = "yyyy-MM-dd HH:mm:ss.fff";
                if (lastUpdatedDate != null)
                    dt = ((DateTime)lastUpdatedDate).ToString(format);
                return JOBTypeDAO.GetJOBTypes((int)JOBTypeOp.GetJOBTypesGreaterThanLUD, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JOBType GetJOBTypeByID(Decimal TypeID)
        {
            try
            {
                return JOBTypeDAO.GetJOBType((int)JOBTypeOp.GetJOBTypeByTypeID, TypeID.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> JobTypes2Strings()
        {
            try
            {
                return JOBTypeDAO.JobTypes2Strings((int)JOBTypeOp.GetJOBTypes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        public int AddJOBType(JOBType oJOBType)
        {
            try
            {
                return JOBTypeDAO.InsertUpdateJOBType(oJOBType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        public List<JOBType> DeserializeJOBTypes(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<JOBType>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializeJOBTypes(string Path, List<JOBType> JOBTypes)
        {
            try
            {
                GenericXmlSerializer<List<JOBType>>.Serialize(JOBTypes, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
