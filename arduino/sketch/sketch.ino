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
byte type = 0;
byte vibrate = 0;
const int tick = 2000;
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
  stats[0] = getStickAxis(PSS_LX);
  stats[1] = getStickAxis(PSS_LY);
  for( int i = 0; i < 16; ++i){
    stats[2+i/8] <<= 1;
    if(ps2x.Button(buttons[i]))
      stats[2+i/8] += 1;
  }
  //stats[2] = ps2x.Analog(PSS_LX);
  //stats[3] = ps2x.Analog(PSS_LY);
  if(stats[0] != 0 || stats[1] != 0)
    digitalWrite(13, HIGH);
  else 
    digitalWrite(13, LOW);
  Serial.write(stats, 4);
  delayMicroseconds(tick);
}

int range = 20;               // output range of X or Y movement
int threshold = range / 4;
int center = range / 2; // resting threshold
int distance;

int getStickAxis(int axis)
{
  int coordinate = ps2x.Analog(axis);
  byte reading = (coordinate * range)/255;
  distance = reading - center;
  // if(abs(distance) < threshold) distance = 0;
  return reading;
}