using REDTR.DB.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using TnT.DataLayer.EmailService;
using TnT.DataLayer.Security;
using TnT.Models;

namespace TnT.DataLayer.JobService
{
    public enum BatchEventType
    {
        batchCreationEvent,
        batchVerificationEvent,
        batchNotVerifyEvent,
        batchQtyRequested,
        productCreationEvent,
        productVerifyEvent,
        productNotVerifyEvent,
        SettingRestoreDbRequest,
        VerifyRestoreDbRequest

    }
    public class BatchNotificationHelper
    {
        bool IsNotificationEnabled = false;
        ApplicationDbContext db = new ApplicationDbContext();
        Emailer mailer = new Emailer();
        List<string> mailingList = new List<string>();
        string mailMessage = string.Empty;
        string mailSubject = string.Empty;

        public BatchNotificationHelper()
        {
            var isenl = Utilities.getAppSettings("EmailNotificationEnable");
            if (!string.IsNullOrEmpty(isenl))
            {
                try
                {
                    IsNotificationEnabled = Convert.ToBoolean(isenl);
                }
                catch (Exception)
                {

                    IsNotificationEnabled = false;
                }

            }
        }
        public void notifyPerimissionHolders(ref Job job, BatchEventType batchEvent, CustomPrincipal userData)
        {
            if (!IsNotificationEnabled) return;
            string bccmailid = Utilities.getAppSettings("BccEmailIdForVerifyRestoreDb");
            getEmailsIdsAndMailOfPermHolders(batchEvent, ref job, userData);
            if (mailingList.Count > 0)
            {
                foreach (var item in mailingList)
                {
                    if (IsValidEmail(item))
                    {
                        
                            mailer.sendMail(mailMessage, mailSubject, "Batch Manager", item, true, "", "");
                       
                    }

                }
            }
        }

    


