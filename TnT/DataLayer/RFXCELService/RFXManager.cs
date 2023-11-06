using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using TnT.DataLayer.EPCISHelper;
using TnT.RFXProcessMsgService;

namespace TnT.DataLayer.RFXCELService
{
    public class RFXManager
    {
        string UserName;
        string Password;
        HttpWebRequest webRequest;
        string soapResult;
        List<string> errorList = new List<string>();
        public RFXManager()
        {
            UserName = Utilities.getAppSettings("RXLUserName");
            Password = Utilities.getAppSettings("RFXLPassword"); ;
        }
        public string sendData(string fpath, string url, string bizgln, int custid)
        {
            string msg = "";
            EPCISConfig epcisConf = new EPCISConfig();
            XmlDocument xd = new XmlDocument();
            xd.Load(fpath);

            XmlDocument soapEnvelopeXml = CreateSoapEnvelopeForProcessMessages(xd, bizgln,epcisConf.GetEPCGLN(custid));
            var _url = Utilities.getAppSettings("RFXLEPCISUploadUrl");

            var _action = "http://xxxxxxxx/Service1.asmx?op=HelloWorld";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                webRequest = CreateWebRequest(_url, _action);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

                // begin async call to web request.
                IAsyncResult asyncResult = webRequest.BeginGetResponse(new AsyncCallback(FinishWebRequest), null);

                // suspend this thread until call is complete. You might want to
                // do something usefull here like update your UI.
                asyncResult.AsyncWaitHandle.WaitOne();

                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(soapResult);

                XmlNodeList nodelist = xdoc.GetElementsByTagName("ns2:msg");
                if (nodelist.Count > 0)
                {
                    msg = nodelist[0].InnerXml;
                }
                XmlNodeList nodelist1 = xdoc.GetElementsByTagName("ns3:msg");
                if (nodelist1.Count > 0)
                {
                    msg = nodelist1[1].InnerXml;
                }
            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    string text = reader.ReadToEnd();
                    xd.LoadXml(text);
                    msg = xd.InnerText;
                }
            }


            return msg;

        }


        public List<string> getErrors()
        {
            return errorList;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            //webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        void FinishWebRequest(IAsyncResult result)
        {
            WebResponse webResponse = webRequest.EndGetResponse(result);
            using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
            {
                soapResult = rd.ReadToEnd();
            }
            Console.Write(soapResult);
        }

        private XmlDocument CreateSoapEnvelopeForProcessMessages(XmlDocument xd, string GLN, string SGLN)
        {
            string xml = xd.InnerXml;
            string ld = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<SOAP-ENV:Envelope xmlns:ns3=""http://xmlns.rfxcel.com/traceability/3"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"" xmlns:ns0=""http://xmlns.rfxcel.com/traceability/api/3"" xmlns:ns1=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns2=""http://xmlns.rfxcel.com/traceability/messagingService/3"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"">
	<SOAP-ENV:Header xmlns:wsa=""http://schemas.xmlsoap.org/ws/2004/08/addressing"" xsi:schemaLocation=""http://schemas.xmlsoap.org/ws/2004/08/addressing ../SOAP-Security/addressing.xsd"">
		<wsse:Security mustUnderstand=""true"">
			<wsse:UsernameToken>
				<wsse:Username>" + UserName + @"</wsse:Username>
				<wsse:Password>" + Password + @"</wsse:Password>
				<wsu:Created>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ffffffzzz") + @"</wsu:Created>
			</wsse:UsernameToken>
		</wsse:Security>
	</SOAP-ENV:Header>
	<ns1:Body>
		<ns2:processMessages contentStructVer=""3.1.3"" createDateTime=""" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + @""" requestId=""WSCALL033" + DateTime.Now.ToString("ddMMyyyyyHHMMssffff") + @""">
			<ns2:msgEnvelopeList>
				<ns0:envelope>
					<ns0:header>
						<ns0:msgInfo>
							<ns0:sysDeploymentId xsi:type=""ns3:QualifiedIdType"" qlfr=""SYS_DEF"">" + GLN + @"</ns0:sysDeploymentId>
							<ns0:senderInstId xsi:type=""ns3:QualifiedIdType"" qlfr=""SYS_DEF"">" + SGLN + @"</ns0:senderInstId>
							<ns0:senderId xsi:type=""ns3:TypedQualifiedIdType"" qlfr=""ORG_DEF"" type=""ORG_ID"">" + GLN + @"</ns0:senderId>
							<ns0:receiverInstId xsi:type=""ns3:QualifiedIdType"" qlfr=""SYS_DEF"">rfXcel</ns0:receiverInstId>
							<ns0:receiverId xsi:type=""ns3:TypedQualifiedIdType"" qlfr=""ORG_DEF"" type=""ORG_ID"">1</ns0:receiverId>
							<ns0:msgFormat xsi:type=""ns3:OptionallyVersionedEnumType"">XML</ns0:msgFormat>
							<ns0:msgType xsi:type=""ns3:OptionallyVersionedEnumType"" ver=""1.1"">SYS_EVENTS_ENV</ns0:msgType>
							<ns0:msgId/>
							<ns0:corrMsgId/>
						</ns0:msgInfo>
					</ns0:header>
					<ns0:body>
						<XML_SYS_EVENTS_ENV>
							" + xd.DocumentElement.OuterXml + @"
						</XML_SYS_EVENTS_ENV>
					</ns0:body>
				</ns0:envelope>
			</ns2:msgEnvelopeList>
		</ns2:processMessages>
	</ns1:Body>
</SOAP-ENV:Envelope>";
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