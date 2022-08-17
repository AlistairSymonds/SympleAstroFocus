# Device Descriptor
 * **VID**: 0x0038 (decimal 56)
 * **PID**: 0x004e (decimal 78)
 * **Product**: Symple Astro Focus Stepper
 * **Serial**: is valid and is based upon the microcontroller uniqued identifier.
 * **Type**: HID

# HID Config

## Endpoints

Note: All values are little endian.

### Host to Device
+ Size: 0x40 (64 bytes)
+ EP addr: 0x01
+ Type: Interrupt

### Device to Host
+ Size: 0x40 (64 bytes)
+ EP addr: 0x81
+ Type: Interrupt


## Data

In both directions the data sent over the HID interface is broken up into repeated chunks of packets which take the following form:

|Bits  | 63 - 32 | 31 | 30 - 0 |
| --- |--- |--- |--- |
|**Field** | Value to be written or Read val  | R = 0, W = 1 | Field ID   |

An invalid packet that should be ignored take this form instead:
|Bits  | 63 - 32 | 31 - 0 |
| --- |--- |--- |
|**Field** | Ignored   | 0xFFFF_FFFF   |


Multiple packets can be included within the 64 byte endpoint and unused packet slots will be marked invalid.

```

63     56   55    48   47    40   39    32   31    24    23   16   15     8   7        byte 0
v        v v        v v        v v        v v        v v        v v        v v        v
+--------+ +--------+ +--------+ +--------+ +--------+ +--------+ +--------+ +--------+
|Packet  | |Packet  | |Packet  | |Packet  | |Packet  | |Packet  | |Packet  | |Packet  |
+--------+ +--------+ +--------+ +--------+ +--------+ +--------+ +--------+ +--------+
```

## From Host to Device
The hose will create transfers by concatenating one or more of these packets and writing these to the Host to Device endpoint. The device will then respond to the commands by writing responses to the Device to Host endpoint. If a write is issued to the device, the relevant field will be updated whilst a read will initiate the return of the field present value. The device will ignore write commands to any read only Field IDs.

## From Device to Host
The device is allowed to send data back to the host at any time regardless of incoming commands and this must be handled by the host. The device will not issue write commands to the host.



## Field ID Map
| Field ID | Name | Encoding| Read/Write or Read Only |
| ---   |---   |---      |---                      |
|0x00000001      | Commands      | [Command Bits](#command-bits)      |RW |
|0x00000002      | Status Flags      | [Status Flags](#status-flags)      | RO |
|0x00000003      | Current Position  | 32 bit integer| RO |
|0x00000004      | Set Position      | 32 bit integer| RW |
|0x00000005      | Max Position      | 32 bit integer| RW |
|0x00000006      | Step Time  microsec       | 32 bit integer? |  RW  |
|0x00000007      | Driver Configuration   | [Driver Configuration](#driver-config-options)                 | RW |
|0x00000008      | Driver Status         | [Driver Status](#driver-status-flags)  | RO |
|0x3FFFFFF9      | Firmware Commit ID      | [git Commit ID](#git-commit-id)       | RO |
|0x3FFFFFFA      | Stepper Driver Type  | Unknown? | RO |
|0x3FFFFFFB      | MCU Type      | Unknown? | RO |
|0x3FFFFFFC      | Firmware state      | Unknown? | RO |
|0x3FFFFFFD      | GUID[31:0]       |                 |  RO  |
|0x3FFFFFFE      | GUID[63:32]         |                 | RO |
|0x3FFFFFFF      | GUID[95:64]         |                 | RO |



### Command Bits
These are cleared once the specified command  is complete.

| Bit  | Name |
| --- |--- |
| 31-9 | *Unused* |
| 8 | Update Set (Target) Position |
| 7 | Toggle Home in Positive Direction |
| 6 | Toggle Home in Negative Direction |
| 5 | Trigger Homing |
| 4 | Enable Stall Detection |
| 3 | Save State to Flash |
| 2 | Halt Motor |
| 1 | Set Current Position to Zero |
| 0 | Toggle Reverse Step Direction |

### Status Flags
| Bit  | Name |
| --- |--- |
| 31-9 | *Unused* |
| 8 | Stepper Driver Enabled |
| 7 | Stepper Driver Communication Error |
| 6 | Stepper Driver Error |
| 5 | Home in Positive Direction |
| 4 | Home in Negative Direction |
| 3 | Homing |
| 2 | Stalled |
| 1 | Moving |
| 0 | Step Direction Reversed |

### Driver Config Options
Note: All values are little endian.
| Bit  | Name |
| --- |--- |
| 31-20 | *Unused* |
| 19-10 | Stallguard Threshold |
| 9-5 | Motor Run Current |
| 4-0 | Motor Hold Current |

### Driver Status Flags
| Bit  | Name |
| --- |--- |
| 31 | Motor Stopped |
| 30-21 | *Unused* |
| 20-16 | Motor Actual Current |
| 15 | S2VSB: Low side short, Phase B   |
| 14 | S2VSA: Low side short, Phase A |
| 13 | S2GB: Short to Ground, Phase B |
| 12 | S2GA: Short to Ground, Phase A |
| 11 | Overtemperature Error |
| 10 | Overtermperature Warning |
| 9-0 | Stallguard Actual Value |

### git Commit ID
The value of this response can be treated as integer and formatted into a fixed width 7 character hexadecimal field to represent the short form git commit the firmware was built from.



# Example
1) If the host wanted to move the focusser to position 12000, it would send the following command:
Field ID 0x00000004 with a payload of 12000 (0x2EE0). The complete command would encode as:
```
(12000) << 32 | (1 << 31) | (0x04) ≡ 0x2ee080000004 ≡ [0, 0, 46, 224, 128, 0, 0, 4]
```
2) This is then written to the Host to Device endpoint.
3) The device will start moving to the target.
4) The host is then able to poll the Moving bit (0x01) in [Status Flags](#status-flags) which will clear upon the completion of the move.

