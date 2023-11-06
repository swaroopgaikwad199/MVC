using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

public partial class MessageBoxAsync : Form
{
    public delegate void MessageHandler(DialogResult res,object param);
    private delegate void UpdateMessage(msg data);
    msg m_Current = null;
    public static bool CustomMsg = true;
    public MessageHandler OnMessageClose;
    class msg
    {
        public string Caption;
        public string Text;
        public object Param;
        public msg(string oText,string oCaption,object oParam)
        {
            Caption = oCaption;
            Text = oText;
            Param = oParam;
        }
    };
    private Queue<msg>MessageQueue = new Queue<msg>();
    private void UpdateMsgDetails(msg data)
    {
        if (HeaderMessage.InvokeRequired == true)
        {
            UpdateMessage d = new UpdateMessage(UpdateMsgDetails);
            this.Invoke(d, new object[] { data});
        }
        else
        {
            this.Text = Header.Text = data.Caption;
            HeaderMessage.Text = data.Text;
        }
    }
    private void MessageBoxAsync_Click(object Sender, EventArgs e)
    {
       
        if (OnMessageClose != null)
            OnMessageClose(DialogResult, m_Current.Param);
        if (MessageQueue.Count > 0)
        {
            m_Current = MessageQueue.Dequeue();
            UpdateMsgDetails(m_Current);
        }
        else
            this.Hide();

    }
    public void AddMessage(string Text, string Caption,object param)
    {
        MessageQueue.Enqueue(new msg(Text,Caption,param));
    }
    private void AddButton(string Name, int TabIndex)
    {
        Button button = new Button();
        button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
        button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        button.ForeColor = System.Drawing.Color.White;
        button.Name = Name;
        button.Size = new System.Drawing.Size(100, 40);
        button.TabIndex = 1;
        button.Text = Name;
        this.BTN_Panel.Controls.Add(button);
        button.Click += new EventHandler(MessageBoxAsync_Click);
    }
    public MessageBoxAsync()
    {
        this.Font = SystemFonts.MessageBoxFont;
        this.ForeColor = SystemColors.WindowText;
        InitializeComponent();
        //this.SuspendLayout();
        //AddButton("BTN_OK", 0);
        //this.ResumeLayout(false);
        //this.PerformLayout();
    }

    private void MessageBoxAsync_Load(object sender, EventArgs e)
    {
        if (MessageQueue.Count == 1)
        {
            m_Current = MessageQueue.Dequeue();
            UpdateMsgDetails(m_Current);
        }
    }
}

