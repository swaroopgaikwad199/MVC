using System;
using System.Collections.Generic;
using System.Data;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public partial class ChinaUIDBLL
    {
        private ChinaUIDDAO _ChinaUIDDAO;

        public ChinaUIDDAO ChinaUIDDAO
        {
            get { return _ChinaUIDDAO; }
            set { _ChinaUIDDAO = value; }
        }

        public ChinaUIDBLL()
        {
            ChinaUIDDAO = new ChinaUIDDAO();
        }
       
        public int UpdateChinaUID(ChinaUID oChinaUID)
        {
            try
            {
                return ChinaUIDDAO.UpdateChinaUID(oChinaUID,2);     //Send Flag Value 2 for Update Record. 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateChinaUID(ChinaUID oChinaUID,int flag)
        {
            try
            {
                return ChinaUIDDAO.UpdateChinaUID(oChinaUID, flag);     //Send Flag Value 2 for Update Record. 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertChinaUID(ChinaUID oChinaUID)
        {
            try
            {
                return ChinaUIDDAO.UpdateChinaUID(oChinaUID, 1);     //Send Flag Value 1 for Insert Record. 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertChinaUIDFromStore(int JobID, Int32 Quantity, string Deck,int PAID)
        {
            try
            {
                return ChinaUIDDAO.InsertChinaUIDFromStore(JobID, Quantity, Deck, PAID);     
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Int32 GetReamainingUIDCount(string Deck,int PAID)
        {
            try
            {
                return ChinaUIDDAO.GetReamainingUIDCount(Deck,PAID);     
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxTransNo(int JobID, string Deck)
        {
            try
            {
                return ChinaUIDDAO.GetTransactionNumber(1,JobID, Deck);     //Send Flag Value 1 for Getting Max Trans No. 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxTransNoForReport(int JobID, string Deck)
        {
            try
            {
                return ChinaUIDDAO.GetTransactionNumber(2, JobID, Deck);     //Send Flag Value 1 for Getting Max Trans No. 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RestoreChinaUID(int JobID, string Deck)
        {
            try
            {
                return ChinaUIDDAO.RestoreChinaUID(1, JobID,Deck);     //Restore Remaining UIDs. 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> GetUIDsForChinaCode(int JobID, string oDeck)
        {
            try
            {
                return ChinaUIDDAO.GetChinaUIDs(1, JobID, oDeck);     
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> GetUnUsedUIDsForChinaCode(int JobID, string oDeck)
        {
            try
            {
                return ChinaUIDDAO.GetChinaUIDs(2, JobID, oDeck);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateAcceptedToRejected(string code, int JID)
        {
            try
            {
                return ChinaUIDDAO.UpdateAcceptedToRejected(code,JID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetChinaUIDsDataTable(int JobID,int MaxTransNo)
        {
            try
            {
                String Query = "";
                String GoodWhere, BadWhere = "";
                
                for (int i = 1; i <= MaxTransNo; i++)
                {
                    if (MaxTransNo == 1)
                    {
                        GoodWhere = "  WHERE JobId = " + JobID.ToString() + " and TransId =1 AND (IsUsed =1 And Result = 1) ";
                        BadWhere = "  WHERE JobId = " + JobID.ToString() + " and TransId =1 AND (IsUsed =1 And Result = 0) ";
                    }
                    else if (i == MaxTransNo)
                    {
                        GoodWhere = "  WHERE JobId = " + JobID.ToString() + " and TransId =" + i.ToString() + " AND (IsUsed =1 And Result = 1) ";
                        BadWhere = "  WHERE JobId = " + JobID.ToString() + " and TransId =" + i.ToString() + " AND (IsUsed =1 And Result = 0) ";
                    }
                    else
                    {
                        GoodWhere = "  WHERE JobId = " + JobID.ToString() + " and TransId =" + i.ToString() + " AND (IsUsed =1) ";
                        BadWhere = "  WHERE JobId = " + JobID.ToString() + " and TransId >" + i.ToString() + " AND (IsUsed =1) ";
                    }
                    Query += " SELECT " + JobID.ToString() + " AS JobId, " + i.ToString() + " AS TransID, 1 AS RESULT,[UID],(CAST((SELECT COUNT(*) FROM ChinaUID " + GoodWhere + " )AS VARCHAR)) AS GoodCount, " +
                    " (CAST((SELECT COUNT(*) FROM ChinaUID " + BadWhere + " )AS VARCHAR)) AS BadCount " +
                    " FROM ChinaUID  "+ GoodWhere +
                    " UNION ALL "+
                    " SELECT " + JobID.ToString() + " AS JobId, " + i.ToString() + " AS TransID, 0 AS RESULT,[UID],(CAST((SELECT COUNT(*) FROM ChinaUID " + GoodWhere + ")AS VARCHAR)) AS GoodCount, " +
                    " (CAST((SELECT COUNT(*) FROM ChinaUID " + BadWhere + ")AS VARCHAR)) AS BadCount " +
                    " FROM ChinaUID " + BadWhere;
                    if (i != MaxTransNo)
                        Query += " UNION ALL ";
                }
                    return ChinaUIDDAO.GetChinaUIDsDataTable(Query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public List<ChinaUID> DeserializeChinaUIDs(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<ChinaUID>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializeChinaUIDs(string Path, List<ChinaUID> ChinaUIDs)
        {
            try
            {
                GenericXmlSerializer<List<ChinaUID>>.Serialize(ChinaUIDs, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
