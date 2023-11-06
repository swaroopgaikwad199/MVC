namespace TnT.Models.LblLytDsg
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class S_DPI
    {
        [Key]      
        public int id { get; set; }
               
       
        [StringLength(10)]
        public string dpi { get; set; }
    }
}
