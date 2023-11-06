using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.TraceLinkImporter
{
    public class M_SOM
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }


        [StringLength(30)]
        public string BusinessId { get; set; }

        [Display(Name = "SOMIndexBusinessName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [StringLength(50)]
        public string BusinessName { get; set; }

        [StringLength(50)]
        public string Street1 { get; set; }

        [StringLength(30)]
        public string City { get; set; }

        [StringLength(30)]
        public string StateOrRegion { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [Display(Name = "SOMIndexCountry", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [StringLength(20)]
        public string Country { get; set; }



        //
        [Display(Name = "SOMDetailsGLN", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [StringLength(30)]
        public string FacilityId_GLN { get; set; }

        [Display(Name = "SOMDetailsGLN", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [StringLength(30)]
        public string FacilityId_SGLN { get; set; }

        // SFLI = shipment Location Info
        [Display(Name = "SOMDetailsBusinessName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [StringLength(50)]
        public string SFLI_BusinessName { get; set; }

        [Display(Name = "SOMEditStreet1", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [StringLength(50)]
        public string SFLI_Street1 { get; set; }

        [Display(Name = "SOMEditCity", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [StringLength(30)]
        public string SFLI_City { get; set; }

        [Display(Name = "SOMDetailsStateOrRegion", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [StringLength(30)]
        public string SFLI_StateOrRegion { get; set; }

        [Display(Name = "SOMDetailsPostalCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [StringLength(10)]
        public string SFLI_PostalCode { get; set; }

        [Display(Name = "SOMDetailsCountry", ResourceType = typeof(TnT.LangResource.GlobalRes))]

        [StringLength(20)]
        public string SFLI_Country { get; set; }


        [Display(Name = "SOMDeliveryNumber", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string DeliveryNumber { get; set; }

        [Display(Name = "SOMDeliveryCompleteFlag", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool DeliveryCompleteFlag { get; set; }

        [Display(Name = "SOMTransactionIdentifier", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string TransactionIdentifier { get; set; }

        [Display(Name = "SOMTransactionDate", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "SOMSalesDistributionType", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string SalesDistributionType { get; set; }

        [Display(Name = "SOMIsSerialized", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool IsSerialized { get; set; }

        [Display(Name = "SOMFromBusinessPartyLookupId", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string FromBusinessPartyLookupId { get; set; }

        [Display(Name = "SOMShipFromLocationLookupId", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ShipFromLocationLookupId { get; set; }

        [Display(Name = "SOMToBusinessPartyLookupId", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ToBusinessPartyLookupId { get; set; }

        [Display(Name = "SOMShipToLocationLookupId", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ShipToLocationLookupId { get; set; }

  
    }
}