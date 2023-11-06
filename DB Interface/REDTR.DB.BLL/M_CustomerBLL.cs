using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{

    public partial class M_CustomerBLL
    {
        private M_CustomerDAO _M_CustomerDAO;

        public M_CustomerDAO M_CustomerDAO
        {
            get { return _M_CustomerDAO; }
            set { _M_CustomerDAO = value; }
        }

        public M_CustomerBLL()
        {
            M_CustomerDAO = new M_CustomerDAO();
        }

        public M_Customer GetCustomer(int Flag,string Id)
        {
            try
            {
                return M_CustomerDAO.GetCustomer(Flag,Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
