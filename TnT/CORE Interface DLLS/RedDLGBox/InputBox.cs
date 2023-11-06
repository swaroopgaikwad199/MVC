using System;
using System.Windows.Forms;
using RedKB;

namespace Red.Utils
{
    public partial class InputBox : Form
    {
        KeyBoard kb = new KeyBoard();
        
        public KeyBoard.KeyBoardType KeyBoardType
        {
            get { return kb.KBType; }
            set { if (kb != null) kb.KBType = value; }
        }
        public InputBox(string Msg, string Header)
        {
            InitializeComponent();
            label1.Text = Msg;
            this.Text = Header;
            kb.KBType = KeyBoard.KeyBoardType.ALFANUMERIC;
            kb.OnTextChange += new KeyBoard.TextChanged(KeyBordTextChanged);
            //kb.StartPosition = FormStartPosition.WindowsDefaultLocation;
            //kb.Location = new Point(this.Right, this.Bottom + this.Top);
            kb.MaxLength = 30;
        }
        public void InitKB(KeyBoard.KeyBoardType kbType, int MaxCharLen)
        {
            if (KeyBoard.KeyBoardType.IsDefined(typeof(KeyBoard.KeyBoardType), kbType) == false)
                kbType = KeyBoard.KeyBoardType.ALFANUMERIC;
            if (MaxCharLen <= 0)
                MaxCharLen = 30;

            kb.KBType = kbType;
            kb.MaxLength = MaxCharLen;
        }
        public void InitKB(KeyBoard.KeyBoardType kbType, int MaxCharLen, int maxVal, int minVal)
        {
            if (KeyBoard.KeyBoardType.IsDefined(typeof(KeyBoard.KeyBoardType), kbType) == false)
                kbType = KeyBoard.KeyBoardType.ALFANUMERIC;
            if (MaxCharLen <= 0)
                MaxCharLen = 30;

            kb.KBType = kbType;
            kb.MaxLength = MaxCharLen;
            
            kb.MaxValue = maxVal;
            kb.MinValue = minVal;
        }
        public string Value
        {
            get { return txtData.Text; }
        }
        private void BTNOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
        private void BTNCancel_Click(object sender, EventArgs e)
        {
            //kb.Dispose();
            DialogResult = DialogResult.Cancel;
        }
        private void KeyBordTextChanged(Object sender, EventArgs e)
        {
            KeyBoard kb = (KeyBoard)sender;
            txtData.Text = kb.KBText;
        }
        private void txtData_Enter(object sender, EventArgs e)
        {
            if (txtData.Text != "0")
                kb.KBText = txtData.Text;
            kb.MaxValue = txtData.MaxLength;
            kb.ShowDialog();
            //BTNOk_Click(null,null);
        }
        private void txtData_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtData.Text != "0")
                kb.KBText = txtData.Text;
            kb.MaxValue = txtData.MaxLength;
            kb.ShowDialog();
           // BTNOk_Click(null, null);
        }
    }
}
