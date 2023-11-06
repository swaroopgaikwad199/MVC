using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.Job
{
    public class M_SMSSync
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        
        public int JID { get; set; }

        public bool IsSync { get; set; }

        public DateTime LastUpdatedDate { get; set; }

       
    }
}