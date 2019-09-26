namespace WindowsFormsApplicationClientDemo
{
    partial class FrmClient
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtipstr = new System.Windows.Forms.TextBox();
            this.btnFrist = new System.Windows.Forms.Button();
            this.btnSec = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(50, 100);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(50, 196);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP:";
            // 
            // txtipstr
            // 
            this.txtipstr.Location = new System.Drawing.Point(85, 38);
            this.txtipstr.Name = "txtipstr";
            this.txtipstr.Size = new System.Drawing.Size(163, 25);
            this.txtipstr.TabIndex = 5;
            this.txtipstr.Text = "192.168.1.147";
            // 
            // btnFrist
            // 
            this.btnFrist.Location = new System.Drawing.Point(307, 57);
            this.btnFrist.Name = "btnFrist";
            this.btnFrist.Size = new System.Drawing.Size(110, 23);
            this.btnFrist.TabIndex = 6;
            this.btnFrist.Text = "1.首次握手";
            this.btnFrist.UseVisualStyleBackColor = true;
            this.btnFrist.Click += new System.EventHandler(this.btnFrist_Click);
            // 
            // btnSec
            // 
            this.btnSec.Location = new System.Drawing.Point(307, 100);
            this.btnSec.Name = "btnSec";
            this.btnSec.Size = new System.Drawing.Size(110, 23);
            this.btnSec.TabIndex = 7;
            this.btnSec.Text = "2.总召唤";
            this.btnSec.UseVisualStyleBackColor = true;
            this.btnSec.Click += new System.EventHandler(this.btnSec_Click);
            // 
            // FrmClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 253);
            this.Controls.Add(this.btnSec);
            this.Controls.Add(this.btnFrist);
            this.Controls.Add(this.txtipstr);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnConnect);
            this.Name = "FrmClient";
            this.Text = "FrmClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtipstr;
        private System.Windows.Forms.Button btnFrist;
        private System.Windows.Forms.Button btnSec;
    }
}