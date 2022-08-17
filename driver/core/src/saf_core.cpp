#include <iostream>
#include <thread>
#include <mutex>
#include <chrono>
#include "hidapi.h"
#include "saf_core.h"




class saf_core_impl : public SympleAFCore
{
private:
	std::atomic<bool> stopUsbThread;
	std::mutex commandQueueMutex;
	std::thread usbThread;
	void runUsbThread();
	hid_device *handle;
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
		uint8_t readData[65];
		uint32_t readDataDwords[65];
		bytesRead = hid_read(handle, readData, 65);
		for (size_t i = 0; i < (64/4); i++)
		{
			int b = i + 1;
			readDataDwords[i] = 0;
			readDataDwords[i] |= readData[b];
			readDataDwords[i] |= readData[b+1] << 8;
			readDataDwords[i] |= readData[b+2] << 16;
			readDataDwords[i] |= readData[b+3] << 24;
		}

		for (size_t i = 0; i < bytesRead/4; i++)
		{
			printf("%08x ", readDataDwords[i]);
		}
		std::cout << std::endl;
		std::this_thread::sleep_for(std::chrono::milliseconds(50));
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
	std::cout << "Constructing" << std::endl;
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
