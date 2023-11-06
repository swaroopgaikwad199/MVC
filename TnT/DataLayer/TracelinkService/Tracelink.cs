using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using TnT.Models.Code;
using TnT.Models.TraceLinkImporter;

namespace TnT.DataLayer.TracelinkService
{
    public enum TracelinkImportErrors
    {


    }
    public class Tracelink
    {
        string UserName;
        string Password;
        List<string> errorList = new List<string>();
        public Tracelink()
        {
            UserName = Utilities.getAppSettings("TraceLinkUserName").ToString();
            Password = Utilities.getAppSettings("TraceLinkPassword").ToString();
        }


        public List<X_TracelinkUIDStore> getDataFromTracelink(string APIUrl, string SendingSystem, string ReceivingSystem, int Size, string GTINValue, string type, string CompanyCode, string filterValue)
        {
            try
            {
                string token = Utilities.getAppSettings("TracelinkAuthCode");
                var _url = APIUrl;
                var _action = "Basic " + token;
                M_Identities idnt = new M_Identities();
                List<X_Identities> listIds = new List<X_Identities>();
                List<X_TracelinkUIDStore> listTLUIDStore = new List<X_TracelinkUIDStore>();
                string msg = string.Empty;

                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(SendingSystem, ReceivingSystem, Size, GTINValue, type, CompanyCode, filterValue);
                HttpWebRequest webRequest = CreateWebRequest(_url, _action);

                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                NetworkCredential netCredential = new NetworkCredential(UserName, Password);
                Uri uri = new Uri(_url);
                ICredentials credentials = netCredential.GetCredential(uri, "Basic");
                //service.Credentials = credentials;

                webRequest.Credentials = credentials;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                // begin async call to web request.
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                // suspend this thread until call is complete. You might want to
                // do something usefull here like update your UI.
                asyncResult.AsyncWaitHandle.WaitOne();

                // get the response from the completed web request.
                DataSet xmlDS = new DataSet();
                XmlTextReader reader = null;
                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        System.IO.StringReader xmlSR = new System.IO.StringReader(rd.ReadLine());
                        soapResult = rd.ReadToEnd();
                        soapResult = rd.ReadToEnd();
                        reader = new XmlTextReader(xmlSR);
                        xmlDS.ReadXml(reader);

                        if (xmlDS.Tables.Contains("RfidException"))
                        {
                            if (xmlDS.Tables["RfidException"].Rows.Count > 0)
                            {
                                foreach (DataRow err in xmlDS.Tables["RfidException"].Rows)
                                {
                                    errorList.Add(err["ErrorMessage"].ToString());

                                }

                            }
                        }

                        DataSet ds = new DataSet();
                        if (type != "SSCC" && type != "LSSCC")
                        {
                            idnt.GTIN = xmlDS.Tables[3].Rows[0].ItemArray[1].ToString();
                        }
                        else
                        {
                            idnt.GTIN = GTINValue;
                        }
                        idnt.CreatedOn = DateTime.Now;
                        if (xmlDS.Tables.Contains("SerialNo"))
                        {
                            for (int i = 0; i < xmlDS.Tables["SerialNo"].Rows.Count; i++)
                            {
                                string sscc1 = xmlDS.Tables["SerialNo"].Rows[i].ItemArray[0].ToString();
                                string sscc = "";
                                if (type != "SSCC" && type != "LSSCC")
                                {
                                    sscc = sscc1.Substring(18);
                                }
                                else
                                {
                                    sscc = sscc1.Substring(2);
                                }

                                //X_Identities TUID = new X_Identities();
                                //TUID.MasterId = 1;
                                //TUID.SerialNo = sscc;
                                //TUID.CodeType = false;
                                //TUID.IsTransfered = false;
                                //listIds.Add(TUID);

                                X_TracelinkUIDStore TLUid = new X_TracelinkUIDStore();
                                TLUid.SerialNo = sscc;
                                TLUid.TLRequestId = -1;
                                TLUid.IsUsed = false;
                                TLUid.GTIN = idnt.GTIN;
                                TLUid.Type = type;
                                listTLUIDStore.Add(TLUid);
                            }
                        }
                        else if (xmlDS.Tables.Contains("RandomizedNumberList"))  //Added By Kiran
                        {
                            for (int i = 0; i < xmlDS.Tables["RandomizedNumberList"].Rows.Count; i++)
                            {
                                string sscc1 = xmlDS.Tables["RandomizedNumberList"].Rows[i].ItemArray[0].ToString();
                                string sscc = sscc1.Substring(18);

                                X_TracelinkUIDStore TLUid = new X_TracelinkUIDStore();
                                TLUid.SerialNo = sscc;
                                TLUid.TLRequestId = -1;
                                TLUid.IsUsed = false;
                                TLUid.GTIN = idnt.GTIN;
                                TLUid.Type = type;
                                listTLUIDStore.Add(TLUid);
                            }
                        }
                    }
                }
                return listTLUIDStore;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<string> getErrors()
        {
            return errorList;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

            webRequest.Headers.Add("Authorization", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(string SendingSystem, string ReceivingSystem, int Size, string GTINValue, string type, string CompanyCode, string filterValue)
        {
            string ld = "";
            string SoapTLRq = Utilities.getAppSettings("SoapTLRq");

            if (SoapTLRq == "2017")
            {
                if (type == "SSCC" || type == "LSSCC")
                {
                    ld = @"<x:Envelope xmlns:x=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:tracelink:soap"" xmlns:snx=""http://tracelink.com/snx"">
                          <x:Header/>
                          <x:Body>
                           <urn:serialNumbersRequest>
                            <SendingSystem>" + SendingSystem + "</SendingSystem>" +
                               "<ReceivingSystem>" + ReceivingSystem + "</ReceivingSystem>" +
                               "<IdType>GS1_SER</IdType>" +
                                "<EncodingType>SSCC</EncodingType>" +
                                 "<Size>" + Size + "</Size>" +
                                 "<ObjectKey>" +
                                  "<Name>COMPANY_PREFIX</Name>" +
                                  "<Value>" + CompanyCode + "|" + filterValue + "</Value>" +
                                  "</ObjectKey>" +
                                  "</urn:serialNumbersRequest>" +
                                    "</x:Body></x:Envelope>";
                }
                else
                {
                    ld = @"<x:Envelope xmlns:x=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:tracelink:soap"" xmlns:snx=""http://tracelink.com/snx"">
                                 <x:Header/>
                                 <x:Body> 
                                  <urn:serialNumbersRequest>
                                   <SendingSystem>" + SendingSystem + "</SendingSystem>" +
                                    "<ReceivingSystem>" + ReceivingSystem + "</ReceivingSystem>" +
                                    "<IdType>GS1_SER</IdType>" +
                                     "<EncodingType>SGTIN</EncodingType>" +
                                      "<Size>" + Size + "</Size>" +
                                      "<ObjectKey>" +
                                       "<Name>" + type + "</Name>" +
                                       "<Value>" + GTINValue + "</Value>" +
                                       "</ObjectKey>" +
                                        "<RequestRandomizedNumber></RequestRandomizedNumber>" +
                                         "</urn:serialNumbersRequest>" +
                                         "</x:Body></x:Envelope>";
                }
            }
            else if (SoapTLRq == "2018")
            {
                if (type == "SSCC" || type == "LSSCC")
                {
                    ld = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:tracelink:soap"">" +
                              "<soapenv:Header/>" +
                              "<soapenv:Body>" +
                              "<urn:serialNumbersRequest>" +
                              "<SendingSystem>" + SendingSystem + "</SendingSystem >" +
                              "<ReceivingSystem >" + ReceivingSystem + "</ReceivingSystem >" +
                              "<IdType>GS1_SER</IdType>" +
                              "<EncodingType>SGTIN</EncodingType>" +
                              "<Size>" + Size + "</Size>" +
                              "<ObjectKey>" +
                                  "<Name>COMPANY_PREFIX</Name>" +
                                  "<Value>" + CompanyCode + "|" + filterValue + "</Value>" +
                              "</ObjectKey> " +
                            "</urn:serialNumbersRequest> " +
                           "</soapenv:Body> " +
                           "</soapenv:Envelope>";
                }
                else
                {
                    ld = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:urn=""urn:tracelink:soap"">" +
                              "<soapenv:Header/>" +
                              "<soapenv:Body>" +
                              "<urn:serialNumbersRequest>" +
                              "<SendingSystem>" + SendingSystem + "</SendingSystem >" +
                              "<ReceivingSystem >" + ReceivingSystem + "</ReceivingSystem >" +
                              "<IdType>GS1_SER</IdType>" +
                              "<EncodingType>SGTIN</EncodingType>" +
                              "<Size>" + Size + "</Size>" +
                              "<ObjectKey>" +
                                  "<Name>" + type + "</Name>" +
                                  "<Value>" + GTINValue + "</Value>" +
                              "</ObjectKey> " +
                            "</urn:serialNumbersRequest> " +
                           "</soapenv:Body> " +
                           "</soapenv:Envelope>";
                }
            }

            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(ld);
            return soapEnvelop;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

    }
}