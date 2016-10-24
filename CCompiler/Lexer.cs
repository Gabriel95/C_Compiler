using System;
using System.Globalization;


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
                        if (char.IsLetter(currentChar) || currentChar.Equals('_'))
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
                        }
                        else if (char.IsWhiteSpace(currentChar))
                        {
                            currentChar = GetCurrentSymbol();
                        }
                        else if (currentChar.Equals('#'))
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                            state = 8;
                        }
                        else if (currentChar == '\'')
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                            state = 10;
                        }
                        break;

                    case 1:
                        if (char.IsLetterOrDigit(currentChar) || currentChar.Equals('_'))
                        {
                            lexeme += currentChar;
                            state = 1;
                            currentChar = GetCurrentSymbol();
                        }
                        else
                        {
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
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
                        else if (currentChar.Equals('u')||currentChar.Equals('U') || currentChar.Equals('l') || currentChar.Equals('L'))
                        {
                            var startedWithU = currentChar.Equals('u') || currentChar.Equals('U');
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                            if (startedWithU)
                            {
                                if (currentChar.Equals('l') || currentChar.Equals('L'))
                                {
                                    lexeme += currentChar;
                                    currentChar = GetCurrentSymbol();
                                }
                            }        
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
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
                        else if (currentChar.Equals('.'))
                        {
                            lexeme += currentChar;
                            state = 9;
                            currentChar = GetCurrentSymbol();
                        }
                        else
                        {
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
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
                            if ((lexeme + currentChar).Equals("->"))
                            {
                                lexeme += currentChar;
                                return new Token()
                                {
                                    Column = currentColumn,
                                    Line = currentLine,
                                    Lexeme = lexeme,
                                    Type = Dictionaries.TwoLengthOperatorDictionary[lexeme]
                                };
                            }
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
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0' )
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
                            state = 2;

                        }
                        break;
                    case 7:
                        if (char.IsDigit(currentChar))
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                        }
                        else if (currentChar.Equals('u') || currentChar.Equals('U') || currentChar.Equals('l') || currentChar.Equals('L'))
                        {
                            var prev_lexeme = lexeme;
                            var startedWithU = currentChar.Equals('u') || currentChar.Equals('U');
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                            if (startedWithU)
                            {
                                if (currentChar.Equals('l') || currentChar.Equals('L'))
                                {
                                    lexeme += currentChar;
                                    currentChar = GetCurrentSymbol();
                                }
                            }
                            if (!prev_lexeme.IsOctal())
                            {
                                throw new Exception("Invalid Octal");
                            }
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0' )
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
                        else
                        {
                            if (!lexeme.IsOctal())
                            {
                                throw new Exception("Invalid Octal");
                            }
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
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
                    case 8:
                        if (char.IsDigit(currentChar) || currentChar.Equals('-'))
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                        }
                        else if (currentChar.Equals('i'))
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                            state = 1;
                        }
                        else
                        {
                            if (currentChar.Equals('#'))
                            {
                                var substring = lexeme.Substring(1);
                                DateTime expectedDate;
                                if (DateTime.TryParseExact(substring, "dd-MM-yyyy", new CultureInfo("en-US"),
                                    DateTimeStyles.None, out expectedDate))
                                {
                                    lexeme += currentChar;
                                    return new Token()
                                    {
                                        Column = currentColumn,
                                        Lexeme = lexeme,
                                        Line = currentLine,
                                        Type = TokenTypes.TYPE_DATE
                                    };
                                }
                                throw new Exception("Incorrect date format");
                            }
                            throw new Exception("Incomplete statement");
                        }
                        break;
                    case 9:
                        if (char.IsDigit(currentChar))
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                        }
                        else
                        {
                            if (lexeme.EndsWith("."))
                            {
                                throw new Exception("Float can't end with \".\"");
                            }
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0' )
                            {
                                _currentPointer--;
                            }
                            return new Token()
                            {
                                Column = currentColumn,
                                Lexeme = lexeme,
                                Line = currentLine,
                                Type = TokenTypes.FLOAT_LITERAL
                            };
                        }
                        break;
                    case 10:
                        if (currentChar == '\'')
                        {
                            var substring = lexeme.Substring(1);
                            lexeme += currentChar;
                            if (Dictionaries.EscapeList.Contains(substring) || lexeme.Length == 3)
                            {
                                return new Token()
                                {
                                    Column = currentColumn,
                                    Lexeme = lexeme,
                                    Line = currentLine,
                                    Type = TokenTypes.CHAR_LITERAL
                                };
                            }
                            throw new Exception("Invalid char");
                        }
                        lexeme += currentChar;
                        currentChar = GetCurrentSymbol();
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
