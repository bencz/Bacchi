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
 *  Defines the class \c TupleDefinition, which represents a single definition of a tuple variable.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    /** Class which represents a single tuple variable definition. */
    public class TupleDefinition: Definition
    {
#region Literal attributes
        private Type[] _types;
        /** The list of types of the fields of the tuple variable. */
        public Type[] Types
        {
            get { return _types; }
        }
#endregion

#region Synthetic attributes
        public override TypeKind BaseType
        {
            get { return TypeKind.Tuple; }
        }
#endregion

        /** Constructor for the \c TupleDefinition class. */
        public TupleDefinition(Position position, string name, Type[] types):
            base(NodeKind.TupleDefinition, position, name)
        {
            _types = types;
            foreach (Type type in _types)
                type.Above = this;
        }

        /** Parses a sequence of tokens and returns a new \c TupleDefinition instance representing the parsed tokens. */
        public static new TupleDefinition Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            // Parse the list of types denoting fields in the tuple variable.
            tokens.Match(TokenKind.Symbol_BracketBegin);
            var types = new List<Type>();
            for (;;)
            {
                var type = Type.Parse(tokens);
                types.Add(type);

                if (tokens.Peek.Kind == TokenKind.Symbol_BracketClose)
                    break;
                tokens.Match(TokenKind.Symbol_Comma);
            }
            tokens.Match(TokenKind.Symbol_BracketClose);

            // Parse the name of the typle variable.
            string name = tokens.Match(TokenKind.Identifier).Text;

            // Create and return the new \c TupleDefinition instance.
            return new TupleDefinition(start.Position, name, types.ToArray());
        }

        /** Implements the \c Visitor pattern. */
        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}


