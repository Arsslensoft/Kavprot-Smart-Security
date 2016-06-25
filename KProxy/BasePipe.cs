using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

namespace KProxy
{
     public abstract class BasePipe
    {
        protected Socket _baseSocket;
        protected SslStream _httpsStream;
        private int _iTransmitDelayMS;
        protected internal string _sHackSessionList;
        protected internal string _sPipeName;
        private bool _bIsWebSocket;
        protected internal uint iUseCount;

        public BasePipe(Socket oSocket, string sName)
        {
            this._sPipeName = sName;
            this._baseSocket = oSocket;
        }

        public void End()
        {
            try
            {
                if (this._httpsStream != null)
                {
                    this._httpsStream.Close();
                }
                if (this._baseSocket != null)
                {
                    this._baseSocket.Shutdown(SocketShutdown.Both);
                    this._baseSocket.Close();
                }
            }
            catch
            {
            }
            this._baseSocket = null;
            this._httpsStream = null;
        }

        public Socket GetRawSocket()
        {
            return this._baseSocket;
        }

        internal void IncrementUse(int iSession)
        {
            this._iTransmitDelayMS = 0;
            this.iUseCount++;
            this._sHackSessionList = this._sHackSessionList + iSession.ToString() + ",";
        }

        internal int Receive(byte[] arrBuffer)
        {
            if (this.bIsSecured)
            {
                return this._httpsStream.Read(arrBuffer, 0, arrBuffer.Length);
            }
            return this._baseSocket.Receive(arrBuffer);
        }

        public void Send(byte[] oBytes)
        {
            this.Send(oBytes, 0, oBytes.Length);
        }

        internal void Send(byte[] oBytes, int iOffset, int iCount)
        {
            if (oBytes != null)
            {
                if ((iOffset + iCount) > oBytes.LongLength)
                {
                    iCount = oBytes.Length - iOffset;
                }
                if (iCount >= 1)
                {
                    if (this._iTransmitDelayMS < 1)
                    {
                        if (this.bIsSecured)
                        {
                            this._httpsStream.Write(oBytes, iOffset, iCount);
                        }
                        else
                        {
                            this._baseSocket.Send(oBytes, iOffset, iCount, SocketFlags.None);
                        }
                    }
                    else
                    {
                        int count = 0x400;
                        for (int i = iOffset; i < (iOffset + iCount); i += count)
                        {
                            if ((i + count) > (iOffset + iCount))
                            {
                                count = (iOffset + iCount) - i;
                            }
                            Thread.Sleep((int) (this._iTransmitDelayMS / 2));
                            if (this.bIsSecured)
                            {
                                this._httpsStream.Write(oBytes, i, count);
                            }
                            else
                            {
                                this._baseSocket.Send(oBytes, i, count, SocketFlags.None);
                            }
                            Thread.Sleep((int) (this._iTransmitDelayMS / 2));
                        }
                    }
                }
            }
        }

        public IPAddress Address
        {
            get
            {
                if ((this._baseSocket != null) && (this._baseSocket.RemoteEndPoint != null))
                {
                    return (this._baseSocket.RemoteEndPoint as IPEndPoint).Address;
                }
                return new IPAddress(0L);
            }
        }

        public bool bIsSecured
        {
            get
            {
                return (null != this._httpsStream);
            }
        }

        public bool bIsWebSocket
        {
            get
            {
                return this._bIsWebSocket;
            }
           set
            {
                this._bIsWebSocket = value;
            }
        }

        public bool Connected
        {
            get
            {
                if (this._baseSocket == null)
                {
                    return false;
                }
                return this._baseSocket.Connected;
            }
        }

        public int LocalPort
        {
            get
            {
                if ((this._baseSocket != null) && (this._baseSocket.LocalEndPoint != null))
                {
                    return (this._baseSocket.LocalEndPoint as IPEndPoint).Port;
                }
                return 0;
            }
        }

        public int Port
        {
            get
            {
                if ((this._baseSocket != null) && (this._baseSocket.RemoteEndPoint != null))
                {
                    return (this._baseSocket.RemoteEndPoint as IPEndPoint).Port;
                }
                return 0;
            }
        }

        public int TransmitDelay
        {
            get
            {
                return this._iTransmitDelayMS;
            }
            set
            {
                this._iTransmitDelayMS = value;
            }
        }
    }
    public class ClientPipe : BasePipe
    {
        private byte[] _arrReceivedAndPutBack;
        private static bool _bWantClientCert = KProxyApplication.Prefs.GetBoolPref("KProxy.network.https.requestclientcertificate", false);
        private int _iProcessID;
        private string _sProcessName;
        internal static int _timeoutReceiveInitial = KProxyApplication.Prefs.GetInt32Pref("KProxy.network.timeouts.clientpipe.receive.initial", 0xea60);
        internal static int _timeoutReceiveReused = KProxyApplication.Prefs.GetInt32Pref("KProxy.network.timeouts.clientpipe.receive.reuse", 0x7530);

