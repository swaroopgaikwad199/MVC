using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TnT.Models.SettingsNUtility
{
    public class RoleAlarms
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Roles")]
        public int Role_ID { get; set; }

        [ForeignKey("Alarms")]
        public decimal Alarm_ID { get; set; }

        public virtual Alarms Alarms { get; set; }

        public virtual Roles Roles { get; set; }
    }
}