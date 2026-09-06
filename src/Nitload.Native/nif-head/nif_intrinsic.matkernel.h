#ifndef NIF_MATKERNEL
#define NIF_MATKERNEL

#include <immintrin.h>
#include <stdint.h>

#include "nif_internal.h"

/* Kernel storage unions for a fixed 4x4 block. */
#define ELEMENTSIZE(T) sizeof(T)
#define XMMCOUNT(T) (sizeof(T))
#define YMMCOUNT(T) (sizeof(T) / 2)
#define ZMMCOUNT(T) (sizeof(T) / 4)

#define NIF_MAT_KERNEL_MEMBER(T)        \
        {                              \
                __m128 x[XMMCOUNT(T)]; \
                __m256 y[YMMCOUNT(T)]; \
                __m512 z[ZMMCOUNT(T)]; \
                T m[4][4];             \
        }
        
#define NIF_MAT_KERNEL_DEFINE(T) \
        typedef union NIF_MAT_KERNEL_TYPE_##T NIF_MAT_KERNEL_MEMBER(T) NIF_MAT_KERNEL_TYPE_##T

#define MAT_KERNEL(T) NIF_MAT_KERNEL_TYPE(T)
#define NIF_MAT_KERNEL_TYPE(T) NIF_MAT_KERNEL_TYPE_##T

NIF_MAT_KERNEL_DEFINE(float);
NIF_MAT_KERNEL_DEFINE(double);
NIF_MAT_KERNEL_DEFINE(uint64_t);


#define NIF_VEC_SEGMENT_MEMBER(T)               \
        {                                \
                __m128 f32_x[XMMCOUNT(T) / 4]; \
                __m256 f32_y[YMMCOUNT(T) / 4]; \
                __m512 f32_z[ZMMCOUNT(T) / 4]; \
                T v[4];                        \
        }

#define NIF_VEC_SEGMENT_DEFINE(T) \
        typedef union NIF_VEC_SEGMENT_TYPE_##T NIF_VEC_SEGMENT_MEMBER(T) NIF_VEC_SEGMENT_TYPE_##T

#define VEC_SEGMENT(T) NIF_VEC_SEGMENT_TYPE(T)
#define NIF_VEC_SEGMENT_TYPE(T) NIF_VEC_SEGMENT_TYPE_##T

NIF_VEC_SEGMENT_DEFINE(float);
NIF_VEC_SEGMENT_DEFINE(double);

/* Global zero kernel declaration (defined in dllmain.c), aligned to 64 bytes */
extern __declspec(align(64)) MAT_KERNEL(u64) NIF_ZERO_KERNEL;

#endif /* NIF_MATKERNEL */
