#pragma once
class SympleAFCore
{
private:
    /* data */
public:
    SympleAFCore(/* args */){};
    virtual int Connect() = 0;
    virtual int Disconnect() = 0;
    virtual std::string toString() = 0;

    
    virtual uint32_t getCurrentPosition() = 0;
    virtual uint32_t getSetPosition() = 0;
    //virtual uint32_t setSetPosition() = 0;
    virtual uint32_t getMaxPosition() = 0;
    virtual void setMaxPosition(uint32_t pos) = 0;
    virtual bool     getReversed() = 0;
    //virtual uint32_t setZero() = 0;
    virtual uint32_t getStepPeriodUs() = 0;
    //virtual uint32_t setStepPeriodUs() = 0;
    virtual bool     getMoving() = 0;

    
    /*
    //TMC specialised methods

    virtual bool     getStepperEnabled() = 0;
    virtual bool     getStepperCommsError() = 0;
    virtual bool     getStepperError() = 0;

    virtual uint32_t getSgResult() = 0;
    virtual uint32_t getCsActual() = 0;

    virtual uint32_t getIRun() = 0;
    virtual uint32_t setIRun(uint8_t irun) = 0;

    virtual uint32_t getIHold() = 0;
    virtual uint32_t setIHold(uint8_t irun) = 0;


    virtual uint32_t getStallThresh() = 0;
    virtual uint32_t setStallThresh(uint32_t thresh) = 0;

    virtual bool     getHomingTowardsZero() = 0;
    virtual uint32_t setHomingTowardsZero(bool val) = 0;

    virtual bool     getHomingTowardsMax() = 0;
    virtual uint32_t setHomingTowardsMax(bool val) = 0;

    virtual bool     getHoming() = 0;
    virtual uint32_t triggerHoming() = 0;
    */
    virtual ~SympleAFCore(){};
};

class saf_core_factory
{
public:
    static std::unique_ptr<SympleAFCore> create();
};
