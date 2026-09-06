cmake_minimum_required(VERSION 3.16)
project(Nitload.Native LANGUAGES C)

include(${CMAKE_CURRENT_LIST_DIR}/ExternalLibs.cmake OPTIONAL)

set(NIF_TARGET_OS "auto" CACHE STRING "Target OS identifier: auto, windows, linux, macos, ios, android, unknown")
set_property(CACHE NIF_TARGET_OS PROPERTY STRINGS auto windows linux macos ios android unknown)

set(NIF_TARGET_ARCH "auto" CACHE STRING "Target ISA identifier: auto, x86_64, arm64, riscv64, ppc64le")
set_property(CACHE NIF_TARGET_ARCH PROPERTY STRINGS auto x86_64 arm64 riscv64 ppc64le)

nif_resolve_target_identifiers(
  "${NIF_TARGET_OS}"
  "${NIF_TARGET_ARCH}"
  NIF_RESOLVED_OS_NAME
  NIF_RESOLVED_OS_DEFINE
  NIF_RESOLVED_ARCH_NAME
  NIF_RESOLVED_ARCH_DEFINE)

set(NIF_NATIVE_SOURCES
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/dllmain.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/load_glfw.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/load_intrinsic.x86.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/m.bufop.x86.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/m.solve.x86.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/m.transpose.x86.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/mk.fma.pack.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/mk.fma.x86.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/mk.p_scan.x86.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/s.factorial.x86.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/s.hash.x86.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/simdid_compare.x86.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/simdid.x86.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/sk.fma.x86.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/thread.linux.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/thread.windows.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/vector.c
  ${CMAKE_CURRENT_LIST_DIR}/nif-src/vector_dot.c)

set(NIF_NATIVE_HEADERS
  ${CMAKE_CURRENT_LIST_DIR}/nif-head/glfw_function.h
  ${CMAKE_CURRENT_LIST_DIR}/nif-head/nif_internal.h
  ${CMAKE_CURRENT_LIST_DIR}/nif-head/nif_intrinsic.context.base.h
  ${CMAKE_CURRENT_LIST_DIR}/nif-head/nif_intrinsic.context.x86.h
  ${CMAKE_CURRENT_LIST_DIR}/nif-head/nif_intrinsic.h
  ${CMAKE_CURRENT_LIST_DIR}/nif-head/nif_intrinsic.matkernel.h
  ${CMAKE_CURRENT_LIST_DIR}/nif-head/nif_intrinsic.std.h
  ${CMAKE_CURRENT_LIST_DIR}/nif-head/nif_target.h
  ${CMAKE_CURRENT_LIST_DIR}/nif-head/NitloadFrameWork.h)

set(NIF_NATIVE_TEMPLATES
  ${CMAKE_CURRENT_LIST_DIR}/nif-template/nif_template.fma.x86.h)

# Nitload.Native now lives under <root>/src/Nitload.Native, so walk up two
# levels (project dir -> src -> solution root) to reach the solution root.
get_filename_component(NIF_NATIVE_SRC_ROOT ${CMAKE_CURRENT_LIST_DIR} DIRECTORY)
get_filename_component(NIF_SOLUTION_ROOT ${NIF_NATIVE_SRC_ROOT} DIRECTORY)

if(NIF_RESOLVED_OS_NAME STREQUAL "windows")
  set(NIF_OS_TAG win)
elseif(NIF_RESOLVED_OS_NAME STREQUAL "linux")
  set(NIF_OS_TAG linux)
else()
  set(NIF_OS_TAG unknown)
endif()

if(NIF_RESOLVED_ARCH_NAME STREQUAL "x86_64")
  set(NIF_ARCH_TAG x64)
else()
  set(NIF_ARCH_TAG ${NIF_RESOLVED_ARCH_NAME})
endif()

set(NIF_BUILD_OUTPUT_DIR ${NIF_SOLUTION_ROOT}/bin/${NIF_OS_TAG}-${NIF_ARCH_TAG})
# Compile-time intermediates (native PDB / .obj side files) live inside the CMake
# build tree (cmake_build/<preset>) so they get cleaned up with it.
set(NIF_OBJECT_ROOT_DIR ${CMAKE_CURRENT_BINARY_DIR}/obj/${PROJECT_NAME})
set(NIF_NATIVE_PROJECT_FILES
  ${NIF_NATIVE_SOURCES}
  ${NIF_NATIVE_HEADERS}
  ${NIF_NATIVE_TEMPLATES})

