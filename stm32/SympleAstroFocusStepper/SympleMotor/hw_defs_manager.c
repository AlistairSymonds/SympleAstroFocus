/*
 * hw_defs.c
 *
 *  Created on: 27 May 2022
 *      Author: alist
 */
#include "hw_defs_manager.h"
void request_transmission_of_hw_defs(){
	hw_defs_requested = 1;
}

int is_requested_hw_defs(){
	return hw_defs_requested;
}
