using REDTR.DB.BLL;
using REDTR.HELPER;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.DataLayer.JobService;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.PushToGlobal;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class PushToGlobalController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        // GET: PushToGlobal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCloseJob()
        {
            DbHelper m_dbhelper = new DbHelper();
            string Query = "select * from Job" +
        " where JID in (Select distinct JobID from PackagingDetails where PackagingDetails.SSCC is not null and(PackagingDetails.DavaPortalUpload = 1) and (PackagingDetails.IsDecomission = 0 or PackagingDetails.IsDecomission is null) and PackagingDetails.IsRejected is not null )  and Job.JobStatus = 3 and (TransferToGlobal=0 or TransferToGlobal is null) and (ProviderId=2 or ProviderId=4)";
            DataSet ds = m_dbhelper.GetDataSet(Query);
            var joblst = ds.Tables[0].AsEnumerable().Select(dataRow => new PushToGlobalViewModel { Jid = dataRow.Field<decimal>("JID"),Jobname=dataRow.Field<string>("JobName"),Qty=dataRow.Field<int>("Quantity"),BatchNo=dataRow.Field<string>("BatchNo") }).ToList();
            return Json(joblst);
        }

        public ActionResult TrasferJob(int id,string name)
        {
            GlobalServerDataTransferHelper hlpr = new GlobalServerDataTransferHelper();
            var job = db.Job.Where(x => x.JID == id).FirstOrDefault();
            var response = hlpr.TranferToGLobal(id, name,Convert.ToInt32(User.ID));
            if (response==true)
            {
               
                
                
                if(job!=null)
                {
                    job.TransferToGlobal = true;
                    db.SaveChanges();
                }
                trail.AddTrail(User.FirstName+" "+job.JobName+TnT.LangResource.GlobalRes.TempDataTrabsferBatchtoGloble, Convert.ToInt32(User.ID), User.FirstName + " " + job.JobName + TnT.LangResource.GlobalRes.TempDataTrabsferBatchtoGloble, TnT.LangResource.GlobalRes.TrailActionBatchActivity);
            
            }
            else
            {
                
                if (job != null)
                {
                    job.TransferToGlobal = false;
                    db.SaveChanges();
                }
            }
            return Json(response);
        }
    }
}