using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace KAVE.BaseEngine
{
   public static class Activation
    {
       public static string User;
       internal static string Password;
       public static string SK;
       public static DateTime Expiration;
       public static bool Expired = false;
       internal static string Encrypt(string originalString)
       {

           if (String.IsNullOrEmpty(originalString))
           {
               throw new ArgumentNullException
                      ("The string which needs to be encrypted can not be null.");
           }
           DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
           MemoryStream memoryStream = new MemoryStream();
           CryptoStream cryptoStream = new CryptoStream(memoryStream,
               cryptoProvider.CreateEncryptor(ASCIIEncoding.ASCII.GetBytes("actvx2d5"), ASCIIEncoding.ASCII.GetBytes("actvx2d5")), CryptoStreamMode.Write);
           StreamWriter writer = new StreamWriter(cryptoStream);
           writer.Write(originalString);
           writer.Flush();
           cryptoStream.FlushFinalBlock();
           writer.Flush();
           return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);

       }
       internal static string Decrypt(string cryptedString)
       {

           if (String.IsNullOrEmpty(cryptedString))
           {
               throw new ArgumentNullException
                  ("The string which needs to be decrypted can not be null.");
           }
           DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
           MemoryStream memoryStream = new MemoryStream
                   (Convert.FromBase64String(cryptedString));
           CryptoStream cryptoStream = new CryptoStream(memoryStream,
               cryptoProvider.CreateDecryptor(ASCIIEncoding.ASCII.GetBytes("actvx2d5"), ASCIIEncoding.ASCII.GetBytes("actvx2d5")), CryptoStreamMode.Read);
           StreamReader reader = new StreamReader(cryptoStream);
           return reader.ReadToEnd();


       }



       internal static void Initialize()
       {
           try
           {
               FileSecurity fs = new FileSecurity();
               if (File.Exists(actfile))
               {
                   string[] lines = fs.ACTReadAllLines(actfile);
                   User = lines[0];
                   Password = lines[1];
                   SK = lines[2];
                   Expiration = DateTime.Parse(lines[3]);
                   string Machine = lines[4];
                  
                   
                   if (Expiration.ToFileTime() > DateTime.Now.ToFileTime() && Environment.MachineName.ToUpper()  == Machine.ToUpper())
                   {
                       Expired = false;
                   }
                   else
                   {
                       var result = MessageBox.Show("Kavprot is Expired. \r\nDo you want to activate it ?", "Activation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                       if (result == DialogResult.Yes)
                       {
                           Expired = true;
                           Process.Start("http://www.arsslensoft.tk/avl/RegisterKavprot.html");
                           ActivationForm actf = new ActivationForm();
                           actf.ShowDialog();
                           Application.Restart();
                       }
                       else
                       {
                           Expired = true;
                       }

                   }
               }
               else
               {
                   var result = MessageBox.Show("Kavprot is Expired. \r\nDo you want to activate it ?", "Activation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                   if (result == DialogResult.Yes)
                   {
                       Expired = true;
                       ActivationForm actf = new ActivationForm();
                       actf.ShowDialog();
                    
                   }
                   else
                   {
                       Expired = true;
                   }
               }
           }
           catch (Exception ex)
           {
           
               MessageBox.Show(ex.Message, "Activation", MessageBoxButtons.OK, MessageBoxIcon.Hand);
               Expired = true;
           }
           finally
           {

           }
       }
       static string actfile = Application.StartupPath + @"\Lic.avlc";
       internal static bool PostProductInformation(string content)
       {
           string url = "http://www.arsslensoft.tk/avl/Kavprot/register.php";

           WebRequest req = WebRequest.Create(url);
           req.ContentType = "application/x-www-form-urlencoded";
           req.Method = "POST";
           byte[] postdata = Encoding.ASCII.GetBytes("content=" + content + "&profile=profiles/" + Activation.User + "@" + Environment.MachineName + ".kprof&timestamp=CONTENT RECEIVED FROM " + Activation.User + "@" + Environment.MachineName + " AT " + DateTime.Now.ToString());
                Stream dataStream = req.GetRequestStream();
           // Write the data to the request stream.
                dataStream.Write(postdata, 0, postdata.Length);
           // Close the Stream object.
           dataStream.Close();

           
           StreamReader sr = new StreamReader(req.GetResponse().GetResponseStream());
           if (sr.ReadToEnd() == "OK")
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       private static readonly Random _rng = new Random();

       private static string RandomString(int size)
       {
           char[] buffer = new char[size];

           for (int i = 0; i < size; i++)
           {
               //buffer[i] = _chars[_rng.Next(_chars.Length)];
               buffer[i] = (char)_rng.Next(32, 218);
           }
           return new string(buffer);
       }
       
       public static bool GenerateActivation(string Username, string pass, string SKey, string email)
       {
           try
           {
               string Machine = Environment.MachineName;
               string con = string.Format("user={0}&pass={1}&SK={2}&Machine={3}&email={4}&MTD=VALIDATE", Username, pass, SKey, Machine.ToLower(), email);
             byte[] b =  Encoding.ASCII.GetBytes(con);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://www.arsslensoft.tk/avl/Kavprot/kpavdbv.php");
             req.Method = "POST";
             req.Accept = "gzip";
             req.AutomaticDecompression = DecompressionMethods.GZip;
             req.ContentType = "application/x-www-form-urlencoded";
             req.Timeout = 20000;
             Stream stream = req.GetRequestStream();
             stream.Write(b, 0, b.Length);
             stream.Close();
             stream = req.GetResponse().GetResponseStream();
              
             StreamReader sr = new StreamReader(stream);
             string text = sr.ReadToEnd();
             stream.Close();
             if (text == "FALSE")
             {
                 return false;
             }
             else
             {
                 DateTime dt = DateTime.Parse("24/11/1995");
                bool parse = DateTime.TryParse(text, out dt);
                if (parse)
                {
                    FileSecurity fs = new FileSecurity();
                    string[] lines = { Username, pass, SKey, dt.ToString(), Machine };
                    fs.ACTWriteAllLines(lines, actfile);
                    return true;
                }
         
             }

             return false;
              
           }
           catch (Exception ex)
           {
               AntiCrash.LogException(ex,0);
               MessageBox.Show(ex.Message, "Activation", MessageBoxButtons.OK, MessageBoxIcon.Hand);
               Expired = true;
           }
           finally
           {

           }
           return false;
       }

       public static bool Login(string user, string pass)
       {
           if (user == Activation.User && pass == Activation.Password)
           {
              
                  return true;
           }
           else
           {
               MessageBox.Show("Login Failed");
               return false;
           
           }
       }
    }
}
