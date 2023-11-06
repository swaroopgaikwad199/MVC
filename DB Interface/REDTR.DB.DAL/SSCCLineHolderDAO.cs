using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public partial class SSCCLineHolderDAO
	{
		public SSCCLineHolderDAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<SSCCLineHolder> GetSSCCLineHolders(int Flag)
		{
			try
			{
				List<SSCCLineHolder> lstSSCCLineHolders = new List<SSCCLineHolder>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETSSCCLineHolder]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, DBNull.Value));

				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					SSCCLineHolder oSSCCLineHolder = new SSCCLineHolder();

					if(oDbDataReader["ID"] != DBNull.Value)
						oSSCCLineHolder.ID = Convert.ToInt32(oDbDataReader["ID"]);

					if(oDbDataReader["PackageIndicator"] != DBNull.Value)
						oSSCCLineHolder.PackageIndicator = Convert.ToDecimal(oDbDataReader["PackageIndicator"]);

					if(oDbDataReader["LastSSCC"] != DBNull.Value)
						oSSCCLineHolder.LastSSCC = Convert.ToDecimal(oDbDataReader["LastSSCC"]);
                    
                    if (oDbDataReader["FirstSSCC"] != DBNull.Value)
                        oSSCCLineHolder.FirstSSCC = Convert.ToDecimal(oDbDataReader["FirstSSCC"]);

                    if (oDbDataReader["JobID"] != DBNull.Value)
                        oSSCCLineHolder.JobID = Convert.ToDecimal(oDbDataReader["JobID"]);

					if(oDbDataReader["Remarks"] != DBNull.Value)
						oSSCCLineHolder.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["LineCode"] != DBNull.Value)
                        oSSCCLineHolder.LineCode = Convert.ToString(oDbDataReader["LineCode"]);

					lstSSCCLineHolders.Add(oSSCCLineHolder);
				}
				oDbDataReader.Close();
				return lstSSCCLineHolders;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public SSCCLineHolder GetSSCCLineHolder(int Flag,int ID)
        {
            try
            {
                SSCCLineHolder oSSCCLineHolder = new SSCCLineHolder();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETSSCCLineHolder]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, ID));

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                  

                    if (oDbDataReader["ID"] != DBNull.Value)
                        oSSCCLineHolder.ID = Convert.ToInt32(oDbDataReader["ID"]);

                    if (oDbDataReader["PackageIndicator"] != DBNull.Value)
                        oSSCCLineHolder.PackageIndicator = Convert.ToDecimal(oDbDataReader["PackageIndicator"]);

                    if (oDbDataReader["LastSSCC"] != DBNull.Value)
                        oSSCCLineHolder.LastSSCC = Convert.ToDecimal(oDbDataReader["LastSSCC"]);

                    if (oDbDataReader["FirstSSCC"] != DBNull.Value)
                        oSSCCLineHolder.FirstSSCC = Convert.ToDecimal(oDbDataReader["FirstSSCC"]);

                    if (oDbDataReader["JobID"] != DBNull.Value)
                        oSSCCLineHolder.JobID = Convert.ToDecimal(oDbDataReader["JobID"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oSSCCLineHolder.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["LineCode"] != DBNull.Value)
                        oSSCCLineHolder.LineCode = Convert.ToString(oDbDataReader["LineCode"]);

                }
                oDbDataReader.Close();
                return oSSCCLineHolder;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public int GetLatestSSCCLineHolder()
        {
            try
            {
                int SSCC = 0;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("Select ISNULL(Max(FirstSSCC),0) As SSCC  from SSCCLineHolder", CommandType.Text);
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["SSCC"] != DBNull.Value)
                        SSCC = Convert.ToInt32(oDbDataReader["SSCC"]);
                }
                oDbDataReader.Close();
                return SSCC;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InsertOrUpdateSSCCLineHolder(SSCCLineHolder oSSCCLineHolder)
		{
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateSSCCLineHolder]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PackageIndicator", DbType.Decimal, oSSCCLineHolder.PackageIndicator));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastSSCC", DbType.Decimal, oSSCCLineHolder.LastSSCC));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FirstSSCC", DbType.Decimal, oSSCCLineHolder.FirstSSCC));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JobID", DbType.Decimal, oSSCCLineHolder.JobID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oSSCCLineHolder.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, oSSCCLineHolder.LineCode));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}
	}
}
