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
 *  The different kinds of AST nodes that exist in the system.
 */

namespace Bacchi.Syntax
{
    /** A list of all the AST nodes that exist in the system. */
    public enum NodeKind
    {
        /** Reserved for variable initialization; it is invalid (as the name might suggest). */
        Invalid,

        Argument,
        Assignment,
        Block,
        File,
        Guard,
        Module,
        Parameter,
        Program,

        BooleanDefinition,
        ConstantDefinition,
        IntegerDefinition,
        /** A partial procedure definition including only the block (body) part. */
        ProcedureCompletion,
        /** A partial procedure definition including only the declaration (prototype) part. */
        ProcedureDeclaration,
        /** A complete procedure definition including declaration (prototype) and block (body).*/
        ProcedureDefinition,
        TupleDefinition,
        TypeDefinition,

        IntegerExpression,

        BooleanLiteral,
        IntegerLiteral,
        StringLiteral,

        CallStatement,
        DoStatement,
        ForallStatement,
        IfStatement,
        LetStatement,
        ReadStatement,
        ReturnStatement,
        SkipStatement,
        WriteStatement,

        /** A reference (assignable) to an array index. */
        ArrayReference,
        /** A reference (assignable) to a symbol. */
        IdentifierReference,

        BooleanType,
        IdentifierType,
        IntegerType,
        TupleType
    }
}
