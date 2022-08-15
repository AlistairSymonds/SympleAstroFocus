#pragma once
class saf_core
{
private:
    /* data */
public:
    saf_core(/* args */){};
    virtual int connect() = 0;
    virtual ~saf_core(){};
};

class saf_core_factory
{
public:
    static std::unique_ptr<saf_core> create();
};