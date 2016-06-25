namespace KProxy
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Security;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Windows.Forms;

    public class KProxyApplication
    {
        internal static PreferenceBag _Prefs = new PreferenceBag(null);
        public static bool isClosing;
        internal static readonly PeriodicWorker Janitor = new PeriodicWorker();
        internal static X509Certificate oDefaultClientCertificate;
        [CodeDescription("Kavprot Proxy's core proxy engine.")]
        public static Proxy oProxy;
        internal static KProxyTranscoders oTranscoders = new KProxyTranscoders();

        public static event SessionStateHandler AfterSessionComplete;

        public static event SessionStateHandler BeforeRequest;
        public static event SessionStateHandler LocalHost;

        public static event SessionStateHandler BeforeResponse;

        internal static event SessionStateHandler BeforeReturningError;

        [CodeDescription("Sync this event to be notified when Kavprot Proxy has attached as the system proxy.")]
        internal static event SimpleEventHandler KProxyAttach;

        [CodeDescription("Sync this event to be notified when Kavprot Proxy has detached as the system proxy.")]
        internal static event SimpleEventHandler KProxyDetach;

        internal static event EventHandler<NotificationEventArgs> OnNotification;

        internal static event EventHandler<RawReadEventArgs> OnReadResponseBuffer;

        internal static event EventHandler<ValidateServerCertificateEventArgs> OnValidateServerCertificate;

        internal static event SessionStateHandler RequestHeadersAvailable;

        internal static event SessionStateHandler ResponseHeadersAvailable;

        internal static void CallLocalHost(Session session)
        {
            if (LocalHost != null)
            {
                LocalHost(session);
            }
        
                
        }
        static KProxyApplication()
        {
            
        }

        private KProxyApplication()
        {
        }

  
        internal static void CheckOverrideCertificatePolicy(Session oS, string sExpectedCN, X509Certificate ServerCertificate, X509Chain ServerCertificateChain, SslPolicyErrors sslPolicyErrors, ref CertificateValidity oValidity)
        {
            if (OnValidateServerCertificate != null)
            {
                ValidateServerCertificateEventArgs e = new ValidateServerCertificateEventArgs(oS, sExpectedCN, ServerCertificate, ServerCertificateChain, sslPolicyErrors);
                OnValidateServerCertificate(oS, e);
                oValidity = e.ValidityState;
            }
        }

        public static Proxy CreateProxyEndpoint(int iPort, bool bAllowRemote, X509Certificate2 certHTTPS)
        {
            Proxy proxy = new Proxy(false);
            if (certHTTPS != null)
            {
                proxy.AssignEndpointCertificate(certHTTPS);
            }
            if (proxy.Start(iPort, bAllowRemote))
            {
                return proxy;
            }
            proxy.Dispose();
            return null;
        }

        public static Proxy CreateProxyEndpoint(int iPort, bool bAllowRemote, string sHTTPSHostname)
        {
            Proxy proxy = new Proxy(false);
            if (!string.IsNullOrEmpty(sHTTPSHostname))
            {
                proxy.ActAsHTTPSEndpointForHostname(sHTTPSHostname);
            }
            if (proxy.Start(iPort, bAllowRemote))
            {
                return proxy;
            }
            proxy.Dispose();
            return null;
        }

        internal static void DebugSpew(string sMessage)
        {
            if (KPCONFIG.bDebugSpew)
            {
                Trace.WriteLine(sMessage);
            }
        }

        internal static void DoAfterSessionComplete(Session oSession)
        {
            if (AfterSessionComplete != null)
            {
                AfterSessionComplete(oSession);
            }
        }

        internal static void DoBeforeRequest(Session oSession)
        {
            if (BeforeRequest != null)
            {
                BeforeRequest(oSession);
            }
        }

        internal static void DoBeforeResponse(Session oSession)
        {
            if (BeforeResponse != null)
            {
                BeforeResponse(oSession);
            }
        }

        internal static void DoBeforeReturningError(Session oSession)
        {
            if (BeforeReturningError != null)
            {
                BeforeReturningError(oSession);
            }
        }

        public static bool DoExport(string sExportFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> ehPCEA)
        {
            if (string.IsNullOrEmpty(sExportFormat))
            {
                return false;
            }
            TranscoderTuple tuple = oTranscoders.GetExporter(sExportFormat);
            if (tuple == null)
            {
                return false;
            }
            bool flag = false;
            try
            {
                ISessionExporter exporter = (ISessionExporter) Activator.CreateInstance(tuple.typeFormatter);
                if (ehPCEA == null)
                {
                    ehPCEA = delegate (object sender, ProgressCallbackEventArgs oPCE) {
                        string str = (oPCE.PercentComplete > 0) ? ("Export is " + oPCE.PercentComplete + "% complete; ") : string.Empty;
                        
                        Application.DoEvents();
                    };
                }
                flag = exporter.ExportSessions(sExportFormat, oSessions, dictOptions, ehPCEA);
                exporter.Dispose();
            }
            catch (Exception exception)
            {
                LogAddonException(exception, "Exporter for " + sExportFormat + " failed.");
                flag = false;
            }
            return flag;
        }

        public static Session[] DoImport(string sImportFormat, bool bAddToSessionList, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> ehPCEA)
        {
            Session[] sessionArray;
            if (string.IsNullOrEmpty(sImportFormat))
            {
                return null;
            }
            TranscoderTuple tuple = oTranscoders.GetImporter(sImportFormat);
            if (tuple == null)
            {
                return null;
            }
            try
            {
                ISessionImporter importer = (ISessionImporter) Activator.CreateInstance(tuple.typeFormatter);
                if (ehPCEA == null)
                {
                    ehPCEA = delegate (object sender, ProgressCallbackEventArgs oPCE) {
                        string str = (oPCE.PercentComplete > 0) ? ("Import is " + oPCE.PercentComplete + "% complete; ") : string.Empty;
                 
                        Application.DoEvents();
                    };
                }
                sessionArray = importer.ImportSessions(sImportFormat, dictOptions, ehPCEA);
                importer.Dispose();
                if (sessionArray == null)
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                LogAddonException(exception, "Importer for " + sImportFormat + " failed.");
                sessionArray = null;
            }
            return sessionArray;
        }

        internal static void DoNotifyUser(string sMessage, string sTitle)
        {
            DoNotifyUser(sMessage, sTitle, MessageBoxIcon.None);
        }

        internal static void DoNotifyUser(string sMessage, string sTitle, MessageBoxIcon oIcon)
        {
            if (OnNotification != null)
            {
                NotificationEventArgs e = new NotificationEventArgs(string.Format("{0} - {1}", sTitle, sMessage));
                OnNotification(null, e);
            }
            if (!KPCONFIG.QuietMode)
            {
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, oIcon);
            }
        }

        internal static bool DoReadResponseBuffer(Session oS, byte[] arrBytes, int cBytes)
        {
            if (OnReadResponseBuffer != null)
            {
                RawReadEventArgs e = new RawReadEventArgs(oS, arrBytes, cBytes);
                OnReadResponseBuffer(oS, e);
                if (e.AbortReading)
                {
                    return false;
                }
            }
            return true;
        }

        internal static void DoRequestHeadersAvailable(Session oSession)
        {
            if (RequestHeadersAvailable != null)
            {
                RequestHeadersAvailable(oSession);
            }
        }

        internal static void DoResponseHeadersAvailable(Session oSession)
        {
            if (ResponseHeadersAvailable != null)
            {
                ResponseHeadersAvailable(oSession);
            }
        }

        public static string GetDetailedInfo()
        {
            string str3 = string.Empty;
            string str = str3 + "\nRunning on: " + KPCONFIG.sMachineNameLowerCase + ":" + oProxy.ListenPort.ToString() + "\n";
            if (KPCONFIG.bHookAllConnections)
            {
                str = str + "Listening to: All Adapters\n";
            }
            else
            {
                str = str + "Listening to: " + (KPCONFIG.sHookConnectionNamed ?? "Default LAN") + "\n";
            }
            if (KPCONFIG.iReverseProxyForPort > 0)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "Acting as reverse proxy for port #", KPCONFIG.iReverseProxyForPort, "\n" });
            }
            if (oProxy.oAutoProxy != null)
            {
                str = str + "Gateway: Using Script\n" + oProxy.oAutoProxy.ToString();
            }
            else
            {
                IPEndPoint point = oProxy.FindGatewayForOrigin("http", "www.arsslensoft.tk");
                if (point != null)
                {
                    string str4 = str;
                    str = str4 + "Gateway: " + point.Address.ToString() + ":" + point.Port.ToString();
                }
                else
                {
                    str = str + "Gateway: No Gateway";
                }
            }
            string str2 = string.Empty;
            return string.Format("Kavprot Proxy ({0})\n{9}\n{1}-bit {2}, VM: {3:N2}mb, WS: {4:N2}mb\n{5}\n{6}\n\nYou've run KProxy: {7:N0} times.\n{8}\n", new object[] { KPCONFIG.bIsBeta ? string.Format("v{0} beta", Application.ProductVersion) : string.Format("v{0}", Application.ProductVersion), (8 == IntPtr.Size) ? "64" : "32", Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"), Process.GetCurrentProcess().PagedMemorySize64 / 0x100000L, Process.GetCurrentProcess().WorkingSet64 / 0x100000L, ".NET " + Environment.Version, Environment.OSVersion.VersionString, KPCONFIG.iStartupCount, str, str2 });
        }

        public static string GetVersionString()
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            string str = string.Empty;
            string str2 = "Kavprot Proxy";
            return string.Format("{0}/{1}.{2}.{3}.{4}{5}", new object[] { str2, versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart, str });
        }

        internal static void HandleHTTPError(Session oSession, SessionFlags flagViolation, bool bPoisonClientConnection, bool bPoisonServerConnection, string sMessage)
        {
            if (bPoisonClientConnection)
            {
                oSession.PoisonClientPipe();
            }
            if (bPoisonServerConnection)
            {
                oSession.PoisonServerPipe();
            }
            oSession.SetBitFlag(flagViolation, true);
            oSession["ui-backcolor"] = "LightYellow";
            sMessage = "[ProtocolViolation] " + sMessage;
            if ((oSession["x-HTTPProtocol-Violation"] == null) || !oSession["x-HTTPProtocol-Violation"].Contains(sMessage))
            {
                Session session;
                (session = oSession)["x-HTTPProtocol-Violation"] = session["x-HTTPProtocol-Violation"] + sMessage;
            }
        }

        public static bool IsStarted()
        {
            return (null != oProxy);
        }

        public static bool IsSystemProxy()
        {
            return ((oProxy != null) && oProxy.IsAttached);
        }

        internal static void LogAddonException(Exception eX, string sTitle)
        {
            if (Prefs.GetBoolPref("KProxy.debug.extensions.showerrors", false) || Prefs.GetBoolPref("KProxy.debug.extensions.verbose", false))
            {
                ReportException(eX, sTitle);
            }
        }

        internal static void OnKProxyAttach()
        {
            if (KProxyAttach != null)
            {
                KProxyAttach();
            }
        }

        internal static void OnKProxyDetach()
        {
            if (KProxyDetach != null)
            {
                KProxyDetach();
            }
        }

        internal static void ReportException(Exception eX)
        {
            ReportException(eX, "Sorry, you may have found a bug...");
        }

        public static void ReportException(Exception eX, string sTitle)
        {
            if (!(eX is ThreadAbortException) || !isClosing)
            {
                                 string str;
                    if (eX is OutOfMemoryException)
                    {
                        sTitle = "Out of Memory Error";
                        str = "An out-of-memory exception was encountered. To help avoid out-of-memory conditions. ";
                    }
                    else
                    {
                        str = "Kavprot Proxy has encountered an unexpected problem. If you believe this is a bug in KProxy, please copy this message by hitting CTRL+C, and submit a bug report using the Help | Send Feedback menu.\n\n";
                    }
                    DoNotifyUser(string.Concat(new object[] { 
                        str, eX.Message, "\n\nType: ", eX.GetType().ToString(), "\nSource: ", eX.Source, "\n", eX.StackTrace, "\n\n", eX.InnerException, "\nKProxy v", Application.ProductVersion, (8 == IntPtr.Size) ? " (x64 " : " (x86 ", Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"), ") [.NET ", Environment.Version, 
                        " on ", Environment.OSVersion.VersionString, "] "
                     }), sTitle, MessageBoxIcon.Hand);
                
                Trace.Write(string.Concat(new object[] { eX.Message, "\n", eX.StackTrace, "\n", eX.InnerException }));
            }
        }

        [CodeDescription("Reset the SessionID counter to 0. This method can lead to confusing UI, so call sparingly.")]
        public static void ResetSessionCounter()
        {
            Session.ResetSessionCounter();
        }

        public static void Shutdown()
        {
            if (oProxy != null)
            {
                oProxy.Detach();
                oProxy.Dispose();
                oProxy = null;
            }
        }

        public static void Startup(int iListenPort, KProxyStartupFlags oFlags)
        {
            if (oProxy != null)
            {
                throw new InvalidOperationException("Calling startup twice without calling shutdown is not permitted.");
            }
            if ((iListenPort < 0) || (iListenPort > 0xffff))
            {
                throw new ArgumentOutOfRangeException("bListenPort", "Port must be between 0 and 65535.");
            }
            KPCONFIG.ListenPort = iListenPort;
            KPCONFIG.bAllowRemoteConnections = KProxyStartupFlags.None < (oFlags & KProxyStartupFlags.AllowRemoteClients);
            KPCONFIG.bMITM_HTTPS = KProxyStartupFlags.None < (oFlags & KProxyStartupFlags.DecryptSSL);
            KPCONFIG.bCaptureCONNECT = true;
            KPCONFIG.bForwardToGateway = KProxyStartupFlags.None < (oFlags & KProxyStartupFlags.ChainToUpstreamGateway);
            KPCONFIG.bHookAllConnections = KProxyStartupFlags.None < (oFlags & KProxyStartupFlags.MonitorAllConnections);
            if (KProxyStartupFlags.None < (oFlags & KProxyStartupFlags.CaptureLocalhostTraffic))
            {
                KPCONFIG.sHostsThatBypassKProxy = KPCONFIG.sHostsThatBypassKProxy;
            }
            oProxy = new Proxy(true);
            if (oProxy.Start(KPCONFIG.ListenPort, KPCONFIG.bAllowRemoteConnections))
            {
                if (iListenPort == 0)
                {
                    KPCONFIG.ListenPort = oProxy.ListenPort;
                }
                if (KProxyStartupFlags.None < (oFlags & KProxyStartupFlags.RegisterAsSystemProxy))
                {
                    oProxy.Attach(true);
                }
                else if (KProxyStartupFlags.None < (oFlags & KProxyStartupFlags.ChainToUpstreamGateway))
                {
                    oProxy.CollectConnectoidAndGatewayInfo();
                }
            }
        }

        public static void Startup(int iListenPort, bool bRegisterAsSystemProxy, bool bDecryptSSL)
        {
            KProxyStartupFlags oFlags = KProxyStartupFlags.Default;
            if (bRegisterAsSystemProxy)
            {
                oFlags |= KProxyStartupFlags.RegisterAsSystemProxy;
            }
            else
            {
                oFlags &= ~KProxyStartupFlags.RegisterAsSystemProxy;
            }
            if (bDecryptSSL)
            {
                oFlags |= KProxyStartupFlags.DecryptSSL;
            }
            else
            {
                oFlags &= ~KProxyStartupFlags.DecryptSSL;
            }
            Startup(iListenPort, oFlags);
        }

        public static void Startup(int iListenPort, bool bRegisterAsSystemProxy, bool bDecryptSSL, bool bAllowRemote)
        {
            KProxyStartupFlags oFlags = KProxyStartupFlags.Default;
            if (bRegisterAsSystemProxy)
            {
                oFlags |= KProxyStartupFlags.RegisterAsSystemProxy;
            }
            else
            {
                oFlags &= ~KProxyStartupFlags.RegisterAsSystemProxy;
            }
            if (bDecryptSSL)
            {
                oFlags |= KProxyStartupFlags.DecryptSSL;
            }
            else
            {
                oFlags &= ~KProxyStartupFlags.DecryptSSL;
            }
            if (bAllowRemote)
            {
                oFlags |= KProxyStartupFlags.AllowRemoteClients;
            }
            else
            {
                oFlags &= ~KProxyStartupFlags.AllowRemoteClients;
            }
            Startup(iListenPort, oFlags);
        }

        [CodeDescription("Kavprot Proxy's Preferences collection.")]
        internal static IKProxyPreferences Prefs
        {
            get
            {
                return _Prefs;
            }
        }
    }
}

