#include <iostream>
#include <thread>
#include <mutex>
#include <chrono>
#include <format>
#include <queue>
#include "hidapi.h"
#include "saf_core.h"
#include "sym_defs.h"




class saf_core_impl : public SympleAFCore
{
private:
	std::atomic<bool> stopUsbThread;
	std::mutex commandQueueMutex;
	std::thread usbThread;
	void runUsbThread();
	void writeDataPendingDataToDevice();
	void readDataFromDevice();
	void processDataFromDevice(uint32_t data[16]);
	hid_device *handle;
	hid_device_info usb_info;
	typedef struct
	{
		bool isWrite;
		uint32_t stateDwordId;
		uint32_t requestedValue;
	} symple_state_request_t;


	std::queue < symple_state_request_t> commandQueue;
	uint32_t pending_commands;

	uint32_t dev_status;
	uint32_t dev_cur_pos;
	uint32_t dev_set_pos;
	uint32_t dev_max_pos;
	uint32_t dev_step_time_us;
	uint32_t dev_driver_conf;
	uint32_t dev_driver_status;

	uint32_t dev_guid[3];
	uint32_t dev_mcu_type;
	uint32_t dev_stepper_driver_type;
	uint32_t dev_fw_state;
	uint32_t dev_fw_commit;

public:
    saf_core_impl();
	saf_core_impl(const saf_core_impl&);
    int Connect();
	int Disconnect();
	std::string toString();
    ~saf_core_impl();


	virtual uint32_t getCurrentPosition();
	virtual uint32_t getSetPosition();
	virtual void     setSetPosition(uint32_t pos);
	virtual uint32_t getMaxPosition();
	virtual void     setMaxPosition(uint32_t pos);
	virtual void     setZero();
	virtual uint32_t getStepPeriodUs();
	virtual void     setStepPeriodUs(uint32_t us);
	virtual bool     getMoving();
	
	virtual bool     getReversed();
	virtual void     toggleReversed();

};

std::unique_ptr<SympleAFCore> saf_core_factory::create(){
	
	std::cout << "bout to ccreate" << std::endl;
	return std::make_unique<saf_core_impl>(saf_core_impl());
}


void saf_core_impl::runUsbThread()
{
	while (!stopUsbThread)
	{
		writeDataPendingDataToDevice();
		readDataFromDevice();
		std::this_thread::sleep_for(std::chrono::milliseconds(150));
	}
}

void saf_core_impl::writeDataPendingDataToDevice()
{
	commandQueueMutex.lock();
	while (!commandQueue.empty())
	{
		uint32_t dwords_to_device[16];
		for (int i = 0; i < sizeof(dwords_to_device)/sizeof(uint32_t); i++)
		{
			dwords_to_device[i] = 0xFFFFFFFF;
		}
		for (size_t i = 0; i < sizeof(dwords_to_device) / sizeof(uint32_t)/2; i++)
		{
			if (!commandQueue.empty())
			{
				symple_state_request_t s;
				s = commandQueue.front();
				commandQueue.pop();
				dwords_to_device[(i * 2)] = ((((uint32_t) s.isWrite )<< 31) | s.stateDwordId);
				dwords_to_device[(i * 2)+1] = s.requestedValue;
			}

		}
		uint8_t byte_to_dev[65];
		byte_to_dev[0] = 1;
		for (size_t i = 0; i < sizeof(dwords_to_device) / sizeof(uint32_t); i++)
		{
			byte_to_dev[(i*4) + 1] = dwords_to_device[i];
			byte_to_dev[(i*4) + 2] = dwords_to_device[i] >> 8;
			byte_to_dev[(i*4) + 3] = dwords_to_device[i] >> 16;
			byte_to_dev[(i*4) + 4] = dwords_to_device[i] >> 24;
		}
		hid_write(handle, byte_to_dev, 65);
	}
	pending_commands = 0;
	commandQueueMutex.unlock();
}

void saf_core_impl::readDataFromDevice()
{
	std::cout << "usb thread still vibing" << std::endl;
	int bytesRead;
	uint8_t readData[64];
	uint32_t readDataDwords[16];
	memset(readDataDwords, 0xFFFFFFFF, 16);
	bytesRead = hid_read(handle, readData, 64);
	for (size_t i = 0; i < (bytesRead / 4); i++)
	{
		int b = i + 1;
		readDataDwords[i] = 0;
		readDataDwords[i] |= readData[(i * 4) + 3] << 24;
		readDataDwords[i] |= readData[(i * 4) + 2] << 16;
		readDataDwords[i] |= readData[(i * 4) + 1] << 8;
		readDataDwords[i] |= readData[(i * 4) + 0];
	}

	for (size_t i = 0; i < bytesRead / 4; i++)
	{
		//printf("%08x ", readDataDwords[i]);
	}
	processDataFromDevice(readDataDwords);
}

