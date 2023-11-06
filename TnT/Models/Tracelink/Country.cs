using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.Models.Tracelink
{
    public class Country
    {
        public int Id { get; set; }

        public string CountryName { get; set; }

        public string TwoLetterAbbreviation { get; set; }

        public string ThreeLetterAbbreviation { get; set; }
    }
}