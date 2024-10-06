
#include "StgSharpC.h"
#include "ssc_intrinsic.h"
#include <stdio.h>
#include <pmmintrin.h>

INTERNAL void __cdecl transpose42(__4_columnset *source, __2_columnset *target)
{
        __m128 t0 = _mm_shuffle_ps(ALIGN(source->column[0]),
                                   ALIGN(source->column[1]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t2 = _mm_shuffle_ps(ALIGN(source->column[2]),
                                   ALIGN(source->column[3]),
                                   _MM_SHUFFLE(1, 0, 1, 0));

        target->column[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
}

INTERNAL void __cdecl transpose43(__4_columnset *source, __3_columnset *target)
{
        __m128 t0 = _mm_shuffle_ps(ALIGN(source->column[0]),
                                   ALIGN(source->column[1]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t1 = _mm_shuffle_ps(ALIGN(source->column[0]),
                                   ALIGN(source->column[1]),
                                   _MM_SHUFFLE(3, 2, 3, 2));
        __m128 t2 = _mm_shuffle_ps(ALIGN(source->column[2]),
                                   ALIGN(source->column[3]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t3 = _mm_shuffle_ps(ALIGN(source->column[2]),
                                   ALIGN(source->column[3]),
                                   _MM_SHUFFLE(3, 2, 3, 2));

        target->column[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
        target->column[2] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(2, 0, 2, 0));
}

INTERNAL void __cdecl transpose44(__4_columnset *source, __4_columnset *target)
{
        __m128 t0 = _mm_shuffle_ps(ALIGN(source->column[0]),
                                   ALIGN(source->column[1]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t1 = _mm_shuffle_ps(ALIGN(source->column[0]),
                                   ALIGN(source->column[1]),
                                   _MM_SHUFFLE(3, 2, 3, 2));
        __m128 t2 = _mm_shuffle_ps(ALIGN(source->column[2]),
                                   ALIGN(source->column[3]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t3 = _mm_shuffle_ps(ALIGN(source->column[2]),
                                   ALIGN(source->column[3]),
                                   _MM_SHUFFLE(3, 2, 3, 2));

        target->column[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
        target->column[2] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[3] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(3, 1, 3, 1));
}

INTERNAL float __cdecl det_mat4(__4_columnset *matPtr, __4_columnset *transpose)
{
        __m128 mc0 = ALIGN(matPtr->column[0]);
        __m128 mc1 = ALIGN(matPtr->column[1]);
        __m128 mc2 = ALIGN(matPtr->column[2]);
        __m128 mc3 = ALIGN(matPtr->column[3]);

        __m128 cogeneration = _mm_set_ps(det_mat3_internal(mc0, mc1, mc2),
                                         det_mat3_internal(mc0, mc1, mc3),
                                         det_mat3_internal(mc0, mc2, mc3),
                                         det_mat3_internal(mc1, mc2, mc3));
        __m128 retvec = _mm_mul_ps(ALIGN(transpose->column[3]), cogeneration);
        retvec = _mm_hsub_ps(retvec, retvec);
        retvec = _mm_hadd_ps(retvec, retvec);
        return -retvec.m128_f32[0];
}
