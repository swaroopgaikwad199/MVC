using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class USerTrail
	{
		private Decimal _ID;

		public Decimal ID
		{
			get { return _ID; }
			set { _ID = value; }
		}
		
		private Nullable<DateTime> _AccessedAt;

		public Nullable<DateTime> AccessedAt
		{
			get { return _AccessedAt; }
			set { _AccessedAt = value; }
		}

		private string _Reason;

		public string Reason
		{
			get { return _Reason; }
			set { _Reason = value; }
		}

		private string _Remarks;

		public string Remarks
		{
			get { return _Remarks; }
			set { _Remarks = value; }
		}

        private string _LineCode;

        public string LineCode
        {
            get { return _LineCode; }
            set { _LineCode = value; }
        }

		public USerTrail()
		{ }

		public USerTrail(Decimal ID,Nullable<DateTime> AccessedAt,string Reason,string Remarks)
		{
			this.ID = ID;			
			this.AccessedAt = AccessedAt;
			this.Reason = Reason;
			this.Remarks = Remarks;
		}

		public override string ToString()
		{
			return "ID = " + ID.ToString() + ",AccessedAt = " + AccessedAt.ToString() + ",Reason = " + Reason + ",Remarks = " + Remarks;
		}

		public class IDComparer : System.Collections.Generic.IComparer<USerTrail>
		{
			public SorterMode SorterMode;
			public IDComparer()
			{ }
			public IDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<USerTrail> Membres
			int System.Collections.Generic.IComparer<USerTrail>.Compare(USerTrail x, USerTrail y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.ID.CompareTo(x.ID);
				}
				else
				{
					return x.ID.CompareTo(y.ID);
				}
			}
			#endregion
		}	
		public class ReasonComparer : System.Collections.Generic.IComparer<USerTrail>
		{
			public SorterMode SorterMode;
			public ReasonComparer()
			{ }
			public ReasonComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<USerTrail> Membres
			int System.Collections.Generic.IComparer<USerTrail>.Compare(USerTrail x, USerTrail y)
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
		public class RemarksComparer : System.Collections.Generic.IComparer<USerTrail>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<USerTrail> Membres
			int System.Collections.Generic.IComparer<USerTrail>.Compare(USerTrail x, USerTrail y)
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
	}
}
