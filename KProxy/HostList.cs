namespace KProxy
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    public class HostList
    {
        private bool bEverythingMatches;
        private bool bLoopbackMatches;
        private bool bNonPlainHostnameMatches;
        private bool bPlainHostnameMatches;
        private List<HostPortTuple> hplComplexRules;
        private List<string> slSimpleHosts;

        public HostList()
        {
            this.slSimpleHosts = new List<string>();
            this.hplComplexRules = new List<HostPortTuple>();
        }

        public HostList(string sInitialList)
        {
            this.slSimpleHosts = new List<string>();
            this.hplComplexRules = new List<HostPortTuple>();
            this.AssignFromString(sInitialList);
        }

        public bool AssignFromString(string sIn)
        {
            string str;
            return this.AssignFromString(sIn, out str);
        }

        public bool AssignFromString(string sIn, out string sErrors)
        {
            sErrors = string.Empty;
            this.Clear();
            sIn = sIn.Trim();
            if (string.IsNullOrEmpty(sIn))
            {
                return true;
            }
            foreach (string str in sIn.ToLower().Split(new char[] { ',', ';', '\t', ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (str.Equals("*"))
                {
                    this.bEverythingMatches = true;
                }
                else
                {
                    if (str.StartsWith("<"))
                    {
                        if (str.Equals("<loopback>"))
                        {
                            this.bLoopbackMatches = true;
                            goto Label_0153;
                        }
                        if (str.Equals("<local>"))
                        {
                            this.bPlainHostnameMatches = true;
                            goto Label_0153;
                        }
                        if (str.Equals("<nonlocal>"))
                        {
                            this.bNonPlainHostnameMatches = true;
                            goto Label_0153;
                        }
                    }
                    if (str.Length >= 1)
                    {
                        if (str.Contains("?"))
                        {
                            sErrors = sErrors + string.Format("Ignored invalid rule '{0}'-- ? may not appear.\n", str);
                        }
                        else if (str.LastIndexOf("*") > 0)
                        {
                            sErrors = sErrors + string.Format("Ignored invalid rule '{0}'-- * may only appear at the front of the string.\n", str);
                        }
                        else
                        {
                            string str2;
                            int iPort = -1;
                            Utilities.CrackHostAndPort(str, out str2, ref iPort);
                            if ((-1 == iPort) && !str2.StartsWith("*"))
                            {
                                this.slSimpleHosts.Add(str);
                            }
                            else
                            {
                                HostPortTuple item = new HostPortTuple(str2, iPort);
                                this.hplComplexRules.Add(item);
                            }
                        }
                    }
                Label_0153:;
                }
            }
            if (this.bNonPlainHostnameMatches && this.bPlainHostnameMatches)
            {
                this.bEverythingMatches = true;
            }
            return (sErrors == string.Empty);
        }

        public void Clear()
        {
            this.bLoopbackMatches = this.bPlainHostnameMatches = this.bNonPlainHostnameMatches = this.bEverythingMatches = false;
            this.slSimpleHosts.Clear();
            this.hplComplexRules.Clear();
        }

        public bool ContainsHost(string sHost)
        {
            string str;
            int iPort = -1;
            Utilities.CrackHostAndPort(sHost, out str, ref iPort);
            return this.ContainsHost(str, iPort);
        }

        public bool ContainsHost(string sHostname, int iPort)
        {
            if (this.bEverythingMatches)
            {
                return true;
            }
            if (this.bPlainHostnameMatches || this.bNonPlainHostnameMatches)
            {
                bool flag = Utilities.isPlainHostName(sHostname);
                if (this.bPlainHostnameMatches && flag)
                {
                    return true;
                }
                if (this.bNonPlainHostnameMatches && !flag)
                {
                    return true;
                }
            }
            if (this.bLoopbackMatches && Utilities.isLocalhostname(sHostname))
            {
                return true;
            }
            sHostname = sHostname.ToLower();
            if (this.slSimpleHosts.Contains(sHostname))
            {
                return true;
            }
            foreach (HostPortTuple tuple in this.hplComplexRules)
            {
                if ((iPort == tuple._iPort) || (-1 == tuple._iPort))
                {
                    if (tuple._bTailMatch && sHostname.EndsWith(tuple._sHostname))
                    {
                        return true;
                    }
                    if (tuple._sHostname == sHostname)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ContainsHostname(string sHostname)
        {
            return this.ContainsHost(sHostname, -1);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (this.bEverythingMatches)
            {
                builder.Append("*; ");
            }
            if (this.bPlainHostnameMatches)
            {
                builder.Append("<local>; ");
            }
            if (this.bNonPlainHostnameMatches)
            {
                builder.Append("<nonlocal>; ");
            }
            if (this.bLoopbackMatches)
            {
                builder.Append("<loopback>; ");
            }
            foreach (string str in this.slSimpleHosts)
            {
                builder.Append(str);
                builder.Append("; ");
            }
            foreach (HostPortTuple tuple in this.hplComplexRules)
            {
                if (tuple._bTailMatch)
                {
                    builder.Append("*");
                }
                builder.Append(tuple._sHostname);
                if (tuple._iPort > -1)
                {
                    builder.Append(":");
                    builder.Append(tuple._iPort.ToString());
                }
                builder.Append("; ");
            }
            if (builder.Length > 1)
            {
                builder.Remove(builder.Length - 1, 1);
            }
            return builder.ToString();
        }

        private class HostPortTuple
        {
            public bool _bTailMatch;
            public int _iPort;
            public string _sHostname;

            internal HostPortTuple(string sHostname, int iPort)
            {
                this._iPort = iPort;
                if (sHostname.StartsWith("*"))
                {
                    this._bTailMatch = true;
                    this._sHostname = sHostname.Substring(1);
                }
                else
                {
                    this._sHostname = sHostname;
                }
            }
        }
    }
}

