using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Account
{
    public class License
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LicenseNo { get; set; }

    }
}