using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCompiler
{
    public static class KeyWordDictionary
    {
        public static readonly Dictionary<string,TokenTypes> KwDictionary = new Dictionary<string, TokenTypes>
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
        };
    }
}
