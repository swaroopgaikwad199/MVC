using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using TnT.Models.LblLytDsg;
using Westwind.Web.Mvc;

namespace TnT.DataLayer.LabelDesigner
{
    public class LabelDesignHelper
    {
        string Path;
        string ZPLPath;
        public LabelDesignHelper()
        {
            var PhysicalPath = HttpRuntime.AppDomainAppPath;
            Path = PhysicalPath+ "/Content/LabelDesigners/";
            ZPLPath = PhysicalPath + "/Content/LabelDesigners/JSON/";
        }
        public List<string> getLabelDesignerFiles()
        {
            string folderPath = Path;
            string[] files = null;
            List<string> lFiles = new List<string>();
            if (Directory.Exists(folderPath))
            {
                files = new DirectoryInfo(folderPath).GetFiles().Select(o => o.Name).ToArray();
                foreach (var item in files)
                {
                    lFiles.Add(item);
                }
            }

            return lFiles;
        }

        public string[] getZPLJSONFiles()
        {
            string folderPath = ZPLPath;
            string[] files = null;
            List<string> lFiles = new List<string>();
            if (Directory.Exists(ZPLPath))
            {
                files = new DirectoryInfo(ZPLPath).GetFiles().Select(o => o.Name).ToArray();
                foreach (var item in files)
                {
                    lFiles.Add(item);
                }
            }

            return files;
        }

        private DataSet ReadAllContentfromFile(string FileName)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(Path + FileName);
            return ds;
        }

        public string GetFieldSetView(string FileName)
        {
            var ds = ReadAllContentfromFile(FileName);            
            if (ds.Tables.Contains("Field") && ds.Tables.Contains("Field_Appearance"))
            {
                var FieldSets = BindFieldSets(ds.Tables["Field_Appearance"]);
                FieldSets.Fields = BindFields(ds.Tables["Field"]);
                var data = ViewRenderer.RenderPartialView("~/Views/LblLytDsg/partialViewFieldProps.cshtml", FieldSets);
                return data;
            }
            else
            {
                return TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            }
        }

        // Below function used to show product page Label Preview.
        public string GetFieldSetPreview(string FileName)
        {
            var ds = ReadAllContentfromFile(FileName);
            if (ds.Tables.Contains("Field") && ds.Tables.Contains("Field_Appearance"))
            {
                var FieldSets = BindFieldSets(ds.Tables["Field_Appearance"]);
                FieldSets.Fields = BindFields(ds.Tables["Field"]);
                var data = ViewRenderer.RenderPartialView("~/Views/Product/labelPreview.cshtml", FieldSets);
                return data;
            }
            else
            {
                return TnT.LangResource.GlobalRes.toastrLblLytDsgNoData;
            }
        }
        private List<LabelDesignerFields> BindFields(DataTable dt)
        {
            double Left, Top, Width, FontSizeOrHeight;
            List<LabelDesignerFields> favm = new List<LabelDesignerFields>();
            foreach (DataRow item in dt.Rows)
            {
                LabelDesignerFields vm = new LabelDesignerFields();
                if (!string.IsNullOrEmpty(item["RPTMap"].ToString())) vm.RPTMap = item["RPTMap"].ToString(); else vm.RPTMap = "";
                if (!string.IsNullOrEmpty(item["LabelName"].ToString())) vm.LabelName = item["LabelName"].ToString(); else vm.LabelName = "";
                if (!string.IsNullOrEmpty(item["DataType"].ToString())) vm.DataType = item["DataType"].ToString(); else vm.DataType = "";
                if (!string.IsNullOrEmpty(item["Data"].ToString())) vm.Data = item["Data"].ToString(); else vm.Data = "";
                if (!string.IsNullOrEmpty(item["DataField"].ToString())) vm.DataField = item["DataField"].ToString(); else vm.DataField = "";
                Double.TryParse(item["Left"].ToString(), out Left);
                Double.TryParse(item["Top"].ToString(), out Top);
                Double.TryParse(item["Width"].ToString(), out Width);
                Double.TryParse(item["FontSizeOrHeight"].ToString(), out FontSizeOrHeight);

                if (!string.IsNullOrEmpty(item["Left"].ToString())) vm.Left = Left; else vm.Left = 0;
                if (!string.IsNullOrEmpty(item["Top"].ToString())) vm.Top = Top; else vm.Top = 0;
                if (!string.IsNullOrEmpty(item["Width"].ToString())) vm.Width = Width; else vm.Width = 0;
                if (!string.IsNullOrEmpty(item["FontSizeOrHeight"].ToString())) vm.FontSizeOrHeight = FontSizeOrHeight; else vm.FontSizeOrHeight = 0;

                if (!string.IsNullOrEmpty(item["IsBold"].ToString())) vm.IsBold = Convert.ToBoolean(item["IsBold"]); else vm.IsBold = false;
                if (!string.IsNullOrEmpty(item["IsShow"].ToString())) vm.IsShow = Convert.ToBoolean(item["IsShow"]); else vm.IsShow = false;
                if (!string.IsNullOrEmpty(item["IsManualData"].ToString())) vm.IsManualData = Convert.ToBoolean(item["IsManualData"]); else vm.IsManualData = false;
                if (!string.IsNullOrEmpty(item["Rotate"].ToString())) vm.Rotate = Convert.ToInt32(item["Rotate"]); else vm.Rotate = 0;
                

                if (!string.IsNullOrEmpty(vm.LabelName))
                {
                    favm.Add(vm);
                }
            }
            return favm;
        }

