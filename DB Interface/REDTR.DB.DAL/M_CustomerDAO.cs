using REDTR.DB.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace REDTR.DB.DAL
{
    public partial class M_CustomerDAO
    {
        public M_CustomerDAO()
        {
            DbProviderHelper.GetConnection();
        }

        public M_Customer GetCustomer(int Flag, string ParamVal)
        {
            M_Customer oCustomer = new M_Customer();
            DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETCustomer]", CommandType.StoredProcedure);
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, ParamVal));

            DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
            while (oDbDataReader.Read())
            {
                oCustomer.Id= Convert.ToInt32(oDbDataReader["Id"]);

                if (oDbDataReader["CompanyName"] != DBNull.Value)
                    oCustomer.CompanyName = Convert.ToString(oDbDataReader["CompanyName"]);

                if (oDbDataReader["ContactPerson"] != DBNull.Value)
                    oCustomer.ContactPerson = Convert.ToString(oDbDataReader["ContactPerson"]);

                if (oDbDataReader["ContactNo"] != DBNull.Value)
                    oCustomer.ContactNo = Convert.ToString(oDbDataReader["ContactNo"]);

                if (oDbDataReader["Email"] != DBNull.Value)
                    oCustomer.Email = Convert.ToString(oDbDataReader["Email"]);

                if (oDbDataReader["Address"] != DBNull.Value)
                    oCustomer.Address = Convert.ToString(oDbDataReader["Address"]);

                if (oDbDataReader["Country"] != DBNull.Value)
                    oCustomer.Country = Convert.ToInt32(oDbDataReader["Country"]);

                if (oDbDataReader["IsActive"] != DBNull.Value)
                    oCustomer.IsActive = Convert.ToBoolean(oDbDataReader["IsActive"]);

                if (oDbDataReader["APIKey"] != DBNull.Value)
                    oCustomer.APIKey = Convert.ToString(oDbDataReader["APIKey"]);

                if (oDbDataReader["SenderId"] != DBNull.Value)
                    oCustomer.SenderId = Convert.ToString(oDbDataReader["SenderId"]);

                if (oDbDataReader["ReceiverId"] != DBNull.Value)
                    oCustomer.ReceiverId = Convert.ToString(oDbDataReader["ReceiverId"]);

                if (oDbDataReader["CreatedOn"] != DBNull.Value)
                    oCustomer.CreatedOn = Convert.ToDateTime(oDbDataReader["CreatedOn"]);

                if (oDbDataReader["LastModified"] != DBNull.Value)
                    oCustomer.LastModified = Convert.ToDateTime(oDbDataReader["LastModified"]);

                if (oDbDataReader["CreatedBy"] != DBNull.Value)
                    oCustomer.CreatedBy = Convert.ToInt32(oDbDataReader["CreatedBy"]);

                if (oDbDataReader["ModifiedBy"] != DBNull.Value)
                    oCustomer.ModifiedBy = Convert.ToInt32(oDbDataReader["ModifiedBy"]);

                if (oDbDataReader["IsDeleted"] != DBNull.Value)
                    oCustomer.IsDeleted = Convert.ToBoolean(oDbDataReader["IsDeleted"]);

                if (oDbDataReader["APIUrl"] != DBNull.Value)
                    oCustomer.APIUrl = Convert.ToString(oDbDataReader["APIUrl"]);

                if (oDbDataReader["ProviderId"] != DBNull.Value)
                    oCustomer.ProviderId = Convert.ToInt32(oDbDataReader["ProviderId"]);

                if (oDbDataReader["IsSSCC"] != DBNull.Value)
                    oCustomer.IsSSCC = Convert.ToBoolean(oDbDataReader["IsSSCC"]);

                if (oDbDataReader["CompanyCode"] != DBNull.Value)
                    oCustomer.CompanyCode = Convert.ToString(oDbDataReader["CompanyCode"]);

                if (oDbDataReader["BizLocGLN"] != DBNull.Value)
                    oCustomer.BizLocGLN = Convert.ToString(oDbDataReader["BizLocGLN"]);

                if (oDbDataReader["BizLocGLN_Ext"] != DBNull.Value)
                    oCustomer.BizLocGLN_Ext = Convert.ToString(oDbDataReader["BizLocGLN_Ext"]);

                if (oDbDataReader["stateOrRegion"] != DBNull.Value)
                    oCustomer.stateOrRegion = Convert.ToInt32(oDbDataReader["stateOrRegion"]);

                if (oDbDataReader["city"] != DBNull.Value)
                    oCustomer.city = Convert.ToString(oDbDataReader["city"]);

                if (oDbDataReader["postalCode"] != DBNull.Value)
                    oCustomer.postalCode = Convert.ToString(oDbDataReader["postalCode"]);

                if (oDbDataReader["License"] != DBNull.Value)
                    oCustomer.License = Convert.ToString(oDbDataReader["License"]);

                if (oDbDataReader["LicenseState"] != DBNull.Value)
                    oCustomer.LicenseState = Convert.ToString(oDbDataReader["LicenseState"]);

                if (oDbDataReader["LicenseAgency"] != DBNull.Value)
                    oCustomer.LicenseAgency = Convert.ToString(oDbDataReader["LicenseAgency"]);

                if (oDbDataReader["street1"] != DBNull.Value)
                    oCustomer.street1 = Convert.ToString(oDbDataReader["street1"]);

                if (oDbDataReader["street2"] != DBNull.Value)
                    oCustomer.street2 = Convert.ToString(oDbDataReader["street2"]);

                if (oDbDataReader["Host"] != DBNull.Value)
                    oCustomer.Host = Convert.ToString(oDbDataReader["Host"]);

                if (oDbDataReader["HostPswd"] != DBNull.Value)
                    oCustomer.HostPswd = Convert.ToString(oDbDataReader["HostPswd"]);

                if (oDbDataReader["HostPort"] != DBNull.Value)
                    oCustomer.HostPort = Convert.ToInt32(oDbDataReader["HostPort"]);

                if (oDbDataReader["HostUser"] != DBNull.Value)
                    oCustomer.HostUser = Convert.ToString(oDbDataReader["HostUser"]);

                if (oDbDataReader["LoosExt"] != DBNull.Value)
                    oCustomer.Loosext = Convert.ToString(oDbDataReader["LoosExt"]);
            }
            oDbDataReader.Close();
            return oCustomer;
        }
            
    }
}
