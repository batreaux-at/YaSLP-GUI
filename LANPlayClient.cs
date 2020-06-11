using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace LANPlayClient
{
	public class LANPlayClient : Form
	{
		private IContainer components = null;

		private Label lbl_firststep;

		private ComboBox drp_srvlist;

		private Button btn_loadsrvlist;

		private Label lbl_secondstep;

		private Label lbl_thirdstep;

		private Button btn_connectserver;

		private GroupBox grp_srvstatus;

		private Label lbl_usract;

		private Label lbl_usridl;

		private Label lbl_usronl;

		private Label lbl_srvtyp;

		private Label lbl_srvstatus;

		private Label label4;

		private Label lbl_txt_usridle;

		private Label lbl_txt_usronl;

		private Label lbl_txt_srvtyp;

		private Label lbl_txt_srvstatus;

		private Label lbl_srvver;

		private Label lbl_txt_srvver;

		private Label lbl_ping;

		private Label lbl_txt_ping;

		private Label lbl_httptimeout;

		private MenuStrip menuStrip1;

		private ToolStripMenuItem mnu_Datei;

		private ToolStripMenuItem mnu_einstellungen;

		private ToolStripMenuItem mnu_beenden;

		private Button btn_winpcapdl;

		public LANPlayClient()
		{
			this.InitializeComponent();
		}

		private void btn_connectserver_Click(object sender, EventArgs e)
		{
			RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\r3n3kutaro\\LPgui");
			string LPClientDir = key.GetValue("LPClientDir").ToString();
			string exepath = string.Concat(LPClientDir, "\\lan-play-win64.exe");
			string usesrv = this.drp_srvlist.Text;
			int srvaddrlength = usesrv.Length - 5 - 7;
			string serveraddress = usesrv.Substring(7, srvaddrlength);
			string parameters = string.Concat(" --relay-server-addr ", serveraddress);
			Process.Start(exepath, parameters);
		}

		private void btn_loadsrvlist_Click(object sender, EventArgs e)
		{
			this.drp_srvlist.Items.Clear();
			string ipaddress = "";
			string port = "";
			RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\r3n3kutaro\\LPgui");
			string serverlisturl = key.GetValue("serverlisturl").ToString();
			byte[] raw1 = (new WebClient()).DownloadData(serverlisturl);
			string SrvListFull = Encoding.UTF8.GetString(raw1);
			string[] Servers = SrvListFull.Split(new char[] { '{' });
			int ctr01 = 0;
			string[] strArrays = Servers;
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string ch = strArrays[i];
				string[] ServerValues = Servers[ctr01].Split(new char[] { ':' });
				ctr01++;
				int ctr02 = 0;
				string[] strArrays1 = ServerValues;
				for (int j = 0; j < (int)strArrays1.Length; j++)
				{
					string st = strArrays1[j];
					string srvval = ServerValues[ctr02].Replace(" ", "");
					srvval = srvval.Replace("\"", string.Empty);
					if (srvval.Contains("ip"))
					{
						ipaddress = ServerValues[ctr02 + 1].Substring(1, ServerValues[ctr02 + 1].IndexOf(',') - 2).Replace("\"", string.Empty);
					}
					if (srvval.Contains("port"))
					{
						port = ServerValues[ctr02 + 1].Substring(1, ServerValues[ctr02 + 1].IndexOf(',')).Replace("\"", string.Empty).Replace(",", "");
						string url = string.Concat(new string[] { "http://", ipaddress, ":", port, "/info" });
						if (this.ping(url))
						{
							this.drp_srvlist.Items.Add(url);
							this.drp_srvlist.SelectedIndex = 0;
						}
					}
					ctr02++;
				}
			}
		}

		private void btn_winpcapdl_Click(object sender, EventArgs e)
		{
			Process.Start("https://www.winpcap.org/install/");
		}

		protected override void Dispose(bool disposing)
		{
			if ((!disposing ? false : this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void drp_srvlist_SelectedIndexChanged(object sender, EventArgs e)
		{
			WebClient wc = new WebClient();
			string url = this.drp_srvlist.SelectedItem.ToString();
			Stopwatch timer = new Stopwatch();
			timer.Start();
			byte[] raw = wc.DownloadData(url);
			timer.Stop();
			string webData = Encoding.UTF8.GetString(raw);
			string[] DataArray = webData.Split(new char[] { ':' });
			Label lblPing = this.lbl_ping;
			long elapsedMilliseconds = timer.ElapsedMilliseconds;
			lblPing.Text = string.Concat(elapsedMilliseconds.ToString(), " ms");
			timer.Reset();
			if (!webData.Contains("rust"))
			{
				string srvver = DataArray[2].Substring(1, DataArray[2].IndexOf('\"', 1) - 1);
				string useronline = DataArray[1].Substring(0, DataArray[1].IndexOf(","));
				this.lbl_srvstatus.Text = "online";
				this.lbl_srvver.Text = srvver;
				this.lbl_usract.Text = "N/A";
				this.lbl_usridl.Text = "N/A";
				this.lbl_usronl.Text = useronline;
				this.lbl_srvtyp.Text = "N/A";
			}
			else
			{
				this.lbl_srvtyp.Text = "Rust";
				string srvver = DataArray[3].Substring(1, DataArray[3].IndexOf('\"', 1) - 1);
				this.lbl_srvstatus.Text = "online";
				int useronline = int.Parse(DataArray[1].Substring(0, DataArray[1].IndexOf(",")));
				int useridle = int.Parse(DataArray[2].Substring(0, DataArray[2].IndexOf(",")));
				this.lbl_usronl.Text = useronline.ToString();
				this.lbl_usridl.Text = useridle.ToString();
				this.lbl_usract.Text = (useronline - useridle).ToString();
				this.lbl_srvver.Text = srvver;
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			if (Registry.CurrentUser.OpenSubKey("SOFTWARE\\r3n3kutaro\\LPgui") == null)
			{
				RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\r3n3kutaro\\LPgui");
				key.SetValue("httptimeout", "300");
				key.SetValue("serverlisturl", "https://raw.githubusercontent.com/GreatWizard/lan-play-status/master/public/data/servers.json");
				key.SetValue("LPClientDir", "C:\\Kutaro-R3n3-LanplayGUI");
			}
			RegistryKey regkey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\r3n3kutaro\\LPgui");
			string LPClientDir = regkey.GetValue("LPClientDir").ToString();
			if (!Directory.Exists(LPClientDir))
			{
				Directory.CreateDirectory(LPClientDir);
			}
			if (!File.Exists(string.Concat(LPClientDir, "\\lan-play-win64.exe")))
			{
				string downloadurl = "https://github.com/spacemeowx2/switch-lan-play/releases/download/v0.2.1/lan-play-win64.exe";
				WebClient downloadclient = new WebClient();
				frm_update displayupdate = new frm_update();
				displayupdate.Show();
				downloadclient.DownloadFile(downloadurl, string.Concat(LPClientDir, "\\lan-play-win64.exe"));
				displayupdate.Close();
			}
			if (Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\WinPcap") == null)
			{
				MessageBox.Show("WinPCAP fehlt! Lanplay kann nicht gestartet werden!", "WinPCAP fehlt", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.btn_connectserver.Enabled = false;
				this.btn_winpcapdl.Visible = true;
			}
		}

		private void InitializeComponent()
		{
			this.lbl_firststep = new Label();
			this.drp_srvlist = new ComboBox();
			this.btn_loadsrvlist = new Button();
			this.lbl_secondstep = new Label();
			this.lbl_thirdstep = new Label();
			this.btn_connectserver = new Button();
			this.grp_srvstatus = new GroupBox();
			this.lbl_ping = new Label();
			this.lbl_txt_ping = new Label();
			this.lbl_srvver = new Label();
			this.lbl_txt_srvver = new Label();
			this.lbl_usract = new Label();
			this.lbl_usridl = new Label();
			this.lbl_usronl = new Label();
			this.lbl_srvtyp = new Label();
			this.lbl_srvstatus = new Label();
			this.label4 = new Label();
			this.lbl_txt_usridle = new Label();
			this.lbl_txt_usronl = new Label();
			this.lbl_txt_srvtyp = new Label();
			this.lbl_txt_srvstatus = new Label();
			this.lbl_httptimeout = new Label();
			this.menuStrip1 = new MenuStrip();
			this.mnu_Datei = new ToolStripMenuItem();
			this.mnu_einstellungen = new ToolStripMenuItem();
			this.mnu_beenden = new ToolStripMenuItem();
			this.btn_winpcapdl = new Button();
			this.grp_srvstatus.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			base.SuspendLayout();
			this.lbl_firststep.AutoSize = true;
			this.lbl_firststep.Font = new System.Drawing.Font("Calibri", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lbl_firststep.Location = new Point(23, 46);
			this.lbl_firststep.Name = "lbl_firststep";
			this.lbl_firststep.Size = new System.Drawing.Size(139, 19);
			this.lbl_firststep.TabIndex = 1;
			this.lbl_firststep.Text = "1. Serverliste laden";
			this.drp_srvlist.FormattingEnabled = true;
			this.drp_srvlist.Location = new Point(188, 103);
			this.drp_srvlist.Name = "drp_srvlist";
			this.drp_srvlist.Size = new System.Drawing.Size(245, 21);
			this.drp_srvlist.TabIndex = 2;
			this.drp_srvlist.SelectedIndexChanged += new EventHandler(this.drp_srvlist_SelectedIndexChanged);
			this.btn_loadsrvlist.Location = new Point(260, 42);
			this.btn_loadsrvlist.Name = "btn_loadsrvlist";
			this.btn_loadsrvlist.Size = new System.Drawing.Size(103, 23);
			this.btn_loadsrvlist.TabIndex = 3;
			this.btn_loadsrvlist.Text = "Serverliste laden";
			this.btn_loadsrvlist.UseVisualStyleBackColor = true;
			this.btn_loadsrvlist.Click += new EventHandler(this.btn_loadsrvlist_Click);
			this.lbl_secondstep.AutoSize = true;
			this.lbl_secondstep.Font = new System.Drawing.Font("Calibri", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lbl_secondstep.Location = new Point(24, 105);
			this.lbl_secondstep.Name = "lbl_secondstep";
			this.lbl_secondstep.Size = new System.Drawing.Size(146, 19);
			this.lbl_secondstep.TabIndex = 4;
			this.lbl_secondstep.Text = "2. Server auswählen";
			this.lbl_thirdstep.AutoSize = true;
			this.lbl_thirdstep.Font = new System.Drawing.Font("Calibri", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lbl_thirdstep.Location = new Point(24, 166);
			this.lbl_thirdstep.Name = "lbl_thirdstep";
			this.lbl_thirdstep.Size = new System.Drawing.Size(95, 19);
			this.lbl_thirdstep.TabIndex = 5;
			this.lbl_thirdstep.Text = "3. Verbinden";
			this.btn_connectserver.Location = new Point(273, 156);
			this.btn_connectserver.Name = "btn_connectserver";
			this.btn_connectserver.Size = new System.Drawing.Size(75, 41);
			this.btn_connectserver.TabIndex = 6;
			this.btn_connectserver.Text = "Verbinde mit Server";
			this.btn_connectserver.UseVisualStyleBackColor = true;
			this.btn_connectserver.Click += new EventHandler(this.btn_connectserver_Click);
			this.grp_srvstatus.BackColor = SystemColors.ActiveCaption;
			this.grp_srvstatus.Controls.Add(this.lbl_ping);
			this.grp_srvstatus.Controls.Add(this.lbl_txt_ping);
			this.grp_srvstatus.Controls.Add(this.lbl_srvver);
			this.grp_srvstatus.Controls.Add(this.lbl_txt_srvver);
			this.grp_srvstatus.Controls.Add(this.lbl_usract);
			this.grp_srvstatus.Controls.Add(this.lbl_usridl);
			this.grp_srvstatus.Controls.Add(this.lbl_usronl);
			this.grp_srvstatus.Controls.Add(this.lbl_srvtyp);
			this.grp_srvstatus.Controls.Add(this.lbl_srvstatus);
			this.grp_srvstatus.Controls.Add(this.label4);
			this.grp_srvstatus.Controls.Add(this.lbl_txt_usridle);
			this.grp_srvstatus.Controls.Add(this.lbl_txt_usronl);
			this.grp_srvstatus.Controls.Add(this.lbl_txt_srvtyp);
			this.grp_srvstatus.Controls.Add(this.lbl_txt_srvstatus);
			this.grp_srvstatus.Font = new System.Drawing.Font("Calibri", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.grp_srvstatus.ForeColor = SystemColors.HighlightText;
			this.grp_srvstatus.Location = new Point(453, 42);
			this.grp_srvstatus.Name = "grp_srvstatus";
			this.grp_srvstatus.Size = new System.Drawing.Size(305, 162);
			this.grp_srvstatus.TabIndex = 7;
			this.grp_srvstatus.TabStop = false;
			this.grp_srvstatus.Text = "Status des gewählten Servers";
			this.lbl_ping.AutoSize = true;
			this.lbl_ping.Location = new Point(120, 136);
			this.lbl_ping.Name = "lbl_ping";
			this.lbl_ping.Size = new System.Drawing.Size(0, 19);
			this.lbl_ping.TabIndex = 13;
			this.lbl_txt_ping.AutoSize = true;
			this.lbl_txt_ping.Location = new Point(7, 136);
			this.lbl_txt_ping.Name = "lbl_txt_ping";
			this.lbl_txt_ping.Size = new System.Drawing.Size(47, 19);
			this.lbl_txt_ping.TabIndex = 12;
			this.lbl_txt_ping.Text = "Ping: ";
			this.lbl_srvver.AutoSize = true;
			this.lbl_srvver.Location = new Point(120, 60);
			this.lbl_srvver.Name = "lbl_srvver";
			this.lbl_srvver.Size = new System.Drawing.Size(0, 19);
			this.lbl_srvver.TabIndex = 11;
			this.lbl_txt_srvver.AutoSize = true;
			this.lbl_txt_srvver.Location = new Point(7, 61);
			this.lbl_txt_srvver.Name = "lbl_txt_srvver";
			this.lbl_txt_srvver.Size = new System.Drawing.Size(107, 19);
			this.lbl_txt_srvver.TabIndex = 10;
			this.lbl_txt_srvver.Text = "Serverversion:";
			this.lbl_usract.AutoSize = true;
			this.lbl_usract.Location = new Point(120, 117);
			this.lbl_usract.Name = "lbl_usract";
			this.lbl_usract.Size = new System.Drawing.Size(0, 19);
			this.lbl_usract.TabIndex = 9;
			this.lbl_usridl.AutoSize = true;
			this.lbl_usridl.Location = new Point(120, 98);
			this.lbl_usridl.Name = "lbl_usridl";
			this.lbl_usridl.Size = new System.Drawing.Size(0, 19);
			this.lbl_usridl.TabIndex = 8;
			this.lbl_usronl.AutoSize = true;
			this.lbl_usronl.Location = new Point(120, 79);
			this.lbl_usronl.Name = "lbl_usronl";
			this.lbl_usronl.Size = new System.Drawing.Size(0, 19);
			this.lbl_usronl.TabIndex = 7;
			this.lbl_srvtyp.AutoSize = true;
			this.lbl_srvtyp.Location = new Point(120, 43);
			this.lbl_srvtyp.Name = "lbl_srvtyp";
			this.lbl_srvtyp.Size = new System.Drawing.Size(0, 19);
			this.lbl_srvtyp.TabIndex = 6;
			this.lbl_srvstatus.AutoSize = true;
			this.lbl_srvstatus.Location = new Point(120, 27);
			this.lbl_srvstatus.Name = "lbl_srvstatus";
			this.lbl_srvstatus.Size = new System.Drawing.Size(0, 19);
			this.lbl_srvstatus.TabIndex = 5;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(7, 117);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 19);
			this.label4.TabIndex = 4;
			this.label4.Text = "User aktiv:";
			this.lbl_txt_usridle.AutoSize = true;
			this.lbl_txt_usridle.Location = new Point(7, 98);
			this.lbl_txt_usridle.Name = "lbl_txt_usridle";
			this.lbl_txt_usridle.Size = new System.Drawing.Size(93, 19);
			this.lbl_txt_usridle.TabIndex = 3;
			this.lbl_txt_usridle.Text = "User inaktiv:";
			this.lbl_txt_usronl.AutoSize = true;
			this.lbl_txt_usronl.Location = new Point(7, 79);
			this.lbl_txt_usronl.Name = "lbl_txt_usronl";
			this.lbl_txt_usronl.Size = new System.Drawing.Size(92, 19);
			this.lbl_txt_usronl.TabIndex = 2;
			this.lbl_txt_usronl.Text = "User Online:";
			this.lbl_txt_srvtyp.AutoSize = true;
			this.lbl_txt_srvtyp.Location = new Point(7, 43);
			this.lbl_txt_srvtyp.Name = "lbl_txt_srvtyp";
			this.lbl_txt_srvtyp.Size = new System.Drawing.Size(80, 19);
			this.lbl_txt_srvtyp.TabIndex = 1;
			this.lbl_txt_srvtyp.Text = "Servertyp:";
			this.lbl_txt_srvstatus.AutoSize = true;
			this.lbl_txt_srvstatus.Location = new Point(7, 27);
			this.lbl_txt_srvstatus.Name = "lbl_txt_srvstatus";
			this.lbl_txt_srvstatus.Size = new System.Drawing.Size(98, 19);
			this.lbl_txt_srvstatus.TabIndex = 0;
			this.lbl_txt_srvstatus.Text = "Serverstatus:";
			this.lbl_httptimeout.AutoSize = true;
			this.lbl_httptimeout.Location = new Point(370, 51);
			this.lbl_httptimeout.Name = "lbl_httptimeout";
			this.lbl_httptimeout.Size = new System.Drawing.Size(45, 13);
			this.lbl_httptimeout.TabIndex = 8;
			this.lbl_httptimeout.Text = "Timeout";
			this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.mnu_Datei });
			this.menuStrip1.Location = new Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(777, 24);
			this.menuStrip1.TabIndex = 9;
			this.menuStrip1.Text = "menuStrip1";
			this.mnu_Datei.DropDownItems.AddRange(new ToolStripItem[] { this.mnu_einstellungen, this.mnu_beenden });
			this.mnu_Datei.Name = "mnu_Datei";
			this.mnu_Datei.Size = new System.Drawing.Size(46, 20);
			this.mnu_Datei.Text = "Datei";
			this.mnu_einstellungen.Name = "mnu_einstellungen";
			this.mnu_einstellungen.Size = new System.Drawing.Size(145, 22);
			this.mnu_einstellungen.Text = "Einstellungen";
			this.mnu_einstellungen.Click += new EventHandler(this.mnu_einstellungen_Click);
			this.mnu_beenden.Name = "mnu_beenden";
			this.mnu_beenden.Size = new System.Drawing.Size(145, 22);
			this.mnu_beenden.Text = "Beenden";
			this.mnu_beenden.Click += new EventHandler(this.mnu_beenden_Click);
			this.btn_winpcapdl.Location = new Point(358, 156);
			this.btn_winpcapdl.Name = "btn_winpcapdl";
			this.btn_winpcapdl.Size = new System.Drawing.Size(75, 41);
			this.btn_winpcapdl.TabIndex = 10;
			this.btn_winpcapdl.Text = "WinPCAP Download";
			this.btn_winpcapdl.UseVisualStyleBackColor = true;
			this.btn_winpcapdl.Visible = false;
			this.btn_winpcapdl.Click += new EventHandler(this.btn_winpcapdl_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(777, 217);
			base.Controls.Add(this.btn_winpcapdl);
			base.Controls.Add(this.lbl_httptimeout);
			base.Controls.Add(this.grp_srvstatus);
			base.Controls.Add(this.btn_connectserver);
			base.Controls.Add(this.lbl_thirdstep);
			base.Controls.Add(this.lbl_secondstep);
			base.Controls.Add(this.btn_loadsrvlist);
			base.Controls.Add(this.drp_srvlist);
			base.Controls.Add(this.lbl_firststep);
			base.Controls.Add(this.menuStrip1);
			base.MainMenuStrip = this.menuStrip1;
			base.Name = "LANPlayClient";
			this.Text = "LAN Play Client GUI";
			base.Load += new EventHandler(this.Form1_Load);
			this.grp_srvstatus.ResumeLayout(false);
			this.grp_srvstatus.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void mnu_beenden_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void mnu_einstellungen_Click(object sender, EventArgs e)
		{
			(new frm_einstellungen()).Show();
		}

		private bool ping(string url)
		{
			bool flag;
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\r3n3kutaro\\LPgui");
				int reg_httptimeout = int.Parse(key.GetValue("httptimeout").ToString());
				request.Timeout = reg_httptimeout;
				this.lbl_httptimeout.Text = string.Concat("Timeout:", reg_httptimeout.ToString());
				request.AllowAutoRedirect = false;
				request.Method = "HEAD";
				using (WebResponse response = request.GetResponse())
				{
					flag = true;
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}
	}
}