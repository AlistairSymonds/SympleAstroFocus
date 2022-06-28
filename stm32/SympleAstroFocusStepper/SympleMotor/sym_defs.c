#include "sym_defs.h"
#include "hw_defs.h"
void process_command_bits(symple_state_t ss){

	//is the bit in the command dword set?
	//if so go into the state, do the op, and clear the command bit
	if (ss[COMMAND_DWORD] & COMMAND_TOGGLE_REVERSE_BIT){
		ss[STATUS_DWORD] ^= STATUS_REVERSE_BIT;
		ss[COMMAND_DWORD] &= ~COMMAND_TOGGLE_REVERSE_BIT;
		//this is persistent state, so trigger a save
		ss[COMMAND_DWORD] |= COMMAND_SAVE_TO_FLASH_BIT;
	}

	if (ss[COMMAND_DWORD] & COMMAND_SET_ZERO_BIT){
		ss[CURRENT_POSITION_DWORD] = 0;
		ss[SET_POSITION_DWORD] = 0;
		ss[COMMAND_DWORD] &= ~COMMAND_SET_ZERO_BIT;
	}

	if (ss[COMMAND_DWORD] & COMMAND_TOGGLE_HOME_TOWARDS_ZERO_BIT){
		ss[STATUS_DWORD] ^= STATUS_HOME_TOWARDS_ZERO_ENABLED;
		ss[COMMAND_DWORD] &= ~COMMAND_TOGGLE_HOME_TOWARDS_ZERO_BIT;
	}

	if (ss[COMMAND_DWORD] & COMMAND_TOGGLE_HOME_TOWARDS_MAX_BIT){
		ss[STATUS_DWORD] ^= STATUS_HOME_TOWARDS_MAX_ENABLED;
		ss[COMMAND_DWORD] &= ~COMMAND_TOGGLE_HOME_TOWARDS_MAX_BIT;
	}

}

int is_writeable_state_dword(uint32_t state_dword_id){
	if(
		state_dword_id == COMMAND_DWORD ||
		state_dword_id == SET_POSITION_DWORD ||
		state_dword_id == MAX_POSITION_DWORD ||
		state_dword_id == STEP_TIME_MICROSEC ||
		state_dword_id == STEPPER_DRIVER_CONF
	){
		return 1;
	} else {
		return 0;
	}
}

void save_recieved_state(uint32_t usb_data[SYM_EP_SIZE_DWORDS], symple_state_t ss){
	for (int i = 0; i < SYM_EP_SIZE_DWORDS; i+= 2) {
		uint32_t state_id = usb_data[i] & 0x7FFFFFFF;
		uint32_t is_write_req = usb_data[i] & WRITE_BIT;
		if (usb_data[i] != INVALID_DWORD && is_write_req && is_writeable_state_dword(state_id)){
			ss[state_id] = usb_data[i+1];
		}
	}

}
