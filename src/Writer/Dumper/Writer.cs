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

using Bacchi.Syntax;

namespace Bacchi.Writer
{
    /** Dumper used to dump the Abstract Syntax Tree (AST) to a file. */
    public class Dumper: Visitor
    {
        private System.IO.StreamWriter _writer;

        public Dumper(string filename)
        {
            _writer = System.IO.File.CreateText(filename);
        }

        public void Close()
        {
            _writer.Close();
            _writer = null;
        }

        public void Enter(Node that)
        {
            string above = (that.Above != null) ? that.Above.Id.ToString("D4") : "    ";
            System.Console.Write("{0} {1}  ", above, that.Id.ToString("D4"));
        }

        public void Leave(Node that)
        {
            System.Console.WriteLine();
            System.Console.WriteLine();
        }

        public void Print(string title, Node[] nodes)
        {
            System.Console.Write("{0} = [ ", title);
            foreach (Node node in nodes)
            {
                System.Console.Write(node.Id);
                if (node != nodes[nodes.Length - 1])
                    System.Console.Write(", ");
            }
            System.Console.Write(']');
        }

        public void Sweep(Node[] nodes)
        {
            foreach (Node node in nodes)
                node.Visit(this);
        }

        public object Visit(Argument that)
        {
            Enter(that);
            System.Console.Write("Value = {0}", that.Value.Id);
            Leave(that);

            that.Value.Visit(this);

            return null;
        }

        public object Visit(ArrayExpression that)
        {
            Enter(that);
            System.Console.Write("Array = {0}, Index = {1}", that.Array.Id, that.Index.Id);
            Leave(that);

            that.Array.Visit(this);
            that.Index.Visit(this);

            return null;
        }

        public object Visit(ArrayReference that)
        {
            Enter(that);
            System.Console.Write("Reference = {0}, Expression = {1}", that.Reference.Id, that.Expression.Id);
            Leave(that);

            return null;
        }

        public object Visit(ArrayType that)
        {
            Enter(that);
            System.Console.Write("Base = {0}, Name = {1}", that.Base.Id, that.Name);
            Leave(that);

            return null;
        }

        public object Visit(Assignment that)
        {
            Enter(that);
            System.Console.Write("Reference = {0}, Expression = {1}", that.Reference.Id, that.Expression.Id);
            Leave(that);

            return null;
        }

        public object Visit(BinaryExpression that)
        {
            Enter(that);
            System.Console.Write(
                "Operator = {0}, First = {1}, Other = {2}",
                that.Operator.ToString(),
                that.First.Id,
                that.Other.Id
            );
            Leave(that);

            return null;
        }

        public object Visit(Block that)
        {
            Enter(that);
            Print("Definitions", that.Definitions);
            System.Console.Write(" ");
            Print("Statements", that.Statements);
            Leave(that);

            Sweep(that.Definitions);
            Sweep(that.Statements);

            return null;
        }

        public object Visit(BooleanLiteral that)
        {
            Enter(that);
            System.Console.Write("Value = {0}", that.Value ? "true" : "false");
            Leave(that);

            return null;
        }

        public object Visit(BooleanType that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(CallStatement that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(Definition that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(File that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(ForallStatement that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(Guard that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(IdentifierExpression that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(IdentifierReference that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(IdentifierType that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(IntegerExpression that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(IntegerLiteral that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(IntegerType that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(MemberExpression that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(Module that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(Parameter that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(ParenthesisExpression that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(Program that)
        {
            Enter(that);
            Leave(that);

            foreach (File file in that.Files)
                file.Visit(this);

            return null;
        }

        public object Visit(RangeType that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(Statement that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(StringLiteral that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(TupleExpression that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(TupleIndexExpression that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(TupleType that)
        {
            Enter(that);
            Leave(that);

            return null;
        }

        public object Visit(UnaryExpression that)
        {
            Enter(that);
            Leave(that);

            return null;
        }
    }
}

