using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.Models.DAVAPortal
{
    public class SSCCsViewModel
    {

        public List<SSCCDetails> SSCCs { get; set; }
        public int BatchQuatityScanned { get; set; }

        public string  BatchType { get; set; }

        public int ActualBatchQty { get; set; }
        public int ExportBatchQty { get; set; }
        public int RemainingBatchQty { get; set; }

    }
    public class SSCCDetails
    {
        public string SSCC { get; set; }

        public bool Active { get; set; }

        public int PrimaryPackQty { get; set; }
    }
}