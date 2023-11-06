using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.Models.DOMaster
{
    public class DOMaster
    {
       public int DONo { get; set; }
       public string LabelName { get; set; }
       public string PCMAP { get; set; }
       public int DO_Qty { get; set; }

       public string Batches { get; set; }
    }
}