using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
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
			RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\r3n3kutaro\\LPgui");
			string lpclientdir = key.GetValue("LPClientDir").ToString();
			this.fld_dia_client.SelectedPath = lpclientdir;
			this.fld_dia_client.ShowDialog();
		}

		private void btn_save_Click(object sender, EventArgs e)
		{
			RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\r3n3kutaro\\LPgui");
			key.SetValue("httptimeout", this.txt_httptimeout.Text);
			key.SetValue("serverlisturl", this.txt_serverlisturl.Text);
			key.SetValue("LPClientDir", this.fld_dia_client.SelectedPath);
			base.Close();
		}

		protected override void Dispose(bool disposing)
		{
			if ((!disposing ? false : this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void frm_einstellungen_Load(object sender, EventArgs e)
		{
			RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\r3n3kutaro\\LPgui");
			string serverlisturl = key.GetValue("serverlisturl").ToString();
			int reg_httptimeout = int.Parse(key.GetValue("httptimeout").ToString());
			this.txt_httptimeout.Text = reg_httptimeout.ToString();
			this.txt_serverlisturl.Text = serverlisturl;
		}

		private void InitializeComponent()
		{
			this.lbl_httptimeout = new Label();
			this.txt_httptimeout = new TextBox();
			this.txt_serverlisturl = new TextBox();
			this.lbl_serverurl = new Label();
			this.btn_save = new Button();
			this.btn_cancel = new Button();
			this.lbl_cldir = new Label();
			this.fld_dia_client = new FolderBrowserDialog();
			this.btn_choosedir = new Button();
			base.SuspendLayout();
			this.lbl_httptimeout.AutoSize = true;
			this.lbl_httptimeout.Location = new Point(12, 9);
			this.lbl_httptimeout.Name = "lbl_httptimeout";
			this.lbl_httptimeout.Size = new System.Drawing.Size(80, 13);
			this.lbl_httptimeout.TabIndex = 0;
			this.lbl_httptimeout.Text = "HTTP Timeout:";
			this.txt_httptimeout.Location = new Point(110, 4);
			this.txt_httptimeout.Name = "txt_httptimeout";
			this.txt_httptimeout.Size = new System.Drawing.Size(100, 20);
			this.txt_httptimeout.TabIndex = 1;
			this.txt_serverlisturl.Location = new Point(110, 30);
			this.txt_serverlisturl.Name = "txt_serverlisturl";
			this.txt_serverlisturl.Size = new System.Drawing.Size(100, 20);
			this.txt_serverlisturl.TabIndex = 2;
			this.lbl_serverurl.AutoSize = true;
			this.lbl_serverurl.Location = new Point(12, 34);
			this.lbl_serverurl.Name = "lbl_serverurl";
			this.lbl_serverurl.Size = new System.Drawing.Size(81, 13);
			this.lbl_serverurl.TabIndex = 3;
			this.lbl_serverurl.Text = "Serverliste URL";
			this.btn_save.Location = new Point(15, 89);
			this.btn_save.Name = "btn_save";
			this.btn_save.Size = new System.Drawing.Size(75, 23);
			this.btn_save.TabIndex = 4;
			this.btn_save.Text = "Speichern";
			this.btn_save.UseVisualStyleBackColor = true;
			this.btn_save.Click += new EventHandler(this.btn_save_Click);
			this.btn_cancel.Location = new Point(135, 89);
			this.btn_cancel.Name = "btn_cancel";
			this.btn_cancel.Size = new System.Drawing.Size(75, 23);
			this.btn_cancel.TabIndex = 5;
			this.btn_cancel.Text = "Abbrechen";
			this.btn_cancel.UseVisualStyleBackColor = true;
			this.btn_cancel.Click += new EventHandler(this.btn_cancel_Click);
			this.lbl_cldir.AutoSize = true;
			this.lbl_cldir.Location = new Point(15, 57);
			this.lbl_cldir.Name = "lbl_cldir";
			this.lbl_cldir.Size = new System.Drawing.Size(90, 13);
			this.lbl_cldir.TabIndex = 6;
			this.lbl_cldir.Text = "Client Speicherort";
			this.btn_choosedir.Location = new Point(111, 52);
			this.btn_choosedir.Name = "btn_choosedir";
			this.btn_choosedir.Size = new System.Drawing.Size(99, 23);
			this.btn_choosedir.TabIndex = 7;
			this.btn_choosedir.Text = "Ordner wählen";
			this.btn_choosedir.UseVisualStyleBackColor = true;
			this.btn_choosedir.Click += new EventHandler(this.btn_choosedir_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(218, 124);
			base.Controls.Add(this.btn_choosedir);
			base.Controls.Add(this.lbl_cldir);
			base.Controls.Add(this.btn_cancel);
			base.Controls.Add(this.btn_save);
			base.Controls.Add(this.lbl_serverurl);
			base.Controls.Add(this.txt_serverlisturl);
			base.Controls.Add(this.txt_httptimeout);
			base.Controls.Add(this.lbl_httptimeout);
			base.Name = "frm_einstellungen";
			this.Text = "Einstellungen";
			base.Load += new EventHandler(this.frm_einstellungen_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}