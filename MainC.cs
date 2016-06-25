using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using KAVE.Engine;
using KAVE;
using KAVE.BaseEngine;
using DevComponents.DotNetBar;
using System.Diagnostics;

namespace Kavprot
{
    public partial class MainC : UserControl
    {
        public MainC()
        {
            InitializeComponent();
            this.userlb.Text = "<div align=\"right\"><font size=\"+4\">" + Activation.User + "</font><br/></div>";
            LoadStats();
            AVEngine.EventsManager.FileChanged += new EventHandler(pfrm_PSLChanged);
            AVEngine.EventsManager.WebChanged += new EventHandler(wprot_WSLChanged);
            
        }
        int scannedfiles = 0;
        void pfrm_PSLChanged(object sender, EventArgs e)
        {
            try
            {
                scannedfiles++;
               Forms.frm.sflb.Text = "SF : " + scannedfiles;
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
        }
        int scannedwebsites = 0;
        void wprot_WSLChanged(object sender, EventArgs e)
        {
            try
            {
                scannedwebsites++;
                Forms.frm.swlb.Text = "SW : " + scannedwebsites;
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
        }
        internal void LoadStats()
        {
            if (!KavprotManager.Protected)
            {
                protstat.Text = "Computer is unprotected";
                protstat.ForeColor = Color.Silver;
                labelX1.Text = "Protection : Disabled";
                labelX1.ForeColor = Color.Silver;
                protimgbox.Image = Kavprot.Properties.Resources.monitor_red;

            }
            else if (!(SettingsManager.SystemMonitor && SettingsManager.WebAgent && SettingsManager.NIDS && SettingsManager.Firewall))
            {
                protstat.Text = "Computer is partially protected";
                protstat.ForeColor = Color.Orange;
                labelX1.Text = "Protection : Partially protected";
                labelX1.ForeColor = Color.Orange;
                protimgbox.Image = Kavprot.Properties.Resources.monitor_red;

            }
            else
            {
                protstat.Text = "Computer is protected";
                protstat.ForeColor = Color.White;
                labelX1.Text = "Protection : Active";
                labelX1.ForeColor = Color.White;
                protimgbox.Image = Kavprot.Properties.Resources.monitor_green;

            }

            threatlb.Text = "Threats : " + KavprotManager.LastThreat;
            labelX2.Text = "Databases : " + VDB.version;

         TimeSpan ts = Activation.Expiration.Subtract(DateTime.Now);
         labelX3.Text = "License : " + Math.Abs(ts.Days) + " Days " + Math.Abs(ts.Hours) + " Hours " + Math.Abs(ts.Minutes) + " Minutes";
        }
        void Updatewsc(string text, LabelX CurFile)
        {
            CurFile.Text = text;

        }

        private void avpbtn_Click(object sender, EventArgs e)
        {
            AVPC a = (AVPC)Forms.GetControl("avp");
            a.SLoad();
            Forms.frm.ShowModalPanel(a, DevComponents.DotNetBar.Controls.eSlideSide.Right);
        }

        private void sfgbtn_Click(object sender, EventArgs e)
        {
            FirewallC a = (FirewallC)Forms.GetControl("firewall");
            a.SLoad();
            Forms.frm.ShowModalPanel(a, DevComponents.DotNetBar.Controls.eSlideSide.Left);
        }

        private void csbtn_Click(object sender, EventArgs e)
        {
            ScanTypeFrm a = (ScanTypeFrm)Forms.GetControl("scan");
            Forms.frm.ShowModalPanel(a, DevComponents.DotNetBar.Controls.eSlideSide.Bottom);
        }

        private void fdoctbtn_Click(object sender, EventArgs e)
        {
            Utilities a = (Utilities)Forms.GetControl("utils");
            Forms.frm.ShowModalPanel(a, DevComponents.DotNetBar.Controls.eSlideSide.Top);
        }

        private void vqbtn_Click(object sender, EventArgs e)
        {
            Quarantinefrm a = (Quarantinefrm)Forms.GetControl("quarantine");
            Forms.frm.ShowModalPanel(a, DevComponents.DotNetBar.Controls.eSlideSide.Left);
        }

        private void settingbtn_Click(object sender, EventArgs e)
        {
            //if (Process.GetProcessesByName("KAI").Length == 0)
            //    Process.Start(Application.StartupPath + @"\KAI.exe");
              
        }

        private void logbtn_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + @"\Logs\Errors.txt");
        }

        private void sndbtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
                Process.Start(Application.StartupPath + @"\SandBox.exe", "\"" + openFileDialog2.FileName + "\"");
        
        }


    }
}
