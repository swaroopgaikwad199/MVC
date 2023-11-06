using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class JobAssoDeck
	{
		private Decimal _ID;

		public Decimal ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		private Decimal _JobID;

		public Decimal JobID
		{
			get { return _JobID; }
			set { _JobID = value; }
		}

		private string _DeckCode;

		public string DeckCode
		{
			get { return _DeckCode; }
			set { _DeckCode = value; }
		}

        private DateTime _LastUpdatedDate;
        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { _LastUpdatedDate = value; }
        }

        //private Decimal _RecipeID;

        //public Decimal RecipeID
        //{
        //    get { return _RecipeID; }
        //    set { _RecipeID = value; }
        //}

		public JobAssoDeck()
		{ }

        public JobAssoDeck(Decimal ID, Decimal JobID, string DeckCode, DateTime LastUpdatedDate)//,Decimal RecipeID
		{
			this.ID = ID;
			this.JobID = JobID;
			this.DeckCode = DeckCode;
            this.LastUpdatedDate = LastUpdatedDate;
			//this.RecipeID = RecipeID;
		}

		public override string ToString()
		{
            return "ID = " + ID.ToString() + ",JobID = " + JobID.ToString() + ",DeckCode = " + DeckCode + ",LastUpdatedDate=" + LastUpdatedDate;// +",RecipeID = " + RecipeID.ToString();
		}

		public class IDComparer : System.Collections.Generic.IComparer<JobAssoDeck>
		{
			public SorterMode SorterMode;
			public IDComparer()
			{ }
			public IDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<JobAssoDeck> Membres
			int System.Collections.Generic.IComparer<JobAssoDeck>.Compare(JobAssoDeck x, JobAssoDeck y)
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
		public class JobIDComparer : System.Collections.Generic.IComparer<JobAssoDeck>
		{
			public SorterMode SorterMode;
			public JobIDComparer()
			{ }
			public JobIDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<JobAssoDeck> Membres
			int System.Collections.Generic.IComparer<JobAssoDeck>.Compare(JobAssoDeck x, JobAssoDeck y)
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
		public class DeckCodeComparer : System.Collections.Generic.IComparer<JobAssoDeck>
		{
			public SorterMode SorterMode;
			public DeckCodeComparer()
			{ }
			public DeckCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<JobAssoDeck> Membres
			int System.Collections.Generic.IComparer<JobAssoDeck>.Compare(JobAssoDeck x, JobAssoDeck y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.DeckCode.CompareTo(x.DeckCode);
				}
				else
				{
					return x.DeckCode.CompareTo(y.DeckCode);
				}
			}
			#endregion
		}
	
	}
}
