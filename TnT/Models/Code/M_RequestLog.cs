using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Code
{
    public class M_RequestLog
    {
        public M_RequestLog()
        {
            RequestDate = DateTime.Now;
            IsExtraRequested = false;
            IsSynced = false;
        }

        [Key]
        public int id { get; set; }

        [Required]
        public string ServiceKey { get; set; }

        [Required]
        public int JobId { get; set; }

        [Required]
        public DateTime MfgDate { get; set; }

        [Required]
        public DateTime ExpDate { get; set; }

        [Required]
        public string BatchNo { get; set; }


        public int Quantity { get; set; }

        
        public string LineId { get; set; }

        public string SAPCode { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        [Required]
        public bool IsExtraRequested { get; set; }

        [Required]
        public bool IsSynced { get; set; }

        public DateTime? SyncedDate { get; set; }

        public bool IsReceived { get; set; }

        public string  AcknowldgeMessage { get; set; }

        public DateTime? AcknowldgeDtTm { get; set; }

    }
}