#pragma once

#include "./include/StgSharpC.h"

#include <stdio.h>
#include <stdalign.h>

#include <xmmintrin.h>
#include <pmmintrin.h>
#include <immintrin.h>



/*

Basic functions as a copy of SIMD intrisic function

*/


SSCAPI __m128* __cdecl set_m128ptr_default()
{
    return defaultVecPtr;
}









