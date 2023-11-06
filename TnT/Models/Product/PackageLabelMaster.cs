using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.Product
{
    public class PackageLabelMaster
    {
        [Key]
        public int Id { get; set; }

        //[ForeignKey("Products")]
        public decimal PAID { get; set; }

        public decimal JobTypeID { get; set; }

        public string Code { get; set; }

        public string LabelName { get; set; }

        public string Filter { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        //public virtual ICollection<PackagingAsso> Products { get; set; }

    }
}