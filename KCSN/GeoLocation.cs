using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace KCSN
{
   public static class GeoLocation
    {
       public static string GetLocation()
       {
           HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://api.hostip.info/get_html.php?position=true");
           req.Accept = "gzip, deflate";
           req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
           return new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
       }
       public static string GetLocationJson()
       {
           HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://api.hostip.info/get_json.php");
           req.Accept = "gzip, deflate";
           req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
          return new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
       }
    }
}
