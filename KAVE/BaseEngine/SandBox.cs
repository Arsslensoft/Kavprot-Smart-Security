using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;
using KAVE.BaseEngine.Classes;

namespace KAVE
{
   public class SandBox
    {
       SandBoxSettings sets;
       public SandBoxSettings Settings
       {
           get { return sets; }
           set { sets = value; }
       }
       public StrongName CreateStrongName(Assembly assembly)
       {
           if (assembly == null)
               throw new ArgumentNullException("assembly");

           AssemblyName assemblyName = assembly.GetName();
           Debug.Assert(assemblyName != null, "Could not get assembly name");

           // get the public key blob
           byte[] publicKey = assemblyName.GetPublicKey();
           if (publicKey == null || publicKey.Length == 0)
               throw new InvalidOperationException("Assembly is not strongly named");

           StrongNamePublicKeyBlob keyBlob = new StrongNamePublicKeyBlob(publicKey);

           // and create the StrongName
           return new StrongName(keyBlob, assemblyName.Name, assemblyName.Version);
       }
      public AppDomain sandbox;
      public SandBox(bool ACF, bool APC, bool AREG, bool AFDLG, bool AENV, bool GUI, bool EVLOG, KAVE.BaseEngine.Classes.SecurityState security)
      {
          Settings = new SandBoxSettings(ACF, APC, AREG, AFDLG, AENV, GUI, EVLOG, security);
      }
       public void Start( string assembly, string Name)
       {
           PermissionSet pset = new PermissionSet(PermissionState.None);
           if (Settings.Security == KAVE.BaseEngine.Classes.SecurityState.FullTrusted)
           {
               pset.AddPermission(new SecurityPermission(SecurityPermissionFlag.AllFlags));
               pset.AddPermission(new System.Security.Permissions.IsolatedStorageFilePermission(PermissionState.Unrestricted));
           }
           else if (Settings.Security == KAVE.BaseEngine.Classes.SecurityState.Trusted)
           {
               pset.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode | SecurityPermissionFlag.Execution | SecurityPermissionFlag.RemotingConfiguration | SecurityPermissionFlag.ControlThread));
               pset.AddPermission(new System.Security.Permissions.IsolatedStorageFilePermission(PermissionState.None));
           }
           else if (Settings.Security == KAVE.BaseEngine.Classes.SecurityState.UnTrusted)
           {
               pset.AddPermission(new SecurityPermission(SecurityPermissionFlag.NoFlags));
               pset.AddPermission(new System.Security.Permissions.IsolatedStorageFilePermission(PermissionState.None));
           }
           else
           {
               pset.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode | SecurityPermissionFlag.Execution));
               pset.AddPermission(new System.Security.Permissions.IsolatedStorageFilePermission(PermissionState.None));
           }

           if (Settings.AccessFiles)
           {
               pset.AddPermission(new System.Security.Permissions.FileIOPermission(PermissionState.Unrestricted));
           }
           else
           {
               pset.AddPermission(new System.Security.Permissions.FileIOPermission(PermissionState.None));
           }
           if (Settings.AccessRegistry)
           {
               pset.AddPermission(new System.Security.Permissions.RegistryPermission(PermissionState.Unrestricted));
           }
           else
           {
               pset.AddPermission(new System.Security.Permissions.RegistryPermission(PermissionState.None));
           }
           if (Settings.AccessEnvironment)
           {
               pset.AddPermission(new System.Security.Permissions.EnvironmentPermission(PermissionState.Unrestricted));
  
           }
           else
           {
               pset.AddPermission(new System.Security.Permissions.EnvironmentPermission(PermissionState.None));
          
           }

           if (Settings.AccessPerformanceCounter)
           {

               pset.AddPermission(new System.Diagnostics.PerformanceCounterPermission(PermissionState.Unrestricted));
           }
           else
           {

               pset.AddPermission(new System.Diagnostics.PerformanceCounterPermission(PermissionState.None));
           }
           if (Settings.AccessFileDialog)
           {
               pset.AddPermission(new System.Security.Permissions.FileDialogPermission(FileDialogPermissionAccess.OpenSave));
           }
           else
           {
               pset.AddPermission(new System.Security.Permissions.FileDialogPermission(FileDialogPermissionAccess.None));
           }
           if (Settings.AccessGUI)
           {
               pset.AddPermission(new System.Security.Permissions.UIPermission(PermissionState.Unrestricted));
           }
           else
           {
               pset.AddPermission(new System.Security.Permissions.UIPermission(PermissionState.None));
           }
           if (Settings.AccessEventLog)
           {
               pset.AddPermission(new System.Diagnostics.EventLogPermission(PermissionState.Unrestricted));
           }
           else
           {
               pset.AddPermission(new System.Diagnostics.EventLogPermission(PermissionState.None));
           }
           pset.AddPermission(new System.Security.Permissions.KeyContainerPermission(PermissionState.Unrestricted));
            
           pset.AddPermission(new System.Security.Permissions.ReflectionPermission(PermissionState.Unrestricted));

           AppDomainSetup ads = new AppDomainSetup();
           ads.ApplicationName = Name;

           ads.ApplicationBase = SettingsManager.SandBoxPath;
           // create the sandboxed domain
           sandbox = AppDomain.CreateDomain(
                 "Sandboxed Domain",
                 AppDomain.CurrentDomain.Evidence,
                 ads, pset);

           sandbox.ExecuteAssembly(assembly);
           MessageBox.Show("SandBoxing Completed Successfully.", "SandBox Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
       }
       public void Stop()
       {
         
       }
    }
}
