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
 *  Defines the C++ source code writer.
 *
 *  The transformation of a GCL program to a C++ program is roughly as follows:
 *
 *      1. For each module, create a corresponding class.
 *      2. For each public symbol, create a corresponding public member.
 *      3. For each private symbol, create a corresponding private member.
 *      4. For each module code block, create a corresponding default constructor.
 *      5. For each tuple, a global structure definition is made and then reused whenever another tuple matches its layout.
 *
 *  I initially tried to write a C code generator, but found that it made things too complicated for my taste.
 *
 *  \note This pass assumes that the Checker pass has already been run.
 *  \note This pass assumes that the \c ConvertTupleToStructPass has already been run.
 */

using Bacchi.Kernel;                    // Error, Position
using Bacchi.Syntax;

namespace Bacchi.Passes
{
    /** Writer used to write C++ source code to a file. */
    public class WriteCPlusPlusSourcePass: Visitor
    {
        private Symbols  _symbols;
        private Indenter _writer;

        public WriteCPlusPlusSourcePass(string basename)
        {
            _writer = new Indenter(basename + ".cpp", System.Text.Encoding.ASCII);
        }

        private string FindTupleType(TupleType type)
        {
            for (int i = 0; i < type.World.Tuples.Length; i++)
            {
                TupleType tuple = type.World.Tuples[i];

                if (type.Compare(tuple))
                    return "tuple" + (i + 1).ToString();
            }

            throw new InternalError("Could not find tuple definition");
        }

        public void Visit(Node node)
        {
            node.Visit(this);
        }

        public void Visit(Node[] nodes)
        {
            foreach (Node node in nodes)
                node.Visit(this);
        }

        public void Visit(Argument that)
        {
            /** \todo Determine if a value or reference parameter; the latter must sometimes be prefixed by an ampersand (&). */
            Visit(that.Value);
        }

        public void Visit(ArrayExpression that)
        {
            /** \todo If an array slice, call memcpy(), otherwise use simple assignment. */
            Visit(that.Array);
            _writer.Write('[');
            Visit(that.Index);
            _writer.Write(']');
        }

        public void Visit(ArrayReference that)
        {
            Visit(that.Reference);
            _writer.Write('[');
            Visit(that.Expression);
            _writer.Write(']');
        }

        public void Visit(ArrayType that)
        {
            Visit(that.Base);
        }

        public void Visit(Assignment that)
        {
            Visit(that.Reference);
            _writer.Write(" = ");
            Visit(that.Expression);
            _writer.WriteLine(';');
        }

        public void Visit(BinaryExpression that)
        {
            Visit(that.First);
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
            Visit(that.Other);
        }

        public void Visit(Block that)
        {
            Visit(that.Definitions);
            Visit(that.Statements);
        }

        public void Visit(BooleanDefinition that)
        {
            _writer.Write("bool ");
            _writer.Write(that.Name);
            _writer.WriteLine(';');
        }

        public void Visit(BooleanLiteral that)
        {
            _writer.Write(that.Value ? "true" : "false");
        }

        public void Visit(BooleanType that)
        {
            _writer.Write("bool");
        }

        public void Visit(CallStatement that)
        {
            _writer.Write(that.Name);
            _writer.Write('(');
            foreach (Argument argument in that.Arguments)
            {
                Visit(argument);
                if (argument != that.Arguments[that.Arguments.Length - 1])
                    _writer.Write(", ");
            }
            _writer.WriteLine(");");
        }

        public void Visit(ConstantDefinition that)
        {
            _writer.Write("const ");
            switch (that.Literal.BaseType)
            {
                case TypeKind.Boolean: _writer.Write("bool "); break;
                case TypeKind.Integer: _writer.Write("int "); break;
                default              : throw new System.ArgumentException("that.Literal.BaseType");
            }
            _writer.Write(that.Name);
            _writer.Write(" = ");
            Visit(that.Literal);
            _writer.WriteLine(';');
        }

        public void Visit(DoStatement that)
        {
            _writer.WriteLine("for (;;)");
            _writer.WriteLine('{');
            _writer.Indent();
            foreach (Guard guard in that.Guards)
            {
                if (guard != that.Guards[0])
                    _writer.Write("else ");
                _writer.Write("if (");
                Visit(guard.Expression);
                _writer.WriteLine(')');
                _writer.WriteLine('{');
                _writer.Indent();
                foreach (Statement statement in guard.Statements)
                    Visit(statement);
                _writer.Dedent();
                _writer.WriteLine('}');
            }
            _writer.WriteLine("else");
            _writer.Indent();
            _writer.WriteLine("break;");
            _writer.Dedent();
            _writer.Dedent();
            _writer.WriteLine('}');
        }

