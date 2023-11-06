using System;
using System.Text;
using System.Runtime.Serialization;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	//[DataContract()]
	public class DavaFileRunningSeqNo
	{
		private int _ID;

		//[DataMember]
		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		private Nullable<Decimal> _LastRunningSeqNo;

		//[DataMember]
		public Nullable<Decimal> LastRunningSeqNo
		{
			get { return _LastRunningSeqNo; }
			set { _LastRunningSeqNo = value; }
		}

		private string _Remarks;

		//[DataMember]
		public string Remarks
		{
			get { return _Remarks; }
			set { _Remarks = value; }
		}

		public DavaFileRunningSeqNo()
		{ }

		public DavaFileRunningSeqNo(int ID,Nullable<Decimal> LastRunningSeqNo,string Remarks)
		{
			this.ID = ID;
			this.LastRunningSeqNo = LastRunningSeqNo;
			this.Remarks = Remarks;
		}

		public override string ToString()
		{
			return "ID = " + ID.ToString() + ",LastRunningSeqNo = " + LastRunningSeqNo.ToString() + ",Remarks = " + Remarks;
		}

		public class IDComparer : System.Collections.Generic.IComparer<DavaFileRunningSeqNo>
		{
			public SorterMode SorterMode;
			public IDComparer()
			{ }
			public IDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<DavaFileRunningSeqNo> Membres
			int System.Collections.Generic.IComparer<DavaFileRunningSeqNo>.Compare(DavaFileRunningSeqNo x, DavaFileRunningSeqNo y)
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
		public class RemarksComparer : System.Collections.Generic.IComparer<DavaFileRunningSeqNo>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<DavaFileRunningSeqNo> Membres
			int System.Collections.Generic.IComparer<DavaFileRunningSeqNo>.Compare(DavaFileRunningSeqNo x, DavaFileRunningSeqNo y)
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
