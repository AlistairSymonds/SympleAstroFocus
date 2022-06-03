/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file           : main.c
  * @brief          : Main program body
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; Copyright (c) 2021 STMicroelectronics.
  * All rights reserved.</center></h2>
  *
  * This software component is licensed by ST under BSD 3-Clause license,
  * the "License"; You may not use this file except in compliance with the
  * License. You may obtain a copy of the License at:
  *                        opensource.org/licenses/BSD-3-Clause
  *
  ******************************************************************************
  */
/* USER CODE END Header */
/* Includes ------------------------------------------------------------------*/
#include "main.h"
#include "usb_device.h"


/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */
#include "tmc/ic/TMC2209/TMC2209.h"

#include "sym_defs.h"
#include "hw_defs.h"
#include "hw_defs_manager.h"
/* USER CODE END Includes */

/* Private typedef -----------------------------------------------------------*/
/* USER CODE BEGIN PTD */

/* USER CODE END PTD */

/* Private define ------------------------------------------------------------*/
/* USER CODE BEGIN PD */
/* USER CODE END PD */

/* Private macro -------------------------------------------------------------*/
/* USER CODE BEGIN PM */

/* USER CODE END PM */

/* Private variables ---------------------------------------------------------*/
TIM_HandleTypeDef htim3;

UART_HandleTypeDef huart1;
UART_HandleTypeDef huart2;

/* USER CODE BEGIN PV */

/* USER CODE END PV */

/* Private function prototypes -----------------------------------------------*/
void SystemClock_Config(void);
static void MX_GPIO_Init(void);
static void MX_USART2_UART_Init(void);
static void MX_TIM3_Init(void);
static void MX_USART1_UART_Init(void);
/* USER CODE BEGIN PFP */
static void STEPPER_Init(void);
static void FLASH_init(void);
static void set_stepper_period_us(int us);
static void read_stepper_state(void);
static void load_state_from_flash(symple_state_t ss);
static void write_state_to_flash(symple_state_t ss);
/* USER CODE END PFP */

/* Private user code ---------------------------------------------------------*/

extern int __USER_FLASH_SECTION_START;
extern int __USER_FLASH_SECTION_END;
/*
//the state is saved across the flash in such a way to reduce the number of erase cycles needed
//when erasing the entire page is reset to 1s in every bit
//additionally the stm32f103 has some additional restrictions on writing, namely:
	+ a halfword can be written to any value ONLY when it is currently 0xFFFF
	+ a halfword can always be written to 0x0000
	+ a halfword can be set to 0xFFFF when the whole page is erased

So each half word keeps track of two writes:
 0xff (no writes) -> 0xfe -> (one write) -> 0x00 (two writes)

then the management will store in the next word
*/
int USER_FLASH_SIZE;
const int FLASH_JOURNAL_HEADER_SIZE_DWORDS = 8;
int USER_FLASH_JOURNAL_BYTES;
int NUM_STATE_WRITES_PER_USER_FLASH;


/* USER CODE BEGIN 0 */
#define CCLK_FREQ 48000000

extern USBD_HandleTypeDef hUsbDeviceFS;
TIM_HandleTypeDef htim3;

static TMC2209TypeDef TMC2209;
volatile static ConfigurationTypeDef TMC2209_config;
// Helper macro - index is always 1 here (channel 1 <-> index 0, channel 2 <-> index 1)
//#define TMC2209_CRC(data, length) tmc_CRC8(data, length, 1)
#define TMC2209_UART_BAUDRATE 100000
volatile int flash_save_needed = 0;

uint32_t last_usb_ms = 0;
uint32_t last_flash_ms = 0;
uint32_t last_motor_ms = 0;
uint32_t last_tmc_management_ms = 0;
uint32_t last_tmc_read_attempt_ms = 0;
uint32_t last_stall_handler_ms = 0;
uint32_t last_timer_set_period_us = 0;

typedef enum
{
  POSEDGE,
  NEGEDGE
} edge_dir_t;

typedef enum
{
  TOWARDS_ZERO,
  TOWARDS_MAX
} homing_dir_t;

volatile edge_dir_t next_edge_dir = POSEDGE;
homing_dir_t current_homing_dir = TOWARDS_ZERO;


