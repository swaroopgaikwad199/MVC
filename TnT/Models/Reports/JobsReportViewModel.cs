using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.Models.Reports
{
    public class JobsReportViewModel
    {
        public string LocationId { get; set; }
        public string LocationCode { get; set; }

        public string DivisionCode { get; set; }

        public string PlantCode { get; set; }

        public string LineCode { get; set; }

        public string ProductName { get; set; }

        public string JobStatus { get; set; }

        public decimal IdBatchNo { get; set; }

        public decimal IdJobName { get; set; }

        public DateTime JobFromCreatedDate { get; set; }

        public DateTime JobToCreatedDate { get; set; }

        public DateTime MfgDateWiseFrom { get; set; }

        public DateTime MfgDateWiseTo { get; set; }

        public DateTime ExpDateWiseFrom { get; set; }
        public DateTime ExpDateWiseTo { get; set; }

        public bool DecommisionJobs { get; set; }



    }

    class JobStatus
    {
        public string Status { get; set; }
    }

}