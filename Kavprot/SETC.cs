using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using KAVE.Monitors;
using KAVE.BaseEngine;
using KAVE;
using System.Diagnostics;

namespace Kavprot
{
    public partial class SETC : UserControl
    {
        public SETC()
        {
            InitializeComponent();

            // web protection tab
            webprotcheck.Checked = SettingsManager.WebAgent;
            websmartcheck.Checked = SettingsManager.WebAgentSmartDetection;
            parentalcheck.Checked = SettingsManager.ParentalControl;
            bbcheck.Checked = SettingsManager.BlockUrls;
            hsfilter.Checked = SettingsManager.HighSenseFilter;
            filterdt.Checked = SettingsManager.FilterData;
            chttps.Checked = SettingsManager.CaptureHTTPS;
            dhttps.Checked = SettingsManager.DecryptHTTPS;
            Ice.Checked = SettingsManager.ICE;
            rcon.Checked = SettingsManager.RemoteConnection;
            strafficheck.Checked = SettingsManager.SaveTraffic;
            // ave tab
            hscheck.Checked = SettingsManager.HighSense;
            notxt.Text = SettingsManager.NoScannner;
            tstxt.Text = SettingsManager.ASCIIScannner;
            petxt.Text = SettingsManager.PEScannner;
            htxt.Text = SettingsManager.HashScannner;
            atxt.Text = SettingsManager.ARCHScannner;
       
            // RT TAB
            smbackupcheck.Checked = SettingsManager.SmartBackup;
            procheck.Checked = SettingsManager.SystemMonitor;
            sbstxt.Text = SettingsManager.SmartBackupSize.ToString();
            comboBox1.Text = SettingsManager.Scansense.ToString();
            sbetxt.Text = SettingsManager.SmartBackupListS;
            aspamcheck.Checked = SettingsManager.AntiSpam;

            // sandbox tab
            afcheck.Checked = SettingsManager.AccessFiles;
            afdlgcheck.Checked = SettingsManager.AccessFileDialog;
            aenvcheck.Checked = SettingsManager.AccessEnvironment;
            apcheck.Checked = SettingsManager.AccessPerformanceCounter;
            aevlogcheck.Checked = SettingsManager.AccessEventLog;
            aregcheck.Checked = SettingsManager.AccessRegistry;
            aguicheck.Checked = SettingsManager.AccessGUI;
            comboBox2.Text = SettingsManager.Security.ToString();

            // General TAB
            silencecheck.Checked = SettingsManager.Silence;
            vrpscheck.Checked = SettingsManager.VRPS;
            sbtxt.Text = SettingsManager.SandBoxPath;
            otscheck.Checked = SettingsManager.OneTimeScan;
            logincheck.Checked = SettingsManager.Login;
            turbocheck.Checked = SettingsManager.TurboMode;
            // net and priv TAB
            vfwcheck.Checked = SettingsManager.NIDS;
            vdbmpc.Text = SettingsManager.MaxPages.ToString();
            vdbps.Text = SettingsManager.PageSize.ToString();
            vdbc.Text = SettingsManager.CacheSize.ToString();
            rtsf.Text = SettingsManager.RTSFSTR;
            updatech.Text = SettingsManager.UpdateVDBEach.ToString();

            berkfiltxt.Text = SettingsManager.BrekleyFilter;

            fwcheck.Checked = SettingsManager.Firewall;
            sdcheck.Checked = SettingsManager.SelfDefense;
                    seatxt.Text = Encoding.UTF8.GetString(SettingsManager.SEAKey);
            mobadrtxt.Text = SettingsManager.MobileAdress;
            appadrtxt.Text = SettingsManager.ApplicationAdress;
            kaimlcheck.Checked = SettingsManager.KAI;
            rcheck.Checked = SettingsManager.KavprotRemoteControl;
            opguicheck.Checked = SettingsManager.OptimizeGUI;

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                WebMonitor.CleanWINETCache(filescheck.Checked, cookiescheck.Checked);
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();
            lst.Add("sysmon=" + Convert.ToString(procheck.Checked));
            lst.Add("hs=" + Convert.ToString(hscheck.Checked));
            lst.Add("vrps=" + Convert.ToString(vrpscheck.Checked));
            lst.Add("wa=" + Convert.ToString(webprotcheck.Checked));
            lst.Add("gm=" + Convert.ToString(silencecheck.Checked));
            lst.Add("ssense=" + comboBox1.Text);
            lst.Add("pc=" + Convert.ToString(parentalcheck.Checked));
            lst.Add("wasd=" + Convert.ToString(websmartcheck.Checked));
            lst.Add("ots=" + Convert.ToString(otscheck.Checked));
            lst.Add("log=" + Convert.ToString(logincheck.Checked));
            lst.Add("bb=" + Convert.ToString(bbcheck.Checked));
            lst.Add("nsl=" + notxt.Text);
            lst.Add(@"sdp=" + sbtxt.Text);
            lst.Add("archsl=" + atxt.Text);
            lst.Add("pesl=" + petxt.Text);
            lst.Add("asl=" + tstxt.Text);
            lst.Add("hsl=" + htxt.Text);
            lst.Add("sb=" + Convert.ToString(smbackupcheck.Checked));
            lst.Add("sbs=" + sbstxt.Text);
            lst.Add("sbe=" + sbetxt.Text);
     
            lst.Add("firewall=" + Convert.ToString(fwcheck.Checked));
        
            lst.Add("selfdefense=" + Convert.ToString(sdcheck.Checked));

            lst.Add("filter=" + Convert.ToString(filterdt.Checked));
            lst.Add("HSFILTER=" + Convert.ToString(hsfilter.Checked));

            lst.Add("ssec=" + comboBox2.Text);
            lst.Add("saf=" + Convert.ToString(this.afcheck.Checked));
            lst.Add("sareg=" + Convert.ToString(aregcheck.Checked));
            lst.Add("sagui=" + Convert.ToString(aguicheck.Checked));
            lst.Add("sadlg=" + Convert.ToString(afdlgcheck.Checked));
            lst.Add("saenv=" + Convert.ToString(aenvcheck.Checked));
            lst.Add("saevlog=" + Convert.ToString(aevlogcheck.Checked));
            lst.Add("sapc=" + Convert.ToString(apcheck.Checked));

            lst.Add(@"ICE=" + Convert.ToString(Ice.Checked));
            lst.Add(@"CHTTPS=" + Convert.ToString(chttps.Checked));
            lst.Add(@"DHTTPS=" + Convert.ToString(dhttps.Checked));
            lst.Add(@"RS=" + Convert.ToString(rcon.Checked));


            lst.Add(@"NIDS=" + Convert.ToString(vfwcheck.Checked));
            lst.Add(@"aspam=" + Convert.ToString(aspamcheck.Checked));

            lst.Add(@"turbo=" + Convert.ToString(turbocheck.Checked));
            lst.Add(@"straffic=" + Convert.ToString(strafficheck.Checked));


            lst.Add(@"cachesize=" + vdbc.Text);
            lst.Add(@"pagesize=" + vdbps.Text);
            lst.Add(@"maxpages=" + vdbmpc.Text);
            lst.Add(@"rtsf=" + rtsf.Text);
            lst.Add(@"update=" + updatech.Text);

           lst.Add(@"bpfilter=" + berkfiltxt.Text);

           lst.Add(@"appadr=" + appadrtxt.Text);
           lst.Add(@"mobadr="+mobadrtxt.Text);
           lst.Add("seakey=" + seatxt.Text);
           lst.Add(@"kai="+Convert.ToString(kaimlcheck.Checked));
           lst.Add(@"krc=" + Convert.ToString(rcheck.Checked));
           lst.Add(@"opgui=" + Convert.ToString(rcheck.Checked));
   
            SettingsManager.Write(Application.StartupPath + @"\Conf\Config.avcnf", lst);
            Forms.frm.hidethis = false;
            SingleInstance.Stop();
            KavprotManager.Restart();
        }

    }
}
