using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Windows.Forms;
using System.IO;

namespace KAVE.BaseEngine
{
   public static class AntivirusState
    {
      public static void Init()
       {
           if (!Isregistred())
               RegisterAV();
         
       }
       static ManagementObject AVOBJ;
       public static bool Isregistred()
       {
           List<string> av = new List<string>();
                string computer = Environment.MachineName;
           string wmipath = @"\\" + computer + @"\root\SecurityCenter";
           ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmipath,
             "SELECT * FROM AntivirusProduct");
           ManagementObjectCollection instances = searcher.Get();
          
         foreach (ManagementObject queryObj in instances)
          {
           if (queryObj["displayName"].ToString() == "Kavprot smart security")
           {
               AVOBJ = queryObj;
               return true;
           }
           else
           {
              
           }
          }
         return false;
       }
       public static void RegisterAV()
       {
           BackupLastAv();
           string computer = Environment.MachineName;
           string wmipath = @"\\" + computer + @"\root\SecurityCenter";

           ManagementScope oScope = new ManagementScope(wmipath);
           ManagementPath oPath = new ManagementPath("AntiVirusProduct");
           ObjectGetOptions oGetOp = new ObjectGetOptions();
           ManagementClass oProcess = new ManagementClass(oScope, oPath, oGetOp);
           if (AVOBJ != null)
           {
               // Obtain in-parameters for the method
               AVOBJ.SetPropertyValue(
                 "displayName", "Kavprot smart security");
               AVOBJ.SetPropertyValue("companyName", "Arsslensoft");
               AVOBJ.SetPropertyValue("instanceGuid", @"{105d8e5c-97fc-47ad-b730-b26780cc5926}");
               AVOBJ.SetPropertyValue("onAccessScanningEnabled", true);
               AVOBJ.SetPropertyValue("productUptoDate", true);
               AVOBJ.SetPropertyValue("versionNumber", "1.0");
               AVOBJ.Put();
           }
           else
           {
               ManagementObject obj = oProcess.CreateInstance();
               // Obtain in-parameters for the method
               obj.SetPropertyValue(
                 "displayName", "Kavprot smart security");
               obj.SetPropertyValue("companyName", "Arsslensoft");
               obj.SetPropertyValue("instanceGuid", @"{105d8e5c-97fc-47ad-b730-b26780cc5926}");
               obj.SetPropertyValue("onAccessScanningEnabled", true);
               obj.SetPropertyValue("productUptoDate", true);
               obj.SetPropertyValue("versionNumber", "1.0");
               obj.Put();
               AVOBJ = obj;
           }

       }
       public static void RemoveInstance()
       {
           string computer = Environment.MachineName;
           string wmipath = @"\\" + computer + @"\root\SecurityCenter";
           ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmipath,
             "SELECT * FROM AntivirusProduct");
           ManagementObjectCollection instances = searcher.Get();
           //MessageBox.Show(instances.Count.ToString()); 
           StreamWriter str = new StreamWriter(Application.StartupPath + @"\LastAV.txt", true);
           foreach (ManagementObject queryObj in instances)
           {
               if (queryObj["displayName"].ToString() == "Kavprot smart security")
               {
                   queryObj.Delete();
               }
               else
               {

               }

           }
 
       }
       public static void BackupLastAv()
       {
           string computer = Environment.MachineName;
           string wmipath = @"\\" + computer + @"\root\SecurityCenter";
           ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmipath,
             "SELECT * FROM AntivirusProduct");
           ManagementObjectCollection instances = searcher.Get();
           //MessageBox.Show(instances.Count.ToString()); 
           StreamWriter str = new StreamWriter(Application.StartupPath + @"\LastAV.txt", true);
           foreach (ManagementObject queryObj in instances)
           {
               str.WriteLine(queryObj["displayName"]);
               str.WriteLine(queryObj["companyName"]);
               str.WriteLine(queryObj["instanceGuid"]);
               str.WriteLine(queryObj["versionNumber"]);
           }
           str.Close();
       }
       public static void SetProtection(bool value)
       {
           SetUptodate(value);
           if (AVOBJ != null)
           {
               AVOBJ.SetPropertyValue("onAccessScanningEnabled", value);
               AVOBJ.Put();
           }
           else
           {
               string computer = Environment.MachineName;
               string wmipath = @"\\" + computer + @"\root\SecurityCenter";

               ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmipath,
             "SELECT * FROM AntivirusProduct");
               ManagementObjectCollection instances = searcher.Get();
               //MessageBox.Show(instances.Count.ToString()); 
              foreach (ManagementObject queryObj in instances)
               {
                   if (queryObj["displayName"].ToString() == "Kavprot smart security")
                   {
                       queryObj.SetPropertyValue("onAccessScanningEnabled", value);
                       queryObj.Put();
                       AVOBJ = queryObj;
                   }
               }

           }
       }
       public static bool GetProtection()
       {
           if (AVOBJ != null)
           {
              return Convert.ToBoolean(AVOBJ.GetPropertyValue("onAccessScanningEnabled"));
           }
           else
           {
               string computer = Environment.MachineName;
               string wmipath = @"\\" + computer + @"\root\SecurityCenter";

               ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmipath,
             "SELECT * FROM AntivirusProduct");
               ManagementObjectCollection instances = searcher.Get();
               //MessageBox.Show(instances.Count.ToString()); 
               StreamWriter str = new StreamWriter(Application.StartupPath + @"\LastAV.txt", true);
               foreach (ManagementObject queryObj in instances)
               {
                   if (queryObj["displayName"].ToString() == "Kavprot smart security")
                   {
                       AVOBJ = queryObj;
                       return Convert.ToBoolean(queryObj.GetPropertyValue("onAccessScanningEnabled"));
                    
                   }
                   else
                   {
                       return false;
                   }
               }

           }
           return false;
       }
       public static void SetUptodate(bool value)
       {
           if (AVOBJ != null)
           {
               AVOBJ.SetPropertyValue("onAccessScanningEnabled", value);
               AVOBJ.Put();
           }
           else
           {
               string computer = Environment.MachineName;
               string wmipath = @"\\" + computer + @"\root\SecurityCenter";

               ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmipath,
             "SELECT * FROM AntivirusProduct");
               ManagementObjectCollection instances = searcher.Get();
               //MessageBox.Show(instances.Count.ToString()); 
               StreamWriter str = new StreamWriter(Application.StartupPath + @"\LastAV.txt", true);
               foreach (ManagementObject queryObj in instances)
               {
                   if (queryObj["displayName"].ToString() == "Kavprot smart security")
                   {
                       queryObj.SetPropertyValue("productUptoDate", value);
                       queryObj.Put();
                       AVOBJ = queryObj;
                   }
            
               }
           }
       }
    }
}
