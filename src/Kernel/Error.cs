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
 *  Defines the \c Error exception class, which is used to report all user-related errors encountered while processing input.
 */

using System;
using System.Collections.Generic;

namespace Bacchi.Kernel
{
    /** Represents a single error detected while translating a Braceless source set.
     *  It is used to record all warnings and errors so as to be able to output them to a log file after the compilation is done.
     */
    public class Error: BaseError
    {
        private Position _position;
        /** The source file position of the warning/error/etc. */
        public Position Position
        {
            get { return _position; }
        }

        private int _code;
        /** The global error code that will be documented eventually. */
        public int Code
        {
            get { return _code; }
        }

        private string _text;
        /** The error message that was generated when the problem was detected. */
        public string Text
        {
            get { return _text; }
        }

        /** Constructor for the \c Error class. */
        public Error(Position position, int code, string text):
            base(text)
        {
            _position = position;
            _code = code;
            _text = text;
        }
    }
}
