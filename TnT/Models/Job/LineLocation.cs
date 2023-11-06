using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.Job
{
    public class LineLocation
    {
       
        [Key]
       
        public string ID { get; set; }

        [Display(Name = "LineLocationLocationCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string LocationCode { get; set; }

        [Display(Name = "LineLocationDivisionCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string DivisionCode { get; set; }

        [Display(Name = "LineLocationPlantCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string PlantCode { get; set; }

        [Display(Name = "LineLocationLineCode", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string LineCode { get; set; }

        [Display(Name = "LineLocationLineIp", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string LineIP { get; set; }

        [Display(Name = "LineLocationServerName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string ServerName { get; set; }

        [Display(Name = "LineLocationDBName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string DBName { get; set; }

        [Display(Name = "LineLocationSQLUserName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string SQLUsername { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "LineLocationSQLPassword", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string SQLPassword { get; set; }

        [Display(Name = "LineLocationIsActive", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public bool IsActive { get; set; }

        [Display(Name = "LineLocationLineName", ResourceType = typeof(TnT.LangResource.GlobalRes))]
        public string LineName { get; set; }

        [Display(Name = "Read GLN")]
        public string ReadGLN { get; set; }

        [Display(Name = "GLN Extension")]
        public string GLNExtension { get; set; }

        [NotMapped]
        public string Fullname
        {
            get
            {
                return string.Format("{0} {1}", PlantCode, LineCode);
            }
        }
        public bool AllowMultipleBatches { get; set; }

    }
}