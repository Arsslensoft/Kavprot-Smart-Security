namespace KProxy
{
    using System;
    using System.Runtime.InteropServices;

    public static class URLMonInterop
    {
        private const uint INTERNET_OPEN_TYPE_DIRECT = 1;
        private const uint INTERNET_OPEN_TYPE_PRECONFIG = 0;
        private const uint INTERNET_OPEN_TYPE_PRECONFIG_WITH_NO_AUTOPROXY = 4;
        private const uint INTERNET_OPEN_TYPE_PROXY = 3;
        private const uint INTERNET_OPTION_PROXY = 0x26;
        private const uint INTERNET_OPTION_REFRESH = 0x25;
        private const uint URLMON_OPTION_USERAGENT = 0x10000001;
        private const uint URLMON_OPTION_USERAGENT_REFRESH = 0x10000002;

        public static string GetProxyInProcess()
        {
            int size = 0;
            byte[] optionInfo = new byte[1];
            size = optionInfo.Length;
            if (!InternetQueryOption(IntPtr.Zero, 0x26, optionInfo, ref size) && (size != optionInfo.Length))
            {
                optionInfo = new byte[size];
                size = optionInfo.Length;
                bool flag = InternetQueryOption(IntPtr.Zero, 0x26, optionInfo, ref size);
            }
            return Utilities.ByteArrayToHexView(optionInfo, 0x10);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("wininet.dll", CharSet=CharSet.Ansi, SetLastError=true)]
        private static extern bool InternetQueryOption(IntPtr hInternet, int Option, byte[] OptionInfo, ref int size);
        public static void ResetProxyInProcessToDefault()
        {
            UrlMkSetSessionOptionProxy(0x25, null, 0, 0);
        }

        public static void SetProxyDisabledForProcess()
        {
            INTERNET_PROXY_INFO structure = new INTERNET_PROXY_INFO();
            structure.dwAccessType = 1;
            structure.lpszProxy = (string) (structure.lpszProxyBypass = null);
            uint dwLen = (uint) Marshal.SizeOf(structure);
            UrlMkSetSessionOptionProxy(0x26, structure, dwLen, 0);
        }

        public static void SetProxyInProcess(string sProxy, string sBypassList)
        {
            INTERNET_PROXY_INFO structure = new INTERNET_PROXY_INFO();
            structure.dwAccessType = 3;
            structure.lpszProxy = sProxy;
            structure.lpszProxyBypass = sBypassList;
            uint dwLen = (uint) Marshal.SizeOf(structure);
            UrlMkSetSessionOptionProxy(0x26, structure, dwLen, 0);
        }

        public static void SetUAStringInProcess(string sUA)
        {
            UrlMkSetSessionOptionUA(0x10000001, sUA, (uint) sUA.Length, 0);
        }

        [DllImport("urlmon.dll", EntryPoint="UrlMkSetSessionOption", CharSet=CharSet.Auto, SetLastError=true)]
        private static extern int UrlMkSetSessionOptionProxy(uint dwOption, INTERNET_PROXY_INFO structNewProxy, uint dwLen, uint dwZero);
        [DllImport("urlmon.dll", EntryPoint="UrlMkSetSessionOption", CharSet=CharSet.Ansi, SetLastError=true)]
        private static extern int UrlMkSetSessionOptionUA(uint dwOption, string sNewUA, uint dwLen, uint dwZero);

        [StructLayout(LayoutKind.Sequential)]
        private class INTERNET_PROXY_INFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint dwAccessType;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpszProxy;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpszProxyBypass;
        }
    }
}

