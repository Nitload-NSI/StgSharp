#ifndef SN_INTRINSIC_CALLCONV_STD
#define SN_INTRINSIC_CALLCONV_STD

#include "sn_internal.h"
#include "sn_intrinsic.h"

#ifndef static_assert
#define static_assert _Static_assert
#endif

#define SN_STD_TARGET_ATTR(T_arch, T_feature) SN_STD_TARGET_ATTR_IMPL(T_arch, T_feature)

#pragma region mk_proc attributes
#define MK_PROC_NAME_STD(T_type, T_arch, T_feature, T_op) \
        T_type##_multi_kernel_proc_##T_op##_##T_arch##_##T_feature
#define GET_MK_PROC_STD(T_type, T_arch, T_feature, T_op) \
        ((UNI_MK_PROC)MK_PROC_NAME_STD(T_type, T_arch, T_feature, T_op))
#define SN_MK_PROC_DECL(T_type, P_name) INTERNAL void SN_DECL P_name(MAT_TASK(T_type) const *task)
#define SN_MK_PROC_DECL_STD(T_type, T_arch, T_feature, T_op)                                   \
        INTERNAL NOINLINE SN_STD_TARGET_ATTR(T_arch, T_feature) void SN_DECL MK_PROC_NAME_STD( \
                T_type, T_arch, T_feature, T_op)(MAT_TASK(T_type) const *task)
#pragma endregion

#pragma region sk_proc attributes
#define SK_PROC_NAME_STD(T_type, T_arch, T_feature, T_op) \
        T_type##_single_kernel_proc_##T_op##_##T_arch##_##T_feature
#define GET_SK_PROC_STD(T_type, T_arch, T_feature, T_op) \
        ((UNI_SK_PROC)SK_PROC_NAME_STD(T_type, T_arch, T_feature, T_op))
#define SN_SK_PROC_DECL_STD(T_type, T_arch, T_feature, T_op)                                   \
        INTERNAL NOINLINE SN_STD_TARGET_ATTR(T_arch, T_feature) void SN_DECL SK_PROC_NAME_STD( \
                T_type, T_arch, T_feature,                                                     \
                T_op)(MAT_KERNEL(T_type) const *left, MAT_KERNEL(T_type) const *right,         \
                      MAT_KERNEL(T_type) *restrict ans, uint64_t scalar)
#pragma endregion

#define _SN_STD__PROBE_0 0
#define _SN_STD__PROBE_1 1
#define SN_STD__PROBE_PRINT(x) _SN_STD__PROBE_##x
#define SN_STD__PROBE(x) SN_STD__PROBE_PRINT(x)

#pragma region param styles

#define _SN_MATPROC_LEFT_RIGHT_ANS 1
#define _SN_MATPROC_RIGHT_ANS 1
#define _SN_MATPROC_LEFT_RIGHT_ANS_SCALAR 1
#define _SN_MATPROC_RIGHT_ANS_SCALAR 1
#define _SN_MATPROC_ANS_SCALAR 1

#define _SN_MK_EXPAND_BUFFER_SIZE(i)\
                int offset_##i = task->offset_##i;\
                int count_##i = task->count_##i;\

#define _SN_MK_EXPAND_RIGHT_ANS                                                             \
        SN_MAT_KERNEL_LOCAL const *right = (SN_MAT_KERNEL_LOCAL const *)task->buffer_1.ptr; \
        SN_MAT_KERNEL_LOCAL *restrict ans = (SN_MAT_KERNEL_LOCAL *)task->buffer_2.ptr;\
        _SN_MK_EXPAND_BUFFER_SIZE(1); \
        _SN_MK_EXPAND_BUFFER_SIZE(2);

#define _SN_MK_EXPAND_LEFT_RIGHT_ANS                                                        \
        SN_MAT_KERNEL_LOCAL const *left = (SN_MAT_KERNEL_LOCAL const *)task->buffer_0.ptr;  \
        SN_MAT_KERNEL_LOCAL const *right = (SN_MAT_KERNEL_LOCAL const *)task->buffer_1.ptr; \
        SN_MAT_KERNEL_LOCAL *restrict ans = (SN_MAT_KERNEL_LOCAL *)task->buffer_2.ptr;\
        _SN_MK_EXPAND_BUFFER_SIZE(0); \
        _SN_MK_EXPAND_BUFFER_SIZE(1);\
        _SN_MK_EXPAND_BUFFER_SIZE(2);

#define _SN_MK_EXPAND_RIGHT_ANS_SCALAR                                                      \
        SN_MAT_KERNEL_LOCAL const *right = (SN_MAT_KERNEL_LOCAL const *)task->buffer_1.ptr; \
        SN_MAT_KERNEL_LOCAL *restrict ans = (SN_MAT_KERNEL_LOCAL *)task->buffer_2.ptr;      \
        M128 scalar = task->scalar_pack;\
        _SN_MK_EXPAND_BUFFER_SIZE(1);\
        _SN_MK_EXPAND_BUFFER_SIZE(2);

#define _SN_MK_EXPAND_LEFT_RIGHT_ANS_SCALAR                                                 \
        SN_MAT_KERNEL_LOCAL const *left = (SN_MAT_KERNEL_LOCAL const *)task->buffer_0.ptr;  \
        SN_MAT_KERNEL_LOCAL const *right = (SN_MAT_KERNEL_LOCAL const *)task->buffer_1.ptr; \
        SN_MAT_KERNEL_LOCAL *restrict ans = (SN_MAT_KERNEL_LOCAL *)task->buffer_2.ptr;      \
        M128 scalar = task->scalar_pack;\
        _SN_MK_EXPAND_BUFFER_SIZE(0);\
        _SN_MK_EXPAND_BUFFER_SIZE(1);\
        _SN_MK_EXPAND_BUFFER_SIZE(2);

#define _SN_MK_EXPAND_ANS_SCALAR                                                       \
        SN_MAT_KERNEL_LOCAL *restrict ans = (SN_MAT_KERNEL_LOCAL *)task->buffer_2.ptr; \
        M128 scalar = task->scalar_pack;\
        _SN_MK_EXPAND_BUFFER_SIZE(2);

#define _SN_PARAM_STYLE(T) _SN_MATPROC_##T
#define _SN_MK_EXPAND_PARAM(style) _SN_MK_EXPAND_##style

#pragma endregion

#define __mk_param_std(param, T)                               \
        typedef MAT_KERNEL(T) SN_MAT_KERNEL_LOCAL;             \
        matrix_task const *_ = (_SN_PARAM_STYLE(param), task); \
        _SN_MK_EXPAND_PARAM(param)

#define __sk_param_std(T)                          \
        typedef MAT_KERNEL(T) SN_MAT_KERNEL_LOCAL; \
        (void)scalar
#endif // !SN_INTRINSIC_PARAM_EXPAND
