#include <PS2X_lib.h>  //for v1.6

#define PS2_SEL        9   //yellow
#define PS2_DAT        10  //brown 
#define PS2_CMD        11  //orange
#define PS2_CLK        12  //blue

/******************************************************************
 * select modes of PS2 controller:
 *   - pressures = analog reading of push-butttons 
 *   - rumble    = motor rumbling
 ******************************************************************/
#define pressures   false
#define rumble      false
PS2X ps2x; 


const uint16_t buttons[] = {
  PSB_PAD_UP   ,  
  PSB_PAD_RIGHT,  
  PSB_PAD_DOWN ,  
  PSB_PAD_LEFT , 
  PSB_CROSS    ,  
  PSB_SQUARE   ,  
  PSB_START    ,  
  PSB_SELECT   , 
  PSB_TRIANGLE ,  
  PSB_CIRCLE   ,  
  PSB_L1       ,  
  PSB_L2       ,  
  PSB_L3       ,
  PSB_R1       ,  
  PSB_R2       ,  
  PSB_R3       ,
} ;
int error = 0;
byte vibrate = 0;
const int tick = 1000;

void setup(){
  digitalWrite(13, LOW);
  Serial.begin(57600);   
  error = ps2x.config_gamepad(PS2_CLK, PS2_CMD, PS2_SEL, PS2_DAT, pressures, rumble); 
  delay(5000);
}

void loop() {
  if(error == 1)
    return; 
  ps2x.read_gamepad(false, vibrate); 
  uint8_t stats[] = {0, 0, 0, 0};
  stats[0] = getStickAxis(PSS_LX, PSS_LY);
  stats[1] = getStickAxis(PSS_RX, PSS_RY);
  for( int i = 0; i < 16; ++i){
    stats[2+i/8] <<= 1;
    if(ps2x.Button(buttons[i]))
      stats[2+i/8] += 1;
  }
  if(stats[2] != 0 || stats[3] != 0)
    digitalWrite(13, HIGH);
  else 
    digitalWrite(13, LOW);
  Serial.write(stats, 4);
  delayMicroseconds(tick);
}

int range = 12;               
int getStickAxis(byte X, byte Y)
{
  int coordinateX = ps2x.Analog(X);
  int coordinateY = ps2x.Analog(Y);
  byte x = (coordinateX * range)/255;
  byte y = (coordinateY * range)/255;
  x = x << 4;
  int c = x + y;
  return c;
}