using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public partial class SSCCLineHolderBLL
    {
        public enum SSCCLineHolderOp
        {
            GETSSCCLineHolderss = 1,
            GETSSCCLineHolder = 2,//[@ID as SSCCLineHolder ID]
            GetLastSSCC = 3,//[@ID as SSCCLineHolder PI]

        }
        private SSCCLineHolderDAO _SSCCLineHolderDAO;

        public SSCCLineHolderDAO SSCCLineHolderDAO
        {
            get { return _SSCCLineHolderDAO; }
            set { _SSCCLineHolderDAO = value; }
        }

        public SSCCLineHolderBLL()
        {
            SSCCLineHolderDAO = new SSCCLineHolderDAO();
        }
        public List<SSCCLineHolder> GetSSCCLineHolders()
        {
            try
            {
                return SSCCLineHolderDAO.GetSSCCLineHolders((int)SSCCLineHolderOp.GETSSCCLineHolderss);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public SSCCLineHolder GetSSCCLineHolder(SSCCLineHolderOp OP, int ID)
        {
            try
            {
                return SSCCLineHolderDAO.GetSSCCLineHolder((int)OP, ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetLatestSSCC()
        {
            try
            {
                return SSCCLineHolderDAO.GetLatestSSCCLineHolder();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdateSSCCLineHolder(SSCCLineHolder oSSCCLineHolder)
        {
            try
            {
                return SSCCLineHolderDAO.InsertOrUpdateSSCCLineHolder(oSSCCLineHolder);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SSCCLineHolder> DeserializeSSCCLineHolders(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<SSCCLineHolder>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializeSSCCLineHolders(string Path, List<SSCCLineHolder> SSCCLineHolders)
        {
            try
            {
                GenericXmlSerializer<List<SSCCLineHolder>>.Serialize(SSCCLineHolders, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
