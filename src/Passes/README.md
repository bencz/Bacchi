# Bacchi Compiler Passes #
The Bacchi compiler uses a number of passes, which must be executed in the order given below:

1. ResolveSymbolsPass: Assigns the `Definition` member of each named entity in the tree.
2. CheckStaticTypesPass: Checks that the source program conforms to GCL's static type rules.
3. WriteCPlusPlusSourcePass: Generates the output C++ source file.

The `WriteAbstractSyntaxTreePass` pass can be invoked whenever convenient although it is currently done as the very last pass so as to ensure that all synthetic fields in the tree have been updated properly.
