# Stg# #

![Stg#Logo](https://github.com/Nitload-NSI/StgSharp/blob/main/STG%23LOGO.png "Stg#LOGO")


## General Introduction ##

Stg# is a new generation STG game engine. STG# is written in C# based on OpenGL. It provide a both low hand high level binding to OpenGL, multimedia and device control, giving everything you need to make your own stg games.We are looking forward to provide full support on Linux and other OS platforms, but it requires a long journey to go.

The Stg# engine contains the fallowing modules:

1. A high efficiency math library
2. A MVVM windowed application framework
3. A compiler to compile scripts
4. A sets of methods to optimize performance
5. A game resource manager

The Stg# main program is in project named "StgSharp", which contains most of the math method, graphic APIs, and application resources manager.

Most of the basic graphic functions are contained in the project called "StgSharpGraphic". And the functions to optimize performance, mainly to accelerate vector computing, are contained in project called "StgSharpC".

StgSharp provides full support of all OpenGL APIs and features, and provides a cross platform viewport manager based on [GLFW](https://www.glfw.org/). 

As well, we provide support to [EXPRESS language](https://www.expresslang.org/) (or [ISO 1303-11](https://www.iso.org/standard/38047.html)). You can use it to build a blueprint or other structured data. One thing to be noticed is that our support to EXPRESS lang is not fully implemented, some of the keywords and language feature is not available, due to complexity in C#. C# does not support multi-inheritance, so related features in EXPRESS like SUPERTYPEOF or SUBTYPEOF cannot be converted to C#　code easily unless use some tricks like LINQ.

## Install ##

Package can be downloaded from [nuget](https://127.0.0.1) (not available right now, do not click).

StgSharp is a project mainly focusing on C# GUI and CG designing, so API in other languages are not available currently.

## Future Plan ##

We are looking forward to making some optimization to provide various features.

We are planning to including Skia (based on [SkiaSharp](https://github.com/mono/SkiaSharp)) to StgSharp to improve simple graphic rendering. We will also provide support to different graphic APIs. We are looking forward to providing Vulcan support at 2.0 version.

StgSharp will also bew available on different OS platforms, listed in Version History. It is needed to be informed that we removed the plan on OS X series and HarmonyOS from recent plans.

## Version History *

A brief introduction to all main versions are listed bellow, for more detailed info, please read our [release blog](https://github.com/Nitload-NSI/StgSharp/blob/main/UpdateBlog.md).

| Version | Description                                        | Windows    | Linux      | Android    |
|---------|----------------------------------------------------|------------|------------|------------|
| 0.3.0   | The first usable version with min usable function  | No Release | No Release | No Release |
| 0.4.x   | Rewrite the OpenGL api loader, more function added | No Release | No Release | No Release |
| 0.5.x   | Support MVVM mode                                  | No Release | No Release | No Release |
| 0.6.x   | Support script compile(Mainly EXPRESSION languages)| No Release | No Release | No Release |

*No current support plan for HarmonyOS, OS X series and some other mobile operating system is the result of runtime support.
