using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.EPCIS
{
    public class Dispositions
    {
        [Key]
        public int Id { get; set; }
        
        public int BizStepId { get; set; }

        [StringLength(50)]
        public string Disposition { get; set; }

        public bool IsReused { get; set; }

        [StringLength(20)]
        public string Action { get; set; }

    }
}