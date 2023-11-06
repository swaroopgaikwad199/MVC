using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.TraceLinkImporter
{
    public class TracelinkViewModel
    {

        [Display(Name = "ImporterTracelinkCustomer", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int CustomerId { get; set; }

        [Display(Name = "ImporterTracelinkQuantity", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int Quantity { get; set; }

        [Display(Name = "ImporterTracelinkGTIN", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string GTIN { get; set; }

        public string SrnoType { get; set; }

        public string filterValue { get; set; }

        public string PAID { get; set; }
    }
}