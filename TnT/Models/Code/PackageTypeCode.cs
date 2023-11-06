using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.Models.Code
{
    public class PackageTypeCode
    {
        public string Code { get; set; }

        public int CodeSeq { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Remarks { get; set; }
    }
}