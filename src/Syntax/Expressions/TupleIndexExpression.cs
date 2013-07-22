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
 *  Defines the \c TupleIndexExpression class, which represents a tuple indexing expression (tuple.1).
 */

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public class TupleIndexExpression: Expression
    {
#region Literal attributes
        private Expression _prefix;
        public Expression Prefix
        {
            get { return _prefix; }
        }

        private int _index;
        public int Index
        {
            get { return _index; }
        }
#endregion

#region Synthetic attributes
        public override TypeKind BaseType
        {
            get
            {
                /** Look up the Nth field in the definition of the tuple and return its type. */
                if (_prefix.Kind != NodeKind.IdentifierExpression)
                    throw new InternalError("Expected identifier but found: " + _prefix.Kind.ToString());

                string name = ((IdentifierExpression) _prefix).Name;
                Definition definition = this.World.Symbols.Lookup(_prefix.Position, name);

                if (definition.Kind != NodeKind.TupleDefinition)
                    throw new Error(_prefix.Position, 0, "Expected tuple definition");
                TupleDefinition tuple = (TupleDefinition) definition;
                if (_index < 0 || _index >= tuple.Types.Length)
                    throw new Error(_prefix.Position, 0, "Tuple index outside valid range");

                return tuple.Types[_index].BaseType;
            }
        }

        protected override bool ComputeIsConstant
        {
            get { return false; }
        }

        protected override int ComputeConstantExpression
        {
            get { throw new InternalError("Cannot compute constant value of tuple index expression."); }
        }
#endregion

        public TupleIndexExpression(Position position, Expression prefix, int index):
            base(NodeKind.TupleIndexExpression, position)
        {
            _prefix = prefix;
            _prefix.Above = this;

            _index = index;
        }

        /** \note Parsing is done in the \c Expression class. */

        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}


