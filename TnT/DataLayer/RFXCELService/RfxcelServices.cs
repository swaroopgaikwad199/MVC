using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using TnT.Models.Code;
using TnT.Models.TraceLinkImporter;

namespace TnT.DataLayer.RFXCELService
{
    public class RfxcelServices
    {
        string UserName;
        string Password;
        List<string> errorList = new List<string>();
        public RfxcelServices()
        {
            UserName = Utilities.getAppSettings("RXLUserName");
            Password = Utilities.getAppSettings("RFXLPassword");
        }


        public List<X_TracelinkUIDStore> getSerialNumbersFromRfxcel(string APIUrl, string SendingSystem, string ReceivingSystem, int Size, string GTINValue, string GLN)
        {
           
            List<X_TracelinkUIDStore> listTLUIDStore = new List<X_TracelinkUIDStore>();
            try
            {

                var _url = APIUrl;
                M_Identities idnt = new M_Identities();
                List<X_Identities> listIds = new List<X_Identities>();
               
                string msg = string.Empty;
                XmlDocument soapEnvelopeXml = CreateSoapEnvelopeForSerialNumberRequest(SendingSystem, ReceivingSystem, Size, GTINValue, GLN);
                HttpWebRequest webRequest = CreateWebRequest(_url);
                ExceptionHandler.ExceptionLogger.LogError("After soapEnvelopeXml");
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
                ExceptionHandler.ExceptionLogger.LogError("After InsertSoapEnvelopeIntoWebRequest");
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                asyncResult.AsyncWaitHandle.WaitOne();

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
                        ExceptionHandler.ExceptionLogger.LogError("After xmlDS");
                        if (xmlDS.Tables.Contains("id"))
                        {
                            if (xmlDS.Tables["id"].Rows.Count == 0)
                            {
                                errorList.Add("UIDs Not Received.");
                            }
                        }
                        else
                        {
                            errorList.Add("UIDs Not Received.");
                        }

                        idnt.GTIN = GTINValue;
                        idnt.CreatedOn = DateTime.Now;
                        if (xmlDS.Tables.Contains("id"))
                        {
                            for (int i = 0; i < xmlDS.Tables["id"].Rows.Count; i++)
                            {
                                string sscc = xmlDS.Tables["id"].Rows[i].ItemArray[0].ToString();

                                X_TracelinkUIDStore TLUid = new X_TracelinkUIDStore();
                                TLUid.SerialNo = sscc;
                                TLUid.TLRequestId = -1;
                                TLUid.IsUsed = false;
                                TLUid.GTIN = idnt.GTIN;
                                listTLUIDStore.Add(TLUid);
                            }
                        }
                    }
                }
            
            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    string text = reader.ReadToEnd();
                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(text);
                    XmlNodeList nodelist = xd.GetElementsByTagName("ns1:msg");
                    if (nodelist.Count > 0)
                    {
                        errorList.Add(nodelist[0].InnerText);

                    }

                    XmlNodeList nodelist1 = xd.GetElementsByTagName("ns2:msg");
                    if(nodelist1.Count>0)
                    {
                        errorList.Add(nodelist1[1].InnerText);
                    }
                }
            }
            return listTLUIDStore;
        }

        public List<string> getErrors()
        {
            return errorList;
        }

        private HttpWebRequest CreateWebRequest(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

            //webRequest.Headers.Add("Authorization", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private XmlDocument CreateSoapEnvelopeForSerialNumberRequest(string SendingSystem, string ReceivingSystem, int Size, string GTINValue, string GLN)
        {
       
            string ld = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns=""http://xmlns.rfxcel.com/traceability/serializationService/3"" xmlns:ns1=""http://xmlns.rfxcel.com/traceability/3"" xmlns:xm=""http://www.w3.org/2004/11/xmlmime"">
	                        <soapenv:Header>
		                        <wsa:Action xmlns:wsa=""http://schemas.xmlsoap.org/ws/2004/08/addressing"">http://wsop.rfxcel.com/messaging/2/getMessages</wsa:Action>
		                        <wsa:ReplyTo xmlns:wsa=""http://schemas.xmlsoap.org/ws/2004/08/addressing"">
			                        <wsa:Address>http://schemas.xmlsoap.org/ws/2004/08/addressing/role/anonymous</wsa:Address>
		                        </wsa:ReplyTo>
		                        <wsa:To xmlns:wsa=""http://schemas.xmlsoap.org/ws/2004/08/addressing"">https://test.rfxcel.net/services/IrfxMessagingSoapHttpPort</wsa:To>
		                        <wsse:Security xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""> 
			                        <wsu:Timestamp wsu:Id=""TS-2"">
				                        <wsu:Created>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + @"</wsu:Created>
				                        <wsu:Expires>" + DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ") + @"</wsu:Expires>
			                        </wsu:Timestamp>
			                        <wsse:UsernameToken wsu:Id=""UsernameToken-1""> 
				                        <wsse:Username>" + UserName + @"</wsse:Username> 
				                        <wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">" + Password + @"</wsse:Password> 
			                        </wsse:UsernameToken> 
		                        </wsse:Security> 
	                        </soapenv:Header>
	                        <soapenv:Body>
		                        <ns:syncAllocateTraceIds contentStructVer=""3.1.3"" createDateTime=""" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + @""" requestId=""Allocate" + DateTime.Now.ToString("ddMMyyyyHHmmssffff") + @""">
			                        <ns:orgId qlfr=""ORG_DEF"">1234567891019</ns:orgId>
			                        <ns:eventId>PROPIX-SERIAL-NUMBER-REQUEST-" + DateTime.Now.ToString("ddMMyyyyHHmmssffff") + @"</ns:eventId>
			                        <ns:itemId qlfr=""GTIN"">" + GTINValue + @"</ns:itemId>
			                        <ns:siteHierId qlfr=""ORG_DEF"">Manufacturing</ns:siteHierId>
			                        <ns:siteId qlfr=""GLN"" type=""LOCATION"">" + ReceivingSystem + @"</ns:siteId>
			                        <ns:idTextFormat>PURE_ID_URI</ns:idTextFormat>
			                        <ns:separatePrefixSuffix>true</ns:separatePrefixSuffix>
			                        <ns:returnDataStruct>LIST</ns:returnDataStruct>
			                        <ns:idCount>" + Size + @"</ns:idCount>
		                        </ns:syncAllocateTraceIds>
	                        </soapenv:Body>
                        </soapenv:Envelope>";


            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(ld);
            return soapEnvelop;
        }

        private void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }
}