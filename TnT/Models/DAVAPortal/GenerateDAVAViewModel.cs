using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.DAVAPortal
{
    public class GenerateDAVAViewModel
    {
        public bool IsProductSelected { get; set; }

      

        public bool IsAllBatchSelected { get; set; }

     

        public string BtcNo { get; set; }

        [Display(Name = "TracelinkProductXmlBatchNo", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string BatchNo { get; set; }

        public string BatchType { get; set; }

        [Display(Name = "DAVAIndexSelectOptionExemptedFromBarcoding", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool IsExemptedBarcode { get; set; }

        [Display(Name = "DAVAIndexSelectOptionExemptedFromBarcoding", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ExempCode { get; set; }

        [Display(Name = "DAVAIndexSelectOptionExemptedCountryCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ExemptedCountryCode { get; set; }

        public bool IsProductionSelected { get; set; }

        public bool IsWholeBatch { get; set; }

        public string SSCCCodeForPartialBatch { get; set; }

        public DateTime EXEMPTION_NOTIFICATION_AND_DATE { get; set; }



    }
}