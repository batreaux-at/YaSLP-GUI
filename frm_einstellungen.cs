using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace LANPlayClient
{
	public class frm_einstellungen : Form
	{
		private IContainer components = null;

		private Label lbl_httptimeout;

		private TextBox txt_httptimeout;

		private TextBox txt_serverlisturl;

		private Label lbl_serverurl;

		private Button btn_save;

		private Button btn_cancel;

		private Label lbl_cldir;

		private FolderBrowserDialog fld_dia_client;
        private Label lbl_txt_clparam;
        private TextBox txt_clparam;
        private Label lbl_txt_parammode;
        private RadioButton rb_standard;
        private RadioButton rb_acnh;
        private RadioButton rb_manual;
        private Button btn_choosedir;

		public frm_einstellungen()
		{
			this.InitializeComponent();
		}

		private void btn_cancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void btn_choosedir_Click(object sender, EventArgs e)
		{
			this.fld_dia_client.ShowDialog();
		}

		private void btn_save_Click(object sender, EventArgs e)
		{
			RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\r3n3kutaro\\LPgui");
			key.SetValue("httptimeout", this.txt_httptimeout.Text);
			key.SetValue("serverlisturl", this.txt_serverlisturl.Text);
			key.SetValue("LPClientDir", this.fld_dia_client.SelectedPath);
            key.SetValue("Parameters", txt_clparam.Text);
            int parammode = 1;
            if (rb_standard.Checked) { parammode = 1; }
            if (rb_acnh.Checked) { parammode = 2; }
            if (rb_manual.Checked) { parammode = 3; }
            key.SetValue("Parametersmode", parammode);
            base.Close();
		}

		protected override void Dispose(bool disposing)
		{
			if ((disposing && this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		public void frm_einstellungen_Load(object sender, EventArgs e)
		{
			RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\r3n3kutaro\\LPgui");
			string serverlisturl = key.GetValue("serverlisturl").ToString();
			int reg_httptimeout = int.Parse(key.GetValue("httptimeout").ToString());
			this.txt_httptimeout.Text = reg_httptimeout.ToString();
			this.txt_serverlisturl.Text = serverlisturl;
			fld_dia_client.SelectedPath = key.GetValue("LPClientDir").ToString();
            txt_clparam.Text = key.GetValue("Parameters").ToString();
            int parammode = int.Parse(key.GetValue("Parametersmode").ToString());
            if (parammode == 0) { parammode = 1; }
            switch (parammode)
                {
                    case 1:
                    rb_standard.Checked = true;
                    rb_manual.Checked = false;
                    rb_acnh.Checked = false;
                    txt_clparam.Enabled = false;
                    break;

                    case 2:
                    rb_standard.Checked = false;
                    rb_manual.Checked = false;
                    rb_acnh.Checked = true;
                    txt_clparam.Enabled = false;
                    break;

                    case 3:
                    rb_standard.Checked = false;
                    rb_manual.Checked = true;
                    rb_acnh.Checked = false;
                    txt_clparam.Enabled = true;
                    break;
            }
		}

		private void InitializeComponent()
		{
            this.lbl_httptimeout = new System.Windows.Forms.Label();
            this.txt_httptimeout = new System.Windows.Forms.TextBox();
            this.txt_serverlisturl = new System.Windows.Forms.TextBox();
            this.lbl_serverurl = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.lbl_cldir = new System.Windows.Forms.Label();
            this.fld_dia_client = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_choosedir = new System.Windows.Forms.Button();
            this.lbl_txt_clparam = new System.Windows.Forms.Label();
            this.txt_clparam = new System.Windows.Forms.TextBox();
            this.lbl_txt_parammode = new System.Windows.Forms.Label();
            this.rb_standard = new System.Windows.Forms.RadioButton();
            this.rb_acnh = new System.Windows.Forms.RadioButton();
            this.rb_manual = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // lbl_httptimeout
            // 
            this.lbl_httptimeout.AutoSize = true;
            this.lbl_httptimeout.Location = new System.Drawing.Point(12, 9);
            this.lbl_httptimeout.Name = "lbl_httptimeout";
            this.lbl_httptimeout.Size = new System.Drawing.Size(80, 13);
            this.lbl_httptimeout.TabIndex = 0;
            this.lbl_httptimeout.Text = "HTTP Timeout:";
            // 
            // txt_httptimeout
            // 
            this.txt_httptimeout.Location = new System.Drawing.Point(110, 4);
            this.txt_httptimeout.Name = "txt_httptimeout";
            this.txt_httptimeout.Size = new System.Drawing.Size(72, 20);
            this.txt_httptimeout.TabIndex = 1;
            // 
            // txt_serverlisturl
            // 
            this.txt_serverlisturl.Location = new System.Drawing.Point(110, 28);
            this.txt_serverlisturl.Name = "txt_serverlisturl";
            this.txt_serverlisturl.Size = new System.Drawing.Size(215, 20);
            this.txt_serverlisturl.TabIndex = 2;
            // 
            // lbl_serverurl
            // 
            this.lbl_serverurl.AutoSize = true;
            this.lbl_serverurl.Location = new System.Drawing.Point(12, 34);
            this.lbl_serverurl.Name = "lbl_serverurl";
            this.lbl_serverurl.Size = new System.Drawing.Size(81, 13);
            this.lbl_serverurl.TabIndex = 3;
            this.lbl_serverurl.Text = "Serverliste URL";
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(49, 144);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 4;
            this.btn_save.Text = "Speichern";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(218, 144);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 5;
            this.btn_cancel.Text = "Abbrechen";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // lbl_cldir
            // 
            this.lbl_cldir.AutoSize = true;
            this.lbl_cldir.Location = new System.Drawing.Point(12, 58);
            this.lbl_cldir.Name = "lbl_cldir";
            this.lbl_cldir.Size = new System.Drawing.Size(90, 13);
            this.lbl_cldir.TabIndex = 6;
            this.lbl_cldir.Text = "Client Speicherort";
            // 
            // btn_choosedir
            // 
            this.btn_choosedir.Location = new System.Drawing.Point(109, 52);
            this.btn_choosedir.Name = "btn_choosedir";
            this.btn_choosedir.Size = new System.Drawing.Size(99, 23);
            this.btn_choosedir.TabIndex = 7;
            this.btn_choosedir.Text = "Ordner wählen";
            this.btn_choosedir.UseVisualStyleBackColor = true;
            this.btn_choosedir.Click += new System.EventHandler(this.btn_choosedir_Click);
            // 
            // lbl_txt_clparam
            // 
            this.lbl_txt_clparam.AutoSize = true;
            this.lbl_txt_clparam.Location = new System.Drawing.Point(12, 112);
            this.lbl_txt_clparam.Name = "lbl_txt_clparam";
            this.lbl_txt_clparam.Size = new System.Drawing.Size(84, 13);
            this.lbl_txt_clparam.TabIndex = 8;
            this.lbl_txt_clparam.Text = "Client Parameter";
            // 
            // txt_clparam
            // 
            this.txt_clparam.Location = new System.Drawing.Point(110, 106);
            this.txt_clparam.Name = "txt_clparam";
            this.txt_clparam.Size = new System.Drawing.Size(215, 20);
            this.txt_clparam.TabIndex = 9;
            // 
            // lbl_txt_parammode
            // 
            this.lbl_txt_parammode.AutoSize = true;
            this.lbl_txt_parammode.Location = new System.Drawing.Point(12, 84);
            this.lbl_txt_parammode.Name = "lbl_txt_parammode";
            this.lbl_txt_parammode.Size = new System.Drawing.Size(119, 13);
            this.lbl_txt_parammode.TabIndex = 10;
            this.lbl_txt_parammode.Text = "Client Parameter Modus";
            // 
            // rb_standard
            // 
            this.rb_standard.AutoSize = true;
            this.rb_standard.Location = new System.Drawing.Point(138, 83);
            this.rb_standard.Name = "rb_standard";
            this.rb_standard.Size = new System.Drawing.Size(68, 17);
            this.rb_standard.TabIndex = 11;
            this.rb_standard.TabStop = true;
            this.rb_standard.Text = "Standard";
            this.rb_standard.UseVisualStyleBackColor = true;
            this.rb_standard.CheckedChanged += new System.EventHandler(this.rb_standard_CheckedChanged);
            // 
            // rb_acnh
            // 
            this.rb_acnh.AutoSize = true;
            this.rb_acnh.Location = new System.Drawing.Point(208, 83);
            this.rb_acnh.Name = "rb_acnh";
            this.rb_acnh.Size = new System.Drawing.Size(55, 17);
            this.rb_acnh.TabIndex = 12;
            this.rb_acnh.TabStop = true;
            this.rb_acnh.Text = "ACNH";
            this.rb_acnh.UseVisualStyleBackColor = true;
            this.rb_acnh.CheckedChanged += new System.EventHandler(this.rb_acnh_CheckedChanged);
            // 
            // rb_manual
            // 
            this.rb_manual.AutoSize = true;
            this.rb_manual.Location = new System.Drawing.Point(270, 83);
            this.rb_manual.Name = "rb_manual";
            this.rb_manual.Size = new System.Drawing.Size(61, 17);
            this.rb_manual.TabIndex = 13;
            this.rb_manual.TabStop = true;
            this.rb_manual.Text = "manuell";
            this.rb_manual.UseVisualStyleBackColor = true;
            this.rb_manual.CheckedChanged += new System.EventHandler(this.rb_manual_CheckedChanged);
            // 
            // frm_einstellungen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 190);
            this.Controls.Add(this.rb_manual);
            this.Controls.Add(this.rb_acnh);
            this.Controls.Add(this.rb_standard);
            this.Controls.Add(this.lbl_txt_parammode);
            this.Controls.Add(this.txt_clparam);
            this.Controls.Add(this.lbl_txt_clparam);
            this.Controls.Add(this.btn_choosedir);
            this.Controls.Add(this.lbl_cldir);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.lbl_serverurl);
            this.Controls.Add(this.txt_serverlisturl);
            this.Controls.Add(this.txt_httptimeout);
            this.Controls.Add(this.lbl_httptimeout);
            this.Name = "frm_einstellungen";
            this.Text = "Einstellungen";
            this.Load += new System.EventHandler(this.frm_einstellungen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private void rb_standard_CheckedChanged(object sender, EventArgs e)
        {
            int parammode = 1;
            if (rb_standard.Checked) { parammode = 1; }
            if (rb_acnh.Checked) { parammode = 2; }
            if (rb_manual.Checked) { parammode = 3; }
            switch (parammode)
            {
                case 1:
                    rb_standard.Checked = true;
                    rb_manual.Checked = false;
                    rb_acnh.Checked = false;
                    txt_clparam.Enabled = false;
                    txt_clparam.Text = " --relay-server-addr ";
                    break;

                case 2:
                    rb_standard.Checked = false;
                    rb_manual.Checked = false;
                    rb_acnh.Checked = true;
                    txt_clparam.Enabled = false;
                    txt_clparam.Text = " --pmtu 500 --relay-server-addr ";
                    break;

                case 3:
                    rb_standard.Checked = false;
                    rb_manual.Checked = true;
                    rb_acnh.Checked = false;
                    txt_clparam.Enabled = true;
                    break;
            }
        }

        private void rb_acnh_CheckedChanged(object sender, EventArgs e)
        {
            int parammode = 1;
            if (rb_standard.Checked) { parammode = 1; }
            if (rb_acnh.Checked) { parammode = 2; }
            if (rb_manual.Checked) { parammode = 3; }
            switch (parammode)
            {
                case 1:
                    rb_standard.Checked = true;
                    rb_manual.Checked = false;
                    rb_acnh.Checked = false;
                    txt_clparam.Enabled = false;
                    txt_clparam.Text = " --relay-server-addr ";
                    break;

                case 2:
                    rb_standard.Checked = false;
                    rb_manual.Checked = false;
                    rb_acnh.Checked = true;
                    txt_clparam.Enabled = false;
                    txt_clparam.Text = " --pmtu 500 --relay-server-addr ";
                    break;

                case 3:
                    rb_standard.Checked = false;
                    rb_manual.Checked = true;
                    rb_acnh.Checked = false;
                    txt_clparam.Enabled = true;
                    break;
            }
        }

        private void rb_manual_CheckedChanged(object sender, EventArgs e)
        {
            int parammode = 1;
            if (rb_standard.Checked) { parammode = 1; }
            if (rb_acnh.Checked) { parammode = 2; }
            if (rb_manual.Checked) { parammode = 3; }
            switch (parammode)
            {
                case 1:
                    rb_standard.Checked = true;
                    rb_manual.Checked = false;
                    rb_acnh.Checked = false;
                    txt_clparam.Enabled = false;
                    txt_clparam.Text = " --relay-server-addr ";
                    break;

                case 2:
                    rb_standard.Checked = false;
                    rb_manual.Checked = false;
                    rb_acnh.Checked = true;
                    txt_clparam.Enabled = false;
                    txt_clparam.Text = " --pmtu 500 --relay-server-addr ";
                    break;

                case 3:
                    rb_standard.Checked = false;
                    rb_manual.Checked = true;
                    rb_acnh.Checked = false;
                    txt_clparam.Enabled = true;
                    break;
            }
        }
    }
}