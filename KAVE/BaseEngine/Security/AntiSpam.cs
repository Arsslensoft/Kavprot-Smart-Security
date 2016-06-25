using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
/// <summary>
/// Summary description for BayesTheorem
/// </summary>
/// 
namespace KAVE
{
    public static class AntiSpam
    {
        public static bool TCheck(string text, out string word)
        {
            bool res = false;
 
                object r = VDB.GetSpam(text, out word);
                if (r != null)
                    res = true;

                word = null;
            return res;
        }
    
        public static bool Check(string file, out string word)
        {
            bool res = false;
            using (StreamReader sr = new StreamReader(file))
            {
                object r = VDB.GetSpam(sr.ReadLine(), out word);
                if (r != null)
                    res = true;
            }
            word = null;
            return res;
        }
    }
}
