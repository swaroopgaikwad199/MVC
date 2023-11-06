using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.DAL
{
	public partial class LineLocationDAO
	{
		public LineLocationDAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<LineLocation> GetLineLocations(int Flag,string Value)
		{
			try
			{
				List<LineLocation> lstLineLocations = new List<LineLocation>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GetLineLocation]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, Value));

				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					LineLocation oLineLocation = new LineLocation();
					oLineLocation.ID = Convert.ToString(oDbDataReader["ID"]);

					if(oDbDataReader["LocationCode"] != DBNull.Value)
						oLineLocation.LocationCode = Convert.ToString(oDbDataReader["LocationCode"]);

					if(oDbDataReader["DivisionCode"] != DBNull.Value)
						oLineLocation.DivisionCode = Convert.ToString(oDbDataReader["DivisionCode"]);

					if(oDbDataReader["PlantCode"] != DBNull.Value)
						oLineLocation.PlantCode = Convert.ToString(oDbDataReader["PlantCode"]);

					if(oDbDataReader["LineCode"] != DBNull.Value)
						oLineLocation.LineCode = Convert.ToString(oDbDataReader["LineCode"]);
					lstLineLocations.Add(oLineLocation);
				}
				oDbDataReader.Close();
				return lstLineLocations;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int AddLineLocation(LineLocation oLineLocation)
		{
			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("INSERTLineLocation",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID",DbType.String,oLineLocation.ID));
				if (oLineLocation.LocationCode!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LocationCode",DbType.String,oLineLocation.LocationCode));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LocationCode",DbType.String,DBNull.Value));
				if (oLineLocation.DivisionCode!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DivisionCode",DbType.String,oLineLocation.DivisionCode));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@DivisionCode",DbType.String,DBNull.Value));
				if (oLineLocation.PlantCode!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PlantCode",DbType.String,oLineLocation.PlantCode));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PlantCode",DbType.String,DBNull.Value));
				if (oLineLocation.LineCode!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode",DbType.String,oLineLocation.LineCode));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode",DbType.String,DBNull.Value));

				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        public DataSet GetLineLocationDataSet()
        {
            try
            {
                DataSet ds;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GetLineLocation]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, 1));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, "Test"));

                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                ds = DbProviderHelper.FillDataSet(DbAdpt);
                return ds;
            }
            catch (Exception ex )
            {
                throw ex;
            }

        }

        public List<LineLocation> GetLineLocationCode()
        {
            try
            {
                List<LineLocation> lstLineLocations = new List<LineLocation>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("Select Distinct LocationCode from LineLocation", CommandType.Text);

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    LineLocation oLineLocation = new LineLocation();
                    if (oDbDataReader["LocationCode"] != DBNull.Value)
                        oLineLocation.LocationCode = Convert.ToString(oDbDataReader["LocationCode"]);

                    lstLineLocations.Add(oLineLocation);
                }
                oDbDataReader.Close();
                return lstLineLocations;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<LineLocation> GetLineDivisionCode()
        {
            try
            {
                List<LineLocation> lstLineLocations = new List<LineLocation>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("Select Distinct DivisionCode from LineLocation", CommandType.Text);

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    LineLocation oLineLocation = new LineLocation();
                   
                    if (oDbDataReader["DivisionCode"] != DBNull.Value)
                        oLineLocation.DivisionCode = Convert.ToString(oDbDataReader["DivisionCode"]);

                    lstLineLocations.Add(oLineLocation);
                }
                oDbDataReader.Close();
                return lstLineLocations;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<LineLocation> GetLinePlantCode()
        {
            try
            {
                List<LineLocation> lstLineLocations = new List<LineLocation>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("Select Distinct PlantCode from LineLocation", CommandType.Text);

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    LineLocation oLineLocation = new LineLocation();

                    if (oDbDataReader["PlantCode"] != DBNull.Value)
                        oLineLocation.PlantCode = Convert.ToString(oDbDataReader["PlantCode"]);

                    lstLineLocations.Add(oLineLocation);
                }
                oDbDataReader.Close();
                return lstLineLocations;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable GetLineLocationDataTable(string query)
        {
            try
            {
                DataTable dt = new DataTable();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand(query, CommandType.Text);
                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                if (oDbDataReader !=null)
                  dt.Load(oDbDataReader);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetFinishedJobDetailView(int Flag)
        {
            try
            {
                DataSet ds;
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETLineDetails]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Decimal, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineID", DbType.Int32, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineName", DbType.String, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineDescription", DbType.String, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineOfficer", DbType.String, DBNull.Value));

                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@BatchNo", DbType.String, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CreatedBy", DbType.Decimal, DBNull.Value));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LastUpdatedDate", DbType.DateTime, DateTime.Now));
                DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
                ds = DbProviderHelper.FillDataSet(DbAdpt);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
// Added BY Ansuman For LineLocation Details in Reports

        public LineLocation GetLineLocationss(string value)
        {
            try
            {
                LineLocation oLineLocation = new LineLocation();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GetLineLocation]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, 3));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, value));

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    oLineLocation.ID = Convert.ToString(oDbDataReader["ID"]);

                    if (oDbDataReader["LocationCode"] != DBNull.Value)
                        oLineLocation.LocationCode = Convert.ToString(oDbDataReader["LocationCode"]);

                    if (oDbDataReader["DivisionCode"] != DBNull.Value)
                        oLineLocation.DivisionCode = Convert.ToString(oDbDataReader["DivisionCode"]);

                    if (oDbDataReader["PlantCode"] != DBNull.Value)
                        oLineLocation.PlantCode = Convert.ToString(oDbDataReader["PlantCode"]);

                    if (oDbDataReader["LineCode"] != DBNull.Value)
                        oLineLocation.LineCode = Convert.ToString(oDbDataReader["LineCode"]);

                    if (oDbDataReader["LineIP"] != DBNull.Value)
                        oLineLocation.LineIP = Convert.ToString(oDbDataReader["LineIP"]);

                    if (oDbDataReader["DBName"] != DBNull.Value)
                        oLineLocation.DBName = Convert.ToString(oDbDataReader["DBName"]);

                    if (oDbDataReader["IsActive"] != DBNull.Value)
                        oLineLocation.IsActive = Convert.ToBoolean(oDbDataReader["IsActive"]);

                    if (oDbDataReader["ServerName"] != DBNull.Value)
                        oLineLocation.ServerName = Convert.ToString(oDbDataReader["ServerName"]);

                    if (oDbDataReader["SQLPassword"] != DBNull.Value)
                        oLineLocation.SQLPassword = Convert.ToString(oDbDataReader["SQLPassword"]);

                    if (oDbDataReader["SQLUsername"] != DBNull.Value)
                        oLineLocation.SQLUsername = Convert.ToString(oDbDataReader["SQLUsername"]);

                    if (oDbDataReader["LineName"] != DBNull.Value)
                        oLineLocation.LineName = Convert.ToString(oDbDataReader["LineName"]);

                    if (oDbDataReader["ReadGLN"] != DBNull.Value)
                        oLineLocation.ReadGLN = Convert.ToString(oDbDataReader["ReadGLN"]);

                    if (oDbDataReader["GLNExtension"] != DBNull.Value)
                        oLineLocation.GLNExtension = Convert.ToString(oDbDataReader["GLNExtension"]);
                }
                oDbDataReader.Close();
                return oLineLocation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

	}

}
