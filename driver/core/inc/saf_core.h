#pragma once
class SympleAFCore
{
private:
    /* data */
public:
    SympleAFCore(/* args */){};
    virtual int Connect() = 0;
    virtual int DumpUsb() = 0;
    virtual ~SympleAFCore(){};
};

class saf_core_factory
{
public:
    static std::unique_ptr<SympleAFCore> create();
};
