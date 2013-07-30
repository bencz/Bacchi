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
 *  Defines the \c ConvertTuplesToStructsPass class, which traverses the tree and converts anonymous tuples into structures.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error
using Bacchi.Syntax;                    // Visitor, nodes

namespace Bacchi.Passes
{
    /** Converts tuple expressions and tuple definitions into corresponding structure definitions with named members. */
    public class ConvertTuplesToStructsPass: Visitor
    {
        /** A complete list of all distinct tuple types found in the AST. */
        private List<TupleType> _tuples = new List<TupleType>();

        public ConvertTuplesToStructsPass()
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
            Visit(that.Definitions);
            /** \note \c that.Statements cannot define any tuples. */
        }

        public void Visit(BooleanDefinition that)
        {
            /** \note There are no relevant nodes below this node. */
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
            /** \note There are no relevant nodes below this node. */
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
            /** \note There are no relevant nodes below this node. */
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
            Visit(that.Definitions);
            if (that.Block != null)
                that.Block.Visit(this);
        }

        public void Visit(ModuleIndexExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(Parameter that)
        {
            if (that.Type.Kind == NodeKind.TupleType)
            {
                // Create a replacement struct type.
                foreach (TupleType type in _tuples)
                {
                    if (that.Type.Compare(type))
                        return;
                }

                _tuples.Add((TupleType) that.Type);
            }
        }

        public void Visit(ParenthesisExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(ProcedureCompletion that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(ProcedureDeclaration that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(ProcedureDefinition that)
        {
            Visit(that.Parameters);
        }

        public void Visit(Program that)
        {
            Visit(that.Files);

            that.Tuples = _tuples.ToArray();
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
            /** \note There are no relevant nodes below this node. */
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
            /** \note I use a slow linear search (instead of mangling and storing in a dictionary) for the sake of simplicity. */
            // See if this tuple layout has already been defined.
            foreach (TupleType type in _tuples)
            {
                if (that.Compare(type))
                    return;
            }

            // Add this tuple layout to the list of known tuple layouts.
            _tuples.Add(that);
        }

        public void Visit(TypeDefinition that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(UnaryExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(VariableDefinition that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(WriteStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }
    }
}


