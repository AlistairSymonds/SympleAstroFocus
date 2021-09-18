using System;
public class Constants
{

    public const int STATE_ID_DWORD = 0;
    public const int STATE_ID_0 = 0;



    public const int COMMAND_DWORD = 1;
    public const int STATUS_DWORD = 2;
    public const int CURRENT_POSITION_DWORD = 3;
    public const int SET_POSITION_DWORD = 4;
    public const int MAX_POSITION_DWORD = 5;
    public const int STEP_TIME = 6;
    public const int STEP_MODE = 7;

    [Flags]
    public enum Status_Dword_Bits : uint
    {
        STATUS_REVERSE_BIT   = 1 << 0,
        STATUS_IS_MOVING_BIT = 1 << 1,
        STATUS_STALLED_BIT   = 1 << 2,
        STATUS_HOMING_BIT    = 1 << 3
    }
}
