#region license
// Copyright (C) 2013 Mikael Lyngvig (mikael@lyngvig.org).  All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following
// conditions are met:
//
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the disclaimer below.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following
//       disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of Mikael Lyngvig nor the names of its contributors may be used to endorse or promote products derived
//       from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,
// BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT
// SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

/** \file
 *  The GCL scanner.
 */

using System.Collections.Generic;       // Dictionary<T1, T2>
using System.IO;                        // TextReader
using System.Text;                      // StringBuilder

using Bacchi.Kernel;                    // Error, Position

namespace Bacchi.Reader
{
    /** The Bacchi scanner, which consumes an entire input file and returns a complete array of tokens. */
    public class Scanner
    {
        /** We blatantly assume that each TAB equals four spaces (the width of tabs in my editor). */
        const int TABSIZE = 4;

        const char EOF = '\uFFFF';

        /** The entire source text is read into this variable. */
        private string        _source;
        /** The offset of the next character to process in \c _source. */
        private int           _offset;

        /** The current position in the input file. */
        private Position      _cursor;
        /** The current look-ahead character (the next character in the input file). */
        private char          _lookahead;

        /** The currently buffered token text, if any, of strings, integers, etc. */
        private StringBuilder _text;

        private List<Token>   _tokens = new List<Token>();
        /** Returns the entire array of scanned tokens. */
        public Token[] Tokens
        {
            get { return _tokens.ToArray(); }
        }

        /** The list of all known keywords supported by the scanner. */
        private Dictionary<string, TokenKind> _keywords = new Dictionary<string, TokenKind>()
        {
            { "array",   TokenKind.Keyword_Array   },
            { "begin",   TokenKind.Keyword_Begin   },
            { "Boolean", TokenKind.Keyword_Boolean },
            { "const",   TokenKind.Keyword_Const   },
            { "do",      TokenKind.Keyword_Do      },
            { "end",     TokenKind.Keyword_End     },
            { "false",   TokenKind.Keyword_False   },
            { "fi",      TokenKind.Keyword_Fi      },
            { "forall",  TokenKind.Keyword_Forall  },
            { "if",      TokenKind.Keyword_If      },
            { "integer", TokenKind.Keyword_Integer },
            { "llarof",  TokenKind.Keyword_Llarof  },
            { "module",  TokenKind.Keyword_Module  },
            { "od",      TokenKind.Keyword_Od      },
            { "private", TokenKind.Keyword_Private },
            { "proc",    TokenKind.Keyword_Proc    },
            { "range",   TokenKind.Keyword_Range   },
            { "read",    TokenKind.Keyword_Read    },
            { "ref",     TokenKind.Keyword_Ref     },
            { "return",  TokenKind.Keyword_Return  },
            { "skip",    TokenKind.Keyword_Skip    },
            { "true",    TokenKind.Keyword_True    },
            { "typedef", TokenKind.Keyword_Typedef },
            { "val",     TokenKind.Keyword_Val     },
            { "write",   TokenKind.Keyword_Write   }
        };

        /** Constructor for the \c Scanner class. */
        public Scanner(string filename)
        {
            _source = System.IO.File.ReadAllText(filename);
            _offset = 0;
            _cursor = new Position(filename, 1, 1);
            _text   = new StringBuilder(256);

            // Grab the first input character (initializes _lookahead!).
            ReadChar();

            while (ProduceToken())
            {
            }
        }

        /** Reads a single character, expanding tabs and maintaining the _cursor member variable. */
        private char ReadChar()
        {
            if (_offset == _source.Length)
                return EOF;

            char result = _lookahead;
            _lookahead  = _source[_offset++];

            // Silently discard the carriage-return present on Winblows platforms.
            if (result == '\r' && _lookahead == '\n')
                ReadChar();

            _cursor.NextChar();
            if (result == '\n')
                _cursor.NextLine();
            else if (result == '\t')
            {
                // Expand tabs inline so as to get the correct cursor position for error messages in lines that include tabs.
                int width = TABSIZE - (_cursor.Char % TABSIZE) + 1;
                for (int i = 0; i < width; i+= 1)
                    _cursor.NextChar();
            }

            return result;
        }

        /** Returns \c true if the specified character is an ASCII letter. */
        private bool IsLetter(char value)
        {
            return (value >= 'a' && value <= 'z') || (value >= 'A' && value <= 'Z');
        }

        /** Returns \c true if the specified character is a decimal digit. */
        private bool IsDigit(char value)
        {
            return (value >= '0' && value <= '9');
        }

        /** Returns \c true if the specified character is a valid GCL identifier character. */
        private bool IsName(char value)
        {
            return IsLetter(value) || IsDigit(value) || value == '_';
        }

        /** Checks if a name is valid according to the Ada-ish requirements of GCL. */
        private bool ValidName(string name)
        {
            if (name.Length == 0)
                return false;

            if (name[0] == '_')
                return false;

            bool underscore = false;
            foreach (char ch in name)
            {
                if (ch == '_')
                {
                    if (underscore)
                        return false;
                    underscore = true;
                }
                else
                    underscore = false;
            }

            if (underscore)
                return false;

            return true;
        }

