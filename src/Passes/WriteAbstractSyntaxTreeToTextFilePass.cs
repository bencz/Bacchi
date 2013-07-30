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
 *  Defines the \c Visitor interface, which is used to traverse the Abstract Syntax Tree (AST).
 */

using System.Collections.Generic;       // Stack<T>

using Bacchi.Syntax;

namespace Bacchi.Passes
{
    /** Dumper used to dump the Abstract Syntax Tree (AST) to a text file. */
    public class WriteAbstractSyntaxTreeToTextFilePass: Visitor
    {
        private System.IO.StreamWriter _writer;
        private Stack<bool> _commas = new Stack<bool>();
        private bool        _comma = false;

        public WriteAbstractSyntaxTreeToTextFilePass(string filename)
        {
            _writer = System.IO.File.CreateText(filename);
            _writer.WriteLine("THIS (OVER) FIELDS");
        }

        private void Enter(Node that)
        {
            _commas.Push(_comma);
            _comma = false;

            string above = (that.Above != null) ? that.Above.Id.ToString("D4") : "none";
            _writer.Write("{1} ({0}) {2}: ", above, that.Id.ToString("D4"), that.Kind.ToString());
        }

        private void Leave(Node that)
        {
            _writer.WriteLine();
            _comma = _commas.Pop();
        }

        private void Print(string title, Node node)
        {
            if (_comma)
                _writer.Write(", ");
            _comma = true;
            _writer.Write("{0} = {1}", title, node.Id);
        }

        private void Print(string title, Node[] nodes)
        {
            if (_comma)
                _writer.Write(", ");
            _comma = true;
            _writer.Write("{0} = [ ", title);
            foreach (Node node in nodes)
            {
                _writer.Write(node.Id);
                if (node != nodes[nodes.Length - 1])
                    _writer.Write(", ");
            }
            if (nodes.Length > 0)
                _writer.Write(' ');
            _writer.Write(']');
        }

        private void Print(string title, string value, bool quote = false)
        {
            if (_comma)
                _writer.Write(", ");
            _comma = true;
            _writer.Write("{0} = ", title);
            if (quote)
                _writer.Write('\'');
            _writer.Write(value);
            if (quote)
                _writer.Write('\'');
        }

        private void Visit(Node node)
        {
            node.Visit(this);
        }

        private void Visit(Node[] nodes)
        {
            foreach (Node node in nodes)
                node.Visit(this);
        }

        /** Visitor methods defines in the \c Visitor interface. *****************************************************************/

        public void Visit(Argument that)
        {
            Enter(that);
            Print("Value", that.Value);
            Leave(that);

            Visit(that.Value);
        }

        public void Visit(ArrayExpression that)
        {
            Enter(that);
            Print("Array", that.Array);
            Print("Index", that.Index);
            Leave(that);

            Visit(that.Array);
            Visit(that.Index);
        }

        public void Visit(ArrayType that)
        {
            Enter(that);
            Print("Base", that.Base);
            Print("Name", that.Name);
            Leave(that);

            Visit(that.Base);
        }

        public void Visit(Assignment that)
        {
            Enter(that);
            Print("Variable", that.Variable);
            Print("Expression", that.Expression);
            Leave(that);

            Visit(that.Variable);
            Visit(that.Expression);
        }

        public void Visit(BinaryExpression that)
        {
            Enter(that);
            Print("Operator", that.Operator.ToString());
            Print("First", that.First);
            Print("Other", that.Other);
            Leave(that);

            Visit(that.First);
            Visit(that.Other);
        }

        public void Visit(Block that)
        {
            Enter(that);
            Print("Definitions", that.Definitions);
            Print("Statements", that.Statements);
            Leave(that);

            Visit(that.Definitions);
            Visit(that.Statements);
        }

        public void Visit(BooleanDefinition that)
        {
            Enter(that);
            Print("Name", that.Name);
            Leave(that);
        }

        public void Visit(BooleanLiteral that)
        {
            Enter(that);
            Print("Value", that.Value ? "true" : "false");
            Leave(that);
        }

        public void Visit(BooleanType that)
        {
            Enter(that);
            Leave(that);
        }

        public void Visit(CallStatement that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Arguments", that.Arguments);
            Leave(that);

            Visit(that.Arguments);
        }

        public void Visit(ConstantDefinition that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Literal", that.Literal);
            Leave(that);

            Visit(that.Literal);
        }

        public void Visit(DoStatement that)
        {
            Enter(that);
            Print("Guards", that.Guards);
            Leave(that);

            Visit(that.Guards);
        }

        public void Visit(File that)
        {
            Enter(that);
            Print("Name", that.Name, true);
            Print("Modules", that.Modules);
            Leave(that);

            Visit(that.Modules);
        }

