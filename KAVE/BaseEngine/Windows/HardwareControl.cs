using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace KAVE.Windows
{
   internal class EjectMedia
    {
        // Constants used in DLL methods
        const uint GENERICREAD = 0x80000000;
        const uint OPENEXISTING = 3;
        const uint IOCTL_STORAGE_EJECT_MEDIA = 2967560;
        const int INVALID_HANDLE = -1;

        // File Handle
        private static IntPtr fileHandle;
        private static uint returnedBytes;
        // Use Kernel32 via interop to access required methods
        // Get a File Handle
        [DllImport("kernel32", SetLastError = true)]
        static extern IntPtr CreateFile(string fileName,
        uint desiredAccess,
        uint shareMode,
        IntPtr attributes,
        uint creationDisposition,
        uint flagsAndAttributes,
        IntPtr templateFile);
        [DllImport("kernel32", SetLastError = true)]
        static extern int CloseHandle(IntPtr driveHandle);
        [DllImport("kernel32", SetLastError = true)]
        static extern bool DeviceIoControl(IntPtr driveHandle,
        uint IoControlCode,
        IntPtr lpInBuffer,
        uint inBufferSize,
        IntPtr lpOutBuffer,
        uint outBufferSize,
        ref uint lpBytesReturned,
                 IntPtr lpOverlapped);

        public static void Eject(string driveLetter)
        {
            try
            {
                // Create an handle to the drive
                fileHandle = CreateFile(driveLetter,
                 GENERICREAD,
                 0,
                 IntPtr.Zero,
                 OPENEXISTING,
                 0,
                  IntPtr.Zero);
                if ((int)fileHandle != INVALID_HANDLE)
                {
                    // Eject the disk
                    DeviceIoControl(fileHandle,
                     IOCTL_STORAGE_EJECT_MEDIA,
                     IntPtr.Zero, 0,
                     IntPtr.Zero, 0,
                     ref returnedBytes,
                            IntPtr.Zero);
                }
            }
            catch
            {
                throw new Exception(Marshal.GetLastWin32Error().ToString());
            }
            finally
            {
                // Close Drive Handle
                CloseHandle(fileHandle);
                fileHandle = IntPtr.Zero;
            }
        }
    }
   public class Native
   {
       [DllImport("user32.dll", CharSet = CharSet.Auto)]
       public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, DEV_BROADCAST_DEVICEINTERFACE NotificationFilter, UInt32 Flags);

       [DllImport("user32.dll", CharSet = CharSet.Auto)]
       public static extern uint UnregisterDeviceNotification(IntPtr hHandle);

       [DllImport("setupapi.dll", SetLastError = true)]
       public static extern IntPtr SetupDiGetClassDevs(ref Guid gClass, UInt32 iEnumerator, IntPtr hParent, UInt32 nFlags);

       [DllImport("setupapi.dll", SetLastError = true)]
       public static extern int SetupDiDestroyDeviceInfoList(IntPtr lpInfoSet);

       [DllImport("setupapi.dll", SetLastError = true)]
       public static extern bool SetupDiEnumDeviceInfo(IntPtr lpInfoSet, UInt32 dwIndex, SP_DEVINFO_DATA devInfoData);

       [DllImport("setupapi.dll", SetLastError = true)]
       public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr lpInfoSet, SP_DEVINFO_DATA DeviceInfoData, UInt32 Property, UInt32 PropertyRegDataType, StringBuilder PropertyBuffer, UInt32 PropertyBufferSize, IntPtr RequiredSize);

       [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
       public static extern bool SetupDiSetClassInstallParams(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, IntPtr ClassInstallParams, int ClassInstallParamsSize);

       [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
       public static extern Boolean SetupDiCallClassInstaller(UInt32 InstallFunction, IntPtr DeviceInfoSet, IntPtr DeviceInfoData);

       // Structure with information for RegisterDeviceNotification.
       [StructLayout(LayoutKind.Sequential)]
       public struct DEV_BROADCAST_HANDLE
       {
           public int dbch_size;
           public int dbch_devicetype;
           public int dbch_reserved;
           public IntPtr dbch_handle;
           public IntPtr dbch_hdevnotify;
           public Guid dbch_eventguid;
           public long dbch_nameoffset;
           public byte dbch_data;
           public byte dbch_data1;
       }

       // Struct for parameters of the WM_DEVICECHANGE message
       [StructLayout(LayoutKind.Sequential)]
       public class DEV_BROADCAST_DEVICEINTERFACE
       {
           public int dbcc_size;
           public int dbcc_devicetype;
           public int dbcc_reserved;
       }

       //SP_DEVINFO_DATA
       [StructLayout(LayoutKind.Sequential)]
       public class SP_DEVINFO_DATA
       {
           public int cbSize;
           public Guid classGuid;
           public int devInst;
           public ulong reserved;
       };

       [StructLayout(LayoutKind.Sequential)]
       public class SP_DEVINSTALL_PARAMS
       {
           public int cbSize;
           public int Flags;
           public int FlagsEx;
           public IntPtr hwndParent;
           public IntPtr InstallMsgHandler;
           public IntPtr InstallMsgHandlerContext;
           public IntPtr FileQueue;
           public IntPtr ClassInstallReserved;
           public int Reserved;
           [MarshalAs(UnmanagedType.LPTStr)]
           public string DriverPath;
       };

       [StructLayout(LayoutKind.Sequential)]
       public class SP_PROPCHANGE_PARAMS
       {
           public SP_CLASSINSTALL_HEADER ClassInstallHeader = new SP_CLASSINSTALL_HEADER();
           public int StateChange;
           public int Scope;
           public int HwProfile;
       };

       [StructLayout(LayoutKind.Sequential)]
       public class SP_CLASSINSTALL_HEADER
       {
           public int cbSize;
           public int InstallFunction;
       };

       //PARMS
       public const int DIGCF_ALLCLASSES = (0x00000004);
       public const int DIGCF_PRESENT = (0x00000002);
       public const int INVALID_HANDLE_VALUE = -1;
       public const int SPDRP_DEVICEDESC = (0x00000000);
       public const int MAX_DEV_LEN = 1000;
       public const int DEVICE_NOTIFY_WINDOW_HANDLE = (0x00000000);
       public const int DEVICE_NOTIFY_SERVICE_HANDLE = (0x00000001);
       public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = (0x00000004);
       public const int DBT_DEVTYP_DEVICEINTERFACE = (0x00000005);
       public const int DBT_DEVNODES_CHANGED = (0x0007);
       public const int WM_DEVICECHANGE = (0x0219);
       public const int DIF_PROPERTYCHANGE = (0x00000012);
       public const int DICS_FLAG_GLOBAL = (0x00000001);
       public const int DICS_FLAG_CONFIGSPECIFIC = (0x00000002);
       public const int DICS_ENABLE = (0x00000001);
       public const int DICS_DISABLE = (0x00000002);
   }
   public static class HardwareControl
    {
        #region native
        public static bool HookHardwareNotifications(IntPtr callback, bool UseWindowHandle)
        {
            try
            {
                Native.DEV_BROADCAST_DEVICEINTERFACE dbdi = new Native.DEV_BROADCAST_DEVICEINTERFACE();
                dbdi.dbcc_size = Marshal.SizeOf(dbdi);
                dbdi.dbcc_reserved = 0;
                dbdi.dbcc_devicetype = Native.DBT_DEVTYP_DEVICEINTERFACE;
                if (UseWindowHandle)
                {
                    Native.RegisterDeviceNotification(callback,
                        dbdi,
                        Native.DEVICE_NOTIFY_ALL_INTERFACE_CLASSES |
                        Native.DEVICE_NOTIFY_WINDOW_HANDLE);
                }
                else
                {
                    Native.RegisterDeviceNotification(callback,
                        dbdi,
                        Native.DEVICE_NOTIFY_ALL_INTERFACE_CLASSES |
                        Native.DEVICE_NOTIFY_SERVICE_HANDLE);
                }
                return true;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return false;
            }
        }
        public static void CutLooseHardwareNotifications(IntPtr callback)
        {
            try
            {
                Native.UnregisterDeviceNotification(callback);
            }
            catch
            {
                //Just being extra cautious since the code is unmanged
            }
        }
        private static bool ChangeIt(IntPtr hDevInfo, Native.SP_DEVINFO_DATA devInfoData, bool bEnable)
        {
            try
            {
                //Marshalling vars
                int szOfPcp;
                IntPtr ptrToPcp;
                int szDevInfoData;
                IntPtr ptrToDevInfoData;

                Native.SP_PROPCHANGE_PARAMS pcp = new Native.SP_PROPCHANGE_PARAMS();
                if (bEnable)
                {
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(Native.SP_CLASSINSTALL_HEADER));
                    pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE;
                    pcp.StateChange = Native.DICS_ENABLE;
                    pcp.Scope = Native.DICS_FLAG_GLOBAL;
                    pcp.HwProfile = 0;

                    //Marshal the params
                    szOfPcp = Marshal.SizeOf(pcp);
                    ptrToPcp = Marshal.AllocHGlobal(szOfPcp);
                    Marshal.StructureToPtr(pcp, ptrToPcp, true);
                    szDevInfoData = Marshal.SizeOf(devInfoData);
                    ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData);

                    if (Native.SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(typeof(Native.SP_PROPCHANGE_PARAMS))))
                    {
                        Native.SetupDiCallClassInstaller(Native.DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData);
                    }
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(Native.SP_CLASSINSTALL_HEADER));
                    pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE;
                    pcp.StateChange = Native.DICS_ENABLE;
                    pcp.Scope = Native.DICS_FLAG_CONFIGSPECIFIC;
                    pcp.HwProfile = 0;
                }
                else
                {
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(Native.SP_CLASSINSTALL_HEADER));
                    pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE;
                    pcp.StateChange = Native.DICS_DISABLE;
                    pcp.Scope = Native.DICS_FLAG_CONFIGSPECIFIC;
                    pcp.HwProfile = 0;
                }
                //Marshal the params
                szOfPcp = Marshal.SizeOf(pcp);
                ptrToPcp = Marshal.AllocHGlobal(szOfPcp);
                Marshal.StructureToPtr(pcp, ptrToPcp, true);
                szDevInfoData = Marshal.SizeOf(devInfoData);
                ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData);
                Marshal.StructureToPtr(devInfoData, ptrToDevInfoData, true);

                bool rslt1 = Native.SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(typeof(Native.SP_PROPCHANGE_PARAMS)));
                bool rstl2 = Native.SetupDiCallClassInstaller(Native.DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData);
                if ((!rslt1) || (!rstl2))
                {
                    throw new Exception("Unable to change device state!");
                   
                }
                else
                {
                    return true;
                }
            }
            catch
            {

                return false;
            }
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);

        #endregion
    
       /// <summary>
       /// Eject a drive from drive letter
       /// </summary>
       /// <param name="letter">drive letter</param>
       public static void EjectDrive(string letter)
       {
           try
           {
               EjectMedia.Eject(@"\\.\" + letter + ":");
           }
           catch
           {

           }
       }
       /// <summary>
       /// Turn off monitor
       /// </summary>
       public static void TurnOffMonitor()
       {
           SendMessage(0xFFFF, 0x0112, 0xF170, 2);//DLL function
       }
       /// <summary>
       /// Beep
       /// </summary>
       /// <param name="frequency">frequency between 37 and 32767</param>
       /// <param name="time">time in milliseconds</param>
       public static void Beep(int frequency, int time)
       {
           Console.Beep(frequency, time);
       }
        /// <summary>
       /// Get all devices in the computer
     /// </summary>
     /// <returns>list of all windows devices</returns>
       public static string[] GetAllDevices()
       {
           List<string> HWList = new List<string>();
           try
           {
               Guid myGUID = System.Guid.Empty;
               IntPtr hDevInfo = Native.SetupDiGetClassDevs(ref myGUID, 0, IntPtr.Zero, Native.DIGCF_ALLCLASSES | Native.DIGCF_PRESENT);
               if (hDevInfo.ToInt32() == Native.INVALID_HANDLE_VALUE)
               {
                   throw new Exception("Invalid Handle");
               }
               Native.SP_DEVINFO_DATA DeviceInfoData;
               DeviceInfoData = new Native.SP_DEVINFO_DATA();
               DeviceInfoData.cbSize = 28;
               //is devices exist for class
               DeviceInfoData.devInst = 0;
               DeviceInfoData.classGuid = System.Guid.Empty;
               DeviceInfoData.reserved = 0;
               UInt32 i;
               StringBuilder DeviceName = new StringBuilder("");
               DeviceName.Capacity = Native.MAX_DEV_LEN;
               for (i = 0; Native.SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData); i++)
               {
                   //Declare vars
                   while (!Native.SetupDiGetDeviceRegistryProperty(hDevInfo,
                                                                   DeviceInfoData,
                                                                   Native.SPDRP_DEVICEDESC,
                                                                   0,
                                                                   DeviceName,
                                                                   Native.MAX_DEV_LEN,
                                                                   IntPtr.Zero))
                   {
                       //Skip
                   }
                   HWList.Add(DeviceName.ToString());
               }
               Native.SetupDiDestroyDeviceInfoList(hDevInfo);
           }
           catch (Exception ex)
           {
               throw new Exception("Failed to enumerate device tree!", ex);
           }
           return HWList.ToArray();
       }
       /// <summary>
       /// Set device state
       /// </summary>
       /// <param name="match">devices to disable</param>
       /// <param name="bEnable">device state (true or false)</param>
       /// <returns>return execution state</returns>
       public static bool SetDeviceState(string[] match, bool bEnable)
       {
           try
           {
               Guid myGUID = System.Guid.Empty;
               IntPtr hDevInfo = Native.SetupDiGetClassDevs(ref myGUID, 0, IntPtr.Zero, Native.DIGCF_ALLCLASSES | Native.DIGCF_PRESENT);
               if (hDevInfo.ToInt32() == Native.INVALID_HANDLE_VALUE)
               {
                   return false;
               }
               Native.SP_DEVINFO_DATA DeviceInfoData;
               DeviceInfoData = new Native.SP_DEVINFO_DATA();
               DeviceInfoData.cbSize = 28;
               //is devices exist for class
               DeviceInfoData.devInst = 0;
               DeviceInfoData.classGuid = System.Guid.Empty;
               DeviceInfoData.reserved = 0;
               UInt32 i;
               StringBuilder DeviceName = new StringBuilder("");
               DeviceName.Capacity = Native.MAX_DEV_LEN;
               for (i = 0; Native.SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData); i++)
               {
                   //Declare vars
                   while (!Native.SetupDiGetDeviceRegistryProperty(hDevInfo,
                                                                   DeviceInfoData,
                                                                   Native.SPDRP_DEVICEDESC,
                                                                   0,
                                                                   DeviceName,
                                                                   Native.MAX_DEV_LEN,
                                                                   IntPtr.Zero))
                   {
                       //Skip
                   }
                   bool bMatch = true;
                   foreach (string search in match)
                   {
                       if (!DeviceName.ToString().ToLower().Contains(search.ToLower()))
                       {
                           bMatch = false;
                           break;
                       }
                   }
                   if (bMatch)
                   {
                       ChangeIt(hDevInfo, DeviceInfoData, bEnable);
                   }
               }
               Native.SetupDiDestroyDeviceInfoList(hDevInfo);
           }
           catch (Exception ex)
           {
               throw new Exception("Failed to enumerate device tree!", ex);
          
           }
           return true;
       }


      
    }
}
