using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class ROLESPermission
	{
		private Decimal _ID;

		public Decimal ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		private Nullable<int> _Roles_Id;

		public Nullable<int> Roles_Id
		{
			get { return _Roles_Id; }
			set { _Roles_Id = value; }
		}

		private Nullable<Decimal> _Permission_Id;

		public Nullable<Decimal> Permission_Id
		{
			get { return _Permission_Id; }
			set { _Permission_Id = value; }
		}

		private string _Remarks;

		public string Remarks
		{
			get { return _Remarks; }
			set { _Remarks = value; }
		}

		public ROLESPermission()
		{ }

		public ROLESPermission(Decimal ID,Nullable<int> Roles_Id,Nullable<Decimal> Permission_Id,string Remarks)
		{
			this.ID = ID;
			this.Roles_Id = Roles_Id;
			this.Permission_Id = Permission_Id;
			this.Remarks = Remarks;
		}

		public override string ToString()
		{
			return "ID = " + ID.ToString() + ",Roles_Id = " + Roles_Id.ToString() + ",Permission_Id = " + Permission_Id.ToString() + ",Remarks = " + Remarks;
		}

		public class IDComparer : System.Collections.Generic.IComparer<ROLESPermission>
		{
			public SorterMode SorterMode;
			public IDComparer()
			{ }
			public IDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<ROLESPermission> Membres
			int System.Collections.Generic.IComparer<ROLESPermission>.Compare(ROLESPermission x, ROLESPermission y)
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
		public class RemarksComparer : System.Collections.Generic.IComparer<ROLESPermission>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<ROLESPermission> Membres
			int System.Collections.Generic.IComparer<ROLESPermission>.Compare(ROLESPermission x, ROLESPermission y)
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
