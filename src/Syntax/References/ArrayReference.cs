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
 *  Defines the \c ArrayReference class, which represents a single left-hand-side (lhs) array expression that can be assigned to.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public class ArrayReference: Reference
    {
#region Literal attributes
        private Reference _reference;
        public Reference Reference
        {
            get { return _reference; }
        }

        private Expression _expression;
        public Expression Expression
        {
            get { return _expression; }
        }
#endregion

#region Synthetic attributes
        protected override TypeKind ComputeBaseType
        {
            /** \todo Return the proper base type of multi-dimensional arrays. */
            get { return _reference.BaseType; }
        }
#endregion

        public ArrayReference(Position position, Reference reference, Expression expression):
            base(NodeKind.ArrayReference, position)
        {
            _reference = reference;
            _reference.Above = this;

            _expression = expression;
            _expression.Above = this;
        }

#if false
        public static ArrayReference Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            Token name = tokens.Match(TokenKind.Identifier);
            ArrayReference result = new IdentifierArrayReference(name.Position, name.Text);

            // Parse optional array indexer(s).
            while (tokens.Peek.Kind == TokenKind.Symbol_BracketBegin)
            {
                Token start1 = tokens.Match(TokenKind.Symbol_BracketBegin);
                Expression expression = Expression.Parse(tokens, TokenKind.SymbolBracketClose);
                result = new ArrayIndexer(start1.Position, result, expression);
                tokens.Match(TokenKind.Symbol_BracketClose);
            }

            return result;
        }

        public static ArrayReference[] ParseList(Tokens tokens, TokenKind terminator)
        {
            var expressions = new List<ArrayReference>();
            do
            {
                var expression = ArrayReference.Parse(tokens);
                if (tokens.Peek.Kind != TokenKind.Symbol_Comma)
                    break;
                tokens.Match(TokenKind.Symbol_Comma);
            } while (tokens.Peek.Kind != terminator);

            return expressions.ToArray();
        }
#endif

        public override void Visit(Visitor that)
        {
            that.Visit(this);
        }
    }
}


