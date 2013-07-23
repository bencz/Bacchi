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
        private Symbols  _symbols;
        private Indenter _writer;

        public Writer(string filename)
        {
            _writer = new Indenter(filename, System.Text.Encoding.ASCII);
        }

        public void Close()
        {
            if (_writer == null)
                return;

            _writer.Close();
            _writer = null;
        }

        public void Sweep(Node node)
        {
            node.Visit(this);
        }

        public void Sweep(Node[] nodes)
        {
            foreach (Node node in nodes)
                node.Visit(this);
        }

        public object Visit(Argument that)
        {
            /** \todo Determine if a value or reference parameter; the latter must sometimes be prefixed by an ampersand (&). */
            Sweep(that.Value);

            return null;
        }

        public object Visit(ArrayExpression that)
        {
            /** \todo If an array slice, call memcpy(), otherwise use simple assignment. */
            Sweep(that.Array);
            _writer.Write('[');
            Sweep(that.Index);
            _writer.Write(']');

            return null;
        }

        public object Visit(ArrayReference that)
        {
            Sweep(that.Reference);
            _writer.Write('[');
            Sweep(that.Expression);
            _writer.Write(']');
            return null;
        }

        public object Visit(ArrayType that)
        {
            /** \todo ArrayType must be handled by the parent node as the type and the name must be output. */
            return null;
        }

        public object Visit(Assignment that)
        {
            Sweep(that.Reference);
            _writer.Write(" = ");
            Sweep(that.Expression);
            _writer.WriteLine(';');
            return null;
        }

        public object Visit(BinaryExpression that)
        {
            Sweep(that.First);
            string value;
            switch (that.Operator)
            {
                case BinaryKind.Add         : value = "+"; break;
                case BinaryKind.And         : value = "&"; break;
                case BinaryKind.Divide      : value = "/"; break;
                case BinaryKind.Multiply    : value = "*"; break;
                case BinaryKind.Or          : value = "|"; break;
                case BinaryKind.Remainder   : value = "%"; break;
                case BinaryKind.Equality    : value = "=="; break;
                case BinaryKind.Difference  : value = "!="; break;
                case BinaryKind.GreaterEqual: value = ">="; break;
                case BinaryKind.GreaterThan : value = ">"; break;
                case BinaryKind.LessEqual   : value = "<="; break;
                case BinaryKind.LessThan    : value = "<"; break;
                default                     : throw new System.ArgumentException("that.Operator");
            }
            _writer.Write(' ');
            _writer.Write(value);
            _writer.Write(' ');
            Sweep(that.Other);
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
            _writer.Write("bool ");
            _writer.Write(that.Name);
            _writer.WriteLine(';');
            return null;
        }

        public object Visit(BooleanLiteral that)
        {
            _writer.Write(that.Value ? "true" : "false");
            return null;
        }

        public object Visit(BooleanType that)
        {
            _writer.Write("bool");
            return null;
        }

        public object Visit(CallStatement that)
        {
            _writer.Write(that.Name);
            _writer.Write('(');
            foreach (Argument argument in that.Arguments)
            {
                Sweep(argument);
                if (argument != that.Arguments[that.Arguments.Length - 1])
                    _writer.Write(", ");
            }
            _writer.WriteLine(");");
            return null;
        }

        public object Visit(ConstantDefinition that)
        {
            return null;
        }

        public object Visit(DoStatement that)
        {
            _writer.WriteLine("for (;;)");
            _writer.WriteLine('{');
            _writer.Indent();
            foreach (Guard guard in that.Guards)
            {
                if (guard != that.Guards[0])
                    _writer.Write("else ");
                _writer.Write("if (");
                Sweep(guard.Expression);
                _writer.WriteLine(')');
                _writer.WriteLine('{');
                _writer.Indent();
                foreach (Statement statement in guard.Statements)
                    Sweep(statement);
                _writer.Dedent();
                _writer.WriteLine('}');
            }
            _writer.WriteLine("else");
            _writer.Indent();
            _writer.WriteLine("break;");
            _writer.Dedent();
            _writer.Dedent();
            _writer.WriteLine('}');
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
            _writer.Write(that.Name);
            return null;
        }

        public object Visit(IdentifierReference that)
        {
            _writer.Write(that.Name);
            return null;
        }

        public object Visit(IdentifierType that)
        {
            return null;
        }

        public object Visit(IfStatement that)
        {
            for (int i = 0; i < that.Guards.Length; i++)
            {
                Guard guard = that.Guards[i];

                if (i == 0)
                    _writer.Write("if (");
                else
                    _writer.Write("else if (");
                Sweep(guard.Expression);
                _writer.WriteLine(')');
                _writer.WriteLine('{');
                _writer.Indent();
                Sweep(guard.Statements);
                _writer.Dedent();
                _writer.WriteLine('}');
            }
            return null;
        }

        public object Visit(IntegerDefinition that)
        {
            _writer.Write("int ");
            _writer.Write(that.Name);
            _writer.WriteLine(';');
            return null;
        }

        public object Visit(IntegerLiteral that)
        {
            _writer.Write(that.Value.ToString());
            return null;
        }

        public object Visit(IntegerType that)
        {
            _writer.Write("int");
            return null;
        }

        public object Visit(LetStatement that)
        {
            /** \todo Generate a thread-safe procedure for each assignment and launch them as separate threads. */
            if (that.Assignments.Length > 1)
                _writer.WriteLine("/* The next {0} statements are supposed to execute in parallel: */", that.Assignments.Length);
            Sweep(that.Assignments);
            return null;
        }

        public object Visit(Module that)
        {
            that.World.Symbols.EnterModule(that, true);

            _writer.WriteLine("/* MODULE: {0} */", that.Name);

            Sweep(that.Definitions);
            Sweep(that.Block);

            that.World.Symbols.LeaveModule(that);

            return null;
        }

        public object Visit(ModuleIndexExpression that)
        {
            Sweep(that.Prefix);
            _writer.Write("__");
            _writer.Write(that.Field);
            return null;
        }

        public object Visit(Parameter that)
        {
            Sweep(that.Type);
            _writer.Write(' ');
            if (that.Mode == ModeKind.Reference)
                _writer.Write('*');
            _writer.Write(that.Name);
            return null;
        }

        public object Visit(ParenthesisExpression that)
        {
            _writer.Write('(');
            Sweep(that.Expression);
            _writer.Write(')');
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
            _writer.Write("void ");
            _writer.Write(that.Name);
            _writer.Write('(');
            foreach (Parameter parameter in that.Parameters)
            {
                Sweep(parameter);
                if (parameter != that.Parameters[that.Parameters.Length - 1])
                    _writer.Write(", ");
            }
            _writer.WriteLine(')');
            _writer.WriteLine('{');
            _writer.Indent();
            Sweep(that.Block);
            _writer.Dedent();
            _writer.WriteLine('}');

            return null;
        }

        public object Visit(Program that)
        {
            /** Cache global symbol table locally. */
            _symbols = that.Symbols;

            _writer.WriteLine("/* DO NOT EDIT: This file was generated by Bacchi v0.01. */");
            _writer.WriteLine();

            Sweep(that.Files);

            _symbols = null;

            return null;
        }

        public object Visit(RangeType that)
        {
            Sweep(that.Type);
            return null;
        }

        public object Visit(ReadStatement that)
        {
            foreach (Reference reference in that.References)
            {
                Sweep(reference);
                _writer.WriteLine(" = __gcl_integer_parse();");
            }
            return null;
        }

        public object Visit(ReturnStatement that)
        {
            _writer.WriteLine("return;");
            return null;
        }

        public object Visit(SkipStatement that)
        {
            _writer.WriteLine("(void) 0;");
            return null;
        }

        public object Visit(StringLiteral that)
        {
            _writer.Write('"');
            _writer.Write(that.Value);
            _writer.Write('"');
            return null;
        }

        public object Visit(TupleDefinition that)
        {
            _writer.Write("struct { ");
            for (int i = 0; i < that.Types.Length; i++)
            {
                Type type = that.Types[i];
                Sweep(type);
                _writer.Write(" _");
                _writer.Write((i + 1).ToString());

                if (i != that.Types.Length - 1)
                    _writer.Write("; ");
            }
            _writer.Write(" } ");
            _writer.Write(that.Name);
            _writer.WriteLine(';');
            return null;
        }

        public object Visit(TupleExpression that)
        {
            _writer.Write("{ ");
            foreach (Expression expression in that.Expressions)
            {
                Sweep(expression);

                if (expression != that.Expressions[that.Expressions.Length - 1])
                    _writer.Write(", ");
            }
            _writer.Write(" }");
            return null;
        }

        public object Visit(TupleIndexExpression that)
        {
            Sweep(that.Prefix);
            _writer.Write("._");
            _writer.Write(that.Index.ToString());
            return null;
        }

        public object Visit(TupleType that)
        {
            _writer.Write("struct { ");
            for (int i = 0; i < that.Types.Length; i++)
            {
                Type type = that.Types[i];
                Sweep(type);
                _writer.Write(" _");
                _writer.Write((i + 1).ToString());

                if (i != that.Types.Length - 1)
                    _writer.Write("; ");
            }
            _writer.Write(" }");

            return null;
        }

        public object Visit(TypeDefinition that)
        {
            return null;
        }

        public object Visit(UnaryExpression that)
        {
            switch (that.Operator)
            {
                case UnaryKind.Minus:
                    _writer.Write('-');
                    break;

                case UnaryKind.Not:
                    switch (that.BaseType)
                    {
                        case TypeKind.Boolean:
                            _writer.Write('!');
                            break;

                        case TypeKind.Integer:
                            _writer.Write('~');
                            break;

                        default:
                            throw new System.ArgumentException("that.BaseType");
                    }
                    break;

                case UnaryKind.Plus:
                    _writer.Write('+');
                    break;

                default:
                    throw new System.ArgumentException("that.Operator");
            }
            Sweep(that.Expression);

            return null;
        }

        public object Visit(VariableDefinition that)
        {
            /** \todo Take special care to output a local structure for tuples and a proper array declaration for arrays. */
            _writer.Write(that.Type);
            _writer.Write(' ');
            _writer.Write(that.Name);
            _writer.WriteLine(';');
            return null;
        }

        public object Visit(WriteStatement that)
        {
            foreach (Expression expression in that.Expressions)
            {
                switch (expression.BaseType)
                {
                    case TypeKind.Boolean:
                        _writer.Write("__gcl_boolean_print(");
                        break;

                    case TypeKind.Integer:
                        _writer.Write("__gcl_integer_print(");
                        break;

                    case TypeKind.String:
                        _writer.Write("__gcl_string_print(");
                        break;

                    default:
                        throw new Error(expression.Position, 0, "Cannot output non-scalar values in 'write' statement: " + expression.Id.ToString());
                }
                expression.Visit(this);
                _writer.WriteLine(");");
            }

            return null;
        }
    }
}

