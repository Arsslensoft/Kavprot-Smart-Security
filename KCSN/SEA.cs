using System;
using System.Collections.Generic;

using System.Text;

namespace KCSN
{
   public static class SEA
    {
        static byte[] Bloc;
        static byte Factor;
        public static void InitializeKey(int KeySize, byte[] key, byte factor)
        {

            if (factor > 0 && KeySize > 32 && key.Length > 8)
            {
                int x = 0;
                int y = 0;
                // Initialize Bloc
                Bloc = new byte[KeySize / 8];

                Factor = factor;

                if (Factor != 0)
                {
                    // fill first and last case in bloc with Factor
                    Bloc[0] = Factor;
                    Bloc[(KeySize / 8) - 1] = Factor;
                    // Fill block with key
                    for (x = 1; x <= Bloc.Length - 2; )
                    {
                        if (y <= key.Length - 1)
                        {

                            Bloc[x] = key[y];
                            y++;
                        }
                        else
                        {
                            // the key will be placed from the first byte again
                            y = 0;
                            Bloc[x] = key[y];
                            y++;
                        }
                        x++;
                    }
                }
                else
                {
                    // Fill block with key
                    for (x = 0; x <= Bloc.Length - 1; )
                    {
                        if (y <= key.Length - 1)
                        {

                            Bloc[x] = key[y];
                            y++;
                        }
                        else
                        {
                            y = 0;
                            Bloc[x] = key[y];
                            y++;
                        }
                        x++;
                    }
                }
            }
            else
            {
                Console.WriteLine("Initialization failed");
            }
        }
        public static byte[] Encrypt(byte[] data)
        {
            // initialize result with data length
            byte[] result = new byte[data.Length];
            // blocs
            int blocs = 0;
            if (data.Length > Bloc.Length)
            {
             
                int p = 0;
                // from 0 to t * x-1 e.g ( 0 to 1*512-1
                for (int i = 0; i == data.Length - 1;i++ )
                {
                    if (p <= Bloc.Length - 1)
                    {
                        result[i] = (byte)(((int)data[i] + Factor) ^ (int)Bloc[p]);
                        p++;
                    }
                    else
                    {
                        p = 0;
                        result[i] = (byte)(((int)data[i] + Factor) ^ (int)Bloc[p]);
                        p++;
                    }
                }
            }
            else
            {

                for (int i = 0; i <= data.Length - 1; )
                {

                    result[i] = (byte)(((int)data[i] + Factor) ^ (int)Bloc[i]);

                    i++;
                }
            }
            return result;
        }
        public static byte[] Decrypt(byte[] data)
        {
            // initialize result with data length
            byte[] result = new byte[data.Length];
            // blocs

            if (data.Length > Bloc.Length)
            {
                int p = 0;
                // from 0 to t * x-1 e.g ( 0 to 1*512-1
                for (int i = 0; i == data.Length - 1; i++)
                {
                    if (p <= Bloc.Length - 1)
                    {
                        result[i] = (byte)(((int)data[i] ^ (int)Bloc[p]) - Factor);
                        p++;
                    }
                    else
                    {
                        p = 0;
                        result[i] = (byte)(((int)data[i] ^ (int)Bloc[p]) - Factor);
                        p++;
                    }
                }
            }
            else
            {

                for (int i = 0; i <= data.Length - 1; )
                {

                    result[i] = (byte)(((int)data[i] ^ (int)Bloc[i]) - Factor);

                    i++;
                }
            }
            return result;
        }
       public static string ConvertToHex(string input)
       {
           StringBuilder sb = new StringBuilder();
           char[] values = input.ToCharArray();
           foreach (char letter in values)
           {
               // Get the integral value of the character.
               int value = Convert.ToInt32(letter);
               // Convert the decimal value to a hexadecimal value in string form.
               string hexOutput = String.Format("{0:x}", value);
                     sb.Append(hexOutput);
           }
           return sb.ToString();
       }
      
        public static string HexAsciiConvert(string hex)
        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i <= hex.Length - 2; i += 2)
            {

                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2),

                System.Globalization.NumberStyles.HexNumber))));

            }

            return sb.ToString();

        }
  

        public static string EncryptToBase64(byte[] data)
        {

            // initialize result with data length
            byte[] result = new byte[data.Length];
            // blocs
            int blocs = 0;
            if (data.Length > Bloc.Length)
            {
            
                int p = 0;
                // from 0 to t * x-1 e.g ( 0 to 1*512-1
                for (int i = 0; i <= data.Length - 1;i++ )
                {
                    if (p <= Bloc.Length - 1)
                    {
                        result[i] = (byte)(((int)data[i] + Factor) ^ (int)Bloc[p]);
                        p++;
                    }
                    else
                    {
                        p = 0;
                        result[i] = (byte)(((int)data[i] + Factor) ^ (int)Bloc[p]);
                        p++;
                    }
                 
                }
            }
            else
            {

                for (int i = 0; i <= data.Length - 1; )
                {

                    result[i] = (byte)(((int)data[i] + Factor) ^ (int)Bloc[i]);

                    i++;
                }
            }
            return Convert.ToBase64String(result);
        }
        public static byte[] DecryptFromBase64(string base64)
        {
            byte[] data = Convert.FromBase64String(base64);
            // initialize result with data length
            byte[] result = new byte[data.Length];
            // blocs

            if (data.Length > Bloc.Length)
            {
        
                int p = 0;
                // from 0 to t * x-1 e.g ( 0 to 1*512-1
                for (int i = 0; i <= data.Length - 1; i++ )
                {
                    if (p <= Bloc.Length - 1)
                    {
                        result[i] = (byte)(((int)data[i] ^ (int)Bloc[p]) - Factor);
                        p++;
                    }
                    else
                    {
                        p = 0;
                        result[i] = (byte)(((int)data[i] ^ (int)Bloc[p]) - Factor);
                        p++;
                    }
                 
                }
            }
            else
            {

                for (int i = 0; i <= data.Length - 1; )
                {

                    result[i] = (byte)(((int)data[i] ^ (int)Bloc[i]) - Factor);

                    i++;
                }
            }
            return result;
        }
    }
}
