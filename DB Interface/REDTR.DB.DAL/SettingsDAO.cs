using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.DAL
{
	public class SettingsDAO
	{
		public SettingsDAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<Settings> GetSettingss(int Flag)
		{
			try
			{
				List<Settings> lstSettingss = new List<Settings>();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETSettings]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32,DBNull.Value));

				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					Settings oSettings = new Settings();

					if(oDbDataReader["id"] != DBNull.Value)
						oSettings.id = Convert.ToInt32(oDbDataReader["id"]);

					if(oDbDataReader["CompanyName"] != DBNull.Value)
						oSettings.CompanyName = Convert.ToString(oDbDataReader["CompanyName"]);

					if(oDbDataReader["Address"] != DBNull.Value)
						oSettings.Address = Convert.ToString(oDbDataReader["Address"]);

					if(oDbDataReader["Logo"] != DBNull.Value)
						oSettings.Logo = Convert.ToString(oDbDataReader["Logo"]);

					if(oDbDataReader["CompanyCode"] != DBNull.Value)
						oSettings.CompanyCode = Convert.ToString(oDbDataReader["CompanyCode"]);

					if(oDbDataReader["LineCode"] != DBNull.Value)
						oSettings.LineCode = Convert.ToString(oDbDataReader["LineCode"]);

					if(oDbDataReader["Remarks"] != DBNull.Value)
						oSettings.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["PlantCode"] != DBNull.Value)
                        oSettings.PlantCode = Convert.ToInt32(oDbDataReader["PlantCode"]);
					
                    lstSettingss.Add(oSettings);
				}
				oDbDataReader.Close();
				return lstSettingss;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        public Settings GetSetting(int Flag,int SettingID)
        {
            try
            {
                Settings oSettings = new Settings();
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_GETSettings]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, Flag));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ID", DbType.Int32, SettingID));

                DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
                while (oDbDataReader.Read())
                {
                    if (oDbDataReader["id"] != DBNull.Value)
                        oSettings.id = Convert.ToInt32(oDbDataReader["id"]);

                    if (oDbDataReader["CompanyName"] != DBNull.Value)
                        oSettings.CompanyName = Convert.ToString(oDbDataReader["CompanyName"]);

                    if (oDbDataReader["Address"] != DBNull.Value)
                        oSettings.Address = Convert.ToString(oDbDataReader["Address"]);

                    if (oDbDataReader["Logo"] != DBNull.Value)
                        oSettings.Logo = Convert.ToString(oDbDataReader["Logo"]);

                    if (oDbDataReader["CompanyCode"] != DBNull.Value)
                        oSettings.CompanyCode = Convert.ToString(oDbDataReader["CompanyCode"]);

                    if (oDbDataReader["LineCode"] != DBNull.Value)
                        oSettings.LineCode = Convert.ToString(oDbDataReader["LineCode"]);

                    if (oDbDataReader["Remarks"] != DBNull.Value)
                        oSettings.Remarks = Convert.ToString(oDbDataReader["Remarks"]);

                    if (oDbDataReader["PlantCode"] != DBNull.Value)
                        oSettings.PlantCode = Convert.ToInt32(oDbDataReader["PlantCode"]);
                }
                oDbDataReader.Close();
                return oSettings;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		public int AddSettings(Settings oSettings)
		{
            try
            {
                DbCommand oDbCommand = DbProviderHelper.CreateCommand("[up_InsertOrUpdateSettings]", CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CompanyName", DbType.String, oSettings.CompanyName));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Address", DbType.String, oSettings.Address));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Logo", DbType.String, oSettings.Logo));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CompanyCode", DbType.String, oSettings.CompanyCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@LineCode", DbType.String, oSettings.LineCode));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Remarks", DbType.String, oSettings.Remarks));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@PlantCode", DbType.Int32, oSettings.PlantCode));
                return DbProviderHelper.ExecuteNonQuery(oDbCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}

        public DataTable GetSenderDetails(int Id)
        {
            DataTable dt;
            DbCommand oDbCommand = DbProviderHelper.CreateCommand(" Select s.CompanyName, s.Street, st.StateName, s.city, s.postalCode, c.CountryName, s.License, s.LicenseState, s.LicenseAgency, s.GLN from dbo.Settings s, country c, S_State st where s.country = c.id, s.stateOrRegion = st.ID and s.id=" + Id, CommandType.Text);
            DbDataAdapter DbAdpt = DbProviderHelper.CreateDataAdapter(oDbCommand);
            dt = DbProviderHelper.FillDataTable(DbAdpt);
            return dt;
        }
    }
}
