using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public class PackageTypeCodeBLL
    {

        public enum PackageTypeCodeOp
        {
            GetAllPackageTypeCode = 1, //Param[@Flag]
            GetPackageTypeCode = 2,//Param[@Flag,@Value]           
        }

        private PackageTypeCodeDAO _PackageTypeCodeDAO;

        public PackageTypeCodeDAO PackageTypeCodeDAO
        {
            get { return _PackageTypeCodeDAO; }
            set { _PackageTypeCodeDAO = value; }
        }

        public PackageTypeCodeBLL()
        {
            PackageTypeCodeDAO = new PackageTypeCodeDAO();
        }
        public List<PackageTypeCode> GetPackageTypeCodes()
        {
            try
            {
                return PackageTypeCodeDAO.GetPackageTypeCodes((int)PackageTypeCodeOp.GetAllPackageTypeCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PackageTypeCode GetPackageTypeCode(string Code)
        {
            try
            {
                return PackageTypeCodeDAO.GetPackageTypeCode((int)PackageTypeCodeOp.GetPackageTypeCode, Code);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddPackageTypeCode(PackageTypeCode oPackageTypeCode)
        {
            try
            {
                return PackageTypeCodeDAO.AddPackageTypeCode(oPackageTypeCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemovePackageTypeCode(string Code)
        {
            try
            {
                return PackageTypeCodeDAO.RemovePackageTypeCode(Code);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PackageTypeCode> DeserializePackageTypeCodes(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<PackageTypeCode>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializePackageTypeCodes(string Path, List<PackageTypeCode> PackageTypeCodes)
        {
            try
            {
                GenericXmlSerializer<List<PackageTypeCode>>.Serialize(PackageTypeCodes, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
