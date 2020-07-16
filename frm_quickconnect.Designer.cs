namespace LANPlayClient
{
    partial class frm_quickconnect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_quickconnect));
            this.txt_srvaddr = new System.Windows.Forms.TextBox();
            this.lbl_txt_srvaddr = new System.Windows.Forms.Label();
            this.btn_connect = new System.Windows.Forms.Button();
            this.btn_back = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_srvaddr
            // 
            this.txt_srvaddr.Location = new System.Drawing.Point(13, 31);
            this.txt_srvaddr.Name = "txt_srvaddr";
            this.txt_srvaddr.Size = new System.Drawing.Size(216, 20);
            this.txt_srvaddr.TabIndex = 0;
            // 
            // lbl_txt_srvaddr
            // 
            this.lbl_txt_srvaddr.AutoSize = true;
            this.lbl_txt_srvaddr.Location = new System.Drawing.Point(13, 12);
            this.lbl_txt_srvaddr.Name = "lbl_txt_srvaddr";
            this.lbl_txt_srvaddr.Size = new System.Drawing.Size(82, 13);
            this.lbl_txt_srvaddr.TabIndex = 1;
            this.lbl_txt_srvaddr.Text = "Server Address:";
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(13, 69);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(75, 23);
            this.btn_connect.TabIndex = 2;
            this.btn_connect.Text = "Connect";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // btn_back
            // 
            this.btn_back.Location = new System.Drawing.Point(154, 69);
            this.btn_back.Name = "btn_back";
            this.btn_back.Size = new System.Drawing.Size(75, 23);
            this.btn_back.TabIndex = 3;
            this.btn_back.Text = "Back";
            this.btn_back.UseVisualStyleBackColor = true;
            this.btn_back.Click += new System.EventHandler(this.btn_back_Click);
            // 
            // frm_quickconnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 100);
            this.Controls.Add(this.btn_back);
            this.Controls.Add(this.btn_connect);
            this.Controls.Add(this.lbl_txt_srvaddr);
            this.Controls.Add(this.txt_srvaddr);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_quickconnect";
            this.Text = "Quick Connect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_srvaddr;
        private System.Windows.Forms.Label lbl_txt_srvaddr;
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Button btn_back;
    }
}