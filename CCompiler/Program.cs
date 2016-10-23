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
            var lex = new Lexer(" int main () {\n int ar[] = {0,1,2};\n return 0;\n}");
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
