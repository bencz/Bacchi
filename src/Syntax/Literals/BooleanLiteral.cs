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
 *  Defines the class \c BooleanLiteral, which represents a single boolean literal (the keyword \c false or \c true).
 */

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    /** Class which represents a single boolean literal. */
    public class BooleanLiteral: Literal
    {
#region Literal attributes
        protected bool _value;
        public bool Value
        {
            get { return _value; }
        }
#endregion

#region Synthetic attributes
        public override TypeKind BaseType
        {
            get { return TypeKind.Boolean; }
        }

        protected override bool ComputeIsConstant
        {
            get { return true; }
        }

        protected override int ComputeConstantExpression
        {
            get { return _value ? 1 : 0; }
        }
#endregion

        /** Constructor for the \c BooleanLiteral class. */
        public BooleanLiteral(Position position, bool value):
            base(NodeKind.BooleanLiteral, position)
        {
            _value = value;
        }

        /** Parses a sequence of tokens and returns a new \c BooleanLiteral instance representing the parsed tokens. */
        public static new BooleanLiteral Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            bool value;
            switch (start.Kind)
            {
                case TokenKind.Keyword_False:
                    tokens.Match(TokenKind.Keyword_False);
                    value = false;
                    break;

                case TokenKind.Keyword_True:
                    tokens.Match(TokenKind.Keyword_True);
                    value = true;
                    break;

                default:
                    throw new Error(start.Position, 0, "Expected boolean literal ('false' or 'true')");
            }

            return new BooleanLiteral(start.Position, value);
        }

        /** Implements the Visitor pattern. */
        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}


