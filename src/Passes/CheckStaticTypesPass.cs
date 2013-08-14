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
 *  Defines the \c CheckStaticTypesPass class, which traverses the AST and checks if all staticically types items are valid.
 */

using Bacchi.Kernel;                    // Error
using Bacchi.Syntax;

namespace Bacchi.Passes
{
    /** Checks the static type constraints of the GCL language. */
    public class CheckStaticTypesPass: Visitor
    {
        private Symbols _symbols;

        public CheckStaticTypesPass()
        {
        }

        public void Visit(Node node)
        {
            node.Visit(this);
        }

        public void Visit(Node[] nodes)
        {
            foreach (Node node in nodes)
                node.Visit(this);
        }

        public void Visit(ArrayIndexExpression that)
        {
            /** \todo Check that the array index type matches the base type of array expression. */
        }

        public void Visit(ArrayType that)
        {
            /** \todo Check that the array's base type is either Boolean or integer. */
        }

        public void Visit(Assignment that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(BinaryExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(Block that)
        {
            foreach (Definition definition in that.Definitions)
                definition.Visit(this);
            /** \note Don't visit the statements as they cannot create new symbols. */
        }

        public void Visit(BooleanDefinition that)
        {
        }

        public void Visit(BooleanLiteral that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(BooleanType that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(CallStatement that)
        {
            /** \todo Fetch the \c ProcedureDefinition for the called procedure and check the arguments one by one. */
            Definition procdef = _symbols.Lookup(that.Name);
        }

        public void Visit(ConstantDefinition that)
        {
        }

        public void Visit(DoStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(File that)
        {
            if (that.Modules.Length == 0)
                throw new Error(that.Position, 0, "No modules found in file");

            Visit(that.Modules);
        }

        public void Visit(ForallStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(Guard that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(IdentifierExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(IdentifierType that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(IfStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(IntegerDefinition that)
        {
        }

        public void Visit(IntegerLiteral that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(IntegerType that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(LetStatement that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(Module that)
        {
            _symbols.EnterModule(that, true);

            Visit(that.Definitions);
            if (that.Block != null)
                that.Block.Visit(this);

            _symbols.LeaveModule(that);
        }

        public void Visit(ModuleIndexExpression that)
        {
            // Check that the module name is in fact a module name.
            if (that.Prefix.Kind != NodeKind.IdentifierExpression)
                throw new InternalError("Parser delivered broken AST: ModuleIndexExpression.Prefix is not an identifier");

            string module = ((IdentifierExpression) that.Prefix).Name;
            Definition definition = that.World.Symbols.Lookup(module);
            if (definition == null)
                throw new Error(that.Prefix.Position, 0, "Module '" + module + "' not found");
            if (definition.Kind != NodeKind.Module)
                throw new Error(that.Prefix.Position, 0, "Expected a module name");

            // Check that the symbol name actually exists in the module.
            definition = that.World.Symbols.Lookup(that.Position, module, that.Field);
            if (definition == null)
                throw new Error(that.Position, 0, "Symbol '" + that.Field + "' not found in module '" + module + "'");
        }

        public void Visit(Parameter that)
        {
        }

        public void Visit(ParenthesisExpression that)
        {
            /** \note There are no relevant nodes below this node. */
        }

        public void Visit(ProcedureCompletion that)
        {
            Node definition = _symbols.Lookup(that.Name);
            if (definition == null)
                throw new Error(that.Position, 0, "Cannot complete undeclared procedure '" + that.Name + "'");
        }

        public void Visit(ProcedureDeclaration that)
        {
        }

        public void Visit(ProcedureDefinition that)
        {
        }

        public void Visit(Program that)
        {
            _symbols = that.Symbols;

            if (that.Files.Length == 0)
                throw new Error(that.Position, 0, "No source files found");

            foreach (File file in that.Files)
                file.Visit(this);

            _symbols = null;
        }

        public void Visit(RangeType that)
        {
            // Check that the lower bound is a boolean or integer constant.
            if (!that.Lower.IsConstant || (that.Lower.BaseType != TypeKind.Boolean && that.Lower.BaseType != TypeKind.Integer))
                throw new Error(that.Lower.Position, 0, "Expected a boolean or integer constant as the lower bound of range");

            // Check that the upper bound is an integer constant.
            if (!that.Upper.IsConstant || (that.Upper.BaseType != TypeKind.Boolean && that.Upper.BaseType != TypeKind.Integer))
                throw new Error(that.Upper.Position, 0, "Expected a boolean or integer constant as the upper bound of range");

#if false
            /** \todo Make 2nd+ checker pass check if range bounds are sensible. */
            // Check that the lower bound is less than the upper bound.
            int lower = that.Lower.Evaluate();
            int upper = that.Lower.Evaluate();
            if (lower >= upper)
                throw new Error(that.Lower.Position, 0, "Lower bound is greater than or equal to upper bound");
#endif

            // Visit children.
            Visit(that.Lower);
            Visit(that.Upper);
        }

        public void Visit(ReadStatement that)
        {
#if false
            // Check that the variables are all of integer type (they may be array members, though).
            foreach (Reference reference in that.References)
            {
                if (reference.BaseType != TypeKind.Integer)
                    throw new Error(reference.Position, 0, "Expected integer variable to input into");
            }
#endif

            // Visit children.
            Visit(that.Variables);
        }

        public void Visit(ReturnStatement that)
        {
            /** \note There are no relevant checks on this node. */
        }

        public void Visit(SkipStatement that)
        {
            /** \note There are no relevant checks on this node. */
        }

        public void Visit(StringLiteral that)
        {
            /** \note There are no relevant checks on this node. */
        }

        public void Visit(TupleDefinition that)
        {
            // Check that the tuple's fields are boolean or integer as they cannot be other types.
            foreach (Type type in that.Types)
            {
                if (type.BaseType != TypeKind.Boolean && type.BaseType != TypeKind.Integer)
                    throw new Error(type.Position, 0, "Expected boolean or integer field in tuple type");
            }

            // Visit children.
            Visit(that.Types);
        }

        public void Visit(TupleExpression that)
        {
            // Check that the tuple's expressions are either boolean or integer as they cannot be other types.
            foreach (Expression expression in that.Expressions)
            {
                if (expression.BaseType != TypeKind.Boolean && expression.BaseType != TypeKind.Integer)
                    throw new Error(expression.Position, 0, "Expected boolean or integer value in tuple expression");
            }

            // Visit children.
            Visit(that.Expressions);
        }

        public void Visit(TupleIndexExpression that)
        {
            // Check that the indexed expression is indeed a tuple expression.
            if (that.Prefix.Kind != NodeKind.IdentifierExpression)
                throw new Error(that.Prefix.Position, 0, "Expected a tuple variable name");
            if (that.Prefix.BaseType != TypeKind.Tuple)
                throw new Error(that.Prefix.Position, 0, "Expected a variable of tuple type");

            string name = ((IdentifierExpression) that.Prefix).Name;
            Definition definition = that.World.Symbols.Lookup(name);
            if (definition == null)
                throw new Error(that.Prefix.Position, 0, "Symbol not found: " + name);

            // Check that the tuple index is valid (within range).
            if (definition.Kind != NodeKind.TupleDefinition)
                throw new Error(that.Prefix.Position, 0, "Expected a variable of tuple type");
            TupleDefinition tuple = (TupleDefinition) definition;
            if (that.Index <= 0 || that.Index > tuple.Types.Length)
                throw new Error(that.Prefix.Position, 0, "Tuple index outside valid range");

            // Visit children.
            Visit(that.Prefix);
        }

        public void Visit(TupleType that)
        {
            // Visit children.
            Visit(that.Types);
        }

        public void Visit(TypeDefinition that)
        {
            /** \todo (Low) Check if a similar type definition exists already elsewhere and warn the user if so. */

            // Visit children.
            Visit(that.Type);
        }

        public void Visit(UnaryExpression that)
        {
            // Check that the expression is either boolean or integer.
            switch (that.Expression.BaseType)
            {
                case TypeKind.Boolean:
                case TypeKind.Integer:
                    break;

                default:
                    throw new Error(that.Expression.Position, 0, "Expected boolean or integer expression");
            }

            // Visit children.
            Visit(that.Expression);
        }

        public void Visit(VariableDefinition that)
        {
            // Check that the type name is indeed a typedefed name.
            Definition definition = that.World.Symbols.Lookup(that.Type);
            if (definition == null)
                throw new Error(that.Position, 0, "Unknown type: " + that.Type);
            if (definition.Kind != NodeKind.TypeDefinition)
                throw new Error(that.Position, 0, "Expected typedef name");

            // No children to visit.
        }

        public void Visit(WriteStatement that)
        {
            // Check that the arguments to 'write' are all booleans, integers, or strings.
            foreach (Expression expression in that.Expressions)
            {
                switch (expression.BaseType)
                {
                    case TypeKind.Boolean:
                    case TypeKind.Integer:
                    case TypeKind.String:
                        continue;

                    default:
                        throw new Error(expression.Position, 0, "Expected boolean, integer, or string for output by 'write'");
                }
            }

            // Visit children.
            Visit(that.Expressions);
        }
    }
}


