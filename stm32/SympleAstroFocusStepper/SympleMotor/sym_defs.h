#ifndef SYM_DEFS_H
#define SYM_DEFS_H
#include <stdint.h>

#define SYM_EP_SIZE 64 //64bytes
#define SYM_EP_SIZE_DWORDS 16
#define NUM_MUTABLE_STATE_WORDS 16

#define SYMPLE_STATE_FLASH_SIZE NUM_MUTABLE_STATE_WORDS


//Definitions for stuff being saved/restored
#define COMMAND_DWORD 1
#define STATUS_DWORD  2
#define CURRENT_POSITION_DWORD 3
#define SET_POSITION_DWORD 4
#define MAX_POSITION_DWORD 5
#define STEP_TIME_MICROSEC 6
#define STEPPER_DRIVER_CONF 7
#define STEPPER_DRIVER_STATUS 8

//Hardcoded read only state
#define GUID_2_DWORD 0x3FFFFFFF
#define GUID_1_DWORD 0x3FFFFFFE
#define GUID_0_DWORD 0x3FFFFFFD
#define FW_DWORD     0x3FFFFFFC
#define MCU_DWORD    0x3FFFFFFB
#define STEPPER_DRIVER_TYPE_DWORD 0x3FFFFFFA
#define FW_COMMIT_DWORD 0x3FFFFFF9

#define INVALID_DWORD 0xFFFFFFFF
#define WRITE_BIT 0x80000000


#define COMMAND_TOGGLE_REVERSE_BIT  ( 1 << 0)
#define COMMAND_SET_ZERO_BIT   		( 1 << 1)
#define COMMAND_HALT_BIT   			( 1 << 2)
#define COMMAND_SAVE_TO_FLASH_BIT  	( 1 << 3)
#define COMMAND_TRIGGER_HOMING_BIT	( 1 << 5)

#define COMMAND_TOGGLE_HOME_TOWARDS_ZERO_BIT  	( 1 << 6)
#define COMMAND_TOGGLE_HOME_TOWARDS_MAX_BIT  	( 1 << 7)
#define COMMAND_UPDATE_SET_POS_BIT			  	( 1 << 8)

#define STATUS_REVERSE_BIT   					( 1 << 0)
#define STATUS_IS_MOVING_BIT 					( 1 << 1)
#define STATUS_STALLED_BIT   					( 1 << 2)
#define STATUS_HOMING_BIT    					( 1 << 3)
#define STATUS_HOME_TOWARDS_ZERO_ENABLED    	( 1 << 4)
#define STATUS_HOME_TOWARDS_MAX_ENABLED    		( 1 << 5)
#define STATUS_STEPPER_DRIVER_ERROR_BIT    		( 1 << 6)
#define STATUS_STEPPER_DRIVER_COMMS_ERROR_BIT   ( 1 << 7)
#define STATUS_STEPPER_DRIVER_ENABLED_BIT    	( 1 << 8)

#define DRIVER_CONFIG_IHOLD_MASK				( 0x1F)
#define DRIVER_CONFIG_IHOLD_SHIFT				0
#define DRIVER_CONFIG_IRUN_MASK					( 0x3E0)
#define DRIVER_CONFIG_IRUN_SHIFT                5
#define DRIVER_CONFIG_SGTHRS_MASK               ( 0xFFC00)
#define DRIVER_CONFIG_SGTHRS_SHIFT              10

#define DRIVER_STATUS_SG_RESULT_MASK 			( 0x3FF)
#define DRIVER_STATUS_SG_RESULT_SHIFT			0

#define DRIVER_STATUS_CS_ACTUAL_MASK 			( 0x1F0000)
#define DRIVER_STATUS_CS_ACTUAL_SHIFT			16

typedef volatile uint32_t symple_state_t[NUM_MUTABLE_STATE_WORDS];

symple_state_t symple_state;

void read_state_word(uint32_t idx, symple_state_t ss);

void process_command_bits(symple_state_t ss);

void save_recieved_state(uint32_t usb_data[SYM_EP_SIZE_DWORDS], symple_state_t ss);

#endif