        public void Visit(ForallStatement that)
        {
            Enter(that);
            Print("Variable", that.Variable);
            Print("Statements", that.Statements);
            Leave(that);

            Visit(that.Statements);
        }

        public void Visit(Guard that)
        {
            Enter(that);
            Print("Expression", that.Expression);
            Print("Statements", that.Statements);
            Leave(that);

            Visit(that.Expression);
            Visit(that.Statements);
        }

        public void Visit(IdentifierExpression that)
        {
            Enter(that);
            Print("Name", that.Name);
            Leave(that);
        }

        public void Visit(IdentifierType that)
        {
            Enter(that);
            Print("Name", that.Name);
            Leave(that);
        }

        public void Visit(IfStatement that)
        {
            Enter(that);
            Print("Guards", that.Guards);
            Leave(that);

            Visit(that.Guards);
        }

        public void Visit(IntegerDefinition that)
        {
            Enter(that);
            Print("Name", that.Name);
            Leave(that);
        }

        public void Visit(IntegerLiteral that)
        {
            Enter(that);
            Print("Value", that.Value.ToString());
            Leave(that);
        }

        public void Visit(IntegerType that)
        {
            Enter(that);
            Leave(that);
        }

        public void Visit(LetStatement that)
        {
            Enter(that);
            Print("Assignments", that.Assignments);
            Leave(that);

            Visit(that.Assignments);
        }

        public void Visit(Module that)
        {
            Enter(that);
            Print("Definitions", that.Definitions);
            Print("Block", that.Block);
            Leave(that);

            Visit(that.Definitions);
            Visit(that.Block);
        }

        public void Visit(ModuleIndexExpression that)
        {
            Enter(that);
            Print("Prefix", that.Prefix);
            Print("Field", that.Field);
            Leave(that);

            Visit(that.Prefix);
        }

        public void Visit(Parameter that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Mode", that.Mode.ToString());
            Print("Type", that.Type);
            Leave(that);

            Visit(that.Type);
        }

        public void Visit(ParenthesisExpression that)
        {
            Enter(that);
            Print("Expression", that.Expression);
            Leave(that);

            Visit(that.Expression);
        }

        public void Visit(ProcedureCompletion that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Block", that.Block);
            Leave(that);
        }

        public void Visit(ProcedureDeclaration that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Parameters", that.Parameters);
            Leave(that);

            Visit(that.Parameters);
        }

        public void Visit(ProcedureDefinition that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Parameters", that.Parameters);
            Print("Block", that.Block);
            Leave(that);

            Visit(that.Parameters);
            Visit(that.Block);
        }

        public void Visit(Program that)
        {
            try
            {
                Enter(that);
                Print("Files", that.Files);
                Leave(that);

                Visit(that.Files);
            }
            finally
            {
                _writer.Close();
                _writer = null;
            }
        }

        public void Visit(RangeType that)
        {
            Enter(that);
            Print("Type", that.Type);
            Print("Lower", that.Lower);
            Print("Upper", that.Upper);
            Leave(that);

            Visit(that.Type);
            Visit(that.Lower);
            Visit(that.Upper);
        }

        public void Visit(ReadStatement that)
        {
            Enter(that);
            Print("Variables", that.Variables);
            Leave(that);

            Visit(that.Variables);
        }

        public void Visit(ReturnStatement that)
        {
            Enter(that);
            Leave(that);
        }

        public void Visit(SkipStatement that)
        {
            Enter(that);
            Leave(that);
        }

        public void Visit(StringLiteral that)
        {
            Enter(that);
            Print("Value", that.Value, true);
            Leave(that);
        }

        public void Visit(TupleDefinition that)
        {
            Enter(that);
            Print("Types", that.Types);
            Leave(that);

            Visit(that.Types);
        }

        public void Visit(TupleExpression that)
        {
            Enter(that);
            Print("Expressions", that.Expressions);
            Leave(that);

            Visit(that.Expressions);
        }

        public void Visit(TupleIndexExpression that)
        {
            Enter(that);
            Print("Prefix", that.Prefix);
            Print("Index", that.Index.ToString());
            Leave(that);

            Visit(that.Prefix);
        }

        public void Visit(TupleType that)
        {
            Enter(that);
            Print("Types", that.Types);
            Leave(that);

            Visit(that.Types);
        }

        public void Visit(TypeDefinition that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Type", that.Type);
            Leave(that);

            Visit(that.Type);
        }

        public void Visit(UnaryExpression that)
        {
            Enter(that);
            Print("Operator", that.Operator.ToString());
            Print("Expression", that.Expression);
            Leave(that);

            Visit(that.Expression);
        }

        public void Visit(VariableDefinition that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Type", that.Type);
            Leave(that);
        }

        public void Visit(WriteStatement that)
        {
            Enter(that);
            Print("Expressions", that.Expressions);
            Leave(that);

            Visit(that.Expressions);
        }
    }
}

