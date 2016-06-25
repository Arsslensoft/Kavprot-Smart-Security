using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using KAVE.Engine;

namespace KAVE.BaseEngine
{
    public enum ScannerType
    {
        PEScanner,
        ArchiveScanner,
        ASCIIScanner,
        NoScanner,
        HashScanner
    }
    public enum DBT
    {
        PEMD5,
        HDB,
        WDB,
        SDB,
        HEUR
    }
    public static class FileFormat
    {
        public static Dictionary<string, string> DBS;
        public static Dictionary<string, IScanner> FileFormatDB;
        public static Dictionary<string, string> VRPS;
        public static Dictionary<string,string> RTSF;
        /// <summary>
        /// 53 file type scanner / 5 scanners
        /// </summary>
        public static void Initialize()
        {

            FileFormatDB = new Dictionary<string, IScanner>();
            DBS = new Dictionary<string, string>();
         
            VRPS = new Dictionary<string, string>();
            RTSF = new Dictionary<string, string>();
            
            foreach (string ext in SettingsManager.RTSF)
            {
                RTSF.Add(ext, "SCAN");
            }

            foreach (string ext in SettingsManager.PEScannnerList)
            {
                AddThese(ext, ScannerType.PEScanner);
            }
            foreach (string ext in SettingsManager.ASCIIScannnerList)
            {
                AddThese(ext, ScannerType.ASCIIScanner);
            }
            foreach (string ext in SettingsManager.HashScannnerList)
            {
                AddThese(ext, ScannerType.HashScanner);
            }
            foreach (string ext in SettingsManager.ARCHScannnerList)
            {
                AddThese(ext, ScannerType.ArchiveScanner);
            }


            DBS.Add("A", "A");
            DBS.Add("B", "B");
            DBS.Add("C", "C");
            DBS.Add("D", "D");
            DBS.Add("E", "E");
            DBS.Add("F", "F");
            DBS.Add("G", "G");
            DBS.Add("H", "H");
            DBS.Add("I", "I");
            DBS.Add("J", "J");
            DBS.Add("K", "K");
            DBS.Add("L", "L");
            DBS.Add("M", "M");
            DBS.Add("N", "N");
            DBS.Add("O", "O");
            DBS.Add("P", "P");
            DBS.Add("Q", "Q");
            DBS.Add("R", "R");
            DBS.Add("S", "S");
            DBS.Add("T", "T");
            DBS.Add("U", "U");
            DBS.Add("V", "V");
            DBS.Add("W", "W");
            DBS.Add("X", "X");
            DBS.Add("Y", "Y");
            DBS.Add("Z", "Z");

            DBS.Add("0", "ZERO");
            DBS.Add("1", "ONE");
            DBS.Add("2", "TWO");
            DBS.Add("3", "THREE");
            DBS.Add("4", "FOUR");
            DBS.Add("5", "FIVE");
            DBS.Add("6", "SIX");
            DBS.Add("7", "SEVEN");
            DBS.Add("8", "EIGHT");
            DBS.Add("9", "NINE");

            VRPS.Add(".exe", "2");
            VRPS.Add(".dll", "2");
            VRPS.Add(".sys", "2");
            VRPS.Add(".com", "1");
            VRPS.Add(".vbs", "1");
            VRPS.Add(".js", "1");
            VRPS.Add(".bat", "1");
        }

        public static string GetRTSF(string sua)
        {
            string ua = Path.GetExtension(sua).ToLower();
            if (RTSF.ContainsKey(ua))
                return RTSF[ua];
            else
                return "false";
           
        }
        public static string GetVRPS(string ext)
        {

            if (VRPS.ContainsKey(ext))
                return VRPS[ext];
           else
              return "false";
           
        }
        public static string GetTable(string hash)
        {
            string first = hash.Substring(0, 1).ToUpper();
            if (DBS.ContainsKey(first))
                return DBS[first];       
            else
                return first;
           
        }
        public static void AddThese(string format, ScannerType type)
        {
            IScanner pe = (IScanner)AVEngine.PETypeScanner;
            IScanner ARCH = (IScanner)AVEngine.ArchiveTypeScanner;
            IScanner hash = (IScanner)AVEngine.HashScanner;
            IScanner ascii = (IScanner)AVEngine.ScriptTypeScanner;
            IScanner no = (IScanner)AVEngine.NothingScanner;

           if (type == ScannerType.ArchiveScanner)
            {
                if (!FileFormatDB.ContainsKey(format.ToLower()))
                    FileFormatDB.Add(format, ARCH);
               
            }
            else if (type == ScannerType.NoScanner)
            {
                if (!FileFormatDB.ContainsKey(format.ToLower()))
                     FileFormatDB.Add(format, no);
               
            }
            else if (type == ScannerType.PEScanner)
            {
                if (!FileFormatDB.ContainsKey(format.ToLower()))
                    FileFormatDB.Add(format, pe);
               
            }
            else if (type == ScannerType.ASCIIScanner)
            {
                if (!FileFormatDB.ContainsKey(format.ToLower()))
                    FileFormatDB.Add(format, ascii);
                
            }
            else if (type == ScannerType.HashScanner)
            {
                if (!FileFormatDB.ContainsKey(format.ToLower()))
                       FileFormatDB.Add(format, hash);
                
            }

        }
        public static IScanner GetFileFormat(string filename)
        {
            string ext = Path.GetExtension(filename).ToLower();
            if (FileFormatDB.ContainsKey(ext))
                return FileFormatDB[ext];          
            else
                return (IScanner)AVEngine.NothingScanner;
        }
    }
}
