using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TnT.Models.Product;

namespace TnT.Models.ProductGlueSetting
{
    public class ProductGlueSetting
    {
        [Key]
        [ForeignKey("PackagingAsso")]
        public decimal ServerPAID { get; set; }

        public double HotGlueStartDistance { get; set; }

        public double HotGlueGapDistance { get; set; }

        public double HotGlueDotSize { get; set; }

        public double ColdGlueStartDistance { get; set; }

        public double ColdGlueGapDistance { get; set; }

        public double ColdGlueDotSize { get; set; }

         public virtual PackagingAsso PackagingAsso { get; set; }
    }
}