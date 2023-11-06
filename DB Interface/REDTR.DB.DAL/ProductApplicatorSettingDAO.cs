using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using REDTR.DB.BusinessObjects;
using System.Data.Common;
using System.Data;

namespace REDTR.DB.DAL
{
    public partial class ProductApplicatorSettingDAO
    {
        public ProductApplicatorSettingDAO()
        {
            DbProviderHelper.GetConnection();
        }

        public int InsertOrUpdateProductApplicatorSetting(ProductApplicatorSetting pro)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_insertorupdateProductApplicatorSetting]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ServerPAID", DbType.Decimal, pro.ServerPAID));

                if (pro.S1 != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S1", DbType.Decimal, pro.S1));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S1", DbType.Decimal, DBNull.Value));
                }

                if (pro.S2 != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S2", DbType.Decimal, pro.S2));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S2", DbType.Decimal, DBNull.Value));
                }

                if (pro.S3 != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S3", DbType.Decimal, pro.S3));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S3", DbType.Decimal, DBNull.Value));
                }

                if (pro.S4 != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S4", DbType.Decimal, pro.S4));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S4", DbType.Decimal, DBNull.Value));
                }

                if (pro.S5 != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S5", DbType.Decimal, pro.S5));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@S5", DbType.Decimal, DBNull.Value));
                }
                if (pro.BackLabelOffset != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BackLabelOffset", DbType.Decimal, pro.BackLabelOffset));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BackLabelOffset", DbType.Decimal, DBNull.Value));
                }

                if (pro.FrontLabelOffset != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FrontLabelOffset", DbType.Decimal, pro.FrontLabelOffset));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FrontLabelOffset", DbType.Decimal, DBNull.Value));
                }
                if (pro.CartonLength != null)
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CartonLength", DbType.Decimal, pro.CartonLength));
                }
                else
                {
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CartonLength", DbType.Decimal, DBNull.Value));
                }
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ProductApplicatorSetting GetProductApplicationSettind(decimal PAID,int Flag)
        {
            ProductApplicatorSetting pro=new ProductApplicatorSetting();
            DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GETProductApplicatorSetting", CommandType.StoredProcedure);
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ServerPAID", DbType.Decimal, PAID));
            oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
            DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
            while (oDbDataReader.Read())
            {
                pro.S1 =Convert.ToSingle(oDbDataReader["S1"]);
                pro.S2 = Convert.ToSingle(oDbDataReader["S3"]);
                pro.S3 = Convert.ToSingle(oDbDataReader["S3"]);
                pro.S4 = Convert.ToSingle(oDbDataReader["S4"]);
                pro.S5 = Convert.ToSingle(oDbDataReader["S5"]);
                pro.FrontLabelOffset= Convert.ToSingle(oDbDataReader["FrontLabelOffset"]);
                pro.BackLabelOffset = Convert.ToSingle(oDbDataReader["BackLabelOffset"]);
                pro.CartonLength = Convert.ToSingle(oDbDataReader["CartonLength"]);
            }
            oDbDataReader.Close();
            return pro;
        }
    }

}
