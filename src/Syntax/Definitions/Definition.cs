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
 *  Defines the class \c Definition, which is an abstract base class for all types of definitions.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public abstract class Definition: Node
    {
        private string _name;
        /** The name of the definition. */
        public string Name
        {
            get { return _name; }
        }

        public Definition(NodeKind kind, Position position, string name):
            base(kind, position)
        {
            _name = name;
        }

        public static Definition Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            Definition result;
            switch (start.Kind)
            {
                case TokenKind.Keyword_Boolean:
                    result = BooleanDefinition.Parse(tokens);
                    break;

                case TokenKind.Symbol_BracketBegin:
                    result = TupleDefinition.Parse(tokens);
                    break;

                case TokenKind.Keyword_Const:
                    result = ConstantDefinition.Parse(tokens);
                    break;

                case TokenKind.Keyword_Integer:
                    result = IntegerDefinition.Parse(tokens);
                    break;

                case TokenKind.Keyword_Proc:
                    result = ProcedureDefinition.Parse(tokens);
                    break;

                case TokenKind.Keyword_Typedef:
                    result = TypeDefinition.Parse(tokens);
                    break;

                default:
                    throw new Error(start.Position, 0, "Unexpected token: " + start.ToString());
            }
            tokens.Match(TokenKind.Symbol_Semicolon);

            return result;
        }

        public static Definition[] ParseList(Tokens tokens, TokenKind terminator)
        {
            var definitions = new List<Definition>();
            do
            {
                var definition = Definition.Parse(tokens);
                definitions.Add(definition);
            }  while (tokens.Peek.Kind != terminator);

            return definitions.ToArray();
        }

        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}
