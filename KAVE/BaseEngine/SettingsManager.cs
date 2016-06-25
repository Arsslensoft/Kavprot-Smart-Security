using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using KAVE.BaseEngine.Classes;
using KAVE.Engine;
using KAVE.BaseEngine;

namespace KAVE
{
   public static class SettingsManager
   {
       public static bool OptimizeGUI
       {
           get { return Boolean.Parse(sprefs["opgui"]); }
       }

       public static bool Firewall
       {
           get { return Boolean.Parse(sprefs["firewall"]); }
       }
    
       public static bool SelfDefense
       {
           get { return Boolean.Parse(sprefs["selfdefense"]); }
       }
       public static string BrekleyFilter
       {
           get { return sprefs["bpfilter"]; }
       }
      public static bool SystemMonitor
       {
           get { return Boolean.Parse(sprefs["sysmon"]); }
       }
       public static bool AntiSpam
       {
           get { return Boolean.Parse(sprefs["aspam"]); }
       }
       public static bool TurboMode
       {
           get { return Boolean.Parse(sprefs["turbo"]); }
       }
       public static bool SaveTraffic
       {
           get { return Boolean.Parse(sprefs["straffic"]); }
       }
       public static bool HighSense
       {
           get { return Boolean.Parse(sprefs["hs"]); }
       }
       public static ScanSense Scansense
       {
           get {
               if (sprefs["ssense"] == "medium")
                   return ScanSense.Medium;
               else if (sprefs["ssense"] == "low")
                   return ScanSense.Low;
               else
                   return ScanSense.High;
                  }
       }
       public static bool WebAgent
       {
           get { return Boolean.Parse(sprefs["wa"]); }
       }
       public static bool Login
       {
           get { return Boolean.Parse(sprefs["log"]); }
       }
       public static bool NIDS
       {
           get { return Boolean.Parse(sprefs["NIDS"]); }
       }
       public static bool WebAgentSmartDetection
       {
           get { return Boolean.Parse(sprefs["wasd"]); }
       }
       public static bool OneTimeScan
       {
           get { return Boolean.Parse(sprefs["ots"]); }
       }
       public static bool SmartBackup
       {
           get { return Boolean.Parse(sprefs["sb"]); }
       }
       public static int SmartBackupSize
       {
           get { return Int32.Parse(sprefs["sbs"]); }
       }
       public static bool ParentalControl
       {
           get { return Boolean.Parse(sprefs["pc"]); }
       }
       public static bool VRPS
      {
          get { return Boolean.Parse(sprefs["vrps"]); }
      }
      public static string SandBoxPath
      {
          get { return sprefs["sdp"]; }
      }
      public static bool Silence
      {
          get { return Boolean.Parse(sprefs["gm"]); }
      }

       // new web
      public static bool BlockUrls
      {
          get { return Boolean.Parse(sprefs["bb"]); }
      }
      public static bool FilterData
      {
          get { return Boolean.Parse(sprefs["filter"]); }

      }
      public static bool HighSenseFilter
      {
          get { return Boolean.Parse(sprefs["HSFILTER"]); }
      }

      // new sandbox
      public static bool AccessFiles
      {
          get { return Boolean.Parse(sprefs["saf"]); }
      }
      public static bool AccessPerformanceCounter
      {
          get { return Boolean.Parse(sprefs["sapc"]); }
      }
      public static bool AccessRegistry
      {
          get { return Boolean.Parse(sprefs["sareg"]); }
      }
      public static bool AccessEnvironment
      {
          get { return Boolean.Parse(sprefs["saenv"]); }
      }
      public static bool AccessFileDialog
      {
          get { return Boolean.Parse(sprefs["sadlg"]); }
      }
      public static bool AccessGUI
      {
          get { return Boolean.Parse(sprefs["sagui"]); }
      }
      public static bool AccessEventLog
      {
          get { return Boolean.Parse(sprefs["saevlog"]); }
      }
      public static SecurityState Security
      {
          get
          {
              string a = sprefs["ssec"];
              switch (a)
              {
                  case "FullTrusted":
                      return SecurityState.FullTrusted;
                     
                  case "Trusted":
                      return SecurityState.Trusted;
                     
                  case "Normal":
                      return SecurityState.Normal;
                   
                  default:
                      return SecurityState.UnTrusted;
                      
              }
          }
      }
      public static int UpdateVDBEach
      {
          get { return Int32.Parse(sprefs["update"]); }
      }
      public static int CacheSize
      {
          get { return Int32.Parse(sprefs["cachesize"]); }
      }
      public static int PageSize
      {
          get { return Int32.Parse(sprefs["pagesize"]); }
      }
      public static int MaxPages
      {
          get { return Int32.Parse(sprefs["maxpages"]); }
      }
      public static string[] RTSF
      {
          get
          {
              string s = sprefs["rtsf"];
              string[] list = s.Split('|');
              return list;
          }
      }
      public static string RTSFSTR
      {
          get
          {
              return sprefs["rtsf"];
          }
      }

