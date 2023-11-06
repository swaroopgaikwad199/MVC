using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.RFXLImport
{
    public class RFXLImportViewModel
    {
        
        [Display(Name = "ImporterTracelinkCustomer", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "ImporterTracelinkQuantity", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int Quantity { get; set; }

        [Display(Name = "ImporterTracelinkGTIN", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string GTIN { get; set; }
    }
}