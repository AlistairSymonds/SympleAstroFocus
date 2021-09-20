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
Currently only one stateword is defined
+ State ID 0 (32'h00) :


## State ID 0 fields
| DWORD | Name | Encoding| Read/Write or Read Only |
| ---   |---   |---      |---                      |
|0      | state ID          | Constant, 32'h00| N/A |
|1      | Command Bits      | See below       |RW |
|2      | Status Flags      | See below       | RO |
|3      | Current Position  | Constant, 32'h00| RO |
|4      | Set Position      | Constant, 32'h00| RW |
|5      | Max Position      | Constant, 32'h00| RW |
|6      | Step Time         |                 |  RW  | 
|7      | Step Mode         |                 | RW |

### Status Flags 
|Bit  | 31 | 30 | 29 | 28 | 27 | 26 | 25 | 24 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    |    |

|Bit  | 23 | 22 | 21 | 20 | 19 | 18 | 17 | 16 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    |    |

|Bit  | 15 | 14 | 13 | 12 | 11 | 10 | 9 | 8 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    |    |

|Bit  | 7 | 6 | 5 | 4 | 3 | 2 | 1 | 0 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |  | |  |   | Set Zero to current pos   |  Toggle Reverse Step direction |

### Status Flags 
|Bit  | 31 | 30 | 29 | 28 | 27 | 26 | 25 | 24 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    |    |

|Bit  | 23 | 22 | 21 | 20 | 19 | 18 | 17 | 16 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    |    |

|Bit  | 15 | 14 | 13 | 12 | 11 | 10 | 9 | 8 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |    |    |    |    |    |    |

|Bit  | 7 | 6 | 5 | 4 | 3 | 2 | 1 | 0 |
| --- |--- |--- |--- |--- |--- |--- |--- |--- |
|Name |    |    |  Home towards max enabled  | Home towards 0 enabled   |  Homing   |   Stalled |   Moving |  Reverse Step direction |

## Out of host
This is how the host controls the device, a very similar scheme is taken to the In direction, where the Host gives back a stateword, and the device will update any of its writeable DWORD fields
with the corresponding values from the host, read only fields are not changed in anyway.



## Example
If the host wanted to move the focusser to position 12000, it would send the following command:
DWORD 0: State ID, set to 0
DWORD 3: Set position, set to 12000

Then it should wait until current position = set position = 12000 and the moving bit in the status flags to be low before reporting to the application that the move is complete.
