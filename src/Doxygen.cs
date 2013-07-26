/** \file
 *  Various document comments relating strictly to the Doxygen generated documentation in ../obj/docs/html/index.html.
 *
 *  To update the Doxygen documentation, ensure both Doxygen v1.8.4+ and GraphViz v2.26.3+ are in your PATH and then simply invoke
 *  Doxygen from the directory that contains this file.
 */

/** \mainpage Bacchi: A Guarded Command Language v7 Compiler.
 *
 *  \section intro Introduction
 *  Bacchi is an experimental toy compiler for version 7 of the Guarded Command Language (GCL).
 *
 *  \section language Programming Language
 *  Bacchi is written exclusively in C# because C# is a mostly no-nonsense, sane, and sensible language as Borland would put it.
 *
 *  \section srcdoc Source Code Documentation
 *  The source code documentation that you are currently reading is generated automatically by the Doxygen program available from
 *  http://www.doxygen.org.
 *
 *  If parts of the code or documentation seem unclear to you, please tell me (Mikael Lyngvig, mikael@lyngvig.org) to fix it.
 */

/* Below follows namespace (Doxygen: Package) documentation, which is nowhere else to be found. */

/** The root namespace, which contains \b all other symbols in the compiler. */
namespace Bacchi
{
    /** The \c Builder namespace is used solely for the ad-hoc build tool, which supports .NET and Mono. */
    namespace Builder
    {
    }

    /** The \c Kernel namespace contains stuff that's being shared by all components of the compiler. */
    namespace Kernel
    {
    }

    /** The \c Passes namspace contains the list of all available passes in the compiler. */
    namespace Passes
    {
    }

    /** The \c Syntax namespace includes everything related to the internal syntactical representation of a source program. */
    namespace Syntax
    {
    }
}

