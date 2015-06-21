#include <SoftwareSerial.h>

#define PIN_DOOR_MAGNETS 4
#define PIN_REED_SWITCHES 8
#define PIN_BLACKLIGHT 9
#define PIN_RFID_1 5
#define PIN_RFID_2 6
#define PIN_RFID_3 7

void setup() 
{
  pinMode(PIN_BLACKLIGHT, OUTPUT );
  pinMode(PIN_REED_SWITCHES, INPUT_PULLUP );
  pinMode(PIN_DOOR_MAGNETS, OUTPUT );
  pinMode(PIN_RFID_1, INPUT);
  pinMode(PIN_RFID_2, INPUT);
  pinMode(PIN_RFID_3, INPUT);
  
  Serial.begin(9600);
}

void loop() 
{
  bool oldState = false;
  while(true)
  {
        bool isDoorOpen = digitalRead(PIN_REED_SWITCHES);
        if (isDoorOpen != oldState)
        {
          oldState = isDoorOpen;
          digitalWrite(PIN_BLACKLIGHT, isDoorOpen ? LOW : HIGH);
        }
  }
    
  /*delay(5000);
  Serial.print("Turning on magnets");
  digitalWrite(PIN_DOOR_MAGNETS, HIGH);
  
  delay(5000);
  Serial.print("Turning off magnets");
  digitalWrite(PIN_DOOR_MAGNETS, LOW);
  */

}

