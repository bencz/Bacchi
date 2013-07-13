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
