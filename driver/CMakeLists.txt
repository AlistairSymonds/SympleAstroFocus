cmake_minimum_required(VERSION 3.22)
project(saf_driver)


add_subdirectory(core)

add_executable(saf_cli cli/saf_cli.cpp)

target_link_libraries(saf_cli saf_core)