using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCompiler
{
    public static class SymbolDictionary
    {
        public static readonly Dictionary<string,TokenTypes> SDictionary = new Dictionary<string, TokenTypes>
        {
            {"+",TokenTypes.AOP_SUM},
            {"-",TokenTypes.AOP_SUB},
            {"*",TokenTypes.AOP_MUL},
            {"/",TokenTypes.AOP_DIV},
            {"%",TokenTypes.AOP_MOD},
            {"=",TokenTypes.ASOP_EQUAL},
            {"<",TokenTypes.ROP_LESS_THAN},
            {">",TokenTypes.ROP_GREATER_THAN},
            {"|",TokenTypes.BOP_OR},
            {"~",TokenTypes.BOP_COMPLIMENT},
            {"^",TokenTypes.BOP_XOR},
            {"&",TokenTypes.BOP_AND},
            {"(",TokenTypes.TK_OPEN_PARENTHESIS},
            {")",TokenTypes.TK_CLOSE_PARENTHESIS},
            {"{",TokenTypes.TK_OPEN_BRACE},
            {"}",TokenTypes.TK_CLOSE_BRACE},
            {"[",TokenTypes.TK_OPEN_BRACKET},
            {"]",TokenTypes.TK_CLOSE_BRACKET},
            {",",TokenTypes.TK_COMMA},
            {".",TokenTypes.TK_PERIOD},
            {";",TokenTypes.TK_SEMICOLON},
        };
    }
}
