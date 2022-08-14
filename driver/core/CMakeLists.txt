
cmake_minimum_required(VERSION 3.22)
project(saf_core)
include(FetchContent)
include(GenerateExportHeader)

#getting HIDAPI deps
FetchContent_Declare(hidapi
    GIT_REPOSITORY https://github.com/libusb/hidapi.git
)

FetchContent_MakeAvailable(hidapi)
set(HIDAPI_WITH_HIDRAW TRUE)
set(HIDAPI_WITH_LIBUSB FALSE)

set(CMAKE_WINDOWS_EXPORT_ALL_SYMBOLS TRUE)

# SAF core stuff
add_library(${PROJECT_NAME} SHARED
    inc/saf_core.h
    src/saf_core.cpp
)

generate_export_header(saf_core)

target_link_libraries(saf_core PRIVATE hidapi::hidapi)

target_include_directories(${PROJECT_NAME} PUBLIC ${CMAKE_CURRENT_SOURCE_DIR}/inc ${CMAKE_BINARY_DIR}/exports)
#include_directories(${CMAKE_CURRENT_SOURCE_DIR}/inc ${CMAKE_BINARY_DIR}/exports)
