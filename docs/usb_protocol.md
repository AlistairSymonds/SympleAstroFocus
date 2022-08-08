# THIS NEEDS REJIGGING - packing multiple state into one big word might be easy on the surface but hard to handle both host and device modifying same fields and keeping in sync who is right when changing other field instead

# Device Descriptor
VID: 56

PID: 78

iProduct must be: SympleAstroFocus

Type: HID

# HID Config:

# Endpoints:

## In endpoint
+ Size: 0x40 (64 bytes)
+ EP addr: 0x01
+ Type: Interrupt 

## Out endpoint
+ Size: 0x40 (64 bytes)
+ EP addr: 0x81
+ Type: Interrupt 


# Data

In both directions the data sent over USB is broken up into repeated chunks of packets which take the following form:

|Bits  | 63 - 32 | 31 | 30 - 0 |
| --- |--- |--- |--- |
|Field | Value to be written or Read val  | R = 0, W = 1 | Dword ID   |

An invalid packet that should be ignored take this form instead:
|Bits  | 63 - 32 | 31 - 0 |
| --- |--- |--- |
|Field | Ignored   | 0xFFFF_FFFF   |


Then these are laid out linearly across the whole endpoint in a transaction, multiple packets can be batched together with unused packets being marked as invalid.

```

63     56   55    48   47    40   39    32   31    24    23   16   15     8   7        byte 0                           
v        v v        v v        v v        v v        v v        v v        v v        v
+--------+ +--------+ +--------+ +--------+ +--------+ +--------+ +--------+ +--------+
|Packet  | |Packet  | |Packet  | |Packet  | |Packet  | |Packet  | |Packet  | |Packet  |
+--------+ +--------+ +--------+ +--------+ +--------+ +--------+ +--------+ +--------+
```

## Out of host
The hose will create USB transfers made up of one or more of these packets, which the device will then respond to the command based on the dwords below, either updating the field if its a writeable dword and write command or returning data on a read. The device will ignore write commands to any read only dwords.

## In to Host
Whilst there are separate read/write command the host can send to the device, the device is alloweed to send read data back to the host at any time regardless of whether it has been requested and the host must be able to handle it.



## State dwords 
| DWORD | Name | Encoding| Read/Write or Read Only |
| ---   |---   |---      |---                      |
|1      | Command Bits      | See below       |RW |
|2      | Status Flags      | See below       | RO |
|3      | Current Position  | Constant, 32'h00| RO |
|4      | Set Position      | Constant, 32'h00| RW |
|5      | Max Position      | Constant, 32'h00| RW |
|6      | Step Time  microsec       |                 |  RW  | 
|7      | Driver Config         |                 | RW |
|8      | Driver Status         |                 | RO |
|0x3FFFFFF9      | Commit ID of FW      | See below       | RO |
|0x3FFFFFFA      | Stepper Driver Type  | Constant, 32'h00| RO |
|0x3FFFFFFB      | MCU Type      | | RO |
|0x3FFFFFFC      | Firmware state      | | RO |
|0x3FFFFFFD      | GUID[31:0]       |                 |  RO  | 
|0x3FFFFFFE      | GUID[63:32]         |                 | RO |
|0x3FFFFFFF      | GUID[95:64]         |                 | RO |



### Command Bits
These are cleared once the sepcified operation is complete
|Bit  | 31 | 30 | 29 | 28 | 27 | 26 | 25 | 24 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    |    |

|Bit  | 23 | 22 | 21 | 20 | 19 | 18 | 17 | 16 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    |    |

|Bit  | 15 | 14 | 13 | 12 | 11 | 10 | 9 | 8 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    | Update Set Pos  |

|Bit  | 7 | 6 | 5 | 4 | 3 | 2 | 1 | 0 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |  Toggle Home Towards Max  |  Toggle Home Towards Zero | Trigger Homing | Stall detection enabled | Save state to Flash | Halt Motor  | Set Zero to current pos   |  Toggle Reverse Step direction |

### Status Flags 
|Bit  | 31 | 30 | 29 | 28 | 27 | 26 | 25 | 24 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    |    |

|Bit  | 23 | 22 | 21 | 20 | 19 | 18 | 17 | 16 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    |    |

|Bit  | 15 | 14 | 13 | 12 | 11 | 10 | 9 | 8 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    | Stepper driver enabled   |

|Bit  | 7 | 6 | 5 | 4 | 3 | 2 | 1 | 0 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name | Stepper Driver communication error   |  Stepper driver drror  |  Home towards max enabled  | Home towards 0 enabled   |  Homing   |   Stalled |   Moving |  Reverse step direction |

### Driver Config Options 
|Bit  | 31 | 30 | 29 | 28 | 27 | 26 | 25 | 24 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    |    |

|Bit  | 23 | 22 | 21 | 20 | 19 | 18 | 17 | 16 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |SGTHRS_MSB|<--|---|---|

|Bit  | 15 | 14 | 13 | 12 | 11 | 10 | 9 | 8 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |---|---|---|---|-->|SGTHRS_LSB|  IRUN_MSB  |<--|

|Bit  | 7 | 6 | 5 | 4 | 3 | 2 | 1 | 0 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name|---|-->|IRUN_LSB|IHOLD_MSB|<--|---|-->| IHOLD_LSB |

### Driver Status Flags 
|Bit  | 31 | 30 | 29 | 28 | 27 | 26 | 25 | 24 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name | Standstill   |    |    |    |    |    |    |    |

|Bit  | 23 | 22 | 21 | 20 | 19 | 18 | 17 | 16 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    | CS_ACTUAL MSB  |  <--  |  ---  |  -->  | CS_ACTUAL LSB   |

|Bit  | 15 | 14 | 13 | 12 | 11 | 10 | 9 | 8 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name | s2vsb   |  s2vsa  | s2gb | s2ga   |  Overtemp  | Overtemp warning   |  SG_RESULT MSB  | <--|

|Bit  | 7 | 6 | 5 | 4 | 3 | 2 | 1 | 0 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name|---|---|---|---|---|---| --> |   SG_RESULT LSB |



## Example
If the host wanted to move the focusser to position 12000, it would send the following command:
DWORD 3: Set position, write, set to 12000

Then it should wait until the moving bit read out of the status flags is low before declaring the move complete.
