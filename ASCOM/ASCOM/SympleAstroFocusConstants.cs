using System;
public class Constants
{

    public const uint STATE_ID_DWORD = 0;
    public const uint STATE_ID_0 = 0;
    public const uint STATE_ID_HW_DEFS = 0xFFFFFFFF;



    public const uint COMMAND_DWORD = 1;
    public const uint STATUS_DWORD = 2;
    public const uint CURRENT_POSITION_DWORD = 3;
    public const uint SET_POSITION_DWORD = 4;
    public const uint MAX_POSITION_DWORD = 5;
    public const uint STEP_TIME_US_DWORD = 6;
    public const uint DRIVER_CONFIG_DWORD = 7;
    public const uint DRIVER_STATUS_DWORD = 8;

    [Flags]
    public enum Command_Dword_Bits : uint
    {
        TOGGLE_REVERSE_BIT  = 1 << 0,
        SET_ZERO_BIT        = 1 << 1,
        HALT_MOTOR_BIT      = 1 << 2,
        SAVE_TO_FLASH_BIT   = 1 << 3,
        TRIGGER_HOMING      = 1 << 5,
        TOGGLE_HOME_TOWARDS_ZERO = 1 << 6,
        TOGGLE_HOME_TOWARDS_MAX  = 1 << 7,
        UPDATE_SET_POS      = 1 << 8
    }

    [Flags]
    public enum Status_Dword_Bits : uint
    {
        STATUS_REVERSE_BIT   = 1 << 0,
        STATUS_IS_MOVING_BIT = 1 << 1,
        STATUS_STALLED_BIT   = 1 << 2,
        STATUS_HOMING_BIT    = 1 << 3,
        STATUS_HOMING_TOWARDS_ZERO_ENABLED      = 1 << 4,
        STATUS_HOMING_TOWARDS_MAX_ENABLED       = 1 << 5,
        STATUS_STEPPER_DRIVER_ERROR_BIT         = 1 << 6,
        STATUS_STEPPER_DRIVER_COMMS_ERROR_BIT   = 1 << 7,
        STATUS_STEPPER_DRIVER_ENABLED_BIT       = 1 << 8
    }

    public const uint DRIVER_CONFIG_IHOLD_MASK = 0x1F;
    public const int DRIVER_CONFIG_IHOLD_SHIFT = 0;

    public const uint DRIVER_CONFIG_IRUN_MASK = 0x3E0;
    public const int DRIVER_CONFIG_IRUN_SHIFT = 5;


    public const uint DRIVER_STATUS_SG_RESULT_MASK = 0x3FF;
    public const int DRIVER_STATUS_SG_RESULT_SHIFT = 0;

    public const uint DRIVER_STATUS_CS_ACTUAL_MASK = 0x1F0000;
    public const int DRIVER_STATUS_CS_ACTUAL_SHIFT = 16;

    public enum mcu_t : uint
    {
        UNDEFINED = 0,
        MCU_TYPE_STM32F103C8T6 = 1,
        MCU_TYPE_STM32F103T8U6 = 2
    }

    public enum stepper_driver_t : uint
    {
        UNDEFINED = 0,
        TMC2209 = 1
    }

}
