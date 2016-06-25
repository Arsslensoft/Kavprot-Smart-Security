namespace KAVE
{
    partial class ScanForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanForm));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.fullscanlist = new DevComponents.DotNetBar.ItemPanel();
            this.scanworker = new System.ComponentModel.BackgroundWorker();
            this.curfilequickscan = new System.Windows.Forms.Label();
            this.progressQuickscan = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.quickscanbtn = new DevComponents.DotNetBar.ButtonX();
            this.cancelquickscan = new DevComponents.DotNetBar.ButtonX();
            this.panelEx5 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx1.SuspendLayout();
            this.panelEx5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.fullscanlist);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(873, 343);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // fullscanlist
            // 
            // 
            // 
            // 
            this.fullscanlist.BackgroundStyle.Class = "ItemPanel";
            this.fullscanlist.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.fullscanlist.ContainerControlProcessDialogKey = true;
            this.fullscanlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fullscanlist.ForeColor = System.Drawing.Color.White;
            this.fullscanlist.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.fullscanlist.Location = new System.Drawing.Point(0, 0);
            this.fullscanlist.MultiLine = true;
            this.fullscanlist.Name = "fullscanlist";
            this.fullscanlist.Size = new System.Drawing.Size(873, 343);
            this.fullscanlist.TabIndex = 2;
            this.fullscanlist.Text = "itemPanel1";
            this.fullscanlist.ItemClick += new System.EventHandler(this.fullscanlist_ItemClick);
            this.fullscanlist.ItemAdded += new System.EventHandler(this.fullscanlist_ItemAdded);
            // 
            // scanworker
            // 
            this.scanworker.WorkerSupportsCancellation = true;
            this.scanworker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.scanworker_DoWork);
            this.scanworker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.scanworker_RunWorkerCompleted);
            // 
            // curfilequickscan
            // 
            this.curfilequickscan.AutoSize = true;
            this.curfilequickscan.ForeColor = System.Drawing.Color.White;
            this.curfilequickscan.Location = new System.Drawing.Point(22, 21);
            this.curfilequickscan.Name = "curfilequickscan";
            this.curfilequickscan.Size = new System.Drawing.Size(30, 13);
            this.curfilequickscan.TabIndex = 0;
            this.curfilequickscan.Text = "scan";
            // 
            // progressQuickscan
            // 
            // 
            // 
            // 
            this.progressQuickscan.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.progressQuickscan.ForeColor = System.Drawing.Color.White;
            this.progressQuickscan.Location = new System.Drawing.Point(12, 47);
            this.progressQuickscan.Name = "progressQuickscan";
            this.progressQuickscan.Size = new System.Drawing.Size(833, 18);
            this.progressQuickscan.TabIndex = 1;
            this.progressQuickscan.Text = "progressBarX3";
            // 
            // quickscanbtn
            // 
            this.quickscanbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.quickscanbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.quickscanbtn.Location = new System.Drawing.Point(646, 78);
            this.quickscanbtn.Name = "quickscanbtn";
            this.quickscanbtn.Size = new System.Drawing.Size(96, 25);
            this.quickscanbtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.quickscanbtn.TabIndex = 2;
            this.quickscanbtn.Text = "Scan";
            this.quickscanbtn.Click += new System.EventHandler(this.quickscanbtn_Click);
            // 
            // cancelquickscan
            // 
            this.cancelquickscan.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cancelquickscan.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cancelquickscan.Enabled = false;
            this.cancelquickscan.Location = new System.Drawing.Point(748, 78);
            this.cancelquickscan.Name = "cancelquickscan";
            this.cancelquickscan.Size = new System.Drawing.Size(96, 25);
            this.cancelquickscan.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cancelquickscan.TabIndex = 3;
            this.cancelquickscan.Text = "Cancel";
            this.cancelquickscan.Click += new System.EventHandler(this.cancelquickscan_Click);
            // 
            // panelEx5
            // 
            this.panelEx5.CanvasColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelEx5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx5.Controls.Add(this.cancelquickscan);
            this.panelEx5.Controls.Add(this.quickscanbtn);
            this.panelEx5.Controls.Add(this.progressQuickscan);
            this.panelEx5.Controls.Add(this.curfilequickscan);
            this.panelEx5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx5.Location = new System.Drawing.Point(0, 234);
            this.panelEx5.Name = "panelEx5";
            this.panelEx5.Size = new System.Drawing.Size(873, 109);
            this.panelEx5.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx5.Style.BackColor1.Color = System.Drawing.Color.Transparent;
            this.panelEx5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx5.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx5.Style.GradientAngle = 90;
            this.panelEx5.TabIndex = 3;
            // 
            // ScanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(873, 343);
            this.Controls.Add(this.panelEx5);
            this.Controls.Add(this.panelEx1);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ScanForm";
            this.Text = "Kavprot smart security scan";
            this.Load += new System.EventHandler(this.ScanForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScanForm_FormClosing);
            this.panelEx1.ResumeLayout(false);
            this.panelEx5.ResumeLayout(false);
            this.panelEx5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.Label curfilequickscan;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressQuickscan;
        private DevComponents.DotNetBar.PanelEx panelEx5;
        internal System.ComponentModel.BackgroundWorker scanworker;
        internal DevComponents.DotNetBar.ItemPanel fullscanlist;
        internal DevComponents.DotNetBar.ButtonX quickscanbtn;
        internal DevComponents.DotNetBar.ButtonX cancelquickscan;
    }
}