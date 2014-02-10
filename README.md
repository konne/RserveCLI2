Overview
---------
RserveCLI2 is a .NET/CLR client for Rserve. It allows .NET/CLR client to access R on the same machine or across the network.

Details
-------
Rserve (http://www.rforge.net/Rserve/) is a server application that allows users to access an R (http://www.r-project.org/) session remotely across the network. RserveCLI2 is a client library that allows one to access an Rserve server from a .NET/CLR environment using languages such as C#, Visual Basic, etc.

Fork of RserveCLI
-----------------
- This is a fork of RserveCLI, by Oliver M. Haynold which is hosted here: https://rservecli.codeplex.com
- Forked at commit 14449
- I wanted to add features and fix bugs unencumbered by the author's bandwidth
- I'm targeting Visual Studio 2010 and the 3.5 framework (dropping Mono Develop)

Added Features
--------------
- Greatly improved testing framework: testing against R/Rserve is now self-contained in the Test project (integration tests become unit tests!)
- Bi-directional support for Date type
- Read matrix as a 2d array
- Create SEXP from decimal
- Can now create usable data.frames
- Bi-directional support for matrix row/column names
- Can now make named vectors
- Now supports sending and receiving large data (>16MB)