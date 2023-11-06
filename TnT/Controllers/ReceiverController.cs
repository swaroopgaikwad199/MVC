using TnT.DataLayer;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.Code;

using PTPLCRYPTORENGINE;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Configuration;

namespace TnT.Controllers
{
    public class ReceiverController : ApiController
    {
        private Trails trail = new Trails();
        private ApplicationDbContext db = new ApplicationDbContext();
        private int jobId,PAID,RequestId;
        private string JobName, BatchNo, Key, invalidIds;
        private bool IsSynced = false;


        /// <summary>
        /// Perform the actions(validating , and saving the  xml file data ) when an Post request is recived
        /// 1 sep 2016
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]

        public IHttpActionResult PostFile()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var frm = httpRequest.Form;

                if (frm == null)
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidReq);

                //string ServiceKey, int JobId, string BatchNo
                this.jobId = Convert.ToInt32(frm["JobId"]);
                this.Key = frm["ServiceKey"];
                this.BatchNo = frm["BatchNo"];
                if (!Request.Content.IsMimeMultipartContent())
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverUnsupportedMediaType);
                ///////////////////
                Service obj;
                obj = new Service();

                if ((Key == "") || (Key == null))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidKy);
                if (!obj.IsKeyValid(Key))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidKy);
                if (!obj.IsJobIdExisting(jobId))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidJob);
                if (!obj.IsProductValid(jobId, BatchNo))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidProduct);
                /////////////////
                RequestId = 0;
                var req = db.M_RequestLog.Where(r => r.JobId == jobId).FirstOrDefault();
                RequestId = req.id;
                if (RequestId == 0)
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidReq);

                if (!req.IsReceived)
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverCanntUploadFile);

                IsSynced = db.M_RequestLog.Where(r => r.id == RequestId).Select(k => k.IsSynced).FirstOrDefault();
                if (IsSynced)
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverAlredyFileSync);

                if (httpRequest.Files.Count > 0)
                {

                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        FileInfo fi = new FileInfo(postedFile.FileName);
                        if (fi.Extension == ".xml" || fi.Extension == ".json")
                        {
                            string fNm = RequestId + "_" + jobId + "_" + postedFile.FileName;
                            string filePath = HttpContext.Current.Server.MapPath("~/DataFiles/" + fNm);
                            postedFile.SaveAs(filePath);
                            docfiles.Add(filePath);
                            var result = PeformActions(fNm, fi);
                            return result;
                        }
                        else
                        {
                            return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidFile);
                        }
                    }

                    var vendor = db.M_Vendor.Where(m => m.ServiceKey == Key).FirstOrDefault();
                    trail.AddTrail(vendor.CompanyName + TnT.LangResource.GlobalRes.TrailSyncFileSuccessfully, vendor.Id, vendor.CompanyName + TnT.LangResource.GlobalRes.TrailSyncFileSuccessfully, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverFileSuccessfullySync + docfiles + " !");
                }
                else
                {
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverNoFileFound);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.StackTrace);

            }
        }

        /// <summary>
        /// sync the data to the server.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private IHttpActionResult PeformActions(string FileName, FileInfo fi)
        {
            try
            {
                List<string> unidentified = new List<string>();
                DataSet ds = new DataSet();

                if (fi.Extension == ".xml")
                {
                    XMLHelper helper = new XMLHelper();
                    ds = helper.readXML(FileName);
                }
                else
                {
                    JSONHelper jhelp = new JSONHelper();
                    ds = jhelp.readJSON(FileName);
                }

                DataTable dtProductInfo, dtJobInfo;

                if (!ds.Tables.Contains("ProductInfo"))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidXmlFile);
                if (!ds.Tables.Contains("JobInfo"))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidXmlFile);

                dtJobInfo = new DataTable();
                dtJobInfo = ds.Tables["JobInfo"];
                if (!(dtJobInfo.Rows.Count > 0))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverNoDtaAvailblIn + dtJobInfo.TableName);

                dtProductInfo = new DataTable();
                dtProductInfo = ds.Tables["ProductInfo"];
                if (!(dtProductInfo.Rows.Count > 0))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverNoDtaAvailblIn + dtProductInfo.TableName + " !");


                //jobId = Convert.ToInt32(dtJobInfo.Rows[0]["JID"]);
                //JobName = dtJobInfo.Rows[0]["JobName"].ToString();
                //PAID = Convert.ToInt32(dtJobInfo.Rows[0]["PAID"].ToString());
                //BatchNo = dtJobInfo.Rows[0]["BatchNo"].ToString();

                DataTable dtPackagingDetails = new DataTable();


                dtPackagingDetails.Columns.Add("Code", typeof(string));
                dtPackagingDetails.Columns.Add("JobID", typeof(decimal));
                dtPackagingDetails.Columns.Add("PackageTypeCode", typeof(string));
                dtPackagingDetails.Columns.Add("MfgPackDate", typeof(DateTime));
                dtPackagingDetails.Columns.Add("ExpPackDate", typeof(DateTime));
                dtPackagingDetails.Columns.Add("NextLevelCode", typeof(string));
                dtPackagingDetails.Columns.Add("IsRejected", typeof(string));
                dtPackagingDetails.Columns.Add("Reason", typeof(string));
                dtPackagingDetails.Columns.Add("SSCC", typeof(string));
                dtPackagingDetails.Columns.Add("SSCCVarificationStatus", typeof(string));
                dtPackagingDetails.Columns.Add("CreatedDate", typeof(DateTime));
                dtPackagingDetails.Columns.Add("LastUpdatedDate", typeof(DateTime));
                dtPackagingDetails.Columns.Add("LineCode", typeof(string));

                ReceiverValidator validator = new ReceiverValidator(RequestId);

                foreach (DataRow row in dtProductInfo.Rows)
                {
                    string code = row["Code"].ToString();
                    //if (db.X_Code.Any(p => p.Code == code && p.RequestId == RequestId))
                    //{                                       
                    if (validator.IsUIdValid(code))
                    {
                        code = AESCryptor.Encrypt(code, "PTPLCRYPTOSYS");
                        DataRow dr = dtPackagingDetails.NewRow();
                        dr["Code"] = code;
                        dr["JobID"] = jobId;
                        dr["PackageTypeCode"] = row["PackageTypeCode"].ToString();
                        dr["MfgPackDate"] = Convert.ToDateTime(row["MfgPackDate"]);
                        dr["ExpPackDate"] = Convert.ToDateTime(row["ExpPackDate"]);
                        dr["NextLevelCode"] = row["NextLevelCode"].ToString();


                        if (row["Reason"].ToString() != null)
                        {
                            dr["Reason"] = row["Reason"].ToString();
                        }
                        else
                        {
                            dr["Reason"] = "";
                        }

                        if (row["SSCC"].ToString() != null)
                        {
                            dr["SSCC"] = row["SSCC"].ToString();
                        }
                        else
                        {
                            dr["SSCC"] = "";
                        }

                        dr["SSCCVarificationStatus"] = row["SSCCVarificationStatus"];
                        dr["CreatedDate"] = Convert.ToDateTime(row["CreatedDate"]);
                        dr["LastUpdatedDate"] = Convert.ToDateTime(row["LastUpdatedDate"]);
                        dr["LineCode"] = row["LineCode"].ToString();
                        dr["IsRejected"] = row["IsRejected"];
                        dtPackagingDetails.Rows.Add(dr);

                    }
                    else
                    {
                        unidentified.Add(code);
                    }
                }

                BulkDataHelper bulkHlpr = new BulkDataHelper();
                var connectionString = Utilities.getConnectionString("DefaultConnection");
                if (bulkHlpr.InsertPackagingDetails(dtPackagingDetails,connectionString))
                {
                    M_RequestLog mreq = db.M_RequestLog.Find(RequestId);
                    mreq.IsSynced = true;
                    mreq.SyncedDate = DateTime.Now;
                    db.SaveChanges();

                    if (unidentified.Count > 0)
                    {
                        invalidIds = string.Join(",", unidentified.ToArray());
                        return Ok("" + unidentified.Count + " Invalid UIDs :  " + invalidIds);
                    }
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverDataSyncSucc);
                }
                else
                {
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverIssuesSavData);
                }
            }
            catch (Exception ex)
            {
                return Ok( TnT.LangResource.GlobalRes.TempdataProductError + ex.StackTrace);
            }
        }

    }
}
