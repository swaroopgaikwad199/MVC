using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TnT.Models.Customer;

namespace TnT.Models.TraceLinkImporter
{
    public class M_TracelinkRequest
    {

        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public int Quatity { get; set; }

        public string GTIN { get; set; }

        public DateTime  RequestedOn { get; set; }

        public M_Customer Customer { get; set; }

        public bool IsDeleted { get; set; }

        public int Threshold { get; set; }

        public int? ProviderId { get; set; }

        public string SrnoType { get; set; }

        public string FilterValue { get; set; }

    }
}