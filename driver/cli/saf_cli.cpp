#include <iostream>
#include "saf_core.h"
int main(){
    std::cout << "hellooooo" << std::endl;
    auto saf = saf_core_factory::create();
    
    std::cout << "created" << std::endl;
    saf->Connect();
    std::cout << "goodbyyyye" << std::endl;
}