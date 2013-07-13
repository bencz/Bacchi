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
 *  Defines the \c Program node, which represents an entire multi-source GCL program.
 */

using System.Collections.Generic;       // List<T>

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public class Program: Node
    {
        private File[] _files;
        public File[] Files
        {
            get { return _files; }
        }

        public Program(File[] files):
            base(NodeKind.Program, new Position())
        {
            _files = files;
            foreach (File file in _files)
                file.Above = this;
        }

        public static Program Parse(string[] filenames)
        {
            // Process each input file in turn.
            var files = new List<File>();
            foreach (string filename in filenames)
            {
                // Scan the file (convert it into tokens aka words).
                Scanner scanner = new Scanner(filename);
                Tokens tokens = new Tokens(scanner.Tokens);

                // Parse the file (convert the tokens into an Abstract Syntax Tree).
                File file = File.Parse(tokens);
                files.Add(file);
            }

            return new Program(files.ToArray());
        }

        public override object Visit(Visitor that)
        {
            return that.Visit(this);
        }
    }
}
