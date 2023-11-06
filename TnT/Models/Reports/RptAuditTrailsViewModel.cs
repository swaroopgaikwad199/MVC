using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.Models.Reports
{
    public class RptAuditTrailsViewModel
    {
        public string UID { get; set; }
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string UserName { get; set; }
        public string RptType { get; set; }

        public DateTime FrmDt { get; set; }

        public DateTime ToDt { get; set; }

        public string lineLocation { get; set; }

        public string Activity { get; set; }

        public string ActivityId { get; set; }
        public string Type { get; set; }
        public List<ATrailings> Trails { get; set; }
       
    }

    public class ATrailings
    {
        public string UserName { get; set; }

        public string UserType { get; set; }

        public DateTime Time { get; set; }

        public string Reason { get; set; }

        public string Activity { get; set; }
        
    }
}