using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Reports
{
    public class RptUserViewModel
    {
        [Key]
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }
        public List<UDetail> UsersDetail { get; set; }

        public bool ReportType { get; set; }
    }

    public class UDetail
    {
        public string UserName { get; set; }

        public string UserId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated{ get; set; }
        [Key]
        public string RoleName { get; set; }
    }
}