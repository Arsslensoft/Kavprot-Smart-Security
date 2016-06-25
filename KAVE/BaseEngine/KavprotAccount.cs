using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

using System.Drawing;
using System.Diagnostics;
using JVP.ALN;
using System.Text.RegularExpressions;
using JVP;
using KAVE;
using KAVE.BaseEngine;
using System.Net.Sockets;
using KCompress;
using System.Xml;
using System.Media;
using System.Windows.Forms;

namespace KAVE
{
   
  public static class KavprotRemoteControl
    {
    static System.Net.Sockets.UdpClient server = new System.Net.Sockets.UdpClient(9555);
   public static void Init()
      {
          try
          {

              SEA.InitializeKey(2048, SettingsManager.SEAKey, 32);
          }
          catch (Exception ex)
          {
              AntiCrash.LogException(ex);
          }
          finally
          {

          }
      }
   public static string ComputeCRC32(byte[] data)
      {
          uint crc = 0xFFFFFFFF;       // initial contents of LFBSR
          uint poly = 0xEDB88320;   // reverse polynomial
          for (int i = 0; i < data.Length; i++)
          {
              uint temp = (crc ^ data[i]) & 0xff;

              // read 8 bits one at a time
              for (int j = 0; j < 8; j++)
              {
                  if ((temp & 1) == 1) temp = (temp >> 1) ^ poly;
                  else temp = (temp >> 1);
              }
              crc = (crc >> 8) ^ temp;
          }

          // flip bits
          crc = crc ^ 0xffffffff;
          string hex = crc.ToString("x2");
          if (hex.Length == 8)
          {
              return hex;
          }
          else if (hex.Length == 7)
          {
              return hex + "0";
          }
          else if (hex.Length == 6)
          {
              return hex + "00";
          }
          else if (hex.Length == 5)
          {
              return hex + "000";
          }
          else if (hex.Length == 5)
          {
              return hex + "0000";
          }
          else
          {
              return "7x9a8g3q";
          }
        
      }
   public static string BuildARCPacket(string command, string accept, string Sadress, string timeout, byte[] data)
   {
       try
       {
           if (command.Length == 8 && accept.Length == 6 && Sadress.Length == 14 && timeout.Length == 4 && data.Length < 512000)
           {
               // base arcp header
               string packetheader = Sadress + accept + command + timeout;


               // Encrypt data with SEA
               string dataSEAb64 = SEA.EncryptToBase64(data);
               // compute CRC32
               string checksum = ComputeCRC32(data);
               // append checksum to packet header
               packetheader += checksum;
               return Security.ConvertToHex(packetheader + dataSEAb64);
           }
           else
           {
               return null;
           }
       }
       catch
       {
           return null;
       }
   }
   public static bool SendPacket(string source, string Destination, string packet)
      {
          try
          {
              HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://arsslenserv.eb2a.com/SendARCPacket.php");
              request.Method = "POST";
              request.Accept = "gzip, deflate";
              request.Proxy = null;
              request.Timeout = 15000;
              request.KeepAlive = true;
              request.CookieContainer = _cookieContainer;
              request.UserAgent = "Arsslensoft/UserAgent";
              request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
              string postData = "S=" + source + "&D=" + Destination + "&U=ATKOCAPCRTPBR&M=" + packet;
              byte[] byteArray = Encoding.UTF8.GetBytes(postData);
              // Set the ContentType property of the WebRequest.
              request.ContentType = "application/x-www-form-urlencoded";
              // Set the ContentLength property of the WebRequest.
              request.ContentLength = byteArray.Length;
              // Get the request stream.
              Stream dataStream = request.GetRequestStream();
              // Write the data to the request stream.
              dataStream.Write(byteArray, 0, byteArray.Length);
              // Close the Stream object.
              dataStream.Close();
              // Get the response.
              WebResponse sresponse = request.GetResponse();
              dataStream = sresponse.GetResponseStream();

              StreamReader reader = new StreamReader(dataStream);

              string responseFromServer = reader.ReadToEnd();
              if (responseFromServer.Contains("Message sent"))
              {

                  reader.Close();
                  dataStream.Close();
                  sresponse.Close();
                  return true;



              }
              else
              {
                  return false;
              }
          }
          catch
          {
              return false;
          }
      }
   private static CookieContainer _cookieContainer = new CookieContainer();
      public static bool ReceiveARCPacket(string source, string destination,out string packet)
      {
          try
          {
              HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://arsslenserv.eb2a.com/ReceiveARCPacket.php");
              request.Method = "POST";
              request.Accept = "gzip, deflate";
              request.Timeout = 15000;
              request.KeepAlive = true;
              request.CookieContainer = _cookieContainer;
              request.UserAgent = "Arsslensoft/UserAgent";
             request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
              string postData = "S=" + source + "&D=" + destination + "&U=ATKOCAPCRTPBR";
              byte[] byteArray = Encoding.UTF8.GetBytes(postData);
              // Set the ContentType property of the WebRequest.
              request.ContentType = "application/x-www-form-urlencoded";
              // Set the ContentLength property of the WebRequest.
              request.ContentLength = byteArray.Length;
              request.Proxy = GlobalProxySelection.GetEmptyWebProxy();
              // Get the request stream.
              Stream dataStream = request.GetRequestStream();
              // Write the data to the request stream.
              dataStream.Write(byteArray, 0, byteArray.Length);
              // Close the Stream object.
              dataStream.Close();
              // Get the response.
              WebResponse response = request.GetResponse();
              dataStream = response.GetResponseStream();

              StreamReader reader = new StreamReader(dataStream);

              string responseFromServer = reader.ReadToEnd();
              if (responseFromServer != "ACCESS DENIED" && responseFromServer != "" && !responseFromServer.Contains("<html>") && !responseFromServer.Contains("onfigure router"))
              {

                  reader.Close();
                  dataStream.Close();
                  response.Close();
                  packet = Security.HexAsciiConvert(responseFromServer);
                  return true;

              }
              reader.Close();
              dataStream.Close();
              response.Close();
              packet = null;
              return false;

          }
          catch
          {
              packet = null;
              return false;
          }
      }
      public static int Timeout = 5000;

