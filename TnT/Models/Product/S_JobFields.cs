using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.Product
{
    public class S_JobFields
    {
        public S_JobFields()
        {
            IsChecked = false;
        }
        [Key]
        public int Id { get; set; }


        public string FieldCode { get; set; }

        public string FieldName { get; set; }

        public bool IsActive { get; set; }


        [NotMapped]
        [Display(Name = "Allow")]
        public bool IsChecked { get; set; }
    }


    public class X_JobTypeFields
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("jobType")]
        public decimal JobTypeId { get; set; }

        [ForeignKey("field")]
        public int JobFieldId { get; set; }

        public virtual S_JobFields field { get; set; }
        public virtual JOBType jobType { get; set; }


    }

}