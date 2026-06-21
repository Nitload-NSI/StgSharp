#include "sn_target.h"

#if SN_IS_ARCH(SN_ARCH_X86_64)

#include "sn_intrinsic.context.x86.h"

/*
* pack left matrix data to L4 buffer
* 
* CONVENSIONS:
* 
* buffer_1 is pointer to original colum, 
* offset_1 is the offset to the original column, 
* count_1 is the count can be packed
* 
* C# shceduler is responsible for calculating the offset and count based on the actual buffer size and matrix dimension, 
* and make sure that count_1 will not exceeds lenght of the column
* 
* scalar pack carries column length and offset data:
* [column length, empty, empty, empty]
*/
SN_MK_PROC_DECL_STD(float, sse, , fma_pack_left)
{
        __mk_param_std(RIGHT_ANS_SCALAR, float);
        // for SSE branch, no need for rounding, a direct copy is sufficient as long as the buffer is large enough to hold the packed data
        memcpy(ans, right + offset_1, sizeof(MAT_KERNEL(float)) * count_2);
}

SN_MK_PROC_DECL_STD(float, avx, , fma_pack_left)
{
        __mk_param_std(RIGHT_ANS_SCALAR, float);
        size_t i = 0;
        for (; i < (count_1 & (~0x1)); i += 2) {
                register __m256 ker_0_left = _mm256_load_ps((float *)&right[offset_1 + i].y[0]);
                register __m256 ker_0_right = _mm256_load_ps((float *)&right[offset_1 + i].y[1]);
                register __m256 ker_1_left = _mm256_load_ps((float *)&right[offset_1 + i].y[0]);
                register __m256 ker_1_right = _mm256_load_ps((float *)&right[offset_1 + i].y[1]);
                register __m256 col_0 = _mm256_unpacklo_ps(ker_0_left, ker_1_left);
                register __m256 col_1 = _mm256_unpackhi_ps(ker_0_left, ker_1_left);
                register __m256 col_2 = _mm256_unpacklo_ps(ker_0_right, ker_1_right);
                register __m256 col_3 = _mm256_unpackhi_ps(ker_0_right, ker_1_right);

                _mm256_store_ps((float *)&ans[i].y[0], col_0);
                _mm256_store_ps((float *)&ans[i].y[1], col_1);
                _mm256_store_ps((float *)&ans[i].y[2], col_2);
                _mm256_store_ps((float *)&ans[i].y[3], col_3);
        }
        if (count_1 & 0x1) {
                register __m256 ker_0_left = _mm256_load_ps((float *)&right[offset_1 + i].y[0]);
                register __m256 ker_0_right = _mm256_load_ps((float *)&right[offset_1 + i].y[1]);
                register __m256 zero = _mm256_setzero_ps();
                register __m256 col_0 = _mm256_unpacklo_ps(ker_0_left, zero);
                register __m256 col_1 = _mm256_unpackhi_ps(ker_0_left, zero);
                register __m256 col_2 = _mm256_unpacklo_ps(ker_0_right, zero);
                register __m256 col_3 = _mm256_unpackhi_ps(ker_0_right, zero);

                _mm256_store_ps((float *)&ans[i].y[0], col_0);
                _mm256_store_ps((float *)&ans[i].y[1], col_1);
                _mm256_store_ps((float *)&ans[i].y[2], col_2);
                _mm256_store_ps((float *)&ans[i].y[3], col_3);
        }
}

