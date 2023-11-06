using System;
using System.IO;
using System.Net;
using System.Text;
using TnT.Models;
using TnT.Models.AS2;

namespace TnT.DataLayer.AS2
{
    public struct ProxySettings
    {
        public string Name;
        public string Username;
        public string Password;
        public string Domain;
    }
    public class AS2Sender
    {
        private ApplicationDbContext db ;

        public AS2Sender()
        {
            db = new ApplicationDbContext();
        }
        public HttpStatusCode uploadDataToAS2Server(string epcisfilePath, int AS2ServerId)
        {
            //string fileName = "LOT222_v1.1_17_04_2018 21_33_35.xml";
            //byte[] fileData = File.ReadAllBytes(fileName);
            //string from = "Propix";
            //string to = "RisingPharma";
            //Uri actionUrl2 = new Uri("https://serial.risingpharma.com:8080/as2/receive");

            M_ServersAS2 serverDetails = db.M_ServersAS2.Find(AS2ServerId);           
            byte[] fileData = File.ReadAllBytes(epcisfilePath);
            string from = serverDetails.FromName;
            string to = serverDetails.ToName;
            Uri actionUrl2 = new Uri(serverDetails.HostAddress);
           
           return SendFile(serverDetails, epcisfilePath, fileData, new ProxySettings(), 300000);

        }

        private static HttpStatusCode SendFile(M_ServersAS2 serverDetails, string filename, byte[] fileData, ProxySettings proxySettings, int timeoutMs)
        {
            if (String.IsNullOrEmpty(filename)) throw new ArgumentNullException("filename");

            if (fileData.Length == 0) throw new ArgumentException("filedata");

            byte[] content = fileData;

            //Initialise the request
            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(new Uri(serverDetails.HostAddress));

            if (!String.IsNullOrEmpty(proxySettings.Name))
            {
                WebProxy proxy = new WebProxy(proxySettings.Name);

                NetworkCredential proxyCredential = new NetworkCredential();
                proxyCredential.Domain = proxySettings.Domain;
                proxyCredential.UserName = proxySettings.Username;
                proxyCredential.Password = proxySettings.Password;

                proxy.Credentials = proxyCredential;

                http.Proxy = proxy;
            }

            //Define the standard request objects
            http.Method = "POST";

            http.AllowAutoRedirect = true;

            http.KeepAlive = true;

            http.PreAuthenticate = false; //Means there will be two requests sent if Authentication required.
            http.SendChunked = false;

            http.UserAgent = "PROPIX SENDING AGENT";

            //These Headers are common to all transactions
            http.Headers.Add("Mime-Version", "1.0");
            http.Headers.Add("AS2-Version", "1.2");

            http.Headers.Add("AS2-From", serverDetails.FromName);
            http.Headers.Add("AS2-To", serverDetails.ToName);
            http.Headers.Add("Subject", filename + " transmission.");
            http.Headers.Add("Message-Id", "<PROPIX_AS2_" + DateTime.Now.ToString("hhmmssddd") + ">");


            http.Headers.Add("Disposition-notification-options", "signed-receipt-protocol=optional,pkcs7-signature;signed-receipt-micalg=optional,sha1");
            //http.Headers.Add("User-Agent", "Paul Salvi Dev");


            http.Timeout = timeoutMs;

            string contentType = (Path.GetExtension(filename) == ".xml") ? "application/xml" : "application/EDIFACT";

            bool encrypt = !string.IsNullOrEmpty(serverDetails.HostPublicKeyPath);
            bool sign = !string.IsNullOrEmpty(serverDetails.SelfPrivateKeyPath );

            if (!sign && !encrypt)
            {
                http.Headers.Add("Content-Transfer-Encoding", "binary");
                http.Headers.Add("Content-Disposition", "inline; filename=\"" + filename + "\"");
            }
            if (sign)
            {
                // Wrap the file data with a mime header
                content = AS2MIMEUtilities.CreateMessage(contentType, "binary", "", content);

                content = AS2MIMEUtilities.Sign(content, serverDetails.SelfPrivateKeyPath, serverDetails.SelfPrivateKeyPassword, out contentType);

                http.Headers.Add("EDIINT-Features", "multiple-attachments");

            }
            if (encrypt)
            {
                if (string.IsNullOrEmpty(serverDetails.HostPublicKeyPath))
                {
                    throw new ArgumentNullException(serverDetails.HostPublicKeyPath, "if encrytionAlgorithm is specified then recipientCertFilename must be specified");
                }

                byte[] signedContentTypeHeader =  ASCIIEncoding.ASCII.GetBytes("Content-Type: " + contentType + Environment.NewLine);
                byte[] contentWithContentTypeHeaderAdded = AS2MIMEUtilities.ConcatBytes(signedContentTypeHeader, content);

                content = AS2Encryption.Encrypt(contentWithContentTypeHeaderAdded, serverDetails.HostPublicKeyPath, EncryptionAlgorithm.DES3);


                contentType = "application/pkcs7-mime; smime-type=enveloped-data; name=\"smime.p7m\"";
            }

            http.ContentType = contentType;
            http.ContentLength = content.Length;

            SendWebRequest(http, content);

            return HandleWebResponse(http);
        }

        private static HttpStatusCode HandleWebResponse(HttpWebRequest http)
        {
            HttpWebResponse response = (HttpWebResponse)http.GetResponse();

            //WebHeaderCollection header = response.Headers;
            //var encoding = ASCIIEncoding.ASCII;
            //using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            //{
            //    string responseText = reader.ReadToEnd();
            //    Console.WriteLine(response.StatusCode);
            //    Console.WriteLine(responseText);
            //}

            response.Close();
            return response.StatusCode;
        }

        private static void SendWebRequest(HttpWebRequest http, byte[] fileData)
        {
            Stream oRequestStream = http.GetRequestStream();
            oRequestStream.Write(fileData, 0, fileData.Length);
            oRequestStream.Flush();
            oRequestStream.Close();
        }
    }
}