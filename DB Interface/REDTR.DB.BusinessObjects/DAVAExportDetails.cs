using System;
using System.Text;
using System.Runtime.Serialization;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	//[DataContract()]
	public class DAVAExportDetails
	{
		private int _Id;

		//[DataMember]
		public int Id
		{
			get { return _Id; }
			set { _Id = value; }
		}

		private int _JobId;

		//[DataMember]
		public int JobId
		{
			get { return _JobId; }
			set { _JobId = value; }
		}

		private string _BatchName;

		//[DataMember]
		public string BatchName
		{
			get { return _BatchName; }
			set { _BatchName = value; }
		}

		private string _ProductCode;

		//[DataMember]
		public string ProductCode
		{
			get { return _ProductCode; }
			set { _ProductCode = value; }
		}

		private Nullable<int> _BatchQuantity;

		//[DataMember]
		public Nullable<int> BatchQuantity
		{
			get { return _BatchQuantity; }
			set { _BatchQuantity = value; }
		}

		private Nullable<bool> _ExemptedFromBarcoding;

		//[DataMember]
		public Nullable<bool> ExemptedFromBarcoding
		{
			get { return _ExemptedFromBarcoding; }
			set { _ExemptedFromBarcoding = value; }
		}

		private Nullable<DateTime> _ExemptionDate;

		//[DataMember]
		public Nullable<DateTime> ExemptionDate
		{
			get { return _ExemptionDate; }
			set { _ExemptionDate = value; }
		}

		private string _ExemptedCountryCode;

		//[DataMember]
		public string ExemptedCountryCode
		{
			get { return _ExemptedCountryCode; }
			set { _ExemptedCountryCode = value; }
		}

		private string _BatchStatus;

		//[DataMember]
		public string BatchStatus
		{
			get { return _BatchStatus; }
			set { _BatchStatus = value; }
		}

		private Nullable<DateTime> _LastUpdatedDate;

		//[DataMember]
		public Nullable<DateTime> LastUpdatedDate
		{
			get { return _LastUpdatedDate; }
			set { _LastUpdatedDate = value; }
		}

		private Nullable<int> _PrimaryPackPCMap;

		//[DataMember]
		public Nullable<int> PrimaryPackPCMap
		{
			get { return _PrimaryPackPCMap; }
			set { _PrimaryPackPCMap = value; }
		}

        private Nullable<int> _ProductionInfo_Id;

        public Nullable<int> ProductionInfo_Id
        {
            get { return _ProductionInfo_Id; }
            set { _ProductionInfo_Id = value; }
        }

		public DAVAExportDetails()
		{ }

        public DAVAExportDetails(int Id, int JobId, string BatchName, string ProductCode, Nullable<int> BatchQuantity, Nullable<bool> ExemptedFromBarcoding, Nullable<DateTime> ExemptionDate, string ExemptedCountryCode, string BatchStatus, Nullable<DateTime> LastUpdatedDate, Nullable<int> PrimaryPackPCMap, Nullable<int> ProductionInfo_Id)
		{
			this.Id = Id;
			this.JobId = JobId;
			this.BatchName = BatchName;
			this.ProductCode = ProductCode;
			this.BatchQuantity = BatchQuantity;
			this.ExemptedFromBarcoding = ExemptedFromBarcoding;
			this.ExemptionDate = ExemptionDate;
			this.ExemptedCountryCode = ExemptedCountryCode;
			this.BatchStatus = BatchStatus;
			this.LastUpdatedDate = LastUpdatedDate;
			this.PrimaryPackPCMap = PrimaryPackPCMap;
            this.ProductionInfo_Id = ProductionInfo_Id;
        }

		public override string ToString()
		{
			return "Id = " + Id.ToString() + ",JobId = " + JobId.ToString() + ",BatchName = " + BatchName + ",ProductCode = " + ProductCode + ",BatchQuantity = " + BatchQuantity.ToString() + ",ExemptedFromBarcoding = " + ExemptedFromBarcoding.ToString() + ",ExemptionDate = " + ExemptionDate.ToString() + ",ExemptedCountryCode = " + ExemptedCountryCode + ",BatchStatus = " + BatchStatus + ",LastUpdatedDate = " + LastUpdatedDate.ToString() + ",PrimaryPackPCMap = " + PrimaryPackPCMap.ToString()+",ProductionInfo_Id = " + ProductionInfo_Id.ToString();
		}

		public class IdComparer : System.Collections.Generic.IComparer<DAVAExportDetails>
		{
			public SorterMode SorterMode;
			public IdComparer()
			{ }
			public IdComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<DAVAExportDetails> Membres
			int System.Collections.Generic.IComparer<DAVAExportDetails>.Compare(DAVAExportDetails x, DAVAExportDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Id.CompareTo(x.Id);
				}
				else
				{
					return x.Id.CompareTo(y.Id);
				}
			}
			#endregion
		}
		public class JobIdComparer : System.Collections.Generic.IComparer<DAVAExportDetails>
		{
			public SorterMode SorterMode;
			public JobIdComparer()
			{ }
			public JobIdComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<DAVAExportDetails> Membres
			int System.Collections.Generic.IComparer<DAVAExportDetails>.Compare(DAVAExportDetails x, DAVAExportDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.JobId.CompareTo(x.JobId);
				}
				else
				{
					return x.JobId.CompareTo(y.JobId);
				}
			}
			#endregion
		}
		public class BatchNameComparer : System.Collections.Generic.IComparer<DAVAExportDetails>
		{
			public SorterMode SorterMode;
			public BatchNameComparer()
			{ }
			public BatchNameComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<DAVAExportDetails> Membres
			int System.Collections.Generic.IComparer<DAVAExportDetails>.Compare(DAVAExportDetails x, DAVAExportDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.BatchName.CompareTo(x.BatchName);
				}
				else
				{
					return x.BatchName.CompareTo(y.BatchName);
				}
			}
			#endregion
		}
		public class ProductCodeComparer : System.Collections.Generic.IComparer<DAVAExportDetails>
		{
			public SorterMode SorterMode;
			public ProductCodeComparer()
			{ }
			public ProductCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<DAVAExportDetails> Membres
			int System.Collections.Generic.IComparer<DAVAExportDetails>.Compare(DAVAExportDetails x, DAVAExportDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.ProductCode.CompareTo(x.ProductCode);
				}
				else
				{
					return x.ProductCode.CompareTo(y.ProductCode);
				}
			}
			#endregion
		}
		public class ExemptedCountryCodeComparer : System.Collections.Generic.IComparer<DAVAExportDetails>
		{
			public SorterMode SorterMode;
			public ExemptedCountryCodeComparer()
			{ }
			public ExemptedCountryCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<DAVAExportDetails> Membres
			int System.Collections.Generic.IComparer<DAVAExportDetails>.Compare(DAVAExportDetails x, DAVAExportDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.ExemptedCountryCode.CompareTo(x.ExemptedCountryCode);
				}
				else
				{
					return x.ExemptedCountryCode.CompareTo(y.ExemptedCountryCode);
				}
			}
			#endregion
		}
		public class BatchStatusComparer : System.Collections.Generic.IComparer<DAVAExportDetails>
		{
			public SorterMode SorterMode;
			public BatchStatusComparer()
			{ }
			public BatchStatusComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<DAVAExportDetails> Membres
			int System.Collections.Generic.IComparer<DAVAExportDetails>.Compare(DAVAExportDetails x, DAVAExportDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.BatchStatus.CompareTo(x.BatchStatus);
				}
				else
				{
					return x.BatchStatus.CompareTo(y.BatchStatus);
				}
			}
			#endregion
		}
	}
}
