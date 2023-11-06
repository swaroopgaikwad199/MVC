using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.BusinessObjects;
using REDTR.DB.DAL;

namespace REDTR.DB.BLL
{
	public partial class DavaFileRunningSeqNoBLL
	{

        public enum DavaFileRunningSeqNoOp
        {
            GETDavaFileRunningSeqNos = 1,
            GETDavaFileRunningSeqNo = 2,//[@ID as SSCCLineHolder ID]
          //  GetLastSSCC = 3,//[@ID as SSCCLineHolder PI]

        }
		private DavaFileRunningSeqNoDAO _DavaFileRunningSeqNoDAO;

		public DavaFileRunningSeqNoDAO DavaFileRunningSeqNoDAO
		{
			get { return _DavaFileRunningSeqNoDAO; }
			set { _DavaFileRunningSeqNoDAO = value; }
		}

		public DavaFileRunningSeqNoBLL()
		{
			DavaFileRunningSeqNoDAO = new DavaFileRunningSeqNoDAO();
		}
		public List<DavaFileRunningSeqNo> GetDavaFileRunningSeqNos(DavaFileRunningSeqNoOp daavaop)
		{
			try
			{
                return DavaFileRunningSeqNoDAO.GetDavaFileRunningSeqNos((int)daavaop);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
        public DavaFileRunningSeqNo GetDavaFileRunningSeqNos(DavaFileRunningSeqNoOp daavaop, int ID)
        {
            try
            {
                return DavaFileRunningSeqNoDAO.GetDavaFileRunningSeqNos((int)daavaop, ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		public DataTable GetDavaFileRunningSeqNosDataTable()
		{
			try
			{
                return null;//DavaFileRunningSeqNoDAO.GetDavaFileRunningSeqNosDataTable();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		public int AddDavaFileRunningSeqNo(DavaFileRunningSeqNo oDavaFileRunningSeqNo)
		{
			try
			{
				return DavaFileRunningSeqNoDAO.AddDavaFileRunningSeqNo(oDavaFileRunningSeqNo);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<DavaFileRunningSeqNo> DeserializeDavaFileRunningSeqNos(string Path)
		{
			try
			{
				return GenericXmlSerializer<List<DavaFileRunningSeqNo>>.Deserialize(Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public void SerializeDavaFileRunningSeqNos(string Path, List<DavaFileRunningSeqNo> DavaFileRunningSeqNos)
		{
			try
			{
				GenericXmlSerializer<List<DavaFileRunningSeqNo>>.Serialize(DavaFileRunningSeqNos, Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
