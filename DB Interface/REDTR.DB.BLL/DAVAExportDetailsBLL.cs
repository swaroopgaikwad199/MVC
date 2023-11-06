using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
	public partial class DAVAExportDetailsBLL
	{
		private DAVAExportDetailsDAO _DAVAExportDetailsDAO;

		public DAVAExportDetailsDAO DAVAExportDetailsDAO
		{
			get { return _DAVAExportDetailsDAO; }
			set { _DAVAExportDetailsDAO = value; }
		}

		public DAVAExportDetailsBLL()
		{
			DAVAExportDetailsDAO = new DAVAExportDetailsDAO();
		}
		public List<DAVAExportDetails> GetDAVAExportDetailss()
		{
			try
			{
				return DAVAExportDetailsDAO.GetDAVAExportDetailss();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public DataTable GetDAVAExportDetailssDataTable()
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
		public DAVAExportDetails GetDAVAExportDetails(int Id)
		{
			try
			{
				return DAVAExportDetailsDAO.GetDAVAExportDetails(Id);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public int AddDAVAExportDetails(DAVAExportDetails oDAVAExportDetails)
		{
			try
			{
				return DAVAExportDetailsDAO.AddDAVAExportDetails(oDAVAExportDetails);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int UpdateDAVAExportDetails(DAVAExportDetails oDAVAExportDetails)
		{
			try
			{
				return DAVAExportDetailsDAO.UpdateDAVAExportDetails(oDAVAExportDetails);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemoveDAVAExportDetails(DAVAExportDetails oDAVAExportDetails)
		{
			try
			{
				return DAVAExportDetailsDAO.RemoveDAVAExportDetails(oDAVAExportDetails);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemoveDAVAExportDetails(int Id)
		{
			try
			{
				return DAVAExportDetailsDAO.RemoveDAVAExportDetails(Id);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<DAVAExportDetails> DeserializeDAVAExportDetailss(string Path)
		{
			try
			{
				return GenericXmlSerializer<List<DAVAExportDetails>>.Deserialize(Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public void SerializeDAVAExportDetailss(string Path, List<DAVAExportDetails> DAVAExportDetailss)
		{
			try
			{
				GenericXmlSerializer<List<DAVAExportDetails>>.Serialize(DAVAExportDetailss, Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
