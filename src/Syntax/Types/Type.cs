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
 *  Defines the base class \c Type, which serves as the base class of all types in the system.
 */

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public abstract class Type: Node
    {
        /** Literal attributes. */

        /** Synthetic attributes. */
        public abstract TypeKind BaseType { get; }

        public Type(NodeKind kind, Position position):
            base(kind, position)
        {
        }

        public abstract bool Compare(Type other);

        public static Type Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            // Handle the special case of a tuple first (you cannot make arrays or ranges of tuples).
            if (start.Kind == TokenKind.Symbol_BracketBegin)
                return TupleType.Parse(tokens);

            // Parse the basic types that can be followed by either an array or a range specifier.
            Type result;
            switch (start.Kind)
            {
                case TokenKind.Keyword_Boolean:
                    result = BooleanType.Parse(tokens);
                    break;

                case TokenKind.Keyword_Integer:
                    result = IntegerType.Parse(tokens);
                    break;

                case TokenKind.Identifier:
                    result = IdentifierType.Parse(tokens);
                    break;

                default:
                    throw new ParserError(start.Position, 0, "Expected boolean or integer type, or a type name");
            }

            // Parse the optional array or range specifier
            if (tokens.Peek.Kind == TokenKind.Keyword_Array)
            {
                tokens.Match(TokenKind.Keyword_Array);
                do
                {
                    Token start1 = tokens.Match(TokenKind.Symbol_BracketBegin);
                    string name = tokens.Match(TokenKind.Identifier).Text;
                    result = new ArrayType(start1.Position, result, name);
                    tokens.Match(TokenKind.Symbol_BracketClose);
                } while (tokens.Peek.Kind == TokenKind.Symbol_BracketBegin);
            }
            else if (tokens.Peek.Kind == TokenKind.Keyword_Range)
            {
                Token start2 = tokens.Match(TokenKind.Keyword_Range);
                tokens.Match(TokenKind.Symbol_BracketBegin);
                Expression lower = Expression.Parse(tokens);
                tokens.Match(TokenKind.Symbol_Ellipsis);
                Expression upper = Expression.Parse(tokens);
                tokens.Match(TokenKind.Symbol_BracketClose);
                result = new RangeType(start2.Position, result, lower, upper);
            }

            return result;
        }

        public static Type ParseSymbol(Tokens tokens)
        {
            Token start = tokens.Peek;

            switch (start.Kind)
            {
                case TokenKind.Keyword_Boolean:
                    return BooleanType.Parse(tokens);

                case TokenKind.Identifier:
                    return IdentifierType.Parse(tokens);

                case TokenKind.Keyword_Integer:
                    return IntegerType.Parse(tokens);

                default:
                    throw new Error(start.Position, 0, "Expected 'boolean', 'integer', or identifier");
            }
        }
    }
}


