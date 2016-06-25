using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;
using KAVE.BaseEngine;

namespace KAVE.Monitors
{
    public delegate void DenyRule(string username,string app, string protocol, string source, string destination, string direction);
    public delegate void ProcessConnected(string app, int pid, string username);
     public delegate void TDINotifier(string notification,string protocol, string source,string app);
    public delegate void ComponentError(Exception ex);

   public static class Firewall
    {
       public static List<string> Apps = new List<string>();
       public static event ProcessConnected NewProcess;
       public static event TDINotifier FirewallEvent;
       public static event DenyRule AccessDenied;
       public static event ComponentError ErrorOccured;
       static Server server;
       static Regex splitter = new Regex(@"END_OF_REQ", RegexOptions.Compiled | RegexOptions.IgnoreCase);
       static Regex split = new Regex(@"<N>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
       public static void Init()
       {
           try
           {
             
      
               server = new Server();
               server.MessageReceived += new Server.MessageReceivedHandler(server_MessageReceived);
               server.PipeName = "\\\\.\\pipe\\128.215.59";
               server.Start();
               Restart();
           }
           catch (Exception ex)
           {
               if (ErrorOccured != null)
                   ErrorOccured(ex);
           }
           finally
           {

           }
       }
       static ASCIIEncoding encoder = new ASCIIEncoding();
       static void server_MessageReceived(string data)
       {
           try
           {
               string sd = splitter.Split(data, 2)[0];
                string[] d = split.Split( sd);
                if (d[0] == "DENY\t[default]")
                {
                    if(AccessDenied != null)
                        AccessDenied(d[7],d[6],d[1],d[3], d[4],d[2]);
                }
                else if (d[0] == "LIST")
                {
                    if (FirewallEvent != null)
                        FirewallEvent("LISTEN", d[1], d[2], d[3]);
                }
                else if (d[0] == "NLIS")
                {
                    if (FirewallEvent != null)
                        FirewallEvent("NOT_LISTEN", d[1], d[2], d[3]);
                }
                else if (d[0] == "PROC")
                {
                    if (NewProcess != null)
                        NewProcess(d[2], int.Parse(d[1]), d[3]);
                }
                else if (d[0] == "BAD_PACKET")
                {
                    if (FirewallEvent != null)
                        FirewallEvent("BAD_PACKET", d[1], d[3], d[6]);

                    Alert.Attack("Bad Packet", "a bad packet comming from " + d[3] + " using " + d[1] + " protocol was blocked. Application :" + Path.GetFileName(d[6]), System.Windows.Forms.ToolTipIcon.Warning, false); 
                }
            
           }
           catch (Exception ex)
           {
               if (ErrorOccured != null)
                   ErrorOccured(ex);
           }
       }
       public static bool Runing = false;
       public static void Start()
       {
           try
           {
               System.Diagnostics.Process p = new System.Diagnostics.Process();
               p.StartInfo.FileName = "cmd";
               p.StartInfo.Arguments = @"/c net start tdifw";
               p.StartInfo.CreateNoWindow = true;
               p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
               p.StartInfo.RedirectStandardOutput = true;
               p.StartInfo.UseShellExecute = false;
               p.EnableRaisingEvents = true;
               p.Start();
               Runing = true;
            //   p.WaitForExit();
           }
           catch (Exception ex)
           {
               if (ErrorOccured != null)
                   ErrorOccured(ex);

           }
       }
       public static void Stop()
       {

           try
           {
               System.Diagnostics.Process p = new System.Diagnostics.Process();
               p.StartInfo.FileName = "cmd";
               p.StartInfo.Arguments = @"/c net stop tdifw";
               p.StartInfo.CreateNoWindow = true;
               p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
               p.StartInfo.RedirectStandardOutput = true;
               p.StartInfo.UseShellExecute = false;
               p.EnableRaisingEvents = true;
               p.Start();
               Runing = false;
   //            p.WaitForExit();
              
           }
           catch (Exception ex)
           {
               if (ErrorOccured != null)
                   ErrorOccured(ex);

           }
       }
       public static void Restart()
       {

           try
           {
               System.Diagnostics.Process p = new System.Diagnostics.Process();
               p.StartInfo.FileName = "cmd";
               p.StartInfo.Arguments = @"/c net stop tdifw && net start tdifw";
               p.StartInfo.CreateNoWindow = true;
               p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
               p.StartInfo.RedirectStandardOutput = true;
               p.StartInfo.UseShellExecute = false;
               p.EnableRaisingEvents = true;
               p.Start();
               Runing = true;
              // p.WaitForExit();
               
           }
           catch (Exception ex)
           {
               if (ErrorOccured != null)
                   ErrorOccured(ex);

           }
       }
       public static bool Add(string access, string App)
       {
           try
           {
               using (SQLiteTransaction trans = VDB.SDB.BeginTransaction())
               {
                   using (SQLiteCommand cmd = VDB.SDB.CreateCommand())
                   {
                       cmd.CommandText = "SELECT access FROM TDI WHERE app='"+App+"';";
                       if (cmd.ExecuteScalar() != null)
                       {
                           if (cmd.ExecuteScalar().ToString() != access)
                           {
                               // update rule
                               cmd.CommandText = "UPDATE TDI SET access='"+access+"' WHERE app='"+App+"'";
                               cmd.ExecuteNonQuery();
                             
                               using (StreamWriter str = new StreamWriter(Environment.SystemDirectory + @"\drivers\etc\tdifw.conf", false))
                               {
                                   str.WriteLine("[_signature_]");
                                   str.WriteLine("_signature_=$tdi_fw$");
                                   str.WriteLine("[_config_]");
                                   str.WriteLine("eventlog_allow=0");
                                   str.WriteLine("eventlog_deny=0");
                                   str.WriteLine("eventlog_error=0");
                                   str.WriteLine("[_main_]");
                                   str.WriteLine("_default_=openlocal");

                                   cmd.CommandText = "SELECT * FROM TDI";
                                   SQLiteDataReader dr = cmd.ExecuteReader();
                                   while (dr.Read())
                                   {
                                       str.WriteLine(dr["app"] + "=" + dr["access"]);
                                   }


                                   str.WriteLine("[_users_]");
                                   str.WriteLine("_default_=*");
                                   str.WriteLine(@"NT AUTHORITY\SYSTEM=*");
                                   str.WriteLine("[_hosts_]");
                                   str.WriteLine("ANY=0.0.0.0/0");
                                   str.WriteLine("SELF=0.0.0.0/0");
                                   str.WriteLine("localhost=127.0.0.1");
                                   str.WriteLine("[openlocal]");
                                   str.WriteLine("ALLOW * * FROM ANY TO localhost NOLOG");
                                   str.WriteLine("ALLOW UDP * FROM SELF:137-138 TO ANY:137-138 NOLOG");
                                   str.WriteLine("[AllowAll]");
                                   str.WriteLine("Usr1: ALLOW * * FROM ANY TO ANY NOLOG");
                                   str.WriteLine("[DenyAll]");
                                   str.WriteLine("Usr2: DENY * * FROM ANY TO ANY NOLOG");

                               }
                               Restart();
                           }

                       }
                       else
                       {
                           // add new rule
                           cmd.CommandText = "INSERT INTO TDI (access, App) VALUES('" + access + "', '" + App + "');";
                           cmd.ExecuteNonQuery();
                      
                           using (StreamWriter str = new StreamWriter(Environment.SystemDirectory + @"\drivers\etc\tdifw.conf", false))
                           {
                               str.WriteLine("[_signature_]");
                               str.WriteLine("_signature_=$tdi_fw$");
                               str.WriteLine("[_config_]");
                               str.WriteLine("eventlog_allow=0");
                               str.WriteLine("eventlog_deny=0");
                               str.WriteLine("eventlog_error=0");
                               str.WriteLine("[_main_]");
                               str.WriteLine("_default_=openlocal");
                              
                               cmd.CommandText = "SELECT * FROM TDI";
                               SQLiteDataReader dr = cmd.ExecuteReader();
                               while (dr.Read())
                               {
                                   str.WriteLine(dr["app"] + "=" + dr["access"]);
                               }
                         

                               str.WriteLine("[_users_]");
                               str.WriteLine("_default_=*");
                               str.WriteLine(@"NT AUTHORITY\SYSTEM=*");
                               str.WriteLine("[_hosts_]");
                               str.WriteLine("ANY=0.0.0.0/0");
                               str.WriteLine("SELF=0.0.0.0/0");
                               str.WriteLine("localhost=127.0.0.1");
                               str.WriteLine("[openlocal]");
                               str.WriteLine("ALLOW * * FROM ANY TO localhost NOLOG");
                               str.WriteLine("ALLOW UDP * FROM SELF:137-138 TO ANY:137-138 NOLOG");
                               str.WriteLine("[AllowAll]");
                               str.WriteLine("Usr1: ALLOW * * FROM ANY TO ANY NOLOG");
                               str.WriteLine("[DenyAll]");
                               str.WriteLine("Usr2: DENY * * FROM ANY TO ANY NOLOG");
                           
                           }
                           Restart();
                       }
                   }
                   trans.Commit();
               }
               return true;
           }
           catch (Exception ex)
           {
               if (ErrorOccured != null)
                   ErrorOccured(ex);
               return false;
           }
       }
       public static bool Remove(string access, string App)
       {
           try
           {
               using (SQLiteTransaction trans = VDB.SDB.BeginTransaction())
               {
                   using (SQLiteCommand cmd = VDB.SDB.CreateCommand())
                   {
                       cmd.CommandText = "DELETE FROM TDI WHERE app='" + App + "' AND access='" + access + "';";
                       cmd.ExecuteNonQuery();
                       Restart();
                       using (StreamWriter str = new StreamWriter(Environment.SystemDirectory + @"\drivers\etc\tdifw.conf", false))
                       {
                           str.WriteLine("[_signature_]");
                           str.WriteLine("_signature_=$tdi_fw$");
                           str.WriteLine("[_config_]");
                           str.WriteLine("eventlog_allow=0");
                           str.WriteLine("eventlog_deny=0");
                           str.WriteLine("eventlog_error=0");
                           str.WriteLine("[_main_]");
                           str.WriteLine("_default_=openlocal");

                           cmd.CommandText = "SELECT * FROM TDI";
                           SQLiteDataReader dr = cmd.ExecuteReader();
                           while (dr.Read())
                           {
                               str.WriteLine(dr["app"] + "=" + dr["access"]);
                           }


                           str.WriteLine("[_users_]");
                           str.WriteLine("_default_=*");
                           str.WriteLine(@"NT AUTHORITY\SYSTEM=*");
                           str.WriteLine("[_hosts_]");
                           str.WriteLine("ANY=0.0.0.0/0");
                           str.WriteLine("SELF=0.0.0.0/0");
                           str.WriteLine("localhost=127.0.0.1");
                           str.WriteLine("[openlocal]");
                           str.WriteLine("ALLOW * * FROM ANY TO localhost NOLOG");
                           str.WriteLine("ALLOW UDP * FROM SELF:137-138 TO ANY:137-138 NOLOG");
                           str.WriteLine("[AllowAll]");
                           str.WriteLine("Usr1: ALLOW * * FROM ANY TO ANY NOLOG");
                           str.WriteLine("[DenyAll]");
                           str.WriteLine("Usr2: DENY * * FROM ANY TO ANY NOLOG");

                       }
                 
                       return true;
                   }
                   trans.Commit();
               }
               return false;
           }
           catch (Exception ex)
           {
               if (ErrorOccured != null)
                   ErrorOccured(ex);
               return false;
           }
       }

   }
}
