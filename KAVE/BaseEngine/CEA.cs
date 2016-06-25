using System;
using System.Collections.Generic;
using System.Text;

namespace KAVE
{

    /// <summary>
    /// Complex Encryption Algorithm
    /// </summary>
    public  static class CEA
    {
        // Delta's Binary
        private static readonly bool[] D0 = new bool[8];
        private static readonly bool[] D1 = new bool[8];
        private static readonly bool[] D2 = new bool[8];
        private static readonly bool[] D3 = new bool[8];
        private static byte[] Delta;
        // Key sum
        public static byte KS = 0;
        public static byte KSP = 0;
        // Combination S
        private static readonly bool[] S = 
        {
           false,false, false, false,
           false,false,false,true,
           false,false,true,false,
           false,false,true,true,

           false,true,false,false,
           false,true,false,true,
           false,true,true,false,
           false,true,true,true,

           true,false,false,false,
           true,false,false,true,
           true,false,true,false,
           true,false,true,true,

           true,true,false,false,
           true,true,false,true,
           true,true,true,false,
           true,true,true,true

        };
        // Reverse Combination RS
        private static readonly bool[] RS =
        {
           true,true,true,true,
           false,false,true,true,
           true,true,false,true,
           false,false,true,false,
         

           false,true,false,false,
            true,false,true,false,
           false,true,true,false,
           true,true,false,false,
            
           false,false, false, false,
           true,false,true,true,
           true,false,false,true,
           false,true,false,true,
        
            true,false,false,false,
           false,true,true,true,     
           true,true,true,false,
           false,false,false,true

        };
        // Key Bloc
        public static byte[] KeyBloc;
        // S-Box Matrix
        // The S box
        private static readonly byte[] SB =
		{
			 112,  62, 181, 102,  72,   3, 246,  14,
            97,  53,  87, 185, 134, 193,  29, 158,
            225, 248, 152,  17, 105, 217, 142, 148,
            155,  30, 135, 233, 206,  85,  40, 223,
            140, 161, 137,  13, 191, 230,  66, 104,
            65, 153,  45,  15, 176,  84, 187,  22,
			99, 124, 119, 123, 242, 107, 111, 197,
            48,   1, 103,  43, 254, 215, 171, 118,
            202, 130, 201, 125, 250,  89,  71, 240,
            173, 212, 162, 175, 156, 164, 114, 192,
            183, 253, 147,  38,  54,  63, 247, 204,
            52, 165, 229, 241, 113, 216,  49,  21,
            4, 199,  35, 195,  24, 150,   5, 154,
            7,  18, 128, 226, 235,  39, 178, 117,
            9, 131,  44,  26,  27, 110,  90, 160,
            82,  59, 214, 179,  41, 227,  47, 132,
            83, 209,   0, 237,  32, 252, 177,  91,
            106, 203, 190,  57,  74,  76,  88, 207,
            208, 239, 170, 251,  67,  77,  51, 133,
            69, 249,   2, 127,  80,  60, 159, 168,
            81, 163,  64, 143, 146, 157,  56, 245,
            188, 182, 218,  33,  16, 255, 243, 210,
            205,  12,  19, 236,  95, 151,  68,  23,
            196, 167, 126,  61, 100,  93,  25, 115,
            96, 129,  79, 220,  34,  42, 144, 136,
            70, 238, 184,  20, 222,  94,  11, 219,
            224,  50,  58,  10,  73,   6,  36,  92,
            194, 211, 172,  98, 145, 149, 228, 121,
            231, 200,  55, 109, 141, 213,  78, 169,
            108,  86, 244, 234, 101, 122, 174,   8,
            186, 120,  37,  46,  28, 166, 180, 198,
            232, 221, 116,  31,  75, 189, 139, 138,
           
		
		};
        // The inverse S-box
        private static readonly byte[] Si =
		{
            160, 224, 59, 77, 174, 42, 245, 176,
			82, 9, 106, 213, 48, 54, 165, 56,
			200, 235, 187, 60, 131, 83, 153, 97,
			23, 43, 4, 126, 186, 119, 214, 38,
			225, 105, 20, 99, 85, 33, 12, 125,
            71, 241, 26, 113, 29, 41, 197, 137,
			111, 183, 98, 14, 170, 24, 190, 27,
			252, 86, 62, 75, 198, 210, 121, 32,
			154, 219, 192, 254, 120, 205, 90, 244,
			31, 221, 168, 51, 136, 7, 199, 49,
			177, 18, 16, 89, 39, 128, 236, 95,
			96, 81, 127, 169, 25, 181, 74, 13,
			84, 123, 148, 50, 166, 194, 35, 61,
			238, 76, 149, 11, 66, 250, 195, 78,
			8, 46, 161, 102, 40, 217, 36, 178,
			118, 91, 162, 73, 109, 139, 209, 37,
			114, 248, 246, 100, 134, 104, 152, 22,
			212, 164, 92, 204, 93, 101, 182, 146,
			108, 112, 72, 80, 253, 237, 185, 218,
			94, 21, 70, 87, 167, 141, 157, 132,
			144, 216, 171, 0, 140, 188, 211, 10,
			247, 228, 88, 5, 184, 179, 69, 6,
			208, 44, 30, 143, 202, 63, 15, 2,
			193, 175, 189, 3, 1, 19, 138, 107,
			58, 145, 17, 65, 79, 103, 220, 234,
			151, 242, 207, 206, 240, 180, 230, 115,
			150, 172, 116, 34, 231, 173, 53, 133,
			226, 249, 55, 232, 28, 117, 223, 110,
		    191, 64, 163, 158, 129, 243, 215, 251,
			124, 227, 57, 130, 155, 47, 255, 135,
			52, 142, 67, 68, 196, 222, 233, 203,
			45, 229, 122, 159, 147, 201, 156, 239,
			
		
		};

