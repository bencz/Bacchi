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
 *  Defines the class \c TypeDefinition, which represents a single definition of a type.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    /** Class which represents a single type definition. */
    public class TypeDefinition: Definition
    {
#region Literal attributes
        private Type _type;
        public Type Type
        {
            get { return _type; }
        }
#endregion

#region Synthetic attributes
        public override TypeKind BaseType
        {
            get { return _type.BaseType; }
        }
#endregion

        /** Constructor for the \c TypeDefinition class. */
        public TypeDefinition(Position position, string name, Type type):
            base(NodeKind.TypeDefinition, position, name)
        {
            _type = type;
            _type.Above = this;
        }

        /** Parses a sequence of tokens and returns a new \c TypeDefinition instance representing the parsed tokens. */
        public static new TypeDefinition Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            tokens.Match(TokenKind.Keyword_Typedef);

            // Parse the type.
            var type = Type.Parse(tokens);

            // Parse the name of the new type.
            string name = tokens.Match(TokenKind.Identifier).Text;

            // Create and return the new \c TypeDefinition instance.
            return new TypeDefinition(start.Position, name, type);
        }

        /** Implements the \c Visitor pattern. */
        public override void Visit(Visitor that)
        {
            that.Visit(this);
        }
    }
}



