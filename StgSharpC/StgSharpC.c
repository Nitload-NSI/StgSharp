#include "ssgc_framework.h"
#include "gl.h"
#include "glfw3.h"
#include "StgSharpC.h"
#include <stdio.h>

GladGLContext* currentContext;
extern char infolog[512] = { 0 };

SSCAPI int SSCDECL glCheckShaderStat(GladGLContext* context,uint64_t shaderHandle , int key, char** logRef)
{
    int stat = 0;
    context->GetShaderiv(shaderHandle, key, &stat);
    if (!stat)
    {
        context->GetShaderInfoLog(shaderHandle, 512, NULL, infolog);
    }
    *logRef = infolog;
    return stat;
}

SSCAPI void SSCDECL initGL(int majorVersion, int minorVersion)
{
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, majorVersion);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, minorVersion);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
}

SSCAPI GLFWglproc SSCDECL loadGlfuncDefault(char* procName)
{
    void* ret = glfwGetProcAddress(procName);
    //printf("%llu\n", (uint64_t)ret);
    if (ret = NULL)
    {
        *infolog = "Failed to load OpenGL api:";
        for (size_t i = 0; i < sizeof(procName); i++)
        {
            infolog[30 + i] = procName[i];
        }
        printf("%s\n", infolog);
    }
    return (GLFWglproc)ret;
}

SSCAPI unsigned int linkShaderProgram(GladGLContext* context, GLuint shaderProgram)
{
    context->LinkProgram(shaderProgram);

    int success;
    context->GetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
    if (!success)
    {
        context->GetProgramInfoLog(shaderProgram, 512, NULL, infolog);
        return 0;
    }
    else
    {
        return shaderProgram;
    }
}

SSCAPI void SSCDECL loadImageData(char* location, Image* out, imageLoader loader)
{
    if (loader == NULL)
    {
        loader = stbi_load;
    }
    out->pixelPtr = loader(location, &(out->width), &(out->height), &(out->channel), 0);
}

SSCAPI void SSCDECL unloadImageData(Image* out)
{
    free(out->pixelPtr);
}

char* _cdecl readLog()
{
    infolog[511] = '\0';
    return &infolog;
}