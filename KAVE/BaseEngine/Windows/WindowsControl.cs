using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace KAVE.Windows
{
    public static class WindowsControl
    {
       
        /// <summary>
        /// Shutdown computer in given time
        /// </summary>
        /// <param name="time"> time in seconds</param>
        public static void Shutdown(int time)
        {
            Process.Start("shutdown", "/s /t "+ time.ToString());
        }
        /// <summary>
        /// Restart computer in given time
        /// </summary>
        /// <param name="time">time in seconds</param>
        public static void Reboot(int time)
        {
            Process.Start("shutdown", "/r /t " + time.ToString());
        }
        /// <summary>
        /// Logoff  in given time
        /// </summary>
        /// <param name="time">time in seconds</param>
        public static void LogOff(int time)
        {
            Process.Start("shutdown", "/l /t " + time.ToString());
        }
        /// <summary>
        /// Abort shutdown, restart, logoff (only during the time-out period)
        /// </summary>
        public static void AbortShutdown()
        {
            Process.Start("shutdown", "/a ");
        }

      
    }
}