void saf_core_impl::processDataFromDevice(uint32_t data[16]) {
	for (size_t i = 0; i < 8; i++)
	{
		symple_state_request_t s;
		s.isWrite = data[i*2] & 0x8000000;
		s.stateDwordId = data[i * 2] & (~0x8000000);
		s.requestedValue = data[(i * 2) + 1];

		switch (s.stateDwordId)
		{
			case STATUS_DWORD:
				dev_status = s.requestedValue;
				break;

			case CURRENT_POSITION_DWORD:
				dev_cur_pos = s.requestedValue;
				break;

			case SET_POSITION_DWORD:
				dev_set_pos = s.requestedValue;
				break;

			case MAX_POSITION_DWORD:
				dev_max_pos = s.requestedValue;
				break;

			case STEP_TIME_MICROSEC:
				dev_step_time_us = s.requestedValue;
				break;

			case STEPPER_DRIVER_CONF:
				dev_driver_conf = s.requestedValue;
				break;		
			
			case STEPPER_DRIVER_STATUS:
				dev_driver_status = s.requestedValue;
				break;
		}
	}
}


saf_core_impl::saf_core_impl(/* args */)
{	
	pending_commands = 0;
	handle = NULL;
	stopUsbThread = false;
	hid_init();
	std::cout << "Constructing" << std::endl;
}

saf_core_impl::saf_core_impl(const saf_core_impl &obj)
{
	handle = obj.handle;//is this even legal?
	stopUsbThread = obj.stopUsbThread.load();
	hid_init();
	std::cout << "Copy constructing" << std::endl;
}

std::string saf_core_impl::toString() {
	return std::format(
		"USB Handle ptr: {}\n"
		"Current Position: {}\n"
		"Set Position: {}\n"
		"Max Position: {}\n"
		"is Reversed: {}\n"
		"Step Period us: {}\n"
		"is Moving: {}\n",
		(uint64_t) handle, getCurrentPosition(), getSetPosition(), getMaxPosition(), getReversed(), getStepPeriodUs(), getMoving());
}

int saf_core_impl::Connect()
{
	handle = hid_open(56, 78, NULL);
	if (handle == NULL)
	{
		std::cout << "Couldn't open device" << std::endl;

		return 1;
	} else {
		std::cout << "Connected" << std::endl;
		
		auto usb_info_pr = hid_get_device_info(handle);
		usbThread = std::thread(&saf_core_impl::runUsbThread, this);

		return 0;
	}
}

int saf_core_impl::Disconnect()
{
	stopUsbThread = true;
	if (handle != NULL)
	{
		usbThread.join();
		hid_close(handle);
	}
	return 0;
}

saf_core_impl::~saf_core_impl()
{
	Disconnect();
	hid_exit();
	
}

uint32_t saf_core_impl::getCurrentPosition()
{
	return dev_cur_pos;
}

uint32_t saf_core_impl::getSetPosition()
{
	return dev_set_pos;
}

void saf_core_impl::setSetPosition(uint32_t pos)
{
	symple_state_request_t s;
	s.isWrite = true;
	s.requestedValue = pos;
	s.stateDwordId = SET_POSITION_DWORD;
	commandQueueMutex.lock();
	commandQueue.push(s);
	commandQueueMutex.unlock();
}

uint32_t saf_core_impl::getMaxPosition()
{
	return dev_max_pos;
}

void saf_core_impl::setMaxPosition(uint32_t pos)
{	
	symple_state_request_t s;
	s.isWrite = true;
	s.requestedValue = pos;
	s.stateDwordId = MAX_POSITION_DWORD;
	commandQueueMutex.lock();
	commandQueue.push(s);
	commandQueueMutex.unlock();
}

bool saf_core_impl::getReversed()
{
	return dev_status & STATUS_REVERSE_BIT;
}

void saf_core_impl::toggleReversed()
{
	commandQueueMutex.lock();
	pending_commands |= COMMAND_TOGGLE_REVERSE_BIT;
	symple_state_request_t s;
	s.isWrite = true;
	s.requestedValue = pending_commands;
	s.stateDwordId = COMMAND_DWORD;
	commandQueue.push(s);
	commandQueueMutex.unlock();
}

void saf_core_impl::setZero()
{

	commandQueueMutex.lock();
	pending_commands |= COMMAND_SET_ZERO_BIT;
	symple_state_request_t s;
	s.isWrite = true;
	s.requestedValue = pending_commands;
	s.stateDwordId = COMMAND_DWORD;
	commandQueue.push(s);
	commandQueueMutex.unlock();
}

uint32_t saf_core_impl::getStepPeriodUs()
{
	return dev_step_time_us;
}

void saf_core_impl::setStepPeriodUs(uint32_t us)
{
	symple_state_request_t s;
	s.isWrite = true;
	s.requestedValue = us;
	s.stateDwordId = STEP_TIME_MICROSEC;
	commandQueueMutex.lock();
	commandQueue.push(s);
	commandQueueMutex.unlock();
}

bool saf_core_impl::getMoving()
{
	return dev_status & STATUS_IS_MOVING_BIT;
}

//Standard boring getter and setter methods
