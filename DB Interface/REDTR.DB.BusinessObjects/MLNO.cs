using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class MLNO
	{
		private Decimal _ID;

		public Decimal ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

        private string _ML_NO;

        public string ML_NO
		{
            get { return _ML_NO; }
            set { _ML_NO = value; }
		}

		private string _Description;

		public string Description
		{
			get { return _Description; }
			set { _Description = value; }
		}

		private DateTime _LastUpdatedDate;

		public DateTime LastUpdatedDate
		{
			get { return _LastUpdatedDate; }
			set { _LastUpdatedDate = value; }
		}

		public MLNO()
		{ }

        public MLNO(Decimal ID, string ML_NO, string Description, DateTime LastUpdatedDate)
		{
			this.ID = ID;
            this.ML_NO = ML_NO;
			this.Description = Description;
			this.LastUpdatedDate = LastUpdatedDate;
		}

		public override string ToString()
		{
            return "ID = " + ID.ToString() + ",ML_NO = " + ML_NO + ",Description = " + Description + ",LastUpdatedDate = " + LastUpdatedDate.ToString();
		}

        public class IDComparer : System.Collections.Generic.IComparer<MLNO>
		{
			public SorterMode SorterMode;
			public IDComparer()
			{ }
			public IDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
            }
			#region IComparer<MLNO> Membres
            int System.Collections.Generic.IComparer<MLNO>.Compare(MLNO x, MLNO y)
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
		public class ML_NOComparer : System.Collections.Generic.IComparer<MLNO>
		{
			public SorterMode SorterMode;
			public ML_NOComparer()
			{ }
			public ML_NOComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<MLNO> Membres
			int System.Collections.Generic.IComparer<MLNO>.Compare(MLNO x, MLNO y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
                    return y.ML_NO.CompareTo(x.ML_NO);
				}
				else
				{
                    return x.ML_NO.CompareTo(y.ML_NO);
				}
			}
			#endregion
		}
		public class DescriptionComparer : System.Collections.Generic.IComparer<MLNO>
		{
			public SorterMode SorterMode;
			public DescriptionComparer()
			{ }
			public DescriptionComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<MLNO> Membres
			int System.Collections.Generic.IComparer<MLNO>.Compare(MLNO x, MLNO y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Description.CompareTo(x.Description);
				}
				else
				{
					return x.Description.CompareTo(y.Description);
				}
			}
			#endregion
		}
		public class LastUpdatedDateComparer : System.Collections.Generic.IComparer<MLNO>
		{
			public SorterMode SorterMode;
			public LastUpdatedDateComparer()
			{ }
			public LastUpdatedDateComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<MLNO> Membres
			int System.Collections.Generic.IComparer<MLNO>.Compare(MLNO x, MLNO y)
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
