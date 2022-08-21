#include <iostream>
#include <thread>
#include <mutex>
#include <chrono>
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
	void processDataFromDevice(uint32_t data[16]);
	hid_device *handle;
	typedef struct
	{
		bool isWrite;
		uint32_t stateDwordId;
		uint32_t requestedValue;
	} symple_state_request_t;

public:
    saf_core_impl();
	saf_core_impl(const saf_core_impl&);
    int Connect();
	int Disconnect();
    ~saf_core_impl();
};

std::unique_ptr<SympleAFCore> saf_core_factory::create(){
	
	std::cout << "bout to ccreate" << std::endl;
	return std::make_unique<saf_core_impl>(saf_core_impl());
}


void saf_core_impl::runUsbThread()
{
	while (!stopUsbThread)
	{
		std::cout << "usb thread still vibing" << std::endl;
		int bytesRead;
		uint8_t readData[64];
		uint32_t readDataDwords[16];
		bytesRead = hid_read(handle, readData, 64);
		for (size_t i = 0; i < (64/4); i++)
		{
			int b = i + 1;
			readDataDwords[i] = 0;
			readDataDwords[i] |= readData[(i*4)+3] << 24;
			readDataDwords[i] |= readData[(i * 4) +2] << 16;
			readDataDwords[i] |= readData[(i * 4) +1] << 8;
			readDataDwords[i] |= readData[(i * 4) +0] ;
		}

		for (size_t i = 0; i < bytesRead/4; i++)
		{
			printf("%08x ", readDataDwords[i]);
		}
		processDataFromDevice(readDataDwords);
		std::cout << std::endl;
		std::this_thread::sleep_for(std::chrono::milliseconds(50));
	}
}

void saf_core_impl::processDataFromDevice(uint32_t data[16]) {
	for (size_t i = 0; i < 8; i++)
	{
		symple_state_request_t s;
		s.isWrite = data[i*2] & 0x8000000;
		s.stateDwordId = data[i * 2] & (~0x8000000);
		s.requestedValue = data[(i * 2) + 1];


	}
}


saf_core_impl::saf_core_impl(/* args */)
{	
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

int saf_core_impl::Connect()
{
    #define MAX_STR 255   

	wchar_t wstr[MAX_STR];
	handle = hid_open(56, 78, NULL);
	if (handle == NULL)
	{
		std::cout << "Couldn't open device" << std::endl;
	} else {
		std::cout << "Connected" << std::endl;
		usbThread = std::thread(&saf_core_impl::runUsbThread, this);
	}
	return 0;
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
