using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TnT.Models.Reports.Jobs
{
    public class RptDtlsWithOptrViewModel
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }
      
        public string UserName { get; set; }

        public List<OptrBatchDetails> JobOpretors { get; set; }


        public string Packinglevel { get; set; }

        public IEnumerable<SelectListItem> Packaginlevels { get; set; }


    }
    public class OptrBatchDetails
    {
        public string BatchNo { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }
        public List<OperatorBatchWork> OperatorWorkings { get; set; }
    }

    public class OperatorBatchWork
    {
        public string OperatorName { get; set; }

        public int GoodCnt { get; set; }

        public int BadCnt { get; set; }

        public int NoRead { get; set; }
    }
}