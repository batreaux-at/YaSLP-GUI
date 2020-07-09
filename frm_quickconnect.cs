using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LANPlayClient
{
    public partial class frm_quickconnect : Form
    {
        public frm_quickconnect()
        {
            InitializeComponent();
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\r3n3kutaro\\LPgui");
            string LPClientDir = key.GetValue("LPClientDir").ToString();
            string exepath = string.Concat(LPClientDir, "\\lan-play-win64.exe");
            string serveraddress = txt_srvaddr.Text;
            string parameters = key.GetValue("Parameters").ToString();
            string runparameters = string.Concat(parameters, serveraddress);
            Process.Start(exepath, runparameters);
        }

        private void btn_back_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frm_quickconnect_Load(object sender, EventArgs e)
        {

        }
    }
}
