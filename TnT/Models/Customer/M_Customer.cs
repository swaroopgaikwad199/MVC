using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TnT.Models.Providers;
using TnT.Models.SettingsNUtility;

namespace TnT.Models.Customer
{
    public class M_Customer
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        
        [Display(Name = "M_VendorCompanyName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "M_VendorContactPerson", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ContactPerson { get; set; }

        [Required]
        [Display(Name = "M_VendorContactNo", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ContactNo { get; set; }

        [Required]
        [Display(Name = "M_VendorEmail", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "M_VendorAddress", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Address { get; set; }

        [ForeignKey("M_Country")]
        [Display(Name = "SOMDetailsCountry", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int? Country { get; set; }


        [DefaultValue(true)]
        [Display(Name = "ProductIsActive", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool IsActive { get; set; }


        [Display(Name = "M_VendorAPIURL", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string APIUrl { get; set; }



        
        [Display(Name = "M_VendorKey", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string APIKey { get; set; }


        
        [Display(Name = "M_CustomerSenderId", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string SenderId { get; set; }


       
        [Display(Name = "M_CustomerReceiverId", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ReceiverId { get; set; }

        [Required]
        [Display(Name = "M_CustomerCreatedOn", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public DateTime CreatedOn { get; set; }

        [Required]
        [Display(Name = "M_CustomerLastModified", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public DateTime LastModified { get; set; }

        [Required]
        [Display(Name = "M_CustomerCreatedBy", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int CreatedBy { get; set; }

        [Required]
        [Display(Name = "M_CustomerModifiedBy", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int ModifiedBy { get; set; }

        [Required]
        [Display(Name = "M_CustomerIsDeleted", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool IsDeleted { get; set; }

        [Display(Name = "M_CustomerIsSSCC", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool IsSSCC { get; set; }

        [ForeignKey("Proivder")]
        [Display(Name = "M_CustomerProviderId", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int? ProviderId { get; set; }

        [Display(Name = "M_CustomerCompanyCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string CompanyCode { get; set; }

        [Display(Name = "M_CustomerBizLocGLN", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string BizLocGLN { get; set; }

        [Display(Name = "M_CustomerBizLocGLN_Ext", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string BizLocGLN_Ext { get; set; }

        [ForeignKey("M_State")]
        [Display(Name = "SOMDetailsStateOrRegion", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int stateOrRegion { get; set; }
 
        [Display(Name = "SOMEditCity", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string city { get; set; }
      
        [Display(Name = "SOMDetailsPostalCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string postalCode { get; set; }

        [Display(Name = "SettingsLicense", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string License { get; set; }

        [Display(Name = "SettingsLicenseState", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string LicenseState { get; set; }

        [Display(Name = "SettingsLicenseAgency", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string LicenseAgency { get; set; }

     
        [Display(Name = "M_CustomerStreetLine1", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string street1 { get; set; }

     
        [Display(Name = "M_CustomerStreetLine2", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string street2 { get; set; }

 
        [Display(Name = "M_CustomerSFTPAddress", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Host { get; set; }

       
        [Display(Name = "M_CustomerSFTPUser", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string HostUser { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "M_CustomerSFTPPassword", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string HostPswd { get; set; }

        [Display(Name = "M_CustomerSFTPPort", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int HostPort { get; set; }

        [Display(Name = "M_CustomerSToCorpID", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ToCorpID { get; set; }

        public string SSCCExt { get; set; }
        public bool? IsProvideCodeForMiddleDeck { get; set; }

        public string LoosExt { get; set; }

        public virtual M_Providers Proivder { get; set; }

        public virtual Country M_Country  { get; set; }

        public virtual S_State M_State { get; set; }
        [Display(Name ="Filter Value")]
        public string FilterValue { get; set; }
    }
}