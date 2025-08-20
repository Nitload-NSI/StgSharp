#include "StgSharpNative.h"
#include "sn_intrinsic.h"
#include <stdio.h>

INTERNAL void _cdecl transpose32(__3_columnset *source, __2_columnset *target)
{
        __m128 t0 = _mm_shuffle_ps(ALIGN(source->column[0]), ALIGN(source->column[1]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t2 = _mm_shuffle_ps(ALIGN(source->column[2]), zero_vec, _MM_SHUFFLE(1, 0, 1, 0));

        target->column[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
}

INTERNAL void _cdecl transpose33(__3_columnset *source, __3_columnset *target)
{
        __m128 t0 = _mm_shuffle_ps(ALIGN(source->column[0]), ALIGN(source->column[1]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t1 = _mm_shuffle_ps(ALIGN(source->column[0]), ALIGN(source->column[1]),
                                   _MM_SHUFFLE(3, 2, 3, 2));
        __m128 t2 = _mm_shuffle_ps(ALIGN(source->column[2]), zero_vec, _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t3 = _mm_shuffle_ps(ALIGN(source->column[2]), zero_vec, _MM_SHUFFLE(3, 2, 3, 2));

        target->column[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
        target->column[2] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(2, 0, 2, 0));
}

INTERNAL void _cdecl transpose34(__3_columnset *source, __4_columnset *target)
{
        __m128 t0 = _mm_shuffle_ps(ALIGN(source->column[0]), ALIGN(source->column[1]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t1 = _mm_shuffle_ps(ALIGN(source->column[0]), ALIGN(source->column[1]),
                                   _MM_SHUFFLE(3, 2, 3, 2));
        __m128 t2 = _mm_shuffle_ps(ALIGN(source->column[2]), zero_vec, _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t3 = _mm_shuffle_ps(ALIGN(source->column[2]), zero_vec, _MM_SHUFFLE(3, 2, 3, 2));

        target->column[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
        target->column[2] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[3] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(3, 1, 3, 1));
}

INTERNAL float _cdecl det_mat3_internal(__m128 const c0, __m128 const c1, __m128 const c2)
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

        __m128 ret = _mm_hadd_ps(cache, cache); //element 1 add element 2
        ret = _mm_add_ps(ret, cache); //(e1 add e2) add element 3

        return ret.m128_f32[2];
}

INTERNAL float _cdecl det_mat3(__3_columnset *source, __3_columnset *transpose)
{
        return det_mat3_internal(source->column[0], source->column[1], source->column[2]);
}

#pragma region add

INTERNAL void __cdecl addmatrix3_sse(__3_columnset *left, __3_columnset *right, __3_columnset *ans)
{
        __m128 temp0 = _mm_add_ps(ALIGN(left->column[0]), ALIGN(right->column[0]));
        __m128 temp1 = _mm_add_ps(ALIGN(left->column[1]), ALIGN(right->column[1]));
        __m128 temp2 = _mm_add_ps(ALIGN(left->column[2]), ALIGN(right->column[2]));

        _mm_storeu_ps(&ans->m[0][0], temp0);
        _mm_storeu_ps(&ans->m[0][1], temp1);
        _mm_storeu_ps(&ans->m[0][2], temp2);
}

INTERNAL void __cdecl addmatrix3_avx(__3_columnset *left, __3_columnset *right, __3_columnset *ans)
{
        __m256 temp0 = _mm256_add_ps(ALIGN256(left->part), ALIGN256(right->part));
        __m128 temp1 = _mm_add_ps(ALIGN(left->column[2]), ALIGN(right->column[2]));

        _mm256_storeu_ps(&ans->m[0][0], temp0);
        _mm_storeu_ps(&ans->m[0][2], temp1);
}

INTERNAL void __cdecl addmatrix3_512(__3_columnset *left, __3_columnset *right, __3_columnset *ans)
{
        __m512 temp = _mm512_add_ps(_mm512_loadu_ps(left), _mm512_loadu_ps(right));
        _mm512_mask_store_ps(ans, 0x0fff, temp);
}

INTERNAL void __cdecl submatrix3_sse(__3_columnset *left, __3_columnset *right, __3_columnset *ans)
{
        __m128 temp0 = _mm_sub_ps(ALIGN(left->column[0]), ALIGN(right->column[0]));
        __m128 temp1 = _mm_sub_ps(ALIGN(left->column[1]), ALIGN(right->column[1]));
        __m128 temp2 = _mm_sub_ps(ALIGN(left->column[2]), ALIGN(right->column[2]));

        _mm_storeu_ps(&ans->m[0][0], temp0);
        _mm_storeu_ps(&ans->m[0][1], temp1);
        _mm_storeu_ps(&ans->m[0][2], temp2);
}

INTERNAL void __cdecl submatrix3_avx(__3_columnset *left, __3_columnset *right, __3_columnset *ans)
{
        __m256 temp0 = _mm256_sub_ps(ALIGN256(left->part), ALIGN256(right->part));
        __m128 temp1 = _mm_sub_ps(ALIGN(left->column[2]), ALIGN(right->column[2]));

        _mm256_storeu_ps(&ans->m[0][0], temp0);
        _mm_storeu_ps(&ans->m[0][2], temp1);
}

INTERNAL void __cdecl submatrix3_512(__3_columnset *left, __3_columnset *right, __3_columnset *ans)
{
        __m512 temp = _mm512_sub_ps(_mm512_loadu_ps(left), _mm512_loadu_ps(right));
        _mm512_mask_store_ps(ans, 0x0fff, temp);
}

#pragma endregion
