﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CCompiler
{
    class Lexer
    {
        private readonly string _codeContent;
        private int _currentPointer;
        private int _line;
        private int _column;
        public Lexer(string codeContent)
        {
            _codeContent = codeContent;
            _currentPointer = 0;
            _line = 0;
            _column = 0;
        }

        public Token GetNextToken()
        {
            var currentLine = _line;
            var currentColumn = _column;
            var currentChar = GetCurrentSymbol();
            var lexeme = string.Empty;
            var state = 0;
            while (true)
            {
                switch (state)
                {
                    case 0:
                        if (char.IsLetter(currentChar))
                        {
                            lexeme += currentChar;
                            state = 1;
                            currentChar = GetCurrentSymbol();
                        }
                        else if (char.IsDigit(currentChar))
                        {
                            lexeme += currentChar;
                            state = 2;
                            currentChar = GetCurrentSymbol();
                        }
                        else if (SymbolDictionary.SDictionary.ContainsKey(currentChar.ToString()))
                        {
                            lexeme += currentChar;
                            state = 3;
                            currentChar = GetCurrentSymbol();
                        }
                        else if (currentChar == '\0')
                        {
                            return new Token
                            {
                                Lexeme = lexeme,
                                Type = TokenTypes.EOF,
                                Column = currentColumn,
                                Line = currentLine
                            };
                        }else if (char.IsWhiteSpace(currentChar))
                        {
                            currentChar = GetCurrentSymbol();
                        }
                        break;

                    case 1:
                        if (char.IsLetterOrDigit(currentChar))
                        {
                            lexeme += currentChar;
                            state = 1;
                            currentChar = GetCurrentSymbol();
                        }
                        else
                        {
                            return new Token()
                            {
                                Column = currentColumn,
                                Lexeme = lexeme,
                                Line = currentLine,
                                Type = KeyWordDictionary.KwDictionary.ContainsKey(lexeme) ? KeyWordDictionary.KwDictionary[lexeme] : TokenTypes.ID
                            };
                        }
                        break;
                    case 2:
                        if (char.IsDigit(currentChar))
                        {
                            lexeme += currentChar;
                            state = 2;
                            currentChar = GetCurrentSymbol();
                        }
                        else
                        {
                            return new Token()
                            {
                                Column = currentColumn,
                                Lexeme = lexeme,
                                Line = currentLine,
                                Type = TokenTypes.NUMBER_LITERAL
                            };
                        }
                        break;
                    case 3:
                        return new Token()
                        {
                            Column = currentColumn,
                            Lexeme = lexeme,
                            Line = currentLine,
                            Type = SymbolDictionary.SDictionary[lexeme]
                        };
                        break;
                }
            }
        }

        private char GetCurrentSymbol()
        {
            if (_currentPointer < _codeContent.Length)
            {
                var current = _codeContent[_currentPointer++];
                if (current.Equals('\n'))
                {
                    _line++;
                    _column = 0;
                }
                else
                {
                    _column++;
                }
                return current;
            }
            return '\0';
        }
    }
}
