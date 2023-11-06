using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Account
{
    public class M_UserPasswords
    {
        [Key]
        public int Id { get; set; }

        public decimal UserId { get; set; }

        public string PasswordOld { get; set; }

        public string PasswordOlder { get; set; }

        public string PasswordOldest { get; set; }
    }
}