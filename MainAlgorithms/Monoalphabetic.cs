using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();

            char[] Key = new char[26];
            String _key = "";
            string chars = "abcdefghijklmnopqrstuvwxyz";
            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();
            //throw  new NotImplementedException();
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (plainText[i] == chars[j])
                    {
                        Key[j] = cipherText[i];

                    }

                }

            }

            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < chars.Length; j++)
                {
                    if (cipherText[i] == chars[j])
                    {
                        chars = chars.Remove(j, 1);
                    }

                }

            }
            int k = 0;
            for (int i = 0; i < 26; i++)
            {
                if (Key[i] == '\0')
                {
                    Key[i] = chars[k++];
                }

            }

            for (int i = 0; i < 26; i++)
            {
                _key += Key[i];
            }
            return _key;
        }

        public string Decrypt(string cipherText, string key)
        {

            //throw new NotImplementedException();
            string PT = "";
            string chars = "abcdefghijklmnopqrstuvwxyz";
            cipherText = cipherText.ToLower();
            //throw  new NotImplementedException();
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (cipherText[i] == key[j])
                    {
                        PT += chars[j];

                    }
                }

            }
            return PT;
       
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            string CT = "";
            string chars = "abcdefghijklmnopqrstuvwxyz";
            plainText = plainText.ToLower();
            //throw  new NotImplementedException();
            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (plainText[i] == chars[j])
                    {
                        CT += key[j];

                    }
                }

            }
            return CT;
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            Dictionary<char, int> dic = new Dictionary<char, int>();

            cipher = cipher.ToLower();
            string chars = "abcdefghijklmnopqrstuvwxyz";
            string replace = "etaoinsrhldcumfpgwybvkxjqz";
            for (int i = 0; i < 26; i++)
            {
                char d = chars[i];
                int counter = 0;
                for (int j = 0; j < cipher.Length; j++)
                {
                    if (chars[i] == cipher[j])
                    {
                        counter++;
                    }

                }

                dic.Add(chars[i], counter);

            }

            

            var sortedDict = from entry in dic orderby entry.Value descending select entry;

            int index = 0;
            Dictionary<char, char> dic_1 = new Dictionary<char, char>();
            foreach (KeyValuePair<char, int> d in sortedDict) {
                dic_1.Add(d.Key, replace[index++]);
            
            }
            string PT="";
            for (int i = 0; i <cipher.Length; i++)
            {

                foreach (KeyValuePair<char, char> d in dic_1)
                {
                    if (d.Key == cipher[i])
                    {
                        PT += d.Value;
                    }
                
                }
            }

                return PT;
        }
    }
}
