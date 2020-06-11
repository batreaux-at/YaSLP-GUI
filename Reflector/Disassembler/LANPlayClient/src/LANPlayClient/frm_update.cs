namespace LANPlayClient
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lbl_update = new Label();
            base.SuspendLayout();
            this.lbl_update.AutoSize = true;
            this.lbl_update.FlatStyle = FlatStyle.System;
            this.lbl_update.Font = new Font("Calibri", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lbl_update.Location = new Point(12, 0x1d);
            this.lbl_update.Name = "lbl_update";
            this.lbl_update.Size = new Size(0x220, 0x1a);
            this.lbl_update.TabIndex = 0;
            this.lbl_update.Text = "Aktuelle Version von LAN-Play wird heruntergeladen! Biite warten!";
            this.lbl_update.UseCompatibleTextRendering = true;
            this.lbl_update.UseWaitCursor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x235, 0x4f);
            base.ControlBox = false;
            base.Controls.Add(this.lbl_update);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frm_update";
            this.Text = "Update";
            base.UseWaitCursor = true;
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

