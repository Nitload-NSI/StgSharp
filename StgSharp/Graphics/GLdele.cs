/*
 * This file is created to link functions in OpenGL to version in C#
 * The name of delegates and organization to init all these delegates 
 * are refferd from GLAD1 in agreement of MIT license generated from 
 * https://glad.dav1d.de/
 */


using StgSharp.Graphics;
using System;
using System.Runtime.InteropServices;

namespace StgSharp
{
    public static partial class Graphic
    {



        #region GL_dele

        #region gl_1_0

        internal static PFNGLCULLFACEPROC glCullFace;
        internal static PFNGLFRONTFACEPROC glFrontFace;
        internal static PFNGLHINTPROC glHint;
        internal static PFNGLLINEWIDTHPROC glLineWidth;
        internal static PFNGLPOINTSIZEPROC glPointSize;
        internal static PFNGLPOLYGONMODEPROC glPolygonMode;
        internal static PFNGLSCISSORPROC glScissor;
        internal static PFNGLTEXPARAMETERFPROC glTexParameterf;
        internal static PFNGLTEXPARAMETERFVPROC glTexParameterfv;
        internal static PFNGLTEXPARAMETERIPROC glTexParameteri;
        internal static PFNGLTEXPARAMETERIVPROC glTexParameteriv;
        internal static PFNGLTEXIMAGE1DPROC glTexImage1D;
        internal static PFNGLTEXIMAGE2DPROC glTexImage2D;
        internal static PFNGLDRAWBUFFERPROC glDrawBuffer;
        internal static PFNGLCLEARPROC glClear;
        internal static PFNGLCLEARCOLORPROC glClearColor;
        internal static PFNGLCLEARSTENCILPROC glClearStencil;
        internal static PFNGLCLEARDEPTHPROC glClearDepth;
        internal static PFNGLSTENCILMASKPROC glStencilMask;
        internal static PFNGLCOLORMASKPROC glColorMask;
        internal static PFNGLDEPTHMASKPROC glDepthMask;
        internal static PFNGLDISABLEPROC glDisable;
        internal static PFNGLENABLEPROC glEnable;
        internal static PFNGLFINISHPROC glFinish;
        internal static PFNGLFLUSHPROC glFlush;
        internal static PFNGLBLENDFUNCPROC glBlendFunc;
        internal static PFNGLLOGICOPPROC glLogicOp;
        internal static PFNGLSTENCILFUNCPROC glStencilFunc;
        internal static PFNGLSTENCILOPPROC glStencilOp;
        internal static PFNGLDEPTHFUNCPROC glDepthFunc;
        internal static PFNGLPIXELSTOREFPROC glPixelStoref;
        internal static PFNGLPIXELSTOREIPROC glPixelStorei;
        internal static PFNGLREADBUFFERPROC glReadBuffer;
        internal static PFNGLREADPIXELSPROC glReadPixels;
        internal static PFNGLGETBOOLEANVPROC glGetBooleanv;
        internal static PFNGLGETDOUBLEVPROC glGetDoublev;
        internal static PFNGLGETERRORPROC glGetError;
        internal static PFNGLGETFLOATVPROC glGetFloatv;
        internal static PFNGLGETINTEGERVPROC glGetIntegerv;
        internal static PFNGLGETSTRINGPROC glGetString;
        internal static PFNGLGETTEXIMAGEPROC glGetTexImage;
        internal static PFNGLGETTEXPARAMETERFVPROC glGetTexParameterfv;
        internal static PFNGLGETTEXPARAMETERIVPROC glGetTexParameteriv;
        internal static PFNGLGETTEXLEVELPARAMETERFVPROC glGetTexLevelParameterfv;
        internal static PFNGLGETTEXLEVELPARAMETERIVPROC glGetTexLevelParameteriv;
        internal static PFNGLISENABLEDPROC glIsEnabled;
        internal static PFNGLDEPTHRANGEPROC glDepthRange;
        internal static PFNGLVIEWPORTPROC glViewport;

        #endregion

        #region gl_1_1

        internal static PFNGLDRAWARRAYSPROC glDrawArrays;
        internal static PFNGLDRAWELEMENTSPROC glDrawElements;
        internal static PFNGLPOLYGONOFFSETPROC glPolygonOffset;
        internal static PFNGLCOPYTEXIMAGE1DPROC glCopyTexImage1D;
        internal static PFNGLCOPYTEXIMAGE2DPROC glCopyTexImage2D;
        internal static PFNGLCOPYTEXSUBIMAGE1DPROC glCopyTexSubImage1D;
        internal static PFNGLCOPYTEXSUBIMAGE2DPROC glCopyTexSubImage2D;
        internal static PFNGLTEXSUBIMAGE1DPROC glTexSubImage1D;
        internal static PFNGLTEXSUBIMAGE2DPROC glTexSubImage2D;
        internal static PFNGLBINDTEXTUREPROC glBindTexture;
        internal static PFNGLDELETETEXTURESPROC glDeleteTextures;
        internal static PFNGLGENTEXTURESPROC glGenTextures;
        internal static PFNGLISTEXTUREPROC glIsTexture;

        #endregion

        #region gl_1_2

        internal static PFNGLDRAWRANGEELEMENTSPROC glDrawRangeElements;
        internal static PFNGLTEXIMAGE3DPROC glTexImage3D;
        internal static PFNGLTEXSUBIMAGE3DPROC glTexSubImage3D;
        internal static PFNGLCOPYTEXSUBIMAGE3DPROC glCopyTexSubImage3D;

        #endregion

        #region gl_1_3

        internal static PFNGLACTIVETEXTUREPROC glActiveTexture;
        internal static PFNGLSAMPLECOVERAGEPROC glSampleCoverage;
        internal static PFNGLCOMPRESSEDTEXIMAGE3DPROC glCompressedTexImage3D;
        internal static PFNGLCOMPRESSEDTEXIMAGE2DPROC glCompressedTexImage2D;
        internal static PFNGLCOMPRESSEDTEXIMAGE1DPROC glCompressedTexImage1D;
        internal static PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC glCompressedTexSubImage3D;
        internal static PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC glCompressedTexSubImage2D;
        internal static PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC glCompressedTexSubImage1D;
        internal static PFNGLGETCOMPRESSEDTEXIMAGEPROC glGetCompressedTexImage;

        #endregion

        #region gl_1_4

        internal static PFNGLBLENDFUNCSEPARATEPROC glBlendFuncSeparate;
        internal static PFNGLMULTIDRAWARRAYSPROC glMultiDrawArrays;
        internal static PFNGLMULTIDRAWELEMENTSPROC glMultiDrawElements;
        internal static PFNGLPOINTPARAMETERFPROC glPointParameterf;
        internal static PFNGLPOINTPARAMETERFVPROC glPointParameterfv;
        internal static PFNGLPOINTPARAMETERIPROC glPointParameteri;
        internal static PFNGLPOINTPARAMETERIVPROC glPointParameteriv;
        internal static PFNGLBLENDCOLORPROC glBlendColor;
        internal static PFNGLBLENDEQUATIONPROC glBlendEquation;

        #endregion

        #region gl_1_5

        internal static PFNGLGENQUERIESPROC glGenQueries;
        internal static PFNGLDELETEQUERIESPROC glDeleteQueries;
        internal static PFNGLISQUERYPROC glIsQuery;
        internal static PFNGLBEGINQUERYPROC glBeginQuery;
        internal static PFNGLENDQUERYPROC glEndQuery;
        internal static PFNGLGETQUERYIVPROC glGetQueryiv;
        internal static PFNGLGETQUERYOBJECTIVPROC glGetQueryObjectiv;
        internal static PFNGLGETQUERYOBJECTUIVPROC glGetQueryObjectuiv;
        internal static PFNGLBINDBUFFERPROC glBindBuffer;
        internal static PFNGLDELETEBUFFERSPROC glDeleteBuffers;
        internal static PFNGLGENBUFFERSPROC glGenBuffers;
        internal static PFNGLISBUFFERPROC glIsBuffer;
        internal static PFNGLBUFFERDATAPROC glBufferData;
        internal static PFNGLBUFFERSUBDATAPROC glBufferSubData;
        internal static PFNGLGETBUFFERSUBDATAPROC glGetBufferSubData;
        internal static PFNGLMAPBUFFERPROC glMapBuffer;
        internal static PFNGLUNMAPBUFFERPROC glUnmapBuffer;
        internal static PFNGLGETBUFFERPARAMETERIVPROC glGetBufferParameteriv;
        internal static PFNGLGETBUFFERPOINTERVPROC glGetBufferPointerv;

        #endregion

        #region gl_2_0

        internal static PFNGLBLENDEQUATIONSEPARATEPROC glBlendEquationSeparate;
        internal static PFNGLDRAWBUFFERSPROC glDrawBuffers;
        internal static PFNGLSTENCILOPSEPARATEPROC glStencilOpSeparate;
        internal static PFNGLSTENCILFUNCSEPARATEPROC glStencilFuncSeparate;
        internal static PFNGLSTENCILMASKSEPARATEPROC glStencilMaskSeparate;
        internal static PFNGLATTACHSHADERPROC glAttachShader;
        internal static PFNGLBINDATTRIBLOCATIONPROC glBindAttribLocation;
        internal static PFNGLCOMPILESHADERPROC glCompileShader;
        internal static PFNGLCREATEPROGRAMPROC glCreateProgram;
        internal static PFNGLCREATESHADERPROC glCreateShader;
        internal static PFNGLDELETEPROGRAMPROC glDeleteProgram;
        internal static PFNGLDELETESHADERPROC glDeleteShader;
        internal static PFNGLDETACHSHADERPROC glDetachShader;
        internal static PFNGLDISABLEVERTEXATTRIBARRAYPROC glDisableVertexAttribArray;
        internal static PFNGLENABLEVERTEXATTRIBARRAYPROC glEnableVertexAttribArray;
        internal static PFNGLGETACTIVEATTRIBPROC glGetActiveAttrib;
        internal static PFNGLGETACTIVEUNIFORMPROC glGetActiveUniform;
        internal static PFNGLGETATTACHEDSHADERSPROC glGetAttachedShaders;
        internal static PFNGLGETATTRIBLOCATIONPROC glGetAttribLocation;
        internal static PFNGLGETPROGRAMIVPROC glGetProgramiv;
        internal static PFNGLGETPROGRAMINFOLOGPROC glGetProgramInfoLog;
        internal static PFNGLGETSHADERIVPROC glGetShaderiv;
        internal static PFNGLGETSHADERINFOLOGPROC glGetShaderInfoLog;
        internal static PFNGLGETSHADERSOURCEPROC glGetShaderSource;
        internal static PFNGLGETUNIFORMLOCATIONPROC glGetUniformLocation;
        internal static PFNGLGETUNIFORMFVPROC glGetUniformfv;
        internal static PFNGLGETUNIFORMIVPROC glGetUniformiv;
        internal static PFNGLGETVERTEXATTRIBDVPROC glGetVertexAttribdv;
        internal static PFNGLGETVERTEXATTRIBFVPROC glGetVertexAttribfv;
        internal static PFNGLGETVERTEXATTRIBIVPROC glGetVertexAttribiv;
        internal static PFNGLGETVERTEXATTRIBPOINTERVPROC glGetVertexAttribPointerv;
        internal static PFNGLISPROGRAMPROC glIsProgram;
        internal static PFNGLISSHADERPROC glIsShader;
        internal static PFNGLLINKPROGRAMPROC glLinkProgram;
        internal static PFNGLSHADERSOURCEPROC glShaderSource;
        internal static PFNGLUSEPROGRAMPROC glUseProgram;
        internal static PFNGLUNIFORM1FPROC glUniform1f;
        internal static PFNGLUNIFORM2FPROC glUniform2f;
        internal static PFNGLUNIFORM3FPROC glUniform3f;
        internal static PFNGLUNIFORM4FPROC glUniform4f;
        internal static PFNGLUNIFORM1IPROC glUniform1i;
        internal static PFNGLUNIFORM2IPROC glUniform2i;
        internal static PFNGLUNIFORM3IPROC glUniform3i;
        internal static PFNGLUNIFORM4IPROC glUniform4i;
        internal static PFNGLUNIFORM1FVPROC glUniform1fv;
        internal static PFNGLUNIFORM2FVPROC glUniform2fv;
        internal static PFNGLUNIFORM3FVPROC glUniform3fv;
        internal static PFNGLUNIFORM4FVPROC glUniform4fv;
        internal static PFNGLUNIFORM1IVPROC glUniform1iv;
        internal static PFNGLUNIFORM2IVPROC glUniform2iv;
        internal static PFNGLUNIFORM3IVPROC glUniform3iv;
        internal static PFNGLUNIFORM4IVPROC glUniform4iv;
        internal static PFNGLUNIFORMMATRIX2FVPROC glUniformMatrix2fv;
        internal static PFNGLUNIFORMMATRIX3FVPROC glUniformMatrix3fv;
        internal static PFNGLUNIFORMMATRIX4FVPROC glUniformMatrix4fv;
        internal static PFNGLVALIDATEPROGRAMPROC glValidateProgram;
        internal static PFNGLVERTEXATTRIB1DPROC glVertexAttrib1d;
        internal static PFNGLVERTEXATTRIB1DVPROC glVertexAttrib1dv;
        internal static PFNGLVERTEXATTRIB1FPROC glVertexAttrib1f;
        internal static PFNGLVERTEXATTRIB1FVPROC glVertexAttrib1fv;
        internal static PFNGLVERTEXATTRIB1SPROC glVertexAttrib1s;
        internal static PFNGLVERTEXATTRIB1SVPROC glVertexAttrib1sv;
        internal static PFNGLVERTEXATTRIB2DPROC glVertexAttrib2d;
        internal static PFNGLVERTEXATTRIB2DVPROC glVertexAttrib2dv;
        internal static PFNGLVERTEXATTRIB2FPROC glVertexAttrib2f;
        internal static PFNGLVERTEXATTRIB2FVPROC glVertexAttrib2fv;
        internal static PFNGLVERTEXATTRIB2SPROC glVertexAttrib2s;
        internal static PFNGLVERTEXATTRIB2SVPROC glVertexAttrib2sv;
        internal static PFNGLVERTEXATTRIB3DPROC glVertexAttrib3d;
        internal static PFNGLVERTEXATTRIB3DVPROC glVertexAttrib3dv;
        internal static PFNGLVERTEXATTRIB3FPROC glVertexAttrib3f;
        internal static PFNGLVERTEXATTRIB3FVPROC glVertexAttrib3fv;
        internal static PFNGLVERTEXATTRIB3SPROC glVertexAttrib3s;
        internal static PFNGLVERTEXATTRIB3SVPROC glVertexAttrib3sv;
        internal static PFNGLVERTEXATTRIB4NBVPROC glVertexAttrib4Nbv;
        internal static PFNGLVERTEXATTRIB4NIVPROC glVertexAttrib4Niv;
        internal static PFNGLVERTEXATTRIB4NSVPROC glVertexAttrib4Nsv;
        internal static PFNGLVERTEXATTRIB4NUBPROC glVertexAttrib4Nub;
        internal static PFNGLVERTEXATTRIB4NUBVPROC glVertexAttrib4Nubv;
        internal static PFNGLVERTEXATTRIB4NUIVPROC glVertexAttrib4Nuiv;
        internal static PFNGLVERTEXATTRIB4NUSVPROC glVertexAttrib4Nusv;
        internal static PFNGLVERTEXATTRIB4BVPROC glVertexAttrib4bv;
        internal static PFNGLVERTEXATTRIB4DPROC glVertexAttrib4d;
        internal static PFNGLVERTEXATTRIB4DVPROC glVertexAttrib4dv;
        internal static PFNGLVERTEXATTRIB4FPROC glVertexAttrib4f;
        internal static PFNGLVERTEXATTRIB4FVPROC glVertexAttrib4fv;
        internal static PFNGLVERTEXATTRIB4IVPROC glVertexAttrib4iv;
        internal static PFNGLVERTEXATTRIB4SPROC glVertexAttrib4s;
        internal static PFNGLVERTEXATTRIB4SVPROC glVertexAttrib4sv;
        internal static PFNGLVERTEXATTRIB4UBVPROC glVertexAttrib4ubv;
        internal static PFNGLVERTEXATTRIB4UIVPROC glVertexAttrib4uiv;
        internal static PFNGLVERTEXATTRIB4USVPROC glVertexAttrib4usv;
        internal static PFNGLVERTEXATTRIBPOINTERPROC glVertexAttribPointer;

        #endregion

        #region gl_2_1

        internal static PFNGLUNIFORMMATRIX2X3FVPROC glUniformMatrix2x3fv;
        internal static PFNGLUNIFORMMATRIX3X2FVPROC glUniformMatrix3x2fv;
        internal static PFNGLUNIFORMMATRIX2X4FVPROC glUniformMatrix2x4fv;
        internal static PFNGLUNIFORMMATRIX4X2FVPROC glUniformMatrix4x2fv;
        internal static PFNGLUNIFORMMATRIX3X4FVPROC glUniformMatrix3x4fv;
        internal static PFNGLUNIFORMMATRIX4X3FVPROC glUniformMatrix4x3fv;

        #endregion

        #region gl_3_0

        internal static PFNGLCOLORMASKIPROC glColorMaski;
        internal static PFNGLGETBOOLEANI_VPROC glGetBooleani_v;
        internal static PFNGLGETINTEGERI_VPROC glGetIntegeri_v;
        internal static PFNGLENABLEIPROC glEnablei;
        internal static PFNGLDISABLEIPROC glDisablei;
        internal static PFNGLISENABLEDIPROC glIsEnabledi;
        internal static PFNGLBEGINTRANSFORMFEEDBACKPROC glBeginTransformFeedback;
        internal static PFNGLENDTRANSFORMFEEDBACKPROC glEndTransformFeedback;
        internal static PFNGLBINDBUFFERRANGEPROC glBindBufferRange;
        internal static PFNGLBINDBUFFERBASEPROC glBindBufferBase;
        internal static PFNGLTRANSFORMFEEDBACKVARYINGSPROC glTransformFeedbackVaryings;
        internal static PFNGLGETTRANSFORMFEEDBACKVARYINGPROC glGetTransformFeedbackVarying;
        internal static PFNGLCLAMPCOLORPROC glClampColor;
        internal static PFNGLBEGINCONDITIONALRENDERPROC glBeginConditionalRender;
        internal static PFNGLENDCONDITIONALRENDERPROC glEndConditionalRender;
        internal static PFNGLVERTEXATTRIBIPOINTERPROC glVertexAttribIPointer;
        internal static PFNGLGETVERTEXATTRIBIIVPROC glGetVertexAttribIiv;
        internal static PFNGLGETVERTEXATTRIBIUIVPROC glGetVertexAttribIuiv;
        internal static PFNGLVERTEXATTRIBI1IPROC glVertexAttribI1i;
        internal static PFNGLVERTEXATTRIBI2IPROC glVertexAttribI2i;
        internal static PFNGLVERTEXATTRIBI3IPROC glVertexAttribI3i;
        internal static PFNGLVERTEXATTRIBI4IPROC glVertexAttribI4i;
        internal static PFNGLVERTEXATTRIBI1UIPROC glVertexAttribI1ui;
        internal static PFNGLVERTEXATTRIBI2UIPROC glVertexAttribI2ui;
        internal static PFNGLVERTEXATTRIBI3UIPROC glVertexAttribI3ui;
        internal static PFNGLVERTEXATTRIBI4UIPROC glVertexAttribI4ui;
        internal static PFNGLVERTEXATTRIBI1IVPROC glVertexAttribI1iv;
        internal static PFNGLVERTEXATTRIBI2IVPROC glVertexAttribI2iv;
        internal static PFNGLVERTEXATTRIBI3IVPROC glVertexAttribI3iv;
        internal static PFNGLVERTEXATTRIBI4IVPROC glVertexAttribI4iv;
        internal static PFNGLVERTEXATTRIBI1UIVPROC glVertexAttribI1uiv;
        internal static PFNGLVERTEXATTRIBI2UIVPROC glVertexAttribI2uiv;
        internal static PFNGLVERTEXATTRIBI3UIVPROC glVertexAttribI3uiv;
        internal static PFNGLVERTEXATTRIBI4UIVPROC glVertexAttribI4uiv;
        internal static PFNGLVERTEXATTRIBI4BVPROC glVertexAttribI4bv;
        internal static PFNGLVERTEXATTRIBI4SVPROC glVertexAttribI4sv;
        internal static PFNGLVERTEXATTRIBI4UBVPROC glVertexAttribI4ubv;
        internal static PFNGLVERTEXATTRIBI4USVPROC glVertexAttribI4usv;
        internal static PFNGLGETUNIFORMUIVPROC glGetUniformuiv;
        internal static PFNGLBINDFRAGDATALOCATIONPROC glBindFragDataLocation;
        internal static PFNGLGETFRAGDATALOCATIONPROC glGetFragDataLocation;
        internal static PFNGLUNIFORM1UIPROC glUniform1ui;
        internal static PFNGLUNIFORM2UIPROC glUniform2ui;
        internal static PFNGLUNIFORM3UIPROC glUniform3ui;
        internal static PFNGLUNIFORM4UIPROC glUniform4ui;
        internal static PFNGLUNIFORM1UIVPROC glUniform1uiv;
        internal static PFNGLUNIFORM2UIVPROC glUniform2uiv;
        internal static PFNGLUNIFORM3UIVPROC glUniform3uiv;
        internal static PFNGLUNIFORM4UIVPROC glUniform4uiv;
        internal static PFNGLTEXPARAMETERIIVPROC glTexParameterIiv;
        internal static PFNGLTEXPARAMETERIUIVPROC glTexParameterIuiv;
        internal static PFNGLGETTEXPARAMETERIIVPROC glGetTexParameterIiv;
        internal static PFNGLGETTEXPARAMETERIUIVPROC glGetTexParameterIuiv;
        internal static PFNGLCLEARBUFFERIVPROC glClearBufferiv;
        internal static PFNGLCLEARBUFFERUIVPROC glClearBufferuiv;
        internal static PFNGLCLEARBUFFERFVPROC glClearBufferfv;
        internal static PFNGLCLEARBUFFERFIPROC glClearBufferfi;
        internal static PFNGLGETSTRINGIPROC glGetStringi;
        internal static PFNGLISRENDERBUFFERPROC glIsRenderbuffer;
        internal static PFNGLBINDRENDERBUFFERPROC glBindRenderbuffer;
        internal static PFNGLDELETERENDERBUFFERSPROC glDeleteRenderbuffers;
        internal static PFNGLGENRENDERBUFFERSPROC glGenRenderbuffers;
        internal static PFNGLRENDERBUFFERSTORAGEPROC glRenderbufferStorage;
        internal static PFNGLGETRENDERBUFFERPARAMETERIVPROC glGetRenderbufferParameteriv;
        internal static PFNGLISFRAMEBUFFERPROC glIsFramebuffer;
        internal static PFNGLBINDFRAMEBUFFERPROC glBindFramebuffer;
        internal static PFNGLDELETEFRAMEBUFFERSPROC glDeleteFramebuffers;
        internal static PFNGLGENFRAMEBUFFERSPROC glGenFramebuffers;
        internal static PFNGLCHECKFRAMEBUFFERSTATUSPROC glCheckFramebufferStatus;
        internal static PFNGLFRAMEBUFFERTEXTURE1DPROC glFramebufferTexture1D;
        internal static PFNGLFRAMEBUFFERTEXTURE2DPROC glFramebufferTexture2D;
        internal static PFNGLFRAMEBUFFERTEXTURE3DPROC glFramebufferTexture3D;
        internal static PFNGLFRAMEBUFFERRENDERBUFFERPROC glFramebufferRenderbuffer;
        internal static PFNGLGETFRAMEBUFFERATTACHMENTPARAMETERIVPROC glGetFramebufferAttachmentParameteriv;
        internal static PFNGLGENERATEMIPMAPPROC glGenerateMipmap;
        internal static PFNGLBLITFRAMEBUFFERPROC glBlitFramebuffer;
        internal static PFNGLRENDERBUFFERSTORAGEMULTISAMPLEPROC glRenderbufferStorageMultisample;
        internal static PFNGLFRAMEBUFFERTEXTURELAYERPROC glFramebufferTextureLayer;
        internal static PFNGLMAPBUFFERRANGEPROC glMapBufferRange;
        internal static PFNGLFLUSHMAPPEDBUFFERRANGEPROC glFlushMappedBufferRange;
        internal static PFNGLBINDVERTEXARRAYPROC glBindVertexArray;
        internal static PFNGLDELETEVERTEXARRAYSPROC glDeleteVertexArrays;
        internal static PFNGLGENVERTEXARRAYSPROC glGenVertexArrays;
        internal static PFNGLISVERTEXARRAYPROC glIsVertexArray;

        #endregion

        #region gl_3_1

        internal static PFNGLDRAWARRAYSINSTANCEDPROC glDrawArraysInstanced;
        internal static PFNGLDRAWELEMENTSINSTANCEDPROC glDrawElementsInstanced;
        internal static PFNGLTEXBUFFERPROC glTexBuffer;
        internal static PFNGLPRIMITIVERESTARTINDEXPROC glPrimitiveRestartIndex;
        internal static PFNGLCOPYBUFFERSUBDATAPROC glCopyBufferSubData;
        internal static PFNGLGETUNIFORMINDICESPROC glGetUniformIndices;
        internal static PFNGLGETACTIVEUNIFORMSIVPROC glGetActiveUniformsiv;
        internal static PFNGLGETACTIVEUNIFORMNAMEPROC glGetActiveUniformName;
        internal static PFNGLGETUNIFORMBLOCKINDEXPROC glGetUniformBlockIndex;
        internal static PFNGLGETACTIVEUNIFORMBLOCKIVPROC glGetActiveUniformBlockiv;
        internal static PFNGLGETACTIVEUNIFORMBLOCKNAMEPROC glGetActiveUniformBlockName;
        internal static PFNGLUNIFORMBLOCKBINDINGPROC glUniformBlockBinding;

        #endregion

        #region gl_3_2

        internal static PFNGLDRAWELEMENTSBASEVERTEXPROC glDrawElementsBaseVertex;
        internal static PFNGLDRAWRANGEELEMENTSBASEVERTEXPROC glDrawRangeElementsBaseVertex;
        internal static PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXPROC glDrawElementsInstancedBaseVertex;
        internal static PFNGLMULTIDRAWELEMENTSBASEVERTEXPROC glMultiDrawElementsBaseVertex;
        internal static PFNGLPROVOKINGVERTEXPROC glProvokingVertex;
        internal static PFNGLFENCESYNCPROC glFenceSync;
        internal static PFNGLISSYNCPROC glIsSync;
        internal static PFNGLDELETESYNCPROC glDeleteSync;
        internal static PFNGLCLIENTWAITSYNCPROC glClientWaitSync;
        internal static PFNGLWAITSYNCPROC glWaitSync;
        internal static PFNGLGETINTEGER64VPROC glGetInteger64v;
        internal static PFNGLGETSYNCIVPROC glGetSynciv;
        internal static PFNGLGETINTEGER64I_VPROC glGetInteger64i_v;
        internal static PFNGLGETBUFFERPARAMETERI64VPROC glGetBufferParameteri64v;
        internal static PFNGLFRAMEBUFFERTEXTUREPROC glFramebufferTexture;
        internal static PFNGLTEXIMAGE2DMULTISAMPLEPROC glTexImage2DMultisample;
        internal static PFNGLTEXIMAGE3DMULTISAMPLEPROC glTexImage3DMultisample;
        internal static PFNGLGETMULTISAMPLEFVPROC glGetMultisamplefv;
        internal static PFNGLSAMPLEMASKIPROC glSampleMaski;

        #endregion

        #region gl_3_3

        internal static PFNGLBINDFRAGDATALOCATIONINDEXEDPROC glBindFragDataLocationIndexed;
        internal static PFNGLGETFRAGDATAINDEXPROC glGetFragDataIndex;
        internal static PFNGLGENSAMPLERSPROC glGenSamplers;
        internal static PFNGLDELETESAMPLERSPROC glDeleteSamplers;
        internal static PFNGLISSAMPLERPROC glIsSampler;
        internal static PFNGLBINDSAMPLERPROC glBindSampler;
        internal static PFNGLSAMPLERPARAMETERIPROC glSamplerParameteri;
        internal static PFNGLSAMPLERPARAMETERIVPROC glSamplerParameteriv;
        internal static PFNGLSAMPLERPARAMETERFPROC glSamplerParameterf;
        internal static PFNGLSAMPLERPARAMETERFVPROC glSamplerParameterfv;
        internal static PFNGLSAMPLERPARAMETERIIVPROC glSamplerParameterIiv;
        internal static PFNGLSAMPLERPARAMETERIUIVPROC glSamplerParameterIuiv;
        internal static PFNGLGETSAMPLERPARAMETERIVPROC glGetSamplerParameteriv;
        internal static PFNGLGETSAMPLERPARAMETERIIVPROC glGetSamplerParameterIiv;
        internal static PFNGLGETSAMPLERPARAMETERFVPROC glGetSamplerParameterfv;
        internal static PFNGLGETSAMPLERPARAMETERIUIVPROC glGetSamplerParameterIuiv;
        internal static PFNGLQUERYCOUNTERPROC glQueryCounter;
        internal static PFNGLGETQUERYOBJECTI64VPROC glGetQueryObjecti64v;
        internal static PFNGLGETQUERYOBJECTUI64VPROC glGetQueryObjectui64v;
        internal static PFNGLVERTEXATTRIBDIVISORPROC glVertexAttribDivisor;
        internal static PFNGLVERTEXATTRIBP1UIPROC glVertexAttribP1ui;
        internal static PFNGLVERTEXATTRIBP1UIVPROC glVertexAttribP1uiv;
        internal static PFNGLVERTEXATTRIBP2UIPROC glVertexAttribP2ui;
        internal static PFNGLVERTEXATTRIBP2UIVPROC glVertexAttribP2uiv;
        internal static PFNGLVERTEXATTRIBP3UIPROC glVertexAttribP3ui;
        internal static PFNGLVERTEXATTRIBP3UIVPROC glVertexAttribP3uiv;
        internal static PFNGLVERTEXATTRIBP4UIPROC glVertexAttribP4ui;
        internal static PFNGLVERTEXATTRIBP4UIVPROC glVertexAttribP4uiv;
        internal static PFNGLVERTEXP2UIPROC glVertexP2ui;
        internal static PFNGLVERTEXP2UIVPROC glVertexP2uiv;
        internal static PFNGLVERTEXP3UIPROC glVertexP3ui;
        internal static PFNGLVERTEXP3UIVPROC glVertexP3uiv;
        internal static PFNGLVERTEXP4UIPROC glVertexP4ui;
        internal static PFNGLVERTEXP4UIVPROC glVertexP4uiv;
        internal static PFNGLTEXCOORDP1UIPROC glTexCoordP1ui;
        internal static PFNGLTEXCOORDP1UIVPROC glTexCoordP1uiv;
        internal static PFNGLTEXCOORDP2UIPROC glTexCoordP2ui;
        internal static PFNGLTEXCOORDP2UIVPROC glTexCoordP2uiv;
        internal static PFNGLTEXCOORDP3UIPROC glTexCoordP3ui;
        internal static PFNGLTEXCOORDP3UIVPROC glTexCoordP3uiv;
        internal static PFNGLTEXCOORDP4UIPROC glTexCoordP4ui;
        internal static PFNGLTEXCOORDP4UIVPROC glTexCoordP4uiv;
        internal static PFNGLMULTITEXCOORDP1UIPROC glMultiTexCoordP1ui;
        internal static PFNGLMULTITEXCOORDP1UIVPROC glMultiTexCoordP1uiv;
        internal static PFNGLMULTITEXCOORDP2UIPROC glMultiTexCoordP2ui;
        internal static PFNGLMULTITEXCOORDP2UIVPROC glMultiTexCoordP2uiv;
        internal static PFNGLMULTITEXCOORDP3UIPROC glMultiTexCoordP3ui;
        internal static PFNGLMULTITEXCOORDP3UIVPROC glMultiTexCoordP3uiv;
        internal static PFNGLMULTITEXCOORDP4UIPROC glMultiTexCoordP4ui;
        internal static PFNGLMULTITEXCOORDP4UIVPROC glMultiTexCoordP4uiv;
        internal static PFNGLNORMALP3UIPROC glNormalP3ui;
        internal static PFNGLNORMALP3UIVPROC glNormalP3uiv;
        internal static PFNGLCOLORP3UIPROC glColorP3ui;
        internal static PFNGLCOLORP3UIVPROC glColorP3uiv;
        internal static PFNGLCOLORP4UIPROC glColorP4ui;
        internal static PFNGLCOLORP4UIVPROC glColorP4uiv;
        internal static PFNGLSECONDARYCOLORP3UIPROC glSecondaryColorP3ui;
        internal static PFNGLSECONDARYCOLORP3UIVPROC glSecondaryColorP3uiv;

        #endregion

        #region gl_4_0

