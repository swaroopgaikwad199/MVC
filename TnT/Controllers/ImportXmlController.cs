using Ionic.Zip;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using TnT.DataLayer.Reports;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models;
using TnT.Models.Account;
using TnT.Models.Code;
using TnT.Models.ImportXml;
using TnT.Models.Job;
using System.Xml.Schema;
//using Microsoft.Xml.XMLGen;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class ImportXmlController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Trails trail = new Trails();
        string constr = Utilities.getConnectionString("DefaultConnection");
        // GET: ImportXml
        public ActionResult Index()
        {
            ViewBag.Job = db.Job.Where(x => x.JobStatus == 3).ToList();
            return View();
        }



        //[HttpPost]
        //public void DownloadFiles(ModelViewXmlUIDList mv)
        //{
        //    if (mv.JobID != 0)
        //    {
        //        var job = db.Job.Where(x => x.JID == mv.JobID).FirstOrDefault();

        //        string path = Server.MapPath("~/Content/ImportXML/PML/PML_" + job.BatchNo);
        //        if (Directory.Exists(path))
        //        {
        //            string[] filePaths = Directory.GetFiles(Server.MapPath("~/Content/ImportXML/PML/PML_" + job.BatchNo + "/"));
        //            using (ZipFile zip = new ZipFile())
        //            {

        //                zip.AddDirectoryByName("Files");
        //                if (filePaths.Count() > 0)
        //                {
        //                    foreach (string filePath in filePaths)
        //                    {
        //                        zip.AddFile(filePath, "CreateXmlUIDList");
        //                    }
        //                }
        //                else
        //                {
        //                    ViewBag.Job = db.Job.Where(x => x.JobStatus == 3).ToList();
        //                 //   return View("CreateXmlUIDList");
        //                }

        //                string zipName = String.Format(job.JobName + "_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
        //                zip.Save(Server.MapPath("~/Content/ImportXml/"+zipName));
        //                return File(Server.MapPath("~/zipfiles/bundle.zip"),
        //                "application/zip", "Satya.zip");

        //                TempData["Success"] = "File Downloaded Successfully";
        //                ViewBag.Job = db.Job.Where(x => x.JobStatus == 3).ToList();
        //               // return View("CreateXmlUIDList");
        //            }

        //        }
        //        else
        //        {
        //            TempData["Success"] = "File Does Not Exist";
        //            ViewBag.Job = db.Job.Where(x => x.JobStatus == 3).ToList();
        //          //  return View("CreateXmlUIDList");
        //        }
        //    }
        //    else
        //    {
        //        TempData["Success"] = "Please Select Batch Name";
        //        ViewBag.Job = db.Job.Where(x => x.JobStatus == 3).ToList();
        //      //  return View("CreateXmlUIDList");
        //    }
        //}

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile, M_SKMaster m)
        {
            if (postedFile != null)
            {

                string path = Server.MapPath("~/Content/ImportXML/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));

                DataSet ds = new DataSet();
                ds.ReadXml(path + Path.GetFileName(postedFile.FileName));

                string[] numberfrom = (ds.Tables["Interval"].Rows[0]["NumberFrom"].ToString()).Split(')');
                string[] gtin = numberfrom[1].Split('(');
                decimal nfrom = Convert.ToDecimal(numberfrom[2]);
                string[] numberto = (ds.Tables["Interval"].Rows[0]["NumberTo"].ToString()).Split(')');
                decimal nto = Convert.ToDecimal(numberto[2]);
                string query = "select * from M_SKMaster where NFrom between " + nfrom + " and " + nto + " or nto between " + nfrom + " and " + nto + "";
                DataSet data = SqlHelper.ExecuteDataset(constr, CommandType.Text, query);
                if (data.Tables[0].Rows.Count == 0)
                {
                    if (gtin[0] == ds.Tables["ObjectKey"].Rows[0]["Value"].ToString())
                    {
                        M_SKMaster mdata = new M_SKMaster();
                        mdata.ReceivingSystem = ds.Tables["NumberRangeConfirmationMessage"].Rows[0]["ReceivingSystem"].ToString();
                        mdata.ActionCode = ds.Tables["NumberRangeConfirmationMessage"].Rows[0]["ActionCode"].ToString();
                        mdata.IDType = ds.Tables["NumberRangeConfirmationMessage"].Rows[0]["IDType"].ToString();
                        mdata.EncodingType = ds.Tables["NumberRangeConfirmationMessage"].Rows[0]["EncodingType"].ToString();
                        mdata.NumberFrom = ds.Tables["Interval"].Rows[0]["NumberFrom"].ToString();
                        mdata.NFrom = nfrom;
                        mdata.NumberTo = ds.Tables["Interval"].Rows[0]["NumberTo"].ToString();
                        mdata.Nto = nto;
                        mdata.IsUsed = false;
                        db.M_SKMaster.Add(mdata);
                        db.SaveChanges();

                        SK_ObjectKey skdata = new SK_ObjectKey();
                        for (int i = 0; i < ds.Tables["ObjectKey"].Rows.Count; i++)
                        {

                            skdata.MID = mdata.MID;
                            skdata.Name = ds.Tables["ObjectKey"].Rows[i]["Name"].ToString();
                            skdata.Value = ds.Tables["ObjectKey"].Rows[i]["Value"].ToString();
                            skdata.IsUsed = false;
                            db.SK_ObjectKey.Add(skdata);
                            db.SaveChanges();

                        }


                        TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImportXmlUploadFileSuccess;
                        trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImportXmlUploadFileSuccess, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImportXmlUploadFileSuccess, TnT.LangResource.GlobalRes.TrailActionSKoreaActivity);
                    }
                    else
                    {
                        TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImportXmlInvalidData;
                        return View();
                    }
                }
                else
                {

                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImportXmlUidSeriesExistAlredy;
                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImportXmlUidSeriesExistAlredy, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImportXmlUidSeriesExistAlredy, TnT.LangResource.GlobalRes.TrailActionSKoreaActivity);
                }
            }

            return View();
        }

        //public ActionResult GetXsd()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult GetXsd(HttpPostedFileBase postedFile, M_SKMaster m)
        //{
        //    string path = Server.MapPath("~/Content/xsd File/");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    if (System.IO.File.Exists(path + Path.GetFileName(postedFile.FileName)))
        //    {
        //        System.IO.File.Delete(path + Path.GetFileName(postedFile.FileName));
        //    }

        //    postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
            
        //    XmlTextWriter textWriter = new XmlTextWriter(path+"po.xml", null);
        //    textWriter.Formatting = Formatting.Indented;
        //    XmlQualifiedName qname = new XmlQualifiedName("PurchaseOrder",
        //                               "http://tempuri.org");
            
        //    XmlSampleGenerator generator = new XmlSampleGenerator(path + postedFile.FileName, qname);
        //    generator.WriteXml(textWriter);
        //    return View();
        //}
      

    
   
        public ActionResult CreateXmlUIDList()
        {
            ViewBag.Job = db.Job.Where(x => x.JobStatus == 3 && x.TID == 10054).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult getDeck(int JobId)
        {
            var data = db.JobDetails.Where(x => x.JD_JobID == JobId);
            return Json(data);
        }

        [HttpPost]
        public ActionResult CreateXmlUIDList(ModelViewXmlUIDList mv)
        {
            ReportXMLUIDLIST rpt = new ReportXMLUIDLIST();
            string msg = rpt.GenerateXMLReportFile(mv);

            TempData["Success"] = msg;
            trail.AddTrail(msg, Convert.ToInt32(User.ID), msg, TnT.LangResource.GlobalRes.TrailActionSKoreaActivity);
            if (mv.JobID != 0)
            {
                var job = db.Job.Where(x => x.JID == mv.JobID).FirstOrDefault();

                string path = Server.MapPath("~/Content/ImportXML/PML/PML_" + job.BatchNo);
                if (Directory.Exists(path))
                {
                    string[] filePaths = Directory.GetFiles(Server.MapPath("~/Content/ImportXML/PML/PML_" + job.BatchNo + "/"));
                    using (ZipFile zip = new ZipFile())
                    {

                        //  zip.AddDirectoryByName("Files");
                        if (filePaths.Count() > 0)
                        {
                            foreach (string filePath in filePaths)
                            {
                                zip.AddFile(filePath, job.BatchNo);
                            }
                        }
                        else
                        {
                            ViewBag.Job = db.Job.Where(x => x.JobStatus == 3).ToList();
                            TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImportXmlDataNotFound;
                            trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImportXmlDataNotFound, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImportXmlDataNotFound, TnT.LangResource.GlobalRes.TrailActionSKoreaActivity);

                        }

                        string zipName = String.Format(job.BatchNo + "_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                        zip.Save(Server.MapPath("~/Content/ImportXml/PML/Zip" + zipName));
                        //return File(Server.MapPath("~/Content/ImportXml/PML/Zip" + zipName),
                        //"application/zip", zipName);
                        Response.ContentType = "application/zip";
                        Response.AddHeader("Content-Disposition", "attachment; filename="+zipName);
                        Response.TransmitFile(Server.MapPath("~/Content/ImportXml/PML/Zip" + zipName));
                        TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImportXmlFileDownloadedSuccess;

                        trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImportXmlFileDownloadedSuccess, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImportXmlFileDownloadedSuccess, TnT.LangResource.GlobalRes.TrailActionSKoreaActivity);
                        ViewBag.Job = db.Job.Where(x => x.JobStatus == 3).ToList();

                    }

                }
                else
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImportXmlFileDoesNtExist;
                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImportXmlFileDoesNtExist, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImportXmlFileDoesNtExist, TnT.LangResource.GlobalRes.TrailActionSKoreaActivity);
                    ViewBag.Job = db.Job.Where(x => x.JobStatus == 3).ToList();

                }
            }
            else
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.ToolTipReportBatchName;
                trail.AddTrail(TnT.LangResource.GlobalRes.ToolTipReportBatchName, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.ToolTipReportBatchName, TnT.LangResource.GlobalRes.TrailActionSKoreaActivity);
                ViewBag.Job = db.Job.Where(x => x.JobStatus == 3).ToList();

            }
            return View();
        }

        public ActionResult ImportUIDs()
        {
            ViewBag.products = (from prod in db.PackagingAsso join lbl in db.PackageLabelMaster on prod.PAID equals lbl.PAID where prod.VerifyProd == true && prod.IsActive == true && prod.AuthorizedNo!=null && prod.SubType!=null && prod.SubTypeSpec!=null && prod.Workshop!=null select new { prod.PAID, prod.Name }).Distinct().ToList();
            ViewBag.jobs = (from job in db.Job where job.JobStatus == 3 && job.TID == 10056 select new { job.JID, job.JobName }).Distinct().ToList();
            return View();
        }
        [HttpPost]
        public ActionResult ImportUIDs(int Product, string Decks, HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Content/ImportUIDs/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));

                var fileContents = System.IO.File.ReadAllText(path + Path.GetFileName(postedFile.FileName));
                var lines = fileContents
                                .Split(Environment.NewLine.ToCharArray())
                                .Skip(2)
                                .ToList();

                var resultset = from x in db.X_ChinaUIDs where lines.Contains(x.Code) select x.Code;
                if (resultset.Count() > 0)
                {
                    TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImportXmlUidImportedalredy;
                    trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImportXmlUidImportedalredy, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImportXmlUidImportedalredy, TnT.LangResource.GlobalRes.TrailActionChinaActivity);
                    return RedirectToAction("ImportUIDs");
                }

                foreach (var item in lines)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        X_ChinaUIDs uid = new X_ChinaUIDs()
                        {
                            Code = item,
                            IsUsed = false,
                            PackageTypeCode = Decks,
                            PAID = Product
                        };
                        db.X_ChinaUIDs.Add(uid);
                    }
                }
                db.SaveChanges();

                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImportXmlUidImportedSuccessfully;
                trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImportXmlUidImportedSuccessfully, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImportXmlUidImportedSuccessfully, TnT.LangResource.GlobalRes.TrailActionChinaActivity);
            }
            else
            {

                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImportXmlChooseFiletoUplod;
                trail.AddTrail(TnT.LangResource.GlobalRes.TempDataImportXmlChooseFiletoUplod, Convert.ToInt32(User.ID), TnT.LangResource.GlobalRes.TempDataImportXmlChooseFiletoUplod, TnT.LangResource.GlobalRes.TrailActionChinaActivity);
            }
            return RedirectToAction("ImportUIDs");
        }
        [HttpPost]
        public ActionResult getDecksofProduct(int PAID)
        {
            var decks = db.PackagingAssoDetails.Where(x => x.PAID == PAID).OrderBy(x => x.Id).Select(x => x.PackageTypeCode).ToList();
            return Json(decks);
        }

        [HttpPost]
        public ActionResult GenerateXML()
        {
            int JobId = Convert.ToInt32(Request["Job"]);

            bool relationCreateXML = GenerateRelationCreateXML(JobId);
            bool salesWareHouseXML = GenerateSalesWareHouse(JobId);

            if (relationCreateXML == false || salesWareHouseXML == false)
            {
                TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImportXmlErrorCreatingXml;
                return RedirectToAction("ImportUIDs");
            }
            var job = db.Job.Where(x => x.JID == JobId).SingleOrDefault();

            string path = Server.MapPath("~/Content/ChinaXMLs/" + job.JobName);
            if (Directory.Exists(path))
            {
                string[] filePaths = Directory.GetFiles(path);
                using (ZipFile zip = new ZipFile())
                {
                    if (filePaths.Count() > 0)
                    {
                        foreach (string filePath in filePaths)
                        {
                            zip.AddFile(filePath, job.BatchNo);
                        }
                    }
                    else
                    {
                        TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImportXmlFileNotFound;
                        trail.AddTrail(job.JobName+ TnT.LangResource.GlobalRes.TempDataImportXmlFileNotFound, Convert.ToInt32(User.ID), job.JobName+ TnT.LangResource.GlobalRes.TempDataImportXmlFileNotFound, TnT.LangResource.GlobalRes.TrailActionChinaActivity);
                        return RedirectToAction("ImportUIDs");
                    }

                    string zipName = String.Format(job.JobName + "_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    zip.Save(path + zipName);
                 //   TempData["Success"] = "File Downloaded Successfully";
                    trail.AddTrail(zipName+ TnT.LangResource.GlobalRes.TempDataImportXmlFileDownloadedSuccess, Convert.ToInt32(User.ID), job.JobName + zipName+ TnT.LangResource.GlobalRes.TempDataImportXmlFileDownloadedSuccess, TnT.LangResource.GlobalRes.TrailActionChinaActivity);
                    return File(path + zipName, "application/zip", zipName);

                }
            }
            TempData["Success"] = TnT.LangResource.GlobalRes.TempDataImportXmlFileNotFound;
            return RedirectToAction("ImportUIDs");
        }

        private bool GenerateRelationCreateXML(int JobId)
        {
            if (JobId == 0)
                return false;

            var jb = db.Job.Where(x => x.JID == JobId).FirstOrDefault();
            string path = Server.MapPath("~/Content/ChinaXMLs/" + jb.JobName + "/");
            StreamWriter writer;
            try
            {
                int DeckSize = 0;
                var PA = db.PackagingAsso.Where(x => x.PAID == jb.PAID).FirstOrDefault();
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
                path += jb.BatchNo + "_RelationCreate.xml";
                var Pack = db.PackagingDetails.Where(x => x.JobID == JobId && x.IsUsed == true && x.IsRejected == false && x.IsDecomission == false).ToList();
                writer = new StreamWriter(path);

                DeckSize = db.PackagingAssoDetails.Where(x => x.PAID == jb.PAID && x.PackageTypeCode == "ISH").FirstOrDefault().Size;

                var ObjUser = db.Users.Where(x => x.ID == jb.CreatedBy).SingleOrDefault();

                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<Document xmlns:xsi=\"http://www.w3.org/2001/xmlschema-instance\" xsi:noNamespaceSchemaLocation=\"关联关系XML Schema-3.0.xsd\" License=\"\">");
                writer.WriteLine("<Events version=\"3.0\">");
                writer.WriteLine("<Event name=\"RelationCreate\">");
                //productCode = 72552 " + rw.ProductCode + ", subTypeNo=81" + rw.ProductName + "
                if (PA != null)
                    writer.WriteLine("<Relation productCode=\"" + PA.ProductCode + "\" subTypeNo=\"" + PA.SubTypeNo + "\" cascade=\"1:" + DeckSize.ToString() + "\" packageSpec=\"" + PA.PackageSpec + "\" comment=\"\" >");
                writer.WriteLine("<Batch batchNo=\"" + jb.BatchNo + "\" madeDate=\"" + jb.MfgDate.ToString("yyyy-MM-dd") + "\" validateDate=\"" + jb.ExpDate.ToString("yyyy-MM-dd") + "\" workshop=\"" + PA.Workshop + "\" lineName=\"" + jb.LineCode + "\" lineManager=\"" + ObjUser.UserName + "\">");
                string codeflag = string.Empty;
                List<PackagingDetails> lstCodeFlag = new List<PackagingDetails>();

                foreach (var itm in Pack.Where(x => string.IsNullOrEmpty(x.NextLevelCode) && (!string.IsNullOrEmpty(x.SSCC))).ToList())
                {
                    lstCodeFlag = Pack.FindAll(item => item.NextLevelCode == itm.Code);
                    if (lstCodeFlag.Count == DeckSize)
                        codeflag = "0";
                    else
                        codeflag = "2";
                    string ParentCode = "<Code curCode=\"" + itm.Code.Trim() + "\" packLayer=\"2\" parentCode=\"\" flag=\"" + codeflag + "\" />";

                    writer.WriteLine(ParentCode);
                    foreach (PackagingDetails itm1 in Pack)
                    {
                        if (itm.Code == itm1.NextLevelCode)
                        {
                            string ChildCode = "<Code curCode=\"" + itm1.Code.Trim() + "\" packLayer=\"1\" parentCode=\"" + itm.Code.Trim() + "\" flag=\"" + codeflag + "\" />";
                            writer.WriteLine(ChildCode);
                        }
                    }
                }
                writer.WriteLine("</Batch>");
                writer.WriteLine("</Relation>");
                writer.WriteLine("</Event>");
                writer.WriteLine("</Events>");
                writer.WriteLine("</Document>");
                writer.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private bool GenerateSalesWareHouse(int JobId)
        {
            if (JobId == 0)
                return false;
            Job jb = db.Job.Where(x => x.JID == JobId).SingleOrDefault();
            StreamWriter writer;

            string ToCorpID = db.M_Customer.Where(x => x.Id == jb.CustomerId).SingleOrDefault().ToCorpID.ToString();
            string path = Server.MapPath("~/Content/ChinaXMLs/" + jb.JobName + "/");
            try
            {
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);

                path += "\\" + jb.BatchNo + "_SalesWareHouseOut.xml";

                writer = new StreamWriter(path);
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writer.WriteLine("<Document xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"终端接口XML Schema-3.0.xsd\" Version=\"3.0\">");
                writer.WriteLine("  <Events>");
                writer.WriteLine("    <Event Name=\"SalesWareHouseOut\" MainAction=\"WareHouseOut\">");
                writer.WriteLine("      <ActionMapping>");
                writer.WriteLine("        <Action Name=\"WareHouseOut\">");
                writer.WriteLine("          <ActionData>CorpOrderID</ActionData>");
                writer.WriteLine("          <ActionData>Actor</ActionData>");
                writer.WriteLine("          <ActionData>ActDate</ActionData>");
                writer.WriteLine("          <ActionData>ToCorpID</ActionData>");
                writer.WriteLine("          <ActionData>Code</ActionData>");
                writer.WriteLine("        </Action>");
                writer.WriteLine("      </ActionMapping>");
                writer.WriteLine("      <DataMaping>");
                writer.WriteLine("        <MetaData Name=\"CorpOrderID\" Type=\"String\" />");
                writer.WriteLine("        <MetaData Name=\"Actor\" Type=\"String\" />");
                writer.WriteLine("        <MetaData Name=\"ActDate\" Type=\"Date\" />");
                writer.WriteLine("        <MetaData Name=\"ToCorpID\" Type=\"String\" />");
                writer.WriteLine("        <MetaData Name=\"Code\" Type=\"String\" />");
                writer.WriteLine("      </DataMaping>");
                writer.WriteLine("      <DataDesc>");
                writer.WriteLine("        <MetaDataDesc MetaName=\"Actor\" IsParent=\"true\">");
                writer.WriteLine("        <MetaDataDesc MetaName=\"Code\" IsParent=\"false\" />");
                writer.WriteLine("      </MetaDataDesc>");
                writer.WriteLine("        <MetaDataDesc MetaName=\"CorpOrderID\" IsParent=\"true\">");
                writer.WriteLine("        <MetaDataDesc MetaName=\"Code\" IsParent=\"false\" />");
                writer.WriteLine("      </MetaDataDesc>");
                writer.WriteLine("      </DataDesc>");
                writer.WriteLine("      <DataField>");

                List<PackagingDetails> Pack = db.PackagingDetails.Where(x => x.JobID == JobId && x.PackageTypeCode == "MOC" && x.IsUsed == true && x.IsRejected == false && x.IsDecomission == false).ToList();
                List<PackagingDetails> lstISH = db.PackagingDetails.Where(x => x.JobID == JobId && x.PackageTypeCode == "ISH" && x.IsUsed == true && x.IsRejected == false && x.IsDecomission == false).ToList();
                List<Users> LstUsers = db.Users.ToList();
                int i = 0;
                Users oUser = null;
                string UserName = "Operator";
                foreach (PackagingDetails itm in lstISH)
                {
                    oUser = LstUsers.Find(item => item.ID == itm.OperatorId);
                    if (oUser != null)
                        UserName = oUser.UserName;
                    if (i == 0)
                    {
                        writer.WriteLine("<Data Code=\"" + itm.Code.Trim() + "\" CorpOrderID=\"" + jb.JobName + "\" Actor=\"" + UserName + "\" ActDate=\"" + itm.LastUpdatedDate.ToString("yyyy-MM-dd hh:mm:ss") + "\" WrongCode=\"False\" ToCorpID=\"" + ToCorpID + "\" />");
                    }
                    else
                    {
                        writer.WriteLine("<Data Code=\"" + itm.Code.Trim() + "\" CorpOrderID=\"" + jb.JobName + "\" Actor=\"" + UserName + "\" ActDate=\"" + itm.LastUpdatedDate.ToString("yyyy-MM-dd hh:mm:ss") + "\" WrongCode=\"False\" ToCorpID=\"" + ToCorpID + "\" />");
                    }
                    i++;
                }
                writer.WriteLine("</DataField>");
                writer.WriteLine("</Event>");
                writer.WriteLine("</Events>");
                writer.WriteLine("</Document>");
                writer.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}