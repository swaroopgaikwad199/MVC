using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.TraceKey
{
    public class TraceKeyViewModel
    {
        [Key]
        public int JID { get; set; }

        public int Cid { get; set; }

        public string ProductionDocument { get; set; }

        public string TypeVersion { get; set; }
    }

    public class TraceKeyImportUIDViewModel
    {
        [Display(Name = "ImporterTracelinkProduct", ResourceType = typeof(LangResource.GlobalRes))]
        public int PAID { get; set; }

        public string GTIN { get; set; }
        public int Customer { get; set; }
        [Display(Name = "ImporterTracelinkQuantity", ResourceType = typeof(LangResource.GlobalRes))]
        public int Quantity { get; set; }
    }

    public class M_TkeySerialRequest
    {
        [Key]
        public int ID { get; set; }
        public string GTIN { get; set; }
        [Display(Name = "ImporterTracelinkQuantity", ResourceType = typeof(LangResource.GlobalRes))]
        
        public int Quantity { get; set; }
        public string docVersion { get; set; }
        public string docType { get; set; }
        [Display(Name = "M_CustomerCreatedOn", ResourceType = typeof(LangResource.GlobalRes))]
       
        public DateTime? CreatedOn { get; set; }
        public string TicketId { get; set; }
        public string ResponseStatus { get; set; }
        public string Message { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string docIdentifier { get; set; }
        public int Status { get; set; }
        public int CustomerId { get; set; }
        public int ProviderId { get; set; }
        public int TLRequestId { get; set; }
    }

    public enum TKEY_SERIAL_REQUEST_STATUS
    {
        REQUEST_ADDED = 1,
        REQUEST_PROCESS = 2,
        REQUEST_DOWNLOADED = 3,
    }
}