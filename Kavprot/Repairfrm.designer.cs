namespace Kavprot
{
    partial class Repairfrm
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
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.virntxtlb = new DevComponents.DotNetBar.LabelX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.filetxt = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.virntxt = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.virntxtlb);
            this.panelEx1.Controls.Add(this.buttonX1);
            this.panelEx1.Controls.Add(this.filetxt);
            this.panelEx1.Controls.Add(this.virntxt);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(766, 406);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelX2.Location = new System.Drawing.Point(46, 39);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(523, 29);
            this.labelX2.TabIndex = 19;
            this.labelX2.Text = "Kavprot smart security repair";
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelX1.Location = new System.Drawing.Point(16, 170);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(516, 23);
            this.labelX1.TabIndex = 6;
            this.labelX1.Text = "file path";
            // 
            // virntxtlb
            // 
            // 
            // 
            // 
            this.virntxtlb.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.virntxtlb.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.virntxtlb.Location = new System.Drawing.Point(16, 115);
            this.virntxtlb.Name = "virntxtlb";
            this.virntxtlb.Size = new System.Drawing.Size(373, 23);
            this.virntxtlb.TabIndex = 5;
            this.virntxtlb.Text = "Virus name";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(468, 243);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(101, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 3;
            this.buttonX1.Text = "Repair";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // filetxt
            // 
            this.filetxt.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.filetxt.Border.Class = "TextBoxBorder";
            this.filetxt.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.filetxt.ForeColor = System.Drawing.Color.Black;
            this.filetxt.Location = new System.Drawing.Point(16, 197);
            this.filetxt.Name = "filetxt";
            this.filetxt.Size = new System.Drawing.Size(572, 20);
            this.filetxt.TabIndex = 2;
            // 
            // virntxt
            // 
            this.virntxt.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.virntxt.Border.Class = "TextBoxBorder";
            this.virntxt.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.virntxt.ForeColor = System.Drawing.Color.Black;
            this.virntxt.Location = new System.Drawing.Point(16, 144);
            this.virntxt.Name = "virntxt";
            this.virntxt.Size = new System.Drawing.Size(572, 20);
            this.virntxt.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Repairfrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx1);
            this.Name = "Repairfrm";
            this.Size = new System.Drawing.Size(766, 406);
            this.Load += new System.EventHandler(this.Repairfrm_Load);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.Controls.TextBoxX virntxt;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.Controls.TextBoxX filetxt;
        private DevComponents.DotNetBar.LabelX virntxtlb;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}