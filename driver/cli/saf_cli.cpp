#include <thread>
#include <iostream>
#include <chrono>
#include "saf_core.h"
int main(){
    std::cout << "hellooooo" << std::endl;
    auto saf = saf_core_factory::create();
    uint32_t old_max_pos;
    std::cout << "created" << std::endl;

    saf->Connect();
    std::this_thread::sleep_for(std::chrono::seconds(1));
    std::cout << saf->toString() << std::endl;

    old_max_pos = saf->getMaxPosition();
    saf->setMaxPosition(123456);
    std::this_thread::sleep_for(std::chrono::seconds(1));
    std::cout << saf->toString() << std::endl;

    saf->setMaxPosition(old_max_pos);
    std::this_thread::sleep_for(std::chrono::seconds(1));
    std::cout << saf->toString() << std::endl;

    std::this_thread::sleep_for(std::chrono::seconds(1));
    std::cout << saf->toString() << std::endl;
    std::cout << "goodbyyyye" << std::endl;
}