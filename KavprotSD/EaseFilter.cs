///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2011 EaseFilter Technologies Inc.
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace KAVE.BaseEngine
{

    static public class EaseFilter
    {
        public delegate Boolean FilterDelegate(IntPtr sendData, IntPtr replyData);
        public delegate void DisconnectDelegate();
        static GCHandle gchFilter;
        static GCHandle gchDisconnect;
        static bool isFilterStarted = false;
        public const int MAX_FILE_NAME_LENGTH = 512;
        public const int MAX_SID_LENGTH = 256;
        public const int MAX_MESSAGE_LENGTH = 65536;
        public const int MAX_PATH = 260;
        public const int MAX_ERROR_MESSAGE_SIZE = 1024;

        public const uint MESSAGE_SEND_VERIFICATION_NUMBER = 0xFF000001;

        public enum MessageType : uint
        {
            PRE_CREATE = 0x00000001,
            POST_CREATE = 0x00000002,
            PRE_FASTIO_READ = 0x00000004,
            POST_FASTIO_READ = 0x00000008,
            PRE_CACHE_READ = 0x00000010,
            POST_CACHE_READ = 0x00000020,
            PRE_NOCACHE_READ = 0x00000040,
            POST_NOCACHE_READ = 0x00000080,
            PRE_PAGING_IO_READ = 0x00000100,
            POST_PAGING_IO_READ = 0x00000200,
            PRE_FASTIO_WRITE = 0x00000400,
            POST_FASTIO_WRITE = 0x00000800,
            PRE_CACHE_WRITE = 0x00001000,
            POST_CACHE_WRITE = 0x00002000,
            PRE_NOCACHE_WRITE = 0x00004000,
            POST_NOCACHE_WRITE = 0x00008000,
            PRE_PAGING_IO_WRITE = 0x00010000,
            POST_PAGING_IO_WRITE = 0x00020000,
            PRE_QUERY_INFORMATION = 0x00040000,
            POST_QUERY_INFORMATION = 0x00080000,
            PRE_SET_INFORMATION = 0x00100000,
            POST_SET_INFORMATION = 0x00200000,
            PRE_DIRECTORY = 0x00400000,
            POST_DIRECTORY = 0x00800000,
            PRE_QUERY_SECURITY = 0x01000000,
            POST_QUERY_SECURITY = 0x02000000,
            PRE_SET_SECURITY = 0x04000000,
            POST_SET_SECURITY = 0x08000000,
            PRE_CLEANUP = 0x10000000,
            POST_CLEANUP = 0x20000000,
            PRE_CLOSE = 0x40000000,
            POST_CLOSE = 0x80000000,

        }

        public const uint ALLOW_MAX_RIGHT_ACCESS = 0xfffffff0;

        public enum AccessFlag : uint
        {
            EXCLUDE_FILTER_RULE = 0X00000000,
            INCLUDE_FILTER_RULE = 0x00000001,
            REPARSE_FILE_OPEN = 0x00000002,
            HIDE_FILES_IN_DIRECTORY_BROWSING = 0x00000004,
            ALLOW_OPEN_WTIH_ACCESS_SYSTEM_SECURITY = 0x00000010,
            ALLOW_OPEN_WITH_READ_ACCESS = 0x00000020,
            ALLOW_OPEN_WITH_WRITE_ACCESS = 0x00000040,
            ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS = 0x00000080,
            ALLOW_OPEN_WITH_DELETE_ACCESS = 0x00000100,
            ALLOW_READ_ACCESS = 0x00000200,
            ALLOW_WRITE_ACCESS = 0x00000400,
            ALLOW_QUERY_INFORMATION_ACCESS = 0x00000800,
            //allow to change file time and file attributes
            ALLOW_SET_INFORMATION = 0x00001000,
            ALLOW_FILE_RENAME = 0x00002000,
            ALLOW_FILE_DELETE = 0x00004000,
            ALLOW_FILE_SIZE_CHANGE = 0x00008000,
            ALLOW_QUERY_SECURITY_ACCESS = 0x00010000,
            ALLOW_SET_SECURITY_ACCESS = 0x00020000,
            ALLOW_DIRECTORY_LIST_ACCESS = 0x00040000,
        }

        public enum FilterType : uint
        {
            FILE_SYSTEM_MONITOR = 0,
            FILE_SYSTEM_CONTROL = 1,
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MessageSendData
        {
            public uint MessageId;          //this is the request sequential number.
            public IntPtr FileObject;       //the address of FileObject,it is equivalent to file handle,it is unique per file stream open.
            public IntPtr FsContext;        //the address of FsContext,it is unique per file.
            public uint MessageType;        //the I/O request type.
            public uint ProcessId;          //the process ID for the process associated with the thread that originally requested the I/O operation.
            public uint ThreadId;           //the thread ID which requested the I/O operation.
            public long Offset;             //the read/write offset.
            public uint Length;             //the read/write length.
            public long FileSize;           //the size of the file for the I/O operation.
            public long TransactionTime;    //the transaction time in UTC of this request.
            public long CreationTime;       //the creation time in UTC of the file.
            public long LastAccessTime;     //the last access time in UTC of the file.
            public long LastWriteTime;      //the last write time in UTC of the file.
            public uint FileAttributes;     //the file attributes.
            public uint DesiredAccess;      //the DesiredAccess for file open, please reference CreateFile windows API.
            public uint Disposition;        //the Disposition for file open, please reference CreateFile windows API.
            public uint SharedAccess;       //the SharedAccess for file open, please reference CreateFile windows API.
            public uint CreateOptions;      //the CreateOptions for file open, please reference CreateFile windows API.
            public uint CreateStatus;       //the CreateStatus after file was openned, please reference CreateFile windows API.
            public uint InfoClass;          //the information class or security information
            public uint Status;             //the I/O status which returned from file system.
            public uint FileNameLength;     //the file name length in byte.
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_FILE_NAME_LENGTH)]
            public string FileName;         //the file name of the I/O operation.
            public uint SidLength;          //the length of the security identifier.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SID_LENGTH)]
            public byte[] Sid;              //the security identifier data.
            public uint DataBufferLength;   //the data buffer length.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_MESSAGE_LENGTH)]
            public byte[] DataBuffer;       //the data buffer which contains read/write/query information/set information data.
            public uint VerificationNumber; //the verification number which verifiys the data structure integerity.
        }

        public enum FilterStatus : uint
        {
            FILTER_MESSAGE_IS_DIRTY = 0x00000001,
            FILTER_COMPLETE_PRE_OPERATION = 0x00000002, //ONLY FOR PRE CALL OPERATION,the IO won't pass down to the lower drivers and file system.
            FILTER_DATA_BUFFER_IS_UPDATED = 0x00000004, //only for pre create,to reparse the file open to the new file name.	
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MessageReplyData
        {
            public uint MessageId;
            public uint MessageType;
            public uint ReturnStatus;
            public uint FilterStatus;
            public uint DataBufferLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 65536)]
            public byte[] DataBuffer;
        }

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool InstallDriver();

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool UnInstallDriver();

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool SetRegistrationKey([MarshalAs(UnmanagedType.LPStr)]string key);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool Disconnect();

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool GetLastErrorMessage(
            [MarshalAs(UnmanagedType.LPWStr)] 
            string errorMessage,
            ref int messageLength);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RegisterMessageCallback(
            int threadCount,
            IntPtr filterCallback,
            IntPtr disconnectCallback);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool ResetConfigData();

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool SetFilterType(uint filterType);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool SetConnectionTimeout(uint timeOutInSeconds);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddFilterRule(
         uint accessFlag,
        [MarshalAs(UnmanagedType.LPWStr)]string filterMask,        
        [MarshalAs(UnmanagedType.LPWStr)] string reparseMask);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveFilterRule(
        [MarshalAs(UnmanagedType.LPWStr)] string filterMask);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool AddExcludedProcessId(uint processId);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RemoveExcludeProcessId(uint processId);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool RegisterIoRequest(uint requestRegistration);

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool GetFileHandleInFilter(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
             IntPtr fileHandle);

         [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool ConvertSidToStringSid(
            [In] IntPtr sid,
            [Out] out IntPtr sidString);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr hMem);

        public static string GetLastErrorMessage()
        {
            int len = MAX_ERROR_MESSAGE_SIZE;
            string errorMessage = new string((char)0, len);

            if (!GetLastErrorMessage(errorMessage, ref len))
            {
                errorMessage = new string((char)0, len);
                if (!GetLastErrorMessage(errorMessage, ref len))
                {
                    return "failed to get last error message.";
                }
            }

            return errorMessage;
        }

        static public bool StartFilter(string registerKey,int threadCount, FilterDelegate filterCallback, DisconnectDelegate disconnectCallback)
        {
            if (!isFilterStarted)
            {
                if (!SetRegistrationKey(registerKey))
                {
                    return false;
                }

                gchFilter = GCHandle.Alloc(filterCallback);
                IntPtr filterCallbackPtr = Marshal.GetFunctionPointerForDelegate(filterCallback);

                gchDisconnect = GCHandle.Alloc(disconnectCallback);
                IntPtr disconnectCallbackPtr = Marshal.GetFunctionPointerForDelegate(disconnectCallback);

                isFilterStarted = RegisterMessageCallback(threadCount, filterCallbackPtr, disconnectCallbackPtr);

            }

            return isFilterStarted;
        }

        static public void StopFilter()
        {
            if (isFilterStarted)
            {
                Disconnect();
                gchFilter.Free();
                gchDisconnect.Free();
                isFilterStarted = false;
            }

            return;
        }

        static public bool IsFilterStarted
        {
            get { return isFilterStarted; }
        }

    }
}
