using System;
using System.Text;
using System.Runtime.Serialization;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	//[DataContract()]
	public class DAVAExportFileTagsInfo
	{
		private int _ProductionInfo_Id;

		//[DataMember]
		public int ProductionInfo_Id
		{
			get { return _ProductionInfo_Id; }
			set { _ProductionInfo_Id = value; }
		}

        private string _FileData;

        public string FileData
        {
            get { return _FileData; }
            set { _FileData = value; }
        }

		private string _FILENAME;

		//[DataMember]
		public string FILENAME
		{
			get { return _FILENAME; }
			set { _FILENAME = value; }
		}

		private Nullable<DateTime> _CreatedDate;

		//[DataMember]
		public Nullable<DateTime> CreatedDate
		{
			get { return _CreatedDate; }
			set { _CreatedDate = value; }
		}

		private string _TypeofUpload;

		//[DataMember]
		public string TypeofUpload
		{
			get { return _TypeofUpload; }
			set { _TypeofUpload = value; }
		}

   
		public DAVAExportFileTagsInfo()
		{ }

        public DAVAExportFileTagsInfo(int ProductionInfo_Id, string FILENAME, Nullable<DateTime> CreatedDate, string TypeofUpload, string FileData)
		{
			this.ProductionInfo_Id = ProductionInfo_Id;
			//this.ENVELOPE_Id = ENVELOPE_Id;
			this.FILENAME = FILENAME;
			this.CreatedDate = CreatedDate;
			this.TypeofUpload = TypeofUpload;
            this.FileData = FileData;
		}

		public override string ToString()
		{
			return "ProductionInfo_Id = " + ProductionInfo_Id.ToString() + ",FILENAME = " + FILENAME + ",CreatedDate = " + CreatedDate.ToString() + ",TypeofUpload = " + TypeofUpload+",FileData="+FileData;
		}

		public class ProductionInfo_IdComparer : System.Collections.Generic.IComparer<DAVAExportFileTagsInfo>
		{
			public SorterMode SorterMode;
			public ProductionInfo_IdComparer()
			{ }
			public ProductionInfo_IdComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<DAVAExportFileTagsInfo> Membres
			int System.Collections.Generic.IComparer<DAVAExportFileTagsInfo>.Compare(DAVAExportFileTagsInfo x, DAVAExportFileTagsInfo y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.ProductionInfo_Id.CompareTo(x.ProductionInfo_Id);
				}
				else
				{
					return x.ProductionInfo_Id.CompareTo(y.ProductionInfo_Id);
				}
			}
			#endregion
		}
		public class FILENAMEComparer : System.Collections.Generic.IComparer<DAVAExportFileTagsInfo>
		{
			public SorterMode SorterMode;
			public FILENAMEComparer()
			{ }
			public FILENAMEComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<DAVAExportFileTagsInfo> Membres
			int System.Collections.Generic.IComparer<DAVAExportFileTagsInfo>.Compare(DAVAExportFileTagsInfo x, DAVAExportFileTagsInfo y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.FILENAME.CompareTo(x.FILENAME);
				}
				else
				{
					return x.FILENAME.CompareTo(y.FILENAME);
				}
			}
			#endregion
		}
		public class TypeofUploadComparer : System.Collections.Generic.IComparer<DAVAExportFileTagsInfo>
		{
			public SorterMode SorterMode;
			public TypeofUploadComparer()
			{ }
			public TypeofUploadComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<DAVAExportFileTagsInfo> Membres
			int System.Collections.Generic.IComparer<DAVAExportFileTagsInfo>.Compare(DAVAExportFileTagsInfo x, DAVAExportFileTagsInfo y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.TypeofUpload.CompareTo(x.TypeofUpload);
				}
				else
				{
					return x.TypeofUpload.CompareTo(y.TypeofUpload);
				}
			}
			#endregion
		}
	}
}
