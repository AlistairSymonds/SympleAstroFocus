#include "sym_defs.h"

void process_command_bits(){
	if (symple_state[COMMAND_DWORD] & COMMAND_TOGGLE_REVERSE_BIT){
		symple_state[STATUS_DWORD] ^= STATUS_REVERSE_BIT;
		symple_state[COMMAND_DWORD] &= ~STATUS_REVERSE_BIT;
	}

	if (symple_state[COMMAND_DWORD] & COMMAND_TOGGLE_REVERSE_BIT){
		symple_state[CURRENT_POSITION_DWORD] = 0;
		symple_state[COMMAND_DWORD] &= ~COMMAND_TOGGLE_REVERSE_BIT;
	}


}
