using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TnT.Models.Customer;
using TnT.Models.Providers;

namespace TnT.Models.Crypto
{
    public class M_CryptOrder
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }      

        [StringLength(50)]
        public string OrderId { get; set; }

        public string SubjectId { get; set; }

        public int TLRequestId { get; set; }

        [StringLength(50)]
        public string GTIN { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ReceivedQuantity { get; set; }

        public int Quantity { get; set; }

        public int Status { get; set; }

        public long CodeGenTime { get; set; }

        public virtual M_Customer Customer { get; set; }
    }


    public  class X_CryptOrder
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Master")]
        public int CryptoMasterID { get; set; }

        public string SerialNo { get; set; }

        public bool CodeType { get; set; }

        public string CryptoCode { get; set; }

        public bool IsUsed { get; set; }

        public virtual M_CryptOrder Master { get; set; }
    }

    public class Utilisation
    {
        [Display(Name = "Batch No")]
        public int JobId { get; set; }
    }

    public class Aggregation
    {
        [Display(Name = "Batch No")]
        public int JobId { get; set; }
    }
    public class DropOut
    {
        [Display(Name = "Batch No")]
        public int JobId { get; set; }
    }
}