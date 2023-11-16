// StgShqrpGraphic.cpp : 定义 DLL 的导出函数。
//

#include "./include/ssgc_framework.h"
#include "./include/glad_gl.h"
#include "./include/glfw3.h"
#include "./include/StgSharpC.h"

#include <stdio.h>

GladGLContext* currentContext;
extern char* infolog[512] = { 0 };

void printShaderCode(char* const* code)
{
    printf(*code);
}





SSCAPI void _cdecl initGL(int majorVersion, int minorVersion)
{
    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, majorVersion);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, minorVersion);
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

#ifdef  __APPLE__
    glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
#endif 

}



SSCAPI unsigned int loadShaderCode(char* const shaderCode, GLint shadertype)
{
    if (currentContext != NULL)
    {
        unsigned int shader = currentContext->CreateShader(shadertype);
        currentContext->ShaderSource(shader, 1, &shaderCode, NULL);
        currentContext->CompileShader(shader);
        int success;
        currentContext->GetShaderiv(shader, GL_COMPILE_STATUS, &success);
        if (!success)
        {
            currentContext->GetShaderInfoLog(shader, 512, NULL, infolog);
            return 0;
        }
        else
        {
            return shader;
        }
    }
    else
    {
        return 0;
    }
}

SSCAPI unsigned int linkShaderProgram(GLuint shaderProgram)
{
    currentContext->LinkProgram(shaderProgram);

    int success;
    currentContext->GetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
    if (!success)
    {
        currentContext->GetProgramInfoLog(shaderProgram, 512, NULL, infolog);
        return 0;
    }
    else
    {
        return shaderProgram;
    }

}

char* _cdecl readLog()
{
    infolog[511] = '\0';
    return &infolog;
}

SSCAPI void SSCDECL setCurrentGLContext(GLFWwindow* window, GladGLContext* contextPtr)
{
    glfwMakeContextCurrent(window);
    currentContext = contextPtr;
}

