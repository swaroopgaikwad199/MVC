using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.EPCIS
{
    public class EpcisEventDetails
    {
        [Key]
        public int Id { get; set; }

        public int JobId { get; set; }

        [StringLength(50)]
        public string UUID { get; set; }

        [StringLength(50)]
        public string EventType { get; set; }

        public int BizStepId { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? EventTime { get; set; }

        public DateTime? RecordTime { get; set; }

        [StringLength(50)]
        public string EventTimeZoneOffset { get; set; }

        [StringLength(100)]
        public string ParentID { get; set; }

        [Column(TypeName = "text")]
        public string ChildEPC { get; set; }

        [Column(TypeName = "text")]
        public string EpcList { get; set; }

        [StringLength(50)]
        public string Action { get; set; }

        [StringLength(50)]
        public string BizStep { get; set; }

        [StringLength(50)]
        public string Disposition { get; set; }

        [StringLength(100)]
        public string ReadPoint { get; set; }

        [StringLength(100)]
        public string BizLocation { get; set; }

        [Column(TypeName = "text")]
        public string BizTransactionList { get; set; }

        [Column(TypeName = "text")]
        public string ExtensionData1 { get; set; }

        [Column(TypeName = "text")]
        public string ExtensionData2 { get; set; }

        [Column(TypeName = "text")]
        public string UserData1 { get; set; }

        [Column(TypeName = "text")]
        public string UserData2 { get; set; }

        [Column(TypeName = "text")]
        public string UserData3 { get; set; }

        public double? EpcisVersion { get; set; }
    }
}