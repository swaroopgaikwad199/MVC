using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.Models.Job;
using TnT.Models.Product;

namespace TnT.Models.Reports.Jobs
{
    public class RptDetailedJobInfoViewModel
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public PackagingAsso Product { get; set; }

        public JobDetails jbDetails { get; set; }
        public Job.Job Job { get; set; }

        public string jobtype { get; set; }
        public LineLocation LocationDetails { get; set; }
        public List<ProductionSummaryDeckWise> ProductionSummary { get; set; }

        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string VerifiedBy { get; set; }

        public string NoReadCount { get; set; }

        public string ExtraCount { get; set; }
        public string UserName { get; set; }
    }
    public class ProductionSummaryDeckWise
    {
        public string Deck { get; set; }

        public int GoodCnt { get; set; }

        public int RejectedCnt { get; set; }

        public int Total { get; set; }

        public int NRNU { get; set; }
        public int Decommisioned { get; set; }

        public int QASample { get; set; }

        public int ChallengeTest { get; set; }

        public int TotalUsedCartons { get; set; }

        public double RejectionPercent { get; set; }

    }
}