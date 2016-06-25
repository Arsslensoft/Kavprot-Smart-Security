using System;
using System.Collections.Generic;
using System.Text;
using KDM.Iso9660;
using System.IO;
using System.Text.RegularExpressions;
using DevComponents.DotNetBar.Controls;
using KAVE.Engine;

namespace KAVE.BaseEngine
{
   public static class SystemRescue
   {

       public static void MakeRescue(string Directory, string Destination, ProgressBarX progress)
       {
           int i = 0;
           CDBuilder builder = new CDBuilder();
           builder.VolumeIdentifier = "SYS_RESCUE_DISK";
           builder.UseJoliet = true;
        List<string> files =  FileHelper.GetFilesRecursive(Directory);
      
           GUI.UpdateProgress(progress, 0, files.Count); 
       
        using (StreamWriter str = new StreamWriter(Directory + @"\SYSRESCUEINFO.KPAVRSCI", true))
        {
            str.WriteLine("BEGIN SYS_RESC_DISK|" + DateTime.Now.ToString() + "|KAVPROT_ISO_RESCUE|"+ Environment.MachineName + "|" + Environment.OSVersion.ToString() + "|" + Environment.UserName );
            foreach (string file in files)
            {
                i++;
                try
                {
                    GUI.UpdateProgress(progress, i, files.Count); 
                    builder.AddFile(Path.GetFileName(file), file);
                    str.WriteLine(file);
                    builder.Build(Destination);
                }
                catch (Exception ex)
                {
                    AntiCrash.LogException(ex,2);
                }
                finally
                {

                }
            }
            str.WriteLine("END SYS_RESC_DISK " + files.Count);            
        }
        builder.AddFile("SYSRESCUEINFO.KPAVRSCI", Directory + @"\SYSRESCUEINFO.KPAVRSCI");

        builder.Build(Destination);
       }
   }
}
