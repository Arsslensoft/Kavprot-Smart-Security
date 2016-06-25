using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace KAVE
{
  internal static class VirusReportService
    {
      public static void Initialize()
      {
          detected = new Dictionary<string, string>();
          VT.Init();
      }

      public static Dictionary<string, string> detected;
      public static void ReportFile(string hash, string virusname)
      {
          string url = "http://www.arsslensoft.tk/avl/Kavprot/VRPS.php";

          WebRequest req = WebRequest.Create(url);
          req.ContentType = "application/x-www-form-urlencoded";
          req.Method = "POST";
          byte[] postdata = Encoding.ASCII.GetBytes("content=" + "<vd content='" + hash+"' type='md5'>"+virusname+"</vd>");
          Stream dataStream = req.GetRequestStream();
          // Write the data to the request stream.
          dataStream.Write(postdata, 0, postdata.Length);
          // Close the Stream object.
          dataStream.Close();


          StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream());
      }

    }
}