        // ER (Encryption Rounds)
        public static int rounds = 2;
        // Global factor's
        private static byte x = 0;
        private static byte y = 0;

        public static void Initialize(byte[] key, byte[] delta)
        {
            if (key.Length < 8)
                throw new ArgumentException("Key length must be greater or equal to 8 bytes (64 bits key)");
            if (delta.Length != 4)
                throw new ArgumentException("Delta length must be greater or equal to 4 bytes (32 bits)");

            KeyBloc = new byte[256];
            Delta = delta;
            rounds = 1;
            // key schedule
            int k = 0;
            for (int i = 0; i <= 255; i++)
            {


                if (k != key.Length - 1)
                {
                    KeyBloc[i] = (byte)((KE(key[k], SB[i]) ^ Si[255 - i]) ^ (delta[0] + delta[2]));
                    k++;
                }
                else
                {
                    KeyBloc[i] = (byte)((KE(key[k], SB[i]) ^ Si[255 - i]) ^ (delta[0] + delta[2]));
                    k = 0;

                }
                KS = (byte)(KeyBloc[i] + KS);
            }
            // Key sum
            for (int i = 0; i <= key.Length - 1; i++)
                KSP = (byte)(key[i] + KSP);

            x = (byte)(KS / 16);
            y = (byte)(KS % 16);

        }
        public static void Initialize(byte[] key, int blocsize, byte[] delta)
        {
            if (key.Length < 8)
                throw new ArgumentException("Key length must be greater or equal to 8 bytes (64 bits key)");
            if (delta.Length != 4)
                throw new ArgumentException("Delta length must be greater or equal to 4 bytes (32 bits)");
            if (blocsize < 16)
                throw new ArgumentException("Bloc size must be greater or equal to 16 bytes (128 bits)");

            KeyBloc = new byte[blocsize];
            Delta = delta;
            rounds = 1;

            // key schedule
            int k = 0;
            int a = 0;
            for (int i = 0; i <= blocsize - 1; i++)
            {

                if (a == 256)
                    a = 0;

                if (k != key.Length - 1)
                {
                    KeyBloc[i] = (byte)((KE(key[k], SB[a]) ^ Si[255 - a]) ^ (delta[0] + delta[2]));
                    k++;
                    a++;
                }
                else
                {
                    KeyBloc[i] = (byte)((KE(key[k], SB[a]) ^ Si[255 - a]) ^ (delta[0] + delta[2]));
                    k = 0;
                    a++;
                }
                KS = (byte)(KeyBloc[i] + KS);
            }

            // Key sum
            for (int i = 0; i <= key.Length - 1; i++)
                KSP = (byte)(key[i] + KSP);

            x = (byte)(KS / 16);
            y = (byte)(KS % 16);
        }
        public static void Initialize(byte[] key, byte[] delta, byte round)
        {
            if (key.Length < 8)
                throw new ArgumentException("Key length must be greater or equal to 8 bytes (64 bits key)");
            if (delta.Length != 4)
                throw new ArgumentException("Delta length must be greater or equal to 4 bytes (32 bits)");

            if (round < 0 && round > 64)
                throw new ArgumentException("Round count must be from [1..64]");


            KeyBloc = new byte[256];
            Delta = delta;
            rounds = round;

            // key schedule
            int k = 0;
            for (int i = 0; i <= 255; i++)
            {
                KS = (byte)(KeyBloc[i] + KS);
                if (k != key.Length - 1)
                {
                    KeyBloc[i] = (byte)((KE(key[k], SB[i]) ^ Si[255 - i]) ^ (delta[0] + delta[2]));
                    k++;
                }
                else
                {
                    KeyBloc[i] = (byte)((KE(key[k], SB[i]) ^ Si[255 - i]) ^ (delta[0] + delta[2]));
                    k = 0;

                }
                KS = (byte)(KeyBloc[i] + KS);
            }

            // Key sum
            for (int i = 0; i <= key.Length - 1; i++)
                KSP = (byte)(key[i] + KSP);

            x = (byte)(KS / 16);
            y = (byte)(KS % 16);
        }
        public static void Initialize(byte[] key, int blocsize, byte[] delta, byte round)
        {
            if (key.Length < 8)
                throw new ArgumentException("Key length must be greater or equal to 8 bytes (64 bits key)");
            if (delta.Length != 4)
                throw new ArgumentException("Delta length must be greater or equal to 4 bytes (32 bits)");
            if (blocsize < 16)
                throw new ArgumentException("Bloc size must be greater or equal to 16 bytes (128 bits)");

            if (round < 0 && round > 64)
                throw new ArgumentException("Round count must be from [1..64]");


            KeyBloc = new byte[blocsize];
            Delta = delta;
            rounds = round;

            // key schedule
            int k = 0;
            int a = 0;
            for (int i = 0; i <= blocsize - 1; i++)
            {
                KS = (byte)(KeyBloc[i] + KS);
                if (a == 256)
                    a = 0;

                if (k != key.Length - 1)
                {
                    KeyBloc[i] = (byte)((KE(key[k], SB[a]) ^ Si[255 - a]) ^ (delta[0] + delta[2]));
                    k++;
                    a++;
                }
                else
                {
                    KeyBloc[i] = (byte)((KE(key[k], SB[a]) ^ Si[255 - a]) ^ (delta[0] + delta[2]));
                    k = 0;
                    a++;

                }
                KS = (byte)(KeyBloc[i] + KS);
            }

            // Key sum
            for (int i = 0; i <= key.Length - 1; i++)
                KSP = (byte)(key[i] + KSP);

            x = (byte)(KS / 16);
            y = (byte)(KS % 16);

        }

