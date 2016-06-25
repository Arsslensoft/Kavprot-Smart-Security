using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using KAVE.Engine;
using System.Security.Cryptography;

namespace KAVE.BaseEngine
{
    public struct Virus
    {
        public string Name;
        public string Location;
        public string FileName;
        public IScanner Scanner;
        bool Q;
        bool R;
        public Virus(string name, string location, string filename, IScanner scanner)
        {
            Name = name;
            Location = location;
            FileName = filename;
            Scanner = scanner;
            Q = false;
                 R = false;
        }
        public Virus(string name, string filename, IScanner scanner)
        {
            Name = name;
            Location = filename;
            FileName = filename;
            Scanner = scanner;
            Q = false;
           R = false;
        }

        public void Quarantine()
        {
            try
            {
                if (Scanner == AVEngine.ArchiveTypeScanner)
                {
                   
                    KCompress.KCompressExtractor extr = new KCompress.KCompressExtractor(Location);
                    extr.ExtractArchive(AVEngine.TempDir + @"QA\A\");

                    EncryptFile(AVEngine.TempDir + @"QA\A\" + FileName, Application.StartupPath + @"\Quarantine\" + Path.GetFileName(FileName) + ".KPQ", "ac1s8y9s");
                    File.WriteAllText(Application.StartupPath + @"\Quarantine\" + Path.GetFileName(FileName) + ".KPQI", FileName + "\r\n" + Name);
                    Q = true;
                    File.Delete(AVEngine.TempDir + @"QA\A\" + FileName);
                    File.Copy(Location, Application.StartupPath + @"\Quarantine\ARCHIVEBACKUP\" + Path.GetFileName(Location) + ".BACKUP");
                    KCompress.KCompressCompressor comp = new KCompress.KCompressCompressor();
                    comp.IncludeEmptyDirectories = true;
                    comp.FastCompression = true;
                    comp.CompressionLevel = KCompress.CompressionLevel.High;
                    comp.CompressionMethod = KCompress.CompressionMethod.Default;
                    comp.CompressionMode = KCompress.CompressionMode.Create;
                    comp.CompressDirectory(AVEngine.TempDir + @"QA\A\", Location);
                    Directory.Delete(AVEngine.TempDir + @"QA\A\",true);

                }
                else
                {
                    EncryptFile(Location, Application.StartupPath + @"\Quarantine\" + Path.GetFileName(Location) + ".KPQ", "ac1s8y9s");
                    File.WriteAllText(Application.StartupPath + @"\Quarantine\" + Path.GetFileName(Location) + ".KPQI", Location + "\r\n" + Name);
                    Q = true;
                    File.Delete(Location);
                }
            }
            catch (Exception ex)
            {
                if (Scanner == AVEngine.ArchiveTypeScanner && ex is KCompress.KCompressException) 
                {
                    if (File.Exists(AVEngine.TempDir + @"QAB\" + Path.GetFileName(Location)))
                        File.Copy(AVEngine.TempDir + @"QAB\" + Path.GetFileName(Location), Location);
                }
            }
            finally
            {

            }
        }
        public void Repair()
        {
            Scanner.Repair(this);
        }
        void EncryptFile(string _FileToEncrypt, string _cryptoFile, string _Password)
        {
            try
            {
                // Step 1. Create the Stream objects
                FileStream inFile = new FileStream(_FileToEncrypt, FileMode.Open, FileAccess.Read);
                FileStream outFile = new FileStream(_cryptoFile, FileMode.OpenOrCreate, FileAccess.Write);

                // Step 2. Create the Symetrical algo object
                SymmetricAlgorithm symAlgo = new RijndaelManaged();

                // Step 3. Specify a key (optional)
                byte[] salt = Encoding.ASCII.GetBytes("QUARANTINESALT");
                Rfc2898DeriveBytes theKey = new Rfc2898DeriveBytes(_Password, salt);
                symAlgo.Key = theKey.GetBytes(symAlgo.KeySize / 8);
                symAlgo.IV = theKey.GetBytes(symAlgo.BlockSize / 8);

                // Read the unencrypted file file into fileData
                byte[] fileData = new byte[inFile.Length];
                inFile.Read(fileData, 0, (int)inFile.Length);

                // Step 4. Create the ICryptoTransfor object
                ICryptoTransform encryptor = symAlgo.CreateEncryptor();

                // Step 5. Create teh Crypto Stream object
                CryptoStream encryptStream = new CryptoStream(outFile, encryptor, CryptoStreamMode.Write);

                // Step 6. Write the contents to the CryptoStream
                encryptStream.Write(fileData, 0, fileData.Length);

                // Close file handles
                encryptStream.Close();
                inFile.Close();
                outFile.Close();
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