void tmc2209_readWriteArray(uint8_t channel, uint8_t *data, size_t writeLength, size_t readLength)
{

	while (HAL_GetTick() - last_tmc_read_attempt_ms < 5){
		//spin wait
	}

	HAL_UART_Transmit(&huart2, data, writeLength, HAL_MAX_DELAY);

	uint8_t buffer[9];
	//given we need to delay and wait for return after writing, only do that when a read is actually required
	if (readLength) {
		//+1 for the byte that will be there from the TX - the HW does no buffering by default
		HAL_StatusTypeDef rx_status = HAL_UART_Receive(&huart2, buffer, readLength+1, 10); //capture the data we just wrote back in, clearing buffers etc


		if (rx_status == HAL_TIMEOUT || rx_status == HAL_ERROR) {
			symple_state[STATE_ID_0][STATUS_DWORD] |= STATUS_STEPPER_DRIVER_COMMS_ERROR_BIT;
		} else {
			//otherwise no error no worries
			symple_state[STATE_ID_0][STATUS_DWORD] &= ~STATUS_STEPPER_DRIVER_COMMS_ERROR_BIT;
		}
		last_tmc_read_attempt_ms = HAL_GetTick();
		memcpy(data, &(buffer[1]), readLength);

	}
}

// Return the CRC8 of [length] bytes of data stored in the [data] array.
uint8_t tmc2209_CRC8(uint8_t *data, size_t length)
{

	uint8_t crc = 0;
	for (uint8_t i = 0; i < length; i++) {
		uint8_t currentByte = data[i];
		for (uint8_t j = 0; j < 8; j++) {
			if ((crc >> 7) ^ (currentByte & 0x01)) {
				crc = (crc << 1) ^ 0x07;
			} else {
				crc = (crc << 1);
			}
			crc &= 0xff;
			currentByte = currentByte >> 1;
		}
	}

	return crc; //tmc_CRC8(data, length, 0);
}

void stall_handler() {
	if (symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_HOMING_BIT){
		symple_state[STATE_ID_0][SET_POSITION_DWORD] = symple_state[STATE_ID_0][CURRENT_POSITION_DWORD];

		if (current_homing_dir == TOWARDS_ZERO && symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_HOME_TOWARDS_ZERO_ENABLED){
			symple_state[STATE_ID_0][SET_POSITION_DWORD] = 0;
			symple_state[STATE_ID_0][CURRENT_POSITION_DWORD] = 0;
			current_homing_dir = TOWARDS_MAX;

			if (!(symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_HOME_TOWARDS_MAX_ENABLED)){
				symple_state[STATE_ID_0][STATUS_DWORD] &= ~STATUS_HOMING_BIT;
				current_homing_dir = TOWARDS_ZERO;
			}
			return;
		}


		if ((current_homing_dir == TOWARDS_MAX && (symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_HOME_TOWARDS_MAX_ENABLED)) ||
				(
				current_homing_dir == TOWARDS_ZERO && (symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_HOME_TOWARDS_MAX_ENABLED) &&
				!(symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_HOME_TOWARDS_ZERO_ENABLED)
				)
		){
			symple_state[STATE_ID_0][MAX_POSITION_DWORD] = symple_state[STATE_ID_0][CURRENT_POSITION_DWORD];

			current_homing_dir = TOWARDS_ZERO;
			symple_state[STATE_ID_0][STATUS_DWORD] &= ~STATUS_HOMING_BIT;
			return;
		}
	}

}


/* USER CODE END 0 */

/**
  * @brief  The application entry point.
  * @retval int
  */
