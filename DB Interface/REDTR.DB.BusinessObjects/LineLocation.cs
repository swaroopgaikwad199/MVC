using System;
using System.Text;
using System.Runtime.Serialization;
using REDTR.DB.BusinessObjects;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	
	public class LineLocation
	{
		private string _ID;

		
		public string ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		private string _LocationCode;

		
		public string LocationCode
		{
			get { return _LocationCode; }
			set { _LocationCode = value; }
		}

		private string _DivisionCode;

		
		public string DivisionCode
		{
			get { return _DivisionCode; }
			set { _DivisionCode = value; }
		}

		private string _PlantCode;

		
		public string PlantCode
		{
			get { return _PlantCode; }
			set { _PlantCode = value; }
		}

		private string _LineCode;

		
		public string LineCode
		{
			get { return _LineCode; }
			set { _LineCode = value; }
		}

        private string _LineIP;


        public string LineIP
        {
            get { return _LineIP; }
            set { _LineIP= value; }
        }

        private string _DBName;


        public string DBName
        {
            get { return _DBName; }
            set { _DBName = value; }
        }
        private bool _IsActive;


        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }
        private string _ServerName;


        public string ServerName
        {
            get { return _ServerName; }
            set { _ServerName = value; }
        }
        private string _SQLPassword;


        public string SQLPassword
        {
            get { return _SQLPassword; }
            set { _SQLPassword = value; }
        }
        private string _LineName;


        public string LineName
        {
            get { return _LineName; }
            set { _LineName = value; }
        }

        private string _ReadGLN;


        public string ReadGLN
        {
            get { return _ReadGLN; }
            set { _ReadGLN = value; }
        }
        private string _GLNExtension;


        public string GLNExtension
        {
            get { return _GLNExtension; }
            set { _GLNExtension = value; }
        }

        private string _SQLUsername;


        public string SQLUsername
        {
            get { return _SQLUsername; }
            set { _SQLUsername = value; }
        }
        public LineLocation()
		{ }

		public LineLocation(string ID,string LocationCode,string DivisionCode,string PlantCode,string LineCode,string LineIP,string DBName,bool IsActive,string ServerName,string SQLPassword,string LineName,string ReadGLN,string GLNExtension,string SQLUsername)
		{
			this.ID = ID;
			this.LocationCode = LocationCode;
			this.DivisionCode = DivisionCode;
			this.PlantCode = PlantCode;
			this.LineCode = LineCode;
            this.LineIP = LineIP;
            this.DBName = DBName;
            this.IsActive = IsActive;
            this.ServerName = ServerName;
            this.SQLPassword = SQLPassword;
            this.LineName = LineName;
            this.ReadGLN = ReadGLN;
            this.GLNExtension = GLNExtension;
            this.SQLUsername = SQLUsername;
		}

		public override string ToString()
		{
			return "ID = " + ID.ToString() + ",LocationCode = " + LocationCode + ",DivisionCode = " + DivisionCode + ",PlantCode = " + PlantCode + ",LineCode = " + LineCode+ ",LineIP="+LineIP+ ",DBName="+DBName+ ",IsActive="+IsActive+ ",ServerName="+ServerName+ ",SQLPassword="+SQLPassword+ ",LineName="+LineName+ ",ReadGLN="+ReadGLN+ ",GLNExtension="+GLNExtension+ ",SQLUsername="+SQLUsername;

        }

		public class IDComparer : System.Collections.Generic.IComparer<LineLocation>
		{
			public SorterMode SorterMode;
			public IDComparer()
			{ }
			public IDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<LineLocation> Membres
			int System.Collections.Generic.IComparer<LineLocation>.Compare(LineLocation x, LineLocation y)
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
		public class LocationCodeComparer : System.Collections.Generic.IComparer<LineLocation>
		{
			public SorterMode SorterMode;
			public LocationCodeComparer()
			{ }
			public LocationCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<LineLocation> Membres
			int System.Collections.Generic.IComparer<LineLocation>.Compare(LineLocation x, LineLocation y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.LocationCode.CompareTo(x.LocationCode);
				}
				else
				{
					return x.LocationCode.CompareTo(y.LocationCode);
				}
			}
			#endregion
		}
		public class DivisionCodeComparer : System.Collections.Generic.IComparer<LineLocation>
		{
			public SorterMode SorterMode;
			public DivisionCodeComparer()
			{ }
			public DivisionCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<LineLocation> Membres
			int System.Collections.Generic.IComparer<LineLocation>.Compare(LineLocation x, LineLocation y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.DivisionCode.CompareTo(x.DivisionCode);
				}
				else
				{
					return x.DivisionCode.CompareTo(y.DivisionCode);
				}
			}
			#endregion
		}
		public class PlantCodeComparer : System.Collections.Generic.IComparer<LineLocation>
		{
			public SorterMode SorterMode;
			public PlantCodeComparer()
			{ }
			public PlantCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<LineLocation> Membres
			int System.Collections.Generic.IComparer<LineLocation>.Compare(LineLocation x, LineLocation y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.PlantCode.CompareTo(x.PlantCode);
				}
				else
				{
					return x.PlantCode.CompareTo(y.PlantCode);
				}
			}
			#endregion
		}
		public class LineCodeComparer : System.Collections.Generic.IComparer<LineLocation>
		{
			public SorterMode SorterMode;
			public LineCodeComparer()
			{ }
			public LineCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<LineLocation> Membres
			int System.Collections.Generic.IComparer<LineLocation>.Compare(LineLocation x, LineLocation y)
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
	}
}
