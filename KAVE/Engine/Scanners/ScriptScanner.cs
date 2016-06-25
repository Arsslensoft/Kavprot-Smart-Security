using System;
using System.Collections.Generic;
using System.Text;
using KAVE.BaseEngine;
using System.IO;

namespace KAVE.Engine
{
   public class ScriptScanner : IScanner
    {
       public int MaximumSize
       {
           get { return 1000000; }
       }
       public string Name
       {
           get { return "SCRIPT-SCANNER"; }
       }

       public object ScanHS(string filename)
       {
           FileInfo fi = new FileInfo(filename);
           if (fi.Length < MaximumSize)
           {
               object x = null;
               FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
               using (StreamReader sr = new StreamReader(fileStream))
               {
                   StringBuilder sb = new StringBuilder();
                   string hex = Security.DumpHex(sr, sb);

                   x = VDB.GetHSCript(hex);

               }
               if (x != null)
                   return x;
               else
                   return AVEngine.HashScanner.ScanHS(filename);
           }
           else
               return null;

       }
       public object Scan(string filename)
       {
           FileInfo fi = new FileInfo(filename);
           if (fi.Length < MaximumSize)
           {
               object x = null;
               FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
               using (StreamReader sr = new StreamReader(fileStream))
               {
                   StringBuilder sb = new StringBuilder();
                   string hex = Security.DumpHex(sr, sb);

                   x = VDB.GetScript(hex);
               }
               return x;
           }
           else
               return null;
       }
       public object ScanM(string filename)
       {
           FileInfo fi = new FileInfo(filename);
           if (fi.Length< MaximumSize)
           {
                object x = null;
               FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
               using (StreamReader sr = new StreamReader(fileStream))
               {
                   StringBuilder sb = new StringBuilder();
                   string hex = Security.DumpHex(sr, sb);

                   x = VDB.GetScript(hex);
               }
               if (x != null)
                   return x;
               else
                   return AVEngine.HashScanner.ScanHS(filename);
           }
            else
               return null;
         

       }

       public object ScanHS(string filename, System.Windows.Forms.Label lb)
       {
           FileInfo fi = new FileInfo(filename);
           if (fi.Length < MaximumSize)
           {
               object x = null;
               FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
               using (StreamReader sr = new StreamReader(fileStream))
               {
                   StringBuilder sb = new StringBuilder();
                   string hex = Security.DumpHex(sr, sb);

                   x = VDB.GetHSCript(hex);

               }
               if (x != null)
                   return x;
               else
                   return AVEngine.HashScanner.ScanHS(filename);
           }
           else
               return null;

       }
       public object Scan(string filename, System.Windows.Forms.Label lb)
       {
           FileInfo fi = new FileInfo(filename);
           if (fi.Length < MaximumSize)
           {
               object x = null;
               FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
               using (StreamReader sr = new StreamReader(fileStream))
               {
                   StringBuilder sb = new StringBuilder();
                   string hex = Security.DumpHex(sr, sb);

                   x = VDB.GetScript(hex);
               }
               return x;
           }
           else  
               return null;

       }
       public object ScanM(string filename, System.Windows.Forms.Label lb)
       {
           FileInfo fi = new FileInfo(filename);
           if (fi.Length < MaximumSize)
           {
               object x = null;
               FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
               using (StreamReader sr = new StreamReader(fileStream))
               {
                   StringBuilder sb = new StringBuilder();
                   string hex = Security.DumpHex(sr, sb);

                   x = VDB.GetScript(hex);
               }
               if (x != null)
                   return x;
               else
                   return AVEngine.HashScanner.ScanM(filename);
           }
           else
               return null;
       }
    
       
       public bool Repair(Virus virus)
       {
           try
           {
               string result = VDB.GetRepair(virus.Name);

               if (result != null)
               {
                   if (result != "false" && result != string.Empty)
                   {
                       // repair
                       string hex = Security.DumpHex(virus.Location);
                       string hexresult = hex.Replace(result, "");
                       using (StreamWriter str = new StreamWriter(virus.Location))
                       {
                           str.Write(Security.HexAsciiConvert(hexresult));
                       }
                       return true;
                   }

               }

               return false;
           }
           catch
           {
               return false;
           }
           finally
           {

           }
       }
 
    }
}
