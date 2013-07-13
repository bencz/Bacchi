#region license
// Copyright (C) 2010-2013 Mikael Lyngvig (mikael@lyngvig.org).  All rights reserved.
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
 *  Implements the \c Position class, which represents a point in a source file.
 */

namespace Bacchi.Kernel
{
    /** Defines a source file position. */
    public class Position
    {
        /** The name of the source file (disk name). */
        private string _file;
        public string File
        {
            get { return _file; }
        }

        /** The line number in the source file (1-based). */
        private int _line;
        public int Line
        {
            get { return _line; }
            set { _line = value; }
        }

        /** The character index in the source line in the source file (1-based). */
        private int _char;
        public int Char
        {
            get { return _char; }
            set { _char = value; }
        }

        /** Constructor for the \c Position class. */
        public Position()
        {
            _file = null;
            _line = 0;
            _char = 0;
        }

        /** Constructor for the \c Position class. */
        public Position(Position position)
        {
            _file = position.File;
            _line = position.Line;
            _char = position.Char;
        }

        /** Constructor for the \c Position class. */
        public Position(string file)
        {
            _file = file;
            _line = 0;
            _char = 0;
        }

        /** Constructor for the \c Position class. */
        public Position(string file, int line, int @char)
        {
            _file = file;
            _line = line;
            _char = @char;
        }

        /** Move "cursor" (line/char) to the specified position. */
        public void Goto(int line, int @char)
        {
            _line = line;
            _char = @char;
        }

        /** Step forward one character. */
        public void NextChar()
        {
            _char += 1;
        }

        /** Step forward one line. */
        public void NextLine()
        {
            _line += 1;
            _char = 1;
        }

        /** Convert to human-readable format. */
        public override string ToString()
        {
            System.Text.StringBuilder text = new System.Text.StringBuilder(256);

            text.Append('(');
            if (File != null)
            {
                if (File.IndexOf(' ') != -1)
                    text.Append('"');
                text.Append(File);
                if (File.IndexOf(' ') != -1)
                    text.Append('"');
            }
            if (Line != 0)
            {
                text.Append(' ');
                text.Append(Line.ToString());
            }
            if (Char != 0)
            {
                text.Append(':');
                text.Append(Char.ToString());
            }
            text.Append(')');

            return text.ToString();
        }
    }
}

