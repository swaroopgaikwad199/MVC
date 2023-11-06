using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TnT.Models.PushToGlobal
{
    public class PushToGlobalViewModel
    {
        [Key]
        public decimal Jid { get; set; }

        public string Jobname { get; set; }

        public int Qty { get; set; }

        public string BatchNo { get; set; }
    }

 
}