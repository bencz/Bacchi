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
 *  Defines the \c BinaryExpression class, which represents a binary operator and its left and right expressions.
 */

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public class BinaryExpression: Expression
    {
#region Literal attributes
        private BinaryKind _operator;
        public BinaryKind Operator
        {
            get { return _operator; }
        }

        private Expression _first;
        public Expression First
        {
            get { return _first; }
        }

        private Expression _other;
        public Expression Other
        {
            get { return _other; }
        }
#endregion

#region Synthetic attributes
        public override TypeKind BaseType
        {
            get { return _first.BaseType; }
        }

        protected override bool ComputeIsConstant
        {
            /** \note Use '&&' instead of '&' to avoid computing the right hand side if the left hand side is false. */
            get { return _first.IsConstant && _other.IsConstant; }
        }

        protected override int ComputeConstantExpression
        {
            get
            {
                if (!this.IsConstant)
                    throw new InternalError("Attempt of evaluating a non-constant expression");

                // Compute the result of the constant expression.
                int first = _first.ConstantExpression;
                int other = _other.ConstantExpression;
                int result;
                switch (_operator)
                {
                    case BinaryKind.Add:
                        if (this.BaseType != TypeKind.Integer)
                            throw new Error(this.Position, 0, "Type mismatch in constant addition");
                        result = first + other;
                        if (result < first && result < other)
                            throw new Error(this.Position, 0, "Overflow in constant expression");
                        break;

                    case BinaryKind.And:
                        if (this.BaseType != TypeKind.Boolean || this.BaseType != TypeKind.Integer)
                            throw new Error(this.Position, 0, "Attempt of adding non-scalar expressions");
                        result = first & other;
                        break;

                    case BinaryKind.Divide:
                        if (this.BaseType != TypeKind.Integer)
                            throw new Error(this.Position, 0, "Type mismatch in constant division");
                        if (other == 0)
                            throw new Error(this.Position, 0, "Division by zero in constant expression");
                        result = first / other;
                        break;

                    case BinaryKind.Multiply:
                        if (this.BaseType != TypeKind.Integer)
                            throw new Error(this.Position, 0, "Type mismatch in constant multiplication");
                        result = first * other;
                        /** \todo Detect overflows in constant expression multiplications. */
                        break;

                    case BinaryKind.Or:
                        result = first | other;
                        break;

                    case BinaryKind.Remainder:
                        if (this.BaseType != TypeKind.Integer)
                            throw new Error(this.Position, 0, "Type mismatch in constant remainder");
                        if (other == 0)
                            throw new Error(this.Position, 0, "Division by zero in constant expression");
                        result = first % other;
                        break;

                    case BinaryKind.Subtract:
                        if (this.BaseType != TypeKind.Integer)
                            throw new Error(this.Position, 0, "Type mismatch in constant subtraction");
                        result = first - other;
                        if (result > first && result > other)
                            throw new Error(this.Position, 0, "Underflow in constant expression");
                        break;

                    case BinaryKind.Equality:
                        if (this.BaseType != TypeKind.Boolean)
                            throw new Error(this.Position, 0, "Wrong type in constant relational '=' operation");
                        result = (first == other) ? 1 : 0;
                        break;

                    case BinaryKind.Difference:
                        if (this.BaseType != TypeKind.Boolean)
                            throw new Error(this.Position, 0, "Wrong type in constant relational '#' operation");
                        result = (first != other) ? 1 : 0;
                        break;

                    case BinaryKind.GreaterEqual:
                        if (this.BaseType != TypeKind.Boolean)
                            throw new Error(this.Position, 0, "Wrong type in constant relational '>=' operation");
                        result = (first >= other) ? 1 : 0;
                        break;

                    case BinaryKind.GreaterThan:
                        if (this.BaseType != TypeKind.Boolean)
                            throw new Error(this.Position, 0, "Wrong type in constant relational '>' operation");
                        result = (first > other) ? 1 : 0;
                        break;

                    case BinaryKind.LessEqual:
                        if (this.BaseType != TypeKind.Boolean)
                            throw new Error(this.Position, 0, "Wrong type in constant relational '<=' operation");
                        result = (first <= other) ? 1 : 0;
                        break;

                    case BinaryKind.LessThan:
                        if (this.BaseType != TypeKind.Boolean)
                            throw new Error(this.Position, 0, "Wrong type in constant relational '<' operation");
                        result = (first < other) ? 1 : 0;
                        break;

                    default:
                        throw new InternalError("Invalid or unknown binary operator: " + _operator.ToString());
                }

                return result;
            }
        }
#endregion

        public BinaryExpression(Position position, BinaryKind @operator, Expression first, Expression other):
            base(NodeKind.BinaryExpression, position)
        {
            _operator = @operator;

            _first = first;
            _first.Above = this;

            _other = other;
            _other.Above = this;
        }

        public static BinaryKind Convert(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Operator_Add           : return BinaryKind.Add;
                case TokenKind.Operator_And           : return BinaryKind.And;
                case TokenKind.Operator_Divide        : return BinaryKind.Divide;
                case TokenKind.Operator_Multiply      : return BinaryKind.Multiply;
                case TokenKind.Operator_Or            : return BinaryKind.Or;
                case TokenKind.Operator_Remainder     : return BinaryKind.Remainder;
                case TokenKind.Operator_Subtract      : return BinaryKind.Subtract;
                case TokenKind.Relational_Equality    : return BinaryKind.Equality;
                case TokenKind.Relational_Difference  : return BinaryKind.Difference;
                case TokenKind.Relational_GreaterEqual: return BinaryKind.GreaterEqual;
                case TokenKind.Relational_GreaterThan : return BinaryKind.GreaterThan;
                case TokenKind.Relational_LessEqual   : return BinaryKind.LessEqual;
                case TokenKind.Relational_LessThan    : return BinaryKind.LessThan;

                default:
                    throw new InternalError("Expected relational operator token index: " + kind.ToString());
            }
        }

        /** \note Parsing is done in \c Expression.Parse(). */

        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}





