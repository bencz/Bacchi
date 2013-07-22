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
 *  Defines the \c Module class, which represents a single module in a program.
 */

using System.Collections.Generic;       // Dictionary<T1, T2>, List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public class Module: Definition
    {
#region Literal attributes
        private Definition[] _definitions;
        public Definition[] Definitions
        {
            get { return _definitions; }
        }

        private Block _block;
        public Block Block
        {
            get { return _block; }
        }
#endregion

#region Synthetic attributes
        public override TypeKind BaseType
        {
            get { return TypeKind.Module; }
        }
#endregion

        /** Constructor for the \c Module class.
         *
         *  \note \c block may be null, if there's no private part (the module contains only public definitions).
         */
        public Module(Position position, string name, Definition[] definitions, Block block):
            base(NodeKind.Module, position, name)
        {
            _definitions = definitions;
            foreach (Definition definition in _definitions)
                definition.Above = this;

            _block = block;
            if (_block != null)
                _block.Above = this;
        }

        public static new Module Parse(Tokens tokens)
        {
            Token start = tokens.Match(TokenKind.Keyword_Module);
            string name = tokens.Match(TokenKind.Identifier).Text;

            // Parse module interface.
            var definitions = Definition.ParseList(tokens, TokenKind.Keyword_Private, TokenKind.Keyword_Begin);

            // Parse optional private block.
            Block block = null;
            if (tokens.Peek.Kind == TokenKind.Keyword_Private)
            {
                tokens.Match(TokenKind.Keyword_Private);
                block = Block.Parse(tokens);
            }
            tokens.Match(TokenKind.Symbol_Dot);

            return new Module(start.Position, name, definitions, block);
        }

        public static Module[] ParseList(Tokens tokens)
        {
            var modules = new List<Module>();
            while (tokens.Peek.Kind != TokenKind.EndOfFile)
            {
                var module = Module.Parse(tokens);
                modules.Add(module);
            }

            return modules.ToArray();
        }

        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}

