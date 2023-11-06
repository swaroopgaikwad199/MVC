using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.AS2
{
    public class UploadViewModel
    {
        [Required]
        public int ServerId { get; set; }
    }
}