        public void Visit(File that)
        {
            _writer.WriteLine("/******* STANDARD ENVIRONMENT *********/");
            _writer.WriteLine("extern int  __gcl_integer_parse(void);");
            _writer.WriteLine("extern void __gcl_integer_print(int value);");
            _writer.WriteLine("extern void __gcl_string_print(const char *value);");
            _writer.WriteLine();

            _writer.WriteLine("/* SOURCE FILE: {0} */", that.Name);
            Visit(that.Modules);
        }

        public void Visit(ForallStatement that)
        {
        }

        public void Visit(Guard that)
        {
        }

        public void Visit(IdentifierExpression that)
        {
            _writer.Write(that.Name);
        }

        public void Visit(IdentifierReference that)
        {
            _writer.Write(that.Name);
        }

        public void Visit(IdentifierType that)
        {
            _writer.Write(that.Name);
        }

        public void Visit(IfStatement that)
        {
            for (int i = 0; i < that.Guards.Length; i++)
            {
                Guard guard = that.Guards[i];

                if (i == 0)
                    _writer.Write("if (");
                else
                    _writer.Write("else if (");
                Visit(guard.Expression);
                _writer.WriteLine(')');
                _writer.WriteLine('{');
                _writer.Indent();
                Visit(guard.Statements);
                _writer.Dedent();
                _writer.WriteLine('}');
            }
        }

        public void Visit(IntegerDefinition that)
        {
            _writer.Write("int ");
            _writer.Write(that.Name);
            _writer.WriteLine(';');
        }

        public void Visit(IntegerLiteral that)
        {
            _writer.Write(that.Value.ToString());
        }

        public void Visit(IntegerType that)
        {
            _writer.Write("int");
        }

        public void Visit(LetStatement that)
        {
            /** \todo Generate a thread-safe procedure for each assignment and launch them as separate threads. */
            if (that.Assignments.Length > 1)
                _writer.WriteLine("/* The next {0} statements are supposed to execute in parallel: */", that.Assignments.Length);
            Visit(that.Assignments);
        }

        public void Visit(Module that)
        {
            that.World.Symbols.EnterModule(that, true);

            _writer.WriteLine("/* SOURCE MODULE: {0} */", that.Name);

            _writer.Write("class _module_");
            _writer.WriteLine(that.Name);
            _writer.WriteLine('{');

            // Output public definitions (including the default constructor that we create).
            _writer.WriteLine("public:");
            _writer.Indent();
            _writer.Write("_module_");
            _writer.Write(that.Name);
            _writer.WriteLine("();");
            _writer.WriteLine();
            Visit(that.Definitions);
            _writer.Dedent();

            // Output private definitions in private part of class.
            _writer.WriteLine("private:");
            _writer.Indent();
            Visit(that.Block.Definitions);
            _writer.Dedent();
            _writer.WriteLine("};");
            _writer.WriteLine();

            _writer.Write("_module_");
            _writer.Write(that.Name);
            _writer.Write("::_module_");
            _writer.Write(that.Name);
            _writer.WriteLine("()");
            _writer.WriteLine('{');
            _writer.Indent();
            Visit(that.Block.Statements);
            _writer.Dedent();
            _writer.WriteLine("};");
            _writer.WriteLine();

            that.World.Symbols.LeaveModule(that);
        }

        public void Visit(ModuleIndexExpression that)
        {
            Visit(that.Prefix);
            _writer.Write("__");
            _writer.Write(that.Field);
        }

        public void Visit(Parameter that)
        {
            Visit(that.Type);
            _writer.Write(' ');
            if (that.Mode == ModeKind.Reference)
                _writer.Write('*');
            _writer.Write(that.Name);
        }

        public void Visit(ParenthesisExpression that)
        {
            _writer.Write('(');
            Visit(that.Expression);
            _writer.Write(')');
        }

        public void Visit(ProcedureCompletion that)
        {
        }

        public void Visit(ProcedureDeclaration that)
        {
        }

        public void Visit(ProcedureDefinition that)
        {
            _writer.Write("void ");
            _writer.Write(that.Name);
            _writer.Write('(');
            foreach (Parameter parameter in that.Parameters)
            {
                Visit(parameter);
                if (parameter != that.Parameters[that.Parameters.Length - 1])
                    _writer.Write(", ");
            }
            _writer.WriteLine(')');
            _writer.WriteLine('{');
            _writer.Indent();
            Visit(that.Block);
            _writer.Dedent();
            _writer.WriteLine('}');
        }

