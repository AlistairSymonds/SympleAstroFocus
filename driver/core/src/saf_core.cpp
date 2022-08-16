#include <iostream>
#include <thread>
#include <chrono>
#include "hidapi.h"
#include "saf_core.h"




class saf_core_impl : public saf_core
{
private:
    //std::thread usb_thread;
	hid_device *handle;
public:
    saf_core_impl();
    int Connect();
	int DumpUsb();
    ~saf_core_impl();
};

std::unique_ptr<saf_core> saf_core_factory::create(){
	
	std::cout << "bout to ccreate" << std::endl;
	return std::make_unique<saf_core_impl>(saf_core_impl());
}


saf_core_impl::saf_core_impl(/* args */)
{	
	handle = NULL;
	std::cout << "Constructing" << std::endl;
}

int saf_core_impl::Connect()
{

	
    #define MAX_STR 255   

	wchar_t wstr[MAX_STR];
	
	
	hid_init();
	handle = hid_open(56, 78, NULL);
	if (handle == NULL)
	{
		std::cout << "Couldn't open device" << std::endl;
	} else {
		std::cout << "Connected" << std::endl;

	}
	return 0;
}

int saf_core_impl::DumpUsb(){
	int res;
	unsigned char buf[65];
	std::this_thread::sleep_for(std::chrono::seconds(1));
	res = hid_read(handle, buf, 65);
	for (int i = 0; i < 65; i++)
		std::cout << "buf[" << i << "]: "  << std::hex << +buf[i] << std::endl;
	
	return res;

}
saf_core_impl::~saf_core_impl()
{
	if (handle != NULL)
	{
		hid_close(handle);
	}
	hid_exit();
	
}
