# Bacchi #
Compiler development based in GCL syntax (GUARDED COMMAND LANGUAGE).

## Notes ##
This implementation deviates from the standard definition in the following ways:

1. Definitions only allow a single identifier so it is **not** possible to define multiple variables in a single definition in the `definitionPart` of a `module` or a `block`.

## Usability ##
Bacchi does **NOT** work yet!  It barely runs, but only barely so.  It cannot parse the sample yet.

## Terminology ##
The terminology is pretty straightforward except that *I* call nodes that are created as a direct result of a source code construct for "literal nodes".  This because they represent the source text literally.  This makes three classes of AST attributes:

1. Literal attributes (aka source code attributes), which directly represents a source code construct.
2. Inherited attributes, which are attributes that are inherited from somewhere higher up in the AST.
3. Synthesic attributes, which are attributes that are generated from nodes somewhere lower in the tree.
