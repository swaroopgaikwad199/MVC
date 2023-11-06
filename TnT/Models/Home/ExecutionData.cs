using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Home
{
    public class ExecutionData
    {
        [Key]
        public int ID { get; set; }

        public string LineCode { get; set; }

        public string ProductName { get; set; }

        public string JobName { get; set; }

        public int Quantity { get; set; }

        public string PackagingLevel { get; set; }

        public DateTime LastUpdatedDate { get; set; }

        public string BatchNo { get; set; }

        public int JobStatus { get; set; }

    }

    public class LineIdelTime
    {
        [Key]
        public int ID { get; set; }

        public string LineCode { get; set; }
        
        public string JobName { get; set; }
        
        public string BatchNo { get; set; }

        public string IdlTime { get; set; }
    }
}