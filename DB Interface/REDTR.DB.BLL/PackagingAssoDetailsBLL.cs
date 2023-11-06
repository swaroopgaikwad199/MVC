using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public class PackagingAssoDetailsBLL
    {
        public enum PckAssoDtlsOp
        {
            GetPckAssoDtlssOfPckAsso = 1, //Param[@Flag]      
            GetPackagingAssoDetails = 2,//Param[@Flag],@PckType,@Value1  
            GetPackagingAssoDetailsLUD = 3 ,//Param[@Flag],@PckType,@Value1     

            AddPckAssoDtls = 1,
            UpdatePckAssoDtls = 2
        }

        private PackagingAssoDetailsDAO _PackagingAssoDetailsDAO;

        private PackagingAssoDetailsDAO PackagingAssoDetailsDAO
        {
            get { return _PackagingAssoDetailsDAO; }
            set { _PackagingAssoDetailsDAO = value; }
        }

        public PackagingAssoDetailsBLL()
        {
            PackagingAssoDetailsDAO = new PackagingAssoDetailsDAO();
        }

        public int InsertOrUpdatePckAssoDtls(PckAssoDtlsOp Op, PackagingAssoDetails oPackagingAssoDetails)
        {
            try
            {
                return PackagingAssoDetailsDAO.InsertOrUpdatePckAssoDtls((int)Op, oPackagingAssoDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PackagingAssoDetails> GetPckAssoDtlss(Nullable<DateTime> lastUpdatedDate, string format)
        {
            try
            {
                string dt = null;
                if (string.IsNullOrEmpty(format))
                    format = "yyyy-MM-dd HH:mm:ss.fff";
                if (lastUpdatedDate != null)
                    dt = ((DateTime)lastUpdatedDate).ToString(format);

                return PackagingAssoDetailsDAO.GetPckAssoDtlssOfPckAsso((int)PckAssoDtlsOp.GetPackagingAssoDetailsLUD, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PackagingAssoDetails> GetPckAssoDtlss(Decimal PAID)
        {
            try
            {
                return PackagingAssoDetailsDAO.GetPckAssoDtlssOfPckAsso((int)PckAssoDtlsOp.GetPckAssoDtlssOfPckAsso, PAID.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PackagingAssoDetails GetPckAssoDtls(PckAssoDtlsOp op, Decimal PAID, string PackTypeCode)
        {
            try
            {
                return PackagingAssoDetailsDAO.GetPckAssoDtls((int)op, PAID.ToString(), PackTypeCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PackagingAssoDetails> DeserializePackagingAssoDetailss(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<PackagingAssoDetails>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializePackagingAssoDetailss(string Path, List<PackagingAssoDetails> PackagingAssoDetailss)
        {
            try
            {
                GenericXmlSerializer<List<PackagingAssoDetails>>.Serialize(PackagingAssoDetailss, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
