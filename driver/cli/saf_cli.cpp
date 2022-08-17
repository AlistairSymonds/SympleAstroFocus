#include <thread>
#include <iostream>
#include <chrono>
#include "saf_core.h"
int main(){
    std::cout << "hellooooo" << std::endl;
    auto saf = saf_core_factory::create();
    
    std::cout << "created" << std::endl;
    saf->Connect();
    std::this_thread::sleep_for(std::chrono::seconds(1));
    std::cout << "goodbyyyye" << std::endl;
}