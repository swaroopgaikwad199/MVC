using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
	public partial class CountryBLL
	{
		private CountryDAO _CountryDAO;

		public CountryDAO CountryDAO
		{
			get { return _CountryDAO; }
			set { _CountryDAO = value; }
		}

		public CountryBLL()
		{
			CountryDAO = new CountryDAO();
		}
		public List<Country> GetCountrys()
		{
			try
			{
				return CountryDAO.GetCountrys();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
        //public DataTable GetCountrysDataTable()
        //{
        //    try
        //    {
        //        return CountryDAO.GetCountrysDataTable();
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
		public Country GetCountry(int Id)
		{
			try
			{
				return CountryDAO.GetCountry(Id);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public int AddCountry(Country oCountry)
		{
			try
			{
				return CountryDAO.AddCountry(oCountry);
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
				return CountryDAO.UpdateCountry(oCountry);
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
				return CountryDAO.RemoveCountry(oCountry);
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
				return CountryDAO.RemoveCountry(Id);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<Country> DeserializeCountrys(string Path)
		{
			try
			{
				return GenericXmlSerializer<List<Country>>.Deserialize(Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public void SerializeCountrys(string Path, List<Country> Countrys)
		{
			try
			{
				GenericXmlSerializer<List<Country>>.Serialize(Countrys, Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
