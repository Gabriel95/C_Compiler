using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
//            var lex = new Lexer(@"#include<iostream>
//#include<math.h>
//using namespace std;
//int main()
//{
//    int startNum,endNum;
//    int found=0,count=0;
//    float = 5E-5F;
//    cout<<""Enter Number START of Range:  "";
//    cin>>startNum;
//    cout<<""Enter Number END of Range:  "";
//    cin>>endNum;
//    for(int i=startNum;i<=endNum;i++)
//       {
//           for(int j=2;j<=sqrt(i);j++)
//               {
//               if(i%j==0)
//                  count++;
//               }
//               if(count==0&&i!=1)
//               { found++;
//                 cout<<""Prime Number -> ""<<i<<endl;
//                 count=0;
//               }
//               count=0;
//       }

// cout<<""Total Prime Number Between Range ""<<startNum<<"" to ""<<endNum<<"" = ""<<found<<endl;
// return 1;
//}");
            var lex = new Lexer("12a");
            var currentToken = lex.GetNextToken();
            while (currentToken.Type != TokenTypes.EOF)
            {
                Console.WriteLine(currentToken.ToString());
                try
                {
                    currentToken = lex.GetNextToken();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                }
                
            }
            Console.ReadKey();
        }
    }
}
