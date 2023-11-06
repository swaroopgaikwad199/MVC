using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.AS2
{   
    public class M_ServersAS2
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        [Display (Name = "As2ServerIndexHostAddress",ResourceType =typeof(LangResource.GlobalRes))]
      
        public string HostAddress { get; set; }

        [Required]
        [Display(Name = "Host Port")]
        public int HostPort { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "From Name")]
        public string FromName { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "To Name")]
        public string ToName { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Host PublicKeyPath")]
        public string HostPublicKeyPath { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Self PublicKeyPath")]
        public string SelfPublicKeyPath { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Self PrivateKeyPath")]
        public string SelfPrivateKeyPath { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(300)]
        [Display(Name = "Self PrivateKeyPassword")]
        public string SelfPrivateKeyPassword { get; set; }

        [Display(Name = "ProductIsActive",ResourceType =typeof(LangResource.GlobalRes))]
        public bool IsActive { get; set; }


    }
}