using System;
using System.Collections.Generic;
using System.Text;
using KAVE.BaseEngine;
using System.IO;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using KAVE.Heuristic;

namespace KAVE.Engine
{
   public static class Scanner
    {
       public static void FullScan(Label CurFile, ProgressBarX progress, ItemPanel lst)
       {
           Stopwatch st = Stopwatch.StartNew();
           int total = 0;
           int scanned = 0;
           if (AVEngine.ScanSensitivity == ScanSense.High)
           {
               try
               {
                   GUI.UpdateLabel(CurFile, "Initializing...");
                   #region GetCount

                   foreach (string drv in Environment.GetLogicalDrives())
                   {
                          Stack<string> stack = new Stack<string>();
                       // 3.
                       // Add initial directory.
                       stack.Push(drv);

                       // 4.
                       // Continue while there are directories to process
                       while (stack.Count > 0)
                       {
                           // A.
                           // Get top directory
                           string dir = stack.Pop();

                           try
                           {
                               // scan all files in directory
                               foreach (string file in Directory.GetFiles(dir, "*.*"))
                               {

                                   total++;
                               }




                               // C
                               // Add all directories at this directory.
                               foreach (string dn in Directory.GetDirectories(dir))
                               {
                                   stack.Push(dn);
                               }
                           }
                           catch
                           {

                           }
                       }

                   }

                   #endregion
                   object vir = null;
                   foreach (string drv in Environment.GetLogicalDrives())
                   {
                       Stack<string> stack = new Stack<string>();
                       // 3.
                       // Add initial directory.
                       stack.Push(drv);

                       // 4.
                       // Continue while there are directories to process
                       while (stack.Count > 0)
                       {
                           // A.
                           // Get top directory
                           string dir = stack.Pop();

                           try
                           {
                               // scan all files in directory
                               foreach (string file in Directory.GetFiles(dir, "*.*"))
                               {
                                   scanned++;
                                   GUI.UpdateProgress(progress, scanned, total);
                                   GUI.UpdateLabel(CurFile,file);

                                   vir = FileFormat.GetFileFormat(file).ScanHS(file,CurFile);
                                   if (vir != null)
                                   {
                                       if (FileFormat.GetFileFormat(file) == AVEngine.ArchiveTypeScanner)
                                       {
                                           string[] s = vir.ToString().Split('&');
                                           GUI.UpdatePanel(new Virus(s[0], file,s[1], FileFormat.GetFileFormat(file)), lst);
                                       }
                                       else
                                         GUI.UpdatePanel(new Virus(vir.ToString(), file, FileFormat.GetFileFormat(file)), lst);
                                   }
                                   
                               }




                               // C
                               // Add all directories at this directory.
                               foreach (string dn in Directory.GetDirectories(dir))
                               {
                                   stack.Push(dn);
                               }
                           }
                           catch
                           {

                           }
                       }

                   }
               }
               catch
               {
                   // alert needed
                   GUI.UpdateLabel(CurFile, "Scan Completed");

               }
               finally
               {

               }
           }
           else if (AVEngine.ScanSensitivity == ScanSense.Medium)
           {
               try
               {
                   GUI.UpdateLabel(CurFile, "Initializing...");

                   #region GetCount

                   foreach (string drv in Environment.GetLogicalDrives())
                   {
                          Stack<string> stack = new Stack<string>();
                       // 3.
                       // Add initial directory.
                       stack.Push(drv);

                       // 4.
                       // Continue while there are directories to process
                       while (stack.Count > 0)
                       {
                           // A.
                           // Get top directory
                           string dir = stack.Pop();

                           try
                           {
                               // scan all files in directory
                               foreach (string file in Directory.GetFiles(dir, "*.*"))
                               {

                                   total++;
                               }




                               // C
                               // Add all directories at this directory.
                               foreach (string dn in Directory.GetDirectories(dir))
                               {
                                   stack.Push(dn);
                               }
                           }
                           catch
                           {

                           }
                       }

                   }

                   #endregion
                   object vir = null;
                   foreach (string drv in Environment.GetLogicalDrives())
                   {
   Stack<string> stack = new Stack<string>();
                       // 3.
                       // Add initial directory.
                       stack.Push(drv);

                       // 4.
                       // Continue while there are directories to process
                       while (stack.Count > 0)
                       {
                           // A.
                           // Get top directory
                           string dir = stack.Pop();

                           try
                           {
                               // scan all files in directory
                               foreach (string file in Directory.GetFiles(dir, "*.*"))
                               {
                                   scanned++;
                                   GUI.UpdateProgress(progress, scanned, total);
                                   GUI.UpdateLabel(CurFile, file);

                                   vir = FileFormat.GetFileFormat(file).ScanM(file, CurFile);
                                   if (vir != null)
                                   {
                                       if (FileFormat.GetFileFormat(file) == AVEngine.ArchiveTypeScanner)
                                       {
                                           string[] s = vir.ToString().Split('&');
                                           GUI.UpdatePanel(new Virus(s[0], file, s[1], FileFormat.GetFileFormat(file)), lst);
                                       }
                                       else
                                           GUI.UpdatePanel(new Virus(vir.ToString(), file, FileFormat.GetFileFormat(file)), lst);
                                   }

                               }




                               // C
                               // Add all directories at this directory.
                               foreach (string dn in Directory.GetDirectories(dir))
                               {
                                   stack.Push(dn);
                               }
                           }
                           catch
                           {

                           }
                       }

                   }
               }
               catch
               {
                   // alert needed
                   GUI.UpdateLabel(CurFile, "Scan Completed");

               }
               finally
               {

               }
           }
           else
           {
               try
               {
                   GUI.UpdateLabel(CurFile, "Initializing...");
                   #region GetCount

                   foreach (string drv in Environment.GetLogicalDrives())
                   {
                          Stack<string> stack = new Stack<string>();
                       // 3.
                       // Add initial directory.
                       stack.Push(drv);

                       // 4.
                       // Continue while there are directories to process
                       while (stack.Count > 0)
                       {
                           // A.
                           // Get top directory
                           string dir = stack.Pop();

                           try
                           {
                               // scan all files in directory
                               foreach (string file in Directory.GetFiles(dir, "*.*"))
                               {

                                   total++;
                               }




                               // C
                               // Add all directories at this directory.
                               foreach (string dn in Directory.GetDirectories(dir))
                               {
                                   stack.Push(dn);
                               }
                           }
                           catch
                           {

                           }
                       }

                   }

                   #endregion
                   object vir = null;
                   foreach (string drv in Environment.GetLogicalDrives())
                   {
                          Stack<string> stack = new Stack<string>();
                       // 3.
                       // Add initial directory.
                       stack.Push(drv);

                       // 4.
                       // Continue while there are directories to process
                       while (stack.Count > 0)
                       {
                           // A.
                           // Get top directory
                           string dir = stack.Pop();

                           try
                           {
                               // scan all files in directory
                               foreach (string file in Directory.GetFiles(dir, "*.*"))
                               {
                                   scanned++;
                                   GUI.UpdateProgress(progress, scanned, total);
                                   GUI.UpdateLabel(CurFile, file);

                                   vir = FileFormat.GetFileFormat(file).Scan(file, CurFile);
                                   if (vir != null)
                                   {
                                       if (FileFormat.GetFileFormat(file) == AVEngine.ArchiveTypeScanner)
                                       {
                                           string[] s = vir.ToString().Split('&');
                                           GUI.UpdatePanel(new Virus(s[0], file, s[1], FileFormat.GetFileFormat(file)), lst);
                                       }
                                       else
                                           GUI.UpdatePanel(new Virus(vir.ToString(), file, FileFormat.GetFileFormat(file)), lst);
                                   }

                               }




                               // C
                               // Add all directories at this directory.
                               foreach (string dn in Directory.GetDirectories(dir))
                               {
                                   stack.Push(dn);
                               }
                           }
                           catch
                           {

                           }
                       }

                   }
               }
               catch
               {
               

               }
               finally
               {
                   st.Stop();
                   // alert needed
                   GUI.UpdateLabel(CurFile, "Scan Performed in " + st.Elapsed.ToString());
                   Alert.ScanCompleted();
               }
           }
       }
       public static void QuickScan(Label CurFile, ProgressBarX progress, ItemPanel lst, bool block)
       {
           Stopwatch st = Stopwatch.StartNew();
           int total = 0;
           int scanned = 0;
           object vir = null;
           try{
               if (AVEngine.ScanSensitivity == ScanSense.High)
               {
                   List<string> list = FileHelper.GetFilesRecursive(Environment.SystemDirectory);
                   total = list.Count;
                   foreach (string file in list)
                   {
                       scanned++;
                       GUI.UpdateProgress(progress, scanned, total);
                       GUI.UpdateLabel(CurFile, file);
                       vir = FileFormat.GetFileFormat(file).ScanHS(file, CurFile);
                       if (vir != null)
                       {
                           if (FileFormat.GetFileFormat(file) == AVEngine.ArchiveTypeScanner)
                           {
                               string[] s = vir.ToString().Split('&');
                               GUI.UpdatePanel(new Virus(s[0], file, s[1], FileFormat.GetFileFormat(file)), lst);
                           }
                           else
                               GUI.UpdatePanel(new Virus(vir.ToString(), file, FileFormat.GetFileFormat(file)), lst);
                       }

                   }
               }
               else if (AVEngine.ScanSensitivity == ScanSense.Medium)
               {
                   List<string> list = FileHelper.GetFilesRecursive(Environment.SystemDirectory);
                   total = list.Count;
                   foreach (string file in list)
                   {
                       scanned++;
                       GUI.UpdateProgress(progress, scanned, total);
                       GUI.UpdateLabel(CurFile, file);
                       vir = FileFormat.GetFileFormat(file).ScanM(file, CurFile);
                       if (vir != null)
                       {
                           if (FileFormat.GetFileFormat(file) == AVEngine.ArchiveTypeScanner)
                           {
                               string[] s = vir.ToString().Split('&');
                               GUI.UpdatePanel(new Virus(s[0], file, s[1], FileFormat.GetFileFormat(file)), lst);
                           }
                           else
                               GUI.UpdatePanel(new Virus(vir.ToString(), file, FileFormat.GetFileFormat(file)), lst);
                       }

                   }
               }
               else
               {
                   List<string> list = FileHelper.GetFilesRecursive(Environment.SystemDirectory);
                   total = list.Count;
                   foreach (string file in list)
                   {
                       scanned++;
                       GUI.UpdateProgress(progress, scanned, total);
                       GUI.UpdateLabel(CurFile, file);
                       vir = FileFormat.GetFileFormat(file).Scan(file, CurFile);
                       if (vir != null)
                       {
                           if (FileFormat.GetFileFormat(file) == AVEngine.ArchiveTypeScanner)
                           {
                               string[] s = vir.ToString().Split('&');
                               GUI.UpdatePanel(new Virus(s[0], file, s[1], FileFormat.GetFileFormat(file)), lst);
                           }
                           else
                               GUI.UpdatePanel(new Virus(vir.ToString(), file, FileFormat.GetFileFormat(file)), lst);
                       }

                   }
               }

              
                }
               catch
               {
              

               }
               finally
               {
                   st.Stop();
                   // alert needed
                   GUI.UpdateLabel(CurFile, "Scan Performed in " + st.Elapsed.ToString());
                   Alert.ScanCompleted();
               }

       }
       public static void IDPScan(Label CurFile, ProgressBarX progress, ItemPanel lst)
       {
           Stopwatch st = Stopwatch.StartNew();
           int total = 0;
           int scanned = 0;
           if (VDB.GetIDPCount() > 2)
           {
               #region GetCount

               foreach (string drv in Environment.GetLogicalDrives())
               {
                   Stack<string> stack = new Stack<string>();
                   // 3.
                   // Add initial directory.
                   stack.Push(drv);

                   // 4.
                   // Continue while there are directories to process
                   while (stack.Count > 0)
                   {
                       // A.
                       // Get top directory
                       string dir = stack.Pop();

                       try
                       {
                           // scan all files in directory
                           foreach (string file in Directory.GetFiles(dir, "*.*"))
                           {

                               total++;
                           }




                           // C
                           // Add all directories at this directory.
                           foreach (string dn in Directory.GetDirectories(dir))
                           {
                               stack.Push(dn);
                           }
                       }
                       catch
                       {

                       }
                   }

               }

               #endregion

  Stack<string> sstack = new Stack<string>();

            // 3.
            // Add initial directory.
            foreach (string drive in Environment.GetLogicalDrives())
            {
                sstack.Push(drive);
            }


            // 4.
            // Continue while there are directories to process
            while (sstack.Count > 0)
            {
                // A.
                // Get top directory
                string dir = sstack.Pop();

                try
                {

                    foreach (string file in Directory.GetFiles(dir, "*.exe"))
                    {
                        try
                        {
                            if (File.Exists(file))
                            {
                                GUI.UpdateLabel(CurFile, file);
                                scanned++;

                                GUI.UpdateProgress(progress, scanned, total);
                                if (!VDB.GetIDP(file, Security.GetMD5HashFromFile(file)))
                                {
                                    Virus item = new Virus("KavProtSense.IdentityChanged", file,AVEngine.PETypeScanner);
                                    GUI.UpdatePanel(item, lst);
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

                    foreach (string file in Directory.GetFiles(dir, "*.msi"))
                    {
                        try
                        {
                            if (File.Exists(file))
                            {
                                GUI.UpdateLabel(CurFile, file);
                                scanned++;

                                GUI.UpdateProgress(progress, scanned, total);
                                if (!VDB.GetIDP(file, Security.GetMD5HashFromFile(file)))
                                {
                                    Virus item = new Virus("KavProtSense.IdentityChanged", file, AVEngine.PETypeScanner);
                                    GUI.UpdatePanel(item, lst);
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

                    foreach (string file in Directory.GetFiles(dir, "*.dll"))
                    {
                        try
                        {
                            if (File.Exists(file))
                            {
                                GUI.UpdateLabel(CurFile, file);
                                scanned++;

                                GUI.UpdateProgress(progress, scanned, total);
                                if (!VDB.GetIDP(file, Security.GetMD5HashFromFile(file)))
                                {
                                    Virus item = new Virus("KavProtSense.IdentityChanged", file, AVEngine.PETypeScanner);
                                    GUI.UpdatePanel(item, lst);
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
                    // C
                    // Add all directories at this directory.
                    foreach (string dn in Directory.GetDirectories(dir))
                    {
                        sstack.Push(dn);
                    }
                }
                catch
                {
                    // D
                    // Could not open the directory
                }
            }
              
              
           }
           else
           {
               Stack<string> sstack = new Stack<string>();

               // 3.
               // Add initial directory.
               foreach (string drive in Environment.GetLogicalDrives())
               {
                   sstack.Push(drive);
               }


               // 4.
               // Continue while there are directories to process
               while (sstack.Count > 0)
               {
                   // A.
                   // Get top directory
                   string dir = sstack.Pop();

                   try
                   {

                       VDB.AddIDP(Directory.GetFiles(dir, "*.exe"), progress);

                       VDB.AddIDP(Directory.GetFiles(dir, "*.msi"), progress);

                       VDB.AddIDP(Directory.GetFiles(dir,"*.dll"), progress);
                       // C
                       // Add all directories at this directory.
                       foreach (string dn in Directory.GetDirectories(dir))
                       {
                           sstack.Push(dn);
                       }
                   }
                   catch
                   {
                       // D
                       // Could not open the directory
                   }
               }
              
              
           }
           st.Stop();
           // alert needed
           GUI.UpdateLabel(CurFile, "Scan Performed in " + st.Elapsed.ToString());
           Alert.ScanCompleted();
  

       }
       public static void ZoneScan(string path, Label CurFile, ProgressBarX progress, ItemPanel lst)
       {
           Stopwatch st = Stopwatch.StartNew();
           int total = 0;
           int scanned = 0;
           object vir = null;
           try
           {
               if (AVEngine.ScanSensitivity == ScanSense.High)
               {
                   List<string> list = FileHelper.GetFilesRecursive(path);
                   total = list.Count;
                   foreach (string file in list)
                   {
                       scanned++;
                       GUI.UpdateProgress(progress, scanned, total);
                       GUI.UpdateLabel(CurFile, file);
                       vir = FileFormat.GetFileFormat(file).ScanHS(file, CurFile);
                       if (vir != null)
                       {
                           if (FileFormat.GetFileFormat(file) == AVEngine.ArchiveTypeScanner)
                           {
                               string[] s = vir.ToString().Split('&');
                               GUI.UpdatePanel(new Virus(s[0], file, s[1], FileFormat.GetFileFormat(file)), lst);
                           }
                           else
                               GUI.UpdatePanel(new Virus(vir.ToString(), file, FileFormat.GetFileFormat(file)), lst);
                       }

                   }
               }
               else if (AVEngine.ScanSensitivity == ScanSense.Medium)
               {
                   List<string> list = FileHelper.GetFilesRecursive(path);
                   total = list.Count;
                   foreach (string file in list)
                   {
                       scanned++;
                       GUI.UpdateProgress(progress, scanned, total);
                       GUI.UpdateLabel(CurFile, file);
                       vir = FileFormat.GetFileFormat(file).ScanM(file, CurFile);
                       if (vir != null)
                       {
                           if (FileFormat.GetFileFormat(file) == AVEngine.ArchiveTypeScanner)
                           {
                               string[] s = vir.ToString().Split('&');
                               GUI.UpdatePanel(new Virus(s[0], file, s[1], FileFormat.GetFileFormat(file)), lst);
                           }
                           else
                               GUI.UpdatePanel(new Virus(vir.ToString(), file, FileFormat.GetFileFormat(file)), lst);
                       }

                   }
               }
               else
               {
                   List<string> list = FileHelper.GetFilesRecursive(path);
                   total = list.Count;
                   foreach (string file in list)
                   {
                       scanned++;
                       GUI.UpdateProgress(progress, scanned, total);
                       GUI.UpdateLabel(CurFile, file);
                       vir = FileFormat.GetFileFormat(file).Scan(file, CurFile);
                       if (vir != null)
                       {
                           if (FileFormat.GetFileFormat(file) == AVEngine.ArchiveTypeScanner)
                           {
                               string[] s = vir.ToString().Split('&');
                               GUI.UpdatePanel(new Virus(s[0], file, s[1], FileFormat.GetFileFormat(file)), lst);
                           }
                           else
                               GUI.UpdatePanel(new Virus(vir.ToString(), file, FileFormat.GetFileFormat(file)), lst);
                       }

                   }
               }
           }
           catch
           {
            
           }
           finally
           {
               st.Stop();
               // alert needed
               GUI.UpdateLabel(CurFile, "Scan Performed in " + st.Elapsed.ToString());
               Alert.ScanCompleted();
           }

       }
       public static void ScanFile(string filename)
       {
           try
           {
               object slst = null;
               if (AVEngine.ScanSensitivity == ScanSense.High)
                   slst = FileFormat.GetFileFormat(filename).ScanHS(filename);
               else if (AVEngine.ScanSensitivity == ScanSense.Medium)
                   slst = FileFormat.GetFileFormat(filename).ScanM(filename);
               
               else
                   slst = FileFormat.GetFileFormat(filename).Scan(filename);
            

               if (slst != null)
               {

                   Virus vi = new Virus(slst.ToString(), filename, FileFormat.GetFileFormat(filename));
                       Alert.Infected(vi);
               }
               else
                   CheckVRPS(filename);


           }
           catch
           {

           }
           finally
           {
           
           }
       }
       public static void ScanRTPFile(string filename)
       {
           try
           {
               object slst = null;
               if (AVEngine.ScanSensitivity == ScanSense.High)
                   slst = FileFormat.GetFileFormat(filename).ScanHS(filename);
               else if (AVEngine.ScanSensitivity == ScanSense.Medium)
                   slst = FileFormat.GetFileFormat(filename).ScanM(filename);

               else
                   slst = FileFormat.GetFileFormat(filename).Scan(filename);


               if (slst != null)
               {

                   Virus vi = new Virus(slst.ToString(), filename, AVEngine.NothingScanner);
                   Alert.Infected(vi);
               }
               else
                   CheckVRPS(filename);


           }
           catch
           {

           }
           finally
           {

           }
       }
       internal static bool CheckReputation(string filename)
       {
           try
           {
                   if (FileFormat.GetVRPS(Path.GetExtension(filename)) != "false")
                   {
                       // only VT
                       if (FileFormat.GetVRPS(Path.GetExtension(filename)) == "1")
                       {
                           string vn = null;
                           bool ssresult = VT.Check(Security.GetMD5HashFromFile(filename), out vn);
                           if (ssresult)
                           {
                             return true;

                           }


                       }
                       else if (FileFormat.GetVRPS(Path.GetExtension(filename)) == "2")
                       {
                           string vn = null;
                           bool ssresult = VT.Check(Security.GetMD5HashFromFile(filename), out vn);
                           if (ssresult)
                           {
                               return true;

                           }
                           else
                           {
                               string infec = null;
                               bool sysresult = ThreadExpert.Check(Security.GetMD5HashFromFile(filename), out infec);
                               if (sysresult)
                               {
                                   if (infec != "ThreatExpert Submission Report")
                                   {
                                       if (Regex.Match(infec, @"[A-Z]", RegexOptions.IgnoreCase).Success)
                                       {
                                           return true;
                                       }
                                   }

                               }

                           }
                       }


                   }
                   return false;
               
           }
           catch
           {

           }
           finally
           {

           }
           return false;
       }
       internal static void CheckVRPS(string filename)
       {
           try
           {
               if (SettingsManager.VRPS)
               {
                   if (FileFormat.GetVRPS(Path.GetExtension(filename)) != "false")
                   {
                       // only VT
                       if (FileFormat.GetVRPS(Path.GetExtension(filename)) == "1")
                       {
                           string vn = null;
                           bool ssresult = VT.Check(Security.GetMD5HashFromFile(filename), out vn);
                           if (ssresult)
                           {
                               Alert.InfectedByMany(vn, filename);

                           }


                       }
                       else if (FileFormat.GetVRPS(Path.GetExtension(filename)) == "2")
                       {
                           string vn = null;
                           bool ssresult = VT.Check(Security.GetMD5HashFromFile(filename), out vn);
                           if (ssresult)
                           {
                               Alert.InfectedByMany(vn, filename);

                           }
                           else
                           {
                               string infec = null;
                               bool sysresult = ThreadExpert.Check(Security.GetMD5HashFromFile(filename), out infec);
                               if (sysresult)
                               {
                                   if (infec != "ThreatExpert Submission Report")
                                   {
                                       if (Regex.Match(infec, @"[A-Z]", RegexOptions.IgnoreCase).Success)
                                       {
                                           Alert.InfectedByMany(infec, filename);
                                       }
                                   }

                               }

                           }
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
       public static void HeuristicScan(string filename)
       {
           Disassembler dis;
           try
           {
               dis = new Disassembler(filename, VDB.SDB);
               bool result = false;
               dis.LoadAssembly();
               dis.DisassembleAndRate(Application.StartupPath + @"\Temp\DIS_" + Security.GetMD5HashFromFile(filename) + ".asil", out result);
               if (result == true)
               {
                   Virus inf = new Virus("Kavprot.GHE.Virus", filename, filename, AVEngine.PETypeScanner);
                   Alert.Infected(inf);
               }
           }
           catch
           {
               dis = null;
           }
           finally
           {
           }
       }
       public static void ScanFiles(List<string> list, Label CurFile, ProgressBarX progress, ItemPanel lst, bool block)
       {
           Stopwatch st = Stopwatch.StartNew();
           int total = 0;
           int scanned = 0;
           object vir = null;
           try
           {
               if (AVEngine.ScanSensitivity == ScanSense.High)
               {
                   total = list.Count;
                   foreach (string file in list)
                   {
                       scanned++;
                       GUI.UpdateProgress(progress, scanned, total);
                       GUI.UpdateLabel(CurFile, file);
                       vir = FileFormat.GetFileFormat(file).ScanHS(file, CurFile);
                       if (vir != null)
                       {
                           if (FileFormat.GetFileFormat(file) == AVEngine.ArchiveTypeScanner)
                           {
                               string[] s = vir.ToString().Split('&');
                               GUI.UpdatePanel(new Virus(s[0], file, s[1], FileFormat.GetFileFormat(file)), lst);
                           }
                           else
                               GUI.UpdatePanel(new Virus(vir.ToString(), file, FileFormat.GetFileFormat(file)), lst);
                       }

                   }
               }
               else if (AVEngine.ScanSensitivity == ScanSense.Medium)
               {
                   total = list.Count;
                   foreach (string file in list)
                   {
                       scanned++;
                       GUI.UpdateProgress(progress, scanned, total);
                       GUI.UpdateLabel(CurFile, file);
                       vir = FileFormat.GetFileFormat(file).ScanM(file, CurFile);
                       if (vir != null)
                       {
                           if (FileFormat.GetFileFormat(file) == AVEngine.ArchiveTypeScanner)
                           {
                               string[] s = vir.ToString().Split('&');
                               GUI.UpdatePanel(new Virus(s[0], file, s[1], FileFormat.GetFileFormat(file)), lst);
                           }
                           else
                               GUI.UpdatePanel(new Virus(vir.ToString(), file, FileFormat.GetFileFormat(file)), lst);
                       }

                   }
               }
               else
               {
           
                   total = list.Count;
                   foreach (string file in list)
                   {
                       scanned++;
                       GUI.UpdateProgress(progress, scanned, total);
                       GUI.UpdateLabel(CurFile, file);
                       vir = FileFormat.GetFileFormat(file).Scan(file, CurFile);
                       if (vir != null)
                       {
                           if (FileFormat.GetFileFormat(file) == AVEngine.ArchiveTypeScanner)
                           {
                               string[] s = vir.ToString().Split('&');
                               GUI.UpdatePanel(new Virus(s[0], file, s[1], FileFormat.GetFileFormat(file)), lst);
                           }
                           else
                               GUI.UpdatePanel(new Virus(vir.ToString(), file, FileFormat.GetFileFormat(file)), lst);
                       }

                   }
               }
           }
           catch
           {
       

           }
           finally
           {
               st.Stop();
               // alert needed
               GUI.UpdateLabel(CurFile, "Scan Performed in " + st.Elapsed.ToString());
               Alert.ScanCompleted();
           }

       }

    }
}
