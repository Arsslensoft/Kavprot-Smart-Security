using System;
using System.Collections.Generic;
using System.Text;
using System.Media;
using System.Windows.Forms;
using KAVE.BaseEngine;

namespace KAVE.Engine
{
    public enum ScanSense
    {
        High,
        Medium,
        Low
    }
   public static class AVEngine
    {
        public static void AlertMaximalinfection()
        {
            try
            {
                SoundPlayer sound = new SoundPlayer(Application.StartupPath + @"\Sounds\VIRALERT.wav");
                sound.Play();
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
        public static void AlertVirus()
        {
            try
            {
                SoundPlayer sound = new SoundPlayer(Application.StartupPath + @"\Sounds\Infected.wav");
                sound.Play();
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
        public static void AlertScan()
        {
            try
            {
                SoundPlayer sound = new SoundPlayer(Application.StartupPath + @"\Sounds\SCANEND.wav");
                sound.Play();
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
       public static KAVE.BaseEngine.KavprotEvents EventsManager;
        public static AnyScanner HashScanner;
        public static PEScanner PETypeScanner;
        public static NoScanner NothingScanner;
        public static ScriptScanner ScriptTypeScanner;
        public static ArchiveScanner ArchiveTypeScanner;
        public static ScanSense ScanSensitivity;

        static void Init()
        {
            try
            {
                HashScanner = new AnyScanner();
                PETypeScanner = new PEScanner();
                NothingScanner = new NoScanner();
                ScriptTypeScanner = new ScriptScanner();
                ArchiveTypeScanner = new ArchiveScanner();
                KAVE.BaseEngine.FileFormat.Initialize();
            }
            catch(Exception ex)
            {
                AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
        public static string TempDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\KSS2\";
       public static void Initialize(ScanSense sense)
       {
           try
           {
               ScanSensitivity = sense;
            
               EventsManager = new KAVE.BaseEngine.KavprotEvents();
              AsyncInvoke inv = new AsyncInvoke(VirusReportService.Initialize);
              inv.BeginInvoke(null, null);

               // load scanners
              AsyncInvoke sinv = new AsyncInvoke(Init);
              sinv.BeginInvoke(null, null);
              if (SettingsManager.TurboMode)
              {
                  AsyncInvoke ssinv = new AsyncInvoke(VDB.Initialize);
                  ssinv.BeginInvoke(null, null);
              }
              else
                VDB.Initialize();

           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
       }
    }
}
