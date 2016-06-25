using System;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace KAIMLBot.AIMLTagHandlers
{
    /// <summary>
    /// The random element instructs the AIML interpreter to return exactly one of its contained li 
    /// elements randomly. The random element must contain one or more li elements of type 
    /// defaultListItem, and cannot contain any other elements.
    /// </summary>
    public class wiki : KAIMLBot.Utils.AIMLTagHandler
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
        public wiki(KAIMLBot.Bot bot,
                        KAIMLBot.User user,
                        KAIMLBot.Utils.SubQuery query,
                        KAIMLBot.Request request,
                        KAIMLBot.Result result,
                        XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
            this.isRecursive = false;
        }
        public static object SearchWikipedia(string lang, string search)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://" + lang + ".wikipedia.org/wiki/" + search);
                request.Proxy = null;
                request.Timeout = 20000;
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
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }
        protected override string ProcessChange()
        {
            if (this.templateNode.Name.ToLower() == "wiki")
            {
                if (this.templateNode.InnerText.ToLower() == "wikien")
                {
                    return (string)SearchWikipedia("en", this.request.rawInput.ToUpper().Replace("XFIND ","").Replace("WHAT IS ", "").Replace("IN ENGLISH", ""));
                }
                else if (this.templateNode.InnerText.ToLower() == "wikifr")
                {
                    return (string)SearchWikipedia("fr", this.request.rawInput.ToUpper().Replace("WHAT IS ", "").Replace("IN FRENCH", ""));
                }
                else if (this.templateNode.InnerText.ToLower() == "wikiar")
                {
                    return (string)SearchWikipedia("ar", this.request.rawInput.ToUpper().Replace("WHAT IS ", "").Replace("IN ARABIC", ""));
                }
                else
                {
                    return (string)SearchWikipedia("en", this.request.rawInput.ToUpper().Replace("WHAT IS ", ""));
                }
            }
            return string.Empty;
        }
    }
}
