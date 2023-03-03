using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            string CT = "";
            string chars = "abcdefghijklmnopqrstuvwxyz";
            plainText = plainText.ToLower();
            //throw  new NotImplementedException();
            for (int i = 0; i < plainText.Length;i++ )
            {
                for (int j = 0; j < 26; j++)
                {
                    if (plainText[i] == chars[j])
                    {
                        CT += chars[(j + key) % 26];
                    
                    }
                }
                

            }
            return CT;
        }

        public string Decrypt(string cipherText, int key)
        {
            string PT = "";
            string chars = "abcdefghijklmnopqrstuvwxyz";
            cipherText = cipherText.ToLower();
            //throw  new NotImplementedException();
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (cipherText[i] == chars[j])
                    {
                        int result = (j - key) % 26;
                        if (result < 0)
                        {
                            PT += chars[result + 26];
                        }
                        else
                        {
                            PT += chars[result];
                        }


                    }
                }


            }
            return PT;
        }

        public int Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();

            //string chars = "abcdefghijklmnopqrstuvwxyz";
            //throw  new NotImplementedException();
            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();
            if ((cipherText[0] - plainText[0]) >= 0)
            {

                //cipherText[i] - plainText[i];
               return(cipherText[0] - plainText[0]);
            }
            else
            {
              
                return ((cipherText[0] - plainText[0]) + 26);
            }

            //return 0;
        }
    }
}
