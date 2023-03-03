using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            //pt=computer , keystream=hellohel  ,cipher= jsxaiaic
            plainText = plainText.ToLower(); //  k = ct - pt
            cipherText = cipherText.ToLower();
            string chars = "abcdefghijklmnopqrstuvwxyz";
            string keyStream = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                keyStream = keyStream + chars[((chars.IndexOf(cipherText[i]) - chars.IndexOf(plainText[i])) + 26) % 26];

            }
            //keystream=hellohel
            string key = keyStream, tempK = "";
            int keyLength = keyStream.Length, tempInd = 0;
            for (int i = 0; i < keyLength; i++)
            {
                tempK = tempK.Insert(i, key[i].ToString());
                tempInd = key.IndexOf(tempK, i + 1);
                if (tempInd == tempK.Length)
                {
                    key = key.Remove(tempK.Length, key.Length - tempK.Length);
                    break;
                }
            }
            return key;
        }

        public string Decrypt(string cipherText, string key)
        {
            key = key.ToLower();
            cipherText = cipherText.ToLower();
            int ciphertxt_len = cipherText.Length;
            int i = 0;
            while (true)
            {
                if (ciphertxt_len == i)
                    i = 0;

                if (key.Length == cipherText.Length)
                    break;
                key = key + key[i];
                i++;
            }
            //pt=computer , keystream=hellohel  ,cipher= jsxaiaic
            string chars = "abcdefghijklmnopqrstuvwxyz";
            string plaintxt = "";
            int z = 0;
            while ( z < cipherText.Length)
            {
                plaintxt = plaintxt + chars[((chars.IndexOf(cipherText[z]) - chars.IndexOf(key[z])) + 26) % 26];
                z++;
            }
            return plaintxt;
        }

        public string Encrypt(string plainText, string key)
        {
            key = key.ToLower();
            int plaintxt_len = plainText.Length;
            int i = 0;
            while (true)
            {
                if (plaintxt_len == i)
                    i = 0;

                if (key.Length == plainText.Length)
                    break;
                key = key + key[i];
                i++;
            }
            //pt=computer , keystream=hellohel
            string chars = "abcdefghijklmnopqrstuvwxyz";
            string ciphertxt = "";
            for (int j = 0; j < plainText.Length; j++)
            {
                ciphertxt = ciphertxt + chars[(chars.IndexOf(plainText[j]) + chars.IndexOf(key[j])) % 26];
            }
            return ciphertxt;
        }
    }
}