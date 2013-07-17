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
 *  Defines the base class \c ConstantDefinition, which is a base class for constant definitions.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    /** Class which represents a single constant definition. */
    public class ConstantDefinition: Definition
    {
        private Literal _literal;
        public Literal Literal
        {
            get { return _literal; }
        }

        /** Constructor for the \c ConstantDefinition class. */
        public ConstantDefinition(Position position, string name, Literal literal):
            base(NodeKind.ConstantDefinition, position, name)
        {
            _literal = literal;
            _literal.Above = this;
        }

        /** Parses a sequence of tokens and returns a new \c ConstantDefinition instance representing the parsed tokens. */
        public static new ConstantDefinition Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            tokens.Match(TokenKind.Keyword_Const);
            string name = tokens.Match(TokenKind.Identifier).Text;
            tokens.Match(TokenKind.Relational_Equality);

            Literal literal;
            switch (tokens.Peek.Kind)
            {
                case TokenKind.Keyword_False:
                case TokenKind.Keyword_True:
                    literal = BooleanLiteral.Parse(tokens);
                    break;

                case TokenKind.Integer:
                    literal = IntegerLiteral.Parse(tokens);
                    break;

                case TokenKind.String:
                    literal = StringLiteral.Parse(tokens);
                    break;

                default:
                    throw new Error(start.Position, 0, "Expected boolean, integer, or string literal");
            }

            return new ConstantDefinition(start.Position, name, literal);
        }

        /** Implements the \c Visitor pattern. */
        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}

