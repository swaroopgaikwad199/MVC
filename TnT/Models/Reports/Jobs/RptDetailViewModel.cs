using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.Models.Product;

namespace TnT.Models.Reports.Jobs
{
    public class RptDetailViewModel
    {
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string UserName { get; set; }
        public List<JobInfoDetails> Jobs  {get;set;}
                
    }

    public class JobInfoDetails
    {
        public PackagingAsso Product { get; set; }

        public TnT.Models.Job.Job Job { get; set; }

        public string ProductName { get; set; }
        public string Btachno { get; set; }
        public DateTime MfgDate { get; set; }
        public DateTime ExpDate { get; set; }
        public string AutoBatchClose { get; set; }
        public int Qty { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string VerifiedBy { get; set; }
    }


}