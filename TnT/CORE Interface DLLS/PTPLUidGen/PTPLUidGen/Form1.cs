using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace PTPLUidGen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private delegate void DelIncreaseCount(Int32 sender);
        bool Start = false;
        StreamWriter Writer;
        Int32 Cont = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            string txtTime1 = txtTime.Text;
            string txtTimeCurrent = DateTime.Now.ToString("dd/MM/yyyy hh:mm;ss:") + DateTime.Now.Millisecond.ToString();
            while (true)
            {
                Thread.Sleep(1);
                if (txtTime1.Equals(txtTimeCurrent))
                {
                    //Writer = new StreamWriter("D:\\UIDFile.txt", true);
                    DBConfig.SQL_connection();
                    Thread th = new Thread(new ThreadStart(StartProcess));
                    th.IsBackground = true;
                    th.Start();
                    break;
                }
                txtTime1 = txtTime.Text;
                txtTimeCurrent = DateTime.Now.ToString("dd/MM/yyyy hh:mm;ss:") + DateTime.Now.Millisecond.ToString();
            }

        }
        private void StartProcess()
        {
            try
            {
                string UID = string.Empty;
                Start = true;
                long Val=0;
                while (Start)
                {
                    //UID = UIDGen.GenerateUID(12);
                    //UID = UIDGenNew.GenerateUID(12);
                    //Val = UIDGen.GenerateUIDVal(12);
                    string Query = "Insert into tblUID(Code) values ('" + UID + "')";
                    DBConfig.ExecuteQuery(Query);
                    //Writer.WriteLine(UID);
                    Cont++;
                    UpdateCount(Cont);
                }
                //Writer.Close();
            }
            catch (Exception)
            {
                //Writer.Close();
                throw;
            }
        }
        private void UpdateCount(Int32 count)
        {
            try
            {
                if (lblCount.InvokeRequired)
                {
                    DelIncreaseCount d = new DelIncreaseCount(UpdateCount);
                    this.Invoke(d, new object[] { count });
                }
                else
                {
                    lblCount.Text = count.ToString();
                }
                    
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Start = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblTick.Text = "Tick Count :"+System.Environment.TickCount;
            lblTime.Text = "Time :" + DateTime.Now.ToString("dd/MM/yyyy hh:mm;ss:") + DateTime.Now.Millisecond.ToString();
            txtTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm;ss:") + DateTime.Now.Millisecond.ToString();
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            lblTick.Text = "Tick Count :" + System.Environment.TickCount;
            lblTime.Text = "Time :" + DateTime.Now.ToString("dd/MM/yyyy hh:mm;ss:") + DateTime.Now.Millisecond.ToString();
            txtTime.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm;ss:") + DateTime.Now.Millisecond.ToString();
        }
    }
}
