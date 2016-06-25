using System;
using System.Collections.Generic;
using System.Text;
using DevComponents.DotNetBar.Controls;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Xml;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using KAVE.BaseEngine;
using KAVE.Engine;


namespace KAVE
{
   public static class UpdateManager
    {

        // The stream of data retrieved from the web server
        private static Stream strResponse;
        // The stream of data that we write to the harddrive
        private static Stream strLocal;
        // The request to the web server for file information
        private static HttpWebRequest webRequest;
        // The response from the web server containing information about the file
        private static HttpWebResponse webResponse;
        private static void Download(string url, string dest, Label lb)
        {
            using (WebClient wcDownload = new WebClient())
            {
                try
                {
                    // Create a request to the file we are downloading
                    webRequest = (HttpWebRequest)WebRequest.Create(url);
                    // Set default authentication for retrieving the file
                    webRequest.Credentials = CredentialCache.DefaultCredentials;
                    // Retrieve the response from the server
                    webResponse = (HttpWebResponse)webRequest.GetResponse();
                    // Ask the server for the file size and store it
                    Int64 fileSize = webResponse.ContentLength;

                    // Open the URL for download 
                    strResponse = wcDownload.OpenRead(url);
                    // Create a new file stream where we will be saving the data (local drive)
                    strLocal = new FileStream(dest, FileMode.Create, FileAccess.Write, FileShare.None);

                    // It will store the current number of bytes we retrieved from the server
                    int bytesSize = 0;
                    // A buffer for storing and writing the data retrieved from the server
                    byte[] downBuffer = new byte[2048];

                    // Loop through the buffer until the buffer is empty
                    while ((bytesSize = strResponse.Read(downBuffer, 0, downBuffer.Length)) > 0)
                    {
                        // Write the data from the buffer to the local hard drive
                        strLocal.Write(downBuffer, 0, bytesSize);
                        // Invoke the method that updates the form's label and progress bar
                        GUI.UpdateLabel(lb, "Downloaded " + strLocal.Length.ToString() + " out of " + fileSize.ToString());
                    }
                }
                finally
                {
                    // When the above code has ended, close the streams
                    strResponse.Close();
                    strLocal.Close();
                }
            }
        }

