using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using TnT.Models.Tracelink;
using TnT.Models.Job;
using TnT.Models.Customer;
using TnT.Models.Product;
using TnT.Models.SettingsNUtility;
using TnT.Models;
using TnT.Models.TraceLinkImporter;
using System.Linq;
using TnT.Models.Code;
using System;
using System.Configuration;
using System.Net;

namespace TnT.DataLayer.TracelinkService
{
    public enum TLFileTypes
    {
        SOMFile,
        DispositionFile,
        DispositionUpdateFile
    }

    public class TraceLinkUtils
    {      

         ApplicationDbContext db = new ApplicationDbContext();
        string FileName = "TracelinkData";

        #region Utils

        public string getFileName()
        {
          
            return FileName;
        }


        public byte[] genertefileContent(TLFileTypes FType,int JobId,M_SOM som,bool IsMoc)
        {
            XMLHelper hlpr = new XMLHelper();
            bool TLCustomer =Convert.ToBoolean(Utilities.getAppSettings("TLCustomer"));
            var job = db.Job.Where(x => x.JID == JobId).FirstOrDefault();
            StringBuilder DataBuilder = null;
            string disp = string.Empty;
            string date = DateTime.Now.ToString("ddMMyyyyhhmm");
            switch (FType)
            {
                case TLFileTypes.SOMFile:
                    if (TLCustomer == true)
                    {
                        DataBuilder = hlpr.getSOMXMLOld(JobId, som,IsMoc);
                    }
                    else
                    {
                        DataBuilder = hlpr.getSOMXML(JobId, som, IsMoc);
                    }
                    FileName = "Tracelink_SOM_"+job.BatchNo+"_"+date+".xml";
                    break;
                case TLFileTypes.DispositionFile:

                    disp = hlpr.getDispositionXML(JobId, IsMoc);
                    FileName = "DispositionAssign_"+job.BatchNo+"_"+date+".xml";
                    break;
                case TLFileTypes.DispositionUpdateFile:

                    DataBuilder = hlpr.getDispositionUpdateXml(JobId,IsMoc);
                    FileName = "DispositionUpdate_" + job.BatchNo + "_" + date + ".xml";
                    break;
              
            }

            if (DataBuilder != null)
            {
                return Encoding.ASCII.GetBytes(DataBuilder.ToString());

              

            }
            else
            {
                if (disp != string.Empty)
                {
                    return Encoding.ASCII.GetBytes(disp);
                }
                else
                {
                    return null;
                }
            }

          
        }

        public byte[] GenerateProduct(Job job, Settings setting,TLProductDetailViewModel vm,M_Customer cm,PackagingAsso pkg,PackagingAssoDetails pkgDetail)
        {
            XMLHelper xml = new XMLHelper();
            StringBuilder sw = null;
            sw = xml.getProductDetailXML(job, setting, vm,cm,pkg,pkgDetail);
            if (sw != null)
            {
                return Encoding.ASCII.GetBytes(sw.ToString());

            }
            else
            {
                return null;
            }

        }


        public bool IsUIDsAvailable(int Quantity)
        {
            if (Quantity > 0)
            {
                int cnt = db.X_TracelinkUIDStore.Where(x => x.IsUsed == false).Count();
                return (cnt > Quantity) ? true : false;

            }else
            {
                return false;
            }

        }

        public List<TLGTINQty> convertStringToGTINList(string data)
        {
            List<TLGTINQty> lst = new List<TLGTINQty>();
            var GTINList = data.Split(',');
            //if (GTINList.Count() >= 2)
            //{                
                foreach (var gtinItem in GTINList)
                {
                    if (!string.IsNullOrEmpty(gtinItem))
                    {
                        var gt = gtinItem.Split(':');
                        string gtin = gt[0].ToString().Trim();
                        int qty = Convert.ToInt32(gt[1]);
                        TLGTINQty obj = new TLGTINQty();
                        obj.GTIN = gtin;
                        obj.Qty = Convert.ToInt32(gt[1]);
                        if (gt.Count() > 2)
                        {
                            obj.ProviderId = Convert.ToInt32(gt[2]);
                        }
                        if (gt.Count() > 3)
                        {
                            obj.compType = gt[3];
                        }
                        lst.Add(obj);
                    }
                }
           // }
            return lst;
        }

        public bool checkTUIDExists(TLGTINQty gt)
        {
            if (gt != null)
            {
                int MasterUIdCnt = 0;
                var requests = db.M_TracelinkRequest.Where(x => x.GTIN == gt.GTIN && x.ProviderId==gt.ProviderId);
                if (requests != null)
                {
                    foreach (var item in requests)
                    {
                        var cnt = db.X_TracelinkUIDStore.Where(x => x.TLRequestId == item.Id && x.IsUsed == false).Count();
                        MasterUIdCnt += cnt;
                    }
                    if (MasterUIdCnt >= gt.Qty)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public string requestExtraUId(TLGTINQty gt,string type,string plantid)
        {

            bool isGlobal = Convert.ToBoolean(Utilities.getAppSettings("IsGlobalServer")); //Utilities.getAppSettings("IsGlobalServer"]

            if (isGlobal)
            {
                
                var url = new Uri((Utilities.getAppSettings("GlobalServerAddress")).Trim() + "api/ExtraTLUidRequest?GTIN=" + gt.GTIN + "&UidQuantity=" + gt.Qty+ "&Type="+type+"&plantId="+plantid);
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    var result = client.UploadString(url, "POST");
                    string[] res = result.Split('"');
                    return res[1];
                }
            }
            else
            {
                ExtraTLUidRequest ex = new ExtraTLUidRequest();
                string result = ex.TLExtraUIDs(gt.GTIN, gt.Qty,gt.ProviderId,type).ToString();
                return result;
            }
        }


        public string requestRFXCELExtraUID(TLGTINQty gt,string type)
        {
            ExtraTLUidRequest ex = new ExtraTLUidRequest();
            string result = ex.RFXLExtraUIDs(gt.GTIN, gt.Qty, gt.ProviderId,type).ToString();
            return result;
        }

        #endregion

    }

    public class TLGTINQty
    {
        public int Qty { get; set; }
        public string GTIN { get; set; }
        public int ProviderId { get; set; }
        public string compType { get; set; }
    }
}