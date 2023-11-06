using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TnT.Models;
using TnT.Models.Russia;

namespace TnT.DataLayer.Russia
{

    public class RussiaFileGeneration
    {
        string RUSSIA_DOC_VERSION = string.Empty;

        public RussiaFileGeneration()
        {
            RUSSIA_DOC_VERSION = Convert.ToString(Utilities.getAppSettings("RUSSIA_DOC_VERSION"));
            if (string.IsNullOrEmpty(RUSSIA_DOC_VERSION))
            {
                RUSSIA_DOC_VERSION = "1.28";
            }
        }
        private ApplicationDbContext db = new ApplicationDbContext();
        public string generate321(int JID, RussiaViewModel vm)
        {
            string xml = "";
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                var job = db.Job.Where(x => x.JID == JID).FirstOrDefault();
                var pkg = db.PackagingDetails.Where(x => x.JobID == JID && x.PackageTypeCode == "MOC" && x.IsUsed == true && x.IsDecomission == false && x.IsRejected == false && x.NextLevelCode != "FFFFF").Select(x => x.Code);
                if (pkg != null)
                {
                    var GTIN = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == "MOC").Select(x => x.JD_GTIN).FirstOrDefault();

                    writer.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                    writer.Append("<documents session_ui=\"" + Guid.NewGuid() + "\" version=\"" + RUSSIA_DOC_VERSION + "\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                    writer.Append("<foreign_emission action_id=\"321\">");
                    writer.Append("<subject_id>" + vm.subject_id + "</subject_id>");
                    writer.Append("<operation_date>" + job.LastUpdatedDate.ToString("yyyy-MM-ddThh:mm:ss+05:00") + "</operation_date>");
                    writer.Append("<packing_id>" + vm.packing_id + "</packing_id>");
                    writer.Append("<control_id>" + vm.control_id + "</control_id>");
                    writer.Append("<series_number>" + vm.series_number + "</series_number>");
                    writer.Append("<expiration_date>" + job.ExpDate.ToString("dd.MM.yyyy") + "</expiration_date>");
                    writer.Append("<gtin>" + GTIN + "</gtin>");
                    writer.Append("<signs>");
                    foreach (var item in pkg)
                    {
                        writer.Append("<sgtin>" + GTIN + item + "</sgtin>");
                    }
                    writer.Append("</signs>");
                    writer.Append("</foreign_emission>");
                    writer.Append("</documents>");
                }
                xml = writer.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string generate331(int JID, RussiaViewModel vm)
        {
            string xml = "";
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                var job = db.Job.Where(x => x.JID == JID).FirstOrDefault();
                var pkg = db.PackagingDetails.Where(x => x.JobID == JID && x.PackageTypeCode == "MOC" && x.IsUsed == true && x.IsDecomission == false && x.IsRejected == false && x.NextLevelCode != "FFFFF").Select(x => x.Code);
                if (pkg != null)
                {
                    var GTIN = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == "MOC").Select(x => x.JD_GTIN).FirstOrDefault();

                    writer.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                    writer.Append("<documents session_ui=\"" + Guid.NewGuid() + "\" version=\"" + RUSSIA_DOC_VERSION + "\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                    writer.Append("<foreign_shipment action_id=\"331\">");
                    writer.Append("<subject_id>" + vm.subject_id + "</subject_id>");
                    writer.Append("<seller_id>" + vm.seller_id + "</seller_id>");
                    writer.Append("<receiver_id>" + vm.receiver_id + "</receiver_id>");
                    writer.Append("<custom_receiver_id>" + vm.custom_receiver_id + "</custom_receiver_id>");
                    writer.Append("<operation_date>" + job.LastUpdatedDate.ToString("yyyy-MM-ddThh:mm:ss+05:00") + "</operation_date>");
                    writer.Append("<contract_type>" + vm.contract_type + "</contract_type>");
                    writer.Append("<doc_num>" + vm.doc_num + "</doc_num>");
                    writer.Append("<doc_date>" + vm.doc_date.ToString("dd.MM.yyyy") + "</doc_date>");
                    writer.Append("<order_details>");
                    foreach (var item in pkg)
                    {
                        writer.Append("<sgtin>" + GTIN + item + "</sgtin>");
                    }
                    writer.Append("</order_details>");
                    writer.Append("</foreign_shipment>");
                    writer.Append("</documents>");
                }
                xml = writer.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string generate332(int JID, RussiaViewModel vm)
        {
            string xml = "";
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                var job = db.Job.Where(x => x.JID == JID).FirstOrDefault();
                var pkg = db.PackagingDetails.Where(x => x.JobID == JID && x.PackageTypeCode == "MOC" && x.IsUsed == true && x.IsDecomission == false && x.IsRejected == false && x.NextLevelCode != "FFFFF").Select(x => x.Code);
                if (pkg != null)
                {
                    var GTIN = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == "MOC").Select(x => x.JD_GTIN).FirstOrDefault();

                    writer.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                    writer.Append("<documents session_ui=\"" + Guid.NewGuid() + "\" version=\"" + RUSSIA_DOC_VERSION + "\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                    writer.Append("<foreign_import action_id=\"332\">");
                    writer.Append("<subject_id>" + vm.subject_id + "</subject_id>");
                    writer.Append("<seller_id>" + vm.seller_id + "</seller_id>");
                    writer.Append("<shipper_id>" + vm.shipper_id + "</shipper_id>");
                    writer.Append("<custom_receiver_id>" + vm.custom_receiver_id + "</custom_receiver_id>");
                    writer.Append("<operation_date>" + job.LastUpdatedDate.ToString("yyyy-MM-ddThh:mm:ss+05:00") + "</operation_date>");
                    writer.Append("<contract_type>" + vm.contract_type + "</contract_type>");
                    writer.Append("<doc_num>" + vm.doc_num + "</doc_num>");
                    writer.Append("<doc_date>" + vm.doc_date.ToString("dd.MM.yyyy") + "</doc_date>");
                    writer.Append("<order_details>");
                    foreach (var item in pkg)
                    {
                        writer.Append("<sgtin>" + GTIN + item + "</sgtin>");
                    }
                    writer.Append("</order_details>");
                    writer.Append("</foreign_import>");
                    writer.Append("</documents>");
                }
                xml = writer.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public string generate911(int JID, string SSCC, RussiaViewModel vm)
        {
            string xml = "";
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                var job = db.Job.Where(x => x.JID == JID).FirstOrDefault();
                string LstCode = db.PackagingDetails.Where(x => x.SSCC.Contains(SSCC) && x.JobID == JID).Select(x => x.Code).FirstOrDefault();
                var pkg = db.PackagingDetails.Where(x => x.JobID == JID && x.IsUsed == true && x.IsDecomission == false && x.IsRejected == false && x.NextLevelCode == LstCode);

                // var pkg = db.PackagingDetails.Where(x => x.JobID == JID && x.PackageTypeCode == "MOC" && x.IsUsed == true && x.IsDecomission == false && x.IsRejected == false && x.NextLevelCode != "FFFFF").Select(x => x.Code);
                if (pkg != null)
                {
                    string secondlstlvl = pkg.Select(x => x.PackageTypeCode).FirstOrDefault();
                    var GTIN = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == secondlstlvl).Select(x => x.JD_GTIN).FirstOrDefault();
                    // var GTIN = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == "MOC").Select(x => x.JD_GTIN).FirstOrDefault();

                    writer.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                    writer.Append("<documents session_ui=\"" + Guid.NewGuid() + "\" version=\"" + RUSSIA_DOC_VERSION + "\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                    writer.Append("<unit_pack action_id=\"911\">");
                    writer.Append("<subject_id>" + vm.subject_id + "</subject_id>");
                    writer.Append("<sscc>" + SSCC + "</sscc>");
                    writer.Append("<operation_date>" + job.LastUpdatedDate.ToString("yyyy-MM-ddThh:mm:ss+05:00") + "</operation_date>");
                    writer.Append("<content>");
                    foreach (var item in pkg)
                    {
                        writer.Append("<sgtin>" + GTIN + item.Code + "</sgtin>");
                    }
                    writer.Append("</content>");
                    writer.Append("</unit_pack>");
                    writer.Append("</documents>");
                }
                xml = writer.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string generate914(int JID, string SSCC, RussiaViewModel vm)
        {
            string xml = "";
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                var job = db.Job.Where(x => x.JID == JID).FirstOrDefault();
                string LstCode = db.PackagingDetails.Where(x => x.SSCC.Contains(SSCC) && x.JobID == JID).Select(x => x.Code).FirstOrDefault();
                var pkg = db.PackagingDetails.Where(x => x.JobID == JID && x.IsUsed == true && x.IsDecomission == false && x.IsRejected == false && x.NextLevelCode == LstCode);
                if (pkg != null)
                {

                    string secondlstlvl = pkg.Select(x => x.PackageTypeCode).FirstOrDefault();
                    var GTIN = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == secondlstlvl).Select(x => x.JD_GTIN).FirstOrDefault();

                    writer.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                    writer.Append("<documents session_ui=\"" + Guid.NewGuid() + "\" version=\"" + RUSSIA_DOC_VERSION + "\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                    writer.Append("<unit_append action_id=\"914\">");
                    writer.Append("<subject_id>" + vm.subject_id + "</subject_id>");
                    writer.Append("<operation_date>" + job.LastUpdatedDate.ToString("yyyy-MM-ddThh:mm:ss+05:00") + "</operation_date>");
                    writer.Append("<sscc>" + SSCC + "</sscc>");
                    writer.Append("<content>");
                    foreach (var item in pkg)
                    {
                        writer.Append("<sgtin>" + GTIN + item.Code + "</sgtin>");
                    }
                    writer.Append("</content>");
                    writer.Append("</unit_append>");
                    writer.Append("</documents>");
                }
                xml = writer.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public string generate915(int JID, RussiaViewModel vm)
        {
            string xml = "";
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                var job = db.Job.Where(x => x.JID == JID).FirstOrDefault();
                var lvls = ProductPackageHelper.getAllDeck(JID.ToString());
                lvls = ProductPackageHelper.sorttheLevelsDesc(lvls); //All the levels for the selected batch sorted in Desc order
                string LastpckLevel = lvls.First();
                var pkg = db.PackagingDetails.Where(x => x.JobID == JID && x.IsUsed == true && x.IsDecomission == false && x.IsRejected == false);
                if (pkg != null)
                {
                    var GTIN = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == "MOC").Select(x => x.JD_GTIN).FirstOrDefault();

                    writer.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                    writer.Append("<documents session_ui=\"" + Guid.NewGuid() + "\" version=\"" + RUSSIA_DOC_VERSION + "\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                    writer.Append("<multi_pack action_id=\"915\">");
                    writer.Append("<subject_id>" + vm.subject_id + "</subject_id>");
                    writer.Append("<operation_date>" + job.LastUpdatedDate.ToString("yyyy-MM-ddThh:mm:ss+05:00") + "</operation_date>");
                    writer.Append("<by_sgtin>");
                    var SSCC = pkg.Where(x => x.PackageTypeCode == LastpckLevel);
                    foreach (var item in SSCC)
                    {
                        writer.Append("<detail>");
                        writer.Append("<sscc>" + item.SSCC + "</sscc>");
                        writer.Append("<content>");
                        var mocCode = pkg.Where(x => x.NextLevelCode == item.SSCC).Select(x => x.Code);
                        foreach (var Code in mocCode)
                        {
                            writer.Append("<sgtin>" + GTIN + Code + "</sgtin>");
                        }
                        writer.Append("</content>");
                        writer.Append("</detail>");
                    }
                    writer.Append("</by_sgtin>");
                    writer.Append("</multi_pack>");
                    writer.Append("</documents>");
                }
                xml = writer.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public string generate213(int JID, RussiaViewModel vm)
        {
            string xml = "";
            StringBuilder writer;
            try
            {
                writer = new StringBuilder();
                var expdate = db.Job.Where(x => x.JID == JID).Select(x => x.ExpDate).FirstOrDefault();
                var lvls = ProductPackageHelper.getAllDeck(JID.ToString());
                lvls = ProductPackageHelper.sorttheLevelsDesc(lvls); //All the levels for the selected batch sorted in Desc order
                string LastpckLevel = lvls.First();
                var pkg = db.PackagingDetails.Where(x => x.JobID == JID && x.IsUsed == true && x.IsDecomission == false && x.IsRejected == false && x.PackageTypeCode == LastpckLevel).Select(x => x.SSCC);
                if (pkg != null)
                {
                    var GTIN = db.JobDetails.Where(x => x.JD_JobID == JID && x.JD_Deckcode == "MOC").Select(x => x.JD_GTIN).FirstOrDefault();

                    writer.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                    writer.Append("<documents session_ui=\"" + Guid.NewGuid() + "\" version=\"" + RUSSIA_DOC_VERSION + "\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                    writer.Append("<booking_sscc action_id=\"213\">");
                    writer.Append("<subject_id>" + vm.subject_id + "</subject_id>");
                    writer.Append("<operation_type>" + vm.operation_type + "</operation_type>");
                    writer.Append("<signs>");
                    foreach (var item in pkg)
                    {
                        writer.Append("<sscc>" + item + "</sscc>");
                    }
                    writer.Append("</signs>");
                    writer.Append("</booking_sscc>");
                    writer.Append("</documents>");
                }
                xml = writer.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}