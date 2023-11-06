using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.Models.Home
{
    public class LineChartViewModel
    {
        public string LineName { get; set; }
        public List<PrintedPackDetails> printingDetails { get; set; }
        public List<PrintStatusInfo> printingExecutionDetails { get; set; }

    }
}