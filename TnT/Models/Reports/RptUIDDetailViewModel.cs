using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Reports
{
    public class RptUIDDetailViewModel
    {
        [Key]
        public int JID { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string UserName { get; set; }
        public string Type { get; set; }
        public string JobType { get; set; }
        public decimal TID { get; set; }
        public string BPR { get; set; }
        public string BatchNo { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string GTIN { get; set; }
        public DateTime MfgDate { get; set; }
        public DateTime ExpDate { get; set; }

        public int BatchQty { get; set; }
        public string JobWithUid { get; set; }

        public string LocationCode { get; set; }

        public string DivCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public string InspectionSet { get; set; }
        public string PackagingType { get; set; }
        public DateTime PackagingDate { get; set; }
        public string Operator { get; set; }
        public string Status { get; set; }
        public string SSCC { get; set; }
        public string CaseNo { get; set; }

        public string SSCCVerifiedStatus { get; set; }
        public string parentCode { get; set; }

        public string UIDCode { get; set; }

        public string FailureReason { get; set; }

       
        public List<ChildCode> ChildCode { get; set; }

    }

    public class ChildCode
    {
        [Key]
        public string SrNo { get; set; }
    }
}