using System;
using System.Collections.Generic;
using System.Text;

namespace KAVE.BaseEngine.Classes
{
    public enum SecurityState
    {
        FullTrusted,
        Trusted,
        Normal,
        UnTrusted
    }
   public class SandBoxSettings
    {

       public bool AccessFiles;
       public bool AccessPerformanceCounter;
       public bool AccessRegistry;
       public bool AccessEnvironment;
       public bool AccessFileDialog;
       public bool AccessGUI;
       public bool AccessEventLog;
       public SecurityState Security;

       public SandBoxSettings(bool ACF, bool APC, bool AREG, bool AFDLG, bool AENV, bool GUI, bool EVLOG, SecurityState security)
       {
           AccessFiles = ACF;
           AccessPerformanceCounter = APC;
           AccessRegistry = AREG;
           AccessFileDialog = AFDLG;
           AccessEnvironment = AENV;
           AccessGUI = GUI;
           AccessEventLog = EVLOG;
           Security = security;

       }
    }
}
