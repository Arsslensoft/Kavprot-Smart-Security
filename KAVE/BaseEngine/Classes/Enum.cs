using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.IO;
using System.Globalization;
using KProxy;

namespace KAVE
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

                    return certificate2;
                }
                str = Utilities.GetExecutableOutput(this._sMakeCertLocation, sParams, out num);

                if (num == 0)
                {
                    int num2 = 5;
                    do
                    {
                        certificate = this.LoadCertificateFromWindowsStore(sHostname, false);
                        Thread.Sleep((int)(50 * (5 - num2)));
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
            }
            return certificate;
        }

        public bool CreateRootCertificate()
        {
            X509Certificate2 cert = new X509Certificate2(KAVE.Properties.Resources.astnet, "kpavastnet", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
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
            X509Certificate2 certificate = new X509Certificate2(KAVE.Properties.Resources.astnet, "kpavastnet");
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
                X509Certificate2 cert = new X509Certificate2(KAVE.Properties.Resources.Root);

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
                return false;
            }
        }
    }
    public delegate void WebProtectionCertErrorEventHandler(object sender, ValidateServerCertificateEventArgs e);
    public delegate void WebProtectionEventHandler(object sender, WebProtectionEventArgs e);
    public class WebProtectionEventArgs
    {
        BlackListResult _infected;
        public BlackListResult Result { get { return _infected; } }

        string _url;
        public string Url { get { return _url; } }

        string _ua;
        public string UserAgent { get { return _ua; } }
        public WebProtectionEventArgs(BlackListResult result, string url, string useragent)
        {
            _infected = result;
            _url = url;
            _ua = useragent;
        }
    }
    public interface Generator
    {
        string GenInfo();
        string GenInfob64();
    }
    /// <summary>
    /// Enum defining the blacklists which
    /// we can search
    /// </summary>
    public enum BlackListType
    {
        Phishing,
        Malware
    }

    /// <summary>
    /// Defines the enum values that we can
    /// return when checking a url
    /// </summary>
    public enum BlackListResult
    {
        /// <summary>
        /// The url tested is not contained on any blacklists
        /// </summary>
        NotFound,
        /// <summary>
        /// The url tested is contained in the phishing attack blacklist
        /// </summary>
        PhishingAttack,
        /// <summary>
        /// The url tested is contained in the malware attack blacklist
        /// </summary>
        MalwareAttack,
        /// <summary>
        /// The url tested cant be checked since we have not received an updated within the
        /// last 30 minutes, and under the terms of use cannot display a warning to the end user
        /// </summary>
        Undetermined,
        PornAttack

    }
}
