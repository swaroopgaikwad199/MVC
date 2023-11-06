using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TnT.Models.Customer;
using TnT.Models.Providers;

namespace TnT.Models.Code
{
    public class M_Identities
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }      

        [StringLength(50)]
        public string GTIN { get; set; }

        [StringLength(50)]
        public string PPN { get; set; }

        public DateTime CreatedOn { get; set; }

        public string PackageTypeCode { get; set; }

        public virtual M_Customer Customer { get; set; }

        public decimal JID { get; set; }

        public bool IsExtra { get; set; }

        public bool IsTransfered { get; set; }

        public string NTIN { get; set; }
    }


    public  class X_Identities
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Master")]
        public int MasterId { get; set; }

        [StringLength(50)]
        public string SerialNo { get; set; }

        //CodeType ::  0=UID 1 = SSCC

        public bool CodeType { get; set; }

        [StringLength(3)]
        public string PackTypeCode { get; set; }

        public bool IsTransfered { get; set; }

        public string CryptoCode { get; set; }
        public string PublicKey { get; set; }

        public virtual M_Identities Master { get; set; }

    }
}