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
 *  Defines the layout and contents of a single token.
 */

namespace Bacchi.Kernel
{
    /** A single token as read from an input file. */
    public class Token
    {
        private string _text;
        /** The token text string. */
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private TokenKind _kind;
        /** The token kind. */
        public TokenKind Kind
        {
            get { return _kind; }
            set { _kind = value; }
        }

        private Position _position;
        /** The starting character and line number of the token. */
        public Position Position
        {
            get { return _position; }
        }

        /** Default constructor for the \c Token class. */
        public Token()
        {
            _position = new Position();
        }

        /** Clone constructor for the \c Token class. */
        public Token(Position position)
        {
            /** \note Make a \b deep copy of all fields so as to avoid various issues with dangling references. */
            _position = new Position(position.File, position.Line, position.Char);
        }

        /** Converts a token into a human-readable string. */
        public override string ToString()
        {
            return string.Format("{0}: {1}", _kind.ToString(), _text);
        }
    }
}