int main(void)
{
  /* USER CODE BEGIN 1 */

  /* USER CODE END 1 */

  /* MCU Configuration--------------------------------------------------------*/

  /* Reset of all peripherals, Initializes the Flash interface and the Systick. */
  HAL_Init();

  /* USER CODE BEGIN Init */
  FLASH_init();
  //setup internal values before doing any sort of IO
  SympleState_Init();
  /* USER CODE END Init */

  /* Configure the system clock */
  SystemClock_Config();

  /* USER CODE BEGIN SysInit */

  /* USER CODE END SysInit */

  /* Initialize all configured peripherals */
  MX_GPIO_Init();
  MX_USART2_UART_Init();
  MX_USB_DEVICE_Init();
  MX_USART1_UART_Init();
  /* USER CODE BEGIN 2 */

  /* USER CODE BEGIN Init */
  STEPPER_Init();
  tmc2209_restore(&TMC2209);

  set_stepper_period_us(symple_state[STATE_ID_0][STEP_TIME_MICROSEC]);
  HAL_TIM_Base_Start_IT(&htim3);
  /* USER CODE END Init */



  /* Infinite loop */
  /* USER CODE BEGIN WHILE */
  while (1)
  {
	  if (last_timer_set_period_us != symple_state[STATE_ID_0][STEP_TIME_MICROSEC]){
		  HAL_TIM_Base_Stop_IT(&htim3);
		  set_stepper_period_us(symple_state[STATE_ID_0][STEP_TIME_MICROSEC]);
		  HAL_TIM_Base_Start_IT(&htim3);
	  }

	  if (HAL_GetTick() - last_usb_ms > 16){
		  if (is_requested_hw_defs()){
			  uint8_t defs_usb_data[SYM_EP_SIZE];
			  uint32_t hw_defs_state[4];
			  hw_defs_state[0] = hw_defs.fw_git_commit;
			  hw_defs_state[1] = hw_defs.mcu_type;
			  hw_defs_state[2] = hw_defs.stepper_driver_type;
			  hw_defs_state[3] = hw_defs.release_type;
			  memcpy(defs_usb_data, symple_state[STATE_ID_0], 40);

			  USBD_CUSTOM_HID_SendReport(&hUsbDeviceFS, (uint8_t*)defs_usb_data, SYM_EP_SIZE);

		  }

		  uint8_t usb_data[SYM_EP_SIZE];
		  memcpy(usb_data, symple_state[STATE_ID_0], 40);

		  USBD_CUSTOM_HID_SendReport(&hUsbDeviceFS, (uint8_t*)usb_data, SYM_EP_SIZE);
		  last_usb_ms = HAL_GetTick();
	  }

	  if (HAL_GetTick() - last_flash_ms > 5000){

		  if((symple_state[STATE_ID_0][COMMAND_DWORD] & COMMAND_SAVE_TO_FLASH_BIT) || flash_save_needed){
			  //write to flash
			  //clear the cmd bit before it occurs, otherwise we'll save a command that has ocurred every time
			  symple_state[STATE_ID_0][COMMAND_DWORD] &= ~COMMAND_SAVE_TO_FLASH_BIT;
			  flash_save_needed = 0;
			  write_state_to_flash(symple_state);

		  }
		  last_flash_ms = HAL_GetTick();
	  }

	  if (HAL_GetTick() - last_tmc_management_ms > 100){
		  //read the regs we care about, or set it to enabled if there aren't any errors, otherwise start the setup process
		  if (TMC2209_config.state == CONFIG_READY){


			  uint32_t ifcnt = tmc2209_readInt(&TMC2209, TMC2209_IFCNT);

			  //it takes 15 dwords to setup the tmc, so if it hasn't written 15 it can't be properly programmed
			  //this will lead to a few extra restores but that isn't the end of the world
			  if ((symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_STEPPER_DRIVER_COMMS_ERROR_BIT) == 0 && ifcnt > 15){
				  //if no comms error and its configured we should enabled it
				  symple_state[STATE_ID_0][STATUS_DWORD] |= STATUS_STEPPER_DRIVER_ENABLED_BIT;
				  read_stepper_state();

			  } else {
				  //something has gone wrong, disable and restore it

				  symple_state[STATE_ID_0][STATUS_DWORD] &= ~STATUS_STEPPER_DRIVER_ENABLED_BIT;
				  tmc2209_restore(&TMC2209);
			  }


	  	  } else {
			  //happens in this loop to handle cases where the TMC loses motor power
	  		symple_state[STATE_ID_0][STATUS_DWORD] &= ~STATUS_STEPPER_DRIVER_ENABLED_BIT;
	  		tmc2209_restore(&TMC2209);

		  }

		  last_tmc_management_ms = HAL_GetTick();
	  }


	  if (
			  ((symple_state[STATE_ID_0][STEPPER_DRIVER_STATUS] &  DRIVER_STATUS_SG_RESULT_MASK) >> DRIVER_STATUS_SG_RESULT_SHIFT) < 40 &&
			  (symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_IS_MOVING_BIT))
	  {
		  if (HAL_GetTick() - last_stall_handler_ms > 500){
			  stall_handler();
			  last_stall_handler_ms = HAL_GetTick();
		  }
	  }

	  if (!symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_HOMING_BIT){
		  current_homing_dir = TOWARDS_ZERO;
	  }

	  tmc2209_periodicJob(&TMC2209, HAL_GetTick());
	  //only enable the driver if its setup and verified that the regs have been loaded
	  //otherwise use the ENN pin to stop the driver and hopefully prevent damage

	  if(symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_STEPPER_DRIVER_ENABLED_BIT)
	  {
		  HAL_GPIO_WritePin(GPIOA, ENN_Pin, GPIO_PIN_RESET);
	  } else {
		  HAL_GPIO_WritePin(GPIOA, ENN_Pin, GPIO_PIN_SET);
	  }

  }
  /* USER CODE END WHILE */

  /* USER CODE BEGIN 3 */

  /* USER CODE END 3 */
}