        internal ClientPipe(Socket oSocket)
            : base(oSocket, "C")
        {
            try
            {
                oSocket.NoDelay = true;
                if (KPCONFIG.bMapSocketToProcess)
                {
                    this._iProcessID = Winsock.MapLocalPortToProcessId(((IPEndPoint)oSocket.RemoteEndPoint).Port);
                    if (this._iProcessID > 0)
                    {
                        this._sProcessName = ProcessHelper.GetProcessName(this._iProcessID);
                    }
                }
            }
            catch
            {
            }
        }

        internal void putBackSomeBytes(byte[] toPutback)
        {
            this._arrReceivedAndPutBack = new byte[toPutback.Length];
            Buffer.BlockCopy(toPutback, 0, this._arrReceivedAndPutBack, 0, toPutback.Length);
        }

        internal int Receive(byte[] arrBuffer)
        {
            if (this._arrReceivedAndPutBack == null)
            {
                return base.Receive(arrBuffer);
            }
            int length = this._arrReceivedAndPutBack.Length;
            Buffer.BlockCopy(this._arrReceivedAndPutBack, 0, arrBuffer, 0, length);
            this._arrReceivedAndPutBack = null;
            return length;
        }

        internal bool SecureClientPipe(string sHostname, HTTPResponseHeaders oHeaders)
        {
            X509Certificate2 certificate;
            try
            {
                certificate = CertMaker.FindCert(sHostname, true);
            }
            catch (Exception exception)
            {
                //KProxyApplication.Log.LogFormat("KProxy.https> Failed to obtain certificate for {0} due to {1}", new object[] { sHostname, exception.Message });
                certificate = null;
            }
            try
            {
                if (certificate == null)
                {
                    KProxyApplication.DoNotifyUser("Unable to find Certificate for " + sHostname, "HTTPS Interception Failure");
                    oHeaders.HTTPResponseCode = 0x1f6;
                    oHeaders.HTTPResponseStatus = "502 KProxy unable to generate certificate";
                }
                if (KPCONFIG.bDebugSpew)
                {
                    KProxyApplication.DebugSpew("SecureClientPipe for: " + this.ToString() + " sending data to client:\n" + Utilities.ByteArrayToHexView(oHeaders.ToByteArray(true, true), 0x20));
                }
                base.Send(oHeaders.ToByteArray(true, true));
                if (oHeaders.HTTPResponseCode != 200)
                {
                    KProxyApplication.DebugSpew("SecureClientPipe returning FALSE because HTTPResponseCode != 200");
                    return false;
                }
                base._httpsStream = new SslStream(new NetworkStream(base._baseSocket, false), false);
                base._httpsStream.AuthenticateAsServer(certificate, _bWantClientCert, KPCONFIG.oAcceptedClientHTTPSProtocols, false);
                return true;
            }
            catch (Exception exception2)
            {
                //KProxyApplication.Log.LogFormat("Secure client pipe failed: {0}{1}.", new object[] { exception2.Message, (exception2.InnerException == null) ? string.Empty : (" InnerException: " + exception2.InnerException.Message) });
                KProxyApplication.DebugSpew("Secure client pipe failed: " + exception2.Message);
                try
                {
                    base.End();
                }
                catch
                {
                }
            }
            return false;
        }

        internal bool SecureClientPipeDirect(X509Certificate2 certServer)
        {
            try
            {
                base._httpsStream = new SslStream(new NetworkStream(base._baseSocket, false), false);
                base._httpsStream.AuthenticateAsServer(certServer, _bWantClientCert, KPCONFIG.oAcceptedClientHTTPSProtocols, false);
                return true;
            }
            catch (Exception exception)
            {
                //KProxyApplication.Log.LogFormat("Secure client pipe failed: {0}{1}.", new object[] { exception.Message, (exception.InnerException == null) ? string.Empty : (" InnerException: " + exception.InnerException.Message) });
                KProxyApplication.DebugSpew("Secure client pipe failed: " + exception.Message);
                try
                {
                    base.End();
                }
                catch
                {
                }
            }
            return false;
        }

        internal void setReceiveTimeout()
        {
            try
            {
                base._baseSocket.ReceiveTimeout = (base.iUseCount < 2) ? _timeoutReceiveInitial : _timeoutReceiveReused;
            }
            catch
            {
            }
        }

        public override string ToString()
        {
            return string.Format("[ClientPipe: {0}:{1}; UseCnt: {2}; Port: {3}; {4}]", new object[] { this._sProcessName, this._iProcessID, base.iUseCount, base.Port, base.bIsSecured ? "SECURE" : "PLAINTTEXT" });
        }

        public int LocalProcessID
        {
            get
            {
                return this._iProcessID;
            }
        }

        public string LocalProcessName
        {
            get
            {
                return (this._sProcessName ?? string.Empty);
            }
        }
    }
}

