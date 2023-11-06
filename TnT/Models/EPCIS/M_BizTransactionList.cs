using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.EPCIS
{
    public class M_BizTransactionList
    {
        [Key]
        public decimal ID { get; set; }

        [StringLength(50)]
        public string Value { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(50)]
        public string Definition { get; set; }
    }
}