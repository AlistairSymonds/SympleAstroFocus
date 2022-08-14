#include <iostream>
#include "saf_core.h"
int main(){
    std::cout << "hellooooo" << std::endl;
    saf_core saf;
    saf.connect();
    std::cout << "goodbyyyye" << std::endl;
}