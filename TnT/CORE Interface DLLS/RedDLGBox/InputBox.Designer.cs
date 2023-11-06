namespace Red.Utils
{
    partial class InputBox
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
            this.txtData = new System.Windows.Forms.TextBox();
            this.BTNCancel = new Red.Controls.Buttons.RoundButton();
            this.BTNOk = new Red.Controls.Buttons.RoundButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtData
            // 
            this.txtData.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtData.Location = new System.Drawing.Point(14, 45);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(316, 27);
            this.txtData.TabIndex = 0;
            this.txtData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtData_MouseDown);
            this.txtData.Enter += new System.EventHandler(this.txtData_Enter);
            // 
            // BTNCancel
            // 
            this.BTNCancel.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.BTNCancel.ButtonShape = Red.Controls.Buttons.RoundButton.E_ButtonShape.Rectangle;
            this.BTNCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BTNCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTNCancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BTNCancel.Location = new System.Drawing.Point(228, 92);
            this.BTNCancel.Name = "BTNCancel";
            this.BTNCancel.RecessDepth = 0;
            this.BTNCancel.Size = new System.Drawing.Size(101, 33);
            this.BTNCancel.TabIndex = 1;
            this.BTNCancel.Text = "Cancel";
            this.BTNCancel.UseVisualStyleBackColor = false;
            this.BTNCancel.Click += new System.EventHandler(this.BTNCancel_Click);
            // 
            // BTNOk
            // 
            this.BTNOk.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.BTNOk.ButtonShape = Red.Controls.Buttons.RoundButton.E_ButtonShape.Rectangle;
            this.BTNOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTNOk.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BTNOk.Location = new System.Drawing.Point(117, 92);
            this.BTNOk.Name = "BTNOk";
            this.BTNOk.RecessDepth = 0;
            this.BTNOk.Size = new System.Drawing.Size(101, 33);
            this.BTNOk.TabIndex = 2;
            this.BTNOk.Text = "OK";
            this.BTNOk.UseVisualStyleBackColor = false;
            this.BTNOk.Click += new System.EventHandler(this.BTNOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // InputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.CancelButton = this.BTNCancel;
            this.ClientSize = new System.Drawing.Size(341, 137);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BTNOk);
            this.Controls.Add(this.BTNCancel);
            this.Controls.Add(this.txtData);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InputForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtData;
        private Red.Controls.Buttons.RoundButton BTNCancel;
        private Red.Controls.Buttons.RoundButton BTNOk;
        private System.Windows.Forms.Label label1;
    }
}