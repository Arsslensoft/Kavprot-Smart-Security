using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using KAVE.Engine;
using KAVE.BaseEngine;

namespace KAVE
{
    public class Quarantine
    {
        static string tempf = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + Security.GetMd5Hashofstring("AVDefender") + @"\";

        static string key = "xqf4r9an";
        public static void StoreR(string filename, string infection)
        {
            try
            {
                AVEngine.EventsManager.CallQuarantined();
                EncryptFile(filename, Application.StartupPath + @"\Quarantine\" + Path.GetFileName(filename) + ".KPQ", key);
                File.WriteAllText(Application.StartupPath + @"\Quarantine\" + Path.GetFileName(filename) + ".KPQI", filename + "\r\n" + infection);

            }
            catch (Exception ex)
            {
               AntiCrash.LogException(ex,4);
            }
            finally
            {

            }
        }
        public static void Store(string filename, string infection)
        {
            try
            {
                if (filename.StartsWith(Environment.SystemDirectory))
                {
                    var result = MessageBox.Show("Kavprot smart security will quarantine a file system. \r\n Do you want to remove this file completely and place it in Quarantine? + \n"+filename, "Kavprot Quarantine", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        AVEngine.EventsManager.CallQuarantined();
                        File.WriteAllText(Application.StartupPath + @"\Quarantine\" + Path.GetFileName(filename) + ".KPQI", filename + "\r\n" + infection);
                        EncryptFile(filename, Application.StartupPath + @"\Quarantine\" + Path.GetFileName(filename) + ".KPQ", key);
                        File.Delete(filename);
                    }
                }
                else
                {
                    AVEngine.EventsManager.CallQuarantined();
                    File.WriteAllText(Application.StartupPath + @"\Quarantine\" + Path.GetFileName(filename) + ".KPQI", filename + "\r\n" + infection);
                    EncryptFile(filename, Application.StartupPath + @"\Quarantine\" + Path.GetFileName(filename) + ".KPQ", key);
                    File.Delete(filename);
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
        public static void Activate(string Qfile)
        {
            try
            {

                DecryptFile(Application.StartupPath + @"\Quarantine\" + Qfile, File.ReadAllLines(Application.StartupPath + @"\Quarantine\" + Qfile + "I")[0], key);
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
        public static List<string> Filelist()
        {
            List<string> list = new List<string>();
      
            foreach (string file in Directory.GetFiles(Application.StartupPath + @"\Quarantine", "*.KPQ", SearchOption.AllDirectories))
            {
                if (file.EndsWith(".KPQ"))
                   list.Add(Path.GetFileName(file));
               
            }
            return list;
        }
        static void EncryptFile(string _FileToEncrypt, string _cryptoFile, string _Password)
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

        static void DecryptFile(string _FileToDencrypt, string _cryptoFile, string _Password)
        {
            try
            {
                // Step 1. Create the Stream objects
                FileStream outFile = new FileStream(_cryptoFile, FileMode.OpenOrCreate, FileAccess.Write);

                FileStream inFile = new FileStream(_FileToDencrypt, FileMode.Open, FileAccess.Read);

                // Step 2. Create the Symetrical algo object
                SymmetricAlgorithm symAlgo = new RijndaelManaged();

                // Step 3. Specify a key (optional)
                byte[] salt = Encoding.ASCII.GetBytes("QUARANTINESALT");
                Rfc2898DeriveBytes theKey = new Rfc2898DeriveBytes(_Password, salt);
                symAlgo.Key = theKey.GetBytes(symAlgo.KeySize / 8);
                symAlgo.IV = theKey.GetBytes(symAlgo.BlockSize / 8);

                // Step 4. Create the ICryptoTransfor object
                ICryptoTransform decryptor = symAlgo.CreateDecryptor();

                // Step 5. Create the Crypto Stream object
                CryptoStream decryptStream = new CryptoStream(inFile, decryptor, CryptoStreamMode.Read);

                // Step 6. Write the contents to the CryptoStream
                // Read the encrypted file file into fileData
                byte[] fileData = new byte[inFile.Length];
                decryptStream.Read(fileData, 0, (int)inFile.Length);

                // Save unecrypted data
                outFile.Write(fileData, 0, fileData.Length);

                // Close the file handles
                decryptStream.Close();
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
