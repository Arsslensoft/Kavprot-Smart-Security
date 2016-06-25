using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace KAVE.BaseEngine
{
    public delegate void AlertNotify(string title, string text, System.Windows.Forms.ToolTipIcon icon);
    public delegate void ScanDEL(string file);
    
  public class KavprotEvents
    {
     
      public event AlertNotify Notify;
      public event ScanDEL ScanFile;
        public event EventHandler Initialized;
        public event EventHandler Closing;
        public event EventHandler ActivationValidated;
         public event EventHandler UrlBlocked;
        public event EventHandler VirusDetected;
        public event EventHandler ScanCompleted;
        public event EventHandler ScanCanceled;
        public event EventHandler VDBUpdateCompleted;
        public event EventHandler VDBUpdateCanceled;
        public event EventHandler Quarantined;
        public event EventHandler FileStored;
        public event EventHandler NewDriveConnected;
        public event EventHandler WebChanged;
        public event EventHandler FileChanged;
        internal void CallWebChanged()
        {
            if (WebChanged != null)
                WebChanged(this, EventArgs.Empty);
        }
        internal void CallFileChanged()
        {
            if (FileChanged != null)
                FileChanged(this, EventArgs.Empty);
        }
      internal void CallInitialized()
      {
          if (Initialized != null)
          Initialized(this, EventArgs.Empty);
      }
        internal void CallClosing()
        {
            if (Closing != null)
                Closing(this, EventArgs.Empty);
        }
      internal void CallActivationValidated()
      {
          if(ActivationValidated != null)
          ActivationValidated(this, EventArgs.Empty);
      }
      internal void CallScanFile(string file)
      {
          if (ScanFile != null)
              ScanFile(file);
      }
  

      internal  void CallUrlBlocked()
      {
          if (UrlBlocked != null)
          UrlBlocked(this, EventArgs.Empty);
      }
      internal void CallVirusDetected()
      {
          if (VirusDetected != null)
          VirusDetected(this, EventArgs.Empty);
      }
      internal void CallScanCompleted()
      {
          if (ScanCompleted != null)
          ScanCompleted(this, EventArgs.Empty);
      }
      internal void CallScanCanceled()
      {
          if (ScanCanceled != null)
          ScanCanceled(this, EventArgs.Empty);
      }
      internal void CallVDBUpdateCompleted()
      {
          if (VDBUpdateCompleted != null)
          VDBUpdateCompleted(this, EventArgs.Empty);
      }
      internal void CallVDBUpdateCanceled()
      {
          if (VDBUpdateCanceled != null)
          VDBUpdateCanceled(this, EventArgs.Empty);
      }
      internal void CallQuarantined()
      {
          if (Quarantined != null)
          Quarantined(this, EventArgs.Empty);
      }
      internal void CallNotify(string t, string tt, ToolTipIcon icon)
      {
          if (Notify != null)
              Notify(t, tt, icon);
      }
      internal void CallFileStored()
      {
          if (FileStored != null)
          FileStored(this, EventArgs.Empty);
      }
      internal void CallNewDriveConnected()
      {
          if (NewDriveConnected != null)
          NewDriveConnected(this, EventArgs.Empty);
      }

     
    }
}
