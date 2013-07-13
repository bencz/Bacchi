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
 *  Defines the class \c ProcedureDefinition, which represents complete definition of a procedure.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    /** Class which represents a single procedure definition (a declaration and a body). */
    public class ProcedureDefinition: Definition
    {
        private Parameter[] _parameters;
        public Parameter[] Parameters
        {
            get { return _parameters; }
        }

        private Block _block;
        public Block Block
        {
            get { return _block; }
        }

        /** Constructor for the \c ProcedureDefinition class. */
        public ProcedureDefinition(Position position, string name, Parameter[] parameters, Block block):
            base(NodeKind.ProcedureDefinition, position, name)
        {
            _parameters = parameters;
            foreach (Parameter parameter in _parameters)
                parameter.Above = this;
            _block = block;
            _block.Above = this;
        }

        /** Parses a sequence of tokens and returns a new \c ProcedureDefinition instance representing the parsed tokens. */
        public static new Definition Parse(Tokens tokens)
        {
            Token start = tokens.Peek;

            // Parse "proc" Name
            tokens.Match(TokenKind.Keyword_Proc);
            string name = tokens.Match(TokenKind.Identifier).Text;

            // Parse parameters, if any (if no parameters are present, it is a definition of a previous declaration).
            List<Parameter> parameters = null;
            if (tokens.Peek.Kind == TokenKind.Symbol_ParenthesisBegin)
            {
                parameters = new List<Parameter>();
                tokens.Match(TokenKind.Symbol_ParenthesisBegin);
                while (tokens.Peek.Kind != TokenKind.Symbol_ParenthesisClose)
                {
                    var parameter = Parameter.Parse(tokens);
                    parameters.Add(parameter);

                    if (tokens.Peek.Kind != TokenKind.Symbol_ParenthesisClose)
                        tokens.Match(TokenKind.Symbol_Semicolon);
                }
                tokens.Match(TokenKind.Symbol_ParenthesisClose);
            }

            // Handle special case that no block is supplied (equivalent to a C prototype).
            if (tokens.Peek.Kind == TokenKind.Symbol_Semicolon)
            {
                // Handle the case: "proc" name ";"
                if (parameters == null)
                    throw new Error(start.Position, 0, "Cannot use a procedure definition as a declaration");

                return new ProcedureDeclaration(start.Position, name, parameters.ToArray());
            }

            // Parse procedure body.
            Block block = Block.Parse(tokens);

            // Handle the case: "proc" name block (i.e. a procedure completion).
            if (parameters == null)
                return new ProcedureCompletion(start.Position, name, block);

            return new ProcedureDefinition(start.Position, name, parameters.ToArray(), block);
        }

        /** Implements the \c Visitor pattern. */
        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}

