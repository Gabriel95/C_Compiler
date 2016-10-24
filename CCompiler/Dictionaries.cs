using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCompiler
{
    public static class Dictionaries
    {
        public static readonly Dictionary<string, TokenTypes> SymbolDictionary = new Dictionary<string, TokenTypes>
        {
            {"(",TokenTypes.TK_OPEN_PARENTHESIS},
            {")",TokenTypes.TK_CLOSE_PARENTHESIS},
            {"{",TokenTypes.TK_OPEN_BRACE},
            {"}",TokenTypes.TK_CLOSE_BRACE},
            {"[",TokenTypes.TK_OPEN_BRACKET},
            {"]",TokenTypes.TK_CLOSE_BRACKET},
            {",",TokenTypes.TK_COMMA},
            {".",TokenTypes.TK_PERIOD},
            {";",TokenTypes.TK_SEMICOLON},
            {":",TokenTypes.TK_COLON}
        };

        public static readonly Dictionary<string, TokenTypes> KeyWordDictionary = new Dictionary<string, TokenTypes>
        {
            {"print",TokenTypes.KW_PRINT} ,
            {"scan",  TokenTypes.KW_SCAN},
            {"int", TokenTypes.TYPE_INT},
            {"float", TokenTypes.TYPE_FLOAT},
            {"char", TokenTypes.TYPE_CHAR},
            {"bool", TokenTypes.TYPE_BOOL},
            {"string", TokenTypes.TYPE_STRING},
            {"date", TokenTypes.TYPE_DATE},
            {"enum", TokenTypes.TYPE_ENUM},
            {"struct", TokenTypes.TYPE_STRUCT},
            {"if", TokenTypes.KW_IF},
            {"while", TokenTypes.KW_WHILE},
            {"do", TokenTypes.KW_DO},
            {"for", TokenTypes.KW_FOR},
            {"switch", TokenTypes.KW_SWITCH},
            {"foreach", TokenTypes.KW_FOREACH},
            {"break", TokenTypes.KW_BREAK},
            {"continue", TokenTypes.KW_CONTINUE},
            {"void", TokenTypes.KW_VOID},
            {"else",TokenTypes.KW_ELSE},
            {"false", TokenTypes.KW_FALSE},
            {"true",TokenTypes.KW_TRUE},
            {"#include",TokenTypes.TK_INCLUDE}
        };

        public static readonly Dictionary<string, TokenTypes> SingleLengthOperatorDictionary = new Dictionary
            <string, TokenTypes>()
        {
            {"+", TokenTypes.AOP_SUM},
            {"-", TokenTypes.AOP_SUB},
            {"*", TokenTypes.AOP_MUL},
            {"/", TokenTypes.AOP_DIV},
            {"%", TokenTypes.AOP_MOD},
            {"=", TokenTypes.ASOP_EQUAL},
            {"<", TokenTypes.ROP_LESS_THAN},
            {">", TokenTypes.ROP_GREATER_THAN},
            {"|", TokenTypes.BOP_OR},
            {"~", TokenTypes.BOP_COMPLIMENT},
            {"^", TokenTypes.BOP_XOR},
            {"&", TokenTypes.BOP_AND},
        };

        public static readonly Dictionary<string, TokenTypes> TwoLengthOperatorDictionary = new Dictionary<string, TokenTypes>()
        {
            {"==",TokenTypes.ROP_EQUAL},
            {"!=",TokenTypes.ROP_NOT_EQUAL},
            {">=",TokenTypes.ROP_GREATER_EQUAL},
            {"<=",TokenTypes.ROP_LESS_EQUAL},
            {"&&",TokenTypes.LOP_AND},
            {"||",TokenTypes.LOP_OR},
            {"<<",TokenTypes.BOP_LEFT_SHIFT},
            {">>",TokenTypes.BOP_RIGHT_SHIFT},
            {"+=",TokenTypes.ASOP_SUM},
            {"-=",TokenTypes.ASOP_SUB},
            {"*=",TokenTypes.ASOP_MUL},
            {"/=",TokenTypes.ASOP_DIV},
            {"%=",TokenTypes.ASOP_MOD},
            {"&=",TokenTypes.ASOP_AND},
            {"^=",TokenTypes.ASOP_XOR},
            {"|=",TokenTypes.ASOP_OR},
            {"++",TokenTypes.AOP_INCREMENT},
            {"--",TokenTypes.AOP_DECREMENT},
        };

        public static readonly Dictionary<string, TokenTypes> ThreeLengthOperatorDictionary = new Dictionary <string, TokenTypes>()
        {
            {"<<=",TokenTypes.ASOP_LEFT_SHIFT},
            {">>=",TokenTypes.ASOP_RIGHT_SHIFT}
        };

        public static List<string> EscapeList = new List<string>()
        {
            "\\\\",
            "\\\'",
            "\\\"",
            "\\?",
            "\\a",
            "\\b",
            "\\f",
            "\\n",
            "\\r",
            "\\t",
            "\\v"
        }; 
    }
}
