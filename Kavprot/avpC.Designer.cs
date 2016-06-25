namespace Kavprot
{
    partial class AVPC
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
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
            this.remvbtn = new DevComponents.DotNetBar.ButtonX();
            this.addbtn = new DevComponents.DotNetBar.ButtonX();
            this.filetxt = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.delcheck = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.readcheck = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.wrcheck = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.qscheck = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.rencheck = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.accheck = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.textBoxX1 = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.SuspendLayout();
            // 
            // itemPanel1
            // 
            // 
            // 
            // 
            this.itemPanel1.BackgroundStyle.Class = "ItemPanel";
            this.itemPanel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemPanel1.ContainerControlProcessDialogKey = true;
            this.itemPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.itemPanel1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanel1.Location = new System.Drawing.Point(0, 0);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(766, 267);
            this.itemPanel1.TabIndex = 0;
            this.itemPanel1.Text = "itemPanel1";
            // 
            // remvbtn
            // 
            this.remvbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.remvbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.remvbtn.Location = new System.Drawing.Point(463, 362);
            this.remvbtn.Name = "remvbtn";
            this.remvbtn.Size = new System.Drawing.Size(107, 28);
            this.remvbtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.remvbtn.TabIndex = 1;
            this.remvbtn.Text = "Remove";
            this.remvbtn.Click += new System.EventHandler(this.remvbtn_Click);
            // 
            // addbtn
            // 
            this.addbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.addbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.addbtn.Location = new System.Drawing.Point(463, 328);
            this.addbtn.Name = "addbtn";
            this.addbtn.Size = new System.Drawing.Size(107, 28);
            this.addbtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.addbtn.TabIndex = 2;
            this.addbtn.Text = "Add";
            this.addbtn.Click += new System.EventHandler(this.addbtn_Click);
            // 
            // filetxt
            // 
            this.filetxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            // 
            // 
            // 
            this.filetxt.Border.Class = "TextBoxBorder";
            this.filetxt.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.filetxt.ForeColor = System.Drawing.Color.White;
            this.filetxt.Location = new System.Drawing.Point(19, 302);
            this.filetxt.Name = "filetxt";
            this.filetxt.Size = new System.Drawing.Size(416, 20);
            this.filetxt.TabIndex = 3;
            // 
            // delcheck
            // 
            // 
            // 
            // 
            this.delcheck.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.delcheck.Location = new System.Drawing.Point(19, 328);
            this.delcheck.Name = "delcheck";
            this.delcheck.Size = new System.Drawing.Size(100, 23);
            this.delcheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.delcheck.TabIndex = 4;
            this.delcheck.Text = "Allow Delete";
            this.delcheck.TextColor = System.Drawing.Color.White;
            // 
            // readcheck
            // 
            // 
            // 
            // 
            this.readcheck.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.readcheck.Location = new System.Drawing.Point(19, 365);
            this.readcheck.Name = "readcheck";
            this.readcheck.Size = new System.Drawing.Size(100, 23);
            this.readcheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.readcheck.TabIndex = 5;
            this.readcheck.Text = "Allow Read";
            this.readcheck.TextColor = System.Drawing.Color.White;
            // 
            // wrcheck
            // 
            // 
            // 
            // 
            this.wrcheck.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.wrcheck.Location = new System.Drawing.Point(147, 328);
            this.wrcheck.Name = "wrcheck";
            this.wrcheck.Size = new System.Drawing.Size(100, 23);
            this.wrcheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.wrcheck.TabIndex = 6;
            this.wrcheck.Text = "Allow Write";
            this.wrcheck.TextColor = System.Drawing.Color.White;
            // 
            // qscheck
            // 
            // 
            // 
            // 
            this.qscheck.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.qscheck.Location = new System.Drawing.Point(147, 370);
            this.qscheck.Name = "qscheck";
            this.qscheck.Size = new System.Drawing.Size(149, 23);
            this.qscheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.qscheck.TabIndex = 7;
            this.qscheck.Text = "Allow Query and Set";
            this.qscheck.TextColor = System.Drawing.Color.White;
            // 
            // rencheck
            // 
            // 
            // 
            // 
            this.rencheck.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.rencheck.Location = new System.Drawing.Point(335, 328);
            this.rencheck.Name = "rencheck";
            this.rencheck.Size = new System.Drawing.Size(100, 23);
            this.rencheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.rencheck.TabIndex = 8;
            this.rencheck.Text = "Allow Rename";
            this.rencheck.TextColor = System.Drawing.Color.White;
            // 
            // accheck
            // 
            // 
            // 
            // 
            this.accheck.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.accheck.Location = new System.Drawing.Point(335, 365);
            this.accheck.Name = "accheck";
            this.accheck.Size = new System.Drawing.Size(100, 23);
            this.accheck.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.accheck.TabIndex = 9;
            this.accheck.Text = "Allow Access";
            this.accheck.TextColor = System.Drawing.Color.White;
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelX1.Location = new System.Drawing.Point(19, 273);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(452, 23);
            this.labelX1.TabIndex = 10;
            this.labelX1.Text = "Directory path";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelX2.Location = new System.Drawing.Point(454, 273);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(97, 23);
            this.labelX2.TabIndex = 11;
            this.labelX2.Text = "Filter (Extension)";
            // 
            // textBoxX1
            // 
            this.textBoxX1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            // 
            // 
            // 
            this.textBoxX1.Border.Class = "TextBoxBorder";
            this.textBoxX1.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxX1.ForeColor = System.Drawing.Color.White;
            this.textBoxX1.Location = new System.Drawing.Point(452, 302);
            this.textBoxX1.Name = "textBoxX1";
            this.textBoxX1.Size = new System.Drawing.Size(143, 20);
            this.textBoxX1.TabIndex = 12;
            // 
            // AVPC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.Controls.Add(this.textBoxX1);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.accheck);
            this.Controls.Add(this.rencheck);
            this.Controls.Add(this.qscheck);
            this.Controls.Add(this.wrcheck);
            this.Controls.Add(this.readcheck);
            this.Controls.Add(this.delcheck);
            this.Controls.Add(this.filetxt);
            this.Controls.Add(this.addbtn);
            this.Controls.Add(this.remvbtn);
            this.Controls.Add(this.itemPanel1);
            this.Name = "AVPC";
            this.Size = new System.Drawing.Size(766, 406);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ItemPanel itemPanel1;
        private DevComponents.DotNetBar.ButtonX remvbtn;
        private DevComponents.DotNetBar.ButtonX addbtn;
        private DevComponents.DotNetBar.Controls.TextBoxX filetxt;
        private DevComponents.DotNetBar.Controls.CheckBoxX delcheck;
        private DevComponents.DotNetBar.Controls.CheckBoxX readcheck;
        private DevComponents.DotNetBar.Controls.CheckBoxX wrcheck;
        private DevComponents.DotNetBar.Controls.CheckBoxX qscheck;
        private DevComponents.DotNetBar.Controls.CheckBoxX rencheck;
        private DevComponents.DotNetBar.Controls.CheckBoxX accheck;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX1;
        private DevComponents.DotNetBar.LabelX labelX2;


    }
}
