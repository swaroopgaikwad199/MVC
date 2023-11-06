using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.Models.Job
{
    public class JobTransferViewModel
    {
        public int JID { get; set; }

        public string JobName { get; set; }

        public string ProductName { get; set; }

        public string BatchNo { get; set; }

        public int TransferToLine { get; set; }

    }
}