cmake_minimum_required(VERSION 3.16)

if(NOT DEFINED STGSHARP_NATIVE_EXTERNAL_TARGET)
  message(FATAL_ERROR "STGSHARP_NATIVE_EXTERNAL_TARGET must be set before adding lib/.")
endif()

include(${CMAKE_CURRENT_LIST_DIR}/../ExternalLibs.cmake OPTIONAL)

file(GLOB LIB_PACKAGE_DIRS RELATIVE ${CMAKE_CURRENT_LIST_DIR} ${CMAKE_CURRENT_LIST_DIR}/*)

foreach(_pkg ${LIB_PACKAGE_DIRS})
  set(_pkg_dir ${CMAKE_CURRENT_LIST_DIR}/${_pkg})
  if(NOT IS_DIRECTORY ${_pkg_dir})
    continue()
  endif()

  if(EXISTS ${_pkg_dir}/CMakeLists.txt)
    add_subdirectory(${_pkg_dir})
  else()
    sn_register_external_package(${STGSHARP_NATIVE_EXTERNAL_TARGET} ${_pkg} ${_pkg_dir})
  endif()
endforeach()