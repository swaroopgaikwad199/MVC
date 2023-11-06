using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Code
{
    public class PackagingDetails
    {
        

        [Key]
        public decimal PackDtlsID { get; set; }

        public string  Code { get; set; }

        public decimal? PAID { get; set; }

        public decimal JobID { get; set; }

        public string PackageTypeCode { get; set; }

        public DateTime MfgPackDate { get; set; }

        public DateTime ExpPackDate { get; set; }

        public string NextLevelCode { get; set; }

        public bool? IsRejected { get; set; }

        public string Reason { get; set; }

        public string BadImage { get; set; }

        public string SSCC { get; set; }

        public bool? SSCCVarificationStatus { get; set; }

        public bool? IsManualUpdated { get; set; }

        public string ManualUpdateDesc { get; set; }

        public decimal? CaseSeqNum { get; set; }

        public decimal? OperatorId { get; set; }

        public string Remarks { get; set; }

        public bool? IsDecomission { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string LineCode { get; set; }

        public bool? SYNC { get; set; }

        public int? RCResult { get; set; }

        public bool? DavaPortalUpload { get; set; }

        public bool? IsUsed { get; set; }

        public bool? IsLoose { get; set; }

        public string TwoDGrade { get; set; }

        public int? FirstDeckCount { get; set; }

        public string CryptoCode { get; set; }
        public string PublicKey { get; set; }
    }

}