using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class Settings
	{
		private int _id;

		public int id
		{
			get { return _id; }
			set { _id = value; }
		}

		private string _CompanyName;

		public string CompanyName
		{
			get { return _CompanyName; }
			set { _CompanyName = value; }
		}

		private string _Address;

		public string Address
		{
			get { return _Address; }
			set { _Address = value; }
		}

		private string _Logo;

		public string Logo
		{
			get { return _Logo; }
			set { _Logo = value; }
		}

		private string _CompanyCode;

		public string CompanyCode
		{
			get { return _CompanyCode; }
			set { _CompanyCode = value; }
		}

		private string _LineCode;

		public string LineCode
		{
			get { return _LineCode; }
			set { _LineCode = value; }
		}

        private int _PlantCode;

        public int PlantCode
        {
            get { return _PlantCode; }
            set { _PlantCode = value; }
        }

		private string _Remarks;

		public string Remarks
		{
			get { return _Remarks; }
			set { _Remarks = value; }
		}

		public Settings()
		{ }

		public Settings(int id,string CompanyName,string Address,string Logo,string CompanyCode,string LineCode,string Remarks)
		{
			this.id = id;
			this.CompanyName = CompanyName;
			this.Address = Address;
			this.Logo = Logo;
			this.CompanyCode = CompanyCode;
			this.LineCode = LineCode;
			this.Remarks = Remarks;
		}

		public override string ToString()
		{
			return "id = " + id.ToString() + ",CompanyName = " + CompanyName + ",Address = " + Address + ",Logo = " + Logo + ",CompanyCode = " + CompanyCode + ",LineCode = " + LineCode + ",Remarks = " + Remarks;
		}

		public class idComparer : System.Collections.Generic.IComparer<Settings>
		{
			public SorterMode SorterMode;
			public idComparer()
			{ }
			public idComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Settings> Membres
			int System.Collections.Generic.IComparer<Settings>.Compare(Settings x, Settings y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.id.CompareTo(x.id);
				}
				else
				{
					return x.id.CompareTo(y.id);
				}
			}
			#endregion
		}
		public class CompanyNameComparer : System.Collections.Generic.IComparer<Settings>
		{
			public SorterMode SorterMode;
			public CompanyNameComparer()
			{ }
			public CompanyNameComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Settings> Membres
			int System.Collections.Generic.IComparer<Settings>.Compare(Settings x, Settings y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.CompanyName.CompareTo(x.CompanyName);
				}
				else
				{
					return x.CompanyName.CompareTo(y.CompanyName);
				}
			}
			#endregion
		}
		public class AddressComparer : System.Collections.Generic.IComparer<Settings>
		{
			public SorterMode SorterMode;
			public AddressComparer()
			{ }
			public AddressComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Settings> Membres
			int System.Collections.Generic.IComparer<Settings>.Compare(Settings x, Settings y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Address.CompareTo(x.Address);
				}
				else
				{
					return x.Address.CompareTo(y.Address);
				}
			}
			#endregion
		}
		public class LogoComparer : System.Collections.Generic.IComparer<Settings>
		{
			public SorterMode SorterMode;
			public LogoComparer()
			{ }
			public LogoComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Settings> Membres
			int System.Collections.Generic.IComparer<Settings>.Compare(Settings x, Settings y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Logo.CompareTo(x.Logo);
				}
				else
				{
					return x.Logo.CompareTo(y.Logo);
				}
			}
			#endregion
		}
		public class CompanyCodeComparer : System.Collections.Generic.IComparer<Settings>
		{
			public SorterMode SorterMode;
			public CompanyCodeComparer()
			{ }
			public CompanyCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Settings> Membres
			int System.Collections.Generic.IComparer<Settings>.Compare(Settings x, Settings y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.CompanyCode.CompareTo(x.CompanyCode);
				}
				else
				{
					return x.CompanyCode.CompareTo(y.CompanyCode);
				}
			}
			#endregion
		}
		public class LineCodeComparer : System.Collections.Generic.IComparer<Settings>
		{
			public SorterMode SorterMode;
			public LineCodeComparer()
			{ }
			public LineCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Settings> Membres
			int System.Collections.Generic.IComparer<Settings>.Compare(Settings x, Settings y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.LineCode.CompareTo(x.LineCode);
				}
				else
				{
					return x.LineCode.CompareTo(y.LineCode);
				}
			}
			#endregion
		}
		public class RemarksComparer : System.Collections.Generic.IComparer<Settings>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Settings> Membres
			int System.Collections.Generic.IComparer<Settings>.Compare(Settings x, Settings y)
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
