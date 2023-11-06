using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.SettingsNUtility
{
    public class ROLESPermission
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal ID { get; set; }

        [ForeignKey("Roles")]
        public int Roles_Id { get; set; }

        [ForeignKey("Permissions")]
        public decimal Permission_Id { get; set; }

        public string Remarks { get; set; }

        public virtual Permissions Permissions { get; set; }

        public virtual Roles Roles { get; set; }
    }
}