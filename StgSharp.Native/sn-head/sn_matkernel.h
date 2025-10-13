#ifndef SN_MATKERNEL
#define SN_MATKERNEL

#include <immintrin.h>

#define ELEMENTSIZE(T) sizeof(T)
#define XMMCOUNT(T) sizeof(T)
#define YMMCOUNT(T) sizeof(T) / 2
#define ZMMCOUNT(T) sizeof(T) / 4

#define MAT_KERNEL(T)                    \
        union mat_kernel_##T##{          \
                __m128 xmm[XMMCOUNT(T)]; \
                __m256 ymm[YMMCOUNT(T)]; \
                __m512 zmm[ZMMCOUNT(T)]; \
                T m[4][4];               \
        }

#define VEC(T)                               \
        union vec_##T##{                     \
                __m128 xmm[XMMCOUNT(T) / 4]; \
                __m256 ymm[YMMCOUNT(T) / 4]; \
                __m512 zmm[ZMMCOUNT(T) / 4]; \
                T v[4];                      \
        }

/*
#undef ELEMENTSIZE
#undef XMMCOUNT
#undef YMMCOUNT
#undef ZMMCOUNT
/**/

#endif
