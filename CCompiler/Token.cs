using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCompiler
{
    class Token
    {
        public string Lexeme { get; set; }
        public TokenTypes Type { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public override string ToString()
        {
            return "Lexeme: " + Lexeme + " Type: " + Type + " Line: " + Line + " Column: " + Column;
        }
    }
}
