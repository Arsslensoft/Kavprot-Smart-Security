using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using KAVE.Engine;
using KAVE.BaseEngine.Classes;

namespace KAVE.BaseEngine
{
   public static class Alert
    {
        public static void InfectedByMany(string infections, string filename)
        {
            if (SettingsManager.Silence)
            {
                Quarantine.Store(filename, infections);
            }
            else
            {
             
                Virus vi = new Virus(infections,filename,AVEngine.NothingScanner);
              
                InfectionFrm frm = new InfectionFrm(vi, true);
                frm.ShowDialog();
            }
        }
      internal  static void LearnDefinitin(string filename, string infections)
        {
            try
            {
                Dictionary<string, string> db = new Dictionary<string, string>();
                VDBT vdt = VirusDBTeacher.GetSignatures(filename, true, infections);
                switch (vdt.SIGID)
                {
                    case "PES":
                        if (vdt.TEXTHASH.Length > 4)
                            db.Add(vdt.TEXTHASH, vdt.VirusName);

                        if (vdt.DATAHASH.Length > 4)
                            db.Add(vdt.DATAHASH, vdt.VirusName);

                        VDB.AddKeys(db, DBT.PEMD5);

                        break;
                    case "ARS":
                        if (vdt.FILEHASH.Length > 4)
                            db.Add(vdt.FILEHASH, vdt.VirusName);

                        VDB.AddKeys(db, DBT.HDB);
                        break;
                    case "HAS":
                        if (vdt.FILEHASH.Length > 4)
                            db.Add(vdt.FILEHASH, vdt.VirusName);

                        VDB.AddKeys(db, DBT.HDB);
                        break;
                    case "ASC":
                        if (vdt.FILEHASH.Length > 4)
                            db.Add(vdt.FILEHASH, vdt.VirusName);

                        if (vdt.FILESOURCE.Length > 4)
                            db.Add(vdt.FILESOURCE, vdt.VirusName);

                        VDB.AddKeys(db, DBT.SDB);
                        break;

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
  
       public static void Infected(Virus infection)
       {
           KavprotVoice.SpeakAsync("This file was infected by " + infection.Name);
           AVEngine.EventsManager.CallVirusDetected();
           KavprotRemoteControl.SendPacket(SettingsManager.ApplicationAdress, SettingsManager.MobileAdress, KavprotRemoteControl.BuildARCPacket("SHOWTEXT", "ALDATA", SettingsManager.ApplicationAdress, "0005", Encoding.UTF8.GetBytes("File name : " + infection.Location + " \r\n Infection : " + infection.Name)));
           KavprotManager.LastThreat = infection.Name;
          if (SettingsManager.Silence)
           {
               if (infection.Scanner.Name == "HASH-TYPE-SCANNER")
               {
                   Alert.Attack("Virus Detected " + infection.Name, "Kavprot smart security detected a virus by the Hash scan system (" +infection.Location+"). sometimes false-positive alert can happen by any antivirus software for more informations visit : http://www.arsslensoft.tk/AVL/FPAlert.html", ToolTipIcon.Warning, true);
               }
               else
               {
                   Quarantine.Store(infection.Location, infection.Name);
               }
           }
           else
           {
                   if (infection.Name != "KavProtSensor.UnPackableArchive")
                   {
                       if (SettingsManager.Silence)
                       {
                           if (infection.Location.Contains(Environment.SystemDirectory))
                           {
                               AVEngine.AlertVirus();
                               InfectionFrm inf = new InfectionFrm(infection);
                               inf.ShowDialog();
                           }
                           else
                           {
                               Quarantine.Store(infection.Location, infection.Name);
                           }
                       }
                       else
                       {
                           AVEngine.AlertVirus();
                           InfectionFrm inf = new InfectionFrm(infection);
                           inf.ShowDialog();
                       }
                   }
                   
               
           }

       }
       public static void Attack(string attack, string attackdesc, ToolTipIcon icon, bool isattack)
       {
         
           
               KavprotVoice.SpeakAsync(attackdesc);

           if (!SettingsManager.Silence)
           {
               if (isattack)
               {
                   AVEngine.AlertMaximalinfection();
               }
           
               AVEngine.EventsManager.CallNotify(attack, attackdesc, icon);
           }
       }
     
       public static void ScanCompleted()
       {
           KavprotVoice.SpeakAsync("Scan completed successfully");
           AVEngine.EventsManager.CallScanCompleted();
           DevComponents.DotNetBar.TaskDialogInfo inf = new DevComponents.DotNetBar.TaskDialogInfo();
           inf.DialogButtons = DevComponents.DotNetBar.eTaskDialogButton.Ok;

           inf.Title = "Scan";
           inf.Text = "Scan completed successfully";
           inf.TaskDialogIcon = DevComponents.DotNetBar.eTaskDialogIcon.Flag;
           inf.Header = "Scan completed";
           inf.FooterText = "Kavprot smart security";
           inf.DialogColor = DevComponents.DotNetBar.eTaskDialogBackgroundColor.Silver;
  
               DevComponents.DotNetBar.TaskDialog.Show(inf);
        
       }
       public static void UpdateProg(string current, string dest, string path)
       {
           KavprotVoice.SpeakAsync("Do you want to upgrade Kavprot smart security From " + current + " To " + dest );
           DevComponents.DotNetBar.TaskDialogInfo inf = new DevComponents.DotNetBar.TaskDialogInfo();
           inf.DialogButtons = DevComponents.DotNetBar.eTaskDialogButton.Yes | DevComponents.DotNetBar.eTaskDialogButton.No;

           inf.Title = "New Version ready to be installed";
           inf.Text = string.Format("Kavprot smart security downloaded the update package. if you want to upgrade this version from {0} to {1} click 'yes' if not click 'no'.", current, dest);
           inf.TaskDialogIcon = DevComponents.DotNetBar.eTaskDialogIcon.Flag;
           inf.Header = "Scan completed";
           inf.FooterText = "Kavprot smart security";
           inf.DialogColor = DevComponents.DotNetBar.eTaskDialogBackgroundColor.Silver;

          var result = DevComponents.DotNetBar.TaskDialog.Show(inf);
          if (result == DevComponents.DotNetBar.eTaskDialogResult.Yes)
          {
              Process.Start(Application.StartupPath + @"\KPAVUPDATE.exe", path);
              Application.Exit();
          }


       }
       public static DevComponents.DotNetBar.eTaskDialogResult NewDrive(DriveInfo drv)
       {
           KavprotVoice.SpeakAsync("New Drive detected. would you like to scan it ?");
           AVEngine.EventsManager.CallNewDriveConnected();
           DevComponents.DotNetBar.TaskDialogInfo inf = new DevComponents.DotNetBar.TaskDialogInfo();
           inf.DialogButtons = DevComponents.DotNetBar.eTaskDialogButton.Yes  | DevComponents.DotNetBar.eTaskDialogButton.No;

           inf.Title = "New drive Detected (" + drv.Name + ") " + drv.VolumeLabel;
           inf.Text = "New Drive detected do you want to scan it";
           inf.TaskDialogIcon = DevComponents.DotNetBar.eTaskDialogIcon.Exclamation;
           inf.Header = "New Drive Connected";
           inf.FooterText = "Kavprot smart security";
           inf.DialogColor = DevComponents.DotNetBar.eTaskDialogBackgroundColor.Silver;
      
           DevComponents.DotNetBar.eTaskDialogResult dl = DevComponents.DotNetBar.TaskDialog.Show(inf);
           return dl;
       }

    }
}
