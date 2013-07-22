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
 *  Defines the \c Node class, which is the base class of \b all Abstract Syntax Tree (AST) nodes in the system.
 */

using Bacchi.Kernel;                    // Error, Position, Tokens

namespace Bacchi.Syntax
{
    public abstract class Node
    {
        private static int _count = 0;
        private int _id = ++_count;
        /** Returns the unique node ID. */
        public int Id
        {
            get { return _id; }
        }

        private NodeKind _kind;
        /** Returns the node kind value. */
        public NodeKind Kind
        {
            get { return _kind; }
        }

        private Position _position;
        /** Returns the starting position of the construct that caused this node to be created. */
        public Position Position
        {
            get { return _position; }
        }

        private Node _above;
        /** Returns the parent node. \note This field is always \c null for the topmost \c Program node. */
        public Node Above
        {
            get { return _above; }
            set
            {
                if (_above != null)
                    throw new InternalError("Cannot redefine parent of node");
                if (value == null)
                    throw new InternalError("Cannot reset parent of node");
                _above = value;
            }
        }

        private Program _world;
        /** The topmost node in the system. */
        public Program World
        {
            get
            {
                if (_world == null)
                    _world = _above.World;
                return _world;
            }
        }

        public Node(NodeKind kind, Position position)
        {
            _kind     = kind;
            _position = new Position(position);     /** \note Make \b deep copy to avoid problems with updated references. */
        }

        public abstract object Visit(Visitor that);
    }
}
