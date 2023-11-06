using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.Models.Code;
using TnT.Models.Job;
using TnT.Models.Product;

namespace TnT.Models.Reports.Jobs
{
    public class RptJobWiseSSCCViewModel
    {
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string UserName { get; set; }

        public string Jobtype { get; set; }
        public LineLocation LineDetails { get; set; }

        public PackagingAsso product { get; set; }

        public JobDetails jbDetail { get; set; }
        public PackagingAssoDetails packagingAsso { get; set; }

        public TnT.Models.Job.Job job { get; set; }

        public List<JobWiseSSCCc> SSCCs { get; set; }

    }

    public class JobWiseSSCCc
    {
        public DateTime PackagingDate { get; set; }

        public string SSCC { get; set; }

        public bool SSCCVerification { get; set; }

    }
}