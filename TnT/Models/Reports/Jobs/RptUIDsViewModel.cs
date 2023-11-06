using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.Models.Code;
using TnT.Models.Job;
using TnT.Models.Product;

namespace TnT.Models.Reports.Jobs
{
    public class RptUIDsViewModel
    {
       
        public string CompanyName { get; set; }

        public string Address { get; set; }

       
        public string UserName { get; set; }
        public string DivisionCode { get; set; }

        public string Jobtype { get; set; }
        public PackagingAsso product { get; set; }

      

        public PackagingAssoDetails packagingAsso { get; set; }

        public JobDetails jbDetails { get; set; }
        public LineLocation LineDetails { get; set; }

        public TnT.Models.Job.Job job { get; set; }

        public List<PackagingDetails> packagingDetails { get; set; }

        public List<UIDFailReason> UIDFailReasons { get; set; }


        public string Packinglevel { get; set; }

        public string Reason { get; set; }
        public IEnumerable<SelectListItem> Packaginlevels { get; set; }

        public List<UIDFailReasonDetails> UIDFailReasonsDetails { get; set; }

    }

    public class UIDFailReason
    {
        public string status { get; set; }

        public string FailedID { get; set; }
        public string Code { get; set; }
        public string LastUpdatedDate { get; set; }


    }

    public class UIDFailReasonDetails
    {
        public List<status> status { get; set; }
        public List<string> FailedIds { get; set; }
        public string Code { get; set; }
    }

    public class status
    {
        public string stat { get; set; }
        public string feild { get; set; }
    }
}