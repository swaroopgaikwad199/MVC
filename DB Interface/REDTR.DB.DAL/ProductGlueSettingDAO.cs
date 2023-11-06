using REDTR.DB.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace REDTR.DB.DAL
{
    public partial class ProductGlueSettingDAO
    {
        public ProductGlueSettingDAO()
        {
            DbProviderHelper.GetConnection();
        }

        public ProductGlueSetting GETProductGlueSetting(decimal Flag,decimal ServerPAID)
        {
            ProductGlueSetting pro = new ProductGlueSetting();
            DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GETProductGlueSetting", CommandType.StoredProcedure);
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag ", DbType.Decimal, Flag));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ServerPAID ", DbType.Decimal, ServerPAID));
            DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
            while (oDbDataReader.Read())
            {
                pro.HotGlueDotSize = Convert.ToSingle(oDbDataReader["HotGlueDotSize"]);
                pro.HotGlueGapDistance = Convert.ToSingle(oDbDataReader["HotGlueGapDistance"]);
                pro.HotGlueStartDistance = Convert.ToSingle(oDbDataReader["HotGlueStartDistance"]);
                pro.ColdGlueDotSize = Convert.ToSingle(oDbDataReader["ColdGlueDotSize"]);
                pro.ColdGlueGapDistance = Convert.ToSingle(oDbDataReader["ColdGlueGapDistance"]);
                pro.ColdGlueStartDistance = Convert.ToSingle(oDbDataReader["ColdGlueStartDistance"]);
            }
            oDbDataReader.Close();
            return pro;
        }


    }
}
