using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.SettingsNUtility
{
    public class AlarmsViewModel
    {
        public int? ID { get; set; }

        [Required]
        [Display(Name = "ProductName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Roles_Name { get; set; }

        public List<Alarms> Alarms { get; set; }
    }
}