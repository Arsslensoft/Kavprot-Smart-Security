using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using System.Threading;
using KAVE;
using KAVE.Monitors;
using KAVE.BaseEngine;
using KAVE.Engine;
using System.Speech.Recognition;
using System.Diagnostics;

namespace Kavprot
{
    public partial class MainForm : DevComponents.DotNetBar.Metro.MetroAppForm
    {
        #region paint
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );
        Login logfrm;
        public MainForm()
        {
            if (SettingsManager.Login)
            {
                logfrm = new Login();
                logfrm.ShowDialog();
                if (logfrm.result != DialogResult.OK)
                {
                    hidethis = false;
                    KavprotManager.ShutDown();
                }
            }
          
            InitializeComponent();
            Forms.Initialize(this);
            if (!AntivirusState.Isregistred())
                   AntivirusState.RegisterAV();
         
           Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
           knvirlb.Text ="Known viruses : " +VDB.definitions.ToString();
           MainC m = (MainC)Forms.GetControl("main");
           Forms.frm.ShowModalPanel(m, DevComponents.DotNetBar.Controls.eSlideSide.Left);
           AVEngine.EventsManager.VDBUpdateCanceled += new EventHandler(EventsManager_VDBUpdateCanceled);
           AVEngine.EventsManager.Notify += new AlertNotify(EventsManager_Notify);
           AVEngine.EventsManager.VDBUpdateCompleted += new EventHandler(EventsManager_VDBUpdateCompleted);
           AVEngine.EventsManager.ScanCompleted += new EventHandler(EventsManager_ScanCompleted);
           AVEngine.EventsManager.Closing += new EventHandler(EventsManager_Closing);
           AVEngine.EventsManager.ScanFile += new ScanDEL(EventsManager_ScanFile);
       

         
        }
     
