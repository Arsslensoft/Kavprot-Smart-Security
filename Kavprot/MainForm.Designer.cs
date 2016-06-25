namespace Kavprot
{
    partial class MainForm
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

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.metroShell1 = new DevComponents.DotNetBar.Metro.MetroShell();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.metroStatusBar1 = new DevComponents.DotNetBar.Metro.MetroStatusBar();
            this.Helpbtn = new DevComponents.DotNetBar.ButtonItem();
            this.aboutbtn = new DevComponents.DotNetBar.ButtonItem();
            this.licbtn = new DevComponents.DotNetBar.ButtonItem();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.knvirlb = new DevComponents.DotNetBar.LabelItem();
            this.sflb = new DevComponents.DotNetBar.LabelItem();
            this.swlb = new DevComponents.DotNetBar.LabelItem();
            this.metroTileItem2 = new DevComponents.DotNetBar.Metro.MetroTileItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.bEditPopup = new DevComponents.DotNetBar.ButtonItem();
            this.avuibtn = new DevComponents.DotNetBar.ButtonItem();
            this.protbtn = new DevComponents.DotNetBar.ButtonItem();
            this.updatebtn = new DevComponents.DotNetBar.ButtonItem();
            this.blocknet = new DevComponents.DotNetBar.ButtonItem();
            this.qbtn = new DevComponents.DotNetBar.ButtonItem();
            this.rpbtn = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.bSelectAll = new DevComponents.DotNetBar.ButtonItem();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // metroShell1
            // 
            this.metroShell1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            // 
            // 
            // 
            this.metroShell1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroShell1.CaptionVisible = true;
            this.metroShell1.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroShell1.ForeColor = System.Drawing.Color.White;
            this.metroShell1.HelpButtonText = "Settings";
            this.metroShell1.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this.metroShell1.Location = new System.Drawing.Point(1, 1);
            this.metroShell1.Name = "metroShell1";
            this.metroShell1.SettingsButtonText = "Back";
            this.metroShell1.Size = new System.Drawing.Size(771, 28);
            this.metroShell1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.metroShell1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.metroShell1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.metroShell1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.metroShell1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.metroShell1.SystemText.QatDialogAddButton = "&Add >>";
            this.metroShell1.SystemText.QatDialogCancelButton = "Cancel";
            this.metroShell1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.metroShell1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.metroShell1.SystemText.QatDialogOkButton = "OK";
            this.metroShell1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatDialogRemoveButton = "&Remove";
            this.metroShell1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.metroShell1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.metroShell1.TabIndex = 0;
            this.metroShell1.TabStripFont = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroShell1.Text = "metroShell1";
            this.metroShell1.SettingsButtonClick += new System.EventHandler(this.metroShell1_SettingsButtonClick);
            this.metroShell1.HelpButtonClick += new System.EventHandler(this.metroShell1_HelpButtonClick);
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70))))), System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(120)))), ((int)(((byte)(143))))));
            // 
            // metroStatusBar1
            // 
            this.metroStatusBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            // 
            // 
            // 
            this.metroStatusBar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroStatusBar1.ContainerControlProcessDialogKey = true;
            this.metroStatusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.metroStatusBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroStatusBar1.ForeColor = System.Drawing.Color.White;
            this.metroStatusBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.Helpbtn,
            this.aboutbtn,
            this.licbtn,
            this.labelItem1,
            this.knvirlb,
            this.sflb,
            this.swlb});
            this.metroStatusBar1.Location = new System.Drawing.Point(1, 438);
            this.metroStatusBar1.Name = "metroStatusBar1";
            this.metroStatusBar1.ResizeHandleVisible = false;
            this.metroStatusBar1.Size = new System.Drawing.Size(771, 26);
            this.metroStatusBar1.TabIndex = 1;
            this.metroStatusBar1.Text = "metroStatusBar1";
            // 
            // Helpbtn
            // 
            this.Helpbtn.Name = "Helpbtn";
            this.Helpbtn.Text = "Update";
            this.Helpbtn.Click += new System.EventHandler(this.Helpbtn_Click);
            // 
            // aboutbtn
            // 
            this.aboutbtn.Name = "aboutbtn";
            this.aboutbtn.Text = "About";
            this.aboutbtn.Click += new System.EventHandler(this.aboutbtn_Click);
            // 
            // licbtn
            // 
            this.licbtn.Name = "licbtn";
            this.licbtn.Text = "License ";
            this.licbtn.Click += new System.EventHandler(this.licbtn_Click);
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Stretch = true;
            // 
            // knvirlb
            // 
            this.knvirlb.Name = "knvirlb";
            this.knvirlb.Text = "Known viruses";
            // 
            // sflb
            // 
            this.sflb.Name = "sflb";
            this.sflb.Text = "SF";
            // 
            // swlb
            // 
            this.swlb.Name = "swlb";
            this.swlb.Text = "SW";
            // 
            // metroTileItem2
            // 
            this.metroTileItem2.Name = "metroTileItem2";
            this.metroTileItem2.TileColor = DevComponents.DotNetBar.Metro.eMetroTileColor.Default;
            // 
            // 
            // 
            this.metroTileItem2.TileStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // timer1
            // 
            this.timer1.Interval = 10800000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Kavprot smart security";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.contextMenuBar1.ForeColor = System.Drawing.Color.White;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.bEditPopup});
            this.contextMenuBar1.Location = new System.Drawing.Point(309, 231);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(150, 25);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 16;
            this.contextMenuBar1.TabStop = false;
            // 
            // bEditPopup
            // 
            this.bEditPopup.AutoExpandOnClick = true;
            this.bEditPopup.GlobalName = "bEditPopup";
            this.bEditPopup.Name = "bEditPopup";
            this.bEditPopup.PopupAnimation = DevComponents.DotNetBar.ePopupAnimation.SystemDefault;
            this.bEditPopup.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.avuibtn,
            this.protbtn,
            this.updatebtn,
            this.blocknet,
            this.qbtn,
            this.rpbtn,
            this.buttonItem1,
            this.bSelectAll});
            this.bEditPopup.Text = "bEditPopup";
            // 
            // avuibtn
            // 
            this.avuibtn.Name = "avuibtn";
            this.avuibtn.Text = "Open user interface of Kavprot";
            this.avuibtn.Click += new System.EventHandler(this.avuibtn_Click);
            // 
            // protbtn
            // 
            this.protbtn.BeginGroup = true;
            this.protbtn.GlobalName = "protbtn";
            this.protbtn.ImageIndex = 5;
            this.protbtn.Name = "protbtn";
            this.protbtn.PopupAnimation = DevComponents.DotNetBar.ePopupAnimation.SystemDefault;
            this.protbtn.Text = "Disable Protection        ";
            this.protbtn.Click += new System.EventHandler(this.protbtn_Click);
            // 
            // updatebtn
            // 
            this.updatebtn.Name = "updatebtn";
            this.updatebtn.Text = "Update";
            this.updatebtn.Click += new System.EventHandler(this.updatebtn_Click);
            // 
            // blocknet
            // 
            this.blocknet.Name = "blocknet";
            this.blocknet.Text = "Block Web Traffic";
            this.blocknet.Click += new System.EventHandler(this.blocknet_Click);
            // 
            // qbtn
            // 
            this.qbtn.GlobalName = "qbtn";
            this.qbtn.ImageIndex = 4;
            this.qbtn.Name = "qbtn";
            this.qbtn.PopupAnimation = DevComponents.DotNetBar.ePopupAnimation.SystemDefault;
            this.qbtn.Text = "Quarantine";
            this.qbtn.Click += new System.EventHandler(this.qbtn_Click);
            // 
            // rpbtn
            // 
            this.rpbtn.GlobalName = "rpbtn";
            this.rpbtn.ImageIndex = 12;
            this.rpbtn.Name = "rpbtn";
            this.rpbtn.PopupAnimation = DevComponents.DotNetBar.ePopupAnimation.SystemDefault;
            this.rpbtn.Text = "Repair";
            this.rpbtn.Click += new System.EventHandler(this.rpbtn_Click);
            // 
            // buttonItem1
            // 
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "About";
            this.buttonItem1.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // bSelectAll
            // 
            this.bSelectAll.BeginGroup = true;
            this.bSelectAll.GlobalName = "bSelectAll";
            this.bSelectAll.Name = "bSelectAll";
            this.bSelectAll.PopupAnimation = DevComponents.DotNetBar.ePopupAnimation.SystemDefault;
            this.bSelectAll.Text = "Exit";
            this.bSelectAll.Click += new System.EventHandler(this.bSelectAll_Click);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 60000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.ClientSize = new System.Drawing.Size(773, 465);
            this.Controls.Add(this.contextMenuBar1);
            this.Controls.Add(this.metroStatusBar1);
            this.Controls.Add(this.metroShell1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.ModalPanelBoundsExcludeStatusBar = true;
            this.Name = "MainForm";
            this.Sizable = false;
            this.Text = "Kavprot smart security";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Metro.MetroShell metroShell1;
        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.ButtonItem Helpbtn;
        private DevComponents.DotNetBar.ButtonItem aboutbtn;
        private DevComponents.DotNetBar.ButtonItem licbtn;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.Metro.MetroTileItem metroTileItem2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        internal DevComponents.DotNetBar.ButtonItem bEditPopup;
        internal DevComponents.DotNetBar.ButtonItem avuibtn;
        internal DevComponents.DotNetBar.ButtonItem protbtn;
        private DevComponents.DotNetBar.ButtonItem blocknet;
        internal DevComponents.DotNetBar.ButtonItem qbtn;
        internal DevComponents.DotNetBar.ButtonItem rpbtn;
        internal DevComponents.DotNetBar.ButtonItem buttonItem1;
        internal DevComponents.DotNetBar.ButtonItem bSelectAll;
        internal DevComponents.DotNetBar.LabelItem sflb;
        internal DevComponents.DotNetBar.LabelItem swlb;
        private DevComponents.DotNetBar.LabelItem knvirlb;
        private DevComponents.DotNetBar.ButtonItem updatebtn;
        internal DevComponents.DotNetBar.Metro.MetroStatusBar metroStatusBar1;
        private System.Windows.Forms.Timer timer2;



    }
}

