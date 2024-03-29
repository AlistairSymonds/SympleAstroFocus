# SympleAstroFocus
A symple AF DIY way to control a stepper motor from a PC for astronomy

The aims of the project is simple setup, DIY hardware creation and use. However it aims to have a very rich feature set that 'just works', including but not limited to:
* Native USB connection (No more messing about with serial COM ports!)
* Sensorless homing/stall detection
* Dynamic current scaling to save power
* Configurability if needed past sensible defaults.
* Designed around common hardware, allowing for easy sourcing of parts for DIY construction using only bread-board-able components.

Tested and developed around my custom Nema17 belt driven focuser for a rokinnon 135mm lens and a Moonlite stepper motor attached to a GSO RC8. 

Short demo video on rokinnon lens: https://www.youtube.com/watch?v=gBrDgyxgXSc

Homing and movement of Moonlite carrying full setup: https://www.youtube.com/watch?v=20G9TGgZjsw 

I'm also aiming to have prebuilt modules available for sale in future once I'm confident in the quality of the design. Don't want to be accepting any money before I'm 100% confident in the product.

# Getting started with the project

To actually make a SAF and get it properly setup, see the wiki for details - it list the ways you can make one and the extra HW needed for each
As of right now the only way to control it is through an ASCOM driver, however I aim to add an INDI driver in the future.

https://github.com/AlistairSymonds/SympleAstroFocus/wiki

# Premade PCB module

![saf_pcb](https://user-images.githubusercontent.com/5210224/174461619-b2c8f1a6-9a44-44a0-a194-0b4afad55e60.jpg)

All supported by JLCPCB's assembly service, schematic available here:

+ https://oshwlab.com/alistair.symonds/sympleaf_rev1_copy
+ https://oshwlab.com/alistair.symonds/sympleaf_rev1

_These two PCBs are functionally identical - just using a different package of the same microcontroller to alleviate supply chain issues. Use whichever is cheaper/in stock!_

# Licensing
This project is licensed using the MIT license, as a result you're free to take it and modify however you want for any use, just be sure to include attribution back to this repo.
