using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Providers
{
    public class M_Providers
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "RolesIndexName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        [Display(Name = "RolesIndexIsActive", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public string Code { get; set; }

    }
}