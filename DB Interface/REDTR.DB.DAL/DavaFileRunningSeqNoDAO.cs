using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public partial class DavaFileRunningSeqNoDAO
	{
		public DavaFileRunningSeqNoDAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<DavaFileRunningSeqNo> GetDavaFileRunningSeqNos(int Flag)
		{
			try
			{
				List<DavaFileRunningSeqNo> lstDavaFileRunningSeqNos = new List<DavaFileRunningSeqNo>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GETDavaFileRunningSeqNo", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
             
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					DavaFileRunningSeqNo oDavaFileRunningSeqNo = new DavaFileRunningSeqNo();
					oDavaFileRunningSeqNo.ID = Convert.ToInt32(oDbDataReader["ID"]);

					if(oDbDataReader["LastRunningSeqNo"] != DBNull.Value)
						oDavaFileRunningSeqNo.LastRunningSeqNo = Convert.ToDecimal(oDbDataReader["LastRunningSeqNo"]);

					if(oDbDataReader["Remarks"] != DBNull.Value)
						oDavaFileRunningSeqNo.Remarks = Convert.ToString(oDbDataReader["Remarks"]);
					lstDavaFileRunningSeqNos.Add(oDavaFileRunningSeqNo);
				}
				oDbDataReader.Close();
				return lstDavaFileRunningSeqNos;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public DavaFileRunningSeqNo GetDavaFileRunningSeqNos(int Flag, int ID)
        {
            try
            {
                DavaFileRunningSeqNo oDavaFileRunningSeqNo = new DavaFileRunningSeqNo();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GETDavaFileRunningSeqNo", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, ID));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                  
                    oDavaFileRunningSeqNo.ID = Convert.ToInt32(oDbDataReader["ID"]);

                    if (oDbDataReader["LastRunningSeqNo"] != DBNull.Value)
                        oDavaFileRunningSeqNo.LastRunningSeqNo = Convert.ToDecimal(oDbDataReader["LastRunningSeqNo"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oDavaFileRunningSeqNo.Remarks = Convert.ToString(oDbDataReader["Remarks"]);
                   // lstDavaFileRunningSeqNos.Add(oDavaFileRunningSeqNo);
                }
                oDbDataReader.Close();
                return oDavaFileRunningSeqNo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		public int AddDavaFileRunningSeqNo(DavaFileRunningSeqNo oDavaFileRunningSeqNo)
		{
			try
			{
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_InsertOrUpdateDavaFileRunningSeqNo", CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID",DbType.Int32,oDavaFileRunningSeqNo.ID));
				if (oDavaFileRunningSeqNo.LastRunningSeqNo.HasValue)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastRunningSeqNo",DbType.Decimal,oDavaFileRunningSeqNo.LastRunningSeqNo));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastRunningSeqNo",DbType.Decimal,DBNull.Value));
				if (oDavaFileRunningSeqNo.Remarks!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks",DbType.String,oDavaFileRunningSeqNo.Remarks));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks",DbType.String,DBNull.Value));

				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
