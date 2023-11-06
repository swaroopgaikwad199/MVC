using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
    public partial class PackageLabelDAO
    {
        public PackageLabelDAO()
        {
            DbProviderHelper.GetConnection();
        }

        public List<PackageLabelAsso> GetPackageLabelAssos(int Flag, string Param1, string Param2)
        {
            try
            {
                List<PackageLabelAsso> lstPackaginglabelAssos = new List<PackageLabelAsso>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETPackageLabelAssos]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Param1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value1", DbType.String, Param2));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    PackageLabelAsso oPackagingLabelAsso = new PackageLabelAsso();
                    oPackagingLabelAsso.PAID = Convert.ToDecimal(oDbDataReader["PAID"]);
                    if (oDbDataReader["JobTypeID"] != DBNull.Value)
                        oPackagingLabelAsso.JobTypeID = Convert.ToDecimal(oDbDataReader["JobTypeID"]);

                    if (oDbDataReader["Code"] != DBNull.Value)
                        oPackagingLabelAsso.Code = Convert.ToString(oDbDataReader["Code"]);

                    if (oDbDataReader["LabelName"] != DBNull.Value)
                        oPackagingLabelAsso.LabelName = Convert.ToString(oDbDataReader["LabelName"]);


                    if (oDbDataReader["Filter"] != DBNull.Value)
                        oPackagingLabelAsso.Filter = Convert.ToString(oDbDataReader["Filter"]);

                    if (oDbDataReader["LastUpdatedDate"] != DBNull.Value)
                        oPackagingLabelAsso.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);
                    lstPackaginglabelAssos.Add(oPackagingLabelAsso);
                }
                oDbDataReader.Close();
                return lstPackaginglabelAssos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertOrUpdatePackageLabel(int Flag, PackageLabelAsso oPackagingLabel)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdatePackageLabel]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Int32, oPackagingLabel.PAID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobTypeID", DbType.Int32, oPackagingLabel.JobTypeID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Code", DbType.String, oPackagingLabel.Code));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LabelName", DbType.String, oPackagingLabel.LabelName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Filter", DbType.String, oPackagingLabel.Filter));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate", DbType.DateTime, DateTime.Now));
                DbProviderHelper.ExecuteNonQuery(oDbCommand);
                //return Convert.ToInt32(oDbCommand.Parameters["@RetID"].Value);
                //return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
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
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_DeletePackageLabelAsso", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PAID", DbType.Decimal, PAID));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
