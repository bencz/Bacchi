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
 *  Defines the \c ForallStatement class, which represents a \c forall statement.
 */

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public class ForallStatement: Statement
    {
        private string _variable;
        public string Variable
        {
            get { return _variable; }
        }

        private Statement[] _statements;
        public Statement[] Statements
        {
            get { return _statements; }
        }

        public ForallStatement(Position position, string variable, Statement[] statements):
            base(NodeKind.ForallStatement, position)
        {
            _variable = variable;
            _statements = statements;
            foreach (Statement statement in _statements)
                statement.Above = this;
        }

        public static new ForallStatement Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            tokens.Match(TokenKind.Keyword_Forall);
            string variable = tokens.Match(TokenKind.Identifier).Text;
            tokens.Match(TokenKind.Symbol_Then);
            Statement[] statements = Statement.ParseList(tokens, TokenKind.Keyword_Llarof, TokenKind.Symbol_Brackets);
            tokens.Match(TokenKind.Keyword_Llarof);
            tokens.Match(TokenKind.Symbol_Semicolon);

            return new ForallStatement(start.Position, variable, statements);
        }

        public override void Visit(Visitor that)
        {
            that.Visit(this);
        }
    }
}
