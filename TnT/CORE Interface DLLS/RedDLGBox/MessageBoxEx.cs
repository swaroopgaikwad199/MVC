using System;
using System.Drawing;
using System.Windows.Forms;
using REDMSG;
using REDTR.UTILS.SystemIntegrity;
public partial class MessageBoxEx : Form
{
    public delegate void MessageHandler(DialogResult res, object param);
    public static int MsgCounts = 0;
    object m_store;
    public bool IsAsync
    {
        get { return m_IsAsync; }
        set
        {
            if (m_IsAsync == value)
                return;

            m_IsAsync = value;
            if (m_IsAsync == true)
            {
                foreach (Control ctrl in BTN_Panel.Controls)
                {
                    ((Button)ctrl).Click += new EventHandler(MessageBoxEx_Click);
                }
            }
        }
    }
    public enum MessageBoxIcon
    {
        Error,
        Explorer,
        Find,
        Information,
        Mail,
        Media,
        Print,
        Question,
        RecycleBinEmpty,
        RecycleBinFull,
        Stop,
        User,
        Warning
    }
    public enum MessageBoxButtonsEx
    {
        AbortRetryIgnore,
        OK,
        OKCancel,
        RetryCancel,
        YesNo,
        YesNoCancel,
        PauseStopCancel,
        LoadNewCancel,
        Start
    }
    public static bool CustomMsg = true;
    public static string MsgBoxTitle = "PTPLDLGBox";
    public MessageHandler OnMessageClose;
    private bool m_IsAsync = false;
    private void MessageBoxEx_Click(object Sender, EventArgs e)
    {
        if (OnMessageClose != null)
            OnMessageClose(DialogResult, m_store);
    }
    static public DialogResult Show(string Text, int msgid)
    {
        if (CustomMsg == true)
        {
            if (msgid > 0)
            {
                try
                {
                    if (RedMessage.IsInit == true)
                    {
                        Text = RedMessage.MsgList[msgid].ToString();
                        if (RedMessage.PrevFileIsNum == true && RedMessage.PrevFile != 0)
                        {
                            Text = RedMessage.PrevFile.ToString() + " " + Text;
                        }
                        else if (RedMessage.PrevFile != 0)
                        {
                            Text = RedMessage.MsgList[RedMessage.PrevFile].ToString() + " " + Text;
                        }
                        if (RedMessage.NextFileIsNum == true && RedMessage.NextFile != 0)
                        {
                            Text = Text + " " + RedMessage.NextFile.ToString();
                        }
                        else if (RedMessage.NextFile != 0)
                        {
                            Text = Text + " " + RedMessage.MsgList[RedMessage.NextFile].ToString();
                        }

                        REDMSG.RedMessage.Play(msgid);
                    }
                }
                catch
                {
                }
            }
            using (MessageBoxEx dialog = new MessageBoxEx(MsgBoxTitle, Text, global::PTPLDLGBox.Properties.Resources.SmallLogo, MessageBoxButtonsEx.OK))
            {
                MsgCounts++;
                DialogResult result = dialog.ShowDialog();
                dialog.Font.Dispose();
                dialog.Dispose();
                return result;
            }
        }
        else
        {
            return MessageBox.Show(Text, Application.ProductName);
        }
    }
    static public DialogResult Show(string Text, string Caption, MessageBoxButtonsEx btns, int msgid)
    {
        if (CustomMsg == true)
        {
            if (msgid > 0)
            {
                try
                {
                    if (RedMessage.IsInit)
                    {
                        Text = RedMessage.MsgList[msgid].ToString();
                        if (RedMessage.PrevFileIsNum == true && RedMessage.PrevFile != 0)
                        {
                            Text = RedMessage.PrevFile.ToString() + " " + Text;
                        }
                        else if (RedMessage.PrevFile != 0)
                        {
                            Text = RedMessage.MsgList[RedMessage.PrevFile].ToString() + " " + Text;
                        }
                        if (RedMessage.NextFileIsNum == true && RedMessage.NextFile != 0)
                        {
                            Text = Text + " " + RedMessage.NextFile.ToString();
                        }
                        else if (RedMessage.NextFile != 0)
                        {
                            Text = Text + " " + RedMessage.MsgList[RedMessage.NextFile].ToString();
                        }
                        REDMSG.RedMessage.Play(msgid);
                    }
                }
                catch
                {
                }
            }
            using (MessageBoxEx dialog = new MessageBoxEx(Caption, Text, global::PTPLDLGBox.Properties.Resources.SmallLogo, btns))
            {
                MsgCounts++;
                DialogResult result = dialog.ShowDialog();
                dialog.Font.Dispose();
                dialog.Dispose();
                return result;
            }
        }
        else
        {
            MessageBoxButtons btn;
            switch (btns)
            {
                case MessageBoxButtonsEx.OK:
                    btn = MessageBoxButtons.OK;
                    break;
                case MessageBoxButtonsEx.OKCancel:
                    btn = MessageBoxButtons.OKCancel;
                    break;
                case MessageBoxButtonsEx.RetryCancel:
                    btn = MessageBoxButtons.RetryCancel;
                    break;
                case MessageBoxButtonsEx.YesNo:
                    btn = MessageBoxButtons.YesNo;
                    break;
                case MessageBoxButtonsEx.YesNoCancel:
                    btn = MessageBoxButtons.YesNoCancel;
                    break;
                case MessageBoxButtonsEx.AbortRetryIgnore:
                    btn = MessageBoxButtons.AbortRetryIgnore;
                    break;
                case MessageBoxButtonsEx.PauseStopCancel:
                    btn = MessageBoxButtons.AbortRetryIgnore;
                    break;
                case MessageBoxButtonsEx.LoadNewCancel:
                    btn = MessageBoxButtons.YesNoCancel;
                    break;
                case MessageBoxButtonsEx.Start:
                    btn = MessageBoxButtons.OK;
                    break;
                default:
                    btn = MessageBoxButtons.OKCancel;
                    break;
            }
            return MessageBox.Show(Text, Caption, btn);
        }
    }
    static public DialogResult Show(string Text, string Caption, MessageBoxButtonsEx btns, int msgid, Point location)
    {
        if (CustomMsg == true)
        {
            if (msgid > 0)
            {
                try
                {
                    if (RedMessage.IsInit)
                    {
                        Text = RedMessage.MsgList[msgid].ToString();
                        if (RedMessage.PrevFileIsNum == true && RedMessage.PrevFile != 0)
                        {
                            Text = RedMessage.PrevFile.ToString() + " " + Text;
                        }
                        else if (RedMessage.PrevFile != 0)
                        {
                            Text = RedMessage.MsgList[RedMessage.PrevFile].ToString() + " " + Text;
                        }
                        if (RedMessage.NextFileIsNum == true && RedMessage.NextFile != 0)
                        {
                            Text = Text + " " + RedMessage.NextFile.ToString();
                        }
                        else if (RedMessage.NextFile != 0)
                        {
                            Text = Text + " " + RedMessage.MsgList[RedMessage.NextFile].ToString();
                        }
                        REDMSG.RedMessage.Play(msgid);
                    }
                }
                catch
                {
                }
            }
            using (MessageBoxEx dialog = new MessageBoxEx(Caption, Text, global::PTPLDLGBox.Properties.Resources.SmallLogo, btns))
            {
                MsgCounts++;
                dialog.StartPosition = FormStartPosition.Manual;
                dialog.Location = location;
                DialogResult result = dialog.ShowDialog();
                dialog.Font.Dispose();
                dialog.Dispose();
                return result;
            }
        }
        else
        {
            MessageBoxButtons btn;
            switch (btns)
            {
                case MessageBoxButtonsEx.OK:
                    btn = MessageBoxButtons.OK;
                    break;
                case MessageBoxButtonsEx.OKCancel:
                    btn = MessageBoxButtons.OKCancel;
                    break;
                case MessageBoxButtonsEx.RetryCancel:
                    btn = MessageBoxButtons.RetryCancel;
                    break;
                case MessageBoxButtonsEx.YesNo:
                    btn = MessageBoxButtons.YesNo;
                    break;
                case MessageBoxButtonsEx.YesNoCancel:
                    btn = MessageBoxButtons.YesNoCancel;
                    break;
                case MessageBoxButtonsEx.AbortRetryIgnore:
                    btn = MessageBoxButtons.AbortRetryIgnore;
                    break;
                case MessageBoxButtonsEx.PauseStopCancel:
                    btn = MessageBoxButtons.AbortRetryIgnore;
                    break;
                case MessageBoxButtonsEx.LoadNewCancel:
                    btn = MessageBoxButtons.YesNoCancel;
                    break;
                case MessageBoxButtonsEx.Start:
                    btn = MessageBoxButtons.OK;
                    break;
                default:
                    btn = MessageBoxButtons.OKCancel;
                    break;
            }
            return MessageBox.Show(Text, Caption, btn);
        }
    }
    static public DialogResult Show(string Text, string Caption, string btn1, string btn2, string btn3, int msgid)
    {
        if (CustomMsg == true)
        {
            if (msgid > 0)
            {
                try
                {
                    if (RedMessage.IsInit)
                    {
                        Text = RedMessage.MsgList[msgid].ToString();
                        if (RedMessage.PrevFileIsNum == true && RedMessage.PrevFile != 0)
                        {
                            Text = RedMessage.PrevFile.ToString() + " " + Text;
                        }
                        else if (RedMessage.PrevFile != 0)
                        {
                            Text = RedMessage.MsgList[RedMessage.PrevFile].ToString() + " " + Text;
                        }
                        if (RedMessage.NextFileIsNum == true && RedMessage.NextFile != 0)
                        {
                            Text = Text + " " + RedMessage.NextFile.ToString();
                        }
                        else if (RedMessage.NextFile != 0)
                        {
                            Text = Text + " " + RedMessage.MsgList[RedMessage.NextFile].ToString();
                        }
                        REDMSG.RedMessage.Play(msgid);
                    }
                }
                catch
                {
                }
            }
            using (MessageBoxEx dialog = new MessageBoxEx(Caption, Text.ToUpper(), null, btn1, btn2, btn3))
            {
                MsgCounts++;
                DialogResult result = dialog.ShowDialog();
                dialog.Font.Dispose();
                dialog.Dispose();
                return result;
            }
        }
        else
        {
            return MessageBox.Show(Text, Caption, MessageBoxButtons.YesNoCancel);
        }
    }
    public void ShowAsync(string Text, string Caption, MessageBoxButtonsEx btns, object param)
    {
        using (MessageBoxEx dialog = new MessageBoxEx(Caption, Text, global::PTPLDLGBox.Properties.Resources.SmallLogo, btns))
        {
            MsgCounts++;
            dialog.TopLevel = true;
            dialog.TopMost = true;
            dialog.Parent = this;
            m_store = param;
            dialog.Show();
        }
    }

