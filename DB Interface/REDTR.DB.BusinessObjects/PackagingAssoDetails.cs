using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class PackagingAssoDetails
	{
		private Decimal _PAID;

		public Decimal PAID
		{
			get { return _PAID; }
			set { _PAID = value; }
		}

		private string _PackageTypeCode;

		public string PackageTypeCode
		{
			get { return _PackageTypeCode; }
			set { _PackageTypeCode = value; }
		}

		private Nullable<int> _Size;

		public Nullable<int> Size
		{
			get { return _Size; }
			set { _Size = value; }
		}

        private Nullable<int> _BundleQty;

        public Nullable<int> BundleQty
        {
            get { return _BundleQty; }
            set { _BundleQty = value; }
        }

        private string _PPN;

        public string PPN
        {
            get { return _PPN; }
            set { _PPN = value; }
        }


        private string _GTIN;

		public string GTIN
		{
			get { return _GTIN; }
			set { _GTIN = value; }
		}

        private string _NTIN;

        public string NTIN
        {
            get { return _NTIN; }
            set { _NTIN = value; }
        }

        private string _GTINCTI;

        public string GTINCTI
        {
            get { return _GTINCTI; }
            set { _GTINCTI = value; }
        }

        private Decimal _MRP;

        public Decimal MRP
        {
            get { return _MRP; }
            set { _MRP = value; }
        }

		private Nullable<int> _TerCaseIndex;

		public Nullable<int> TerCaseIndex
		{
			get { return _TerCaseIndex; }
			set { _TerCaseIndex = value; }
		}

		private string _Remarks;

		public string Remarks
		{
			get { return _Remarks; }
			set { _Remarks = value; }
		}

        private DateTime _LastUpdatedDate;
        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { _LastUpdatedDate = value; }
        }

        private string _FGCode; // Newly Added Murtaza

        public string FGCode
        {
            get { return _FGCode; }
            set { _FGCode = value; }
        }

		public PackagingAssoDetails()
		{ }

        public PackagingAssoDetails(Decimal PAID, string PackageTypeCode, Nullable<int> Size, string GTIN, Decimal MRP, Nullable<int> TerCaseIndex, string Remarks, DateTime LastUpdatedDate,string FGCode,string NTIN)
		{
			this.PAID = PAID;
			this.PackageTypeCode = PackageTypeCode;
			this.Size = Size;
			this.GTIN = GTIN;
			this.MRP = MRP;
			this.TerCaseIndex = TerCaseIndex;
			this.Remarks = Remarks;
            this.LastUpdatedDate = LastUpdatedDate;
            this.GTINCTI = GTINCTI;
            this.BundleQty = BundleQty;
            this.FGCode = FGCode; // Murtaza.
            this.NTIN = NTIN;
		}

		public override string ToString()
		{
            return "PAID = " + PAID.ToString() + ",PackageTypeCode = " + PackageTypeCode + ",Size = " + Size.ToString() + ",GTIN = " + GTIN + ",MRP=" + MRP + ",TerCaseIndex = " + TerCaseIndex.ToString() + ",Remarks = " + Remarks + " ,LastUpdatedDate=" + LastUpdatedDate+",NTIN="+NTIN;
		}

		public class PAIDComparer : System.Collections.Generic.IComparer<PackagingAssoDetails>
		{
			public SorterMode SorterMode;
			public PAIDComparer()
			{ }
			public PAIDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingAssoDetails> Membres
			int System.Collections.Generic.IComparer<PackagingAssoDetails>.Compare(PackagingAssoDetails x, PackagingAssoDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.PAID.CompareTo(x.PAID);
				}
				else
				{
					return x.PAID.CompareTo(y.PAID);
				}
			}
			#endregion
		}
		public class PackageTypeCodeComparer : System.Collections.Generic.IComparer<PackagingAssoDetails>
		{
			public SorterMode SorterMode;
			public PackageTypeCodeComparer()
			{ }
			public PackageTypeCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingAssoDetails> Membres
			int System.Collections.Generic.IComparer<PackagingAssoDetails>.Compare(PackagingAssoDetails x, PackagingAssoDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.PackageTypeCode.CompareTo(x.PackageTypeCode);
				}
				else
				{
					return x.PackageTypeCode.CompareTo(y.PackageTypeCode);
				}
			}
			#endregion
		}
		public class GTINComparer : System.Collections.Generic.IComparer<PackagingAssoDetails>
		{
			public SorterMode SorterMode;
			public GTINComparer()
			{ }
			public GTINComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingAssoDetails> Membres
			int System.Collections.Generic.IComparer<PackagingAssoDetails>.Compare(PackagingAssoDetails x, PackagingAssoDetails y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.GTIN.CompareTo(x.GTIN);
				}
				else
				{
					return x.GTIN.CompareTo(y.GTIN);
				}
			}
			#endregion
		}
        public class MRPComparer : System.Collections.Generic.IComparer<PackagingAssoDetails>
        {
            public SorterMode SorterMode;
            public MRPComparer()
            { }
            public MRPComparer(SorterMode SorterMode)
            {
                this.SorterMode = SorterMode;
            }
            #region IComparer<PackagingAssoDetails> Membres
            int System.Collections.Generic.IComparer<PackagingAssoDetails>.Compare(PackagingAssoDetails x, PackagingAssoDetails y)
            {
                if (SorterMode == SorterMode.Ascending)
                {
                    return y.MRP.CompareTo(x.MRP);
                }
                else
                {
                    return x.MRP.CompareTo(y.MRP);
                }
            }
            #endregion
        }
		public class RemarksComparer : System.Collections.Generic.IComparer<PackagingAssoDetails>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingAssoDetails> Membres
			int System.Collections.Generic.IComparer<PackagingAssoDetails>.Compare(PackagingAssoDetails x, PackagingAssoDetails y)
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
