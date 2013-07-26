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
            int result;
            try
            {
                // Parse the input files into a single, coherent Abstract Syntax Tree (AST).
                Program program = Program.Parse(arguments);

#if TEST
                // Dump AST to 'program.ast' file.
                var dumper = new Bacchi.Writer.Dumper.Writer("program.ast");
                try
                {
                    dumper.Visit(program);
                }
                finally
                {
                    dumper.Close();
                }
#endif
                // Fill out the inherited and synthetic attributes in the AST.
                Visitor[] passes = new Visitor[]
                {
                    new PopulateSymbolTablePass(),
                    new CheckStaticTypesPass()
                };
                foreach (Visitor pass in passes)
                {
                    pass.Visit(program);
                }

#if TEST
                // Dump symbol table to 'program.sym' file.
                var symbol_dumper = System.IO.File.CreateText("program.sym");
                program.Symbols.Dump(symbol_dumper);
                symbol_dumper.Close();
#endif

                var writer = new Bacchi.Writer.CPlusPlus.Writer("program");
                try
                {
                    writer.Visit(program);
                }
                finally
                {
                    writer.Close();
                }

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

