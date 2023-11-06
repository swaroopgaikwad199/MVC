using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Job
{
    public class JobDetails
    {
        [Key]
        public int Id { get; set; }

        public decimal JD_JobID { get; set; }

        public string JD_ProdName { get; set; }

        public string JD_ProdCode { get; set; }

        [Required]
        public string JD_Deckcode { get; set; }

        public string JD_PPN { get; set; }

        [Required]
        public string JD_GTIN { get; set; }

        [Required]
        public int JD_DeckSize { get; set; }

        public decimal JD_MRP { get; set; }

        public string JD_Description { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string LineCode { get; set; }

        public bool SYNC { get; set; }

        public string GTINCTI { get; set; }

        public int BundleQty { get; set; }

        public string JD_FGCode { get; set; }

        public string LabelName { get; set; }

        public string Filter { get; set; }

        public string JD_NTIN { get;set; }


    }
}