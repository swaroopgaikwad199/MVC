using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.SettingsNUtility
{
    public class Settings
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "SettingsIndexCompanyInfoCompanyName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string  CompanyName { get; set; }

        [Required]
        [Display(Name = "SettingsIndexCompanyInfoAddress", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Address { get; set; }

        [Display(Name = "SettingsLogo", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Logo { get; set; }

        [Display(Name = "SettingsCompanyCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string  CompanyCode { get; set; }

        [Display(Name = "LineLocationLineCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string LineCode { get; set; }

        [Display(Name = "UserRemark", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Display(Name = "LineLocationPlantCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string PlantCode { get; set; }

        [Display(Name = "SettingsIAC_CIN", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string IAC_CIN { get; set; }

        [Display(Name = "SOMDetailsGLN", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string GLN { get; set; }

        [Display(Name = "SOMDetailsStreet", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Street { get; set; }
         
        [Display(Name = "SOMDetailsStateOrRegion", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [ForeignKey("M_State")]
        public int StateOrRegion { get; set; }

        [Display(Name = "SOMDetailsCity", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string City { get; set; }

        [Display(Name = "SOMDetailsPostalCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string PostalCode { get; set; }

        [Display(Name = "SOMDetailsCountry", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [ForeignKey("M_Country")]
        public int Country { get; set; }

        [Display(Name = "SettingsLicense", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string License { get; set; }

        [Display(Name = "SettingsLicenseState", ResourceType = typeof(TnT.LangResource.GlobalRes))]

        public string LicenseState { get; set; }

        [Display(Name = "SettingsLicenseAgency", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string LicenseAgency { get; set; }
        [Display(Name = "SettingDistrict",ResourceType =typeof(TnT.LangResource.GlobalRes))]
        public string District { get; set; }
        public virtual Country M_Country { get; set; }

        public virtual S_State M_State { get; set; }


    }
}