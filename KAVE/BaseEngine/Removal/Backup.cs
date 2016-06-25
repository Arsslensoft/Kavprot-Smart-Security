using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using KAVE.Engine;
using KAVE.BaseEngine;

namespace KAVE
{
  public static  class Backup
    {
    public static void Make(string _FileToEncrypt, string _cryptoFile, string _Password)
        {
            try
            {

                FileStream inFile = new FileStream(_FileToEncrypt, FileMode.Open, FileAccess.Read);
                FileStream outFile = new FileStream(_cryptoFile, FileMode.OpenOrCreate, FileAccess.Write);

                // Step 2. Create the Symetrical algo object
                SymmetricAlgorithm symAlgo = new RijndaelManaged();

                // Step 3. Specify a key (optional)
                byte[] salt = Encoding.ASCII.GetBytes("BACKUPSALT");
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
                AVEngine.EventsManager.CallFileStored();
            }
            catch (Exception ex)
            {
                AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
    public static void Restore(string _FileToDencrypt, string _cryptoFile, string _Password)
           {
               try
               {
                   // Step 1. Create the Stream objects
                   FileStream outFile = new FileStream(_FileToDencrypt, FileMode.OpenOrCreate, FileAccess.Write);
                   FileStream inFile = new FileStream(_cryptoFile, FileMode.Open, FileAccess.Read);

                   // Step 2. Create the Symetrical algo object
                   SymmetricAlgorithm symAlgo = new RijndaelManaged();

                   // Step 3. Specify a key (optional)
                   byte[] salt = Encoding.ASCII.GetBytes("BACKUPSALT");
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
