using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
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
                            state = currentChar.Equals('0') ? 5 : 2;
                            currentChar = GetCurrentSymbol();
                        }
                        else if (Dictionaries.SingleLengthOperatorDictionary.ContainsKey(currentChar.ToString()))
                        {
                            lexeme += currentChar;
                            state = 3;
                        }
                        else if (Dictionaries.SymbolDictionary.ContainsKey(currentChar.ToString()))
                        {
                            lexeme += currentChar;
                            state = 4;
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
                            if (!char.IsWhiteSpace(currentChar))
                            {
                                _currentPointer--;
                            }
                            return new Token()
                            {
                                Column = currentColumn,
                                Lexeme = lexeme,
                                Line = currentLine,
                                Type = Dictionaries.KeyWordDictionary.ContainsKey(lexeme) ? Dictionaries.KeyWordDictionary[lexeme] : TokenTypes.ID
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
                            if (!char.IsWhiteSpace(currentChar))
                            {
                                _currentPointer--;
                            }
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

                        if (!currentChar.Equals('~'))
                        {
                            var tempChar = currentChar;
                            currentChar = GetCurrentSymbol();
                            if (currentChar.Equals(tempChar) || currentChar.Equals('='))
                            {
                                var isThreeLength = false;
                                lexeme += currentChar;
                                if (lexeme.Equals("<<") || lexeme.Equals(">>"))
                                {
                                    currentChar = GetCurrentSymbol();
                                    if (currentChar.Equals('='))
                                    {
                                        lexeme += currentChar;
                                        isThreeLength = true;
                                    }
                                }
                                return new Token()
                                {
                                    Column = currentColumn,
                                    Line = currentLine,
                                    Lexeme = lexeme,
                                    Type = isThreeLength ? Dictionaries.ThreeLengthOperatorDictionary[lexeme] : Dictionaries.TwoLengthOperatorDictionary[lexeme]
                                };
                            }
                        }

                        return new Token()
                        {
                            Column = currentColumn,
                            Lexeme = lexeme,
                            Line = currentLine,
                            Type = Dictionaries.SingleLengthOperatorDictionary[lexeme]
                        };
                    case 4:
                        return new Token()
                        {
                            Column = currentColumn,
                            Lexeme = lexeme,
                            Line = currentLine,
                            Type = Dictionaries.SymbolDictionary[lexeme]
                        };
                    case 5:
                        if (currentChar.Equals('x') || currentChar.Equals('X'))
                        {
                            lexeme += currentChar;
                            state = 6;
                            currentChar = GetCurrentSymbol();
                        }
                        else if (currentChar.IsOct())
                        {
                            lexeme += currentChar;
                            state = 7;
                            currentChar = GetCurrentSymbol();
                        }
                        else
                        {
                            if (!char.IsWhiteSpace(currentChar))
                            {
                                _currentPointer--;
                            }
                            return new Token()
                            {
                                Column = currentColumn,
                                Lexeme = lexeme,
                                Line = currentLine,
                                Type = TokenTypes.NUMBER_LITERAL
                            };
                        }
                        break;
                    case 6:
                        if (currentChar.IsHex())
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                        }
                        else 
                        {
                            if (lexeme.Equals("0x") || lexeme.Equals("0X"))
                            {
                                throw new Exception();
                            }

                            if (!char.IsWhiteSpace(currentChar))
                            {
                                _currentPointer--;
                            }

                            return new Token()
                            {
                                Column = currentColumn,
                                Lexeme = lexeme,
                                Line = currentLine,
                                Type = TokenTypes.NUMBER_LITERAL
                            };

                        }
                        break;
                    case 7:
                        if (currentChar.IsOct())
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                        }
                        else
                        {
                            if (!char.IsWhiteSpace(currentChar))
                            {
                                _currentPointer--;
                            }

                            return new Token()
                            {
                                Column = currentColumn,
                                Lexeme = lexeme,
                                Line = currentLine,
                                Type = TokenTypes.NUMBER_LITERAL
                            };
                        }
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
