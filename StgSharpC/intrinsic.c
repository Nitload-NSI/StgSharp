#include "StgSharpC.h"
#include "ssc_intrinsic.h"
#include <stdio.h>

SSCAPI void load_intrinsic_function(void *intrinsic_context)
{
        ssc_null_assert(intrinsic_context);

        ssc_intrinsic *context = (ssc_intrinsic *)intrinsic_context;

        context->transpose_23 = transpose23;
        context->transpose_24 = transpose24;
        context->transpose_32 = transpose32;
        context->transpose_33 = transpose33;
        context->transpose_34 = transpose34;
        context->transpose_42 = transpose42;
        context->transpose_43 = transpose43;
        context->transpose_44 = transpose44;
        context->normalize_3 = normalize;
        context->det_matrix33 = det_mat3;
        context->det_matrix44 = det_mat4;
        context->string_quick_hash = quick_string_hash;

}
