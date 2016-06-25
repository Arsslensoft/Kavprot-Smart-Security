using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using KAVE.BaseEngine;

namespace Kavprot
{
    public partial class CryptoCenter : UserControl
    {
        public CryptoCenter()
        {
            InitializeComponent();
            gen = new PasswordGenerator();
        }
        PasswordGenerator gen;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            try
            {
                gen.Exclusions = textBoxX1.Text;
                gen.ExcludeSymbols = checkBoxX1.Checked;
                gen.RepeatCharacters = checkBoxX2.Checked;
                gen.ConsecutiveCharacters = true;
                gen.Maximum = Int32.Parse(textBoxX2.Text);
                gen.Minimum = Int32.Parse(textBoxX3.Text);
                Clipboard.SetText(gen.Generate(), TextDataFormat.Text);
                MessageBox.Show("Password Generated and copied to clipboard", "Password Generator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                  AntiCrash.LogException(ex);
            }
            finally
            {

            }

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxX6.Text = Encrypt(textBoxX4.Text, textBoxX5.Text);
            }
            catch (Exception ex)
            {
                  AntiCrash.LogException(ex);
            }
            finally
            {

            }
        }
        public string Encrypt(string PlainText, string Password)
        {
            string Salt = "Adtpec";
            string HashAlgorithm = "SHA1";
            int PasswordIterations = 2;
            string InitialVector = "OFRna73m*aze01xY";
            int KeySize = 256;
            if (string.IsNullOrEmpty(PlainText))
                return "";
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] PlainTextBytes = Encoding.UTF8.GetBytes(PlainText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            byte[] CipherTextBytes = null;
            using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream())
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
                    {
                        CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
                        CryptoStream.FlushFinalBlock();
                        CipherTextBytes = MemStream.ToArray();
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }
            SymmetricKey.Clear();
            return Convert.ToBase64String(CipherTextBytes);
        }
        public string Decrypt(string CipherText, string Password)
        {
            string Salt = "Adtpec";
            string HashAlgorithm = "SHA1";
            int PasswordIterations = 2;
            string InitialVector = "OFRna73m*aze01xY";
            int KeySize = 256;

            if (string.IsNullOrEmpty(CipherText))
                return "";
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] CipherTextBytes = Convert.FromBase64String(CipherText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            byte[] PlainTextBytes = new byte[CipherTextBytes.Length];
            int ByteCount = 0;
            using (ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes))
            {
                using (MemoryStream MemStream = new MemoryStream(CipherTextBytes))
                {
                    using (CryptoStream CryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read))
                    {

                        ByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
                        MemStream.Close();
                        CryptoStream.Close();
                    }
                }
            }
            SymmetricKey.Clear();
            return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount);
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxX4.Text = Decrypt(textBoxX6.Text, textBoxX5.Text);
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
