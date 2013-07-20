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
        object Visit(Argument that);
        object Visit(ArrayExpression that);
        object Visit(ArrayReference that);
        object Visit(ArrayType that);
        object Visit(Assignment that);
        object Visit(BinaryExpression that);
        object Visit(Block that);
        object Visit(BooleanDefinition that);
        object Visit(BooleanLiteral that);
        object Visit(BooleanType that);
        object Visit(CallStatement that);
        object Visit(ConstantDefinition that);
        object Visit(DoStatement that);
        object Visit(File that);
        object Visit(ForallStatement that);
        object Visit(Guard that);
        object Visit(IdentifierExpression that);
        object Visit(IdentifierReference that);
        object Visit(IdentifierType that);
        object Visit(IfStatement that);
        object Visit(IntegerDefinition that);
        object Visit(IntegerExpression that);
        object Visit(IntegerLiteral that);
        object Visit(IntegerType that);
        object Visit(LetStatement that);
        object Visit(MemberExpression that);
        object Visit(Module that);
        object Visit(Parameter that);
        object Visit(ParenthesisExpression that);
        object Visit(ProcedureCompletion that);
        object Visit(ProcedureDeclaration that);
        object Visit(ProcedureDefinition that);
        object Visit(Program that);
        object Visit(RangeType that);
        object Visit(ReadStatement that);
        object Visit(ReturnStatement that);
        object Visit(SkipStatement that);
        object Visit(StringLiteral that);
        object Visit(TupleDefinition that);
        object Visit(TupleExpression that);
        object Visit(TupleIndexExpression that);
        object Visit(TupleType that);
        object Visit(TypeDefinition that);
        object Visit(UnaryExpression that);
        object Visit(VariableDefinition that);
        object Visit(WriteStatement that);
    }
}

