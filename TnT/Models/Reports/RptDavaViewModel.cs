using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Reports
{
    public class RptDavaViewModel
    {
        [Key]
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string UserNAme { get; set; }

        public string ReportType { get; set; }

        public List<DavaData> DavaData { get; set; } 

        public string stat { get; set; }
      
    }

    public class DavaData
    {
        [Key]
        public string DavaType { get; set; }
        public string Name { get; set; }

        public DateTime GenerationDate { get; set; }

        public string Status { get; set; }

        public decimal Qty { get; set; }

        public decimal SSCCDone { get; set; }
    }
}