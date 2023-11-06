using TnT.Models;
using TnT.Models.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace TnT.DataLayer.LicenseChecker
{
    public class Validator
    {
        public enum ResponderResult
        {
            Fail = 0,
            Success = 1,
            LicenseExpired = 2,
            LicenseSuspended = 3,
            NoResponse = 4
        };

    

        public int ReadLicense()
        {
            try
            {
                string VerifyLic = "http://www.propixtech.com/license/verify";
                string LicNo = getLicense();
                string baseURL = GetSiteUrl();

                var http = (HttpWebRequest)WebRequest.Create(VerifyLic);
                string postData = "LicNo=" + LicNo + "&BaseURL=" + baseURL + "";

                byte[] send = Encoding.Default.GetBytes(postData);
                http.Method = "POST";
                http.ContentType = "application/x-www-form-urlencoded";
                http.ContentLength = send.Length;
                http.Proxy = null;
                http.Headers.Add("Authorization", "Basic dcmGV25hZFzc3VudDM6cGzdCdvQ=");
                Stream sout = http.GetRequestStream();
                sout.Write(send, 0, send.Length);
                sout.Flush();
                sout.Close();

                WebResponse res = http.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream());
                string returnvalue = sr.ReadToEnd();

                return Convert.ToInt32(returnvalue);
            }
            catch (Exception)
            {

                throw;
            }
          

        }

        private string GetSiteUrl()
        {
            try
            {
                string url = string.Empty;
                HttpRequest request = HttpContext.Current.Request;
                if (request.IsSecureConnection)
                {
                    url = "https://";
                }
                else
                {
                    url = "http://";
                    url += request["HTTP_HOST"] + "/";
                }
                return url;
            }
            catch (Exception)
            {
                throw;
            }

        }
    
        private string getLicense()
        {
            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                string lic = db.License.Select(m => m.LicenseNo).FirstOrDefault();
                return lic;
            }
            catch (Exception)
            {

                throw;
            }
         
            
          
        }



    }  
}
