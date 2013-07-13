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
 *  Implements the \c Tokens class,, which keeps track of the current token and provides LL(n) look-ahead.
 */

namespace Bacchi.Kernel
{
    /**
     * Handles all state information related to matching and consuming tokens while parsing.
     */
    public class Tokens
    {
        private Token[] _tokens;        // array of all tokens in the input file
        private int    _index;          // index into \c _tokens
        private Token   _cache;         // cache of the current token

        /** Returns the current token in the token sequence. */
        public Token Peek
        {
            get { return _cache; }
        }

        /** Returns the next token in the token sequence, thus providing LL(1) lookahead. */
        public Token Next
        {
            get
            {
                // Return the end-of-file token on attempts to inspect past the end of the file.
                int length = _tokens.Length;
                if (_index + 1 >= length)
                    return _tokens[length - 1];

                // Return the next token.
                return _tokens[_index + 1];
            }
        }

        /** Array indexer - provides LL(k) look-ahead (rare in most grammars). */
        public Token this[int index]
        {
            get
            {
                // Compute position to look at.
                index += _index;

                // Ensure we're not reading before the start of the file.
                if (index < 0)
                    throw new InternalError("Attempt to read before the beginning of the input file");

                // If reading past the end, simply return the end-of-file token (the last token).
                if (index >= _tokens.Length)
                    index = _tokens.Length - 1;

                // Return the requested or adjusted token.
                return _tokens[index];
            }
        }

        /** Constructor for the \c Tokens class. */
        public Tokens(Token[] tokens)
        {
            // Safe guard against bogus clients.
            if (tokens.Length == 0)
                throw new InternalError("There must be at least an end-of-file token in the token sequence");

            // Set up instance.
            _tokens = tokens;
            _index  = 0;
            _cache  = _tokens[_index];
        }

        /** Matches (skips) a token of the specified kind.
         *
         *  \note Throws \c ParserError() on token mismatch.
         */
        public Token Match(TokenKind kind)
        {
            // Fail if and only if there's a token mismatch.
            if (_cache.Kind != kind)
                throw new ParserError(_cache.Position, 0, string.Format("Token mismatch; found {0}, expected {1}", _cache.Kind.Print(), kind.Print()));

            // Update the cache so as to avoid performing integrity checks over and over again.
            Token result = _cache;
            if (_index + 1 < _tokens.Length)
                _cache = _tokens[++_index];

            return result;
        }

        /** Unconditionally reads the next token.
         *
         *  \note If the next token is end-of-file, the cursor is not advanced so you can never read past the end.
         */
        public Token Read()
        {
            // Simply return the current token.
            Token result = _cache;

            // Advance the cursor, if not at the end-of-file token.
            if (_index < _tokens.Length)
                _cache = _tokens[++_index];

            return result;
        }
    }
}


