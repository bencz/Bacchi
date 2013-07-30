# The Bacchi Source Tree #
To build Bacchi with Microsoft.NET v2.x+, do the following:

	csc Builder.cs
	Builder.exe

To build Bacchi with Mono v2.10+, do the following:

	mcs Builder.cs
	mono Builder.exe

To build the source code documentation using Doxygen, do the following:

	doxygen

**Note:** If you install `GraphViz` in your `PATH`, Doxygen will generate neat inheritance graphs.

## Learning Matters ##
This project is a tiny compiler project which was given to me as a voluntary assignment by the great master Alexandre :-)  He specifically asked me to write down the things I learned from this project:

1. Don't program in GCL; it is way too simple to accomplish anything significanly worthwhile.
2. Don't aim for the skies in the first release; plan ahead a few years and control your ambitions.
3. Type checking is not very difficult once you get the basic idea.  It is just a few more passes, that's all.
4. Symbol resolution is actually rather straightforward, but **do not** go the lazy way around and try to avoid making a permanent link from all AST nodes for all applied occurences of an identifier to the symbol definition.  Either you do it the right way from the beginning or you'll have to discard and rewrite code as I had to do.
5. Alexandre is a genius!  He has been telling me for years that I made the problem too complex to handle.
6. **Do not** (like I did) distribute the parser in the AST nodes; it makes it very difficult for newcomers to understand what's going on and it also requires changing many files when a design error has crept in.
7. **Do not** make a differentiation between left-hand-sides and right-hand-sides of assignments in the AST; it is way easier to treat everything as mere expressions and then later on check if a given left-hand-side is a valid left-hand-side or not.

