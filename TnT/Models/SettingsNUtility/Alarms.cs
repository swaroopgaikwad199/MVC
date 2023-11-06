using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.SettingsNUtility
{
    public class Alarms
    {
        public Alarms()
        {
            IsChecked = false;
        }

        [Key]
        public decimal ID { get; set; }

        public string Aname { get; set; }


        [NotMapped]
        [Display(Name = "Allow")]
        public bool IsChecked { get; set; }

        public bool IsActive { get; set; }
    }
}