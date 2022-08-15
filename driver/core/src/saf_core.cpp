#include <iostream>
#include "hidapi.h"
#include "saf_core.h"




class saf_core_impl : public saf_core
{
private:
    /* data */
public:
    saf_core_impl(/* args */);
    int connect();
    ~saf_core_impl();
};

std::unique_ptr<saf_core> saf_core_factory::create(){
	return std::make_unique<saf_core_impl>(saf_core_impl());
}


saf_core_impl::saf_core_impl(/* args */)
{
}

int saf_core_impl::connect()
{

    #define MAX_STR 255   
	int res;
	unsigned char buf[65];
	wchar_t wstr[MAX_STR];
	hid_device *handle;

	// Initialize the hidapi library
	res = hid_init();

	handle = hid_open(56, 78, NULL);
	if (handle == NULL)
	{
		std::cout << "Couldn't open device" << std::endl;
	}
	/*
	// Read the Manufacturer String
	res = hid_get_manufacturer_string(handle, wstr, MAX_STR);
	std::wstring mstr = std::wstring(wstr); 
	std::wcout << L"Manufacturer String " << mstr << std::endl;


	// Read the Product String
	res = hid_get_product_string(handle, wstr, MAX_STR);
	mstr = std::wstring(wstr); 
	std::wcout << L"Product String " << mstr << std::endl;

	// Read the Serial Number String
	res = hid_get_serial_number_string(handle, wstr, MAX_STR);
	mstr = std::wstring(wstr); 
	std::wcout << L"Serial String " << mstr << std::endl;

	// Read Indexed String 1
	res = hid_get_indexed_string(handle, 1, wstr, MAX_STR);
	mstr = std::wstring(wstr); 
	std::wcout << L"Indexed String 1 " << mstr << std::endl;

	// Read requested state
	res = hid_read(handle, buf, 65);

	// Print out the returned buffer.
	for (int i = 0; i < 65; i++)
		std::wcout << (L"buf[%d]: %d\n", i, buf[i]) << std::endl;
*/
	// Close the device
	hid_close(handle);

	// Finalize the hidapi library
	res = hid_exit();

	return 0;
}


saf_core_impl::~saf_core_impl()
{
}
