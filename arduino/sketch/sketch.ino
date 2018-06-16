#include <PS2X_lib.h>  //for v1.6

#define PS2_DAT        10  //14    
#define PS2_CMD        11  //15
#define PS2_SEL        9   //16
#define PS2_CLK        12  //17

/******************************************************************
 * select modes of PS2 controller:
 *   - pressures = analog reading of push-butttons 
 *   - rumble    = motor rumbling
 * uncomment 1 of the lines for each mode selection
 ******************************************************************/
//#define pressures   true
#define pressures   false
//#define rumble      true
#define rumble      false

const uint16_t buttons[] = {
  PSB_PAD_UP   ,  
  PSB_PAD_RIGHT,  
  PSB_PAD_LEFT , 
  PSB_PAD_DOWN ,  
  PSB_CROSS    ,  
  PSB_SQUARE   ,  
  PSB_START    ,  
  PSB_SELECT   , 
 // PSB_TRIANGLE ,  
 // PSB_CIRCLE   ,  
 // PSB_L1       ,  
 // PSB_L2       ,  
 // PSB_R1       ,  
 // PSB_R2       ,  
} ;
PS2X ps2x; 
int error = 0;
byte type = 0;
byte vibrate = 0;
const int tick = 5000;
int keys_state = 0;
void setup(){
 
  Serial.begin(57600);
   
  //CHANGES for v1.6 HERE!!! **************PAY ATTENTION*************
  //setup pins and settings: GamePad(clock, command, attention, data, Pressures?, Rumble?) check for error
  error = ps2x.config_gamepad(PS2_CLK, PS2_CMD, PS2_SEL, PS2_DAT, pressures, rumble); 
}

void loop() {
  /* You must Read Gamepad to get new values and set vibration values
     ps2x.read_gamepad(small motor on/off, larger motor strenght from 0-255)
     if you don't enable the rumble, use ps2x.read_gamepad(); with no values
     You should call this at least once a second
   */  
  uint8_t stats[] = {0, 0};
  if(error == 1)
    return; 
  ps2x.read_gamepad(false, vibrate); //read controller and set large motor to spin at 'vibrate' speed
  
  for( int i = 0; i < 14; ++i){
    stats[i/8] <<= 1;
    if(ps2x.Button(buttons[i]))
      stats[i/8] += 1;
    delayMicroseconds(tick);
  }
  Serial.write(stats, 2);
}
