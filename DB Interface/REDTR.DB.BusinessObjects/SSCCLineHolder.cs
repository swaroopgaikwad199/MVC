using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class SSCCLineHolder
	{
		private int _ID;

		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		private Nullable<Decimal> _PackageIndicator;

		public Nullable<Decimal> PackageIndicator
		{
			get { return _PackageIndicator; }
			set { _PackageIndicator = value; }
		}

		private Nullable<Decimal> _LastSSCC;

		public Nullable<Decimal> LastSSCC
		{
			get { return _LastSSCC; }
			set { _LastSSCC = value; }
		}
        
        private Nullable<Decimal> _FirstSSCC;

        public Nullable<Decimal> FirstSSCC
        {
            get { return _FirstSSCC; }
            set { _FirstSSCC = value; }
        }

        private Nullable<Decimal> _JobID;

        public Nullable<Decimal> JobID
        {
            get { return _JobID; }
            set { _JobID = value; }
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

		public SSCCLineHolder()
		{ }

		public SSCCLineHolder(int ID,Nullable<Decimal> PackageIndicator,Nullable<Decimal> LastSSCC,string Remarks)
		{
			this.ID = ID;
			this.PackageIndicator = PackageIndicator;
			this.LastSSCC = LastSSCC;
			this.Remarks = Remarks;
		}

		public override string ToString()
		{
			return "ID = " + ID.ToString() + ",PackageIndicator = " + PackageIndicator.ToString() + ",LastSSCC = " + LastSSCC.ToString() + ",Remarks = " + Remarks;
		}

		public class IDComparer : System.Collections.Generic.IComparer<SSCCLineHolder>
		{
			public SorterMode SorterMode;
			public IDComparer()
			{ }
			public IDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<SSCCLineHolder> Membres
			int System.Collections.Generic.IComparer<SSCCLineHolder>.Compare(SSCCLineHolder x, SSCCLineHolder y)
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
		public class RemarksComparer : System.Collections.Generic.IComparer<SSCCLineHolder>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<SSCCLineHolder> Membres
			int System.Collections.Generic.IComparer<SSCCLineHolder>.Compare(SSCCLineHolder x, SSCCLineHolder y)
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
