
#include "StgSharpC.h"
#include "ssc_intrinsic.h"
#include <stdio.h>
#include <pmmintrin.h>

INTERNAL void __cdecl transpose42(__4_columnset const *source, __2_columnset *target)
{
        __m128 t0 = _mm_shuffle_ps(ALIGN(source->column[0]), ALIGN(source->column[1]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t2 = _mm_shuffle_ps(ALIGN(source->column[2]), ALIGN(source->column[3]),
                                   _MM_SHUFFLE(1, 0, 1, 0));

        target->column[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
}

INTERNAL void __cdecl transpose43(__4_columnset const *source, __3_columnset *target)
{
        __m128 t0 = _mm_shuffle_ps(ALIGN(source->column[0]), ALIGN(source->column[1]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t1 = _mm_shuffle_ps(ALIGN(source->column[0]), ALIGN(source->column[1]),
                                   _MM_SHUFFLE(3, 2, 3, 2));
        __m128 t2 = _mm_shuffle_ps(ALIGN(source->column[2]), ALIGN(source->column[3]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t3 = _mm_shuffle_ps(ALIGN(source->column[2]), ALIGN(source->column[3]),
                                   _MM_SHUFFLE(3, 2, 3, 2));

        target->column[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
        target->column[2] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(2, 0, 2, 0));
}

INTERNAL void __cdecl transpose44(__4_columnset const *source, __4_columnset *target)
{
        __m128 t0 = _mm_shuffle_ps(ALIGN(source->column[0]), ALIGN(source->column[1]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t1 = _mm_shuffle_ps(ALIGN(source->column[0]), ALIGN(source->column[1]),
                                   _MM_SHUFFLE(3, 2, 3, 2));
        __m128 t2 = _mm_shuffle_ps(ALIGN(source->column[2]), ALIGN(source->column[3]),
                                   _MM_SHUFFLE(1, 0, 1, 0));
        __m128 t3 = _mm_shuffle_ps(ALIGN(source->column[2]), ALIGN(source->column[3]),
                                   _MM_SHUFFLE(3, 2, 3, 2));

        target->column[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
        target->column[2] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(2, 0, 2, 0));
        target->column[3] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(3, 1, 3, 1));
}

INTERNAL float __cdecl det_mat4(__4_columnset const *mat_ptr, __4_columnset const *transpose)
{
        __m128 mc0 = ALIGN(mat_ptr->column[0]);
        __m128 mc1 = ALIGN(mat_ptr->column[1]);
        __m128 mc2 = ALIGN(mat_ptr->column[2]);
        __m128 mc3 = ALIGN(mat_ptr->column[3]);

        __m128 cogeneration =
                _mm_set_ps(det_mat3_internal(mc0, mc1, mc2), det_mat3_internal(mc0, mc1, mc3),
                           det_mat3_internal(mc0, mc2, mc3), det_mat3_internal(mc1, mc2, mc3));
        __m128 retvec = _mm_mul_ps(ALIGN(transpose->column[3]), cogeneration);
        retvec = _mm_hsub_ps(retvec, retvec);
        retvec = _mm_hadd_ps(retvec, retvec);
        return -retvec.m128_f32[0];
}

INTERNAL void SSCDECL addmatrix4_sse(__4_columnset *left, __4_columnset *right, __4_columnset *ans)
{
        ans->column[0] = _mm_add_ps(ALIGN(left->column[0]), ALIGN(right->column[0]));
        ans->column[1] = _mm_add_ps(ALIGN(left->column[1]), ALIGN(right->column[1]));
        ans->column[2] = _mm_add_ps(ALIGN(left->column[2]), ALIGN(right->column[2]));
        ans->column[3] = _mm_add_ps(ALIGN(left->column[3]), ALIGN(right->column[3]));
}

INTERNAL void SSCDECL addmatrix4_avx(__4_columnset *left, __4_columnset *right, __4_columnset *ans)
{
        ans->half[0] = _mm256_add_ps(ALIGN256(left->half[0]), ALIGN256(right->half[0]));
        ans->half[1] = _mm256_add_ps(ALIGN256(left->half[1]), ALIGN256(right->half[1]));
}

INTERNAL void SSCDECL addmatrix4_512(__4_columnset *left, __4_columnset *right, __4_columnset *ans)
{
        ans->stream = _mm512_add_ps(ALIGN512(left->stream), ALIGN512(right->stream));
}

INTERNAL void SSCDECL submatrix4_sse(__4_columnset *left, __4_columnset *right, __4_columnset *ans)
{
        ans->column[0] = _mm_sub_ps(ALIGN(left->column[0]), ALIGN(right->column[0]));
        ans->column[1] = _mm_sub_ps(ALIGN(left->column[1]), ALIGN(right->column[1]));
        ans->column[2] = _mm_sub_ps(ALIGN(left->column[2]), ALIGN(right->column[2]));
        ans->column[3] = _mm_sub_ps(ALIGN(left->column[3]), ALIGN(right->column[3]));
}

INTERNAL void SSCDECL submatrix4_avx(__4_columnset *left, __4_columnset *right, __4_columnset *ans)
{
        ans->half[0] = _mm256_sub_ps(ALIGN256(left->half[0]), ALIGN256(right->half[0]));
        ans->half[1] = _mm256_sub_ps(ALIGN256(left->half[1]), ALIGN256(right->half[1]));
}

INTERNAL void SSCDECL submatrix4_512(__4_columnset *left, __4_columnset *right, __4_columnset *ans)
{
        ans->stream = _mm512_sub_ps(ALIGN512(left->stream), ALIGN512(right->stream));
}  
