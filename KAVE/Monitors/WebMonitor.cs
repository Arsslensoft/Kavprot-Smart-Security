using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Windows.Forms;
using KProxy;
using KAVE.BaseEngine;
using KAVE.Engine;


namespace KAVE.Monitors
{
   public static class WebMonitor
   {
       #region declaration and property
       static string CTYPE = "text/javascript|text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
       static StreamWriter str;
        static int con = 0;
      static List<string> Pages;
      static List<string> Checked;
      static List<string> Blockers;
        public static bool BlockNet;

       public static bool IsRuning
        {
            get { return KProxyApplication.oProxy.IsAttached; }
        }
        public static int ListenPort
        {
            get { return KProxyApplication.oProxy.ListenPort; }
        }

       #endregion

        /// <summary>
       /// Anti-Phishing, Social Network Protection, SafeBrowsing, Anti-Malware
       /// </summary>
       public static void Initialize()
       {
           try
           {
               if (!Initialized)
               {
                   // load base
                   Pages = new List<string>();
                   BlockNet = false;
                  Checked = new List<string>();
                   // load blockers
                   Blockers = new List<string>();
                   Blockers.AddRange(File.ReadAllLines(Application.StartupPath + @"\Conf\WEBSD.dic"));
                   // load proxy
                   KPCONFIG.bMITM_HTTPS = SettingsManager.CaptureHTTPS;
                   KPCONFIG.IgnoreServerCertErrors = SettingsManager.ICE;
                   KPCONFIG.bDebugCertificateGeneration = SettingsManager.ICE;
                   str = new StreamWriter(Application.StartupPath + @"\Sessions\" + "Sessions" + ".kpavs", true);
                   KProxyApplication.Startup(8777, true, SettingsManager.DecryptHTTPS, SettingsManager.RemoteConnection);

                   // attach events
                   KProxyApplication.BeforeRequest += new SessionStateHandler(KPAVWebProxyApplication_BeforeRequest);
                   KProxyApplication.BeforeResponse += new SessionStateHandler(KPAVWebProxyApplication_BeforeResponse);
                   KProxyApplication.AfterSessionComplete += new SessionStateHandler(KPAVWebProxyApplication_AfterSessionComplete);
                   KProxyApplication.LocalHost += new SessionStateHandler(kavprotavProxyApplication_LocalHost);

                   Initialized = true;
               }
           }
           catch (Exception ex)
           {
               Initialized = false;
               AntiCrash.LogException(ex, 1);
           }
           finally
           {

           }

       }
       public static bool Initialized = false;
       /// <summary>
       /// Start the Kavprot Web Protection
       /// </summary>
       public static void Start()
       {
           try
           {
               if (!IsRuning)
                  KProxyApplication.oProxy.Attach();

              
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }

       /// <summary>
       /// Stop the Kavprot Web Protection
       /// </summary>
       public static void Stop()
       {
           try
           {
               if (IsRuning)
                   KProxyApplication.oProxy.Detach();
             
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }

       /// <summary>
       /// Shutdown the Kavprot Web Protection
       /// </summary>
       public static void Shutdown()
       {
           try
           {
               
                   KProxyApplication.Shutdown();

           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }
      
       /// <summary>
       /// Clean the WinNet Cache
       /// </summary>
       public static void CleanWINETCache(bool files, bool cookies)
       {
           WinINETCache.ClearCacheItems(files, cookies);
       }

       public static bool IsPage(string url)
       {
           return Pages.Contains(url);   
       }
       static bool SafeBrowse(Session session)
       {
           // WBSD
           if (SettingsManager.WebAgentSmartDetection)
           {
               foreach (string word in Blockers)
               {
                   if (session.fullUrl.Contains(word))
                   {
                       if (SettingsManager.BlockUrls)
                       {
                      
                               KavprotVoice.SpeakAsync("This url contains a blocked word.");
                           session.utilCreateResponseAndBypassServer();
                           session.responseBodyBytes = Encoding.ASCII.GetBytes(KAVE.Properties.Resources.ErrorPageHead + string.Format(KAVE.Properties.Resources.Title, "Kavprot smart security Blocked a Malware Attack !!!") + KAVE.Properties.Resources.Ressources + string.Format(KAVE.Properties.Resources.Bodytitle, "Kavprot smart security Blocked a Malware Attack !!!") + string.Format(KAVE.Properties.Resources.Body, KAVE.Properties.Resources.MalwareMessage));
                           session.oResponse.headers = Parser.ParseResponse("HTTP/1.1 200 OK\r\nKPAVWebProxyTemplate: True\r\nContent-Length: 165000");
                           return true;
                       }
                      
                   }
               }
           }

           // filter data
           if (SettingsManager.ParentalControl)
           {
               BlackListResult result = CheckUrl(session.fullUrl);
               if (result == BlackListResult.MalwareAttack)
               {
                   if (SettingsManager.BlockUrls)
                   {
                       
                           KavprotVoice.SpeakAsync("A malware website access was blocked.");
                       session.utilCreateResponseAndBypassServer();
                       session.responseBodyBytes = Encoding.ASCII.GetBytes(KAVE.Properties.Resources.ErrorPageHead + string.Format(KAVE.Properties.Resources.Title, "Kavprot smart security Blocked a Malware Attack !!!") + KAVE.Properties.Resources.Ressources + string.Format(KAVE.Properties.Resources.Bodytitle, "Kavprot smart security Blocked a Malware Attack !!!") + string.Format(KAVE.Properties.Resources.Body, KAVE.Properties.Resources.MalwareMessage));
                       session.oResponse.headers = Parser.ParseResponse("HTTP/1.1 200 OK\r\nKPAVWebProxyTemplate: True\r\nContent-Length: 165000");
                       return true;
                   }
               }
               else if (result == BlackListResult.PhishingAttack)
               {
                   if (SettingsManager.BlockUrls)
                   {
                      
                           KavprotVoice.SpeakAsync("A phishing website access was blocked");

                       session.utilCreateResponseAndBypassServer();

                       session.responseBodyBytes = Encoding.ASCII.GetBytes(KAVE.Properties.Resources.ErrorPageHead + string.Format(KAVE.Properties.Resources.Title, "Kavprot smart security Blocked a Phishing Attack !!!") + KAVE.Properties.Resources.Ressources + string.Format(KAVE.Properties.Resources.Bodytitle, "Kavprot smart security Blocked a Phishing Attack !!!") + string.Format(KAVE.Properties.Resources.Body, KAVE.Properties.Resources.PhishMessage));
                       session.oResponse.headers = Parser.ParseResponse("HTTP/1.1 200 OK\r\nKPAVWebProxyTemplate: True\r\nContent-Length: 165000");
                       return true;
                   }
               }
               else if (result == BlackListResult.PornAttack)
               {
                   if (SettingsManager.BlockUrls)
                   {
                       
                           KavprotVoice.SpeakAsync("A pornographic website access was blocked");

                       session.utilCreateResponseAndBypassServer();

                       session.responseBodyBytes = Encoding.ASCII.GetBytes(KAVE.Properties.Resources.ErrorPageHead + string.Format(KAVE.Properties.Resources.Title, "Kavprot smart security Blocked a Porn Attack !!!") + KAVE.Properties.Resources.Ressources + string.Format(KAVE.Properties.Resources.Bodytitle, "Kavprot smart security Blocked a Porn Attack !!!") + string.Format(KAVE.Properties.Resources.Body, KAVE.Properties.Resources.PornMSG));
                       session.oResponse.headers = Parser.ParseResponse("HTTP/1.1 200 OK\r\nKPAVWebProxyTemplate: True\r\nContent-Length: 165000");
                       return true;
                   }
               }
               else if (result == BlackListResult.Undetermined)
               {
                   if (SettingsManager.BlockUrls)
                   {
                  
                           KavprotVoice.SpeakAsync("Kavprot blocked this website for unknown reason");
                       session.utilCreateResponseAndBypassServer();
                       session.responseBodyBytes = Encoding.ASCII.GetBytes(KAVE.Properties.Resources.ErrorPageHead + string.Format(KAVE.Properties.Resources.Title, "Kavprot smart security Proxy Error") + KAVE.Properties.Resources.Ressources + string.Format(KAVE.Properties.Resources.Bodytitle, "Kavprot smart securityProxy ERROR") + string.Format(KAVE.Properties.Resources.Body, KAVE.Properties.Resources.UnderterminedMSG));
                       session.oResponse.headers = Parser.ParseResponse("HTTP/1.1 200 OK\r\nKPAVWebProxyTemplate: True\r\nContent-Length: 165000");
                       return true;
                   }
    
               }
     
           }


           return false;

       }
       static void FilterData(Session session)
       {
           if (session.fullUrl.EndsWith(".js") || session.fullUrl.EndsWith(".vbs") || session.fullUrl.EndsWith(".bat") || session.fullUrl.EndsWith(".com"))
           {
               object v = VDB.GetScript(Security.ConvertToHex(session.GetResponseBodyAsString()));
               if (v != null)
               {
                
                   KavprotVoice.SpeakAsync("A malicious code detected : " + v.ToString());
                   session.utilCreateResponseAndBypassServer();
                   session.responseBodyBytes = Encoding.ASCII.GetBytes(KAVE.Properties.Resources.ErrorPageHead + string.Format(KAVE.Properties.Resources.Title, "Kavprot smart security Blocked a malicious code : " + v.ToString()) + KAVE.Properties.Resources.Ressources + string.Format(KAVE.Properties.Resources.Bodytitle, "Kavprot smart security Blocked a malicious code : " + v.ToString()) + string.Format(KAVE.Properties.Resources.Body, KAVE.Properties.Resources.MalwareMessage));
                   session.oResponse.headers = Parser.ParseResponse("HTTP/1.1 200 OK\r\nKPAVWebProxyTemplate: True\r\nContent-Length: 165000");
               }
           }
       }

       #region Target Events

      static void kavprotavProxyApplication_LocalHost(Session e)
       {
           e.oRequest.FailSession(200, "Kavprot Proxy Local Host ", " <h1>Kavprot Smart Security</h1>");
       }
    static  int times = 0;
      
       static void KPAVWebProxyApplication_AfterSessionComplete(Session session)
       {
           try
           {
               AVEngine.EventsManager.CallWebChanged();
               Pages.Remove(session.fullUrl);
               if (SettingsManager.SaveTraffic)
               {
                   
                   str.WriteLine("------------------SESSION STARTED " + DateTime.Now.ToString() + " ------------------------------");
                   str.WriteLine(str.NewLine);
                   str.WriteLine(session.ToString());
                   str.WriteLine(str.NewLine);
                   str.WriteLine("------------------SESSION COMPLETED-----------------------------");
               }
               if (times == 50)
               {
                   times = 0;
                   GC.Collect();
                   times++;
               }

               times++;

            
           }
           catch
           {

           }
           finally
           {

           }
       }
       static void KPAVWebProxyApplication_BeforeRequest(Session session)
       {
           try
           {
               con++;
               if (!session.fullUrl.Contains("arsslenserv.eb2a.com") && !session.fullUrl.Contains("arsslensoft.tk") && !(session.fullUrl == "http://www.threatexpert.com/reports.aspx"))
               {

                   if (BlockNet)
                       session.Abort();
                   else
                   {

                       string content = session.oRequest["Accept"];
                       if (session.HTTPMethodIs("GET"))
                       {
                           if (CTYPE.Contains(content))
                           {
                               Pages.Add(session.fullUrl);
                               SafeBrowse(session);
                           }
                       }
                       else if (session.HTTPMethodIs("POST"))
                       {
                           if (SettingsManager.HighSenseFilter)
                           {
                               string w = null;
                               if (AntiSpam.TCheck(session.GetRequestBodyAsString(), out w))
                                   session.requestBodyBytes = session.GetRequestBodyEncoding().GetBytes(session.GetRequestBodyAsString().Replace(w, "UNSAFE"));

                           }
                       }



                   }
               }
              
               
           }
           catch
           {

           }
       }

       static void KPAVWebProxyApplication_BeforeResponse(Session session)
       {
           try
           {
               if (!session.fullUrl.Contains("arsslenserv.eb2a.com") && !session.fullUrl.Contains("arsslensoft.tk"))
               {

                   if (SettingsManager.FilterData)
                       FilterData(session);
               }
           }
           catch
           {
           }
       }
       #endregion

       #region safebrowsing components
       public static BlackListResult CheckUrl(string url)
       
       {
           try
           {

               using (SQLiteCommand cmd = new SQLiteCommand(VDB.WDB))
               {
                   //holds the list of urls strings to use
                   List<string> lookups;


                   //generate the urls to test for this url

                   lookups = GenerateUrlList(url);
                   lookups.Add(url);
                   //search
                   foreach (string lookup in lookups)
                   {
                       if (!Checked.Contains(lookup))
                       {
                           string hash = CreateMd5(lookup);
                           BlackListResult result = CheckUrlHash(hash, cmd);

                           if (result != BlackListResult.NotFound)
                           {
                               return result;
                           }
                           else
                           {
                               //Checked.Add(lookup);
                           }
                       }
                
                   }
               }
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
               return BlackListResult.NotFound;
           }
           finally
           {

           }
           return BlackListResult.NotFound;

       }

       private static string CreateMd5(string input)
       {
           StringBuilder buffer = new StringBuilder();

           MD5 md5 = MD5CryptoServiceProvider.Create();

           byte[] inputBytes = Encoding.UTF8.GetBytes(input);
           byte[] outputBytes = md5.ComputeHash(inputBytes);

           foreach (byte b in outputBytes)
           {
               buffer.AppendFormat("{0:x2}", b);
           }

           return buffer.ToString();
       }
       private static List<string> GenerateUrlList(string url)
       {

           List<string> urls = new List<string>();
           urls.Add(url);

           Uri u = new Uri(url);
           if (url.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
           {
               urls.Add(url.Substring(7));
               urls.Add(url.Substring(11));
           }
           else if (url.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
           {
               url = url.Substring(8);
               urls.Add(url.Substring(12));
           }
           urls.Add(u.Host);
           urls.Add(u.Host + u.LocalPath);
           urls.Add(u.Host + u.AbsolutePath);
           return urls;


       }


       public static BlackListResult CheckUrlHash(string hashedUrl, SQLiteCommand cmd)
       {
           BlackListResult blackListResult = BlackListResult.NotFound;


           cmd.CommandText = string.Format("SELECT blacklistid FROM {0} WHERE hash MATCH '{1}';", FileFormat.GetTable(hashedUrl), hashedUrl);
           object result = cmd.ExecuteScalar();
           if (result == null)
               blackListResult = BlackListResult.NotFound;
           else
           {
               int blackListId = Convert.ToInt32(result);
               int phishingId = 1;
               if (blackListId == phishingId)
               {
                   blackListResult = BlackListResult.PhishingAttack;
               }
               else if (blackListId == 3)
               {
                   blackListResult = BlackListResult.PornAttack;
               }
               else
               {
                   blackListResult = BlackListResult.MalwareAttack;
               }
           }

           return blackListResult;
       }
       #endregion
    }
}
