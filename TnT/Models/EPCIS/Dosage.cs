using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.EPCIS
{
    public class Dosage
    {

        [Key]
        public decimal ID { get; set; }

        [StringLength(100)]
        public string DosageName { get; set; }

        [StringLength(50)]
        public string UseRestrictions { get; set; }

        [StringLength(50)]
        public string ShortName { get; set; }

    
        public decimal FDACode { get; set; }

        [StringLength(50)]
        public string NCIConceptID { get; set; }

    }
}