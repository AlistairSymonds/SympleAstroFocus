#include "sym_defs.h"

void process_command_bits(symple_state_t ss){

	//is the bit in the command dword set?
	//if so go into the state, do the op, and clear the command bit
	if (ss[STATE_ID_0][COMMAND_DWORD] & COMMAND_TOGGLE_REVERSE_BIT){
		ss[STATE_ID_0][STATUS_DWORD] ^= STATUS_REVERSE_BIT;
		ss[STATE_ID_0][COMMAND_DWORD] &= ~COMMAND_TOGGLE_REVERSE_BIT;
	}

	if (ss[STATE_ID_0][COMMAND_DWORD] & COMMAND_SET_ZERO_BIT){
		ss[STATE_ID_0][CURRENT_POSITION_DWORD] = 0;
		ss[STATE_ID_0][COMMAND_DWORD] &= ~COMMAND_SET_ZERO_BIT;
	}

	if (ss[STATE_ID_0][COMMAND_DWORD] & COMMAND_HALT_BIT){
		ss[STATE_ID_0][SET_POSITION_DWORD] = ss[STATE_ID_0][CURRENT_POSITION_DWORD];
		ss[STATE_ID_0][COMMAND_DWORD] &= ~COMMAND_HALT_BIT;
	}


}
