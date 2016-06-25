using System;
using System.Collections.Generic;
using System.Text;
using KAVE.BaseEngine;
using System.Windows.Forms;
using KAVE.Monitors;
using KAVE.Engine;
using System.IO;
using System.Diagnostics;

namespace KAVE
{
    public enum KavprotInitialization
    {
        Engine,
        Full
    }
    public delegate void AsyncInvoke();
   public static class KavprotManager
    {
       public static bool Protected = false;
       public static string LastThreat = "no threat detected";
       public static void Initialize(KavprotInitialization init)
       {
           try
           {
                // init settings
              SettingsManager.Initialize();
              if (SettingsManager.TurboMode)
              {
               AsyncInvoke ainv = new AsyncInvoke(KavprotVoice.Initialize);
               ainv.BeginInvoke(null, null);    
              // Activation.Initialize();
               //if (!Activation.Expired)
               //{
                   if (init == KavprotInitialization.Full)
                   {
                       // init monitors
                     
                           AsyncInvoke inv = new AsyncInvoke(InitMonitors);
                           inv.BeginInvoke(null, null);
                   
                     
                       // init engine
                       AVEngine.Initialize(SettingsManager.Scansense);
                   
                
                       if (SettingsManager.KavprotRemoteControl)
                       {
                           AsyncInvoke dinv = new AsyncInvoke( KavprotRemoteControl.Init);
                           dinv.BeginInvoke(null, null); 
                         
                            AsyncInvoke tinv = new AsyncInvoke(KavprotRemoteControl.ReceiveDataFromMobile);
                          tinv.BeginInvoke(null, null);  
                       }
                      
                       AntivirusState.SetProtection(true);

                   }
                   else
                   {
                      
                       // init engine
                       AVEngine.Initialize(SettingsManager.Scansense);

                   }
               //}
               //else
               //{
               //    MessageBox.Show("Kavprot will be closed after you click ok", "Activation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               //    ShutDown();
               //}
           }
           else{
               KavprotVoice.Initialize();
          
               //Activation.Initialize();
               //if (!Activation.Expired)
               //{
                   if (init == KavprotInitialization.Full)
                   {
                       // init monitors
                       InitMonitors();
                       // init engine
                       AVEngine.Initialize(SettingsManager.Scansense);
                       if (SettingsManager.KavprotRemoteControl)
                       {
                          KavprotRemoteControl.Init();
                           AsyncInvoke inv = new AsyncInvoke(KavprotRemoteControl.ReceiveDataFromMobile);
                           inv.BeginInvoke(null, null);
                       }

                       AntivirusState.SetProtection(true);

                   }
                   else
                   {

                       // init engine
                       AVEngine.Initialize(SettingsManager.Scansense);

                   }
               //}
               //else
               //{
               //    MessageBox.Show("Kavprot will be closed after you click ok", "Activation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               //    ShutDown();
               //}
           }
           }
           catch
           {

           }
       }
       public static void InitMonitors()
       {

           try
           {

               if (SettingsManager.WebAgent)
               {

                   WebMonitor.Initialize();
                   WebMonitor.Start();
               }
               if (SettingsManager.Firewall)
               {
                   Firewall.Init();
                   Firewall.AccessDenied += new DenyRule(Firewall_AccessDenied);
                   Firewall.Start();
               }

               if (SettingsManager.NIDS)
               {
                   NetworkMonitor.Initialize(SettingsManager.BrekleyFilter);
                   NetworkMonitor.Start();
               }
               if (SettingsManager.SystemMonitor)
               {
                  FileSystemMonitor.Initialize(SettingsManager.HighSense);
                  FileSystemMonitor.Start();
               }


               DriveDetector detect = new DriveDetector();
               detect.DeviceArrived += new DriveDetectorEventHandler(detect_DeviceArrived);

               Protected = true;
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }
      
       static void detect_DeviceArrived(object sender, DriveDetectorEventArgs e)
       {
          
           DriveInfo drv = new DriveInfo(e.Drive);
           if (!SettingsManager.Silence)
           {
               if (Alert.NewDrive(drv) == DevComponents.DotNetBar.eTaskDialogResult.Yes)
               {
                   ScanForm frm = new ScanForm(ScanType.Zone, drv.Name);
                   frm.Show();

               }
           }
           else
           {
               if (SettingsManager.HighSense)
               {
                   ScanForm frm = new ScanForm(ScanType.Zone, drv.Name);
                   frm.Show();
                   frm.quickscanbtn.Enabled = false;
                   frm.cancelquickscan.Enabled = true;
                   frm.fullscanlist.Items.Clear();
                   frm.scanworker.RunWorkerAsync();
               }
           }
       }
       static void Firewall_AccessDenied(string username,string app, string protocol, string source, string destination, string direction)
       {
           try
           {
               if (!Firewall.Apps.Contains(app))
               {
                   if (!SettingsManager.Silence)
                   {
                       Firewall.Apps.Add(app);
                       KavprotVoice.SpeakAsync("Would you like to allow this network access");
                       DevComponents.DotNetBar.TaskDialogInfo inf = new DevComponents.DotNetBar.TaskDialogInfo();
                       inf.DialogButtons = DevComponents.DotNetBar.eTaskDialogButton.Yes | DevComponents.DotNetBar.eTaskDialogButton.No;

                       inf.Title = "Firewall Rule";
                       inf.Text = "An application is trying to connect to a remote host (" + destination + ") via " + protocol + " protocol. \n " + Path.GetFileName(app) + "\n do you want to authorize this connection?";
                       inf.TaskDialogIcon = DevComponents.DotNetBar.eTaskDialogIcon.Exclamation;
                       inf.Header = "Application Connection";
                       inf.FooterText = "Kavprot smart security";
                       inf.DialogColor = DevComponents.DotNetBar.eTaskDialogBackgroundColor.Silver;

                       DevComponents.DotNetBar.eTaskDialogResult dl = DevComponents.DotNetBar.TaskDialog.Show(inf);

                       if (dl == DevComponents.DotNetBar.eTaskDialogResult.Yes)
                           Firewall.Add("AllowAll", app);
                       else
                           Firewall.Add("DenyAll", app);
                   }
                   else
                   {
                       Firewall.Apps.Add(app);
                       if(!Scanner.CheckReputation(app))
                           Firewall.Add("AllowAll", app);
                       else
                           Firewall.Add("DenyAll", app);
                     
                   }
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
       public static void Start()
       {
           try
           {
               if (SettingsManager.Firewall)
                   Firewall.Start();
               if (SettingsManager.NIDS)
                   NetworkMonitor.Start();

               if (SettingsManager.SystemMonitor)
                   FileSystemMonitor.Start();

               if (SettingsManager.WebAgent)
                   WebMonitor.Start();


               Protected = true;
               AntivirusState.SetProtection(true);
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }
       public static void Stop()
       {
           try
           {
               if (SettingsManager.Firewall)
                   Firewall.Stop();
               
               if (SettingsManager.NIDS)
                   NetworkMonitor.Stop();

               if (SettingsManager.SystemMonitor)
                   FileSystemMonitor.Stop();

               if (SettingsManager.WebAgent)
                   WebMonitor.Stop();
               Protected = false;
               AntivirusState.SetProtection(false);
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }
       public static void Restart()
       {
           try
           {

               Stop();

               AntivirusState.SetProtection(false);
               Process.Start(Application.ExecutablePath);
               Process.GetCurrentProcess().Kill();
           
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }
       public static void ShutDown()
       {
           try
           {
             
               Stop();

                  AntivirusState.SetProtection(false);
               Process.GetCurrentProcess().Kill();
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }
    }
}
