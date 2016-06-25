using System;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;

namespace KAIMLBot.AIMLTagHandlers
{
    /// <summary>
    /// An element called bot, which may be considered a restricted version of get, is used to 
    /// tell the AIML interpreter that it should substitute the contents of a "bot predicate". The 
    /// value of a bot predicate is set at load-time, and cannot be changed at run-time. The AIML 
    /// interpreter may decide how to set the values of bot predicate at load-time. If the bot 
    /// predicate has no value defined, the AIML interpreter should substitute an empty string.
    /// 
    /// The bot element has a required name attribute that identifies the bot predicate. 
    /// 
    /// The bot element does not have any content. 
    /// </summary>
    public class direct : KAIMLBot.Utils.AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public direct(KAIMLBot.Bot bot,
                        KAIMLBot.User user,
                        KAIMLBot.Utils.SubQuery query,
                        KAIMLBot.Request request,
                        KAIMLBot.Result result,
                        XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (this.templateNode.Name.ToLower() == "direct")
            {
              string[] ss = this.request.rawInput.ToUpper().Replace("GUIDE ME","").Split(';');
              StringBuilder sb = new StringBuilder();
              foreach (string s in GetDirection(ss[0], ss[1], ss[2]))
              {
                  sb.AppendLine(s);
              }
              return sb.ToString();
            }
            return string.Empty;
        }
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
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
            req.Timeout = 20000;
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
                                    s += " duration  " + sel.ChildNodes[1].InnerText + Environment.NewLine;
                                }
                                else if (sel.Name == "distance")
                                {
                                    s += " distance " + sel.ChildNodes[1].InnerText ;

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
