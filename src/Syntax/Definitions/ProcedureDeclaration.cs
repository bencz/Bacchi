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
 *  Defines the class \c ProcedureDeclaration, which represents a single declaration of a procedure (a prototype).
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    /** Class which represents a single procedure interface declaration. */
    public class ProcedureDeclaration: Definition
    {
#region Literal attributes
        private Parameter[] _parameters;
        public Parameter[] Parameters
        {
            get { return _parameters; }
        }
#endregion

#region Synthetic attributes
        public override TypeKind BaseType
        {
            get { return TypeKind.Procedure; }
        }
#endregion

        /** Constructor for the \c ProcedureDeclaration class. */
        public ProcedureDeclaration(Position position, string name, Parameter[] parameters):
            base(NodeKind.ProcedureDeclaration, position, name)
        {
            _parameters = parameters;
            foreach (Parameter parameter in _parameters)
                parameter.Above = this;
        }

        /** \note The \c Parse() method is found in \c ProcedureDefinition.Parse(). */

        /** Implements the Visitor pattern. */
        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}

