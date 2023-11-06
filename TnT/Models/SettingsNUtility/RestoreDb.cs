using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.SettingsNUtility
{
    public class RestoreDb
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public decimal ReqID { get; set; }
        [Display (Name = "lblLytDsgFileName" ,ResourceType =typeof(LangResource.GlobalRes))]
        public string FileName { get; set; }
       
        public decimal Createdby { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal? VerifiedBy { get; set; }

        public DateTime? VeifiedDate { get; set; }
    }
}