using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class Users
	{
		private Decimal _ID;

		public Decimal ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		private string _UserName;

		public string UserName
		{
			get { return _UserName; }
			set { _UserName = value; }
		}

        private string _UserName1;

        public string UserName1
        {
            get { return _UserName1; }
            set { _UserName1 = value; }
        }

		private string _Password;

		public string Password
		{
			get { return _Password; }
			set { _Password = value; }
		}

		private Nullable<int> _RoleID;

		public Nullable<int> RoleID
        {
			get { return _RoleID; }
			set { _RoleID = value; }
        }

		private Nullable<bool> _Active;

		public Nullable<bool> Active
        {
			get { return _Active; }
			set { _Active = value; }
        }

        private Nullable<bool> _IsFirstLogin;

        public Nullable<bool> IsFirstLogin
        {
            get { return _IsFirstLogin; }
            set { _IsFirstLogin = value; }
        }


        private string _Remarks;

		public string Remarks
		{
			get { return _Remarks; }
			set { _Remarks = value; }
		}

		private Nullable<DateTime> _CreatedDate;

		public Nullable<DateTime> CreatedDate
        { 
			get { return _CreatedDate; }
			set { _CreatedDate = value; }
        }

		private Nullable<DateTime> _LastUpdatedDate;

		public Nullable<DateTime> LastUpdatedDate
        {
			get { return _LastUpdatedDate; }
			set { _LastUpdatedDate = value; }
        }

        private Nullable<int> _UserType;

        public Nullable<int> UserType
        {
            get { return _UserType; }
            set { _UserType = value; }
        }

        private string _EmailId;

        public string EmailId
        {
            get { return _EmailId; }
            set { _EmailId = value; }
        }

        public Users()
		{ }

		public Users(Decimal ID,string UserName,string Password,Nullable<int> RoleID, Nullable<int> UserType ,Nullable<bool> Active, Nullable<bool> IsFirstLoginAtmt, string Remarks,Nullable<DateTime> CreatedDate,Nullable<DateTime> LastUpdatedDate,string EmailId)
		{
			this.ID = ID;
			this.UserName = UserName;
			this.Password = Password;
			this.RoleID = RoleID;
			this.Active = Active;
			this.Remarks = Remarks;
			this.CreatedDate = CreatedDate;
            this.LastUpdatedDate = LastUpdatedDate;
            this.IsFirstLogin = IsFirstLoginAtmt;
            this.UserType = UserType;
            this.EmailId = EmailId;
		}      
    
		public override string ToString()
		{
			return "ID = " + ID.ToString() + ",UserName = " + UserName + ",Password = " + Password + ",RoleID = " + RoleID.ToString() + ",Active = " + Active.ToString() + ",Remarks = " + Remarks + ",CreatedDate = " + CreatedDate.ToString() + ",LastUpdatedDate = " + LastUpdatedDate.ToString();
		}

		public class IDComparer : System.Collections.Generic.IComparer<Users>
		{
			public SorterMode SorterMode;
			public IDComparer()
			{ }
			public IDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Users> Membres
			int System.Collections.Generic.IComparer<Users>.Compare(Users x, Users y)
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
		public class UserNameComparer : System.Collections.Generic.IComparer<Users>
		{
			public SorterMode SorterMode;
			public UserNameComparer()
			{ }
			public UserNameComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Users> Membres
			int System.Collections.Generic.IComparer<Users>.Compare(Users x, Users y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.UserName.CompareTo(x.UserName);
				}
				else
				{
					return x.UserName.CompareTo(y.UserName);
				}
			}
			#endregion
		}
		public class PasswordComparer : System.Collections.Generic.IComparer<Users>
		{
			public SorterMode SorterMode;
			public PasswordComparer()
			{ }
			public PasswordComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Users> Membres
			int System.Collections.Generic.IComparer<Users>.Compare(Users x, Users y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Password.CompareTo(x.Password);
				}
				else
				{
					return x.Password.CompareTo(y.Password);
				}
			}
			#endregion
		}
		public class RemarksComparer : System.Collections.Generic.IComparer<Users>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Users> Membres
			int System.Collections.Generic.IComparer<Users>.Compare(Users x, Users y)
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
