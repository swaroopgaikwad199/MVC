using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class JobDetails
	{
		private Decimal _JD_JobID;

		public Decimal JD_JobID
		{
			get { return _JD_JobID; }
			set { _JD_JobID = value; }
		}

		private string _JD_ProdName;

		public string JD_ProdName
		{
			get { return _JD_ProdName; }
			set { _JD_ProdName = value; }
		}

        private string _JD_ProdCode;
        public string JD_ProdCode
		{
            get { return _JD_ProdCode; }
            set { _JD_ProdCode = value; }
		}

        private string _JD_FGCode;
        public string JD_FGCode
        {
            get { return _JD_FGCode; }
            set { _JD_FGCode = value; }
        }
        
		private string _JD_Deckcode;

		public string JD_Deckcode
		{
			get { return _JD_Deckcode; }
			set { _JD_Deckcode = value; }
		}

        private string _JD_PPN;

        public string JD_PPN
        {
            get { return _JD_PPN; }
            set { _JD_PPN = value; }
        }

        private string _JD_GTIN;

		public string JD_GTIN
		{
			get { return _JD_GTIN; }
			set { _JD_GTIN = value; }
		}

        private string _JD_NTIN;

        public string JD_NTIN
        {
            get { return _JD_NTIN; }
            set { _JD_NTIN = value; }
        }

        private string _JD_GTINCTI;

        public string JD_GTINCTI
        {
            get { return _JD_GTINCTI; }
            set { _JD_GTINCTI = value; }
        }

		private int _JD_DeckSize;

		public int JD_DeckSize
		{
			get { return _JD_DeckSize; }
			set { _JD_DeckSize = value; }
		}

        private int _BundleQty;
        public int BundleQty
        {
            get { return _BundleQty; }
            set { _BundleQty = value; }
        }

		private Nullable<Decimal> _JD_MRP;

		public Nullable<Decimal> JD_MRP
		{
			get { return _JD_MRP; }
			set { _JD_MRP = value; }
		}

		private string _JD_Description;

		public string JD_Description
		{
			get { return _JD_Description; }
			set { _JD_Description = value; }
		}

        private DateTime _LastUpdatedDate;

        public DateTime LastUpdatedDate
		{
            get { return _LastUpdatedDate; }
            set { _LastUpdatedDate = value; }
		}

        public string _LabelName;

        public string LabelName
        {
            get { return _LabelName; }
            set { _LabelName = value; }
        }

        public string _Filter;

        public string Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }


        public JobDetails()
		{ }

        public JobDetails(Decimal JD_JobID, string JD_ProdName, string JD_ProdCode, string JD_FGCode,string JD_Deckcode, string JD_GTIN, int JD_DeckSize, Nullable<Decimal> JD_MRP, string JD_Description, DateTime LastUpdatedDate,string LabelName,string Filter,string JD_NTIN)
		{
			this.JD_JobID = JD_JobID;
            this.JD_ProdName = JD_ProdName;
            this.JD_ProdCode = JD_ProdCode;
            this.JD_FGCode=JD_FGCode;
			this.JD_Deckcode = JD_Deckcode;
			this.JD_GTIN = JD_GTIN;
			this.JD_DeckSize = JD_DeckSize;
			this.JD_MRP = JD_MRP;
			this.JD_Description = JD_Description;
            this.LastUpdatedDate = LastUpdatedDate;
            this.JD_GTINCTI = JD_GTINCTI;
            this.BundleQty = BundleQty;
            this.LabelName = LabelName;
            this.Filter = Filter;
            this.JD_NTIN = JD_NTIN;
		}

		public override string ToString()
		{
			return "JD_JobID = " + JD_JobID.ToString() + ",JD_ProdName = " + JD_ProdName + ",JD_Deckcode = " + JD_Deckcode + ",JD_GTIN = " + JD_GTIN + ",JD_DeckSize = " + JD_DeckSize.ToString() + ",JD_MRP = " + JD_MRP.ToString() + ",JD_Description = " + JD_Description+",LabelName="+LabelName+ ",Filter="+Filter+",JD_NTIN="+JD_NTIN;

        }

		public class JD_JobIDComparer : System.Collections.Generic.IComparer<JobDetails>
		{
			public SorterMode SorterMode;
			public JD_JobIDComparer()
			{ }
			public JD_JobIDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<JobDetails> Membres
			int System.Collections.Generic.IComparer<JobDetails>.Compare(JobDetails x, JobDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.JD_JobID.CompareTo(x.JD_JobID);
				}
				else
				{
					return x.JD_JobID.CompareTo(y.JD_JobID);
				}
			}
			#endregion
		}
		public class JD_ProdNameComparer : System.Collections.Generic.IComparer<JobDetails>
		{
			public SorterMode SorterMode;
			public JD_ProdNameComparer()
			{ }
			public JD_ProdNameComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<JobDetails> Membres
			int System.Collections.Generic.IComparer<JobDetails>.Compare(JobDetails x, JobDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.JD_ProdName.CompareTo(x.JD_ProdName);
				}
				else
				{
					return x.JD_ProdName.CompareTo(y.JD_ProdName);
				}
			}
			#endregion
		}
		public class JD_DeckcodeComparer : System.Collections.Generic.IComparer<JobDetails>
		{
			public SorterMode SorterMode;
			public JD_DeckcodeComparer()
			{ }
			public JD_DeckcodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<JobDetails> Membres
			int System.Collections.Generic.IComparer<JobDetails>.Compare(JobDetails x, JobDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.JD_Deckcode.CompareTo(x.JD_Deckcode);
				}
				else
				{
					return x.JD_Deckcode.CompareTo(y.JD_Deckcode);
				}
			}
			#endregion
		}
		public class JD_GTINComparer : System.Collections.Generic.IComparer<JobDetails>
		{
			public SorterMode SorterMode;
			public JD_GTINComparer()
			{ }
			public JD_GTINComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<JobDetails> Membres
			int System.Collections.Generic.IComparer<JobDetails>.Compare(JobDetails x, JobDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.JD_GTIN.CompareTo(x.JD_GTIN);
				}
				else
				{
					return x.JD_GTIN.CompareTo(y.JD_GTIN);
				}
			}
			#endregion
		}
		public class JD_DeckSizeComparer : System.Collections.Generic.IComparer<JobDetails>
		{
			public SorterMode SorterMode;
			public JD_DeckSizeComparer()
			{ }
			public JD_DeckSizeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<JobDetails> Membres
			int System.Collections.Generic.IComparer<JobDetails>.Compare(JobDetails x, JobDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.JD_DeckSize.CompareTo(x.JD_DeckSize);
				}
				else
				{
					return x.JD_DeckSize.CompareTo(y.JD_DeckSize);
				}
			}
			#endregion
		}
		public class JD_DescriptionComparer : System.Collections.Generic.IComparer<JobDetails>
		{
			public SorterMode SorterMode;
			public JD_DescriptionComparer()
			{ }
			public JD_DescriptionComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<JobDetails> Membres
			int System.Collections.Generic.IComparer<JobDetails>.Compare(JobDetails x, JobDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.JD_Description.CompareTo(x.JD_Description);
				}
				else
				{
					return x.JD_Description.CompareTo(y.JD_Description);
				}
			}
			#endregion
		}
	}
}
