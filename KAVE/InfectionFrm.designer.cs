namespace KAVE
{
    partial class InfectionFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfectionFrm));
            this.virlb = new DevComponents.DotNetBar.LabelX();
            this.flb = new DevComponents.DotNetBar.LabelX();
            this.repairbtn = new DevComponents.DotNetBar.ButtonX();
            this.removebtn = new DevComponents.DotNetBar.ButtonX();
            this.quarantinebtn = new DevComponents.DotNetBar.ButtonX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // virlb
            // 
            // 
            // 
            // 
            this.virlb.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.virlb.Dock = System.Windows.Forms.DockStyle.Top;
            this.virlb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.virlb.ForeColor = System.Drawing.Color.Red;
            this.virlb.Location = new System.Drawing.Point(0, 0);
            this.virlb.Name = "virlb";
            this.virlb.Size = new System.Drawing.Size(518, 61);
            this.virlb.TabIndex = 0;
            this.virlb.Text = "Virus";
            this.virlb.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // flb
            // 
            // 
            // 
            // 
            this.flb.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.flb.Dock = System.Windows.Forms.DockStyle.Top;
            this.flb.Location = new System.Drawing.Point(0, 61);
            this.flb.Name = "flb";
            this.flb.Size = new System.Drawing.Size(518, 38);
            this.flb.TabIndex = 1;
            this.flb.Text = "File :";
            // 
            // repairbtn
            // 
            this.repairbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.repairbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.repairbtn.Location = new System.Drawing.Point(355, 10);
            this.repairbtn.Name = "repairbtn";
            this.repairbtn.Size = new System.Drawing.Size(75, 23);
            this.repairbtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.repairbtn.TabIndex = 2;
            this.repairbtn.Text = "Repair";
            this.repairbtn.Click += new System.EventHandler(this.repairbtn_Click);
            // 
            // removebtn
            // 
            this.removebtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.removebtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.removebtn.Location = new System.Drawing.Point(436, 10);
            this.removebtn.Name = "removebtn";
            this.removebtn.Size = new System.Drawing.Size(75, 23);
            this.removebtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.removebtn.TabIndex = 3;
            this.removebtn.Text = "Remove";
            this.removebtn.Click += new System.EventHandler(this.removebtn_Click);
            // 
            // quarantinebtn
            // 
            this.quarantinebtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.quarantinebtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.quarantinebtn.Location = new System.Drawing.Point(274, 10);
            this.quarantinebtn.Name = "quarantinebtn";
            this.quarantinebtn.Size = new System.Drawing.Size(75, 23);
            this.quarantinebtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.quarantinebtn.TabIndex = 4;
            this.quarantinebtn.Text = "Quarantine";
            this.quarantinebtn.Click += new System.EventHandler(this.quarantinebtn_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.buttonX1);
            this.panelEx1.Controls.Add(this.repairbtn);
            this.panelEx1.Controls.Add(this.quarantinebtn);
            this.panelEx1.Controls.Add(this.removebtn);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx1.Location = new System.Drawing.Point(0, 185);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(518, 36);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 5;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(193, 10);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 5;
            this.buttonX1.Text = "Learn";
            this.buttonX1.Visible = false;
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // InfectionFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 221);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.flb);
            this.Controls.Add(this.virlb);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InfectionFrm";
            this.Text = "New virus detected !";
            this.Load += new System.EventHandler(this.InfectionFrm_Load);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX virlb;
        private DevComponents.DotNetBar.LabelX flb;
        private DevComponents.DotNetBar.ButtonX repairbtn;
        private DevComponents.DotNetBar.ButtonX removebtn;
        private DevComponents.DotNetBar.ButtonX quarantinebtn;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
    }
}