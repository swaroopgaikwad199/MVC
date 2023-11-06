using Newtonsoft.Json;
using PTPLCRYPTORENGINE;
using REDTR.HELPER;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.Account;
using TnT.Models.Exporter;
using TnT.Models.Job;
using TnT.Models.SettingsNUtility;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class ExporterController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        // GET: Exporter
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public ActionResult getPackagetypeCode(string jobname)
        {
            var job = db.Job.Where(x => x.JobName == jobname).FirstOrDefault();
            if (job != null)
            {
                var jobdetail = db.JobDetails.Where(x => x.JD_JobID == job.JID).ToList();
                return Json(jobdetail);
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.toastrLblLytDsgNoData);
            }
        }

        [HttpPost]
        public ActionResult Index([Bind(Include = "JobName,PackagingType,Username,Password")] ExporterModelView export)
        {
            TnT.WebReference.BatchDataService b1 = new TnT.WebReference.BatchDataService();
            var udata = db.Users.Where(x => x.ID == User.ID).FirstOrDefault();
            string username = udata.UserName;
              string password = AESCryptor.Decrypt(udata.Password, REDTR.UTILS.SystemIntegrity.Globals.Cipherkey);
          //  string password = udata.Password;
            string data = b1.GetBatchData(username, password, export.JobName, export.PackagingType);
            XmlNode xml = JsonConvert.DeserializeXmlNode("{records:{record:" + data + "}}");
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml.InnerXml);
            XmlReader xmlReader = new XmlNodeReader(xml);
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(xmlReader);

            if (dataSet.Tables.Contains("Table"))
            {
                var dt = dataSet.Tables[1];
                //Datatable to CSV
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    for (int k = 0; k < dt.Columns.Count - 1; k++)
                    {
                        sb.Append(dt.Rows[j][k].ToString() + ',');
                    }
                    sb.Append(Environment.NewLine);
                }
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename = " + export.JobName + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(sb.ToString());
                Response.Flush();
                Response.End();
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataExporterCSVFile;
             
            }
            else
            {
                string msg = dataSet.Tables[0].Rows[0][0].ToString();
                TempData["Success"] = msg;
            }
            return View();
        }
    }
}