SN_MK_PROC_DECL_STD(float, 512, , fma_pack_left)
{
        __mk_param_std(RIGHT_ANS_SCALAR, float);
        size_t i = 0;
        register __m512 ker_0;
        register __m512 ker_1;
        register __m512 ker_2;
        register __m512 ker_3;
        register __m512 col_0;
        register __m512 col_1;
        register __m512 col_2;
        register __m512 col_3;
        for (; i < (count_1 & (~0x3)); i += 4) {
                ker_0 = _mm512_load_ps((float *)&right[offset_1 + i].z[0]);
                ker_1 = _mm512_load_ps((float *)&right[offset_1 + i].z[1]);
                ker_2 = _mm512_load_ps((float *)&right[offset_1 + i].z[2]);
                ker_3 = _mm512_load_ps((float *)&right[offset_1 + i].z[3]);
                col_0 = _mm512_unpacklo_ps(ker_0, ker_2);
                col_1 = _mm512_unpackhi_ps(ker_0, ker_2);
                col_2 = _mm512_unpacklo_ps(ker_1, ker_3);
                col_3 = _mm512_unpackhi_ps(ker_1, ker_3);

                _mm512_store_ps((float *)&ans[i].z[0],
                                _mm512_shuffle_f32x4(col_0, col_2, _MM_SHUFFLE(1, 0, 1, 0)));
                _mm512_store_ps((float *)&ans[i].z[1],
                                _mm512_shuffle_f32x4(col_0, col_2, _MM_SHUFFLE(3, 2, 3, 2)));
                _mm512_store_ps((float *)&ans[i].z[2],
                                _mm512_shuffle_f32x4(col_1, col_3, _MM_SHUFFLE(1, 0, 1, 0)));
                _mm512_store_ps((float *)&ans[i].z[3],
                                _mm512_shuffle_f32x4(col_1, col_3, _MM_SHUFFLE(3, 2, 3, 2)));
        }
        switch (count_1 & 0x3) {
        case 1:
                ker_0 = _mm512_load_ps((float *)&right[offset_1 + i].z[0]);
                col_0 = _mm512_castps128_ps512(_mm512_extractf32x4_ps(ker_0, 0));
                col_1 = _mm512_castps128_ps512(_mm512_extractf32x4_ps(ker_0, 1));
                col_2 = _mm512_castps128_ps512(_mm512_extractf32x4_ps(ker_0, 2));
                col_3 = _mm512_castps128_ps512(_mm512_extractf32x4_ps(ker_0, 3));

                _mm512_store_ps((float *)&ans[i].z[0], col_0);
                _mm512_store_ps((float *)&ans[i].z[1], col_1);
                _mm512_store_ps((float *)&ans[i].z[2], col_2);
                _mm512_store_ps((float *)&ans[i].z[3], col_3);
        case 2:
                ker_0 = _mm512_load_ps((float *)&right[offset_1 + i].z[0]);
                ker_1 = _mm512_load_ps((float *)&right[offset_1 + i].z[1]);
                col_0 = _mm512_castps256_ps512(_mm512_extractf32x8_ps(ker_0, 0));
                col_1 = _mm512_castps256_ps512(_mm512_extractf32x8_ps(ker_0, 1));
                col_2 = _mm512_castps256_ps512(_mm512_extractf32x8_ps(ker_1, 0));
                col_3 = _mm512_castps256_ps512(_mm512_extractf32x8_ps(ker_1, 1));

                _mm512_store_ps((float *)&ans[i].z[0], col_0);
                _mm512_store_ps((float *)&ans[i].z[1], col_1);
                _mm512_store_ps((float *)&ans[i].z[2], col_2);
                _mm512_store_ps((float *)&ans[i].z[3], col_3);
        case 3:
                ker_0 = _mm512_load_ps((float *)&right[offset_1 + i].z[0]);
                ker_1 = _mm512_load_ps((float *)&right[offset_1 + i].z[1]);
                ker_2 = _mm512_load_ps((float *)&right[offset_1 + i].z[2]);
                ker_3 = _mm512_setzero_ps();
                col_0 = _mm512_unpacklo_ps(ker_0, ker_2);
                col_1 = _mm512_unpackhi_ps(ker_0, ker_2);
                col_2 = _mm512_unpacklo_ps(ker_1, ker_3);
                col_3 = _mm512_unpackhi_ps(ker_1, ker_3);

                _mm512_store_ps((float *)&ans[i].z[0],
                                _mm512_shuffle_f32x4(col_0, col_2, _MM_SHUFFLE(1, 0, 1, 0)));
                _mm512_store_ps((float *)&ans[i].z[1],
                                _mm512_shuffle_f32x4(col_0, col_2, _MM_SHUFFLE(3, 2, 3, 2)));
                _mm512_store_ps((float *)&ans[i].z[2],
                                _mm512_shuffle_f32x4(col_1, col_3, _MM_SHUFFLE(1, 0, 1, 0)));
                _mm512_store_ps((float *)&ans[i].z[3],
                                _mm512_shuffle_f32x4(col_1, col_3, _MM_SHUFFLE(3, 2, 3, 2)));
        default:
                break;
        }
}

