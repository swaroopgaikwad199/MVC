using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Product
{
    public class PackagingLevels
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Level { get; set; }

        public bool IsActive { get; set; }

    }
}