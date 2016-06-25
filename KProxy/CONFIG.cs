namespace KProxy
{
    using Microsoft.Win32;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Net.NetworkInformation;
    using System.Runtime.CompilerServices;
    using System.Security.Authentication;
    using System.Text;
    using System.Windows.Forms;
    using System.Reflection;

    public class KPCONFIG
    {
     

        private static int _iReverseProxyForPort;
        internal static bool bAlwaysShowTrayIcon = false;
        public static bool bAttachOnBoot = true;
        public static bool bAutoLoadScript = true;
        public static bool bAutoProxyLogon = false;
        public static bool bBreakOnImages = false;
        public static bool bCaptureCONNECT = true;
        public static bool bCaptureFTP = false;
        internal static bool bCheckCompressionIntegrity = false;
        public static bool bDebugCertificateGeneration = true;
        internal static bool bDebugSpew = false;
        public static bool bEnableIPv6 = (Environment.OSVersion.Version.Major > 5);
        public static bool bForwardToGateway = true;
        public static bool bHideOnMinimize = false;
        internal static bool bHookAllConnections = true;
        internal static bool bHookWithPAC = false;
        private static bool bIgnoreServerCertErrors = false;
        internal static bool bIsBeta = false;
        internal static bool bIsViewOnly = false;
        public static bool bLoadExtensions = true;
        public static bool bLoadInspectors = true;
        public static bool bLoadScript = true;
        public static bool bMapSocketToProcess = true;
        public static bool bMITM_HTTPS = false;
        private static bool bQuietMode = !Environment.UserInteractive;
        public static bool bReportHTTPErrors = true;
        public static bool bReuseClientSockets = true;
        public static bool bReuseServerSockets = true;
        internal static bool bRunningOnCLRv4 = (Environment.Version.Major > 3);
        internal static bool bShowDefaultClientCertificateNeededPrompt = true;
        public static bool bStackedLayout = false;
        internal static bool bStreamAudioVideo = true;
        public static bool bUseAESForSAZ = true;
        public static bool bUseEventLogForExceptions = false;
        internal static bool bUseXceedDecompressForDeflate = false;
        internal static bool bUseXceedDecompressForGZIP = false;
        internal static bool bUsingPortOverride = false;
        public static bool bVersionCheck = true;
        internal static bool bVersionCheckBlocked = false;
        public static System.Drawing.Color colorDisabledEdit = System.Drawing.Color.AliceBlue;
        public static Version KProxyVersionInfo = Assembly.GetExecutingAssembly().GetName().Version;
        public static float flFontSize = 8.25f;
        internal const int I_MAX_CONNECTION_QUEUE = 50;
        public static int iHotkey = 70;
        public static int iHotkeyMod = 3;
        public static int iReporterUpdateInterval = 500;
        public static int iScriptReloadInterval = 0xbb8;
        internal static uint iStartupCount = 0;
        private static bool m_bAllowRemoteConnections;
        private static bool m_bCheckForISA = true;
        private static bool m_bForceExclusivePort;
        private static string m_CompareTool = "windiff.exe";
        private static string m_JSEditor;
        private static int m_ListenPort = 8777;
        private static string m_sHostsThatBypassKProxy;
        private static string m_TextEditor = "notepad.exe";
        public static SslProtocols oAcceptedClientHTTPSProtocols = (SslProtocols.Default | SslProtocols.Ssl2);
        public static SslProtocols oAcceptedServerHTTPSProtocols = SslProtocols.Default;
        public static Encoding oHeaderEncoding = Encoding.UTF8;
        internal static HostList oHLSkipDecryption = null;
        internal static string sKProxyListenHostPort = "127.0.0.1:8777";
        public static string sGatewayPassword;
        public static string sGatewayUsername;
        public static string sHookConnectionNamed = "DefaultLAN";
        internal static string sMachineDomain = string.Empty;
        internal static string sMachineNameLowerCase = string.Empty;
        internal static string sReverseProxyHostname = "localhost";
        internal static string sRootKey = @"SOFTWARE\Microsoft\KProxy\";
        private static string sScriptPath = (sUserPath + @"Scripts\CustomRules.js");
        private static string sUserPath = Application.StartupPath + @"\KProxy\";

        static KPCONFIG()
        {
            try
            {
                IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                sMachineDomain = iPGlobalProperties.DomainName;
                sMachineNameLowerCase = iPGlobalProperties.HostName.ToLower();
                iPGlobalProperties = null;
            }
            catch
            {
            }
            bQuietMode = true;
            bDebugSpew = false;
            m_ListenPort = 0x22a2;
            if ((Environment.OSVersion.Version.Major < 6) && (Environment.OSVersion.Version.Minor < 1))
            {
                bMapSocketToProcess = false;
            }
        }

        internal static void EnsureFoldersExist()
        {
            try
            {
                if (!Directory.Exists(GetPath("Captures")))
                {
                    Directory.CreateDirectory(GetPath("Captures"));
                }
                if (!Directory.Exists(GetPath("Requests")))
                {
                    Directory.CreateDirectory(GetPath("Requests"));
                }
                if (!Directory.Exists(GetPath("Responses")))
                {
                    Directory.CreateDirectory(GetPath("Responses"));
                }
                if (!Directory.Exists(GetPath("Scripts")))
                {
                    Directory.CreateDirectory(GetPath("Scripts"));
                }
            }
            catch (Exception exception)
            {
                KProxyApplication.DoNotifyUser(exception.ToString(), "Folder Creation Failed");
            }
            try
            {
                if ((!KProxyApplication.Prefs.GetBoolPref("KProxy.script.delaycreate", true) && !File.Exists(GetPath("CustomRules"))) && File.Exists(GetPath("SampleRules")))
                {
                    File.Copy(GetPath("SampleRules"), GetPath("CustomRules"));
                }
            }
            catch (Exception exception2)
            {
                KProxyApplication.DoNotifyUser(exception2.ToString(), "Initial file copies failed");
            }
        }

        [CodeDescription("Return a filesystem path. Contact EricLaw for constants.")]
        public static string GetPath(string sWhatPath)
        {
            string folderPath;
            switch (sWhatPath)
            {
                case "App":
                    return (Path.GetDirectoryName(Application.ExecutablePath) + @"\");

                case "AutoKProxys_Machine":
                    return (Path.GetDirectoryName(Application.ExecutablePath) + @"\Scripts\");

                case "AutoKProxys_User":
                    return (sUserPath + @"Scripts\");

                case "AutoResponderDefaultRules":
                    return (sUserPath + @"\AutoResponder.xml");

                case "Captures":
                    return KProxyApplication.Prefs.GetStringPref("KProxy.KPCONFIG.path.captures", sUserPath + @"Captures\");

                case "CustomRules":
                    return sScriptPath;

                case "DefaultClientCertificate":
                    return KProxyApplication.Prefs.GetStringPref("KProxy.KPCONFIG.path.defaultclientcert", sUserPath + "ClientCertificate.cer");

                case "KProxyRootCert":
                    return (sUserPath + "Root.cer");

                case "Filters":
                    return (sUserPath + @"Filters\");

                case "Inspectors":
                    return (Path.GetDirectoryName(Application.ExecutablePath) + @"\Inspectors\");

                case "PerUser-ISA-Config":
                    folderPath = "C:";
                    try
                    {
                        folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    }
                    catch
                    {
                    }
                    return (folderPath + @"\microsoft\firewall client 2004\management.ini");

                case "PerMachine-ISA-Config":
                    folderPath = "C:";
                    try
                    {
                        folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    }
                    catch
                    {
                    }
                    return (folderPath + @"\microsoft\firewall client 2004\management.ini");

                case "MakeCert":
                    folderPath = KProxyApplication.Prefs.GetStringPref("KProxy.KPCONFIG.path.makecert", Path.GetDirectoryName(Application.ExecutablePath) + @"\MakeCert.exe");
                    if (!File.Exists(folderPath))
                    {
                        folderPath = "MakeCert.exe";
                    }
                    return folderPath;

                case "MyDocs":
                    folderPath = "C:";
                    try
                    {
                        folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    }
                    catch (Exception exception)
                    {
                        KProxyApplication.DoNotifyUser("Initialization Error", "Failed to retrieve path to your My Documents folder.\nThis generally means you have a relative environment variable.\nDefaulting to C:\\\n\n" + exception.Message);
                    }
                    return folderPath;

                case "Pac":
                    return KProxyApplication.Prefs.GetStringPref("KProxy.KPCONFIG.path.pac", sUserPath + @"Scripts\BrowserPAC.js");

                case "Requests":
                    return KProxyApplication.Prefs.GetStringPref("KProxy.KPCONFIG.path.requests", sUserPath + @"Captures\Requests\");

                case "Responses":
                    return KProxyApplication.Prefs.GetStringPref("KProxy.KPCONFIG.path.responses", sUserPath + @"Captures\Responses\");

                case "Root":
                    return sUserPath;

                case "SafeTemp":
                    folderPath = "C:";
                    try
                    {
                        folderPath = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
                    }
                    catch (Exception exception2)
                    {
                        KProxyApplication.DoNotifyUser("Failed to retrieve path to your Internet Cache folder.\nThis generally means you have a relative environment variable.\nDefaulting to C:\\\n\n" + exception2.Message, "GetPath(SafeTemp) Failed");
                    }
                    return folderPath;

                case "SampleRules":
                    return (Path.GetDirectoryName(Application.ExecutablePath) + @"\Scripts\SampleRules.js");

                case "Scripts":
                    return (sUserPath + @"Scripts\");

                case "TemplateResponses":
                    return KProxyApplication.Prefs.GetStringPref("KProxy.KPCONFIG.path.templateresponses", Path.GetDirectoryName(Application.ExecutablePath) + @"\ResponseTemplates\");

                case "TextEditor":
                    return KProxyApplication.Prefs.GetStringPref("KProxy.KPCONFIG.path.texteditor", m_TextEditor);

                case "Transcoders_Machine":
                    return (Path.GetDirectoryName(Application.ExecutablePath) + @"\ImportExport\");

                case "Transcoders_User":
                    return (sUserPath + @"ImportExport\");

                case "WINDIFF":
                {
                    string stringPref = KProxyApplication.Prefs.GetStringPref("KProxy.KPCONFIG.path.comparetool", null);
                    if (string.IsNullOrEmpty(stringPref))
                    {
                        if (m_CompareTool == "windiff.exe")
                        {
                            if (File.Exists(GetPath("App") + "windiff.exe"))
                            {
                                return (GetPath("App") + "windiff.exe");
                            }
                            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinMerge.exe", false);
                            if (key != null)
                            {
                                string str3 = (string) key.GetValue(string.Empty, null);
                                key.Close();
                                if (str3 != null)
                                {
                                    return str3;
                                }
                            }
                        }
                        return m_CompareTool;
                    }
                    return stringPref;
                }
            }
            return "C:";
        }

        public static string GetRegPath(string sWhatPath)
        {
            switch (sWhatPath)
            {
                case "Root":
                    return sRootKey;

                case "LMIsBeta":
                    return sRootKey;

                case "MenuExt":
                    return (sRootKey + @"MenuExt\");

                case "UI":
                    return (sRootKey + @"UI\");

                case "Dynamic":
                    return (sRootKey + @"Dynamic\");

                case "Prefs":
                    return (sRootKey + @"Prefs\");
            }
            return sRootKey;
        }

        internal static void PerformISAFirewallCheck()
        {
            if (m_bCheckForISA && !bQuietMode)
            {
                try
                {
                    if (File.Exists(GetPath("PerUser-ISA-Config")))
                    {
                        StreamReader reader = File.OpenText(GetPath("PerUser-ISA-Config"));
                        string str = reader.ReadToEnd();
                        reader.Close();
                        if (str.Contains("EnableWebProxyAutoConfig=1"))
                        {
                            ShowISAFirewallWarning();
                            return;
                        }
                        if (str.Contains("EnableWebProxyAutoConfig=0"))
                        {
                            return;
                        }
                    }
                    if (File.Exists(GetPath("PerMachine-ISA-Config")))
                    {
                        StreamReader reader2 = File.OpenText(GetPath("PerMachine-ISA-Config"));
                        string str2 = reader2.ReadToEnd();
                        reader2.Close();
                        if (str2.Contains("EnableWebProxyAutoConfig=1"))
                        {
                            ShowISAFirewallWarning();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        internal static void PerformProxySettingsPerUserCheck()
        {
            try
            {
                RegistryKey oReg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings", false);
                if ((oReg != null) && (Utilities.GetRegistryInt(oReg, "ProxySettingsPerUser", 1) == 0))
                {
                    //KProxyApplication.Log.LogString("!WARNING: Kavprot Proxy has detected that system policy ProxySettingsPerUser = 0. Unless run as Administrator, Kavprot Proxy may not be able to capture traffic from Internet Explorer and other programs.");
                    oReg.Close();
                }
            }
            catch
            {
            }
        }

        internal static void SetNoDecryptList(string sNewList)
        {
            if (string.IsNullOrEmpty(sNewList))
            {
                oHLSkipDecryption = null;
            }
            else
            {
                oHLSkipDecryption = new HostList();
                oHLSkipDecryption.AssignFromString(sNewList);
            }
        }

        internal static void ShowISAFirewallWarning()
        {
            DialogResult no;
            if (bQuietMode)
            {
                no = DialogResult.No;
            }
            else
            {
                no = MessageBox.Show("Kavprot Proxy has detected that you may be running Microsoft Firewall client\nin Web Browser Automatic Configuration mode. This may cause\nKavprot Proxy to detach from Internet Explorer unexpectedly.\n\nTo disable this warning, click 'Cancel'.", "Possible Conflict Detected", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
            }
            switch (no)
            {
                case DialogResult.Yes:
                   
                    return;

                case DialogResult.Cancel:
                    m_bCheckForISA = false;
                    break;
            }
        }

        [CodeDescription("Returns true if Kavprot Proxy is configured to accept remote clients.")]
        public static bool bAllowRemoteConnections
        {
            get
            {
                return m_bAllowRemoteConnections;
            }
            internal set
            {
                m_bAllowRemoteConnections = value;
            }
        }

        internal static bool bFIP
        {
            get
            {
                bool flag = false;
                try
                {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Arsslensoft\KProxy", false);
                    if (key == null)
                    {
                        return flag;
                    }
                    string str = (string) key.GetValue("SmartAssemblyReportUsage", "False");
                    if (str == "True")
                    {
                        flag = true;
                    }
                    key.Close();
                }
                catch
                {
                }
                return flag;
            }
            set
            {
                try
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Arsslensoft\KProxy");
                    if (key != null)
                    {
                        key.SetValue("SmartAssemblyReportUsage", value.ToString());
                        key.Close();
                    }
                }
                catch
                {
                }
            }
        }
        static bool _bRevertToDefaultLayout;
        public static bool bRevertToDefaultLayout
        {
           
            get
            {
                return _bRevertToDefaultLayout;
            }
       
            private set
            {
                _bRevertToDefaultLayout = value;
            }
        }

        public static bool ForceExclusivePort
        {
            get
            {
                return m_bForceExclusivePort;
            }
            internal set
            {
                m_bForceExclusivePort = value;
            }
        }

        public static bool IgnoreServerCertErrors
        {
            get
            {
                return bIgnoreServerCertErrors;
            }
            set
            {
                bIgnoreServerCertErrors = value;
            }
        }

        public static int iReverseProxyForPort
        {
       
            get
            {
                return _iReverseProxyForPort;
            }
        
            set
            {
                _iReverseProxyForPort = value;
            }
        }

     
        public static string JSEditor
        {
            get
            {
                if ((m_JSEditor == null) || (m_JSEditor.Length < 1))
                {
                    m_JSEditor = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\Microsoft Office\office11\mse7.exe";
                    if (!File.Exists(m_JSEditor))
                    {
                        m_JSEditor = "notepad.exe";
                    }
                }
                return m_JSEditor;
            }
            set
            {
                m_JSEditor = value;
            }
        }

        public static int ListenPort
        {
            get
            {
                return m_ListenPort;
            }
            internal set
            {
                if ((value >= 0) && (value < 0x10000))
                {
                    m_ListenPort = value;
                    sKProxyListenHostPort = Utilities.TrimAfter(sKProxyListenHostPort, ':') + ":" + m_ListenPort.ToString();
                }
            }
        }

        public static bool QuietMode
        {
            get
            {
                return bQuietMode;
            }
            set
            {
                bQuietMode = value;
            }
        }

        public static string sHostsThatBypassKProxy
        {
            get
            {
                return m_sHostsThatBypassKProxy;
            }
            set
            {
                string str = value;
                if (str == null)
                {
                    str = string.Empty;
                }
                if ((str.IndexOf("<-loopback>", StringComparison.OrdinalIgnoreCase) < 0) && (str.IndexOf("<loopback>", StringComparison.OrdinalIgnoreCase) < 0))
                {
                    str = "<-loopback>;" + str;
                }
                m_sHostsThatBypassKProxy = str;
            }
        }
    }
}

