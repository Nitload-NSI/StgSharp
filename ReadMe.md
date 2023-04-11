# Stg# #

Stg# is the new generation STG game engine. STG# is written in C# and runs on .NET platform.

The Stg# engine contains three parts:

1. A high efficiency math library
2. A timeline manager
3. A game resource manager

Timeline of Stg# is controlled by a new type of scipt language called SGSL(Shooting Game Script Language). SGSL allows developers to manage files and debug in an reletively easy mod. Developers edit games in different "Organizations", which can be devided into "Generatings" and "Processings", which defines how bollet creats and move.

The current version of STG# use SSE to accelerate math calulating, but cause problems on cross-platform supporting, requiring a long time to reach same performance on Linux or MacOS.