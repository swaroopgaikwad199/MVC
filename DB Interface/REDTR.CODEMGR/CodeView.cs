using System;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
//using REDTR.HELPER;

namespace REDTR.CODEMGR
{
    public partial class CodeView : UserControl
    {
        ResourceManager rm;
        CultureInfo cul;

        public CodeView()
        {
            InitializeComponent();
            rm = ResourceManager.CreateFileBasedResourceManager("8_Common", Application.StartupPath + "\\ResourceSet", null);
            cul = CultureInfo.CreateSpecificCulture("de");
        }

        private void CodeView_Load(object sender, EventArgs e)
        {
            CodeView_LangConversion();
        }

        private void CodeView_LangConversion()
        {
            label2.Text = rm.GetString("CodeView.label2", cul);
            label3.Text = rm.GetString("CodeView.label3", cul);
            label4.Text = rm.GetString("CodeView.label4", cul);
            label1.Text = rm.GetString("CodeView.label1", cul);
            label5.Text = rm.GetString("CodeView.label5", cul);
            label6.Text = rm.GetString("CodeView.label6", cul);
        }

        public void SetDisplaybles(bool Disp2DCode, bool Disp2DData, bool Disp2DGrade)
        {
            PAN_2DCode.Visible = Disp2DCode;
            PAN_2DData.Visible = Disp2DData;
            PAN_2DGrade.Visible = Disp2DGrade;
        }
        public CodeData ValueCD
        {
            set
            {
                TXT_GTIN.Text = value.GTIN;
                TXT_LOT.Text = value.BatchNo;
                TXT_EXP.Text = value.ExpiryDate.ToString("20yy/MM/dd");
                TXT_SERIAL.Text = value.SerialNo;
                TXT_2DCode.Text = "";
            }
        }
        public string Value
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    CodeData data = GS1Mgr.DecodeCode(value, "");
                    if (data != null)
                    {
                        if (PAN_2DCode.Visible == true)
                            TXT_2DCode.Text = value;
                        if (PAN_2DData.Visible == true)
                        {
                            TXT_GTIN.Text = data.GTIN;
                            TXT_LOT.Text = data.BatchNo;//.Substring(0, data.BatchNo.Length-1);
                            TXT_EXP.Text = data.ExpiryDate.ToString("20yy/MM/dd");
                            TXT_SERIAL.Text = data.SerialNo;
                        }
                        if (PAN_2DGrade.Visible == true)
                            TXT_GRADE.Text = "";
                    }
                }
                else
                {
                    TXT_GTIN.Text = "";
                    TXT_LOT.Text = "";
                    TXT_EXP.Text = "";
                    TXT_SERIAL.Text = "";
                    TXT_2DCode.Text = "";
                }
            }
        }
    }
}
