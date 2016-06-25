namespace KProxy
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Collections.Generic;

    public interface ICertificateProvider
    {
   
        bool ClearCertificateCache();
        bool CreateRootCertificate();
        X509Certificate2 GetCertificateForHost(string sHostname);
        X509Certificate2 GetRootCertificate();
        bool rootCertIsTrusted(out bool bUserTrusted, out bool bMachineTrusted);
        bool TrustRootCertificate();
    }
    public interface ICertificateProvider2 : ICertificateProvider
    {
        bool ClearCertificateCache(bool bClearRoot);
    }
    internal interface IKProxyPreferences
    {
        PreferenceBag.PrefWatcher AddWatcher(string sPrefixFilter, EventHandler<PrefChangeEventArgs> pcehHandler);
        bool GetBoolPref(string sPrefName, bool bDefault);
        int GetInt32Pref(string sPrefName, int iDefault);
        string GetStringPref(string sPrefName, string sDefault);
        void RemovePref(string sPrefName);
        void RemoveWatcher(PreferenceBag.PrefWatcher wliToRemove);
        void SetBoolPref(string sPrefName, bool bValue);
        void SetInt32Pref(string sPrefName, int iValue);
        void SetStringPref(string sPrefName, string sValue);

        string this[string sName] { get; set; }
    }

    public interface ISessionExporter : IDisposable
    {
        bool ExportSessions(string sExportFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications);
    }
    public interface ISessionImporter : IDisposable
    {
        Session[] ImportSessions(string sImportFormat, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications);
    }
}

