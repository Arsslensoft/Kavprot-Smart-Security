using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;

namespace KProxy
{
    internal class DefaultCertificateProvider : ICertificateProvider2, ICertificateProvider
    {
        private string _sMakeCertLocation = KPCONFIG.GetPath("MakeCert");
        private X509Certificate2 certRoot;
        private Dictionary<string, X509Certificate2> certServerCache = new Dictionary<string, X509Certificate2>();
        private ReaderWriterLock oRWLock = new ReaderWriterLock();
        internal DefaultCertificateProvider()
        {

        }

        public bool ClearCertificateCache()
        {
            return this.ClearCertificateCache(true);
        }

        public bool ClearCertificateCache(bool bRemoveRoot)
        {
            bool flag = true;
            try
            {
                X509Certificate2Collection certificates;
                this.oRWLock.AcquireWriterLock(-1);
                this.certServerCache.Clear();
                this.certRoot = null;
                string sFullSubject = "CN = Arsslensoft Root CA, O = Arsslensoft Foundation, L = La Marsa, S = Tunis, C = TN";
                if (bRemoveRoot)
                {
                    certificates = FindCertsBySubject(StoreName.Root, StoreLocation.CurrentUser, sFullSubject);
                    if (certificates.Count > 0)
                    {
                        X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                        store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);
                        try
                        {
                            store.RemoveRange(certificates);
                        }
                        catch
                        {
                            flag = false;
                        }
                        store.Close();
                    }
                }
                certificates = FindCertsByIssuer(StoreName.My, sFullSubject);
                if (certificates.Count <= 0)
                {
                    return flag;
                }
                if (!bRemoveRoot)
                {
                    X509Certificate2 rootCertificate = this.GetRootCertificate();
                    if (rootCertificate != null)
                    {
                        certificates.Remove(rootCertificate);
                        if (certificates.Count < 1)
                        {
                            return true;
                        }
                    }
                }
                X509Store store2 = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store2.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);
                try
                {
                    store2.RemoveRange(certificates);
                }
                catch
                {
                    flag = false;
                }
                store2.Close();
            }
            finally
            {
                this.oRWLock.ReleaseWriterLock();
            }
            return flag;
        }

        private X509Certificate2 CreateCert(string sHostname, bool isRoot)
        {
            int num;
            string str;

            if (sHostname.IndexOfAny(new char[] { '"', '\r', '\n' }) != -1)
            {
                return null;
            }
            if (!File.Exists(this._sMakeCertLocation))
            {
                throw new FileNotFoundException("Cannot locate: " + this._sMakeCertLocation + ". Please move makecert.exe to the KPAVWebProxy installation directory.");
            }
            X509Certificate2 certificate = null;
            string sParams = string.Format(sMakeCertParamsEE, new object[] { sHostname, sMakeCertSubjectO, sMakeCertRootCN, DateTime.Now.AddDays(-7.0).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) });
            try
            {
                this.oRWLock.AcquireWriterLock(-1);
                X509Certificate2 certificate2 = this.LoadCertificateFromWindowsStore(sHostname, false);
                if (certificate2 != null)
                {
                    if (KPCONFIG.bDebugCertificateGeneration)
                    {
                        //KProxyProxyApplication.Log.LogFormat("/KPAVWebProxy.CertMaker>{1} A racing thread already successfully CreatedCert({0})", new object[] { sHostname, Thread.CurrentThread.ManagedThreadId });
                    }
                    return certificate2;
                }
                str = Utilities.GetExecutableOutput(this._sMakeCertLocation, sParams, out num);
                if (KPCONFIG.bDebugCertificateGeneration)
                {
                    //KProxyProxyApplication.Log.LogFormat("/KPAVWebProxy.CertMaker>{3}-CreateCert({0}) => ({1}){2}", new object[] { sHostname, num, (num == 0) ? "." : ("\r\n" + str), Thread.CurrentThread.ManagedThreadId });
                }
                if (num == 0)
                {
                    int num2 = 5;
                    do
                    {
                        certificate = this.LoadCertificateFromWindowsStore(sHostname, false);
                        Thread.Sleep((int)(50 * (5 - num2)));
                        if (KPCONFIG.bDebugCertificateGeneration && (certificate == null))
                        {
                            //KProxyProxyApplication.Log.LogFormat("!WARNING: Couldn't find certificate for {0} on try #{1}", new object[] { sHostname, 5 - num2 });
                        }
                        num2--;
                    }
                    while ((certificate == null) && (num2 >= 0));
                }
                if (certificate != null)
                {
                    if (isRoot)
                    {
                        this.certRoot = certificate;
                    }
                    else
                    {
                        this.certServerCache.Add(sHostname, certificate);
                    }
                }
            }
            finally
            {
                this.oRWLock.ReleaseWriterLock();
            }
            if (certificate == null)
            {
                string sMessage = string.Format("Creation of the interception certificate failed.\n\nmakecert.exe returned {0}.\n\n{1}", num, str);
                //KProxyProxyApplication.Log.LogFormat("KPAVWebProxy.CertMaker> [{0}{1}] Returned Error: {2} ", new object[] { this._sMakeCertLocation, sParams, sMessage });

            }
            return certificate;
        }

        public bool CreateRootCertificate()
        {
            X509Certificate2 cert = new X509Certificate2(KProxy.Properties.Resources.astnet, "kpavastnet", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            X509Store st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            st.Open(OpenFlags.ReadWrite);
            st.Add(cert);
            return true;
        }

        static string sMakeCertRootCN = "\"Arsslensoft Trust Network\"";
        static string sMakeCertSubjectO = ", OU=Origisign, O=Arsslensoft, L=La Marsa, S=Tunis, C=TN";

        static string sMakeCertParamsEE = "-pe -ss my -n \"CN={0}{1}\" -sky exchange -in {2} -is my -eku 1.3.6.1.5.5.7.3.1 -cy end -a sha1 -m 24 -b {3}";

        private static X509Certificate2Collection FindCertsByIssuer(StoreName storeName, string sFullIssuerSubject)
        {
            X509Store store = new X509Store(storeName, StoreLocation.CurrentUser);
            store.Open(OpenFlags.OpenExistingOnly);
            X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindByIssuerDistinguishedName, sFullIssuerSubject, false);
            store.Close();
            return certificates;
        }

        private static X509Certificate2Collection FindCertsBySubject(StoreName storeName, StoreLocation storeLocation, string sFullSubject)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.OpenExistingOnly);
            X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, sFullSubject, false);
            store.Close();
            return certificates;
        }

        public X509Certificate2 GetCertificateForHost(string sHostname)
        {
            try
            {
                this.oRWLock.AcquireReaderLock(-1);
                if (this.certServerCache.ContainsKey(sHostname))
                {
                    return this.certServerCache[sHostname];
                }
            }
            finally
            {
                this.oRWLock.ReleaseReaderLock();
            }
            X509Certificate2 certificate = this.LoadCertificateFromWindowsStore(sHostname, true);
            if (certificate != null)
            {
                try
                {
                    this.oRWLock.AcquireWriterLock(-1);
                    this.certServerCache[sHostname] = certificate;
                }
                finally
                {
                    this.oRWLock.ReleaseWriterLock();
                }
            }
            return certificate;
        }

        public X509Certificate2 GetRootCertificate()
        {
            X509Certificate2 certificate = new X509Certificate2(KProxy.Properties.Resources.astnet, "kpavastnet");
            return certificate;
        }

        internal X509Certificate2 LoadCertificateFromWindowsStore(string sHostname, bool allowCreate)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            string b = string.Format("CN={0}{1}", sHostname, sMakeCertSubjectO);
            X509Certificate2Enumerator enumerator = store.Certificates.GetEnumerator();
            while (enumerator.MoveNext())
            {
                X509Certificate2 current = enumerator.Current;
                if (string.Equals(current.Subject, b, StringComparison.OrdinalIgnoreCase))
                {
                    store.Close();
                    return current;
                }
            }
            store.Close();
            if (!allowCreate)
            {
                return null;
            }
            X509Certificate2 certificate2 = this.CreateCert(sHostname, false);
            if (certificate2 == null)
            {
                //KProxyProxyApplication.Log.LogFormat("!KPAVWebProxy.CertMaker> Tried to create cert for {0}, but can't find it from thread {1}!", new object[] { sHostname, Thread.CurrentThread.ManagedThreadId });
            }
            return certificate2;
        }

        public bool rootCertIsTrusted(out bool bUserTrusted, out bool bMachineTrusted)
        {
            bUserTrusted = 0 < FindCertsBySubject(StoreName.Root, StoreLocation.CurrentUser, "CN=Arsslensoft Root CA, O=Arsslensoft Foundation, L=La Marsa, S=Tunis, C=TN").Count;
            bMachineTrusted = 0 < FindCertsBySubject(StoreName.Root, StoreLocation.LocalMachine, "CN=Arsslensoft Root CA, O=Arsslensoft Foundation, L=La Marsa, S=Tunis, C=TN").Count;
            if (!bUserTrusted)
            {
                return bMachineTrusted;
            }
            return true;
        }

        public bool TrustRootCertificate()
        {
            if (this.GetRootCertificate() == null)
            {
                return false;
            }
            try
            {
                X509Certificate2 cert = new X509Certificate2(KProxy.Properties.Resources.Root);

                X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                try
                {
                    store.Add(cert);
                }
                finally
                {
                    store.Close();
                }
                return true;
            }
            catch
            {
                //KProxyProxyApplication.Log.LogFormat("!KPAVWebProxy.CertMaker> Unable to auto-trust root: {0}", new object[] { exception });
                return false;
            }
        }
    }
   public class CertMaker
    {
        internal static ICertificateProvider oCertProvider;

        static CertMaker()
        {
            if (oCertProvider == null)
            {
                DefaultCertificateProvider certp = new DefaultCertificateProvider();
                oCertProvider = certp;
                oCertProvider.CreateRootCertificate();
                if (!CertMaker.rootCertIsTrusted())
                {
                    CertMaker.trustRootCert();
                }
                else
                {

                }
            }
        }

        public static bool createRootCert()
        {
            return oCertProvider.CreateRootCertificate();
        }

        internal static bool exportRootToDesktop()
        {
            try
            {
                byte[] bytes = getRootCertBytes();
                if (bytes != null)
                {
                    File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\ASRoot.cer", bytes);
                    return true;
                }
            }
            catch (Exception exception)
            {
                KProxyApplication.ReportException(exception);
                return false;
            }
            return false;
        }

        internal static X509Certificate2 FindCert(string sHostname, bool allowCreate)
        {
            return oCertProvider.GetCertificateForHost(sHostname);
        }

        internal static byte[] getRootCertBytes()
        {
            X509Certificate2 rootCertificate = GetRootCertificate();
            if (rootCertificate == null)
            {
                return null;
            }
            return rootCertificate.Export(X509ContentType.Cert);
        }

        public static X509Certificate2 GetRootCertificate()
        {
            return oCertProvider.GetRootCertificate();
        }

        private static ICertificateProvider LoadOverrideCertProvider()
        {
            string stringPref = KProxyApplication.Prefs.GetStringPref("KProxy.certmaker.assembly", KPCONFIG.GetPath("App") + "CertMaker.dll");
            if (File.Exists(stringPref))
            {
                Assembly assembly;
                try
                {
                    assembly = Assembly.LoadFrom(stringPref);
                    if (!Utilities.KProxyMeetsVersionRequirement(assembly, "Certificate Maker"))
                    {
                        return null;
                    }
                }
                catch (Exception exception)
                {
                    KProxyApplication.LogAddonException(exception, "Failed to load CertMaker" + stringPref);
                    return null;
                }
                foreach (Type type in assembly.GetExportedTypes())
                {
                    if ((!type.IsAbstract && type.IsPublic) && (type.IsClass && typeof(ICertificateProvider).IsAssignableFrom(type)))
                    {
                        try
                        {
                            return (ICertificateProvider) Activator.CreateInstance(type);
                        }
                        catch (Exception exception2)
                        {
                            KProxyApplication.DoNotifyUser(string.Format("[Kavprot Proxy] Failure loading {0} CertMaker from {1}: {2}\n\n{3}\n\n{4}", new object[] { type.Name, assembly.CodeBase, exception2.Message, exception2.StackTrace, exception2.InnerException }), "Load Error");
                        }
                    }
                }
            }
            return null;
        }

        public static void removeKProxyGeneratedCerts()
        {
            removeKProxyGeneratedCerts(true);
        }

        public static void removeKProxyGeneratedCerts(bool bRemoveRoot)
        {
            if (oCertProvider is ICertificateProvider2)
            {
                (oCertProvider as ICertificateProvider2).ClearCertificateCache(bRemoveRoot);
            }
            else
            {
                oCertProvider.ClearCertificateCache();
            }
        }

        public static bool rootCertExists()
        {
            try
            {
                X509Certificate2 rootCertificate = GetRootCertificate();
                return (null != rootCertificate);
            }
            catch
            {
                return false;
            }
        }

        public static bool rootCertIsMachineTrusted()
        {
            bool flag;
            bool flag2;
            oCertProvider.rootCertIsTrusted(out flag, out flag2);
            return flag2;
        }

        public static bool rootCertIsTrusted()
        {
            bool flag;
            bool flag2;
            return oCertProvider.rootCertIsTrusted(out flag, out flag2);
        }

        public static bool trustRootCert()
        {
            return oCertProvider.TrustRootCertificate();
        }
    }
}

