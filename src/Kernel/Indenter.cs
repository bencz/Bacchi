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
 *  Defines the \c Indenter class, which handles indentation issues in output text files.
 */

using System;                           // IDisposable

namespace Bacchi.Kernel
{
    /** The \c Indenter class transparently prefixes indentation so that the client does not need to worry about this. */
    public class Indenter
    {
        /** The stream to write to. */
        private System.IO.StreamWriter _writer;
        /** The current level of indentation (0 = none). */
        private int _level = 0;
        /** Tracks whether we're at the beginning of a line so that indentation needs to be output. */
        private bool _newline = true;

        /** The size of each indent (in spaces). */
        private string _indent = "    ";
        public int Size
        {
            get { return _indent.Length; }
            set { _indent = new string(' ', value); }
        }

        /** Constructor for the \c Indenter class. */
        public Indenter(string filename, System.Text.Encoding encoding)
        {
            _writer = new System.IO.StreamWriter(filename, false, encoding);
        }

        ~Indenter()
        {
            if (_writer != null)
                throw new InternalError("Bacchi.Kernel.Indenter.Close() not called!");
        }

        public void Close()
        {
            if (_writer != null)
            {
                _writer.Close();
                _writer = null;
            }
            _indent = null;
        }

        /** Helper function that writes \c _level levels of indentation if at the beginning of a new line. */
        private void WriteIndent()
        {
            if (!_newline)
                return;

            _newline = false;
            for (int i = 0; i < _level; i += 1)
            {
                _writer.Write(_indent);
            }
        }

        /** Increments the number of levels of indentation to be output. */
        public void Indent()
        {
            _level += 1;
        }

        /** Decrements the number of levels of indentation to be output. */
        public void Dedent()
        {
            if (_level <= 0)
                throw new InternalError("Dedent() without Indent()");
            _level -= 1;
        }

#if false
        public void Start(string text)
        {
            _writer.Write(text);
        }

        public void StartLine(string text)
        {
            _writer.WriteLine(text);
        }
#endif

        /** Writes the specified character after having ensured the line is properly indented. */
        public void Write(char text)
        {
            WriteIndent();
            _writer.Write(text);
        }

        /** Writes the specified string after having ensured the line is properly indented. */
        public void Write(string text)
        {
            WriteIndent();
            _writer.Write(text);
        }

        /** Writes the specified character after having ensured the line is properly indented and then terminates the line. */
        public void WriteLine(char text)
        {
            WriteIndent();
            _newline = true;
            _writer.WriteLine(text);
        }

        /** Writes the specified string after having ensured the line is properly indented and then terminates the line. */
        public void WriteLine(string text)
        {
            WriteIndent();
            _newline = true;
            _writer.WriteLine(text);
        }

        /** Writes a linefeed character after having ensured the line is properly indented. */
        public void WriteLine()
        {
            /* technically speaking, this call to \c WriteIndent() is superflous, but it harms nobody and it is consistent. */
            WriteIndent();
            _newline = true;
            _writer.WriteLine();
        }

        public void WriteLine(string format, object arg1)
        {
            WriteIndent();
            _newline = true;
            _writer.WriteLine(format, arg1);
        }

        public void WriteLine(string format, object arg1, object arg2)
        {
            WriteIndent();
            _newline = true;
            _writer.WriteLine(format, arg1, arg2);
        }

        public void WriteLine(string format, object arg1, object arg2, object arg3)
        {
            WriteIndent();
            _newline = true;
            _writer.WriteLine(format, arg1, arg2, arg3);
        }

        public void WriteLine(string format, object arg1, object arg2, object arg3, object arg4)
        {
            WriteIndent();
            _newline = true;
            _writer.WriteLine(format, arg1, arg2, arg3, arg4);
        }
    }
}
