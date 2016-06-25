using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace KAVE.BaseEngine
{
   public static class AntiCrash
   {
       public static void LogException(Exception ex)
       {
           //using (StreamWriter str = new StreamWriter(Application.StartupPath + @"\Logs\Errors.txt", true))
           //{
           //    str.WriteLine("Error   : " + ex.TargetSite.ReflectedType.ToString());
           //    str.WriteLine("Message : " + ex.Message);
           //    str.WriteLine("StackTrace : " + ex.StackTrace);
           //    str.WriteLine("-------------------------------------------------------------------------");

           //}
       }
       public static void LogException(Exception ex, int code)
       {
           using (StreamWriter str = new StreamWriter(Application.StartupPath + @"\Logs\Errors.txt", true))
           {
               str.WriteLine("Error   : " + code);
               str.WriteLine("Message : " + ex.Message );
               str.WriteLine("StackTrace : " + ex.StackTrace);
               str.WriteLine("-------------------------------------------------------------------------");

           }
       }
       public static void LogEvent(string appevent)
       {
           using (StreamWriter str = new StreamWriter(Application.StartupPath + @"\Logs\Events.txt", true))
           {
               str.WriteLine("Event   : " + DateTime.Now.ToString());
               str.WriteLine(appevent); 
               str.WriteLine("-------------------------------------------------------------------------");

           }
       }
    }
}
