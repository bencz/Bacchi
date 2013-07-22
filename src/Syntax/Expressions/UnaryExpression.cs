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
 *  Defines the \c UnaryExpression class, which represents is only used for a logical \b not operation.
 */

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public class UnaryExpression: Expression
    {
#region Literal attributes
        private UnaryKind _operator;
        public UnaryKind Operator
        {
            get { return _operator; }
        }

        private Expression _expression;
        public Expression Expression
        {
            get { return _expression; }
        }
#endregion

#region Synthetic attributes
        public override TypeKind BaseType
        {
            get
            {
                return _expression.BaseType;
            }
        }

        protected override bool ComputeIsConstant
        {
            get { return _expression.IsConstant; }
        }

        protected override int ComputeConstantExpression
        {
            get
            {
                int value = _expression.ConstantExpression;
                int result;
                switch (_operator)
                {
                    case UnaryKind.Minus:
                        result = -value;
                        break;

                    case UnaryKind.Plus:
                        result = +value;
                        break;

                    case UnaryKind.Not:
                        if (_expression.BaseType == TypeKind.Boolean)
                            result = (value == 0) ? 1 : 0;
                        else if (_expression.BaseType == TypeKind.Integer)
                            result = ~value;
                        else
                            throw new InternalError("Checker should have caught this: Unary NOT operation on non-scalar value");
                        break;

                    default:
                        throw new InternalError("Unknown unary operator kind: " + _operator.ToString());

                }
                return result;
            }
        }
#endregion

        public UnaryExpression(Position position, UnaryKind @operator, Expression expression):
            base(NodeKind.UnaryExpression, position)
        {
            _operator = @operator;

            _expression = expression;
            _expression.Above = this;
        }

        public static UnaryKind Convert(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Operator_Add:
                    return UnaryKind.Plus;

                case TokenKind.Operator_Not:
                    return UnaryKind.Not;

                case TokenKind.Operator_Subtract:
                    return UnaryKind.Minus;

                default:
                    throw new InternalError("Unknown unary operator: " + kind.ToString());
            }
        }

        /** \note Parsing is done in Expression.Parse(). */

        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}

