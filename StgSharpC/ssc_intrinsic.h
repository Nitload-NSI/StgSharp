#ifdef _MSC_VER
#pragma once
#endif

#ifndef SSC_INTRIN
#define SSC_INTRIN

#include "StgSharpC.h"
#include <immintrin.h>

typedef union column_2 {
        __m128 column[2];
        float m[4][2];
} __2_columnset;

typedef struct column_3 {
        __m128 column[3];
        float m[4][3];
} __3_columnset;

typedef union column_4 {
        __m128 column[4];
        float m[4][4];
} __4_columnset;

typedef void (*VECTORNORMALIZEPROC)(__m128 *source, __m128 *target);
typedef void (*MATRIXTRANSPOSEPROC)(void *source, void *target);
typedef void (*MATRIXDETPROC)(void *mat, void *transpose);

#pragma region matix function

INTERNAL void normalize(__m128 *source, __m128 *target);

INTERNAL void transpose23(__2_columnset *source, __3_columnset *target);
INTERNAL void transpose24(__2_columnset *source, __4_columnset *target);

INTERNAL void transpose32(__3_columnset *source, __2_columnset *target);
INTERNAL void transpose33(__3_columnset *source, __3_columnset *target);
INTERNAL void transpose34(__3_columnset *source, __4_columnset *target);
INTERNAL float det_mat3(__3_columnset *matrix, __3_columnset *tranpose);

INTERNAL void transpose42(__4_columnset *source, __2_columnset *target);
INTERNAL void transpose43(__4_columnset *source, __3_columnset *target);
INTERNAL void transpose44(__4_columnset *matrix, __4_columnset *target);
INTERNAL float det_mat4(__4_columnset *matrix, __4_columnset *transpose);

#pragma endregion

#pragma region quick_hash

INTERNAL int quick_string_hash(byte *str, int length);

#pragma endregion

typedef struct ssc_intrinsic {
        MATRIXTRANSPOSEPROC transpose_23;
        MATRIXTRANSPOSEPROC transpose_24;
        MATRIXTRANSPOSEPROC transpose_32;
        MATRIXTRANSPOSEPROC transpose_33;
        MATRIXTRANSPOSEPROC transpose_34;
        MATRIXTRANSPOSEPROC transpose_42;
        MATRIXTRANSPOSEPROC transpose_43;
        MATRIXTRANSPOSEPROC transpose_44;
        VECTORNORMALIZEPROC normalize_3;
        MATRIXDETPROC det_matrix33;
        MATRIXDETPROC det_matrix44;
        int (*string_quick_hash)(char *str, int length);
} ssc_intrinsic;

#endif // !SSC_INTRIN
