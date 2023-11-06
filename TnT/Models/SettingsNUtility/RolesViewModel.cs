using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.SettingsNUtility
{
    public class RolesViewModel
    {
        public int? ID { get; set; }

        [Required]
        [Display(Name = "ProductName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Roles_Name { get; set; }

       
        [Display(Name = "UserRemark", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Remarks { get; set; }

        [Display(Name = "RolesCreatePermissions", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public List<Permissions> Permissions { get; set; }
        
    }
}