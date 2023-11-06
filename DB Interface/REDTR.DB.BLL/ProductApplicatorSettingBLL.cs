using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
    public partial class ProductApplicatorSettingBLL
    {
        private ProductApplicatorSettingDAO _ProductApplicatorSettingDAO;

        public ProductApplicatorSettingDAO ProductApplicatorSettingDAO
        {
            get { return _ProductApplicatorSettingDAO; }
            set { _ProductApplicatorSettingDAO = value; }
        }

        public ProductApplicatorSettingBLL()
        {
            ProductApplicatorSettingDAO = new ProductApplicatorSettingDAO();
        }

        public int AddProductApplicatorSetting(ProductApplicatorSetting pro)
        {
            return ProductApplicatorSettingDAO.InsertOrUpdateProductApplicatorSetting(pro);
        }

        public ProductApplicatorSetting GetProductApplicationSettind(decimal PAID,int Flag)
        {
            return ProductApplicatorSettingDAO.GetProductApplicationSettind(PAID,Flag);
        }
    }
}
