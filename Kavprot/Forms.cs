using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Kavprot
{
   public static class Forms
    {
       internal static Control GetControl(string ctrl)
       {
           frm.metroStatusBar1.Refresh();
           switch (ctrl)
           {
               case "main":
                   if (MainUI == null)
                   {
                       MainUI = new MainC();
                       MainUI.Size = new Size(766, 412);
                       return MainUI;
                   }
                   else
                       return MainUI;
                   
               case "about":
                   if (AboutUI == null)
                   {
                       AboutUI = new About();
                       AboutUI.Size = new Size(766, 412);
                       return AboutUI;
                   }
                   else
                       return AboutUI;
                   
               case "update":
                   if (UpdateUI == null)
                   {
                   UpdateUI = new Updatefrm();
                   UpdateUI.Size = new Size(766, 412);
                   return UpdateUI;
                   }
                   else
                       return UpdateUI;
                   
               case "backup":
                   if (BackupUI == null)
                   {
                       BackupUI = new Backupfrm();
                       BackupUI.Size = new Size(766, 412);
                       return BackupUI;
                   }
                   else
                       return BackupUI;
                   
               case "quarantine":
                   if (QuarantineUI == null)
                   {
                       QuarantineUI = new Quarantinefrm();
                       QuarantineUI.Size = new Size(766, 412);
                       return QuarantineUI;
                   }
                   else
                       return QuarantineUI;
                   
               case "repair":
                   if (RepairUI == null)
                   {
                       RepairUI = new Repairfrm();
                       RepairUI.Size = new Size(766, 412);
                       return RepairUI;
                   }
                   else
                       return RepairUI;
                   
               case "scan":
                   if (ScantypeUI == null)
                   {
                       ScantypeUI = new ScanTypeFrm();
                       ScantypeUI.Size = new Size(766, 412);
                       return ScantypeUI;
                   }
                   else
                       return ScantypeUI;
                   
               case "settings":
                   if (SettingsUI == null)
                   {
                       SettingsUI = new SETC();
                       SettingsUI.Size = new Size(766, 412);
                       return SettingsUI;
                   }
                   else
                       return SettingsUI;
                   
               case "firewall":
                   if (FirewallUI == null)
                   {
                       FirewallUI = new FirewallC();
                       FirewallUI.Size = new Size(766, 412);
                       return FirewallUI;
                   }
                   else
                       return FirewallUI;
                   
               case "license":
                   if (ActivationUI == null)
                   {
                       ActivationUI = new Activationfrm();
                       ActivationUI.Size = new Size(766, 412);
                       return ActivationUI;
                   }
                   else
                       return ActivationUI;
                   
               case "avp":
                   if (AVPUI == null)
                   {
                       AVPUI = new AVPC();
                       AVPUI.Size = new Size(766, 412);
                       return AVPUI;
                   }
                   else
                       return AVPUI;
                   
               case "utils":
                   if (ToolUI == null)
                   {
                       ToolUI = new Utilities();
                       ToolUI.Size = new Size(766, 412);
                       return ToolUI;
                   }
                   else
                       return ToolUI;
                   
                         
               case "crypto":
                   if (CryptoUI == null)
                   {
                       CryptoUI = new CryptoCenter();
                       CryptoUI.Size = new Size(766, 412);
                       return CryptoUI;
                   }
                   else
                       return CryptoUI;
                   
               case "wsd":
                   if (WSDUI == null)
                   {
                       WSDUI = new WebSmartD();
                       WSDUI.Size = new Size(766, 412);
                       return WSDUI;
                   }
                   else
                       return WSDUI;
                   
           }
           return null;
       }
      internal static MainForm frm;
       public static void Initialize(MainForm mainfrm)
       {
           MainUI = new MainC();
           MainUI.Size = new Size(766, 412);
           frm = mainfrm;
       }
       public static MainC MainUI;
       public static AVPC AVPUI;
       public static SETC SettingsUI;
       public static About AboutUI;

       public static Activationfrm ActivationUI;
       public static Backupfrm BackupUI;
       public static Quarantinefrm QuarantineUI;
       public static Repairfrm RepairUI;
       public static ScanTypeFrm ScantypeUI;
       public static Updatefrm UpdateUI;
       public static FirewallC FirewallUI;
       
       public static Utilities ToolUI;
        public static CryptoCenter CryptoUI;
       public static WebSmartD WSDUI;
    }
}
