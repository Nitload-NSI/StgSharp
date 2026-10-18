cmake_minimum_required(VERSION 3.16)
project(Nitload.Native LANGUAGES C)

include(${CMAKE_CURRENT_LIST_DIR}/ExternalLibs.cmake OPTIONAL)

set(NFN_TARGET_OS "auto" CACHE STRING "Target OS identifier: auto, windows, linux, macos, ios, android, unknown")
set_property(CACHE NFN_TARGET_OS PROPERTY STRINGS auto windows linux macos ios android unknown)

set(NFN_TARGET_ARCH "auto" CACHE STRING "Target ISA identifier: auto, x86_64, arm64, riscv64, ppc64le")
set_property(CACHE NFN_TARGET_ARCH PROPERTY STRINGS auto x86_64 arm64 riscv64 ppc64le)

nfn_resolve_target_identifiers(
  "${NFN_TARGET_OS}"
  "${NFN_TARGET_ARCH}"
  NFN_RESOLVED_OS_NAME
  NFN_RESOLVED_OS_DEFINE
  NFN_RESOLVED_ARCH_NAME
  NFN_RESOLVED_ARCH_DEFINE)

# CONFIGURE_DEPENDS makes CMake reconfigure when files are added or removed,
# so native sources, headers, and templates need no per-file CMake edits.
file(GLOB_RECURSE NFN_NATIVE_SOURCES CONFIGURE_DEPENDS
  ${CMAKE_CURRENT_LIST_DIR}/nfn-src/*.c)

file(GLOB_RECURSE NFN_NATIVE_HEADERS CONFIGURE_DEPENDS
  ${CMAKE_CURRENT_LIST_DIR}/nfn-head/*.h)

file(GLOB_RECURSE NFN_NATIVE_TEMPLATES CONFIGURE_DEPENDS
  ${CMAKE_CURRENT_LIST_DIR}/nfn-template/*.h)

# Nitload.Native now lives under <root>/src/Nitload.Native, so walk up two
# levels (project dir -> src -> solution root) to reach the solution root.
get_filename_component(NFN_NATIVE_SRC_ROOT ${CMAKE_CURRENT_LIST_DIR} DIRECTORY)
get_filename_component(NFN_SOLUTION_ROOT ${NFN_NATIVE_SRC_ROOT} DIRECTORY)

if(NFN_RESOLVED_OS_NAME STREQUAL "windows")
  set(NFN_OS_TAG win)
elseif(NFN_RESOLVED_OS_NAME STREQUAL "linux")
  set(NFN_OS_TAG linux)
else()
  set(NFN_OS_TAG unknown)
endif()

if(NFN_RESOLVED_ARCH_NAME STREQUAL "x86_64")
  set(NFN_ARCH_TAG x64)
else()
  set(NFN_ARCH_TAG ${NFN_RESOLVED_ARCH_NAME})
endif()

set(NFN_EXPECTED_RID "" CACHE STRING "Optional managed target RID to validate before deployment")
if(NFN_EXPECTED_RID AND NOT NFN_EXPECTED_RID STREQUAL "${NFN_OS_TAG}-${NFN_ARCH_TAG}")
  message(FATAL_ERROR "Native target ${NFN_OS_TAG}-${NFN_ARCH_TAG} does not match managed target ${NFN_EXPECTED_RID}. Use a matching compiler/toolchain and a separate build directory.")
endif()

set(NFN_BUILD_OUTPUT_DIR "${NFN_SOLUTION_ROOT}/bin/${NFN_OS_TAG}-${NFN_ARCH_TAG}"
    CACHE PATH "Native artifact directory shared with managed build deployment")
# Compile-time intermediates (native PDB / .obj side files) live inside the CMake
# build tree (cmake_build/<preset>) so they get cleaned up with it.
set(NFN_OBJECT_ROOT_DIR ${CMAKE_CURRENT_BINARY_DIR}/obj/${PROJECT_NAME})
set(NFN_NATIVE_PROJECT_FILES
  ${NFN_NATIVE_SOURCES}
  ${NFN_NATIVE_HEADERS}
  ${NFN_NATIVE_TEMPLATES})

add_library(${PROJECT_NAME} SHARED ${NFN_NATIVE_PROJECT_FILES})

source_group(TREE ${CMAKE_CURRENT_LIST_DIR} FILES ${NFN_NATIVE_PROJECT_FILES})

set_target_properties(${PROJECT_NAME} PROPERTIES
  C_STANDARD 11
  C_STANDARD_REQUIRED ON
  OUTPUT_NAME "Nitload.Native"
  PDB_NAME "Nitload.Native"
  DEBUG_POSTFIX ""
  RELEASE_POSTFIX ""
  RELWITHDEBINFO_POSTFIX ""
  MINSIZEREL_POSTFIX ""
  RUNTIME_OUTPUT_DIRECTORY "${NFN_BUILD_OUTPUT_DIR}"
  LIBRARY_OUTPUT_DIRECTORY "${NFN_BUILD_OUTPUT_DIR}"
  ARCHIVE_OUTPUT_DIRECTORY "${NFN_BUILD_OUTPUT_DIR}"
  PDB_OUTPUT_DIRECTORY "${NFN_BUILD_OUTPUT_DIR}")

# P/Invoke loads Nitload.Native.dll on Windows, including GNU-style Clang toolchains.
if(WIN32)
  set_target_properties(${PROJECT_NAME} PROPERTIES PREFIX "")
endif()

foreach(_config Debug Release RelWithDebInfo MinSizeRel)
  string(TOUPPER ${_config} _config_upper)
  set_target_properties(${PROJECT_NAME} PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY_${_config_upper} ${NFN_BUILD_OUTPUT_DIR}
    LIBRARY_OUTPUT_DIRECTORY_${_config_upper} ${NFN_BUILD_OUTPUT_DIR}
    ARCHIVE_OUTPUT_DIRECTORY_${_config_upper} ${NFN_BUILD_OUTPUT_DIR}
    PDB_OUTPUT_DIRECTORY_${_config_upper} ${NFN_BUILD_OUTPUT_DIR}
    COMPILE_PDB_OUTPUT_DIRECTORY_${_config_upper} ${NFN_OBJECT_ROOT_DIR}/${_config}/${NFN_ARCH_TAG})
endforeach()

set_target_properties(${PROJECT_NAME} PROPERTIES
  COMPILE_PDB_OUTPUT_DIRECTORY ${NFN_OBJECT_ROOT_DIR}/$<CONFIG>/${NFN_ARCH_TAG})

if(MSVC OR (CMAKE_C_COMPILER_ID STREQUAL "Clang" AND CMAKE_C_COMPILER_FRONTEND_VARIANT STREQUAL "MSVC"))
  target_compile_options(${PROJECT_NAME} PRIVATE
    /Fd${NFN_OBJECT_ROOT_DIR}/$<CONFIG>/${NFN_ARCH_TAG}/)
endif()

if(CMAKE_C_COMPILER_ID STREQUAL "Clang" AND NFN_RESOLVED_ARCH_NAME STREQUAL "x86_64")
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
    ${CMAKE_CURRENT_LIST_DIR}/nfn-head
    ${CMAKE_CURRENT_LIST_DIR}/nfn-template
    ${CMAKE_CURRENT_LIST_DIR}/lib)

target_compile_definitions(${PROJECT_NAME}
  PRIVATE
    NFN_EXTERN_TARGET
    NFN_OS_TARGET=${NFN_RESOLVED_OS_DEFINE}
    NFN_ARCH_TARGET=${NFN_RESOLVED_ARCH_DEFINE})

set(NFN_NATIVE_EXTERNAL_TARGET ${PROJECT_NAME})
add_subdirectory(${CMAKE_CURRENT_LIST_DIR}/lib)

message(STATUS "Nitload.Native target OS: ${NFN_RESOLVED_OS_NAME} (${NFN_RESOLVED_OS_DEFINE})")
message(STATUS "Nitload.Native target ISA: ${NFN_RESOLVED_ARCH_NAME} (${NFN_RESOLVED_ARCH_DEFINE})")
message(STATUS "Nitload.Native output dir: ${NFN_BUILD_OUTPUT_DIR}")
