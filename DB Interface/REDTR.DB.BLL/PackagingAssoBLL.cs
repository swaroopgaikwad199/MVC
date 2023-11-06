using System;
using System.Collections.Generic;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public partial class PackagingAssoBLL
    {
        public enum PackagingAssoOp
        {
            GetProductss = 1, //Param[@Flag]
            GetProduct = 2,//Param[@Flag,@Value As JobID]
            GetProductforXML=11,
            GetProductsOfName = 3,//Param[@Flag,@Value As BatchNo]
            //GetProductsOfGTIN = 4,//Param[@Flag,@Value As ProductID]
            //GetProductsSize = 5,//Param[@Flag,@Value As OperatorId]          
            GetProductsOfProdCode = 6,

            GetProdOFDecksCodes = 7,

            GetSplitDecksCodes = 8,

            /// <summary>
            /// It will take records of Greater Than Last Updated Date.
            /// </summary>
            GetProGreaterThanLUD = 9,///Param[@Value as LUd Date]
            GETNonVeriedPackagingAsso = 10, //Add by Aparna for null records in dgv

            AddPackagingAsso = 1,
            UpdatePackagingAsso = 2, UpdatePackagingAssoforDavaExport = 3,
            updateRemark = 4                //Added by Aparna(05.09.22015)
        }
        private PackagingAssoDAO _PackagingAssoDAO;

        public PackagingAssoDAO PackagingAssoDAO
        {
            get { return _PackagingAssoDAO; }
            set { _PackagingAssoDAO = value; }
        }

        public PackagingAssoBLL()
        {
            PackagingAssoDAO = new PackagingAssoDAO();
        }
        public List<PackagingAsso> GetPackagingAssos()
        {
            try
            {
                return PackagingAssoDAO.GetPackagingAssos((int)PackagingAssoOp.GetProductss, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PackagingAsso> GetNonVerifiedPackagingAssos()           //Added by Aparna for dgv null value
        {
            try
            {
                return PackagingAssoDAO.GetPackagingAssos((int)PackagingAssoOp.GETNonVeriedPackagingAsso, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PackagingAsso> GetPackagingAssos(Nullable<DateTime> lastUpdatedDate, string format)
        {
            try
            {
                string dt = null;
                if (string.IsNullOrEmpty(format))
                    format = "yyyy-MM-dd HH:mm:ss.fff";
                if (lastUpdatedDate != null)
                    dt = ((DateTime)lastUpdatedDate).ToString(format);
                return PackagingAssoDAO.GetPackagingAssos((int)PackagingAssoOp.GetProGreaterThanLUD, dt, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PackagingAsso> GetPackagingAssos(PackagingAssoOp op, string Param1, string Param2)
        {
            try
            {
                return PackagingAssoDAO.GetPackagingAssos((int)op, Param1, Param2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PackagingAsso GetPackagingAsso(PackagingAssoOp op, string Value)
        {
            try
            {
                return PackagingAssoDAO.GetPackagingAsso((int)op, Value, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertOrUpdatePackagingAsso(PackagingAssoOp Op, PackagingAsso oPackagingAsso)
        {
            try
            {
                return PackagingAssoDAO.InsertOrUpdatePackagingAsso((int)Op, oPackagingAsso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemovePackagingAsso(Decimal PAID)
        {
            try
            {
                return PackagingAssoDAO.RemovePackagingAsso(PAID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PackagingAsso> DeserializePackagingAssos(string Path)
        {
            try
            {
                return GenericXmlSerializer<List<PackagingAsso>>.Deserialize(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SerializePackagingAssos(string Path, List<PackagingAsso> PackagingAssos)
        {
            try
            {
                GenericXmlSerializer<List<PackagingAsso>>.Serialize(PackagingAssos, Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
