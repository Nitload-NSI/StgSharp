#pragma once

#include "GLenv/include/glad/glad.h"
#include "GLenv/include/glad/glad.h"
#include "StgSharpGraphic.h"


typedef struct glIOPack
{
    //these are gl functions used to create a new window
    GLFWwindow* (*createWindow)(
        int width,
        int height,
        const char* title,
        GLFWmonitor* monitor,
        GLFWwindow* share);
    void(*makeContextCurrent)(GLFWwindow* window);
    void(*swapInterval)(int interval);
    void(*clearColor)(GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha);
    int(*windowShouldClose)(GLFWwindow* window);
    void(*swapBuffer)(GLFWwindow* window);


    //these are gl functions used to set a callback

    GLFWframebuffersizefun(*SetFramebufferSizeCallback)
        (GLFWwindow* window, GLFWframebuffersizefun callback);


}glIOpackage;