      public static string[] NoScannnerList
      {
          get
          {
              string s = sprefs["nsl"];
              string[] list = s.Split('|');
              return list;
          }
      }
      public static string[] ARCHScannnerList
      {
          get
          {
              string s = sprefs["archsl"];
              string[] list = s.Split('|');
              return list;
          }
      }
      public static string[] PEScannnerList
      {
          get
          {
              string s = sprefs["pesl"];
              string[] list = s.Split('|');
              return list;
          }
      }
      public static string[] ASCIIScannnerList
      {
          get
          {
              string s = sprefs["asl"];
              string[] list = s.Split('|');
              return list;
          }
      }
      public static string[] HashScannnerList
      {
          get
          {
              string s = sprefs["hsl"];
              string[] list = s.Split('|');
              return list;
          }
      }

      public static bool CaptureHTTPS
      {
          get { return Boolean.Parse(sprefs["CHTTPS"]); }
      }
      public static bool DecryptHTTPS
      {
          get { return Boolean.Parse(sprefs["DHTTPS"]); }
      }
      public static bool ICE
      {
          get { return Boolean.Parse(sprefs["ICE"]); }
      }
      public static bool RemoteConnection
      {
          get { return Boolean.Parse(sprefs["RS"]); }
      }

      public static string NoScannner
      {
          get
          {
              return sprefs["nsl"];

          }
      }
      public static string ARCHScannner
      {
          get
          {
              return sprefs["archsl"];

          }
      }
      public static string PEScannner
      {
          get
          {
              return sprefs["pesl"];

          }
      }
      public static string ASCIIScannner
      {
          get
          {
              return sprefs["asl"];

          }
      }
      public static string HashScannner
      {
          get
          {
              return sprefs["hsl"];

          }
      }

