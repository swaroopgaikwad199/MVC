using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TnT.Models;
using TnT.Models.Code;
using TnT.DataLayer;
using Newtonsoft.Json;
using TnT.DataLayer.Trailings;
using System.Globalization;
using TnT.Models.Product;
using System.Xml;

namespace TnT.Controllers
{
    public class ServiceController : ApiController
    {
        private Trails trail = new Trails();
        private ApplicationDbContext db = new ApplicationDbContext();

        // POST: api/Service/5/
        [HttpPost]
        [ResponseType(typeof(JsonTextReader))]
        //[ResponseType(typeof(X_Code))]
        public IHttpActionResult GenerateIds(string ServiceKey, int PAID, int JobId, string BatchNo, string ManufacturingDate, string Expirydate)
        {
            try
            {
                Service obj = new Service();

                DateTime mfg, exp;  // DateFormat yyyy-mm-dd
                //mfg = Convert.ToDateTime(ManufacturingDate);
                //exp = Convert.ToDateTime(Expirydate);

                if (!DateTime.TryParseExact(ManufacturingDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out mfg))
                    return Ok(TnT.LangResource.GlobalRes.TempDataServiceInvalidMfgDt);
                if (!DateTime.TryParseExact(Expirydate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out exp))
                    return Ok(TnT.LangResource.GlobalRes.TempDataServiceInvalidExpDt);

                mfg = Convert.ToDateTime(ManufacturingDate);
                exp = Convert.ToDateTime(Expirydate);

                if ((ServiceKey == "") || (ServiceKey == null))
                    return Ok(TnT.LangResource.GlobalRes.TempDataServiceInvalidKey);
                if (!obj.IsKeyValid(ServiceKey))
                    return Ok(TnT.LangResource.GlobalRes.TempDataServiceInvalidKey);
                if (!obj.IsJobIdExisting(JobId))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidJob);

                if (!obj.IsProductValid(JobId, BatchNo, mfg, exp))
                    return Ok(TnT.LangResource.GlobalRes.TempDataServiceInvalidBatchInfo);

                var rs = db.M_RequestLog.Where(x => x.ServiceKey == ServiceKey && x.JobId == JobId).FirstOrDefault();
                if (rs != null)
                {
                    if (rs.IsReceived)
                    {
                        return Ok(TnT.LangResource.GlobalRes.TempDataServiceInvalidDtaAcq);
                    }
                    else
                    {
                        return Ok(TnT.LangResource.GlobalRes.TempDataServiceInvalidWaitingForAck);
                    }
                }
                else
                {
                    M_RequestLog log = new M_RequestLog();
                    log.ServiceKey = ServiceKey;
                    //log.Quantity = Quantity;
                    log.JobId = JobId;
                    log.BatchNo = BatchNo;
                    log.MfgDate = mfg;
                    log.ExpDate = exp;
                    log.IsReceived = false;
                    db.M_RequestLog.Add(log);
                    db.SaveChanges();
                    int RequestId = log.id;
                    // generate Uids
                    IdGenerationUtlity generator = new IdGenerationUtlity();
                    generator.generateIds(RequestId, JobId, PAID, "DGFT", "-");

                    Response res = new Response();
                    //var DTGTIN = res.getGTINCode(RequestId);
                    //DTGTIN.TableName = "GTIN";
                    var DTSSCC = res.getSSCCCode(RequestId);
                    DTSSCC.TableName = "SSCC";
                    var DTUIDs = res.getUIDs(RequestId);
                    DTUIDs.TableName = "UIDs";


                    DataSet ds = new DataSet();
                    //ds.Merge(DTGTIN);
                    ds.Merge(DTSSCC);
                    ds.Merge(DTUIDs);
                    // var ot = JsonConvert.SerializeObject(ds);
                    var vendor = db.M_Vendor.Where(m => m.ServiceKey == ServiceKey).FirstOrDefault();
                    trail.AddTrail(vendor.CompanyName + TnT.LangResource.GlobalRes.TrailServiceReqIds, vendor.Id, vendor.CompanyName + TnT.LangResource.GlobalRes.TrailServiceReqIds, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                    return Ok(ds);
                }
            }
            catch (Exception ex)
            {
                trail.AddTrail(TnT.LangResource.GlobalRes.TrailServiceErrorOccured + ex.Message, 0, TnT.LangResource.GlobalRes.TrailServiceErrorOccured + ex.Message,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [ResponseType(typeof(JsonTextReader))]
        public IHttpActionResult Acknowledegement(string ServiceKey, int JobId, string AcknowledgeMessage)
        {
            try
            {
                Service obj = new Service();

                if ((ServiceKey == "") || (ServiceKey == null))
                    return Ok(TnT.LangResource.GlobalRes.TempDataServiceInvalidKey);
                if (!obj.IsKeyValid(ServiceKey))
                    return Ok(TnT.LangResource.GlobalRes.TempDataServiceInvalidKey);
                if (obj.IsRequestValid(ServiceKey, JobId))
                {
                    M_RequestLog req = db.M_RequestLog.Where(x => x.JobId == JobId).FirstOrDefault();
                    req.AcknowldgeDtTm = DateTime.Now;
                    req.AcknowldgeMessage = AcknowledgeMessage;
                    req.IsReceived = true;
                    db.Entry(req).State = EntityState.Modified;
                    db.SaveChanges();

                    var vendor = db.M_Vendor.Where(m => m.ServiceKey == ServiceKey).FirstOrDefault();
                    trail.AddTrail(vendor.CompanyName + ":"+ TnT.LangResource.GlobalRes.TrailServiceAckdatareceived, vendor.Id, vendor.CompanyName + ":" + TnT.LangResource.GlobalRes.TrailServiceAckdatareceived, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                    return Ok(TnT.LangResource.GlobalRes.TempDataServiceSuccess);
                }
                else
                {
                    return Ok(TnT.LangResource.GlobalRes.TempDataServiceInvalidReqWthJbId);
                }

            }
            catch (Exception ex)
            {
                trail.AddTrail(TnT.LangResource.GlobalRes.TrailServiceErrorOccured + ex.Message, 0, TnT.LangResource.GlobalRes.TrailServiceErrorOccured + ex.Message,TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
                return Ok(ex.Message);

            }
        }

        [HttpPost]
        [ResponseType(typeof(JsonTextReader))]
        //[ResponseType(typeof(X_Code))]
        public IHttpActionResult GetProductDetails(string ServiceKey, int PAID)
        {
            try
            {
                Service obj = new Service();
                if ((ServiceKey == "") || (ServiceKey == null))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidKy);
                if (!obj.IsKeyValid(ServiceKey))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidKy);
                if (!obj.IsProductExisting(PAID))
                    return Ok(TnT.LangResource.GlobalRes.TempDataServiceInvalProductDetls);
                var product = db.PackagingAsso.Where(p => p.PAID == PAID).Select(p => new { p.Name, p.PAID, p.ProductCode, p.UseExpDay, p.ScheduledDrug, p.Remarks, p.GenericName, p.FGCode, p.Composition, p.Description, p.DoseUsage, p.ExpDateFormat }).ToList();
                var prodDetails = db.PackagingAssoDetails.Where(x => x.PAID == PAID).Select(p => new { p.GTIN, p.PackageTypeCode, p.Size, p.MRP, p.BundleQty, p.Remarks });

                dynamic ProductWrapper = new { Product = product };
                dynamic ProductDetailsWrapper = new { ProductDetails = prodDetails };

                string PRodjson = JsonConvert.SerializeObject(ProductWrapper);
                string PRodDtlsjson = JsonConvert.SerializeObject(ProductDetailsWrapper);

                var ProdDt = JsonConvert.DeserializeObject<DataSet>(PRodjson);
                var ProdDtlsDt = JsonConvert.DeserializeObject<DataSet>(PRodDtlsjson);

                DataSet ds = new DataSet();
                ds.Merge(ProdDt);
                ds.Merge(ProdDtlsDt);


                return Ok(ds);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //throw;
            }

        }

        [HttpPost]
        [ResponseType(typeof(JsonTextReader))]
        //[ResponseType(typeof(X_Code))]
        public IHttpActionResult GetJobDetails(string ServiceKey, int JobId)
        {
            try
            {
                Service obj = new Service();
                if ((ServiceKey == "") || (ServiceKey == null))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidKy);
                if (!obj.IsKeyValid(ServiceKey))
                    return Ok(TnT.LangResource.GlobalRes.TempDataReceiverInvalidKy);
                if (!obj.IsJobIdExisting(JobId))
                    return Ok(TnT.LangResource.GlobalRes.TempDataServiceInvalProductDetls);

                decimal jid = Convert.ToDecimal(JobId);
                var TempJob = db.Job.Find(JobId);
                string JobNorm = "NA";

                if (TempJob.TID != 0)
                {
                    JobNorm = db.JOBTypes.Find(TempJob.TID).Job_Type;
                }


                var jb = db.Job.Where(j => j.JID == jid).Select(j => new { j.JID, j.JobName, j.BatchNo, j.DetailInfo, j.ForExport, j.JobWithUID, j.MLNO, j.PAID, j.PrimaryPCMapCount, j.Quantity, j.Remarks, j.SurPlusQty, j.TenderText, JobNorm, j.MfgDate, j.ExpDate });
                var jbDetails = db.JobDetails.Where(x => x.JD_JobID == JobId).Select(j => new { j.JD_ProdName, j.JD_ProdCode, j.JD_MRP, j.JD_GTIN, j.JD_FGCode, j.JD_Description, j.JD_DeckSize, j.JD_Deckcode, j.BundleQty });

                dynamic JobWrapper = new { Job = jb };
                dynamic JobDetailsWrapper = new { JobDetails = jbDetails };

                string Jobjson = JsonConvert.SerializeObject(JobWrapper);
                string JobDtlsjson = JsonConvert.SerializeObject(JobDetailsWrapper);

                var DtJob = JsonConvert.DeserializeObject<DataSet>(Jobjson);
                var DtJobDtls = JsonConvert.DeserializeObject<DataSet>(JobDtlsjson);

                DataSet ds = new DataSet();
                ds.Merge(DtJob);
                ds.Merge(DtJobDtls);

                return Ok(ds);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                //throw;
            }

        }


        [HttpPost]
        [ResponseType(typeof(JsonTextReader))]
        public IHttpActionResult Test(string Name)
        {
            return Ok("Hello " + Name);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool X_CodeExists(int id)
        {
            return db.X_Code.Count(e => e.Id == id) > 0;
        }
    }
}