void do_step(){
	if (next_edge_dir == POSEDGE)
	  {
	    HAL_GPIO_WritePin(GPIOA, STEP_Pin, GPIO_PIN_SET);
	    next_edge_dir = NEGEDGE;
	  }
	  else
	  {
		HAL_GPIO_WritePin(GPIOA, STEP_Pin, GPIO_PIN_RESET);
	    next_edge_dir = POSEDGE;
	  }
}

void update_motor_pos()
{
  if (((symple_state[STATE_ID_0][CURRENT_POSITION_DWORD] != symple_state[STATE_ID_0][SET_POSITION_DWORD]) ||
		  (symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_HOMING_BIT))
		  &&
		  symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_STEPPER_DRIVER_ENABLED_BIT) //do a step
  {

	GPIO_PinState shaft_dir =
			symple_state[STATE_ID_0][CURRENT_POSITION_DWORD] < symple_state[STATE_ID_0][SET_POSITION_DWORD] ?
					GPIO_PIN_SET : GPIO_PIN_RESET;



	//overwrite direction if we're in homing mode as set/current pos are meaningless
	if (symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_HOMING_BIT){
		if (current_homing_dir == TOWARDS_ZERO){
			shaft_dir = 0;
		} else {
			shaft_dir = 1;
		}
	}

	int counting_dir = shaft_dir;
    if (symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_REVERSE_BIT){
    	shaft_dir = !shaft_dir;
    }

    HAL_GPIO_WritePin(GPIOA, DIR_Pin, shaft_dir);
    do_step();
    if (counting_dir)
    {
    	if (symple_state[STATE_ID_0][CURRENT_POSITION_DWORD] != 0xFFFFFFFF){
        	symple_state[STATE_ID_0][CURRENT_POSITION_DWORD]++;
    	}
    }
    else
    {
    	if (symple_state[STATE_ID_0][CURRENT_POSITION_DWORD] != 0x00000000){
    		symple_state[STATE_ID_0][CURRENT_POSITION_DWORD]--;
    	}
    }
    //if we've moved set the bit, and signal a save will be needed
    symple_state[STATE_ID_0][STATUS_DWORD] |= STATUS_IS_MOVING_BIT;
    flash_save_needed = 1;
  } else {
	  //we've just finished a move - make sure its saved!
	  if (symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_IS_MOVING_BIT){
		  flash_save_needed = 1;
	  }
	  symple_state[STATE_ID_0][STATUS_DWORD] &=~ STATUS_IS_MOVING_BIT;
  }
}

void HAL_TIM_PeriodElapsedCallback(TIM_HandleTypeDef* htim)
{
	update_motor_pos();
}



/**
  * @brief System Clock Configuration
  * @retval None
  */
