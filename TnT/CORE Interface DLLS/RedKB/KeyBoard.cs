using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using REDTR.UTILS.SystemIntegrity;
namespace PTPLKB
{
    public partial class KeyBoard : Form
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);


        enum KyeBoardDisplayPosiion
        {
            CENTER,
            BOTTOM,
            CUSTOM
        }
        public enum KeyBoardType
        {
            NUMERIC,
            ALFA,
            ALFANUMERIC,
            ALL,
            USE_FILETER
        }
        public delegate void TextChanged(object sender, EventArgs e);
        public TextChanged OnTextChange;
        private KeyBoardType m_KbType = KeyBoardType.ALL;
        private string alfa;

        public string Alfa
        {
            get { return alfa; }
        }
        private string alfaNumeric;

        public string AlfaNumeric
        {
            get { return alfaNumeric; }
            set { alfaNumeric = value; }
        }
        private string numeric;
        private int m_MaxLength = 256;
        private int m_MaxValue = 0xFFFFFFF;
        private int m_MinValue = 0;

        public int MinValue
        {
            get { return m_MinValue; }
            set { m_MinValue = value; }
        }

        public int MaxValue
        {
            get { return m_MaxValue; }
            set { m_MaxValue = value; }
        }
        public int MaxLength
        {
            get { return m_MaxLength; }
            set { m_MaxLength = value; TXTKbData.MaxLength = MaxLength; }
        }

        public KeyBoardType KBType
        {
            get { return m_KbType; }
            set { m_KbType = value; }
        }

        private string m_filter;

        public string Filter
        {
            get { return m_filter; }
            set { m_filter = value; }
        }

        public KeyBoard()
        {
            CheckLicAvailability(Application.StartupPath);
            InitializeComponent();
            alfa = "+a+b+c+d+e+f+g+h+i+j+k+l+m+n+o+p+q+r+s+t+u+v+w+x+y+zabcdefghijklmnopqrstuvwxyz{BACKSPACE}";
            alfaNumeric = alfa + "-.1234567890";
            numeric = "1234567890{BACKSPACE}";
            if (Globals.language == "German")
            {
                BTN_Cancel.Text = "Abbrechen"; BTN_Ok.Text = "Ok";
            }
            else
                BTN_Cancel.Text = "CANCEL";
        }
        private void KeyBoard_Load(object sender, EventArgs e)
        {
           
        }
        private void KeyBoard_Shown(object sender, EventArgs e)
        {
            keyboardcontrol1.CapsLock(Control.IsKeyLocked(Keys.CapsLock));
            TXTKbData.Focus();
            if (TXTKbData.Text != null)
                TXTKbData.SelectionStart = TXTKbData.Text.Length;
        }
        
        private void BTN_Ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BTN_Cancel_Click(object sender, EventArgs e)
        {
            TXTKbData.Text = mOriKBText;
            this.Close();
        }

        private string mOriKBText;
        public string KBText
        {
            get { return TXTKbData.Text; }
            set { mOriKBText = TXTKbData.Text = value; }
        }
        public Char PasswordChar
        {
            get { return TXTKbData.PasswordChar; }
            set { TXTKbData.Multiline = false; TXTKbData.PasswordChar = value; }
        }
        
        private void keyboardcontrol1_UserKeyPressed(object sender,PTPLKB.KeyboardEventArgs e)
        {
            TXTKbData.Focus();
            if (e != null && e.KeyboardKeyPressed != null)
            {
                if (e.KeyboardKeyPressed == "{CAPSLOCK}")
                {
                    const int KEYEVENTF_EXTENDEDKEY = 0x1;
                    const int KEYEVENTF_KEYUP = 0x2;
                    keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
                    keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
                }
                else if (KBType == KeyBoardType.ALL)
                {
                    SendKeys.Send(e.KeyboardKeyPressed);
                }
                else if (KBType == KeyBoardType.ALFA && alfa.Contains(e.KeyboardKeyPressed.ToString()))
                {
                    SendKeys.Send(e.KeyboardKeyPressed);
                }
                else if (KBType == KeyBoardType.ALFANUMERIC && alfaNumeric.Contains(e.KeyboardKeyPressed.ToString()))
                {
                    SendKeys.Send(e.KeyboardKeyPressed);
                }
                else if (KBType == KeyBoardType.NUMERIC && numeric.Contains(e.KeyboardKeyPressed.ToString()))
                {
                    SendKeys.Send(e.KeyboardKeyPressed);
                }
                else if (KBType == KeyBoardType.USE_FILETER && !string.IsNullOrEmpty(m_filter) && m_filter.Contains(e.KeyboardKeyPressed.ToString()))
                {
                    SendKeys.Send(e.KeyboardKeyPressed);
                }
            }
        }

        private void TXTKbData_TextChanged(object sender, EventArgs e)
        {
            if (KBType == KeyBoardType.NUMERIC)
            {
                if (TXTKbData.Text != string.Empty && m_MaxValue != 0)
                {
                    int val = 0;
                    if (int.TryParse(TXTKbData.Text, out val) == true)
                    {
                        if (val > m_MaxValue)
                            TXTKbData.Text = m_MaxValue.ToString();
                        else if (val < m_MinValue)
                            TXTKbData.Text = m_MinValue.ToString();
                    }
                    else
                    {
                        TXTKbData.Text = m_MinValue.ToString();
                    }
                }
            }
            if (OnTextChange != null)
                OnTextChange(this, null);
        }
        private void TXTKbData_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == Keys.CapsLock)
            {
                keyboardcontrol1.CapsLock(Control.IsKeyLocked(Keys.CapsLock));
            }
        }
        private void TXTKbData_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.CapsLock)
                keyboardcontrol1.CapsLock(Control.IsKeyLocked(Keys.CapsLock));
        }
        

        private void CheckLicAvailability(string AppStartPath)
        {
            return;
            //rgmyekcil.rgmyekcilDecode keyGen = new rgmyekcil.rgmyekcilDecode();
            //string keyFile = rgmyekcil.LicKeyData. (AppStartPath);
            bool haslic =true; //keyGen.DecodeLic(keyFile);
            bool HasValidLic = false;
            if (haslic == true)
            {
                bool opt1 = true;// keyGen.HasValidSystem();
                bool opt2 = true;// keyGen.HasExecValidity();
                if (opt1 == true && opt2 == true)
                {
                    HasValidLic = true;
                }
                else if (true== true)
                {
                    HasValidLic = true;
                }
                else
                    Trace.TraceInformation("SystemIntigrityError @ InitKB {0},{1},{2}", DateTime.Now, opt1, opt2);
            }
            Trace.Assert(HasValidLic, "FAILED TO INITIALIZE THE SYSTEM.", "CRITICAL FAILURE");
            if (HasValidLic == false)
            {
                Trace.TraceWarning("SystemIntigrityError @ InitKB, {0},{1},{2},", AppStartPath, haslic, HasValidLic);
                Process proc = Process.GetCurrentProcess();
                proc.Kill();
                return;
            }
        }
    }
}