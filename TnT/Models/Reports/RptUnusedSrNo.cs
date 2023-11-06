using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Reports
{
    public class RptUnusedSrNo
    {
        [Key]
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string UserName { get; set; }
        public bool ShowValidity { get; set; }
        public List<UnusedSrno> UnSrNo { get; set; }
    }

    public class UnusedSrno
    {
        public string GTIN { get; set; }
        [Key]
        public string SrNo { get; set; }
        public int ValidFor { get; set; }
    }
}