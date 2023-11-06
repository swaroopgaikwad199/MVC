using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.Models.Home
{
    public class DashboardViewModel
    {
        public class GraphData
        {
            public string y { get; set; }
            public string x { get; set; }
        }

        public class GraphDataBatchProd
        {
            public string batch { get; set; }
            public string prod { get; set; }
            public string month { get; set; }
        }

        public class GraphDataLinewise
        {
            public string line { get; set; }
            public string batchCount { get; set; }
        }

        public class GrapDataBatchStatus
        {
            public string x { get; set; }
            public string y { get; set; }
        }
    }
}