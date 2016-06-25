using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace KCSN
{

   public class Connection
    {
       public bool Connected = false;
       public string Username = "Anonymous";
       public string UID = "00000-0000-000-0000";
       public string Server = "http://csn.arsslensoft.tk/";

       public void Disconnect()
       {
           if (Connected)
           {
               if (DisconnectUID(UID, Username))
                   Connected = false;
               else
                   Connected = true;
           }
       }

       private bool ConnectUID(string uid, string username)
       {
           byte[] data =  Encoding.ASCII.GetBytes("usr=" + username + "&uid=" + uid + "&machine=" + Environment.MachineName.ToLower());
           HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://csn.arsslensoft.tk/con.php");

           req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
           req.Accept = "gzip, deflate";
           req.Method = "POST";
           req.ContentLength = data.LongLength;
           req.ContentType = "";
           req.Proxy = null;
           Stream dataStream = req.GetRequestStream();
           dataStream.Write(data, 0, data.Length);
           dataStream.Close();
           StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream());
           if (sr.ReadToEnd().StartsWith("http://kcsn"))
           {
               Server = sr.ReadToEnd();
               return true;
           }
           else
           {

               return false;
           }
       }
       private bool DisconnectUID(string uid, string username)
       {
           byte[] data = Encoding.ASCII.GetBytes("usr=" + username + "&uid=" + uid + "&machine=" + Environment.MachineName.ToLower());
           HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://csn.arsslensoft.tk/dcon.php");

           req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
           req.Accept = "gzip, deflate";
           req.Method = "POST";
           req.ContentLength = data.LongLength;
           req.ContentType = "";
           req.Proxy = null;
           Stream dataStream = req.GetRequestStream();
           dataStream.Write(data, 0, data.Length);
           dataStream.Close();
           StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream());
           if (sr.ReadToEnd() == "Disconnected")
           {
               return true;
           }
           else
           {

               return false;
           }
       }
       public void Connect(string username, string uid)
       {
   
               if (ConnectUID(uid, username))
                   Connected = true;
               else
                   Connected = false;

               Username = username;
               UID = uid;
          
       }
       public bool PostData(string  sdata, string type, string geolocation)
       {
           if (Connected)
           {
               byte[] data = Encoding.UTF8.GetBytes("uid=" + UID + "&dat=" + sdata + "&type=" + type + "&location="+geolocation);
               HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Server + "csnpost.php");
               req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
               req.Accept = "gzip, deflate";
               req.Method = "POST";
               req.ContentLength = data.LongLength;
               req.ContentType = "";
               req.Proxy = null;
               Stream dataStream = req.GetRequestStream();
               dataStream.Write(data, 0, data.Length);
               dataStream.Close();
               StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream());
               if (sr.ReadToEnd() == "Information Submited")
                   return true;
               else
                   return false;
           }
           else
               return false;
       }
       public string Receive()
       {
           if (Connected)
           {
               byte[] data = Encoding.UTF8.GetBytes("uid=" + UID);
               HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Server + "csnreceive.php");
               req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
               req.Accept = "gzip, deflate";
               req.Method = "POST";
               req.ContentLength = data.LongLength;
               req.ContentType = "";
               req.Proxy = null;
               Stream dataStream = req.GetRequestStream();
               dataStream.Write(data, 0, data.Length);
               dataStream.Close();
               StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream());
               if (sr.ReadToEnd() != "No action")
                   return sr.ReadToEnd();
               else
                   return null;
           }
           else
               return null;
       }
    }
}
