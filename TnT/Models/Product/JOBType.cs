using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Product
{
    public class JOBType
    {
        [Key]
        public decimal TID { get; set; }
        
        public string  Job_Type { get; set; }
    }
}