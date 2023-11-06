using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;


namespace REDTR.DB.DAL
{
	public partial class CountryDAO
	{
		public CountryDAO()
		{
			DbProviderHelper.GetConnection();
		}
		public List<Country> GetCountrys()
		{
			try
			{
				List<Country> lstCountrys = new List<Country>();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("up_GetCountry",CommandType.StoredProcedure);
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Flag", DbType.Int32, 2));
                oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Value", DbType.String, "test"));

				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					Country oCountry = new Country();
					oCountry.Id = Convert.ToInt32(oDbDataReader["Id"]);

					if(oDbDataReader["CountryName"] != DBNull.Value)
						oCountry.CountryName = Convert.ToString(oDbDataReader["CountryName"]);

					if(oDbDataReader["TwoLetterAbbreviation"] != DBNull.Value)
						oCountry.TwoLetterAbbreviation = Convert.ToString(oDbDataReader["TwoLetterAbbreviation"]);

					if(oDbDataReader["ThreeLetterAbbreviation"] != DBNull.Value)
						oCountry.ThreeLetterAbbreviation = Convert.ToString(oDbDataReader["ThreeLetterAbbreviation"]);
					lstCountrys.Add(oCountry);
				}
				oDbDataReader.Close();
				return lstCountrys;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public Country GetCountry(int Id)
		{
			try
			{
				Country oCountry = new Country();
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("SELECTCountry",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Id",DbType.Int32,Id));
				DbDataReader oDbDataReader = DbProviderHelper.ExecuteReader(oDbCommand);
				while (oDbDataReader.Read())
				{
					oCountry.Id = Convert.ToInt32(oDbDataReader["Id"]);

					if(oDbDataReader["CountryName"] != DBNull.Value)
						oCountry.CountryName = Convert.ToString(oDbDataReader["CountryName"]);

					if(oDbDataReader["TwoLetterAbbreviation"] != DBNull.Value)
						oCountry.TwoLetterAbbreviation = Convert.ToString(oDbDataReader["TwoLetterAbbreviation"]);

					if(oDbDataReader["ThreeLetterAbbreviation"] != DBNull.Value)
						oCountry.ThreeLetterAbbreviation = Convert.ToString(oDbDataReader["ThreeLetterAbbreviation"]);
				}
				oDbDataReader.Close();
				return oCountry;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int AddCountry(Country oCountry)
		{
			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("INSERTCountry",CommandType.StoredProcedure);
				if (oCountry.CountryName!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountryName",DbType.String,oCountry.CountryName));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountryName",DbType.String,DBNull.Value));
				if (oCountry.TwoLetterAbbreviation!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TwoLetterAbbreviation",DbType.String,oCountry.TwoLetterAbbreviation));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TwoLetterAbbreviation",DbType.String,DBNull.Value));
				if (oCountry.ThreeLetterAbbreviation!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ThreeLetterAbbreviation",DbType.String,oCountry.ThreeLetterAbbreviation));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ThreeLetterAbbreviation",DbType.String,DBNull.Value));

				return Convert.ToInt32(DbProviderHelper.ExecuteScalar(oDbCommand));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int UpdateCountry(Country oCountry)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("UPDATECountry",CommandType.StoredProcedure);
				if (oCountry.CountryName!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountryName",DbType.String,oCountry.CountryName));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@CountryName",DbType.String,DBNull.Value));
				if (oCountry.TwoLetterAbbreviation!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TwoLetterAbbreviation",DbType.String,oCountry.TwoLetterAbbreviation));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@TwoLetterAbbreviation",DbType.String,DBNull.Value));
				if (oCountry.ThreeLetterAbbreviation!=null)
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ThreeLetterAbbreviation",DbType.String,oCountry.ThreeLetterAbbreviation));
				else
					oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@ThreeLetterAbbreviation",DbType.String,DBNull.Value));
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Id",DbType.Int32,oCountry.Id));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemoveCountry(Country oCountry)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("DELETECountry",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Id",DbType.Int32,oCountry.Id));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemoveCountry(int Id)
		{

			try
			{
				DbCommand oDbCommand = DbProviderHelper.CreateCommand("DELETECountry",CommandType.StoredProcedure);
				oDbCommand.Parameters.Add(DbProviderHelper.CreateParameter("@Id",DbType.Int32,Id));
				return DbProviderHelper.ExecuteNonQuery(oDbCommand);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
