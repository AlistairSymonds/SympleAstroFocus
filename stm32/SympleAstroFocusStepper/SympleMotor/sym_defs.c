#include "sym_defs.h"

void process_command_bits(symple_state_t ss){

	//is the bit in the command dword set?
	//if so go into the state, do the op, and clear the command bit
	if (ss[STATE_ID_0][COMMAND_DWORD] & COMMAND_TOGGLE_REVERSE_BIT){
		ss[STATE_ID_0][STATUS_DWORD] ^= STATUS_REVERSE_BIT;
		ss[STATE_ID_0][COMMAND_DWORD] &= ~COMMAND_TOGGLE_REVERSE_BIT;
		//this is persistent state, so trigger a save
		ss[STATE_ID_0][COMMAND_DWORD] |= COMMAND_SAVE_TO_FLASH_BIT;
	}

	if (ss[STATE_ID_0][COMMAND_DWORD] & COMMAND_SET_ZERO_BIT){
		ss[STATE_ID_0][CURRENT_POSITION_DWORD] = 0;
		ss[STATE_ID_0][COMMAND_DWORD] &= ~COMMAND_SET_ZERO_BIT;
	}

	if (ss[STATE_ID_0][COMMAND_DWORD] & COMMAND_HALT_BIT){
		ss[STATE_ID_0][SET_POSITION_DWORD] = ss[STATE_ID_0][CURRENT_POSITION_DWORD];
		ss[STATE_ID_0][STATUS_DWORD]  &= ~STATUS_HOMING_BIT;
		ss[STATE_ID_0][COMMAND_DWORD] &= ~COMMAND_HALT_BIT;
	}

	if (ss[STATE_ID_0][COMMAND_DWORD] & COMMAND_TOGGLE_HOME_TOWARDS_ZERO_BIT){
		ss[STATE_ID_0][STATUS_DWORD] ^= STATUS_HOME_TOWARDS_ZERO_ENABLED;
	}

	if (ss[STATE_ID_0][COMMAND_DWORD] & COMMAND_TOGGLE_HOME_TOWARDS_MAX_BIT){
		ss[STATE_ID_0][STATUS_DWORD] ^= STATUS_HOME_TOWARDS_MAX_ENABLED;
	}

	//only actually do homing if either are enabled
	if (ss[STATE_ID_0][COMMAND_DWORD] & COMMAND_TRIGGER_HOMING_BIT &&
			ss[STATE_ID_0][STATUS_DWORD] & (STATUS_HOME_TOWARDS_MAX_ENABLED | STATUS_HOME_TOWARDS_ZERO_ENABLED)
	){
		ss[STATE_ID_0][STATUS_DWORD] |= STATUS_HOMING_BIT;
	}


}

void save_recieved_state(uint32_t* state_in , symple_state_t ss){
	uint32_t state_id;
	  state_id = state_in[STATE_ID_DWORD];
	  for (int i = 1; i < STATE_LENGTH_DWORDS; i++){
		  int is_writable_dword = sym_state_writeable_dwords[state_id] & (1 << i);

		  if (i == SET_POSITION_DWORD){
			  is_writable_dword = state_in[COMMAND_DWORD] & COMMAND_UPDATE_SET_POS_BIT;
		  }

		  if (is_writable_dword){
			  uint32_t data = state_in[i];
			  ss[state_id][i] = data;
		  }
	  }
}
