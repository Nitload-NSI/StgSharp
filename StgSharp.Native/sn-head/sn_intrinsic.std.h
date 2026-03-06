#ifndef SN_INTRINSIC_CallCONV_STD
#define SN_INTRINSIC_CallCONV_STD

#include "sn_internal.h"
#include "sn_intrinsic.h"

#ifndef static_assert
#define static_assert _Static_assert
#endif

#define mat_task_local ((_mat_task_local const *)task)

#define MK_PROC_NAME_STD(T_type, T_arch, T_feature, T_op) \
        T_type##_multi_kernel_proc_##T_op##_##T_arch##_##T_feature
#define SN_MK_PROC_DECL(T_type, P_name) INTERNAL void SN_DECL P_name(MAT_TASK(T_type) const *task)
#define SN_MK_PROC_DECL_STD(T_type, T_arch, T_feature, T_op)                          \
        INTERNAL FORCEINLINE void SN_DECL MK_PROC_NAME_STD(T_type, T_arch, T_feature, \
                                                           T_op)(MAT_TASK(T_type) const *task)
#define SK_PROC_NAME_STD(T_type, T_arch, T_feature, T_op) \
        T_type##_single_kernel_proc_##T_op##_##T_arch##_##T_feature
#define SN_SK_PROC_DECL_STD(T_type, T_arch, T_feature, T_op)                                 \
        INTERNAL FORCEINLINE void SN_DECL SK_PROC_NAME_STD(T_type, T_arch, T_feature, T_op)( \
                MAT_KERNEL(T_type) const *left, MAT_KERNEL(T_type) const *right,             \
                MAT_KERNEL(T_type) *restrict ans, uint64_t scalar)

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

#define _SN_MK_EXPAND_RIGHT_ANS                                   \
        SN_MAT_KERNEL_LOCAL const *right = mat_task_local->mat_1; \
        SN_MAT_KERNEL_LOCAL *restrict ans = mat_task_local->mat_2;

#define _SN_MK_EXPAND_LEFT_RIGHT_ANS                              \
        SN_MAT_KERNEL_LOCAL const *left = mat_task_local->mat_0;  \
        SN_MAT_KERNEL_LOCAL const *right = mat_task_local->mat_1; \
        SN_MAT_KERNEL_LOCAL *restrict ans = mat_task_local->mat_2;

#define _SN_MK_EXPAND_RIGHT_ANS_SCALAR                             \
        SN_MAT_KERNEL_LOCAL const *right = mat_task_local->mat_1;  \
        SN_MAT_KERNEL_LOCAL *restrict ans = mat_task_local->mat_2; \
        scalar_pack *restrict scalar = mat_task_local->scalar;

#define _SN_MK_EXPAND_LEFT_RIGHT_ANS_SCALAR                        \
        SN_MAT_KERNEL_LOCAL const *left = mat_task_local->mat_0;   \
        SN_MAT_KERNEL_LOCAL const *right = mat_task_local->mat_1;  \
        SN_MAT_KERNEL_LOCAL *restrict ans = mat_task_local->mat_2; \
        scalar_pack *restrict scalar = mat_task_local->scalar;

#define _SN_MK_EXPAND_ANS_SCALAR                                   \
        SN_MAT_KERNEL_LOCAL *restrict ans = mat_task_local->mat_2; \
        scalar_pack *restrict scalar = &(mat_task_local->padding);

#define _SN_PARAM_STYLE(T) _SN_MATPROC_##T
#define _SN_MK_EXPAND_PARAM(style) _SN_MK_EXPAND_##style

#pragma endregion

#pragma region mem layout

#define _SN_MATPROC_LAYOUT_BUFFER 1
#define _SN_MATPROC_LAYOUT_TILE 1
#define _SN_MATPROC_LAYOUT_KERNEL 1

#define _MK_MKPROC_EXPAND_INDEX_BUFFER                            \
        size_t offset = UNSAFE_AS(size_t, task->padding.data[0]); \
        size_t count = UNSAFE_AS(size_t, task->padding.data[1]);

#define _MK_MKPROC_EXPAND_INDEX_KERNEL                       \
        size_t x = UNSAFE_AS(size_t, task->x);               \
        size_t y = UNSAFE_AS(size_t, task->y);               \
        size_t common_x = UNSAFE_AS(size_t, task->common_x); \
        size_t common_y = UNSAFE_AS(size_t, task->common_y); \
        size_t common_z = UNSAFE_AS(size_t, task->common_z);
#define _MK_MKPROC_EXPAND_INDEX_TILE                         \
        size_t x = UNSAFE_AS(size_t, task->x);               \
        size_t y = UNSAFE_AS(size_t, task->y);               \
        size_t common_x = UNSAFE_AS(size_t, task->common_x); \
        size_t common_y = UNSAFE_AS(size_t, task->common_y); \
        size_t common_z = UNSAFE_AS(size_t, task->common_z);

#define _SN_MK_EXPAND_MEM(mem) _MK_MKPROC_EXPAND_INDEX_##mem
#define _SN_MEM_LAYOUT(T) _SN_MATPROC_LAYOUT_##T

#pragma endregion

#define __mk_param_std(mem, param, T)                                               \
        typedef MAT_KERNEL(T) SN_MAT_KERNEL_LOCAL;                                  \
        typedef MAT_TASK(T) _mat_task_local;                                        \
        _mat_task_local *_ = (_SN_PARAM_STYLE(param), (_SN_MEM_LAYOUT(mem)), task); \
        _SN_MK_EXPAND_PARAM(param);                                                 \
        _SN_MK_EXPAND_MEM(mem)
#endif // !SN_INTRINSIC_PARAM_EXPAND
