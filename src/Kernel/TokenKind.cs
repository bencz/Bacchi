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
 *  The kinds of tokens that are known to the compiler.
 */

namespace Bacchi.Kernel
{
    public static class TokenKindExtensions
    {
        public static string Print(this TokenKind value)
        {
            string result;
            switch (value)
            {
                case TokenKind.None              : result = "(invalid token value)"; break;

                case TokenKind.Comment           : result = "comment"; break;
                case TokenKind.EndOfFile         : result = "end of file"; break;
                case TokenKind.Identifier        : result = "identifier"; break;
                case TokenKind.Integer           : result = "integer"; break;
                case TokenKind.String            : result = "string"; break;

                case TokenKind.Keyword_Array     : result = "keyword 'array'"; break;
                case TokenKind.Keyword_Boolean   : result = "keyword 'Boolean'"; break;
                case TokenKind.Keyword_Const     : result = "keyword 'const'"; break;
                case TokenKind.Keyword_Do        : result = "keyword 'do'"; break;
                case TokenKind.Keyword_End       : result = "keyword 'end'"; break;
                case TokenKind.Keyword_False     : result = "keyword 'false'"; break;
                case TokenKind.Keyword_Fi        : result = "keyword 'fi'"; break;
                case TokenKind.Keyword_Forall    : result = "keyword 'forall'"; break;
                case TokenKind.Keyword_If        : result = "keyword 'if'"; break;
                case TokenKind.Keyword_Integer   : result = "keyword 'integer'"; break;
                case TokenKind.Keyword_Llarof    : result = "keyword 'llarof'"; break;
                case TokenKind.Keyword_Module    : result = "keyword 'module'"; break;
                case TokenKind.Keyword_Od        : result = "keyword 'od'"; break;
                case TokenKind.Keyword_Private   : result = "keyword 'private'"; break;
                case TokenKind.Keyword_Proc      : result = "keyword 'proc'"; break;
                case TokenKind.Keyword_Range     : result = "keyword 'range'"; break;
                case TokenKind.Keyword_Read      : result = "keyword 'read'"; break;
                case TokenKind.Keyword_Ref       : result = "keyword 'ref'"; break;
                case TokenKind.Keyword_Return    : result = "keyword 'return'"; break;
                case TokenKind.Keyword_Skip      : result = "keyword 'skip'"; break;
                case TokenKind.Keyword_True      : result = "keyword 'true'"; break;
                case TokenKind.Keyword_Typedef   : result = "keyword 'typedef'"; break;
                case TokenKind.Keyword_Val       : result = "keyword 'val'"; break;
                case TokenKind.Keyword_Write     : result = "keyword 'write'"; break;

                case TokenKind.Operator_Add      : result = "operator '+'"; break;
                case TokenKind.Operator_And      : result = "operator '&'"; break;
                case TokenKind.Operator_Divide   : result = "operator '/'"; break;
                case TokenKind.Operator_Multiply : result = "operator '*'"; break;
                case TokenKind.Operator_Not      : result = "operator '~'"; break;
                case TokenKind.Operator_Or       : result = "operator '|'"; break;
                case TokenKind.Operator_Remainder: result = "operator '\\'"; break;
                case TokenKind.Operator_Subtract : result = "operator '-'"; break;

                case TokenKind.Relational_Difference  : result = "operator '#'"; break;
                case TokenKind.Relational_Equality    : result = "operator '='"; break;
                case TokenKind.Relational_GreaterEqual: result = "operator '>='"; break;
                case TokenKind.Relational_GreaterThan : result = "operator '>'"; break;
                case TokenKind.Relational_LessEqual   : result = "operator '<='"; break;
                case TokenKind.Relational_LessThan    : result = "operator '<'"; break;

                case TokenKind.Symbol_Assignment      : result = "assignment (:=)"; break;
                case TokenKind.Symbol_BracketBegin    : result = "left bracket ([)"; break;
                case TokenKind.Symbol_BracketClose    : result = "right bracket (])"; break;
                case TokenKind.Symbol_Brackets        : result = "brackets ([])"; break;
                case TokenKind.Symbol_Comma           : result = "comma (,)"; break;
                case TokenKind.Symbol_Dot             : result = "dot (.)"; break;
                case TokenKind.Symbol_Ellipsis        : result = "ellipsis (..)"; break;
                case TokenKind.Symbol_ParenthesisBegin: result = "left parenthesis (()"; break;
                case TokenKind.Symbol_ParenthesisClose: result = "right parenthesis ())"; break;
                case TokenKind.Symbol_Semicolon       : result = "semicolon (;)"; break;
                case TokenKind.Symbol_Then            : result = "symbol '->'"; break;

                default:
                    throw new System.Exception("Unknown token kind: " + value.ToString());
            }
            return result;
        }
    }

    public enum TokenKind
    {
        None,                           // (Reserved for variable initialization.)

        Comment,                        // -- ...
        EndOfFile,                      // EOF
        Identifier,                     // an identifier
        Integer,                        // +digit
        String,                         // "..." | '...'

        Keyword_Array,                  // "array"
        Keyword_Begin,                  // "begin"
        Keyword_Boolean,                // "Boolean"
        Keyword_Const,                  // "const"
        Keyword_Do,                     // "do"
        Keyword_End,                    // "end"
        Keyword_False,                  // "false"
        Keyword_Fi,                     // "fi"
        Keyword_Forall,                 // "forall"
        Keyword_If,                     // "if"
        Keyword_Integer,                // "integer"
        Keyword_Llarof,                 // "llarof"
        Keyword_Module,                 // "module"
        Keyword_Od,                     // "od"
        Keyword_Private,                // "private"
        Keyword_Proc,                   // "proc"
        Keyword_Range,                  // "range"
        Keyword_Read,                   // "read"
        Keyword_Ref,                    // "ref"
        Keyword_Return,                 // "return"
        Keyword_Skip,                   // "skip"
        Keyword_True,                   // "true"
        Keyword_Typedef,                // "typedef"
        Keyword_Val,                    // "val"
        Keyword_Write,                  // "write"

        Operator_Add,                   // +
        Operator_And,                   // &
        Operator_Divide,                // /
        Operator_Multiply,              // *
        Operator_Not,                   // ~
        Operator_Or,                    // |
        Operator_Remainder,             // \
        Operator_Subtract,              // -

        Relational_Difference,          // #
        Relational_Equality,            // =
        Relational_GreaterEqual,        // >=
        Relational_GreaterThan,         // >
        Relational_LessEqual,           // <=
        Relational_LessThan,            // <

        Symbol_Assignment,              // :=
        Symbol_BracketBegin,            // [
        Symbol_BracketClose,            // ]
        Symbol_Brackets,                // []
        Symbol_Comma,                   // ,
        Symbol_Dot,                     // .
        Symbol_Ellipsis,                // ..
        Symbol_ParenthesisBegin,        // (
        Symbol_ParenthesisClose,        // )
        Symbol_Semicolon,               // ;
        Symbol_Then                     // ->
    }
}

