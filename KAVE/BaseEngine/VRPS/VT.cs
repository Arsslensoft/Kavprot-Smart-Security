using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using KavProtHtml;
using System.Text.RegularExpressions;

namespace KAVE
{
    public static class VT
    {
       static Regex MatchRegex;
        public static void Init()
        {
            string beforetd = "<td class=\"positive\">";
            MatchRegex = new Regex(beforetd + @"\s*(.+?)\s*</td>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
        }
        public static bool Check(string hash, out string Virusname)
        {
            WebClient w = new WebClient();
            w.Proxy = null;
            w.Headers["Accept-Encoding"] = "gzip";
            string Html = w.DownloadString("http://www.virustotal.com/latest-report.html?resource=" + hash);
            if (Html.Contains("<table width=\"700\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" id=\"tablaMotores\">"))
            {
            
                Match m = MatchRegex.Match(Html);
                if (m.Success)
                {
                    Virusname = m.Groups[1].Value;
                    return true;
                }
            }
  

            Virusname = null;
            return false;
        }
         
    }
}
