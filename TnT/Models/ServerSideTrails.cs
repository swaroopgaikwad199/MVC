using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models
{
    public class ServerSideTrails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime ActitvityTime { get; set; }

        [Required]

        public string Reason { get; set; }

        [Required]
        public string Activity { get; set; }

        public int RoleId { get; set; }
    }
}