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
 *  Defines the class \c RangeType, which represents a single range specification in a type specification.
 */

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public class RangeType: Type
    {
        /** Literal attributes. */
        private Type _type;
        public Type Type
        {
            get { return _type; }
        }

        private Expression _lower;
        public Expression Lower
        {
            get { return _lower; }
        }

        private Expression _upper;
        public Expression Upper
        {
            get { return _upper; }
        }

        /** Synthetic attributes. */
        public override TypeKind BaseType
        {
            get { return _type.BaseType; }
        }

        public override bool Equal(Symbols symbols, Type other)
        {
            switch (other.Kind)
            {
                case NodeKind.BooleanType:
                    return (_type.Kind == NodeKind.BooleanType);

                case NodeKind.IntegerType:
                    return (_type.Kind == NodeKind.IntegerType);

                case NodeKind.RangeType:
                    return (_type.Kind == NodeKind.RangeType && _type.Equal(symbols, ((RangeType) _type).Type));

                default:
                    return false;
            }
        }

        public RangeType(Position position, Type type, Expression lower, Expression upper):
            base(NodeKind.RangeType, position)
        {
            _type = type;
            _type.Above = this;

            _lower = lower;
            _lower.Above = this;

            _upper = upper;
            _upper.Above = this;
        }

        /** Parsing is handled by \c Type.Parse(). */

        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}

