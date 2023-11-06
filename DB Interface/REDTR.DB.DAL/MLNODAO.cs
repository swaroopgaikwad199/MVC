using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public class MLNODAO
	{
		public MLNODAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<MLNO> GetMLNOs(int Flag)
		{
			try
			{
				List<MLNO> lstMLNOs = new List<MLNO>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETMLNO]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int16, DBNull.Value));
                
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					MLNO oMLNO = new MLNO();
					oMLNO.ID = Convert.ToDecimal(oDbDataReader["ID"]);
                    oMLNO.ML_NO = Convert.ToString(oDbDataReader["ML_NO"]);

					if(oDbDataReader["Description"] != DBNull.Value)
						oMLNO.Description = Convert.ToString(oDbDataReader["Description"]);
					oMLNO.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);
					lstMLNOs.Add(oMLNO);
				}
				oDbDataReader.Close();
				return lstMLNOs;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public MLNO GetMLNO(int Flag ,Decimal ID)
		{
			try
			{
				MLNO oMLNO = new MLNO();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETMLNO]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID",DbType.Decimal,ID));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					oMLNO.ID = Convert.ToDecimal(oDbDataReader["ID"]);
                    oMLNO.ML_NO = Convert.ToString(oDbDataReader["ML_NO"]);

					if(oDbDataReader["Description"] != DBNull.Value)
						oMLNO.Description = Convert.ToString(oDbDataReader["Description"]);
					oMLNO.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);
				}
				oDbDataReader.Close();
				return oMLNO;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public int InsertOrUpdateMLNO(MLNO oMLNO)
		{
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateMLNO]", CommandType.StoredProcedure);

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Decimal, oMLNO.ID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@MLNO", DbType.String, oMLNO.ML_NO));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Desc", DbType.String, oMLNO.Description));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate",DbType.DateTime,oMLNO.LastUpdatedDate));

                return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}	
		public int RemoveMLNO(Decimal ID)
		{

			try
			{
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_DeleteMLNO]", CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID",DbType.Decimal,ID));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
