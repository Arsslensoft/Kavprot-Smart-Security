namespace KProxy
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Net.Sockets;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    public class ServerPipe : BasePipe
    {
        protected bool _bIsConnectedToGateway;
        private int _iMarriedToPID;
        private bool _isAuthenticated;
        private PipeReusePolicy _reusePolicy;
        private string _ServerCertChain;
        protected string _sPoolKey;
        internal DateTime dtConnected;
        internal int iLastPooled;
        private static StringCollection slAcceptableBadCertificates = null;

        internal ServerPipe(string sName, bool WillConnectToGateway) : base(null, sName)
        {
            this._bIsConnectedToGateway = WillConnectToGateway;
        }

        private X509Certificate _GetDefaultCertificate()
        {
            if (KProxyApplication.oDefaultClientCertificate != null)
            {
                return KProxyApplication.oDefaultClientCertificate;
            }
            X509Certificate certificate = null;
            if (System.IO.File.Exists(KPCONFIG.GetPath("DefaultClientCertificate")))
            {
                certificate = X509Certificate.CreateFromCertFile(KPCONFIG.GetPath("DefaultClientCertificate"));
                if ((certificate != null) && KProxyApplication.Prefs.GetBoolPref("KProxy.network.https.cacheclientcert", true))
                {
                    KProxyApplication.oDefaultClientCertificate = certificate;
                }
            }
            return certificate;
        }

        //internal object _GetTransportContext()
        //{
        //    if (base._httpsStream != null)
        //    {
        //        return base._httpsStream.get_TransportContext();
        //    }
        //    return null;
        //}

        private X509Certificate AttachClientCertificate(Session oS, object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            if (localCertificates.Count > 0)
            {
                this.MarkAsAuthenticated(oS.LocalProcessID);
                oS.oFlags["x-client-cert"] = localCertificates[0].Subject + " Serial#" + localCertificates[0].GetSerialNumberString();
                return localCertificates[0];
            }
            if ((remoteCertificate != null) || (acceptableIssuers.Length >= 1))
            {
                X509Certificate certificate = this._GetDefaultCertificate();
                if (certificate != null)
                {
                    this.MarkAsAuthenticated(oS.LocalProcessID);
                    oS.oFlags["x-client-cert"] = certificate.Subject + " Serial#" + certificate.GetSerialNumberString();
                    return certificate;
                }
                if (KPCONFIG.bShowDefaultClientCertificateNeededPrompt && KProxyApplication.Prefs.GetBoolPref("KProxy.network.https.clientcertificate.ephemeral.prompt-for-missing", true))
                {
                    KProxyApplication.DoNotifyUser("The server [" + targetHost + "] requests a client certificate.\nPlease save a client certificate using the filename:\n\n" + KPCONFIG.GetPath("DefaultClientCertificate"), "Client Certificate Requested");
                    KProxyApplication.Prefs.SetBoolPref("KProxy.network.https.clientcertificate.ephemeral.prompt-for-missing", false);
                }
            }
            return null;
        }

        private static bool ConfirmServerCertificate(Session oS, string sExpectedCN, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            CertificateValidity oValidity = CertificateValidity.Default;
            KProxyApplication.CheckOverrideCertificatePolicy(oS, sExpectedCN, certificate, chain, sslPolicyErrors, ref oValidity);
            switch (oValidity)
            {
                case CertificateValidity.ForceInvalid:
                    return false;

                case CertificateValidity.ForceValid:
                    return true;
            }
            if ((oValidity == CertificateValidity.ConfirmWithUser) || ((sslPolicyErrors != SslPolicyErrors.None) && !KPCONFIG.IgnoreServerCertErrors))
            {
                return false;
            }
            return true;
        }

        public string DescribeConnectionSecurity()
        {
            if (base._httpsStream == null)
            {
                return "No connection security";
            }
            string str = string.Empty;
            if (base._httpsStream.IsMutuallyAuthenticated)
            {
                str = "== Client Certificate ==========\nUnknown.\n";
            }
            if (base._httpsStream.LocalCertificate != null)
            {
                str = "\n== Client Certificate ==========\n" + base._httpsStream.LocalCertificate.ToString(true) + "\n";
            }
            StringBuilder builder = new StringBuilder(0x800);
            builder.AppendFormat("Secure Protocol: {0}\n", base._httpsStream.SslProtocol.ToString());
            builder.AppendFormat("Cipher: {0} {1}bits\n", base._httpsStream.CipherAlgorithm.ToString(), base._httpsStream.CipherStrength);
            builder.AppendFormat("Hash Algorithm: {0} {1}bits", base._httpsStream.HashAlgorithm.ToString(), base._httpsStream.HashStrength);
            builder.AppendFormat("Key Exchange: {0} {1}bits\n", base._httpsStream.KeyExchangeAlgorithm.ToString(), base._httpsStream.KeyExchangeStrength);
            builder.Append(str);
            builder.AppendLine("\n== Server Certificate ==========");
            builder.AppendLine(base._httpsStream.RemoteCertificate.ToString(true));
            if (KProxyApplication.Prefs.GetBoolPref("KProxy.network.https.storeservercertchain", false))
            {
                builder.AppendFormat("[Chain]\n {0}\n", this.GetServerCertChain());
            }
            return builder.ToString();
        }

        private X509CertificateCollection GetCertificateCollectionFromFile(string sClientCertificateFilename)
        {
            X509CertificateCollection certificates = null;
            if (!string.IsNullOrEmpty(sClientCertificateFilename))
            {
                sClientCertificateFilename = Utilities.EnsurePathIsAbsolute(KPCONFIG.GetPath("Root"), sClientCertificateFilename);
                if (System.IO.File.Exists(sClientCertificateFilename))
                {
                    certificates = new X509CertificateCollection();
                    certificates.Add(X509Certificate.CreateFromCertFile(sClientCertificateFilename));
                    return certificates;
                }
            }
            return certificates;
        }

        internal string GetServerCertChain()
        {
            if (this._ServerCertChain != null)
            {
                return this._ServerCertChain;
            }
            if (base._httpsStream != null)
            {
                try
                {
                    X509Certificate2 certificate = new X509Certificate2(base._httpsStream.RemoteCertificate);
                    if (certificate == null)
                    {
                        return string.Empty;
                    }
                    StringBuilder builder = new StringBuilder();
                    X509Chain chain = new X509Chain();
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                    chain.Build(certificate);
                    for (int i = chain.ChainElements.Count - 1; i >= 1; i--)
                    {
                        builder.Append(this.SummarizeCert(chain.ChainElements[i].Certificate));
                        builder.Append(" > ");
                    }
                    if (chain.ChainElements.Count > 0)
                    {
                        builder.AppendFormat("{0} [{1}]", this.SummarizeCert(chain.ChainElements[0].Certificate), chain.ChainElements[0].Certificate.SerialNumber);
                    }
                    this._ServerCertChain = builder.ToString();
                    return builder.ToString();
                }
                catch (Exception exception)
                {
                    return exception.Message;
                }
            }
            return string.Empty;
        }

        internal void MarkAsAuthenticated(int clientPID)
        {
            this._isAuthenticated = true;
            int num = KProxyApplication.Prefs.GetInt32Pref("KProxy.network.auth.reusemode", 0);
            if ((num == 0) && (clientPID == 0))
            {
                num = 1;
            }
            if (num == 0)
            {
                this.ReusePolicy = PipeReusePolicy.MarriedToClientProcess;
                this._iMarriedToPID = clientPID;
                this.sPoolKey = string.Format("PID{0}*{1}", clientPID, this.sPoolKey);
            }
            else if (num == 1)
            {
                this.ReusePolicy = PipeReusePolicy.MarriedToClientPipe;
            }
        }

        internal bool SecureExistingConnection(Session oS, string sCertCN, string sClientCertificateFilename, string sPoolingKey, ref int iHandshakeTime)
        {
            RemoteCertificateValidationCallback userCertificateValidationCallback = null;
            LocalCertificateSelectionCallback userCertificateSelectionCallback = null;
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                this.sPoolKey = sPoolingKey;
                X509CertificateCollection certificateCollectionFromFile = this.GetCertificateCollectionFromFile(sClientCertificateFilename);
                if (userCertificateValidationCallback == null)
                {
                    userCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
                        return ConfirmServerCertificate(oS, sCertCN, certificate, chain, sslPolicyErrors);
                    };
                }
                if (userCertificateSelectionCallback == null)
                {
                    userCertificateSelectionCallback = delegate (object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers) {
                        return this.AttachClientCertificate(oS, sender, targetHost, localCertificates, remoteCertificate, acceptableIssuers);
                    };
                }
                base._httpsStream = new SslStream(new NetworkStream(base._baseSocket, false), false, userCertificateValidationCallback, userCertificateSelectionCallback);
                base._httpsStream.AuthenticateAsClient(sCertCN, certificateCollectionFromFile, KPCONFIG.oAcceptedServerHTTPSProtocols, KProxyApplication.Prefs.GetBoolPref("KProxy.https.checkcertificaterevocation", false));
                iHandshakeTime = (int) stopwatch.ElapsedMilliseconds;
            }
            catch (Exception exception)
            {
                iHandshakeTime = (int) stopwatch.ElapsedMilliseconds;
                KProxyApplication.DebugSpew(exception.StackTrace + "\n" + exception.Message);
                return false;
            }
            return true;
        }

        private string SummarizeCert(X509Certificate2 oCert)
        {
            if (!string.IsNullOrEmpty(oCert.FriendlyName))
            {
                return oCert.FriendlyName;
            }
            string subject = oCert.Subject;
            if (string.IsNullOrEmpty(subject))
            {
                return string.Empty;
            }
            if (subject.Contains("CN="))
            {
                return Utilities.TrimAfter(Utilities.TrimBefore(subject, "CN="), ",");
            }
            if (subject.Contains("O="))
            {
                return Utilities.TrimAfter(Utilities.TrimBefore(subject, "O="), ",");
            }
            return subject;
        }

        public override string ToString()
        {
            return string.Format("{0}[Key: {1}; UseCnt: {2} [{3}]; {4}; {5} (:{6} to {7}:{8} {9}) {10}]", new object[] { base._sPipeName, this._sPoolKey, base.iUseCount, base._sHackSessionList, base.bIsSecured ? "Secure" : "PlainText", this._isAuthenticated ? "Authenticated" : "Anonymous", base.LocalPort, base.Address, base.Port, this.isConnectedToGateway ? "Gateway" : "Direct", this._reusePolicy });
        }

        internal bool WrapSocketInPipe(Session oS, Socket oSocket, bool bSecureTheSocket, bool bCreateConnectTunnel, string sCertCN, string sClientCertificateFilename, string sPoolingKey, ref int iHTTPSHandshakeTime)
        {
            RemoteCertificateValidationCallback userCertificateValidationCallback = null;
            LocalCertificateSelectionCallback userCertificateSelectionCallback = null;
            this.sPoolKey = sPoolingKey;
            base._baseSocket = oSocket;
            this.dtConnected = DateTime.Now;
            if (bCreateConnectTunnel)
            {
                this._bIsConnectedToGateway = true;
                base._baseSocket.Send(Encoding.ASCII.GetBytes("CONNECT " + Utilities.TrimBefore(sPoolingKey, ":") + " HTTP/1.1\r\nConnection: close\r\n\r\n"));
                byte[] buffer = new byte[0x2000];
                int num = base._baseSocket.Receive(buffer);
                if ((num <= 12) || !Utilities.isHTTP200Array(buffer))
                {
                    return false;
                }
                this.sPoolKey = "GATEWAY:HTTPS:" + sPoolingKey;
                this._bIsConnectedToGateway = true;
            }
            if (bSecureTheSocket)
            {
                X509CertificateCollection certificateCollectionFromFile = this.GetCertificateCollectionFromFile(sClientCertificateFilename);
                Stopwatch stopwatch = Stopwatch.StartNew();
                if (userCertificateValidationCallback == null)
                {
                    userCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
                        return ConfirmServerCertificate(oS, sCertCN, certificate, chain, sslPolicyErrors);
                    };
                }
                if (userCertificateSelectionCallback == null)
                {
                    userCertificateSelectionCallback = delegate (object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers) {
                        return this.AttachClientCertificate(oS, sender, targetHost, localCertificates, remoteCertificate, acceptableIssuers);
                    };
                }
                base._httpsStream = new SslStream(new NetworkStream(base._baseSocket, false), false, userCertificateValidationCallback, userCertificateSelectionCallback);
                base._httpsStream.AuthenticateAsClient(sCertCN, certificateCollectionFromFile, KPCONFIG.oAcceptedServerHTTPSProtocols, KProxyApplication.Prefs.GetBoolPref("KProxy.https.checkcertificaterevocation", false));
                iHTTPSHandshakeTime = (int) stopwatch.ElapsedMilliseconds;
            }
            return true;
        }

        internal bool isAuthenticated
        {
            get
            {
                return this._isAuthenticated;
            }
        }

        internal bool isClientCertAttached
        {
            get
            {
                return ((base._httpsStream != null) && base._httpsStream.IsMutuallyAuthenticated);
            }
        }

        public bool isConnectedToGateway
        {
            get
            {
                return this._bIsConnectedToGateway;
            }
        }

        public IPEndPoint RemoteEndPoint
        {
            get
            {
                if (base._baseSocket == null)
                {
                    return null;
                }
                return (base._baseSocket.RemoteEndPoint as IPEndPoint);
            }
        }

        public PipeReusePolicy ReusePolicy
        {
            get
            {
                return this._reusePolicy;
            }
            set
            {
                this._reusePolicy = value;
            }
        }

        public string sPoolKey
        {
            get
            {
                return this._sPoolKey;
            }
            private set
            {
                this._sPoolKey = value;
            }
        }
    }
}

