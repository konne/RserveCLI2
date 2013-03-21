RserveCLI2
----------
- This is a fork of RserveCLI, by Oliver M. Haynold which is hosted here: https://rservecli.codeplex.com
- Forked at commit 14449
- I wanted to add features and fix bugs unencumbered by the author's bandwidth
- I'm targeting Visual Studio 2010 and the 4.0 framework (dropping Mono Develop and pre-4.0)

Added Features
--------------
- Greatly improved testing framework: testing against R/Rserve is now self-contained in the Test project (integration tests become unit tests!)
- Date SEXP type supported
- Can get row/column names
- Read matrix as a 2d array
- Create SEXP from decimal
- Can now create usable data.frames