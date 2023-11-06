using EPCIS_XMLs_Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using TnT.DataLayer.RFXCELService;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.TraceLinkImporter;

namespace TnT.DataLayer.TracelinkService
{
    public class ExtraTLUidRequest : ApiController
    {
        int threshhold, uidqty;
        private Trails trail = new Trails();
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        #region TraceLink UID Request for GTIN
        [System.Web.Http.HttpPost]
        public string TLExtraUIDs(string GTIN, int UidQuantity, int ProviderId,string SrNo)
        {
            string msg = string.Empty;
           
            if (UidQuantity <= 0) { return "Invalid UID Quantity."; }
            //if (PlantId <= 0) { return Ok("Invalid UID Quantity"); }

            var tempCustomer = db.M_TracelinkRequest.Where(x => x.GTIN == GTIN && x.ProviderId == ProviderId && x.SrnoType==SrNo).OrderByDescending(x => x.RequestedOn).FirstOrDefault();
            if (tempCustomer == null) { return "Invalid GTIN ! Customer not found."; }

            var customer = db.M_Customer.Find(tempCustomer.CustomerId);
            if (customer == null) { return "Customer not found."; }
            Tracelink tl = new Tracelink();
            if (UidQuantity > 0)
            {
                var isGTINRegistered = db.M_TracelinkRequest.Where(x => x.GTIN == GTIN && x.ProviderId == ProviderId && x.SrnoType == SrNo).OrderByDescending(x => x.RequestedOn).FirstOrDefault();
                if (isGTINRegistered != null)
                {
                    threshhold = isGTINRegistered.Threshold;

                    var tlUnusedUidLst = db.X_TracelinkUIDStore.Where(x =>x.Type==SrNo && x.IsUsed == false && x.GTIN==isGTINRegistered.GTIN).ToList();
                    if (tlUnusedUidLst.Count < (threshhold + UidQuantity))
                    {
                        int uidreq = threshhold + UidQuantity;
                        if (uidreq > tlUnusedUidLst.Count)
                        {
                            int uidqtyrequired = uidreq - tlUnusedUidLst.Count();
                            uidqty = uidqtyrequired;
                        }
                        
                    }
                    else
                    {
                        return TnT.LangResource.GlobalRes.TempDatarequiredNoofUidalredyExist;//tlUnusedUidLst.Count + " .UID ALREADY EXIST AND THE COUNT REQUIRED IS. " + (threshhold + UidQuantity);
                    }


                }
                else
                {
                    uidqty = UidQuantity;
                }
                //if (isGTINRegistered.SrnoType == "NTIN")
                //{
                //    type = "NTIN";
                //}
                if(SrNo=="LSSCC")
                {
                    uidqty = UidQuantity;
                }
                var data = tl.getDataFromTracelink(customer.APIUrl, customer.SenderId, customer.ReceiverId, uidqty, GTIN,isGTINRegistered.SrnoType, customer.CompanyCode,isGTINRegistered.FilterValue);
                if (data != null)
                {
                    if (data.Count > 0)
                    {
                        var errs = tl.getErrors();
                        if (errs.Count() == 0)
                        {
                            string stype = "GTIN";
                            var isGTIN = db.M_TracelinkRequest.Where(x => x.Id == isGTINRegistered.Id && x.SrnoType==SrNo).OrderByDescending(x=>x.RequestedOn).FirstOrDefault();
                           
                            //if (isGTIN == null)
                            //{
                            M_TracelinkRequest req = new M_TracelinkRequest();
                            req.CustomerId = tempCustomer.CustomerId;
                            req.GTIN = GTIN;
                            req.Quatity = uidqty;
                            req.RequestedOn = DateTime.Now;
                            req.IsDeleted = false;
                            req.Threshold = 0;
                            req.SrnoType = SrNo;
                            req.ProviderId = ProviderId;
                            req.FilterValue = isGTIN.FilterValue;
                            db.M_TracelinkRequest.Add(req);
                            db.SaveChanges();
                            foreach (var item in data)
                            {
                                item.TLRequestId = req.Id;
                            }
                            //}
                            //else
                            //{
                            //    foreach (var item in data)
                            //    {
                            //        item.TLRequestId = isGTIN.Id;
                            //    }
                            //}

                            var convertedData = DataLayer.GeneralDataHelper.convertToDataTable(data);
                            BulkDataHelper dataHlpr = new BulkDataHelper();
                            if (dataHlpr.InsertTracelinkUIDIdenties(convertedData))
                            {

                                msg = TnT.LangResource.GlobalRes.TempDataImporterDataimported;// + " " + convertedData.Rows.Count + " serial numbers imported !";
                            }
                            else
                            {
                                msg = TnT.LangResource.GlobalRes.TempDataImporterUnablestore;
                            }
                        }
                    }
                    else
                    {
                        msg = TnT.LangResource.GlobalRes.ShwMsgTracelinkUnabletoConnect;
                    }
                }
            }

            return msg;
        }