        private void MainForm_Resize(object sender, EventArgs e)
        {
           Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
#endregion

        protected override void WndProc(ref Message message)
        {

            if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                ShowWindow();
            }
            base.WndProc(ref message);
        }
        public void ShowWindow()
        {
            try
            {
                this.Show();
            }
            catch
            {

            }
        
        }
        private void metroShell1_HelpButtonClick(object sender, EventArgs e)
        {
            SETC s = (SETC)Forms.GetControl("settings");
            this.ShowModalPanel(s, DevComponents.DotNetBar.Controls.eSlideSide.Right);
        }
        private void metroShell1_SettingsButtonClick(object sender, EventArgs e)
        {
            MainC m = (MainC)Forms.GetControl("main");
            m.LoadStats();
            this.ShowModalPanel(m, DevComponents.DotNetBar.Controls.eSlideSide.Top);
         
        }
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {            
                if (sblocknet)
                     blocknet.Text = "Open Web Traffic";
                 else
                    blocknet.Text = "Block Web Traffic";
                
                bEditPopup.Displayed = false;
                bEditPopup.PopupMenu(MousePosition);
            }
        }
        private void aboutbtn_Click(object sender, EventArgs e)
        {
            About frm = (About)Forms.GetControl("about");
            this.ShowModalPanel(frm, DevComponents.DotNetBar.Controls.eSlideSide.Left);
        }
        private void avuibtn_Click(object sender, EventArgs e)
        {
            if (SettingsManager.Login)
            {
                logfrm = new Login();
                logfrm.ShowDialog();
                if (logfrm.result != DialogResult.OK)
                {
                    hidethis = false;
                    KavprotManager.ShutDown();
                }
            }
           this.Show();
        }
        private void protbtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (SettingsManager.Login)
                {
                    logfrm = new Login();
                    logfrm.ShowDialog();
                    if (logfrm.result != DialogResult.OK)
                    {
                        hidethis = false;
                        KavprotManager.ShutDown();
                    }
                }

                if (WebMonitor.IsRuning || Firewall.Runing || NetworkMonitor.Runing || FileSystemMonitor.Runing )
                {
                    protbtn.Text = "Enable Protection             ";
                    KavprotManager.Stop();
                    AntivirusState.SetProtection(false);
                    MainC m = (MainC)Forms.GetControl("main");
                    m.LoadStats();
                }
                else
                {

                    protbtn.Text = "Disable Protection             ";
                    KavprotManager.Start();
                    AntivirusState.SetProtection(true);
                    MainC m = (MainC)Forms.GetControl("main");
                    m.LoadStats();
                }
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
        bool sblocknet = false;
        private void blocknet_Click(object sender, EventArgs e)
        {
            try
            {
                if (sblocknet)
                {
                    WebMonitor.BlockNet = false;
                    sblocknet = false;
                }
                else
                {
                    WebMonitor.BlockNet = true;
                    sblocknet = true;
                }

            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
        private void qbtn_Click(object sender, EventArgs e)
        {
            if (SettingsManager.Login)
            {
                logfrm = new Login();
                logfrm.ShowDialog();
                if (logfrm.result != DialogResult.OK)
                {
                  hidethis = false;
                    KavprotManager.ShutDown();
                }
            }
            this.Show();
            Quarantinefrm frm = (Quarantinefrm)Forms.GetControl("quarantine");
            Forms.frm.ShowModalPanel(frm, DevComponents.DotNetBar.Controls.eSlideSide.Bottom);

        }
        private void rpbtn_Click(object sender, EventArgs e)
        {
            if (SettingsManager.Login)
            {
                logfrm = new Login();
                logfrm.ShowDialog();
                if (logfrm.result != DialogResult.OK)
                {
                    hidethis = false;
                    KavprotManager.ShutDown();
                }
            }
            this.Show();
            Repairfrm FRM = (Repairfrm)Forms.GetControl("repair");
            Forms.frm.ShowModalPanel(FRM, DevComponents.DotNetBar.Controls.eSlideSide.Left);
        }
        private void buttonItem1_Click(object sender, EventArgs e)
        {
            if (SettingsManager.Login)
            {
                logfrm = new Login();
                logfrm.ShowDialog();
                if (logfrm.result != DialogResult.OK)
                {
                    hidethis = false;
                    KavprotManager.ShutDown();
                }
            }
     
            this.Show();
            About c = (About)Forms.GetControl("about");
            Forms.frm.ShowModalPanel(c, DevComponents.DotNetBar.Controls.eSlideSide.Left);
        }
        internal bool hidethis = true;
        private void bSelectAll_Click(object sender, EventArgs e)
        {
            hidethis = false;
           KavprotManager.ShutDown();
        }
        private void licbtn_Click(object sender, EventArgs e)
        {
            Activationfrm a = (Activationfrm)Forms.GetControl("license");
            this.ShowModalPanel(a, DevComponents.DotNetBar.Controls.eSlideSide.Bottom);
        }

         private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                hidethis = false;
                KAVE.KavprotManager.Stop();
            }
            else if (e.CloseReason == CloseReason.TaskManagerClosing)
            {
                hidethis = false;
                KAVE.KavprotManager.Stop();
            }
            else
                if (hidethis)
                {

                    e.Cancel = true;
                    this.Hide();
                }
        }



         void EventsManager_ScanFile(string file)
         {
             ScanForm frm = new ScanForm(ScanType.File, file);
             frm.Show();
         }
         void EventsManager_Closing(object sender, EventArgs e)
         {
             hidethis = false;
         }
         void EventsManager_Notify(string title, string text, ToolTipIcon icon)
         {
             notifyIcon1.ShowBalloonTip(4000, title, text, icon);
         }
         void EventsManager_VDBUpdateCompleted(object sender, EventArgs e)
         {
             VDB.VDBDefinitions();
             notifyIcon1.ShowBalloonTip(4000, "Update Completed", "Kavprot smart security update completed successfully. (" + VDB.version.ToString() + ")", ToolTipIcon.Info);
         }
         void EventsManager_ScanCompleted(object sender, EventArgs e)
         {
             notifyIcon1.ShowBalloonTip(4000, "Scan Completed", "Kavprot smart security Scan completed successfully.", ToolTipIcon.Info);

         }
         void EventsManager_VDBUpdateCanceled(object sender, EventArgs e)
         {
             notifyIcon1.ShowBalloonTip(4000, "Update Canceled", "Kavprot smart security is unable to update VDB please check your internet connection.", ToolTipIcon.Warning);
             AntivirusState.SetUptodate(false);
         }

         private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
         {
             if (SettingsManager.Login)
             {
                 logfrm = new Login();
                 logfrm.ShowDialog();
                 if (logfrm.result != DialogResult.OK)
                 {
                     hidethis = false;
                     KavprotManager.ShutDown();
                 }
             }
            
             this.Show();
         }
         bool firstshow = true;

         private void MainForm_Shown(object sender, EventArgs e)
         {
             if (firstshow)
             {
                 firstshow = false;
                 Updatefrm u = (Updatefrm)Forms.GetControl("update");
                 Forms.frm.ShowModalPanel(u, DevComponents.DotNetBar.Controls.eSlideSide.Left);
                 u.backgroundWorker1.RunWorkerAsync();
                 timer1.Enabled = true;
             }
             else
                 firstshow = false;
             try
             {
                 if (SettingsManager.KAI)
                     Process.Start(Application.StartupPath + @"\KAIML.exe");
             }
             catch
             {
             }
         }

  
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Updatefrm u = (Updatefrm)Forms.GetControl("update");
            Forms.frm.ShowModalPanel(u, DevComponents.DotNetBar.Controls.eSlideSide.Left);
            u.backgroundWorker1.RunWorkerAsync();
            timer1.Enabled = true;
        }

        private void Helpbtn_Click(object sender, EventArgs e)
        {
            Updatefrm u = (Updatefrm)Forms.GetControl("update");
            Forms.frm.ShowModalPanel(u, DevComponents.DotNetBar.Controls.eSlideSide.Left);
        }

        private void updatebtn_Click(object sender, EventArgs e)
        {
            Updatefrm u = (Updatefrm)Forms.GetControl("update");
            Forms.frm.ShowModalPanel(u, DevComponents.DotNetBar.Controls.eSlideSide.Left);
            u.backgroundWorker1.RunWorkerAsync();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }
      
    }
}
