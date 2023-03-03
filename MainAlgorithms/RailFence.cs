using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            int key = 0;
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            Console.WriteLine(plainText[2]);
            for (int i = 2; i < cipherText.Length; i++)
            {

                if (cipherText[1] == plainText[i])
                {

                    key = i;
                    break;

                }

            }

            return key;
        }

        public string Decrypt(string cipherText, int key)
        {
            double key_ = Convert.ToDouble(key);
            double x = (cipherText.Length) / key_;
            x = Math.Ceiling(x);
            int cols = Convert.ToInt32(x);
            char[,] arr = new char[key, cols];
            string PT = "";
            int k = 0;
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (k < cipherText.Length)
                        arr[i, j] = cipherText[k++];

                }

            }

            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(arr[i, j]);

                }

                Console.Write("\n");
            }

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < key; j++)
                {
                    PT += arr[j, i];

                }

            }

            return PT;
        }

        public string Encrypt(string plainText, int key)
        {

            double key_ = Convert.ToDouble(key);
            double x = (plainText.Length) / key_;
            x = Math.Ceiling(x);
            int cols = Convert.ToInt32(x);
            char[,] arr = new char[key, cols];
            string CT = "";
            int k = 0;
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < key; j++)
                {
                    if (k < plainText.Length)
                        arr[j, i] = plainText[k++];

                }

            }

            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(arr[i, j]);

                }

                Console.Write("\n");
            }

            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    CT += arr[i, j];

                }

            }

            return CT;
        }
    }
}
