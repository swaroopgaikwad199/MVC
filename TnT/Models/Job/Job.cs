using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TnT.Models.Customer;
using TnT.Models.Product;
using TnT.Models.Providers;

namespace TnT.Models.Job
{
    public class Job
    {
     

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal JID { get; set; }

        [Display(Name = "JobsIndexPONumber", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [Required]
        public string JobName{ get; set; }

        [ForeignKey("PackLevel")]
        [Display(Name = "JobsPackagingLevel", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int PackagingLvlId { get; set; }

        [Display(Name = "JobsFGCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [Required]
        public decimal PAID { get; set; }
        [Required]

        [Display(Name = "JobsIndexlLotNo", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string BatchNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required]
        [Display(Name = "JobsManufacturingDate", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public DateTime MfgDate { get; set; }

        
        
        [Display(Name = "JobsExpiryDate", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
         [DataType(DataType.DateTime)]
        public DateTime ExpDate { get; set; }
      

        [Display(Name = "JobsBatchQty", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [Required]
        public int Quantity { get; set; }

        [Display(Name = "JobsSurplusUID", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int SurPlusQty { get; set; }

        [Required]
        public byte JobStatus { get; set; }

        public string DetailInfo { get; set; }

        [Required]
        public DateTime JobStartTime { get; set; }

        public DateTime? JobEndTime { get; set; }

        public decimal? LabelStartIndex { get; set; }


        [Display(Name = "JobsAutomaticBatchClosure", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool AutomaticBatchCloser { get; set; }

        [Display(Name = "JobsType", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public decimal TID { get; set; }

        public string MLNO { get; set; }

        public string TenderText { get; set; }

        [Display(Name = "JobsPrintwithUIDSRNO", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool JobWithUID { get; set; }

        public string Remarks { get; set; }


        public decimal? CreatedBy { get; set; }

        public decimal? VerifiedBy { get; set; }

  
        public DateTime? VerifiedDate { get; set; }

        [Required]
        [Display(Name = "JobsIndexCreateDate", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime LastUpdatedDate { get; set; }

        public decimal AppId { get; set; }

        [Display(Name = "JobsProductionLine", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string LineCode { get; set; }

        public bool SYNC { get; set; }


        [Display(Name = "JobsForExport", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool ForExport { get; set; }

        [Display(Name = "JobsPrimaryPCMap", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public decimal PrimaryPCMapCount { get; set; }

        public bool DavaPortalUpload { get; set; }

        public string PlantCode { get; set; }

        public decimal NoReadCount { get; set; }

        [Display(Name = "JobsExpDateformat", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ExpDateFormat { get; set; }

        [Display(Name = "JobsUseExpiryDay", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [Required]
        public bool UseExpDay { get; set; }

        [ForeignKey("Customer")]
        [Display(Name = "JobsCustomerId", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int? CustomerId { get; set; }

        [ForeignKey("Proivder")]
        public int? ProviderId { get; set; }

        [Display(Name = "JobsPPNCountryCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string PPNCountryCode { get; set; }

        [Display(Name = "JobsPPNPostalCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string PPNPostalCode { get; set; }
        public string CompType { get; set; }
        public bool? TransferToGlobal { get; set; }

        public virtual M_Customer Customer { get; set; }

        public virtual M_Providers Proivder { get; set; }


        public virtual PackagingLevels PackLevel { get; set; }
    }
}