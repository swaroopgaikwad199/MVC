using System;
using System.Text;
using System.Runtime.Serialization;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	
	public class Country
	{
		private int _Id;
		public int Id
		{
			get { return _Id; }
			set { _Id = value; }
		}

		private string _CountryName;

		
		public string CountryName
		{
			get { return _CountryName; }
			set { _CountryName = value; }
		}

		private string _TwoLetterAbbreviation;

		
		public string TwoLetterAbbreviation
		{
			get { return _TwoLetterAbbreviation; }
			set { _TwoLetterAbbreviation = value; }
		}

		private string _ThreeLetterAbbreviation;

		
		public string ThreeLetterAbbreviation
		{
			get { return _ThreeLetterAbbreviation; }
			set { _ThreeLetterAbbreviation = value; }
		}

		public Country()
		{ }

		public Country(int Id,string CountryName,string TwoLetterAbbreviation,string ThreeLetterAbbreviation)
		{
			this.Id = Id;
			this.CountryName = CountryName;
			this.TwoLetterAbbreviation = TwoLetterAbbreviation;
			this.ThreeLetterAbbreviation = ThreeLetterAbbreviation;
		}

		public override string ToString()
		{
			return "Id = " + Id.ToString() + ",CountryName = " + CountryName + ",TwoLetterAbbreviation = " + TwoLetterAbbreviation + ",ThreeLetterAbbreviation = " + ThreeLetterAbbreviation;
		}

		public class IdComparer : System.Collections.Generic.IComparer<Country>
		{
			public SorterMode SorterMode;
			public IdComparer()
			{ }
			public IdComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Country> Membres
			int System.Collections.Generic.IComparer<Country>.Compare(Country x, Country y)
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
		public class CountryNameComparer : System.Collections.Generic.IComparer<Country>
		{
			public SorterMode SorterMode;
			public CountryNameComparer()
			{ }
			public CountryNameComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Country> Membres
			int System.Collections.Generic.IComparer<Country>.Compare(Country x, Country y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.CountryName.CompareTo(x.CountryName);
				}
				else
				{
					return x.CountryName.CompareTo(y.CountryName);
				}
			}
			#endregion
		}
		public class TwoLetterAbbreviationComparer : System.Collections.Generic.IComparer<Country>
		{
			public SorterMode SorterMode;
			public TwoLetterAbbreviationComparer()
			{ }
			public TwoLetterAbbreviationComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Country> Membres
			int System.Collections.Generic.IComparer<Country>.Compare(Country x, Country y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.TwoLetterAbbreviation.CompareTo(x.TwoLetterAbbreviation);
				}
				else
				{
					return x.TwoLetterAbbreviation.CompareTo(y.TwoLetterAbbreviation);
				}
			}
			#endregion
		}
		public class ThreeLetterAbbreviationComparer : System.Collections.Generic.IComparer<Country>
		{
			public SorterMode SorterMode;
			public ThreeLetterAbbreviationComparer()
			{ }
			public ThreeLetterAbbreviationComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<Country> Membres
			int System.Collections.Generic.IComparer<Country>.Compare(Country x, Country y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.ThreeLetterAbbreviation.CompareTo(x.ThreeLetterAbbreviation);
				}
				else
				{
					return x.ThreeLetterAbbreviation.CompareTo(y.ThreeLetterAbbreviation);
				}
			}
			#endregion
		}
	}
}
