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
                                Column = _column - lexeme.Length,
                                Line = _line
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
                        else if (currentChar == '\"')
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                            state = 11;
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
                                _column--;
                                _currentPointer--;
                            }else if (currentChar == '\r')
                            {
                                _column--;
                            }
                            return new Token()
                            {
                                Column = _column - lexeme.Length,
                                Lexeme = lexeme,
                                Line = _line,
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
                        else if (currentChar.Equals('.'))
                        {
                            lexeme += currentChar;
                            state = 9;
                            currentChar = GetCurrentSymbol();
                        }
                        else if (currentChar.Equals('e') || currentChar.Equals('E'))
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                            state = 14;
                        }
                        else
                        {
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
                            {
                                _column--;
                                _currentPointer--;
                            }
                            return new Token()
                            {
                                Column = _column - lexeme.Length,
                                Lexeme = lexeme,
                                Line = _line,
                                Type = TokenTypes.NUMBER_LITERAL
                            };
                        }
                        break;
                    case 3:
                        var prevColumn = _column;
                        if (!currentChar.Equals('~'))
                        {
                            var tempChar = currentChar;
                            
                            currentChar = GetCurrentSymbol();
                            if ((lexeme + currentChar).Equals("->"))
                            {
                                lexeme += currentChar;
                                return new Token()
                                {
                                    Column = _column - lexeme.Length,
                                    Line = _line,
                                    Lexeme = lexeme,
                                    Type = Dictionaries.TwoLengthOperatorDictionary[lexeme]
                                };
                            }
                            if ((lexeme + currentChar).Equals("/*"))
                            {
                                state = 13;
                                lexeme += currentChar;
                                currentChar = GetCurrentSymbol();
                                break;
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
                                    else
                                    {
                                        _column--;
                                        _currentPointer--;
                                    }
                                }
                                else if (lexeme.Equals("//"))
                                {
                                    state = 12;
                                    currentChar = GetCurrentSymbol();
                                    break;
                                }
                                if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
                                {
                                    //_currentPointer--;
                                }
                                return new Token()
                                {
                                    Column = _column - lexeme.Length,
                                    Line = _line,
                                    Lexeme = lexeme,
                                    Type =
                                        isThreeLength
                                            ? Dictionaries.ThreeLengthOperatorDictionary[lexeme]
                                            : Dictionaries.TwoLengthOperatorDictionary[lexeme]
                                };
                            }
                        }
                        currentChar = GetCurrentSymbol(); 
                        if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
                        {
                            _column--;
                            _currentPointer--;
                        }

                        return new Token()
                        {
                            Column = prevColumn - lexeme.Length,
                            Lexeme = lexeme,
                            Line = _line,
                            Type = Dictionaries.SingleLengthOperatorDictionary[lexeme]
                        };
                    case 4:
                        return new Token()
                        {
                            Column = _column - lexeme.Length,
                            Lexeme = lexeme,
                            Line = _line,
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
                                _column--;
                                _currentPointer--;
                            }
                            return new Token()
                            {
                                Column = _column - lexeme.Length,
                                Lexeme = lexeme,
                                Line = _line,
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
                                _column--;
                                _currentPointer--;
                                lexeme = lexeme.Substring(0, lexeme.Length - 1);
                                if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
                                {
                                    _column--;
                                    _currentPointer--;
                                }
                                return new Token()
                                {
                                    Column = _column - lexeme.Length,
                                    Lexeme = lexeme,
                                    Line = _line,
                                    Type = TokenTypes.NUMBER_LITERAL
                                };
                            }
                            state = 2;

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
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
                            {
                                _column--;
                                _currentPointer--;
                            }

                            return new Token()
                            {
                                Column = _column - lexeme.Length,
                                Lexeme = lexeme,
                                Line = _line,
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
                                        Column = _column - lexeme.Length,
                                        Lexeme = lexeme,
                                        Line = _line,
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
                        else if (currentChar == 'f' || currentChar == 'F')
                        {
                            lexeme += currentChar;
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
                            {
                                _column--;
                                _currentPointer--;
                            }
                            return new Token()
                            {
                                Column = _column - lexeme.Length,
                                Lexeme = lexeme,
                                Line = _line,
                                Type = TokenTypes.FLOAT_LITERAL
                            };
                        }
                        else
                        {
                            if (lexeme.EndsWith("."))
                            {
                                _column--;
                                _currentPointer--;
                                lexeme = lexeme.Substring(0, lexeme.Length - 1);
                                if (currentChar != '\0')
                                {
                                    _column--;
                                    _currentPointer--;
                                }
                                return new Token()
                                {
                                    Column = _column - lexeme.Length,
                                    Lexeme = lexeme,
                                    Line = _line,
                                    Type = TokenTypes.NUMBER_LITERAL
                                };
                            }
                            if (currentChar.Equals('E') || currentChar.Equals('e'))
                            {
                                lexeme += currentChar;
                                currentChar = GetCurrentSymbol();
                                state = 14;
                                break;
                            }
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
                            {
                                _column--;
                                _currentPointer--;
                            }
                            return new Token()
                            {
                                Column = _column - lexeme.Length,
                                Lexeme = lexeme,
                                Line = _line,
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
                                    Column = _column - lexeme.Length,
                                    Lexeme = lexeme,
                                    Line = _line,
                                    Type = TokenTypes.CHAR_LITERAL
                                };
                            }
                            throw new Exception("Invalid char");
                        }
                        lexeme += currentChar;
                        currentChar = GetCurrentSymbol();
                        break;
                    case 11:
                        if (currentChar == '\0')
                        {
                            throw new Exception("Unclosed \"");
                        }
                        if (currentChar == '"')
                        {
                            if (lexeme.Contains("\\r\\n") || lexeme.Contains("\r\n"))
                            {
                                throw new Exception("String contains enter " + lexeme);
                            }
                            lexeme += currentChar;
                            return new Token()
                            {
                                Column = _column - lexeme.Length,
                                Lexeme = lexeme,
                                Line = _line,
                                Type = TokenTypes.STRING_LITERAL
                            };

                        }
                        if (currentChar.Equals('\\'))
                        {
                            var temp = currentChar.ToString();
                            currentChar = GetCurrentSymbol();
                            temp += currentChar;
                            currentChar = GetCurrentSymbol();
                            lexeme += temp;
                            if (lexeme.Contains("\\r\\n") || lexeme.Contains("\r\n"))
                            {
                                throw new Exception("String contains enter");
                            }
                            if (!Dictionaries.EscapeList.Contains(temp))
                            {
                                throw new Exception("Invalid escape char");
                            }
                        }
                        else
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                        }
                        break;
                    case 12:
                        var accum = "";
                        while (!accum.EndsWith("\\r\\n") && !accum.EndsWith("\r\n") && currentChar != '\0')
                        {
                            accum += currentChar;
                            currentChar = GetCurrentSymbol();
                        }
                        if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
                        {
                            _column--;
                            _currentPointer--;
                        }
                        return new Token()
                        {
                            Column = _column - lexeme.Length,
                            Lexeme = lexeme,
                            Line = _line,
                            Type = TokenTypes.TK_COMMENT_LINE
                        };
                    case 13:
                        var accum2 = "";
                        while (!accum2.EndsWith("*/") && currentChar != '\0')
                        {
                            accum2 += currentChar;
                            currentChar = GetCurrentSymbol();
                        }
                        lexeme += "*/";
                        return new Token()
                        {
                            Column = _column - lexeme.Length,
                            Lexeme = lexeme,
                            Line = _line,
                            Type = TokenTypes.TK_COMMENT_BLOCK
                        };
                    case 14:
                        if (currentChar.Equals('-') || char.IsDigit(currentChar))
                        {
                            var temp = currentChar;
                            currentChar = GetCurrentSymbol();
                            if (temp.Equals('-')&&!char.IsDigit(currentChar))
                            {
                                _currentPointer-=2;

                                lexeme = lexeme.Substring(0, lexeme.Length - 1);
                                if (currentChar != '\0')
                                {
                                    _column--;
                                    _currentPointer--;
                                }
                                return new Token()
                                {
                                    Column = _column - lexeme.Length,
                                    Lexeme = lexeme,
                                    Line = _line,
                                    Type = lexeme.Contains(".") ? TokenTypes.FLOAT_LITERAL : TokenTypes.NUMBER_LITERAL
                                };
                            }
                            lexeme += temp;
                            state = 15;
                        }
                        else
                        {
                            _column--;
                            _currentPointer--;
                            lexeme = lexeme.Substring(0, lexeme.Length - 1);
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
                            {
                                _column--;
                                _currentPointer--;
                            }
                            return new Token()
                            {
                                Column = _column - lexeme.Length,
                                Lexeme = lexeme,
                                Line = _line,
                                Type = TokenTypes.FLOAT_LITERAL
                            };
                        }
                        break;
                    case 15:
                        if (char.IsDigit(currentChar))
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                        }
                        else if (currentChar == 'f' || currentChar == 'F')
                        {
                            lexeme += currentChar;
                            currentChar = GetCurrentSymbol();
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
                            {
                                _column--;
                                _currentPointer--;
                            }
                            return new Token()
                            {
                                Column = _column - lexeme.Length,
                                Lexeme = lexeme,
                                Line = _line,
                                Type = TokenTypes.FLOAT_LITERAL
                            };
                        }
                        else
                        {
                            if (!char.IsWhiteSpace(currentChar) && currentChar != '\0')
                            {
                                _column--;
                                _currentPointer--;
                            }
                            return new Token()
                            {
                                Column = _column - lexeme.Length,
                                Lexeme = lexeme,
                                Line = _line,
                                Type = TokenTypes.FLOAT_LITERAL
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
