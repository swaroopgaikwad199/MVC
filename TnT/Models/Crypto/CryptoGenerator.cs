using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using REDTR.HELPER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Data.SqlClient;

namespace TnT.Models.Crypto
{
    public class ExecutionStatus
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public enum OrderStatus
    {
        CREATED = 0,
        READY = 1,
        PARTIAL_RECEIVED = 2,
        CLOSED = 3,
    }

    public class CryptoGenerator
    {
        public List<ExecutionStatus> lstExecutionStatus = new List<ExecutionStatus>();

        //public string OMSID { get; set; }
        //public string RUSSIA_CLIENT_TOKEN { get; set; }
        public string MEDIA_TYPE = "application/json"; //"text/plain"; "text/xml"; "application/xml"


        //http://82.202.183.16:45676/api/v2/pharma/orders?omsId=e5b51a8f-7ee2-40f7-abec-f33207a74636
        public string SERVICE_URL_PING_API = "http://82.202.183.16:45676/api/v2/pharma/orders?omsId=e5b51a8f-7ee2-40f7-abec-f33207a74636";

        //http://82.202.183.16:45676/api/v2/pharma/orders?omsId=e5b51a8f-7ee2-40f7-abec-f33207a74636
        public string SERVICE_URL_CREATE_ORDER = "/orders?omsId={omsId}";

        //http://82.202.183.16:45676/api/v2/pharma/orders?omsId=e5b51a8f-7ee2-40f7-abec-f33207a74636
        public string SERVICE_URL_GET_ORDER_STATUS = "/orders?omsId={omsId}";

        //http://82.202.183.16:45676/api/v2/pharma/codes?gtin=08901086091192&lastBlockId=0&omsId=e5b51a8f-7ee2-40f7-abec-f33207a74636&orderId=cd1dfaa7-80b1-41f4-ada5-7c3a5a99f6be&quantity=10
        public string SERVICE_URL_DOWNLOAD_CODES = "/codes?gtin={gtin}&lastBlockId=0&omsId={omsId}&orderId={orderId}&quantity={quantity}";

        //http://82.202.183.16:45676/api/v2/pharma/utilisation?omsId=e5b51a8f-7ee2-40f7-abec-f33207a74636
        public string EXPORT_UTILISATION_URL = "/utilisation?omsId={omsId}";

        //http://82.202.183.16:45676/api/v2/pharma/aggregation?omsId=e5b51a8f-7ee2-40f7-abec-f33207a74636
        public string EXPORT_AGGREGATION_URL = "/aggregation?omsId={omsId}";

        //http://82.202.183.16:45676/api/v2/pharma/dropout?omsId=e5b51a8f-7ee2-40f7-abec-f33207a74636
        public string EXPORT_DROPOUT_URL = "/dropout?omsId={omsId}";


        public string ORDER_TYPE = "1"; // (1) own, (2) contract ---    // 1 = self or 2 = external

        public CryptoGenerator()
        {
            //RUSSIA_CLIENT_TOKEN = Utilities.getAppSettings("RUSSIA_CLIENT_TOKEN");
            //OMSID = Utilities.getAppSettings("OMSID");
        }

        //public CreateOrderResponse CRYPTO_CREATE_ORDER(M_CryptOrder m_CryptOrder, Models.Customer.M_Customer selectedCustomer, int RUSSIA_SERIAL_NO_LENGTH, bool isFreeCode)
        //{
        //    lstExecutionStatus.Clear();
        //    CreateOrderResponse product_result = new CreateOrderResponse();
        //    try
        //    {
        //        string RUSSIA_PAYMENT_TYPE = Convert.ToString(Utilities.getAppSettings("RUSSIA_PAYMENT_TYPE"));
        //        long RUSSIA_TEMPLATE_ID = Convert.ToInt64(Utilities.getAppSettings("RUSSIA_TEMPLATE_ID"));

        //        SERVICE_URL_CREATE_ORDER = selectedCustomer.APIUrl.Trim('/') + SERVICE_URL_CREATE_ORDER.Replace("{omsId}", selectedCustomer.OmsId);

        //        var url = new Uri(SERVICE_URL_CREATE_ORDER);

        //        using (var client = new WebClient())
        //        {
        //            client.Headers[HttpRequestHeader.ContentType] = MEDIA_TYPE;
        //            client.Headers["clientToken"] = selectedCustomer.APIKey.Trim();

