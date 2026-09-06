cmake_minimum_required(VERSION 3.16)

nif_register_external_package(${NIF_NATIVE_EXTERNAL_TARGET} glfw ${CMAKE_CURRENT_LIST_DIR})

if(WIN32)
  if(NOT EXISTS ${CMAKE_CURRENT_LIST_DIR}/glfw3_mt.lib)
    message(FATAL_ERROR "Required GLFW static library not found: ${CMAKE_CURRENT_LIST_DIR}/glfw3_mt.lib")
  endif()

  nif_link_external_binary_if_exists(${NIF_NATIVE_EXTERNAL_TARGET} ${CMAKE_CURRENT_LIST_DIR}/glfw3_mt.lib)
endif()