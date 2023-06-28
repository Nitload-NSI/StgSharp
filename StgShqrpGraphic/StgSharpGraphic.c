// StgShqrpGraphic.cpp : 定义 DLL 的导出函数。
//


#include "GLenv/include/glad/glad.h"
#include "GLenv/include/GLFW/glfw3.h"
#include "framework.h"
#include "StgSharpGraphic.h"
#include "SSGinternal.h"

glIOpackage pack;

STGSHARPGRAPHIC_API glIOpackage loadGLApi(void)
{

    pack.createWindow = glfwCreateWindow;
    pack.makeContextCurrent = glfwMakeContextCurrent;
    pack.swapInterval = glfwSwapInterval;
    pack.clearColor = glClearColor;
    pack.windowShouldClose = glfwWindowShouldClose;
    pack.swapBuffer = glfwSwapBuffers;

    //these are for callback
    pack.SetFramebufferSizeCallback = glfwSetFramebufferSizeCallback;

    glfwCreateWindow(800,600,"Stg#!", NULL,NULL);


    return pack;
}

STGSHARPGRAPHIC_API void initGL(void)
{
    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

#ifdef  __APPLE__
    glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
#endif //  __APPLE__
}


STGSHARPGRAPHIC_API int initGLAD(void)
{
    return gladLoadGLLoader((GLADloadproc)glfwGetProcAddress);

}

GLFWwindow* (*createWindow)(
    int width,
    int height,
    const char* title,
    GLFWmonitor* monitor,
    GLFWwindow* share);


STGSHARPGRAPHIC_API void* getPtr(void)
{
    createWindow = glfwCreateWindow;
    return createWindow;
}