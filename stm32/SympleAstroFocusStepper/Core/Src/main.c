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

#include "tmc/ic/TMC2209/TMC2209.h"

#include "sym_defs.h"
/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */

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

UART_HandleTypeDef huart2;
UART_HandleTypeDef huart3;

bool flash_needs_write;
/* USER CODE BEGIN PV */

/* USER CODE END PV */

/* Private function prototypes -----------------------------------------------*/
void SystemClock_Config(void);
static void MX_GPIO_Init(void);
static void MX_USART2_UART_Init(void);
static void MX_USART3_UART_Init(void);
static void MX_TIM3_Init(void);
static void STEPPER_Init(void);
static void FLASH_init(void);
static void load_state_from_flash(symple_state_t ss);
static void write_state_to_flash(symple_state_t ss);
/* USER CODE BEGIN PFP */

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
extern USBD_HandleTypeDef hUsbDeviceFS;
TIM_HandleTypeDef htim3;

static TMC2209TypeDef TMC2209;
volatile static ConfigurationTypeDef TMC2209_config;
// Helper macro - index is always 1 here (channel 1 <-> index 0, channel 2 <-> index 1)
#define TMC2209_CRC(data, length) tmc_CRC8(data, length, 1)


uint32_t last_usb_ms = 0;
uint32_t last_flash_ms = 0;

typedef enum
{
  POSEDGE,
  NEGEDGE
} edge_dir_t;
volatile edge_dir_t next_edge_dir = POSEDGE;

/* USER CODE END 0 */

void tmc2209_readWriteArray(uint8_t channel, uint8_t *data, size_t writeLength, size_t readLength)
{

	HAL_UART_Transmit(&huart2, data, writeLength, HAL_MAX_DELAY);
}

// Return the CRC8 of [length] bytes of data stored in the [data] array.
uint8_t tmc2209_CRC8(uint8_t *data, size_t length)
{
	return TMC2209_CRC(data, length);
}
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

  /* USER CODE END Init */

  /* Configure the system clock */
  SystemClock_Config();

  FLASH_init();
  //setup internal values before doing any sort of IO
  SympleState_Init();

  /* Initialize all configured peripherals */
  MX_GPIO_Init();
  MX_USART2_UART_Init();
  MX_USART3_UART_Init();
  MX_USB_DEVICE_Init();
  STEPPER_Init();
  tmc2209_restore(&TMC2209);

  MX_TIM3_Init();
  HAL_TIM_Base_Start_IT(&htim3);


  /* Infinite loop */
  /* USER CODE BEGIN WHILE */
  while (1)
  {
    /* USER CODE END WHILE */
	  if (HAL_GetTick() - last_usb_ms > 16){
		  //make some dummy data and send repeatedly

		  uint8_t usb_data[SYM_EP_SIZE];
		  memcpy(usb_data, symple_state[STATE_ID_0], 32);

		  USBD_CUSTOM_HID_SendReport(&hUsbDeviceFS, (uint8_t*)usb_data, SYM_EP_SIZE);
		  last_usb_ms = HAL_GetTick();
	  }


	  tmc2209_periodicJob(&TMC2209, HAL_GetTick());

	  if(TMC2209_config.state == CONFIG_READY){

		  HAL_GPIO_WritePin(GPIOA, ENN_Pin, GPIO_PIN_RESET);
	  }

	  if (HAL_GetTick() - last_flash_ms > 5000){
		  flash_needs_write = 1; //TODO: remove this on real boards
		  if(flash_needs_write){
			  //write to flash
			  write_state_to_flash(symple_state);
			  flash_needs_write = 0;
		  }
		  last_flash_ms = HAL_GetTick();
	  }
  }
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
  if (symple_state[STATE_ID_0][CURRENT_POSITION_DWORD] != symple_state[STATE_ID_0][SET_POSITION_DWORD]
		   && (TMC2209_config.state == CONFIG_READY)) //do a step
  {

	GPIO_PinState shaft_dir =
			symple_state[STATE_ID_0][CURRENT_POSITION_DWORD] < symple_state[STATE_ID_0][SET_POSITION_DWORD] ?
					GPIO_PIN_SET : GPIO_PIN_RESET;

	int counting_dir = shaft_dir;
    if (symple_state[STATE_ID_0][STATUS_DWORD] & STATUS_REVERSE_BIT){
    	shaft_dir = !shaft_dir;
    }

    HAL_GPIO_WritePin(GPIOA, DIR_Pin, shaft_dir);
    do_step();
    if (counting_dir)
    {
    	symple_state[STATE_ID_0][CURRENT_POSITION_DWORD]++;
    }
    else
    {
    	symple_state[STATE_ID_0][CURRENT_POSITION_DWORD]--;
    }
    symple_state[STATE_ID_0][STATUS_DWORD] |= STATUS_IS_MOVING_BIT; //if we've moved set the bit
  } else {
	  symple_state[STATE_ID_0][STATUS_DWORD] &=~ STATUS_IS_MOVING_BIT;
  }
}

