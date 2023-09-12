#include "StgSharpGraphic.h"
#include <stdio.h>
#include <pmmintrin.h>




SSGC_API __inline void __cdecl transpose4to2(matrix4map* source, matrix4map* target)
{
	__m128 t0 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(1, 0, 1, 0));
	__m128 t2 = _mm_shuffle_ps(ALIGN(source->colum[2]), ALIGN(source->colum[3]), _MM_SHUFFLE(1, 0, 1, 0));

	target->colum[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
	target->colum[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
}

SSGC_API __inline void __cdecl transpose4to4(matrix4map* source, matrix4map* target)
{
	__m128 t0 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(1, 0, 1, 0));
	__m128 t1 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(3, 2, 3, 2));
	__m128 t2 = _mm_shuffle_ps(ALIGN(source->colum[2]), ALIGN(source->colum[3]), _MM_SHUFFLE(1, 0, 1, 0));
	__m128 t3 = _mm_shuffle_ps(ALIGN(source->colum[2]), ALIGN(source->colum[3]), _MM_SHUFFLE(3, 2, 3, 2));

	target->colum[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
	target->colum[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
	target->colum[2] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(2, 0, 2, 0));
	target->colum[3] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(3, 1, 3, 1));

}

SSGC_API __inline void __cdecl transpose4to3(matrix4map* source, matrix3map* target)
{
	__m128 t0 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(1, 0, 1, 0));
	__m128 t1 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(3, 2, 3, 2));
	__m128 t2 = _mm_shuffle_ps(ALIGN(source->colum[2]), ALIGN(source->colum[3]), _MM_SHUFFLE(1, 0, 1, 0));
	__m128 t3 = _mm_shuffle_ps(ALIGN(source->colum[2]), ALIGN(source->colum[3]), _MM_SHUFFLE(3, 2, 3, 2));

	target->colum[0] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(2, 0, 2, 0));
	target->colum[1] = _mm_shuffle_ps(t0, t2, _MM_SHUFFLE(3, 1, 3, 1));
	target->colum[2] = _mm_shuffle_ps(t1, t3, _MM_SHUFFLE(2, 0, 2, 0));

}



SSGC_API __inline float __cdecl det_mat4(matrix4map* matPtr, matrix4map* transpose)
{
	__m128 mc0 = ALIGN(matPtr->colum[0]);
	__m128 mc1 = ALIGN(matPtr->colum[1]);
	__m128 mc2 = ALIGN(matPtr->colum[2]);
	__m128 mc3 = ALIGN(matPtr->colum[3]);

	__m128 cogeneration = _mm_set_ps(
		det_mat3_internal(mc0, mc1, mc2),
		det_mat3_internal(mc0, mc1, mc3),
		det_mat3_internal(mc0, mc2, mc3),
		det_mat3_internal(mc1, mc2, mc3)
		);
	__m128 retvec = _mm_mul_ps(ALIGN(transpose->colum[3]),cogeneration);
	retvec = _mm_hsub_ps(retvec,retvec);
	retvec = _mm_hadd_ps(retvec,retvec);
	return -retvec.m128_f32[0];

}

SSGC_API __inline void __cdecl deinit_mat4(matrix4map* matPtr)
{
	_aligned_free(matPtr);
}