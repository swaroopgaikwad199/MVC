using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.ImportXml
{
    public class SK_ObjectKey
    {
        [Key]
        public int OID { get; set; }

        public int MID { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsUsed { get; set; }
    }
}