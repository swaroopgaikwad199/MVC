using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.EPCIS
{
    public class M_Transporter
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "EpcisTransporterName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "EpcisTransporterContactNo", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ContactNo { get; set; }

        [Required]
        [Display(Name = "M_VendorEmail", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string EmailId { get; set; }

        [Required]
        [Display(Name = "EpcisTransporterAddress", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Address { get; set; }
    }
}