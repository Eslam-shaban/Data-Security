using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            List<string> Cols = new List<string>();
            List<int> key = new List<int>();
            char First = cipherText[0];
            char Second = cipherText[1];
            int NumOfCol = 0;
            int check = 0;
            int ElementsPerCol = 0;

            //get num of cols
            for (int i = 0; i < plainText.Length; i++)
            {
                if (plainText[i] == First)
                {
                    //if repetetion in character
                    //ex: attackpostponeduntiltwoam||ttnaaptmtsuoaodwcoiknlpet|| "t" repeating at index 1, 2
                    int idx = i;
                    if (plainText[i + 1] == First)
                    {
                        idx = i + 1;
                    }
                    for (int j = idx + 1; j < plainText.Length - idx; j++)
                    {
                        if (plainText[j] == Second)
                        {
                            NumOfCol = j - idx;
                            break;
                        }
                    }
                    break;//if get NumOfCol exit from outer loop 
                }
            }//outer for

            //get num of rows
            double cols = Convert.ToDouble(NumOfCol);
            double x = (plainText.Length) / cols;
            x = Math.Ceiling(x);
            ElementsPerCol = Convert.ToInt32(x);

            // put element per col of ciphertext in "Cols" List
            for (int i = 0; i < cipherText.Length; i += ElementsPerCol)
            {
                if (cipherText.Length - i >= ElementsPerCol)
                {
                    string Col = cipherText.Substring(i, ElementsPerCol);
                    Cols.Add(Col);
                }
                else
                {
                    check = 1;
                }
            }
            if (check == 1)
                for (int j = 0; j < NumOfCol; j++)
                {
                    key.Add(j);
                }

            //check plaintext to put "x" 
            int k = 0;
            int row = 0;
            int l = 0;
            if (plainText.Length % NumOfCol == 0)
            {
                row = plainText.Length / NumOfCol;
            }
            else
            {
                row = (plainText.Length / NumOfCol) + 1;
                l = (row * NumOfCol) - plainText.Length;
                for (int a = 0; a < l; a++)
                {
                    plainText += 'x';
                }

            }
            char[] cipher = new char[plainText.Length];

            char[,] matrix = new char[row, NumOfCol];
            //put plaintext in matrinx
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < NumOfCol; j++)
                {
                    if (k < plainText.Length)
                    {
                        matrix[i, j] = plainText[k];
                        k += 1;
                    }

                }
            }
            //get new size cipher from matrix
            int c = 0;
            for (int i = 0; i < NumOfCol; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    cipher[c] = matrix[j, i];
                    c++;
                }
            }

            // put element per col of ciphertext in "SotredCols" List (sorted)
            string SortedCipher = new string(cipher);
            List<string> SotredCols = new List<string>();
            for (int i = 0; i < SortedCipher.Length; i += ElementsPerCol)
            {
                if (SortedCipher.Length - i >= ElementsPerCol)
                {
                    string Col = SortedCipher.Substring(i, ElementsPerCol);
                    SotredCols.Add(Col);
                }
                else
                {
                    break;
                }
            }
            //put key element
            if (check == 0)
            {
                for (int i = 0; i < SotredCols.Count; i++)//5cols
                {
                    for (int j = 0; j < Cols.Count; j++)//5cols
                    {
                        if (SotredCols[i] == Cols[j])
                        {
                            key.Add(j + 1);
                        }
                    }
                }
            }
            return key;
            //throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();


            cipherText = cipherText.ToLower();

            string PT = "";
            int cols = key.Count;
            double key_ = Convert.ToDouble(cols);
            double x = (cipherText.Length) / key_;
            x = Math.Ceiling(x);
            int counter = 0;
            int rows = Convert.ToInt32(x);
            bool check = false;
            if (cipherText.Length % 4 != 0)
            {
                check = true;
                String xx = cipherText;
                while (xx.Length % x != 0)
                {
                    xx = xx.Insert(xx.Length - 1, "x");
                    Console.WriteLine(xx.Length);
                    counter++;
                }
            }
            int lenght_ = cipherText.Length;
            int sub_ = 0;
            for (int i = 0; i < counter; i++)
            {

                cipherText = cipherText.Insert(lenght_ - sub_, "x");
                sub_ += 3;

            }

            Console.WriteLine(cipherText);
            char[,] arr = new char[rows, cols];
            for (int i = 0; i < cols; i++)
            {
                dic.Add(key[i], i);
            }
            var sortedDict = from entry in dic orderby entry.Key ascending select entry;
            int[] rightcols = new int[cols];
            int index = 0;
            foreach (KeyValuePair<int, int> d in sortedDict)
            {
                rightcols[index++] = d.Value;
                Console.WriteLine(d.Value);

            }
            int k = 0;

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (k < cipherText.Length)
                        arr[j, rightcols[i]] = cipherText[k++];
                }

            }


            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(arr[i, j]);

                }

                Console.Write("\n");
            }


            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    PT += arr[i, j];

                }


            }




            if (check == true)
                if (PT[PT.Length - 1] == 'x')
                {
                    PT = PT.Remove(PT.Length - 1, 1);
                    for (int i = 0; i < PT.Length; i++)
                    {
                        if (PT[PT.Length - 1] == 'x')
                        {
                            PT = PT.Remove(PT.Length - 1, 1);
                        }
                        else
                            break;

                    }





                }

            return PT;
        }

        public string Encrypt(string plainText, List<int> key)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            plainText = plainText.ToLower();
            int cols = key.Count;
            double key_ = Convert.ToDouble(cols);
            double x = (plainText.Length) / key_;
            x = Math.Ceiling(x);
            int rows = Convert.ToInt32(x);
            char[,] arr = new char[rows, cols];

            string CT = "";
            int k = 0;

            for (int i = 0; i < cols; i++)
            {
                dic.Add(key[i], i);
            }
            var sortedDict = from entry in dic orderby entry.Key ascending select entry;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (k < plainText.Length)
                        arr[i, j] = plainText[k++];
                    else
                    {
                        arr[i, j] = 'x';
                    }

                }

            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(arr[i, j]);

                }

                Console.Write("\n");
            }

            int[] rightcols = new int[cols];
            int index = 0;
            foreach (KeyValuePair<int, int> d in sortedDict)
            {

                rightcols[index++] = d.Value;
                Console.WriteLine(d.Value);

            }

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    CT += arr[j, rightcols[i]];

                }

            }


            return CT;
        }
    }
}
