using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;
 

namespace REDTR.DB.BLL
{
    public partial class PackageLabelAssoBLL
    {
        public enum PackageLabelOp  
        {
            AddNewPackageLabel=1,
            UpdatePackageLabel=2,

            GetAllPackageLabelDetails=1,
            GetPackageLabelDetails=2,
        }

        PackageLabelDAO _PackagelabelDAO;

        public PackageLabelDAO PackagelabelDAO
        {
            get { return _PackagelabelDAO; }
            set { _PackagelabelDAO = value; }
        }

        public PackageLabelAssoBLL()
        {
            _PackagelabelDAO = new PackageLabelDAO();
        }

        public void InsertOrUpdatePackagingLabel(int Flag, PackageLabelAsso oPackaginglabelAsso)
        {
            try
            {
               PackagelabelDAO.InsertOrUpdatePackageLabel(Flag, oPackaginglabelAsso);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PackageLabelAsso> GetPackagingLabelAssos(int Flag, string Param1, string Param2)
        {
            try
            {
                return PackagelabelDAO.GetPackageLabelAssos(Flag, Param1, Param2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int RemovePackageLabelAsso(Decimal PAID)
        {
            try
            {
                return PackagelabelDAO.RemovePackageLabelAsso(PAID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
