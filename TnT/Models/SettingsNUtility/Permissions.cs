using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.SettingsNUtility
{
    public class Permissions
    {
        public Permissions()
        {
            IsChecked = false;
        }
        [Key]
        public decimal ID { get; set; }

        [Display(Name = "Permissions", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Permission { get; set; }

        public string Remarks { get; set; }

        [NotMapped]
        [Display(Name ="Allow")]
        public bool IsChecked { get; set; }

        public bool IsActive { get; set; }

    }
}