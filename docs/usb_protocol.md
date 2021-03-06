# THIS NEEDS REJIGGING - packing multiple state into one big word might be easy on the surface but hard to handle both host and device modifying same fields and keeping in sync who is right when changing other field instead

# Device Descriptor
VID: TBC

PID: TBC

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

## In to Host
All data types into the host are logically broken up into DWORD chunks - that is eight fields across 64bytes. The device is
responsible for updating the host within a reasonable amount of time when something changes, and is encouraged to continually 
update atleast once a second even in cases where nothing has changed to add robustness.

This updates are sent as state words containing arbitrary data about the current state of the focusser 

Nominally enocded as:
```
byte 63                                                                     byte 4  byte 3 byte 0
v                                                                                v  v      v
+---------------------------------------------------------------------------------++--------+
|                                                                                 ||State ID|
+---------------------------------------------------------------------------------++--------+
```
## State IDs
Currently only the following statewords are defined, if other stepper drivers are supported they will likely have their own state id
+ GENERAL_STATE (32'h00)

## State ID 0 fields
| DWORD | Name | Encoding| Read/Write or Read Only |
| ---   |---   |---      |---                      |
|0      | state ID          | Constant, 32'h00| N/A |
|1      | Command Bits      | See below       |RW |
|2      | Status Flags      | See below       | RO |
|3      | Current Position  | Constant, 32'h00| RO |
|4      | Set Position      | Constant, 32'h00| R, Write when enable cmd bit set |
|5      | Max Position      | Constant, 32'h00| RW |
|6      | Step Time  microsec       |                 |  RW  | 
|7      | Driver Config         |                 | RW |
|8      | Driver Status         |                 | R |

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

## Out of host
This is how the host controls the device, a very similar scheme is taken to the In direction, where the Host gives back a stateword, and the device will update any of its writeable DWORD fields
with the corresponding values from the host, read only fields are not changed in anyway.



## Example
If the host wanted to move the focusser to position 12000, it would send the following command:
DWORD 0: State ID, set to 0
DWORD 3: Set position, set to 12000

Then it should wait until current position = set position = 12000 and the moving bit in the status flags to be low before reporting to the application that the move is complete.
