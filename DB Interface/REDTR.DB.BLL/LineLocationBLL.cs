using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using REDTR.DB.DAL;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.BLL
{
	public partial class LineLocationBLL
	{
        public enum LineLocationOp 
        {
            GetAllLines = 1,
            GetFreeLines = 2,

            GetLineDetailsSelectedBatch=5,
            GetFinishedJobDetails=7,
         
        }
		private LineLocationDAO _LineLocationDAO;

		public LineLocationDAO LineLocationDAO
		{
			get { return _LineLocationDAO; }
			set { _LineLocationDAO = value; }
		}

		public LineLocationBLL()
		{
			LineLocationDAO = new LineLocationDAO();
		}
        public List<LineLocation> GetLineLocations(LineLocationOp Flag,string Value)
		{
			try
			{
                return LineLocationDAO.GetLineLocations((int)Flag,Value);
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
        public List<LineLocation> GetLineLocationCode()
        {
            try
            {
                return LineLocationDAO.GetLineLocationCode();
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
                return LineLocationDAO.GetLineDivisionCode();
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
                return LineLocationDAO.GetLinePlantCode();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetLineLocationsDataset()
        {
            try
            {
                return LineLocationDAO.GetLineLocationDataSet();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetLineLocationsDataTable(string Query)
        {
            try
            {
                return LineLocationDAO.GetLineLocationDataTable(Query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetFinishedJobDetailDataSet(LineLocationOp Flag)
        {
            try
            {
                return LineLocationDAO.GetFinishedJobDetailView((int)Flag);
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
				return LineLocationDAO.AddLineLocation(oLineLocation);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public List<LineLocation> DeserializeLineLocations(string Path)
		{
			try
			{
				return GenericXmlSerializer<List<LineLocation>>.Deserialize(Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public void SerializeLineLocations(string Path, List<LineLocation> LineLocations)
		{
			try
			{
				GenericXmlSerializer<List<LineLocation>>.Serialize(LineLocations, Path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        // Added BY Ansuman For LineLocation Details in Reports
        public LineLocation GetLineLocationss(string ID)
        {
            try
            {
                return LineLocationDAO.GetLineLocationss(ID);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
	}
}
