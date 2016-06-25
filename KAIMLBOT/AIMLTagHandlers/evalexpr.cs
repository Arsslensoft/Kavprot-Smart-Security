using System;
using System.Xml;
using System.Text;
using System.Collections.Generic;

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
    public class evalexpr : KAIMLBot.Utils.AIMLTagHandler
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
        public evalexpr(KAIMLBot.Bot bot,
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
            if (this.templateNode.Name.ToLower() == "evalexpr")
            {
                if (this.templateNode.InnerText.ToLower() == "alu")
                {
                    JVP.ALN.ExpressionEval ev = new JVP.ALN.ExpressionEval(this.request.rawInput.ToUpper().Replace("EVALUATE",""));
             return ev.Evaluate().ToString();
                }
                else if (this.templateNode.InnerText.ToLower() == "solvpoly")
                {
                    List<double> src = new List<double>();
                    foreach (string s in this.request.rawInput.ToLower().Replace("solve polynominal","").Split(','))
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
                }
                else
                {
                    string[] se = this.request.rawInput.ToLower().Replace("solve linear", "").Split('|');
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
            return string.Empty;
        }
    }
}
