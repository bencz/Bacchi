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
 *  Defines the class \c VariableDefinition, which represents a single definition of a variable with a named type.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    /** Class which represents a single variable definition. */
    public class VariableDefinition: Definition
    {
        private string _type;
        public string Type
        {
            get { return _type; }
        }

        /** Constructor for the \c VariableDefinition class. */
        public VariableDefinition(Position position, string name, string type):
            base(NodeKind.VariableDefinition, position, name)
        {
            _type = type;
        }

        /** Parses a sequence of tokens and returns a new \c VariableDefinition instance representing the parsed tokens. */
        public static new VariableDefinition Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            string type = tokens.Match(TokenKind.Identifier).Text;
            string name = tokens.Match(TokenKind.Identifier).Text;

            return new VariableDefinition(start.Position, name, type);
        }

        /** Implements the \c Visitor pattern. */
        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}

