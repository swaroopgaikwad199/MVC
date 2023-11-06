namespace PTPLKB
{
    partial class KeyBoard
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
            this.keyboardcontrol1 = new PTPLKB.KBCtrl();
            this.BTN_Ok = new System.Windows.Forms.Button();
            this.TXTKbData = new System.Windows.Forms.TextBox();
            this.BTN_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // keyboardcontrol1
            // 
            this.keyboardcontrol1.KeyboardType = PTPLKB.BoW.Short;
            this.keyboardcontrol1.Location = new System.Drawing.Point(4, 46);
            this.keyboardcontrol1.Name = "keyboardcontrol1";
            this.keyboardcontrol1.Size = new System.Drawing.Size(670, 221);
            this.keyboardcontrol1.TabIndex = 0;
            this.keyboardcontrol1.UserKeyPressed += new PTPLKB.KeyboardDelegate(this.keyboardcontrol1_UserKeyPressed);
            // 
            // BTN_Ok
            // 
            this.BTN_Ok.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BTN_Ok.BackColor = System.Drawing.Color.Silver;
            this.BTN_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BTN_Ok.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Ok.Location = new System.Drawing.Point(514, 224);
            this.BTN_Ok.Name = "BTN_Ok";
            this.BTN_Ok.Size = new System.Drawing.Size(154, 40);
            this.BTN_Ok.TabIndex = 1;
            this.BTN_Ok.Text = "OK";
            this.BTN_Ok.UseVisualStyleBackColor = false;
            this.BTN_Ok.Click += new System.EventHandler(this.BTN_Ok_Click);
            // 
            // TXTKbData
            // 
            this.TXTKbData.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTKbData.Location = new System.Drawing.Point(3, 3);
            this.TXTKbData.Name = "TXTKbData";
            this.TXTKbData.Size = new System.Drawing.Size(671, 38);
            this.TXTKbData.TabIndex = 0;
            this.TXTKbData.TextChanged += new System.EventHandler(this.TXTKbData_TextChanged);
            this.TXTKbData.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TXTKbData_KeyUp);
            this.TXTKbData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXTKbData_KeyPress);
            // 
            // BTN_Cancel
            // 
            this.BTN_Cancel.BackColor = System.Drawing.Color.Silver;
            this.BTN_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BTN_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Cancel.Location = new System.Drawing.Point(7, 224);
            this.BTN_Cancel.Name = "BTN_Cancel";
            this.BTN_Cancel.Size = new System.Drawing.Size(154, 40);
            this.BTN_Cancel.TabIndex = 2;
            this.BTN_Cancel.Text = "CANCEL";
            this.BTN_Cancel.UseVisualStyleBackColor = false;
            this.BTN_Cancel.Click += new System.EventHandler(this.BTN_Cancel_Click);
            // 
            // KeyBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gold;
            this.CancelButton = this.BTN_Ok;
            this.ClientSize = new System.Drawing.Size(677, 271);
            this.Controls.Add(this.BTN_Cancel);
            this.Controls.Add(this.BTN_Ok);
            this.Controls.Add(this.TXTKbData);
            this.Controls.Add(this.keyboardcontrol1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(175, 480);
            this.Name = "KeyBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Touchscreen Keyboard Demo Program";
            this.Load += new System.EventHandler(this.KeyBoard_Load);
            this.Shown += new System.EventHandler(this.KeyBoard_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KBCtrl keyboardcontrol1;
        private System.Windows.Forms.Button BTN_Ok;
        private System.Windows.Forms.TextBox TXTKbData;
        private System.Windows.Forms.Button BTN_Cancel;
    }
}