void SystemClock_Config(void)
{
  RCC_OscInitTypeDef RCC_OscInitStruct = {0};
  RCC_ClkInitTypeDef RCC_ClkInitStruct = {0};
  RCC_PeriphCLKInitTypeDef PeriphClkInit = {0};

  /** Initializes the RCC Oscillators according to the specified parameters
  * in the RCC_OscInitTypeDef structure.
  */
  RCC_OscInitStruct.OscillatorType = RCC_OSCILLATORTYPE_HSE;
  RCC_OscInitStruct.HSEState = RCC_HSE_ON;
  RCC_OscInitStruct.HSEPredivValue = RCC_HSE_PREDIV_DIV1;
  RCC_OscInitStruct.HSIState = RCC_HSI_ON;
  RCC_OscInitStruct.PLL.PLLState = RCC_PLL_ON;
  RCC_OscInitStruct.PLL.PLLSource = RCC_PLLSOURCE_HSE;
  RCC_OscInitStruct.PLL.PLLMUL = RCC_PLL_MUL6;
  if (HAL_RCC_OscConfig(&RCC_OscInitStruct) != HAL_OK)
  {
    Error_Handler();
  }
  /** Initializes the CPU, AHB and APB buses clocks
  */
  RCC_ClkInitStruct.ClockType = RCC_CLOCKTYPE_HCLK|RCC_CLOCKTYPE_SYSCLK
                              |RCC_CLOCKTYPE_PCLK1|RCC_CLOCKTYPE_PCLK2;
  RCC_ClkInitStruct.SYSCLKSource = RCC_SYSCLKSOURCE_PLLCLK;
  RCC_ClkInitStruct.AHBCLKDivider = RCC_SYSCLK_DIV1;
  RCC_ClkInitStruct.APB1CLKDivider = RCC_HCLK_DIV2;
  RCC_ClkInitStruct.APB2CLKDivider = RCC_HCLK_DIV1;

  if (HAL_RCC_ClockConfig(&RCC_ClkInitStruct, FLASH_LATENCY_1) != HAL_OK)
  {
    Error_Handler();
  }
  PeriphClkInit.PeriphClockSelection = RCC_PERIPHCLK_USB;
  PeriphClkInit.UsbClockSelection = RCC_USBCLKSOURCE_PLL;
  if (HAL_RCCEx_PeriphCLKConfig(&PeriphClkInit) != HAL_OK)
  {
    Error_Handler();
  }
}

//these are basically constant, just can't inform the compiler of that since the flas sections come from link script
void FLASH_init(void){
	USER_FLASH_SIZE = 4096;//&__USER_FLASH_SECTION_END - &__USER_FLASH_SECTION_START;
	USER_FLASH_JOURNAL_BYTES = USER_FLASH_SIZE - (FLASH_JOURNAL_HEADER_SIZE_DWORDS * 4);
	NUM_STATE_WRITES_PER_USER_FLASH = (FLASH_JOURNAL_HEADER_SIZE_DWORDS * 4);
}

int get_last_saved_chunk(){
	uint16_t * chunk_headers = (uint16_t*)&__USER_FLASH_SECTION_START;
	//if we never find an empty chunk
	int cur_chunk = 0;
	for (int i = 0; i < FLASH_JOURNAL_HEADER_SIZE_DWORDS * 2; i++){
		uint16_t header = chunk_headers[i];
		//if we're looking at all ones we've already got the latest write
		if (header == 0xFFFF){
			break;
		}

		if (header){ //any bits are set (one write)
			cur_chunk = (i * 2);
		} else { //no bits are set (two writes/the upper chunk tagged by this header word)

			cur_chunk = (i * 2) + 1;
		}
	}

	return cur_chunk;
}


void load_state_from_flash(symple_state_t ss){
	uint32_t* state_loc;
	state_loc = (uint32_t*)&__USER_FLASH_SECTION_START;
	uint32_t chunk_to_read = get_last_saved_chunk();
	for (int i = 0; i < NUM_STATE_INFOS; i++){
		for(int j = 0; j < STATE_LENGTH_DWORDS; j++){
			int raddr_offset = (i*STATE_LENGTH_DWORDS) + j;
			raddr_offset = raddr_offset + (chunk_to_read * STATE_LENGTH_DWORDS * NUM_STATE_INFOS) + FLASH_JOURNAL_HEADER_SIZE_DWORDS;
			ss[i][j] = state_loc[raddr_offset];
		}
	}
	//if we think its from 0 chunk and the zero hasn't be written - either its first boot or
	// the power got removed after an erase but before a write
	if ((chunk_to_read == 0) && (*state_loc & 0x0000FFFF == 0x0000FFFF)){

		symple_state[STATE_ID_0][STATE_ID_DWORD] = STATE_ID_0;
		symple_state[STATE_ID_0][COMMAND_DWORD] = 0;
		symple_state[STATE_ID_0][STATUS_DWORD] = 0;
		symple_state[STATE_ID_0][CURRENT_POSITION_DWORD] = 0;
		symple_state[STATE_ID_0][SET_POSITION_DWORD] = 0;
		symple_state[STATE_ID_0][MAX_POSITION_DWORD] = 60000;
		symple_state[STATE_ID_0][STEP_TIME_MICROSEC] = 50;

		symple_state[STATE_ID_0][STEPPER_DRIVER_CONF] = 0;
		symple_state[STATE_ID_0][STEPPER_DRIVER_CONF] |= (24 << DRIVER_CONFIG_IRUN_SHIFT) & DRIVER_CONFIG_IRUN_MASK;
		symple_state[STATE_ID_0][STEPPER_DRIVER_CONF] |= (8 << DRIVER_CONFIG_IHOLD_SHIFT) & DRIVER_CONFIG_IHOLD_MASK;
	}
}

