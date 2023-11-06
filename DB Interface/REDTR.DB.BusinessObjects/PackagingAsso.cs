using System;
using System.Text;

namespace REDTR.DB.BusinessObjects
{
	[Serializable()]
	public class PackagingAsso
	{
		private Decimal _PAID;

		public Decimal PAID
		{
			get { return _PAID; }
			set { _PAID = value; }
		}

		private string _Name;

		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		private string _ProductCode;

		public string ProductCode
		{
			get { return _ProductCode; }
			set { _ProductCode = value; }
		}


        private string _FGCode;

        public string FGCode
        {
            get { return _FGCode; }
            set { _FGCode = value; }
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

        private string _DosageForm;

        public string DosageForm
        {
            get { return _DosageForm; }
            set { _DosageForm = value; }
        }

        private DateTime _CreatedDate;

		public DateTime CreatedDate
		{
			get { return _CreatedDate; }
			set { _CreatedDate = value; }
		}

        private bool _IsActive;

        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        private bool _DAVAPortalUpload;

        public bool DAVAPortalUpload
        {
            get { return _DAVAPortalUpload; }
            set { _DAVAPortalUpload = value; }
        }


		private DateTime _LastUpdatedDate;

		public DateTime LastUpdatedDate
		{
			get { return _LastUpdatedDate; }
			set { _LastUpdatedDate = value; }
		}

        private bool _ScheduledDrug;

       
        public bool ScheduledDrug
        {
            get { return _ScheduledDrug; }
            set { _ScheduledDrug = value; }
        }

        private string _DoseUsage;

      
        public string DoseUsage
        {
            get { return _DoseUsage; }
            set { _DoseUsage = value; }
        }

        private string _GenericName;

      
        public string GenericName
        {
            get { return _GenericName; }
            set { _GenericName = value; }
        }

        private string _Composition;

        
        public string Composition
        {
            get { return _Composition; }
            set { _Composition = value; }
        }

        private string _InternalMaterialCode;
        public string InternalMaterialCode // By Vikrant Kumbhar
        {
            get { return _InternalMaterialCode; }
            set { _InternalMaterialCode = value; }
        }

        private string _CountryDrugCode;
        public string CountryDrugCode // By Vikrant Kumbhar
        {
            get { return _CountryDrugCode; }
            set { _CountryDrugCode = value; }
        }

        private string _SaudiDrugCode;
        public string SaudiDrugCode // By Vikrant Kumbhar
        {
            get { return _SaudiDrugCode; }
            set { _SaudiDrugCode = value; }
        }

        private bool _UseExpDay;

        public bool UseExpDay                       // By Ansuman For IPCA on 26/09/2015
        {
            get { return _UseExpDay; }
            set { _UseExpDay = value; }
        }

        private string _ExpDateFormat;

        public string ExpDateFormat                 // By Ansuman For IPCA on 26/09/2015
        {
            get { return _ExpDateFormat; }
            set { _ExpDateFormat = value; }
        }

        private string _ProductImage;
      
        public string ProductImage
        {
            get { return _ProductImage; }
            set { _ProductImage = value; }
        }

        private bool _VerifyProd;        // Added by aparna for product verification.

        public bool VerifyProd
        {
            get { return _VerifyProd; }
            set { _VerifyProd = value; }
        }

        private string _FEACN;
        public string FEACN {
            get {return _FEACN; }
            set { _FEACN=value; }
        }
        private string _Workshop;
        public string Workshop
        {
            get { return _Workshop; }
            set { _Workshop = value; }
        }
        private string _ResProdCode;
        public string ResProdCode
        {
            get { return _ResProdCode; }
            set { _ResProdCode = value; }
        }
        private string _PackUnit;
        public string PackUnit
        {
            get { return _PackUnit; }
            set { _PackUnit = value; }
        }
        private string _SubTypeSpec;
        public string SubTypeSpec
        {
            get { return _SubTypeSpec; }
            set { _SubTypeSpec = value; }
        }
        private string _SubType;
        public string SubType
        {
            get { return _SubType; }
            set { _SubType = value; }
        }
        private string _AuthorizedNo;
        public string AuthorizedNo
        {
            get { return _AuthorizedNo; }
            set { _AuthorizedNo = value; }
        }
        private string _PackageSpec;
        public string PackageSpec
        {
            get { return _PackageSpec; }
            set { _PackageSpec = value; }
        }
        private string _SubTypeNo;
        public string SubTypeNo
        {
            get { return _SubTypeNo; }
            set { _SubTypeNo = value; }
        }

        private int _ProviderId;

        public int ProviderId
        {
            get { return _ProviderId; }
            set { _ProviderId = value; }
        }

        private string _PlantCode;

        public string PlantCode
        {
            get { return _PlantCode; }
            set { _PlantCode = value; }
        }

        private string _SAPProductCode;

        public string SAPProductCode
        {
            get { return _SAPProductCode; }
            set { _SAPProductCode = value; }
        }

        private string _Manufacturer;

        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { _Manufacturer = value; }
        }

