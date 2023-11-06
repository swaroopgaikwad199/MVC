using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Reports
{
    public class RptAvailableSerialNo
    {
        [Key]
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string UserName { get; set; }
        public List<SerailNo> SrNoCount { get; set; }
    }

    public class SerailNo
    {
       
        public string GTIN { get; set; }
        [Key]
        public string SerailNoCount { get; set; }
    }
}