        //            //string jsonData = " {\"products\":[{\"gtin\":\"08901086091192\",\"quantity\": 10,\"serialNumberType\": \"SELF_MADE\",\"serialNumbers\": [" +
        //            //              " \"F3EIV8OL58IJE\",\"RRJI8XYLOEF02\",\"SX85MSI7NK6DY\",\"DSXAZJVXHA1FY\",\"QK9WM22Q16VQT\",\"76QZMAALRMJ0V\",\"UJKP4RA3VXLR2\"," +
        //            //              " \"AHD9800T8O4CD\",\"IFCYO1F0Q830T\",\"HRDNHXYRTL6E8\"],\"templateId\": 5}],\"subjectId\": \"c1d795ad-c106-4f88-bdba-1f3f3b8c8d19\"}\"";

        //            Product products = new Product();

        //            products.Gtin = m_CryptOrder.GTIN;
        //            products.Quantity = m_CryptOrder.Quantity;
        //            products.SerialNumberType = "SELF_MADE";
        //            products.TemplateId = RUSSIA_TEMPLATE_ID;
        //            product_result.SerialNumbers = new string[m_CryptOrder.Quantity];

        //            #region GENERATE SERIAL NUMBERS
        //            DataLayer.IDGenrationFactory uidFactory = new DataLayer.IDGenrationFactory();

        //            products.SerialNumbers = uidFactory.generateIDsForRussia(m_CryptOrder.Quantity, RUSSIA_SERIAL_NO_LENGTH, "EU");

        //            #endregion

        //            CreateOrderInput orderInput = new CreateOrderInput();
        //            //orderInput.SubjectId = Guid.NewGuid().ToString();
        //            orderInput.SubjectId = selectedCustomer.ControlID;

        //            orderInput.paymentType = RUSSIA_PAYMENT_TYPE;
        //            //bool isFree = false;
        //            //bool.TryParse(selectedCustomer.FreeСode, out isFree);
        //            orderInput.freeСode = isFreeCode;

        //            if (orderInput.Products == null)
        //            {
        //                orderInput.Products = new Product[1];
        //            }

        //            orderInput.Products[0] = products;

        //            var jsonData = JsonConvert.SerializeObject(orderInput);

        //            DataLayer.ExceptionHandler.ExceptionLogger.LogError("Create Order Body : " + jsonData);

        //            var result = client.UploadString(url, "POST", jsonData);

        //            //string reply ={"omsId": "e5b51a8f-7ee2-40f7-abec-f33207a74636", "orderId": "465d834d-fa53-430a-ad88-42be97e49a21","expectedCompleteTimestamp": 721888 }
        //            product_result = (CreateOrderResponse)JsonConvert.DeserializeObject(result, typeof(CreateOrderResponse));
        //            product_result.SubjectId = orderInput.SubjectId;
        //            product_result.SerialNumbers = products.SerialNumbers;

        //            return product_result;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);
        //        DataLayer.ExceptionHandler.ExceptionLogger.LogError("Create Order URL : " + SERVICE_URL_CREATE_ORDER);

        //        lstExecutionStatus.Add(new ExecutionStatus
        //        {
        //            IsSuccess = false,
        //            Message = "ERROR CRYPTO_CREATE_ORDER()" + ex.Message
        //        });
        //    }
        //    return product_result;
        //}

        //public OrderStatusInfo CRYPTO_ORDER_CHECK_STATUS(M_CryptOrder m_CryptOrder, Models.Customer.M_Customer selectedCustomer)
        //{
        //    lstExecutionStatus.Clear();

        //    OrderStatusInfo statusInfo = null;
        //    try
        //    {
        //        SERVICE_URL_GET_ORDER_STATUS = selectedCustomer.APIUrl.Trim('/') + SERVICE_URL_GET_ORDER_STATUS.Replace("{omsId}", selectedCustomer.OmsId);

        //        var url = new Uri(SERVICE_URL_GET_ORDER_STATUS);

        //        using (HttpClient confClient = new HttpClient())
        //        {
        //            //"Basic", "Bearer"
        //            confClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MEDIA_TYPE));
        //            confClient.DefaultRequestHeaders.Add("clientToken", selectedCustomer.APIKey.Trim());

        //            HttpResponseMessage message = confClient.GetAsync(url).Result;

        //            if (message.IsSuccessStatusCode)
        //            {
        //                var inter = message.Content.ReadAsStringAsync();
        //                var product_result = JsonConvert.DeserializeObject(inter.Result);

        //                statusInfo = JsonConvert.DeserializeObject<OrderStatusInfo>(inter.Result);
        //            }
        //        }

        //        return statusInfo;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);
        //        DataLayer.ExceptionHandler.ExceptionLogger.LogError("Status URL : " + SERVICE_URL_GET_ORDER_STATUS);

        //        lstExecutionStatus.Add(new ExecutionStatus
        //        {
        //            IsSuccess = false,
        //            Message = "ERROR CRYPTO_ORDER_CHECK_STATUS()" + ex.Message
        //        });
        //    }
        //    return statusInfo;
        //}