        public void Visit(Program that)
        {
            try
            {
                /** Cache global symbol table locally. */
                _symbols = that.Symbols;

                _writer.WriteLine("/* DO NOT EDIT: This file was generated by Bacchi v0.01. */");
                _writer.WriteLine();

                /** Output the previously collected structure definitions. */
                for (int i = 0; i < that.Tuples.Length; i++)
                {
                    TupleType tuple = that.Tuples[i];

                    _writer.Write("typedef ");
                    _writer.Write("struct { ");
                    for (int j = 0; j < tuple.Types.Length; j++)
                    {
                        Type type = tuple.Types[j];
                        Visit(type);
                        _writer.Write(" _");
                        _writer.Write((j + 1).ToString());
                        _writer.Write("; ");
                    }
                    _writer.Write(" }");
                    _writer.Write(' ');
                    _writer.Write("tuple" + (i + 1).ToString());
                    _writer.WriteLine(';');
                }
                _writer.WriteLine();

                Visit(that.Files);

                _writer.WriteLine("int main(void)");
                _writer.WriteLine('{');
                _writer.Indent();
                /** \note Create an instance of each module's class so as to ensure that the code executes as per the specification. */
                foreach (File file in that.Files)
                {
                    foreach (Module module in file.Modules)
                    {
                        _writer.Write("_module_");
                        _writer.Write(module.Name);
                        _writer.Write(" *_module_");
                        _writer.Write(module.Name);
                        _writer.Write("_instance = new _module_");
                        _writer.Write(module.Name);
                        _writer.WriteLine("();");
                    }
                }

                _writer.WriteLine("return 0;");
                _writer.Dedent();
                _writer.WriteLine('}');
            }
            finally
            {
                _writer.Close();
                _writer = null;
            }

            _symbols = null;
        }

        public void Visit(RangeType that)
        {
            Visit(that.Type);
        }

        public void Visit(ReadStatement that)
        {
            foreach (Reference reference in that.References)
            {
                Visit(reference);
                _writer.WriteLine(" = __gcl_integer_parse();");
            }
        }

        public void Visit(ReturnStatement that)
        {
            _writer.WriteLine("return;");
        }

        public void Visit(SkipStatement that)
        {
            _writer.WriteLine("(void) 0;");
        }

        public void Visit(StringLiteral that)
        {
            _writer.Write('"');
            _writer.Write(that.Value);
            _writer.Write('"');
        }

        public void Visit(TupleDefinition that)
        {
            _writer.Write("struct { ");
            for (int i = 0; i < that.Types.Length; i++)
            {
                Type type = that.Types[i];
                Visit(type);
                _writer.Write(" _");
                _writer.Write((i + 1).ToString());
                _writer.Write("; ");
            }
            _writer.Write(" } ");
            _writer.Write(that.Name);
            _writer.WriteLine(';');
        }

        public void Visit(TupleExpression that)
        {
            _writer.Write("{ ");
            foreach (Expression expression in that.Expressions)
            {
                Visit(expression);

                if (expression != that.Expressions[that.Expressions.Length - 1])
                    _writer.Write(", ");
            }
            _writer.Write(" }");
        }

        public void Visit(TupleIndexExpression that)
        {
            Visit(that.Prefix);
            _writer.Write("._");
            _writer.Write(that.Index.ToString());
        }

        public void Visit(TupleType that)
        {
            _writer.Write(FindTupleType(that));
        }

        public void Visit(TypeDefinition that)
        {
            _writer.Write("typedef ");
            Visit(that.Type);
            _writer.Write(' ');
            _writer.Write(that.Name);

            /** Arrays must be output carefully because C++ expects the array size AFTER the array name. */
            if (that.Type.Kind == NodeKind.ArrayType)   /** \todo Support multi-dimensional arrays. */
            {
                // Look up array bounds and output them inline without calling the visitor method for ArrayType.
                ArrayType array = (ArrayType) that.Type;
                Definition definition = that.World.Symbols.Lookup(array.Name);
                if (definition == null)
                    throw new InternalError("Checker should have caught this undefined symbol: " + array.Name);
                if (definition.Kind != NodeKind.TypeDefinition)
                    throw new InternalError("There's something spooky here...");

                TypeDefinition typedef = (TypeDefinition) definition;
                RangeType range = (RangeType) typedef.Type;
                int lower = range.Lower.ConstantExpression;
                int upper = range.Upper.ConstantExpression;
                int width = (upper - lower + 1);
                _writer.Write('[');
                _writer.Write(width.ToString());
                _writer.Write(']');
            }

            _writer.WriteLine(';');
        }

        public void Visit(UnaryExpression that)
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
            Visit(that.Expression);
        }

        public void Visit(VariableDefinition that)
        {
            /** \todo Take special care to output a local structure for tuples and a proper array declaration for arrays. */
            _writer.Write(that.Type);
            _writer.Write(' ');
            _writer.Write(that.Name);
            _writer.WriteLine(';');
        }

        public void Visit(WriteStatement that)
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
        }
    }
}