void mark_write_in_flash_header(int chunk){
	uint16_t newval;

	if (chunk % 2){ //the first write is even, then its odd for the upper chunk
		newval = 0x0000; //odd chunk
	} else{
		newval = 0xFFFE;
	}

	uint16_t* state_loc;
	state_loc = (uint16_t*)&__USER_FLASH_SECTION_START;
	uint32_t halfword_offset = chunk >> 1;
	uint16_t *addr_to_write = state_loc + halfword_offset;
	HAL_FLASH_Program(FLASH_TYPEPROGRAM_HALFWORD, addr_to_write, newval);
}

void write_state_to_flash(symple_state_t ss){
	HAL_FLASH_Unlock();
	uint32_t* state_loc;
	state_loc = (uint32_t*)&__USER_FLASH_SECTION_START;

	int chunk_to_write = get_last_saved_chunk();

	if (chunk_to_write == NUM_STATE_WRITES_PER_USER_FLASH-1){
		FLASH_EraseInitTypeDef erase_page_config;
		erase_page_config.TypeErase = TYPEERASE_PAGEERASE;
		erase_page_config.PageAddress = &__USER_FLASH_SECTION_START;
		erase_page_config.NbPages = 4U;
		erase_page_config.Banks = FLASH_BANK_1;
		uint32_t erase_error;
		HAL_FLASHEx_Erase(&erase_page_config, &erase_error);
		//reset the chunk now they've all been erased
		chunk_to_write = 0;
	} else {
		chunk_to_write++;
	}



	for (int i = 0; i < NUM_STATE_INFOS; i++){
		for(int j = 0; j < STATE_LENGTH_DWORDS; j++){
			uint32_t wdata = ss[i][j];
			uint32_t wchunk_offset = (STATE_LENGTH_DWORDS * NUM_STATE_INFOS * chunk_to_write) + FLASH_JOURNAL_HEADER_SIZE_DWORDS;
			uint32_t waddar_offset_dwords = (i*STATE_LENGTH_DWORDS) + j;
			HAL_FLASH_Program(FLASH_TYPEPROGRAM_WORD, state_loc + wchunk_offset + waddar_offset_dwords, wdata);
		}
	}


	mark_write_in_flash_header(chunk_to_write);

	HAL_FLASH_Lock();
}

void SympleState_Init(){


	load_state_from_flash(symple_state);
	//sanity assignment to stop motor from being driven on power up
	symple_state[STATE_ID_0][SET_POSITION_DWORD] = symple_state[STATE_ID_0][CURRENT_POSITION_DWORD];
	//make sure we don't trick ourselves into thinking everything is configured from last time
	symple_state[STATE_ID_0][STATUS_DWORD] = symple_state[STATE_ID_0][STATUS_DWORD] & (~STATUS_STEPPER_DRIVER_ENABLED_BIT);
	//ensure header is set correctly otherwise host will never understand
	symple_state[STATE_ID_0][STATE_ID_DWORD] = STATE_ID_0;

}


static void set_stepper_period_us(int us){

	int period = us;

	TIM_ClockConfigTypeDef sClockSourceConfig = {0};
	TIM_MasterConfigTypeDef sMasterConfig = {0};

	htim3.Instance = TIM3;
	htim3.Init.Prescaler = (CCLK_FREQ/1000000); //1MHz timer clock,
	htim3.Init.CounterMode = TIM_COUNTERMODE_UP;
	htim3.Init.Period = period;
	htim3.Init.ClockDivision = TIM_CLOCKDIVISION_DIV1;
	htim3.Init.AutoReloadPreload = TIM_AUTORELOAD_PRELOAD_DISABLE;
	if (HAL_TIM_Base_Init(&htim3) != HAL_OK)
	{
		Error_Handler();
	}
	sClockSourceConfig.ClockSource = TIM_CLOCKSOURCE_INTERNAL;
	if (HAL_TIM_ConfigClockSource(&htim3, &sClockSourceConfig) != HAL_OK)
	{
		Error_Handler();
	}
	sMasterConfig.MasterOutputTrigger = TIM_TRGO_RESET;
	sMasterConfig.MasterSlaveMode = TIM_MASTERSLAVEMODE_DISABLE;
	if (HAL_TIMEx_MasterConfigSynchronization(&htim3, &sMasterConfig) != HAL_OK)
	{
		Error_Handler();
	}

	last_timer_set_period_us = us;
}


