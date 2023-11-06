using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Reports
{
    public class TKeyUnusedSrNoCount
    {
        [Display(Name = "ProductName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ProductName { get; set; }
        public string GTIN { get; set; }
        [Display(Name = "ImporterTracelinkQuantity", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int Count { get; set; }
    }
}