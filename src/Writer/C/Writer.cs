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
 *  Defines the C source code writer.
 */

using Bacchi.Kernel;                    // Error, Position
using Bacchi.Syntax;

namespace Bacchi.Writer.C
{
    /** Writer used to write C source code to a file. */
    public class Writer: Visitor
    {
        private Symbols                _symbols;
        private System.IO.StreamWriter _writer;

        public Writer(string filename)
        {
            _writer = System.IO.File.CreateText(filename);
        }

        public void Close()
        {
            _writer.Close();
            _writer = null;
        }

        public void Sweep(Node[] nodes)
        {
            foreach (Node node in nodes)
                node.Visit(this);
        }

        public object Visit(Argument that)
        {
            /** \todo Determine if a value or reference parameter; the latter must be prefixed by an asterisk (*). */
            that.Value.Visit(this);

            return null;
        }

        public object Visit(ArrayExpression that)
        {
            /** \todo If an array slice, call memcpy(), otherwise use simple assignment. */
            that.Array.Visit(this);
            that.Index.Visit(this);

            return null;
        }

        public object Visit(ArrayReference that)
        {
            return null;
        }

        public object Visit(ArrayType that)
        {
            return null;
        }

        public object Visit(Assignment that)
        {
            return null;
        }

        public object Visit(BinaryExpression that)
        {
            return null;
        }

        public object Visit(Block that)
        {
            Sweep(that.Definitions);
            Sweep(that.Statements);

            return null;
        }

        public object Visit(BooleanDefinition that)
        {
            return null;
        }

        public object Visit(BooleanLiteral that)
        {
            return null;
        }

        public object Visit(BooleanType that)
        {
            return null;
        }

        public object Visit(CallStatement that)
        {
            return null;
        }

        public object Visit(ConstantDefinition that)
        {
            return null;
        }

        public object Visit(DoStatement that)
        {
            /** \note There are no relavant nodes below this node. */
            return null;
        }

        public object Visit(File that)
        {
            _writer.WriteLine("/* FILE: {0} */", that.Name);
            Sweep(that.Modules);

            return null;
        }

        public object Visit(ForallStatement that)
        {
            return null;
        }

        public object Visit(Guard that)
        {
            return null;
        }

        public object Visit(IdentifierExpression that)
        {
            return null;
        }

        public object Visit(IdentifierReference that)
        {
            return null;
        }

        public object Visit(IdentifierType that)
        {
            return null;
        }

        public object Visit(IfStatement that)
        {
            return null;
        }

        public object Visit(IntegerDefinition that)
        {
            return null;
        }

        public object Visit(IntegerExpression that)
        {
            return null;
        }

        public object Visit(IntegerLiteral that)
        {
            return null;
        }

        public object Visit(IntegerType that)
        {
            return null;
        }

        public object Visit(LetStatement that)
        {
            return null;
        }

        public object Visit(Module that)
        {
            _writer.WriteLine("/* MODULE: {0} */", that.Name);

            Sweep(that.Definitions);
            that.Block.Visit(this);

            return null;
        }

        public object Visit(ModuleIndexExpression that)
        {
            return null;
        }

        public object Visit(Parameter that)
        {
            return null;
        }

        public object Visit(ParenthesisExpression that)
        {
            return null;
        }

        public object Visit(ProcedureCompletion that)
        {
            return null;
        }

        public object Visit(ProcedureDeclaration that)
        {
            return null;
        }

        public object Visit(ProcedureDefinition that)
        {
            return null;
        }

        public object Visit(Program that)
        {
            /** Cache global symbol table locally. */
            _symbols = that.Symbols;

            _writer.WriteLine("/* DO NOT EDIT: This file was generated by Bacchi v0.01. */");
            _writer.WriteLine();

            foreach (File file in that.Files)
                file.Visit(this);

            _symbols = null;

            return null;
        }

        public object Visit(RangeType that)
        {
            return null;
        }

        public object Visit(ReadStatement that)
        {
            return null;
        }

        public object Visit(ReturnStatement that)
        {
            return null;
        }

        public object Visit(SkipStatement that)
        {
            return null;
        }

        public object Visit(StringLiteral that)
        {
            return null;
        }

        public object Visit(TupleDefinition that)
        {
            return null;
        }

        public object Visit(TupleExpression that)
        {
            return null;
        }

        public object Visit(TupleIndexExpression that)
        {
            return null;
        }

        public object Visit(TupleType that)
        {
            return null;
        }

        public object Visit(TypeDefinition that)
        {
            return null;
        }

        public object Visit(UnaryExpression that)
        {
            return null;
        }

        public object Visit(VariableDefinition that)
        {
            /** \todo Take special care to output a local structure for tuples and a proper array declaration for arrays. */
            return null;
        }

        public object Visit(WriteStatement that)
        {
            foreach (Expression expression in that.Expressions)
            {
                switch (expression.BaseType)
                {
                    case TypeKind.Boolean:
                        _writer.Write("    __gcl_write_boolean(");
                        expression.Visit(this);
                        _writer.WriteLine(");");
                        break;

                    case TypeKind.Integer:
                        _writer.Write("    _gcl_writer_integer(");
                        expression.Visit(this);
                        _writer.WriteLine(");");
                        break;

                    default:
                        throw new Error(expression.Position, 0, "Cannot output non-scalar values in 'write' statement");
                }
            }

            return null;
        }
    }
}

