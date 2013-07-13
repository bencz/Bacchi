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
 *  Defines the \c Expression base class, which acts as a base for \b all expression nodes in the compiler.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public abstract class Expression: Node
    {
        public Expression(NodeKind kind, Position position):
            base(kind, position)
        {
        }

        public static Expression Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            Expression result;
            switch (tokens.Peek.Kind)
            {
                case TokenKind.Integer:
                    result = IntegerLiteral.Parse(tokens);
                    break;

                case TokenKind.Identifier:
                    if (tokens[1].Kind != TokenKind.Symbol_Dot)
                        result = IdentifierExpression.Parse(tokens);
                    else if (tokens[2].Kind == TokenKind.Identifier)
                        return GlobalExpression.Parse(tokens);
                    else
                        result = null;
#if false
                    else if (tokens[2].Kind == TokenKind.Integer)
                        return new TupleExpression
#endif
                    break;

                case TokenKind.Keyword_False:
                case TokenKind.Keyword_True:
                    return BooleanLiteral.Parse(tokens);

                default:
                    throw new Error(start.Position, 0, "Expression parser sorely needs finishing up");
            }

            return result;
        }

        public static Expression[] ParseList(Tokens tokens, TokenKind terminator)
        {
            var expressions = new List<Expression>();
            do
            {
                var expression = Expression.Parse(tokens);
                if (tokens.Peek.Kind != TokenKind.Symbol_Comma)
                    break;
                tokens.Match(TokenKind.Symbol_Comma);
            } while (tokens.Peek.Kind != terminator);

            return expressions.ToArray();
        }
    }
}



