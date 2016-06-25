namespace KProxy
{
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.AccessControl;
    using System.Text;
    using System.Threading;

    internal class PreferenceBag : IKProxyPreferences
    {
        private static char[] _arrForbiddenChars = new char[] { '*', ' ', '$', '%', '@', '?', '!' };
        private readonly StringDictionary _dictPrefs = new StringDictionary();
        private readonly List<PrefWatcher> _listWatchers = new List<PrefWatcher>();
        private readonly ReaderWriterLock _RWLockPrefs = new ReaderWriterLock();
        private readonly ReaderWriterLock _RWLockWatchers = new ReaderWriterLock();
        private string _sCurrentProfile = ".default";
        private string _sRegistryPath;

        internal PreferenceBag(string sRegPath)
        {
            this._sRegistryPath = sRegPath;
            this.ReadRegistry();
        }

        private void _NotifyThreadExecute(object objThreadState)
        {
            PrefChangeEventArgs e = (PrefChangeEventArgs) objThreadState;
            string prefName = e.PrefName;
            List<EventHandler<PrefChangeEventArgs>> list = null;
            try
            {
                GetReaderLock(this._RWLockWatchers);
                try
                {
                    foreach (PrefWatcher watcher in this._listWatchers)
                    {
                        if (prefName.StartsWith(watcher.sPrefixToWatch, StringComparison.Ordinal))
                        {
                            if (list == null)
                            {
                                list = new List<EventHandler<PrefChangeEventArgs>>();
                            }
                            list.Add(watcher.fnToNotify);
                        }
                    }
                }
                finally
                {
                    FreeReaderLock(this._RWLockWatchers);
                }
                if (list != null)
                {
                    foreach (EventHandler<PrefChangeEventArgs> handler in list)
                    {
                        try
                        {
                            handler(this, e);
                        }
                        catch (Exception exception)
                        {
                            KProxyApplication.ReportException(exception);
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                KProxyApplication.ReportException(exception2);
            }
        }

        public PrefWatcher AddWatcher(string sPrefixFilter, EventHandler<PrefChangeEventArgs> pcehHandler)
        {
            PrefWatcher item = new PrefWatcher(sPrefixFilter.ToLower(), pcehHandler);
            GetWriterLock(this._RWLockWatchers);
            try
            {
                this._listWatchers.Add(item);
            }
            finally
            {
                FreeWriterLock(this._RWLockWatchers);
            }
            return item;
        }

        private void AsyncNotifyWatchers(PrefChangeEventArgs oNotifyArgs)
        {
            ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(this._NotifyThreadExecute), oNotifyArgs);
        }

        public void Close()
        {
            this._listWatchers.Clear();
            this.WriteRegistry();
        }

        internal string FindMatches(string sFilter)
        {
            StringBuilder builder = new StringBuilder(0x80);
            try
            {
                GetReaderLock(this._RWLockPrefs);
                foreach (DictionaryEntry entry in this._dictPrefs)
                {
                    if (((string) entry.Key).IndexOf(sFilter, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        builder.AppendFormat("{0}:\t{1}\r\n", entry.Key, entry.Value);
                    }
                }
            }
            finally
            {
                FreeReaderLock(this._RWLockPrefs);
            }
            return builder.ToString();
        }

        private static void FreeReaderLock(ReaderWriterLock oLock)
        {
            oLock.ReleaseReaderLock();
        }

        private static void FreeWriterLock(ReaderWriterLock oLock)
        {
            oLock.ReleaseWriterLock();
        }

        public bool GetBoolPref(string sPrefName, bool bDefault)
        {
            bool flag;
            string str = this[sPrefName];
            if ((str != null) && bool.TryParse(str, out flag))
            {
                return flag;
            }
            return bDefault;
        }

        public int GetInt32Pref(string sPrefName, int iDefault)
        {
            int num;
            string s = this[sPrefName];
            if ((s != null) && int.TryParse(s, out num))
            {
                return num;
            }
            return iDefault;
        }

        public string[] GetPrefArray()
        {
            string[] strArray2;
            try
            {
                GetReaderLock(this._RWLockPrefs);
                string[] array = new string[this._dictPrefs.Count];
                this._dictPrefs.Keys.CopyTo(array, 0);
                strArray2 = array;
            }
            finally
            {
                FreeReaderLock(this._RWLockPrefs);
            }
            return strArray2;
        }

        private static void GetReaderLock(ReaderWriterLock oLock)
        {
            oLock.AcquireReaderLock(-1);
        }

        public string GetStringPref(string sPrefName, string sDefault)
        {
            string str = this[sPrefName];
            return (str ?? sDefault);
        }

        private static void GetWriterLock(ReaderWriterLock oLock)
        {
            oLock.AcquireWriterLock(-1);
        }

        private bool isValidName(string sName)
        {
            return (((!string.IsNullOrEmpty(sName) && (0x100 > sName.Length)) && (0 > sName.IndexOf("internal", StringComparison.OrdinalIgnoreCase))) && (0 > sName.IndexOfAny(_arrForbiddenChars)));
        }

        private void ReadRegistry()
        {
            if (this._sRegistryPath != null)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(this._sRegistryPath + @"\" + this._sCurrentProfile, RegistryKeyPermissionCheck.ReadSubTree, RegistryRights.ExecuteKey);
                if (key != null)
                {
                    string[] valueNames = key.GetValueNames();
                    try
                    {
                        GetWriterLock(this._RWLockPrefs);
                        foreach (string str in valueNames)
                        {
                            if ((str.Length >= 1) && !str.Contains("ephemeral"))
                            {
                                try
                                {
                                    this._dictPrefs[str] = (string) key.GetValue(str, string.Empty);
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    finally
                    {
                        FreeWriterLock(this._RWLockPrefs);
                        key.Close();
                    }
                }
            }
        }

        public void RemovePref(string sPrefName)
        {
            bool flag = false;
            try
            {
                GetWriterLock(this._RWLockPrefs);
                flag = this._dictPrefs.ContainsKey(sPrefName);
                this._dictPrefs.Remove(sPrefName);
            }
            finally
            {
                FreeWriterLock(this._RWLockPrefs);
            }
            if (flag)
            {
                PrefChangeEventArgs oNotifyArgs = new PrefChangeEventArgs(sPrefName, null);
                this.AsyncNotifyWatchers(oNotifyArgs);
            }
        }

        public void RemoveWatcher(PrefWatcher wliToRemove)
        {
            GetWriterLock(this._RWLockWatchers);
            try
            {
                this._listWatchers.Remove(wliToRemove);
            }
            finally
            {
                FreeWriterLock(this._RWLockWatchers);
            }
        }

        public void SetBoolPref(string sPrefName, bool bValue)
        {
            this[sPrefName] = bValue.ToString();
        }

        public void SetInt32Pref(string sPrefName, int iValue)
        {
            this[sPrefName] = iValue.ToString();
        }

        public void SetStringPref(string sPrefName, string sValue)
        {
            this[sPrefName] = sValue;
        }

        public override string ToString()
        {
            return this.ToString(true);
        }

        public string ToString(bool bVerbose)
        {
            StringBuilder builder = new StringBuilder(0x80);
            try
            {
                GetReaderLock(this._RWLockPrefs);
                builder.AppendFormat("PreferenceBag [{0} Preferences. {1} Watchers.]", this._dictPrefs.Count, this._listWatchers.Count);
                if (bVerbose)
                {
                    builder.Append("\n");
                    foreach (DictionaryEntry entry in this._dictPrefs)
                    {
                        builder.AppendFormat("{0}:\t{1}\n", entry.Key, entry.Value);
                    }
                }
            }
            finally
            {
                FreeReaderLock(this._RWLockPrefs);
            }
            return builder.ToString();
        }

        private void WriteRegistry()
        {
            if (!KPCONFIG.bIsViewOnly && (this._sRegistryPath != null))
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(this._sRegistryPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
                if (key != null)
                {
                    try
                    {
                        GetReaderLock(this._RWLockPrefs);
                        key.DeleteSubKey(this._sCurrentProfile, false);
                        if (this._dictPrefs.Count >= 1)
                        {
                            key = key.CreateSubKey(this._sCurrentProfile, RegistryKeyPermissionCheck.ReadWriteSubTree);
                            foreach (DictionaryEntry entry in this._dictPrefs)
                            {
                                string name = (string) entry.Key;
                                if (!name.Contains("ephemeral"))
                                {
                                    key.SetValue(name, entry.Value);
                                }
                            }
                        }
                    }
                    finally
                    {
                        FreeReaderLock(this._RWLockPrefs);
                        key.Close();
                    }
                }
            }
        }

        public string CurrentProfile
        {
            get
            {
                return this._sCurrentProfile;
            }
        }

        public string this[string sPrefName]
        {
            get
            {
                string str;
                try
                {
                    GetReaderLock(this._RWLockPrefs);
                    str = this._dictPrefs[sPrefName];
                }
                finally
                {
                    FreeReaderLock(this._RWLockPrefs);
                }
                return str;
            }
            set
            {
                if (!this.isValidName(sPrefName))
                {
                    throw new ArgumentException(string.Format("Preference name must contain 1 or more characters from the set A-z0-9-_ and may not contain the word Internal.\nYou tried to set: \"{0}\"", sPrefName));
                }
                if (value == null)
                {
                    this.RemovePref(sPrefName);
                }
                else
                {
                    bool flag = false;
                    try
                    {
                        GetWriterLock(this._RWLockPrefs);
                        flag = !this._dictPrefs.ContainsKey(sPrefName) || (this._dictPrefs[sPrefName] != value);
                        this._dictPrefs[sPrefName] = value;
                    }
                    finally
                    {
                        FreeWriterLock(this._RWLockPrefs);
                    }
                    if (flag)
                    {
                        PrefChangeEventArgs oNotifyArgs = new PrefChangeEventArgs(sPrefName, value);
                        this.AsyncNotifyWatchers(oNotifyArgs);
                    }
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PrefWatcher
        {
            internal readonly EventHandler<PrefChangeEventArgs> fnToNotify;
            internal readonly string sPrefixToWatch;
            internal PrefWatcher(string sPrefixFilter, EventHandler<PrefChangeEventArgs> fnHandler)
            {
                this.sPrefixToWatch = sPrefixFilter;
                this.fnToNotify = fnHandler;
            }
        }
    }
}