/*
* pack right matrix data to L4 buffer
* 
* CONVENSIONS:
* 
* buffer_1 is pointer to original row, 
* offset_1 is the offset to the original row, 
* count_1 is the count can be packed
* 
* C# shceduler is responsible for calculating the offset and count based on the actual buffer size and matrix dimension, 
* and make sure that count_1 will not exceeds lenght of the row
* 
* scalar pack carries column length and offset data:
* [column length, empty, empty, empty]
*/
SN_MK_PROC_DECL_STD(float, sse, , fma_pack_right)
{
        __mk_param_std(RIGHT_ANS_SCALAR, float);
        int col_length = scalar.i32[0];
        for (size_t i = 0; i < count_1; i++) {
                ans[i] = right[(offset_1 + i) * col_length];
        }
}

SN_MK_PROC_DECL_STD(float, avx, , fma_pack_right)
{
        __mk_param_std(RIGHT_ANS_SCALAR, float);
        int col_length = scalar.i32[0];
        size_t i = 0;
        for (; i < (count_1 & (~0x1)); i += 2) {
                register __m256 ker_0_left =
                        _mm256_load_ps((float *)&right[(offset_1 + i) * col_length].y[0]);
                register __m256 ker_0_right =
                        _mm256_load_ps((float *)&right[(offset_1 + i) * col_length].y[1]);
                register __m256 ker_1_left =
                        _mm256_load_ps((float *)&right[(offset_1 + i + 1) * col_length].y[0]);
                register __m256 ker_1_right =
                        _mm256_load_ps((float *)&right[(offset_1 + i + 1) * col_length].y[1]);
                register __m256 col_0 = _mm256_unpacklo_ps(ker_0_left, ker_1_left);
                register __m256 col_1 = _mm256_unpackhi_ps(ker_0_left, ker_1_left);
                register __m256 col_2 = _mm256_unpacklo_ps(ker_0_right, ker_1_right);
                register __m256 col_3 = _mm256_unpackhi_ps(ker_0_right, ker_1_right);

                _mm256_store_ps((float *)&ans[i].y[0], col_0);
                _mm256_store_ps((float *)&ans[i].y[1], col_1);
                _mm256_store_ps((float *)&ans[i].y[2], col_2);
                _mm256_store_ps((float *)&ans[i].y[3], col_3);
        }
        if (count_1 & 0x1) {
                register __m256 ker_0_left =
                        _mm256_load_ps((float *)&right[(offset_1 + i) * col_length].y[0]);
                register __m256 ker_0_right =
                        _mm256_load_ps((float *)&right[(offset_1 + i) * col_length].y[1]);
                register __m256 zero = _mm256_setzero_ps();
                register __m256 col_0 = _mm256_unpacklo_ps(ker_0_left, zero);
                register __m256 col_1 = _mm256_unpackhi_ps(ker_0_left, zero);
                register __m256 col_2 = _mm256_unpacklo_ps(ker_0_right, zero);
                register __m256 col_3 = _mm256_unpackhi_ps(ker_0_right, zero);

                _mm256_store_ps((float *)&ans[i].y[0], col_0);
                _mm256_store_ps((float *)&ans[i].y[1], col_1);
                _mm256_store_ps((float *)&ans[i].y[2], col_2);
                _mm256_store_ps((float *)&ans[i].y[3], col_3);
        }
}

