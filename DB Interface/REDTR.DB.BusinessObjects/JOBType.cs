using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class JOBType
	{
		private Decimal _TID;

		public Decimal TID
		{
			get { return _TID; }
			set { _TID = value; }
		} 
		private string _Job_Type;

		public string Job_Type
		{
			get { return _Job_Type; }
			set { _Job_Type = value; }
		}

        private string _Action;

        public string Action
		{
            get { return _Action; }
            set { _Action = value; }
		}
        
		private DateTime _LastUpdatedDate;

		public DateTime LastUpdatedDate
		{
			get { return _LastUpdatedDate; }
			set { _LastUpdatedDate = value; }
		}

		public JOBType()
		{ }

        public JOBType(Decimal TID, string Job_Type, string Action, DateTime LastUpdatedDate)
		{
			this.TID = TID;
			this.Job_Type = Job_Type;
            this.Action = Action;
			this.LastUpdatedDate = LastUpdatedDate;
		}

		public override string ToString()
		{
            return "TID = " + TID.ToString() + ",Job_Type = " + Job_Type + ",Action=" + Action + ",LastUpdatedDate = " + LastUpdatedDate.ToString();
		}

		public class TIDComparer : System.Collections.Generic.IComparer<JOBType>
		{
			public SorterMode SorterMode;
			public TIDComparer()
			{ }
			public TIDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<JOBType> Membres
			int System.Collections.Generic.IComparer<JOBType>.Compare(JOBType x, JOBType y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.TID.CompareTo(x.TID);
				}
				else
				{
					return x.TID.CompareTo(y.TID);
				}
			}
			#endregion
		}
		public class Job_TypeComparer : System.Collections.Generic.IComparer<JOBType>
		{
			public SorterMode SorterMode;
			public Job_TypeComparer()
			{ }
			public Job_TypeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<JOBType> Membres
			int System.Collections.Generic.IComparer<JOBType>.Compare(JOBType x, JOBType y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Job_Type.CompareTo(x.Job_Type);
				}
				else
				{
					return x.Job_Type.CompareTo(y.Job_Type);
				}
			}
			#endregion
		}
		public class LastUpdatedDateComparer : System.Collections.Generic.IComparer<JOBType>
		{
			public SorterMode SorterMode;
			public LastUpdatedDateComparer()
			{ }
			public LastUpdatedDateComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<JOBType> Membres
			int System.Collections.Generic.IComparer<JOBType>.Compare(JOBType x, JOBType y)
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
