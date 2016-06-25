using System;
using System.Collections.Generic;
using System.Text;
using KAVE.BaseEngine;
using KCompress;
using System.IO;

namespace KAVE.Engine
{
    public class PEScanner : IScanner
    {
        public int MaximumSize
        {
            get { return 5000000; }
        }
        public string Name
        {
            get { return "PE-SCANNER"; }
        }
        public object ScanHS(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            try
            {
                if (fi.Length < MaximumSize)
                {
                    using (KCompress.KCompressExtractor extr = new KCompressExtractor(filename))
                    {
                        extr.ExtractArchive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\");
                    }
                    
                    object svir = null;
                    foreach (string file in FileHelper.GetFilesRecursive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\"))
                    {
                        if (file.EndsWith(".text"))
                        {
                            svir = VDB.GetPEMD5(Security.GetMD5HashFromFile(file));
                            if (svir != null)
                                return svir;

                        }
                        else if (file.EndsWith(".data"))
                        {
                            svir = VDB.GetPEMD5(Security.GetMD5HashFromFile(file));
                            if (svir != null)
                                return svir;

                        }
                        else if (file.EndsWith(".idata"))
                        {
                            svir = VDB.GetPEMD5(Security.GetMD5HashFromFile(file));
                            if (svir != null)
                                return svir;
                        }
                        File.Delete(file);
                    }
                    svir = VDB.GetMD5(Security.GetMD5HashFromFile(filename));
                    if (svir != null)
                        return svir;
   
                }
                else
                    return null;
            }
            catch
            {
                return null;

            }
            finally
            {
              
            }
            return null;
        }
        public object ScanM(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            try
            {
                if (fi.Length < MaximumSize)
                {
                    using (KCompress.KCompressExtractor extr = new KCompressExtractor(filename))
                    {
                        extr.ExtractArchive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\");
                    }
                    object svir = null;
                    foreach (string file in FileHelper.GetFilesRecursive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\"))
                    {
                        if (file.EndsWith(".text"))
                        {
                            svir = VDB.GetPEMD5(Security.GetMD5HashFromFile(file));
                            if (svir != null)
                                return svir;

                        }
                        else if (file.EndsWith(".data"))
                        {
                            svir = VDB.GetPEMD5(Security.GetMD5HashFromFile(file));
                            if (svir != null)
                                return svir;

                        }
                        else if (file.EndsWith(".idata"))
                        {
                            svir = VDB.GetPEMD5(Security.GetMD5HashFromFile(file));
                            if (svir != null)
                                return svir;
                        }
                        File.Delete(file);
                    }
                    return VDB.GetMD5(Security.GetMD5HashFromFile(filename));
                }
                else
                   return null;



            }
            catch
            {
                return null;

            }
            finally
            {

            }
            return null;
        }
        public object Scan(string filename)
        {
            return VDB.GetMD5(Security.GetMD5HashFromFile(filename));

        }


        public object ScanHS(string filename, System.Windows.Forms.Label lb)
        {
            FileInfo fi = new FileInfo(filename);
            try
            {
                if (fi.Length < MaximumSize)
                {
                    using (KCompress.KCompressExtractor extr = new KCompressExtractor(filename))
                    {
                        extr.ExtractArchive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\");
                    }
                    object svir = null;
                    foreach (string file in FileHelper.GetFilesRecursive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\"))
                    {

                        if (file.EndsWith(".text"))
                        {
                            svir = VDB.GetPEMD5(Security.GetMD5HashFromFile(file));
                            if (svir != null)
                                return svir;

                        }
                        else if (file.EndsWith(".data"))
                        {
                            svir = VDB.GetPEMD5(Security.GetMD5HashFromFile(file));
                            if (svir != null)
                                return svir;

                        }
                        else if (file.EndsWith(".idata"))
                        {
                            svir = VDB.GetPEMD5(Security.GetMD5HashFromFile(file));
                            if (svir != null)
                                return svir;
                        }
                        File.Delete(file);
                    }
                    svir = VDB.GetMD5(Security.GetMD5HashFromFile(filename));
                    if (svir != null)
                        return svir;
                                
                }
                else
                        return null;

            }
            catch
            {
                return null;

            }
            finally
            {

            }
            return null;
        }
        public object ScanM(string filename, System.Windows.Forms.Label lb)
        {
            FileInfo fi = new FileInfo(filename);
            try
            {
                if (fi.Length < MaximumSize)
                {

                    using (KCompress.KCompressExtractor extr = new KCompressExtractor(filename))
                    {
                        extr.ExtractArchive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\");
                    }
                    object svir = null;
                    foreach (string file in FileHelper.GetFilesRecursive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\"))
                    {
                        if (file.EndsWith(".text"))
                        {
                            svir = VDB.GetPEMD5(Security.GetMD5HashFromFile(file));
                            if (svir != null)
                                return svir;

                        }
                        else if (file.EndsWith(".data"))
                        {
                            svir = VDB.GetPEMD5(Security.GetMD5HashFromFile(file));
                            if (svir != null)
                                return svir;

                        }
                        else if (file.EndsWith(".idata"))
                        {
                            svir = VDB.GetPEMD5(Security.GetMD5HashFromFile(file));
                            if (svir != null)
                                return svir;
                        }
                        File.Delete(file);
                    }
                    return VDB.GetMD5(Security.GetMD5HashFromFile(filename));
                }
                else
                   return null;
               
            }
            catch
            {
                return null;

            }
            finally
            {

            }
            return null;
        }
        public object Scan(string filename, System.Windows.Forms.Label lb)
        {
            return VDB.GetMD5(Security.GetMD5HashFromFile(filename));

        }
        public bool Repair(Virus virus)
        {
            return false;
        }
 
    }
}
