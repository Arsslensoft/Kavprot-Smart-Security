using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace KAVE.Engine
{
    public class FileHelper
    {
        public static List<string> GetCloudDriveFiles()
        {
            // 1.
            // Store results in the file results list.
            List<string> result = new List<string>();

            // 2.
            // Store a stack of our directories.
            Stack<string> stack = new Stack<string>();

            // 3.
            // Add initial directory.
            foreach (string drive in Environment.GetLogicalDrives())
            {

                stack.Push(drive);

            }


            // 4.
            // Continue while there are directories to process
            while (stack.Count > 0)
            {
                // A.
                // Get top directory
                string dir = stack.Pop();

                try
                {
                    // B
                    // Add all files at this directory to the result List.
                    result.AddRange(Directory.GetFiles(dir, "*.exe"));
                    result.AddRange(Directory.GetFiles(dir, "*.dll"));
                    result.AddRange(Directory.GetFiles(dir, "*.inf"));
                    result.AddRange(Directory.GetFiles(dir, "*.sys"));
                    result.AddRange(Directory.GetFiles(dir, "*.com"));
                    result.AddRange(Directory.GetFiles(dir, "*.vbs"));
                    result.AddRange(Directory.GetFiles(dir, "*.js"));
                    result.AddRange(Directory.GetFiles(dir, "*.ini"));
                    // C
                    // Add all directories at this directory.
                    foreach (string dn in Directory.GetDirectories(dir))
                    {
                        stack.Push(dn);
                    }
                }
                catch
                {
                    // D
                    // Could not open the directory
                }
            }
            return result;
        }
        public static List<string> GetIDPDriveFiles()
        {
            // 1.
            // Store results in the file results list.
            List<string> result = new List<string>();

            // 2.
            // Store a stack of our directories.
            Stack<string> stack = new Stack<string>();

            // 3.
            // Add initial directory.
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady == true)
                {
                    stack.Push(drive.Name);
                }
                else
                {

                }
            }


            // 4.
            // Continue while there are directories to process
            while (stack.Count > 0)
            {
                // A.
                // Get top directory
                string dir = stack.Pop();

                try
                {
                    // B
                    // Add all files at this directory to the result List.
                    result.AddRange(Directory.GetFiles(dir, "*.exe"));
                    result.AddRange(Directory.GetFiles(dir, "*.dll"));
                    result.AddRange(Directory.GetFiles(dir, "*.msi"));
                    // C
                    // Add all directories at this directory.
                    foreach (string dn in Directory.GetDirectories(dir))
                    {
                        stack.Push(dn);
                    }
                }
                catch
                {
                    // D
                    // Could not open the directory
                }
            }
            return result;
        }
        public static List<string> GetDriveFiles()
        {
            // 1.
            // Store results in the file results list.
            List<string> result = new List<string>();

            // 2.
            // Store a stack of our directories.
            Stack<string> stack = new Stack<string>();

            // 3.
            // Add initial directory.
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady == true)
                {
                    stack.Push(drive.Name);
                }
                else
                {

                }
            }


            // 4.
            // Continue while there are directories to process
            while (stack.Count > 0)
            {
                // A.
                // Get top directory
                string dir = stack.Pop();

                try
                {
                    // B
                    // Add all files at this directory to the result List.
                    result.AddRange(Directory.GetFiles(dir, "*.*"));

                    // C
                    // Add all directories at this directory.
                    foreach (string dn in Directory.GetDirectories(dir))
                    {
                        stack.Push(dn);
                    }
                }
                catch
                {
                    // D
                    // Could not open the directory
                }
            }
            return result;
        }
        public static List<string> GetFilesRecursive(string b)
        {
            // 1.
            // Store results in the file results list.
            List<string> result = new List<string>();

            // 2.
            // Store a stack of our directories.
            Stack<string> stack = new Stack<string>();

            // 3.
            // Add initial directory.
            stack.Push(b);

            // 4.
            // Continue while there are directories to process
            while (stack.Count > 0)
            {
                // A.
                // Get top directory
                string dir = stack.Pop();

                try
                {
                    // B
                    // Add all files at this directory to the result List.
                    result.AddRange(Directory.GetFiles(dir, "*.*"));

                    // C
                    // Add all directories at this directory.
                    foreach (string dn in Directory.GetDirectories(dir))
                    {
                        stack.Push(dn);
                    }
                }
                catch
                {
                    // D
                    // Could not open the directory
                }
            }
            return result;
        }
        public static List<string> GetRealTimeFiles()
        {

            List<string> lst = new List<string>();
            try
            {
                foreach (Process proc in Process.GetProcesses())
                {
                    if (proc.MachineName == System.Windows.Forms.SystemInformation.ComputerName)
                    {
                        if (File.Exists(proc.MainModule.FileName))
                        {
                            lst.Add(proc.MainModule.FileName);
                            foreach (ProcessModule procm in proc.Modules)
                            {
                                if (!lst.Contains(procm.FileName))
                                {
                                    if (File.Exists(procm.FileName))
                                    {
                                        lst.Add(procm.FileName);
                                    }
                                    
                                 
                                }
                            }
                        }
                    
                    }
                  
                }
            }
            finally
            {

            }
            return lst;
        }

    }
}