        private FieldAppearanceViewModel BindFieldSets(DataTable dt)
        {
            double PaperSizeHeight, TopMargin, LeftMargin, BottomMargin, RightMargin, PaperSizeWidth;
            FieldAppearanceViewModel fvm = new FieldAppearanceViewModel();
            DataRow item = dt.Rows[0];

            if (!string.IsNullOrEmpty(item["PaperSize"].ToString())) fvm.PaperSize = item["PaperSize"].ToString(); else fvm.PaperSize = "";
            if (!string.IsNullOrEmpty(item["PaperSizePaperKind"].ToString())) fvm.PaperSizePaperKind = item["PaperSizePaperKind"].ToString(); else fvm.PaperSizePaperKind = "";
            if (!string.IsNullOrEmpty(item["DateFormat"].ToString())) fvm.DateFormat = item["DateFormat"].ToString(); else fvm.DateFormat = "";
            if (!string.IsNullOrEmpty(item["OrientationType"].ToString())) fvm.OrientationType = item["OrientationType"].ToString(); else fvm.OrientationType = "";
            if (!string.IsNullOrEmpty(item["Font"].ToString())) fvm.Font = item["Font"].ToString(); else fvm.Font = "";
            if (!string.IsNullOrEmpty(item["ZFont"].ToString())) fvm.Zfont = item["ZFont"].ToString(); else fvm.Zfont= "";
            if (!string.IsNullOrEmpty(item["Dpi"].ToString())) fvm.Dpi= item["Dpi"].ToString(); else fvm.Dpi= "";
            
            Double.TryParse(item["PaperSizeHeight"].ToString(),out PaperSizeHeight);
            Double.TryParse(item["TopMargin"].ToString(), out TopMargin);
            Double.TryParse(item["LeftMargin"].ToString(), out LeftMargin);
            Double.TryParse(item["BottomMargin"].ToString(), out BottomMargin);
            Double.TryParse(item["RightMargin"].ToString(), out RightMargin);
            Double.TryParse(item["PaperSizeWidth"].ToString(), out PaperSizeWidth);
            if (!string.IsNullOrEmpty(item["PaperSizeHeight"].ToString())) fvm.PaperSizeHeight = PaperSizeHeight; else fvm.PaperSizeHeight = 0;
            if (!string.IsNullOrEmpty(item["PaperSizeWidth"].ToString())) fvm.PaperSizeWidth = PaperSizeWidth; else fvm.PaperSizeWidth = 0;
            if (!string.IsNullOrEmpty(item["TopMargin"].ToString())) fvm.TopMargin = TopMargin; else fvm.TopMargin = 0;
            if (!string.IsNullOrEmpty(item["LeftMargin"].ToString())) fvm.LeftMargin = LeftMargin; else fvm.LeftMargin = 0;
            if (!string.IsNullOrEmpty(item["BottomMargin"].ToString())) fvm.BottomMargin = BottomMargin; else fvm.BottomMargin = 0;
            if (!string.IsNullOrEmpty(item["RightMargin"].ToString())) fvm.RightMargin = RightMargin; else fvm.RightMargin = 0;
            if (!string.IsNullOrEmpty(item["PaperHeight"].ToString())) fvm.PaperHeight = Convert.ToInt32(item["PaperHeight"]); else fvm.PaperHeight = 0;
            
            fvm.LstDateFormats = BindDateFormats();
            fvm.LstDpi = BindDpi();
            fvm.LstFonts = BindFonts();
            fvm.LstOrientationTypes = BindOrientationTypes();
            fvm.LstPaperSize = BindPaperSize();
            fvm.LstZFonts = BindZFonts();

            return fvm;

        }

