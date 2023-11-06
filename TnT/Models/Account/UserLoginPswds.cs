using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.Account
{
    public class UserLoginPswds
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("users")]
        public decimal UserId { get; set; }

        [StringLength(250)]
        public string Password { get; set; }
        public DateTime LastUpdated { get; set; }
        public virtual Users users { get; set; }
    }
}