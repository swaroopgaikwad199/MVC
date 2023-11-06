using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class Job
	{
		private Decimal _JID;

		public Decimal JID
		{
			get { return _JID; }
			set { _JID = value; }
		}

        private Decimal _NoReadCount;
        public Decimal NoReadCount
        {
            get { return _NoReadCount; }
            set { _NoReadCount = value; }
        }
        //,ExpDateFormat,UseExpDay

        private string _ExpDateFormat;
        public string ExpDateFormat
        {
            get { return _ExpDateFormat; }
            set { _ExpDateFormat = value; }
        }

        private bool _UseExpDay;

        public bool UseExpDay
        {
            get { return _UseExpDay; }
            set { _UseExpDay = value; }
        }




        private string _LineCode; // Line Code [Sunil]
        public string LineCode
        {
            get { return _LineCode; }
            set { _LineCode = value; }
        }

		private string _JobName;

		public string JobName
		{
			get { return _JobName; }
			set { _JobName = value; }
		}

		private Decimal _PAID;

		public Decimal PAID
		{
			get { return _PAID; }
			set { _PAID = value; }
		}

		private string _BatchNo;

		public string BatchNo
		{
			get { return _BatchNo; }
			set { _BatchNo = value; }
		}

		private DateTime _MfgDate;

		public DateTime MfgDate
		{
			get { return _MfgDate; }
			set { _MfgDate = value; }
		}

		private DateTime _ExpDate;

		public DateTime ExpDate
		{
			get { return _ExpDate; }
			set { _ExpDate = value; }
		}

		private int _Quantity;

		public int Quantity
		{
			get { return _Quantity; }
			set { _Quantity = value; }
		}


        private int _PrimaryPCMapCount=1;

        public int PrimaryPCMapCount
        {
            get { return _PrimaryPCMapCount; }
            set { _PrimaryPCMapCount = value; }
        }

        private bool _DAVAPortalUpload;

        public bool DAVAPortalUpload
        {
            get { return _DAVAPortalUpload; }
            set { _DAVAPortalUpload = value; }
        }
		private Nullable<int> _SurPlusQty;

		public Nullable<int> SurPlusQty
		{
			get { return _SurPlusQty; }
			set { _SurPlusQty = value; }
		}        

		private SByte _JobStatus;

		public SByte JobStatus
		{
			get { return _JobStatus; }
			set { _JobStatus = value; }
		}

		private string _DetailInfo;

		public string DetailInfo
		{
			get { return _DetailInfo; }
			set { _DetailInfo = value; }
		}

		private DateTime _JobStartTime;

		public DateTime JobStartTime
		{
			get { return _JobStartTime; }
			set { _JobStartTime = value; }
		}

		private Nullable<DateTime> _JobEndTime;

		public Nullable<DateTime> JobEndTime
		{
			get { return _JobEndTime; }
			set { _JobEndTime = value; }
		}

		private Nullable<Decimal> _LabelStartIndex;

		public Nullable<Decimal> LabelStartIndex
		{
			get { return _LabelStartIndex; }
			set { _LabelStartIndex = value; }
		}

		private bool _AutomaticBatchCloser;

		public bool AutomaticBatchCloser
		{
            get { return _AutomaticBatchCloser; }
            set { _AutomaticBatchCloser = value; }
		}

		private Nullable<Decimal> _TID;

		public Nullable<Decimal> TID
		{
			get { return _TID; }
			set { _TID = value; }
		}

        private string  _MLNO;

        public string MLNO
		{
            get { return _MLNO; }
            set { _MLNO = value; }
		}

		private string _TenderText;

		public string TenderText
		{
			get { return _TenderText; }
			set { _TenderText = value; }
		}

        private Nullable<bool> _JobWithUID;

        public Nullable<bool> JobWithUID
		{
            get { return _JobWithUID; }
            set { _JobWithUID = value; }
		}

		private string _Remarks;

		public string Remarks
		{
			get { return _Remarks; }
			set { _Remarks = value; }
		}

		private Nullable<Decimal> _CreatedBy;

		public Nullable<Decimal> CreatedBy
		{
			get { return _CreatedBy; }
			set { _CreatedBy = value; }
		}

		private Nullable<Decimal> _VerifiedBy;

		public Nullable<Decimal> VerifiedBy
		{
			get { return _VerifiedBy; }
			set { _VerifiedBy = value; }
		}

        private Nullable<DateTime> _VerifiedDate;

        public Nullable<DateTime> VerifiedDate
        {
            get { return _VerifiedDate; }
            set { _VerifiedDate = value; }

        }

		private DateTime _CreatedDate;

		public DateTime CreatedDate
		{
			get { return _CreatedDate; }
			set { _CreatedDate = value; }
		}

		private DateTime _LastUpdatedDate;

		public DateTime LastUpdatedDate
		{
			get { return _LastUpdatedDate; }
			set { _LastUpdatedDate = value; }
		}
        private bool _ForExport;

        public bool ForExport
        {
            get { return _ForExport; }
            set { _ForExport = value; }
        }
        private int _AppId;

        public int AppId
        {
            get { return _AppId; }
            set { _AppId = value; }
        }

        private int _PackagingLvlId;

        public int PackagingLvlId
        {
            get { return _PackagingLvlId; }
            set { _PackagingLvlId = value; }
        }

        private Nullable<int> _CustomerId;

        public Nullable<int> CustomerId
        {
            get { return _CustomerId; }
            set { _CustomerId = value; }
        }


        private Nullable<int> _ProviderId;

        public Nullable<int> ProviderId
        {
            get { return _ProviderId; }
            set { _ProviderId = value; }
        }

        private string _PPNCountryCode;

        public string PPNCountryCode
        {
            get { return _PPNCountryCode; }
            set { _PPNCountryCode = value; }
        }

        private string _PPNPostalCode;

        public string PPNPostalCode
        {
            get { return _PPNPostalCode; }
            set { _PPNPostalCode = value; }
        }

        private string _CompType;
        public string CompType
        {
            get { return _CompType; }
            set { _CompType = value; }
        }
        private string _PlantCode;

        public string PlantCode
        {
            get { return _PlantCode; }
            set { _PlantCode = value; }
        }

        public Job()
		{ }

        public Job(Decimal JID, string JobName, Decimal PAID, string BatchNo, DateTime MfgDate, DateTime ExpDate, int Quantity, Nullable<int> SurPlusQty, SByte JobStatus, string DetailInfo, DateTime JobStartTime, Nullable<DateTime> JobEndTime, Nullable<Decimal> LabelStartIndex, Nullable<bool> AutomaticBatchCloser, Nullable<Decimal> TID, string MLNO, string TenderText, Nullable<bool> JobWithUID, string Remarks, Nullable<Decimal> CreatedBy, Nullable<Decimal> VerifiedBy, DateTime CreatedDate, DateTime LastUpdatedDate, int AppId, DateTime verifiedDate, bool ForExport, int PrimaryPCMapCount, bool DAVAPortalUpload,string LineCode, int PackagingLevelId,string PPNCountryCode,string PPNPostalCode,string PlantCode)
		{
			this.JID = JID;
			this.JobName = JobName;
			this.PAID = PAID;
			this.BatchNo = BatchNo;
			this.MfgDate = MfgDate;
			this.ExpDate = ExpDate;
			this.Quantity = Quantity;
            this.SurPlusQty = SurPlusQty;
			this.JobStatus = JobStatus;
			this.DetailInfo = DetailInfo;
			this.JobStartTime = JobStartTime;
			this.JobEndTime = JobEndTime;
			this.LabelStartIndex = LabelStartIndex;
            //this.AutomaticBatchCloser = AutomaticBatchCloser;
			this.TID = TID;
            this.MLNO = MLNO;
			this.TenderText = TenderText;
            this.JobWithUID = JobWithUID;
			this.Remarks = Remarks;
			this.CreatedBy = CreatedBy;
			this.VerifiedBy = VerifiedBy;
			this.CreatedDate = CreatedDate;
			this.LastUpdatedDate = LastUpdatedDate;
            this.AppId = AppId;
            this.VerifiedDate = verifiedDate;
            this.ForExport = ForExport;
            this.PrimaryPCMapCount = PrimaryPCMapCount;
            this.DAVAPortalUpload = DAVAPortalUpload;
            this.LineCode = LineCode;
            this.PackagingLvlId = PackagingLvlId;
            this.CustomerId = CustomerId;
            this.ProviderId = ProviderId;
            this.PPNCountryCode = PPNCountryCode;
            this.PPNPostalCode = PPNPostalCode;
            this.PlantCode = PlantCode;
		}

		public override string ToString()
		{
            return "JID = " + JID.ToString() + ",JobName = " + JobName + ",PAID = " + PAID.ToString() + ",BatchNo = " + BatchNo + ",MfgDate = " + MfgDate.ToString() + ",ExpDate = " + ExpDate.ToString() + ",Quantity = " + Quantity.ToString() + ",SurPlusQty = " + SurPlusQty.ToString() + ",JobStatus = " + JobStatus.ToString() + ",DetailInfo = " + DetailInfo + ",JobStartTime = " + JobStartTime.ToString() + ",JobEndTime = " + JobEndTime.ToString() + ",LabelStartIndex = " + LabelStartIndex.ToString() + ",AutomaticBatchCloser = " + AutomaticBatchCloser.ToString() + ",TID = " + TID.ToString() + ",MLNO = " + MLNO + ",TenderText = " + TenderText + ",JobWithUID = " + JobWithUID.ToString() + ",Remarks = " + Remarks + ",CreatedBy = " + CreatedBy.ToString() + ",VerifiedBy = " + VerifiedBy.ToString() + ",CreatedDate = " + CreatedDate.ToString() + ",LastUpdatedDate = " + LastUpdatedDate.ToString() + ",AppId=" + AppId.ToString() + ",VerifiedDate=" + VerifiedDate + ",FOREXPORT=" + ForExport.ToString() + ",PrimaryPCMapCount = " + PrimaryPCMapCount.ToString() + ",DAVAPortalUpload=" + DAVAPortalUpload.ToString()+",LineCode = "+LineCode+"";
		}

		public class JIDComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public JIDComparer()
			{ }
			public JIDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.JID.CompareTo(x.JID);
				}
				else
				{
					return x.JID.CompareTo(y.JID);
				}
			}
			#endregion
		}
		public class JobNameComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public JobNameComparer()
			{ }
			public JobNameComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.JobName.CompareTo(x.JobName);
				}
				else
				{
					return x.JobName.CompareTo(y.JobName);
				}
			}
			#endregion
		}
		public class PAIDComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public PAIDComparer()
			{ }
			public PAIDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.PAID.CompareTo(x.PAID);
				}
				else
				{
					return x.PAID.CompareTo(y.PAID);
				}
			}
			#endregion
		}
		public class BatchNoComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public BatchNoComparer()
			{ }
			public BatchNoComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.BatchNo.CompareTo(x.BatchNo);
				}
				else
				{
					return x.BatchNo.CompareTo(y.BatchNo);
				}
			}
			#endregion
		}
		public class MfgDateComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public MfgDateComparer()
			{ }
			public MfgDateComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.MfgDate.CompareTo(x.MfgDate);
				}
				else
				{
					return x.MfgDate.CompareTo(y.MfgDate);
				}
			}
			#endregion
		}
		public class ExpDateComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public ExpDateComparer()
			{ }
			public ExpDateComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.ExpDate.CompareTo(x.ExpDate);
				}
				else
				{
					return x.ExpDate.CompareTo(y.ExpDate);
				}
			}
			#endregion
		}
		public class QuantityComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public QuantityComparer()
			{ }
			public QuantityComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Quantity.CompareTo(x.Quantity);
				}
				else
				{
					return x.Quantity.CompareTo(y.Quantity);
				}
			}
			#endregion
		}        
		public class JobStatusComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public JobStatusComparer()
			{ }
			public JobStatusComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.JobStatus.CompareTo(x.JobStatus);
				}
				else
				{
					return x.JobStatus.CompareTo(y.JobStatus);
				}
			}
			#endregion
		}
		public class DetailInfoComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public DetailInfoComparer()
			{ }
			public DetailInfoComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.DetailInfo.CompareTo(x.DetailInfo);
				}
				else
				{
					return x.DetailInfo.CompareTo(y.DetailInfo);
				}
			}
			#endregion
		}
		public class JobStartTimeComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public JobStartTimeComparer()
			{ }
			public JobStartTimeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.JobStartTime.CompareTo(x.JobStartTime);
				}
				else
				{
					return x.JobStartTime.CompareTo(y.JobStartTime);
				}
			}
			#endregion
		}
		public class TenderTextComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public TenderTextComparer()
			{ }
			public TenderTextComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.TenderText.CompareTo(x.TenderText);
				}
				else
				{
					return x.TenderText.CompareTo(y.TenderText);
				}
			}
			#endregion
		}
		public class RemarksComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Remarks.CompareTo(x.Remarks);
				}
				else
				{
					return x.Remarks.CompareTo(y.Remarks);
				}
			}
			#endregion
		}
		public class CreatedDateComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public CreatedDateComparer()
			{ }
			public CreatedDateComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.CreatedDate.CompareTo(x.CreatedDate);
				}
				else
				{
					return x.CreatedDate.CompareTo(y.CreatedDate);
				}
			}
			#endregion
		}
		public class LastUpdatedDateComparer : System.Collections.Generic.IComparer<Job>
		{
			public SorterMode SorterMode;
			public LastUpdatedDateComparer()
			{ }
			public LastUpdatedDateComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Job> Membres
			int System.Collections.Generic.IComparer<Job>.Compare(Job x, Job y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.LastUpdatedDate.CompareTo(x.LastUpdatedDate);
				}
				else
				{
					return x.LastUpdatedDate.CompareTo(y.LastUpdatedDate);
				}
			}
			#endregion
		}
	}
}
