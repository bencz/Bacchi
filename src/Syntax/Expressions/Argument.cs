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
 *  Defines the \c Argument class, which represents a single procedure call argument.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position

namespace Bacchi.Syntax
{
    public class Argument: Expression
    {
#region Literal attributes
        private Expression _value;
        public Expression Value
        {
            get { return _value; }
        }
#endregion

#region Synthetic attributes
        public override TypeKind BaseType
        {
            get { return _value.BaseType; }
        }

        protected override bool ComputeIsConstant
        {
            get { return _value.IsConstant; }
        }

        protected override int ComputeConstantExpression
        {
            get { return _value.ConstantExpression; }
        }
#endregion

        public Argument(Position position, Expression value):
            base(NodeKind.Argument, position)
        {
            _value = value;
            _value.Above = this;
        }

        public static new Argument Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            Expression value = Expression.Parse(tokens);

            return new Argument(start.Position, value);
        }

        public static Argument[] ParseList(Tokens tokens)
        {
            Token start = tokens.Peek;

            tokens.Match(TokenKind.Symbol_ParenthesisBegin);

            var arguments = new List<Argument>();
            var argument = Argument.Parse(tokens);
            arguments.Add(argument);

            while (tokens.Peek.Kind == TokenKind.Symbol_Comma)
            {
                tokens.Match(TokenKind.Symbol_Comma);

                argument = Argument.Parse(tokens);
                arguments.Add(argument);
            }

            tokens.Match(TokenKind.Symbol_ParenthesisClose);

            return arguments.ToArray();
        }

        public override void Visit(Visitor that)
        {
            that.Visit(this);
        }
    }
}