        public static byte KE(byte key, byte encoder)
        {
            byte val = 0;
            for (int i = 0; i <= 7; i++)
            {

                if (!((key % 2 == 1) && (encoder % 2 == 1)))
                    val |= (byte)(Math.Pow(2, i));

                key = (byte)(key / 2);
                encoder = (byte)(encoder / 2);
            }
            return val;
        }


        public static byte[] Encrypt(byte[] data)
        {
            byte[] result = data;

            int k = 0;
            // Bytes encryption
            for (int i = 0; i <= data.Length - 1; i++)
            {
                if (k > KeyBloc.Length - 4)
                    k = 0;

                // spliting and combination
                // convert byte to 8 bit array
                bool[] P = new bool[8];
                for (int j = 0; j <= 7; j++)
                {
                    P[j] = (result[i] % 2 == 1);
                    result[i] = (byte)(result[i] / 2);
                }
                // split bits into 2 sections

                P[0] = P[0] ^ S[y * 4];
                P[1] = P[1] ^ S[(y * 4) + 1];
                P[2] = P[2] ^ S[(y * 4) + 2];
                P[3] = P[3] ^ S[(y * 4) + 3];

                P[4] = P[4] ^ RS[x * 4];
                P[5] = P[5] ^ RS[(x * 4) + 1];
                P[6] = P[6] ^ RS[(x * 4) + 2];
                P[7] = P[7] ^ RS[(x * 4) + 3];



                // Before unify and unify and Globalize
                // convert k0 to 8 bits
                byte kz = KeyBloc[k];
                bool[] K0 = new bool[8];
                for (int j = 0; j <= 7; j++)
                {
                    K0[j] = (kz % 2 == 1);
                    kz = (byte)(kz / 2);
                }
                // convert D3 to 8 bits
                byte dt = Delta[3];
                bool[] D3 = new bool[8];
                for (int j = 0; j <= 7; j++)
                {
                    D3[j] = (dt % 2 == 1);
                    dt = (byte)(dt / 2);
                }

                // operation 1
                P[0] = !(P[0] ^ K0[4]);
                P[1] = !(P[1] ^ K0[5]);
                P[2] = !(P[2] ^ K0[6]);
                P[3] = !(P[3] ^ K0[7]);

                P[4] = !(P[4] ^ K0[0]);
                P[5] = !(P[5] ^ K0[1]);
                P[6] = !(P[6] ^ K0[2]);
                P[7] = !(P[7] ^ K0[3]);

                // operation 2
                P[0] = P[0] ^ D3[4];
                P[1] = P[1] ^ D3[5];
                P[2] = P[2] ^ D3[6];
                P[3] = P[3] ^ D3[7];

                P[4] = P[4] ^ D3[0];
                P[5] = P[5] ^ D3[1];
                P[6] = P[6] ^ D3[2];
                P[7] = P[7] ^ D3[3];

                // unify
                result[i] = 0;
                for (int j = 0; j <= 7; j++)
                {
                    if (P[j])
                        result[i] |= (byte)(Math.Pow(2, j));
                }
                // Globalize
                result[i] = (byte)(result[i] ^ SB[x + y]);




                // Encryption circuits and final
                result[i] = (byte)((((KeyBloc[k + 2] + Delta[2]) + KeyBloc[k + 1]) ^ (result[i] ^ (Delta[1] + KeyBloc[k + 3]))) ^ Delta[0]);

                result[i] = (byte)(result[i] ^ KSP);

                k = k + 4;
            }

            // Cipher inversion
            int b = 0;
            for (int i = 0; i <= (result.Length / 4) - 1; i++)
            {

                byte c = result[b];
                result[b] = result[b + 2];
                result[b + 2] = c;

                c = result[b + 1];
                result[b + 1] = result[b + 3];
                result[b + 3] = c;
                b = b + 4;
            }
            return result;
        }
        public static byte[] Decrypt(byte[] data)
        {
            byte[] result = data;
            // Cipher inversion
            int b = 0;
            for (int i = 0; i <= (result.Length / 4) - 1; i++)
            {
                byte c = result[b];
                result[b] = result[b + 2];
                result[b + 2] = c;

                c = result[b + 1];
                result[b + 1] = result[b + 3];
                result[b + 3] = c;
                b = b + 4;
            }
            //
            // Bytes encryption
            int k = 0;
            for (int i = 0; i <= data.Length - 1; i++)
            {
                if (k > KeyBloc.Length - 4)
                    k = 0;

                // final and Encryption circuits
                result[i] = (byte)(data[i] ^ KSP);

                result[i] = (byte)(((result[i] ^ Delta[0]) ^ (KeyBloc[k + 1] + (KeyBloc[k + 2] + Delta[2]))) ^ (Delta[1] + KeyBloc[k + 3]));


                // Globalize and split and Before unify

                // Globalize
                result[i] = (byte)(result[i] ^ SB[x + y]);

                // split
                bool[] C = new bool[8];
                for (int j = 0; j <= 7; j++)
                {
                    C[j] = (result[i] % 2 == 1);

                    result[i] = (byte)(result[i] / 2);
                }
                // convert k0 to 8 bits
                byte kz = KeyBloc[k];
                bool[] K0 = new bool[8];
                for (int j = 0; j <= 7; j++)
                {
                    K0[j] = (kz % 2 == 1);
                    kz = (byte)(kz / 2);
                }
                // convert D3 to 8 bits
                byte dt = Delta[3];
                bool[] D3 = new bool[8];
                for (int j = 0; j <= 7; j++)
                {
                    D3[j] = (dt % 2 == 1);
                    dt = (byte)(dt / 2);
                }

                // operation 2
                C[0] = C[0] ^ D3[4];
                C[1] = C[1] ^ D3[5];
                C[2] = C[2] ^ D3[6];
                C[3] = C[3] ^ D3[7];

                C[4] = C[4] ^ D3[0];
                C[5] = C[5] ^ D3[1];
                C[6] = C[6] ^ D3[2];
                C[7] = C[7] ^ D3[3];

                // operation 1
                C[0] = !(C[0] ^ K0[4]);
                C[1] = !(C[1] ^ K0[5]);
                C[2] = !(C[2] ^ K0[6]);
                C[3] = !(C[3] ^ K0[7]);

                C[4] = !(C[4] ^ K0[0]);
                C[5] = !(C[5] ^ K0[1]);
                C[6] = !(C[6] ^ K0[2]);
                C[7] = !(C[7] ^ K0[3]);


                // combination and unify

                // unify bits from 2 sections

                C[0] = C[0] ^ S[y * 4];
                C[1] = C[1] ^ S[(y * 4) + 1];
                C[2] = C[2] ^ S[(y * 4) + 2];
                C[3] = C[3] ^ S[(y * 4) + 3];

                C[4] = C[4] ^ RS[x * 4];
                C[5] = C[5] ^ RS[(x * 4) + 1];
                C[6] = C[6] ^ RS[(x * 4) + 2];
                C[7] = C[7] ^ RS[(x * 4) + 3];
                // unification
                result[i] = 0;
                for (int j = 0; j <= 7; j++)
                {
                    if (C[j])
                        result[i] |= (byte)(Math.Pow(2, j));
                }




                k = k + 4;
            }


            return result;
        }