        public string RFXLExtraUIDs(string GTIN,int UidQuantity,int ProviderId,string type)
        {
           
            string msg = string.Empty;
            if (UidQuantity <= 0) { return "Invalid UID Quantity."; }
            var tempCustomer = db.M_TracelinkRequest.Where(x => x.GTIN == GTIN && x.ProviderId == ProviderId).OrderByDescending(x => x.RequestedOn).FirstOrDefault();
            if (tempCustomer == null) { return "Invalid GTIN ! Customer not found."; }
            RfxcelServices r1 = new RfxcelServices();
            var customer = db.M_Customer.Find(tempCustomer.CustomerId);
            if (customer == null) { return "Customer not found."; }
            Tracelink tl = new Tracelink();
            if (UidQuantity > 0)
            {
                var isGTINRegistered = db.M_TracelinkRequest.Where(x => x.GTIN == GTIN && x.ProviderId == ProviderId).OrderByDescending(x => x.RequestedOn).FirstOrDefault();
                if (isGTINRegistered != null)
                {
                    threshhold = isGTINRegistered.Threshold;

                    var tlUnusedUidLst = db.X_TracelinkUIDStore.Where(x => /*x.TLRequestId == isGTINRegistered.Id*/ x.GTIN==isGTINRegistered.GTIN && x.IsUsed == false).ToList();
                    if (tlUnusedUidLst.Count < (threshhold + UidQuantity))
                    {
                        int uidreq = threshhold + UidQuantity;
                        if (uidreq > tlUnusedUidLst.Count)
                        {
                            int uidqtyrequired = uidreq - tlUnusedUidLst.Count();
                            uidqty = uidqtyrequired;
                        }
                       
                    }
                    else
                    {
                        return TnT.LangResource.GlobalRes.TempDatarequiredNoofUidalredyExist;//tlUnusedUidLst.Count + " .UID ALREADY EXIST AND THE COUNT REQUIRED IS. " + (threshhold + UidQuantity);
                    }
                    
                }
                else
                {
                    uidqty = UidQuantity;
                }
                EPCISConfig gln = new EPCISConfig();
              
                var data =r1.getSerialNumbersFromRfxcel(customer.APIUrl, customer.SenderId, customer.ReceiverId, uidqty, GTIN,gln.GetEPCGLN(customer.Id));
                if (data != null)
                {
                    if (data.Count > 0)
                    {
                        var errs = tl.getErrors();
                        if (errs.Count() == 0)
                        {
                            var isGTIN = db.M_TracelinkRequest.Where(x => x.Id == isGTINRegistered.Id).FirstOrDefault();
                            //if (isGTIN == null)
                            //{
                            M_TracelinkRequest req = new M_TracelinkRequest();
                            req.CustomerId = tempCustomer.CustomerId;
                            req.GTIN = GTIN;
                            req.Quatity = uidqty;
                            req.RequestedOn = DateTime.Now;
                            req.IsDeleted = false;
                            req.Threshold = 0;
                            req.ProviderId = ProviderId;
                            req.SrnoType = type;
                            db.M_TracelinkRequest.Add(req);
                            db.SaveChanges();
                            foreach (var item in data)
                            {
                                item.TLRequestId = req.Id;
                            }
                           
                            var convertedData = DataLayer.GeneralDataHelper.convertToDataTable(data);
                            BulkDataHelper dataHlpr = new BulkDataHelper();
                            if (dataHlpr.InsertTracelinkUIDIdenties(convertedData))
                            {

                                msg = TnT.LangResource.GlobalRes.TempDataImporterDataimported;// + " " + convertedData.Rows.Count + " serial numbers imported !";
                            }
                            else
                            {
                                msg = TnT.LangResource.GlobalRes.TempDataImporterUnablestore;
                            }
                        }
                    }
                }
            }
                return msg;
        }
        #endregion
    }
}