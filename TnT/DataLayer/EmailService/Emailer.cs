using TnT.Models.Job;
using TnT.Models.Product;
using TnT.Models.Vendor;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace TnT.DataLayer.EmailService
{
    public class Emailer
    {
        private string FromName;
        private string FromEmail;
        private string SiteURL;
        private string Host;
        private string Port;
        private string Username;
        private string Password;

        public string Composer(PackagingAsso pa, M_Vendor ven, Job job)
        {
            try
            {
                AppSettingsReader MyReader = new AppSettingsReader();
                SiteURL = MyReader.GetValue("SiteUrl", typeof(string)).ToString();
                string APIURl = SiteURL;

                string data = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
<html>
<head>
<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
<title>Notification</title>
</head>

<body style='background-color:#f5f5f5; font-family:Helvetica,Arial,sans-serif; font-size:14px; line-height:24px; color:#363B3E;'>
<table border='0' cellspacing='0' cellpadding='0' style='background-color:#FFF; width:660px; margin:0 auto;  border:1px solid #ebebeb;'>
  <tr >
    <td colspan='3' style='padding:10px 0; text-align:center; background-color:#aaa;'><img style='width:180px' src='http://www.propixtech.com/images/logo_tr.png'  /></td>
  </tr>
  <tr>
    <td style='padding:20px; padding-bottom:0;' colspan='3'>
    <h2 style='font-size:20px; font-weight:normal;'>LLC Printing Job Information</h2>
    <p style='margin-bottom:3px; padding-bottom:0;'>Hello " + ven.ContactPerson + ", </p> ";

                data += @" </td>
  </tr>
  <tr>
    <td style='padding:0 20px;' colspan='3'>
    <div style='padding-top:15px!important;padding-bottom:15px!important;margin-top:15px;margin-bottom:15px;background:#f5f5f5'>
    <table style='border-spacing:0;border-collapse:collapse;vertical-align:top;text-align:left;width:100%;padding:0'>
        <tbody><tr style='vertical-align:top;text-align:left;padding:0' align='left'>
        	<td style='word-break:break-word;border-collapse:collapse!important;vertical-align:top;text-align:left;color:#505050;font-family:Helvetica,Arial,sans-serif;font-weight:normal;line-height:19px;font-size:14px;margin:0;padding:0 20px' align='left' valign='top'>
                <label style='font-weight:bold;'>API URL :</label> " + APIURl + " <br/>";

                data += @"	<label style='font-weight:bold;'>Service Key    :</label> " + ven.ServiceKey + " <br/>";
                data += @"	<label style='font-weight:bold;'>PAID           :</label> " + job.PAID + " <br/>";
                data += @"	<label style='font-weight:bold;'>Job ID         :</label> " + job.JID + "  <br/> ";
                data += @"	<label style='font-weight:bold;'>Batch No       :</label> " + job.BatchNo + " <br/>";
                data += @"	<label style='font-weight:bold;'>Mfg Date       :</label> " + job.MfgDate.ToString("MM/dd/yyyy") + " <br/>";
                data += @"	<label style='font-weight:bold;'>Exp Date       :</label> " + job.ExpDate.ToString("MM/dd/yyyy") + "  <br/> ";

                data += @" </td>
        </tr>
    </tbody></table>
</div>
    </td>
  </tr>
  <tr>
    <td width='270' style='padding:0 30px; color:#EA4335; text-align:center;'>
    
    </td>
  </tr>
 
  <tr>
    <td style='padding:0 30px 30px 30px;' colspan='3'>
    
    <h3 style='margin-bottom:2px; padding:0;'>Thanks</h3>    
    The <a href='http://www.propixtech.com' target='_blank'>Propix Technologies</a>  
    </td>
  </tr>
  <tr>
    <td style=' background-color:#f5f5f5; text-align:center; font-size:12px; padding:12px 0;' colspan='3'>
© 2013 - 2016 Propix Technologies. All rights reserved</td>
  </tr>
</table>

</body>
</html>
";
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void getCredentials()
        {
            try
            {
                AppSettingsReader MyReader = new AppSettingsReader();

                Host = Utilities.getAppSettings("SMTPHost"); //MyReader.GetValue("Host", typeof(string)).ToString();
                Port = Utilities.getAppSettings("SMTPPort");  //MyReader.GetValue("Port", typeof(string)).ToString();
                Username = Utilities.getAppSettings("SMTPUser");  //MyReader.GetValue("User", typeof(string)).ToString();
                Password = Utilities.getAppSettings("SMTPPassword");  //MyReader.GetValue("SMTPPass", typeof(string)).ToString();
                FromName = Utilities.getAppSettings("SMTPFromName");//MyReader.GetValue("SMTPFromName", typeof(string)).ToString();
                FromEmail = Utilities.getAppSettings("SMTPFromEmail");//MyReader.GetValue("SMTPFromEmail", typeof(string)).ToString();

            }
            catch (Exception)
            {
                throw;
            }
         
        }

        public void sendMail(string EmailContent, string Subject, string ToName, string ToEmailAddress, bool IsHtml,string BccEmailId,string BccName)
        {
            try
            {
                getCredentials();
                MailMessage mailMsg = new MailMessage();
                // To
                mailMsg.To.Add(new MailAddress(ToEmailAddress, ToName));
                if (BccEmailId != "")
                {
                    mailMsg.Bcc.Add(new MailAddress(BccEmailId, BccName));
                }
                // From
                mailMsg.From = new MailAddress(FromEmail, FromName);
                //mailMsg.From = new MailAddress("tnt@propixtech.com", "Propix TnT");
                // Subject and multipart/alternative Body
                mailMsg.Subject = Subject;
                if (IsHtml)
                {
                    string html = EmailContent;
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
                }
                else
                {
                    string text = EmailContent;
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                }
            
                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient(Host, Convert.ToInt32(Port));
                NetworkCredential credentials = new NetworkCredential(Username,Password);
                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);


            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}