        public void notifyPerimissionHoldersSettings(string filename, BatchEventType batchEvent, CustomPrincipal userData)
        {
            if (!IsNotificationEnabled) return;
            string bccmailid = Utilities.getAppSettings("BccEmailIdForVerifyRestoreDb");
            getEmailsIdsAndMailOfPermHoldersSettings(batchEvent, filename, userData);
            if (mailingList.Count > 0)
            {
                foreach (var item in mailingList)
                {
                    if (IsValidEmail(item))
                    {
                        if (batchEvent == TnT.DataLayer.JobService.BatchEventType.VerifyRestoreDbRequest)
                        {
                            mailer.sendMail(mailMessage, mailSubject, "Batch Manager", item, true, bccmailid, "Propix");
                        }
                        else
                        {
                            mailer.sendMail(mailMessage, mailSubject, "Batch Manager", item, true, "", "");
                        }
                    }

                }
            }
        }
        private void getEmailsIdsAndMailOfPermHoldersSettings(BatchEventType batchEvent,string filename, CustomPrincipal userData)
        {
            string permissionName = string.Empty;

            switch (batchEvent)
            {
                
                case BatchEventType.SettingRestoreDbRequest:
                    permissionName = TnT.LangResource.GlobalRes.ShwMsgJobServiceRestoreDbREq;
                    mailSubject = TnT.LangResource.GlobalRes.ShwMsgJobServiceRestoreDbREqGerated;
                    using (StreamReader readers = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/content/mailer/notification-email.html")))
                    {
                          
                        mailMessage = readers.ReadToEnd();
                         
                    }
                    mailMessage = mailMessage.Replace("{USER_NAME}", userData.FirstName);
                    mailMessage = mailMessage.Replace("{ACTIVITY_TYPE}", permissionName);
                    mailMessage = mailMessage.Replace("{DESCRIPTION}", "Restore DB Request generated for filename " + filename);
                    //mailMessage = "Restore DB Request generated for filename "+filename;
                    break;
                case BatchEventType.VerifyRestoreDbRequest:
                    permissionName = TnT.LangResource.GlobalRes.ShwMsgJobServiceRestoredbreqquest;
                    mailSubject = TnT.LangResource.GlobalRes.SettingDbResoreSuccessfuly;
                    using (StreamReader readers = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/content/mailer/notification-email.html")))
                    {

                        mailMessage = readers.ReadToEnd();

                    }
                    mailMessage = mailMessage.Replace("{USER_NAME}", userData.FirstName);
                    mailMessage = mailMessage.Replace("{ACTIVITY_TYPE}", permissionName);
                    mailMessage = mailMessage.Replace("{DESCRIPTION}", "Restore DB Request generated for filename " + filename);
                    //mailMessage = "Database Restore Successfully at Of File "+filename;
                    break;
            }
            mailingList = db.Database.SqlQuery<string>("up_GetUserEmailIdsbyPermName @permissionName", new SqlParameter("permissionName", permissionName)).ToList();

        }
        private void getEmailsIdsAndMailOfPermHolders(BatchEventType batchEvent, ref Job job, CustomPrincipal userData)
        {
            string permissionName = string.Empty;

            switch (batchEvent)
            {
                case BatchEventType.batchCreationEvent:
                    permissionName = TnT.LangResource.GlobalRes.JobsVerification;
                    mailSubject = "A new batch is been created : '" + job.JobName + "' ";

                    using (StreamReader readers = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/content/mailer/notification-email.html")))
                    {

                        mailMessage = readers.ReadToEnd();

                    }
                    mailMessage = mailMessage.Replace("{USER_NAME}", userData.FirstName);
                    mailMessage = mailMessage.Replace("{ACTIVITY_TYPE}", permissionName);
                    mailMessage = mailMessage.Replace("{DESCRIPTION}", " Batch is been created at " + job.CreatedDate);
                    //mailMessage = job.JobName + " Batch is been created at " + job.CreatedDate;
                    break;
                case BatchEventType.batchVerificationEvent:
                    permissionName = "Create Batch";
                    mailSubject = job.JobName + TnT.LangResource.GlobalRes.ShwMsgJobServiceBatchVerified;

                    using (StreamReader readers = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/content/mailer/notification-email.html")))
                    {

                        mailMessage = readers.ReadToEnd();

                    }
                    mailMessage = mailMessage.Replace("{USER_NAME}", userData.FirstName);
                    mailMessage = mailMessage.Replace("{ACTIVITY_TYPE}", permissionName);
                    mailMessage = mailMessage.Replace("{DESCRIPTION}", job.JobName + " Batch is been verified at " + job.VerifiedDate);
                    //mailMessage = job.JobName + " Batch is been verified at " + job.VerifiedDate;
                    break;
                case BatchEventType.batchNotVerifyEvent:
                    permissionName = TnT.LangResource.GlobalRes.JobsCreate;
                    mailSubject = job.JobName + TnT.LangResource.GlobalRes.ShwMsgJobServiceNotVerifed;
                    using (StreamReader readers = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/content/mailer/notification-email.html")))
                    {

                        mailMessage = readers.ReadToEnd();

                    }
                    mailMessage = mailMessage.Replace("{USER_NAME}", userData.FirstName);
                    mailMessage = mailMessage.Replace("{ACTIVITY_TYPE}", permissionName);
                    mailMessage = mailMessage.Replace("{DESCRIPTION}", job.JobName + " Batch is not verfied");
                    //mailMessage = job.JobName + " Batch is not verfied";
                    break;
                case BatchEventType.batchQtyRequested:
                    permissionName = TnT.LangResource.GlobalRes.ShwMsgJobServiceVerifyAddBatchQty;
                    mailSubject = "Additional Batch Quantity Requested for Batch " + job.JobName;
                    using (StreamReader readers = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/content/mailer/notification-email.html")))
                    {

                        mailMessage = readers.ReadToEnd();

                    }
                    mailMessage = mailMessage.Replace("{USER_NAME}", userData.FirstName);
                    mailMessage = mailMessage.Replace("{ACTIVITY_TYPE}", permissionName);
                    mailMessage = mailMessage.Replace("{DESCRIPTION}", "Additional Batch Quantity Requested for Batch " + job.JobName);

                    //mailMessage= "Additional Batch Quantity Requested for Batch " + job.JobName;
                    break;
                case BatchEventType.SettingRestoreDbRequest:
                    permissionName = TnT.LangResource.GlobalRes.ShwMsgJobServiceRestoreDbREq;
                    mailSubject = TnT.LangResource.GlobalRes.ShwMsgJobServiceRestoreDbREqGerated;
                    using (StreamReader readers = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/content/mailer/notification-email.html")))
                    {

                        mailMessage = readers.ReadToEnd();

                    }
                    mailMessage = mailMessage.Replace("{USER_NAME}", userData.FirstName);
                    mailMessage = mailMessage.Replace("{ACTIVITY_TYPE}", permissionName);
                    mailMessage = mailMessage.Replace("{DESCRIPTION}", "Restore DB Request generated");
                    //mailMessage= "Restore DB Request generated";
                    break;
                case BatchEventType.VerifyRestoreDbRequest:
                    permissionName = TnT.LangResource.GlobalRes.ShwMsgJobServiceCareatRestoreDb;
                    mailSubject =TnT.LangResource.GlobalRes.SettingDbResoreSuccessfuly;
                    using (StreamReader readers = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/content/mailer/notification-email.html")))
                    {

                        mailMessage = readers.ReadToEnd();

                    }
                    mailMessage = mailMessage.Replace("{USER_NAME}", userData.FirstName);
                    mailMessage = mailMessage.Replace("{ACTIVITY_TYPE}", permissionName);
                    mailMessage = mailMessage.Replace("{DESCRIPTION}", "Database Restore Successfully at " + DateTime.Now);
                    //mailMessage= "Database Restore Successfully at "+DateTime.Now;
                    break;
            }
            mailingList = db.Database.SqlQuery<string>("up_GetUserEmailIdsbyPermName @permissionName", new SqlParameter("permissionName", permissionName)).ToList();

        }



        private void getEmailsIdsAndMailOfPermHoldersProduct(BatchEventType batchEvent, ref PackagingAsso pkg, CustomPrincipal userData)
        {
            string permissionName = string.Empty;

            switch (batchEvent)
            {
                case BatchEventType.productCreationEvent:
                    permissionName = TnT.LangResource.GlobalRes.cmnMenuItemProductMasterSubItemVerifyProducts;
                    mailSubject = "A new product is been created : '" + pkg.Name + "' ";
                    using (StreamReader readers = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/content/mailer/notification-email.html")))
                    {

                        mailMessage = readers.ReadToEnd();

                    }
                    mailMessage = mailMessage.Replace("{USER_NAME}", userData.FirstName);
                    mailMessage = mailMessage.Replace("{ACTIVITY_TYPE}", permissionName);
                    mailMessage = mailMessage.Replace("{DESCRIPTION}", pkg.Name + " Product is been created at " + pkg.CreatedDate);
                    //mailMessage = pkg.Name + " Product is been created at " + pkg.CreatedDate;
                    break;
                case BatchEventType.productVerifyEvent:
                    permissionName = "Create Product";
                    mailSubject = pkg.Name + " :Product is been verified.";
                    using (StreamReader readers = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/content/mailer/notification-email.html")))
                    {

                        mailMessage = readers.ReadToEnd();

                    }
                    mailMessage = mailMessage.Replace("{USER_NAME}", userData.FirstName);
                    mailMessage = mailMessage.Replace("{ACTIVITY_TYPE}", permissionName);
                    mailMessage = mailMessage.Replace("{DESCRIPTION}", pkg.Name + " Product is been verified at " + pkg.CreatedDate);
                    //mailMessage = pkg.Name + " Product is been verified at " + pkg.CreatedDate;
                    break;
                case BatchEventType.productNotVerifyEvent:
                    permissionName = "Create Product";
                    mailSubject = pkg.Name + " :Product is not verified.";
                    using (StreamReader readers = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/content/mailer/notification-email.html")))
                    {

                        mailMessage = readers.ReadToEnd();

                    }
                    mailMessage = mailMessage.Replace("{USER_NAME}", userData.FirstName);
                    mailMessage = mailMessage.Replace("{ACTIVITY_TYPE}", permissionName);
                    mailMessage = mailMessage.Replace("{DESCRIPTION}", pkg.Name + " Product is not verified at " + pkg.CreatedDate);
                    //mailMessage = pkg.Name + " Product is been verified at " + pkg.CreatedDate;
                    break;
            }
            mailingList = db.Database.SqlQuery<string>("up_GetUserEmailIdsbyPermName @permissionName", new SqlParameter("permissionName", permissionName)).ToList();

        }

        public void notifyPerimissionHoldersProduct(ref PackagingAsso pkg, BatchEventType batchEvent,CustomPrincipal userData)
        {
            if (!IsNotificationEnabled) return;

            getEmailsIdsAndMailOfPermHoldersProduct(batchEvent, ref pkg, userData);
            if (mailingList.Count > 0)
            {
                foreach (var item in mailingList)
                {
                    if (IsValidEmail(item))
                    {
                        mailer.sendMail(mailMessage, mailSubject, "Batch Manager", item, true,"","");
                    }

                }
            }
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}