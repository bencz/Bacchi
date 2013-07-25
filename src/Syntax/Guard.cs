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
 *  A node representing a guard (a condition and an associated set of statements).
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public class Guard: Node
    {
        protected Expression _expression;
        public Expression Expression
        {
            get { return _expression; }
        }

        protected Statement[] _statements;
        public Statement[] Statements
        {
            get { return _statements; }
        }

        public Guard(Position position, Expression expression, Statement[] statements):
            base(NodeKind.Guard, position)
        {
            _expression = expression;
            _expression.Above = this;
            _statements = statements;
            foreach (Statement statement in _statements)
                statement.Above = this;
        }

        public static Guard Parse(Tokens tokens, TokenKind terminator)
        {
            Token start = tokens.Peek;

            var expression = Expression.Parse(tokens);
            tokens.Match(TokenKind.Symbol_Then);
            var statements = Statement.ParseList(tokens, terminator, TokenKind.Symbol_Brackets);

            return new Guard(start.Position, expression, statements);
        }

        public static Guard[] ParseList(Tokens tokens, TokenKind terminator)
        {
            Token start = tokens.Peek;

            var guards = new List<Guard>();

            var guard = Guard.Parse(tokens, terminator);
            guards.Add(guard);

            while (tokens.Peek.Kind == TokenKind.Symbol_Brackets)
            {
                tokens.Match(TokenKind.Symbol_Brackets);
                guard = Guard.Parse(tokens, terminator);
                guards.Add(guard);
            }

            return guards.ToArray();
        }

        public override void Visit(Visitor that)
        {
            that.Visit(this);
        }
    }
}