    private void AddButton(string Name, int TabIndex, DialogResult res)
    {
        Red.Controls.Buttons.RedGlowButton button = new Red.Controls.Buttons.RedGlowButton();
        button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
        button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        button.ForeColor = System.Drawing.Color.White;
        button.Name = Name;
        button.Size = new System.Drawing.Size(100, 40);
        button.TabIndex = TabIndex;
        button.BaseColor = Color.FromArgb(161, 161, 161);
        button.ButtonColor = Color.FromArgb(66, 66, 66);
        button.ButtonText = Name;
        button.DialogResult = res;
        button.TextAlign = ContentAlignment.MiddleCenter;
        button.Click += new EventHandler(button_Click);
        button.CornerRadius = 0;
        this.BTN_Panel.Controls.Add(button);
    }

    void button_Click(object sender, EventArgs e)
    {
        --MsgCounts;
        DialogResult = ((Red.Controls.Buttons.RedGlowButton)sender).DialogResult;

    }
    public MessageBoxEx(string title, string MessageText, Image iconSet, MessageBoxButtonsEx btns)
    {
        this.Font = SystemFonts.MessageBoxFont;
        this.ForeColor = SystemColors.WindowText;

        InitializeComponent();
        this.Text = title;
        Header.Text = title;
        HeaderMessage.Text = string.IsNullOrEmpty(MessageText) ? string.Empty : MessageText;
        pictureBox1.Image = iconSet;
        if (Globals.language == "German")
        {
            switch (btns)
            {
                case MessageBoxButtonsEx.AbortRetryIgnore:
                    AddButton("Ignorieren", 2, DialogResult.Ignore);
                    AddButton("Retry", 1, DialogResult.Retry);
                    AddButton("Abbrechen", 0, DialogResult.Abort);
                    break;
                case MessageBoxButtonsEx.OK:
                    AddButton(DialogResult.OK.ToString(), 0, DialogResult.OK);
                    break;
                case MessageBoxButtonsEx.OKCancel:
                    AddButton("Abbrechen", 1, DialogResult.Cancel);
                    AddButton(DialogResult.OK.ToString(), 0, DialogResult.OK);
                    break;
                case MessageBoxButtonsEx.RetryCancel:
                    AddButton("Abbrechen", 0, DialogResult.Cancel);
                    AddButton("Retry", 1, DialogResult.Retry);
                    break;
                case MessageBoxButtonsEx.YesNo:
                    AddButton("Keine", 0, DialogResult.No);
                    AddButton("Ja", 1, DialogResult.Yes);
                    break;
                case MessageBoxButtonsEx.YesNoCancel:
                    AddButton("Abbrechen", 0, DialogResult.Cancel);
                    AddButton("Keine", 1, DialogResult.No);
                    AddButton("Ja", 2, DialogResult.Yes);
                    break;
                case MessageBoxButtonsEx.PauseStopCancel:
                    AddButton("Abbrechen", 0, DialogResult.Cancel);
                    AddButton("Stopp", 1, DialogResult.No);
                    AddButton("Pause", 2, DialogResult.Yes);
                    break;
                case MessageBoxButtonsEx.LoadNewCancel:
                    AddButton("Abbrechen", 2, DialogResult.Cancel);
                    AddButton("Neu", 1, DialogResult.No);
                    AddButton("Last", 0, DialogResult.Yes);
                    break;
                case MessageBoxButtonsEx.Start:
                    AddButton("Starten", 2, DialogResult.OK);
                    break;
            }
        }
        else
        {
            switch (btns)
            {
                case MessageBoxButtonsEx.AbortRetryIgnore:
                    AddButton(DialogResult.Ignore.ToString(), 2, DialogResult.Ignore);
                    AddButton(DialogResult.Retry.ToString(), 1, DialogResult.Retry);
                    AddButton(DialogResult.Abort.ToString(), 0, DialogResult.Abort);
                    break;
                case MessageBoxButtonsEx.OK:
                    AddButton(DialogResult.OK.ToString(), 0, DialogResult.OK);
                    break;
                case MessageBoxButtonsEx.OKCancel:
                    AddButton(DialogResult.Cancel.ToString(), 1, DialogResult.Cancel);
                    AddButton(DialogResult.OK.ToString(), 0, DialogResult.OK);
                    break;
                case MessageBoxButtonsEx.RetryCancel:
                    AddButton(DialogResult.Cancel.ToString(), 0, DialogResult.Cancel);
                    AddButton(DialogResult.Retry.ToString(), 1, DialogResult.Retry);
                    break;
                case MessageBoxButtonsEx.YesNo:
                    AddButton(DialogResult.No.ToString(), 0, DialogResult.No);
                    AddButton(DialogResult.Yes.ToString(), 1, DialogResult.Yes);
                    break;
                case MessageBoxButtonsEx.YesNoCancel:
                    AddButton(DialogResult.Cancel.ToString(), 0, DialogResult.Cancel);
                    AddButton(DialogResult.No.ToString(), 1, DialogResult.No);
                    AddButton(DialogResult.Yes.ToString(), 2, DialogResult.Yes);
                    break;
                case MessageBoxButtonsEx.PauseStopCancel:
                    AddButton("Cancel", 0, DialogResult.Cancel);
                    AddButton("Stop", 1, DialogResult.No);
                    AddButton("Pause", 2, DialogResult.Yes);
                    break;
                case MessageBoxButtonsEx.LoadNewCancel:
                    AddButton("Cancel", 2, DialogResult.Cancel);
                    AddButton("New", 1, DialogResult.No);
                    AddButton("Load", 0, DialogResult.Yes);
                    break;
                case MessageBoxButtonsEx.Start:
                    AddButton("START", 2, DialogResult.OK);
                    break;
            }
        }
    }
    private MessageBoxEx(string title, string MessageText, Image iconSet, string btn1, string btn2, string btn3)
    {
        this.Font = SystemFonts.MessageBoxFont;
        this.ForeColor = SystemColors.WindowText;

        InitializeComponent();
        this.Text = title;
        Header.Text = title;
        HeaderMessage.Text = string.IsNullOrEmpty(MessageText) ? string.Empty : MessageText;
        pictureBox1.Image = iconSet;

        int tab = 0;
        if (false == string.IsNullOrEmpty(btn3))
        {
            AddButton(btn3, tab++, DialogResult.Cancel);
        }
        if (false == string.IsNullOrEmpty(btn2))
        {
            AddButton(btn2, tab++, DialogResult.No);
        }
        if (false == string.IsNullOrEmpty(btn1))
        {
            AddButton(btn1, tab++, DialogResult.Yes);
        }
    }

