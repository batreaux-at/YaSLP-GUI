namespace LANPlayClient
{
    using Microsoft.Win32;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Windows.Forms;

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
            string text = this.drp_srvlist.Text;
            Process.Start(Registry.CurrentUser.CreateSubKey(@"SOFTWARE\r3n3kutaro\LPgui").GetValue("LPClientDir").ToString() + @"\lan-play-win64.exe", " --relay-server-addr " + text.Substring(7, (text.Length - 5) - 7));
        }

        private void btn_loadsrvlist_Click(object sender, EventArgs e)
        {
            this.drp_srvlist.Items.Clear();
            string str = "";
            string str2 = "";
            byte[] bytes = new WebClient().DownloadData(Registry.CurrentUser.CreateSubKey(@"SOFTWARE\r3n3kutaro\LPgui").GetValue("serverlisturl").ToString());
            char[] separator = new char[] { '{' };
            string[] strArray = Encoding.UTF8.GetString(bytes).Split(separator);
            int index = 0;
            string[] strArray2 = strArray;
            int num2 = 0;
            while (num2 < strArray2.Length)
            {
                string str5 = strArray2[num2];
                char[] chArray2 = new char[] { ':' };
                string[] strArray3 = strArray[index].Split(chArray2);
                index++;
                int num3 = 0;
                string[] strArray4 = strArray3;
                int num4 = 0;
                while (true)
                {
                    if (num4 >= strArray4.Length)
                    {
                        num2++;
                        break;
                    }
                    string str6 = strArray4[num4];
                    string str7 = strArray3[num3].Replace(" ", "").Replace("\"", string.Empty);
                    if (str7.Contains("ip"))
                    {
                        str = strArray3[num3 + 1].Substring(1, strArray3[num3 + 1].IndexOf(',') - 2).Replace("\"", string.Empty);
                    }
                    if (str7.Contains("port"))
                    {
                        str2 = strArray3[num3 + 1].Substring(1, strArray3[num3 + 1].IndexOf(',')).Replace("\"", string.Empty).Replace(",", "");
                        string[] textArray1 = new string[] { "http://", str, ":", str2, "/info" };
                        string url = string.Concat(textArray1);
                        if (this.ping(url))
                        {
                            this.drp_srvlist.Items.Add(url);
                            this.drp_srvlist.SelectedIndex = 0;
                        }
                    }
                    num3++;
                    num4++;
                }
            }
        }

        private void btn_winpcapdl_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.winpcap.org/install/");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void drp_srvlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            byte[] bytes = client.DownloadData(this.drp_srvlist.SelectedItem.ToString());
            stopwatch.Stop();
            string str2 = Encoding.UTF8.GetString(bytes);
            char[] separator = new char[] { ':' };
            string[] strArray = str2.Split(separator);
            this.lbl_ping.Text = stopwatch.ElapsedMilliseconds.ToString() + " ms";
            stopwatch.Reset();
            if (!str2.Contains("rust"))
            {
                string str4 = strArray[2].Substring(1, strArray[2].IndexOf('"', 1) - 1);
                string str5 = strArray[1].Substring(0, strArray[1].IndexOf(","));
                this.lbl_srvstatus.Text = "online";
                this.lbl_srvver.Text = str4;
                this.lbl_usract.Text = "N/A";
                this.lbl_usridl.Text = "N/A";
                this.lbl_usronl.Text = str5;
                this.lbl_srvtyp.Text = "N/A";
            }
            else
            {
                this.lbl_srvtyp.Text = "Rust";
                string str3 = strArray[3].Substring(1, strArray[3].IndexOf('"', 1) - 1);
                this.lbl_srvstatus.Text = "online";
                int num2 = int.Parse(strArray[1].Substring(0, strArray[1].IndexOf(",")));
                int num3 = int.Parse(strArray[2].Substring(0, strArray[2].IndexOf(",")));
                this.lbl_usronl.Text = num2.ToString();
                this.lbl_usridl.Text = num3.ToString();
                this.lbl_usract.Text = (num2 - num3).ToString();
                this.lbl_srvver.Text = str3;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (ReferenceEquals(Registry.CurrentUser.OpenSubKey(@"SOFTWARE\r3n3kutaro\LPgui"), null))
            {
                RegistryKey key4 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\r3n3kutaro\LPgui");
                key4.SetValue("httptimeout", "300");
                key4.SetValue("serverlisturl", "https://raw.githubusercontent.com/GreatWizard/lan-play-status/master/public/data/servers.json");
                key4.SetValue("LPClientDir", @"C:\Kutaro-R3n3-LanplayGUI");
            }
            string path = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\r3n3kutaro\LPgui").GetValue("LPClientDir").ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!System.IO.File.Exists(path + @"\lan-play-win64.exe"))
            {
                WebClient client = new WebClient();
                frm_update _update = new frm_update();
                _update.Show();
                client.DownloadFile("https://github.com/spacemeowx2/switch-lan-play/releases/download/v0.2.1/lan-play-win64.exe", path + @"\lan-play-win64.exe");
                _update.Close();
            }
            if (ReferenceEquals(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\WinPcap"), null))
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
            this.lbl_firststep.Font = new Font("Calibri", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_firststep.Location = new Point(0x17, 0x2e);
            this.lbl_firststep.Name = "lbl_firststep";
            this.lbl_firststep.Size = new Size(0x8b, 0x13);
            this.lbl_firststep.TabIndex = 1;
            this.lbl_firststep.Text = "1. Serverliste laden";
            this.drp_srvlist.FormattingEnabled = true;
            this.drp_srvlist.Location = new Point(0xbc, 0x67);
            this.drp_srvlist.Name = "drp_srvlist";
            this.drp_srvlist.Size = new Size(0xf5, 0x15);
            this.drp_srvlist.TabIndex = 2;
            this.drp_srvlist.SelectedIndexChanged += new EventHandler(this.drp_srvlist_SelectedIndexChanged);
            this.btn_loadsrvlist.Location = new Point(260, 0x2a);
            this.btn_loadsrvlist.Name = "btn_loadsrvlist";
            this.btn_loadsrvlist.Size = new Size(0x67, 0x17);
            this.btn_loadsrvlist.TabIndex = 3;
            this.btn_loadsrvlist.Text = "Serverliste laden";
            this.btn_loadsrvlist.UseVisualStyleBackColor = true;
            this.btn_loadsrvlist.Click += new EventHandler(this.btn_loadsrvlist_Click);
            this.lbl_secondstep.AutoSize = true;
            this.lbl_secondstep.Font = new Font("Calibri", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_secondstep.Location = new Point(0x18, 0x69);
            this.lbl_secondstep.Name = "lbl_secondstep";
            this.lbl_secondstep.Size = new Size(0x92, 0x13);
            this.lbl_secondstep.TabIndex = 4;
            this.lbl_secondstep.Text = "2. Server ausw\x00e4hlen";
            this.lbl_thirdstep.AutoSize = true;
            this.lbl_thirdstep.Font = new Font("Calibri", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_thirdstep.Location = new Point(0x18, 0xa6);
            this.lbl_thirdstep.Name = "lbl_thirdstep";
            this.lbl_thirdstep.Size = new Size(0x5f, 0x13);
            this.lbl_thirdstep.TabIndex = 5;
            this.lbl_thirdstep.Text = "3. Verbinden";
            this.btn_connectserver.Location = new Point(0x111, 0x9c);
            this.btn_connectserver.Name = "btn_connectserver";
            this.btn_connectserver.Size = new Size(0x4b, 0x29);
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
            this.grp_srvstatus.Font = new Font("Calibri", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.grp_srvstatus.ForeColor = SystemColors.HighlightText;
            this.grp_srvstatus.Location = new Point(0x1c5, 0x2a);
            this.grp_srvstatus.Name = "grp_srvstatus";
            this.grp_srvstatus.Size = new Size(0x131, 0xa2);
            this.grp_srvstatus.TabIndex = 7;
            this.grp_srvstatus.TabStop = false;
            this.grp_srvstatus.Text = "Status des gew\x00e4hlten Servers";
            this.lbl_ping.AutoSize = true;
            this.lbl_ping.Location = new Point(120, 0x88);
            this.lbl_ping.Name = "lbl_ping";
            this.lbl_ping.Size = new Size(0, 0x13);
            this.lbl_ping.TabIndex = 13;
            this.lbl_txt_ping.AutoSize = true;
            this.lbl_txt_ping.Location = new Point(7, 0x88);
            this.lbl_txt_ping.Name = "lbl_txt_ping";
            this.lbl_txt_ping.Size = new Size(0x2f, 0x13);
            this.lbl_txt_ping.TabIndex = 12;
            this.lbl_txt_ping.Text = "Ping: ";
            this.lbl_srvver.AutoSize = true;
            this.lbl_srvver.Location = new Point(120, 60);
            this.lbl_srvver.Name = "lbl_srvver";
            this.lbl_srvver.Size = new Size(0, 0x13);
            this.lbl_srvver.TabIndex = 11;
            this.lbl_txt_srvver.AutoSize = true;
            this.lbl_txt_srvver.Location = new Point(7, 0x3d);
            this.lbl_txt_srvver.Name = "lbl_txt_srvver";
            this.lbl_txt_srvver.Size = new Size(0x6b, 0x13);
            this.lbl_txt_srvver.TabIndex = 10;
            this.lbl_txt_srvver.Text = "Serverversion:";
            this.lbl_usract.AutoSize = true;
            this.lbl_usract.Location = new Point(120, 0x75);
            this.lbl_usract.Name = "lbl_usract";
            this.lbl_usract.Size = new Size(0, 0x13);
            this.lbl_usract.TabIndex = 9;
            this.lbl_usridl.AutoSize = true;
            this.lbl_usridl.Location = new Point(120, 0x62);
            this.lbl_usridl.Name = "lbl_usridl";
            this.lbl_usridl.Size = new Size(0, 0x13);
            this.lbl_usridl.TabIndex = 8;
            this.lbl_usronl.AutoSize = true;
            this.lbl_usronl.Location = new Point(120, 0x4f);
            this.lbl_usronl.Name = "lbl_usronl";
            this.lbl_usronl.Size = new Size(0, 0x13);
            this.lbl_usronl.TabIndex = 7;
            this.lbl_srvtyp.AutoSize = true;
            this.lbl_srvtyp.Location = new Point(120, 0x2b);
            this.lbl_srvtyp.Name = "lbl_srvtyp";
            this.lbl_srvtyp.Size = new Size(0, 0x13);
            this.lbl_srvtyp.TabIndex = 6;
            this.lbl_srvstatus.AutoSize = true;
            this.lbl_srvstatus.Location = new Point(120, 0x1b);
            this.lbl_srvstatus.Name = "lbl_srvstatus";
            this.lbl_srvstatus.Size = new Size(0, 0x13);
            this.lbl_srvstatus.TabIndex = 5;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(7, 0x75);
            this.label4.Name = "label4";
            this.label4.Size = new Size(80, 0x13);
            this.label4.TabIndex = 4;
            this.label4.Text = "User aktiv:";
            this.lbl_txt_usridle.AutoSize = true;
            this.lbl_txt_usridle.Location = new Point(7, 0x62);
            this.lbl_txt_usridle.Name = "lbl_txt_usridle";
            this.lbl_txt_usridle.Size = new Size(0x5d, 0x13);
            this.lbl_txt_usridle.TabIndex = 3;
            this.lbl_txt_usridle.Text = "User inaktiv:";
            this.lbl_txt_usronl.AutoSize = true;
            this.lbl_txt_usronl.Location = new Point(7, 0x4f);
            this.lbl_txt_usronl.Name = "lbl_txt_usronl";
            this.lbl_txt_usronl.Size = new Size(0x5c, 0x13);
            this.lbl_txt_usronl.TabIndex = 2;
            this.lbl_txt_usronl.Text = "User Online:";
            this.lbl_txt_srvtyp.AutoSize = true;
            this.lbl_txt_srvtyp.Location = new Point(7, 0x2b);
            this.lbl_txt_srvtyp.Name = "lbl_txt_srvtyp";
            this.lbl_txt_srvtyp.Size = new Size(80, 0x13);
            this.lbl_txt_srvtyp.TabIndex = 1;
            this.lbl_txt_srvtyp.Text = "Servertyp:";
            this.lbl_txt_srvstatus.AutoSize = true;
            this.lbl_txt_srvstatus.Location = new Point(7, 0x1b);
            this.lbl_txt_srvstatus.Name = "lbl_txt_srvstatus";
            this.lbl_txt_srvstatus.Size = new Size(0x62, 0x13);
            this.lbl_txt_srvstatus.TabIndex = 0;
            this.lbl_txt_srvstatus.Text = "Serverstatus:";
            this.lbl_httptimeout.AutoSize = true;
            this.lbl_httptimeout.Location = new Point(370, 0x33);
            this.lbl_httptimeout.Name = "lbl_httptimeout";
            this.lbl_httptimeout.Size = new Size(0x2d, 13);
            this.lbl_httptimeout.TabIndex = 8;
            this.lbl_httptimeout.Text = "Timeout";
            ToolStripItem[] toolStripItems = new ToolStripItem[] { this.mnu_Datei };
            this.menuStrip1.Items.AddRange(toolStripItems);
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(0x309, 0x18);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            ToolStripItem[] itemArray2 = new ToolStripItem[] { this.mnu_einstellungen, this.mnu_beenden };
            this.mnu_Datei.DropDownItems.AddRange(itemArray2);
            this.mnu_Datei.Name = "mnu_Datei";
            this.mnu_Datei.Size = new Size(0x2e, 20);
            this.mnu_Datei.Text = "Datei";
            this.mnu_einstellungen.Name = "mnu_einstellungen";
            this.mnu_einstellungen.Size = new Size(0x91, 0x16);
            this.mnu_einstellungen.Text = "Einstellungen";
            this.mnu_einstellungen.Click += new EventHandler(this.mnu_einstellungen_Click);
            this.mnu_beenden.Name = "mnu_beenden";
            this.mnu_beenden.Size = new Size(0x91, 0x16);
            this.mnu_beenden.Text = "Beenden";
            this.mnu_beenden.Click += new EventHandler(this.mnu_beenden_Click);
            this.btn_winpcapdl.Location = new Point(0x166, 0x9c);
            this.btn_winpcapdl.Name = "btn_winpcapdl";
            this.btn_winpcapdl.Size = new Size(0x4b, 0x29);
            this.btn_winpcapdl.TabIndex = 10;
            this.btn_winpcapdl.Text = "WinPCAP Download";
            this.btn_winpcapdl.UseVisualStyleBackColor = true;
            this.btn_winpcapdl.Visible = false;
            this.btn_winpcapdl.Click += new EventHandler(this.btn_winpcapdl_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x309, 0xd9);
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
            new frm_einstellungen().Show();
        }

        private bool ping(string url)
        {
            bool flag;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                int num = int.Parse(Registry.CurrentUser.CreateSubKey(@"SOFTWARE\r3n3kutaro\LPgui").GetValue("httptimeout").ToString());
                request.Timeout = num;
                this.lbl_httptimeout.Text = "Timeout:" + num.ToString();
                request.AllowAutoRedirect = false;
                request.Method = "HEAD";
                using (request.GetResponse())
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

