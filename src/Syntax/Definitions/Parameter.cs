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
 *  Defines the \c Parameter class, which represents a single procedure parameter.
 *
 *  \note The \c Parameter class is not an explicit definition but rather an implicit definition created by a \c proc def.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position

namespace Bacchi.Syntax
{
    public class Parameter: Definition
    {
#region Literal attributes
        private ModeKind _mode;
        public ModeKind Mode
        {
            get { return _mode; }
        }

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

        public Parameter(Position position, ModeKind mode, string name, Type type):
            base(NodeKind.Parameter, position, name)
        {
            _mode = mode;

            _type = type;
            _type.Above = this;
        }

        public static new Parameter Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            ModeKind mode;
            switch (start.Kind)
            {
                case TokenKind.Keyword_Ref:
                    tokens.Match(TokenKind.Keyword_Ref);
                    mode = ModeKind.Reference;
                    break;

                case TokenKind.Keyword_Val:
                    tokens.Match(TokenKind.Keyword_Val);
                    mode = ModeKind.Value;
                    break;

                default:
                    throw new ParserError(start.Position, 0, "Expected 'ref' or 'val' keyword");
            }

            Type type = Type.Parse(tokens);

            string name = tokens.Match(TokenKind.Identifier).Text;

            return new Parameter(start.Position, mode, name, type);
        }

        public static Parameter[] ParseList(Tokens tokens)
        {
            Token start = tokens.Peek;

            tokens.Match(TokenKind.Symbol_ParenthesisBegin);
            var arguments = new List<Parameter>();
            if (tokens.Peek.Kind != TokenKind.Symbol_ParenthesisClose)
            {
                var argument = Parameter.Parse(tokens);
                arguments.Add(argument);

                while (tokens.Peek.Kind == TokenKind.Symbol_Comma)
                {
                    tokens.Match(TokenKind.Symbol_Comma);

                    argument = Parameter.Parse(tokens);
                    arguments.Add(argument);
                }
            }
            tokens.Match(TokenKind.Symbol_ParenthesisClose);

            return arguments.ToArray();
        }

        public override void Visit(Visitor that)
        {
            that.Visit(this);
        }
    }
}


