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
    /** Dumper used to dump the Abstract Syntax Tree (AST) to a file. */
    public class PopulateSymbolTablePass: Visitor
    {
        private Symbols _symbols;

        public PopulateSymbolTablePass(Symbols symbols)
        {
            _symbols = symbols;
        }

        public void Visit(Node[] nodes)
        {
            foreach (Node node in nodes)
                node.Visit(this);
        }

        public object Visit(Argument that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(ArrayExpression that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(ArrayReference that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(ArrayType that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(Assignment that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(BinaryExpression that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(Block that)
        {
            foreach (Definition definition in that.Definitions)
                definition.Visit(this);
            /** \note Don't visit the statements as they cannot create new symbols. */

            return null;
        }

        public object Visit(BooleanDefinition that)
        {
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);
            return null;
        }

        public object Visit(BooleanLiteral that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(BooleanType that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(CallStatement that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(ConstantDefinition that)
        {
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);
            return null;
        }

        public object Visit(File that)
        {
            Visit(that.Modules);
            return null;
        }

        public object Visit(ForallStatement that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(Guard that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(IdentifierExpression that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(IdentifierReference that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(IdentifierType that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(IntegerDefinition that)
        {
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);
            return null;
        }

        public object Visit(IntegerExpression that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(IntegerLiteral that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(IntegerType that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(MemberExpression that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(Module that)
        {
            _symbols.EnterModule(that);

            Visit(that.Definitions);
            if (that.Block != null)
                that.Block.Visit(this);

            _symbols.LeaveModule(that);
            return null;
        }

        public object Visit(Parameter that)
        {
            _symbols.Insert(that, ScopeKind.Local);
            return null;
        }

        public object Visit(ParenthesisExpression that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(ProcedureCompletion that)
        {
            Node definition = _symbols.Lookup(that.Name);
            if (definition == null)
                throw new Error(that.Position, 0, "Cannot complete undeclared procedure '" + that.Name + "'");
            return null;
        }

        public object Visit(ProcedureDeclaration that)
        {
            /** Create a procedure definition entry for the specified procedure, with its block part set to \c null. */
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);
            return null;
        }

        public object Visit(ProcedureDefinition that)
        {
            Node definition = _symbols.Lookup(that.Name);
            if (definition != null)
                throw new Error(that.Position, 0, "Cannot redefine procedure");

            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);

            return null;
        }

        public object Visit(Program that)
        {
            foreach (File file in that.Files)
                file.Visit(this);
            return that;
        }

        public object Visit(RangeType that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(Statement that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(StringLiteral that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(TupleDefinition that)
        {
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);

            return null;
        }

        public object Visit(TupleExpression that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(TupleIndexExpression that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(TupleType that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(TypeDefinition that)
        {
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);

            return null;
        }

        public object Visit(UnaryExpression that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(VariableDefinition that)
        {
            ScopeKind scope = (that.Above is Module) ? ScopeKind.Global : ScopeKind.Local;
            _symbols.Insert(that, scope);

            return null;
        }
    }
}

