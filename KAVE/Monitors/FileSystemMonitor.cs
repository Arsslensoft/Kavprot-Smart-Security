using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using KAVE.Engine;
using KAVE.BaseEngine;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Runtime.InteropServices;
using System.Threading;

namespace KAVE.Monitors
{
    public delegate void NewFileModified(string file);
  public static  class FileSystemMonitor
    {
      static List<string> files = new List<string>(1000);
      static Int16 used =0;
       public static bool Initialized = false;
      static List<FileSystemWatcher> Monitors;
    public  static bool Runing = false;
      public static void Initialize(bool high)
      {
          try
          {
              if (!Initialized)
              {
                 
                  Monitors = new List<FileSystemWatcher>();
                  if (high)
                  {

                      foreach (string drive in Environment.GetLogicalDrives())
                      {
                          FileSystemWatcher watcher = new FileSystemWatcher();
                          watcher.Path = drive;
                          /* Watch for changes in LastAccess and LastWrite times, and
                             the renaming of files or directories. */
                          watcher.NotifyFilter = NotifyFilters.LastWrite;
                          watcher.IncludeSubdirectories = true;
                          // Only watch text files.
                          watcher.Filter = "*.*";

                          // Add event handlers.
                          watcher.Created += new FileSystemEventHandler(OnChanged);
                          watcher.Changed += new FileSystemEventHandler(OnChanged);
                          // Begin watching.
                          watcher.EnableRaisingEvents = true;
                          Monitors.Add(watcher);
                      }
                  }
                  else
                  {
                      FileSystemWatcher watcher = new FileSystemWatcher();
                      watcher.Path = "C:\\";
                      /* Watch for changes in LastAccess and LastWrite times, and
                         the renaming of files or directories. */
                      watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess;
                      watcher.IncludeSubdirectories = true;
                      // Only watch text files.
                      watcher.Filter = "*.*";

                      // Add event handlers.
                      watcher.Created += new FileSystemEventHandler(OnChanged);
                      watcher.Changed += new FileSystemEventHandler(OnChanged);
                      // Begin watching.
                      watcher.EnableRaisingEvents = true;
                      Monitors.Add(watcher);
                  }

           
                  Initialized = true;
                  Runing = true;
              }
          }
          catch (Exception ex)
          {
              AntiCrash.LogException(ex);
          }
      }
      public static void Start()
      {
          try
          {
              // enable monitors
              foreach (FileSystemWatcher watch in Monitors)
              {
                  watch.EnableRaisingEvents = true;

              }
              // enable control filter
              if (Process.GetProcessesByName("KavprotSD").Length == 0)
              if (SettingsManager.SelfDefense)
                  Process.Start(Application.StartupPath + @"\KavprotSD.exe");

 
      
              Runing = true;
          
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
              // enable monitors
              foreach (FileSystemWatcher watch in Monitors)
              {
                  watch.EnableRaisingEvents = false;

              }
         
              foreach(Process p in Process.GetProcessesByName("KavprotSD"))
                  p.Kill();

              Runing = false;

          }
          catch (Exception ex)
          {
              AntiCrash.LogException(ex);
          }
          finally
          {

          }
      }


      private static void OnChanged(object source, FileSystemEventArgs e)
      {
          try
          {
              AVEngine.EventsManager.CallFileChanged();
              if (FileFormat.GetRTSF(e.FullPath) == "SCAN")
              {
                  if (!files.Contains(e.FullPath) && !e.FullPath.Contains(AVEngine.TempDir))
                  {
                    
                      if (SettingsManager.OneTimeScan)
                      {
                          if (used == 999)
                              used = 0;
                          files.Add(e.FullPath);
                          Scanner.ScanFile(e.FullPath);
                          used++;
                      }
                      else
                      {
                          Scanner.ScanFile(e.FullPath);
                      }
                      if (SettingsManager.SmartBackup)
                      {
                          BackupIt(e);
                      }
                  }

              }


          }
          catch
          {

          }
          finally
          {

          }
      }
      static void BackupIt(FileSystemEventArgs e)
      {
          if (SettingsManager.SmartBackupList.Contains(Path.GetExtension(e.FullPath)))
          {
              File.WriteAllText(Application.StartupPath + @"\Backup\" + Path.GetFileName(e.FullPath) + "." + Security.GetMD5HashFromFile(e.FullPath) + ".kpbinfo", e.FullPath);
              Backup.Make(e.FullPath, Application.StartupPath + @"\Backup\" + Path.GetFileName(e.FullPath) + "." + Security.GetMD5HashFromFile(e.FullPath) + ".kpb", "KPAVRSBK");
          }

      }
    }
}