void HAL_TIM_PeriodElapsedCallback(TIM_HandleTypeDef* htim)
{
	HAL_GPIO_TogglePin(GPIOC, LED_Pin);
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
	// the power got removed after an erase but before
	if ((chunk_to_read == 0) && (*state_loc & 0x0000FFFF == 0x0000FFFF)){

		symple_state[STATE_ID_0][STATE_ID_DWORD] = STATE_ID_0;
		symple_state[STATE_ID_0][COMMAND_DWORD] = 0;
		symple_state[STATE_ID_0][STATUS_DWORD] = 0;
		symple_state[STATE_ID_0][CURRENT_POSITION_DWORD] = 0;
		symple_state[STATE_ID_0][SET_POSITION_DWORD] = 0;
		symple_state[STATE_ID_0][MAX_POSITION_DWORD] = 60000;
		symple_state[STATE_ID_0][STEP_TIME] = 50;
		symple_state[STATE_ID_0][STEP_MODE] = 128;
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

void SympleState_Init(void){


	load_state_from_flash(symple_state);
	//sanity assignment to stop motor from being driven on power up
	symple_state[STATE_ID_0][SET_POSITION_DWORD] = symple_state[STATE_ID_0][CURRENT_POSITION_DWORD];
	//ensure header is set correctly otherwise host will never understand
	symple_state[STATE_ID_0][STATE_ID_DWORD] = STATE_ID_0;

}

/**
  * @brief TIM3 Initialization Function
  * @param None
  * @retval None
  */
static void MX_TIM3_Init(void)
{

  /* USER CODE BEGIN TIM3_Init 0 */

  /* USER CODE END TIM3_Init 0 */

  TIM_ClockConfigTypeDef sClockSourceConfig = {0};
  TIM_MasterConfigTypeDef sMasterConfig = {0};

  /* USER CODE BEGIN TIM3_Init 1 */

  /* USER CODE END TIM3_Init 1 */
  htim3.Instance = TIM3;
  htim3.Init.Prescaler = 0;
  htim3.Init.CounterMode = TIM_COUNTERMODE_UP;
  htim3.Init.Period = 65535;
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
  /* USER CODE BEGIN TIM3_Init 2 */

  /* USER CODE END TIM3_Init 2 */

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
  huart2.Init.BaudRate = 115200;
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
  * @brief USART3 Initialization Function
  * @param None
  * @retval None
  */
static void MX_USART3_UART_Init(void)
{

  /* USER CODE BEGIN USART3_Init 0 */

  /* USER CODE END USART3_Init 0 */

  /* USER CODE BEGIN USART3_Init 1 */

  /* USER CODE END USART3_Init 1 */
  huart3.Instance = USART3;
  huart3.Init.BaudRate = 9600;
  huart3.Init.WordLength = UART_WORDLENGTH_8B;
  huart3.Init.StopBits = UART_STOPBITS_1;
  huart3.Init.Parity = UART_PARITY_NONE;
  huart3.Init.Mode = UART_MODE_TX_RX;
  huart3.Init.HwFlowCtl = UART_HWCONTROL_NONE;
  huart3.Init.OverSampling = UART_OVERSAMPLING_16;
  if (HAL_UART_Init(&huart3) != HAL_OK)
  {
    Error_Handler();
  }
  /* USER CODE BEGIN USART3_Init 2 */

  /* USER CODE END USART3_Init 2 */

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
  __HAL_RCC_GPIOC_CLK_ENABLE();
  __HAL_RCC_GPIOD_CLK_ENABLE();
  __HAL_RCC_GPIOA_CLK_ENABLE();
  __HAL_RCC_GPIOB_CLK_ENABLE();

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(LED_GPIO_Port, LED_Pin, GPIO_PIN_RESET);

  /*Configure GPIO pin Output Level */
  HAL_GPIO_WritePin(GPIOA, STEP_Pin|DIR_Pin, GPIO_PIN_RESET);


  HAL_GPIO_WritePin(GPIOA, ENN_Pin, GPIO_PIN_RESET);

  /*Configure GPIO pin : LED_Pin */
  GPIO_InitStruct.Pin = LED_Pin;
  GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
  GPIO_InitStruct.Pull = GPIO_NOPULL;
  GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
  HAL_GPIO_Init(LED_GPIO_Port, &GPIO_InitStruct);

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

void STEPPER_Init(void)
{
	tmc_fillCRC8Table(0x07, true, 1);


	TMC2209_config.channel = 0;

	TMC2209_config.shadowRegister[TMC2209_GCONF] |= TMC2209_INTERNAL_RSENSE_MASK;
	TMC2209_config.shadowRegister[TMC2209_GCONF] |= TMC2209_SHAFT_MASK;
	TMC2209_config.shadowRegister[TMC2209_GCONF] |= TMC2209_PDN_DISABLE_MASK;
	TMC2209_config.shadowRegister[TMC2209_GCONF] |= TMC2209_MSTEP_REG_SELECT_MASK;

	TMC2209_config.shadowRegister[TMC2209_IHOLD_IRUN] = 0;
	TMC2209_config.shadowRegister[TMC2209_IHOLD_IRUN] |= (8 << TMC2209_IRUN_SHIFT)  &  TMC2209_IRUN_MASK;
	TMC2209_config.shadowRegister[TMC2209_IHOLD_IRUN] |= (1 << TMC2209_IHOLD_SHIFT) &  TMC2209_IHOLD_MASK;
	TMC2209_config.shadowRegister[TMC2209_CHOPCONF] |= (8 << TMC2209_MRES_SHIFT) &  TMC2209_MRES_MASK;
	tmc2209_init(&TMC2209, 0, 0, &TMC2209_config, &tmc2209_defaultRegisterResetState[0]);




};
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

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
