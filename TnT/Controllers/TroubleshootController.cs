using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TnT.Models;

namespace TnT.Controllers
{
    public class TroubleshootController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TableDependency
        public ActionResult Index()
        {
            ViewBag.Jobs = db.Job.ToList();
            //ViewBag.FPCodes = db.PackagingAsso.Where(m => m.VerifyProd == true && m.IsActive == true);
            return View();
        }

        [HttpPost]
        public string Create(string PONumber)
        {

            var job = db.Job.FirstOrDefault(x => x.JobName == PONumber);

            Dictionary<string, object> dicparameter = new Dictionary<string, object>();
            dicparameter.Add("JobID", job.JID);
            dicparameter.Add("PAID", job.PAID);
            dicparameter.Add("Flag", 1);
            DBSync dbServer = new DBSync();
            DataSet ds = dbServer.GetDataSet("SP_TableDependency", dicparameter);
            #region UID TO PRINT

            var selectedProduct = db.PackagingAssoDetails.Where(x => x.PAID == job.PAID).OrderBy(s => s.Id).ToList();
            var UIDsToGenrate = job.Quantity + job.SurPlusQty;
            int looseshiper = Convert.ToInt32(Utilities.getAppSettings("LoosShipper"));
            Dictionary<string, int> dicUidToPrint = new Dictionary<string, int>();
            int parentCnt = UIDsToGenrate;
            for (int i = 0; i < selectedProduct.Count(); i++)
            {
                if (i == 0)
                {
                    dicUidToPrint.Add(selectedProduct[i].PackageTypeCode, parentCnt);
                }
                else
                {
                    int loosExtra = (parentCnt % selectedProduct[i].Size);
                    if (loosExtra > 0 && parentCnt < selectedProduct[i].Size)
                    {
                        loosExtra = 0;
                    }

                    float map = (parentCnt / selectedProduct[i].Size);
                    parentCnt = (map == 0) ? 1 : (int)Math.Ceiling(map);

                    parentCnt = parentCnt + ((loosExtra > 0) ? 1 : 0);

                    dicUidToPrint.Add(selectedProduct[i].PackageTypeCode, parentCnt);
                }

            }
            dicUidToPrint.Add("Loose", looseshiper);
            DataTable dt_PCMAP = new DataTable();
            dt_PCMAP.Columns.Add("DeckCode", typeof(string));
            dt_PCMAP.Columns.Add("UIDSToPrint", typeof(int));
            foreach (var deck in dicUidToPrint)
            {
                dt_PCMAP.Rows.Add(new object[] { deck.Key, deck.Value });
            }

            ds.Tables.Add(dt_PCMAP);
            #endregion
            if (ds.Tables[0].Rows.Count == 1)
            {
                ProgressHub.sendMessage("Product Found", 100, 0);

            }
            //int tblPackDtlsno = ds.Tables.Count - 1;
            //if (ds.Tables[tblPackDtlsno].Rows.Count < (job.Quantity + job.SurPlusQty))
            //{
            //    TempData["Error"] = "Pack data not found. Kindly contact Administrator";
            //    return "Batch Qauntity is not same on both side";
            //}
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ds);
            return JSONString;
        }

        [HttpPost]
        public string verify(string PONumber)
        {

            var job = db.Job.FirstOrDefault(x => x.JobName == PONumber);

            Dictionary<string, object> dicparameter = new Dictionary<string, object>();
            dicparameter.Add("JobID", job.JID);
            dicparameter.Add("PAID", job.PAID);
            dicparameter.Add("Flag", 2);
            DBSync dbServer = new DBSync();
            DataSet ds = dbServer.GetDataSet("SP_TableDependency", dicparameter);
            int tblPackDtlsno = ds.Tables.Count - 1;
            if (ds.Tables[tblPackDtlsno].Rows.Count < (job.Quantity + job.SurPlusQty))
            {
                TempData["Error"] = "Pack data not found. Kindly contact Administrator";
                return "Batch Qauntity is not same on both side";
            }
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ds.Tables[4]);
            return JSONString;

        }

        [HttpPost]
        public string Additional(string PONumber)
        {

            var job = db.Job.FirstOrDefault(x => x.JobName == PONumber);

            Dictionary<string, object> dicparameter = new Dictionary<string, object>();
            dicparameter.Add("JobID", job.JID);
            dicparameter.Add("PAID", job.PAID);

            DBSync dbServer = new DBSync();
            DataSet ds = dbServer.GetDataSet("SP_TableDependency", dicparameter);
            int tblPackDtlsno = ds.Tables.Count - 1;
            if (ds.Tables[tblPackDtlsno].Rows.Count < (job.Quantity + job.SurPlusQty))
            {
                TempData["Error"] = "Pack data not found. Kindly contact Administrator";
                return "Batch Qauntity is not same on both side";
            }
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ds.Tables[4]);
            return JSONString;

        }

        [HttpPost]
        public string Extra(string PONumber)
        {

            var job = db.Job.FirstOrDefault(x => x.JobName == PONumber);

            Dictionary<string, object> dicparameter = new Dictionary<string, object>();
            dicparameter.Add("JobID", job.JID);
            dicparameter.Add("PAID", job.PAID);

            DBSync dbServer = new DBSync();
            DataSet ds = dbServer.GetDataSet("SP_TableDependency", dicparameter);
            int tblPackDtlsno = ds.Tables.Count - 1;
            if (ds.Tables[tblPackDtlsno].Rows.Count < (job.Quantity + job.SurPlusQty))
            {
                TempData["Error"] = "Pack data not found. Kindly contact Administrator";
                return "Batch Qauntity is not same on both side";
            }
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(ds.Tables[4]);
            return JSONString;

        }
    }
}