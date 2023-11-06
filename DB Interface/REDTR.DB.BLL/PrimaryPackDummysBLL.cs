using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.DAL;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.BLL
{
	public partial class PrimaryPackDummysBLL
	{
		private PrimaryPackDummysDAO _PrimaryPackDummysDAO;

		public PrimaryPackDummysDAO PrimaryPackDummysDAO
		{
			get { return _PrimaryPackDummysDAO; }
			set { _PrimaryPackDummysDAO = value; }
		}

		public PrimaryPackDummysBLL()
		{
			PrimaryPackDummysDAO = new PrimaryPackDummysDAO();
		}
		public List<PrimaryPackDummys> GetPrimaryPackDummyss()
		{
			try
			{
				return PrimaryPackDummysDAO.GetPrimaryPackDummyss();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public DataTable GetPrimaryPackDummyssDataTable()
		{
			try
			{
                return null;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public PrimaryPackDummys GetPrimaryPackDummys(int Id)
		{
			try
			{
				return PrimaryPackDummysDAO.GetPrimaryPackDummys(Id);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public int AddPrimaryPackDummys(PrimaryPackDummys oPrimaryPackDummys)
		{
			try
			{
				return PrimaryPackDummysDAO.AddPrimaryPackDummys(oPrimaryPackDummys);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int UpdatePrimaryPackDummys(PrimaryPackDummys oPrimaryPackDummys)
		{
			try
			{
				return PrimaryPackDummysDAO.UpdatePrimaryPackDummys(oPrimaryPackDummys);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemovePrimaryPackDummys(PrimaryPackDummys oPrimaryPackDummys)
		{
			try
			{
				return PrimaryPackDummysDAO.RemovePrimaryPackDummys(oPrimaryPackDummys);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemovePrimaryPackDummys(int Id)
		{
			try
			{
				return PrimaryPackDummysDAO.RemovePrimaryPackDummys(Id);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<PrimaryPackDummys> DeserializePrimaryPackDummyss(string Path)
		{
			try
			{
				return GenericXmlSerializer<List<PrimaryPackDummys>>.Deserialize(Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public void SerializePrimaryPackDummyss(string Path, List<PrimaryPackDummys> PrimaryPackDummyss)
		{
			try
			{
				GenericXmlSerializer<List<PrimaryPackDummys>>.Serialize(PrimaryPackDummyss, Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
