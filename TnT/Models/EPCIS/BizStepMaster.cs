using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.EPCIS
{
    public class BizStepMaster
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string BizStep { get; set; }

        public bool CommonIsactive { get; set; }

    } 
}