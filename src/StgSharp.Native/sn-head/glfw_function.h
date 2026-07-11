#ifndef GLFW_FUNCTION_TABLE_H
#define GLFW_FUNCTION_TABLE_H

#include "../lib/glfw/glfw3.h"

#ifdef __cplusplus
extern "C" {
#endif
typedef struct GLFWFunctionTable {
        GLFWcursor *(*glfwCreateCursor)(const GLFWimage *image, int xhot, int yhot);
        GLFWcursor *(*glfwCreateStandardCursor)(int shape);
        GLFWwindow *(*glfwCreateWindow)(int width, int height, const char *title,
                                        GLFWmonitor *monitor, GLFWwindow *share);
        void (*glfwDefaultWindowHints)(void);
        void (*glfwDestroyCursor)(GLFWcursor *cursor);
        void (*glfwDestroyWindow)(GLFWwindow *window);
        int (*glfwExtensionSupported)(const char *extension);
        void (*glfwFocusWindow)(GLFWwindow *window);
        const char *(*glfwGetClipboardString)(GLFWwindow *window);
        GLFWwindow *(*glfwGetCurrentContext)(void);
        void (*glfwGetCursorPos)(GLFWwindow *window, double *xpos, double *ypos);
        int (*glfwGetError)(const char **description);
        void (*glfwGetFrameBufferSize)(GLFWwindow *window, int *width, int *height);
        const char *(*glfwGetGamepadName)(int jid);
        int (*glfwGetGamepadState)(int jid, GLFWgamepadstate *state);
        const GLFWgammaramp *(*glfwGetGammaRamp)(GLFWmonitor *monitor);
        int (*glfwGetInputMode)(GLFWwindow *window, int mode);
        const float *(*glfwGetJoystickAxes)(int jid, int *count);
        const unsigned char *(*glfwGetJoystickButtons)(int jid, int *count);
        const char *(*glfwGetJoystickGUID)(int jid);
        const unsigned char *(*glfwGetJoystickHats)(int jid, int *count);
        const char *(*glfwGetJoystickName)(int jid);
        void *(*glfwGetJoystickUserPointer)(int jid);
        int (*glfwGetKey)(GLFWwindow *window, int key);
        const char *(*glfwGetKeyName)(int key, int scancode);
        int (*glfwGetKeyScancode)(int key);
        void (*glfwGetMonitorContentScale)(GLFWmonitor *monitor, float *xscale, float *yscale);
        const char *(*glfwGetMonitorName)(GLFWmonitor *monitor);
        void (*glfwGetMonitorPhysicalSize)(GLFWmonitor *monitor, int *widthMM, int *heightMM);
        void (*glfwGetMonitorPos)(GLFWmonitor *monitor, int *xpos, int *ypos);
        GLFWmonitor **(*glfwGetMonitors)(int *count);
        void *(*glfwGetMonitorUserPointer)(GLFWmonitor *monitor);
        void (*glfwGetMonitorWorkarea)(GLFWmonitor *monitor, int *xpos, int *ypos, int *width,
                                       int *height);
        int (*glfwGetMouseButton)(GLFWwindow *window, int button);
        GLFWmonitor *(*glfwGetPrimaryMonitor)(void);
        GLFWglproc (*glfwGetProcAddress)(const char *procname);
        const char **(*glfwGetRequiredInstanceExtensions)(unsigned int *count);
        double (*glfwGetTime)(void);
        unsigned long long (*glfwGetTimerFrequency)(void);
        unsigned long long (*glfwGetTimerValue)(void);
        void (*glfwGetVersion)(int *major, int *minor, int *rev);
        const char *(*glfwGetVersionString)(void);
        const GLFWvidmode *(*glfwGetVideoMode)(GLFWmonitor *monitor);
        const GLFWvidmode *(*glfwGetVideoModes)(GLFWmonitor *monitor, int *count);
        int (*glfwGetWindowAttrib)(GLFWwindow *window, int attrib);
        void (*glfwGetWindowContentScale)(GLFWwindow *window, float *xscale, float *yscale);
        void (*glfwGetWindowFrameSize)(GLFWwindow *window, int *left, int *top, int *right,
                                       int *bottom);
        GLFWmonitor *(*glfwGetWindowMonitor)(GLFWwindow *window);
        float (*glfwGetWindowOpacity)(GLFWwindow *window);
        void (*glfwGetWindowPos)(GLFWwindow *window, int *xpos, int *ypos);
        void (*glfwGetWindowSize)(GLFWwindow *window, int *width, int *height);
        void *(*glfwGetWindowUserPointer)(GLFWwindow *window);
        void (*glfwHideWindow)(GLFWwindow *window);
        void (*glfwIconifyWindow)(GLFWwindow *window);
        int (*glfwInit)(void);
        void (*glfwInitHint)(int hint, int value);
        int (*glfwJoystickIsGamePad)(int jid);
        int (*glfwJoystickPresent)(int jid);
        void (*glfwMakeContextCurrent)(GLFWwindow *window);
        void (*glfwMaximizeWindow)(GLFWwindow *window);
        void (*glfwPollEvents)(void);
        void (*glfwPostEmptyEvent)(void);
        int (*glfwRawMouseMotionSupported)(void);
        void (*glfwRequestWindowAttention)(GLFWwindow *window);
        void (*glfwRestoreWindow)(GLFWwindow *window);
        GLFWcharfun (*glfwSetCharCallback)(GLFWwindow *window, GLFWcharfun callback);
        GLFWcharmodsfun (*glfwSetCharModsCallback)(GLFWwindow *window, GLFWcharmodsfun callback);
        void (*glfwSetClipboardString)(GLFWwindow *window, const char *str);
        void (*glfwSetCursor)(GLFWwindow *window, GLFWcursor *cursor);
        GLFWcursorenterfun (*glfwSetCursorEnterCallback)(GLFWwindow *window,
                                                         GLFWcursorenterfun callback);
        void (*glfwSetCursorPos)(GLFWwindow *window, double xpos, double ypos);
        GLFWcursorposfun (*glfwSetCursorPosCallback)(GLFWwindow *window, GLFWcursorposfun callback);
        GLFWdropfun (*glfwSetDropCallback)(GLFWwindow *window, GLFWdropfun callback);
        GLFWerrorfun (*glfwSetErrorCallback)(GLFWerrorfun callback);
        GLFWframebuffersizefun (*glfwSetFramebufferSizeCallback)(GLFWwindow *window,
                                                                 GLFWframebuffersizefun callback);
        void (*glfwSetGamma)(GLFWmonitor *monitor, float gamma);
        void (*glfwSetGammaRamp)(GLFWmonitor *monitor, const GLFWgammaramp *ramp);
        void (*glfwSetInputMode)(GLFWwindow *window, int mode, int value);
        GLFWjoystickfun (*glfwSetJoystickCallback)(GLFWjoystickfun callback);
        void (*glfwSetJoystickUserPointer)(int jid, void *pointer);
        GLFWkeyfun (*glfwSetKeyCallback)(GLFWwindow *window, GLFWkeyfun callback);
        GLFWmonitorfun (*glfwSetMonitorCallback)(GLFWmonitorfun callback);
        void (*glfwSetMonitorUserPointer)(GLFWmonitor *monitor, void *pointer);
        GLFWmousebuttonfun (*glfwSetMouseButtonCallback)(GLFWwindow *window,
                                                         GLFWmousebuttonfun callback);
        GLFWscrollfun (*glfwSetScrollCallback)(GLFWwindow *window, GLFWscrollfun callback);
        void (*glfwSetTime)(double time);
        void (*glfwSetWindowAspectRatio)(GLFWwindow *window, int numer, int denom);
        void (*glfwSetWindowAttrib)(GLFWwindow *window, int attrib, int value);
        GLFWwindowclosefun (*glfwSetWindowCloseCallback)(GLFWwindow *window,
                                                         GLFWwindowclosefun callback);
        GLFWwindowcontentscalefun (*glfwSetWindowContentScaleCallback)(
                GLFWwindow *window, GLFWwindowcontentscalefun callback);
        GLFWwindowfocusfun (*glfwSetWindowFocusCallback)(GLFWwindow *window,
                                                         GLFWwindowfocusfun callback);
        void (*glfwSetWindowIcon)(GLFWwindow *window, int count, const GLFWimage *images);
        GLFWwindowiconifyfun (*glfwSetWindowIconifyCallback)(GLFWwindow *window,
                                                             GLFWwindowiconifyfun callback);
        GLFWwindowmaximizefun (*glfwSetWindowMaximizeCallback)(GLFWwindow *window,
                                                               GLFWwindowmaximizefun callback);
        void (*glfwSetWindowMonitor)(GLFWwindow *window, GLFWmonitor *monitor, int xpos, int ypos,
                                     int width, int height, int refreshRate);
        void (*glfwSetWindowOpacity)(GLFWwindow *window, float opacity);
        void (*glfwSetWindowPos)(GLFWwindow *window, int xpos, int ypos);
        GLFWwindowposfun (*glfwSetWindowPosCallback)(GLFWwindow *window, GLFWwindowposfun callback);
        GLFWwindowrefreshfun (*glfwSetWindowRefreshCallback)(GLFWwindow *window,
                                                             GLFWwindowrefreshfun callback);
        void (*glfwSetWindowShouldClose)(GLFWwindow *window, int value);
        void (*glfwSetWindowSize)(GLFWwindow *window, int width, int height);
        GLFWwindowsizefun (*glfwSetWindowSizeCallback)(GLFWwindow *window,
                                                       GLFWwindowsizefun callback);
        void (*glfwSetWindowSizeLimits)(GLFWwindow *window, int minwidth, int minheight,
                                        int maxwidth, int maxheight);
        void (*glfwSetWindowTitle)(GLFWwindow *window, const char *title);
        void (*glfwSetWindowUserPointer)(GLFWwindow *window, void *pointer);
        void (*glfwShowWindow)(GLFWwindow *window);
        void (*glfwSwapBuffers)(GLFWwindow *window);
        void (*glfwSwapInterval)(int interval);
        void (*glfwTerminate)(void);
        int (*glfwUpdateGamePadMappings)(const char *str);
        int (*glfwVulkanSupported)(void);
        void (*glfwWaitEvents)(void);
        void (*glfwWaitEventsTimeout)(double timeout);
        void (*glfwWindowHint)(int hint, int value);
        void (*glfwWindowHintString)(int hint, const char *value);
        int (*glfwWindowShouldClose)(GLFWwindow *window);
} GLFWFunctionTable;
#ifdef __cplusplus
}
#endif

#endif
