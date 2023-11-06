using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Vendor.ViewModels
{
    public class NotifyViewModel
    {

        [Required]
        [Display(Name = "NotifyViewVendor", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int Id { get; set; }

        [Required]
        [Display(Name = "NotifyViewJob", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public int JID { get; set; }

    }
}