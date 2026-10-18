#ifndef NFN_MATKERNEL
#define NFN_MATKERNEL

#include <immintrin.h>
#include <stdint.h>

#include "nfn_internal.h"

/* Kernel storage unions for a fixed 4x4 block. */
#define ELEMENTSIZE(T) sizeof(T)
#define XMMCOUNT(T) (sizeof(T))
#define YMMCOUNT(T) (sizeof(T) / 2)
#define ZMMCOUNT(T) (sizeof(T) / 4)

#define NFN_MAT_KERNEL_MEMBER(T)        \
        {                              \
                __m128 x[XMMCOUNT(T)]; \
                __m256 y[YMMCOUNT(T)]; \
                __m512 z[ZMMCOUNT(T)]; \
                T m[4][4];             \
        }
        
#define NFN_MAT_KERNEL_DEFINE(T) \
        typedef union NFN_MAT_KERNEL_TYPE_##T NFN_MAT_KERNEL_MEMBER(T) NFN_MAT_KERNEL_TYPE_##T

#define MAT_KERNEL(T) NFN_MAT_KERNEL_TYPE(T)
#define NFN_MAT_KERNEL_TYPE(T) NFN_MAT_KERNEL_TYPE_##T

NFN_MAT_KERNEL_DEFINE(float);
NFN_MAT_KERNEL_DEFINE(double);
NFN_MAT_KERNEL_DEFINE(uint64_t);


#define NFN_VEC_SEGMENT_MEMBER(T)               \
        {                                \
                __m128 f32_x[XMMCOUNT(T) / 4]; \
                __m256 f32_y[YMMCOUNT(T) / 4]; \
                __m512 f32_z[ZMMCOUNT(T) / 4]; \
                T v[4];                        \
        }

#define NFN_VEC_SEGMENT_DEFINE(T) \
        typedef union NFN_VEC_SEGMENT_TYPE_##T NFN_VEC_SEGMENT_MEMBER(T) NFN_VEC_SEGMENT_TYPE_##T

#define VEC_SEGMENT(T) NFN_VEC_SEGMENT_TYPE(T)
#define NFN_VEC_SEGMENT_TYPE(T) NFN_VEC_SEGMENT_TYPE_##T

NFN_VEC_SEGMENT_DEFINE(float);
NFN_VEC_SEGMENT_DEFINE(double);

/* Global zero kernel declaration (defined in dllmain.c), aligned to 64 bytes */
extern __declspec(align(64)) MAT_KERNEL(u64) NFN_ZERO_KERNEL;

#endif /* NFN_MATKERNEL */
