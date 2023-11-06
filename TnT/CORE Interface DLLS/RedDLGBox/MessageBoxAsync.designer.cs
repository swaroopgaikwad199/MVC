
partial class MessageBoxAsync
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Header = new System.Windows.Forms.Label();
            this.TBLLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.HeaderMessage = new System.Windows.Forms.Label();
            this.BTN_Panel = new System.Windows.Forms.FlowLayoutPanel();
            this.TabelPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BTN_OK = new System.Windows.Forms.Button();
            this.TBLLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.BTN_Panel.SuspendLayout();
            this.TabelPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Header
            // 
            this.Header.BackColor = System.Drawing.Color.Indigo;
            this.Header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Header.ForeColor = System.Drawing.Color.White;
            this.Header.Location = new System.Drawing.Point(3, 3);
            this.Header.Margin = new System.Windows.Forms.Padding(3);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(508, 34);
            this.Header.TabIndex = 0;
            this.Header.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TBLLayoutPanel
            // 
            this.TBLLayoutPanel.AutoSize = true;
            this.TBLLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.TBLLayoutPanel.ColumnCount = 2;
            this.TBLLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.TBLLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TBLLayoutPanel.Controls.Add(this.pictureBox1, 0, 0);
            this.TBLLayoutPanel.Controls.Add(this.HeaderMessage, 1, 0);
            this.TBLLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBLLayoutPanel.Location = new System.Drawing.Point(3, 43);
            this.TBLLayoutPanel.Name = "TBLLayoutPanel";
            this.TBLLayoutPanel.RowCount = 1;
            this.TBLLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TBLLayoutPanel.Size = new System.Drawing.Size(508, 133);
            this.TBLLayoutPanel.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::PTPLDLGBox.Properties.Resources.SmallLogo;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(66, 58);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // HeaderMessage
            // 
            this.HeaderMessage.AutoSize = true;
            this.HeaderMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HeaderMessage.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeaderMessage.Location = new System.Drawing.Point(82, 10);
            this.HeaderMessage.Margin = new System.Windows.Forms.Padding(10);
            this.HeaderMessage.Name = "HeaderMessage";
            this.HeaderMessage.Size = new System.Drawing.Size(416, 113);
            this.HeaderMessage.TabIndex = 2;
            this.HeaderMessage.Text = "Dialog message.";
            // 
            // BTN_Panel
            // 
            this.BTN_Panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BTN_Panel.Controls.Add(this.BTN_OK);
            this.BTN_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BTN_Panel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.BTN_Panel.Location = new System.Drawing.Point(3, 182);
            this.BTN_Panel.Name = "BTN_Panel";
            this.BTN_Panel.Size = new System.Drawing.Size(508, 44);
            this.BTN_Panel.TabIndex = 3;
            // 
            // TabelPanel
            // 
            this.TabelPanel.AutoSize = true;
            this.TabelPanel.BackColor = System.Drawing.Color.Gold;
            this.TabelPanel.ColumnCount = 1;
            this.TabelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TabelPanel.Controls.Add(this.TBLLayoutPanel, 0, 1);
            this.TabelPanel.Controls.Add(this.Header, 0, 0);
            this.TabelPanel.Controls.Add(this.BTN_Panel, 0, 2);
            this.TabelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabelPanel.Location = new System.Drawing.Point(0, 0);
            this.TabelPanel.Name = "TabelPanel";
            this.TabelPanel.RowCount = 3;
            this.TabelPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.TabelPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TabelPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TabelPanel.Size = new System.Drawing.Size(514, 229);
            this.TabelPanel.TabIndex = 5;
            // 
            // BTN_OK
            // 
            this.BTN_OK.Location = new System.Drawing.Point(430, 3);
            this.BTN_OK.Name = "BTN_OK";
            this.BTN_OK.Size = new System.Drawing.Size(75, 41);
            this.BTN_OK.TabIndex = 0;
            this.BTN_OK.Text = "OK";
            this.BTN_OK.UseVisualStyleBackColor = true;
            this.BTN_OK.Click += new System.EventHandler(this.MessageBoxAsync_Click);
            // 
            // MessageBoxAsync
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(514, 229);
            this.Controls.Add(this.TabelPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageBoxAsync";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Special Dialog";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MessageBoxAsync_Load);
            this.TBLLayoutPanel.ResumeLayout(false);
            this.TBLLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.BTN_Panel.ResumeLayout(false);
            this.TabelPanel.ResumeLayout(false);
            this.TabelPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Header; 
        private System.Windows.Forms.TableLayoutPanel TBLLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel BTN_Panel;
        private System.Windows.Forms.Label HeaderMessage;
        private System.Windows.Forms.TableLayoutPanel TabelPanel;
        private System.Windows.Forms.Button BTN_OK;

}