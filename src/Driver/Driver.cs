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
 *  The main driver program for the Bacchi compiler.
 */

using System.Collections.Generic;       // Dictionary<T1, T2>

using Bacchi.Kernel;                    // Error
using Bacchi.Passes;                    // XxxPass
using Bacchi.Syntax;                    // Program

namespace Bacchi.Driver
{
    public static class Application
    {
        public static int Main(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                System.Console.Write("Bacchi v0.01");
#if TEST
                System.Console.Write(" (TEST)");
#endif
                System.Console.WriteLine(" - http://github.com/bencz/Bacchi");
                System.Console.WriteLine("Copyright (c) 2013 Mikael Lyngvig.  All rights reserved.");
                System.Console.WriteLine();
                System.Console.WriteLine("Syntax: \"Bacchi.exe\" +filename");
                return 1;
            }

            int result;
            try
            {
                // Parse the input files into a single, coherent Abstract Syntax Tree (AST).
                Program program = Program.Parse(arguments);

                // Create list of passes to go through.
                List<Visitor> passes = new List<Visitor>();

                // Link up each applied occurence of a symbol with its type definition.
                passes.Add(new ResolveSymbolsPass());

                // Enforce the static type rules of the language while creating and propagating attributes in the tree.
                passes.Add(new CheckStaticTypesPass());

                // Create a global struct for each anonymous tuple in the source program.
                passes.Add(new ConvertTuplesToStructsPass());

                // Generate the C++ output.
                passes.Add(new WriteCPlusPlusSourcePass("program"));

#if TEST
                // Dump AST to 'program.ast' file.
                passes.Add(new WriteAbstractSyntaxTreeToTextFilePass("program.ast"));
#endif

                // Perform the multi-pass compilation.
                foreach (Visitor pass in passes)
                {
                    pass.Visit(program);
                }

#if TEST
                // Dump symbol table to 'program.sym' file.
                program.Symbols.Dump("program.sym");
#endif

                result = 0;
            }
            catch (Error that)
            {
                System.Console.WriteLine(that.Position.ToString() + " Error: " + that.Message);
                result = 1;
#if TEST
                throw;
#endif
            }
            catch (System.Exception that)
            {
                System.Console.WriteLine("Error: " + that.Message);
                result = 1;
#if TEST
                throw;
#endif
            }

            return result;
        }
    }
}

