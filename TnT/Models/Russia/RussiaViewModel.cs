using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.Russia
{
    public class RussiaViewModel
    {
        [Key]
        public int JID { get; set; }

        public string FileType { get;set; }

        public string SSCC { get; set; }

        public string subject_id { get; set; }

        public DateTime operation_date { get; set; }

        public string packing_id { get; set; }

        public string control_id { get; set; }

        public string series_number { get; set; }

        public string seller_id { get; set; }

        public string receiver_id { get; set; }

        public string custom_receiver_id { get; set; }

        public string contract_type { get; set; }

        public string doc_num { get; set; }

        public DateTime doc_date { get; set; }

        public string shipper_id { get; set; }

        public string operation_type { get; set; }

    }
}