        //public DownloadCryptoResponse DOWNLOAD_CRYPTO_CODE(M_CryptOrder m_CryptOrder, Models.Customer.M_Customer selectedCustomer)
        //{
        //    lstExecutionStatus.Clear();
        //    DownloadCryptoResponse DownloadCrypto = null;
        //    SERVICE_URL_DOWNLOAD_CODES = "/codes?gtin={gtin}&lastBlockId=0&omsId={omsId}&orderId={orderId}&quantity={quantity}";

        //    try
        //    {
        //        SERVICE_URL_DOWNLOAD_CODES = SERVICE_URL_DOWNLOAD_CODES.Replace("{gtin}", m_CryptOrder.GTIN);
        //        SERVICE_URL_DOWNLOAD_CODES = SERVICE_URL_DOWNLOAD_CODES.Replace("{omsId}", selectedCustomer.OmsId);
        //        SERVICE_URL_DOWNLOAD_CODES = SERVICE_URL_DOWNLOAD_CODES.Replace("{orderId}", m_CryptOrder.OrderId);
        //        int bufferQty = 5000;
        //        string RUSSIA_DOWNLOAD_BUFFER = Convert.ToString(Utilities.getAppSettings("RUSSIA_DOWNLOAD_BUFFER"));

        //        if (!string.IsNullOrEmpty(RUSSIA_DOWNLOAD_BUFFER))
        //        {
        //            int.TryParse(RUSSIA_DOWNLOAD_BUFFER, out bufferQty);
        //        }

        //        if (m_CryptOrder.Quantity < bufferQty || (m_CryptOrder.Quantity - m_CryptOrder.ReceivedQuantity) < bufferQty)
        //        {
        //            bufferQty = (m_CryptOrder.Quantity - m_CryptOrder.ReceivedQuantity);
        //        }

        //        SERVICE_URL_DOWNLOAD_CODES = SERVICE_URL_DOWNLOAD_CODES.Replace("{quantity}", bufferQty.ToString());
        //        SERVICE_URL_DOWNLOAD_CODES = selectedCustomer.APIUrl.Trim('/') + SERVICE_URL_DOWNLOAD_CODES;

        //        var url = new Uri(SERVICE_URL_DOWNLOAD_CODES);

        //        using (HttpClient confClient = new HttpClient())
        //        {
        //            confClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MEDIA_TYPE));
        //            confClient.DefaultRequestHeaders.Add("clientToken", selectedCustomer.APIKey);

        //            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
        //            HttpResponseMessage message = confClient.GetAsync(url).Result;

        //            var inter = message.Content.ReadAsStringAsync();
        //            var product_result = JsonConvert.DeserializeObject(inter.Result);

        //            DataLayer.ExceptionHandler.ExceptionLogger.LogError("inter.Result : " + inter.Result);

        //            if (message.IsSuccessStatusCode)
        //            {
        //                DownloadCrypto = JsonConvert.DeserializeObject<DownloadCryptoResponse>(inter.Result);
        //            }
        //            else
        //            {
        //                ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(inter.Result);
        //                if (errorResponse != null && errorResponse.globalErrors != null)
        //                {
        //                    lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = false, Message = errorResponse.globalErrors[0].error.ToString() });
        //                }
        //                else
        //                {
        //                    lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = false, Message = message.ReasonPhrase });
        //                }
        //            }
        //        }

        //        return DownloadCrypto;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);
        //        DataLayer.ExceptionHandler.ExceptionLogger.LogError("Download URL : " + SERVICE_URL_DOWNLOAD_CODES);
        //        lstExecutionStatus.Add(new ExecutionStatus
        //        {
        //            IsSuccess = false,
        //            Message = "ERROR CRYPTO_ORDER_CHECK_STATUS()" + ex.Message
        //        });
        //    }
        //    return DownloadCrypto;
        //}

        public static SplitedCryptoCode splitCryptoCode(string cryptoString)
        {
            //010357466142405721RYE9ZM715VZ1391ffd092YkBK7v6/HAZqau/eYH5tDl4WeGqza9oNOG/RM1E7fFQ=
            //01    03574661424057  
            //21    RYE9ZM715VZ13
            //91    ffd0
            //92    YkBK7v6/HAZqau/eYH5tDl4WeGqza9oNOG/RM1E7fFQ=

            SplitedCryptoCode obj = new SplitedCryptoCode();
            try
            {
                obj.CryptoString = cryptoString;
                obj.GTIN = cryptoString.Substring(2, 14);
                obj.SerialNo = cryptoString.Substring(18, 13);
                obj.PublicKey = cryptoString.Substring(34, 4);
                obj.CryptoCode = cryptoString.Substring(41, cryptoString.Length - 41);
            }
            catch (Exception)
            {
                return null;
            }
            return obj;
        }

