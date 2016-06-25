using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using KAVE.BaseEngine;

namespace KAVE
{
   public static class ScanSolutions
    {
       public static void PutQuarantine(string file, string infec)
       {
           Quarantine.Store(file, infec);
       }
       public static void Remove(string file)
       {
           try
           {
               if (FileFormat.GetFileFormat(file).Name == "PE-TYPE-SCANNER")
               {
                  // kill processes
                   foreach (Process p in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(file)))
                   {
                       p.Kill();
                   }

                   File.Delete(file);
               }
               else
               {
                   File.Delete(file);
               }
               
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex);
           }
           finally
           {

           }
       }
       private static int _bufferSize = 16384;
       private static string ReadFile(string filename)
       {
           StringBuilder stringBuilder = new StringBuilder();
           FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);

           using (StreamReader streamReader = new StreamReader(fileStream))
           {
               char[] fileContents = new char[_bufferSize];
               int charsRead = streamReader.Read(fileContents, 0, _bufferSize);

               // Can't do much with 0 bytes
               if (charsRead == 0)
                   throw new Exception("File is 0 bytes");

               while (charsRead > 0)
               {
                   stringBuilder.Append(fileContents);
                   charsRead = streamReader.Read(fileContents, 0, _bufferSize);
               }
               streamReader.Close();
           }
           return stringBuilder.ToString();
       }

       public static byte[] FromHex(string hex)
       {
           byte[] raw = new byte[hex.Length / 2];
           for (int i = 0; i < raw.Length; i++)
           {
               raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
           }
           return raw;
       }
    }
}
