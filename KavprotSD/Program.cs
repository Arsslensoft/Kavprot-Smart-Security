using System;
using System.Collections.Generic;
using System.Windows.Forms;
using KAVE.BaseEngine;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.InteropServices;

namespace KavprotSD
{
    static class Program
    {
      static  Regex r = new Regex("=", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
            
                        EaseFilter.UnInstallDriver();
                        EaseFilter.InstallDriver();
                        bool ret = EaseFilter.StartFilter(@"71CD200D-6DD83DB4-47A900C4-1310C7C8-01061608"
                                                       , Environment.ProcessorCount
                                                       , new EaseFilter.FilterDelegate(FilterCallback)
                                                       , new EaseFilter.DisconnectDelegate(DisconnectCallback));

                        EaseFilter.ResetConfigData();
                        EaseFilter.SetFilterType((uint)(EaseFilter.FilterType.FILE_SYSTEM_CONTROL));
                        EaseFilter.SetConnectionTimeout(30);
                        EaseFilter.AddExcludedProcessId((uint)Process.GetCurrentProcess().Id);
                        foreach (Process p in Process.GetProcessesByName("Kavprot"))
                            EaseFilter.AddExcludedProcessId((uint)p.Id);
                
                  
                        foreach (Process p in Process.GetProcessesByName("KAIML"))
                            EaseFilter.AddExcludedProcessId((uint)p.Id);
                        foreach (Process p in Process.GetProcessesByName("KavprotCmd"))
                            EaseFilter.AddExcludedProcessId((uint)p.Id);
                        foreach (Process p in Process.GetProcessesByName("KavprotCloud"))
                            EaseFilter.AddExcludedProcessId((uint)p.Id);
                        foreach (Process p in Process.GetProcessesByName("KHS"))
                            EaseFilter.AddExcludedProcessId((uint)p.Id);
                        foreach (Process p in Process.GetProcessesByName("KPAVUPDATER"))
                            EaseFilter.AddExcludedProcessId((uint)p.Id);
                        foreach (Process p in Process.GetProcessesByName("Sandbox"))
                            EaseFilter.AddExcludedProcessId((uint)p.Id);

                        EaseFilter.AddFilterRule((uint)(EaseFilter.AccessFlag.ALLOW_DIRECTORY_LIST_ACCESS | EaseFilter.AccessFlag.ALLOW_QUERY_SECURITY_ACCESS | EaseFilter.AccessFlag.ALLOW_QUERY_INFORMATION_ACCESS | EaseFilter.AccessFlag.ALLOW_READ_ACCESS | EaseFilter.AccessFlag.ALLOW_SET_INFORMATION | EaseFilter.AccessFlag.ALLOW_SET_SECURITY_ACCESS | EaseFilter.AccessFlag.ALLOW_OPEN_WITH_READ_ACCESS | EaseFilter.AccessFlag.ALLOW_OPEN_WTIH_ACCESS_SYSTEM_SECURITY | EaseFilter.AccessFlag.ALLOW_FILE_SIZE_CHANGE)
                            , System.Windows.Forms.Application.StartupPath + "*", System.Windows.Forms.Application.StartupPath + "*");
                      
                        EaseFilter.RegisterIoRequest((uint)(EaseFilter.MessageType.POST_CREATE));
                       
                  
                    while (EaseFilter.IsFilterStarted)
                    {
                        Thread.Sleep(5000);
                    }
                    EaseFilter.UnInstallDriver();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + Environment.GetCommandLineArgs().Length.ToString());
            }
            
        }

        static bool ProcessReplyMessage(EaseFilter.MessageSendData messageSend, IntPtr replyDataPtr)
        {
            bool ret = true;

            try
            {
                EaseFilter.MessageReplyData messageReply = new EaseFilter.MessageReplyData();
                messageReply = (EaseFilter.MessageReplyData)Marshal.PtrToStructure(replyDataPtr, typeof(EaseFilter.MessageReplyData));

                messageReply.MessageId = messageSend.MessageId;
                messageReply.MessageType = messageSend.MessageType;


                messageReply.FilterStatus = 0;
                messageReply.ReturnStatus = (uint)NtStatus.Status.Success;


                Marshal.StructureToPtr(messageReply, replyDataPtr, true);
            }
            catch
            {

            }

            return ret;
        }
        static Boolean FilterCallback(IntPtr sendDataPtr, IntPtr replyDataPtr)
        {
            Boolean ret = true;

            try
            {
                EaseFilter.MessageSendData messageSend = new EaseFilter.MessageSendData();
                messageSend = (EaseFilter.MessageSendData)Marshal.PtrToStructure(sendDataPtr, typeof(EaseFilter.MessageSendData));

                if (EaseFilter.MESSAGE_SEND_VERIFICATION_NUMBER != messageSend.VerificationNumber)
                    return false;





                ret = ProcessReplyMessage(messageSend, replyDataPtr);


                return ret;
            }
            catch
            {
                return false;
            }

        }
       
        static string FormatDesiredAccess(uint desiredAccess)
        {
            string ret = string.Empty;

            foreach (WinData.DisiredAccess access in Enum.GetValues(typeof(WinData.DisiredAccess)))
            {
                if (access == (WinData.DisiredAccess)((uint)access & desiredAccess))
                {
                    ret += access.ToString() + "; ";
                }
            }

            return ret;
        }


        static void DisconnectCallback()
        {
        }
    }
}
