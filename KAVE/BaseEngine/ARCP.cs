using System;
using System.Collections.Generic;
using System.Text;

namespace KAVE
{
   public static class ARCP
    {
       public static string ComputeCRC32(byte[] data)
       {
           uint crc = 0xFFFFFFFF;       // initial contents of LFBSR
           uint poly = 0xEDB88320;   // reverse polynomial
           for (int i = 0; i < data.Length; i++)
           {
               uint temp = (crc ^ data[i]) & 0xff;

               // read 8 bits one at a time
               for (int j = 0; j < 8; j++)
               {
                   if ((temp & 1) == 1) temp = (temp >> 1) ^ poly;
                   else temp = (temp >> 1);
               }
               crc = (crc >> 8) ^ temp;
           }

           // flip bits
           crc = crc ^ 0xffffffff;
           string hex = crc.ToString("x2");
           if (hex.Length == 8)
           {
               return hex;
           }
           else if (hex.Length == 7)
           {
               return hex + "0";
           }
           else if (hex.Length == 6)
           {
               return hex + "00";
           }
           else if (hex.Length == 5)
           {
               return hex + "000";
           }
           else if (hex.Length == 5)
           {
               return hex + "0000";
           }
           else
           {
               return "7x9a8g3q";
           }

       }
       public static byte[] Append(byte[] data, byte[] append)
       {
           byte[] ba = new byte[data.Length + append.Length];
           int j = -1;
           for (int i = 0; i <= ba.Length - 1; i++)
           {
               if (i < data.Length - 1)
               {
                   ba[i] = data[i];
               }
               else if (i == data.Length - 1)
               {
                   ba[i] = data[i];
                   j++;
               }
               else
               {
                   ba[i] = append[j];
                   j++;
               }

           }
           return ba;
       }
       public static byte[] BuildPacket(string source, string command, string accept, string timeout, byte[] data, byte state)
       {
           try
           {

               if (command.Length == 8 && accept.Length == 6 && source.Length == 14 && timeout.Length == 4 && data.Length < 512000)
               {
      
                  // compute CRC32
                   string checksum = ComputeCRC32(data);
                   byte[] packetheader = Append(Encoding.UTF8.GetBytes(source + accept + command + timeout + checksum), new byte[1] { state });


                   return Append(packetheader, SEA.Encrypt(data));
               }
               else
               {
                   return null;
               }
           }
           catch
           {
               return null;
           }

       }
       public static byte[] GetHeader(byte[] packet)
       {
           byte[] header = new byte[41];
           for (int i = 0; i <= 40; i++)
               header[i] = packet[i];

           return header;
       }
       public static byte[] GetData(byte[] packet)
       {
           byte[] data = new byte[packet.Length - 41];
           for (int i = 41, j = 0; i <= packet.Length - 1; i++,j++)
               data[j] = packet[i];

           return data;
       }

    }
}
