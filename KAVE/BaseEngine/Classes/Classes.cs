using System;
using System.Collections.Generic;
using System.Text;
using KAVE.Engine;
using System.IO;

namespace KAVE.BaseEngine.Classes
{    /// <summary>
    /// Virus Database Teacher
    /// </summary>
    public static class VirusDBTeacher
    {
        public static VDBT GetSignatures(string file, bool GenerateVN, string vir)
        {
            if (FileFormat.GetFileFormat(file).Name == "PE-TYPE-SCANNER")
            {
                string hash = Security.GetMD5HashFromFile(file);
                string th = null;
                string dh = null;
                using (KCompress.KCompressExtractor extr = new KCompress.KCompressExtractor(file))
                    extr.ExtractArchive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(file) + @"\");

                List<string> lst = FileHelper.GetFilesRecursive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(file) + @"\");
                foreach (string sfile in lst)
                {
                    if (sfile.EndsWith(".text"))
                    {
                        th = Security.GetMD5HashFromFile(sfile);
                    }
                    else if (sfile.EndsWith(".data"))
                    {
                        dh = Security.GetMD5HashFromFile(sfile);
                    }
                }

                return new VDBT(null, "Kavprot/VDBT." + vir, th, dh, hash, "PES");
            }
            else if (FileFormat.GetFileFormat(file).Name == "ARCHIVE-TYPE-SCANNER")
            {
                return new VDBT(null, "Kavprot/VDBT." + vir, null, null, Security.GetMD5HashFromFile(file), "ARS");
            }
            else if (FileFormat.GetFileFormat(file).Name == "HASH-TYPE-SCANNER")
            {
                return new VDBT(null, "Kavprot/VDBT." + vir, null, null, Security.GetMD5HashFromFile(file), "HAS");
            }
            else if (FileFormat.GetFileFormat(file).Name == "ASCII-TYPE-SCANNER")
            {
                string hex = null;
                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(file))
                {
                    hex = Security.DumpHex(sr, sb);

                }

                return new VDBT(hex, "Kavprot/VDBT." + vir, null, null, Security.GetMD5HashFromFile(file), "ASC");

            }
            else
            {
                return new VDBT(null, null, null, null, null, "NOT");
            }
        }
        public static VDBT GetSignatures(string file, bool GenerateVN)
        {
            if (FileFormat.GetFileFormat(file).Name == "PE-TYPE-SCANNER")
            {
                string hash = Security.GetMD5HashFromFile(file);
                string th = null;
                string dh = null;
                using (KCompress.KCompressExtractor extr = new KCompress.KCompressExtractor(file))
                    extr.ExtractArchive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(file) + @"\");


                List<string> lst = FileHelper.GetFilesRecursive(AVEngine.TempDir + Path.GetFileNameWithoutExtension(file) + @"\");
                foreach (string sfile in lst)
                {
                    if (sfile.EndsWith(".text"))
                    {
                        th = Security.GetMD5HashFromFile(sfile);
                    }
                    else if (sfile.EndsWith(".data"))
                    {
                        dh = Security.GetMD5HashFromFile(sfile);
                    }
                    else if (sfile.EndsWith(".idata"))
                    {

                    }
                    else
                    {

                    }
                }

                return new VDBT(null, "Kavprot.VDBT.Malware/Unknown", th, dh, hash, "PES");
            }
            else if (FileFormat.GetFileFormat(file).Name == "ARCHIVE-TYPE-SCANNER")
            {
                return new VDBT(null, "Kavprot.VDBT.Malware/Unknown", null, null, Security.GetMD5HashFromFile(file), "ARS");
            }
            else if (FileFormat.GetFileFormat(file).Name == "HASH-TYPE-SCANNER")
            {
                return new VDBT(null, "Kavprot.VDBT.Malware/Unknown", null, null, Security.GetMD5HashFromFile(file), "HAS");
            }
            else if (FileFormat.GetFileFormat(file).Name == "ASCII-TYPE-SCANNER")
            {
                string hex = null;
                StringBuilder sb = new StringBuilder();
                using (StreamReader sr = new StreamReader(file))
                {
                    hex = Security.DumpHex(sr, sb);

                }

                return new VDBT(hex, "Kavprot.VDBT.MaliciousCode/Unknown", null, null, Security.GetMD5HashFromFile(file), "ASC");

            }
            else
            {
                return new VDBT(null, null, null, null, null, "NOS");
            }
        }
    }

    public struct VDBT
    {
        public string VirusName;
        public string TEXTHASH;
        public string DATAHASH;
        public string FILEHASH;
        public string FILESOURCE;
        public string SIGID;
        public VDBT(string src, string vn, string th, string dh, string fh, string sigid)
        {
            SIGID = sigid;
            VirusName = vn;
            TEXTHASH = th;
            DATAHASH = dh;
            FILEHASH = fh;
            FILESOURCE = src;
        }
    }
}
