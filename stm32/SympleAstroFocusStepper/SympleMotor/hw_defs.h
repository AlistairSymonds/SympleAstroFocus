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
#define MCU_TYPE_STM32F103C8T6
#define MCU_TYPE_STM32F103T8U6

#define FW_STATE_DEV 0
#define FW_STATE_RELEASE 10

typedef struct hw_defs_t{
	uint32_t fw_git_commit;
	uint32_t mcu_type;
	uint32_t stepper_driver_type;
	uint32_t release_type;
} hw_defs_t;



static const hw_defs_t hw_defs = {
#ifdef RELEASE
#include "relese_hw_defs_vals.h"
#else
.fw_git_commit = 0,
.mcu_type = MCU_TYPE_UNDEFINED,
.stepper_driver_type = STEPPER_DRIVER_TMC2209,
.release_type = FW_STATE_RELEASE
#endif
};
#endif /* HW_DEFS_H_ */