        public StringBuilder GenerateLabelXml(DataSet ds)
        {
            XmlWriter writer = null;
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF32;
            settings.OmitXmlDeclaration = true;

            StringBuilder sw = new StringBuilder();
            if (ds.Tables[0].Rows.Count > 0)
            {
                using (writer = XmlWriter.Create(sw, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("ArrayOfField_Appearance");
                    writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
                    writer.WriteStartElement("Field_Appearance");
                    writer.WriteStartElement("FieldProps");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string RPTMAP = ds.Tables[0].Rows[i]["RPTMAP"].ToString();
                        string LabelName = ds.Tables[0].Rows[i]["LabelName"].ToString();
                        string Left = ds.Tables[0].Rows[i]["Left"].ToString();
                        string Top = ds.Tables[0].Rows[i]["Top"].ToString();
                        string Width = ds.Tables[0].Rows[i]["Width"].ToString();
                        string FontSizeOrHeight = ds.Tables[0].Rows[i]["FontSizeOrHeight"].ToString();
                        string IsBold = ds.Tables[0].Rows[i]["IsBold"].ToString();
                        string IsShow = ds.Tables[0].Rows[i]["IsShow"].ToString();
                        string IsManualData = ds.Tables[0].Rows[i]["IsManualData"].ToString();
                        string DataType = ds.Tables[0].Rows[i]["DataType"].ToString();
                        string Data = ds.Tables[0].Rows[i]["Data"].ToString();
                        string DATAField = ds.Tables[0].Rows[i]["DATAField"].ToString();
                        string Rotate = ds.Tables[0].Rows[i]["Rotate"].ToString();
                        writer.WriteStartElement("Field");
                        writer.WriteElementString("RPTMAP", RPTMAP);
                        writer.WriteElementString("LabelName", LabelName);
                        writer.WriteElementString("Left", Left);
                        writer.WriteElementString("Top", Top);
                        writer.WriteElementString("Width", Width);
                        writer.WriteElementString("FontSizeOrHeight", FontSizeOrHeight);
                        writer.WriteElementString("IsBold", IsBold);
                        writer.WriteElementString("IsShow", IsShow);
                        writer.WriteElementString("IsManualData", IsManualData);
                        writer.WriteElementString("DataType", DataType);
                        writer.WriteElementString("Data", Data);
                        writer.WriteElementString("Rotate", Rotate);
                        writer.WriteElementString("DATAField", DATAField);
                        //writer.WriteElementString("Rotate", RotateAngel);
                        writer.WriteEndElement();//Field
                    }
                    writer.WriteEndElement();//FieldProps
                    string PaperSize = ds.Tables[1].Rows[0].ItemArray[1].ToString();
                    string PaperSizePaperKind = ds.Tables[1].Rows[0].ItemArray[0].ToString();
                    string PaperSizeHeight = ds.Tables[1].Rows[0].ItemArray[2].ToString();
                    string PaperSizeWidth = ds.Tables[1].Rows[0].ItemArray[3].ToString();
                    string TopMargin = ds.Tables[1].Rows[0].ItemArray[8].ToString();
                    string LeftMargin = ds.Tables[1].Rows[0].ItemArray[9].ToString();
                    string BottomMargin = ds.Tables[1].Rows[0].ItemArray[10].ToString();
                    string RightMargin = ds.Tables[1].Rows[0].ItemArray[11].ToString();
                    string DateFormat = ds.Tables[1].Rows[0].ItemArray[6].ToString();
                    string OrientationType = ds.Tables[1].Rows[0].ItemArray[5].ToString();
                    string Font = ds.Tables[1].Rows[0].ItemArray[12].ToString();
                    string Zfont = ds.Tables[1].Rows[0].ItemArray[13].ToString();
                    string Dpi = ds.Tables[1].Rows[0].ItemArray[7].ToString();
                    string PaperHeight = ds.Tables[1].Rows[0].ItemArray[4].ToString();

                    writer.WriteElementString("PaperSize", PaperSize);
                    writer.WriteElementString("PaperSizePaperKind", PaperSizePaperKind);
                    writer.WriteElementString("PaperSizeHeight", PaperSizeHeight);
                    writer.WriteElementString("PaperSizeWidth", PaperSizeWidth);
                    writer.WriteElementString("TopMargin", TopMargin);
                    writer.WriteElementString("LeftMargin", LeftMargin);
                    writer.WriteElementString("BottomMargin", BottomMargin);
                    writer.WriteElementString("RightMargin", RightMargin);
                    writer.WriteElementString("DateFormat", DateFormat);
                    writer.WriteElementString("OrientationType", OrientationType);
                    writer.WriteElementString("Font", Font);
                    writer.WriteElementString("Zfont", Zfont);
                    writer.WriteElementString("Dpi", Dpi);
                    writer.WriteElementString("PaperHeight", PaperHeight);



                    writer.WriteEndElement();//End Field_Appearance
                    writer.WriteEndDocument();
                }
            }
            return sw;
        }


