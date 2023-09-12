#include "GLenv/include/glad/glad.h"
#include "GLenv/include/GLFW/glfw3.h"
#include "StgSharpGraphic.h"
#include "SSGinternal.h"
#include <stdio.h>

extern ReportApiAddress report;

SSGC_API void getApiReporter(uint64_t reporterAddress)
{
    report = (ReportApiAddress)reporterAddress;
}
