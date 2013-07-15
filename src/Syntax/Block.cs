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
 *  An AST node representing a code block.
 */

using Bacchi.Kernel;                    // Error, Position

namespace Bacchi.Syntax
{
    public class Block: Node
    {
        private Definition[] _definitions;
        public Definition[] Definitions
        {
            get { return _definitions; }
        }

        private Statement[] _statements;
        public Statement[] Statements
        {
            get { return _statements; }
        }

        public Block(Position position, Definition[] definitions, Statement[] statements):
            base(NodeKind.Block, position)
        {
            _definitions = definitions;
            foreach (Definition definition in _definitions)
                definition.Above = this;

            _statements = statements;
            foreach (Statement statement in _statements)
                statement.Above = this;
        }

        public static Block Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            var definitions = Definition.ParseList(tokens, TokenKind.Keyword_Begin);
            tokens.Match(TokenKind.Keyword_Begin);
            var statements = Statement.ParseList(tokens, TokenKind.Keyword_End, TokenKind.None);
            tokens.Match(TokenKind.Keyword_End);

            return new Block(start.Position, definitions, statements);
        }

        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}
