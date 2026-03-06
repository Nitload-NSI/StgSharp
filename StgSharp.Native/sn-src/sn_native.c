#ifndef STB_IMAGE_IMPLEMENTATION
#define STB_IMAGE_IMPLEMENTATION
#endif
#include "StgSharpNative.h"
#include "gl.h"
#include "../lib/glfw/glfw3.h"
#include <stdio.h>
#include <string.h>

extern GladGLContext *currentContext;
extern char infolog[512];

SN_API int SN_DECL glCheckShaderStat(GladGLContext *context, uint64_t shaderHandle, int key,
                                     char **logRef)
{
        int stat = 0;
        context->GetShaderiv(shaderHandle, key, &stat);
        if (!stat) {
                context->GetShaderInfoLog(shaderHandle, 512, NULL, infolog);
        }
        *logRef = infolog;
        return stat;
}

SN_API void SN_DECL initGL(int majorVersion, int minorVersion)
{
        glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, majorVersion);
        glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, minorVersion);
        glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
        glfwWindowHint(GLFW_RESIZABLE, GLFW_TRUE);
}

SN_API GLFWglproc SN_DECL loadGlfuncDefault(char const *procName)
{
        void *ret = glfwGetProcAddress(procName);
        // printf("%llu\n", (uint64_t)ret);
        if (ret == NULL) {
                strcpy_s(infolog, 30, "Failed to load OpenGL api:");
                for (size_t i = 0; i < sizeof(procName); i++) {
                        infolog[30 + i] = procName[i];
                }
                //printf("%s\n", infolog);
        }
        return (GLFWglproc)ret;
}

SN_API unsigned int linkShaderProgram(GladGLContext *context, GLuint shaderProgram)
{
        context->LinkProgram(shaderProgram);

        int success;
        context->GetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
        if (!success) {
                context->GetProgramInfoLog(shaderProgram, 512, NULL, infolog);
                return 0;
        } else {
                return shaderProgram;
        }
}

SN_API void SN_DECL loadImageData(char const *location, Image *out, imageLoader loader)
{
        if (loader == NULL) {
                loader = stbi_load;
        }
        out->pixelPtr = loader(location, &(out->width), &(out->height), &(out->channel), 0);
}
SN_API void SN_DECL unloadImageData(Image *out)
{
        free(out->pixelPtr);
}

SN_API char *SN_DECL readLog()
{
        infolog[511] = '\0';
        return infolog;
}
