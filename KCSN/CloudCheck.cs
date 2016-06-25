using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using System.Text.RegularExpressions;
using System.IO;

namespace KCSN
{
    public delegate void CheckCloudAsync(string hash, string file);
    public delegate void CloudDetected(string hash, string virusname, string file);
    public static class CloudCheck
    {
       static Regex MatchRegex;
       static Regex TEMatchRegex;
        public static void Init()
        {
                 string beforetd = "<td class=\"text-red\">";

            MatchRegex = new Regex(beforetd + @"\s*(.+?)\s*</td>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
            TEMatchRegex = new Regex("<meta name=\"description\" content=\"ThreatExpert Report: " + @"\s*(.+?)\s*" + "\">", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
        }
       public static event CloudDetected Detected;
        public static void Check(string hash, string file)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://www.virustotal.com/latest-report.html?resource=" + hash);
            req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            req.Accept = "gzip, deflate";
          StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream());
          string Html = sr.ReadToEnd();
            
                Match m = MatchRegex.Match(Html);
                if (m.Success)
                {
                    if (Detected != null)
                        Detected(hash, m.Groups[1].Value, file);
                    return;
                }
                else
                {
                    req = (HttpWebRequest)WebRequest.Create("http://www.threatexpert.com/report.aspx?md5=" + hash);
                    req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    req.Accept = "gzip, deflate";
                    sr = new StreamReader(req.GetResponse().GetResponseStream());
                    Html = sr.ReadToEnd();
                    if (Html.Contains("File MD5: 0x" + hash.ToUpper()))
                    {
                        Match s = TEMatchRegex.Match(Html);
                        if (s.Success)
                        {
                            if (Detected != null)
                                Detected(hash, s.Groups[1].Value.Split(',')[1], file);
                            return;
                        }
                    }

                }
         
        }
        public static void CheckAsync(string hash, string file)
        {

            CheckCloudAsync e = new CheckCloudAsync(CloudCheck.Check);
            e.BeginInvoke(hash,file, null, null);
        }
    }
}
