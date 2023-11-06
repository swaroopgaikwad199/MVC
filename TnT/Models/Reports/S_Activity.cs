using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Reports
{
    public class S_Activity
    {
        [Key]
        public int Id { get; set; }

        public string Activity { get; set; }

        public string Type { get; set; }
        public string ActivityGerman { get; set; }

    }
}