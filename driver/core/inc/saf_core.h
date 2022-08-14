#include <hidapi/hidapi.h>

class saf_core
{
private:
    /* data */
public:
    saf_core(/* args */);
    int connect();
    ~saf_core();
};
