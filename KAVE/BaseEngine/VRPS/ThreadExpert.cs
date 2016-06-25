using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using KavProtHtml;

namespace KAVE
{
    public static class ThreadExpert
    {
        public static bool Check(string hash, out string Virusname)
        {
          HtmlWeb wb = new HtmlWeb();
            HtmlDocument doc = wb.Load("http://www.threatexpert.com/report.aspx?md5=" + hash);
            bool found = false;
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//li"))
            {
                if (link.InnerText.Contains("File MD5: 0x" + hash.ToUpper()))
                {
                    found = true;
                }
            }
            if (found)
            {
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//meta[@name]"))
                {
                    HtmlAttribute att = link.Attributes["name"];
                    if (att.Value == "description")
                    {
                        HtmlAttribute satt = link.Attributes["content"];
                        string result = satt.Value;
                        Virusname = result.Replace("ThreatExpert Report: ", "");
                        return true;
                    }
       
                }
                Virusname = null;
                return false;              
            }
            else
            {
                Virusname = null;
                return false;
            }
         }
    }
}