        /** Produces a single token from the string buffer \c _source. */
        public bool ProduceToken()
        {
            bool result = true;         // Assume there are more tokens in the input file.

            Token token = new Token(_cursor);
            _text.Length = 0;           // Reset the global token string buffer.

            char ch = ReadChar();
            switch (ch)
            {
                case ' ':
                case '\t':
                    // Discard embedded spaces and tabs.
                    while (_lookahead == ' ' || _lookahead == '\t')
                        ReadChar();
                    return ProduceToken();

                case '-':
                    switch (_lookahead)
                    {
                        case '-':       // Comment
                            ReadChar(); // Drop second '-'.
                            while (_lookahead != '\n' && _lookahead != EOF)
                                _text.Append(ReadChar());
                            token.Kind = TokenKind.Comment;
                            token.Text = _text.ToString();
                            return ProduceToken();

                        case '>':       // -> symbol
                            ReadChar(); // Drop '>'.
                            token.Kind = TokenKind.Symbol_Then;
                            break;

                        default:        // minus operator
                            token.Kind = TokenKind.Operator_Subtract;
                            break;
                    }
                    break;

                case EOF:
                    token.Kind = TokenKind.EndOfFile;
                    result = false;
                    break;

                case '\n':
                    return ProduceToken();

                case '.':
                    if (_lookahead == '.')
                    {
                        ReadChar();
                        token.Kind = TokenKind.Symbol_Ellipsis;
                        break;
                    }

                    token.Kind = TokenKind.Symbol_Dot;
                    break;

                case '&':
                    token.Kind = TokenKind.Operator_And;
                    break;

                case '/':
                    token.Kind = TokenKind.Operator_Divide;
                    break;

                case ':':
                    if (_lookahead != '=')
                        throw new Error(token.Position, 0, "Invalid character: " + ch);

                    ReadChar();
                    token.Kind = TokenKind.Symbol_Assignment;
                    break;

                case '*':
                    token.Kind = TokenKind.Operator_Multiply;
                    break;

                case '|':
                    token.Kind = TokenKind.Operator_Or;
                    break;

                case '~':
                    token.Kind = TokenKind.Operator_Not;
                    break;

                case '+':
                    token.Kind = TokenKind.Operator_Add;
                    break;

                case '\'':
                case '\"':
                    token.Kind = TokenKind.String;
                    while (_lookahead != ch)
                    {
                        switch (_lookahead)
                        {
                            case '\n':
                                throw new Error(token.Position, 0, "End of line in string literal");

                            case EOF:
                                throw new Error(token.Position, 0, "End of file in string literal");

                            default:
                                _text.Append(_lookahead);
                                ReadChar();
                                break;
                        }
                    }
                    token.Kind = TokenKind.String;
                    token.Text = _text.ToString();
                    break;

                case '\\':
                    token.Kind = TokenKind.Operator_Remainder;
                    break;

                case ',':
                    token.Kind = TokenKind.Symbol_Comma;
                    break;

                case ';':
                    token.Kind = TokenKind.Symbol_Semicolon;
                    break;

                case '(':
                    token.Kind = TokenKind.Symbol_ParenthesisBegin;
                    break;

                case ')':
                    token.Kind = TokenKind.Symbol_ParenthesisClose;
                    break;

                case '<':
                    if (_lookahead == '=')
                    {
                        ReadChar();
                        token.Kind = TokenKind.Relational_LessEqual;
                        break;
                    }

                    token.Kind = TokenKind.Relational_LessThan;
                    token.Text = "<";
                    break;

                case '>':
                    if (_lookahead == '=')
                    {
                        ReadChar();
                        token.Kind = TokenKind.Relational_GreaterEqual;
                        break;
                    }

                    token.Kind = TokenKind.Relational_GreaterThan;
                    break;

                case '[':
                    if (_lookahead == ']')
                    {
                        ReadChar();
                        token.Kind = TokenKind.Symbol_Brackets;
                        break;
                    }

                    token.Kind = TokenKind.Symbol_BracketBegin;
                    break;

                case ']':
                    token.Kind = TokenKind.Symbol_BracketClose;
                    break;

                case '=':
                    token.Kind = TokenKind.Relational_Equality;
                    break;

                case '#':
                    token.Kind = TokenKind.Relational_Difference;
                    break;

                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    _text.Append(ch);
                    while (IsDigit(_lookahead))
                        _text.Append(ReadChar());

                    token.Text = _text.ToString();
                    token.Kind = TokenKind.Integer;
                    break;

                default:
                {
                    // scan the first character (must be a letter)
                    if (!IsLetter(ch))
                        throw new Error(token.Position, 0, "Invalid character: " + ch);
                    _text.Append(ch);

                    // scan the name
                    while (IsName(_lookahead))
                        _text.Append(ReadChar());

                    // check that the name is welformed
                    token.Text = _text.ToString();
                    if (!ValidName(token.Text))
                        throw new Error(token.Position, 0, "Malformed name '" + token.Text + "'");

                    // try to look up keyword and retrieve its numerical identifier
                    TokenKind kind;
                    if (_keywords.TryGetValue(token.Text, out kind))
                        token.Kind = kind;
                    else
                        token.Kind = TokenKind.Identifier;

                    break;
                }
            }

            _tokens.Add(token);
            return result;
        }
    }
}

