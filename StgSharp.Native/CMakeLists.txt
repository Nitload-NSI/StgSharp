cmake_minimum_required(VERSION 3.16)
project(StgSharp.Native LANGUAGES C)

include(${CMAKE_CURRENT_LIST_DIR}/ExternalLibs.cmake OPTIONAL)

set(SN_TARGET_OS "auto" CACHE STRING "Target OS identifier: auto, windows, linux, macos, ios, android, unknown")
set_property(CACHE SN_TARGET_OS PROPERTY STRINGS auto windows linux macos ios android unknown)

set(SN_TARGET_ARCH "auto" CACHE STRING "Target ISA identifier: auto, x86_64, arm64, riscv64, ppc64le")
set_property(CACHE SN_TARGET_ARCH PROPERTY STRINGS auto x86_64 arm64 riscv64 ppc64le)

sn_resolve_target_identifiers(
  "${SN_TARGET_OS}"
  "${SN_TARGET_ARCH}"
  SN_RESOLVED_OS_NAME
  SN_RESOLVED_OS_DEFINE
  SN_RESOLVED_ARCH_NAME
  SN_RESOLVED_ARCH_DEFINE)

file(GLOB STGSHARP_NATIVE_SOURCES CONFIGURE_DEPENDS
  ${CMAKE_CURRENT_LIST_DIR}/sn-src/*.c)

list(REMOVE_ITEM STGSHARP_NATIVE_SOURCES
  ${CMAKE_CURRENT_LIST_DIR}/sn-src/formatted.c
  ${CMAKE_CURRENT_LIST_DIR}/sn-src/StgSharpC.c)

if(NOT STGSHARP_NATIVE_SOURCES)
  message(FATAL_ERROR "No source files found under sn-src/.")
endif()

get_filename_component(STGSHARP_SOURCE_ROOT ${CMAKE_CURRENT_LIST_DIR} DIRECTORY)
get_filename_component(STGSHARP_SOURCE_ROOT_NAME ${STGSHARP_SOURCE_ROOT} NAME)

if(STGSHARP_SOURCE_ROOT_NAME STREQUAL "src")
  get_filename_component(STGSHARP_SOLUTION_ROOT ${STGSHARP_SOURCE_ROOT} DIRECTORY)
else()
  set(STGSHARP_SOLUTION_ROOT ${STGSHARP_SOURCE_ROOT})
endif()

if(SN_RESOLVED_OS_NAME STREQUAL "windows")
  set(STGSHARP_OS_TAG win)
elseif(SN_RESOLVED_OS_NAME STREQUAL "linux")
  set(STGSHARP_OS_TAG linux)
elseif(SN_RESOLVED_OS_NAME STREQUAL "macos")
  set(STGSHARP_OS_TAG osx)
elseif(SN_RESOLVED_OS_NAME STREQUAL "ios")
  set(STGSHARP_OS_TAG ios)
elseif(SN_RESOLVED_OS_NAME STREQUAL "android")
  set(STGSHARP_OS_TAG android)
else()
  set(STGSHARP_OS_TAG unknown)
endif()

if(SN_RESOLVED_ARCH_NAME STREQUAL "x86_64")
  set(STGSHARP_ARCH_TAG x64)
else()
  set(STGSHARP_ARCH_TAG ${SN_RESOLVED_ARCH_NAME})
endif()

set(STGSHARP_BUILD_OUTPUT_DIR ${STGSHARP_SOLUTION_ROOT}/cmake_build/${STGSHARP_OS_TAG}_${STGSHARP_ARCH_TAG})
set(STGSHARP_OBJECT_ROOT_DIR ${STGSHARP_SOLUTION_ROOT}/obj/${PROJECT_NAME})

set(STGSHARP_C_FRONTEND_VARIANT "${CMAKE_C_COMPILER_FRONTEND_VARIANT}")

add_library(${PROJECT_NAME} SHARED ${STGSHARP_NATIVE_SOURCES})

set_target_properties(${PROJECT_NAME} PROPERTIES
  C_STANDARD 11
  C_STANDARD_REQUIRED ON
  RUNTIME_OUTPUT_DIRECTORY ${STGSHARP_BUILD_OUTPUT_DIR}
  LIBRARY_OUTPUT_DIRECTORY ${STGSHARP_BUILD_OUTPUT_DIR}
  ARCHIVE_OUTPUT_DIRECTORY ${STGSHARP_BUILD_OUTPUT_DIR})

foreach(_config Debug Release RelWithDebInfo MinSizeRel)
  string(TOUPPER ${_config} _config_upper)
  set_target_properties(${PROJECT_NAME} PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY_${_config_upper} ${STGSHARP_BUILD_OUTPUT_DIR}
    LIBRARY_OUTPUT_DIRECTORY_${_config_upper} ${STGSHARP_BUILD_OUTPUT_DIR}
    ARCHIVE_OUTPUT_DIRECTORY_${_config_upper} ${STGSHARP_BUILD_OUTPUT_DIR}
    PDB_OUTPUT_DIRECTORY_${_config_upper} ${STGSHARP_BUILD_OUTPUT_DIR}
    COMPILE_PDB_OUTPUT_DIRECTORY_${_config_upper} ${STGSHARP_OBJECT_ROOT_DIR}/${_config}/${STGSHARP_ARCH_TAG})
endforeach()

set_target_properties(${PROJECT_NAME} PROPERTIES
  COMPILE_PDB_OUTPUT_DIRECTORY ${STGSHARP_OBJECT_ROOT_DIR}/$<CONFIG>/${STGSHARP_ARCH_TAG})

if(MSVC OR STGSHARP_C_FRONTEND_VARIANT STREQUAL "MSVC")
  target_compile_options(${PROJECT_NAME} PRIVATE
    /Fd${STGSHARP_OBJECT_ROOT_DIR}/$<CONFIG>/${STGSHARP_ARCH_TAG}/)
endif()

if(CMAKE_C_COMPILER_ID STREQUAL "Clang" AND SN_RESOLVED_ARCH_NAME STREQUAL "x86_64")
  if(STGSHARP_C_FRONTEND_VARIANT STREQUAL "MSVC")
    target_compile_options(${PROJECT_NAME} PRIVATE
      /clang:-mavx
      /clang:-mavx2
      /clang:-mfma
      /clang:-mavx512f
      /clang:-mavx512vl)
  else()
    target_compile_options(${PROJECT_NAME} PRIVATE
      -mavx
      -mavx2
      -mfma
      -mavx512f
      -mavx512vl)
  endif()
endif()

target_include_directories(${PROJECT_NAME}
  PRIVATE
    ${CMAKE_CURRENT_LIST_DIR}/sn-head
    ${CMAKE_CURRENT_LIST_DIR}/sn-template
    ${CMAKE_CURRENT_LIST_DIR}/lib)

target_compile_definitions(${PROJECT_NAME}
  PRIVATE
    SN_EXTERN_TARGET
    SN_OS_TARGET=${SN_RESOLVED_OS_DEFINE}
    SN_ARCH_TARGET=${SN_RESOLVED_ARCH_DEFINE})

set(STGSHARP_NATIVE_EXTERNAL_TARGET ${PROJECT_NAME})
add_subdirectory(${CMAKE_CURRENT_LIST_DIR}/lib)

if(WIN32)
  target_link_libraries(${PROJECT_NAME} PRIVATE opengl32)
endif()

message(STATUS "StgSharp.Native target OS: ${SN_RESOLVED_OS_NAME} (${SN_RESOLVED_OS_DEFINE})")
message(STATUS "StgSharp.Native target ISA: ${SN_RESOLVED_ARCH_NAME} (${SN_RESOLVED_ARCH_DEFINE})")
message(STATUS "StgSharp.Native compiler front-end: ${STGSHARP_C_FRONTEND_VARIANT}")
message(STATUS "StgSharp.Native output dir: ${STGSHARP_BUILD_OUTPUT_DIR}")
