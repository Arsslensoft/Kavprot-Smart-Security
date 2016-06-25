using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using KAVE;

namespace SandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("File : " + args[0]);

                KavprotManager.Initialize(KavprotInitialization.Engine);
                Console.WriteLine("Copyright (c) 2010-2011 Arsslensoft. All rights reserved");
                Console.WriteLine("Copyright (c) 2010-2011 Arsslensoft Labs. All rights reserved");
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine("Executing Assembly....");
                KAVE.SandBox sand = new KAVE.SandBox(SettingsManager.AccessFiles, SettingsManager.AccessPerformanceCounter, SettingsManager.AccessRegistry, SettingsManager.AccessFileDialog, SettingsManager.AccessEnvironment, SettingsManager.AccessGUI, SettingsManager.AccessEventLog, SettingsManager.Security);
                sand.Start(args[0], Path.GetFileNameWithoutExtension(args[0]));

                Console.WriteLine("Execution Completed press any key to exit");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Execution Completed press any key to exit");
                Console.Read();
            }
        }
    }
}
