using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TnT.Models.Product;

namespace TnT.Models.ProductApplicatorSetting
{
    public class ProductApplicatorSetting
    {
        [Key]
        [ForeignKey("PackagingAsso")]
        public decimal ServerPAID { get; set; }
        public float S1 { get; set; }

        public float S2 { get; set; }

        public float S3 { get; set; }

        public float S4 { get; set; }

        public float S5 { get; set; }


        public decimal FrontLabelOffset { get; set; }
        public decimal BackLabelOffset { get; set; }

        public decimal CartonLength { get; set; }

        public virtual PackagingAsso PackagingAsso { get; set; }
    }
}