        private string _Strength;

        public string Strength
        {
            get { return _Strength; }
            set { _Strength = value; }
        }

        private string _ContainerSize;

        public string ContainerSize
        {
            get { return _ContainerSize; }
            set { _ContainerSize = value; }
        }

        private bool _SYNC;

        public bool SYNC
        {
            get { return _SYNC; }
            set { _SYNC = value; }
        }

        private string _NHRN;

        public string NHRN
        {
            get { return _NHRN; }
            set { _NHRN = value; }
        }

        private string _CompType;

        public string CompType
        {
            get { return _CompType; }
            set { _CompType = value; }
        }

        private string _PublicKey;

        public string PublicKey
        {
            get { return _PublicKey; }
            set { _PublicKey = value; }
        }
        /*
            SubTypeNo
            PackageSpec 
            AuthorizedNo
            SubType
            SubTypeSpec
            PackUnit
            ResProdCode 
            Workshop
         */

        public PackagingAsso()
		{ }

        public PackagingAsso(Decimal PAID, string Name, string ProductCode,string FGCode,string Description, string Remarks, DateTime CreatedDate, DateTime LastUpdatedDate, bool IsActive, bool ScheduledDrug, string DoseUsage, string GenericName, string Composition, string ProductImage, bool DAVAPortalUpload,bool VerifyProd,bool UseExpDay,string ExpDateFormat,string FEACN,int ProviderId,string PlantCode,string SAPProductCode,string Manufacturer,string Strength,string ContainerSize,bool SYNC,string NHRN,string CompType)
		{
			this.PAID = PAID;
			this.Name = Name;
			this.ProductCode = ProductCode;
            this.FGCode=FGCode;
			this.Description = Description;
			this.Remarks = Remarks;
			this.CreatedDate = CreatedDate;
			this.LastUpdatedDate = LastUpdatedDate;
            this.IsActive = IsActive;
            this.ScheduledDrug = ScheduledDrug;
            this.DoseUsage = DoseUsage;
            this.GenericName = GenericName;
            this.Composition = Composition;
            this.ProductImage = ProductImage;
            this.DAVAPortalUpload = DAVAPortalUpload;
            this.VerifyProd = VerifyProd;
            this.UseExpDay = UseExpDay;             // By Ansuman
            this.ExpDateFormat = ExpDateFormat;     // By Ansuman
            this.FEACN = FEACN;
            this.ProviderId = ProviderId;
            this.PlantCode = PlantCode;
            this.SAPProductCode = SAPProductCode;
            this.Manufacturer = Manufacturer;
            this.Strength = Strength;
            this.ContainerSize = ContainerSize;
            this.SYNC = SYNC;
            this.NHRN = NHRN;
            this.CompType = CompType;
        }


