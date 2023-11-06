using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.EPCIS
{
    public class ShipmentViewModel
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        public string UUID { get; set; }

        public string EventType { get; set; }

        public int BizStepId { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? EventTime { get; set; }

        public DateTime? RecordTime { get; set; }

        public string EventTimeZoneOffset { get; set; }

        public string ParentID { get; set; }

        public string ChildEPC { get; set; }

        public string EpcList { get; set; }

        public string Action { get; set; }

        public string BizStep { get; set; }

        public string Disposition { get; set; }

        public string ReadPoint { get; set; }

        public string BizLocation { get; set; }

        public string BizTransactionList { get; set; }

        public string ExtensionData1 { get; set; }

        public string ExtensionData2 { get; set; }

        public string UserData1 { get; set; }

        public string UserData2 { get; set; }

        public string UserData3 { get; set; }

        public double EpcisVersion { get; set; }
        public string DocumentType1 { get; set; }
        public string DocumentType2 { get; set; }
        public string DocumentType3 { get; set; }
        public string DocumentDetail1 { get; set; }
        public string DocumentDetail2 { get; set; }
        public string DocumentDetail3 { get; set; }

    }
}