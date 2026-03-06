cmake_minimum_required(VERSION 3.16)
project(StgSharp.Native C)

# Include helper for external libraries if present
include(${CMAKE_CURRENT_LIST_DIR}/ExternalLibs.cmake OPTIONAL)

# Add main source directory
# Follow naming convention: prefer sn-src/sn-head (StgSharp native naming).
if(EXISTS ${CMAKE_CURRENT_LIST_DIR}/sn-src)
  add_subdirectory(sn-src)
elseif(EXISTS ${CMAKE_CURRENT_LIST_DIR}/src)
  add_subdirectory(src)
else()
  message(STATUS "No sn-src/ or src/ directory in StgSharp.Native; create one to add sources.")
endif()

# Register include directories according to naming convention: sn-head preferred
if(EXISTS ${CMAKE_CURRENT_LIST_DIR}/sn-head)
  include_directories(${CMAKE_CURRENT_LIST_DIR}/sn-head)
elseif(EXISTS ${CMAKE_CURRENT_LIST_DIR}/include)
  include_directories(${CMAKE_CURRENT_LIST_DIR}/include)
endif()

# Discover external libraries laid out under external/<libname> (each with head/ and lib/)
if(EXISTS ${CMAKE_CURRENT_LIST_DIR}/external)
  file(GLOB EXTERNAL_DIRS RELATIVE ${CMAKE_CURRENT_SOURCE_DIR} ${CMAKE_CURRENT_SOURCE_DIR}/external/*)
  foreach(_d ${EXTERNAL_DIRS})
    if(IS_DIRECTORY "${CMAKE_CURRENT_SOURCE_DIR}/external/${_d}")
      if(COMMAND use_external_lib)
        use_external_lib(${_d} "${CMAKE_CURRENT_SOURCE_DIR}/external/${_d}")
      else()
        message(STATUS "External lib '${_d}' found at external/${_d} (no use_external_lib defined).")
      endif()
    endif()
  endforeach()
endif()
