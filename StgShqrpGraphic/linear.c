#pragma once

#include "GLenv/include/glad/glad.h"
#include "StgSharpGraphic.h"

#include <stdio.h>
#include <stdalign.h>

#include <xmmintrin.h>
#include <pmmintrin.h>
#include <immintrin.h>



/*

Basic functions as a copy of SIMD intrisic function

*/


SSGC_API __m128* __cdecl set_m128ptr_default()
{
    return defaultVecPtr;
}