       public static void UpdateVDB(ProgressBarX progress, Label lb)
       {
           try
           {
               int curv = VDB.version;
               GUI.UpdateLabel(lb, "Initialzing...");
              GUI.UpdateProgress(progress, 5,100);
               Dictionary<string, string> SCRIPT = new Dictionary<string, string>();
               Dictionary<string, string> MD5 = new Dictionary<string, string>();
               Dictionary<string, string> PEMD5 = new Dictionary<string, string>();
               Dictionary<string, string> URL = new Dictionary<string, string>();
               Dictionary<string, string> HEUR = new Dictionary<string, string>();

               string updateserver = "http://arsslensoft.tk/update/";
               string vdbinfo = "VDB.version";
               // Initializing

               WebClient wbc = new WebClient();
               wbc.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
               wbc.Headers[HttpRequestHeader.Accept] = "text/plain";
               wbc.Headers[HttpRequestHeader.Cookie] = "$Version=1; Skin=new;";
               wbc.Headers[HttpRequestHeader.AcceptCharset] = "utf-8";

               Thread.Sleep(1000);
               // search for update from server
              GUI.UpdateLabel(lb, "Downloading VDB update file...");
              GUI.UpdateProgress(progress, 10, 100);
               string vdbucontent = wbc.DownloadString(updateserver + vdbinfo);
               if (Convert.ToInt32(vdbucontent) > VDB.version)
               {
                   GUI.UpdateLabel(lb, "Downloading Updates...");
                  GUI.UpdateProgress(progress, 30, 100);
                   for (int i = VDB.version + 1; i < Convert.ToInt32(vdbucontent) + 1; i++)
                   {
                       GUI.UpdateLabel(lb, "Downloading Updates (" + i.ToString() + ".vdu)...");
                      GUI.UpdateProgress(progress, 40, 100);
                       string vdupath = AVEngine.TempDir + "UPDATE.vdu";
                       string vdurl = "http://update.arsslensoft.tk/VDB/VD/" + i.ToString() + ".vdu";
                       Download(vdurl, vdupath, lb);
                       XmlDocument doc = new XmlDocument();
                       doc.Load(vdupath);
                       GUI.UpdateLabel(lb, "Downloaded " + i.ToString() + ".vdu");
                       foreach (XmlElement el in doc.DocumentElement.ChildNodes)
                       {
                           if (el.GetAttribute("type") == "md5")
                           {
                               MD5.Add(el.GetAttribute("content"), el.InnerText);
                           }
                           else if (el.GetAttribute("type") == "url")
                           {
                               URL.Add(el.GetAttribute("content"), el.InnerText);
                           }
                           else if (el.GetAttribute("type") == "pemd5")
                           {
                               PEMD5.Add(el.GetAttribute("content"), el.InnerText);
                           }
                           else if (el.GetAttribute("type") == "heuristic")
                           {
                               HEUR.Add(el.GetAttribute("content"), el.InnerText);
                           }
                           else
                           {
                               SCRIPT.Add(el.GetAttribute("content"), el.InnerText);
                           }
                       }
                       GUI.UpdateLabel(lb, "Installing Updates...");
                      GUI.UpdateProgress(progress, 70, 100);

                       if (MD5.Count > 0)
                       {
                           VDB.AddKeys(MD5, DBT.HDB);
                       }
                       else
                       {

                       }
                       if (PEMD5.Count > 0)
                       {
                           VDB.AddKeys(PEMD5, DBT.PEMD5);
                       }
                       else
                       {

                       }
                       if (URL.Count > 0)
                       {
                           VDB.AddKeys(URL, DBT.WDB);
                       }
                       else
                       {

                       }
                       if (SCRIPT.Count > 0)
                       {
                           VDB.AddKeys(SCRIPT, DBT.SDB);
                       }
                       else
                       {

                       }
                       if (HEUR.Count > 0)
                       {
                           VDB.AddKeys(HEUR, DBT.HEUR);
                       }
                       else
                       {

                       }
                       GUI.UpdateLabel(lb, "Updating VDBV...");
                      GUI.UpdateProgress(progress, 90, 100);
                       VDB.Setversion(i.ToString());
                       HEUR.Clear();
                       SCRIPT.Clear();
                       URL.Clear();
                       PEMD5.Clear();
                       MD5.Clear();
                       File.Delete(vdupath);
                   }
                   GUI.UpdateLabel(lb, "Virus Database has been updated. Last version : " + curv.ToString() + " Current Version : " + VDB.version.ToString());
                  GUI.UpdateProgress(progress, 100, 100);
                  Alert.Attack("Virus Database Update", "Kavprot smart security virus database has been updated.", ToolTipIcon.Info, false);
                  GUI.UpdateProgress(progress, 0, 100);
                 
               }
               else
               {
                   GUI.UpdateLabel(lb, "Virus Database is up to date");
                  GUI.UpdateProgress(progress, 100, 100);
                   Alert.Attack("Virus Database Update", "Kavprot smart security virus database is up to date", ToolTipIcon.Info, false);
                  GUI.UpdateProgress(progress, 0, 100);
               }
           }
           catch (Exception ex)
           {
               GUI.UpdateProgress(progress, 0, 100);
               GUI.UpdateLabel(lb, " ");
               AVEngine.EventsManager.CallVDBUpdateCanceled();
               if (ex.TargetSite.ReflectedType.ToString() != "System.Net.WebClient")
                   AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }

       public static void UpdateProgram(ProgressBarX progress, Label lb)
       {
           try
           {
               GUI.UpdateLabel(lb, "Initialzing...");
               GUI.UpdateProgress(progress, 5,100);



               string updateserver = "http://arsslensoft.tk/update/";
               string vdbinfo = "PROG.version";

               // Initializing

               WebClient wbc = new WebClient();
               wbc.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
               wbc.Headers[HttpRequestHeader.Accept] = "text/plain";
               wbc.Headers[HttpRequestHeader.Cookie] = "$Version=1; Skin=new;";
               wbc.Headers[HttpRequestHeader.AcceptCharset] = "utf-8";

               Thread.Sleep(1000);
               // search for update from server
               GUI.UpdateLabel(lb, "Downloading PROGRAM update file...");
               GUI.UpdateProgress(progress, 10,100);
               string vdbucontent = wbc.DownloadString(updateserver + vdbinfo);
               int versf = Int32.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".", ""));
               if (Convert.ToInt32(vdbucontent) > versf)
               {
                   GUI.UpdateLabel(lb, "Downloading Updates " + vdbucontent + ".pgup");
                   GUI.UpdateProgress(progress,50 ,100);
                   WebClient wb = new WebClient();
                   wb.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(wb_DownloadFileCompleted);
                   wb.DownloadFileAsync(new Uri(updateserver + "PROG/WIN32_KPAV_" + vdbucontent + ".pgup"), AVEngine.TempDir + @"PGUP\KPAVNEW.zip");
                   remv = versf.ToString();
                   remcv = vdbucontent;
               }
               else
               {
                   GUI.UpdateLabel(lb, "Kavprot smart security is up to date");
                  GUI.UpdateProgress(progress, 0,100);
               }

           }
           catch (Exception ex)
           {
               GUI.UpdateProgress(progress, 0, 100);
               GUI.UpdateLabel(lb, " ");
             if (ex.TargetSite.ReflectedType.ToString() != "System.Net.WebClient")
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }
       static string remv = null;
       static string remcv = null;
       static void wb_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
       {
           Alert.UpdateProg(remv, remcv, AVEngine.TempDir + @"PGUP\KPAVNEW.zip");
       }

    }
}
