using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Exporter
{
    public class ExporterModelView
    {
        [Key]
        [Required]
        public string JobName { get; set; }

        [Required]
        public string PackagingType { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}