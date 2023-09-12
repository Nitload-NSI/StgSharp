#include "StgSharpGraphic.h"




SSGC_API _inline void __cdecl transpose2to3(matrix2map* source, matrix3map* target)
{
	__m128 t0 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(1, 0, 1, 0));
	__m128 t1 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(3, 2, 3, 2));

	target->colum[0] = _mm_shuffle_ps(t0, *defaultVecPtr, _MM_SHUFFLE(2, 0, 2, 0));
	target->colum[1] = _mm_shuffle_ps(t0, *defaultVecPtr, _MM_SHUFFLE(3, 1, 3, 1));
	target->colum[2] = _mm_shuffle_ps(t1, *defaultVecPtr, _MM_SHUFFLE(2, 0, 2, 0));
}

SSGC_API _inline void __cdecl transpose2to4(matrix2map* source, matrix4map* target)
{
    __m128 t0 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(1, 0, 1, 0));
    __m128 t1 = _mm_shuffle_ps(ALIGN(source->colum[0]), ALIGN(source->colum[1]), _MM_SHUFFLE(3, 2, 3, 2));

    target->colum[0] = _mm_shuffle_ps(t0, *defaultVecPtr, _MM_SHUFFLE(2, 0, 2, 0));
    target->colum[1] = _mm_shuffle_ps(t0, *defaultVecPtr, _MM_SHUFFLE(3, 1, 3, 1));
    target->colum[2] = _mm_shuffle_ps(t1, *defaultVecPtr, _MM_SHUFFLE(2, 0, 2, 0));
    target->colum[3] = _mm_shuffle_ps(t1, *defaultVecPtr, _MM_SHUFFLE(3, 1, 3, 1));
}

SSGC_API _inline void __cdecl deinit_mat2(matrix2map* mat)
{
	_aligned_free(mat);
}
