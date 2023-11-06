using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class PackageTypeCode
	{
		private string _Code;

		public string Code
		{
			get { return _Code; }
			set { _Code = value; }
		}

		private SByte _CodeSeq;

		public SByte CodeSeq
		{
			get { return _CodeSeq; }
			set { _CodeSeq = value; }
		}

		private string _Name;

		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		private string _Description;

		public string Description
		{
			get { return _Description; }
			set { _Description = value; }
		}

		private string _Remarks;

		public string Remarks
		{
			get { return _Remarks; }
			set { _Remarks = value; }
		}

		public PackageTypeCode()
		{ }

		public PackageTypeCode(string Code,SByte CodeSeq,string Name,string Description,string Remarks)
		{
			this.Code = Code;
			this.CodeSeq = CodeSeq;
			this.Name = Name;
			this.Description = Description;
			this.Remarks = Remarks;
		}

		public override string ToString()
		{
			return "Code = " + Code + ",CodeSeq = " + CodeSeq.ToString() + ",Name = " + Name + ",Description = " + Description + ",Remarks = " + Remarks;
		}

		public class CodeComparer : System.Collections.Generic.IComparer<PackageTypeCode>
		{
			public SorterMode SorterMode;
			public CodeComparer()
			{ }
			public CodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackageTypeCode> Membres
			int System.Collections.Generic.IComparer<PackageTypeCode>.Compare(PackageTypeCode x, PackageTypeCode y)
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
		public class CodeSeqComparer : System.Collections.Generic.IComparer<PackageTypeCode>
		{
			public SorterMode SorterMode;
			public CodeSeqComparer()
			{ }
			public CodeSeqComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackageTypeCode> Membres
			int System.Collections.Generic.IComparer<PackageTypeCode>.Compare(PackageTypeCode x, PackageTypeCode y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.CodeSeq.CompareTo(x.CodeSeq);
				}
				else
				{
					return x.CodeSeq.CompareTo(y.CodeSeq);
				}
			}
			#endregion
		}
		public class NameComparer : System.Collections.Generic.IComparer<PackageTypeCode>
		{
			public SorterMode SorterMode;
			public NameComparer()
			{ }
			public NameComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackageTypeCode> Membres
			int System.Collections.Generic.IComparer<PackageTypeCode>.Compare(PackageTypeCode x, PackageTypeCode y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.Name.CompareTo(x.Name);
				}
				else
				{
					return x.Name.CompareTo(y.Name);
				}
			}
			#endregion
		}
		public class DescriptionComparer : System.Collections.Generic.IComparer<PackageTypeCode>
		{
			public SorterMode SorterMode;
			public DescriptionComparer()
			{ }
			public DescriptionComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackageTypeCode> Membres
			int System.Collections.Generic.IComparer<PackageTypeCode>.Compare(PackageTypeCode x, PackageTypeCode y)
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
		public class RemarksComparer : System.Collections.Generic.IComparer<PackageTypeCode>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackageTypeCode> Membres
			int System.Collections.Generic.IComparer<PackageTypeCode>.Compare(PackageTypeCode x, PackageTypeCode y)
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
