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
 *  Defines the \c PopulateSymbolTablePass class, which traverses the tree and builds the symbol table.
 */

using Bacchi.Kernel;                    // Error
using Bacchi.Syntax;

namespace Bacchi.Passes
{
    /** Populates the global symbol table with symbols from the AST. */
    public class PopulateSymbolTablePass: Visitor
    {
        /** Cache of the global symbol table found in the topmost \c Program node. */
        private Symbols _symbols;

        public PopulateSymbolTablePass()
        {
        }

        public void Visit(Node[] nodes)
        {
            foreach (Node node in nodes)
                node.Visit(this);
        }

        public void Visit(Argument that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(ArrayExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(ArrayReference that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(ArrayType that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(Assignment that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(BinaryExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(Block that)
        {
            foreach (Definition definition in that.Definitions)
                definition.Visit(this);
            /** \note Don't visit the statements as they cannot create new symbols. */
        }

        public void Visit(BooleanDefinition that)
        {
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);
        }

        public void Visit(BooleanLiteral that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(BooleanType that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(CallStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(ConstantDefinition that)
        {
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);
        }

        public void Visit(DoStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(File that)
        {
            Visit(that.Modules);
        }

        public void Visit(ForallStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(Guard that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(IdentifierExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(IdentifierReference that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(IdentifierType that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(IfStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(IntegerDefinition that)
        {
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);
        }

        public void Visit(IntegerLiteral that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(IntegerType that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(LetStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(Module that)
        {
            _symbols.EnterModule(that);

            Visit(that.Definitions);
            if (that.Block != null)
                that.Block.Visit(this);

            _symbols.LeaveModule(that);
        }

        public void Visit(ModuleIndexExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(Parameter that)
        {
            _symbols.Insert(that, ScopeKind.Local);
        }

        public void Visit(ParenthesisExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(ProcedureCompletion that)
        {
            Node definition = _symbols.Lookup(that.Name);
            if (definition == null)
                throw new Error(that.Position, 0, "Cannot complete undeclared procedure '" + that.Name + "'");
        }

        public void Visit(ProcedureDeclaration that)
        {
            /** Create a procedure definition entry for the specified procedure, with its block part set to \c null. */
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);
        }

        public void Visit(ProcedureDefinition that)
        {
            Node definition = _symbols.Lookup(that.Name);
            if (definition != null)
                throw new Error(that.Position, 0, "Cannot redefine procedure");

            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);
        }

        public void Visit(Program that)
        {
            // Cache the global symbol table locally.
            _symbols = that.Symbols;

            foreach (File file in that.Files)
                file.Visit(this);

            // Release the cached symbol table.
            _symbols = null;
        }

        public void Visit(RangeType that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(ReadStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(ReturnStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(SkipStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(StringLiteral that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(TupleDefinition that)
        {
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);
        }

        public void Visit(TupleExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(TupleIndexExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(TupleType that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(TypeDefinition that)
        {
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);
        }

        public void Visit(UnaryExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(VariableDefinition that)
        {
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);
        }

        public void Visit(WriteStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }
    }
}

