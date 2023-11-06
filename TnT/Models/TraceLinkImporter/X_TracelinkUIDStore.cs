using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TnT.Models.TraceLinkImporter
{
    public class X_TracelinkUIDStore
    {
        [Key]
        public int Id { get; set; }


        public int TLRequestId { get; set; }

        public string SerialNo { get; set; }

        public string GTIN { get; set; }

        public string Type { get; set; }

        public string CryptoCode { get; set; }

        public bool IsUsed { get; set; }
        public string PublicKey { set; get; }
    }
}