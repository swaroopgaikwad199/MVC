using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;
using System.Data.SqlTypes;

namespace REDTR.DB.DAL
{
    public class USerTrailDAO
    {
        public USerTrailDAO()
        {
            DbProviderHelper.GetConnection();
        }
        public List<USerTrail> GetUSerTrails(int Flag, Nullable<DateTime> FromDate, Nullable<DateTime> ToDate)
        {
            try
            {
                List<USerTrail> lstUSerTrails = new List<USerTrail>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETUSerTrail]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FromDate", DbType.DateTime, FromDate));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ToDate", DbType.DateTime, ToDate));
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    USerTrail oUSerTrail = new USerTrail();
                    oUSerTrail.ID = Convert.ToDecimal(oDbDataReader["ID"]);

                    if (oDbDataReader["AccessedAt"] != DBNull.Value)
                        oUSerTrail.AccessedAt = Convert.ToDateTime(oDbDataReader["AccessedAt"]);

                    if (oDbDataReader["Reason"] != DBNull.Value)
                        oUSerTrail.Reason = Convert.ToString(oDbDataReader["Reason"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oUSerTrail.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["LineCode"] != DBNull.Value)
                        oUSerTrail.LineCode = Convert.ToString(oDbDataReader["LineCode"]);
                    lstUSerTrails.Add(oUSerTrail);
                }
                oDbDataReader.Close();
                return lstUSerTrails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDSUSerTrails(int Flag, Nullable<DateTime> FromDate, Nullable<DateTime> ToDate)
        {
            try
            {
                DataSet ds;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETUSerTrail]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@FromDate", DbType.DateTime, FromDate));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ToDate", DbType.DateTime, ToDate));

                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                ds = DbProviderHelper.FillDataSet(DbAdpt);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddUSerTrail(USerTrail oUSerTrail)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateUSerTrail]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Decimal, oUSerTrail.ID));
                if (oUSerTrail.AccessedAt.HasValue)
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AccessedAt", DbType.DateTime, oUSerTrail.AccessedAt));
                else
                    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AccessedAt", DbType.DateTime, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Reason", DbType.Xml, oUSerTrail.Reason));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oUSerTrail.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, oUSerTrail.LineCode));
                
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddUSerTrail(USerTrail oUSerTrail,int id)
        {
            try
            {
                string Query = "INSERT INTO [dbo].USerTrail(AccessedAt,Reason,Remarks,LineCode) " +
                " VALUES(GetDate(),'" + oUSerTrail.Reason + "','" + oUSerTrail.Remarks + "','" + oUSerTrail.LineCode + "') ";
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(Query, CommandType.Text);
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Decimal, oUSerTrail.ID));
                //if (oUSerTrail.AccessedAt.HasValue)
                //    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AccessedAt", DbType.DateTime, oUSerTrail.AccessedAt));
                //else
                //    oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@AccessedAt", DbType.DateTime, DBNull.Value));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Reason", DbType.Xml, oUSerTrail.Reason));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oUSerTrail.Remarks));
                //oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, oUSerTrail.LineCode));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RemoveUSerTrails(int flag)
        {
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_DeleteUserTrail]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, flag));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
