// StgShqrpGraphic.cpp : 定义 DLL 的导出函数。
//


#include "GLenv/include/glad/glad.h"
#include "GLenv/include/GLFW/glfw3.h"
#include "framework.h"
#include <stdio.h>
#include "StgSharpGraphic.h"


SSGC_API void initGL(int majorVersion, int minorVersion)
{
    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, majorVersion);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, minorVersion);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

#ifdef  __APPLE__
    glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
#endif //  __APPLE__
}


SSGC_API int initGLAD(void)
{
    return gladLoadGLLoader((GLADloadproc)glfwGetProcAddress);
}