        internal static PFNGLMINSAMPLESHADINGPROC glMinSampleShading;
        internal static PFNGLBLENDEQUATIONIPROC glBlendEquationi;
        internal static PFNGLBLENDEQUATIONSEPARATEIPROC glBlendEquationSeparatei;
        internal static PFNGLBLENDFUNCIPROC glBlendFunci;
        internal static PFNGLBLENDFUNCSEPARATEIPROC glBlendFuncSeparatei;
        internal static PFNGLDRAWARRAYSINDIRECTPROC glDrawArraysIndirect;
        internal static PFNGLDRAWELEMENTSINDIRECTPROC glDrawElementsIndirect;
        internal static PFNGLUNIFORM1DPROC glUniform1d;
        internal static PFNGLUNIFORM2DPROC glUniform2d;
        internal static PFNGLUNIFORM3DPROC glUniform3d;
        internal static PFNGLUNIFORM4DPROC glUniform4d;
        internal static PFNGLUNIFORM1DVPROC glUniform1dv;
        internal static PFNGLUNIFORM2DVPROC glUniform2dv;
        internal static PFNGLUNIFORM3DVPROC glUniform3dv;
        internal static PFNGLUNIFORM4DVPROC glUniform4dv;
        internal static PFNGLUNIFORMMATRIX2DVPROC glUniformMatrix2dv;
        internal static PFNGLUNIFORMMATRIX3DVPROC glUniformMatrix3dv;
        internal static PFNGLUNIFORMMATRIX4DVPROC glUniformMatrix4dv;
        internal static PFNGLUNIFORMMATRIX2X3DVPROC glUniformMatrix2x3dv;
        internal static PFNGLUNIFORMMATRIX2X4DVPROC glUniformMatrix2x4dv;
        internal static PFNGLUNIFORMMATRIX3X2DVPROC glUniformMatrix3x2dv;
        internal static PFNGLUNIFORMMATRIX3X4DVPROC glUniformMatrix3x4dv;
        internal static PFNGLUNIFORMMATRIX4X2DVPROC glUniformMatrix4x2dv;
        internal static PFNGLUNIFORMMATRIX4X3DVPROC glUniformMatrix4x3dv;
        internal static PFNGLGETUNIFORMDVPROC glGetUniformdv;
        internal static PFNGLGETSUBROUTINEUNIFORMLOCATIONPROC glGetSubroutineUniformLocation;
        internal static PFNGLGETSUBROUTINEINDEXPROC glGetSubroutineIndex;
        internal static PFNGLGETACTIVESUBROUTINEUNIFORMIVPROC glGetActiveSubroutineUniformiv;
        internal static PFNGLGETACTIVESUBROUTINEUNIFORMNAMEPROC glGetActiveSubroutineUniformName;
        internal static PFNGLGETACTIVESUBROUTINENAMEPROC glGetActiveSubroutineName;
        internal static PFNGLUNIFORMSUBROUTINESUIVPROC glUniformSubroutinesuiv;
        internal static PFNGLGETUNIFORMSUBROUTINEUIVPROC glGetUniformSubroutineuiv;
        internal static PFNGLGETPROGRAMSTAGEIVPROC glGetProgramStageiv;
        internal static PFNGLPATCHPARAMETERIPROC glPatchParameteri;
        internal static PFNGLPATCHPARAMETERFVPROC glPatchParameterfv;
        internal static PFNGLBINDTRANSFORMFEEDBACKPROC glBindTransformFeedback;
        internal static PFNGLDELETETRANSFORMFEEDBACKSPROC glDeleteTransformFeedbacks;
        internal static PFNGLGENTRANSFORMFEEDBACKSPROC glGenTransformFeedbacks;
        internal static PFNGLISTRANSFORMFEEDBACKPROC glIsTransformFeedback;
        internal static PFNGLPAUSETRANSFORMFEEDBACKPROC glPauseTransformFeedback;
        internal static PFNGLRESUMETRANSFORMFEEDBACKPROC glResumeTransformFeedback;
        internal static PFNGLDRAWTRANSFORMFEEDBACKPROC glDrawTransformFeedback;
        internal static PFNGLDRAWTRANSFORMFEEDBACKSTREAMPROC glDrawTransformFeedbackStream;
        internal static PFNGLBEGINQUERYINDEXEDPROC glBeginQueryIndexed;
        internal static PFNGLENDQUERYINDEXEDPROC glEndQueryIndexed;
        internal static PFNGLGETQUERYINDEXEDIVPROC glGetQueryIndexediv;

        #endregion 

        #region gl_4_1

        internal static PFNGLRELEASESHADERCOMPILERPROC glReleaseShaderCompiler;
        internal static PFNGLSHADERBINARYPROC glShaderBinary;
        internal static PFNGLGETSHADERPRECISIONFORMATPROC glGetShaderPrecisionFormat;
        internal static PFNGLDEPTHRANGEFPROC glDepthRangef;
        internal static PFNGLCLEARDEPTHFPROC glClearDepthf;
        internal static PFNGLGETPROGRAMBINARYPROC glGetProgramBinary;
        internal static PFNGLPROGRAMBINARYPROC glProgramBinary;
        internal static PFNGLPROGRAMPARAMETERIPROC glProgramParameteri;
        internal static PFNGLUSEPROGRAMSTAGESPROC glUseProgramStages;
        internal static PFNGLACTIVESHADERPROGRAMPROC glActiveShaderProgram;
        internal static PFNGLCREATESHADERPROGRAMVPROC glCreateShaderProgramv;
        internal static PFNGLBINDPROGRAMPIPELINEPROC glBindProgramPipeline;
        internal static PFNGLDELETEPROGRAMPIPELINESPROC glDeleteProgramPipelines;
        internal static PFNGLGENPROGRAMPIPELINESPROC glGenProgramPipelines;
        internal static PFNGLISPROGRAMPIPELINEPROC glIsProgramPipeline;
        internal static PFNGLGETPROGRAMPIPELINEIVPROC glGetProgramPipelineiv;
        internal static PFNGLPROGRAMUNIFORM1IPROC glProgramUniform1i;
        internal static PFNGLPROGRAMUNIFORM1IVPROC glProgramUniform1iv;
        internal static PFNGLPROGRAMUNIFORM1FPROC glProgramUniform1f;
        internal static PFNGLPROGRAMUNIFORM1FVPROC glProgramUniform1fv;
        internal static PFNGLPROGRAMUNIFORM1DPROC glProgramUniform1d;
        internal static PFNGLPROGRAMUNIFORM1DVPROC glProgramUniform1dv;
        internal static PFNGLPROGRAMUNIFORM1UIPROC glProgramUniform1ui;
        internal static PFNGLPROGRAMUNIFORM1UIVPROC glProgramUniform1uiv;
        internal static PFNGLPROGRAMUNIFORM2IPROC glProgramUniform2i;
        internal static PFNGLPROGRAMUNIFORM2IVPROC glProgramUniform2iv;
        internal static PFNGLPROGRAMUNIFORM2FPROC glProgramUniform2f;
        internal static PFNGLPROGRAMUNIFORM2FVPROC glProgramUniform2fv;
        internal static PFNGLPROGRAMUNIFORM2DPROC glProgramUniform2d;
        internal static PFNGLPROGRAMUNIFORM2DVPROC glProgramUniform2dv;
        internal static PFNGLPROGRAMUNIFORM2UIPROC glProgramUniform2ui;
        internal static PFNGLPROGRAMUNIFORM2UIVPROC glProgramUniform2uiv;
        internal static PFNGLPROGRAMUNIFORM3IPROC glProgramUniform3i;
        internal static PFNGLPROGRAMUNIFORM3IVPROC glProgramUniform3iv;
        internal static PFNGLPROGRAMUNIFORM3FPROC glProgramUniform3f;
        internal static PFNGLPROGRAMUNIFORM3FVPROC glProgramUniform3fv;
        internal static PFNGLPROGRAMUNIFORM3DPROC glProgramUniform3d;
        internal static PFNGLPROGRAMUNIFORM3DVPROC glProgramUniform3dv;
        internal static PFNGLPROGRAMUNIFORM3UIPROC glProgramUniform3ui;
        internal static PFNGLPROGRAMUNIFORM3UIVPROC glProgramUniform3uiv;
        internal static PFNGLPROGRAMUNIFORM4IPROC glProgramUniform4i;
        internal static PFNGLPROGRAMUNIFORM4IVPROC glProgramUniform4iv;
        internal static PFNGLPROGRAMUNIFORM4FPROC glProgramUniform4f;
        internal static PFNGLPROGRAMUNIFORM4FVPROC glProgramUniform4fv;
        internal static PFNGLPROGRAMUNIFORM4DPROC glProgramUniform4d;
        internal static PFNGLPROGRAMUNIFORM4DVPROC glProgramUniform4dv;
        internal static PFNGLPROGRAMUNIFORM4UIPROC glProgramUniform4ui;
        internal static PFNGLPROGRAMUNIFORM4UIVPROC glProgramUniform4uiv;
        internal static PFNGLPROGRAMUNIFORMMATRIX2FVPROC glProgramUniformMatrix2fv;
        internal static PFNGLPROGRAMUNIFORMMATRIX3FVPROC glProgramUniformMatrix3fv;
        internal static PFNGLPROGRAMUNIFORMMATRIX4FVPROC glProgramUniformMatrix4fv;
        internal static PFNGLPROGRAMUNIFORMMATRIX2DVPROC glProgramUniformMatrix2dv;
        internal static PFNGLPROGRAMUNIFORMMATRIX3DVPROC glProgramUniformMatrix3dv;
        internal static PFNGLPROGRAMUNIFORMMATRIX4DVPROC glProgramUniformMatrix4dv;
        internal static PFNGLPROGRAMUNIFORMMATRIX2X3FVPROC glProgramUniformMatrix2x3fv;
        internal static PFNGLPROGRAMUNIFORMMATRIX3X2FVPROC glProgramUniformMatrix3x2fv;
        internal static PFNGLPROGRAMUNIFORMMATRIX2X4FVPROC glProgramUniformMatrix2x4fv;
        internal static PFNGLPROGRAMUNIFORMMATRIX4X2FVPROC glProgramUniformMatrix4x2fv;
        internal static PFNGLPROGRAMUNIFORMMATRIX3X4FVPROC glProgramUniformMatrix3x4fv;
        internal static PFNGLPROGRAMUNIFORMMATRIX4X3FVPROC glProgramUniformMatrix4x3fv;
        internal static PFNGLPROGRAMUNIFORMMATRIX2X3DVPROC glProgramUniformMatrix2x3dv;
        internal static PFNGLPROGRAMUNIFORMMATRIX3X2DVPROC glProgramUniformMatrix3x2dv;
        internal static PFNGLPROGRAMUNIFORMMATRIX2X4DVPROC glProgramUniformMatrix2x4dv;
        internal static PFNGLPROGRAMUNIFORMMATRIX4X2DVPROC glProgramUniformMatrix4x2dv;
        internal static PFNGLPROGRAMUNIFORMMATRIX3X4DVPROC glProgramUniformMatrix3x4dv;
        internal static PFNGLPROGRAMUNIFORMMATRIX4X3DVPROC glProgramUniformMatrix4x3dv;
        internal static PFNGLVALIDATEPROGRAMPIPELINEPROC glValidateProgramPipeline;
        internal static PFNGLGETPROGRAMPIPELINEINFOLOGPROC glGetProgramPipelineInfoLog;
        internal static PFNGLVERTEXATTRIBL1DPROC glVertexAttribL1d;
        internal static PFNGLVERTEXATTRIBL2DPROC glVertexAttribL2d;
        internal static PFNGLVERTEXATTRIBL3DPROC glVertexAttribL3d;
        internal static PFNGLVERTEXATTRIBL4DPROC glVertexAttribL4d;
        internal static PFNGLVERTEXATTRIBL1DVPROC glVertexAttribL1dv;
        internal static PFNGLVERTEXATTRIBL2DVPROC glVertexAttribL2dv;
        internal static PFNGLVERTEXATTRIBL3DVPROC glVertexAttribL3dv;
        internal static PFNGLVERTEXATTRIBL4DVPROC glVertexAttribL4dv;
        internal static PFNGLVERTEXATTRIBLPOINTERPROC glVertexAttribLPointer;
        internal static PFNGLGETVERTEXATTRIBLDVPROC glGetVertexAttribLdv;
        internal static PFNGLVIEWPORTARRAYVPROC glViewportArrayv;
        internal static PFNGLVIEWPORTINDEXEDFPROC glViewportIndexedf;
        internal static PFNGLVIEWPORTINDEXEDFVPROC glViewportIndexedfv;
        internal static PFNGLSCISSORARRAYVPROC glScissorArrayv;
        internal static PFNGLSCISSORINDEXEDPROC glScissorIndexed;
        internal static PFNGLSCISSORINDEXEDVPROC glScissorIndexedv;
        internal static PFNGLDEPTHRANGEARRAYVPROC glDepthRangeArrayv;
        internal static PFNGLDEPTHRANGEINDEXEDPROC glDepthRangeIndexed;
        internal static PFNGLGETFLOATI_VPROC glGetFloati_v;
        internal static PFNGLGETDOUBLEI_VPROC glGetDoublei_v;

        #endregion

        #region gl_4_2

        internal static PFNGLDRAWARRAYSINSTANCEDBASEINSTANCEPROC glDrawArraysInstancedBaseInstance;
        internal static PFNGLDRAWELEMENTSINSTANCEDBASEINSTANCEPROC glDrawElementsInstancedBaseInstance;
        internal static PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXBASEINSTANCEPROC glDrawElementsInstancedBaseVertexBaseInstance;
        internal static PFNGLGETINTERNALFORMATIVPROC glGetInternalformativ;
        internal static PFNGLGETACTIVEATOMICCOUNTERBUFFERIVPROC glGetActiveAtomicCounterBufferiv;
        internal static PFNGLBINDIMAGETEXTUREPROC glBindImageTexture;
        internal static PFNGLMEMORYBARRIERPROC glMemoryBarrier;
        internal static PFNGLTEXSTORAGE1DPROC glTexStorage1D;
        internal static PFNGLTEXSTORAGE2DPROC glTexStorage2D;
        internal static PFNGLTEXSTORAGE3DPROC glTexStorage3D;
        internal static PFNGLDRAWTRANSFORMFEEDBACKINSTANCEDPROC glDrawTransformFeedbackInstanced;
        internal static PFNGLDRAWTRANSFORMFEEDBACKSTREAMINSTANCEDPROC glDrawTransformFeedbackStreamInstanced;

        #endregion

        #region gl_4_3

        internal static PFNGLCLEARBUFFERDATAPROC glClearBufferData;
        internal static PFNGLCLEARBUFFERSUBDATAPROC glClearBufferSubData;
        internal static PFNGLDISPATCHCOMPUTEPROC glDispatchCompute;
        internal static PFNGLDISPATCHCOMPUTEINDIRECTPROC glDispatchComputeIndirect;
        internal static PFNGLCOPYIMAGESUBDATAPROC glCopyImageSubData;
        internal static PFNGLFRAMEBUFFERPARAMETERIPROC glFramebufferParameteri;
        internal static PFNGLGETFRAMEBUFFERPARAMETERIVPROC glGetFramebufferParameteriv;
        internal static PFNGLGETINTERNALFORMATI64VPROC glGetInternalformati64v;
        internal static PFNGLINVALIDATETEXSUBIMAGEPROC glInvalidateTexSubImage;
        internal static PFNGLINVALIDATETEXIMAGEPROC glInvalidateTexImage;
        internal static PFNGLINVALIDATEBUFFERSUBDATAPROC glInvalidateBufferSubData;
        internal static PFNGLINVALIDATEBUFFERDATAPROC glInvalidateBufferData;
        internal static PFNGLINVALIDATEFRAMEBUFFERPROC glInvalidateFramebuffer;
        internal static PFNGLINVALIDATESUBFRAMEBUFFERPROC glInvalidateSubFramebuffer;
        internal static PFNGLMULTIDRAWARRAYSINDIRECTPROC glMultiDrawArraysIndirect;
        internal static PFNGLMULTIDRAWELEMENTSINDIRECTPROC glMultiDrawElementsIndirect;
        internal static PFNGLGETPROGRAMINTERFACEIVPROC glGetProgramInterfaceiv;
        internal static PFNGLGETPROGRAMRESOURCEINDEXPROC glGetProgramResourceIndex;
        internal static PFNGLGETPROGRAMRESOURCENAMEPROC glGetProgramResourceName;
        internal static PFNGLGETPROGRAMRESOURCEIVPROC glGetProgramResourceiv;
        internal static PFNGLGETPROGRAMRESOURCELOCATIONPROC glGetProgramResourceLocation;
        internal static PFNGLGETPROGRAMRESOURCELOCATIONINDEXPROC glGetProgramResourceLocationIndex;
        internal static PFNGLSHADERSTORAGEBLOCKBINDINGPROC glShaderStorageBlockBinding;
        internal static PFNGLTEXBUFFERRANGEPROC glTexBufferRange;
        internal static PFNGLTEXSTORAGE2DMULTISAMPLEPROC glTexStorage2DMultisample;
        internal static PFNGLTEXSTORAGE3DMULTISAMPLEPROC glTexStorage3DMultisample;
        internal static PFNGLTEXTUREVIEWPROC glTextureView;
        internal static PFNGLBINDVERTEXBUFFERPROC glBindVertexBuffer;
        internal static PFNGLVERTEXATTRIBFORMATPROC glVertexAttribFormat;
        internal static PFNGLVERTEXATTRIBIFORMATPROC glVertexAttribIFormat;
        internal static PFNGLVERTEXATTRIBLFORMATPROC glVertexAttribLFormat;
        internal static PFNGLVERTEXATTRIBBINDINGPROC glVertexAttribBinding;
        internal static PFNGLVERTEXBINDINGDIVISORPROC glVertexBindingDivisor;
        internal static PFNGLDEBUGMESSAGECONTROLPROC glDebugMessageControl;
        internal static PFNGLDEBUGMESSAGEINSERTPROC glDebugMessageInsert;
        internal static PFNGLDEBUGMESSAGECALLBACKPROC glDebugMessageCallback;
        internal static PFNGLGETDEBUGMESSAGELOGPROC glGetDebugMessageLog;
        internal static PFNGLPUSHDEBUGGROUPPROC glPushDebugGroup;
        internal static PFNGLPOPDEBUGGROUPPROC glPopDebugGroup;
        internal static PFNGLOBJECTLABELPROC glObjectLabel;
        internal static PFNGLGETOBJECTLABELPROC glGetObjectLabel;
        internal static PFNGLOBJECTPTRLABELPROC glObjectPtrLabel;
        internal static PFNGLGETOBJECTPTRLABELPROC glGetObjectPtrLabel;
        internal static PFNGLGETPOINTERVPROC glGetPointerv;

        #endregion

        #region gl_4_4

        internal static PFNGLBUFFERSTORAGEPROC glBufferStorage;
        internal static PFNGLCLEARTEXIMAGEPROC glClearTexImage;
        internal static PFNGLCLEARTEXSUBIMAGEPROC glClearTexSubImage;
        internal static PFNGLBINDBUFFERSBASEPROC glBindBuffersBase;
        internal static PFNGLBINDBUFFERSRANGEPROC glBindBuffersRange;
        internal static PFNGLBINDTEXTURESPROC glBindTextures;
        internal static PFNGLBINDSAMPLERSPROC glBindSamplers;
        internal static PFNGLBINDIMAGETEXTURESPROC glBindImageTextures;
        internal static PFNGLBINDVERTEXBUFFERSPROC glBindVertexBuffers;

        #endregion

        #region gl_4_5

        internal static PFNGLCLIPCONTROLPROC glClipControl;
        internal static PFNGLCREATETRANSFORMFEEDBACKSPROC glCreateTransformFeedbacks;
        internal static PFNGLTRANSFORMFEEDBACKBUFFERBASEPROC glTransformFeedbackBufferBase;
        internal static PFNGLTRANSFORMFEEDBACKBUFFERRANGEPROC glTransformFeedbackBufferRange;
        internal static PFNGLGETTRANSFORMFEEDBACKIVPROC glGetTransformFeedbackiv;
        internal static PFNGLGETTRANSFORMFEEDBACKI_VPROC glGetTransformFeedbacki_v;
        internal static PFNGLGETTRANSFORMFEEDBACKI64_VPROC glGetTransformFeedbacki64_v;
        internal static PFNGLCREATEBUFFERSPROC glCreateBuffers;
        internal static PFNGLNAMEDBUFFERSTORAGEPROC glNamedBufferStorage;
        internal static PFNGLNAMEDBUFFERDATAPROC glNamedBufferData;
        internal static PFNGLNAMEDBUFFERSUBDATAPROC glNamedBufferSubData;
        internal static PFNGLCOPYNAMEDBUFFERSUBDATAPROC glCopyNamedBufferSubData;
        internal static PFNGLCLEARNAMEDBUFFERDATAPROC glClearNamedBufferData;
        internal static PFNGLCLEARNAMEDBUFFERSUBDATAPROC glClearNamedBufferSubData;
        internal static PFNGLMAPNAMEDBUFFERPROC glMapNamedBuffer;
        internal static PFNGLMAPNAMEDBUFFERRANGEPROC glMapNamedBufferRange;
        internal static PFNGLUNMAPNAMEDBUFFERPROC glUnmapNamedBuffer;
        internal static PFNGLFLUSHMAPPEDNAMEDBUFFERRANGEPROC glFlushMappedNamedBufferRange;
        internal static PFNGLGETNAMEDBUFFERPARAMETERIVPROC glGetNamedBufferParameteriv;
        internal static PFNGLGETNAMEDBUFFERPARAMETERI64VPROC glGetNamedBufferParameteri64v;
        internal static PFNGLGETNAMEDBUFFERPOINTERVPROC glGetNamedBufferPointerv;
        internal static PFNGLGETNAMEDBUFFERSUBDATAPROC glGetNamedBufferSubData;
        internal static PFNGLCREATEFRAMEBUFFERSPROC glCreateFramebuffers;
        internal static PFNGLNAMEDFRAMEBUFFERRENDERBUFFERPROC glNamedFramebufferRenderbuffer;
        internal static PFNGLNAMEDFRAMEBUFFERPARAMETERIPROC glNamedFramebufferParameteri;
        internal static PFNGLNAMEDFRAMEBUFFERTEXTUREPROC glNamedFramebufferTexture;
        internal static PFNGLNAMEDFRAMEBUFFERTEXTURELAYERPROC glNamedFramebufferTextureLayer;
        internal static PFNGLNAMEDFRAMEBUFFERDRAWBUFFERPROC glNamedFramebufferDrawBuffer;
        internal static PFNGLNAMEDFRAMEBUFFERDRAWBUFFERSPROC glNamedFramebufferDrawBuffers;
        internal static PFNGLNAMEDFRAMEBUFFERREADBUFFERPROC glNamedFramebufferReadBuffer;
        internal static PFNGLINVALIDATENAMEDFRAMEBUFFERDATAPROC glInvalidateNamedFramebufferData;
        internal static PFNGLINVALIDATENAMEDFRAMEBUFFERSUBDATAPROC glInvalidateNamedFramebufferSubData;
        internal static PFNGLCLEARNAMEDFRAMEBUFFERIVPROC glClearNamedFramebufferiv;
        internal static PFNGLCLEARNAMEDFRAMEBUFFERUIVPROC glClearNamedFramebufferuiv;
        internal static PFNGLCLEARNAMEDFRAMEBUFFERFVPROC glClearNamedFramebufferfv;
        internal static PFNGLCLEARNAMEDFRAMEBUFFERFIPROC glClearNamedFramebufferfi;
        internal static PFNGLBLITNAMEDFRAMEBUFFERPROC glBlitNamedFramebuffer;
        internal static PFNGLCHECKNAMEDFRAMEBUFFERSTATUSPROC glCheckNamedFramebufferStatus;
        internal static PFNGLGETNAMEDFRAMEBUFFERPARAMETERIVPROC glGetNamedFramebufferParameteriv;
        internal static PFNGLGETNAMEDFRAMEBUFFERATTACHMENTPARAMETERIVPROC glGetNamedFramebufferAttachmentParameteriv;
        internal static PFNGLCREATERENDERBUFFERSPROC glCreateRenderbuffers;
        internal static PFNGLNAMEDRENDERBUFFERSTORAGEPROC glNamedRenderbufferStorage;
        internal static PFNGLNAMEDRENDERBUFFERSTORAGEMULTISAMPLEPROC glNamedRenderbufferStorageMultisample;
        internal static PFNGLGETNAMEDRENDERBUFFERPARAMETERIVPROC glGetNamedRenderbufferParameteriv;
        internal static PFNGLCREATETEXTURESPROC glCreateTextures;
        internal static PFNGLTEXTUREBUFFERPROC glTextureBuffer;
        internal static PFNGLTEXTUREBUFFERRANGEPROC glTextureBufferRange;
        internal static PFNGLTEXTURESTORAGE1DPROC glTextureStorage1D;
        internal static PFNGLTEXTURESTORAGE2DPROC glTextureStorage2D;
        internal static PFNGLTEXTURESTORAGE3DPROC glTextureStorage3D;
        internal static PFNGLTEXTURESTORAGE2DMULTISAMPLEPROC glTextureStorage2DMultisample;
        internal static PFNGLTEXTURESTORAGE3DMULTISAMPLEPROC glTextureStorage3DMultisample;
        internal static PFNGLTEXTURESUBIMAGE1DPROC glTextureSubImage1D;
        internal static PFNGLTEXTURESUBIMAGE2DPROC glTextureSubImage2D;
        internal static PFNGLTEXTURESUBIMAGE3DPROC glTextureSubImage3D;
        internal static PFNGLCOMPRESSEDTEXTURESUBIMAGE1DPROC glCompressedTextureSubImage1D;
        internal static PFNGLCOMPRESSEDTEXTURESUBIMAGE2DPROC glCompressedTextureSubImage2D;
        internal static PFNGLCOMPRESSEDTEXTURESUBIMAGE3DPROC glCompressedTextureSubImage3D;
        internal static PFNGLCOPYTEXTURESUBIMAGE1DPROC glCopyTextureSubImage1D;
        internal static PFNGLCOPYTEXTURESUBIMAGE2DPROC glCopyTextureSubImage2D;
        internal static PFNGLCOPYTEXTURESUBIMAGE3DPROC glCopyTextureSubImage3D;
        internal static PFNGLTEXTUREPARAMETERFPROC glTextureParameterf;
        internal static PFNGLTEXTUREPARAMETERFVPROC glTextureParameterfv;
        internal static PFNGLTEXTUREPARAMETERIPROC glTextureParameteri;
        internal static PFNGLTEXTUREPARAMETERIIVPROC glTextureParameterIiv;
        internal static PFNGLTEXTUREPARAMETERIUIVPROC glTextureParameterIuiv;
        internal static PFNGLTEXTUREPARAMETERIVPROC glTextureParameteriv;
        internal static PFNGLGENERATETEXTUREMIPMAPPROC glGenerateTextureMipmap;
        internal static PFNGLBINDTEXTUREUNITPROC glBindTextureUnit;
        internal static PFNGLGETTEXTUREIMAGEPROC glGetTextureImage;
        internal static PFNGLGETCOMPRESSEDTEXTUREIMAGEPROC glGetCompressedTextureImage;
        internal static PFNGLGETTEXTURELEVELPARAMETERFVPROC glGetTextureLevelParameterfv;
        internal static PFNGLGETTEXTURELEVELPARAMETERIVPROC glGetTextureLevelParameteriv;
        internal static PFNGLGETTEXTUREPARAMETERFVPROC glGetTextureParameterfv;
        internal static PFNGLGETTEXTUREPARAMETERIIVPROC glGetTextureParameterIiv;
        internal static PFNGLGETTEXTUREPARAMETERIUIVPROC glGetTextureParameterIuiv;
        internal static PFNGLGETTEXTUREPARAMETERIVPROC glGetTextureParameteriv;
        internal static PFNGLCREATEVERTEXARRAYSPROC glCreateVertexArrays;
        internal static PFNGLDISABLEVERTEXARRAYATTRIBPROC glDisableVertexArrayAttrib;
        internal static PFNGLENABLEVERTEXARRAYATTRIBPROC glEnableVertexArrayAttrib;
        internal static PFNGLVERTEXARRAYELEMENTBUFFERPROC glVertexArrayElementBuffer;
        internal static PFNGLVERTEXARRAYVERTEXBUFFERPROC glVertexArrayVertexBuffer;
        internal static PFNGLVERTEXARRAYVERTEXBUFFERSPROC glVertexArrayVertexBuffers;
        internal static PFNGLVERTEXARRAYATTRIBBINDINGPROC glVertexArrayAttribBinding;
        internal static PFNGLVERTEXARRAYATTRIBFORMATPROC glVertexArrayAttribFormat;
        internal static PFNGLVERTEXARRAYATTRIBIFORMATPROC glVertexArrayAttribIFormat;
        internal static PFNGLVERTEXARRAYATTRIBLFORMATPROC glVertexArrayAttribLFormat;
        internal static PFNGLVERTEXARRAYBINDINGDIVISORPROC glVertexArrayBindingDivisor;
        internal static PFNGLGETVERTEXARRAYIVPROC glGetVertexArrayiv;
        internal static PFNGLGETVERTEXARRAYINDEXEDIVPROC glGetVertexArrayIndexediv;
        internal static PFNGLGETVERTEXARRAYINDEXED64IVPROC glGetVertexArrayIndexed64iv;
        internal static PFNGLCREATESAMPLERSPROC glCreateSamplers;
        internal static PFNGLCREATEPROGRAMPIPELINESPROC glCreateProgramPipelines;
        internal static PFNGLCREATEQUERIESPROC glCreateQueries;
        internal static PFNGLGETQUERYBUFFEROBJECTI64VPROC glGetQueryBufferObjecti64v;
        internal static PFNGLGETQUERYBUFFEROBJECTIVPROC glGetQueryBufferObjectiv;
        internal static PFNGLGETQUERYBUFFEROBJECTUI64VPROC glGetQueryBufferObjectui64v;
        internal static PFNGLGETQUERYBUFFEROBJECTUIVPROC glGetQueryBufferObjectuiv;
        internal static PFNGLMEMORYBARRIERBYREGIONPROC glMemoryBarrierByRegion;
        internal static PFNGLGETTEXTURESUBIMAGEPROC glGetTextureSubImage;
        internal static PFNGLGETCOMPRESSEDTEXTURESUBIMAGEPROC glGetCompressedTextureSubImage;
        internal static PFNGLGETGRAPHICSRESETSTATUSPROC glGetGraphicsResetStatus;
        internal static PFNGLGETNCOMPRESSEDTEXIMAGEPROC glGetnCompressedTexImage;
        internal static PFNGLGETNTEXIMAGEPROC glGetnTexImage;
        internal static PFNGLGETNUNIFORMDVPROC glGetnUniformdv;
        internal static PFNGLGETNUNIFORMFVPROC glGetnUniformfv;
        internal static PFNGLGETNUNIFORMIVPROC glGetnUniformiv;
        internal static PFNGLGETNUNIFORMUIVPROC glGetnUniformuiv;
        internal static PFNGLREADNPIXELSPROC glReadnPixels;
        internal static PFNGLGETNMAPDVPROC glGetnMapdv;
        internal static PFNGLGETNMAPFVPROC glGetnMapfv;
        internal static PFNGLGETNMAPIVPROC glGetnMapiv;
        internal static PFNGLGETNPIXELMAPFVPROC glGetnPixelMapfv;
        internal static PFNGLGETNPIXELMAPUIVPROC glGetnPixelMapuiv;
        internal static PFNGLGETNPIXELMAPUSVPROC glGetnPixelMapusv;
        internal static PFNGLGETNPOLYGONSTIPPLEPROC glGetnPolygonStipple;
        internal static PFNGLGETNCOLORTABLEPROC glGetnColorTable;
        internal static PFNGLGETNCONVOLUTIONFILTERPROC glGetnConvolutionFilter;
        internal static PFNGLGETNSEPARABLEFILTERPROC glGetnSeparableFilter;
        internal static PFNGLGETNHISTOGRAMPROC glGetnHistogram;
        internal static PFNGLGETNMINMAXPROC glGetnMinmax;
        internal static PFNGLTEXTUREBARRIERPROC glTextureBarrier;

        #endregion

        #region gl_4_6

        internal static PFNGLSPECIALIZESHADERPROC glSpecializeShader;
        internal static PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC glMultiDrawArraysIndirectCount;
        internal static PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC glMultiDrawElementsIndirectCount;
        internal static PFNGLPOLYGONOFFSETCLAMPPROC glPolygonOffsetClamp;

        #endregion

        #region gles

        internal static PFNGLALPHAFUNCPROC glAlphaFunc;
        internal static PFNGLCLIPPLANEFPROC glClipPlanef;
        internal static PFNGLCOLOR4FPROC glColor4f;
        internal static PFNGLFOGFPROC glFogf;
        internal static PFNGLFOGFVPROC glFogfv;
        internal static PFNGLFRUSTUMFPROC glFrustumf;
        internal static PFNGLGETCLIPPLANEFPROC glGetClipPlanef;
        internal static PFNGLGETLIGHTFVPROC glGetLightfv;
        internal static PFNGLGETMATERIALFVPROC glGetMaterialfv;
        internal static PFNGLGETTEXENVFVPROC glGetTexEnvfv;
        internal static PFNGLLIGHTMODELFPROC glLightModelf;
        internal static PFNGLLIGHTMODELFVPROC glLightModelfv;
        internal static PFNGLLIGHTFPROC glLightf;
        internal static PFNGLLIGHTFVPROC glLightfv;
        internal static PFNGLLOADMATRIXFPROC glLoadMatrixf;
        internal static PFNGLMATERIALFPROC glMaterialf;
        internal static PFNGLMATERIALFVPROC glMaterialfv;
        internal static PFNGLMULTMATRIXFPROC glMultMatrixf;
        internal static PFNGLMULTITEXCOORD4FPROC glMultiTexCoord4f;
        internal static PFNGLNORMAL3FPROC glNormal3f;
        internal static PFNGLORTHOFPROC glOrthof;
        internal static PFNGLROTATEFPROC glRotatef;
        internal static PFNGLSCALEFPROC glScalef;
        internal static PFNGLTEXENVFPROC glTexEnvf;
        internal static PFNGLTEXENVFVPROC glTexEnvfv;
        internal static PFNGLTRANSLATEFPROC glTranslatef;
        internal static PFNGLALPHAFUNCXPROC glAlphaFuncx;
        internal static PFNGLCLEARCOLORXPROC glClearColorx;
        internal static PFNGLCLEARDEPTHXPROC glClearDepthx;
        internal static PFNGLCLIENTACTIVETEXTUREPROC glClientActiveTexture;
        internal static PFNGLCLIPPLANEXPROC glClipPlanex;
        internal static PFNGLCOLOR4UBPROC glColor4ub;
        internal static PFNGLCOLOR4XPROC glColor4x;
        internal static PFNGLCOLORPOINTERPROC glColorPointer;
        internal static PFNGLDEPTHRANGEXPROC glDepthRangex;
        internal static PFNGLDISABLECLIENTSTATEPROC glDisableClientState;
        internal static PFNGLENABLECLIENTSTATEPROC glEnableClientState;
        internal static PFNGLFOGXPROC glFogx;
        internal static PFNGLFOGXVPROC glFogxv;
        internal static PFNGLFRUSTUMXPROC glFrustumx;
        internal static PFNGLGETCLIPPLANEXPROC glGetClipPlanex;
        internal static PFNGLGETFIXEDVPROC glGetFixedv;
        internal static PFNGLGETLIGHTXVPROC glGetLightxv;
        internal static PFNGLGETMATERIALXVPROC glGetMaterialxv;
        internal static PFNGLGETTEXENVIVPROC glGetTexEnviv;
        internal static PFNGLGETTEXENVXVPROC glGetTexEnvxv;
        internal static PFNGLGETTEXPARAMETERXVPROC glGetTexParameterxv;
        internal static PFNGLLIGHTMODELXPROC glLightModelx;
        internal static PFNGLLIGHTMODELXVPROC glLightModelxv;
        internal static PFNGLLIGHTXPROC glLightx;
        internal static PFNGLLIGHTXVPROC glLightxv;
        internal static PFNGLLINEWIDTHXPROC glLineWidthx;
        internal static PFNGLLOADIDENTITYPROC glLoadIdentity;
        internal static PFNGLLOADMATRIXXPROC glLoadMatrixx;
        internal static PFNGLMATERIALXPROC glMaterialx;
        internal static PFNGLMATERIALXVPROC glMaterialxv;
        internal static PFNGLMATRIXMODEPROC glMatrixMode;
        internal static PFNGLMULTMATRIXXPROC glMultMatrixx;
        internal static PFNGLMULTITEXCOORD4XPROC glMultiTexCoord4x;
        internal static PFNGLNORMAL3XPROC glNormal3x;
        internal static PFNGLNORMALPOINTERPROC glNormalPointer;
        internal static PFNGLORTHOXPROC glOrthox;
        internal static PFNGLPOINTPARAMETERXPROC glPointParameterx;
        internal static PFNGLPOINTPARAMETERXVPROC glPointParameterxv;
        internal static PFNGLPOINTSIZEXPROC glPointSizex;
        internal static PFNGLPOLYGONOFFSETXPROC glPolygonOffsetx;
        internal static PFNGLPOPMATRIXPROC glPopMatrix;
        internal static PFNGLPUSHMATRIXPROC glPushMatrix;
        internal static PFNGLROTATEXPROC glRotatex;
        internal static PFNGLSAMPLECOVERAGEXPROC glSampleCoveragex;
        internal static PFNGLSCALEXPROC glScalex;
        internal static PFNGLSHADEMODELPROC glShadeModel;
        internal static PFNGLTEXCOORDPOINTERPROC glTexCoordPointer;
        internal static PFNGLTEXENVIPROC glTexEnvi;
        internal static PFNGLTEXENVXPROC glTexEnvx;
        internal static PFNGLTEXENVIVPROC glTexEnviv;
        internal static PFNGLTEXENVXVPROC glTexEnvxv;
        internal static PFNGLTEXPARAMETERXPROC glTexParameterx;
        internal static PFNGLTEXPARAMETERXVPROC glTexParameterxv;
        internal static PFNGLTRANSLATEXPROC glTranslatex;
        internal static PFNGLVERTEXPOINTERPROC glVertexPointer;
        internal static PFNGLBLENDBARRIERPROC glBlendBarrier;
        internal static PFNGLPRIMITIVEBOUNDINGBOXPROC glPrimitiveBoundingBox;

