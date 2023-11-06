using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models
{
   
    public class USerTrail
    {
        [Key]
        public decimal ID { get; set; }    

        [Required]
        public string Reason { get; set; }

        [Required]
        public DateTime AccessedAt { get; set; }

        public string LineCode { get; set; }

    }
}