        //#region EXPORT

        //public class CRPT_ERROR_Response
        //{
        //    public IList<string> globalErrors { get; set; }
        //    public bool success { get; set; }
        //}

        ////public EXPORT_UTILISATION_RESPONSE EXPORT_UTILISATION(Models.Job.Job selectedJob, Models.Customer.M_Customer selectedCustomer, List<string> lstSerialNo)
        ////{
        ////    lstExecutionStatus.Clear();
        ////    EXPORT_UTILISATION_RESPONSE UTILISATION_RESPONSE = null;
        ////    try
        ////    {
        ////        EXPORT_UTILISATION_URL = selectedCustomer.APIUrl.Trim('/') + EXPORT_UTILISATION_URL.Replace("{omsId}", selectedCustomer.OmsId);

        ////        var url = new Uri(EXPORT_UTILISATION_URL);

        ////        int RUSSIA_UTILISATION_EXPORT_SIZE = 150000;

        ////        using (var client = new WebClient())
        ////        {
        ////            client.Headers[HttpRequestHeader.ContentType] = MEDIA_TYPE;
        ////            client.Headers["clientToken"] = selectedCustomer.APIKey.Trim();

        ////            //   BODY SAMPLE
        ////            // { "sntins":[ "SNTIN1", "SNTIN2" ], "usageType":"USED_FOR_PRODUCTION", "expirationDate":"16-09-2020", "orderType":"1",
        ////            //   "ownerId":"050a1fd0-572c-44e0-8932-de487f8085c8", "seriesNumber":"123", "subjectId":"37ce5ee6-ad11-4708-b00e-d934fe97ec4d" }

        ////            EXPORT_UTILISATION_BODY uTILISATION_BODY = new EXPORT_UTILISATION_BODY();

        ////            uTILISATION_BODY.sntins = lstSerialNo;
        ////            uTILISATION_BODY.usageType = "VERIFIED"; //"USED_FOR_PRODUCTION"; 
        ////                                                     //For the pharmacy industry only VERIFIED is used
        ////                                                     // uTILISATION_BODY.expirationDate = selectedJob.ExpDate.ToString("dd-MM-yyyy");
        ////            uTILISATION_BODY.expirationDate = selectedJob.ExpDate.ToString("dd.MM.yyyy"); //NEW UPDATE ON 29.05.2020
        ////            uTILISATION_BODY.seriesNumber = selectedJob.BatchNo.ToString();

        ////            //TILISATION_BODY.subjectId = Guid.NewGuid().ToString();
        ////            uTILISATION_BODY.subjectId = selectedCustomer.ControlID;

        ////            //uTILISATION_BODY.orderType = "1";// (1) own, (2) contract.
        ////            //uTILISATION_BODY.ownerId = selectedCustomer.OmsId;
        ////            uTILISATION_BODY.controlId = selectedCustomer.ControlID;
        ////            uTILISATION_BODY.packingId = selectedCustomer.ControlID;

        ////            var jsonuTILISATION_BODY = JsonConvert.SerializeObject(uTILISATION_BODY);
        ////            DataLayer.ExceptionHandler.ExceptionLogger.LogError("UTILISATION UPLOAD : " + jsonuTILISATION_BODY.ToString());

        ////            //try
        ////            //{
        ////            //    var result = client.UploadString(url, "POST", jsonuTILISATION_BODY);
        ////            //    UTILISATION_RESPONSE = (EXPORT_UTILISATION_RESPONSE)JsonConvert.DeserializeObject(result, typeof(EXPORT_UTILISATION_RESPONSE));
        ////            //}
        ////            //catch (Exception ex)
        ////            //{
        ////            //    DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);
        ////            //    lstExecutionStatus.Add(new ExecutionStatus
        ////            //    {
        ////            //        IsSuccess = false,
        ////            //        Message = "ERROR EXPORT_UTILISATION(1)" + ex.Message
        ////            //    });
        ////            //}


        ////            #region NEW METHOD
        ////            using (HttpClient confClient = new HttpClient())
        ////            {
        ////                confClient.Timeout = TimeSpan.FromMinutes(5);

        ////                //var RequestBody_JSON = JsonConvert.SerializeObject(jsonuTILISATION_BODY);


        ////                HttpContent BodyContent = new StringContent(jsonuTILISATION_BODY, Encoding.UTF8, "application/json");
        ////                //                        confClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", DENTSU_TOKEN);
        ////                confClient.DefaultRequestHeaders.Add("clientToken", selectedCustomer.APIKey.Trim());
        ////                confClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(MEDIA_TYPE));

        ////                //string OriginalHash = CalculateMD5Hash(RequestBody_JSON);
        ////                //confClient.DefaultRequestHeaders.Add(X_ORIGINALHASH_HEADER, OriginalHash);

