using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TnT.Models.SettingsNUtility;

namespace TnT.Models.EPCIS
{
    public class M_EPCISReceiver
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "SettingsIndexCompanyInfoCompanyName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string CompanyName { get; set; }

        [ForeignKey("M_Country")]
        [Required]
        [Display(Name = "SOMDetailsCountry", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int? CountryId { get; set; }

        [ForeignKey("M_State")]
        [Required]
        [Display(Name = "SOMDetailsStateOrRegion", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int? StateId { get; set; }

        [Required]
        [Display(Name = "SOMEditCity", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string City { get; set; }

        [Required]
        [Display(Name = "SOMEditStreet1", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Street1 { get; set; }

        [Required]
        [Display(Name = "EPCISReceiverStreet2", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Street2 { get; set; }

        [Required]
        [Display(Name = "SOMDetailsPostalCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "SOMEditGLN", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string GLN { get; set; }

        [Display(Name = "EpcisReceiversite", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string site { get; set; }

        [Display(Name = "EpcisReceiverstreet3", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string street3 { get; set; }

        [Display(Name = "EPCISReceivercountryCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string countryCode { get; set; }

        [Display(Name = "EpcisReceiverlatitude", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public decimal? latitude { get; set; }

        [Display(Name = "EpcisReceiverlongitude", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public decimal? logitude { get; set; }

        [Display(Name = "EpicsReceiverCreatedby", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int? CreatedBy { get; set; }

        [Display(Name = "LineLocationIsActive", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool IsActive { get; set; }


        [Display(Name = "EpcisReceiverCreatedOn", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public DateTime CreatedOn { get; set; }


        [Display(Name = "EpcisReceiverModifiedOn", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public DateTime LastModified { get; set; }

        [Display(Name = "EpcisReceiveModifiedby", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int? ModifiedBy { get; set; }


        public string Extension { get; set; }

        public virtual Country M_Country { get; set; }

        public virtual S_State M_State { get; set; }
    }
}