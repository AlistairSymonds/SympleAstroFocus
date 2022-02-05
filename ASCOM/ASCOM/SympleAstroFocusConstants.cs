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
    public const int STEP_TIME_MS_DWORD = 6;
    public const int DRIVER_CONFIG_DWORD = 7;
    public const int DRIVER_STATUS_DWORD = 8;

    [Flags]
    public enum Command_Dword_Bits : uint
    {
        TOGGLE_REVERSE_BIT  = 1 << 0,
        SET_ZERO_BIT        = 1 << 1,
        HALT_MOTOR_BIT      = 1 << 2,
        SAVE_TO_FLASH_BIT   = 1 << 3
    }

    [Flags]
    public enum Status_Dword_Bits : uint
    {
        STATUS_REVERSE_BIT   = 1 << 0,
        STATUS_IS_MOVING_BIT = 1 << 1,
        STATUS_STALLED_BIT   = 1 << 2,
        STATUS_HOMING_BIT    = 1 << 3
    }

    public const int DRIVER_STATUS_SG_RESULT_MASK = 0x3FF;
    public const int DRIVER_STATUS_SG_RESULT_SHIFT = 0;

    public const int DRIVER_STATUS_CS_ACTUAL_MASK = 0x1F0000;
    public const int DRIVER_STATUS_CS_ACTUAL_SHIFT = 16;


}
