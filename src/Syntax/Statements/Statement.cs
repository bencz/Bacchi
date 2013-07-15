#region license
// Copyright (C) 2013 Mikael Lyngvig (mikael@lyngvig.org).  All rights reserved.
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
 *  Defines the \c Statement base class, which represents a single statement (the derived classes implement the actual statements).
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public abstract class Statement: Node
    {
        public Statement(NodeKind kind, Position position):
            base(kind, position)
        {
        }

        public static Statement Parse(Tokens tokens)
        {
            switch (tokens.Peek.Kind)
            {
                case TokenKind.Keyword_Do:
                    return DoStatement.Parse(tokens);

                case TokenKind.Keyword_Forall:
                    return ForallStatement.Parse(tokens);

                case TokenKind.Keyword_If:
                    return IfStatement.Parse(tokens);

                case TokenKind.Keyword_Read:
                    return ReadStatement.Parse(tokens);

                case TokenKind.Keyword_Return:
                    return ReturnStatement.Parse(tokens);

                case TokenKind.Keyword_Skip:
                    return SkipStatement.Parse(tokens);

                case TokenKind.Keyword_Write:
                    return WriteStatement.Parse(tokens);

                default:
                    if (tokens[1].Kind == TokenKind.Symbol_ParenthesisBegin)
                        return CallStatement.Parse(tokens);

                    return LetStatement.Parse(tokens);
            }
        }

        public static Statement[] ParseList(Tokens tokens, TokenKind terminator1, TokenKind terminator2)
        {
            var statements = new List<Statement>();
            while (tokens.Peek.Kind != terminator1 && tokens.Peek.Kind != terminator2)
            {
                var statement = Statement.Parse(tokens);
                statements.Add(statement);
            }

            return statements.ToArray();
        }
    }
}
