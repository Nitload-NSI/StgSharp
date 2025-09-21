#ifndef SN_MATKERNEL
#define SN_MATKERNEL

#include <immintrin.h>

#define ELEMENTSIZE(T) sizeof(T)
#define XMMCOUNT(T) sizeof(T) / 16
#define YMMCOUNT(T) sizeof(T) / 32
#define ZMMCOUNT(T) sizeof(T) / 64

#define MAT_KERNEL(T)                    \
        union {                          \
                __m128 xmm[XMMCOUNT(T)]; \
                __m256 ymm[YMMCOUNT(T)]; \
                __m512 zmm[ZMMCOUNT(T)]; \
                T m[16];                 \
        }

#endif
