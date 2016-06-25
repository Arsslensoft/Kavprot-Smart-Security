namespace KProxy
{
    using System;

    public class SessionTimers
    {
        public DateTime ClientBeginRequest;
        public DateTime ClientBeginResponse;
        public DateTime ClientConnected;
        public DateTime ClientDoneRequest;
        public DateTime ClientDoneResponse;
        public int DNSTime;
        public DateTime KProxyBeginRequest;
        public DateTime KProxyGotRequestHeaders;
        public DateTime KProxyGotResponseHeaders;
        public int GatewayDeterminationTime;
        public int HTTPSHandshakeTime;
        public DateTime ServerBeginResponse;
        public DateTime ServerConnected;
        public DateTime ServerDoneResponse;
        public DateTime ServerGotRequest;
        public int TCPConnectTime;

        public override string ToString()
        {
            return this.ToString(false);
        }

        public string ToString(bool bMultiLine)
        {
            if (bMultiLine)
            {
                return string.Format("ClientConnected:\t{0:HH:mm:ss.fff}\r\nClientBeginRequest:\t{1:HH:mm:ss.fff}\r\nGotRequestHeaders:\t{2:HH:mm:ss.fff}\r\nClientDoneRequest:\t{3:HH:mm:ss.fff}\r\nDetermine Gateway:\t{4,0}ms\r\nDNS Lookup: \t\t{5,0}ms\r\nTCP/IP Connect:\t{6,0}ms\r\nHTTPS Handshake:\t{7,0}ms\r\nServerConnected:\t{8:HH:mm:ss.fff}\r\nKProxyBeginRequest:\t{9:HH:mm:ss.fff}\r\nServerGotRequest:\t{10:HH:mm:ss.fff}\r\nServerBeginResponse:\t{11:HH:mm:ss.fff}\r\nGotResponseHeaders:\t{12:HH:mm:ss.fff}\r\nServerDoneResponse:\t{13:HH:mm:ss.fff}\r\nClientBeginResponse:\t{14:HH:mm:ss.fff}\r\nClientDoneResponse:\t{15:HH:mm:ss.fff}\r\n\r\n{16}", new object[] { 
                    this.ClientConnected, this.ClientBeginRequest, this.KProxyGotRequestHeaders, this.ClientDoneRequest, this.GatewayDeterminationTime, this.DNSTime, this.TCPConnectTime, this.HTTPSHandshakeTime, this.ServerConnected, this.KProxyBeginRequest, this.ServerGotRequest, this.ServerBeginResponse, this.KProxyGotResponseHeaders, this.ServerDoneResponse, this.ClientBeginResponse, this.ClientDoneResponse, 
                    (TimeSpan.Zero < (this.ClientDoneResponse - this.ClientBeginRequest)) ? string.Format("\tOverall Elapsed:\t{0:h\\:mm\\:ss\\.fff}\r\n", (TimeSpan) (this.ClientDoneResponse - this.ClientBeginRequest)) : string.Empty
                 });
            }
            return string.Format("ClientConnected: {0:HH:mm:ss.fff}, ClientBeginRequest: {1:HH:mm:ss.fff}, GotRequestHeaders: {2:HH:mm:ss.fff}, ClientDoneRequest: {3:HH:mm:ss.fff}, Determine Gateway: {4,0}ms, DNS Lookup: {5,0}ms, TCP/IP Connect: {6,0}ms, HTTPS Handshake: {7,0}ms, ServerConnected: {8:HH:mm:ss.fff},KProxyBeginRequest: {9:HH:mm:ss.fff}, ServerGotRequest: {10:HH:mm:ss.fff}, ServerBeginResponse: {11:HH:mm:ss.fff}, GotResponseHeaders: {12:HH:mm:ss.fff}, ServerDoneResponse: {13:HH:mm:ss.fff}, ClientBeginResponse: {14:HH:mm:ss.fff}, ClientDoneResponse: {15:HH:mm:ss.fff}{16}", new object[] { 
                this.ClientConnected, this.ClientBeginRequest, this.KProxyGotRequestHeaders, this.ClientDoneRequest, this.GatewayDeterminationTime, this.DNSTime, this.TCPConnectTime, this.HTTPSHandshakeTime, this.ServerConnected, this.KProxyBeginRequest, this.ServerGotRequest, this.ServerBeginResponse, this.KProxyGotResponseHeaders, this.ServerDoneResponse, this.ClientBeginResponse, this.ClientDoneResponse, 
                (TimeSpan.Zero < (this.ClientDoneResponse - this.ClientBeginRequest)) ? string.Format(@", Overall Elapsed: {0:h\:mm\:ss\.fff}", (TimeSpan) (this.ClientDoneResponse - this.ClientBeginRequest)) : string.Empty
             });
        }
    }
}