SN_MK_PROC_DECL_STD(float, 512, , fma_pack_right)
{
        __mk_param_std(RIGHT_ANS_SCALAR, float);
        int col_length = scalar.i32[0];
        size_t i = 0;
        register __m512 ker_0;
        register __m512 ker_1;
        register __m512 ker_2;
        register __m512 ker_3;
        register __m512 col_0;
        register __m512 col_1;
        register __m512 col_2;
        register __m512 col_3;
        for (; i < (count_1 & (~0x3)); i += 4) {
                ker_0 = _mm512_load_ps((float *)&right[(offset_1 + i + 0) * col_length].z[0]);
                ker_1 = _mm512_load_ps((float *)&right[(offset_1 + i + 1) * col_length].z[0]);
                ker_2 = _mm512_load_ps((float *)&right[(offset_1 + i + 2) * col_length].z[0]);
                ker_3 = _mm512_load_ps((float *)&right[(offset_1 + i + 3) * col_length].z[0]);
                col_0 = _mm512_unpacklo_ps(ker_0, ker_2);
                col_1 = _mm512_unpackhi_ps(ker_0, ker_2);
                col_2 = _mm512_unpacklo_ps(ker_1, ker_3);
                col_3 = _mm512_unpackhi_ps(ker_1, ker_3);

                _mm512_store_ps((float *)&ans[i].z[0],
                                _mm512_shuffle_f32x4(col_0, col_2, _MM_SHUFFLE(1, 0, 1, 0)));
                _mm512_store_ps((float *)&ans[i].z[1],
                                _mm512_shuffle_f32x4(col_0, col_2, _MM_SHUFFLE(3, 2, 3, 2)));
                _mm512_store_ps((float *)&ans[i].z[2],
                                _mm512_shuffle_f32x4(col_1, col_3, _MM_SHUFFLE(1, 0, 1, 0)));
                _mm512_store_ps((float *)&ans[i].z[3],
                                _mm512_shuffle_f32x4(col_1, col_3, _MM_SHUFFLE(3, 2, 3, 2)));
        }
        switch (count_1 & 0x3) {
        case 1:
                ker_0 = _mm512_load_ps((float *)&right[(offset_1 + i) * col_length].z[0]);
                col_0 = _mm512_castps128_ps512(_mm512_extractf32x4_ps(ker_0, 0));
                col_1 = _mm512_castps128_ps512(_mm512_extractf32x4_ps(ker_0, 1));
                col_2 = _mm512_castps128_ps512(_mm512_extractf32x4_ps(ker_0, 2));
                col_3 = _mm512_castps128_ps512(_mm512_extractf32x4_ps(ker_0, 3));

                _mm512_store_ps((float *)&ans[i].z[0], col_0);
                _mm512_store_ps((float *)&ans[i].z[1], col_1);
                _mm512_store_ps((float *)&ans[i].z[2], col_2);
                _mm512_store_ps((float *)&ans[i].z[3], col_3);
        case 2:
                ker_0 = _mm512_load_ps((float *)&right[(offset_1 + i + 0) * col_length].z[0]);
                ker_1 = _mm512_load_ps((float *)&right[(offset_1 + i + 1) * col_length].z[1]);
                col_0 = _mm512_castps256_ps512(_mm512_extractf32x8_ps(ker_0, 0));
                col_1 = _mm512_castps256_ps512(_mm512_extractf32x8_ps(ker_0, 1));
                col_2 = _mm512_castps256_ps512(_mm512_extractf32x8_ps(ker_1, 0));
                col_3 = _mm512_castps256_ps512(_mm512_extractf32x8_ps(ker_1, 1));

                _mm512_store_ps((float *)&ans[i].z[0], col_0);
                _mm512_store_ps((float *)&ans[i].z[1], col_1);
                _mm512_store_ps((float *)&ans[i].z[2], col_2);
                _mm512_store_ps((float *)&ans[i].z[3], col_3);
        case 3:
                ker_0 = _mm512_load_ps((float *)&right[(offset_1 + i + 0) * col_length].z[0]);
                ker_1 = _mm512_load_ps((float *)&right[(offset_1 + i + 1) * col_length].z[1]);
                ker_2 = _mm512_load_ps((float *)&right[(offset_1 + i + 2) * col_length].z[2]);
                ker_3 = _mm512_setzero_ps();
                col_0 = _mm512_unpacklo_ps(ker_0, ker_2);
                col_1 = _mm512_unpackhi_ps(ker_0, ker_2);
                col_2 = _mm512_unpacklo_ps(ker_1, ker_3);
                col_3 = _mm512_unpackhi_ps(ker_1, ker_3);

                _mm512_store_ps((float *)&ans[i].z[0],
                                _mm512_shuffle_f32x4(col_0, col_2, _MM_SHUFFLE(1, 0, 1, 0)));
                _mm512_store_ps((float *)&ans[i].z[1],
                                _mm512_shuffle_f32x4(col_0, col_2, _MM_SHUFFLE(3, 2, 3, 2)));
                _mm512_store_ps((float *)&ans[i].z[2],
                                _mm512_shuffle_f32x4(col_1, col_3, _MM_SHUFFLE(1, 0, 1, 0)));
                _mm512_store_ps((float *)&ans[i].z[3],
                                _mm512_shuffle_f32x4(col_1, col_3, _MM_SHUFFLE(3, 2, 3, 2)));
        default:
                break;
        }
}


#endif