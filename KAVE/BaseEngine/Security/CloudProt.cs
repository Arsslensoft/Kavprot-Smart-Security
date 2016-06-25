using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using KAVE.BaseEngine;
using KAVE.Engine;

namespace KAVE
{
   public static class CloudProt
    {
        static string CheckVRPS(string filename)
        {
            string result = "safe";
            try
            {

                if (SettingsManager.VRPS)
                {
                    if (FileFormat.GetVRPS(Path.GetExtension(filename)) != "false")
                    {
                        // only VT
                        if (FileFormat.GetVRPS(Path.GetExtension(filename)) == "1")
                        {
                            string vn = null;
                            bool ssresult = VT.Check(Security.GetMD5HashFromFile(filename), out vn);
                            if (ssresult)
                            {
                               Alert.InfectedByMany(vn, filename);
                                result = vn;
                            }
                           
                        }
                        else if (FileFormat.GetVRPS(Path.GetExtension(filename)) == "2")
                        {
                            string vn = null;
                            bool ssresult = VT.Check(Security.GetMD5HashFromFile(filename), out vn);
                   if (ssresult)
                            {
                           
                                Alert.InfectedByMany(vn, filename);
                                result = vn;
                            }
                            else
                            {
                                string infec = null;
                                bool sysresult = ThreadExpert.Check(Security.GetMD5HashFromFile(filename), out infec);
                                if (sysresult)
                                {
                                    if (Regex.Match(infec, @"[A-Z]", RegexOptions.IgnoreCase).Success)
                                    {
                                        result = infec;
                                        Alert.InfectedByMany(infec, filename);
                                    }
                             
                                }
                              
                            }
                        }
                      

                    }
                }

            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
            finally
            {

            }
            return result;
        }
        public static void Protect()
        {
            List<string> Files = FileHelper.GetCloudDriveFiles();
            foreach (string file in Files)
            {
                try
                {
                    Thread.Sleep(5000);
                    string result = VDB.CheckCloud(Security.GetMD5HashFromFile(file));
                    if (!string.IsNullOrEmpty(result))
                    {
                        Alert.InfectedByMany(result, file);
                    }
                    else
                    {
                        string sr = CheckVRPS(file);
                        if (sr == "safe")
                        {
                            VDB.InsertCloud(Security.GetMD5HashFromFile(file), file, "Safe");
                        }
                        else
                        {
                            VDB.InsertCloud(Security.GetMD5HashFromFile(file), file, sr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AntiCrash.LogException(ex);
                }
                finally
                {

                }
            }
        }
    }
}
