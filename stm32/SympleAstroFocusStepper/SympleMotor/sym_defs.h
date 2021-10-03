#ifndef SYM_DEFS_H
#define SYM_DEFS_H
#include <stdint.h>

#define NUM_STATE_INFOS 1
#define STATE_LENGTH_DWORDS 0x10 //16 dwords per 64bytes
#define SYM_EP_SIZE (STATE_LENGTH_DWORDS * 4) //64bytes

#define SYMPLE_STATE_FLASH_BASE_ADDR
#define SYMPLE_STATE_FLASH_SIZE (STATE_LENGTH_DWORDS-1) * NUM_STATE_INFOS

#define STATE_ID_DWORD 0
#define STATE_ID_0 0x0000


//definitions for fields in state ID 0
#define COMMAND_DWORD 1
#define STATUS_DWORD  2
#define CURRENT_POSITION_DWORD 3
#define SET_POSITION_DWORD 4
#define MAX_POSITION_DWORD 5
#define STEP_TIME 6
#define STEP_MODE 7


#define COMMAND_TOGGLE_REVERSE_BIT   ( 1 << 0)
#define COMMAND_SET_ZERO_BIT   ( 1 << 1)
#define COMMAND_HALT_BIT   	( 1 << 2)

#define STATUS_REVERSE_BIT   ( 1 << 0)
#define STATUS_IS_MOVING_BIT ( 1 << 1)
#define STATUS_STALLED_BIT   ( 1 << 2)
#define STATUS_HOMING_BIT    ( 1 << 3)



typedef volatile uint32_t symple_state_t[NUM_STATE_INFOS][STATE_LENGTH_DWORDS-1];

symple_state_t symple_state;
static const uint16_t sym_state_writeable_dwords[NUM_STATE_INFOS]={
		0xF2
};

void process_command_bits(symple_state_t ss);

void save_recieved_state(uint32_t* state_in , symple_state_t ss);

#endif
