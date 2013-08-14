/** \file
 *  Main task list (to-do list).
 *
 *  \todo Register the GCL standard environment (Boolean, integer, string, false, true) prior to resolving symbols.
 *  \todo Eliminate the \c Syntax/Definition subdirectory as a definition \b ought to be a name and a type.
 *  \todo Ensure that the resolver correctly determines the type of all known symbols - either a type or \c ErrorType.
 *  \todo Drop the idea of using the symbol table for on-going symbol lookup; instead use a reference in each applied occurence.
 *  \todo Fix the problem that partial procedures are not handled at all.
 *  \todo Fix the problem that parameter names are \b not entered into the symbol table.
 *  \todo Generate a single, global structure definition for each tuple layout encountered (requires a separate pass).
 *  \todo Take the address of \c ref arguments when passing them into the called procedure.
 *  \todo If an assignment goes to a \c ref argument, prefix it with asterisk (*).
 *  \todo Clean up partially redundant definitions of \c TupleDefinition and \c TupleType.
 *  \todo Add a recursive \c Compare() method that compares two sub-trees recursively.
 *  \todo Create a list of all the things I've learned from this project for Alexandre.
 */

