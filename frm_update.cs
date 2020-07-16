using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LANPlayClient
{
	public class frm_update : Form
	{
		private IContainer components = null;

		public Label lbl_update;

		public frm_update()
		{
			this.InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if ((!disposing ? false : this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_update));
            this.lbl_update = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_update
            // 
            this.lbl_update.AutoSize = true;
            this.lbl_update.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lbl_update.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_update.Location = new System.Drawing.Point(12, 29);
            this.lbl_update.Name = "lbl_update";
            this.lbl_update.Size = new System.Drawing.Size(560, 26);
            this.lbl_update.TabIndex = 0;
            this.lbl_update.Text = "Current Version of LAN-Play-Client is being downloaded! Please Wait!";
            this.lbl_update.UseCompatibleTextRendering = true;
            this.lbl_update.UseWaitCursor = true;
            // 
            // frm_update
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 79);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_update);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_update";
            this.Text = "Update";
            this.UseWaitCursor = true;
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}