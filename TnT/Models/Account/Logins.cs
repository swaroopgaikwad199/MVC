using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Account
{
    public class Logins
    {
        [Key]
        public decimal UserId { get; set; }
        public string SessionId { get; set; }
        public bool LoggedIn { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}