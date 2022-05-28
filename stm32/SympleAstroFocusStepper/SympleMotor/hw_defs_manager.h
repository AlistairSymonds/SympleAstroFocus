/*
 * hw_defs.h
 *
 *  Created on: 27 May 2022
 *      Author: alist
 */

#ifndef HW_DEFS_MANAGER_H_
#define HW_DEFS_MANAGER_H_

#include "hw_defs.h"

volatile int hw_defs_requested;




void request_transmission_of_hw_defs();

int is_requested_hw_defs();


#endif /* HW_DEFS_MANAGER_H_ */
