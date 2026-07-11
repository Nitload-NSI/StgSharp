#ifndef SN_MATKERNEL
#define SN_MATKERNEL

#include <immintrin.h>
#include <stdint.h>

#include "sn_internal.h"

/* Kernel storage unions for a fixed 4x4 block. */
#define ELEMENTSIZE(T) sizeof(T)
#define XMMCOUNT(T) (sizeof(T))
#define YMMCOUNT(T) (sizeof(T) / 2)
#define ZMMCOUNT(T) (sizeof(T) / 4)

#define SN_MAT_KERNEL_MEMBER(T)        \
        {                              \
                __m128 x[XMMCOUNT(T)]; \
                __m256 y[YMMCOUNT(T)]; \
                __m512 z[ZMMCOUNT(T)]; \
                T m[4][4];             \
        }
        
#define SN_MAT_KERNEL_DEFINE(T) \
        typedef union SN_MAT_KERNEL_TYPE_##T SN_MAT_KERNEL_MEMBER(T) SN_MAT_KERNEL_TYPE_##T

#define MAT_KERNEL(T) SN_MAT_KERNEL_TYPE(T)
#define SN_MAT_KERNEL_TYPE(T) SN_MAT_KERNEL_TYPE_##T

SN_MAT_KERNEL_DEFINE(float);
SN_MAT_KERNEL_DEFINE(double);
SN_MAT_KERNEL_DEFINE(uint64_t);


#define SN_VEC_SEGMENT_MEMBER(T)               \
        {                                \
                __m128 f32_x[XMMCOUNT(T) / 4]; \
                __m256 f32_y[YMMCOUNT(T) / 4]; \
                __m512 f32_z[ZMMCOUNT(T) / 4]; \
                T v[4];                        \
        }

#define SN_VEC_SEGMENT_DEFINE(T) \
        typedef union SN_VEC_SEGMENT_TYPE_##T SN_VEC_SEGMENT_MEMBER(T) SN_VEC_SEGMENT_TYPE_##T

#define VEC_SEGMENT(T) SN_VEC_SEGMENT_TYPE(T)
#define SN_VEC_SEGMENT_TYPE(T) SN_VEC_SEGMENT_TYPE_##T

SN_VEC_SEGMENT_DEFINE(float);
SN_VEC_SEGMENT_DEFINE(double);

/* Global zero kernel declaration (defined in dllmain.c), aligned to 64 bytes */
extern __declspec(align(64)) MAT_KERNEL(u64) SN_ZERO_KERNEL;

#endif /* SN_MATKERNEL */
