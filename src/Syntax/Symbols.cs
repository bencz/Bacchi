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
 *  The symbol table passed from pass to pass, gradually being populated and attributed.
 */

using System.Collections.Generic;       // Dictionary<T1, T2>

using Bacchi.Kernel;                    // Error

namespace Bacchi.Syntax
{
    public class Symbols
    {
        private class NameToSymbolMap: Dictionary<string, Symbol>
        {
        }

        private Dictionary<string, NameToSymbolMap> _modules = new Dictionary<string, NameToSymbolMap>();
        private NameToSymbolMap                     _current;       // The current module.

        public void Dump(string filename)
        {
            using (var writer = System.IO.File.CreateText("program.sym"))
            {
                writer.WriteLine("Symbol table dump:");
                writer.WriteLine();

                foreach (string module in _modules.Keys)
                {
                    writer.WriteLine("Module {0}:", module);
                    foreach (string name in _modules[module].Keys)
                    {
                        // Don't dump the internal-use-only module symbol.
                        if (name.Length == 0)
                            continue;

                        Symbol symbol = _modules[module][name];
                        writer.WriteLine("    {0} {1} @{2}", symbol.Scope.ToString().ToLowerInvariant(), name, symbol.Definition.Id);
                    }
                    writer.WriteLine();
                }
            }
        }

        public void EnterModule(Module that)
        {
            if (_modules.ContainsKey(that.Name))
                throw new Error(that.Position, 0, "Module '" + that.Name + "' already defined");

            _current = new NameToSymbolMap();
            _modules[that.Name] = _current;
            _modules[that.Name][""] = new Symbol(that, ScopeKind.Global);
        }

        public void EnterModule(Module that, bool reusing)
        {
            if (!reusing)
            {
                EnterModule(that);
                return;
            }

            if (!_modules.ContainsKey(that.Name))
                throw new Error(that.Position, 0, "Module '" + that.Name + "' not defined");
            _current = _modules[that.Name];
        }

        public void LeaveModule(Module that)
        {
            _current = null;
        }

        public void Insert(Definition definition, ScopeKind scope)
        {
            // Check that the symbol doesn't already exist in the current scope.
            if (_current.ContainsKey(definition.Name))
                throw new Error(definition.Position, 0, "Symbol '" + definition.Name + "' already defined");

            // Insert the symbol into the current scope.
            Symbol symbol = new Symbol(definition, scope);
            _current[definition.Name] = symbol;
        }

        public Definition Lookup(string name)
        {
            // Try looking up the symbol in the current module.
            if (_current.ContainsKey(name))
                return _current[name].Definition;

            // Try looking up the symbol in the public section of previously defined modules.
            foreach (string key in _modules.Keys)
            {
                // Abort the search when we hit the current module.
                if (key == _current[""].Definition.Name)
                    break;

                // Continue looking if the module being examined does not define the symbol.
                if (!_modules[key].ContainsKey(name))
                    continue;

                // Don't return local symbols; they are private to the module in question.
                if (_modules[key][name].Scope == ScopeKind.Local)
                    continue;

                return _modules[key][name].Definition;
            }

            return null;
        }

        public Definition Lookup(Position position, string name)
        {
            var result = Lookup(name);
            if (result == null)
                throw new Error(position, 0, name);
            return result;
        }

        public Definition Lookup(Position position, string module_name, string name)
        {
            if (!_modules.ContainsKey(module_name))
                throw new Error(position, 0, "Module '" + module_name + "' unknown");

            NameToSymbolMap map = _modules[module_name];
            if (!map.ContainsKey(name) || map[name].Scope != ScopeKind.Global)
                throw new Error(position, 0, "Module '" + module_name + "' does not define the public symbol '" + name + "'");

            return map[name].Definition;
        }
    }
}
