using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.ImportXml
{
    public class X_ChinaUIDs
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }
        public string Code { get; set; }
        public decimal PAID { get; set; }
        public string PackageTypeCode { get; set; }
        public bool IsUsed { get; set; }
    }
}