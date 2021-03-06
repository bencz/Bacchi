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
 *  Defines the \c IntegerType class, which is represent integer types in the source program.
 */

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    /** Class which represents the \c integer keyword in a source program. */
    public class IntegerType: Type
    {
        /** Literal attributes. */

        /** Synthetic attributes. */
        public override TypeKind BaseType
        {
            get { return TypeKind.Integer; }
        }

        /** Constructor for the \c IntegerType class. */
        public IntegerType(Position position):
            base(NodeKind.IntegerType, position)
        {
        }

        public override bool Compare(Type other)
        {
            switch (other.Kind)
            {
                case NodeKind.IntegerType:
                    return true;

                case NodeKind.RangeType:
                    /** \note The compatibility of range types depends upon the base type of the range type. */
                    return ((RangeType) other).Type.Kind == NodeKind.IntegerType;

                default:
                    return false;
            }
        }

        /** Parses a sequence of tokens matching an \c integer type and returns a new \c IntegerType instance. */
        public static new IntegerType Parse(Tokens tokens)
        {
            Token start = tokens.Match(TokenKind.Keyword_Integer);
            return new IntegerType(start.Position);
        }

        /** Implements the visitor pattern. */
        public override void Visit(Visitor that)
        {
            that.Visit(this);
        }
    }
}

