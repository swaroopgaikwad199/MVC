using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class PackagingDetails
	{
		private Decimal _PackDtlsID;

		public Decimal PackDtlsID
		{
			get { return _PackDtlsID; }
			set { _PackDtlsID = value; }
		}

		private string _Code;

		public string Code
		{
			get { return _Code; }
			set { _Code = value; }
		}

		private Decimal _PAID;

		public Decimal PAID
		{
			get { return _PAID; }
			set { _PAID = value; }
		}

		private Decimal _JobID;

		public Decimal JobID
		{
			get { return _JobID; }
			set { _JobID = value; }
		}

		private string _PackageTypeCode;

		public string PackageTypeCode
		{
			get { return _PackageTypeCode; }
			set { _PackageTypeCode = value; }
		}

		private DateTime _MfgPackDate;

		public DateTime MfgPackDate
		{
			get { return _MfgPackDate; }
			set { _MfgPackDate = value; }
		}

        private DateTime _ExpPackDate;

        public DateTime ExpPackDate
		{
            get { return _ExpPackDate; }
            set { _ExpPackDate = value; }
		}

		private string _NextLevelCode;

		public string NextLevelCode
		{
			get { return _NextLevelCode; }
			set { _NextLevelCode = value; }
		}

		private Nullable<bool> _IsRejected;

	    public Nullable<bool> IsRejected
		{
			get { return _IsRejected; }
			set { _IsRejected = value; }
		}

		private string _Reason;

		public string Reason
		{
			get { return _Reason; }
			set { _Reason = value; }
		}

		private Byte[] _BadImage;

		public Byte[] BadImage
		{
			get { return _BadImage; }
			set { _BadImage = value; }
		}
        private bool _DAVAPortalUpload;

        public bool DAVAPortalUpload
        {
            get { return _DAVAPortalUpload; }
            set { _DAVAPortalUpload = value; }
        }
		private string _SSCC;

		public string SSCC
		{
			get { return _SSCC; }
			set { _SSCC = value; }
		}

        private int _RCResult;

        public int RCResult
        {
            get { return _RCResult; }
            set { _RCResult = value; }
        }

		private Nullable<bool> _SSCCVarificationStatus;

		public Nullable<bool> SSCCVarificationStatus
		{
			get { return _SSCCVarificationStatus; }
			set { _SSCCVarificationStatus = value; }
		}

		private Nullable<bool> _IsManualUpdated;

		public Nullable<bool> IsManualUpdated
		{
			get { return _IsManualUpdated; }
			set { _IsManualUpdated = value; }
		}

		private string _ManualUpdateDesc;

		public string ManualUpdateDesc
		{
			get { return _ManualUpdateDesc; }
			set { _ManualUpdateDesc = value; }
		}

        private Nullable<Decimal> _CaseSeqNum;

        public Nullable<Decimal> CaseSeqNum
		{
			get { return _CaseSeqNum; }
			set { _CaseSeqNum = value; }
		}

		private Nullable<Decimal> _OperatorId;

		public Nullable<Decimal> OperatorId
		{
			get { return _OperatorId; }
			set { _OperatorId = value; }
		}

		private string _Remarks;

		public string Remarks
		{
			get { return _Remarks; }
			set { _Remarks = value; }
		}

        private bool _IsDecomission;

        public bool IsDecomission
        {
            get { return _IsDecomission; }
            set { _IsDecomission = value; }
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

        public bool _IsUsed;
        public bool IsUsed
        {
            get { return _IsUsed; }
            set { _IsUsed = value; }
        }

        public bool _SYNC;
        public bool SYNC
        {
            get { return _SYNC; }
            set { _SYNC = value; }
        }

        private string _LineCode;

                public string LineCode
                {
                     get { return _LineCode; }
                     set { _LineCode = value; }
                 }

		public PackagingDetails()
		{ }

        public PackagingDetails(Decimal PackDtlsID, string Code, Decimal PAID, Decimal JobID, string PackageTypeCode, DateTime MfgPackDate, DateTime ExpPackDate, string NextLevelCode, bool IsRejected, string Reason, Byte[] BadImage, string SSCC, Nullable<bool> SSCCVarificationStatus, Nullable<bool> IsManualUpdated, string ManualUpdateDesc, Nullable<Decimal> CaseSeqNum, Nullable<Decimal> OperatorId, string Remarks, bool IsDecomission, DateTime CreatedDate, DateTime LastUpdatedDate,bool DAVAPortalUpload,string LineCode,bool SYNC,bool IsUsed)
		{
			this.PackDtlsID = PackDtlsID;
			this.Code = Code;
			this.PAID = PAID;
			this.JobID = JobID;
			this.PackageTypeCode = PackageTypeCode;
			this.MfgPackDate = MfgPackDate;
            this.ExpPackDate = ExpPackDate;
			this.NextLevelCode = NextLevelCode;
			this.IsRejected = IsRejected;
			this.Reason = Reason;
			this.BadImage = BadImage;
			this.SSCC = SSCC;
			this.SSCCVarificationStatus = SSCCVarificationStatus;
			this.IsManualUpdated = IsManualUpdated;
			this.ManualUpdateDesc = ManualUpdateDesc;
            this.CaseSeqNum = CaseSeqNum;
			this.OperatorId = OperatorId;
			this.Remarks = Remarks;
            this.IsDecomission = IsDecomission;
			this.CreatedDate = CreatedDate;
			this.LastUpdatedDate = LastUpdatedDate;
            this.DAVAPortalUpload = DAVAPortalUpload;
             this.LineCode = LineCode;
            this.SYNC = SYNC;
            this.IsUsed = IsUsed;

        }

		public override string ToString()
		{
            return "PackDtlsID = " + PackDtlsID.ToString() + ",Code = " + Code + ",PAID = " + PAID.ToString() + ",JobID = " + JobID.ToString() + ",PackageTypeCode = " + PackageTypeCode + ",MfgPackDate = " + MfgPackDate.ToString() + ",ExpPackDate = " + ExpPackDate.ToString() + ",NextLevelCode = " + NextLevelCode + ",IsRejected = " + IsRejected.ToString() + ",Reason = " + Reason + ",BadImage = " + BadImage.ToString() + ",SSCC = " + SSCC + ",SSCCVarificationStatus = " + SSCCVarificationStatus.ToString() + ",IsManualUpdated = " + IsManualUpdated.ToString() + ",ManualUpdateDesc = " + ManualUpdateDesc + ",CaseSeqNum = " + CaseSeqNum.ToString() + ",OperatorId = " + OperatorId.ToString() + ",Remarks = " + Remarks + ",IsDecomission=" + IsDecomission + ",CreatedDate = " + CreatedDate.ToString() + ",LastUpdatedDate = " + LastUpdatedDate.ToString() + ",DAVAPortalUpload=" + DAVAPortalUpload.ToString()+",LineCode="+ LineCode +"";
		}

		public class PackDtlsIDComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public PackDtlsIDComparer()
			{ }
			public PackDtlsIDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.PackDtlsID.CompareTo(x.PackDtlsID);
				}
				else
				{
					return x.PackDtlsID.CompareTo(y.PackDtlsID);
				}
			}
			#endregion
		}
		public class CodeComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public CodeComparer()
			{ }
			public CodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Code.CompareTo(x.Code);
				}
				else
				{
					return x.Code.CompareTo(y.Code);
				}
			}
			#endregion
		}
		public class PAIDComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public PAIDComparer()
			{ }
			public PAIDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
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
		public class JobIDComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public JobIDComparer()
			{ }
			public JobIDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.JobID.CompareTo(x.JobID);
				}
				else
				{
					return x.JobID.CompareTo(y.JobID);
				}
			}
			#endregion
		}
		public class PackageTypeCodeComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public PackageTypeCodeComparer()
			{ }
			public PackageTypeCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.PackageTypeCode.CompareTo(x.PackageTypeCode);
				}
				else
				{
					return x.PackageTypeCode.CompareTo(y.PackageTypeCode);
				}
			}
			#endregion
		}
		public class MfgPackDateComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public MfgPackDateComparer()
			{ }
			public MfgPackDateComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.MfgPackDate.CompareTo(x.MfgPackDate);
				}
				else
				{
					return x.MfgPackDate.CompareTo(y.MfgPackDate);
				}
			}
			#endregion
		}
		public class ExpPackDateComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public ExpPackDateComparer()
			{ }
            public ExpPackDateComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
                    return y.ExpPackDate.CompareTo(x.ExpPackDate);
				}
				else
				{
                    return x.ExpPackDate.CompareTo(y.ExpPackDate);
				}
			}
			#endregion
		}
		public class NextLevelCodeComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public NextLevelCodeComparer()
			{ }
			public NextLevelCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.NextLevelCode.CompareTo(x.NextLevelCode);
				}
				else
				{
					return x.NextLevelCode.CompareTo(y.NextLevelCode);
				}
			}
			#endregion
		}
        //public class IsRejectedComparer : System.Collections.Generic.IComparer<PackagingDetails>
        //{
        //    public SorterMode SorterMode;
        //    public IsRejectedComparer()
        //    { }
        //    public IsRejectedComparer(SorterMode SorterMode)
        //    {
        //        this.SorterMode = SorterMode;
        //    }
        //    #region IComparer<PackagingDetails> Membres
        //    //int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
        //    //{
        //    //    //if (SorterMode == SorterMode.Ascending)
        //    //    //{
        //    //    //    return y.IsRejected.CompareTo(x.IsRejected);
        //    //    //}
        //    //    //else
        //    //    //{
        //    //    //    return x.IsRejected.CompareTo(y.IsRejected);
        //    //    //}
        //    //}
        //    #endregion
        //}
		public class ReasonComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public ReasonComparer()
			{ }
			public ReasonComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Reason.CompareTo(x.Reason);
				}
				else
				{
					return x.Reason.CompareTo(y.Reason);
				}
			}
			#endregion
		}
		public class SSCCComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public SSCCComparer()
			{ }
			public SSCCComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.SSCC.CompareTo(x.SSCC);
				}
				else
				{
					return x.SSCC.CompareTo(y.SSCC);
				}
			}
			#endregion
		}
		public class ManualUpdateDescComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public ManualUpdateDescComparer()
			{ }
			public ManualUpdateDescComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.ManualUpdateDesc.CompareTo(x.ManualUpdateDesc);
				}
				else
				{
					return x.ManualUpdateDesc.CompareTo(y.ManualUpdateDesc);
				}
			}
			#endregion
		}
		public class RemarksComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
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
		public class CreatedDateComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public CreatedDateComparer()
			{ }
			public CreatedDateComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
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
		public class LastUpdatedDateComparer : System.Collections.Generic.IComparer<PackagingDetails>
		{
			public SorterMode SorterMode;
			public LastUpdatedDateComparer()
			{ }
			public LastUpdatedDateComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingDetails> Membres
			int System.Collections.Generic.IComparer<PackagingDetails>.Compare(PackagingDetails x, PackagingDetails y)
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
