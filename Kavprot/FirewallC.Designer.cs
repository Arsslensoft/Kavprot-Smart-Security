namespace Kavprot
{
    partial class FirewallC
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
            this.addbtn = new DevComponents.DotNetBar.ButtonX();
            this.remvbtn = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.filetxt = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.switchButton1 = new DevComponents.DotNetBar.Controls.SwitchButton();
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
            this.itemPanel1.TabIndex = 1;
            this.itemPanel1.Text = "itemPanel1";
            // 
            // addbtn
            // 
            this.addbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.addbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.addbtn.Location = new System.Drawing.Point(350, 332);
            this.addbtn.Name = "addbtn";
            this.addbtn.Size = new System.Drawing.Size(107, 28);
            this.addbtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.addbtn.TabIndex = 4;
            this.addbtn.Text = "Add";
            this.addbtn.Click += new System.EventHandler(this.addbtn_Click);
            // 
            // remvbtn
            // 
            this.remvbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.remvbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.remvbtn.Location = new System.Drawing.Point(483, 332);
            this.remvbtn.Name = "remvbtn";
            this.remvbtn.Size = new System.Drawing.Size(107, 28);
            this.remvbtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.remvbtn.TabIndex = 3;
            this.remvbtn.Text = "Remove";
            this.remvbtn.Click += new System.EventHandler(this.remvbtn_Click);
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelX1.Location = new System.Drawing.Point(16, 273);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(452, 23);
            this.labelX1.TabIndex = 12;
            this.labelX1.Text = "Applicaton path";
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
            this.filetxt.Location = new System.Drawing.Point(16, 302);
            this.filetxt.Name = "filetxt";
            this.filetxt.Size = new System.Drawing.Size(599, 20);
            this.filetxt.TabIndex = 11;
            // 
            // switchButton1
            // 
            // 
            // 
            // 
            this.switchButton1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.switchButton1.Location = new System.Drawing.Point(16, 338);
            this.switchButton1.Name = "switchButton1";
            this.switchButton1.OffText = "Deny";
            this.switchButton1.OffTextColor = System.Drawing.Color.Red;
            this.switchButton1.OnText = "Allow";
            this.switchButton1.OnTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.switchButton1.Size = new System.Drawing.Size(136, 22);
            this.switchButton1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.switchButton1.TabIndex = 13;
            this.switchButton1.Value = true;
            this.switchButton1.ValueObject = "Y";
            // 
            // FirewallC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.Controls.Add(this.switchButton1);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.filetxt);
            this.Controls.Add(this.addbtn);
            this.Controls.Add(this.remvbtn);
            this.Controls.Add(this.itemPanel1);
            this.Name = "FirewallC";
            this.Size = new System.Drawing.Size(766, 406);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ItemPanel itemPanel1;
        private DevComponents.DotNetBar.ButtonX addbtn;
        private DevComponents.DotNetBar.ButtonX remvbtn;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX filetxt;
        private DevComponents.DotNetBar.Controls.SwitchButton switchButton1;

     
    }
}
