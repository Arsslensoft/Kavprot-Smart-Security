using System;
using System.Collections.Generic;
using System.Text;
using KAVE.BaseEngine;
using KCompress;
using System.IO;
using System.Windows.Forms;

namespace KAVE.Engine
{
    public class ArchiveScanner : IScanner
    {
        public string Name
        {
            get { return "ARCHIVE-SCANNER"; }
        }
        public int MaximumSize
        {
            get { return 100000000; }
        }

        public object ScanHS(string filename)
        {
            try
            {
                FileInfo sv = new FileInfo(filename);
                if (sv.Length < MaximumSize)
                {
                    using (KCompressExtractor extr = new KCompressExtractor(filename))
                    {
                        extr.ExtractArchive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\");
                    }
                    foreach (string file in FileHelper.GetFilesRecursive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\"))
                    {
                        try
                        {
                            object slst = FileFormat.GetFileFormat(file).ScanHS(file);

                            if (slst != null)
                                return slst + "&" + file.Replace(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\","");

                            File.Delete(file);
                        }
                        catch
                        {

                        }
                    }
                    return null;
                }
                else
                    return null;

            }
            catch
            {
                return "KavprotSensor/Unpackable.Archive";

            }
            finally
            {

            }
        }
        public object Scan(string filename)
        {
            try
            {
                FileInfo sv = new FileInfo(filename);
                if (sv.Length < MaximumSize)
                {
                    using (KCompressExtractor extr = new KCompressExtractor(filename))
                    {
                        extr.ExtractArchive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\");
                    }
                    foreach (string file in FileHelper.GetFilesRecursive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\"))
                    {
                        try
                        {
                            object slst = FileFormat.GetFileFormat(file).Scan(file);

                                if (slst != null)
                                    return slst + "&" + file.Replace(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\", "");


                            File.Delete(file);
                        }
                        catch
                        {

                        }
                    }
                    return null;
                }
                else
                    return null;

            }
            catch
            {
                return "KavprotSensor/Unpackable.Archive";

            }
            finally
            {

            }
        }
        public object ScanM(string filename)
        {
            try
            {
                FileInfo sv = new FileInfo(filename);
                if (sv.Length < MaximumSize)
                {
                    using (KCompressExtractor extr = new KCompressExtractor(filename))
                    {
                        extr.ExtractArchive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\");
                    }
                    foreach (string file in FileHelper.GetFilesRecursive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\"))
                    {
                        try
                        {
                            object slst = FileFormat.GetFileFormat(file).ScanM(file);

                            if (slst != null)
                                return slst + "&" + file.Replace(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\", "");


                            File.Delete(file);
                        }
                        catch
                        {

                        }
                    }
                    return null;
                }
                else
                    return null;

            }
            catch
            {
                return "KavprotSensor/Unpackable.Archive";

            }
            finally
            {

            }
        }

        public object ScanHS(string filename, System.Windows.Forms.Label lb)
        {
            try
            {
                FileInfo sv = new FileInfo(filename);
                if (sv.Length < MaximumSize)
                {
                    using (KCompressExtractor extr = new KCompressExtractor(filename))
                    {
                        extr.ExtractArchive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\");
                    }
                    foreach (string file in FileHelper.GetFilesRecursive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename)))
                    {
                        try
                        {
                            GUI.UpdateLabel(lb, filename + ":" + file.Replace(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename), ""));
                            object slst = FileFormat.GetFileFormat(file).ScanHS(file);

                            if (slst != null)
                                return slst + "&" + file.Replace(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\", "");


                            File.Delete(file);
                        }
                        catch
                        {

                        }
                    }
                    return null;
                }
                else
                    return null;

            }
            catch
            {
                return "KavprotSensor/Unpackable.Archive";

            }
            finally
            {

            }
        }
        public object Scan(string filename, System.Windows.Forms.Label lb)
        {
            try
            {
                FileInfo sv = new FileInfo(filename);
                if (sv.Length < MaximumSize)
                {
                    using (KCompressExtractor extr = new KCompressExtractor(filename))
                    {
                        extr.ExtractArchive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\");
                    }
                    foreach (string file in FileHelper.GetFilesRecursive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\"))
                    {
                        try
                        {
                            GUI.UpdateLabel(lb, filename + ":" + file.Replace(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename), ""));

                            object slst = FileFormat.GetFileFormat(file).Scan(file);

                            if (slst != null)
                                return slst + "&" + file.Replace(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\", "");


                            File.Delete(file);
                        }
                        catch
                        {

                        }
                    }
                    return null;
                }
                else
                    return null;

            }
            catch
            {
                return "KavprotSensor/Unpackable.Archive";

            }
            finally
            {

            }
        }
        public object ScanM(string filename, System.Windows.Forms.Label lb)
        {
            try
            {
                FileInfo sv = new FileInfo(filename);
                if (sv.Length < MaximumSize)
                {
                    using (KCompressExtractor extr = new KCompressExtractor(filename))
                    {
                        extr.ExtractArchive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\");
                    }
                    foreach (string file in FileHelper.GetFilesRecursive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\"))
                    {
                        try
                        {
                            GUI.UpdateLabel(lb, filename + ":" + file.Replace(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename), ""));

                            object slst = FileFormat.GetFileFormat(file).ScanM(file);

                            if (slst != null)
                                return slst + "&" + file.Replace(AVEngine.TempDir + Path.GetFileNameWithoutExtension(filename) + @"\", "");


                            File.Delete(file);
                        }
                        catch
                        {

                        }
                    }
                    return null;
                }
                else
                    return null;

            }
            catch
            {
                return "KavprotSensor/Unpackable.Archive";

            }
            finally
            {

            }
        }

        public bool Repair(Virus virus)
        {
            try
            {

                KCompress.KCompressExtractor extr = new KCompress.KCompressExtractor(virus.Location);
                extr.ExtractArchive(AVEngine.TempDir + @"QA\A\");

               virus.Scanner.Repair(new Virus(virus.Name,AVEngine.TempDir + @"QA\A\"+virus.FileName,FileFormat.GetFileFormat(virus.FileName)));

                File.Copy(virus.Location, Application.StartupPath + @"\Quarantine\ARCHIVEBACKUP\" + Path.GetFileName(virus.Location) + ".BACKUP");
                KCompress.KCompressCompressor comp = new KCompress.KCompressCompressor();
                comp.IncludeEmptyDirectories = true;
                comp.FastCompression = true;
                comp.CompressionLevel = KCompress.CompressionLevel.High;
                comp.CompressionMethod = KCompress.CompressionMethod.Default;
                comp.CompressionMode = KCompress.CompressionMode.Create;
                comp.CompressDirectory(AVEngine.TempDir + @"QA\A\", virus.Location);
                Directory.Delete(AVEngine.TempDir + @"QA\A\", true);
                return true;


            }
            catch
            {

            }
            finally
            {

            }
            return false;
        }

    }
}
