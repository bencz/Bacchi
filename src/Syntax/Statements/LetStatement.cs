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
 *  An AST node representing an assignment statement.
 *
 *  \note GCL allows multiple, concurrent assigments in a single assignment statement.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public class LetStatement: Statement
    {
        private Assignment[] _assignments;
        public Assignment[] Assignments
        {
            get { return _assignments; }
        }

        public LetStatement(Position position, Assignment[] assignments):
            base(NodeKind.LetStatement, position)
        {
            _assignments = assignments;
            foreach (Assignment assignment in _assignments)
                assignment.Above = this;
        }

        public static new LetStatement Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            var references = Reference.ParseList(tokens, TokenKind.Symbol_Assignment);
            tokens.Match(TokenKind.Symbol_Assignment);
            var expressions = Expression.ParseList(tokens, TokenKind.Symbol_Semicolon);
            tokens.Match(TokenKind.Symbol_Semicolon);

            if (expressions.Length < references.Length)
                throw new Error(start.Position, 0, "Too few expressions given for assignment");
            if (expressions.Length > references.Length)
                throw new Error(start.Position, 0, "Too many expressions given for assignment");

            /** Merge the two separate lists of references and expressions into an array of \c Assignments. */
            var assignments = new Assignment[references.Length];
            for (int i = 0; i < references.Length; i++)
                assignments[i] = new Assignment(references[i].Position, references[i], expressions[i]);

            return new LetStatement(start.Position, assignments);
        }

        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}
