using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TnT.Models.LblLytDsg
{
    public class FieldAppearanceViewModel
    {
        public string PaperSize { get; set; }
        public IEnumerable<SelectListItem> LstPaperSize { get; set; }

        public string PaperSizePaperKind { get; set; }

        public double PaperSizeHeight { get; set; }

        public double PaperSizeWidth { get; set; }

        public double TopMargin { get; set; }

        public double LeftMargin { get; set; }

        public double BottomMargin { get; set; }

        public double RightMargin { get; set; }

        public string DateFormat { get; set; }
        public IEnumerable<SelectListItem> LstDateFormats { get; set; }

        public string OrientationType { get; set; }
        public IEnumerable<SelectListItem> LstOrientationTypes { get; set; }

        public string Font { get; set; }
        public IEnumerable<SelectListItem> LstFonts { get; set; }

        public string Zfont { get; set; }
        public IEnumerable<SelectListItem> LstZFonts { get; set; }

        public string Dpi { get; set; }
        public IEnumerable<SelectListItem> LstDpi { get; set; }
        public int PaperHeight { get; set; }

        public List<LabelDesignerFields> Fields { get; set; }
    }


    public class LabelDesignerFields
    {
        public string RPTMap { get; set; }
        public string LabelName { get; set; }
        public double Left { get; set; }

        public double Top { get; set; }

        public double Width { get; set; }

        public double FontSizeOrHeight { get; set; }

        public bool IsBold { get; set; }

        public bool IsShow { get; set; }

        public bool IsManualDate { get; set; }

        public string DataType { get; set; }
        public string Data { get; set; }
        public int Rotate { get; set; }
        public bool IsManualData { get; set; }
        public string DataField { get; set; }

    }
}