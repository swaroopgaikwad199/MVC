using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.SettingsNUtility
{
    public class S_State
    {
        [Key]

        public int ID { get; set; }
        public string StateName { get; set; }
        public int CountryID { get; set; }
        public int displayorder { get; set; }
        public string TwoLetterAbbreviation { get; set; }
    }
}