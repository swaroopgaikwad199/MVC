using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TnT.Models.Reports.Jobs
{
    public class RptOptrStatsViewModel
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }

        public string UserName { get; set; }

        public List<OperatorStats> OpretorStats { get; set; }

        public string Packinglevel { get; set; }

        public IEnumerable<SelectListItem> Packaginlevels { get; set; }


    }
    public class OperatorStats
    {
        public string OperatorName { get; set; }
        public List<BatchStats> BatchStats { get; set; }
    }   

    public class BatchStats
    {

        public string BatchNo { get; set; }
       
        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public int GoodCnt { get; set; }
        public int BadCnt { get; set; }
        public string oprName { get; set; }

        public int NoRead { get; set; }
    }
   
}