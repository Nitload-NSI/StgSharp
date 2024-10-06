#include "StgSharpC.h"
#include "ssc_intrinsic.h"

INTERNAL void __cdecl transpose23(__2_columnset *source,
                                        __3_columnset *target)
{
        __m128 t0 = _mm_shuffle_ps(ALIGN(source->column[0]),
                                   ALIGN(source->column[1]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t1 = _mm_shuffle_ps(ALIGN(source->column[0]),
                                   ALIGN(source->column[1]),
                                   _MM_SHUFFLE(3, 2, 3, 2));

        target->column[0] =
                _mm_shuffle_ps(t0, zeroVec, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[1] =
                _mm_shuffle_ps(t0, zeroVec, _MM_SHUFFLE(3, 1, 3, 1));
        target->column[2] =
                _mm_shuffle_ps(t1, zeroVec, _MM_SHUFFLE(2, 0, 2, 0));
}

INTERNAL void __cdecl transpose24(__2_columnset *source,
                                        __4_columnset *target)
{
        __m128 t0 = _mm_shuffle_ps(ALIGN(source->column[0]),
                                   ALIGN(source->column[1]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t1 = _mm_shuffle_ps(ALIGN(source->column[0]),
                                   ALIGN(source->column[1]),
                                   _MM_SHUFFLE(3, 2, 3, 2));

        target->column[0] =
                _mm_shuffle_ps(t0, zeroVec, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[1] =
                _mm_shuffle_ps(t0, zeroVec, _MM_SHUFFLE(3, 1, 3, 1));
        target->column[2] =
                _mm_shuffle_ps(t1, zeroVec, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[3] =
                _mm_shuffle_ps(t1, zeroVec, _MM_SHUFFLE(3, 1, 3, 1));
}