        ////                HttpResponseMessage httpResponseMessage = confClient.PostAsync(url, BodyContent).Result;

        ////                var inter = httpResponseMessage.Content.ReadAsStringAsync();

        ////                DataLayer.ExceptionHandler.ExceptionLogger.LogError("UPLOAD RESPONSE : " + inter.Result.ToString());


        ////                if (httpResponseMessage.IsSuccessStatusCode)
        ////                {
        ////                    UTILISATION_RESPONSE = (EXPORT_UTILISATION_RESPONSE)JsonConvert.DeserializeObject(inter.Result, typeof(EXPORT_UTILISATION_RESPONSE));
        ////                }
        ////                else
        ////                {
        ////                    try
        ////                    {
        ////                        var cRPT_ERROR = (CRPT_ERROR_Response)JsonConvert.DeserializeObject(inter.Result, typeof(CRPT_ERROR_Response));
        ////                        lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = false, Message = string.Join(",", cRPT_ERROR.globalErrors.ToList()) });
        ////                    }
        ////                    catch (Exception)
        ////                    {
        ////                        lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = false, Message = inter.Result });
        ////                    }
        ////                }
        ////            }

        ////            #endregion
        ////            //string reply ={ "omsId": "e5b51a8f-7ee2-40f7-abec-f33207a74636", "reportId": "487d8b2e-e962-4845-a3f2-0b11e034a6e0" }

        ////            if (UTILISATION_RESPONSE != null && UTILISATION_RESPONSE.omsId == selectedCustomer.OmsId && !string.IsNullOrEmpty(UTILISATION_RESPONSE.reportId))
        ////            {
        ////                DbHelper dbhelper = new DbHelper();

        ////                int isInserted = dbhelper.ExecuteQuery("INSERT INTO [dbo].[RSA_Utilisation] ([JobId],[UsageType],[ExpirationDate],[OwnerId],[SeriesNumber],[SubjectId],[ReportId],Count, UploadedOn)" +
        ////                                        " VALUES (" + selectedJob.JID + ", '" + uTILISATION_BODY.usageType + "', '" + selectedJob.ExpDate.Date.ToString("yyyy-MM-dd") + "','ownerId', '" + uTILISATION_BODY.seriesNumber + "', " +
        ////                                        " '" + uTILISATION_BODY.subjectId + "', '" + UTILISATION_RESPONSE.reportId + "', " + lstSerialNo.Count + ", GETDATE())");

        ////                UTILISATION_RESPONSE = (isInserted > 0) ? UTILISATION_RESPONSE : null;
        ////            }

        ////            return UTILISATION_RESPONSE;
        ////        }
        ////    }
        ////    catch (System.Exception ex)
        ////    {
        ////        DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);

        ////        lstExecutionStatus.Add(new ExecutionStatus
        ////        {
        ////            IsSuccess = false,
        ////            Message = "ERROR EXPORT_UTILISATION()" + ex.Message
        ////        });
        ////    }
        ////    return UTILISATION_RESPONSE;
        ////}

        //public EXPORT_AGGREGATION_RESPONSE EXPORT_AGGREGATION(Models.Job.Job selectedJob, Models.Customer.M_Customer selectedCustomer, List<Models.Job.JobDetails> selectedJobDetails, System.Data.DataSet dsAggregation)
        //{
        //    lstExecutionStatus.Clear();
        //    EXPORT_AGGREGATION_RESPONSE AGGREGATION_RESPONSE = null;
        //    try
        //    {
        //        EXPORT_AGGREGATION_URL = selectedCustomer.APIUrl.Trim('/') + EXPORT_AGGREGATION_URL.Replace("{omsId}", selectedCustomer.OmsId);

        //        var url = new Uri(EXPORT_AGGREGATION_URL);

        //        using (var client = new WebClient())
        //        {
        //            client.Headers[HttpRequestHeader.ContentType] = MEDIA_TYPE;
        //            client.Headers["clientToken"] = selectedCustomer.APIKey.Trim();

        //            //   BODY SAMPLE
        //            //   { "aggregationUnits": [{ "aggregatedItemsCount": 48, "aggregationType": "AGGREGATION", "aggregationUnitCapacity": 50, 
        //            //     "sntins": ["0100000848839984215LJ","0100000848839984215Py"], "unitSerialNumber": "1jdu7-kksb67-qwtg66" } ], "participantId": "string" }

        //            EXPORT_AGGREGATION_BODY aGGREGATION_BODY = new EXPORT_AGGREGATION_BODY();
        //            List<AggregationUnit> lstaggregationUnit = new List<AggregationUnit>();
        //            List<string> lstChildCode = new List<string>();

