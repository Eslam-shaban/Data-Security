using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographicTechnique<string, string>
    {
        /// <summary>
        /// The most common diagrams in english (sorted): TH, HE, AN, IN, ER, ON, RE, ED, ND, HA, AT, EN, ES, OF, NT, EA, TI, TO, IO, LE, IS, OU, AR, AS, DE, RT, VE
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public string Analyse(string plainText)
        {
            throw new NotImplementedException();
        }

        public string Analyse(string plainText, string cipherText)
        {
            throw new NotSupportedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            Console.WriteLine("Anton");
            cipherText = cipherText.ToLower();
            cipherText = cipherText.Replace('j', 'i');
            key = key.ToLower();
            string chars = "abcdefghijklmnopqrstuvwxyz";
            string x = key + chars;
            string PT = "";
            x = x.Replace('j', 'i');
            string xx = new string(x.Distinct().ToArray());
            char[,] arr = new char[5, 5];
            int j = 0;
            //matrix of key
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    arr[row, col] = xx[j];
                    j++;
                }
            }
            // encypt

            for (int i = 0; i < cipherText.Length - 1; i += 2)
            {
                int frow = 0, fcol = 0, srow = 0, scol = 0;
                int count = 0;
               

                for (int row = 0; row < 5; row++)
                {
                    for (int col = 0; col < 5; col++)
                    {
                        if (cipherText[i] == arr[row, col])
                        {
                            frow = row;
                            fcol = col;
                            count++;
                        }
                        else if (cipherText[i + 1] == arr[row, col])
                        {
                            srow = row;
                            scol = col;
                            count++;
                        }

                    }
                    if (count == 2)
                    {
                        break;
                    }
                }

                if (frow == srow)
                {
                    int g = 0, y = 0;
                    if (fcol == 0)
                    {
                        fcol = 4;
                        PT += arr[frow, fcol];
                        g = 1;
                    }
                    if (g == 0)
                        PT += arr[frow, (fcol - 1)];
                    if (scol == 0)
                    {
                        scol = 4;
                        PT += arr[srow, scol];
                        y = 1;
                    }

                    if (y == 0)
                        PT += arr[srow, (scol - 1)];
                }


                else if (fcol == scol)
                {
                    int g = 0, y = 0;
                    if (frow == 0)
                    {
                        frow = 4;
                        PT += arr[frow, fcol];
                        g = 1;

                    }
                    if (g == 0)
                        PT += arr[(frow - 1), fcol];

                    if (srow == 0)
                    {
                        srow = 4;
                        PT += arr[srow, scol];
                        y = 1;
                    }

                    if (y == 0)
                        PT += arr[(srow - 1), scol];
                }

                else
                {
                    PT += arr[frow, scol];
                    PT += arr[srow, fcol];
                }
            }
            string answer = PT;
            int counter = 0;
            for (int i = 0; i < PT.Length-1;i++ )
                if (PT[i] == 'x'&&(i+1)%2==0)
                {
                    if (PT[i - 1] == PT[i + 1])
                    {
                        answer = answer.Remove(i - counter, 1);
                        counter++;
                    }

                }

            if (answer[answer.Length - 1] == 'x')
            {
                answer = answer.Remove(answer.Length - 1, 1);
            }
            Console.WriteLine(answer);
            return answer;
        }


        public string Encrypt(string plainText, string key)
        {

            plainText = plainText.ToLower();
            key = key.ToLower();
            string chars = "abcdefghijklmnopqrstuvwxyz";
            string x = key + chars;
            string cT="";
           x= x.Replace('j','i');
          string xx=new string (x.Distinct().ToArray());
            char [,] arr=new char[5,5];
            int j = 0;
            //matrix of key
            for(int row=0;row<5;row++)
            {
                for(int col=0;col<5;col++)
                {
                    arr[row,col]=xx[j];
                    j++;
                }
            }
            // encypt
    
          for(int i=0;i<plainText.Length-1;i+=2)
          {
              if (plainText[i] == plainText[i + 1])
              {
                  plainText = plainText.Insert(i + 1, "x");
              }

          }
          if (plainText.Length % 2 != 0)
          {
              plainText = plainText + 'x';
          }




          for (int i = 0; i < plainText.Length - 1; i += 2)
          {
              int frow=0, fcol=0, srow=0, scol = 0;
              int count = 0;

              for (int row = 0; row < 5; row++)
              {
                  for (int col = 0; col < 5; col++)
                  {
                      if (plainText[i] == arr[row, col])
                      {
                          frow = row;
                          fcol = col;
                          count++;
                      }
                      else if (plainText[i+1] == arr[row, col])
                      {
                          srow = row;
                          scol = col;
                          count++;
                      }
                      
                  }
                  if (count == 2)
                  {
                      break;
                  }
              }

              if (frow == srow)
              {
                  cT += arr[frow, ((fcol + 1) % 5)];
                  cT += arr[srow, ((scol + 1) % 5)];
              }
              else if (fcol == scol)
              {
                  cT += arr[((frow + 1) % 5), fcol];
                  cT += arr[((srow + 1) % 5), scol];
              }
              else
              {
                  cT += arr[frow, scol];
                  cT += arr[srow, fcol];
              }


          }

          return cT;
         }
       
    }
}
