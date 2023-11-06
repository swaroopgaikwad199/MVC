using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TnT.Models.Job;

namespace TnT.Models.AdditionBatchQty
{
    public class AdditionBatchQty
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("jb")]
        public decimal JID { get; set; }
      [Display(Name = "jobCurrentBatchQty",ResourceType =typeof(LangResource.GlobalRes))]
        public int CurrentBatchQty { get; set; }
        [Display(Name = "jobRequiredBatchQty" ,ResourceType = typeof(LangResource.GlobalRes))]
        public int RequiredBatchQty { get; set; }

        public decimal CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal? VerifiedBy { get; set; }

        public DateTime? VerifiedDate { get; set; }
        [Display(Name = "LineLocationLineCode",ResourceType =typeof(LangResource.GlobalRes))]
        public string LineCode { get; set; }

        public virtual TnT.Models.Job.Job jb { get; set; }
    }
}