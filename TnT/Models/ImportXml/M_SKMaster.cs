using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.ImportXml
{
    public class M_SKMaster
    {
        [Key]
        public int MID { get; set; }

        public string ReceivingSystem { get; set; }

        public string ActionCode { get; set; }

        public string IDType { get; set; }

        public string NumberFrom { get; set; }

        public string NumberTo { get; set; }

        public string EncodingType { get; set; }

        public bool IsUsed { get; set; }

        public decimal NFrom { get; set; }

        public decimal Nto { get; set; }
    }
}