        //            // GET DISTINCT PARENTS
        //            DataTable dtParents = new DataView(dsAggregation.Tables[0]).ToTable(true, "NextLevelCode");

        //            for (int i = 0; i < dtParents.Rows.Count; i++)
        //            {
        //                AggregationUnit unit = new AggregationUnit();

        //                unit.unitSerialNumber = dtParents.Rows[i]["NextLevelCode"].ToString(); //The Identification Code of aggregation unit

        //                // GET CHILDS
        //                var row = dsAggregation.Tables[0].Select("NextLevelCode = '" + unit.unitSerialNumber + "'").ToArray();
        //                unit.sntins = row.Select(s => s[1].ToString()).ToList(); // Array of aggregated, GTIN+serial# 

        //                unit.aggregatedItemsCount = unit.sntins.Count;// Actual number of pieces in the aggregation unit
        //                unit.aggregationUnitCapacity = selectedJobDetails[1].JD_DeckSize;// Aggregation unit capacity

        //                unit.aggregationType = "AGGREGATION"; // Aggregation Type

        //                lstaggregationUnit.Add(unit);
        //            }

        //            aGGREGATION_BODY.aggregationUnits = lstaggregationUnit;
        //            aGGREGATION_BODY.participantId = selectedCustomer.OmsId;

        //            var jsonaGGREGATION_BODY = JsonConvert.SerializeObject(aGGREGATION_BODY);
        //            var result = client.UploadString(url, "POST", jsonaGGREGATION_BODY);

        //            //string reply ={ "omsId": "e5b51a8f-7ee2-40f7-abec-f33207a74636", "reportId": "487d8b2e-e962-4845-a3f2-0b11e034a6e0" }
        //            AGGREGATION_RESPONSE = (EXPORT_AGGREGATION_RESPONSE)JsonConvert.DeserializeObject(result, typeof(EXPORT_AGGREGATION_RESPONSE));

        //            if (AGGREGATION_RESPONSE != null && AGGREGATION_RESPONSE.omsId == selectedCustomer.OmsId && !string.IsNullOrEmpty(AGGREGATION_RESPONSE.reportId))
        //            {
        //                DbHelper dbhelper = new DbHelper();

        //                int isInserted = dbhelper.ExecuteQuery(" INSERT INTO [dbo].[RSA_Aggregation] ([JobId],[ReportId]) VALUES ("
        //                                                       + selectedJob.JID + ", '" + AGGREGATION_RESPONSE.reportId + "')");

        //                AGGREGATION_RESPONSE = (isInserted > 0) ? AGGREGATION_RESPONSE : null;
        //            }

        //            return AGGREGATION_RESPONSE;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);

        //        lstExecutionStatus.Add(new ExecutionStatus
        //        {
        //            IsSuccess = false,
        //            Message = "ERROR CRYPTO_CREATE_ORDER()" + ex.Message
        //        });
        //    }
        //    return AGGREGATION_RESPONSE;
        //}

        //public EXPORT_DROPOUT_RESPONSE EXPORT_DROPOUT(Models.Job.Job selectedJob, Models.Customer.M_Customer selectedCustomer, List<Models.Job.JobDetails> selectedJobDetails, System.Data.DataSet dsDropout)
        //{
        //    lstExecutionStatus.Clear();
        //    EXPORT_DROPOUT_RESPONSE DROPOUT_RESPONSE = null;
        //    try
        //    {
        //        EXPORT_DROPOUT_URL = selectedCustomer.APIUrl.Trim('/') + EXPORT_DROPOUT_URL.Replace("{omsId}", selectedCustomer.OmsId);

        //        var url = new Uri(EXPORT_DROPOUT_URL);

        //        using (var client = new WebClient())
        //        {
        //            client.Headers[HttpRequestHeader.ContentType] = MEDIA_TYPE;
        //            client.Headers["clientToken"] = selectedCustomer.APIKey.Trim();

        //            //   BODY SAMPLE
        //            //   { "dropoutReason": "DEFECT", "originMessageId": "string", "sntins": [ "010133456789433921i7RAGfsaWLTnA", "010133456789433921bRwqoIwHtxzoM" ],
        //            //     "sourceDocDate": "10.10.2010", "sourceDocNum": 235431, "subjectId": "string" }

        //            EXPORT_DROPOUT_BODY eXPORT_DROPOUT_BODY = new EXPORT_DROPOUT_BODY();

        //            eXPORT_DROPOUT_BODY.sntins = new List<string>();

        //            for (int i = 0; i < dsDropout.Tables[0].Rows.Count; i++)
        //            {
        //                eXPORT_DROPOUT_BODY.sntins.Add(dsDropout.Tables[0].Rows[i]["Code"].ToString());
        //            }

