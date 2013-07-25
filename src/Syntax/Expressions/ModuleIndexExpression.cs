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
 *  Defines the \c ModuleIndexExpression class, which represents a module index expression (module.symbol).
 *
 *  \note GCL allows the field part to be either an identifier or an integer.
 */

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public class ModuleIndexExpression: Expression
    {
#region Literal attributes
        private Expression _prefix;
        public Expression Prefix
        {
            get { return _prefix; }
        }

        private string _field;
        /** The name of the field in the public interface part of the specified module. */
        public string Field
        {
            get { return _field; }
        }
#endregion

#region Synthetic attributes
        public override TypeKind BaseType
        {
            get
            {
                string name = ((IdentifierExpression) _prefix).Name;
                Definition definition = this.World.Symbols.Lookup(_prefix.Position, name);
                return definition.BaseType;
            }
        }

        protected override bool ComputeIsConstant
        {
            get
            {
                string name = ((IdentifierExpression) _prefix).Name;
                Definition definition = this.World.Symbols.Lookup(_prefix.Position, name);
                return (definition.Kind == NodeKind.ConstantDefinition);
            }
        }

        protected override int ComputeConstantExpression
        {
            get
            {
                string name = ((IdentifierExpression) _prefix).Name;
                ConstantDefinition definition = (ConstantDefinition) this.World.Symbols.Lookup(_prefix.Position, name);
                if (definition.Literal.Kind == NodeKind.IntegerLiteral)
                    return ((IntegerLiteral) definition.Literal).Value;
                else if (definition.Literal.Kind == NodeKind.BooleanLiteral)
                    return ((BooleanLiteral) definition.Literal).Value ? 1 : 0;
                else
                    throw new InternalError("Was that a mouse that just ran across your keyboard?");
            }
        }
#endregion

        public ModuleIndexExpression(Position position, Expression prefix, string field):
            base(NodeKind.ModuleIndexExpression, position)
        {
            _prefix = prefix;
            _prefix.Above = this;

            _field = field;
        }

        /** Parsing is done in \c Expression.Parse(). */

        public override void Visit(Visitor that)
        {
            that.Visit(this);
        }
    }
}

