using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SHA_256
{
    internal class VitaAIDConvert
    {
        private static byte[] keyData = new byte[] {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x53, 0x72, 0x69, 0x20, 0x4a, 0x61, 0x79, 0x65,
            0x77, 0x61, 0x72, 0x64, 0x65, 0x6e, 0x65, 0x70,
            0x75, 0x72, 0x61, 0x20, 0x4b, 0x6f, 0x74, 0x74,
            0x65
        };

        private static byte[] secretKey = new byte[] {
            0xA9, 0xFA, 0x5A, 0x62, 0x79, 0x9F, 0xCC, 0x4C,
            0x72, 0x6B, 0x4E, 0x2C, 0xE3, 0x50, 0x6D, 0x38
        };

        public static string To64Digits(string aid)
        {
            string returnString = string.Empty;
            if (CheckValidAID(aid) == false)
            {
                return "Input AID Error Input AID Error Input AID Error Input AID Error ";
            }
            
            byte[] sha256 = SHA256.Create().ComputeHash(SetAID(aid));

            RijndaelManaged Rdel = new RijndaelManaged();
            Rdel.Mode = CipherMode.ECB;
            Rdel.Padding = PaddingMode.Zeros;
            Rdel.Key = secretKey;
            
            byte[] ResultArray = Rdel.CreateDecryptor().TransformFinalBlock(sha256, 0, sha256.Length);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in ResultArray)
            {
                sb.Append($"{b:X2}");
            }
            returnString = sb.ToString().ToLower();
            
            StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "\\64digits.txt", false, Encoding.UTF8);
            try
            { 
                sw.Write(returnString);
            }
            catch
            {
                returnString = "FileWrite Error FileWrite Error FileWrite Error FileWrite Error ";
            }
            finally
            {
                sw.Close();
            }


            return returnString;
        }

        private static bool CheckValidAID(string data)
        {
            if (data == null)
                return false;

            if (data.Length != 16)
                return false;

            byte[] byteData = Encoding.ASCII.GetBytes(data);

            foreach (byte b in byteData)
            {
                if (!(0x30 <= b && b <= 0x39 || 0x41 <= b && b <= 0x46 || 0x61 <= b && b <= 0x66))
                    return false;
            }

            return true;
        }
        
        private static byte[] SetAID(string aid)
        {
            byte[] dum = new byte[keyData.Length];
            Array.Copy(keyData, dum, keyData.Length);

            bool odd = true;
            int index = 0;            
            byte f = 0;
            foreach (byte b in aid)
            {
                if (odd)
                    f = b;
                else
                    dum[index++] = GetUnionByte(f, b);

                odd = !odd;
            }

            return dum;
        }
        
        private static byte GetUnionByte(byte f, byte b)
        {
            byte res = 0;

            res = Convert(f);
            res = (byte)(res << 4);
            res = (byte)(res | Convert(b));

            return res;
        }

        private static byte Convert(byte b)
        {
            if (0x30 <= b && b <= 0x39)
                return (byte)(b - 0x30);
            else if (0x41 <= b && b <= 0x46)
                return (byte)(b - 0x37);
            else if (0x61 <= b && b <= 0x66)
                return (byte)(b - 0x57);
            else
                return 0;
        }
    }
}
