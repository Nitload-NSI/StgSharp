# Stg# #

![Stg#Logo](https://github.com/Nitload-NSI/STG_Sharp/blob/main/STG%23LOGO.png "Stg#LOGO")


## General Introduction ##

Stg# is the new generation STG game engine. STG# is written in C# and runs on .NET platform.

The Stg# engine contains the fallowing parts:

1. A high efficiency math library
2. A series of APIs to creat windowed aplication and game structures
3. A series functions to optimize the performance
4. A game resource manager

Timeline of Stg# is controlled by a new type of scipt language called SGSL(Shooting Game Script Language). SGSL allows developers to manage files and debug in an reletively easy mod. Developers edit games in different "Organizations", which can be devided into "Generatings" and "Processings", which defines how bollet creats and move.

The Stg# main program is in project named "StgSharp", which contains most of the math method, graphic APIs, and application resources manager.

Most of the basic graphic functions are contained in the project called "StgSharpGraphic". And the functions to optimize performance, mainly to accelerate vector computing, are contained in project called "StgSharpOptimizing".

Early version of Stg# Math Library use C# to calculate, so there is no project called "StgSharpOptimizing", but is relatively low in efficiency and accuracy. In future versions, the math library will be optimaized by useing SSE Instructions.

## Install ##

Core of Stg# contains two files. 

Stg# have not been completed yet, so no release packages are provided now.

### Windows ###
Stg# on windows has two files, C# code are compiled in file "StgSharp.dll", while other code(mainly C and assembly) are compiled in file "StgSharpC.dll".

### Linux and other OS ###


## Version History

| Version | Description                                       | Windows    | Linux      | Mac OS     |
|---------|---------------------------------------------------|------------|------------|------------|
| 0.3.0   | The first usable version with all usable function | No Release | No Release | No Release |




