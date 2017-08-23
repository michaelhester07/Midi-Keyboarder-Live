#include "HID-Project.h"
void setup() {
  // put your setup code here, to run once:
  Keyboard.begin();
  Serial.begin(115200);
}
int inputDelayCount = 0;
void addInputDelay()
{
  if(inputDelayCount == 0)
    inputDelayCount = 2;
    else
    inputDelayCount++;
}
void applyInputDelay()
{
  if(inputDelayCount > 0)
  {
   // delay(80);
    inputDelayCount--;
  }
  
}
int odelay = 500;
void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available())
  {
    //delay(1000);
    while(Serial.available())
    {
      char c = Serial.read();
      if(c == '\n' || c == '\r') continue;
      if(c == '-')
      {
        odelay = Serial.parseInt();
        delay(2000);
        for(int z = 0; z < 10; z++){
      //   Keyboard.press('8');
      //   Keyboard.release('8');
        Keyboard.press('9');
         Keyboard.release('9');
        // delay(odelay);
         
             Keyboard.press('8');
         Keyboard.release('8');
          delay(odelay);
          Keyboard.press('0');
         Keyboard.release('0');
            delay(odelay);
         //    Keyboard.press('8');
        // Keyboard.release('8');
         Keyboard.press('9');
         Keyboard.release('9');
         //   delay(odelay);
             Keyboard.press('8');
         Keyboard.release('8');
          delay(odelay);
          Keyboard.press('0');
         Keyboard.release('0');
         delay(odelay);
        }
         continue;
      }
     // if(c == '9' || c == '0' )
     //   addInputDelay();
      // delay(80);
     // Serial.write(c);
    // applyInputDelay();
        Keyboard.press(c);
        ///delay(10);
        
      Keyboard.release(c);
      
    }

    
  }
}
