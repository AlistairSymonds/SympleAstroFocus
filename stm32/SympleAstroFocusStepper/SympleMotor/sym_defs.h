
#define STATE_LENGTH_DWORDS 0x08
#define SYM_EP_SIZE (STATE_LENGTH_DWORDS * 8) //64bytes

#define STATE_ID_DWORD 0
#define STATE_ID_0 0x0000;


//definitions for fields in state ID 0
#define COMMAND_DWORD 1
#define STATUS_DWORD  2
#define CURRENT_POSITION_DWORD 3
#define SET_POSITION_DWORD 4
#define MAX_POSITION_DWORD 5
#define STEP_TIME 6
#define STEP_MODE 7


#define STATUS_REVERSE_BIT   ( 1 << 0)
#define STATUS_IS_MOVING_BIT ( 1 << 1)
#define STATUS_STALLED_BIT   ( 1 << 2)
#define STATUS_HOMING_BIT    ( 1 << 3)
