using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.Models.Job;
using TnT.Models.Product;

namespace TnT.Models.Reports.Jobs
{
    public class RptPCRelationshipViewModel
    {
        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string UserName { get; set; }

        public string Jobtype { get; set; }
        public PackagingAsso Product { get; set; }

        public JobDetails jbDetails { get; set; }
        public  TnT.Models.Job.Job Job { get; set; }

        public LineLocation LineDetails { get; set; }

        public List<Shippers> ShipperDetails { get; set; }

        public List<string> PkgLevels { get; set; }

        public string TertiaryLevel { get; set; }

        public string TertiaryGTIN { get; set; }

        public List<PCRela> PCRela { get; set; }
    }

    public class Shippers
    {
        public string SSCC { get; set; }

        public string SSCCCode { get; set; }

        public PCRelationsAll PCRelations { get; set; }
    }


    public class PCRelationsAll
    {
      

        public List<string> PALCode { get; set; }


        public List<string> OSHCode { get; set; }

        public List<string> ISHCode { get; set; }

        public List<string> OBXCode { get; set; }

        public List<string> MOCCode { get; set; }

        public List<string> PPBCode { get; set; }

    }

    public class PCRela
    {
        //public List<string> SSCC { get; set; }

        //public List<string> SSCCCode { get; set; }

        //public List<string> PALCode { get; set; }


        //public List<string> OSHCode { get; set; }

        //public List<string> ISHCode { get; set; }

        //public List<string> OBXCode { get; set; }

        //public List<string> MOCCode { get; set; }

        //public List<string> PPBCode { get; set; }


        public string SSCC { get; set; }

        public string SSCCCode { get; set; }

        public string PALCode { get; set; }

        public string OSHCode { get; set; }

        public string ISHCode { get; set; }

        public string OBXCode { get; set; }

        public string MOCCode { get; set; }

        public string PPBCode { get; set; }
        public bool IsUsed { get; set; }

    }




}