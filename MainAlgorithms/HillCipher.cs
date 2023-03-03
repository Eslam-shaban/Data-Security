using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {
        public int det(Matrix<double> x) // CREATE DETERMINT
        {
            double c = x[0, 0] * (x[1, 1] * x[2, 2] - x[1, 2] * x[2, 1]) -
                       x[0, 1] * (x[1, 0] * x[2, 2] - x[1, 2] * x[2, 0]) +
                       x[0, 2] * (x[1, 0] * x[2, 1] - x[1, 1] * x[2, 0]);
            int a_i = (int)c % 26 >= 0 ? (int)c % 26 : (int)c % 26 + 26;
            for (int i = 0; i < 26; i++)
            {
                if (a_i * i % 26 == 1)
                {
                    return i;
                }
            }

            return -1;

        }
        public Matrix<double> ModMinorCofactor(Matrix<double> M, int A)
        {
            Matrix<double> Res_mat = DenseMatrix.Create(3, 3, 0.0);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int x = i == 0 ? 1 : 0, y = j == 0 ? 1 : 0, x1 = i == 2 ? 1 : 2, y1 = j == 2 ? 1 : 2;
                    double r = ((M[x, y] * M[x1, y1] - M[x, y1] * M[x1, y]) * Math.Pow(-1, i + j) * A) % 26;
                    Res_mat[i, j] = r >= 0 ? r : r + 26;
                }
            }
            return Res_mat;
        }
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            double palin_cols_ = Math.Ceiling(plainText.Count / 2.0);
            int palin_cols = Convert.ToInt32(palin_cols_);
            int[,] plainText_matrix = new int[2, palin_cols];
            int[,] cipher_matrix = new int[2, palin_cols];
            int k = 0;
            for (int i = 0; i < palin_cols; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    plainText_matrix[j, i] = plainText[k++];
                }

            }
            k = 0;
            for (int i = 0; i < palin_cols; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    cipher_matrix[j, i] = plainText[k++];
                }

            }
            List<int> mayBeKey = new List<int>();
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int kk = 0; kk < 26; kk++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            mayBeKey = new List<int>(new[] { i, j, kk, l });
                            List<int> aa = Encrypt(plainText, mayBeKey);
                            if (aa.SequenceEqual(cipherText))
                            {
                                return mayBeKey;
                            }

                        }
                    }
                }
            }


            throw new InvalidAnlysisException();
        }



        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {//******************************

            // List<int> key1 = new List<int>(key.Count);

            int index = 0;
            int size = Convert.ToInt32(Math.Sqrt(key.Count));
            int[,] keyMatrix = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                int j = 0;
                while (j < size)
                {
                    if ((key[index] <= 26)&&(key[index] >= 0) )
                        keyMatrix[i, j] = key[index++];
                    else if (key[index] > 26)
                    {
                        int idex_1 = key[index];
                        idex_1 %= 26;
                        keyMatrix[i, j] = idex_1;
                        index++;
                    }
                    else
                    {
                        break;
                    }
                    j++;
                }
            }
            double R = Get_Det(size, keyMatrix);
            R %= 26;
            if (R < 0)
                R += 26;

            int great_comm_div = get_gcd((int)R);

            if (great_comm_div != 1)
                throw new Exception();

            if (size == 2)
            {

                float Get_inv = 0;
                float ind1, ind2, ind3, ind4;
                ind1 = (keyMatrix[0, 0]); ind2 = (keyMatrix[0, 1]); ind3 = (keyMatrix[1, 0]); ind4 = (keyMatrix[1, 1]);


                /*
                     1 4    1 4 9 8                  
                     9 8
                     
                     
                 */
                float inv1_1 = ind1; float inv2_2 = ind2; float inv3_3 = ind3; float inv4_4 = ind4;

                Get_inv = 1 / ((ind1 * ind4) - (ind2 * ind3));
                ind1 *= Get_inv;
                ind2 *= Get_inv * -1;
                ind3 *= Get_inv * -1;
                ind4 *= Get_inv;
                key[0] = (int)ind4; key[1] = (int)ind2; key[2] = (int)ind3; key[3] = (int)ind1;

                return Encrypt(cipherText, key);

            }

            double A = 0, B = 0, D = 0;
            D = 26 - R;

            index = 1;
            for (int i = 0; i < cipherText.Count; i++)
            {

                if ((26 * index + 1) % D != 0)
                    index++;
                else
                    break;
            }
            A = (26 * index + 1) / D;


            B = 26 - A;

            int[,] small_mat = new int[size - 1, size - 1];
            double[,] out_matrix = new double[size, size];
            int _1st = 0;
            int _2nd = 0;

         

           int i_4 = 0;
            while ( i_4 < 3)
            {
                for (int j = 0; j < 3; j++)
                {

                    int I = 0, J = 0;
                    int _ind_x = 0;
                    while ( _ind_x < 3 )
                    {
                        for (int ind_y = 0; ind_y < 3; ind_y++)
                        {


                            if (!(_ind_x == i_4 || ind_y == j))
                            {
                                small_mat[I, J] = keyMatrix[_ind_x, ind_y];

                                J++;
                                I += (J / 2);
                                J %= 2;


                            }


                        }
                        _ind_x++;
                    }
                    double Result2 = Get_Det(size - 1, small_mat);

                    double Result3 = (B * (Math.Pow(-1, (i_4 + j)) * Result2) % 26);
                    if (Result3 < 0)
                        Result3 += 26;
                    out_matrix[_2nd, _1st] = Result3;
                    _1st++;
                    if (_1st > 2)
                    {
                        _1st = 0;
                        _2nd++;
                    }

                }
             i_4++;
            }
            // 5 7
            // 9 3
            // 5 9
            // 7 3  5 9 7 3
            // transpose part
            int length1 = out_matrix.GetLength(0);
            int length2 = out_matrix.GetLength(1);

            double[,] res2 = new double[length2, length1];
            int i_3 = 0;
            while ( i_3 < length1 )
            {
                int j_3 = 0;
                while ( j_3 < length2)
                {
                    res2[j_3, i_3] = out_matrix[i_3, j_3];
                   j_3++;
                }
                i_3++;
            }

            out_matrix = res2;
            index = 0;
            int o = 0;
            while (o < size)
            {
                int l = 0;
                while (l < size)
                {
                    key[index] = (int)out_matrix[o, l];
                    index++;
                    l++;
                }
                o++;
            }
            return Encrypt(cipherText, key);


        }//*******************************************

        //betgeb el determinant beta3 el matrix
        public double Get_Det(int m, int[,] keyMatrix)
        {

            int[,] small_mat = new int[m - 1, m - 1];
            double det = 0;
            if (m == 2)
            {
                return ((keyMatrix[0, 0] * keyMatrix[1, 1]) - (keyMatrix[1, 0] * keyMatrix[0, 1]));
            }

            else if (m == 3)
            {
                int k = 0;
                while ( k < m )
                {
                    int index_i_1 = 0;
                    int i = 1;
                    while( i < m )
                    {
                        int index_j_2 = 0;
                        for (int j = 0; j < m; j++)
                        {
                            if (j == k)
                            {
                                continue;
                            }
                            small_mat[index_i_1, index_j_2] = keyMatrix[i, j];
                            index_j_2++;
                        }
                        index_i_1++;
                      i++;
                    }
                    double res = Get_Det(m - 1, small_mat);
                    det = det + (Math.Pow(-1, k) * keyMatrix[0, k] * res);
                    k++;
                }

            }
            return det;
        }

        public double[,] inv(double[,] matrix)
        {
            int len1 = matrix.GetLength(0);  int len2 = matrix.GetLength(1);

            double[,] Result = new double[len2, len1];
            int i = 0;
            while ( i < len1)
            {
                int j = 0;
                while ( j < len2)
                {
                    Result[i, j] = matrix[i, j];
                    j++;
                }
                i++;
            }

            return Result;
        }
        private static int get_gcd(int elem1)
        {
            int elem2 = 26;
            while (elem1 != 0 && elem2 != 0)
            {
                if (elem1 > elem2)
                    elem1 %= elem2;
                else
                    elem2 %= elem1;
            }
            if (elem1 == 0) return elem2;
            else return elem1;
            

        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            double rows_ = Math.Sqrt(key.Count());
            int rows = Convert.ToInt32(rows_);
            double palin_cols_ = Math.Ceiling(plainText.Count / rows_);
            int palin_cols = Convert.ToInt32(palin_cols_);
            int key_cols = rows;
            List<int> CT = new List<int>();
            int[,] plainText_matrix = new int[rows, palin_cols];
            int[,] key_matrix = new int[rows, key_cols];
            int k = 0;
            for (int i = 0; i < palin_cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    plainText_matrix[j, i] = plainText[k++];
                }

            }
            k = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < key_cols; j++)
                {
                    key_matrix[i, j] = key[k++];
                }

            }


            for (int i = 0; i < palin_cols; i++)
            {
                int[] arr = new int[rows];
                int index = 0;
                for (int j = 0; j < rows; j++)
                {
                    arr[index++] = plainText_matrix[j, i];
                }

                for (int j = 0; j < rows; j++)
                {
                    int sum = 0;
                    index = 0;
                    for (int h = 0; h < key_cols; h++)
                    {
                        sum += key_matrix[j, h] * arr[index++];
                    }
                    int x = sum % 26 >= 0 ? sum % 26 : sum % 26 + 26;
                    CT.Add(x);

                }


            }
            return CT;

        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {

            List<double> CD = cipherText.ConvertAll(x => (double)x);
            List<double> PD = plainText.ConvertAll(x => (double)x);
            int M = Convert.ToInt32(Math.Sqrt((CD.Count)));
            // Convert m into 32bits
            Matrix<double> A_Matrix = DenseMatrix.OfColumnMajor(M, (int)cipherText.Count / M, CD.AsEnumerable()); // M IS REPRESENT ROW ,2 RE COLUMNS
            Matrix<double> B_Matrix = DenseMatrix.OfColumnMajor(M, (int)plainText.Count / M, PD.AsEnumerable()); // MATRIX P TAKE M ROWS 
            List<int> maybe_key = new List<int>(); // LIST FOR KEY
            Matrix<double> Z_Matrix = DenseMatrix.Create(3, 3, 0);// create matrix 3rows*3columns and no value in matrix
            B_Matrix = ModMinorCofactor(B_Matrix.Transpose(), det(B_Matrix));
            Z_Matrix = (A_Matrix * B_Matrix);// FILL MATRIK Z OF ELEMENTS 
            maybe_key = Z_Matrix.Transpose().Enumerate().ToList().Select(i => (int)i % 26).ToList();
            maybe_key.ForEach(j => Console.WriteLine(j.ToString()));//convert KEY to string 
            return maybe_key; //
        }

    }
}