        #endregion

        #endregion

        public static void LoadGLLoader(glLoader load)
        {

            glGetString = (PFNGLGETSTRINGPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetString)), typeof(PFNGLGETSTRINGPROC));

            if (glGetString == null)
            {
                throw new Exception();
            }

            LoadGL10(load);
            LoadGL11(load);
            LoadGL12(load);
            LoadGL13(load);
            LoadGL14(load);
            LoadGL15(load);
            LoadGL20(load);
            LoadGL21(load);
            LoadGL30(load);
            LoadGL31(load);
            LoadGL32(load);
            LoadGL33(load);
            LoadGL40(load);
            LoadGL41(load);
            LoadGL42(load);
            LoadGL43(load);
            LoadGL44(load);
            LoadGL45(load);
            LoadGL46(load);
        }

        internal static void LoadGL10(glLoader load)
        {
            glCullFace = (PFNGLCULLFACEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCullFace)), typeof(PFNGLCULLFACEPROC));
            glFrontFace = (PFNGLFRONTFACEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFrontFace)), typeof(PFNGLFRONTFACEPROC));
            glHint = (PFNGLHINTPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glHint)), typeof(PFNGLHINTPROC));
            glLineWidth = (PFNGLLINEWIDTHPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glLineWidth)), typeof(PFNGLLINEWIDTHPROC));
            glPointSize = (PFNGLPOINTSIZEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPointSize)), typeof(PFNGLPOINTSIZEPROC));
            glPolygonMode = (PFNGLPOLYGONMODEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPolygonMode)), typeof(PFNGLPOLYGONMODEPROC));
            glScissor = (PFNGLSCISSORPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glScissor)), typeof(PFNGLSCISSORPROC));
            glTexParameterf = (PFNGLTEXPARAMETERFPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexParameterf)), typeof(PFNGLTEXPARAMETERFPROC));
            glTexParameterfv = (PFNGLTEXPARAMETERFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexParameterfv)), typeof(PFNGLTEXPARAMETERFVPROC));
            glTexParameteri = (PFNGLTEXPARAMETERIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexParameteri)), typeof(PFNGLTEXPARAMETERIPROC));
            glTexParameteriv = (PFNGLTEXPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexParameteriv)), typeof(PFNGLTEXPARAMETERIVPROC));
            glTexImage1D = (PFNGLTEXIMAGE1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexImage1D)), typeof(PFNGLTEXIMAGE1DPROC));
            glTexImage2D = (PFNGLTEXIMAGE2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexImage2D)), typeof(PFNGLTEXIMAGE2DPROC));
            glDrawBuffer = (PFNGLDRAWBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawBuffer)), typeof(PFNGLDRAWBUFFERPROC));
            glClear = (PFNGLCLEARPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClear)), typeof(PFNGLCLEARPROC));
            glClearColor = (PFNGLCLEARCOLORPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearColor)), typeof(PFNGLCLEARCOLORPROC));
            glClearStencil = (PFNGLCLEARSTENCILPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearStencil)), typeof(PFNGLCLEARSTENCILPROC));
            glClearDepth = (PFNGLCLEARDEPTHPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearDepth)), typeof(PFNGLCLEARDEPTHPROC));
            glStencilMask = (PFNGLSTENCILMASKPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glStencilMask)), typeof(PFNGLSTENCILMASKPROC));
            glColorMask = (PFNGLCOLORMASKPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glColorMask)), typeof(PFNGLCOLORMASKPROC));
            glDepthMask = (PFNGLDEPTHMASKPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDepthMask)), typeof(PFNGLDEPTHMASKPROC));
            glDisable = (PFNGLDISABLEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDisable)), typeof(PFNGLDISABLEPROC));
            glEnable = (PFNGLENABLEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glEnable)), typeof(PFNGLENABLEPROC));
            glFinish = (PFNGLFINISHPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFinish)), typeof(PFNGLFINISHPROC));
            glFlush = (PFNGLFLUSHPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFlush)), typeof(PFNGLFLUSHPROC));
            glBlendFunc = (PFNGLBLENDFUNCPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBlendFunc)), typeof(PFNGLBLENDFUNCPROC));
            glLogicOp = (PFNGLLOGICOPPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glLogicOp)), typeof(PFNGLLOGICOPPROC));
            glStencilFunc = (PFNGLSTENCILFUNCPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glStencilFunc)), typeof(PFNGLSTENCILFUNCPROC));
            glStencilOp = (PFNGLSTENCILOPPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glStencilOp)), typeof(PFNGLSTENCILOPPROC));
            glDepthFunc = (PFNGLDEPTHFUNCPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDepthFunc)), typeof(PFNGLDEPTHFUNCPROC));
            glPixelStoref = (PFNGLPIXELSTOREFPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPixelStoref)), typeof(PFNGLPIXELSTOREFPROC));
            glPixelStorei = (PFNGLPIXELSTOREIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPixelStorei)), typeof(PFNGLPIXELSTOREIPROC));
            glReadBuffer = (PFNGLREADBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glReadBuffer)), typeof(PFNGLREADBUFFERPROC));
            glReadPixels = (PFNGLREADPIXELSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glReadPixels)), typeof(PFNGLREADPIXELSPROC));
            glGetBooleanv = (PFNGLGETBOOLEANVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetBooleanv)), typeof(PFNGLGETBOOLEANVPROC));
            glGetDoublev = (PFNGLGETDOUBLEVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetDoublev)), typeof(PFNGLGETDOUBLEVPROC));
            glGetError = (PFNGLGETERRORPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetError)), typeof(PFNGLGETERRORPROC));
            glGetFloatv = (PFNGLGETFLOATVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetFloatv)), typeof(PFNGLGETFLOATVPROC));
            glGetIntegerv = (PFNGLGETINTEGERVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetIntegerv)), typeof(PFNGLGETINTEGERVPROC));
            glGetString = (PFNGLGETSTRINGPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetString)), typeof(PFNGLGETSTRINGPROC));
            glGetTexImage = (PFNGLGETTEXIMAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTexImage)), typeof(PFNGLGETTEXIMAGEPROC));
            glGetTexParameterfv = (PFNGLGETTEXPARAMETERFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTexParameterfv)), typeof(PFNGLGETTEXPARAMETERFVPROC));
            glGetTexParameteriv = (PFNGLGETTEXPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTexParameteriv)), typeof(PFNGLGETTEXPARAMETERIVPROC));
            glGetTexLevelParameterfv = (PFNGLGETTEXLEVELPARAMETERFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTexLevelParameterfv)), typeof(PFNGLGETTEXLEVELPARAMETERFVPROC));
            glGetTexLevelParameteriv = (PFNGLGETTEXLEVELPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTexLevelParameteriv)), typeof(PFNGLGETTEXLEVELPARAMETERIVPROC));
            glIsEnabled = (PFNGLISENABLEDPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsEnabled)), typeof(PFNGLISENABLEDPROC));
            glDepthRange = (PFNGLDEPTHRANGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDepthRange)), typeof(PFNGLDEPTHRANGEPROC));
            glViewport = (PFNGLVIEWPORTPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glViewport)), typeof(PFNGLVIEWPORTPROC));

        }

        internal static void LoadGL11(glLoader load)
        {
            glDrawArrays = (PFNGLDRAWARRAYSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawArrays)), typeof(PFNGLDRAWARRAYSPROC));
            glDrawElements = (PFNGLDRAWELEMENTSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawElements)), typeof(PFNGLDRAWELEMENTSPROC));
            glPolygonOffset = (PFNGLPOLYGONOFFSETPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPolygonOffset)), typeof(PFNGLPOLYGONOFFSETPROC));
            glCopyTexImage1D = (PFNGLCOPYTEXIMAGE1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCopyTexImage1D)), typeof(PFNGLCOPYTEXIMAGE1DPROC));
            glCopyTexImage2D = (PFNGLCOPYTEXIMAGE2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCopyTexImage2D)), typeof(PFNGLCOPYTEXIMAGE2DPROC));
            glCopyTexSubImage1D = (PFNGLCOPYTEXSUBIMAGE1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCopyTexSubImage1D)), typeof(PFNGLCOPYTEXSUBIMAGE1DPROC));
            glCopyTexSubImage2D = (PFNGLCOPYTEXSUBIMAGE2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCopyTexSubImage2D)), typeof(PFNGLCOPYTEXSUBIMAGE2DPROC));
            glTexSubImage1D = (PFNGLTEXSUBIMAGE1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexSubImage1D)), typeof(PFNGLTEXSUBIMAGE1DPROC));
            glTexSubImage2D = (PFNGLTEXSUBIMAGE2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexSubImage2D)), typeof(PFNGLTEXSUBIMAGE2DPROC));
            glBindTexture = (PFNGLBINDTEXTUREPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindTexture)), typeof(PFNGLBINDTEXTUREPROC));
            glDeleteTextures = (PFNGLDELETETEXTURESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDeleteTextures)), typeof(PFNGLDELETETEXTURESPROC));
            glGenTextures = (PFNGLGENTEXTURESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGenTextures)), typeof(PFNGLGENTEXTURESPROC));
            glIsTexture = (PFNGLISTEXTUREPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsTexture)), typeof(PFNGLISTEXTUREPROC));

        }

        internal static void LoadGL12(glLoader load)
        {
            glDrawRangeElements = (PFNGLDRAWRANGEELEMENTSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawRangeElements)), typeof(PFNGLDRAWRANGEELEMENTSPROC));
            glTexImage3D = (PFNGLTEXIMAGE3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexImage3D)), typeof(PFNGLTEXIMAGE3DPROC));
            glTexSubImage3D = (PFNGLTEXSUBIMAGE3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexSubImage3D)), typeof(PFNGLTEXSUBIMAGE3DPROC));
            glCopyTexSubImage3D = (PFNGLCOPYTEXSUBIMAGE3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCopyTexSubImage3D)), typeof(PFNGLCOPYTEXSUBIMAGE3DPROC));

        }

        internal static void LoadGL13(glLoader load)
        {
            glActiveTexture = (PFNGLACTIVETEXTUREPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glActiveTexture)), typeof(PFNGLACTIVETEXTUREPROC));
            glSampleCoverage = (PFNGLSAMPLECOVERAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glSampleCoverage)), typeof(PFNGLSAMPLECOVERAGEPROC));
            glCompressedTexImage3D = (PFNGLCOMPRESSEDTEXIMAGE3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCompressedTexImage3D)), typeof(PFNGLCOMPRESSEDTEXIMAGE3DPROC));
            glCompressedTexImage2D = (PFNGLCOMPRESSEDTEXIMAGE2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCompressedTexImage2D)), typeof(PFNGLCOMPRESSEDTEXIMAGE2DPROC));
            glCompressedTexImage1D = (PFNGLCOMPRESSEDTEXIMAGE1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCompressedTexImage1D)), typeof(PFNGLCOMPRESSEDTEXIMAGE1DPROC));
            glCompressedTexSubImage3D = (PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCompressedTexSubImage3D)), typeof(PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC));
            glCompressedTexSubImage2D = (PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCompressedTexSubImage2D)), typeof(PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC));
            glCompressedTexSubImage1D = (PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCompressedTexSubImage1D)), typeof(PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC));
            glGetCompressedTexImage = (PFNGLGETCOMPRESSEDTEXIMAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetCompressedTexImage)), typeof(PFNGLGETCOMPRESSEDTEXIMAGEPROC));

        }

        internal static void LoadGL14(glLoader load)
        {
            glBlendFuncSeparate = (PFNGLBLENDFUNCSEPARATEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBlendFuncSeparate)), typeof(PFNGLBLENDFUNCSEPARATEPROC));
            glMultiDrawArrays = (PFNGLMULTIDRAWARRAYSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiDrawArrays)), typeof(PFNGLMULTIDRAWARRAYSPROC));
            glMultiDrawElements = (PFNGLMULTIDRAWELEMENTSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiDrawElements)), typeof(PFNGLMULTIDRAWELEMENTSPROC));
            glPointParameterf = (PFNGLPOINTPARAMETERFPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPointParameterf)), typeof(PFNGLPOINTPARAMETERFPROC));
            glPointParameterfv = (PFNGLPOINTPARAMETERFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPointParameterfv)), typeof(PFNGLPOINTPARAMETERFVPROC));
            glPointParameteri = (PFNGLPOINTPARAMETERIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPointParameteri)), typeof(PFNGLPOINTPARAMETERIPROC));
            glPointParameteriv = (PFNGLPOINTPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPointParameteriv)), typeof(PFNGLPOINTPARAMETERIVPROC));
            glBlendColor = (PFNGLBLENDCOLORPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBlendColor)), typeof(PFNGLBLENDCOLORPROC));
            glBlendEquation = (PFNGLBLENDEQUATIONPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBlendEquation)), typeof(PFNGLBLENDEQUATIONPROC));

        }

        internal static void LoadGL15(glLoader load)
        {
            glGenQueries = (PFNGLGENQUERIESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGenQueries)), typeof(PFNGLGENQUERIESPROC));
            glDeleteQueries = (PFNGLDELETEQUERIESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDeleteQueries)), typeof(PFNGLDELETEQUERIESPROC));
            glIsQuery = (PFNGLISQUERYPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsQuery)), typeof(PFNGLISQUERYPROC));
            glBeginQuery = (PFNGLBEGINQUERYPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBeginQuery)), typeof(PFNGLBEGINQUERYPROC));
            glEndQuery = (PFNGLENDQUERYPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glEndQuery)), typeof(PFNGLENDQUERYPROC));
            glGetQueryiv = (PFNGLGETQUERYIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetQueryiv)), typeof(PFNGLGETQUERYIVPROC));
            glGetQueryObjectiv = (PFNGLGETQUERYOBJECTIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetQueryObjectiv)), typeof(PFNGLGETQUERYOBJECTIVPROC));
            glGetQueryObjectuiv = (PFNGLGETQUERYOBJECTUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetQueryObjectuiv)), typeof(PFNGLGETQUERYOBJECTUIVPROC));
            glBindBuffer = (PFNGLBINDBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindBuffer)), typeof(PFNGLBINDBUFFERPROC));
            glDeleteBuffers = (PFNGLDELETEBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDeleteBuffers)), typeof(PFNGLDELETEBUFFERSPROC));
            glGenBuffers = (PFNGLGENBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGenBuffers)), typeof(PFNGLGENBUFFERSPROC));
            glIsBuffer = (PFNGLISBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsBuffer)), typeof(PFNGLISBUFFERPROC));
            glBufferData = (PFNGLBUFFERDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBufferData)), typeof(PFNGLBUFFERDATAPROC));
            glBufferSubData = (PFNGLBUFFERSUBDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBufferSubData)), typeof(PFNGLBUFFERSUBDATAPROC));
            glGetBufferSubData = (PFNGLGETBUFFERSUBDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetBufferSubData)), typeof(PFNGLGETBUFFERSUBDATAPROC));
            glMapBuffer = (PFNGLMAPBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMapBuffer)), typeof(PFNGLMAPBUFFERPROC));
            glUnmapBuffer = (PFNGLUNMAPBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUnmapBuffer)), typeof(PFNGLUNMAPBUFFERPROC));
            glGetBufferParameteriv = (PFNGLGETBUFFERPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetBufferParameteriv)), typeof(PFNGLGETBUFFERPARAMETERIVPROC));
            glGetBufferPointerv = (PFNGLGETBUFFERPOINTERVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetBufferPointerv)), typeof(PFNGLGETBUFFERPOINTERVPROC));

        }

        internal static void LoadGL20(glLoader load)
        {
            glBlendEquationSeparate = (PFNGLBLENDEQUATIONSEPARATEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBlendEquationSeparate)), typeof(PFNGLBLENDEQUATIONSEPARATEPROC));
            glDrawBuffers = (PFNGLDRAWBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawBuffers)), typeof(PFNGLDRAWBUFFERSPROC));
            glStencilOpSeparate = (PFNGLSTENCILOPSEPARATEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glStencilOpSeparate)), typeof(PFNGLSTENCILOPSEPARATEPROC));
            glStencilFuncSeparate = (PFNGLSTENCILFUNCSEPARATEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glStencilFuncSeparate)), typeof(PFNGLSTENCILFUNCSEPARATEPROC));
            glStencilMaskSeparate = (PFNGLSTENCILMASKSEPARATEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glStencilMaskSeparate)), typeof(PFNGLSTENCILMASKSEPARATEPROC));
            glAttachShader = (PFNGLATTACHSHADERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glAttachShader)), typeof(PFNGLATTACHSHADERPROC));
            glBindAttribLocation = (PFNGLBINDATTRIBLOCATIONPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindAttribLocation)), typeof(PFNGLBINDATTRIBLOCATIONPROC));
            glCompileShader = (PFNGLCOMPILESHADERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCompileShader)), typeof(PFNGLCOMPILESHADERPROC));
            glCreateProgram = (PFNGLCREATEPROGRAMPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCreateProgram)), typeof(PFNGLCREATEPROGRAMPROC));
            glCreateShader = (PFNGLCREATESHADERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCreateShader)), typeof(PFNGLCREATESHADERPROC));
            glDeleteProgram = (PFNGLDELETEPROGRAMPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDeleteProgram)), typeof(PFNGLDELETEPROGRAMPROC));
            glDeleteShader = (PFNGLDELETESHADERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDeleteShader)), typeof(PFNGLDELETESHADERPROC));
            glDetachShader = (PFNGLDETACHSHADERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDetachShader)), typeof(PFNGLDETACHSHADERPROC));
            glDisableVertexAttribArray = (PFNGLDISABLEVERTEXATTRIBARRAYPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDisableVertexAttribArray)), typeof(PFNGLDISABLEVERTEXATTRIBARRAYPROC));
            glEnableVertexAttribArray = (PFNGLENABLEVERTEXATTRIBARRAYPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glEnableVertexAttribArray)), typeof(PFNGLENABLEVERTEXATTRIBARRAYPROC));
            glGetActiveAttrib = (PFNGLGETACTIVEATTRIBPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetActiveAttrib)), typeof(PFNGLGETACTIVEATTRIBPROC));
            glGetActiveUniform = (PFNGLGETACTIVEUNIFORMPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetActiveUniform)), typeof(PFNGLGETACTIVEUNIFORMPROC));
            glGetAttachedShaders = (PFNGLGETATTACHEDSHADERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetAttachedShaders)), typeof(PFNGLGETATTACHEDSHADERSPROC));
            glGetAttribLocation = (PFNGLGETATTRIBLOCATIONPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetAttribLocation)), typeof(PFNGLGETATTRIBLOCATIONPROC));
            glGetProgramiv = (PFNGLGETPROGRAMIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetProgramiv)), typeof(PFNGLGETPROGRAMIVPROC));
            glGetProgramInfoLog = (PFNGLGETPROGRAMINFOLOGPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetProgramInfoLog)), typeof(PFNGLGETPROGRAMINFOLOGPROC));
            glGetShaderiv = (PFNGLGETSHADERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetShaderiv)), typeof(PFNGLGETSHADERIVPROC));
            glGetShaderInfoLog = (PFNGLGETSHADERINFOLOGPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetShaderInfoLog)), typeof(PFNGLGETSHADERINFOLOGPROC));
            glGetShaderSource = (PFNGLGETSHADERSOURCEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetShaderSource)), typeof(PFNGLGETSHADERSOURCEPROC));
            glGetUniformLocation = (PFNGLGETUNIFORMLOCATIONPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetUniformLocation)), typeof(PFNGLGETUNIFORMLOCATIONPROC));
            glGetUniformfv = (PFNGLGETUNIFORMFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetUniformfv)), typeof(PFNGLGETUNIFORMFVPROC));
            glGetUniformiv = (PFNGLGETUNIFORMIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetUniformiv)), typeof(PFNGLGETUNIFORMIVPROC));
            glGetVertexAttribdv = (PFNGLGETVERTEXATTRIBDVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetVertexAttribdv)), typeof(PFNGLGETVERTEXATTRIBDVPROC));
            glGetVertexAttribfv = (PFNGLGETVERTEXATTRIBFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetVertexAttribfv)), typeof(PFNGLGETVERTEXATTRIBFVPROC));
            glGetVertexAttribiv = (PFNGLGETVERTEXATTRIBIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetVertexAttribiv)), typeof(PFNGLGETVERTEXATTRIBIVPROC));
            glGetVertexAttribPointerv = (PFNGLGETVERTEXATTRIBPOINTERVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetVertexAttribPointerv)), typeof(PFNGLGETVERTEXATTRIBPOINTERVPROC));
            glIsProgram = (PFNGLISPROGRAMPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsProgram)), typeof(PFNGLISPROGRAMPROC));
            glIsShader = (PFNGLISSHADERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsShader)), typeof(PFNGLISSHADERPROC));
            glLinkProgram = (PFNGLLINKPROGRAMPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glLinkProgram)), typeof(PFNGLLINKPROGRAMPROC));
            glShaderSource = (PFNGLSHADERSOURCEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glShaderSource)), typeof(PFNGLSHADERSOURCEPROC));
            glUseProgram = (PFNGLUSEPROGRAMPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUseProgram)), typeof(PFNGLUSEPROGRAMPROC));
            glUniform1f = (PFNGLUNIFORM1FPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform1f)), typeof(PFNGLUNIFORM1FPROC));
            glUniform2f = (PFNGLUNIFORM2FPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform2f)), typeof(PFNGLUNIFORM2FPROC));
            glUniform3f = (PFNGLUNIFORM3FPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform3f)), typeof(PFNGLUNIFORM3FPROC));
            glUniform4f = (PFNGLUNIFORM4FPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform4f)), typeof(PFNGLUNIFORM4FPROC));
            glUniform1i = (PFNGLUNIFORM1IPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform1i)), typeof(PFNGLUNIFORM1IPROC));
            glUniform2i = (PFNGLUNIFORM2IPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform2i)), typeof(PFNGLUNIFORM2IPROC));
            glUniform3i = (PFNGLUNIFORM3IPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform3i)), typeof(PFNGLUNIFORM3IPROC));
            glUniform4i = (PFNGLUNIFORM4IPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform4i)), typeof(PFNGLUNIFORM4IPROC));
            glUniform1fv = (PFNGLUNIFORM1FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform1fv)), typeof(PFNGLUNIFORM1FVPROC));
            glUniform2fv = (PFNGLUNIFORM2FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform2fv)), typeof(PFNGLUNIFORM2FVPROC));
            glUniform3fv = (PFNGLUNIFORM3FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform3fv)), typeof(PFNGLUNIFORM3FVPROC));
            glUniform4fv = (PFNGLUNIFORM4FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform4fv)), typeof(PFNGLUNIFORM4FVPROC));
            glUniform1iv = (PFNGLUNIFORM1IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform1iv)), typeof(PFNGLUNIFORM1IVPROC));
            glUniform2iv = (PFNGLUNIFORM2IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform2iv)), typeof(PFNGLUNIFORM2IVPROC));
            glUniform3iv = (PFNGLUNIFORM3IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform3iv)), typeof(PFNGLUNIFORM3IVPROC));
            glUniform4iv = (PFNGLUNIFORM4IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform4iv)), typeof(PFNGLUNIFORM4IVPROC));
            glUniformMatrix2fv = (PFNGLUNIFORMMATRIX2FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix2fv)), typeof(PFNGLUNIFORMMATRIX2FVPROC));
            glUniformMatrix3fv = (PFNGLUNIFORMMATRIX3FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix3fv)), typeof(PFNGLUNIFORMMATRIX3FVPROC));
            glUniformMatrix4fv = (PFNGLUNIFORMMATRIX4FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix4fv)), typeof(PFNGLUNIFORMMATRIX4FVPROC));
            glValidateProgram = (PFNGLVALIDATEPROGRAMPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glValidateProgram)), typeof(PFNGLVALIDATEPROGRAMPROC));
            glVertexAttrib1d = (PFNGLVERTEXATTRIB1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib1d)), typeof(PFNGLVERTEXATTRIB1DPROC));
            glVertexAttrib1dv = (PFNGLVERTEXATTRIB1DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib1dv)), typeof(PFNGLVERTEXATTRIB1DVPROC));
            glVertexAttrib1f = (PFNGLVERTEXATTRIB1FPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib1f)), typeof(PFNGLVERTEXATTRIB1FPROC));
            glVertexAttrib1fv = (PFNGLVERTEXATTRIB1FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib1fv)), typeof(PFNGLVERTEXATTRIB1FVPROC));
            glVertexAttrib1s = (PFNGLVERTEXATTRIB1SPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib1s)), typeof(PFNGLVERTEXATTRIB1SPROC));
            glVertexAttrib1sv = (PFNGLVERTEXATTRIB1SVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib1sv)), typeof(PFNGLVERTEXATTRIB1SVPROC));
            glVertexAttrib2d = (PFNGLVERTEXATTRIB2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib2d)), typeof(PFNGLVERTEXATTRIB2DPROC));
            glVertexAttrib2dv = (PFNGLVERTEXATTRIB2DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib2dv)), typeof(PFNGLVERTEXATTRIB2DVPROC));
            glVertexAttrib2f = (PFNGLVERTEXATTRIB2FPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib2f)), typeof(PFNGLVERTEXATTRIB2FPROC));
            glVertexAttrib2fv = (PFNGLVERTEXATTRIB2FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib2fv)), typeof(PFNGLVERTEXATTRIB2FVPROC));
            glVertexAttrib2s = (PFNGLVERTEXATTRIB2SPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib2s)), typeof(PFNGLVERTEXATTRIB2SPROC));
            glVertexAttrib2sv = (PFNGLVERTEXATTRIB2SVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib2sv)), typeof(PFNGLVERTEXATTRIB2SVPROC));
            glVertexAttrib3d = (PFNGLVERTEXATTRIB3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib3d)), typeof(PFNGLVERTEXATTRIB3DPROC));
            glVertexAttrib3dv = (PFNGLVERTEXATTRIB3DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib3dv)), typeof(PFNGLVERTEXATTRIB3DVPROC));
            glVertexAttrib3f = (PFNGLVERTEXATTRIB3FPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib3f)), typeof(PFNGLVERTEXATTRIB3FPROC));
            glVertexAttrib3fv = (PFNGLVERTEXATTRIB3FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib3fv)), typeof(PFNGLVERTEXATTRIB3FVPROC));
            glVertexAttrib3s = (PFNGLVERTEXATTRIB3SPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib3s)), typeof(PFNGLVERTEXATTRIB3SPROC));
            glVertexAttrib3sv = (PFNGLVERTEXATTRIB3SVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib3sv)), typeof(PFNGLVERTEXATTRIB3SVPROC));
            glVertexAttrib4Nbv = (PFNGLVERTEXATTRIB4NBVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4Nbv)), typeof(PFNGLVERTEXATTRIB4NBVPROC));
            glVertexAttrib4Niv = (PFNGLVERTEXATTRIB4NIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4Niv)), typeof(PFNGLVERTEXATTRIB4NIVPROC));
            glVertexAttrib4Nsv = (PFNGLVERTEXATTRIB4NSVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4Nsv)), typeof(PFNGLVERTEXATTRIB4NSVPROC));
            glVertexAttrib4Nub = (PFNGLVERTEXATTRIB4NUBPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4Nub)), typeof(PFNGLVERTEXATTRIB4NUBPROC));
            glVertexAttrib4Nubv = (PFNGLVERTEXATTRIB4NUBVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4Nubv)), typeof(PFNGLVERTEXATTRIB4NUBVPROC));
            glVertexAttrib4Nuiv = (PFNGLVERTEXATTRIB4NUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4Nuiv)), typeof(PFNGLVERTEXATTRIB4NUIVPROC));
            glVertexAttrib4Nusv = (PFNGLVERTEXATTRIB4NUSVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4Nusv)), typeof(PFNGLVERTEXATTRIB4NUSVPROC));
            glVertexAttrib4bv = (PFNGLVERTEXATTRIB4BVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4bv)), typeof(PFNGLVERTEXATTRIB4BVPROC));
            glVertexAttrib4d = (PFNGLVERTEXATTRIB4DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4d)), typeof(PFNGLVERTEXATTRIB4DPROC));
            glVertexAttrib4dv = (PFNGLVERTEXATTRIB4DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4dv)), typeof(PFNGLVERTEXATTRIB4DVPROC));
            glVertexAttrib4f = (PFNGLVERTEXATTRIB4FPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4f)), typeof(PFNGLVERTEXATTRIB4FPROC));
            glVertexAttrib4fv = (PFNGLVERTEXATTRIB4FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4fv)), typeof(PFNGLVERTEXATTRIB4FVPROC));
            glVertexAttrib4iv = (PFNGLVERTEXATTRIB4IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4iv)), typeof(PFNGLVERTEXATTRIB4IVPROC));
            glVertexAttrib4s = (PFNGLVERTEXATTRIB4SPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4s)), typeof(PFNGLVERTEXATTRIB4SPROC));
            glVertexAttrib4sv = (PFNGLVERTEXATTRIB4SVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4sv)), typeof(PFNGLVERTEXATTRIB4SVPROC));
            glVertexAttrib4ubv = (PFNGLVERTEXATTRIB4UBVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4ubv)), typeof(PFNGLVERTEXATTRIB4UBVPROC));
            glVertexAttrib4uiv = (PFNGLVERTEXATTRIB4UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4uiv)), typeof(PFNGLVERTEXATTRIB4UIVPROC));
            glVertexAttrib4usv = (PFNGLVERTEXATTRIB4USVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttrib4usv)), typeof(PFNGLVERTEXATTRIB4USVPROC));
            glVertexAttribPointer = (PFNGLVERTEXATTRIBPOINTERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribPointer)), typeof(PFNGLVERTEXATTRIBPOINTERPROC));

        }

        internal static void LoadGL21(glLoader load)
        {
            glUniformMatrix2x3fv = (PFNGLUNIFORMMATRIX2X3FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix2x3fv)), typeof(PFNGLUNIFORMMATRIX2X3FVPROC));
            glUniformMatrix3x2fv = (PFNGLUNIFORMMATRIX3X2FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix3x2fv)), typeof(PFNGLUNIFORMMATRIX3X2FVPROC));
            glUniformMatrix2x4fv = (PFNGLUNIFORMMATRIX2X4FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix2x4fv)), typeof(PFNGLUNIFORMMATRIX2X4FVPROC));
            glUniformMatrix4x2fv = (PFNGLUNIFORMMATRIX4X2FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix4x2fv)), typeof(PFNGLUNIFORMMATRIX4X2FVPROC));
            glUniformMatrix3x4fv = (PFNGLUNIFORMMATRIX3X4FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix3x4fv)), typeof(PFNGLUNIFORMMATRIX3X4FVPROC));
            glUniformMatrix4x3fv = (PFNGLUNIFORMMATRIX4X3FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix4x3fv)), typeof(PFNGLUNIFORMMATRIX4X3FVPROC));

        }

        internal static void LoadGL30(glLoader load)
        {
            glColorMaski = (PFNGLCOLORMASKIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glColorMaski)), typeof(PFNGLCOLORMASKIPROC));
            glGetBooleani_v = (PFNGLGETBOOLEANI_VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetBooleani_v)), typeof(PFNGLGETBOOLEANI_VPROC));
            glGetIntegeri_v = (PFNGLGETINTEGERI_VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetIntegeri_v)), typeof(PFNGLGETINTEGERI_VPROC));
            glEnablei = (PFNGLENABLEIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glEnablei)), typeof(PFNGLENABLEIPROC));
            glDisablei = (PFNGLDISABLEIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDisablei)), typeof(PFNGLDISABLEIPROC));
            glIsEnabledi = (PFNGLISENABLEDIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsEnabledi)), typeof(PFNGLISENABLEDIPROC));
            glBeginTransformFeedback = (PFNGLBEGINTRANSFORMFEEDBACKPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBeginTransformFeedback)), typeof(PFNGLBEGINTRANSFORMFEEDBACKPROC));
            glEndTransformFeedback = (PFNGLENDTRANSFORMFEEDBACKPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glEndTransformFeedback)), typeof(PFNGLENDTRANSFORMFEEDBACKPROC));
            glBindBufferRange = (PFNGLBINDBUFFERRANGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindBufferRange)), typeof(PFNGLBINDBUFFERRANGEPROC));
            glBindBufferBase = (PFNGLBINDBUFFERBASEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindBufferBase)), typeof(PFNGLBINDBUFFERBASEPROC));
            glTransformFeedbackVaryings = (PFNGLTRANSFORMFEEDBACKVARYINGSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTransformFeedbackVaryings)), typeof(PFNGLTRANSFORMFEEDBACKVARYINGSPROC));
            glGetTransformFeedbackVarying = (PFNGLGETTRANSFORMFEEDBACKVARYINGPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTransformFeedbackVarying)), typeof(PFNGLGETTRANSFORMFEEDBACKVARYINGPROC));
            glClampColor = (PFNGLCLAMPCOLORPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClampColor)), typeof(PFNGLCLAMPCOLORPROC));
            glBeginConditionalRender = (PFNGLBEGINCONDITIONALRENDERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBeginConditionalRender)), typeof(PFNGLBEGINCONDITIONALRENDERPROC));
            glEndConditionalRender = (PFNGLENDCONDITIONALRENDERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glEndConditionalRender)), typeof(PFNGLENDCONDITIONALRENDERPROC));
            glVertexAttribIPointer = (PFNGLVERTEXATTRIBIPOINTERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribIPointer)), typeof(PFNGLVERTEXATTRIBIPOINTERPROC));
            glGetVertexAttribIiv = (PFNGLGETVERTEXATTRIBIIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetVertexAttribIiv)), typeof(PFNGLGETVERTEXATTRIBIIVPROC));
            glGetVertexAttribIuiv = (PFNGLGETVERTEXATTRIBIUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetVertexAttribIuiv)), typeof(PFNGLGETVERTEXATTRIBIUIVPROC));
            glVertexAttribI1i = (PFNGLVERTEXATTRIBI1IPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI1i)), typeof(PFNGLVERTEXATTRIBI1IPROC));
            glVertexAttribI2i = (PFNGLVERTEXATTRIBI2IPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI2i)), typeof(PFNGLVERTEXATTRIBI2IPROC));
            glVertexAttribI3i = (PFNGLVERTEXATTRIBI3IPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI3i)), typeof(PFNGLVERTEXATTRIBI3IPROC));
            glVertexAttribI4i = (PFNGLVERTEXATTRIBI4IPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI4i)), typeof(PFNGLVERTEXATTRIBI4IPROC));
            glVertexAttribI1ui = (PFNGLVERTEXATTRIBI1UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI1ui)), typeof(PFNGLVERTEXATTRIBI1UIPROC));
            glVertexAttribI2ui = (PFNGLVERTEXATTRIBI2UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI2ui)), typeof(PFNGLVERTEXATTRIBI2UIPROC));
            glVertexAttribI3ui = (PFNGLVERTEXATTRIBI3UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI3ui)), typeof(PFNGLVERTEXATTRIBI3UIPROC));
            glVertexAttribI4ui = (PFNGLVERTEXATTRIBI4UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI4ui)), typeof(PFNGLVERTEXATTRIBI4UIPROC));
            glVertexAttribI1iv = (PFNGLVERTEXATTRIBI1IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI1iv)), typeof(PFNGLVERTEXATTRIBI1IVPROC));
            glVertexAttribI2iv = (PFNGLVERTEXATTRIBI2IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI2iv)), typeof(PFNGLVERTEXATTRIBI2IVPROC));
            glVertexAttribI3iv = (PFNGLVERTEXATTRIBI3IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI3iv)), typeof(PFNGLVERTEXATTRIBI3IVPROC));
            glVertexAttribI4iv = (PFNGLVERTEXATTRIBI4IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI4iv)), typeof(PFNGLVERTEXATTRIBI4IVPROC));
            glVertexAttribI1uiv = (PFNGLVERTEXATTRIBI1UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI1uiv)), typeof(PFNGLVERTEXATTRIBI1UIVPROC));
            glVertexAttribI2uiv = (PFNGLVERTEXATTRIBI2UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI2uiv)), typeof(PFNGLVERTEXATTRIBI2UIVPROC));
            glVertexAttribI3uiv = (PFNGLVERTEXATTRIBI3UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI3uiv)), typeof(PFNGLVERTEXATTRIBI3UIVPROC));
            glVertexAttribI4uiv = (PFNGLVERTEXATTRIBI4UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI4uiv)), typeof(PFNGLVERTEXATTRIBI4UIVPROC));
            glVertexAttribI4bv = (PFNGLVERTEXATTRIBI4BVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI4bv)), typeof(PFNGLVERTEXATTRIBI4BVPROC));
            glVertexAttribI4sv = (PFNGLVERTEXATTRIBI4SVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI4sv)), typeof(PFNGLVERTEXATTRIBI4SVPROC));
            glVertexAttribI4ubv = (PFNGLVERTEXATTRIBI4UBVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI4ubv)), typeof(PFNGLVERTEXATTRIBI4UBVPROC));
            glVertexAttribI4usv = (PFNGLVERTEXATTRIBI4USVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribI4usv)), typeof(PFNGLVERTEXATTRIBI4USVPROC));
            glGetUniformuiv = (PFNGLGETUNIFORMUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetUniformuiv)), typeof(PFNGLGETUNIFORMUIVPROC));
            glBindFragDataLocation = (PFNGLBINDFRAGDATALOCATIONPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindFragDataLocation)), typeof(PFNGLBINDFRAGDATALOCATIONPROC));
            glGetFragDataLocation = (PFNGLGETFRAGDATALOCATIONPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetFragDataLocation)), typeof(PFNGLGETFRAGDATALOCATIONPROC));
            glUniform1ui = (PFNGLUNIFORM1UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform1ui)), typeof(PFNGLUNIFORM1UIPROC));
            glUniform2ui = (PFNGLUNIFORM2UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform2ui)), typeof(PFNGLUNIFORM2UIPROC));
            glUniform3ui = (PFNGLUNIFORM3UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform3ui)), typeof(PFNGLUNIFORM3UIPROC));
            glUniform4ui = (PFNGLUNIFORM4UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform4ui)), typeof(PFNGLUNIFORM4UIPROC));
            glUniform1uiv = (PFNGLUNIFORM1UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform1uiv)), typeof(PFNGLUNIFORM1UIVPROC));
            glUniform2uiv = (PFNGLUNIFORM2UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform2uiv)), typeof(PFNGLUNIFORM2UIVPROC));
            glUniform3uiv = (PFNGLUNIFORM3UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform3uiv)), typeof(PFNGLUNIFORM3UIVPROC));
            glUniform4uiv = (PFNGLUNIFORM4UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform4uiv)), typeof(PFNGLUNIFORM4UIVPROC));
            glTexParameterIiv = (PFNGLTEXPARAMETERIIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexParameterIiv)), typeof(PFNGLTEXPARAMETERIIVPROC));
            glTexParameterIuiv = (PFNGLTEXPARAMETERIUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexParameterIuiv)), typeof(PFNGLTEXPARAMETERIUIVPROC));
            glGetTexParameterIiv = (PFNGLGETTEXPARAMETERIIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTexParameterIiv)), typeof(PFNGLGETTEXPARAMETERIIVPROC));
            glGetTexParameterIuiv = (PFNGLGETTEXPARAMETERIUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTexParameterIuiv)), typeof(PFNGLGETTEXPARAMETERIUIVPROC));
            glClearBufferiv = (PFNGLCLEARBUFFERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearBufferiv)), typeof(PFNGLCLEARBUFFERIVPROC));
            glClearBufferuiv = (PFNGLCLEARBUFFERUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearBufferuiv)), typeof(PFNGLCLEARBUFFERUIVPROC));
            glClearBufferfv = (PFNGLCLEARBUFFERFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearBufferfv)), typeof(PFNGLCLEARBUFFERFVPROC));
            glClearBufferfi = (PFNGLCLEARBUFFERFIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearBufferfi)), typeof(PFNGLCLEARBUFFERFIPROC));
            glGetStringi = (PFNGLGETSTRINGIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetStringi)), typeof(PFNGLGETSTRINGIPROC));
            glIsRenderbuffer = (PFNGLISRENDERBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsRenderbuffer)), typeof(PFNGLISRENDERBUFFERPROC));
            glBindRenderbuffer = (PFNGLBINDRENDERBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindRenderbuffer)), typeof(PFNGLBINDRENDERBUFFERPROC));
            glDeleteRenderbuffers = (PFNGLDELETERENDERBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDeleteRenderbuffers)), typeof(PFNGLDELETERENDERBUFFERSPROC));
            glGenRenderbuffers = (PFNGLGENRENDERBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGenRenderbuffers)), typeof(PFNGLGENRENDERBUFFERSPROC));
            glRenderbufferStorage = (PFNGLRENDERBUFFERSTORAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glRenderbufferStorage)), typeof(PFNGLRENDERBUFFERSTORAGEPROC));
            glGetRenderbufferParameteriv = (PFNGLGETRENDERBUFFERPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetRenderbufferParameteriv)), typeof(PFNGLGETRENDERBUFFERPARAMETERIVPROC));
            glIsFramebuffer = (PFNGLISFRAMEBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsFramebuffer)), typeof(PFNGLISFRAMEBUFFERPROC));
            glBindFramebuffer = (PFNGLBINDFRAMEBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindFramebuffer)), typeof(PFNGLBINDFRAMEBUFFERPROC));
            glDeleteFramebuffers = (PFNGLDELETEFRAMEBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDeleteFramebuffers)), typeof(PFNGLDELETEFRAMEBUFFERSPROC));
            glGenFramebuffers = (PFNGLGENFRAMEBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGenFramebuffers)), typeof(PFNGLGENFRAMEBUFFERSPROC));
            glCheckFramebufferStatus = (PFNGLCHECKFRAMEBUFFERSTATUSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCheckFramebufferStatus)), typeof(PFNGLCHECKFRAMEBUFFERSTATUSPROC));
            glFramebufferTexture1D = (PFNGLFRAMEBUFFERTEXTURE1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFramebufferTexture1D)), typeof(PFNGLFRAMEBUFFERTEXTURE1DPROC));
            glFramebufferTexture2D = (PFNGLFRAMEBUFFERTEXTURE2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFramebufferTexture2D)), typeof(PFNGLFRAMEBUFFERTEXTURE2DPROC));
            glFramebufferTexture3D = (PFNGLFRAMEBUFFERTEXTURE3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFramebufferTexture3D)), typeof(PFNGLFRAMEBUFFERTEXTURE3DPROC));
            glFramebufferRenderbuffer = (PFNGLFRAMEBUFFERRENDERBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFramebufferRenderbuffer)), typeof(PFNGLFRAMEBUFFERRENDERBUFFERPROC));
            glGetFramebufferAttachmentParameteriv = (PFNGLGETFRAMEBUFFERATTACHMENTPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetFramebufferAttachmentParameteriv)), typeof(PFNGLGETFRAMEBUFFERATTACHMENTPARAMETERIVPROC));
            glGenerateMipmap = (PFNGLGENERATEMIPMAPPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGenerateMipmap)), typeof(PFNGLGENERATEMIPMAPPROC));
            glBlitFramebuffer = (PFNGLBLITFRAMEBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBlitFramebuffer)), typeof(PFNGLBLITFRAMEBUFFERPROC));
            glRenderbufferStorageMultisample = (PFNGLRENDERBUFFERSTORAGEMULTISAMPLEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glRenderbufferStorageMultisample)), typeof(PFNGLRENDERBUFFERSTORAGEMULTISAMPLEPROC));
            glFramebufferTextureLayer = (PFNGLFRAMEBUFFERTEXTURELAYERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFramebufferTextureLayer)), typeof(PFNGLFRAMEBUFFERTEXTURELAYERPROC));
            glMapBufferRange = (PFNGLMAPBUFFERRANGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMapBufferRange)), typeof(PFNGLMAPBUFFERRANGEPROC));
            glFlushMappedBufferRange = (PFNGLFLUSHMAPPEDBUFFERRANGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFlushMappedBufferRange)), typeof(PFNGLFLUSHMAPPEDBUFFERRANGEPROC));
            glBindVertexArray = (PFNGLBINDVERTEXARRAYPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindVertexArray)), typeof(PFNGLBINDVERTEXARRAYPROC));
            glDeleteVertexArrays = (PFNGLDELETEVERTEXARRAYSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDeleteVertexArrays)), typeof(PFNGLDELETEVERTEXARRAYSPROC));
            glGenVertexArrays = (PFNGLGENVERTEXARRAYSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGenVertexArrays)), typeof(PFNGLGENVERTEXARRAYSPROC));
            glIsVertexArray = (PFNGLISVERTEXARRAYPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsVertexArray)), typeof(PFNGLISVERTEXARRAYPROC));

        }

        internal static void LoadGL31(glLoader load)
        {
            glDrawArraysInstanced = (PFNGLDRAWARRAYSINSTANCEDPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawArraysInstanced)), typeof(PFNGLDRAWARRAYSINSTANCEDPROC));
            glDrawElementsInstanced = (PFNGLDRAWELEMENTSINSTANCEDPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawElementsInstanced)), typeof(PFNGLDRAWELEMENTSINSTANCEDPROC));
            glTexBuffer = (PFNGLTEXBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexBuffer)), typeof(PFNGLTEXBUFFERPROC));
            glPrimitiveRestartIndex = (PFNGLPRIMITIVERESTARTINDEXPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPrimitiveRestartIndex)), typeof(PFNGLPRIMITIVERESTARTINDEXPROC));
            glCopyBufferSubData = (PFNGLCOPYBUFFERSUBDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCopyBufferSubData)), typeof(PFNGLCOPYBUFFERSUBDATAPROC));
            glGetUniformIndices = (PFNGLGETUNIFORMINDICESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetUniformIndices)), typeof(PFNGLGETUNIFORMINDICESPROC));
            glGetActiveUniformsiv = (PFNGLGETACTIVEUNIFORMSIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetActiveUniformsiv)), typeof(PFNGLGETACTIVEUNIFORMSIVPROC));
            glGetActiveUniformName = (PFNGLGETACTIVEUNIFORMNAMEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetActiveUniformName)), typeof(PFNGLGETACTIVEUNIFORMNAMEPROC));
            glGetUniformBlockIndex = (PFNGLGETUNIFORMBLOCKINDEXPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetUniformBlockIndex)), typeof(PFNGLGETUNIFORMBLOCKINDEXPROC));
            glGetActiveUniformBlockiv = (PFNGLGETACTIVEUNIFORMBLOCKIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetActiveUniformBlockiv)), typeof(PFNGLGETACTIVEUNIFORMBLOCKIVPROC));
            glGetActiveUniformBlockName = (PFNGLGETACTIVEUNIFORMBLOCKNAMEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetActiveUniformBlockName)), typeof(PFNGLGETACTIVEUNIFORMBLOCKNAMEPROC));
            glUniformBlockBinding = (PFNGLUNIFORMBLOCKBINDINGPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformBlockBinding)), typeof(PFNGLUNIFORMBLOCKBINDINGPROC));

        }

        internal static void LoadGL32(glLoader load)
        {
            glDrawElementsBaseVertex = (PFNGLDRAWELEMENTSBASEVERTEXPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawElementsBaseVertex)), typeof(PFNGLDRAWELEMENTSBASEVERTEXPROC));
            glDrawRangeElementsBaseVertex = (PFNGLDRAWRANGEELEMENTSBASEVERTEXPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawRangeElementsBaseVertex)), typeof(PFNGLDRAWRANGEELEMENTSBASEVERTEXPROC));
            glDrawElementsInstancedBaseVertex = (PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawElementsInstancedBaseVertex)), typeof(PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXPROC));
            glMultiDrawElementsBaseVertex = (PFNGLMULTIDRAWELEMENTSBASEVERTEXPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiDrawElementsBaseVertex)), typeof(PFNGLMULTIDRAWELEMENTSBASEVERTEXPROC));
            glProvokingVertex = (PFNGLPROVOKINGVERTEXPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProvokingVertex)), typeof(PFNGLPROVOKINGVERTEXPROC));
            glFenceSync = (PFNGLFENCESYNCPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFenceSync)), typeof(PFNGLFENCESYNCPROC));
            glIsSync = (PFNGLISSYNCPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsSync)), typeof(PFNGLISSYNCPROC));
            glDeleteSync = (PFNGLDELETESYNCPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDeleteSync)), typeof(PFNGLDELETESYNCPROC));
            glClientWaitSync = (PFNGLCLIENTWAITSYNCPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClientWaitSync)), typeof(PFNGLCLIENTWAITSYNCPROC));
            glWaitSync = (PFNGLWAITSYNCPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glWaitSync)), typeof(PFNGLWAITSYNCPROC));
            glGetInteger64v = (PFNGLGETINTEGER64VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetInteger64v)), typeof(PFNGLGETINTEGER64VPROC));
            glGetSynciv = (PFNGLGETSYNCIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetSynciv)), typeof(PFNGLGETSYNCIVPROC));
            glGetInteger64i_v = (PFNGLGETINTEGER64I_VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetInteger64i_v)), typeof(PFNGLGETINTEGER64I_VPROC));
            glGetBufferParameteri64v = (PFNGLGETBUFFERPARAMETERI64VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetBufferParameteri64v)), typeof(PFNGLGETBUFFERPARAMETERI64VPROC));
            glFramebufferTexture = (PFNGLFRAMEBUFFERTEXTUREPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFramebufferTexture)), typeof(PFNGLFRAMEBUFFERTEXTUREPROC));
            glTexImage2DMultisample = (PFNGLTEXIMAGE2DMULTISAMPLEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexImage2DMultisample)), typeof(PFNGLTEXIMAGE2DMULTISAMPLEPROC));
            glTexImage3DMultisample = (PFNGLTEXIMAGE3DMULTISAMPLEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexImage3DMultisample)), typeof(PFNGLTEXIMAGE3DMULTISAMPLEPROC));
            glGetMultisamplefv = (PFNGLGETMULTISAMPLEFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetMultisamplefv)), typeof(PFNGLGETMULTISAMPLEFVPROC));
            glSampleMaski = (PFNGLSAMPLEMASKIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glSampleMaski)), typeof(PFNGLSAMPLEMASKIPROC));

        }

        internal static void LoadGL33(glLoader load)
        {
            glBindFragDataLocationIndexed = (PFNGLBINDFRAGDATALOCATIONINDEXEDPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindFragDataLocationIndexed)), typeof(PFNGLBINDFRAGDATALOCATIONINDEXEDPROC));
            glGetFragDataIndex = (PFNGLGETFRAGDATAINDEXPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetFragDataIndex)), typeof(PFNGLGETFRAGDATAINDEXPROC));
            glGenSamplers = (PFNGLGENSAMPLERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGenSamplers)), typeof(PFNGLGENSAMPLERSPROC));
            glDeleteSamplers = (PFNGLDELETESAMPLERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDeleteSamplers)), typeof(PFNGLDELETESAMPLERSPROC));
            glIsSampler = (PFNGLISSAMPLERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsSampler)), typeof(PFNGLISSAMPLERPROC));
            glBindSampler = (PFNGLBINDSAMPLERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindSampler)), typeof(PFNGLBINDSAMPLERPROC));
            glSamplerParameteri = (PFNGLSAMPLERPARAMETERIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glSamplerParameteri)), typeof(PFNGLSAMPLERPARAMETERIPROC));
            glSamplerParameteriv = (PFNGLSAMPLERPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glSamplerParameteriv)), typeof(PFNGLSAMPLERPARAMETERIVPROC));
            glSamplerParameterf = (PFNGLSAMPLERPARAMETERFPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glSamplerParameterf)), typeof(PFNGLSAMPLERPARAMETERFPROC));
            glSamplerParameterfv = (PFNGLSAMPLERPARAMETERFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glSamplerParameterfv)), typeof(PFNGLSAMPLERPARAMETERFVPROC));
            glSamplerParameterIiv = (PFNGLSAMPLERPARAMETERIIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glSamplerParameterIiv)), typeof(PFNGLSAMPLERPARAMETERIIVPROC));
            glSamplerParameterIuiv = (PFNGLSAMPLERPARAMETERIUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glSamplerParameterIuiv)), typeof(PFNGLSAMPLERPARAMETERIUIVPROC));
            glGetSamplerParameteriv = (PFNGLGETSAMPLERPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetSamplerParameteriv)), typeof(PFNGLGETSAMPLERPARAMETERIVPROC));
            glGetSamplerParameterIiv = (PFNGLGETSAMPLERPARAMETERIIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetSamplerParameterIiv)), typeof(PFNGLGETSAMPLERPARAMETERIIVPROC));
            glGetSamplerParameterfv = (PFNGLGETSAMPLERPARAMETERFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetSamplerParameterfv)), typeof(PFNGLGETSAMPLERPARAMETERFVPROC));
            glGetSamplerParameterIuiv = (PFNGLGETSAMPLERPARAMETERIUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetSamplerParameterIuiv)), typeof(PFNGLGETSAMPLERPARAMETERIUIVPROC));
            glQueryCounter = (PFNGLQUERYCOUNTERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glQueryCounter)), typeof(PFNGLQUERYCOUNTERPROC));
            glGetQueryObjecti64v = (PFNGLGETQUERYOBJECTI64VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetQueryObjecti64v)), typeof(PFNGLGETQUERYOBJECTI64VPROC));
            glGetQueryObjectui64v = (PFNGLGETQUERYOBJECTUI64VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetQueryObjectui64v)), typeof(PFNGLGETQUERYOBJECTUI64VPROC));
            glVertexAttribDivisor = (PFNGLVERTEXATTRIBDIVISORPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribDivisor)), typeof(PFNGLVERTEXATTRIBDIVISORPROC));
            glVertexAttribP1ui = (PFNGLVERTEXATTRIBP1UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribP1ui)), typeof(PFNGLVERTEXATTRIBP1UIPROC));
            glVertexAttribP1uiv = (PFNGLVERTEXATTRIBP1UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribP1uiv)), typeof(PFNGLVERTEXATTRIBP1UIVPROC));
            glVertexAttribP2ui = (PFNGLVERTEXATTRIBP2UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribP2ui)), typeof(PFNGLVERTEXATTRIBP2UIPROC));
            glVertexAttribP2uiv = (PFNGLVERTEXATTRIBP2UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribP2uiv)), typeof(PFNGLVERTEXATTRIBP2UIVPROC));
            glVertexAttribP3ui = (PFNGLVERTEXATTRIBP3UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribP3ui)), typeof(PFNGLVERTEXATTRIBP3UIPROC));
            glVertexAttribP3uiv = (PFNGLVERTEXATTRIBP3UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribP3uiv)), typeof(PFNGLVERTEXATTRIBP3UIVPROC));
            glVertexAttribP4ui = (PFNGLVERTEXATTRIBP4UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribP4ui)), typeof(PFNGLVERTEXATTRIBP4UIPROC));
            glVertexAttribP4uiv = (PFNGLVERTEXATTRIBP4UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribP4uiv)), typeof(PFNGLVERTEXATTRIBP4UIVPROC));
            glVertexP2ui = (PFNGLVERTEXP2UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexP2ui)), typeof(PFNGLVERTEXP2UIPROC));
            glVertexP2uiv = (PFNGLVERTEXP2UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexP2uiv)), typeof(PFNGLVERTEXP2UIVPROC));
            glVertexP3ui = (PFNGLVERTEXP3UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexP3ui)), typeof(PFNGLVERTEXP3UIPROC));
            glVertexP3uiv = (PFNGLVERTEXP3UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexP3uiv)), typeof(PFNGLVERTEXP3UIVPROC));
            glVertexP4ui = (PFNGLVERTEXP4UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexP4ui)), typeof(PFNGLVERTEXP4UIPROC));
            glVertexP4uiv = (PFNGLVERTEXP4UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexP4uiv)), typeof(PFNGLVERTEXP4UIVPROC));
            glTexCoordP1ui = (PFNGLTEXCOORDP1UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexCoordP1ui)), typeof(PFNGLTEXCOORDP1UIPROC));
            glTexCoordP1uiv = (PFNGLTEXCOORDP1UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexCoordP1uiv)), typeof(PFNGLTEXCOORDP1UIVPROC));
            glTexCoordP2ui = (PFNGLTEXCOORDP2UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexCoordP2ui)), typeof(PFNGLTEXCOORDP2UIPROC));
            glTexCoordP2uiv = (PFNGLTEXCOORDP2UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexCoordP2uiv)), typeof(PFNGLTEXCOORDP2UIVPROC));
            glTexCoordP3ui = (PFNGLTEXCOORDP3UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexCoordP3ui)), typeof(PFNGLTEXCOORDP3UIPROC));
            glTexCoordP3uiv = (PFNGLTEXCOORDP3UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexCoordP3uiv)), typeof(PFNGLTEXCOORDP3UIVPROC));
            glTexCoordP4ui = (PFNGLTEXCOORDP4UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexCoordP4ui)), typeof(PFNGLTEXCOORDP4UIPROC));
            glTexCoordP4uiv = (PFNGLTEXCOORDP4UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexCoordP4uiv)), typeof(PFNGLTEXCOORDP4UIVPROC));
            glMultiTexCoordP1ui = (PFNGLMULTITEXCOORDP1UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiTexCoordP1ui)), typeof(PFNGLMULTITEXCOORDP1UIPROC));
            glMultiTexCoordP1uiv = (PFNGLMULTITEXCOORDP1UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiTexCoordP1uiv)), typeof(PFNGLMULTITEXCOORDP1UIVPROC));
            glMultiTexCoordP2ui = (PFNGLMULTITEXCOORDP2UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiTexCoordP2ui)), typeof(PFNGLMULTITEXCOORDP2UIPROC));
            glMultiTexCoordP2uiv = (PFNGLMULTITEXCOORDP2UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiTexCoordP2uiv)), typeof(PFNGLMULTITEXCOORDP2UIVPROC));
            glMultiTexCoordP3ui = (PFNGLMULTITEXCOORDP3UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiTexCoordP3ui)), typeof(PFNGLMULTITEXCOORDP3UIPROC));
            glMultiTexCoordP3uiv = (PFNGLMULTITEXCOORDP3UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiTexCoordP3uiv)), typeof(PFNGLMULTITEXCOORDP3UIVPROC));
            glMultiTexCoordP4ui = (PFNGLMULTITEXCOORDP4UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiTexCoordP4ui)), typeof(PFNGLMULTITEXCOORDP4UIPROC));
            glMultiTexCoordP4uiv = (PFNGLMULTITEXCOORDP4UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiTexCoordP4uiv)), typeof(PFNGLMULTITEXCOORDP4UIVPROC));
            glNormalP3ui = (PFNGLNORMALP3UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNormalP3ui)), typeof(PFNGLNORMALP3UIPROC));
            glNormalP3uiv = (PFNGLNORMALP3UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNormalP3uiv)), typeof(PFNGLNORMALP3UIVPROC));
            glColorP3ui = (PFNGLCOLORP3UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glColorP3ui)), typeof(PFNGLCOLORP3UIPROC));
            glColorP3uiv = (PFNGLCOLORP3UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glColorP3uiv)), typeof(PFNGLCOLORP3UIVPROC));
            glColorP4ui = (PFNGLCOLORP4UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glColorP4ui)), typeof(PFNGLCOLORP4UIPROC));
            glColorP4uiv = (PFNGLCOLORP4UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glColorP4uiv)), typeof(PFNGLCOLORP4UIVPROC));
            glSecondaryColorP3ui = (PFNGLSECONDARYCOLORP3UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glSecondaryColorP3ui)), typeof(PFNGLSECONDARYCOLORP3UIPROC));
            glSecondaryColorP3uiv = (PFNGLSECONDARYCOLORP3UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glSecondaryColorP3uiv)), typeof(PFNGLSECONDARYCOLORP3UIVPROC));

        }

        internal static void LoadGL40(glLoader load)
        {
            glMinSampleShading = (PFNGLMINSAMPLESHADINGPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMinSampleShading)), typeof(PFNGLMINSAMPLESHADINGPROC));
            glBlendEquationi = (PFNGLBLENDEQUATIONIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBlendEquationi)), typeof(PFNGLBLENDEQUATIONIPROC));
            glBlendEquationSeparatei = (PFNGLBLENDEQUATIONSEPARATEIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBlendEquationSeparatei)), typeof(PFNGLBLENDEQUATIONSEPARATEIPROC));
            glBlendFunci = (PFNGLBLENDFUNCIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBlendFunci)), typeof(PFNGLBLENDFUNCIPROC));
            glBlendFuncSeparatei = (PFNGLBLENDFUNCSEPARATEIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBlendFuncSeparatei)), typeof(PFNGLBLENDFUNCSEPARATEIPROC));
            glDrawArraysIndirect = (PFNGLDRAWARRAYSINDIRECTPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawArraysIndirect)), typeof(PFNGLDRAWARRAYSINDIRECTPROC));
            glDrawElementsIndirect = (PFNGLDRAWELEMENTSINDIRECTPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawElementsIndirect)), typeof(PFNGLDRAWELEMENTSINDIRECTPROC));
            glUniform1d = (PFNGLUNIFORM1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform1d)), typeof(PFNGLUNIFORM1DPROC));
            glUniform2d = (PFNGLUNIFORM2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform2d)), typeof(PFNGLUNIFORM2DPROC));
            glUniform3d = (PFNGLUNIFORM3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform3d)), typeof(PFNGLUNIFORM3DPROC));
            glUniform4d = (PFNGLUNIFORM4DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform4d)), typeof(PFNGLUNIFORM4DPROC));
            glUniform1dv = (PFNGLUNIFORM1DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform1dv)), typeof(PFNGLUNIFORM1DVPROC));
            glUniform2dv = (PFNGLUNIFORM2DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform2dv)), typeof(PFNGLUNIFORM2DVPROC));
            glUniform3dv = (PFNGLUNIFORM3DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform3dv)), typeof(PFNGLUNIFORM3DVPROC));
            glUniform4dv = (PFNGLUNIFORM4DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniform4dv)), typeof(PFNGLUNIFORM4DVPROC));
            glUniformMatrix2dv = (PFNGLUNIFORMMATRIX2DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix2dv)), typeof(PFNGLUNIFORMMATRIX2DVPROC));
            glUniformMatrix3dv = (PFNGLUNIFORMMATRIX3DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix3dv)), typeof(PFNGLUNIFORMMATRIX3DVPROC));
            glUniformMatrix4dv = (PFNGLUNIFORMMATRIX4DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix4dv)), typeof(PFNGLUNIFORMMATRIX4DVPROC));
            glUniformMatrix2x3dv = (PFNGLUNIFORMMATRIX2X3DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix2x3dv)), typeof(PFNGLUNIFORMMATRIX2X3DVPROC));
            glUniformMatrix2x4dv = (PFNGLUNIFORMMATRIX2X4DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix2x4dv)), typeof(PFNGLUNIFORMMATRIX2X4DVPROC));
            glUniformMatrix3x2dv = (PFNGLUNIFORMMATRIX3X2DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix3x2dv)), typeof(PFNGLUNIFORMMATRIX3X2DVPROC));
            glUniformMatrix3x4dv = (PFNGLUNIFORMMATRIX3X4DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix3x4dv)), typeof(PFNGLUNIFORMMATRIX3X4DVPROC));
            glUniformMatrix4x2dv = (PFNGLUNIFORMMATRIX4X2DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix4x2dv)), typeof(PFNGLUNIFORMMATRIX4X2DVPROC));
            glUniformMatrix4x3dv = (PFNGLUNIFORMMATRIX4X3DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformMatrix4x3dv)), typeof(PFNGLUNIFORMMATRIX4X3DVPROC));
            glGetUniformdv = (PFNGLGETUNIFORMDVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetUniformdv)), typeof(PFNGLGETUNIFORMDVPROC));
            glGetSubroutineUniformLocation = (PFNGLGETSUBROUTINEUNIFORMLOCATIONPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetSubroutineUniformLocation)), typeof(PFNGLGETSUBROUTINEUNIFORMLOCATIONPROC));
            glGetSubroutineIndex = (PFNGLGETSUBROUTINEINDEXPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetSubroutineIndex)), typeof(PFNGLGETSUBROUTINEINDEXPROC));
            glGetActiveSubroutineUniformiv = (PFNGLGETACTIVESUBROUTINEUNIFORMIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetActiveSubroutineUniformiv)), typeof(PFNGLGETACTIVESUBROUTINEUNIFORMIVPROC));
            glGetActiveSubroutineUniformName = (PFNGLGETACTIVESUBROUTINEUNIFORMNAMEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetActiveSubroutineUniformName)), typeof(PFNGLGETACTIVESUBROUTINEUNIFORMNAMEPROC));
            glGetActiveSubroutineName = (PFNGLGETACTIVESUBROUTINENAMEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetActiveSubroutineName)), typeof(PFNGLGETACTIVESUBROUTINENAMEPROC));
            glUniformSubroutinesuiv = (PFNGLUNIFORMSUBROUTINESUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUniformSubroutinesuiv)), typeof(PFNGLUNIFORMSUBROUTINESUIVPROC));
            glGetUniformSubroutineuiv = (PFNGLGETUNIFORMSUBROUTINEUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetUniformSubroutineuiv)), typeof(PFNGLGETUNIFORMSUBROUTINEUIVPROC));
            glGetProgramStageiv = (PFNGLGETPROGRAMSTAGEIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetProgramStageiv)), typeof(PFNGLGETPROGRAMSTAGEIVPROC));
            glPatchParameteri = (PFNGLPATCHPARAMETERIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPatchParameteri)), typeof(PFNGLPATCHPARAMETERIPROC));
            glPatchParameterfv = (PFNGLPATCHPARAMETERFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPatchParameterfv)), typeof(PFNGLPATCHPARAMETERFVPROC));
            glBindTransformFeedback = (PFNGLBINDTRANSFORMFEEDBACKPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindTransformFeedback)), typeof(PFNGLBINDTRANSFORMFEEDBACKPROC));
            glDeleteTransformFeedbacks = (PFNGLDELETETRANSFORMFEEDBACKSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDeleteTransformFeedbacks)), typeof(PFNGLDELETETRANSFORMFEEDBACKSPROC));
            glGenTransformFeedbacks = (PFNGLGENTRANSFORMFEEDBACKSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGenTransformFeedbacks)), typeof(PFNGLGENTRANSFORMFEEDBACKSPROC));
            glIsTransformFeedback = (PFNGLISTRANSFORMFEEDBACKPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsTransformFeedback)), typeof(PFNGLISTRANSFORMFEEDBACKPROC));
            glPauseTransformFeedback = (PFNGLPAUSETRANSFORMFEEDBACKPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPauseTransformFeedback)), typeof(PFNGLPAUSETRANSFORMFEEDBACKPROC));
            glResumeTransformFeedback = (PFNGLRESUMETRANSFORMFEEDBACKPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glResumeTransformFeedback)), typeof(PFNGLRESUMETRANSFORMFEEDBACKPROC));
            glDrawTransformFeedback = (PFNGLDRAWTRANSFORMFEEDBACKPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawTransformFeedback)), typeof(PFNGLDRAWTRANSFORMFEEDBACKPROC));
            glDrawTransformFeedbackStream = (PFNGLDRAWTRANSFORMFEEDBACKSTREAMPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawTransformFeedbackStream)), typeof(PFNGLDRAWTRANSFORMFEEDBACKSTREAMPROC));
            glBeginQueryIndexed = (PFNGLBEGINQUERYINDEXEDPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBeginQueryIndexed)), typeof(PFNGLBEGINQUERYINDEXEDPROC));
            glEndQueryIndexed = (PFNGLENDQUERYINDEXEDPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glEndQueryIndexed)), typeof(PFNGLENDQUERYINDEXEDPROC));
            glGetQueryIndexediv = (PFNGLGETQUERYINDEXEDIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetQueryIndexediv)), typeof(PFNGLGETQUERYINDEXEDIVPROC));

        }

        internal static void LoadGL41(glLoader load)
        {
            glReleaseShaderCompiler = (PFNGLRELEASESHADERCOMPILERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glReleaseShaderCompiler)), typeof(PFNGLRELEASESHADERCOMPILERPROC));
            glShaderBinary = (PFNGLSHADERBINARYPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glShaderBinary)), typeof(PFNGLSHADERBINARYPROC));
            glGetShaderPrecisionFormat = (PFNGLGETSHADERPRECISIONFORMATPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetShaderPrecisionFormat)), typeof(PFNGLGETSHADERPRECISIONFORMATPROC));
            glDepthRangef = (PFNGLDEPTHRANGEFPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDepthRangef)), typeof(PFNGLDEPTHRANGEFPROC));
            glClearDepthf = (PFNGLCLEARDEPTHFPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearDepthf)), typeof(PFNGLCLEARDEPTHFPROC));
            glGetProgramBinary = (PFNGLGETPROGRAMBINARYPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetProgramBinary)), typeof(PFNGLGETPROGRAMBINARYPROC));
            glProgramBinary = (PFNGLPROGRAMBINARYPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramBinary)), typeof(PFNGLPROGRAMBINARYPROC));
            glProgramParameteri = (PFNGLPROGRAMPARAMETERIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramParameteri)), typeof(PFNGLPROGRAMPARAMETERIPROC));
            glUseProgramStages = (PFNGLUSEPROGRAMSTAGESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUseProgramStages)), typeof(PFNGLUSEPROGRAMSTAGESPROC));
            glActiveShaderProgram = (PFNGLACTIVESHADERPROGRAMPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glActiveShaderProgram)), typeof(PFNGLACTIVESHADERPROGRAMPROC));
            glCreateShaderProgramv = (PFNGLCREATESHADERPROGRAMVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCreateShaderProgramv)), typeof(PFNGLCREATESHADERPROGRAMVPROC));
            glBindProgramPipeline = (PFNGLBINDPROGRAMPIPELINEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindProgramPipeline)), typeof(PFNGLBINDPROGRAMPIPELINEPROC));
            glDeleteProgramPipelines = (PFNGLDELETEPROGRAMPIPELINESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDeleteProgramPipelines)), typeof(PFNGLDELETEPROGRAMPIPELINESPROC));
            glGenProgramPipelines = (PFNGLGENPROGRAMPIPELINESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGenProgramPipelines)), typeof(PFNGLGENPROGRAMPIPELINESPROC));
            glIsProgramPipeline = (PFNGLISPROGRAMPIPELINEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glIsProgramPipeline)), typeof(PFNGLISPROGRAMPIPELINEPROC));
            glGetProgramPipelineiv = (PFNGLGETPROGRAMPIPELINEIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetProgramPipelineiv)), typeof(PFNGLGETPROGRAMPIPELINEIVPROC));
            glProgramUniform1i = (PFNGLPROGRAMUNIFORM1IPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform1i)), typeof(PFNGLPROGRAMUNIFORM1IPROC));
            glProgramUniform1iv = (PFNGLPROGRAMUNIFORM1IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform1iv)), typeof(PFNGLPROGRAMUNIFORM1IVPROC));
            glProgramUniform1f = (PFNGLPROGRAMUNIFORM1FPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform1f)), typeof(PFNGLPROGRAMUNIFORM1FPROC));
            glProgramUniform1fv = (PFNGLPROGRAMUNIFORM1FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform1fv)), typeof(PFNGLPROGRAMUNIFORM1FVPROC));
            glProgramUniform1d = (PFNGLPROGRAMUNIFORM1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform1d)), typeof(PFNGLPROGRAMUNIFORM1DPROC));
            glProgramUniform1dv = (PFNGLPROGRAMUNIFORM1DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform1dv)), typeof(PFNGLPROGRAMUNIFORM1DVPROC));
            glProgramUniform1ui = (PFNGLPROGRAMUNIFORM1UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform1ui)), typeof(PFNGLPROGRAMUNIFORM1UIPROC));
            glProgramUniform1uiv = (PFNGLPROGRAMUNIFORM1UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform1uiv)), typeof(PFNGLPROGRAMUNIFORM1UIVPROC));
            glProgramUniform2i = (PFNGLPROGRAMUNIFORM2IPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform2i)), typeof(PFNGLPROGRAMUNIFORM2IPROC));
            glProgramUniform2iv = (PFNGLPROGRAMUNIFORM2IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform2iv)), typeof(PFNGLPROGRAMUNIFORM2IVPROC));
            glProgramUniform2f = (PFNGLPROGRAMUNIFORM2FPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform2f)), typeof(PFNGLPROGRAMUNIFORM2FPROC));
            glProgramUniform2fv = (PFNGLPROGRAMUNIFORM2FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform2fv)), typeof(PFNGLPROGRAMUNIFORM2FVPROC));
            glProgramUniform2d = (PFNGLPROGRAMUNIFORM2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform2d)), typeof(PFNGLPROGRAMUNIFORM2DPROC));
            glProgramUniform2dv = (PFNGLPROGRAMUNIFORM2DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform2dv)), typeof(PFNGLPROGRAMUNIFORM2DVPROC));
            glProgramUniform2ui = (PFNGLPROGRAMUNIFORM2UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform2ui)), typeof(PFNGLPROGRAMUNIFORM2UIPROC));
            glProgramUniform2uiv = (PFNGLPROGRAMUNIFORM2UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform2uiv)), typeof(PFNGLPROGRAMUNIFORM2UIVPROC));
            glProgramUniform3i = (PFNGLPROGRAMUNIFORM3IPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform3i)), typeof(PFNGLPROGRAMUNIFORM3IPROC));
            glProgramUniform3iv = (PFNGLPROGRAMUNIFORM3IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform3iv)), typeof(PFNGLPROGRAMUNIFORM3IVPROC));
            glProgramUniform3f = (PFNGLPROGRAMUNIFORM3FPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform3f)), typeof(PFNGLPROGRAMUNIFORM3FPROC));
            glProgramUniform3fv = (PFNGLPROGRAMUNIFORM3FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform3fv)), typeof(PFNGLPROGRAMUNIFORM3FVPROC));
            glProgramUniform3d = (PFNGLPROGRAMUNIFORM3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform3d)), typeof(PFNGLPROGRAMUNIFORM3DPROC));
            glProgramUniform3dv = (PFNGLPROGRAMUNIFORM3DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform3dv)), typeof(PFNGLPROGRAMUNIFORM3DVPROC));
            glProgramUniform3ui = (PFNGLPROGRAMUNIFORM3UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform3ui)), typeof(PFNGLPROGRAMUNIFORM3UIPROC));
            glProgramUniform3uiv = (PFNGLPROGRAMUNIFORM3UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform3uiv)), typeof(PFNGLPROGRAMUNIFORM3UIVPROC));
            glProgramUniform4i = (PFNGLPROGRAMUNIFORM4IPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform4i)), typeof(PFNGLPROGRAMUNIFORM4IPROC));
            glProgramUniform4iv = (PFNGLPROGRAMUNIFORM4IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform4iv)), typeof(PFNGLPROGRAMUNIFORM4IVPROC));
            glProgramUniform4f = (PFNGLPROGRAMUNIFORM4FPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform4f)), typeof(PFNGLPROGRAMUNIFORM4FPROC));
            glProgramUniform4fv = (PFNGLPROGRAMUNIFORM4FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform4fv)), typeof(PFNGLPROGRAMUNIFORM4FVPROC));
            glProgramUniform4d = (PFNGLPROGRAMUNIFORM4DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform4d)), typeof(PFNGLPROGRAMUNIFORM4DPROC));
            glProgramUniform4dv = (PFNGLPROGRAMUNIFORM4DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform4dv)), typeof(PFNGLPROGRAMUNIFORM4DVPROC));
            glProgramUniform4ui = (PFNGLPROGRAMUNIFORM4UIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform4ui)), typeof(PFNGLPROGRAMUNIFORM4UIPROC));
            glProgramUniform4uiv = (PFNGLPROGRAMUNIFORM4UIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniform4uiv)), typeof(PFNGLPROGRAMUNIFORM4UIVPROC));
            glProgramUniformMatrix2fv = (PFNGLPROGRAMUNIFORMMATRIX2FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix2fv)), typeof(PFNGLPROGRAMUNIFORMMATRIX2FVPROC));
            glProgramUniformMatrix3fv = (PFNGLPROGRAMUNIFORMMATRIX3FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix3fv)), typeof(PFNGLPROGRAMUNIFORMMATRIX3FVPROC));
            glProgramUniformMatrix4fv = (PFNGLPROGRAMUNIFORMMATRIX4FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix4fv)), typeof(PFNGLPROGRAMUNIFORMMATRIX4FVPROC));
            glProgramUniformMatrix2dv = (PFNGLPROGRAMUNIFORMMATRIX2DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix2dv)), typeof(PFNGLPROGRAMUNIFORMMATRIX2DVPROC));
            glProgramUniformMatrix3dv = (PFNGLPROGRAMUNIFORMMATRIX3DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix3dv)), typeof(PFNGLPROGRAMUNIFORMMATRIX3DVPROC));
            glProgramUniformMatrix4dv = (PFNGLPROGRAMUNIFORMMATRIX4DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix4dv)), typeof(PFNGLPROGRAMUNIFORMMATRIX4DVPROC));
            glProgramUniformMatrix2x3fv = (PFNGLPROGRAMUNIFORMMATRIX2X3FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix2x3fv)), typeof(PFNGLPROGRAMUNIFORMMATRIX2X3FVPROC));
            glProgramUniformMatrix3x2fv = (PFNGLPROGRAMUNIFORMMATRIX3X2FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix3x2fv)), typeof(PFNGLPROGRAMUNIFORMMATRIX3X2FVPROC));
            glProgramUniformMatrix2x4fv = (PFNGLPROGRAMUNIFORMMATRIX2X4FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix2x4fv)), typeof(PFNGLPROGRAMUNIFORMMATRIX2X4FVPROC));
            glProgramUniformMatrix4x2fv = (PFNGLPROGRAMUNIFORMMATRIX4X2FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix4x2fv)), typeof(PFNGLPROGRAMUNIFORMMATRIX4X2FVPROC));
            glProgramUniformMatrix3x4fv = (PFNGLPROGRAMUNIFORMMATRIX3X4FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix3x4fv)), typeof(PFNGLPROGRAMUNIFORMMATRIX3X4FVPROC));
            glProgramUniformMatrix4x3fv = (PFNGLPROGRAMUNIFORMMATRIX4X3FVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix4x3fv)), typeof(PFNGLPROGRAMUNIFORMMATRIX4X3FVPROC));
            glProgramUniformMatrix2x3dv = (PFNGLPROGRAMUNIFORMMATRIX2X3DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix2x3dv)), typeof(PFNGLPROGRAMUNIFORMMATRIX2X3DVPROC));
            glProgramUniformMatrix3x2dv = (PFNGLPROGRAMUNIFORMMATRIX3X2DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix3x2dv)), typeof(PFNGLPROGRAMUNIFORMMATRIX3X2DVPROC));
            glProgramUniformMatrix2x4dv = (PFNGLPROGRAMUNIFORMMATRIX2X4DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix2x4dv)), typeof(PFNGLPROGRAMUNIFORMMATRIX2X4DVPROC));
            glProgramUniformMatrix4x2dv = (PFNGLPROGRAMUNIFORMMATRIX4X2DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix4x2dv)), typeof(PFNGLPROGRAMUNIFORMMATRIX4X2DVPROC));
            glProgramUniformMatrix3x4dv = (PFNGLPROGRAMUNIFORMMATRIX3X4DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix3x4dv)), typeof(PFNGLPROGRAMUNIFORMMATRIX3X4DVPROC));
            glProgramUniformMatrix4x3dv = (PFNGLPROGRAMUNIFORMMATRIX4X3DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glProgramUniformMatrix4x3dv)), typeof(PFNGLPROGRAMUNIFORMMATRIX4X3DVPROC));
            glValidateProgramPipeline = (PFNGLVALIDATEPROGRAMPIPELINEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glValidateProgramPipeline)), typeof(PFNGLVALIDATEPROGRAMPIPELINEPROC));
            glGetProgramPipelineInfoLog = (PFNGLGETPROGRAMPIPELINEINFOLOGPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetProgramPipelineInfoLog)), typeof(PFNGLGETPROGRAMPIPELINEINFOLOGPROC));
            glVertexAttribL1d = (PFNGLVERTEXATTRIBL1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribL1d)), typeof(PFNGLVERTEXATTRIBL1DPROC));
            glVertexAttribL2d = (PFNGLVERTEXATTRIBL2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribL2d)), typeof(PFNGLVERTEXATTRIBL2DPROC));
            glVertexAttribL3d = (PFNGLVERTEXATTRIBL3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribL3d)), typeof(PFNGLVERTEXATTRIBL3DPROC));
            glVertexAttribL4d = (PFNGLVERTEXATTRIBL4DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribL4d)), typeof(PFNGLVERTEXATTRIBL4DPROC));
            glVertexAttribL1dv = (PFNGLVERTEXATTRIBL1DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribL1dv)), typeof(PFNGLVERTEXATTRIBL1DVPROC));
            glVertexAttribL2dv = (PFNGLVERTEXATTRIBL2DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribL2dv)), typeof(PFNGLVERTEXATTRIBL2DVPROC));
            glVertexAttribL3dv = (PFNGLVERTEXATTRIBL3DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribL3dv)), typeof(PFNGLVERTEXATTRIBL3DVPROC));
            glVertexAttribL4dv = (PFNGLVERTEXATTRIBL4DVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribL4dv)), typeof(PFNGLVERTEXATTRIBL4DVPROC));
            glVertexAttribLPointer = (PFNGLVERTEXATTRIBLPOINTERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribLPointer)), typeof(PFNGLVERTEXATTRIBLPOINTERPROC));
            glGetVertexAttribLdv = (PFNGLGETVERTEXATTRIBLDVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetVertexAttribLdv)), typeof(PFNGLGETVERTEXATTRIBLDVPROC));
            glViewportArrayv = (PFNGLVIEWPORTARRAYVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glViewportArrayv)), typeof(PFNGLVIEWPORTARRAYVPROC));
            glViewportIndexedf = (PFNGLVIEWPORTINDEXEDFPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glViewportIndexedf)), typeof(PFNGLVIEWPORTINDEXEDFPROC));
            glViewportIndexedfv = (PFNGLVIEWPORTINDEXEDFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glViewportIndexedfv)), typeof(PFNGLVIEWPORTINDEXEDFVPROC));
            glScissorArrayv = (PFNGLSCISSORARRAYVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glScissorArrayv)), typeof(PFNGLSCISSORARRAYVPROC));
            glScissorIndexed = (PFNGLSCISSORINDEXEDPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glScissorIndexed)), typeof(PFNGLSCISSORINDEXEDPROC));
            glScissorIndexedv = (PFNGLSCISSORINDEXEDVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glScissorIndexedv)), typeof(PFNGLSCISSORINDEXEDVPROC));
            glDepthRangeArrayv = (PFNGLDEPTHRANGEARRAYVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDepthRangeArrayv)), typeof(PFNGLDEPTHRANGEARRAYVPROC));
            glDepthRangeIndexed = (PFNGLDEPTHRANGEINDEXEDPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDepthRangeIndexed)), typeof(PFNGLDEPTHRANGEINDEXEDPROC));
            glGetFloati_v = (PFNGLGETFLOATI_VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetFloati_v)), typeof(PFNGLGETFLOATI_VPROC));
            glGetDoublei_v = (PFNGLGETDOUBLEI_VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetDoublei_v)), typeof(PFNGLGETDOUBLEI_VPROC));

        }

        internal static void LoadGL42(glLoader load)
        {
            glDrawArraysInstancedBaseInstance = (PFNGLDRAWARRAYSINSTANCEDBASEINSTANCEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawArraysInstancedBaseInstance)), typeof(PFNGLDRAWARRAYSINSTANCEDBASEINSTANCEPROC));
            glDrawElementsInstancedBaseInstance = (PFNGLDRAWELEMENTSINSTANCEDBASEINSTANCEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawElementsInstancedBaseInstance)), typeof(PFNGLDRAWELEMENTSINSTANCEDBASEINSTANCEPROC));
            glDrawElementsInstancedBaseVertexBaseInstance = (PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXBASEINSTANCEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawElementsInstancedBaseVertexBaseInstance)), typeof(PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXBASEINSTANCEPROC));
            glGetInternalformativ = (PFNGLGETINTERNALFORMATIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetInternalformativ)), typeof(PFNGLGETINTERNALFORMATIVPROC));
            glGetActiveAtomicCounterBufferiv = (PFNGLGETACTIVEATOMICCOUNTERBUFFERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetActiveAtomicCounterBufferiv)), typeof(PFNGLGETACTIVEATOMICCOUNTERBUFFERIVPROC));
            glBindImageTexture = (PFNGLBINDIMAGETEXTUREPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindImageTexture)), typeof(PFNGLBINDIMAGETEXTUREPROC));
            glMemoryBarrier = (PFNGLMEMORYBARRIERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMemoryBarrier)), typeof(PFNGLMEMORYBARRIERPROC));
            glTexStorage1D = (PFNGLTEXSTORAGE1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexStorage1D)), typeof(PFNGLTEXSTORAGE1DPROC));
            glTexStorage2D = (PFNGLTEXSTORAGE2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexStorage2D)), typeof(PFNGLTEXSTORAGE2DPROC));
            glTexStorage3D = (PFNGLTEXSTORAGE3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexStorage3D)), typeof(PFNGLTEXSTORAGE3DPROC));
            glDrawTransformFeedbackInstanced = (PFNGLDRAWTRANSFORMFEEDBACKINSTANCEDPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawTransformFeedbackInstanced)), typeof(PFNGLDRAWTRANSFORMFEEDBACKINSTANCEDPROC));
            glDrawTransformFeedbackStreamInstanced = (PFNGLDRAWTRANSFORMFEEDBACKSTREAMINSTANCEDPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDrawTransformFeedbackStreamInstanced)), typeof(PFNGLDRAWTRANSFORMFEEDBACKSTREAMINSTANCEDPROC));

        }

        internal static void LoadGL43(glLoader load)
        {
            glClearBufferData = (PFNGLCLEARBUFFERDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearBufferData)), typeof(PFNGLCLEARBUFFERDATAPROC));
            glClearBufferSubData = (PFNGLCLEARBUFFERSUBDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearBufferSubData)), typeof(PFNGLCLEARBUFFERSUBDATAPROC));
            glDispatchCompute = (PFNGLDISPATCHCOMPUTEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDispatchCompute)), typeof(PFNGLDISPATCHCOMPUTEPROC));
            glDispatchComputeIndirect = (PFNGLDISPATCHCOMPUTEINDIRECTPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDispatchComputeIndirect)), typeof(PFNGLDISPATCHCOMPUTEINDIRECTPROC));
            glCopyImageSubData = (PFNGLCOPYIMAGESUBDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCopyImageSubData)), typeof(PFNGLCOPYIMAGESUBDATAPROC));
            glFramebufferParameteri = (PFNGLFRAMEBUFFERPARAMETERIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFramebufferParameteri)), typeof(PFNGLFRAMEBUFFERPARAMETERIPROC));
            glGetFramebufferParameteriv = (PFNGLGETFRAMEBUFFERPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetFramebufferParameteriv)), typeof(PFNGLGETFRAMEBUFFERPARAMETERIVPROC));
            glGetInternalformati64v = (PFNGLGETINTERNALFORMATI64VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetInternalformati64v)), typeof(PFNGLGETINTERNALFORMATI64VPROC));
            glInvalidateTexSubImage = (PFNGLINVALIDATETEXSUBIMAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glInvalidateTexSubImage)), typeof(PFNGLINVALIDATETEXSUBIMAGEPROC));
            glInvalidateTexImage = (PFNGLINVALIDATETEXIMAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glInvalidateTexImage)), typeof(PFNGLINVALIDATETEXIMAGEPROC));
            glInvalidateBufferSubData = (PFNGLINVALIDATEBUFFERSUBDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glInvalidateBufferSubData)), typeof(PFNGLINVALIDATEBUFFERSUBDATAPROC));
            glInvalidateBufferData = (PFNGLINVALIDATEBUFFERDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glInvalidateBufferData)), typeof(PFNGLINVALIDATEBUFFERDATAPROC));
            glInvalidateFramebuffer = (PFNGLINVALIDATEFRAMEBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glInvalidateFramebuffer)), typeof(PFNGLINVALIDATEFRAMEBUFFERPROC));
            glInvalidateSubFramebuffer = (PFNGLINVALIDATESUBFRAMEBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glInvalidateSubFramebuffer)), typeof(PFNGLINVALIDATESUBFRAMEBUFFERPROC));
            glMultiDrawArraysIndirect = (PFNGLMULTIDRAWARRAYSINDIRECTPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiDrawArraysIndirect)), typeof(PFNGLMULTIDRAWARRAYSINDIRECTPROC));
            glMultiDrawElementsIndirect = (PFNGLMULTIDRAWELEMENTSINDIRECTPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiDrawElementsIndirect)), typeof(PFNGLMULTIDRAWELEMENTSINDIRECTPROC));
            glGetProgramInterfaceiv = (PFNGLGETPROGRAMINTERFACEIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetProgramInterfaceiv)), typeof(PFNGLGETPROGRAMINTERFACEIVPROC));
            glGetProgramResourceIndex = (PFNGLGETPROGRAMRESOURCEINDEXPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetProgramResourceIndex)), typeof(PFNGLGETPROGRAMRESOURCEINDEXPROC));
            glGetProgramResourceName = (PFNGLGETPROGRAMRESOURCENAMEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetProgramResourceName)), typeof(PFNGLGETPROGRAMRESOURCENAMEPROC));
            glGetProgramResourceiv = (PFNGLGETPROGRAMRESOURCEIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetProgramResourceiv)), typeof(PFNGLGETPROGRAMRESOURCEIVPROC));
            glGetProgramResourceLocation = (PFNGLGETPROGRAMRESOURCELOCATIONPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetProgramResourceLocation)), typeof(PFNGLGETPROGRAMRESOURCELOCATIONPROC));
            glGetProgramResourceLocationIndex = (PFNGLGETPROGRAMRESOURCELOCATIONINDEXPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetProgramResourceLocationIndex)), typeof(PFNGLGETPROGRAMRESOURCELOCATIONINDEXPROC));
            glShaderStorageBlockBinding = (PFNGLSHADERSTORAGEBLOCKBINDINGPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glShaderStorageBlockBinding)), typeof(PFNGLSHADERSTORAGEBLOCKBINDINGPROC));
            glTexBufferRange = (PFNGLTEXBUFFERRANGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexBufferRange)), typeof(PFNGLTEXBUFFERRANGEPROC));
            glTexStorage2DMultisample = (PFNGLTEXSTORAGE2DMULTISAMPLEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexStorage2DMultisample)), typeof(PFNGLTEXSTORAGE2DMULTISAMPLEPROC));
            glTexStorage3DMultisample = (PFNGLTEXSTORAGE3DMULTISAMPLEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTexStorage3DMultisample)), typeof(PFNGLTEXSTORAGE3DMULTISAMPLEPROC));
            glTextureView = (PFNGLTEXTUREVIEWPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureView)), typeof(PFNGLTEXTUREVIEWPROC));
            glBindVertexBuffer = (PFNGLBINDVERTEXBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindVertexBuffer)), typeof(PFNGLBINDVERTEXBUFFERPROC));
            glVertexAttribFormat = (PFNGLVERTEXATTRIBFORMATPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribFormat)), typeof(PFNGLVERTEXATTRIBFORMATPROC));
            glVertexAttribIFormat = (PFNGLVERTEXATTRIBIFORMATPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribIFormat)), typeof(PFNGLVERTEXATTRIBIFORMATPROC));
            glVertexAttribLFormat = (PFNGLVERTEXATTRIBLFORMATPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribLFormat)), typeof(PFNGLVERTEXATTRIBLFORMATPROC));
            glVertexAttribBinding = (PFNGLVERTEXATTRIBBINDINGPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexAttribBinding)), typeof(PFNGLVERTEXATTRIBBINDINGPROC));
            glVertexBindingDivisor = (PFNGLVERTEXBINDINGDIVISORPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexBindingDivisor)), typeof(PFNGLVERTEXBINDINGDIVISORPROC));
            glDebugMessageControl = (PFNGLDEBUGMESSAGECONTROLPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDebugMessageControl)), typeof(PFNGLDEBUGMESSAGECONTROLPROC));
            glDebugMessageInsert = (PFNGLDEBUGMESSAGEINSERTPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDebugMessageInsert)), typeof(PFNGLDEBUGMESSAGEINSERTPROC));
            glDebugMessageCallback = (PFNGLDEBUGMESSAGECALLBACKPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDebugMessageCallback)), typeof(PFNGLDEBUGMESSAGECALLBACKPROC));
            glGetDebugMessageLog = (PFNGLGETDEBUGMESSAGELOGPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetDebugMessageLog)), typeof(PFNGLGETDEBUGMESSAGELOGPROC));
            glPushDebugGroup = (PFNGLPUSHDEBUGGROUPPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPushDebugGroup)), typeof(PFNGLPUSHDEBUGGROUPPROC));
            glPopDebugGroup = (PFNGLPOPDEBUGGROUPPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPopDebugGroup)), typeof(PFNGLPOPDEBUGGROUPPROC));
            glObjectLabel = (PFNGLOBJECTLABELPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glObjectLabel)), typeof(PFNGLOBJECTLABELPROC));
            glGetObjectLabel = (PFNGLGETOBJECTLABELPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetObjectLabel)), typeof(PFNGLGETOBJECTLABELPROC));
            glObjectPtrLabel = (PFNGLOBJECTPTRLABELPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glObjectPtrLabel)), typeof(PFNGLOBJECTPTRLABELPROC));
            glGetObjectPtrLabel = (PFNGLGETOBJECTPTRLABELPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetObjectPtrLabel)), typeof(PFNGLGETOBJECTPTRLABELPROC));
            glGetPointerv = (PFNGLGETPOINTERVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetPointerv)), typeof(PFNGLGETPOINTERVPROC));

        }

        internal static void LoadGL44(glLoader load)
        {
            glBufferStorage = (PFNGLBUFFERSTORAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBufferStorage)), typeof(PFNGLBUFFERSTORAGEPROC));
            glClearTexImage = (PFNGLCLEARTEXIMAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearTexImage)), typeof(PFNGLCLEARTEXIMAGEPROC));
            glClearTexSubImage = (PFNGLCLEARTEXSUBIMAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearTexSubImage)), typeof(PFNGLCLEARTEXSUBIMAGEPROC));
            glBindBuffersBase = (PFNGLBINDBUFFERSBASEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindBuffersBase)), typeof(PFNGLBINDBUFFERSBASEPROC));
            glBindBuffersRange = (PFNGLBINDBUFFERSRANGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindBuffersRange)), typeof(PFNGLBINDBUFFERSRANGEPROC));
            glBindTextures = (PFNGLBINDTEXTURESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindTextures)), typeof(PFNGLBINDTEXTURESPROC));
            glBindSamplers = (PFNGLBINDSAMPLERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindSamplers)), typeof(PFNGLBINDSAMPLERSPROC));
            glBindImageTextures = (PFNGLBINDIMAGETEXTURESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindImageTextures)), typeof(PFNGLBINDIMAGETEXTURESPROC));
            glBindVertexBuffers = (PFNGLBINDVERTEXBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindVertexBuffers)), typeof(PFNGLBINDVERTEXBUFFERSPROC));

        }

        internal static void LoadGL45(glLoader load)
        {
            glClipControl = (PFNGLCLIPCONTROLPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClipControl)), typeof(PFNGLCLIPCONTROLPROC));
            glCreateTransformFeedbacks = (PFNGLCREATETRANSFORMFEEDBACKSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCreateTransformFeedbacks)), typeof(PFNGLCREATETRANSFORMFEEDBACKSPROC));
            glTransformFeedbackBufferBase = (PFNGLTRANSFORMFEEDBACKBUFFERBASEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTransformFeedbackBufferBase)), typeof(PFNGLTRANSFORMFEEDBACKBUFFERBASEPROC));
            glTransformFeedbackBufferRange = (PFNGLTRANSFORMFEEDBACKBUFFERRANGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTransformFeedbackBufferRange)), typeof(PFNGLTRANSFORMFEEDBACKBUFFERRANGEPROC));
            glGetTransformFeedbackiv = (PFNGLGETTRANSFORMFEEDBACKIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTransformFeedbackiv)), typeof(PFNGLGETTRANSFORMFEEDBACKIVPROC));
            glGetTransformFeedbacki_v = (PFNGLGETTRANSFORMFEEDBACKI_VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTransformFeedbacki_v)), typeof(PFNGLGETTRANSFORMFEEDBACKI_VPROC));
            glGetTransformFeedbacki64_v = (PFNGLGETTRANSFORMFEEDBACKI64_VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTransformFeedbacki64_v)), typeof(PFNGLGETTRANSFORMFEEDBACKI64_VPROC));
            glCreateBuffers = (PFNGLCREATEBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCreateBuffers)), typeof(PFNGLCREATEBUFFERSPROC));
            glNamedBufferStorage = (PFNGLNAMEDBUFFERSTORAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNamedBufferStorage)), typeof(PFNGLNAMEDBUFFERSTORAGEPROC));
            glNamedBufferData = (PFNGLNAMEDBUFFERDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNamedBufferData)), typeof(PFNGLNAMEDBUFFERDATAPROC));
            glNamedBufferSubData = (PFNGLNAMEDBUFFERSUBDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNamedBufferSubData)), typeof(PFNGLNAMEDBUFFERSUBDATAPROC));
            glCopyNamedBufferSubData = (PFNGLCOPYNAMEDBUFFERSUBDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCopyNamedBufferSubData)), typeof(PFNGLCOPYNAMEDBUFFERSUBDATAPROC));
            glClearNamedBufferData = (PFNGLCLEARNAMEDBUFFERDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearNamedBufferData)), typeof(PFNGLCLEARNAMEDBUFFERDATAPROC));
            glClearNamedBufferSubData = (PFNGLCLEARNAMEDBUFFERSUBDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearNamedBufferSubData)), typeof(PFNGLCLEARNAMEDBUFFERSUBDATAPROC));
            glMapNamedBuffer = (PFNGLMAPNAMEDBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMapNamedBuffer)), typeof(PFNGLMAPNAMEDBUFFERPROC));
            glMapNamedBufferRange = (PFNGLMAPNAMEDBUFFERRANGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMapNamedBufferRange)), typeof(PFNGLMAPNAMEDBUFFERRANGEPROC));
            glUnmapNamedBuffer = (PFNGLUNMAPNAMEDBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glUnmapNamedBuffer)), typeof(PFNGLUNMAPNAMEDBUFFERPROC));
            glFlushMappedNamedBufferRange = (PFNGLFLUSHMAPPEDNAMEDBUFFERRANGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glFlushMappedNamedBufferRange)), typeof(PFNGLFLUSHMAPPEDNAMEDBUFFERRANGEPROC));
            glGetNamedBufferParameteriv = (PFNGLGETNAMEDBUFFERPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetNamedBufferParameteriv)), typeof(PFNGLGETNAMEDBUFFERPARAMETERIVPROC));
            glGetNamedBufferParameteri64v = (PFNGLGETNAMEDBUFFERPARAMETERI64VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetNamedBufferParameteri64v)), typeof(PFNGLGETNAMEDBUFFERPARAMETERI64VPROC));
            glGetNamedBufferPointerv = (PFNGLGETNAMEDBUFFERPOINTERVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetNamedBufferPointerv)), typeof(PFNGLGETNAMEDBUFFERPOINTERVPROC));
            glGetNamedBufferSubData = (PFNGLGETNAMEDBUFFERSUBDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetNamedBufferSubData)), typeof(PFNGLGETNAMEDBUFFERSUBDATAPROC));
            glCreateFramebuffers = (PFNGLCREATEFRAMEBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCreateFramebuffers)), typeof(PFNGLCREATEFRAMEBUFFERSPROC));
            glNamedFramebufferRenderbuffer = (PFNGLNAMEDFRAMEBUFFERRENDERBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNamedFramebufferRenderbuffer)), typeof(PFNGLNAMEDFRAMEBUFFERRENDERBUFFERPROC));
            glNamedFramebufferParameteri = (PFNGLNAMEDFRAMEBUFFERPARAMETERIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNamedFramebufferParameteri)), typeof(PFNGLNAMEDFRAMEBUFFERPARAMETERIPROC));
            glNamedFramebufferTexture = (PFNGLNAMEDFRAMEBUFFERTEXTUREPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNamedFramebufferTexture)), typeof(PFNGLNAMEDFRAMEBUFFERTEXTUREPROC));
            glNamedFramebufferTextureLayer = (PFNGLNAMEDFRAMEBUFFERTEXTURELAYERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNamedFramebufferTextureLayer)), typeof(PFNGLNAMEDFRAMEBUFFERTEXTURELAYERPROC));
            glNamedFramebufferDrawBuffer = (PFNGLNAMEDFRAMEBUFFERDRAWBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNamedFramebufferDrawBuffer)), typeof(PFNGLNAMEDFRAMEBUFFERDRAWBUFFERPROC));
            glNamedFramebufferDrawBuffers = (PFNGLNAMEDFRAMEBUFFERDRAWBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNamedFramebufferDrawBuffers)), typeof(PFNGLNAMEDFRAMEBUFFERDRAWBUFFERSPROC));
            glNamedFramebufferReadBuffer = (PFNGLNAMEDFRAMEBUFFERREADBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNamedFramebufferReadBuffer)), typeof(PFNGLNAMEDFRAMEBUFFERREADBUFFERPROC));
            glInvalidateNamedFramebufferData = (PFNGLINVALIDATENAMEDFRAMEBUFFERDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glInvalidateNamedFramebufferData)), typeof(PFNGLINVALIDATENAMEDFRAMEBUFFERDATAPROC));
            glInvalidateNamedFramebufferSubData = (PFNGLINVALIDATENAMEDFRAMEBUFFERSUBDATAPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glInvalidateNamedFramebufferSubData)), typeof(PFNGLINVALIDATENAMEDFRAMEBUFFERSUBDATAPROC));
            glClearNamedFramebufferiv = (PFNGLCLEARNAMEDFRAMEBUFFERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearNamedFramebufferiv)), typeof(PFNGLCLEARNAMEDFRAMEBUFFERIVPROC));
            glClearNamedFramebufferuiv = (PFNGLCLEARNAMEDFRAMEBUFFERUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearNamedFramebufferuiv)), typeof(PFNGLCLEARNAMEDFRAMEBUFFERUIVPROC));
            glClearNamedFramebufferfv = (PFNGLCLEARNAMEDFRAMEBUFFERFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearNamedFramebufferfv)), typeof(PFNGLCLEARNAMEDFRAMEBUFFERFVPROC));
            glClearNamedFramebufferfi = (PFNGLCLEARNAMEDFRAMEBUFFERFIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glClearNamedFramebufferfi)), typeof(PFNGLCLEARNAMEDFRAMEBUFFERFIPROC));
            glBlitNamedFramebuffer = (PFNGLBLITNAMEDFRAMEBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBlitNamedFramebuffer)), typeof(PFNGLBLITNAMEDFRAMEBUFFERPROC));
            glCheckNamedFramebufferStatus = (PFNGLCHECKNAMEDFRAMEBUFFERSTATUSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCheckNamedFramebufferStatus)), typeof(PFNGLCHECKNAMEDFRAMEBUFFERSTATUSPROC));
            glGetNamedFramebufferParameteriv = (PFNGLGETNAMEDFRAMEBUFFERPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetNamedFramebufferParameteriv)), typeof(PFNGLGETNAMEDFRAMEBUFFERPARAMETERIVPROC));
            glGetNamedFramebufferAttachmentParameteriv = (PFNGLGETNAMEDFRAMEBUFFERATTACHMENTPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetNamedFramebufferAttachmentParameteriv)), typeof(PFNGLGETNAMEDFRAMEBUFFERATTACHMENTPARAMETERIVPROC));
            glCreateRenderbuffers = (PFNGLCREATERENDERBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCreateRenderbuffers)), typeof(PFNGLCREATERENDERBUFFERSPROC));
            glNamedRenderbufferStorage = (PFNGLNAMEDRENDERBUFFERSTORAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNamedRenderbufferStorage)), typeof(PFNGLNAMEDRENDERBUFFERSTORAGEPROC));
            glNamedRenderbufferStorageMultisample = (PFNGLNAMEDRENDERBUFFERSTORAGEMULTISAMPLEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glNamedRenderbufferStorageMultisample)), typeof(PFNGLNAMEDRENDERBUFFERSTORAGEMULTISAMPLEPROC));
            glGetNamedRenderbufferParameteriv = (PFNGLGETNAMEDRENDERBUFFERPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetNamedRenderbufferParameteriv)), typeof(PFNGLGETNAMEDRENDERBUFFERPARAMETERIVPROC));
            glCreateTextures = (PFNGLCREATETEXTURESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCreateTextures)), typeof(PFNGLCREATETEXTURESPROC));
            glTextureBuffer = (PFNGLTEXTUREBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureBuffer)), typeof(PFNGLTEXTUREBUFFERPROC));
            glTextureBufferRange = (PFNGLTEXTUREBUFFERRANGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureBufferRange)), typeof(PFNGLTEXTUREBUFFERRANGEPROC));
            glTextureStorage1D = (PFNGLTEXTURESTORAGE1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureStorage1D)), typeof(PFNGLTEXTURESTORAGE1DPROC));
            glTextureStorage2D = (PFNGLTEXTURESTORAGE2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureStorage2D)), typeof(PFNGLTEXTURESTORAGE2DPROC));
            glTextureStorage3D = (PFNGLTEXTURESTORAGE3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureStorage3D)), typeof(PFNGLTEXTURESTORAGE3DPROC));
            glTextureStorage2DMultisample = (PFNGLTEXTURESTORAGE2DMULTISAMPLEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureStorage2DMultisample)), typeof(PFNGLTEXTURESTORAGE2DMULTISAMPLEPROC));
            glTextureStorage3DMultisample = (PFNGLTEXTURESTORAGE3DMULTISAMPLEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureStorage3DMultisample)), typeof(PFNGLTEXTURESTORAGE3DMULTISAMPLEPROC));
            glTextureSubImage1D = (PFNGLTEXTURESUBIMAGE1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureSubImage1D)), typeof(PFNGLTEXTURESUBIMAGE1DPROC));
            glTextureSubImage2D = (PFNGLTEXTURESUBIMAGE2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureSubImage2D)), typeof(PFNGLTEXTURESUBIMAGE2DPROC));
            glTextureSubImage3D = (PFNGLTEXTURESUBIMAGE3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureSubImage3D)), typeof(PFNGLTEXTURESUBIMAGE3DPROC));
            glCompressedTextureSubImage1D = (PFNGLCOMPRESSEDTEXTURESUBIMAGE1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCompressedTextureSubImage1D)), typeof(PFNGLCOMPRESSEDTEXTURESUBIMAGE1DPROC));
            glCompressedTextureSubImage2D = (PFNGLCOMPRESSEDTEXTURESUBIMAGE2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCompressedTextureSubImage2D)), typeof(PFNGLCOMPRESSEDTEXTURESUBIMAGE2DPROC));
            glCompressedTextureSubImage3D = (PFNGLCOMPRESSEDTEXTURESUBIMAGE3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCompressedTextureSubImage3D)), typeof(PFNGLCOMPRESSEDTEXTURESUBIMAGE3DPROC));
            glCopyTextureSubImage1D = (PFNGLCOPYTEXTURESUBIMAGE1DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCopyTextureSubImage1D)), typeof(PFNGLCOPYTEXTURESUBIMAGE1DPROC));
            glCopyTextureSubImage2D = (PFNGLCOPYTEXTURESUBIMAGE2DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCopyTextureSubImage2D)), typeof(PFNGLCOPYTEXTURESUBIMAGE2DPROC));
            glCopyTextureSubImage3D = (PFNGLCOPYTEXTURESUBIMAGE3DPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCopyTextureSubImage3D)), typeof(PFNGLCOPYTEXTURESUBIMAGE3DPROC));
            glTextureParameterf = (PFNGLTEXTUREPARAMETERFPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureParameterf)), typeof(PFNGLTEXTUREPARAMETERFPROC));
            glTextureParameterfv = (PFNGLTEXTUREPARAMETERFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureParameterfv)), typeof(PFNGLTEXTUREPARAMETERFVPROC));
            glTextureParameteri = (PFNGLTEXTUREPARAMETERIPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureParameteri)), typeof(PFNGLTEXTUREPARAMETERIPROC));
            glTextureParameterIiv = (PFNGLTEXTUREPARAMETERIIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureParameterIiv)), typeof(PFNGLTEXTUREPARAMETERIIVPROC));
            glTextureParameterIuiv = (PFNGLTEXTUREPARAMETERIUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureParameterIuiv)), typeof(PFNGLTEXTUREPARAMETERIUIVPROC));
            glTextureParameteriv = (PFNGLTEXTUREPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureParameteriv)), typeof(PFNGLTEXTUREPARAMETERIVPROC));
            glGenerateTextureMipmap = (PFNGLGENERATETEXTUREMIPMAPPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGenerateTextureMipmap)), typeof(PFNGLGENERATETEXTUREMIPMAPPROC));
            glBindTextureUnit = (PFNGLBINDTEXTUREUNITPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glBindTextureUnit)), typeof(PFNGLBINDTEXTUREUNITPROC));
            glGetTextureImage = (PFNGLGETTEXTUREIMAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTextureImage)), typeof(PFNGLGETTEXTUREIMAGEPROC));
            glGetCompressedTextureImage = (PFNGLGETCOMPRESSEDTEXTUREIMAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetCompressedTextureImage)), typeof(PFNGLGETCOMPRESSEDTEXTUREIMAGEPROC));
            glGetTextureLevelParameterfv = (PFNGLGETTEXTURELEVELPARAMETERFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTextureLevelParameterfv)), typeof(PFNGLGETTEXTURELEVELPARAMETERFVPROC));
            glGetTextureLevelParameteriv = (PFNGLGETTEXTURELEVELPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTextureLevelParameteriv)), typeof(PFNGLGETTEXTURELEVELPARAMETERIVPROC));
            glGetTextureParameterfv = (PFNGLGETTEXTUREPARAMETERFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTextureParameterfv)), typeof(PFNGLGETTEXTUREPARAMETERFVPROC));
            glGetTextureParameterIiv = (PFNGLGETTEXTUREPARAMETERIIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTextureParameterIiv)), typeof(PFNGLGETTEXTUREPARAMETERIIVPROC));
            glGetTextureParameterIuiv = (PFNGLGETTEXTUREPARAMETERIUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTextureParameterIuiv)), typeof(PFNGLGETTEXTUREPARAMETERIUIVPROC));
            glGetTextureParameteriv = (PFNGLGETTEXTUREPARAMETERIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTextureParameteriv)), typeof(PFNGLGETTEXTUREPARAMETERIVPROC));
            glCreateVertexArrays = (PFNGLCREATEVERTEXARRAYSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCreateVertexArrays)), typeof(PFNGLCREATEVERTEXARRAYSPROC));
            glDisableVertexArrayAttrib = (PFNGLDISABLEVERTEXARRAYATTRIBPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glDisableVertexArrayAttrib)), typeof(PFNGLDISABLEVERTEXARRAYATTRIBPROC));
            glEnableVertexArrayAttrib = (PFNGLENABLEVERTEXARRAYATTRIBPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glEnableVertexArrayAttrib)), typeof(PFNGLENABLEVERTEXARRAYATTRIBPROC));
            glVertexArrayElementBuffer = (PFNGLVERTEXARRAYELEMENTBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexArrayElementBuffer)), typeof(PFNGLVERTEXARRAYELEMENTBUFFERPROC));
            glVertexArrayVertexBuffer = (PFNGLVERTEXARRAYVERTEXBUFFERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexArrayVertexBuffer)), typeof(PFNGLVERTEXARRAYVERTEXBUFFERPROC));
            glVertexArrayVertexBuffers = (PFNGLVERTEXARRAYVERTEXBUFFERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexArrayVertexBuffers)), typeof(PFNGLVERTEXARRAYVERTEXBUFFERSPROC));
            glVertexArrayAttribBinding = (PFNGLVERTEXARRAYATTRIBBINDINGPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexArrayAttribBinding)), typeof(PFNGLVERTEXARRAYATTRIBBINDINGPROC));
            glVertexArrayAttribFormat = (PFNGLVERTEXARRAYATTRIBFORMATPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexArrayAttribFormat)), typeof(PFNGLVERTEXARRAYATTRIBFORMATPROC));
            glVertexArrayAttribIFormat = (PFNGLVERTEXARRAYATTRIBIFORMATPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexArrayAttribIFormat)), typeof(PFNGLVERTEXARRAYATTRIBIFORMATPROC));
            glVertexArrayAttribLFormat = (PFNGLVERTEXARRAYATTRIBLFORMATPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexArrayAttribLFormat)), typeof(PFNGLVERTEXARRAYATTRIBLFORMATPROC));
            glVertexArrayBindingDivisor = (PFNGLVERTEXARRAYBINDINGDIVISORPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glVertexArrayBindingDivisor)), typeof(PFNGLVERTEXARRAYBINDINGDIVISORPROC));
            glGetVertexArrayiv = (PFNGLGETVERTEXARRAYIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetVertexArrayiv)), typeof(PFNGLGETVERTEXARRAYIVPROC));
            glGetVertexArrayIndexediv = (PFNGLGETVERTEXARRAYINDEXEDIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetVertexArrayIndexediv)), typeof(PFNGLGETVERTEXARRAYINDEXEDIVPROC));
            glGetVertexArrayIndexed64iv = (PFNGLGETVERTEXARRAYINDEXED64IVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetVertexArrayIndexed64iv)), typeof(PFNGLGETVERTEXARRAYINDEXED64IVPROC));
            glCreateSamplers = (PFNGLCREATESAMPLERSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCreateSamplers)), typeof(PFNGLCREATESAMPLERSPROC));
            glCreateProgramPipelines = (PFNGLCREATEPROGRAMPIPELINESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCreateProgramPipelines)), typeof(PFNGLCREATEPROGRAMPIPELINESPROC));
            glCreateQueries = (PFNGLCREATEQUERIESPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glCreateQueries)), typeof(PFNGLCREATEQUERIESPROC));
            glGetQueryBufferObjecti64v = (PFNGLGETQUERYBUFFEROBJECTI64VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetQueryBufferObjecti64v)), typeof(PFNGLGETQUERYBUFFEROBJECTI64VPROC));
            glGetQueryBufferObjectiv = (PFNGLGETQUERYBUFFEROBJECTIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetQueryBufferObjectiv)), typeof(PFNGLGETQUERYBUFFEROBJECTIVPROC));
            glGetQueryBufferObjectui64v = (PFNGLGETQUERYBUFFEROBJECTUI64VPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetQueryBufferObjectui64v)), typeof(PFNGLGETQUERYBUFFEROBJECTUI64VPROC));
            glGetQueryBufferObjectuiv = (PFNGLGETQUERYBUFFEROBJECTUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetQueryBufferObjectuiv)), typeof(PFNGLGETQUERYBUFFEROBJECTUIVPROC));
            glMemoryBarrierByRegion = (PFNGLMEMORYBARRIERBYREGIONPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMemoryBarrierByRegion)), typeof(PFNGLMEMORYBARRIERBYREGIONPROC));
            glGetTextureSubImage = (PFNGLGETTEXTURESUBIMAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetTextureSubImage)), typeof(PFNGLGETTEXTURESUBIMAGEPROC));
            glGetCompressedTextureSubImage = (PFNGLGETCOMPRESSEDTEXTURESUBIMAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetCompressedTextureSubImage)), typeof(PFNGLGETCOMPRESSEDTEXTURESUBIMAGEPROC));
            glGetGraphicsResetStatus = (PFNGLGETGRAPHICSRESETSTATUSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetGraphicsResetStatus)), typeof(PFNGLGETGRAPHICSRESETSTATUSPROC));
            glGetnCompressedTexImage = (PFNGLGETNCOMPRESSEDTEXIMAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnCompressedTexImage)), typeof(PFNGLGETNCOMPRESSEDTEXIMAGEPROC));
            //failed to load
            /*
            glGetnTexImage = (PFNGLGETNTEXIMAGEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnTexImage)), typeof(PFNGLGETNTEXIMAGEPROC));
            glGetnUniformdv = (PFNGLGETNUNIFORMDVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnUniformdv)), typeof(PFNGLGETNUNIFORMDVPROC));
            /*
            glGetnUniformfv = (PFNGLGETNUNIFORMFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnUniformfv)), typeof(PFNGLGETNUNIFORMFVPROC));
            glGetnUniformiv = (PFNGLGETNUNIFORMIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnUniformiv)), typeof(PFNGLGETNUNIFORMIVPROC));
            glGetnUniformuiv = (PFNGLGETNUNIFORMUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnUniformuiv)), typeof(PFNGLGETNUNIFORMUIVPROC));
            glReadnPixels = (PFNGLREADNPIXELSPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glReadnPixels)), typeof(PFNGLREADNPIXELSPROC));
            glGetnMapdv = (PFNGLGETNMAPDVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnMapdv)), typeof(PFNGLGETNMAPDVPROC));
            glGetnMapfv = (PFNGLGETNMAPFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnMapfv)), typeof(PFNGLGETNMAPFVPROC));
            glGetnMapiv = (PFNGLGETNMAPIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnMapiv)), typeof(PFNGLGETNMAPIVPROC));
            glGetnPixelMapfv = (PFNGLGETNPIXELMAPFVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnPixelMapfv)), typeof(PFNGLGETNPIXELMAPFVPROC));
            glGetnPixelMapuiv = (PFNGLGETNPIXELMAPUIVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnPixelMapuiv)), typeof(PFNGLGETNPIXELMAPUIVPROC));
            glGetnPixelMapusv = (PFNGLGETNPIXELMAPUSVPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnPixelMapusv)), typeof(PFNGLGETNPIXELMAPUSVPROC));
            glGetnPolygonStipple = (PFNGLGETNPOLYGONSTIPPLEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnPolygonStipple)), typeof(PFNGLGETNPOLYGONSTIPPLEPROC));
            glGetnColorTable = (PFNGLGETNCOLORTABLEPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnColorTable)), typeof(PFNGLGETNCOLORTABLEPROC));
            glGetnConvolutionFilter = (PFNGLGETNCONVOLUTIONFILTERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnConvolutionFilter)), typeof(PFNGLGETNCONVOLUTIONFILTERPROC));
            glGetnSeparableFilter = (PFNGLGETNSEPARABLEFILTERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnSeparableFilter)), typeof(PFNGLGETNSEPARABLEFILTERPROC));
            glGetnHistogram = (PFNGLGETNHISTOGRAMPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnHistogram)), typeof(PFNGLGETNHISTOGRAMPROC));
            glGetnMinmax = (PFNGLGETNMINMAXPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glGetnMinmax)), typeof(PFNGLGETNMINMAXPROC));
            glTextureBarrier = (PFNGLTEXTUREBARRIERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glTextureBarrier)), typeof(PFNGLTEXTUREBARRIERPROC));
            /**/
        }

        internal static void LoadGL46(glLoader load)
        {
            /*
            glSpecializeShader = (PFNGLSPECIALIZESHADERPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glSpecializeShader)), typeof(PFNGLSPECIALIZESHADERPROC));
            glMultiDrawArraysIndirectCount = (PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiDrawArraysIndirectCount)), typeof(PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC));
            glMultiDrawElementsIndirectCount = (PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glMultiDrawElementsIndirectCount)), typeof(PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC));
            glPolygonOffsetClamp = (PFNGLPOLYGONOFFSETCLAMPPROC)Marshal.GetDelegateForFunctionPointer(load(nameof(glPolygonOffsetClamp)), typeof(PFNGLPOLYGONOFFSETCLAMPPROC));

            /**/

        }




    }




    /// <summary>
    /// Function handler to load an opengl function by serching its name.
    /// </summary>
    /// <param name="funcname">the name of the Opengl function</param>
    /// <returns>An intptr value representing the pointer to the function</returns>
    public delegate IntPtr glLoader(string name);

    public delegate void FrameBufferSizeCallback(
        IntPtr window, int width, int height);

    #region GLFW_dele

    internal unsafe delegate void GLFWwindowsizefun(GLFWwindow* window, int width, int height);
    internal unsafe delegate void GLFWwindowrefreshfun(GLFWwindow* window);
    internal unsafe delegate void GLFWwindowposfun(GLFWwindow* window, int xpos, int ypos);
    internal unsafe delegate void GLFWwindowmaximizefun(GLFWwindow* window, int maximized);
    internal unsafe delegate void GLFWwindowiconifyfun(GLFWwindow* window, int iconified);
    internal unsafe delegate void GLFWwindowfocusfun(GLFWwindow* window, int focused);
    internal unsafe delegate void GLFWwindowcontentscalefun(GLFWwindow* window, float xscale, float yscale);
    internal unsafe delegate void GLFWwindowclosefun(GLFWwindow* window);
    internal unsafe delegate void GLFWvkproc();
    internal unsafe delegate void GLFWscrollfun(GLFWwindow* window, double xoffset, double yoffset);
    internal unsafe delegate void GLFWmousebuttonfun(GLFWwindow* window, int button, int action, int mods);
    internal unsafe delegate void GLFWmonitorfun(GLFWmonitor* monitor, int monitorevent);
    internal unsafe delegate void GLFWkeyfun(GLFWwindow* window, int key, int scancode, int action, int mods);
    internal unsafe delegate void GLFWjoystickfun(int jid, int keyevent);
    public unsafe delegate void GLFWglproc();
    internal unsafe delegate void GLFWframebuffersizefun(IntPtr window, int width, int height); //window is the pointer to a glfwwindow instance
    internal unsafe delegate void GLFWerrorfun(int error_code, sbyte* description);
    internal unsafe delegate void GLFWdropfun(GLFWwindow* window, int path_count, sbyte*[] paths);
    internal unsafe delegate void GLFWcursorposfun(GLFWwindow* window, double xpos, double ypos);
    internal unsafe delegate void GLFWcursorenterfun(GLFWwindow* window, int entered);
    internal unsafe delegate void GLFWcharmodsfun(GLFWwindow* window, uint codepoint, int mods);
    internal unsafe delegate void GLFWcharfun(GLFWwindow* window, uint codepoint);


    #endregion

    #region glDEBUG

    internal unsafe delegate void GLDEBUGPROC(int source, int type, uint id, int severity, int length, sbyte* message, void* userParam);
    internal unsafe delegate void GLDEBUGPROCARB(int source, int type, uint id, int severity, int length, sbyte* message, void* userParam);
    internal unsafe delegate void GLDEBUGPROCKHR(int source, int type, uint id, int severity, int length, sbyte* message, void* userParam);
    internal unsafe delegate void GLDEBUGPROCAMD(uint id, int category, int severity, int length, sbyte* message, void* userParam);

    #endregion

    #region gl_1_0

    internal unsafe delegate void PFNGLCULLFACEPROC(int mode);
    internal unsafe delegate void PFNGLFRONTFACEPROC(int mode);
    internal unsafe delegate void PFNGLHINTPROC(int target, int mode);
    internal unsafe delegate void PFNGLLINEWIDTHPROC(float width);
    internal unsafe delegate void PFNGLPOINTSIZEPROC(float size);
    internal unsafe delegate void PFNGLPOLYGONMODEPROC(int face, int mode);
    internal unsafe delegate void PFNGLSCISSORPROC(int x, int y, int width, int height);
    internal unsafe delegate void PFNGLTEXPARAMETERFPROC(int target, int pname, float param);
    internal unsafe delegate void PFNGLTEXPARAMETERFVPROC(int target, int pname, float* parameters);
    internal unsafe delegate void PFNGLTEXPARAMETERIPROC(int target, int pname, int param);
    internal unsafe delegate void PFNGLTEXPARAMETERIVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLTEXIMAGE1DPROC(int target, int level, int internalformat, int width, int border, int format, int type, void* pixels);
    internal unsafe delegate void PFNGLTEXIMAGE2DPROC(int target, int level, int internalformat, int width, int height, int border, int format, int type, void* pixels);
    internal unsafe delegate void PFNGLDRAWBUFFERPROC(int buf);
    internal unsafe delegate void PFNGLCLEARPROC(uint mask);
    internal unsafe delegate void PFNGLCLEARCOLORPROC(float red, float green, float blue, float alpha);
    internal unsafe delegate void PFNGLCLEARSTENCILPROC(int s);
    internal unsafe delegate void PFNGLCLEARDEPTHPROC(double depth);
    internal unsafe delegate void PFNGLSTENCILMASKPROC(uint mask);
    internal unsafe delegate void PFNGLCOLORMASKPROC(bool red, bool green, bool blue, bool alpha);
    internal unsafe delegate void PFNGLDEPTHMASKPROC(bool flag);
    internal unsafe delegate void PFNGLDISABLEPROC(int cap);
    internal unsafe delegate void PFNGLENABLEPROC(int cap);
    internal unsafe delegate void PFNGLFINISHPROC();
    internal unsafe delegate void PFNGLFLUSHPROC();
    internal unsafe delegate void PFNGLBLENDFUNCPROC(int sfactor, int dfactor);
    internal unsafe delegate void PFNGLLOGICOPPROC(int opcode);
    internal unsafe delegate void PFNGLSTENCILFUNCPROC(int func, int reference, uint mask);
    internal unsafe delegate void PFNGLSTENCILOPPROC(int fail, int zfail, int zpass);
    internal unsafe delegate void PFNGLDEPTHFUNCPROC(int func);
    internal unsafe delegate void PFNGLPIXELSTOREFPROC(int pname, float param);
    internal unsafe delegate void PFNGLPIXELSTOREIPROC(int pname, int param);
    internal unsafe delegate void PFNGLREADBUFFERPROC(int src);
    internal unsafe delegate void PFNGLREADPIXELSPROC(int x, int y, int width, int height, int format, int type, void* pixels);
    internal unsafe delegate void PFNGLGETBOOLEANVPROC(int pname, bool* data);
    internal unsafe delegate void PFNGLGETDOUBLEVPROC(int pname, double* data);
    internal unsafe delegate int PFNGLGETERRORPROC();
    internal unsafe delegate void PFNGLGETFLOATVPROC(int pname, float* data);
    internal unsafe delegate void PFNGLGETINTEGERVPROC(int pname, int* data);
    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    internal unsafe delegate string PFNGLGETSTRINGPROC(int name);
    internal unsafe delegate void PFNGLGETTEXIMAGEPROC(int target, int level, int format, int type, void* pixels);
    internal unsafe delegate void PFNGLGETTEXPARAMETERFVPROC(int target, int pname, float* parameters);
    internal unsafe delegate void PFNGLGETTEXPARAMETERIVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETTEXLEVELPARAMETERFVPROC(int target, int level, int pname, float* parameters);
    internal unsafe delegate void PFNGLGETTEXLEVELPARAMETERIVPROC(int target, int level, int pname, int* parameters);
    internal unsafe delegate bool PFNGLISENABLEDPROC(int cap);
    internal unsafe delegate void PFNGLDEPTHRANGEPROC(double n, double f);
    internal unsafe delegate void PFNGLVIEWPORTPROC(int x, int y, int width, int height);

    #endregion

    #region gl_1_1


    internal unsafe delegate void PFNGLDRAWARRAYSPROC(int mode, int first, int count);
    internal unsafe delegate void PFNGLDRAWELEMENTSPROC(int mode, int count, int type, void* indices);
    internal unsafe delegate void PFNGLPOLYGONOFFSETPROC(float factor, float units);
    internal unsafe delegate void PFNGLCOPYTEXIMAGE1DPROC(int target, int level, int internalformat, int x, int y, int width, int border);
    internal unsafe delegate void PFNGLCOPYTEXIMAGE2DPROC(int target, int level, int internalformat, int x, int y, int width, int height, int border);
    internal unsafe delegate void PFNGLCOPYTEXSUBIMAGE1DPROC(int target, int level, int xoffset, int x, int y, int width);
    internal unsafe delegate void PFNGLCOPYTEXSUBIMAGE2DPROC(int target, int level, int xoffset, int yoffset, int x, int y, int width, int height);
    internal unsafe delegate void PFNGLTEXSUBIMAGE1DPROC(int target, int level, int xoffset, int width, int format, int type, void* pixels);
    internal unsafe delegate void PFNGLTEXSUBIMAGE2DPROC(int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, void* pixels);
    internal unsafe delegate void PFNGLBINDTEXTUREPROC(int target, uint texture);
    internal unsafe delegate void PFNGLDELETETEXTURESPROC(int n, uint* textures);
    internal unsafe delegate void PFNGLGENTEXTURESPROC(int n, uint* textures);
    internal unsafe delegate bool PFNGLISTEXTUREPROC(uint texture);

    #endregion

    #region gl_1_2

    internal unsafe delegate void PFNGLDRAWRANGEELEMENTSPROC(int mode, uint start, uint end, int count, int type, void* indices);
    internal unsafe delegate void PFNGLTEXIMAGE3DPROC(int target, int level, int internalformat, int width, int height, int depth, int border, int format, int type, void* pixels);
    internal unsafe delegate void PFNGLTEXSUBIMAGE3DPROC(int target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, void* pixels);
    internal unsafe delegate void PFNGLCOPYTEXSUBIMAGE3DPROC(int target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);

    #endregion

    #region gl_1_3

    internal unsafe delegate void PFNGLACTIVETEXTUREPROC(int texture);
    internal unsafe delegate void PFNGLSAMPLECOVERAGEPROC(float value, bool invert);
    internal unsafe delegate void PFNGLCOMPRESSEDTEXIMAGE3DPROC(int target, int level, int internalformat, int width, int height, int depth, int border, int imageSize, void* data);
    internal unsafe delegate void PFNGLCOMPRESSEDTEXIMAGE2DPROC(int target, int level, int internalformat, int width, int height, int border, int imageSize, void* data);
    internal unsafe delegate void PFNGLCOMPRESSEDTEXIMAGE1DPROC(int target, int level, int internalformat, int width, int border, int imageSize, void* data);
    internal unsafe delegate void PFNGLCOMPRESSEDTEXSUBIMAGE3DPROC(int target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int imageSize, void* data);
    internal unsafe delegate void PFNGLCOMPRESSEDTEXSUBIMAGE2DPROC(int target, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, void* data);
    internal unsafe delegate void PFNGLCOMPRESSEDTEXSUBIMAGE1DPROC(int target, int level, int xoffset, int width, int format, int imageSize, void* data);
    internal unsafe delegate void PFNGLGETCOMPRESSEDTEXIMAGEPROC(int target, int level, void* img);

    #endregion

    #region gl_1_4

    internal unsafe delegate void PFNGLBLENDFUNCSEPARATEPROC(int sfactorRGB, int dfactorRGB, int sfactorAlpha, int dfactorAlpha);
    internal unsafe delegate void PFNGLMULTIDRAWARRAYSPROC(int mode, int* first, int* count, int drawcount);
    internal unsafe delegate void PFNGLMULTIDRAWELEMENTSPROC(int mode, int* count, int type, void** indices, int drawcount);
    internal unsafe delegate void PFNGLPOINTPARAMETERFPROC(int pname, float param);
    internal unsafe delegate void PFNGLPOINTPARAMETERFVPROC(int pname, float* parameters);
    internal unsafe delegate void PFNGLPOINTPARAMETERIPROC(int pname, int param);
    internal unsafe delegate void PFNGLPOINTPARAMETERIVPROC(int pname, int* parameters);
    internal unsafe delegate void PFNGLBLENDCOLORPROC(float red, float green, float blue, float alpha);
    internal unsafe delegate void PFNGLBLENDEQUATIONPROC(int mode);

    #endregion

    #region gl_1_5

    internal unsafe delegate void PFNGLGENQUERIESPROC(int n, uint* ids);
    internal unsafe delegate void PFNGLDELETEQUERIESPROC(int n, uint* ids);
    internal unsafe delegate bool PFNGLISQUERYPROC(uint id);
    internal unsafe delegate void PFNGLBEGINQUERYPROC(int target, uint id);
    internal unsafe delegate void PFNGLENDQUERYPROC(int target);
    internal unsafe delegate void PFNGLGETQUERYIVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETQUERYOBJECTIVPROC(uint id, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETQUERYOBJECTUIVPROC(uint id, int pname, uint* parameters);
    internal unsafe delegate void PFNGLBINDBUFFERPROC(int target, uint buffer);
    internal unsafe delegate void PFNGLDELETEBUFFERSPROC(int n, uint* buffers);
    internal unsafe delegate void PFNGLGENBUFFERSPROC(int n, uint* buffers);
    internal unsafe delegate bool PFNGLISBUFFERPROC(uint buffer);
    internal unsafe delegate void PFNGLBUFFERDATAPROC(int target, int* size, void* data, int usage);
    internal unsafe delegate void PFNGLBUFFERSUBDATAPROC(int target, long offset, int* size, void* data);
    internal unsafe delegate void PFNGLGETBUFFERSUBDATAPROC(int target, long offset, int* size, void* data);
    internal unsafe delegate void PFNGLMAPBUFFERPROC(int target, int access);
    internal unsafe delegate bool PFNGLUNMAPBUFFERPROC(int target);
    internal unsafe delegate void PFNGLGETBUFFERPARAMETERIVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETBUFFERPOINTERVPROC(int target, int pname, void** parameters);

    #endregion

    #region gl_2_0

    internal unsafe delegate void PFNGLBLENDEQUATIONSEPARATEPROC(int modeRGB, int modeAlpha);
    internal unsafe delegate void PFNGLDRAWBUFFERSPROC(int n, int* bufs);
    internal unsafe delegate void PFNGLSTENCILOPSEPARATEPROC(int face, int sfail, int dpfail, int dppass);
    internal unsafe delegate void PFNGLSTENCILFUNCSEPARATEPROC(int face, int func, int reference, uint mask);
    internal unsafe delegate void PFNGLSTENCILMASKSEPARATEPROC(int face, uint mask);
    internal unsafe delegate void PFNGLATTACHSHADERPROC(uint program, uint shader);
    internal unsafe delegate void PFNGLBINDATTRIBLOCATIONPROC(uint program, uint index, sbyte* name);
    internal unsafe delegate void PFNGLCOMPILESHADERPROC(uint shader);
    internal unsafe delegate uint PFNGLCREATEPROGRAMPROC();
    internal unsafe delegate uint PFNGLCREATESHADERPROC(int type);
    internal unsafe delegate void PFNGLDELETEPROGRAMPROC(uint program);
    internal unsafe delegate void PFNGLDELETESHADERPROC(uint shader);
    internal unsafe delegate void PFNGLDETACHSHADERPROC(uint program, uint shader);
    internal unsafe delegate void PFNGLDISABLEVERTEXATTRIBARRAYPROC(uint index);
    internal unsafe delegate void PFNGLENABLEVERTEXATTRIBARRAYPROC(uint index);
    internal unsafe delegate void PFNGLGETACTIVEATTRIBPROC(uint program, uint index, int bufSize, int* length, int* size, int* type, sbyte* name);
    internal unsafe delegate void PFNGLGETACTIVEUNIFORMPROC(uint program, uint index, int bufSize, int* length, int* size, int* type, sbyte* name);
    internal unsafe delegate void PFNGLGETATTACHEDSHADERSPROC(uint program, int maxCount, int* count, uint* shaders);
    internal unsafe delegate int PFNGLGETATTRIBLOCATIONPROC(uint program, sbyte* name);
    internal unsafe delegate void PFNGLGETPROGRAMIVPROC(uint program, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETPROGRAMINFOLOGPROC(uint program, int bufSize, int* length, sbyte* infoLog);
    internal unsafe delegate void PFNGLGETSHADERIVPROC(uint shader, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETSHADERINFOLOGPROC(uint shader, int bufSize, int* length, sbyte* infoLog);
    internal unsafe delegate void PFNGLGETSHADERSOURCEPROC(uint shader, int bufSize, int* length, sbyte* source);
    internal unsafe delegate int PFNGLGETUNIFORMLOCATIONPROC(uint program, sbyte* name);
    internal unsafe delegate void PFNGLGETUNIFORMFVPROC(uint program, int location, float* parameters);
    internal unsafe delegate void PFNGLGETUNIFORMIVPROC(uint program, int location, int* parameters);
    internal unsafe delegate void PFNGLGETVERTEXATTRIBDVPROC(uint index, int pname, double* parameters);
    internal unsafe delegate void PFNGLGETVERTEXATTRIBFVPROC(uint index, int pname, float* parameters);
    internal unsafe delegate void PFNGLGETVERTEXATTRIBIVPROC(uint index, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETVERTEXATTRIBPOINTERVPROC(uint index, int pname, void** pointer);
    internal unsafe delegate bool PFNGLISPROGRAMPROC(uint program);
    internal unsafe delegate bool PFNGLISSHADERPROC(uint shader);
    internal unsafe delegate void PFNGLLINKPROGRAMPROC(uint program);
    internal unsafe delegate void PFNGLSHADERSOURCEPROC(uint shader, int count, sbyte** str, int* length);
    internal unsafe delegate void PFNGLUSEPROGRAMPROC(uint program);
    internal unsafe delegate void PFNGLUNIFORM1FPROC(int location, float v0);
    internal unsafe delegate void PFNGLUNIFORM2FPROC(int location, float v0, float v1);
    internal unsafe delegate void PFNGLUNIFORM3FPROC(int location, float v0, float v1, float v2);
    internal unsafe delegate void PFNGLUNIFORM4FPROC(int location, float v0, float v1, float v2, float v3);
    internal unsafe delegate void PFNGLUNIFORM1IPROC(int location, int v0);
    internal unsafe delegate void PFNGLUNIFORM2IPROC(int location, int v0, int v1);
    internal unsafe delegate void PFNGLUNIFORM3IPROC(int location, int v0, int v1, int v2);
    internal unsafe delegate void PFNGLUNIFORM4IPROC(int location, int v0, int v1, int v2, int v3);
    internal unsafe delegate void PFNGLUNIFORM1FVPROC(int location, int count, float* value);
    internal unsafe delegate void PFNGLUNIFORM2FVPROC(int location, int count, float* value);
    internal unsafe delegate void PFNGLUNIFORM3FVPROC(int location, int count, float* value);
    internal unsafe delegate void PFNGLUNIFORM4FVPROC(int location, int count, float* value);
    internal unsafe delegate void PFNGLUNIFORM1IVPROC(int location, int count, int* value);
    internal unsafe delegate void PFNGLUNIFORM2IVPROC(int location, int count, int* value);
    internal unsafe delegate void PFNGLUNIFORM3IVPROC(int location, int count, int* value);
    internal unsafe delegate void PFNGLUNIFORM4IVPROC(int location, int count, int* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX2FVPROC(int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX3FVPROC(int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX4FVPROC(int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLVALIDATEPROGRAMPROC(uint program);
    internal unsafe delegate void PFNGLVERTEXATTRIB1DPROC(uint index, double x);
    internal unsafe delegate void PFNGLVERTEXATTRIB1DVPROC(uint index, double* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB1FPROC(uint index, float x);
    internal unsafe delegate void PFNGLVERTEXATTRIB1FVPROC(uint index, float* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB1SPROC(uint index, short x);
    internal unsafe delegate void PFNGLVERTEXATTRIB1SVPROC(uint index, short* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB2DPROC(uint index, double x, double y);
    internal unsafe delegate void PFNGLVERTEXATTRIB2DVPROC(uint index, double* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB2FPROC(uint index, float x, float y);
    internal unsafe delegate void PFNGLVERTEXATTRIB2FVPROC(uint index, float* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB2SPROC(uint index, short x, short y);
    internal unsafe delegate void PFNGLVERTEXATTRIB2SVPROC(uint index, short* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB3DPROC(uint index, double x, double y, double z);
    internal unsafe delegate void PFNGLVERTEXATTRIB3DVPROC(uint index, double* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB3FPROC(uint index, float x, float y, float z);
    internal unsafe delegate void PFNGLVERTEXATTRIB3FVPROC(uint index, float* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB3SPROC(uint index, short x, short y, short z);
    internal unsafe delegate void PFNGLVERTEXATTRIB3SVPROC(uint index, short* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4NBVPROC(uint index, byte* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4NIVPROC(uint index, int* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4NSVPROC(uint index, short* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4NUBPROC(uint index, byte x, byte y, byte z, byte w);
    internal unsafe delegate void PFNGLVERTEXATTRIB4NUBVPROC(uint index, byte* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4NUIVPROC(uint index, uint* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4NUSVPROC(uint index, ushort* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4BVPROC(uint index, byte* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4DPROC(uint index, double x, double y, double z, double w);
    internal unsafe delegate void PFNGLVERTEXATTRIB4DVPROC(uint index, double* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4FPROC(uint index, float x, float y, float z, float w);
    internal unsafe delegate void PFNGLVERTEXATTRIB4FVPROC(uint index, float* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4IVPROC(uint index, int* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4SPROC(uint index, short x, short y, short z, short w);
    internal unsafe delegate void PFNGLVERTEXATTRIB4SVPROC(uint index, short* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4UBVPROC(uint index, byte* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4UIVPROC(uint index, uint* v);
    internal unsafe delegate void PFNGLVERTEXATTRIB4USVPROC(uint index, ushort* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBPOINTERPROC(uint index, int size, int type, bool normalized, int stride, void* pointer);

    #endregion

    #region gl_2_1

    internal unsafe delegate void PFNGLUNIFORMMATRIX2X3FVPROC(int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX3X2FVPROC(int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX2X4FVPROC(int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX4X2FVPROC(int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX3X4FVPROC(int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX4X3FVPROC(int location, int count, bool transpose, float* value);

    #endregion

    #region gl_3_0

    internal unsafe delegate void PFNGLCOLORMASKIPROC(uint index, bool r, bool g, bool b, bool a);
    internal unsafe delegate void PFNGLGETBOOLEANI_VPROC(int target, uint index, bool* data);
    internal unsafe delegate void PFNGLGETINTEGERI_VPROC(int target, uint index, int* data);
    internal unsafe delegate void PFNGLENABLEIPROC(int target, uint index);
    internal unsafe delegate void PFNGLDISABLEIPROC(int target, uint index);
    internal unsafe delegate bool PFNGLISENABLEDIPROC(int target, uint index);
    internal unsafe delegate void PFNGLBEGINTRANSFORMFEEDBACKPROC(int primitiveMode);
    internal unsafe delegate void PFNGLENDTRANSFORMFEEDBACKPROC();
    internal unsafe delegate void PFNGLBINDBUFFERRANGEPROC(int target, uint index, uint buffer, long offset, int* size);
    internal unsafe delegate void PFNGLBINDBUFFERBASEPROC(int target, uint index, uint buffer);
    internal unsafe delegate void PFNGLTRANSFORMFEEDBACKVARYINGSPROC(uint program, int count, sbyte** varyings, int bufferMode);
    internal unsafe delegate void PFNGLGETTRANSFORMFEEDBACKVARYINGPROC(uint program, uint index, int bufSize, int* length, int* size, int* type, sbyte* name);
    internal unsafe delegate void PFNGLCLAMPCOLORPROC(int target, int clamp);
    internal unsafe delegate void PFNGLBEGINCONDITIONALRENDERPROC(uint id, int mode);
    internal unsafe delegate void PFNGLENDCONDITIONALRENDERPROC();
    internal unsafe delegate void PFNGLVERTEXATTRIBIPOINTERPROC(uint index, int size, int type, int stride, void* pointer);
    internal unsafe delegate void PFNGLGETVERTEXATTRIBIIVPROC(uint index, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETVERTEXATTRIBIUIVPROC(uint index, int pname, uint* parameters);
    internal unsafe delegate void PFNGLVERTEXATTRIBI1IPROC(uint index, int x);
    internal unsafe delegate void PFNGLVERTEXATTRIBI2IPROC(uint index, int x, int y);
    internal unsafe delegate void PFNGLVERTEXATTRIBI3IPROC(uint index, int x, int y, int z);
    internal unsafe delegate void PFNGLVERTEXATTRIBI4IPROC(uint index, int x, int y, int z, int w);
    internal unsafe delegate void PFNGLVERTEXATTRIBI1UIPROC(uint index, uint x);
    internal unsafe delegate void PFNGLVERTEXATTRIBI2UIPROC(uint index, uint x, uint y);
    internal unsafe delegate void PFNGLVERTEXATTRIBI3UIPROC(uint index, uint x, uint y, uint z);
    internal unsafe delegate void PFNGLVERTEXATTRIBI4UIPROC(uint index, uint x, uint y, uint z, uint w);
    internal unsafe delegate void PFNGLVERTEXATTRIBI1IVPROC(uint index, int* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBI2IVPROC(uint index, int* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBI3IVPROC(uint index, int* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBI4IVPROC(uint index, int* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBI1UIVPROC(uint index, uint* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBI2UIVPROC(uint index, uint* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBI3UIVPROC(uint index, uint* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBI4UIVPROC(uint index, uint* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBI4BVPROC(uint index, byte* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBI4SVPROC(uint index, short* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBI4UBVPROC(uint index, byte* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBI4USVPROC(uint index, ushort* v);
    internal unsafe delegate void PFNGLGETUNIFORMUIVPROC(uint program, int location, uint* parameters);
    internal unsafe delegate void PFNGLBINDFRAGDATALOCATIONPROC(uint program, uint color, sbyte* name);
    internal unsafe delegate int PFNGLGETFRAGDATALOCATIONPROC(uint program, sbyte* name);
    internal unsafe delegate void PFNGLUNIFORM1UIPROC(int location, uint v0);
    internal unsafe delegate void PFNGLUNIFORM2UIPROC(int location, uint v0, uint v1);
    internal unsafe delegate void PFNGLUNIFORM3UIPROC(int location, uint v0, uint v1, uint v2);
    internal unsafe delegate void PFNGLUNIFORM4UIPROC(int location, uint v0, uint v1, uint v2, uint v3);
    internal unsafe delegate void PFNGLUNIFORM1UIVPROC(int location, int count, uint* value);
    internal unsafe delegate void PFNGLUNIFORM2UIVPROC(int location, int count, uint* value);
    internal unsafe delegate void PFNGLUNIFORM3UIVPROC(int location, int count, uint* value);
    internal unsafe delegate void PFNGLUNIFORM4UIVPROC(int location, int count, uint* value);
    internal unsafe delegate void PFNGLTEXPARAMETERIIVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLTEXPARAMETERIUIVPROC(int target, int pname, uint* parameters);
    internal unsafe delegate void PFNGLGETTEXPARAMETERIIVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETTEXPARAMETERIUIVPROC(int target, int pname, uint* parameters);
    internal unsafe delegate void PFNGLCLEARBUFFERIVPROC(int buffer, int drawbuffer, int* value);
    internal unsafe delegate void PFNGLCLEARBUFFERUIVPROC(int buffer, int drawbuffer, uint* value);
    internal unsafe delegate void PFNGLCLEARBUFFERFVPROC(int buffer, int drawbuffer, float* value);
    internal unsafe delegate void PFNGLCLEARBUFFERFIPROC(int buffer, int drawbuffer, float depth, int stencil);
    internal unsafe delegate byte PFNGLGETSTRINGIPROC(int name, uint index);
    internal unsafe delegate bool PFNGLISRENDERBUFFERPROC(uint renderbuffer);
    internal unsafe delegate void PFNGLBINDRENDERBUFFERPROC(int target, uint renderbuffer);
    internal unsafe delegate void PFNGLDELETERENDERBUFFERSPROC(int n, uint* renderbuffers);
    internal unsafe delegate void PFNGLGENRENDERBUFFERSPROC(int n, uint* renderbuffers);
    internal unsafe delegate void PFNGLRENDERBUFFERSTORAGEPROC(int target, int internalformat, int width, int height);
    internal unsafe delegate void PFNGLGETRENDERBUFFERPARAMETERIVPROC(int target, int pname, int* parameters);
    internal unsafe delegate bool PFNGLISFRAMEBUFFERPROC(uint framebuffer);
    internal unsafe delegate void PFNGLBINDFRAMEBUFFERPROC(int target, uint framebuffer);
    internal unsafe delegate void PFNGLDELETEFRAMEBUFFERSPROC(int n, uint* framebuffers);
    internal unsafe delegate void PFNGLGENFRAMEBUFFERSPROC(int n, uint* framebuffers);
    internal unsafe delegate int PFNGLCHECKFRAMEBUFFERSTATUSPROC(int target);
    internal unsafe delegate void PFNGLFRAMEBUFFERTEXTURE1DPROC(int target, int attachment, int textarget, uint texture, int level);
    internal unsafe delegate void PFNGLFRAMEBUFFERTEXTURE2DPROC(int target, int attachment, int textarget, uint texture, int level);
    internal unsafe delegate void PFNGLFRAMEBUFFERTEXTURE3DPROC(int target, int attachment, int textarget, uint texture, int level, int zoffset);
    internal unsafe delegate void PFNGLFRAMEBUFFERRENDERBUFFERPROC(int target, int attachment, int renderbuffertarget, uint renderbuffer);
    internal unsafe delegate void PFNGLGETFRAMEBUFFERATTACHMENTPARAMETERIVPROC(int target, int attachment, int pname, int* parameters);
    internal unsafe delegate void PFNGLGENERATEMIPMAPPROC(int target);
    internal unsafe delegate void PFNGLBLITFRAMEBUFFERPROC(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);
    internal unsafe delegate void PFNGLRENDERBUFFERSTORAGEMULTISAMPLEPROC(int target, int samples, int internalformat, int width, int height);
    internal unsafe delegate void PFNGLFRAMEBUFFERTEXTURELAYERPROC(int target, int attachment, uint texture, int level, int layer);
    internal unsafe delegate void* PFNGLMAPBUFFERRANGEPROC(int target, long offset, int* length, uint access);
    internal unsafe delegate void PFNGLFLUSHMAPPEDBUFFERRANGEPROC(int target, long offset, int* length);
    internal unsafe delegate void PFNGLBINDVERTEXARRAYPROC(uint array);
    internal unsafe delegate void PFNGLDELETEVERTEXARRAYSPROC(int n, uint* arrays);
    internal unsafe delegate void PFNGLGENVERTEXARRAYSPROC(int n, uint* arrays);
    internal unsafe delegate bool PFNGLISVERTEXARRAYPROC(uint array);

    #endregion

    #region gl_3_1

    internal unsafe delegate void PFNGLDRAWARRAYSINSTANCEDPROC(int mode, int first, int count, int instancecount);
    internal unsafe delegate void PFNGLDRAWELEMENTSINSTANCEDPROC(int mode, int count, int type, void* indices, int instancecount);
    internal unsafe delegate void PFNGLTEXBUFFERPROC(int target, int internalformat, uint buffer);
    internal unsafe delegate void PFNGLPRIMITIVERESTARTINDEXPROC(uint index);
    internal unsafe delegate void PFNGLCOPYBUFFERSUBDATAPROC(int readTarget, int writeTarget, long readOffset, long writeOffset, int* size);
    internal unsafe delegate void PFNGLGETUNIFORMINDICESPROC(uint program, int uniformCount, sbyte** uniformNames, uint* uniformIndices);
    internal unsafe delegate void PFNGLGETACTIVEUNIFORMSIVPROC(uint program, int uniformCount, uint* uniformIndices, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETACTIVEUNIFORMNAMEPROC(uint program, uint uniformIndex, int bufSize, int* length, sbyte* uniformName);
    internal unsafe delegate uint PFNGLGETUNIFORMBLOCKINDEXPROC(uint program, sbyte* uniformBlockName);
    internal unsafe delegate void PFNGLGETACTIVEUNIFORMBLOCKIVPROC(uint program, uint uniformBlockIndex, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETACTIVEUNIFORMBLOCKNAMEPROC(uint program, uint uniformBlockIndex, int bufSize, int* length, sbyte* uniformBlockName);
    internal unsafe delegate void PFNGLUNIFORMBLOCKBINDINGPROC(uint program, uint uniformBlockIndex, uint uniformBlockBinding);

    #endregion

    #region gl_3_2

    internal unsafe delegate void PFNGLDRAWELEMENTSBASEVERTEXPROC(int mode, int count, int type, void* indices, int basevertex);
    internal unsafe delegate void PFNGLDRAWRANGEELEMENTSBASEVERTEXPROC(int mode, uint start, uint end, int count, int type, void* indices, int basevertex);
    internal unsafe delegate void PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXPROC(int mode, int count, int type, void* indices, int instancecount, int basevertex);
    internal unsafe delegate void PFNGLMULTIDRAWELEMENTSBASEVERTEXPROC(int mode, int* count, int type, void** indices, int drawcount, int* basevertex);
    internal unsafe delegate void PFNGLPROVOKINGVERTEXPROC(int mode);
    internal unsafe delegate GLsync* PFNGLFENCESYNCPROC(int condition, uint flags);
    internal unsafe delegate bool PFNGLISSYNCPROC(GLsync* sync);
    internal unsafe delegate void PFNGLDELETESYNCPROC(GLsync* sync);
    internal unsafe delegate int PFNGLCLIENTWAITSYNCPROC(GLsync* sync, uint flags, ulong timeout);
    internal unsafe delegate void PFNGLWAITSYNCPROC(GLsync* sync, uint flags, ulong timeout);
    internal unsafe delegate void PFNGLGETINTEGER64VPROC(int pname, long* data);
    internal unsafe delegate void PFNGLGETSYNCIVPROC(GLsync* sync, int pname, int count, int* length, int* values);
    internal unsafe delegate void PFNGLGETINTEGER64I_VPROC(int target, uint index, long* data);
    internal unsafe delegate void PFNGLGETBUFFERPARAMETERI64VPROC(int target, int pname, long* parameters);
    internal unsafe delegate void PFNGLFRAMEBUFFERTEXTUREPROC(int target, int attachment, uint texture, int level);
    internal unsafe delegate void PFNGLTEXIMAGE2DMULTISAMPLEPROC(int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations);
    internal unsafe delegate void PFNGLTEXIMAGE3DMULTISAMPLEPROC(int target, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations);
    internal unsafe delegate void PFNGLGETMULTISAMPLEFVPROC(int pname, uint index, float* val);
    internal unsafe delegate void PFNGLSAMPLEMASKIPROC(uint maskNumber, uint mask);

    #endregion

    #region gl_3_3

    internal unsafe delegate void PFNGLBINDFRAGDATALOCATIONINDEXEDPROC(uint program, uint colorNumber, uint index, sbyte* name);
    internal unsafe delegate int PFNGLGETFRAGDATAINDEXPROC(uint program, sbyte* name);
    internal unsafe delegate void PFNGLGENSAMPLERSPROC(int count, uint* samplers);
    internal unsafe delegate void PFNGLDELETESAMPLERSPROC(int count, uint* samplers);
    internal unsafe delegate bool PFNGLISSAMPLERPROC(uint sampler);
    internal unsafe delegate void PFNGLBINDSAMPLERPROC(uint unit, uint sampler);
    internal unsafe delegate void PFNGLSAMPLERPARAMETERIPROC(uint sampler, int pname, int param);
    internal unsafe delegate void PFNGLSAMPLERPARAMETERIVPROC(uint sampler, int pname, int* param);
    internal unsafe delegate void PFNGLSAMPLERPARAMETERFPROC(uint sampler, int pname, float param);
    internal unsafe delegate void PFNGLSAMPLERPARAMETERFVPROC(uint sampler, int pname, float* param);
    internal unsafe delegate void PFNGLSAMPLERPARAMETERIIVPROC(uint sampler, int pname, int* param);
    internal unsafe delegate void PFNGLSAMPLERPARAMETERIUIVPROC(uint sampler, int pname, uint* param);
    internal unsafe delegate void PFNGLGETSAMPLERPARAMETERIVPROC(uint sampler, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETSAMPLERPARAMETERIIVPROC(uint sampler, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETSAMPLERPARAMETERFVPROC(uint sampler, int pname, float* parameters);
    internal unsafe delegate void PFNGLGETSAMPLERPARAMETERIUIVPROC(uint sampler, int pname, uint* parameters);
    internal unsafe delegate void PFNGLQUERYCOUNTERPROC(uint id, int target);
    internal unsafe delegate void PFNGLGETQUERYOBJECTI64VPROC(uint id, int pname, long* parameters);
    internal unsafe delegate void PFNGLGETQUERYOBJECTUI64VPROC(uint id, int pname, ulong* parameters);
    internal unsafe delegate void PFNGLVERTEXATTRIBDIVISORPROC(uint index, uint divisor);
    internal unsafe delegate void PFNGLVERTEXATTRIBP1UIPROC(uint index, int type, bool normalized, uint value);
    internal unsafe delegate void PFNGLVERTEXATTRIBP1UIVPROC(uint index, int type, bool normalized, uint* value);
    internal unsafe delegate void PFNGLVERTEXATTRIBP2UIPROC(uint index, int type, bool normalized, uint value);
    internal unsafe delegate void PFNGLVERTEXATTRIBP2UIVPROC(uint index, int type, bool normalized, uint* value);
    internal unsafe delegate void PFNGLVERTEXATTRIBP3UIPROC(uint index, int type, bool normalized, uint value);
    internal unsafe delegate void PFNGLVERTEXATTRIBP3UIVPROC(uint index, int type, bool normalized, uint* value);
    internal unsafe delegate void PFNGLVERTEXATTRIBP4UIPROC(uint index, int type, bool normalized, uint value);
    internal unsafe delegate void PFNGLVERTEXATTRIBP4UIVPROC(uint index, int type, bool normalized, uint* value);
    internal unsafe delegate void PFNGLVERTEXP2UIPROC(int type, uint value);
    internal unsafe delegate void PFNGLVERTEXP2UIVPROC(int type, uint* value);
    internal unsafe delegate void PFNGLVERTEXP3UIPROC(int type, uint value);
    internal unsafe delegate void PFNGLVERTEXP3UIVPROC(int type, uint* value);
    internal unsafe delegate void PFNGLVERTEXP4UIPROC(int type, uint value);
    internal unsafe delegate void PFNGLVERTEXP4UIVPROC(int type, uint* value);
    internal unsafe delegate void PFNGLTEXCOORDP1UIPROC(int type, uint coords);
    internal unsafe delegate void PFNGLTEXCOORDP1UIVPROC(int type, uint* coords);
    internal unsafe delegate void PFNGLTEXCOORDP2UIPROC(int type, uint coords);
    internal unsafe delegate void PFNGLTEXCOORDP2UIVPROC(int type, uint* coords);
    internal unsafe delegate void PFNGLTEXCOORDP3UIPROC(int type, uint coords);
    internal unsafe delegate void PFNGLTEXCOORDP3UIVPROC(int type, uint* coords);
    internal unsafe delegate void PFNGLTEXCOORDP4UIPROC(int type, uint coords);
    internal unsafe delegate void PFNGLTEXCOORDP4UIVPROC(int type, uint* coords);
    internal unsafe delegate void PFNGLMULTITEXCOORDP1UIPROC(int texture, int type, uint coords);
    internal unsafe delegate void PFNGLMULTITEXCOORDP1UIVPROC(int texture, int type, uint* coords);
    internal unsafe delegate void PFNGLMULTITEXCOORDP2UIPROC(int texture, int type, uint coords);
    internal unsafe delegate void PFNGLMULTITEXCOORDP2UIVPROC(int texture, int type, uint* coords);
    internal unsafe delegate void PFNGLMULTITEXCOORDP3UIPROC(int texture, int type, uint coords);
    internal unsafe delegate void PFNGLMULTITEXCOORDP3UIVPROC(int texture, int type, uint* coords);
    internal unsafe delegate void PFNGLMULTITEXCOORDP4UIPROC(int texture, int type, uint coords);
    internal unsafe delegate void PFNGLMULTITEXCOORDP4UIVPROC(int texture, int type, uint* coords);
    internal unsafe delegate void PFNGLNORMALP3UIPROC(int type, uint coords);
    internal unsafe delegate void PFNGLNORMALP3UIVPROC(int type, uint* coords);
    internal unsafe delegate void PFNGLCOLORP3UIPROC(int type, uint color);
    internal unsafe delegate void PFNGLCOLORP3UIVPROC(int type, uint* color);
    internal unsafe delegate void PFNGLCOLORP4UIPROC(int type, uint color);
    internal unsafe delegate void PFNGLCOLORP4UIVPROC(int type, uint* color);
    internal unsafe delegate void PFNGLSECONDARYCOLORP3UIPROC(int type, uint color);
    internal unsafe delegate void PFNGLSECONDARYCOLORP3UIVPROC(int type, uint* color);

    #endregion

    #region gl_4_0

    internal unsafe delegate void PFNGLMINSAMPLESHADINGPROC(float value);
    internal unsafe delegate void PFNGLBLENDEQUATIONIPROC(uint buf, int mode);
    internal unsafe delegate void PFNGLBLENDEQUATIONSEPARATEIPROC(uint buf, int modeRGB, int modeAlpha);
    internal unsafe delegate void PFNGLBLENDFUNCIPROC(uint buf, int src, int dst);
    internal unsafe delegate void PFNGLBLENDFUNCSEPARATEIPROC(uint buf, int srcRGB, int dstRGB, int srcAlpha, int dstAlpha);
    internal unsafe delegate void PFNGLDRAWARRAYSINDIRECTPROC(int mode, void* indirect);
    internal unsafe delegate void PFNGLDRAWELEMENTSINDIRECTPROC(int mode, int type, void* indirect);
    internal unsafe delegate void PFNGLUNIFORM1DPROC(int location, double x);
    internal unsafe delegate void PFNGLUNIFORM2DPROC(int location, double x, double y);
    internal unsafe delegate void PFNGLUNIFORM3DPROC(int location, double x, double y, double z);
    internal unsafe delegate void PFNGLUNIFORM4DPROC(int location, double x, double y, double z, double w);
    internal unsafe delegate void PFNGLUNIFORM1DVPROC(int location, int count, double* value);
    internal unsafe delegate void PFNGLUNIFORM2DVPROC(int location, int count, double* value);
    internal unsafe delegate void PFNGLUNIFORM3DVPROC(int location, int count, double* value);
    internal unsafe delegate void PFNGLUNIFORM4DVPROC(int location, int count, double* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX2DVPROC(int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX3DVPROC(int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX4DVPROC(int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX2X3DVPROC(int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX2X4DVPROC(int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX3X2DVPROC(int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX3X4DVPROC(int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX4X2DVPROC(int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLUNIFORMMATRIX4X3DVPROC(int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLGETUNIFORMDVPROC(uint program, int location, double* parameters);
    internal unsafe delegate int PFNGLGETSUBROUTINEUNIFORMLOCATIONPROC(uint program, int shadertype, sbyte* name);
    internal unsafe delegate uint PFNGLGETSUBROUTINEINDEXPROC(uint program, int shadertype, sbyte* name);
    internal unsafe delegate void PFNGLGETACTIVESUBROUTINEUNIFORMIVPROC(uint program, int shadertype, uint index, int pname, int* values);
    internal unsafe delegate void PFNGLGETACTIVESUBROUTINEUNIFORMNAMEPROC(uint program, int shadertype, uint index, int bufSize, int* length, sbyte* name);
    internal unsafe delegate void PFNGLGETACTIVESUBROUTINENAMEPROC(uint program, int shadertype, uint index, int bufSize, int* length, sbyte* name);
    internal unsafe delegate void PFNGLUNIFORMSUBROUTINESUIVPROC(int shadertype, int count, uint* indices);
    internal unsafe delegate void PFNGLGETUNIFORMSUBROUTINEUIVPROC(int shadertype, int location, uint* parameters);
    internal unsafe delegate void PFNGLGETPROGRAMSTAGEIVPROC(uint program, int shadertype, int pname, int* values);
    internal unsafe delegate void PFNGLPATCHPARAMETERIPROC(int pname, int value);
    internal unsafe delegate void PFNGLPATCHPARAMETERFVPROC(int pname, float* values);
    internal unsafe delegate void PFNGLBINDTRANSFORMFEEDBACKPROC(int target, uint id);
    internal unsafe delegate void PFNGLDELETETRANSFORMFEEDBACKSPROC(int n, uint* ids);
    internal unsafe delegate void PFNGLGENTRANSFORMFEEDBACKSPROC(int n, uint* ids);
    internal unsafe delegate bool PFNGLISTRANSFORMFEEDBACKPROC(uint id);
    internal unsafe delegate void PFNGLPAUSETRANSFORMFEEDBACKPROC();
    internal unsafe delegate void PFNGLRESUMETRANSFORMFEEDBACKPROC();
    internal unsafe delegate void PFNGLDRAWTRANSFORMFEEDBACKPROC(int mode, uint id);
    internal unsafe delegate void PFNGLDRAWTRANSFORMFEEDBACKSTREAMPROC(int mode, uint id, uint stream);
    internal unsafe delegate void PFNGLBEGINQUERYINDEXEDPROC(int target, uint index, uint id);
    internal unsafe delegate void PFNGLENDQUERYINDEXEDPROC(int target, uint index);
    internal unsafe delegate void PFNGLGETQUERYINDEXEDIVPROC(int target, uint index, int pname, int* parameters);

    #endregion

    #region gl_4_1

    internal unsafe delegate void PFNGLRELEASESHADERCOMPILERPROC();
    internal unsafe delegate void PFNGLSHADERBINARYPROC(int count, uint* shaders, int binaryFormat, void* binary, int length);
    internal unsafe delegate void PFNGLGETSHADERPRECISIONFORMATPROC(int shadertype, int precisiontype, int* range, int* precision);
    internal unsafe delegate void PFNGLDEPTHRANGEFPROC(float n, float f);
    internal unsafe delegate void PFNGLCLEARDEPTHFPROC(float d);
    internal unsafe delegate void PFNGLGETPROGRAMBINARYPROC(uint program, int bufSize, int* length, int* binaryFormat, void* binary);
    internal unsafe delegate void PFNGLPROGRAMBINARYPROC(uint program, int binaryFormat, void* binary, int length);
    internal unsafe delegate void PFNGLPROGRAMPARAMETERIPROC(uint program, int pname, int value);
    internal unsafe delegate void PFNGLUSEPROGRAMSTAGESPROC(uint pipeline, uint stages, uint program);
    internal unsafe delegate void PFNGLACTIVESHADERPROGRAMPROC(uint pipeline, uint program);
    internal unsafe delegate uint PFNGLCREATESHADERPROGRAMVPROC(int type, int count, sbyte** strings);
    internal unsafe delegate void PFNGLBINDPROGRAMPIPELINEPROC(uint pipeline);
    internal unsafe delegate void PFNGLDELETEPROGRAMPIPELINESPROC(int n, uint* pipelines);
    internal unsafe delegate void PFNGLGENPROGRAMPIPELINESPROC(int n, uint* pipelines);
    internal unsafe delegate bool PFNGLISPROGRAMPIPELINEPROC(uint pipeline);
    internal unsafe delegate void PFNGLGETPROGRAMPIPELINEIVPROC(uint pipeline, int pname, int* parameters);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM1IPROC(uint program, int location, int v0);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM1IVPROC(uint program, int location, int count, int* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM1FPROC(uint program, int location, float v0);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM1FVPROC(uint program, int location, int count, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM1DPROC(uint program, int location, double v0);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM1DVPROC(uint program, int location, int count, double* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM1UIPROC(uint program, int location, uint v0);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM1UIVPROC(uint program, int location, int count, uint* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM2IPROC(uint program, int location, int v0, int v1);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM2IVPROC(uint program, int location, int count, int* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM2FPROC(uint program, int location, float v0, float v1);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM2FVPROC(uint program, int location, int count, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM2DPROC(uint program, int location, double v0, double v1);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM2DVPROC(uint program, int location, int count, double* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM2UIPROC(uint program, int location, uint v0, uint v1);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM2UIVPROC(uint program, int location, int count, uint* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM3IPROC(uint program, int location, int v0, int v1, int v2);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM3IVPROC(uint program, int location, int count, int* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM3FPROC(uint program, int location, float v0, float v1, float v2);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM3FVPROC(uint program, int location, int count, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM3DPROC(uint program, int location, double v0, double v1, double v2);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM3DVPROC(uint program, int location, int count, double* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM3UIPROC(uint program, int location, uint v0, uint v1, uint v2);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM3UIVPROC(uint program, int location, int count, uint* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM4IPROC(uint program, int location, int v0, int v1, int v2, int v3);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM4IVPROC(uint program, int location, int count, int* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM4FPROC(uint program, int location, float v0, float v1, float v2, float v3);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM4FVPROC(uint program, int location, int count, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM4DPROC(uint program, int location, double v0, double v1, double v2, double v3);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM4DVPROC(uint program, int location, int count, double* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM4UIPROC(uint program, int location, uint v0, uint v1, uint v2, uint v3);
    internal unsafe delegate void PFNGLPROGRAMUNIFORM4UIVPROC(uint program, int location, int count, uint* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX2FVPROC(uint program, int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX3FVPROC(uint program, int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX4FVPROC(uint program, int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX2DVPROC(uint program, int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX3DVPROC(uint program, int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX4DVPROC(uint program, int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX2X3FVPROC(uint program, int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX3X2FVPROC(uint program, int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX2X4FVPROC(uint program, int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX4X2FVPROC(uint program, int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX3X4FVPROC(uint program, int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX4X3FVPROC(uint program, int location, int count, bool transpose, float* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX2X3DVPROC(uint program, int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX3X2DVPROC(uint program, int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX2X4DVPROC(uint program, int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX4X2DVPROC(uint program, int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX3X4DVPROC(uint program, int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLPROGRAMUNIFORMMATRIX4X3DVPROC(uint program, int location, int count, bool transpose, double* value);
    internal unsafe delegate void PFNGLVALIDATEPROGRAMPIPELINEPROC(uint pipeline);
    internal unsafe delegate void PFNGLGETPROGRAMPIPELINEINFOLOGPROC(uint pipeline, int bufSize, int* length, sbyte* infoLog);
    internal unsafe delegate void PFNGLVERTEXATTRIBL1DPROC(uint index, double x);
    internal unsafe delegate void PFNGLVERTEXATTRIBL2DPROC(uint index, double x, double y);
    internal unsafe delegate void PFNGLVERTEXATTRIBL3DPROC(uint index, double x, double y, double z);
    internal unsafe delegate void PFNGLVERTEXATTRIBL4DPROC(uint index, double x, double y, double z, double w);
    internal unsafe delegate void PFNGLVERTEXATTRIBL1DVPROC(uint index, double* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBL2DVPROC(uint index, double* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBL3DVPROC(uint index, double* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBL4DVPROC(uint index, double* v);
    internal unsafe delegate void PFNGLVERTEXATTRIBLPOINTERPROC(uint index, int size, int type, int stride, void* pointer);
    internal unsafe delegate void PFNGLGETVERTEXATTRIBLDVPROC(uint index, int pname, double* parameters);
    internal unsafe delegate void PFNGLVIEWPORTARRAYVPROC(uint first, int count, float* v);
    internal unsafe delegate void PFNGLVIEWPORTINDEXEDFPROC(uint index, float x, float y, float w, float h);
    internal unsafe delegate void PFNGLVIEWPORTINDEXEDFVPROC(uint index, float* v);
    internal unsafe delegate void PFNGLSCISSORARRAYVPROC(uint first, int count, int* v);
    internal unsafe delegate void PFNGLSCISSORINDEXEDPROC(uint index, int left, int bottom, int width, int height);
    internal unsafe delegate void PFNGLSCISSORINDEXEDVPROC(uint index, int* v);
    internal unsafe delegate void PFNGLDEPTHRANGEARRAYVPROC(uint first, int count, double* v);
    internal unsafe delegate void PFNGLDEPTHRANGEINDEXEDPROC(uint index, double n, double f);
    internal unsafe delegate void PFNGLGETFLOATI_VPROC(int target, uint index, float* data);
    internal unsafe delegate void PFNGLGETDOUBLEI_VPROC(int target, uint index, double* data);

    #endregion

    #region gl_4_2

    internal unsafe delegate void PFNGLDRAWARRAYSINSTANCEDBASEINSTANCEPROC(int mode, int first, int count, int instancecount, uint baseinstance);
    internal unsafe delegate void PFNGLDRAWELEMENTSINSTANCEDBASEINSTANCEPROC(int mode, int count, int type, void* indices, int instancecount, uint baseinstance);
    internal unsafe delegate void PFNGLDRAWELEMENTSINSTANCEDBASEVERTEXBASEINSTANCEPROC(int mode, int count, int type, void* indices, int instancecount, int basevertex, uint baseinstance);
    internal unsafe delegate void PFNGLGETINTERNALFORMATIVPROC(int target, int internalformat, int pname, int count, int* parameters);
    internal unsafe delegate void PFNGLGETACTIVEATOMICCOUNTERBUFFERIVPROC(uint program, uint bufferIndex, int pname, int* parameters);
    internal unsafe delegate void PFNGLBINDIMAGETEXTUREPROC(uint unit, uint texture, int level, bool layered, int layer, int access, int format);
    internal unsafe delegate void PFNGLMEMORYBARRIERPROC(uint barriers);
    internal unsafe delegate void PFNGLTEXSTORAGE1DPROC(int target, int levels, int internalformat, int width);
    internal unsafe delegate void PFNGLTEXSTORAGE2DPROC(int target, int levels, int internalformat, int width, int height);
    internal unsafe delegate void PFNGLTEXSTORAGE3DPROC(int target, int levels, int internalformat, int width, int height, int depth);
    internal unsafe delegate void PFNGLDRAWTRANSFORMFEEDBACKINSTANCEDPROC(int mode, uint id, int instancecount);
    internal unsafe delegate void PFNGLDRAWTRANSFORMFEEDBACKSTREAMINSTANCEDPROC(int mode, uint id, uint stream, int instancecount);

    #endregion

    #region gl_4_3

    internal unsafe delegate void PFNGLCLEARBUFFERDATAPROC(int target, int internalformat, int format, int type, void* data);
    internal unsafe delegate void PFNGLCLEARBUFFERSUBDATAPROC(int target, int internalformat, long offset, int* size, int format, int type, void* data);
    internal unsafe delegate void PFNGLDISPATCHCOMPUTEPROC(uint num_groups_x, uint num_groups_y, uint num_groups_z);
    internal unsafe delegate void PFNGLDISPATCHCOMPUTEINDIRECTPROC(long indirect);
    internal unsafe delegate void PFNGLCOPYIMAGESUBDATAPROC(uint srcName, int srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, int dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth);
    internal unsafe delegate void PFNGLFRAMEBUFFERPARAMETERIPROC(int target, int pname, int param);
    internal unsafe delegate void PFNGLGETFRAMEBUFFERPARAMETERIVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETINTERNALFORMATI64VPROC(int target, int internalformat, int pname, int count, long* parameters);
    internal unsafe delegate void PFNGLINVALIDATETEXSUBIMAGEPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth);
    internal unsafe delegate void PFNGLINVALIDATETEXIMAGEPROC(uint texture, int level);
    internal unsafe delegate void PFNGLINVALIDATEBUFFERSUBDATAPROC(uint buffer, long offset, int* length);
    internal unsafe delegate void PFNGLINVALIDATEBUFFERDATAPROC(uint buffer);
    internal unsafe delegate void PFNGLINVALIDATEFRAMEBUFFERPROC(int target, int numAttachments, int* attachments);
    internal unsafe delegate void PFNGLINVALIDATESUBFRAMEBUFFERPROC(int target, int numAttachments, int* attachments, int x, int y, int width, int height);
    internal unsafe delegate void PFNGLMULTIDRAWARRAYSINDIRECTPROC(int mode, void* indirect, int drawcount, int stride);
    internal unsafe delegate void PFNGLMULTIDRAWELEMENTSINDIRECTPROC(int mode, int type, void* indirect, int drawcount, int stride);
    internal unsafe delegate void PFNGLGETPROGRAMINTERFACEIVPROC(uint program, int programInterface, int pname, int* parameters);
    internal unsafe delegate uint PFNGLGETPROGRAMRESOURCEINDEXPROC(uint program, int programInterface, sbyte* name);
    internal unsafe delegate void PFNGLGETPROGRAMRESOURCENAMEPROC(uint program, int programInterface, uint index, int bufSize, int* length, sbyte* name);
    internal unsafe delegate void PFNGLGETPROGRAMRESOURCEIVPROC(uint program, int programInterface, uint index, int propCount, int* props, int count, int* length, int* parameters);
    internal unsafe delegate int PFNGLGETPROGRAMRESOURCELOCATIONPROC(uint program, int programInterface, sbyte* name);
    internal unsafe delegate int PFNGLGETPROGRAMRESOURCELOCATIONINDEXPROC(uint program, int programInterface, sbyte* name);
    internal unsafe delegate void PFNGLSHADERSTORAGEBLOCKBINDINGPROC(uint program, uint storageBlockIndex, uint storageBlockBinding);
    internal unsafe delegate void PFNGLTEXBUFFERRANGEPROC(int target, int internalformat, uint buffer, long offset, int* size);
    internal unsafe delegate void PFNGLTEXSTORAGE2DMULTISAMPLEPROC(int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations);
    internal unsafe delegate void PFNGLTEXSTORAGE3DMULTISAMPLEPROC(int target, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations);
    internal unsafe delegate void PFNGLTEXTUREVIEWPROC(uint texture, int target, uint origtexture, int internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers);
    internal unsafe delegate void PFNGLBINDVERTEXBUFFERPROC(uint bindingindex, uint buffer, long offset, int stride);
    internal unsafe delegate void PFNGLVERTEXATTRIBFORMATPROC(uint attribindex, int size, int type, bool normalized, uint relativeoffset);
    internal unsafe delegate void PFNGLVERTEXATTRIBIFORMATPROC(uint attribindex, int size, int type, uint relativeoffset);
    internal unsafe delegate void PFNGLVERTEXATTRIBLFORMATPROC(uint attribindex, int size, int type, uint relativeoffset);
    internal unsafe delegate void PFNGLVERTEXATTRIBBINDINGPROC(uint attribindex, uint bindingindex);
    internal unsafe delegate void PFNGLVERTEXBINDINGDIVISORPROC(uint bindingindex, uint divisor);
    internal unsafe delegate void PFNGLDEBUGMESSAGECONTROLPROC(int source, int type, int severity, int count, uint* ids, bool enabled);
    internal unsafe delegate void PFNGLDEBUGMESSAGEINSERTPROC(int source, int type, uint id, int severity, int length, sbyte* buf);
    internal unsafe delegate void PFNGLDEBUGMESSAGECALLBACKPROC(GLDEBUGPROC callback, void* userParam);
    internal unsafe delegate uint PFNGLGETDEBUGMESSAGELOGPROC(uint count, int bufSize, int* sources, int* types, uint* ids, int* severities, int* lengths, sbyte* messageLog);
    internal unsafe delegate void PFNGLPUSHDEBUGGROUPPROC(int source, uint id, int length, sbyte* message);
    internal unsafe delegate void PFNGLPOPDEBUGGROUPPROC();
    internal unsafe delegate void PFNGLOBJECTLABELPROC(int identifier, uint name, int length, sbyte* label);
    internal unsafe delegate void PFNGLGETOBJECTLABELPROC(int identifier, uint name, int bufSize, int* length, sbyte* label);
    internal unsafe delegate void PFNGLOBJECTPTRLABELPROC(void* ptr, int length, sbyte* label);
    internal unsafe delegate void PFNGLGETOBJECTPTRLABELPROC(void* ptr, int bufSize, int* length, sbyte* label);
    internal unsafe delegate void PFNGLGETPOINTERVPROC(int pname, void** parameters);

    #endregion

    #region gl_4_4

    internal unsafe delegate void PFNGLBUFFERSTORAGEPROC(int target, int* size, void* data, uint flags);
    internal unsafe delegate void PFNGLCLEARTEXIMAGEPROC(uint texture, int level, int format, int type, void* data);
    internal unsafe delegate void PFNGLCLEARTEXSUBIMAGEPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, void* data);
    internal unsafe delegate void PFNGLBINDBUFFERSBASEPROC(int target, uint first, int count, uint* buffers);
    internal unsafe delegate void PFNGLBINDBUFFERSRANGEPROC(int target, uint first, int count, uint* buffers, long* offsets, int** sizes);
    internal unsafe delegate void PFNGLBINDTEXTURESPROC(uint first, int count, uint* textures);
    internal unsafe delegate void PFNGLBINDSAMPLERSPROC(uint first, int count, uint* samplers);
    internal unsafe delegate void PFNGLBINDIMAGETEXTURESPROC(uint first, int count, uint* textures);
    internal unsafe delegate void PFNGLBINDVERTEXBUFFERSPROC(uint first, int count, uint* buffers, long* offsets, int* strides);

    #endregion

    #region gl_4_5

    internal unsafe delegate void PFNGLCLIPCONTROLPROC(int origin, int depth);
    internal unsafe delegate void PFNGLCREATETRANSFORMFEEDBACKSPROC(int n, uint* ids);
    internal unsafe delegate void PFNGLTRANSFORMFEEDBACKBUFFERBASEPROC(uint xfb, uint index, uint buffer);
    internal unsafe delegate void PFNGLTRANSFORMFEEDBACKBUFFERRANGEPROC(uint xfb, uint index, uint buffer, long offset, int* size);
    internal unsafe delegate void PFNGLGETTRANSFORMFEEDBACKIVPROC(uint xfb, int pname, int* param);
    internal unsafe delegate void PFNGLGETTRANSFORMFEEDBACKI_VPROC(uint xfb, int pname, uint index, int* param);
    internal unsafe delegate void PFNGLGETTRANSFORMFEEDBACKI64_VPROC(uint xfb, int pname, uint index, long* param);
    internal unsafe delegate void PFNGLCREATEBUFFERSPROC(int n, uint* buffers);
    internal unsafe delegate void PFNGLNAMEDBUFFERSTORAGEPROC(uint buffer, int* size, void* data, uint flags);
    internal unsafe delegate void PFNGLNAMEDBUFFERDATAPROC(uint buffer, int* size, void* data, int usage);
    internal unsafe delegate void PFNGLNAMEDBUFFERSUBDATAPROC(uint buffer, long offset, int* size, void* data);
    internal unsafe delegate void PFNGLCOPYNAMEDBUFFERSUBDATAPROC(uint readBuffer, uint writeBuffer, long readOffset, long writeOffset, int* size);
    internal unsafe delegate void PFNGLCLEARNAMEDBUFFERDATAPROC(uint buffer, int internalformat, int format, int type, void* data);
    internal unsafe delegate void PFNGLCLEARNAMEDBUFFERSUBDATAPROC(uint buffer, int internalformat, long offset, int* size, int format, int type, void* data);
    internal unsafe delegate void* PFNGLMAPNAMEDBUFFERPROC(uint buffer, int access);
    internal unsafe delegate void* PFNGLMAPNAMEDBUFFERRANGEPROC(uint buffer, long offset, int* length, uint access);
    internal unsafe delegate bool PFNGLUNMAPNAMEDBUFFERPROC(uint buffer);
    internal unsafe delegate void PFNGLFLUSHMAPPEDNAMEDBUFFERRANGEPROC(uint buffer, long offset, int* length);
    internal unsafe delegate void PFNGLGETNAMEDBUFFERPARAMETERIVPROC(uint buffer, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETNAMEDBUFFERPARAMETERI64VPROC(uint buffer, int pname, long* parameters);
    internal unsafe delegate void PFNGLGETNAMEDBUFFERPOINTERVPROC(uint buffer, int pname, void** parameters);
    internal unsafe delegate void PFNGLGETNAMEDBUFFERSUBDATAPROC(uint buffer, long offset, int* size, void* data);
    internal unsafe delegate void PFNGLCREATEFRAMEBUFFERSPROC(int n, uint* framebuffers);
    internal unsafe delegate void PFNGLNAMEDFRAMEBUFFERRENDERBUFFERPROC(uint framebuffer, int attachment, int renderbuffertarget, uint renderbuffer);
    internal unsafe delegate void PFNGLNAMEDFRAMEBUFFERPARAMETERIPROC(uint framebuffer, int pname, int param);
    internal unsafe delegate void PFNGLNAMEDFRAMEBUFFERTEXTUREPROC(uint framebuffer, int attachment, uint texture, int level);
    internal unsafe delegate void PFNGLNAMEDFRAMEBUFFERTEXTURELAYERPROC(uint framebuffer, int attachment, uint texture, int level, int layer);
    internal unsafe delegate void PFNGLNAMEDFRAMEBUFFERDRAWBUFFERPROC(uint framebuffer, int buf);
    internal unsafe delegate void PFNGLNAMEDFRAMEBUFFERDRAWBUFFERSPROC(uint framebuffer, int n, int* bufs);
    internal unsafe delegate void PFNGLNAMEDFRAMEBUFFERREADBUFFERPROC(uint framebuffer, int src);
    internal unsafe delegate void PFNGLINVALIDATENAMEDFRAMEBUFFERDATAPROC(uint framebuffer, int numAttachments, int* attachments);
    internal unsafe delegate void PFNGLINVALIDATENAMEDFRAMEBUFFERSUBDATAPROC(uint framebuffer, int numAttachments, int* attachments, int x, int y, int width, int height);
    internal unsafe delegate void PFNGLCLEARNAMEDFRAMEBUFFERIVPROC(uint framebuffer, int buffer, int drawbuffer, int* value);
    internal unsafe delegate void PFNGLCLEARNAMEDFRAMEBUFFERUIVPROC(uint framebuffer, int buffer, int drawbuffer, uint* value);
    internal unsafe delegate void PFNGLCLEARNAMEDFRAMEBUFFERFVPROC(uint framebuffer, int buffer, int drawbuffer, float* value);
    internal unsafe delegate void PFNGLCLEARNAMEDFRAMEBUFFERFIPROC(uint framebuffer, int buffer, int drawbuffer, float depth, int stencil);
    internal unsafe delegate void PFNGLBLITNAMEDFRAMEBUFFERPROC(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, int filter);
    internal unsafe delegate int PFNGLCHECKNAMEDFRAMEBUFFERSTATUSPROC(uint framebuffer, int target);
    internal unsafe delegate void PFNGLGETNAMEDFRAMEBUFFERPARAMETERIVPROC(uint framebuffer, int pname, int* param);
    internal unsafe delegate void PFNGLGETNAMEDFRAMEBUFFERATTACHMENTPARAMETERIVPROC(uint framebuffer, int attachment, int pname, int* parameters);
    internal unsafe delegate void PFNGLCREATERENDERBUFFERSPROC(int n, uint* renderbuffers);
    internal unsafe delegate void PFNGLNAMEDRENDERBUFFERSTORAGEPROC(uint renderbuffer, int internalformat, int width, int height);
    internal unsafe delegate void PFNGLNAMEDRENDERBUFFERSTORAGEMULTISAMPLEPROC(uint renderbuffer, int samples, int internalformat, int width, int height);
    internal unsafe delegate void PFNGLGETNAMEDRENDERBUFFERPARAMETERIVPROC(uint renderbuffer, int pname, int* parameters);
    internal unsafe delegate void PFNGLCREATETEXTURESPROC(int target, int n, uint* textures);
    internal unsafe delegate void PFNGLTEXTUREBUFFERPROC(uint texture, int internalformat, uint buffer);
    internal unsafe delegate void PFNGLTEXTUREBUFFERRANGEPROC(uint texture, int internalformat, uint buffer, long offset, int* size);
    internal unsafe delegate void PFNGLTEXTURESTORAGE1DPROC(uint texture, int levels, int internalformat, int width);
    internal unsafe delegate void PFNGLTEXTURESTORAGE2DPROC(uint texture, int levels, int internalformat, int width, int height);
    internal unsafe delegate void PFNGLTEXTURESTORAGE3DPROC(uint texture, int levels, int internalformat, int width, int height, int depth);
    internal unsafe delegate void PFNGLTEXTURESTORAGE2DMULTISAMPLEPROC(uint texture, int samples, int internalformat, int width, int height, bool fixedsamplelocations);
    internal unsafe delegate void PFNGLTEXTURESTORAGE3DMULTISAMPLEPROC(uint texture, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations);
    internal unsafe delegate void PFNGLTEXTURESUBIMAGE1DPROC(uint texture, int level, int xoffset, int width, int format, int type, void* pixels);
    internal unsafe delegate void PFNGLTEXTURESUBIMAGE2DPROC(uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int type, void* pixels);
    internal unsafe delegate void PFNGLTEXTURESUBIMAGE3DPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, void* pixels);
    internal unsafe delegate void PFNGLCOMPRESSEDTEXTURESUBIMAGE1DPROC(uint texture, int level, int xoffset, int width, int format, int imageSize, void* data);
    internal unsafe delegate void PFNGLCOMPRESSEDTEXTURESUBIMAGE2DPROC(uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, void* data);
    internal unsafe delegate void PFNGLCOMPRESSEDTEXTURESUBIMAGE3DPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int imageSize, void* data);
    internal unsafe delegate void PFNGLCOPYTEXTURESUBIMAGE1DPROC(uint texture, int level, int xoffset, int x, int y, int width);
    internal unsafe delegate void PFNGLCOPYTEXTURESUBIMAGE2DPROC(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height);
    internal unsafe delegate void PFNGLCOPYTEXTURESUBIMAGE3DPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
    internal unsafe delegate void PFNGLTEXTUREPARAMETERFPROC(uint texture, int pname, float param);
    internal unsafe delegate void PFNGLTEXTUREPARAMETERFVPROC(uint texture, int pname, float* param);
    internal unsafe delegate void PFNGLTEXTUREPARAMETERIPROC(uint texture, int pname, int param);
    internal unsafe delegate void PFNGLTEXTUREPARAMETERIIVPROC(uint texture, int pname, int* parameters);
    internal unsafe delegate void PFNGLTEXTUREPARAMETERIUIVPROC(uint texture, int pname, uint* parameters);
    internal unsafe delegate void PFNGLTEXTUREPARAMETERIVPROC(uint texture, int pname, int* param);
    internal unsafe delegate void PFNGLGENERATETEXTUREMIPMAPPROC(uint texture);
    internal unsafe delegate void PFNGLBINDTEXTUREUNITPROC(uint unit, uint texture);
    internal unsafe delegate void PFNGLGETTEXTUREIMAGEPROC(uint texture, int level, int format, int type, int bufSize, void* pixels);
    internal unsafe delegate void PFNGLGETCOMPRESSEDTEXTUREIMAGEPROC(uint texture, int level, int bufSize, void* pixels);
    internal unsafe delegate void PFNGLGETTEXTURELEVELPARAMETERFVPROC(uint texture, int level, int pname, float* parameters);
    internal unsafe delegate void PFNGLGETTEXTURELEVELPARAMETERIVPROC(uint texture, int level, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETTEXTUREPARAMETERFVPROC(uint texture, int pname, float* parameters);
    internal unsafe delegate void PFNGLGETTEXTUREPARAMETERIIVPROC(uint texture, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETTEXTUREPARAMETERIUIVPROC(uint texture, int pname, uint* parameters);
    internal unsafe delegate void PFNGLGETTEXTUREPARAMETERIVPROC(uint texture, int pname, int* parameters);
    internal unsafe delegate void PFNGLCREATEVERTEXARRAYSPROC(int n, uint* arrays);
    internal unsafe delegate void PFNGLDISABLEVERTEXARRAYATTRIBPROC(uint vaobj, uint index);
    internal unsafe delegate void PFNGLENABLEVERTEXARRAYATTRIBPROC(uint vaobj, uint index);
    internal unsafe delegate void PFNGLVERTEXARRAYELEMENTBUFFERPROC(uint vaobj, uint buffer);
    internal unsafe delegate void PFNGLVERTEXARRAYVERTEXBUFFERPROC(uint vaobj, uint bindingindex, uint buffer, long offset, int stride);
    internal unsafe delegate void PFNGLVERTEXARRAYVERTEXBUFFERSPROC(uint vaobj, uint first, int count, uint* buffers, long* offsets, int* strides);
    internal unsafe delegate void PFNGLVERTEXARRAYATTRIBBINDINGPROC(uint vaobj, uint attribindex, uint bindingindex);
    internal unsafe delegate void PFNGLVERTEXARRAYATTRIBFORMATPROC(uint vaobj, uint attribindex, int size, int type, bool normalized, uint relativeoffset);
    internal unsafe delegate void PFNGLVERTEXARRAYATTRIBIFORMATPROC(uint vaobj, uint attribindex, int size, int type, uint relativeoffset);
    internal unsafe delegate void PFNGLVERTEXARRAYATTRIBLFORMATPROC(uint vaobj, uint attribindex, int size, int type, uint relativeoffset);
    internal unsafe delegate void PFNGLVERTEXARRAYBINDINGDIVISORPROC(uint vaobj, uint bindingindex, uint divisor);
    internal unsafe delegate void PFNGLGETVERTEXARRAYIVPROC(uint vaobj, int pname, int* param);
    internal unsafe delegate void PFNGLGETVERTEXARRAYINDEXEDIVPROC(uint vaobj, uint index, int pname, int* param);
    internal unsafe delegate void PFNGLGETVERTEXARRAYINDEXED64IVPROC(uint vaobj, uint index, int pname, long* param);
    internal unsafe delegate void PFNGLCREATESAMPLERSPROC(int n, uint* samplers);
    internal unsafe delegate void PFNGLCREATEPROGRAMPIPELINESPROC(int n, uint* pipelines);
    internal unsafe delegate void PFNGLCREATEQUERIESPROC(int target, int n, uint* ids);
    internal unsafe delegate void PFNGLGETQUERYBUFFEROBJECTI64VPROC(uint id, uint buffer, int pname, long offset);
    internal unsafe delegate void PFNGLGETQUERYBUFFEROBJECTIVPROC(uint id, uint buffer, int pname, long offset);
    internal unsafe delegate void PFNGLGETQUERYBUFFEROBJECTUI64VPROC(uint id, uint buffer, int pname, long offset);
    internal unsafe delegate void PFNGLGETQUERYBUFFEROBJECTUIVPROC(uint id, uint buffer, int pname, long offset);
    internal unsafe delegate void PFNGLMEMORYBARRIERBYREGIONPROC(uint barriers);
    internal unsafe delegate void PFNGLGETTEXTURESUBIMAGEPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, int bufSize, void* pixels);
    internal unsafe delegate void PFNGLGETCOMPRESSEDTEXTURESUBIMAGEPROC(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, void* pixels);
    internal unsafe delegate int PFNGLGETGRAPHICSRESETSTATUSPROC();
    internal unsafe delegate void PFNGLGETNCOMPRESSEDTEXIMAGEPROC(int target, int lod, int bufSize, void* pixels);
    internal unsafe delegate void PFNGLGETNTEXIMAGEPROC(int target, int level, int format, int type, int bufSize, void* pixels);
    internal unsafe delegate void PFNGLGETNUNIFORMDVPROC(uint program, int location, int bufSize, double* parameters);
    internal unsafe delegate void PFNGLGETNUNIFORMFVPROC(uint program, int location, int bufSize, float* parameters);
    internal unsafe delegate void PFNGLGETNUNIFORMIVPROC(uint program, int location, int bufSize, int* parameters);
    internal unsafe delegate void PFNGLGETNUNIFORMUIVPROC(uint program, int location, int bufSize, uint* parameters);
    internal unsafe delegate void PFNGLREADNPIXELSPROC(int x, int y, int width, int height, int format, int type, int bufSize, void* data);
    internal unsafe delegate void PFNGLGETNMAPDVPROC(int target, int query, int bufSize, double* v);
    internal unsafe delegate void PFNGLGETNMAPFVPROC(int target, int query, int bufSize, float* v);
    internal unsafe delegate void PFNGLGETNMAPIVPROC(int target, int query, int bufSize, int* v);
    internal unsafe delegate void PFNGLGETNPIXELMAPFVPROC(int map, int bufSize, float* values);
    internal unsafe delegate void PFNGLGETNPIXELMAPUIVPROC(int map, int bufSize, uint* values);
    internal unsafe delegate void PFNGLGETNPIXELMAPUSVPROC(int map, int bufSize, ushort* values);
    internal unsafe delegate void PFNGLGETNPOLYGONSTIPPLEPROC(int bufSize, byte* pattern);
    internal unsafe delegate void PFNGLGETNCOLORTABLEPROC(int target, int format, int type, int bufSize, void* table);
    internal unsafe delegate void PFNGLGETNCONVOLUTIONFILTERPROC(int target, int format, int type, int bufSize, void* image);
    internal unsafe delegate void PFNGLGETNSEPARABLEFILTERPROC(int target, int format, int type, int rowBufSize, void* row, int columnBufSize, void* column, void* span);
    internal unsafe delegate void PFNGLGETNHISTOGRAMPROC(int target, bool reset, int format, int type, int bufSize, void* values);
    internal unsafe delegate void PFNGLGETNMINMAXPROC(int target, bool reset, int format, int type, int bufSize, void* values);
    internal unsafe delegate void PFNGLTEXTUREBARRIERPROC();

    #endregion

    #region gl_4_6

    internal unsafe delegate void PFNGLSPECIALIZESHADERPROC(uint shader, sbyte* pEntryPoint, uint numSpecializationants, uint* pantIndex, uint* pantValue);
    internal unsafe delegate void PFNGLMULTIDRAWARRAYSINDIRECTCOUNTPROC(int mode, void* indirect, long drawcount, int maxdrawcount, int stride);
    internal unsafe delegate void PFNGLMULTIDRAWELEMENTSINDIRECTCOUNTPROC(int mode, int type, void* indirect, long drawcount, int maxdrawcount, int stride);
    internal unsafe delegate void PFNGLPOLYGONOFFSETCLAMPPROC(float factor, float units, float clamp);

    #endregion

    #region gl_escm_1_0

    internal unsafe delegate void PFNGLALPHAFUNCPROC(int func, float reference);
    internal unsafe delegate void PFNGLCLIPPLANEFPROC(int p, float* eqn);
    internal unsafe delegate void PFNGLCOLOR4FPROC(float red, float green, float blue, float alpha);
    internal unsafe delegate void PFNGLFOGFPROC(int pname, float param);
    internal unsafe delegate void PFNGLFOGFVPROC(int pname, float* parameters);
    internal unsafe delegate void PFNGLFRUSTUMFPROC(float l, float r, float b, float t, float n, float f);
    internal unsafe delegate void PFNGLGETCLIPPLANEFPROC(int plane, float* equation);
    internal unsafe delegate void PFNGLGETLIGHTFVPROC(int light, int pname, float* parameters);
    internal unsafe delegate void PFNGLGETMATERIALFVPROC(int face, int pname, float* parameters);
    internal unsafe delegate void PFNGLGETTEXENVFVPROC(int target, int pname, float* parameters);
    internal unsafe delegate void PFNGLLIGHTMODELFPROC(int pname, float param);
    internal unsafe delegate void PFNGLLIGHTMODELFVPROC(int pname, float* parameters);
    internal unsafe delegate void PFNGLLIGHTFPROC(int light, int pname, float param);
    internal unsafe delegate void PFNGLLIGHTFVPROC(int light, int pname, float* parameters);
    internal unsafe delegate void PFNGLLOADMATRIXFPROC(float* m);
    internal unsafe delegate void PFNGLMATERIALFPROC(int face, int pname, float param);
    internal unsafe delegate void PFNGLMATERIALFVPROC(int face, int pname, float* parameters);
    internal unsafe delegate void PFNGLMULTMATRIXFPROC(float* m);
    internal unsafe delegate void PFNGLMULTITEXCOORD4FPROC(int target, float s, float t, float r, float q);
    internal unsafe delegate void PFNGLNORMAL3FPROC(float nx, float ny, float nz);
    internal unsafe delegate void PFNGLORTHOFPROC(float l, float r, float b, float t, float n, float f);
    internal unsafe delegate void PFNGLROTATEFPROC(float angle, float x, float y, float z);
    internal unsafe delegate void PFNGLSCALEFPROC(float x, float y, float z);
    internal unsafe delegate void PFNGLTEXENVFPROC(int target, int pname, float param);
    internal unsafe delegate void PFNGLTEXENVFVPROC(int target, int pname, float* parameters);
    internal unsafe delegate void PFNGLTRANSLATEFPROC(float x, float y, float z);
    internal unsafe delegate void PFNGLALPHAFUNCXPROC(int func, int reference);
    internal unsafe delegate void PFNGLCLEARCOLORXPROC(int red, int green, int blue, int alpha);
    internal unsafe delegate void PFNGLCLEARDEPTHXPROC(int depth);
    internal unsafe delegate void PFNGLCLIENTACTIVETEXTUREPROC(int texture);
    internal unsafe delegate void PFNGLCLIPPLANEXPROC(int plane, int* equation);
    internal unsafe delegate void PFNGLCOLOR4UBPROC(byte red, byte green, byte blue, byte alpha);
    internal unsafe delegate void PFNGLCOLOR4XPROC(int red, int green, int blue, int alpha);
    internal unsafe delegate void PFNGLCOLORPOINTERPROC(int size, int type, int stride, void* pointer);
    internal unsafe delegate void PFNGLDEPTHRANGEXPROC(int n, int f);
    internal unsafe delegate void PFNGLDISABLECLIENTSTATEPROC(int array);
    internal unsafe delegate void PFNGLENABLECLIENTSTATEPROC(int array);
    internal unsafe delegate void PFNGLFOGXPROC(int pname, int param);
    internal unsafe delegate void PFNGLFOGXVPROC(int pname, int* param);
    internal unsafe delegate void PFNGLFRUSTUMXPROC(int l, int r, int b, int t, int n, int f);
    internal unsafe delegate void PFNGLGETCLIPPLANEXPROC(int plane, int* equation);
    internal unsafe delegate void PFNGLGETFIXEDVPROC(int pname, int* parameters);
    internal unsafe delegate void PFNGLGETLIGHTXVPROC(int light, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETMATERIALXVPROC(int face, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETTEXENVIVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETTEXENVXVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLGETTEXPARAMETERXVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLLIGHTMODELXPROC(int pname, int param);
    internal unsafe delegate void PFNGLLIGHTMODELXVPROC(int pname, int* param);
    internal unsafe delegate void PFNGLLIGHTXPROC(int light, int pname, int param);
    internal unsafe delegate void PFNGLLIGHTXVPROC(int light, int pname, int* parameters);
    internal unsafe delegate void PFNGLLINEWIDTHXPROC(int width);
    internal unsafe delegate void PFNGLLOADIDENTITYPROC();
    internal unsafe delegate void PFNGLLOADMATRIXXPROC(int* m);
    internal unsafe delegate void PFNGLMATERIALXPROC(int face, int pname, int param);
    internal unsafe delegate void PFNGLMATERIALXVPROC(int face, int pname, int* param);
    internal unsafe delegate void PFNGLMATRIXMODEPROC(int mode);
    internal unsafe delegate void PFNGLMULTMATRIXXPROC(int* m);
    internal unsafe delegate void PFNGLMULTITEXCOORD4XPROC(int texture, int s, int t, int r, int q);
    internal unsafe delegate void PFNGLNORMAL3XPROC(int nx, int ny, int nz);
    internal unsafe delegate void PFNGLNORMALPOINTERPROC(int type, int stride, void* pointer);
    internal unsafe delegate void PFNGLORTHOXPROC(int l, int r, int b, int t, int n, int f);
    internal unsafe delegate void PFNGLPOINTPARAMETERXPROC(int pname, int param);
    internal unsafe delegate void PFNGLPOINTPARAMETERXVPROC(int pname, int* parameters);
    internal unsafe delegate void PFNGLPOINTSIZEXPROC(int size);
    internal unsafe delegate void PFNGLPOLYGONOFFSETXPROC(int factor, int units);
    internal unsafe delegate void PFNGLPOPMATRIXPROC();
    internal unsafe delegate void PFNGLPUSHMATRIXPROC();
    internal unsafe delegate void PFNGLROTATEXPROC(int angle, int x, int y, int z);
    internal unsafe delegate void PFNGLSAMPLECOVERAGEXPROC(int value, bool invert);
    internal unsafe delegate void PFNGLSCALEXPROC(int x, int y, int z);
    internal unsafe delegate void PFNGLSHADEMODELPROC(int mode);
    internal unsafe delegate void PFNGLTEXCOORDPOINTERPROC(int size, int type, int stride, void* pointer);
    internal unsafe delegate void PFNGLTEXENVIPROC(int target, int pname, int param);
    internal unsafe delegate void PFNGLTEXENVXPROC(int target, int pname, int param);
    internal unsafe delegate void PFNGLTEXENVIVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLTEXENVXVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLTEXPARAMETERXPROC(int target, int pname, int param);
    internal unsafe delegate void PFNGLTEXPARAMETERXVPROC(int target, int pname, int* parameters);
    internal unsafe delegate void PFNGLTRANSLATEXPROC(int x, int y, int z);
    internal unsafe delegate void PFNGLVERTEXPOINTERPROC(int size, int type, int stride, void* pointer);

    #endregion

    #region gl_es_3_2

    internal unsafe delegate void PFNGLBLENDBARRIERPROC();
    internal unsafe delegate void PFNGLPRIMITIVEBOUNDINGBOXPROC(float minX, float minY, float minZ, float minW, float maxX, float maxY, float maxZ, float maxW);

    #endregion



}
