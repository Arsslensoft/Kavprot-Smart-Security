namespace KProxy
{
    using System;
    using System.Net.Security;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;

    public class ValidateServerCertificateEventArgs : EventArgs
    {
        private readonly X509Certificate _oServerCertificate;
        private readonly Session _oSession;
        private readonly X509Chain _ServerCertificateChain;
        private readonly string _sExpectedCN;
        private readonly SslPolicyErrors _sslPolicyErrors;
        private CertificateValidity _ValidityState;

        internal ValidateServerCertificateEventArgs(Session inSession, string inExpectedCN, X509Certificate inServerCertificate, X509Chain inServerCertificateChain, SslPolicyErrors inSslPolicyErrors)
        {
            this._oSession = inSession;
            this._sExpectedCN = inExpectedCN;
            this._oServerCertificate = inServerCertificate;
            this._ServerCertificateChain = inServerCertificateChain;
            this._sslPolicyErrors = inSslPolicyErrors;
        }

        public SslPolicyErrors CertificatePolicyErrors
        {
            get
            {
                return this._sslPolicyErrors;
            }
        }

        public string ExpectedCN
        {
            get
            {
                return this._sExpectedCN;
            }
        }

        public X509Certificate ServerCertificate
        {
            get
            {
                return this._oServerCertificate;
            }
        }

        public X509Chain ServerCertificateChain
        {
            get
            {
                return this._ServerCertificateChain;
            }
        }

        public Session Session
        {
            get
            {
                return this._oSession;
            }
        }

        public int TargetPort
        {
            get
            {
                return this._oSession.port;
            }
        }

        public CertificateValidity ValidityState
        {
            [CompilerGenerated]
            get
            {
                return this._ValidityState;
            }
            [CompilerGenerated]
            set
            {
                this._ValidityState = value;
            }
        }
    }
}

