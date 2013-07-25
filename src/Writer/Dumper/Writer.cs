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

namespace Bacchi.Writer.Dumper
{
    /** Dumper used to dump the Abstract Syntax Tree (AST) to a file. */
    public class Writer: Visitor
    {
        private System.IO.StreamWriter _writer;
        private Stack<bool> _commas = new Stack<bool>();
        private bool        _comma = false;

        public Writer(string filename)
        {
            _writer = System.IO.File.CreateText(filename);
            _writer.WriteLine("THIS (OVER) FIELDS");
        }

        public void Close()
        {
            _writer.Close();
            _writer = null;
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

            return;
        }

        public void Visit(ArrayExpression that)
        {
            Enter(that);
            Print("Array", that.Array);
            Print("Index", that.Index);
            Leave(that);

            Visit(that.Array);
            Visit(that.Index);

            return;
        }

        public void Visit(ArrayReference that)
        {
            Enter(that);
            Print("Reference", that.Reference);
            Print("Expression", that.Expression);
            Leave(that);

            Visit(that.Reference);
            Visit(that.Expression);

            return;
        }

        public void Visit(ArrayType that)
        {
            Enter(that);
            Print("Base", that.Base);
            Print("Name", that.Name);
            Leave(that);

            Visit(that.Base);

            return;
        }

        public void Visit(Assignment that)
        {
            Enter(that);
            Print("Reference", that.Reference);
            Print("Expression", that.Expression);
            Leave(that);

            Visit(that.Reference);
            Visit(that.Expression);

            return;
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

            return;
        }

        public void Visit(Block that)
        {
            Enter(that);
            Print("Definitions", that.Definitions);
            Print("Statements", that.Statements);
            Leave(that);

            Visit(that.Definitions);
            Visit(that.Statements);

            return;
        }

        public void Visit(BooleanDefinition that)
        {
            Enter(that);
            Print("Name", that.Name);
            Leave(that);

            return;
        }

        public void Visit(BooleanLiteral that)
        {
            Enter(that);
            Print("Value", that.Value ? "true" : "false");
            Leave(that);

            return;
        }

        public void Visit(BooleanType that)
        {
            Enter(that);
            Leave(that);

            return;
        }

        public void Visit(CallStatement that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Arguments", that.Arguments);
            Leave(that);

            Visit(that.Arguments);

            return;
        }

        public void Visit(ConstantDefinition that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Literal", that.Literal);
            Leave(that);

            Visit(that.Literal);

            return;
        }

        public void Visit(DoStatement that)
        {
            Enter(that);
            Print("Guards", that.Guards);
            Leave(that);

            Visit(that.Guards);

            return;
        }

        public void Visit(File that)
        {
            Enter(that);
            Print("Name", that.Name, true);
            Print("Modules", that.Modules);
            Leave(that);

            Visit(that.Modules);

            return;
        }

        public void Visit(ForallStatement that)
        {
            Enter(that);
            Print("Variable", that.Variable);
            Print("Statements", that.Statements);
            Leave(that);

            Visit(that.Statements);

            return;
        }

        public void Visit(Guard that)
        {
            Enter(that);
            Print("Expression", that.Expression);
            Print("Statements", that.Statements);
            Leave(that);

            Visit(that.Expression);
            Visit(that.Statements);

            return;
        }

        public void Visit(IdentifierExpression that)
        {
            Enter(that);
            Print("Name", that.Name);
            Leave(that);

            return;
        }

        public void Visit(IdentifierReference that)
        {
            Enter(that);
            Print("Name", that.Name);
            Leave(that);

            return;
        }

        public void Visit(IdentifierType that)
        {
            Enter(that);
            Print("Name", that.Name);
            Leave(that);

            return;
        }

        public void Visit(IfStatement that)
        {
            Enter(that);
            Print("Guards", that.Guards);
            Leave(that);

            Visit(that.Guards);

            return;
        }

        public void Visit(IntegerDefinition that)
        {
            Enter(that);
            Print("Name", that.Name);
            Leave(that);

            return;
        }

        public void Visit(IntegerLiteral that)
        {
            Enter(that);
            Print("Value", that.Value.ToString());
            Leave(that);

            return;
        }

        public void Visit(IntegerType that)
        {
            Enter(that);
            Leave(that);

            return;
        }

        public void Visit(LetStatement that)
        {
            Enter(that);
            Print("Assignments", that.Assignments);
            Leave(that);

            Visit(that.Assignments);

            return;
        }

        public void Visit(Module that)
        {
            Enter(that);
            Print("Definitions", that.Definitions);
            Print("Block", that.Block);
            Leave(that);

            Visit(that.Definitions);
            Visit(that.Block);

            return;
        }

        public void Visit(ModuleIndexExpression that)
        {
            Enter(that);
            Print("Prefix", that.Prefix);
            Print("Field", that.Field);
            Leave(that);

            Visit(that.Prefix);

            return;
        }

        public void Visit(Parameter that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Mode", that.Mode.ToString());
            Print("Type", that.Type);
            Leave(that);

            Visit(that.Type);

            return;
        }

        public void Visit(ParenthesisExpression that)
        {
            Enter(that);
            Print("Expression", that.Expression);
            Leave(that);

            Visit(that.Expression);

            return;
        }

        public void Visit(ProcedureCompletion that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Block", that.Block);
            Leave(that);

            return;
        }

        public void Visit(ProcedureDeclaration that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Parameters", that.Parameters);
            Leave(that);

            Visit(that.Parameters);

            return;
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

            return;
        }

        public void Visit(Program that)
        {
            Enter(that);
            Print("Files", that.Files);
            Leave(that);

            Visit(that.Files);

            return;
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

            return;
        }

        public void Visit(ReadStatement that)
        {
            Enter(that);
            Print("References", that.References);
            Leave(that);

            Visit(that.References);

            return;
        }

        public void Visit(ReturnStatement that)
        {
            Enter(that);
            Leave(that);

            return;
        }

        public void Visit(SkipStatement that)
        {
            Enter(that);
            Leave(that);

            return;
        }

        public void Visit(StringLiteral that)
        {
            Enter(that);
            Print("Value", that.Value, true);
            Leave(that);

            return;
        }

        public void Visit(TupleDefinition that)
        {
            Enter(that);
            Print("Types", that.Types);
            Leave(that);

            Visit(that.Types);

            return;
        }

        public void Visit(TupleExpression that)
        {
            Enter(that);
            Print("Expressions", that.Expressions);
            Leave(that);

            Visit(that.Expressions);

            return;
        }

        public void Visit(TupleIndexExpression that)
        {
            Enter(that);
            Print("Prefix", that.Prefix);
            Print("Index", that.Index.ToString());
            Leave(that);

            Visit(that.Prefix);

            return;
        }

        public void Visit(TupleType that)
        {
            Enter(that);
            Print("Types", that.Types);
            Leave(that);

            Visit(that.Types);

            return;
        }

        public void Visit(TypeDefinition that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Type", that.Type);
            Leave(that);

            Visit(that.Type);

            return;
        }

        public void Visit(UnaryExpression that)
        {
            Enter(that);
            Print("Operator", that.Operator.ToString());
            Print("Expression", that.Expression);
            Leave(that);

            Visit(that.Expression);

            return;
        }

        public void Visit(VariableDefinition that)
        {
            Enter(that);
            Print("Name", that.Name);
            Print("Type", that.Type);
            Leave(that);

            return;
        }

        public void Visit(WriteStatement that)
        {
            Enter(that);
            Print("Expressions", that.Expressions);
            Leave(that);

            Visit(that.Expressions);

            return;
        }
    }
}

