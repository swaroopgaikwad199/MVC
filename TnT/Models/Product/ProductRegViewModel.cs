using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LL_Core.Models.Product
{
    public class ProductRegViewModel
    {
        [Key]
        public int PAID { get; set; }

        [Display(Name="Product Name")]
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        public bool ScheduledDrug { get; set; }
        public string DoseUsage { get; set; }

        [Display(Name = "Generic Name")]
        public string GenericName { get; set; }

        public string Composition { get; set; }

        public string ProductImage { get; set; }

        //////////////
        public string FGCode { get; set; }

        public bool UseExpDay { get; set; }
        public string ExpDateFormat { get; set; }



    }
}