#ifndef SSG
#define SSG

#define SSG_Api

#ifdef SSG_Api
#define STGSHARPGRAPHIC_API __declspec(dllexport)
#else
#define STGSHARPGRAPHIC_API __declspec(dllimport)
#endif


#include "GLenv/include/glad/glad.h"
#include "GLenv/include/GLFW/glfw3.h"
#include "SSGinternal.h"



STGSHARPGRAPHIC_API glIOpackage loadGLApi(void);
STGSHARPGRAPHIC_API void initGL(void);
STGSHARPGRAPHIC_API int initGLAD(void);
STGSHARPGRAPHIC_API void* getPtr(void);


#endif // !SSG

