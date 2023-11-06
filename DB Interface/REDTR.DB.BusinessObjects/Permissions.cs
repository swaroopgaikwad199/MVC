using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public partial class Permissions
	{
		private Decimal _ID;

		public Decimal ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

        public enum PermissionsList
        {
            CanAddProject = 1,
            CanAddUser = 2,
            CanByPassJobs = 3,
            CanDeleteJobs = 4,
            CanEditProject = 5,
            CanPrintReport = 6,
            CanViewReport = 7,
            CanAccessAdvSettings = 8,
            CanDeleteUser = 9,
            CanSetCamera = 10
        }
		private string _Permission;

		public string Permission
		{
			get { return _Permission; }
			set { _Permission = value; }
		}

		private string _Remarks;

		public string Remarks
		{
			get { return _Remarks; }
			set { _Remarks = value; }
		}

		public Permissions()
		{ }

		public Permissions(Decimal ID,string Permission,string Remarks)
		{
			this.ID = ID;
			this.Permission = Permission;
			this.Remarks = Remarks;
		}

		public override string ToString()
		{
			return "ID = " + ID.ToString() + ",Permission = " + Permission + ",Remarks = " + Remarks;
		}

		public class IDComparer : System.Collections.Generic.IComparer<Permissions>
		{
			public SorterMode SorterMode;
			public IDComparer()
			{ }
			public IDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Permissions> Membres
			int System.Collections.Generic.IComparer<Permissions>.Compare(Permissions x, Permissions y)
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
		public class PermissionComparer : System.Collections.Generic.IComparer<Permissions>
		{
			public SorterMode SorterMode;
			public PermissionComparer()
			{ }
			public PermissionComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Permissions> Membres
			int System.Collections.Generic.IComparer<Permissions>.Compare(Permissions x, Permissions y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Permission.CompareTo(x.Permission);
				}
				else
				{
					return x.Permission.CompareTo(y.Permission);
				}
			}
			#endregion
		}
		public class RemarksComparer : System.Collections.Generic.IComparer<Permissions>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Permissions> Membres
			int System.Collections.Generic.IComparer<Permissions>.Compare(Permissions x, Permissions y)
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
