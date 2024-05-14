#include "StgSharpC.h"
#include "ssgc_internal.h"
#include <stdio.h>

SSCAPI __inline void _cdecl transpose3to2(matrix3map* source, matrix2map* target)
{
    __m128 t0 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(1, 0, 1, 0));
    __m128 t2 = _mm_shuffle_ps(ALIGN(source->colum[2]), zeroVec, _MM_SHUFFLE(1, 0, 1, 0));

    target->colum[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
    target->colum[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
}

SSCAPI __inline void _cdecl transpose3to3(matrix3map* source, matrix3map* target)
{
    __m128 t0 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(1, 0, 1, 0));
    __m128 t1 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(3, 2, 3, 2));
    __m128 t2 = _mm_shuffle_ps(ALIGN(source->colum[2]), zeroVec, _MM_SHUFFLE(1, 0, 1, 0));
    __m128 t3 = _mm_shuffle_ps(ALIGN(source->colum[2]), zeroVec, _MM_SHUFFLE(3, 2, 3, 2));

    target->colum[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
    target->colum[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
    target->colum[2] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(2, 0, 2, 0));
}

SSCAPI __inline void _cdecl transpose3to4(matrix3map* source, matrix4map* target)
{
    __m128 t0 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(1, 0, 1, 0));
    __m128 t1 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(3, 2, 3, 2));
    __m128 t2 = _mm_shuffle_ps(ALIGN(source->colum[2]), zeroVec, _MM_SHUFFLE(1, 0, 1, 0));
    __m128 t3 = _mm_shuffle_ps(ALIGN(source->colum[2]), zeroVec, _MM_SHUFFLE(3, 2, 3, 2));

    target->colum[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
    target->colum[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
    target->colum[2] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(2, 0, 2, 0));
    target->colum[3] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(3, 1, 3, 1));
}

SSCAPI _inline float _cdecl det_mat3_internal(__m128 const c0, __m128 const c1, __m128 const c2)
{
    //let adding row aligned
    __m128 addc1 = _mm_shuffle_ps(c1, c1, _MM_SHUFFLE(3, 0, 2, 1));
    __m128 addc2 = _mm_shuffle_ps(c2, c2, _MM_SHUFFLE(3, 1, 0, 2));

    //let sub row aligned
    __m128 subc1 = _mm_shuffle_ps(c1, c1, _MM_SHUFFLE(3, 1, 0, 2));
    __m128 subc2 = _mm_shuffle_ps(c2, c2, _MM_SHUFFLE(3, 0, 2, 1));

    //multple all add row
    __m128 cache = _mm_mul_ps(c0, addc1);
    cache = _mm_mul_ps(cache, addc2);

    //multiple all sub row
    __m128 sub = _mm_mul_ps(c0, subc1);
    sub = _mm_mul_ps(sub, subc2);

    //main-corner sub sub-corner
    cache = _mm_sub_ps(cache, sub);

    __m128 ret = _mm_hadd_ps(cache, cache);    //element 1 add element 2
    ret = _mm_add_ps(ret, cache);    //(e1 add e2) add element 3

    return ret.m128_f32[2];
}

SSCAPI _inline float _cdecl det_mat3(matrix3map* mat)
{
    return det_mat3_internal(
        mat->colum[0],
        mat->colum[1],
        mat->colum[2]
    );
}