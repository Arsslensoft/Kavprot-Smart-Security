namespace KProxy
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class WinINETConnectoids
    {
        internal Dictionary<string, WinINETConnectoid> _oConnectoids = new Dictionary<string, WinINETConnectoid>();

        public WinINETConnectoids()
        {
            foreach (string str in RASInfo.GetConnectionNames())
            {
                if (!this._oConnectoids.ContainsKey(str))
                {
                    WinINETProxyInfo info = WinINETProxyInfo.CreateFromNamedConnection(str);
                    if (info != null)
                    {
                        WinINETConnectoid connectoid = new WinINETConnectoid();
                        connectoid.sConnectionName = str;
                        if ((info.sHttpProxy != null) && info.sHttpProxy.Contains(KPCONFIG.sKProxyListenHostPort))
                        {
                            info.sHttpProxy = info.sHttpsProxy = (string) (info.sFtpProxy = null);
                            info.bUseManualProxies = false;
                            info.bAllowDirect = true;
                        }
                        connectoid.oOriginalProxyInfo = info;
                        this._oConnectoids.Add(str, connectoid);
                    }
                }
            }
        }

        internal WinINETProxyInfo GetDefaultConnectionGatewayInfo()
        {
            string sHookConnectionNamed = KPCONFIG.sHookConnectionNamed;
            if (string.IsNullOrEmpty(sHookConnectionNamed))
            {
                sHookConnectionNamed = "DefaultLAN";
            }
            if (!this._oConnectoids.ContainsKey(sHookConnectionNamed))
            {
                sHookConnectionNamed = "DefaultLAN";
            }
            return this._oConnectoids[sHookConnectionNamed].oOriginalProxyInfo;
        }

        internal bool HookConnections(WinINETProxyInfo oNewInfo)
        {
            if (KPCONFIG.bIsViewOnly)
            {
                return false;
            }
            bool flag = false;
            foreach (WinINETConnectoid connectoid in this._oConnectoids.Values)
            {
                if ((KPCONFIG.bHookAllConnections || (connectoid.sConnectionName == KPCONFIG.sHookConnectionNamed)) && oNewInfo.SetToWinINET(connectoid.sConnectionName))
                {
                    flag = true;
                    connectoid.bIsHooked = true;
                }
            }
            return flag;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("WinINET Connectoids");
            foreach (KeyValuePair<string, WinINETConnectoid> pair in this._oConnectoids)
            {
                builder.AppendFormat("\nWinINET settings for '{0}' are:\n{1}\n\n", pair.Key, pair.Value.oOriginalProxyInfo.ToString());
            }
            return builder.ToString();
        }

        internal bool UnhookAllConnections()
        {
            if (KPCONFIG.bIsViewOnly)
            {
                return false;
            }
            bool flag = false;
            foreach (WinINETConnectoid connectoid in this._oConnectoids.Values)
            {
                if (connectoid.bIsHooked && connectoid.oOriginalProxyInfo.SetToWinINET(connectoid.sConnectionName))
                {
                    flag = true;
                    connectoid.bIsHooked = false;
                }
            }
            return flag;
        }
    }
}