		public override string ToString()
		{
            return "";///"PAID = " + PAID.ToString() + ",Name = " + Name + ",ProductCode = " + ProductCode + ",Description = " + Description + ",Remarks = " + Remarks + ",CreatedDate = " + CreatedDate.ToString() + ",LastUpdatedDate = " + LastUpdatedDate.ToString() + ",IsActive = " + IsActive.ToString() + ",ScheduledDrug = " + ScheduledDrug.ToString() + ",DoseUsage = " + DoseUsage + ",GenericName = " + GenericName + ",Composition = " + Composition + ",ProductImage = " + ProductImage + ",DAVAPortalUpload=" + DAVAPortalUpload.ToString() + ",VerifyProd=" + VerifyProd.ToString() + ",UseExpDAy=" + UseExpDay.ToString() + ",ExpDateFormat=" + ExpDateFormat.ToString();  // Modified By Ansuman
		}

		public class PAIDComparer : System.Collections.Generic.IComparer<PackagingAsso>
		{
			public SorterMode SorterMode;
			public PAIDComparer()
			{ }
			public PAIDComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingAsso> Membres
			int System.Collections.Generic.IComparer<PackagingAsso>.Compare(PackagingAsso x, PackagingAsso y)
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
		public class NameComparer : System.Collections.Generic.IComparer<PackagingAsso>
		{
			public SorterMode SorterMode;
			public NameComparer()
			{ }
			public NameComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingAsso> Membres
			int System.Collections.Generic.IComparer<PackagingAsso>.Compare(PackagingAsso x, PackagingAsso y)
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
		public class ProductCodeComparer : System.Collections.Generic.IComparer<PackagingAsso>
		{
			public SorterMode SorterMode;
			public ProductCodeComparer()
			{ }
			public ProductCodeComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingAsso> Membres
			int System.Collections.Generic.IComparer<PackagingAsso>.Compare(PackagingAsso x, PackagingAsso y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.ProductCode.CompareTo(x.ProductCode);
				}
				else
				{
					return x.ProductCode.CompareTo(y.ProductCode);
				}
			}
			#endregion
		}
		public class DescriptionComparer : System.Collections.Generic.IComparer<PackagingAsso>
		{
			public SorterMode SorterMode;
			public DescriptionComparer()
			{ }
			public DescriptionComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingAsso> Membres
			int System.Collections.Generic.IComparer<PackagingAsso>.Compare(PackagingAsso x, PackagingAsso y)
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
		public class RemarksComparer : System.Collections.Generic.IComparer<PackagingAsso>
		{
			public SorterMode SorterMode;
			public RemarksComparer()
			{ }
			public RemarksComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingAsso> Membres
			int System.Collections.Generic.IComparer<PackagingAsso>.Compare(PackagingAsso x, PackagingAsso y)
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
		public class CreatedDateComparer : System.Collections.Generic.IComparer<PackagingAsso>
		{
			public SorterMode SorterMode;
			public CreatedDateComparer()
			{ }
			public CreatedDateComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingAsso> Membres
			int System.Collections.Generic.IComparer<PackagingAsso>.Compare(PackagingAsso x, PackagingAsso y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.CreatedDate.CompareTo(x.CreatedDate);
				}
				else
				{
					return x.CreatedDate.CompareTo(y.CreatedDate);
				}
			}
			#endregion
		}
		public class LastUpdatedDateComparer : System.Collections.Generic.IComparer<PackagingAsso>
		{
			public SorterMode SorterMode;
			public LastUpdatedDateComparer()
			{ }
			public LastUpdatedDateComparer(SorterMode SorterMode)
			{
				this.SorterMode = SorterMode;
			}
			#region IComparer<PackagingAsso> Membres
			int System.Collections.Generic.IComparer<PackagingAsso>.Compare(PackagingAsso x, PackagingAsso y)
			{
				if (SorterMode == SorterMode.Ascending)
				{
					return y.LastUpdatedDate.CompareTo(x.LastUpdatedDate);
				}
				else
				{
					return x.LastUpdatedDate.CompareTo(y.LastUpdatedDate);
				}
			}
			#endregion
		}
	}
}
