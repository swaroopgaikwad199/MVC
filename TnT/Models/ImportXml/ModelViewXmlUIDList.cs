using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.ImportXml
{
    public class ModelViewXmlUIDList
    {
        [Key]

        [Display(Name ="Batch Name")]
        public decimal JobID { get; set; }

        [Display(Name = "Deck")]
        public string Deck { get; set; }

        [Display(Name = "Number of records per file")]
        public decimal iNoofRecPerFile { get; set; }

        [Display(Name = "Reader Id")]
        public string StrReaderId { get; set; }

        [Display(Name = "Command Id")]
        public string StrCommand { get; set; }

        [Display(Name = "Device Id")]
        public string StrDeviceId { get; set; }

        [Display(Name = "UOM")]
        public string StrUOM { get; set; }
    }
}