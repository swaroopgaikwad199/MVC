using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public partial class Roles
	{
		private int _ID;

		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		private string _Roles_Name;

		public string Roles_Name
		{
			get { return _Roles_Name; }
			set { _Roles_Name = value; }
		}

		private string _Remarks;

		public string Remarks
		{
			get { return _Remarks; }
			set { _Remarks = value; }
		}

		public Roles()
		{ }

		public Roles(int ID,string Roles_Name,string Remarks)
		{
			this.ID = ID;
			this.Roles_Name = Roles_Name;
			this.Remarks = Remarks;
		}

		public override string ToString()
		{
			return "ID = " + ID.ToString() + ",Roles_Name = " + Roles_Name + ",Remarks = " + Remarks;
		}

		public class IDComparer : System.Collections.Generic.IComparer<Roles>
		{
			public SorterMode SorterMode;
			public IDComparer()
			{ }
			public IDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Roles> Membres
			int System.Collections.Generic.IComparer<Roles>.Compare(Roles x, Roles y)
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
		public class Roles_NameComparer : System.Collections.Generic.IComparer<Roles>
		{
			public SorterMode SorterMode;
			public Roles_NameComparer()
			{ }
			public Roles_NameComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Roles> Membres
			int System.Collections.Generic.IComparer<Roles>.Compare(Roles x, Roles y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Roles_Name.CompareTo(x.Roles_Name);
				}
				else
				{
					return x.Roles_Name.CompareTo(y.Roles_Name);
				}
			}
			#endregion
		}
		public class RemarksComparer : System.Collections.Generic.IComparer<Roles>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Roles> Membres
			int System.Collections.Generic.IComparer<Roles>.Compare(Roles x, Roles y)
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
