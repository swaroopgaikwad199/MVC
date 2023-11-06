using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.SettingsNUtility
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string CountryName { get; set; }
        public string TwoLetterAbbreviation { get; set; }
        public string ThreeLetterAbbreviation { get; set; }

    }
}