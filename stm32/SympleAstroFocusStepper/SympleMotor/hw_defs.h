/*
 * hw_defs.h
 *
 *  Created on: 27 May 2022
 *      Author: alist
 */

#ifndef HW_DEFS_H_
#define HW_DEFS_H_
#include <stdint.h>
#define STEPPER_DRIVER_UNDEFINED 0
#define STEPPER_DRIVER_TMC2209 1

#define MCU_TYPE_UNDEFINED 0
#define MCU_TYPE_STM32F103C8T6 1
#define MCU_TYPE_STM32F103T8U6 0

#define FW_STATE_DEV 0
#define FW_STATE_RELEASE 10



#ifdef RELEASE
#include "relese_hw_defs_vals.h"
#else
#define HW_DEF_FW_STATE FW_STATE_DEV
#define HW_DEF_MCU_TYPE MCU_TYPE_UNDEFINED
#define HW_DEF_STEPPER_DRIVER STEPPER_DRIVER_UNDEFINED
#define HW_DEF_GIT_COMMIT_ID 0
#endif
#endif /* HW_DEFS_H_ */
