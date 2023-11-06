using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TnT.Models.Providers;

namespace TnT.Models.Product
{
    public class PackagingAsso
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal PAID { get; set; }

        [Required]        
        [Display(Name = "ProductName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "ProductProductCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ProductCode { get; set; }

        [Display(Name = "ProductDescription", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Description { get; set; }

        [Display(Name = "ProductRemarks", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Remarks { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "ProductIsActive", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool IsActive { get; set; }
        [Required]
        public DateTime LastUpdatedDate { get; set; }
        public string LineCode { get; set; }
        public bool? SYNC { get; set; }

        [Display(Name = "ProductIsScheduledDrug", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool ScheduledDrug { get; set; }

        [Display(Name = "ProductDoseusage", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string DoseUsage { get; set; }

        [Display(Name = "ProductGenericName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string GenericName { get; set; }

        [Display(Name = "ProductComposition", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Composition { get; set; }
        public string ProductImage { get; set; }
        public bool? DAVAPortalUpload { get; set; }

        [Required]
        [Display(Name = "ProductFGCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string FGCode { get; set; }

        public bool VerifyProd { get; set; }

      

        [Display(Name = "JobsExpDateformat", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ExpDateFormat { get; set; }
        public string PlantCode { get; set; }
        public string SAPProductCode { get; set; }


        [Display(Name = "JobsUseExpiryDay", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool UseExpDay { get; set; }

        [Display(Name = "ProductInternalMaterialCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string InternalMaterialCode { get; set; }

        [Display(Name = "ProductNationalDrugCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string CountryDrugCode { get; set; }

        [Display(Name = "SaudiDrugCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string SaudiDrugCode { get; set; }

        [Display(Name = "ProductDoseageForm", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string DosageForm { get; set; }

        [Display(Name = "ProductType", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [NotMapped]
        public int Type { get; set; }

        [Display(Name = "ProductFeacn", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string FEACN { get; set; }

        [ForeignKey("Proivder")]
        public int? ProviderId { get; set; }

        [NotMapped]
        public int Dose { get; set; }
        [Display(Name = "SubTypeNumber", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string SubTypeNo { get; set; }
        [Display(Name = "PackageSpecification", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string PackageSpec { get; set; }
        [Display(Name = "AuthorizedNo", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string AuthorizedNo { get; set; }
        [Display(Name = "SubType", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string SubType { get; set; }
        [Display(Name = "SubTypeSpecification", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string SubTypeSpec { get; set; }
        [Display(Name = "PackUnit", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string PackUnit { get; set; }
        [Display(Name = "ResProdCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ResProdCode { get; set; }
        [Display(Name = "Workshop", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Workshop { get; set; }

        public string NHRN { get; set; }


        //[Display(Name = "ProductPublicKey", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        //public string PublicKey { get; set; }

        //public string CompType { get; set; }
        public virtual M_Providers Proivder { get; set; }
        //public virtual ICollection<PackagingAssoDetails> PackagingAssoDetails { get; set; }
        //public virtual ICollection<PackageLabelMaster> PackageLabelMaster { get; set; }

    }
}