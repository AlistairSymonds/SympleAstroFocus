cmake_minimum_required(VERSION 3.22)
project(saf_driver)



set (CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)
set (CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)
set (CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)

add_subdirectory(core)

add_executable(saf_cli cli/saf_cli.cpp)

target_link_libraries(saf_cli saf_core)