/**
  * @brief USART1 Initialization Function
  * @param None
  * @retval None
  */
static void MX_USART1_UART_Init(void)
{

  /* USER CODE BEGIN USART1_Init 0 */

  /* USER CODE END USART1_Init 0 */

  /* USER CODE BEGIN USART1_Init 1 */

  /* USER CODE END USART1_Init 1 */
  huart1.Instance = USART1;
  huart1.Init.BaudRate = 9600;
  huart1.Init.WordLength = UART_WORDLENGTH_8B;
  huart1.Init.StopBits = UART_STOPBITS_1;
  huart1.Init.Parity = UART_PARITY_NONE;
  huart1.Init.Mode = UART_MODE_TX_RX;
  huart1.Init.HwFlowCtl = UART_HWCONTROL_NONE;
  huart1.Init.OverSampling = UART_OVERSAMPLING_16;
  if (HAL_UART_Init(&huart1) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN USART1_Init 2 */

  /* USER CODE END USART1_Init 2 */

}

/**
  * @brief USART2 Initialization Function
  * @param None
  * @retval None
  */
static void MX_USART2_UART_Init(void)
{

  /* USER CODE BEGIN USART2_Init 0 */

  /* USER CODE END USART2_Init 0 */

  /* USER CODE BEGIN USART2_Init 1 */

  /* USER CODE END USART2_Init 1 */
  huart2.Instance = USART2;
  huart2.Init.BaudRate = TMC2209_UART_BAUDRATE;
  huart2.Init.WordLength = UART_WORDLENGTH_8B;
  huart2.Init.StopBits = UART_STOPBITS_1;
  huart2.Init.Parity = UART_PARITY_NONE;
  huart2.Init.Mode = UART_MODE_TX_RX;
  huart2.Init.HwFlowCtl = UART_HWCONTROL_NONE;
  huart2.Init.OverSampling = UART_OVERSAMPLING_16;
  if (HAL_UART_Init(&huart2) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN USART2_Init 2 */

  /* USER CODE END USART2_Init 2 */

}

/**
  * @brief GPIO Initialization Function
  * @param None
  * @retval None
  */
static void MX_GPIO_Init(void)
{
  GPIO_InitTypeDef GPIO_InitStruct = {0};

  /* GPIO Ports Clock Enable */
  __HAL_RCC_GPIOD_CLK_ENABLE();
  __HAL_RCC_GPIOA_CLK_ENABLE();

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(GPIOA, STEP_Pin|DIR_Pin|ENN_Pin, GPIO_PIN_RESET);

  /*Configure GPIO pins : STEP_Pin DIR_Pin ENN_Pin */
  GPIO_InitStruct.Pin = STEP_Pin|DIR_Pin|ENN_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
  HAL_GPIO_Init(GPIOA, &GPIO_InitStruct);

  /*Configure GPIO pin : DIAG_Pin */
  GPIO_InitStruct.Pin = DIAG_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_INPUT;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  HAL_GPIO_Init(DIAG_GPIO_Port, &GPIO_InitStruct);

}

/* USER CODE BEGIN 4 */

void load_symple_state_into_tmc_config(void){

	//mask then shift on the symnple state to get the value out of symple state, then shift/mask to set the
	TMC2209_config.shadowRegister[TMC2209_IHOLD_IRUN] &= (~TMC2209_IRUN_MASK);
	uint8_t irun = (symple_state[STATE_ID_0][STEPPER_DRIVER_CONF] & DRIVER_CONFIG_IRUN_MASK) >> DRIVER_CONFIG_IRUN_SHIFT;
	TMC2209_config.shadowRegister[TMC2209_IHOLD_IRUN] |= (irun << TMC2209_IRUN_SHIFT)  & TMC2209_IRUN_MASK;

	TMC2209_config.shadowRegister[TMC2209_IHOLD_IRUN] &= (~TMC2209_IHOLD_MASK);
	uint8_t ihold = (symple_state[STATE_ID_0][STEPPER_DRIVER_CONF] & DRIVER_CONFIG_IHOLD_MASK) >> DRIVER_CONFIG_IHOLD_SHIFT;
	TMC2209_config.shadowRegister[TMC2209_IHOLD_IRUN] |= (ihold << TMC2209_IHOLD_SHIFT)  & TMC2209_IHOLD_MASK;

}

void STEPPER_Init(void)
{
	tmc_fillCRC8Table(0x07, true, 0);
	//load whatever we've got from either boot into motor stat


	tmc2209_init(&TMC2209, 0, 0, &TMC2209_config, &tmc2209_defaultRegisterResetState[0]);

	TMC2209_config.shadowRegister[TMC2209_GCONF] = 0x00000040;
	TMC2209_config.shadowRegister[TMC2209_IHOLD_IRUN] = 0x00071703;
	TMC2209_config.shadowRegister[TMC2209_TPOWERDOWN] = 0x00000014;
	TMC2209_config.shadowRegister[TMC2209_CHOPCONF] = 0x10000053;
	TMC2209_config.shadowRegister[TMC2209_PWMCONF] = 0xC10D0024;

	load_symple_state_into_tmc_config();




	TMC2209_config.shadowRegister[TMC2209_GCONF] &= (~TMC2209_I_SCALE_ANALOG_MASK);
	TMC2209_config.shadowRegister[TMC2209_GCONF] |= TMC2209_PDN_DISABLE_MASK;
	TMC2209_config.shadowRegister[TMC2209_GCONF] |= TMC2209_MSTEP_REG_SELECT_MASK;
	TMC2209_config.shadowRegister[TMC2209_GCONF] |= TMC2209_SHAFT_MASK;

	TMC2209_config.shadowRegister[TMC2209_CHOPCONF] &= (~TMC2209_MRES_MASK);
	TMC2209_config.shadowRegister[TMC2209_CHOPCONF] |= (0 << TMC2209_MRES_SHIFT) &  TMC2209_MRES_MASK;
	TMC2209_config.shadowRegister[TMC2209_CHOPCONF] |= (0xF << TMC2209_TOFF_SHIFT) &  TMC2209_TOFF_MASK;

/*
	TMC2209_config.shadowRegister[TMC2209_COOLCONF] = 0;
	TMC2209_config.shadowRegister[TMC2209_COOLCONF] |= (0x4 << TMC2209_SEMIN_SHIFT) &  TMC2209_SEMIN_MASK;
*/
	TMC2209_config.shadowRegister[TMC2209_SGTHRS] = 0;



};

void read_stepper_state(void) {



	uint32_t drvstatus = tmc2209_readInt(&TMC2209, TMC2209_DRVSTATUS);
	uint32_t sg_result = tmc2209_readInt(&TMC2209, TMC2209_SG_RESULT);

	symple_state[STATE_ID_0][STEPPER_DRIVER_STATUS] &= (~DRIVER_STATUS_SG_RESULT_MASK);
	symple_state[STATE_ID_0][STEPPER_DRIVER_STATUS] |= sg_result;

	symple_state[STATE_ID_0][STEPPER_DRIVER_STATUS] &= (~DRIVER_STATUS_CS_ACTUAL_MASK);
	uint32_t cs_actual = (drvstatus & TMC2209_CS_ACTUAL_MASK) >> TMC2209_CS_ACTUAL_SHIFT;
	symple_state[STATE_ID_0][STEPPER_DRIVER_STATUS] |= (cs_actual) << DRIVER_STATUS_CS_ACTUAL_SHIFT;

}
/**
  * @brief  This function is executed in case of error occurrence.
  * @retval None
  */
void Error_Handler(void)
{
  /* USER CODE BEGIN Error_Handler_Debug */
  /* User can add his own implementation to report the HAL error return state */
  __disable_irq();
  while (1)
  {
  }
  /* USER CODE END Error_Handler_Debug */
}

#ifdef  USE_FULL_ASSERT
/**
  * @brief  Reports the name of the source file and the source line number
  *         where the assert_param error has occurred.
  * @param  file: pointer to the source file name
  * @param  line: assert_param error line source number
  * @retval None
  */
void assert_failed(uint8_t *file, uint32_t line)
{
  /* USER CODE BEGIN 6 */
  /* User can add his own implementation to report the file name and line number,
     ex: printf("Wrong parameters value: file %s on line %d\r\n", file, line) */
  /* USER CODE END 6 */
}
#endif /* USE_FULL_ASSERT */