        public static byte[] EncryptHL(byte[] data)
        {
            byte[] result = data;

            int k = 0;
            // Bytes encryption
            for (int i = 0; i <= data.Length - 1; i++)
            {
                k = 0;
                for (int r = 0; r <= rounds - 1; r++)
                {
                    if (k > KeyBloc.Length - 4)
                        k = 0;

                    // spliting and combination
                    // convert byte to 8 bit array
                    bool[] P = new bool[8];
                    for (int j = 0; j <= 7; j++)
                    {
                        P[j] = (result[i] % 2 == 1);
                        result[i] = (byte)(result[i] / 2);
                    }
                    // split bits into 2 sections

                    P[0] = P[0] ^ S[y * 4];
                    P[1] = P[1] ^ S[(y * 4) + 1];
                    P[2] = P[2] ^ S[(y * 4) + 2];
                    P[3] = P[3] ^ S[(y * 4) + 3];

                    P[4] = P[4] ^ RS[x * 4];
                    P[5] = P[5] ^ RS[(x * 4) + 1];
                    P[6] = P[6] ^ RS[(x * 4) + 2];
                    P[7] = P[7] ^ RS[(x * 4) + 3];



                    // Before unify and unify and Globalize
                    // convert k0 to 8 bits
                    byte kz = KeyBloc[k];
                    bool[] K0 = new bool[8];
                    for (int j = 0; j <= 7; j++)
                    {
                        K0[j] = (kz % 2 == 1);
                        kz = (byte)(kz / 2);
                    }
                    // convert D3 to 8 bits
                    byte dt = Delta[3];
                    bool[] D3 = new bool[8];
                    for (int j = 0; j <= 7; j++)
                    {
                        D3[j] = (dt % 2 == 1);
                        dt = (byte)(dt / 2);
                    }

                    // operation 1
                    P[0] = !(P[0] ^ K0[4]);
                    P[1] = !(P[1] ^ K0[5]);
                    P[2] = !(P[2] ^ K0[6]);
                    P[3] = !(P[3] ^ K0[7]);

                    P[4] = !(P[4] ^ K0[0]);
                    P[5] = !(P[5] ^ K0[1]);
                    P[6] = !(P[6] ^ K0[2]);
                    P[7] = !(P[7] ^ K0[3]);

                    // operation 2
                    P[0] = P[0] ^ D3[4];
                    P[1] = P[1] ^ D3[5];
                    P[2] = P[2] ^ D3[6];
                    P[3] = P[3] ^ D3[7];

                    P[4] = P[4] ^ D3[0];
                    P[5] = P[5] ^ D3[1];
                    P[6] = P[6] ^ D3[2];
                    P[7] = P[7] ^ D3[3];

                    // unify
                    result[i] = 0;
                    for (int j = 0; j <= 7; j++)
                    {
                        if (P[j])
                            result[i] |= (byte)(Math.Pow(2, j));
                    }
                    // Globalize
                    result[i] = (byte)(result[i] ^ SB[x + y]);




                    // Encryption circuits and final
                    result[i] = (byte)((((KeyBloc[k + 2] + Delta[2]) + KeyBloc[k + 1]) ^ (result[i] ^ (Delta[1] + KeyBloc[k + 3]))) ^ Delta[0]);

                    result[i] = (byte)(result[i] ^ KSP);

                    k = k + 4;
                }
            }

            // Cipher inversion
            int b = 0;
            for (int i = 0; i <= (result.Length / 4) - 1; i++)
            {

                byte c = result[b];
                result[b] = result[b + 2];
                result[b + 2] = c;

                c = result[b + 1];
                result[b + 1] = result[b + 3];
                result[b + 3] = c;
                b = b + 4;
            }
            return result;
        }
        public static byte[] DecryptHL(byte[] data)
        {
            byte[] result = data;
            // Cipher inversion
            int b = 0;
            for (int i = 0; i <= (result.Length / 4) - 1; i++)
            {
                byte c = result[b];
                result[b] = result[b + 2];
                result[b + 2] = c;

                c = result[b + 1];
                result[b + 1] = result[b + 3];
                result[b + 3] = c;
                b = b + 4;
            }
            //
            // Bytes encryption
            int k = 0;
            for (int i = 0; i <= data.Length - 1; i++)
            {
                k = (4 * rounds) - 1;
                for (int r = 0; r <= rounds - 1; r++)
                {
                    if (k < 0)
                        k = 0;

                    // final and Encryption circuits
                    result[i] = (byte)(data[i] ^ KSP);

                    result[i] = (byte)(((result[i] ^ Delta[0]) ^ (KeyBloc[k - 2] + (KeyBloc[k - 1] + Delta[2]))) ^ (Delta[1] + KeyBloc[k]));


                    // Globalize and split and Before unify

                    // Globalize
                    result[i] = (byte)(result[i] ^ SB[x + y]);

                    // split
                    bool[] C = new bool[8];
                    for (int j = 0; j <= 7; j++)
                    {
                        C[j] = (result[i] % 2 == 1);

                        result[i] = (byte)(result[i] / 2);
                    }
                    // convert k0 to 8 bits
                    byte kz = KeyBloc[k - 3];
                    bool[] K0 = new bool[8];
                    for (int j = 0; j <= 7; j++)
                    {
                        K0[j] = (kz % 2 == 1);
                        kz = (byte)(kz / 2);
                    }
                    // convert D3 to 8 bits
                    byte dt = Delta[3];
                    bool[] D3 = new bool[8];
                    for (int j = 0; j <= 7; j++)
                    {
                        D3[j] = (dt % 2 == 1);
                        dt = (byte)(dt / 2);
                    }

                    // operation 2
                    C[0] = C[0] ^ D3[4];
                    C[1] = C[1] ^ D3[5];
                    C[2] = C[2] ^ D3[6];
                    C[3] = C[3] ^ D3[7];

                    C[4] = C[4] ^ D3[0];
                    C[5] = C[5] ^ D3[1];
                    C[6] = C[6] ^ D3[2];
                    C[7] = C[7] ^ D3[3];

                    // operation 1
                    C[0] = !(C[0] ^ K0[4]);
                    C[1] = !(C[1] ^ K0[5]);
                    C[2] = !(C[2] ^ K0[6]);
                    C[3] = !(C[3] ^ K0[7]);

                    C[4] = !(C[4] ^ K0[0]);
                    C[5] = !(C[5] ^ K0[1]);
                    C[6] = !(C[6] ^ K0[2]);
                    C[7] = !(C[7] ^ K0[3]);


                    // combination and unify

                    // unify bits from 2 sections

                    C[0] = C[0] ^ S[y * 4];
                    C[1] = C[1] ^ S[(y * 4) + 1];
                    C[2] = C[2] ^ S[(y * 4) + 2];
                    C[3] = C[3] ^ S[(y * 4) + 3];

                    C[4] = C[4] ^ RS[x * 4];
                    C[5] = C[5] ^ RS[(x * 4) + 1];
                    C[6] = C[6] ^ RS[(x * 4) + 2];
                    C[7] = C[7] ^ RS[(x * 4) + 3];
                    // unification
                    result[i] = 0;
                    for (int j = 0; j <= 7; j++)
                    {
                        if (C[j])
                            result[i] |= (byte)(Math.Pow(2, j));
                    }




                    k = k - 4;
                }
            }


            return result;
        }
    }
}
