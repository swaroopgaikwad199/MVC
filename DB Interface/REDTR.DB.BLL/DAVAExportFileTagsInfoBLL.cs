using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
	public partial class DAVAExportFileTagsInfoBLL
	{
		private DAVAExportFileTagsInfoDAO _DAVAExportFileTagsInfoDAO;

		public DAVAExportFileTagsInfoDAO DAVAExportFileTagsInfoDAO
		{
			get { return _DAVAExportFileTagsInfoDAO; }
			set { _DAVAExportFileTagsInfoDAO = value; }
		}

		public DAVAExportFileTagsInfoBLL()
		{
			DAVAExportFileTagsInfoDAO = new DAVAExportFileTagsInfoDAO();
		}
		public List<DAVAExportFileTagsInfo> GetDAVAExportFileTagsInfos(int Flag,string Value)
		{
			try
			{
				return DAVAExportFileTagsInfoDAO.GetDAVAExportFileTagsInfos(Flag,Value);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public DataTable GetDAVAExportFileTagsInfosDataTable()
		{
			try
			{
                return null; //DAVAExportFileTagsInfoDAO.GetDAVAExportFileTagsInfosDataTable();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public DAVAExportFileTagsInfo GetDAVAExportFileTagsInfo(int ProductionInfo_Id)
		{
			try
			{
				return DAVAExportFileTagsInfoDAO.GetDAVAExportFileTagsInfo(ProductionInfo_Id);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
       
		public int AddDAVAExportFileTagsInfo(DAVAExportFileTagsInfo oDAVAExportFileTagsInfo)
		{
			try
			{
				return DAVAExportFileTagsInfoDAO.AddDAVAExportFileTagsInfo(oDAVAExportFileTagsInfo);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int UpdateDAVAExportFileTagsInfo(DAVAExportFileTagsInfo oDAVAExportFileTagsInfo)
		{
			try
			{
				return DAVAExportFileTagsInfoDAO.UpdateDAVAExportFileTagsInfo(oDAVAExportFileTagsInfo);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemoveDAVAExportFileTagsInfo(DAVAExportFileTagsInfo oDAVAExportFileTagsInfo)
		{
			try
			{
				return DAVAExportFileTagsInfoDAO.RemoveDAVAExportFileTagsInfo(oDAVAExportFileTagsInfo);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public int RemoveDAVAExportFileTagsInfo(int ProductionInfo_Id)
		{
			try
			{
				return DAVAExportFileTagsInfoDAO.RemoveDAVAExportFileTagsInfo(ProductionInfo_Id);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<DAVAExportFileTagsInfo> DeserializeDAVAExportFileTagsInfos(string Path)
		{
			try
			{
				return GenericXmlSerializer<List<DAVAExportFileTagsInfo>>.Deserialize(Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public void SerializeDAVAExportFileTagsInfos(string Path, List<DAVAExportFileTagsInfo> DAVAExportFileTagsInfos)
		{
			try
			{
				GenericXmlSerializer<List<DAVAExportFileTagsInfo>>.Serialize(DAVAExportFileTagsInfos, Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
