# Stg# #

![Stg#Logo](https://github.com/Nitload-NSI/StgSharp/blob/main/STG%23LOGO.png "Stg#LOGO")


## General Introduction ##

Stg# is a new generation STG game engine. STG# is written in C# based on OpenGL. It provide a both low hand high level binding to OpenGL, multimedia and device control, giving everything you need to make your own stg games.We are looking forward to provide full support on Linux and other OS platforms, but it requires a long journey to go.

The Stg# engine contains the fallowing parts:

1. A high efficiency math library
2. A sets of APIs to create a MVVM windowed application
3. A sets of functions to optimize performance
4. A game resource manager


The Stg# main program is in project named "StgSharp", which contains most of the math method, graphic APIs, and application resources manager.

Most of the basic graphic functions are contained in the project called "StgSharpGraphic". And the functions to optimize performance, mainly to accelerate vector computing, are contained in project called "StgSharpC".

StgSharp provides full support of all OpenGL APIs nad features. And a cross platform window manager based on [GLFW](https://www.glfw.org/). 

## Install ##

Core of Stg# contains two files. 

Stg# have not been completed yet, so no release packages are provided now.

### Windows ###
Stg# on windows has two files, C# code are compiled in file "StgSharp.dll", while other code(mainly C and assembly) are compiled in file "StgSharpC.dll".

### Linux and other OS ###


## Future Plan ##

StgSharp looks forward to provide support to different graphic APIs. We are lokking forward to providing Vulcan support at 2.0 version.

StgSharp will also bew available on different OS platforms, listed in Version History. It is needed to be informed that we removed the plan on OS X series and HarmonyOS from recent plans.

## Version History *

| Version | Description                                        | Windows    | Linux      | Android    |
|---------|----------------------------------------------------|------------|------------|------------|
| 0.3.0   | The first usable version with min usable function  | No Release | No Release | No Release |
| 0.4.x   | Rewrite the OpenGL api loader, more function added | No Release | No Release | No Release |
| 0.5.x   | Support MVVM mode                                  | No Release | No Release | No Release |

*No current support plan for HarmonyOS and OS X series is the result of no dotNet JIT optimization support.


