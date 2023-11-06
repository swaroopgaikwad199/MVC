using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Code
{
    public class SSCCLineHolder
    {
        [Key]
        public int ID { get; set; }

        public decimal? PackageIndicator { get; set; }

        public decimal LastSSCC { get; set; }

        public string Remarks { get; set; }

        public decimal JobID { get; set; }

        public decimal FirstSSCC { get; set; }

        public string LineCode { get; set; }

        public int? RequestId { get; set; }

        public string Type { get; set; }

        public int Customer { get; set; }

    }
}