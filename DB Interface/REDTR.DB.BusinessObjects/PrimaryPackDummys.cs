using System;
using System.Text;
using System.Runtime.Serialization;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	//[DataContract()]
	public class PrimaryPackDummys
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

		private string _NextLevelCode;

		//[DataMember]
		public string NextLevelCode
		{
			get { return _NextLevelCode; }
			set { _NextLevelCode = value; }
		}

		private string _Code;

	//	[DataMember]
		public string Code
		{
			get { return _Code; }
			set { _Code = value; }
		}

		private Nullable<DateTime> _CreatedDate;

		//[DataMember]
		public Nullable<DateTime> CreatedDate
		{
			get { return _CreatedDate; }
			set { _CreatedDate = value; }
		}

		public PrimaryPackDummys()
		{ }

		public PrimaryPackDummys(int Id,int JobId,string NextLevelCode,string Code,Nullable<DateTime> CreatedDate)
		{
			this.Id = Id;
			this.JobId = JobId;
			this.NextLevelCode = NextLevelCode;
			this.Code = Code;
			this.CreatedDate = CreatedDate;
		}

		public override string ToString()
		{
			return "Id = " + Id.ToString() + ",JobId = " + JobId.ToString() + ",NextLevelCode = " + NextLevelCode + ",Code = " + Code + ",CreatedDate = " + CreatedDate.ToString();
		}

		public class IdComparer : System.Collections.Generic.IComparer<PrimaryPackDummys>
		{
			public SorterMode SorterMode;
			public IdComparer()
			{ }
			public IdComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PrimaryPackDummys> Membres
			int System.Collections.Generic.IComparer<PrimaryPackDummys>.Compare(PrimaryPackDummys x, PrimaryPackDummys y)
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
		public class JobIdComparer : System.Collections.Generic.IComparer<PrimaryPackDummys>
		{
			public SorterMode SorterMode;
			public JobIdComparer()
			{ }
			public JobIdComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PrimaryPackDummys> Membres
			int System.Collections.Generic.IComparer<PrimaryPackDummys>.Compare(PrimaryPackDummys x, PrimaryPackDummys y)
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
		public class NextLevelCodeComparer : System.Collections.Generic.IComparer<PrimaryPackDummys>
		{
			public SorterMode SorterMode;
			public NextLevelCodeComparer()
			{ }
			public NextLevelCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PrimaryPackDummys> Membres
			int System.Collections.Generic.IComparer<PrimaryPackDummys>.Compare(PrimaryPackDummys x, PrimaryPackDummys y)
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
		public class CodeComparer : System.Collections.Generic.IComparer<PrimaryPackDummys>
		{
			public SorterMode SorterMode;
			public CodeComparer()
			{ }
			public CodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PrimaryPackDummys> Membres
			int System.Collections.Generic.IComparer<PrimaryPackDummys>.Compare(PrimaryPackDummys x, PrimaryPackDummys y)
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
	}
}
