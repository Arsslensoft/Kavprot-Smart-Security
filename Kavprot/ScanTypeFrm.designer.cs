namespace Kavprot
{
    partial class ScanTypeFrm
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.scanfilebtn = new DevComponents.DotNetBar.ButtonX();
            this.zonescan = new DevComponents.DotNetBar.ButtonX();
            this.quickscanbtn = new DevComponents.DotNetBar.ButtonX();
            this.fullscanbtn = new DevComponents.DotNetBar.ButtonX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // scanfilebtn
            // 
            this.scanfilebtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.scanfilebtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.scanfilebtn.Location = new System.Drawing.Point(387, 283);
            this.scanfilebtn.Name = "scanfilebtn";
            this.scanfilebtn.Size = new System.Drawing.Size(122, 23);
            this.scanfilebtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.scanfilebtn.TabIndex = 15;
            this.scanfilebtn.Text = "Scan file";
            this.scanfilebtn.Click += new System.EventHandler(this.scanfilebtn_Click);
            // 
            // zonescan
            // 
            this.zonescan.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.zonescan.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.zonescan.Location = new System.Drawing.Point(259, 283);
            this.zonescan.Name = "zonescan";
            this.zonescan.Size = new System.Drawing.Size(122, 23);
            this.zonescan.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.zonescan.TabIndex = 14;
            this.zonescan.Text = "Zone Scan";
            this.zonescan.Click += new System.EventHandler(this.zonescan_Click);
            // 
            // quickscanbtn
            // 
            this.quickscanbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.quickscanbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.quickscanbtn.Location = new System.Drawing.Point(131, 283);
            this.quickscanbtn.Name = "quickscanbtn";
            this.quickscanbtn.Size = new System.Drawing.Size(122, 23);
            this.quickscanbtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.quickscanbtn.TabIndex = 13;
            this.quickscanbtn.Text = "Quick Scan";
            this.quickscanbtn.Click += new System.EventHandler(this.quickscanbtn_Click);
            // 
            // fullscanbtn
            // 
            this.fullscanbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.fullscanbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.fullscanbtn.Location = new System.Drawing.Point(3, 283);
            this.fullscanbtn.Name = "fullscanbtn";
            this.fullscanbtn.Size = new System.Drawing.Size(122, 23);
            this.fullscanbtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.fullscanbtn.TabIndex = 12;
            this.fullscanbtn.Text = "Full Scan";
            this.fullscanbtn.Click += new System.EventHandler(this.fullscanbtn_Click);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(527, 283);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(122, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 17;
            this.buttonX1.Text = "Identity Scan";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelX1.Location = new System.Drawing.Point(14, 22);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(523, 29);
            this.labelX1.TabIndex = 18;
            this.labelX1.Text = "Kavprot smart security scan";
            // 
            // pictureBox5
            // 
            this.pictureBox5.ForeColor = System.Drawing.Color.Black;
            this.pictureBox5.Image = global::Kavprot.Properties.Resources.identity;
            this.pictureBox5.Location = new System.Drawing.Point(527, 119);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(122, 126);
            this.pictureBox5.TabIndex = 16;
            this.pictureBox5.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.ForeColor = System.Drawing.Color.Black;
            this.pictureBox4.Image = global::Kavprot.Properties.Resources.quick_scan;
            this.pictureBox4.Location = new System.Drawing.Point(131, 119);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(122, 126);
            this.pictureBox4.TabIndex = 11;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.ForeColor = System.Drawing.Color.Black;
            this.pictureBox3.Image = global::Kavprot.Properties.Resources.scanfile;
            this.pictureBox3.Location = new System.Drawing.Point(387, 119);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(122, 126);
            this.pictureBox3.TabIndex = 10;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.ForeColor = System.Drawing.Color.Black;
            this.pictureBox2.Image = global::Kavprot.Properties.Resources.zonescan;
            this.pictureBox2.Location = new System.Drawing.Point(259, 119);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(122, 126);
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ForeColor = System.Drawing.Color.Black;
            this.pictureBox1.Image = global::Kavprot.Properties.Resources.deep_scan;
            this.pictureBox1.Location = new System.Drawing.Point(3, 119);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(122, 126);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // ScanTypeFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.scanfilebtn);
            this.Controls.Add(this.zonescan);
            this.Controls.Add(this.quickscanbtn);
            this.Controls.Add(this.fullscanbtn);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ScanTypeFrm";
            this.Size = new System.Drawing.Size(766, 406);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX scanfilebtn;
        private DevComponents.DotNetBar.ButtonX zonescan;
        private DevComponents.DotNetBar.ButtonX quickscanbtn;
        private DevComponents.DotNetBar.ButtonX fullscanbtn;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private System.Windows.Forms.PictureBox pictureBox5;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}
