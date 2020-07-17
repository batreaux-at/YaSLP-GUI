using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace YaSLPGUI
{
	public class frm_main : Form
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

		private MenuStrip menuStrip1;

		private ToolStripMenuItem mnu_Datei;

		private ToolStripMenuItem mnu_einstellungen;

		private ToolStripMenuItem mnu_beenden;
        private PictureBox pic_yoshi;
        private ProgressBar pb_loadsrvlist;
        private ToolStripMenuItem quickConnectToolStripMenuItem;
        private Label madeby;
        private ComboBox drp_tmpsrvlist;
        private ToolStripMenuItem mnu_update;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private Button btn_winpcapdl;

		public frm_main()
		{
			this.InitializeComponent();
		}

		private void btn_connectserver_Click(object sender, EventArgs e)
		{
			RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\YaSLP-GUI");
			string LPClientDir = key.GetValue("LPClientDir").ToString();
			string exepath = string.Concat(LPClientDir, "\\lan-play-win64.exe");
            if (drp_srvlist.SelectedItem == null) 
            {
                MessageBox.Show("Kein Server ausgewählt!", "FEHLER", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string chosenitem = drp_srvlist.SelectedItem.ToString();
            int elementid = int.Parse(chosenitem.Substring(0, chosenitem.IndexOf(':')));
            string usesrv = srvlistcl.srvlist[elementid,0] + ":" + srvlistcl.srvlist[elementid,1];
            string serveraddress = usesrv;
            string parameters = key.GetValue("Parameters").ToString();
			string runparameters = string.Concat(parameters, "--relay-server-addr ", serveraddress);
			Process.Start(exepath, runparameters);
		}
        public static string[,] Loadsrvlist()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\YaSLP-GUI");
            System.Uri srvlisturi = new System.Uri(key.GetValue("serverlisturl").ToString());
            byte[] raw1 = (new WebClient()).DownloadData(key.GetValue("serverlisturl").ToString());
            string SrvListFull = Encoding.UTF8.GetString(raw1).Replace("[", string.Empty).Replace("]", string.Empty).Replace("\"", string.Empty).Replace(" ", string.Empty).Replace("}", string.Empty);
            string[] Servers = SrvListFull.Split(new char[] { '{' });
            JsonTextReader reader = new JsonTextReader(new StringReader(Encoding.UTF8.GetString(raw1)));
            int counter = 0;
            int servercount = 0;
            if (Servers[0] == "" && Servers[1] == "servers:")
            {
                servercount = Servers.Length - 2;
            } else
            {
                servercount = Servers.Length;
            }
            string[,] serverliste = new string[servercount, 6];
            while (reader.Read())
            {
                if (reader.Value != null)
                {
                    //MessageBox.Show(reader.Path);
                    if (reader.Path != "[0].servers")
                    {
                        int posopen = 0;
                        int posclose = 0;
                        if (reader.Path.StartsWith("[0].servers"))
                        {
                            posclose = reader.Path.IndexOf(']', 3);
                            posopen = reader.Path.IndexOf('[', 3);
                        } else
                        {
                            posclose = reader.Path.IndexOf(']');
                            posopen = reader.Path.IndexOf('[');
                        }
                            counter = int.Parse(reader.Path.Substring(posopen+1,posclose-posopen - 1));
                        string pathport = "";
                        string pathflag = "";
                        string pathplatform = "";
                        string pathtype = "";
                        string pathhidden = "";
                        string pathip = "";

                        if (reader.Path.StartsWith("[0].servers"))
                        {
                            pathport = "[0].servers[" + counter.ToString() + "].port";
                            pathflag = "[0].servers[" + counter.ToString() + "].flag";
                            pathplatform = "[0].servers[" + counter.ToString() + "].platform";
                            pathtype = "[0].servers[" + counter.ToString() + "].type";
                            pathhidden = "[0].servers[" + counter.ToString() + "].hidden";
                            pathip = "[0].servers[" + counter.ToString() + "].ip";
                        } else
                        {
                            pathport = "[" + counter.ToString() + "].port";
                            pathflag = "[" + counter.ToString() + "].flag";
                            pathplatform = "[" + counter.ToString() + "].platform";
                            pathtype = "[" + counter.ToString() + "].type";
                            pathhidden = "[" + counter.ToString() + "].hidden";
                            pathip = "[" + counter.ToString() + "].ip";
                        }
                        if (reader.Path.ToString() == (pathport) && reader.Value.ToString() != "port")
                        {
                            serverliste[counter, 1] = reader.Value.ToString();
                        }
                        if (reader.Path.ToString() == (pathflag) && reader.Value.ToString() != "flag")
                        {
                            serverliste[counter, 2] = reader.Value.ToString();
                        }
                        if (reader.Path.ToString() == (pathplatform) && reader.Value.ToString() != "platform")
                        {
                            serverliste[counter, 3] = reader.Value.ToString();
                        }
                        if (reader.Path.ToString() == (pathtype) && reader.Value.ToString() != "type")
                        {
                            serverliste[counter, 4] = reader.Value.ToString();
                        }
                        if (reader.Path.ToString() == (pathhidden) && reader.Value.ToString() != "hidden")
                        {
                            serverliste[counter, 5] = reader.Value.ToString();
                        }
                        if (reader.Path.ToString() == (pathip) && reader.Value.ToString() != "ip")
                        {
                            serverliste[counter, 0] = reader.Value.ToString();
                        }
                    }
                }
            }
            return serverliste;
        }
            static class srvlistcl
            {
                public static string[,] srvlist = Loadsrvlist();
            }
            public void btn_loadsrvlist_Click(object sender, EventArgs e)
		{

            //var t1 = await Task.Run(() => Loadsrvlist());
            //string[,] srvlist = Loadsrvlist();
            btn_loadsrvlist.Enabled = false;
            int counter2 = 0;
            drp_srvlist.Items.Clear();
            drp_tmpsrvlist.Items.Clear();
            
            Cursor.Current = Cursors.WaitCursor;
            foreach (string s in srvlistcl.srvlist)

            {
                if (counter2 < srvlistcl.srvlist.GetLength(0))
                {
                    if (srvlistcl.srvlist[counter2, 0] != null)
                    {
                        string url = null;
                        if (srvlistcl.srvlist[counter2, 4] == "dotnet")
                        {
                            url = "http://" + srvlistcl.srvlist[counter2, 0] + ":" + srvlistcl.srvlist[counter2, 1] + "/";
                        }
                        else
                        {
                            url = "http://" + srvlistcl.srvlist[counter2, 0] + ":" + srvlistcl.srvlist[counter2, 1] + "/info";
                        }
                        drp_tmpsrvlist.Items.Add(counter2 + ":" + srvlistcl.srvlist[counter2, 0] + ":" + srvlistcl.srvlist[counter2, 1]);
                        counter2++;
                    }
                }
            }
            
            //drp_srvlist.SelectedIndex = 0;
            checkonlinestatus();
            Cursor.Current = Cursors.Default;
            
        }

        public async void checkonlinestatus()
        {
            pb_loadsrvlist.Visible = true;
            pic_yoshi.Enabled = true;
            int counter = 0;
            foreach (string items in drp_tmpsrvlist.Items)
            {
                pb_loadsrvlist.Maximum = drp_tmpsrvlist.Items.Count;
                pb_loadsrvlist.Value = counter;
                string type = srvlistcl.srvlist[Int32.Parse(items.Substring(0, items.IndexOf(':'))), 4];
                string url = "";
                if (type == "dotnet")
                {
                    url = "http://" + items.Substring(items.IndexOf(':') + 1) + "/";
                } else
                {
                    url = "http://" + items.Substring(items.IndexOf(':') + 1) + "/info";
                }
                bool pingres = false;
                await Task.Run(() => pingres = ping(url));
                //MessageBox.Show("Server: " + items + " Ping: " + pingres);
                if (pingres == true)
                {
                    drp_srvlist.Items.Add(items);
                }
                counter++;
            }
            pb_loadsrvlist.Visible = false;
            pic_yoshi.Enabled = false;
            btn_loadsrvlist.Enabled = true;
            

        }
		private void btn_winpcapdl_Click(object sender, EventArgs e)
		{
			Process.Start("https://www.winpcap.org/install/");
		}

		protected override void Dispose(bool disposing)
		{
			if ((disposing && this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void drp_srvlist_SelectedIndexChanged(object sender, EventArgs e)
		{
			WebClient wc = new WebClient();
            if (drp_srvlist.SelectedItem != null)
            {
                string chosenitem = drp_srvlist.SelectedItem.ToString();
                int elementid = int.Parse(chosenitem.Substring(0, chosenitem.IndexOf(':')));
                string item = srvlistcl.srvlist[elementid, 0] + ":" + srvlistcl.srvlist[elementid, 1];
                string url = null;
                if (srvlistcl.srvlist[elementid, 4] == "dotnet")
                {
                    url = "http://" + item;
                }
                else
                {
                    url = "http://" + item + "/info";
                }
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
                if (webData.Contains("rust"))
                {
                    this.lbl_srvtyp.Text = srvlistcl.srvlist[elementid, 4];
                    string srvver = DataArray[3].Substring(1, DataArray[3].IndexOf('\"', 1) - 1);
                    this.lbl_srvstatus.Text = "online";
                    int useronline = int.Parse(DataArray[1].Substring(0, DataArray[1].IndexOf(",")));
                    int useridle = int.Parse(DataArray[2].Substring(0, DataArray[2].IndexOf(",")));
                    this.lbl_usronl.Text = useronline.ToString();
                    this.lbl_usridl.Text = useridle.ToString();
                    this.lbl_usract.Text = (useronline - useridle).ToString();
                    this.lbl_srvver.Text = srvver;
                }
                else if (srvlistcl.srvlist[elementid, 4] == "dotnet")
                {
                    string srvver = DataArray[2].Substring(1, DataArray[2].IndexOf('\"', 1) - 1);
                    string useronline = DataArray[5].Substring(0, DataArray[5].IndexOf('}'));
                    this.lbl_srvstatus.Text = "online";
                    this.lbl_srvver.Text = "N/A";
                    this.lbl_usract.Text = "N/A";
                    this.lbl_usridl.Text = "N/A";
                    this.lbl_usronl.Text = useronline;
                    this.lbl_srvtyp.Text = srvlistcl.srvlist[elementid, 4];
                }
                else
                {
                    string srvver = DataArray[2].Substring(1, DataArray[2].IndexOf('\"', 1) - 1);
                    string useronline = DataArray[1].Substring(0, DataArray[1].IndexOf(","));
                    this.lbl_srvstatus.Text = "online";
                    this.lbl_srvver.Text = srvver;
                    this.lbl_usract.Text = "N/A";
                    this.lbl_usridl.Text = "N/A";
                    this.lbl_usronl.Text = useronline;
                    this.lbl_srvtyp.Text = srvlistcl.srvlist[elementid, 4];
                }
            }
		}

		private void Form1_Load(object sender, EventArgs e)
		{
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\YaSLP-GUI");
            if (key.GetValue("httptimeout") == null)
            {
                key.SetValue("httptimeout", "1000");
            }
            if (key.GetValue("serverlisturl") == null)
            {
                key.SetValue("serverlisturl", "https://raw.githubusercontent.com/GreatWizard/lan-play-status/master/public/data/servers.json");
            }
            if (key.GetValue("LPClientDir") == null)
            {
               var answer = MessageBox.Show("Do you want to use the default Directory for the LANPlay Client to store? (C:\\YaSLP-GUI\\)", "Directory Setting", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                FolderBrowserDialog clientpath = new FolderBrowserDialog();
                clientpath.Description = "Choose the Folder for the LANPlay Client";
               if (answer == DialogResult.No)
                {
                    DialogResult result = clientpath.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        key.SetValue("LPClientDir", clientpath.SelectedPath.ToString());
                    } else
                    {
                        key.SetValue("LPClientDir", "C:\\YaSLP-GUI");
                    }
                } else
                {
                    key.SetValue("LPClientDir", "C:\\YaSLP-GUI");
                }
               //key.SetValue("LPClientDir", "C:\\Kutaro-R3n3-LanplayGUI");
            }
            if (key.GetValue("Parameters") == null)
            {
                key.SetValue("Parameters", " ");
            }
            if (key.GetValue("Parametersmode") == null)
            {
                key.SetValue("Parametersmode", "1");
            }

            RegistryKey regkey = Registry.CurrentUser.CreateSubKey("Software\\YaSLP-GUI");
			string LPClientDir = regkey.GetValue("LPClientDir").ToString();
			if (!Directory.Exists(LPClientDir))
			{
				Directory.CreateDirectory(LPClientDir);
			}
			if (!File.Exists(string.Concat(LPClientDir, "\\lan-play-win64.exe")))
			{
                doclientupdate();
			}
            RegistryKey npcapparam = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\npcap\\Parameters");
			if (Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\WinPcap") == null && npcapparam.GetValue("WinPcapCompatible").ToString() != "1")
			{
                string npcaperrortext = "";
                if (npcapparam.GetValue("WinPcapCompatible").ToString() == "0")
                {
                    npcaperrortext = "\r\n NPCAP ist falsch konfiguriert! \r\n WinPcapCompatible: "+ npcapparam.GetValue("WinPcapCompatible").ToString();
                }
				MessageBox.Show("WinPCAP oder NPCAP fehlt! \r\n Lanplay kann nicht gestartet werden!" + npcaperrortext, "WinPCAP oder NPCAP fehlt", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				this.btn_connectserver.Enabled = false;
				this.btn_winpcapdl.Visible = true;
			}
		}

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_main));
            this.lbl_firststep = new System.Windows.Forms.Label();
            this.drp_srvlist = new System.Windows.Forms.ComboBox();
            this.btn_loadsrvlist = new System.Windows.Forms.Button();
            this.lbl_secondstep = new System.Windows.Forms.Label();
            this.lbl_thirdstep = new System.Windows.Forms.Label();
            this.btn_connectserver = new System.Windows.Forms.Button();
            this.grp_srvstatus = new System.Windows.Forms.GroupBox();
            this.lbl_ping = new System.Windows.Forms.Label();
            this.lbl_txt_ping = new System.Windows.Forms.Label();
            this.lbl_srvver = new System.Windows.Forms.Label();
            this.lbl_txt_srvver = new System.Windows.Forms.Label();
            this.lbl_usract = new System.Windows.Forms.Label();
            this.lbl_usridl = new System.Windows.Forms.Label();
            this.lbl_usronl = new System.Windows.Forms.Label();
            this.lbl_srvtyp = new System.Windows.Forms.Label();
            this.lbl_srvstatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_txt_usridle = new System.Windows.Forms.Label();
            this.lbl_txt_usronl = new System.Windows.Forms.Label();
            this.lbl_txt_srvtyp = new System.Windows.Forms.Label();
            this.lbl_txt_srvstatus = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnu_Datei = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_einstellungen = new System.Windows.Forms.ToolStripMenuItem();
            this.quickConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_update = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_beenden = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_winpcapdl = new System.Windows.Forms.Button();
            this.pic_yoshi = new System.Windows.Forms.PictureBox();
            this.pb_loadsrvlist = new System.Windows.Forms.ProgressBar();
            this.madeby = new System.Windows.Forms.Label();
            this.drp_tmpsrvlist = new System.Windows.Forms.ComboBox();
            this.grp_srvstatus.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_yoshi)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_firststep
            // 
            this.lbl_firststep.AutoSize = true;
            this.lbl_firststep.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_firststep.Location = new System.Drawing.Point(23, 46);
            this.lbl_firststep.Name = "lbl_firststep";
            this.lbl_firststep.Size = new System.Drawing.Size(126, 19);
            this.lbl_firststep.TabIndex = 1;
            this.lbl_firststep.Text = "1. Load Serverlist";
            // 
            // drp_srvlist
            // 
            this.drp_srvlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.drp_srvlist.FormattingEnabled = true;
            this.drp_srvlist.Location = new System.Drawing.Point(28, 96);
            this.drp_srvlist.Name = "drp_srvlist";
            this.drp_srvlist.Size = new System.Drawing.Size(437, 528);
            this.drp_srvlist.TabIndex = 2;
            this.drp_srvlist.SelectedIndexChanged += new System.EventHandler(this.drp_srvlist_SelectedIndexChanged);
            // 
            // btn_loadsrvlist
            // 
            this.btn_loadsrvlist.Location = new System.Drawing.Point(260, 42);
            this.btn_loadsrvlist.Name = "btn_loadsrvlist";
            this.btn_loadsrvlist.Size = new System.Drawing.Size(103, 23);
            this.btn_loadsrvlist.TabIndex = 3;
            this.btn_loadsrvlist.Text = "Load it!";
            this.btn_loadsrvlist.UseVisualStyleBackColor = true;
            this.btn_loadsrvlist.Click += new System.EventHandler(this.btn_loadsrvlist_Click);
            // 
            // lbl_secondstep
            // 
            this.lbl_secondstep.AutoSize = true;
            this.lbl_secondstep.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_secondstep.Location = new System.Drawing.Point(24, 69);
            this.lbl_secondstep.Name = "lbl_secondstep";
            this.lbl_secondstep.Size = new System.Drawing.Size(114, 19);
            this.lbl_secondstep.TabIndex = 4;
            this.lbl_secondstep.Text = "2. Select Server";
            // 
            // lbl_thirdstep
            // 
            this.lbl_thirdstep.AutoSize = true;
            this.lbl_thirdstep.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_thirdstep.Location = new System.Drawing.Point(39, 645);
            this.lbl_thirdstep.Name = "lbl_thirdstep";
            this.lbl_thirdstep.Size = new System.Drawing.Size(148, 19);
            this.lbl_thirdstep.TabIndex = 5;
            this.lbl_thirdstep.Text = "3. Connect to Server";
            // 
            // btn_connectserver
            // 
            this.btn_connectserver.Location = new System.Drawing.Point(288, 635);
            this.btn_connectserver.Name = "btn_connectserver";
            this.btn_connectserver.Size = new System.Drawing.Size(75, 41);
            this.btn_connectserver.TabIndex = 6;
            this.btn_connectserver.Text = "Connect to Server";
            this.btn_connectserver.UseVisualStyleBackColor = true;
            this.btn_connectserver.Click += new System.EventHandler(this.btn_connectserver_Click);
            // 
            // grp_srvstatus
            // 
            this.grp_srvstatus.BackColor = System.Drawing.SystemColors.ActiveCaption;
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
            this.grp_srvstatus.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_srvstatus.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grp_srvstatus.Location = new System.Drawing.Point(475, 150);
            this.grp_srvstatus.Name = "grp_srvstatus";
            this.grp_srvstatus.Size = new System.Drawing.Size(305, 162);
            this.grp_srvstatus.TabIndex = 7;
            this.grp_srvstatus.TabStop = false;
            this.grp_srvstatus.Text = "Server Status of selected Server";
            // 
            // lbl_ping
            // 
            this.lbl_ping.AutoSize = true;
            this.lbl_ping.Location = new System.Drawing.Point(120, 136);
            this.lbl_ping.Name = "lbl_ping";
            this.lbl_ping.Size = new System.Drawing.Size(0, 19);
            this.lbl_ping.TabIndex = 13;
            // 
            // lbl_txt_ping
            // 
            this.lbl_txt_ping.AutoSize = true;
            this.lbl_txt_ping.Location = new System.Drawing.Point(7, 136);
            this.lbl_txt_ping.Name = "lbl_txt_ping";
            this.lbl_txt_ping.Size = new System.Drawing.Size(47, 19);
            this.lbl_txt_ping.TabIndex = 12;
            this.lbl_txt_ping.Text = "Ping: ";
            // 
            // lbl_srvver
            // 
            this.lbl_srvver.AutoSize = true;
            this.lbl_srvver.Location = new System.Drawing.Point(120, 60);
            this.lbl_srvver.Name = "lbl_srvver";
            this.lbl_srvver.Size = new System.Drawing.Size(0, 19);
            this.lbl_srvver.TabIndex = 11;
            // 
            // lbl_txt_srvver
            // 
            this.lbl_txt_srvver.AutoSize = true;
            this.lbl_txt_srvver.Location = new System.Drawing.Point(7, 61);
            this.lbl_txt_srvver.Name = "lbl_txt_srvver";
            this.lbl_txt_srvver.Size = new System.Drawing.Size(67, 19);
            this.lbl_txt_srvver.TabIndex = 10;
            this.lbl_txt_srvver.Text = "Version: ";
            // 
            // lbl_usract
            // 
            this.lbl_usract.AutoSize = true;
            this.lbl_usract.Location = new System.Drawing.Point(120, 117);
            this.lbl_usract.Name = "lbl_usract";
            this.lbl_usract.Size = new System.Drawing.Size(0, 19);
            this.lbl_usract.TabIndex = 9;
            // 
            // lbl_usridl
            // 
            this.lbl_usridl.AutoSize = true;
            this.lbl_usridl.Location = new System.Drawing.Point(120, 98);
            this.lbl_usridl.Name = "lbl_usridl";
            this.lbl_usridl.Size = new System.Drawing.Size(0, 19);
            this.lbl_usridl.TabIndex = 8;
            // 
            // lbl_usronl
            // 
            this.lbl_usronl.AutoSize = true;
            this.lbl_usronl.Location = new System.Drawing.Point(120, 79);
            this.lbl_usronl.Name = "lbl_usronl";
            this.lbl_usronl.Size = new System.Drawing.Size(0, 19);
            this.lbl_usronl.TabIndex = 7;
            // 
            // lbl_srvtyp
            // 
            this.lbl_srvtyp.AutoSize = true;
            this.lbl_srvtyp.Location = new System.Drawing.Point(120, 43);
            this.lbl_srvtyp.Name = "lbl_srvtyp";
            this.lbl_srvtyp.Size = new System.Drawing.Size(0, 19);
            this.lbl_srvtyp.TabIndex = 6;
            // 
            // lbl_srvstatus
            // 
            this.lbl_srvstatus.AutoSize = true;
            this.lbl_srvstatus.Location = new System.Drawing.Point(120, 27);
            this.lbl_srvstatus.Name = "lbl_srvstatus";
            this.lbl_srvstatus.Size = new System.Drawing.Size(0, 19);
            this.lbl_srvstatus.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 19);
            this.label4.TabIndex = 4;
            this.label4.Text = "Active Users:";
            // 
            // lbl_txt_usridle
            // 
            this.lbl_txt_usridle.AutoSize = true;
            this.lbl_txt_usridle.Location = new System.Drawing.Point(7, 98);
            this.lbl_txt_usridle.Name = "lbl_txt_usridle";
            this.lbl_txt_usridle.Size = new System.Drawing.Size(78, 19);
            this.lbl_txt_usridle.TabIndex = 3;
            this.lbl_txt_usridle.Text = "Idle Users:";
            // 
            // lbl_txt_usronl
            // 
            this.lbl_txt_usronl.AutoSize = true;
            this.lbl_txt_usronl.Location = new System.Drawing.Point(7, 79);
            this.lbl_txt_usronl.Name = "lbl_txt_usronl";
            this.lbl_txt_usronl.Size = new System.Drawing.Size(98, 19);
            this.lbl_txt_usronl.TabIndex = 2;
            this.lbl_txt_usronl.Text = "Online Users:";
            // 
            // lbl_txt_srvtyp
            // 
            this.lbl_txt_srvtyp.AutoSize = true;
            this.lbl_txt_srvtyp.Location = new System.Drawing.Point(7, 43);
            this.lbl_txt_srvtyp.Name = "lbl_txt_srvtyp";
            this.lbl_txt_srvtyp.Size = new System.Drawing.Size(50, 19);
            this.lbl_txt_srvtyp.TabIndex = 1;
            this.lbl_txt_srvtyp.Text = "Type: ";
            // 
            // lbl_txt_srvstatus
            // 
            this.lbl_txt_srvstatus.AutoSize = true;
            this.lbl_txt_srvstatus.Location = new System.Drawing.Point(7, 27);
            this.lbl_txt_srvstatus.Name = "lbl_txt_srvstatus";
            this.lbl_txt_srvstatus.Size = new System.Drawing.Size(56, 19);
            this.lbl_txt_srvstatus.TabIndex = 0;
            this.lbl_txt_srvstatus.Text = "Status:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_Datei,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(794, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // mnu_Datei
            // 
            this.mnu_Datei.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_einstellungen,
            this.quickConnectToolStripMenuItem,
            this.mnu_update,
            this.mnu_beenden});
            this.mnu_Datei.Name = "mnu_Datei";
            this.mnu_Datei.Size = new System.Drawing.Size(50, 20);
            this.mnu_Datei.Text = "Menu";
            // 
            // mnu_einstellungen
            // 
            this.mnu_einstellungen.Name = "mnu_einstellungen";
            this.mnu_einstellungen.Size = new System.Drawing.Size(168, 22);
            this.mnu_einstellungen.Text = "Settings";
            this.mnu_einstellungen.Click += new System.EventHandler(this.mnu_einstellungen_Click);
            // 
            // quickConnectToolStripMenuItem
            // 
            this.quickConnectToolStripMenuItem.Name = "quickConnectToolStripMenuItem";
            this.quickConnectToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.quickConnectToolStripMenuItem.Text = "Quick Connect";
            this.quickConnectToolStripMenuItem.Click += new System.EventHandler(this.quickConnectToolStripMenuItem_Click);
            // 
            // mnu_update
            // 
            this.mnu_update.Name = "mnu_update";
            this.mnu_update.Size = new System.Drawing.Size(168, 22);
            this.mnu_update.Text = "Update";
            this.mnu_update.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // mnu_beenden
            // 
            this.mnu_beenden.Name = "mnu_beenden";
            this.mnu_beenden.Size = new System.Drawing.Size(168, 22);
            this.mnu_beenden.Text = "Quit / Exit /Finito!";
            this.mnu_beenden.Click += new System.EventHandler(this.mnu_beenden_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // btn_winpcapdl
            // 
            this.btn_winpcapdl.Location = new System.Drawing.Point(373, 635);
            this.btn_winpcapdl.Name = "btn_winpcapdl";
            this.btn_winpcapdl.Size = new System.Drawing.Size(75, 41);
            this.btn_winpcapdl.TabIndex = 10;
            this.btn_winpcapdl.Text = "WinPCAP Download";
            this.btn_winpcapdl.UseVisualStyleBackColor = true;
            this.btn_winpcapdl.Visible = false;
            this.btn_winpcapdl.Click += new System.EventHandler(this.btn_winpcapdl_Click);
            // 
            // pic_yoshi
            // 
            this.pic_yoshi.Enabled = false;
            this.pic_yoshi.Image = YaSLPGUI.Properties.Resources.FarawayNaiveBarnacle_size_restricted;
            this.pic_yoshi.Location = new System.Drawing.Point(501, 353);
            this.pic_yoshi.Name = "pic_yoshi";
            this.pic_yoshi.Size = new System.Drawing.Size(256, 256);
            this.pic_yoshi.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pic_yoshi.TabIndex = 11;
            this.pic_yoshi.TabStop = false;
            // 
            // pb_loadsrvlist
            // 
            this.pb_loadsrvlist.Location = new System.Drawing.Point(382, 42);
            this.pb_loadsrvlist.Name = "pb_loadsrvlist";
            this.pb_loadsrvlist.Size = new System.Drawing.Size(365, 23);
            this.pb_loadsrvlist.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pb_loadsrvlist.TabIndex = 12;
            this.pb_loadsrvlist.Visible = false;
            // 
            // madeby
            // 
            this.madeby.AutoSize = true;
            this.madeby.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.madeby.Location = new System.Drawing.Point(639, 663);
            this.madeby.Name = "madeby";
            this.madeby.Size = new System.Drawing.Size(148, 13);
            this.madeby.TabIndex = 13;
            this.madeby.Text = "Made by Batreaux and Kutaro";
            this.madeby.Click += new System.EventHandler(this.label1_Click);
            // 
            // drp_tmpsrvlist
            // 
            this.drp_tmpsrvlist.CausesValidation = false;
            this.drp_tmpsrvlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.drp_tmpsrvlist.FormattingEnabled = true;
            this.drp_tmpsrvlist.Location = new System.Drawing.Point(143, 92);
            this.drp_tmpsrvlist.Name = "drp_tmpsrvlist";
            this.drp_tmpsrvlist.Size = new System.Drawing.Size(333, 527);
            this.drp_tmpsrvlist.TabIndex = 14;
            this.drp_tmpsrvlist.Visible = false;
            // 
            // frm_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 688);
            this.Controls.Add(this.drp_tmpsrvlist);
            this.Controls.Add(this.madeby);
            this.Controls.Add(this.pb_loadsrvlist);
            this.Controls.Add(this.pic_yoshi);
            this.Controls.Add(this.btn_winpcapdl);
            this.Controls.Add(this.grp_srvstatus);
            this.Controls.Add(this.btn_connectserver);
            this.Controls.Add(this.lbl_thirdstep);
            this.Controls.Add(this.lbl_secondstep);
            this.Controls.Add(this.btn_loadsrvlist);
            this.Controls.Add(this.drp_srvlist);
            this.Controls.Add(this.lbl_firststep);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frm_main";
            this.Text = "YaSLP-GUI";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grp_srvstatus.ResumeLayout(false);
            this.grp_srvstatus.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_yoshi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
				RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\YaSLP-GUI");
				int reg_httptimeout = int.Parse(key.GetValue("httptimeout").ToString());
				request.Timeout = reg_httptimeout;
				request.AllowAutoRedirect = false;
				request.Method = "GET";
				using (WebResponse response = request.GetResponse())
				{
					flag = true;
				}
			}
			catch
			{
				flag = false;
			}
            //if (url == "http://lan-play.tk:11451/info")
            //{
            //    MessageBox.Show(url + " " + flag);
            //}
			return flag;
		}

        private void quickConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new frm_quickconnect()).Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doclientupdate(); 
        }

        public async void doclientupdate()
        {
            RegistryKey regkey = Registry.CurrentUser.CreateSubKey("Software\\YaSLP-GUI");
            string LPClientDir = regkey.GetValue("LPClientDir").ToString();
            if (!Directory.Exists(LPClientDir))
            {
                Directory.CreateDirectory(LPClientDir);
            }
            string downloadurl = "https://github.com/spacemeowx2/switch-lan-play/releases/latest/download/lan-play-win64.exe";
            System.Uri downloaduri = new System.Uri(downloadurl);
            WebClient downloadclient = new WebClient();
            frm_update displayupdate = new frm_update();
            displayupdate.Show();
            displayupdate.TopMost = true;
            await Task.Run(() => downloadclient.DownloadFile(downloaduri, string.Concat(LPClientDir, "\\lan-play-win64.exe")));
            displayupdate.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frm_about().Show();
        }
    }
}