add_library(${PROJECT_NAME} SHARED ${NIF_NATIVE_PROJECT_FILES})

source_group(TREE ${CMAKE_CURRENT_LIST_DIR} FILES ${NIF_NATIVE_PROJECT_FILES})

set_target_properties(${PROJECT_NAME} PROPERTIES
  C_STANDARD 11
  C_STANDARD_REQUIRED ON
  RUNTIME_OUTPUT_DIRECTORY ${NIF_BUILD_OUTPUT_DIR}
  LIBRARY_OUTPUT_DIRECTORY ${NIF_BUILD_OUTPUT_DIR}
  ARCHIVE_OUTPUT_DIRECTORY ${NIF_BUILD_OUTPUT_DIR})

foreach(_config Debug Release RelWithDebInfo MinSizeRel)
  string(TOUPPER ${_config} _config_upper)
  set_target_properties(${PROJECT_NAME} PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY_${_config_upper} ${NIF_BUILD_OUTPUT_DIR}
    LIBRARY_OUTPUT_DIRECTORY_${_config_upper} ${NIF_BUILD_OUTPUT_DIR}
    ARCHIVE_OUTPUT_DIRECTORY_${_config_upper} ${NIF_BUILD_OUTPUT_DIR}
    PDB_OUTPUT_DIRECTORY_${_config_upper} ${NIF_BUILD_OUTPUT_DIR}
    COMPILE_PDB_OUTPUT_DIRECTORY_${_config_upper} ${NIF_OBJECT_ROOT_DIR}/${_config}/${NIF_ARCH_TAG})
endforeach()

set_target_properties(${PROJECT_NAME} PROPERTIES
  COMPILE_PDB_OUTPUT_DIRECTORY ${NIF_OBJECT_ROOT_DIR}/$<CONFIG>/${NIF_ARCH_TAG})

if(MSVC OR (CMAKE_C_COMPILER_ID STREQUAL "Clang" AND CMAKE_C_COMPILER_FRONTEND_VARIANT STREQUAL "MSVC"))
  target_compile_options(${PROJECT_NAME} PRIVATE
    /Fd${NIF_OBJECT_ROOT_DIR}/$<CONFIG>/${NIF_ARCH_TAG}/)
endif()

if(CMAKE_C_COMPILER_ID STREQUAL "Clang" AND NIF_RESOLVED_ARCH_NAME STREQUAL "x86_64")
  # Keep the intrinsic type system and builtin declarations visible to all TUs.
  # The runtime dispatch contract still follows T_arch/T_feature at the function level.
  if(CMAKE_C_COMPILER_FRONTEND_VARIANT STREQUAL "MSVC")
    target_compile_options(${PROJECT_NAME} PRIVATE
      /clang:-mavx
      /clang:-mavx2
      /clang:-mfma
      /clang:-mf16c
      /clang:-mavx512f
      /clang:-mavx512cd
      /clang:-mavx512dq
      /clang:-mavx512bw
      /clang:-mavx512vl)
  else()
    target_compile_options(${PROJECT_NAME} PRIVATE
      -mavx
      -mavx2
      -mfma
      -mf16c
      -mavx512f
      -mavx512cd
      -mavx512dq
      -mavx512bw
      -mavx512vl)
  endif()
endif()

target_include_directories(${PROJECT_NAME}
  PRIVATE
    ${CMAKE_CURRENT_LIST_DIR}/nif-head
    ${CMAKE_CURRENT_LIST_DIR}/nif-template
    ${CMAKE_CURRENT_LIST_DIR}/lib)

target_compile_definitions(${PROJECT_NAME}
  PRIVATE
    NIF_EXTERN_TARGET
    NIF_OS_TARGET=${NIF_RESOLVED_OS_DEFINE}
    NIF_ARCH_TARGET=${NIF_RESOLVED_ARCH_DEFINE})

set(NIF_NATIVE_EXTERNAL_TARGET ${PROJECT_NAME})
add_subdirectory(${CMAKE_CURRENT_LIST_DIR}/lib)

message(STATUS "Nitload.Native target OS: ${NIF_RESOLVED_OS_NAME} (${NIF_RESOLVED_OS_DEFINE})")
message(STATUS "Nitload.Native target ISA: ${NIF_RESOLVED_ARCH_NAME} (${NIF_RESOLVED_ARCH_DEFINE})")
message(STATUS "Nitload.Native output dir: ${NIF_BUILD_OUTPUT_DIR}")
