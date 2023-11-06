using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TnT.Models;
using TnT.Models.Product;

namespace TnT.DataLayer
{
    public class ComplianceManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<S_JobFields> getComplianceData(decimal jobTypeId, bool IsNHNR)
        {
            if (jobTypeId == 20060)
            {
                if (IsNHNR == true)
                {
                    var data = from x in db.X_JobTypeFields join jt in db.JOBTypes on x.JobTypeId equals jt.TID join s in db.S_JobFields on x.JobFieldId equals s.Id where jt.TID == jobTypeId select s;
                    return data.ToList();
                }
                else
                {
                    var data = from x in db.X_JobTypeFields join jt in db.JOBTypes on x.JobTypeId equals jt.TID join s in db.S_JobFields on x.JobFieldId equals s.Id where jt.TID == jobTypeId && s.FieldCode != "NHRN" select s;
                    return data.ToList();
                }
            }
            else
            {
                var data = from x in db.X_JobTypeFields join jt in db.JOBTypes on x.JobTypeId equals jt.TID join s in db.S_JobFields on x.JobFieldId equals s.Id where jt.TID == jobTypeId select s;
               

                return data.ToList();
            }
        }
    }
}