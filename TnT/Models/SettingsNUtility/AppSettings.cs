using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.SettingsNUtility
{
    public class AppSettings
    {
        [Key]
        public int Id { get; set; }      

        [Required]
        public string Key { get; set; }

        [Required]
        [Display(Name = "UsersPasswordExpiryDays", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Value { get; set; }
    }
}