      public static void ReceiveDataFromMobile()
      {
      Lb_001:
          {
              try
              {
                  string packet = null;
                      bool c = ReceiveARCPacket(SettingsManager.MobileAdress, SettingsManager.ApplicationAdress,out packet);
                      if (c)
                      {
                       
                          if (packet.Substring(0, 14) == SettingsManager.MobileAdress)
                          {
                              // from 14 to 20 (accept)
                              string accept = packet.Substring(0, 20).Remove(0, 14);
                              // from 20 to 28 (command)
                              string command = packet.Substring(0, 28).Remove(0, 20);
                              // from 28 to 32 (timeout)
                              Int32 timeout = Int32.Parse(packet.Substring(0,32).Remove(0, 28));
                              // from 32 to 40 (crc)
                              string checksum = packet.Substring(0, 40).Remove(0, 32);
                              Timeout = timeout * 1000;


                            
                              object obj = null;
                              byte[] data = SEA.DecryptFromBase64(packet.Remove(0,40));
                              string crcdt = ComputeCRC32(data);
                              if (crcdt  == checksum)
                              {
                                  string o = "";
                                  try
                                  {
                                      obj = ProcessCommand(command, data, out o);
                                      goto Lb_003;
                                  }
                                  catch
                                  {
                                      SendPacket(SettingsManager.ApplicationAdress, SettingsManager.MobileAdress, BuildARCPacket("SHOWTEXT", "ALDATA", SettingsManager.ApplicationAdress, "0005", Encoding.UTF8.GetBytes("Error while executing command")));
                                  }

                              Lb_003:
                                  {
                                      if (o != "DONOT")
                                      {
                                          if (accept == "STRING")
                                          {
                                              SendPacket(SettingsManager.ApplicationAdress, SettingsManager.MobileAdress, BuildARCPacket("SHOWTEXT", "ALDATA", SettingsManager.ApplicationAdress, "0005", Encoding.UTF8.GetBytes(obj.ToString())));
                                          }
                                          else if (accept == "AUDIOS")
                                          {
                                              // accept audio
                                              if (o == "STRING" || o == "AUDIOS")
                                              {
                                                  KavprotVoice.SpeakInWave(obj.ToString(), "C:\\ASC.wav");
                                                  SendPacket(SettingsManager.ApplicationAdress, SettingsManager.MobileAdress, BuildARCPacket("PLAYAUDI", "ALDATA", SettingsManager.ApplicationAdress, "0005", File.ReadAllBytes("C:\\ASC.wav")));
                                             
                                              }
                                          }
                                          else
                                          {
                                              if (o == "STRING" || o == "AUDIOS")
                                              {
                                                  KavprotVoice.SpeakInWave(obj.ToString(), "C:\\ASC.wav");
                                                  SendPacket(SettingsManager.ApplicationAdress, SettingsManager.MobileAdress, BuildARCPacket("PLAYAUDI", "ALDATA", SettingsManager.ApplicationAdress, "0005", File.ReadAllBytes("C:\\ASC.wav")));
                                            
                                              }
                                          }

                                      }
                                  }

                              }
                              else
                              {
                                  SendPacket(SettingsManager.ApplicationAdress, SettingsManager.MobileAdress, BuildARCPacket("SHOWTEXT", "ALDATA", SettingsManager.ApplicationAdress, "0005", Encoding.UTF8.GetBytes("Data was modified, Cyclic redundancy check (UNMATCH)")));

                              }



                          }
                      }
                
              }
              catch (Exception ex)
              {
                  SendPacket(SettingsManager.ApplicationAdress, SettingsManager.MobileAdress, BuildARCPacket("SHOWTEXT", "ALDATA", SettingsManager.ApplicationAdress, "0005", Encoding.UTF8.GetBytes(ex.Message)));

              }
              finally
              {

              }
          }
          // sleep for settings checktime
          Thread.Sleep(Timeout);
          goto Lb_001;
      } 
      static object VoiceCommand(string text)
      {
          // add more
          if (text.StartsWith("run process "))
          {
              Process.Start(text.Replace("run process", ""));
              return "Process started";
          }
          else if (text.StartsWith("kill process "))
          {
              foreach (Process p in Process.GetProcessesByName(text.Replace("kill process", "")))
              {
                  p.Kill();
              }
              return "Process killed";
          }
          else if (text.StartsWith("say "))
          {
              KavprotVoice.SpeakAsync(text.Replace("say", ""));
              return "text said";
          }
          else if (text.StartsWith("shutdown computer in "))
          {
              KAVE.Windows.WindowsControl.Shutdown(Int32.Parse(text.Replace("shutdown computer in ", "").Replace("seconds", "")));
              return "shuting down computer";
          }
          else if (text.StartsWith("reboot computer in "))
          {
              KAVE.Windows.WindowsControl.Reboot(Int32.Parse(text.Replace("rboot computer in ", "").Replace("seconds", "")));
              return "rebooting computer";
          }
          else
          {
              KavprotVoice.SpeakAsync("Unknow command, try again");
              return "Unknown command";
          }

      }
      public static string Answer(string question)
      {
          if (SettingsManager.KAI)
          {
              if (Process.GetProcessesByName("KAIML").Length > 0)
              {
    
                  IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 15000);
                  byte[] data = Encoding.ASCII.GetBytes(question);
                  server.Send(data, data.Length, iep);

                
                  server.Client.ReceiveTimeout = 4000;
                  IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 0);
                  byte[] d = server.Receive(ref ipe);

                  return Encoding.UTF8.GetString(d);
              }
              else
                  return "KAIML Process isn't runing";
          }
          else
          {
              return "Kavprot AIML Bot is disabled";
          }
      }
    static  SoundPlayer sp = new SoundPlayer(Application.StartupPath + @"\Sounds\SIREN.wav");
      public static object ProcessCommand(string command, byte[] data, out string o)
      {
          try
          {
              switch (command)
              {
                  case "RECOTEXT":
                      o = "STRING";
                      return Answer(Encoding.UTF8.GetString(data));
                       
              
                  case "VCMDTEXT":
                      o = "STRING";
                      return VoiceCommand(Encoding.UTF8.GetString(data));

                  case "LAUNALER":
                      o = "STRING";
                      sp.Play();
                
                      VolumeControl.VolumeControl.SetVolume(int.MaxValue);
                      return "Alert is launched";

                  case "EJVPASMB":
                      o = "STRING";
                      return CoreManager.ExecuteAssembly(Encoding.UTF8.GetString(data), null);
                  case "EJVPINST":
                      o = "STRING";
                      return CoreManager.ExecuteInstruction(Encoding.UTF8.GetString(data));
                 
                 case "SHUTDOWN":
                      o = "STRING";
                       KAVE.Windows.WindowsControl.Shutdown(Int32.Parse(Encoding.UTF8.GetString(data)));
                      return "Computer will shutdown in " + Encoding.UTF8.GetString(data) + " seconds";

                 case "REBOOTPC":
                      o = "STRING";
                      KAVE.Windows.WindowsControl.Reboot(Int32.Parse(Encoding.UTF8.GetString(data)));
                      return "Computer will reboot in " + Encoding.UTF8.GetString(data) + " seconds";

                 case "ABORSHUT":
                      o = "STRING";
                      KAVE.Windows.WindowsControl.AbortShutdown();
                      return "Session end will abort ";

                 case "EJECTDVD":
                      o = "STRING";
                      KAVE.Windows.HardwareControl.EjectDrive(Encoding.UTF8.GetString(data));
                      return "Drive " + Encoding.UTF8.GetString(data) + " was successfully ejected";

                 case "DEVICEDI":
                      o = "STRING";
                      KAVE.Windows.HardwareControl.SetDeviceState(new string[] {Encoding.UTF8.GetString(data)}, false);
                      return "Device " + Encoding.UTF8.GetString(data) + " was successfully disabled";

                 case "DEVICENA":
                      o = "STRING";
                      KAVE.Windows.HardwareControl.SetDeviceState(new string[] { Encoding.UTF8.GetString(data) }, true);
                      return "Device " + Encoding.UTF8.GetString(data) + " was successfully enabled";
                 case "DIRECTIO":
                      o = "STRING";
                          string[] sat = Encoding.UTF8.GetString(data).Split(',');
                      StringBuilder sb = new StringBuilder();
                      foreach (string s in GetDirection(sat[0], sat[1], sat[2]))
                      {
                          sb.Append(s + "  " + Environment.NewLine);
                      }
                      return sb.ToString();
                 case "DEVICELS":
                      o = "STRING";
                      StringBuilder sasb = new StringBuilder();
                      foreach (string s in KAVE.Windows.HardwareControl.GetAllDevices())
                      {
                          sasb.Append(s + "  " + Environment.NewLine);
                      }
                      return sasb.ToString();
                 case "SCANPATH":
                      o = "STRING";
                    ScanForm  sfrm = new ScanForm(ScanType.Zone, Encoding.UTF8.GetString(data));
                      sfrm.quickscanbtn.Enabled = false;
                      sfrm.cancelquickscan.Enabled = true;
                      sfrm.scanworker.RunWorkerAsync();
                    sfrm.fullscanlist.Items.Clear();
                    sfrm.Show();
                      return "Scanning path";

                 case "FASTSCAN":
                      o = "STRING";
                      sfrm = new ScanForm(ScanType.Quick,null);
                      sfrm.Show();
                      sfrm.quickscanbtn.Enabled = false;
                      sfrm.cancelquickscan.Enabled = true;
                      sfrm.scanworker.RunWorkerAsync();
                      sfrm.fullscanlist.Items.Clear();
                  
                      return " fast Scanning";

                 case "FULLSCAN":
                      o = "STRING";
                      sfrm = new ScanForm(ScanType.Full, null);
                      sfrm.Show();
                      sfrm.quickscanbtn.Enabled = false;
                      sfrm.cancelquickscan.Enabled = true;
                      sfrm.scanworker.RunWorkerAsync();
                      sfrm.fullscanlist.Items.Clear();
                   
                      return " Full Scanning";

                 case "WIKIPEDI":
                      o = "STRING";
                  string[] t = Encoding.UTF8.GetString(data).Split(',');
                      return SearchWikipedia(t[0], t[1]);

                 case "SENDSMST":
                      o = "STRING";
                      string[] st = Encoding.UTF8.GetString(data).Split(',');
                    SMSNeuron.SendBulkSMS(st[0], st[1], st[2], st[3]);
                      return "SMS Message Sent";

                 case "EVALEXPR":
                      o = "STRING";
                      ExpressionEval expr = new ExpressionEval();
                      expr.Expression = Encoding.UTF8.GetString(data);
                      return expr.Evaluate();
                
                  case "SOLVPOLY":
                      o = "STRING";
                      List<double> src = new List<double>();
                      foreach(string s in Encoding.UTF8.GetString(data).Split(','))
                      {
                      src.Add(Convert.ToDouble(s));
                      }

                     List<double> sd = SolveEquation.SolveEquations.SolvePolynomialEquation(src);
                     StringBuilder sssb = new StringBuilder();
                     int a = 0;
                     foreach (double di in sd)
                     {
                         a++;
                         sssb.AppendLine("s" + a.ToString() + "=" + di.ToString());
                     }
                     
                      return sssb.ToString();
                 
                  case "SOLVLINE":
                      o = "STRING";
                      string[] se = Encoding.UTF8.GetString(data).Split('|');
                      int l = se.Length;
                      double[,] d = new double[se.Length, se.Length + 1];
                      int j = 0;
                      int i = 0;
                      foreach (string si in se)
                      {
                          foreach (string sk in si.Split(','))
                          {
                              d[i, j] = Double.Parse(sk);

                              j++;
                          }
                          j = 0;
                          i++;
                      }

                      double[] sol = SolveEquation.SolveEquations.SolveLinearEquation(d);
                      StringBuilder ssb = new StringBuilder();
                      i = 0;
                      foreach (double so in sol)
                      {
                          i++;
                          ssb.AppendLine("s" + i + "=" + so);
                      }

                      return ssb.ToString();


              }
          }
          catch (Exception ex)
          {
              o = "STRING";
              return ex.Message;
          }
          o = "STRING";
          return "no action for this command";
      }
      public static string StripTagsRegex(string source)
      {
          return Regex.Replace(source, "<.*?>", string.Empty);
      }
      public static object SearchWikipedia(string lang, string search)
      {
          try
          {

              HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://"+lang+".wikipedia.org/wiki/" + search);
              request.Proxy = null;
              request.Credentials = CredentialCache.DefaultCredentials;
              request.UserAgent = "Mozilla/5.0 (Windows NT 5.1; en-US;) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.142 Safari/535.19";
              HttpWebResponse response = (HttpWebResponse)request.GetResponse();
              Stream dataStream = response.GetResponseStream();

              StreamReader sr = new StreamReader(dataStream);
              Match m = Regex.Match(sr.ReadToEnd(), @"<p>\s*(.+?)\s*</p>");
              if (m.Success)
              {
                  foreach (Group g in m.Groups)
                  {

                      if (g.Value.Length > 150)
                      {
                          return StripTagsRegex(g.Value);

                      }

                  }
              }
              return "I can't search";
          }
          catch (Exception ex)
          {
              return ex.Message;
          }
      }
     internal static List<string> GetDirection(string source, string destination, string mode)
      {
          string s = "";
          List<string> lst = new List<string>();
          lst.Add(s);
          string url = "http://maps.googleapis.com/maps/api/directions/xml?origin=" + source + "&destination=" + destination + "&sensor=false&mode=" + mode + "&language=en";
          HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
          req.Accept = "gzip,deflate";
          req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
          req.Proxy = null;

          WebResponse resp = req.GetResponse();
          XmlDocument doc = new XmlDocument();
          doc.Load(resp.GetResponseStream());
          foreach (XmlNode el in doc.DocumentElement.ChildNodes)
          {
              if (el.Name == "status")
              {
                  if (el.InnerText != "OK")
                      return lst;

              }
              else
              {
                  foreach (XmlElement cel in el.ChildNodes)
                  {
                      if (cel.Name == "summary")
                          lst.Add("Summary : " + cel.InnerText);
                      else if (cel.Name == "leg")
                      {
                          foreach (XmlElement sel in cel.ChildNodes)
                          {
                              if (sel.Name == "step")
                              {
                                  lst.Add(StripTagsRegex(sel.ChildNodes[5].InnerText) + " for " + sel.ChildNodes[6].ChildNodes[1].InnerText);
                              }
                              else if (sel.Name == "duration")
                              {
                                  s += " duration = " + sel.ChildNodes[1].InnerText + Environment.NewLine;
                              }
                              else if (sel.Name == "distance")
                              {
                                  s += " distance = " + sel.ChildNodes[1].InnerText + Environment.NewLine;

                              }
                          }
                      }

                  }
              }
          }
          lst[0] = s;
          return lst;
      }
    }
}
