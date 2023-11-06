using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Reports
{
    public class RptTlinkRequest
    {
        [Key]
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string UserName { get; set; }
        public List<RequestedGtin> RGTIN { get; set; }
    }


    public class RequestedGtin
    {
        public string GTIN { get; set; }
        [Key]
        public int  Quantity { get; set; }
        public string Customer { get; set; }
        public DateTime RequestedOn { get; set; }
    }

}