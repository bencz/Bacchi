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

namespace Bacchi.Syntax
{
    /** Interface used by the Visitor pattern. */
    public interface Visitor
    {
        void Visit(ArrayIndexExpression that);
        void Visit(ArrayType that);
        void Visit(Assignment that);
        void Visit(BinaryExpression that);
        void Visit(Block that);
        void Visit(BooleanDefinition that);
        void Visit(BooleanLiteral that);
        void Visit(BooleanType that);
        void Visit(CallStatement that);
        void Visit(ConstantDefinition that);
        void Visit(DoStatement that);
        void Visit(File that);
        void Visit(ForallStatement that);
        void Visit(Guard that);
        void Visit(IdentifierExpression that);
        void Visit(IdentifierType that);
        void Visit(IfStatement that);
        void Visit(IntegerDefinition that);
        void Visit(IntegerLiteral that);
        void Visit(IntegerType that);
        void Visit(LetStatement that);
        void Visit(Module that);
        void Visit(ModuleIndexExpression that);
        void Visit(Parameter that);
        void Visit(ParenthesisExpression that);
        void Visit(ProcedureCompletion that);
        void Visit(ProcedureDeclaration that);
        void Visit(ProcedureDefinition that);
        void Visit(Program that);
        void Visit(RangeType that);
        void Visit(ReadStatement that);
        void Visit(ReturnStatement that);
        void Visit(SkipStatement that);
        void Visit(StringLiteral that);
        void Visit(TupleDefinition that);
        void Visit(TupleExpression that);
        void Visit(TupleIndexExpression that);
        void Visit(TupleType that);
        void Visit(TypeDefinition that);
        void Visit(UnaryExpression that);
        void Visit(VariableDefinition that);
        void Visit(WriteStatement that);
    }
}

