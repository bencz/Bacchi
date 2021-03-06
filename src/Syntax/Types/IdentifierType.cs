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
 *  Defines the \c IdentifierType class, which represents a named type.
 */

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public class IdentifierType: Type
    {
        /** Literal attributes. */
        private string _name;
        /** The name of the type being referenced. */
        public string Name
        {
            get { return _name; }
        }

        /** Synthetic attributes. */
        public override TypeKind BaseType
        {
            get
            {
                Definition definition = this.World.Symbols.Lookup(this.Position, _name);
                return definition.BaseType;
            }
        }

        /** Constructor for the \c IdentifierType class. */
        public IdentifierType(Position position, string name):
            base(NodeKind.IdentifierType, position)
        {
            _name = name;
        }

        public override bool Compare(Type other)
        {
            Definition definition = this.World.Symbols.Lookup(this.Position, _name);
            if (definition.Kind != NodeKind.TypeDefinition)
                return false;

            Type first = ((TypeDefinition) definition).Type;
            return first.Compare(other);
        }

        public static new IdentifierType Parse(Tokens tokens)
        {
            Token start = tokens.Match(TokenKind.Identifier);
            return new IdentifierType(start.Position, start.Text);
        }

        public override void Visit(Visitor that)
        {
            that.Visit(this);
        }
    }
}

