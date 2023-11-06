using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TnT.DataLayer.LabelDesigner;
using TnT.DataLayer.Security;
using TnT.DataLayer.Trailings;
using TnT.Models.LblLytDsg;

namespace TnT.Controllers
{
    [CustomAuthorize(RolesConfigKey = "RolesConfigKey")]
    public class LblLytDsgController : BaseController
    {
        LabelDesignHelper lblHelper = new LabelDesignHelper();
        [HttpPost]
        public ActionResult loadFileContent(string FileName)
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                var data = lblHelper.GetFieldSetView(FileName);
                return Json(data);
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.toastrLblLytDsgNoData);
            }
        }

        private void BindLabelDesignerFiles()
        {
            var filesNames = lblHelper.getLabelDesignerFiles();
            filesNames.Insert(0, TnT.LangResource.GlobalRes.LblLytSelect);
            var da = filesNames.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
            ViewBag.LabelFiles = da;
        }
        private Trails trail = new Trails();
        // GET: LblLytDsg
        public ActionResult Index()
        {
            BindLabelDesignerFiles();
            trail.AddTrail(User.FirstName + " " + TnT.LangResource.GlobalRes.TrailLblLytDsgViewLabelLayout, Convert.ToInt32(User.ID), User.FirstName + " " + TnT.LangResource.GlobalRes.TrailLblLytDsgViewLabelLayout, TnT.LangResource.GlobalRes.TrailActionLabelActivity);
            return View();
        }

        [HttpPost]
        public ActionResult SaveLabelFile(string[] fieldData, string[] pageSettings, string FileName)
        {
            try
            {
                DataTable dtFieldDatadata = ConvertFieldDataToDataset(fieldData);
                dtFieldDatadata.TableName = "Field";
                DataTable dtPageSettings = ConvertPageSettingsToDataset(pageSettings);
                dtPageSettings.TableName = "Field_Appearance";

                DataSet ds = new DataSet();
                ds.Merge(dtFieldDatadata);
                ds.Merge(dtPageSettings);
                ds.DataSetName = "ArrayOfField_Appearance";
                //ds.Namespace = "http:xml";
                //ds.Prefix = "xsds";
                //ds.WriteXml()
                var PhysicalPath = HttpRuntime.AppDomainAppPath;
                string Path = PhysicalPath + "Content\\LabelDesigners\\";
                string fileName = FileName;
                //ds.WriteXml(Path + fileName);
                LabelDesignHelper hlpr = new LabelDesignHelper();
                var data = hlpr.GenerateLabelXml(ds);
                using (StreamWriter sw = new StreamWriter(Path + fileName))
                {
                    sw.WriteLine(data.ToString());
                }
                string contentType = MimeMapping.GetMimeMapping(Path + fileName);
                return Json(TnT.LangResource.GlobalRes.TempDataLblLytDsgSuccSaved);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
                throw;
            }

        }

        [HttpPost]
        public ActionResult SaveAsLabel(string FileName, string NewFileName)
        {
            try
            {
                if (string.IsNullOrEmpty(FileName) && string.IsNullOrEmpty(NewFileName))
                    return Json(TnT.LangResource.GlobalRes.MsgLblLytDsgprovideFileName);

                object OldFile = new StringBuilder(FileName).ToString();
                object NewFile = new StringBuilder(NewFileName).ToString();
                string xs = (string)OldFile;
                string ys = (string)NewFile;

                if (xs != ys)
                {
                    var PhysicalPath = HttpRuntime.AppDomainAppPath;
                    string Path = PhysicalPath + "Content\\LabelDesigners\\";

                    System.IO.File.Copy(Path + FileName, Path + NewFileName);
                    return Json(TnT.LangResource.GlobalRes.TempDataLblLytDsgSuccSaved);
                }
                else
                {
                    return Json(TnT.LangResource.GlobalRes.MsgLblLytFileNameNotSame);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
                throw;
            }
        }

        private DataTable ConvertFieldDataToDataset(string[] fieldData)
        {
            List<Dictionary<string, string>> DRlst = new List<Dictionary<string, string>>();

            foreach (var item in fieldData)
            {
                string[] FieldRow = item.Split(',');
                int rwCnt = FieldRow.Count();
                Dictionary<string, string> dr = new Dictionary<string, string>();
                foreach (var fld in FieldRow)
                {
                    if (fld != "")
                    {
                        string[] f = fld.Split(':');
                        if (f.Count() >= 2)
                        {
                            if (f[0].ToString() == "DataType")
                            {
                                if (f[1].ToString() != "FDT_IMAGE")
                                {
                                    string Key = f[0].Trim();
                                    string val = f[1].Trim();
                                    dr.Add(Key, val);
                                }
                            }
                            else
                            {
                                string Key = f[0].Trim();
                                string val = f[1].Trim();
                                dr.Add(Key, val);
                            }
                        }

                    }
                }
                DRlst.Add(dr);
            }
            var sdfs = ToDataTable(DRlst);
            return sdfs;
        }

        private DataTable ConvertPageSettingsToDataset(string[] fieldData)
        {
            List<Dictionary<string, string>> DRlst = new List<Dictionary<string, string>>();

            foreach (var item in fieldData)
            {
                string[] FieldRow = item.Split(',');

                int rwCnt = FieldRow.Count() - 1;
                Dictionary<string, string> dr = new Dictionary<string, string>();
                foreach (var fld in FieldRow)
                {
                    if (fld != "")
                    {
                        string[] f = fld.Split(':');
                        string Key = f[0].Trim();
                        string val = f[1].Trim();
                        dr.Add(Key, val);
                    }
                }
                DRlst.Add(dr);
            }
            var sdfs = ToDataTable(DRlst);
            return sdfs;
        }

        private DataTable ToDataTable(List<Dictionary<string, string>> list)
        {
            DataTable result = new DataTable();
            if (list.Count == 0)
                return result;

            var columnNames = list.SelectMany(dict => dict.Keys).Distinct().ToList();

            result.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());
            foreach (Dictionary<string, string> item in list)
            {
                var row = result.NewRow();
                foreach (var key in item.Keys)
                {
                    row[key] = item[key];
                }

                result.Rows.Add(row);
            }
            return result;
        }

        public ActionResult ZPLDesigner()
        {
            BindZPLFiles();
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailLblLytDsgViewLabelLayout, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailLblLytDsgViewLabelLayout, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            return View();
        }

        public string BindZPLFiles()
        {
            var filesNames = lblHelper.getZPLJSONFiles();
            var filArray = filesNames.ToArray();
            return string.Join(",", filArray);
        }

        [HttpPost]
        public ActionResult getZPLImage(LblDgnData ls)
        {

            var img = ls.png.Split(',');

            string imagePath = Server.MapPath("~/Content/LabelDesigners/PrintView/");
            if (!System.IO.Directory.Exists(imagePath))
            {
                System.IO.Directory.CreateDirectory(imagePath);
            }

            byte[] bytes = Convert.FromBase64String(img[1]);
            byte[] zplByte = Convert.FromBase64String(ls.zpl);
            byte[] jsonByte = Convert.FromBase64String(ls.jsonstring);

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (Bitmap bm2 = new Bitmap(ms))
                {
                    bm2.Save(imagePath + ls.lblName + ".png");
                }
            }

            string zplFilePath = Server.MapPath("~/Content/LabelDesigners/");
            if (!System.IO.Directory.Exists(zplFilePath))
            {
                System.IO.Directory.CreateDirectory(zplFilePath);
            }

            System.IO.File.WriteAllBytes(zplFilePath + ls.lblName + ".zpl", zplByte);

            string jsonFilePath = Server.MapPath("~/Content/LabelDesigners/JSON/");
            if (!System.IO.Directory.Exists(jsonFilePath))
            {
                System.IO.Directory.CreateDirectory(jsonFilePath);
            }
            System.IO.File.WriteAllBytes(jsonFilePath + ls.lblName + ".txt", jsonByte);
            trail.AddTrail(ls.lblName + TnT.LangResource.GlobalRes.TrailLblLytLeblesave + User.FirstName, Convert.ToInt32(User.ID), ls.lblName + TnT.LangResource.GlobalRes.TrailLblLytLeblesave + User.FirstName, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            return Json("Success");
        }

        [HttpPost]
        public ActionResult LoadLabel(string fileName)
        {
            string jsonFilePath = Server.MapPath("~/Content/LabelDesigners/JSON/");
            //string data = System.IO.File.ReadAllText(jsonFilePath + fileName, Encoding.GetEncoding("iso-8859-1"));
            string data = System.IO.File.ReadAllText(jsonFilePath + fileName, Encoding.GetEncoding("utf-8"));
            trail.AddTrail(User.FirstName + TnT.LangResource.GlobalRes.TrailLblLytDsgReqLoadLbl + fileName, Convert.ToInt32(User.ID), User.FirstName + TnT.LangResource.GlobalRes.TrailLblLytDsgReqLoadLbl + fileName, TnT.LangResource.GlobalRes.TrailInfoSettingActivity);
            return new JsonResult { Data = data, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public ActionResult SendDataToPrinter(LblPrntData lp)
        {
            if (!string.IsNullOrEmpty(lp.IPAddress) && lp.PortNumber != 0)
            {
                try
                {

                    IPAddress[] iphostInfo = Dns.GetHostAddresses(lp.IPAddress);
                    IPAddress ipAdress = iphostInfo[0];
                    IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, lp.PortNumber);

                    Socket client = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    client.Connect(ipEndpoint);
                    string zplstring = lp.Zpl;
                    ReturnUTF8Hexdata(ref zplstring);
                    byte[] sendmsg = Encoding.ASCII.GetBytes(zplstring);
                    int n = client.Send(sendmsg);
                    client.Shutdown(SocketShutdown.Send);
                    client.Close();

                }

                catch (Exception ex)
                {
                    return Json(TnT.LangResource.GlobalRes.TempDataPrintNotConnected);
                    
                }
            }
            else
            {
                return Json(TnT.LangResource.GlobalRes.TempDataReqPrintrorPort);
            }
            return Json("Success");
        }
        private string ReturnUTF8Hexdata(ref string PrnFileData)
        {
            try
            {
                var unicodeCharacterList = PrnFileData.Distinct()
                .Select(c => c.ToString())
                .Select(c => new { key = c, UTF8Bytes = Encoding.UTF8.GetBytes(c) })
                .Where(c => c.UTF8Bytes.Length > 1);
                foreach (var character in unicodeCharacterList)
                {
                    var characterHexCode = string.Join("", character.UTF8Bytes.Select(c => "_" + BitConverter.ToString(new byte[] { c }).ToLower()).ToArray());
                    PrnFileData = PrnFileData.Replace(character.key, characterHexCode);
                }
                return PrnFileData;
            }
            catch (Exception ex)
            {
                //return Json("Exception at ReturnUTF8Hexdata : {0},{1},{2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace);
            }
            return PrnFileData;
        }
    }
}
    public class LblDgnData
{
    public string zpl { get; set; }
    public string jsonstring { get; set; }
    public string png { get; set; }
    public string lblName { get; set; }
}

public class LblPrntData
{
    public string Zpl { get; set; }
    public string IPAddress { get; set; }
    public int PortNumber { get; set; }
}