        //            eXPORT_DROPOUT_BODY.dropoutReason = "DEFECT";
        //            eXPORT_DROPOUT_BODY.originMessageId = "string";
        //            eXPORT_DROPOUT_BODY.sourceDocDate = "10.10.2010";
        //            eXPORT_DROPOUT_BODY.sourceDocNum = 12345;
        //            eXPORT_DROPOUT_BODY.subjectId = Guid.NewGuid().ToString();

        //            var jsoneXPORT_DROPOUT_BODY = JsonConvert.SerializeObject(eXPORT_DROPOUT_BODY);
        //            var result = client.UploadString(url, "POST", jsoneXPORT_DROPOUT_BODY);

        //            //string reply ={ "omsId": "e5b51a8f-7ee2-40f7-abec-f33207a74636", "reportId": "487d8b2e-e962-4845-a3f2-0b11e034a6e0" }
        //            DROPOUT_RESPONSE = (EXPORT_DROPOUT_RESPONSE)JsonConvert.DeserializeObject(result, typeof(EXPORT_DROPOUT_RESPONSE));

        //            if (DROPOUT_RESPONSE != null && DROPOUT_RESPONSE.omsId == selectedCustomer.OmsId && !string.IsNullOrEmpty(DROPOUT_RESPONSE.reportId))
        //            {
        //                DbHelper dbhelper = new DbHelper();

        //                int isInserted = dbhelper.ExecuteQuery(" INSERT INTO [dbo].[RSA_DropOut] ([JobId],[ReportId]) VALUES ("
        //                                                       + selectedJob.JID + ", '" + DROPOUT_RESPONSE.reportId + "')");

        //                DROPOUT_RESPONSE = (isInserted > 0) ? DROPOUT_RESPONSE : null;
        //            }

        //            return DROPOUT_RESPONSE;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DataLayer.ExceptionHandler.ExceptionLogger.logException(ex);

        //        lstExecutionStatus.Add(new ExecutionStatus
        //        {
        //            IsSuccess = false,
        //            Message = "ERROR CRYPTO_CREATE_ORDER()" + ex.Message
        //        });
        //    }
        //    return DROPOUT_RESPONSE;
        //}


        //#endregion

        public object ServerBulkUpdate(string Query, Dictionary<string, object> parameter, List<ExecutionStatus> lstExecutionStatus)
        {
            string connectionString = Utilities.getConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();

                    transaction = connection.BeginTransaction();

                    SqlCommand cmd = new SqlCommand(Query, connection, transaction);

                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var par in parameter)
                    {
                        cmd.Parameters.AddWithValue(par.Key, par.Value);
                    }
                    cmd.CommandTimeout = 0;

                    var output = cmd.ExecuteNonQuery();

                    transaction.Commit();

