using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.Models;
using TnT.Models.Job;
using TnT.Models.Product;

namespace TnT.Models.Reports.Jobs
{
    public class RptSummaryViewModel
    {
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string Username { get; set; }

        public List<JobSummary> JobInfo { get; set; }

        // public List<JobInfoStats> Jobs { get; set; }

        public string Packinglevel { get; set; }

        public IEnumerable<SelectListItem> Packaginlevels { get; set; }

    }
    
    public class JobSummary
    {
        public TnT.Models.Job.Job Job;

        public PackagingAsso product;
        public JobDetails jb;
        public string GTINtertiary { get; set; }

        public string GTIN { get; set; }

        public string DECK { get; set; }

        public int GoodCount { get; set; }

        public int BadCount { get; set; }

        public int decommisionedCount { get; set; }

        public int notVerified { get; set; }


        public int Total { get; set; }
    }


}