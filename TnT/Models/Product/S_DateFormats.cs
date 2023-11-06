using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Product
{
    public class S_DateFormats
    {
        [Key]
        public int Id { get; set; }
        public string Format { get; set; }
    }
}