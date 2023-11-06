using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.Models.Job;
using TnT.Models.Product;

namespace TnT.Models.Reports.Jobs
{
    public class RptDecommisionedUidsViewModel
    {
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public PackagingAsso Product { get; set; }
        public Job.Job Job { get; set; }
        public LineLocation LocationDetails { get; set; }

        public List<DecommisionedUidsLst> DecomUidLst { get; set; }

        public string Packinglevel { get; set; }
        public IEnumerable<SelectListItem> Packaginlevels { get; set; }
    }
    public class DecommisionedUidsLst
    {
        public string Code { get; set; }

        public DateTime  PckDt { get; set; }

        public string Status { get; set; }

    }
}