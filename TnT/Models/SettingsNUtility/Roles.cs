using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.SettingsNUtility
{
    public class Roles
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "ProductName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Roles_Name { get; set; }

        public string ADRole { get; set; }

        [Required]
        [Display(Name = "UserRemark", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Remarks { get; set; }

        [Display(Name = "RolesIndexIsActive", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool IsActive { get; set; }

    }
}