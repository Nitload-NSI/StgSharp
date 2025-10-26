# Helper for external libs layout: external/<libname>/{head,lib}
# Provides function `use_external_lib(<name> <path>)` that sets include directories
# and attempts to link static/shared libs found under lib/.

function(use_external_lib NAME PATH)
    # Expect headers in ${PATH}/head and libs in ${PATH}/lib
    set(HEAD_DIR "${PATH}/head")
    set(LIB_DIR "${PATH}/lib")

    if(EXISTS "${HEAD_DIR}")
        message(STATUS "Registering external include: ${HEAD_DIR}")
        include_directories(${HEAD_DIR})
    else()
        message(WARNING "External lib ${NAME} missing head/ directory at ${HEAD_DIR}")
    endif()

    if(EXISTS "${LIB_DIR}")
        # try to find static/shared libs inside LIB_DIR
        file(GLOB LIB_FILES RELATIVE ${LIB_DIR} ${LIB_DIR}/*)
        foreach(_lib ${LIB_FILES})
            if(_lib MATCHES ".*\\.(a|lib|so|dylib)$")
                message(STATUS "Found library for ${NAME}: ${LIB_DIR}/${_lib}")
                # Consumer targets should link explicitly; provide variable for convenience
                set(EXTERNAL_LIB_${NAME} "${LIB_DIR}/${_lib}" PARENT_SCOPE)
            endif()
        endforeach()
    else()
        message(STATUS "No lib/ directory for external lib ${NAME} at ${LIB_DIR}")
    endif()
endfunction()
