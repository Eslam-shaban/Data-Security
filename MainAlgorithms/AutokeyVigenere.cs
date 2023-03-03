using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            //string plainText = Console.ReadLine(); //computer
            //string cipherText = Console.ReadLine();//jsxaivsd
           
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            int z1 = cipherText.Length;
            int z2 = plainText.Length;
            string ans = "";

            for (int i = 0; i < z1; i++)
            {
                int t = cipherText[i] - plainText[i];
                if (t < 0)
                    t += 26;
                ans = ans + (char)('a' + t);
            }
            int count = 0;
            for (int i = 0; i < ans.Length; i++)
            {
                if (ans[i] == plainText[0])
                {
                    count = i;
                    break;
                }
            }
            ans = ans.Remove(count);
            return ans;
           // throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            string ans = "";
            int count = 0;
            while (cipherText.Length > key.Length)
            {

                int t = cipherText[count] - key[count];
                if (t < 0)
                    t += 26;

                count++;
                key = key + (char)('a' + t);
                

            }
            

            int z1 = cipherText.Length;
            int z2 = key.Length;


            for (int i = 0; i < z1; i++)
            {
                int t = cipherText[i] - key[i];
                if (t < 0)
                    t += 26;
                ans = ans + (char)('a' + t);
            }

            return ans;
            //throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            
            int count = plainText.Length - key.Length;
            char[] arr1 = new char[count];
            char[] arr = new char[60];
            if (plainText.Length != key.Length)
            {

                for (int j = 0; j < count; j++)
                {

                    arr1[j] = plainText[j];
                    key = key + arr1[j];

                }
            }

            char temp = 'a';
            for (int row = 0; row < 26; row++)
            {
                arr[row] = temp;
                temp++;

            }
            temp = 'a';
            for (int row = 26; row <= 50; row++)
            {
                arr[row] = temp;
                temp++;
            }
            int z1 = plainText.Length;
            int z2 = key.Length;
            string ans = "";
            for (int i = 0; i < Math.Min(z1, z2); i++)
            {
                int x = plainText[i] - 'a';
                int y = key[i] - 'a';
                ans = ans + arr[x + y];
            }

                return ans;
            //throw new NotImplementedException();
        }
    }
}