      public static string SmartBackupListS
      {
          get
          {

              return sprefs["sbe"];
          }
      }
      public static List<string> SmartBackupList
      {
          get
          {
              List<string> lst = new List<string>();
              string s = sprefs["sbe"];
              string[] list = s.Split('|');
              foreach (string si in list)
              {
                  lst.Add(si);
              }
              return lst;
          }
      }
      public static byte[] SEAKey
      {
          get
          {
           return  Encoding.UTF8.GetBytes( sprefs["seakey"]);
          }
      }
      public static string ApplicationAdress
      {
          get { return sprefs["appadr"]; }
      }
      public static string MobileAdress
      {
          get { return sprefs["mobadr"]; }
      }
      public static bool KAI
      {
          get { return Boolean.Parse(sprefs["kai"]); }
      }
      public static bool KavprotRemoteControl
      {
          get { return Boolean.Parse(sprefs["krc"]); }
      }
      public static void Default()
       {
           List<string> lst = new List<string>();
           lst.Add("firewall=True");
           lst.Add("selfdefense=True");
           lst.Add(@"bpfilter=dst port 135 and tcp port 135 and ip[2:2]==48 or icmp[icmptype]==icmp-echo and ip[2:2]==92 and icmp[8:4]==0xAAAAAAAA");
          lst.Add(@"aspam=False");
           lst.Add(@"turbo=False");
           lst.Add(@"straffic=False");
           lst.Add("hs=False");
           lst.Add("ssense=medium");
           lst.Add("wa=True");
           lst.Add("log=False");
           lst.Add(@"NIDS=True");
           lst.Add("wasd=False");
           lst.Add("ots=False");
           lst.Add("sb=False");
           lst.Add("sbs=200000000");
           lst.Add("pc=True");
           lst.Add(@"vrps=False");
           lst.Add(@"sdp=C:\SandBox");
           lst.Add("gm=False");
           lst.Add("bb=True");
           lst.Add("filter=False");
           lst.Add("HSFILTER=False");
           lst.Add("ssec=Normal");
           lst.Add("saf=True");
           lst.Add("sareg=True");
           lst.Add("sagui=True");
           lst.Add("sadlg=True");
           lst.Add("saenv=False");
           lst.Add("saevlog=True");
           lst.Add("sapc=True");
           lst.Add("sysmon=True");
           lst.Add(@"update=10800000");
           lst.Add(@"cachesize=1000");
           lst.Add(@"pagesize=1000");
           lst.Add(@"maxpages=1000");
           lst.Add(@"rtsf=.com|.rtf|.rpm|.ocx|.cpl|.lnk|.scr|.pif|.url|.wsf|.wsc|.exe|.dll|.msi|.sys|.txt|.vbs|.bat|.ini|.cmd|.hta|.inf|.reg");
           lst.Add("nsl=.ace|.pdf|.log");
           lst.Add("archsl=arj|.cab|.chm|.cpio|.deb|.dmg|.hfs|.iso|.lzh|.lzma|.nsis|.rar|.rpm|.udf|.wim|.xar|.z|.zip|.msi");
           lst.Add("pesl=.exe|.dll|.sys");
           lst.Add("asl=.txt|.js|.vbs|.html|.htm|.bat|.ini|.pl|.aspl|.asdl|.cmd|.hta|.inf|.reg");
           lst.Add("hsl=.com|.rtf|.rpm|.ocx|.cpl|.lnk|.scr|.pif|.url|.wsf|.wsc");
           lst.Add(@"CHTTPS=True");
           lst.Add(@"DHTTPS=True");
           lst.Add(@"ICE=False");
           lst.Add(@"RS=False");
           lst.Add("sbe=.txt|.sys|.rtf|.bat|.doc|.pdf|.docx");

           lst.Add(@"appadr=A:00000000:RCP");
           lst.Add(@"mobadr=M:00000000:RCP");
           lst.Add("seakey=testkey1");
           lst.Add(@"kai=False");
           lst.Add(@"krc=True");
           lst.Add(@"opgui=True");

            Write(Application.StartupPath + @"\Conf\Config.avcnf", lst);
       }
 
  
      public static void Write(string file, List<string> prefs)
       {
           try
           {
               using (StreamWriter str = new StreamWriter(Application.StartupPath + @"\d.tmp", true))
               {
                   foreach (string pref in prefs)
                   {
                       str.WriteLine(pref);
                   }
                   str.Close();
               }
               string code = File.ReadAllText(Application.StartupPath + @"\d.tmp");

               File.WriteAllText(file, code);
               File.Delete(Application.StartupPath + @"\d.tmp");
           }
           catch
           {

           }
           finally
           {

           }
       }
       static Dictionary<string, string> sprefs;
       public static void Initialize(string confile)
       {
           try
           {
               if (!File.Exists(confile))
                   Default();
             

               sprefs = new Dictionary<string, string>();
               Regex reg = new Regex(@"=", RegexOptions.IgnoreCase | RegexOptions.Compiled);

               using (StreamReader sr = new StreamReader(confile))
               {
                   while (sr.Peek() >= 0)
                   {
                       string[] t = reg.Split(sr.ReadLine(), 2);
                       sprefs.Add(t[0], t[1]);
                   }
               }
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
       }
       public static void Initialize()
       {
           try
           {
               if (!File.Exists(Application.StartupPath + @"\Conf\Config.avcnf"))
                   Default();


               sprefs = new Dictionary<string, string>();
               Regex reg = new Regex(@"=", RegexOptions.IgnoreCase | RegexOptions.Compiled);

               using (StreamReader sr = new StreamReader(Application.StartupPath + @"\Conf\Config.avcnf"))
               {
                   while (sr.Peek() >= 0)
                   {
                       string[] t = reg.Split(sr.ReadLine(), 2);
                       sprefs.Add(t[0], t[1]);
                   }
               }
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
       }

 


    }
}
