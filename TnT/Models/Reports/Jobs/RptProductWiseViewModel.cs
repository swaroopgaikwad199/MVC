using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.Models.Job;
using TnT.Models.Product;

namespace TnT.Models.Reports.Jobs
{
    public class RptProductWiseViewModel
    {
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string UserName { get; set; }
        public List<ProductInfoStats> Products { get; set; }

       // public List<JobInfoStats> Jobs { get; set; }

        public string Packinglevel { get; set; }

        public IEnumerable<SelectListItem> Packaginlevels { get; set; }
    }

    public class ProductInfoStats
    {
        public PackagingAsso product;

        public PackagingAssoDetails jb;
        public string GTINtertiary { get; set; }

        public string GTIN { get; set; }

        public string DECK { get; set; }

        public string totalbatchcnt { get; set; }

        public int totalSrn { get; set; }
        public List<JobInfoStats> Jobs { get; set; }

    }

    public class JobInfoStats
    {  

        public TnT.Models.Job.Job Job;
        
        public int GoodCount { get; set; }

        public int BadCount { get; set; }

        public int decommisionedCount { get; set; }

        public int notVerified { get; set; }


        public int Total { get; set; }
    }
}