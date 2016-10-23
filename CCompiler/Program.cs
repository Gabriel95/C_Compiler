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
            //var lex = new Lexer(@"float shazam = 69;
            //                        cont = cont + 1;
            //cont2 = 0;");
            var lex = new Lexer(@"int main () {
                                    int ar[] = {0,1,2}; 
                                    int counter = 0x;
                                    counter--;
                                    if(counter < 69 && counter > 1){
                                        counter <<= 1;
                                    }else{
                                        counter = 1;
                                    }
                                    return 0;
                                    }");
            var currentToken = lex.GetNextToken();
            while (currentToken.Type != TokenTypes.EOF)
            {
                Console.WriteLine(currentToken.ToString());
                currentToken = lex.GetNextToken();
            }
            Console.ReadKey();
        }
    }
}