    /*
    static public DialogResult Show(string Text, string Caption)
    {
        if (CustomMsg == true)
        {
            using (MessageBoxEx dialog = new MessageBoxEx(Caption, Text, global::RUTILS.Properties.Resources.SmallLogo, MessageBoxButtonsEx.OK))
            {
                DialogResult result = dialog.ShowDialog();
                dialog.Font.Dispose();
                dialog.Dispose();
                return result;
            }
        }
        else
        {
            return MessageBox.Show(Text, Caption);
        }
    }
    static public DialogResult Show(string Text, string Caption, Image iconSet, MessageBoxButtonsEx btns)
    {
        if (CustomMsg == true)
        {
            using (MessageBoxEx dialog = new MessageBoxEx(Caption, Text, iconSet, btns))
            {
                DialogResult result = dialog.ShowDialog();
                dialog.Font.Dispose();
                dialog.Dispose();
                return result;
            }
        }
        else
        {
            MessageBoxButtons btn;
            switch (btns)
            {
                case MessageBoxButtonsEx.OK:
                    btn = MessageBoxButtons.OK;
                    break;
                case MessageBoxButtonsEx.OKCancel:
                    btn = MessageBoxButtons.OKCancel;
                    break;
                case MessageBoxButtonsEx.RetryCancel:
                    btn = MessageBoxButtons.RetryCancel;
                    break;
                case MessageBoxButtonsEx.YesNo:
                    btn = MessageBoxButtons.YesNo;
                    break;
                case MessageBoxButtonsEx.YesNoCancel:
                    btn = MessageBoxButtons.YesNoCancel;
                    break;
                case MessageBoxButtonsEx.AbortRetryIgnore:
                    btn = MessageBoxButtons.AbortRetryIgnore;
                    break;
                case MessageBoxButtonsEx.PauseStopCancel:
                    btn = MessageBoxButtons.AbortRetryIgnore;
                    break;
                case MessageBoxButtonsEx.Start:
                    btn = MessageBoxButtons.OK;
                    break;
                default:
                    btn = MessageBoxButtons.OKCancel;
                    break;
            }
            return MessageBox.Show(Text, Caption, btn);
        }
    }
     *     static public DialogResult Show(string Text, string Caption, Image iconSet, string btn1, string btn2, string btn3)
    {
        if (CustomMsg == true)
        {
            using (MessageBoxEx dialog = new MessageBoxEx(Text, Caption, iconSet, btn1, btn2, btn3))
            {
                DialogResult result = dialog.ShowDialog();
                dialog.Font.Dispose();
                dialog.Dispose();
                return result;
            }
        }
        else
        {
            return MessageBox.Show(Text, Caption, MessageBoxButtons.YesNoCancel);
        }
    }

    */
}
