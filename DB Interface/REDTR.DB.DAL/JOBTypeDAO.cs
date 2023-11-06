using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public class JOBTypeDAO
	{
		public JOBTypeDAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<JOBType> GetJOBTypes(int Flag,string Param)
		{
			try
			{
				List<JOBType> lstJOBTypes = new List<JOBType>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETJOBType]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Param));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					JOBType oJOBType = new JOBType();
					oJOBType.TID = Convert.ToDecimal(oDbDataReader["TID"]); 
					oJOBType.Job_Type = Convert.ToString(oDbDataReader["Job_Type"]);
                    oJOBType.Action = Convert.ToString(oDbDataReader["Action"]);                    
					oJOBType.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);
					lstJOBTypes.Add(oJOBType);
				}
				oDbDataReader.Close();
				return lstJOBTypes;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public List<string> JobTypes2Strings(int Flag)
        {
            try
            {
                List<string> lstJOBTypes = new List<string>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETJOBType]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, DBNull.Value));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    string oJob_Type = Convert.ToString(oDbDataReader["Job_Type"]);
                    lstJOBTypes.Add(oJob_Type);
                }
                oDbDataReader.Close();
                return lstJOBTypes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JOBType GetJOBType(int Flag, string  Param)
		{
			try
			{
				JOBType oJOBType = new JOBType();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GETJOBType", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int16, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Param));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					oJOBType.TID = Convert.ToDecimal(oDbDataReader["TID"]);
					oJOBType.Job_Type = Convert.ToString(oDbDataReader["Job_Type"]);
                    oJOBType.Action = Convert.ToString(oDbDataReader["Action"]);                    
					oJOBType.LastUpdatedDate = Convert.ToDateTime(oDbDataReader["LastUpdatedDate"]);
				}
				oDbDataReader.Close();
				return oJOBType;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public int InsertUpdateJOBType(JOBType oJOBType)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateJOBType]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int64, oJOBType.TID));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@JOBType", DbType.String, oJOBType.Job_Type));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Action", DbType.Xml, oJOBType.Action));                
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }	
	}
}
