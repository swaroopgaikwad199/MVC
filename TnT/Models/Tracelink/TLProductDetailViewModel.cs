using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TnT.Models.Customer;
using TnT.Models.Product;

namespace TnT.Models.Tracelink
{
    public class TLProductDetailViewModel
    {

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "TracelinkProductXmlBatchNo", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public decimal JID { get; set; }

        [Display(Name = "TracelinkProductXmlProductName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public decimal PAID { get; set; }

        [Required]

        [Display(Name = "JobsIndexlLotNo", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string BatchNo { get; set; }

        [Required]
        public string JobName { get; set; }

        [Required]
        [Display(Name = "TracelinkProductXmlCountryName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string CountryName { get; set; }

        [Required]

        [Display(Name = "TracelinkProductXmlTargetMarketExemption", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool TMrktExemption { get; set; }

        [Required]

        [Display(Name = "TracelinkProductXmlTargetMarketExemptionDate", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd}")]
        public DateTime TMrktExemptionDate { get; set; }
        [Required]

        [Display(Name = "TracelinkProductXmlTargetMarketExemptionReference", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string TMrktRefNo { get; set; }
        [Required]

        [Display(Name = "TracelinkProductXmlCurrencyCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]

        public string currencyCode { get; set; }

        public string currencyRate { get; set; }


    }
}