                    return output;
                }
                catch (Exception ex)
                {
                    lstExecutionStatus.Add(new ExecutionStatus { IsSuccess = false, Message = "ERRO :" + Query + "," + ex.Message });
                    transaction.Rollback();
                    return 0;
                }
            }
        }
    }

    #region CREATE ORDER TYPE

    public partial class CreateOrderInput
    {
        [JsonProperty("products")]
        public Product[] Products { get; set; }

        [JsonProperty("subjectId")]
        public string SubjectId { get; set; }

        [JsonProperty("paymentType")]
        public string paymentType { get; set; }

        [JsonProperty("freeСode")]
        public bool freeСode { get; set; }

    }

    public partial class Product
    {
        [JsonProperty("gtin")]
        public string Gtin { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("serialNumberType")]
        public string SerialNumberType { get; set; }

        [JsonProperty("serialNumbers")]
        public string[] SerialNumbers { get; set; }

        [JsonProperty("templateId")]
        public long TemplateId { get; set; }
    }

    public partial class CreateOrderResponse
    {
        [JsonProperty("omsId")]
        public string OmsId { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        public string SubjectId { get; set; }

        [JsonProperty("expectedCompleteTimestamp")]
        public long ExpectedCompleteTimestamp { get; set; }

        public string[] SerialNumbers { get; set; }
    }
    #endregion

    #region ORDER STATUS INFO RESPONSE
    public partial class OrderStatusInfo
    {
        [JsonProperty("omsId")]
        public Guid OmsId { get; set; }

        [JsonProperty("orderInfos")]
        public OrderInfo[] OrderInfos { get; set; }
    }

    public partial class OrderInfo
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("orderStatus")]
        public string OrderStatus { get; set; }

        [JsonProperty("createdTimestamp")]
        public long CreatedTimestamp { get; set; }

        [JsonProperty("totalQuantity")]
        public long TotalQuantity { get; set; }

        [JsonProperty("numOfProducts")]
        public long NumOfProducts { get; set; }

        [JsonProperty("productGroupType")]
        public string ProductGroupType { get; set; }

        [JsonProperty("buffers")]
        public Buffer[] Buffers { get; set; }

        [JsonProperty("showSignButton")]
        public bool ShowSignButton { get; set; }
    }

    public partial class Buffer
    {
        [JsonProperty("poolInfos")]
        public PoolInfo[] PoolInfos { get; set; }

        [JsonProperty("leftInBuffer")]
        public long LeftInBuffer { get; set; }

        [JsonProperty("poolsExhausted")]
        public bool PoolsExhausted { get; set; }

        [JsonProperty("totalCodes")]
        public long TotalCodes { get; set; }

        [JsonProperty("unavailableCodes")]
        public long UnavailableCodes { get; set; }

        [JsonProperty("availableCodes")]
        public long AvailableCodes { get; set; }

        [JsonProperty("orderId")]
        public Guid OrderId { get; set; }

        [JsonProperty("gtin")]
        public string Gtin { get; set; }

        [JsonProperty("bufferStatus")]
        public string BufferStatus { get; set; }

        [JsonProperty("rejectionReason")]
        public string RejectionReason { get; set; }

        [JsonProperty("totalPassed")]
        public long TotalPassed { get; set; }

        [JsonProperty("omsId")]
        public Guid OmsId { get; set; }
    }

    public partial class PoolInfo
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("leftInRegistrar")]
        public long LeftInRegistrar { get; set; }

        [JsonProperty("rejectionReason")]
        public string RejectionReason { get; set; }

        [JsonProperty("registrarId")]
        public string RegistrarId { get; set; }

        [JsonProperty("isRegistrarReady")]
        public bool IsRegistrarReady { get; set; }

        [JsonProperty("registrarErrorCount")]
        public long RegistrarErrorCount { get; set; }

        [JsonProperty("lastRegistrarErrorTimestamp")]
        public long LastRegistrarErrorTimestamp { get; set; }
    }
    #endregion

    #region DOWNLOAD CRYPTOCODE

    public partial class SplitedCryptoCode
    {
        public string CryptoString { get; set; }
        public string GTIN { get; set; }
        public string SerialNo { get; set; }
        public string PublicKey { get; set; }
        public string CryptoCode { get; set; }
    }

    public partial class DownloadCryptoResponse
    {
        [JsonProperty("omsId")]
        public Guid OmsId { get; set; }

        [JsonProperty("codes")]
        public string[] Codes { get; set; }

        [JsonProperty("blockId")]
        public string BlockId { get; set; }
    }
    #endregion

    #region EXPORT_UTILISATION
    public class EXPORT_UTILISATION_BODY
    {
        public string usageType { get; set; }
        public string expirationDate { get; set; }
        public string seriesNumber { get; set; }
        public string subjectId { get; set; }
        public List<string> sntins { get; set; }
        public string controlId { get; set; }
        public string packingId { get; set; }

        //public string ownerId { get; set; }
        //public string orderType { get; set; } //(1) own, (2) contract. OPTIONAL

    }

    public class EXPORT_UTILISATION_RESPONSE
    {
        public string omsId { get; set; }
        public string reportId { get; set; }
    }

    #endregion

    #region EXPORT_AGGREGATION
    public class AggregationUnit
    {
        public int aggregatedItemsCount { get; set; }
        public string aggregationType { get; set; }
        public int aggregationUnitCapacity { get; set; }
        public List<string> sntins { get; set; }
        public string unitSerialNumber { get; set; }
    }

    public class EXPORT_AGGREGATION_BODY
    {
        public List<AggregationUnit> aggregationUnits { get; set; }
        public string participantId { get; set; }
    }
    public class EXPORT_AGGREGATION_RESPONSE
    {
        public string omsId { get; set; }
        public string reportId { get; set; }
    }
    #endregion

    #region EXPORT_DROPOUT

    public class EXPORT_DROPOUT_BODY
    {
        public string dropoutReason { get; set; }
        public string originMessageId { get; set; }
        public List<string> sntins { get; set; }
        public string sourceDocDate { get; set; }
        public int sourceDocNum { get; set; }
        public string subjectId { get; set; }
    }
    public class EXPORT_DROPOUT_RESPONSE
    {
        public string omsId { get; set; }
        public string reportId { get; set; }
    }

    #endregion
    #region ERROR CLASS

    //public partial class ErrorResponse
    //{
    //    [JsonProperty("globalErrors")]
    //    public string[] GlobalErrors { get; set; }

    //    [JsonProperty("success")]
    //    public bool Success { get; set; }
    //}

    public class GlobalError
    {
        public string error { get; set; }
        public int errorCode { get; set; }
    }

    public class ErrorResponse
    {
        public bool success { get; set; }
        public List<GlobalError> globalErrors { get; set; }
    }


    #endregion

}