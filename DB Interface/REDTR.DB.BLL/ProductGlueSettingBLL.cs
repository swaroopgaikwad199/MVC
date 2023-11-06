using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REDTR.DB.BLL
{
    public partial class ProductGlueSettingBLL
    {
        private ProductGlueSettingDAO _ProductGlueSettingDAO;

        public ProductGlueSettingDAO ProductGlueSettingDAO
        {
            get { return _ProductGlueSettingDAO; }
            set { _ProductGlueSettingDAO = value; }
        }

        public ProductGlueSettingBLL()
        {
            ProductGlueSettingDAO = new ProductGlueSettingDAO();
        }

        public ProductGlueSetting GETProductGlueSetting(decimal Flag,decimal ServerPAID)
        {
            return ProductGlueSettingDAO.GETProductGlueSetting(Flag, ServerPAID);
        }
    }
}
