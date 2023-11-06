using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TnT.Models.Product;

namespace TnT.Models.Compliance
{
    public class ComplianceViewModel
    {
        public int? ID { get; set; }
        [Required]      
        public string ComplianceName { get; set; }        
        public string Remarks { get; set; }
        public List<S_JobFields> fields { get; set; }
    }
}