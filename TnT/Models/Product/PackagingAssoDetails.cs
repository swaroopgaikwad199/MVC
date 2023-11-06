using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.Product
{
    public class PackagingAssoDetails
    {
        [Key]
        public int Id { get; set; }

       // [ForeignKey("Products")]
        public decimal PAID { get; set; }

        public string PackageTypeCode { get; set; }

        public int Size { get; set; }

        public decimal MRP { get; set; }

        public int TerCaseIndex { get; set; }

        public string Remarks { get; set; }

        [Required]
        public DateTime LastUpdatedDate { get; set; }

        public string LineCode { get; set; }

        public bool SYNC { get; set; }

        public string PPN { get; set; }

        public string GTIN { get; set; }

        public string GTINCTI { get; set; }

        public int BundleQty { get; set; }

        public string NTIN { get; set; }
        //public virtual ICollection<PackagingAsso> Products { get; set; }

    }
}