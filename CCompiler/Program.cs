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
            var lex = new Lexer(@"int main () {
                                                int ar[] = {0,1,2}; 
                                                int counter = 01L;
                                                float f = 10000.69;
                                                date d = #09-02-2016#;
                                                char ch = '$';
                                                counter->shazam;
                                                if(counter < 69 && counter > 1){
                                                    counter <<= 1;
                                                }else{
                                                    counter = 1;
                                                }
            forreasons
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
