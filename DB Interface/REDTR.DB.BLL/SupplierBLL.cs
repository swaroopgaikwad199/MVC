using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public partial class SupplierBLL
    {
        public enum UIDOp
        {
            inserUIDs=1,
            UpdateUIDFields=2,
            UpdateUIDStatus=3,
            GetCheckedUIDs=4,
            GetMaxTransNo=5,
            GetCheckedUIDsWithTransNo=6,

            insertUIDsInStore=7,
            GetAllData=8,
            GetUIDResult=9,
            GetUIDResultFromStore=10,
            GetUIDListfromStore=11,

            InsertIntoSupplierMaster=1,
            GetAllBatchesFromMaster=4,
            getJobIdByname=6,

            UIDcount=1,
            FirstId=2
        }

        private SupplierDAO _SupplierDAO;

        public SupplierDAO SupplierDAO
        {
            get { return _SupplierDAO; }
            set { _SupplierDAO = value; }
        }

        public SupplierBLL()
        {
            SupplierDAO = new SupplierDAO();
        }

        public int AddUIDs(Supplier oSupplier)
        {
            try
            {
                return SupplierDAO.insertOrUpdateSupplierUIDs((int)UIDOp.inserUIDs,oSupplier);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddUIDsInStore(Supplier oSupplier,int PAID)
        {
            try
            {
                return SupplierDAO.insertOrUpdateSupplierUIDsInStore((int)UIDOp.insertUIDsInStore, oSupplier, PAID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UnusedUIDCount(Supplier oSupplier)
        {
            try
            {
                return SupplierDAO.getUIDCount((int)UIDOp.UIDcount,oSupplier.SFlag1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetFirstId(Supplier oSupplier)
        {
            try
            {
                return SupplierDAO.getTopID((int)UIDOp.FirstId, oSupplier.SFlag1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int updateSupplierUIDs(Supplier oSupplier)
        {
            try
            {
                return SupplierDAO.UpdateSupplierUIDs((int)UIDOp.UpdateUIDFields,oSupplier);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<Supplier> GetCheckedUIDs(UIDOp Op, string paramValue)
        {
            try
            {
                return SupplierDAO.GetCheckedUIDs((int)Op, paramValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetMaxTransNo(UIDOp Op)
        {
            try
            {
                return SupplierDAO.GetmaxTransNo((int)Op);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<Supplier> GetUIDsWithTransNo(UIDOp Op, Supplier oSupplier)
        {
            try
            {
                return SupplierDAO.GetCheckedUIDsWithTransNo((int)Op, oSupplier);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int updateUIDStatus(Supplier oSupplier)
        {
            try
            {
                return SupplierDAO.UpdateUIDStatus((int)UIDOp.UpdateUIDStatus, oSupplier);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public  List<Supplier> GetAllData(UIDOp Op)
        {
            try
            {
                return SupplierDAO.GetAllData((int)UIDOp.GetAllData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetUIDResult(UIDOp Op, Supplier oSupplier)
        {
            try
            {
                return SupplierDAO.GetUIDResult((int)Op,oSupplier);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetUIDResultFromStore(UIDOp Op,string ParamValue)
        {
            try
            {
                return SupplierDAO.GetUIDResultFromStore((int)Op, ParamValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetAllUIdsFromStore(UIDOp Op)
        {
            try
            {
                return SupplierDAO.GetAllUIDsFromStore((int)Op);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertIntoSupplierMaster(UIDOp Op, Supplier oSupplier)
        {
            try
            {
                return SupplierDAO.InsertOrUpdateSupplierMaster((int)Op,oSupplier);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<string> GetAllBatchesFromSupplierMaster(UIDOp Op)
        {
            try
            {
                return SupplierDAO.GetAllBatchNamesFromSupplierMAster((int)Op);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetJobIdByName(UIDOp Op, string Paramvalue)
        {
            try
            {
                return SupplierDAO.GetBatchIdFromBatchName((int)Op,Paramvalue);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    
//public List<Supplier> GetAllData(UIDOp uIDOp,bool p)
//{
//    throw new NotImplementedException();
//}
    }
}