        #region Bind DropDownLists

        private IEnumerable<SelectListItem> BindDateFormats()
        {
            var Dates = GeneralUtilities.getDateFormats();
            Dates.Insert(0, "Select");
            var dts = Dates.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
            return dts;
        }

        private IEnumerable<SelectListItem> BindDpi()
        {
            var Dates = GeneralUtilities.getDPIs();
            Dates.Insert(0, "Select");
            var dts = Dates.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
            return dts;
        }
        private IEnumerable<SelectListItem> BindFonts()
        {
            var Dates = GeneralUtilities.getSystemFonts();
            Dates.Insert(0, "Select");
            var dts = Dates.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
            return dts;
        }

        private IEnumerable<SelectListItem> BindOrientationTypes()
        {
            var Dates = GeneralUtilities.getOrientationTypes();
            Dates.Insert(0, "Select");
            var dts = Dates.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
            return dts;
        }

        private IEnumerable<SelectListItem> BindPaperSize()
        {
            var papers = GeneralUtilities.getPaperSize().ToList();
            papers.Insert(0,new KeyValuePair<string, string>("Select","0"));
            var dts = papers.Select(x => new SelectListItem() { Value = x.Value.ToString(), Text = x.Key.ToString() });
            return dts;
        }

        private IEnumerable<SelectListItem> BindZFonts()
        {
            var zFonts = GeneralUtilities.getZPLFonts();
            zFonts.Insert(0, "Select");
            var dts = zFonts.Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
            return dts;
        }


        #endregion

    }
}